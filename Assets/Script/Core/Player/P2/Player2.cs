using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    //是否移动
    public bool isMoving=false;
    //复活点
    public Transform reSetPoint;
    //无敌时间
    public float defenceTime = 5;
    private float defenceTimer;
    public bool isDefence = false;
    //超级时间
    public float supperTime = 5;
    private float supperTimer = 0;
    public bool isSupper = false;
    //受伤时间
    public float hurtTime;
    private float hurtTimer;
    public bool isHurt = false;
    public float destroyTime = 1;
    private float destoryTimer;
    public bool isDestory = false;
    
    //生命
    public float HP = 3;
    public float maxHP = 3;
    //子弹
    public GameObject Bullet;
    public float bulletSpeed;
    public float shootLimit;
    public float shootDecrease;
    //音效
    public AudioClip FootStepSound;
    public AudioClip JumpSound;
    public AudioClip DeathSound;
    //方向
    static public bool right = true;

    //速度
    public int speed;

    //允许空中冲刺的时间
    public float dashTime = 0.5f;

    //子弹缓冲时间
    public float shootTime = 0.5f;
    public float shootTimer = 0;
    //可以跳跃  思路：值为1时进行第一次跳跃，2时二段跳（跳跃力低于第一段），3时无法跳跃，触发碰撞检测时回到1
    public JumpState jumpFlag = JumpState.Ground;
    //获取到的道具数量
    public int itemNumber = 0;
    //私有变量
    private float dashTimer = 0;
    //动画控制
    public Animator m_animator;
    private Transform transform;
    private Rigidbody2D rigidbody2D;
    private AudioSource m_audioSource;

   
    void Start()
    {
        transform = this.gameObject.GetComponent<Transform>();
        rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
        shootTimer = shootTime;
        m_animator = this.gameObject.GetComponent<Animator>();
        m_audioSource = this.gameObject.GetComponent<AudioSource>();
        StartCoroutine(FootStepPlayer());
        
        
    }


    void Update()
    {
        Vector2 MoveDir = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            right = false;
            MoveDir.x = -1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            right = true;
            MoveDir.x = 1;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && isHurt == false && !isDestory)
        {
            Jump();
        }
        if (MoveDir != Vector2.zero && isHurt == false && !isDestory)
        {
            Move(MoveDir);
        }
        else
        {
            Idle();
        }

        //动画控制
        AnimatorController();

        //无敌时间中变色
        Defence();
        if (isDefence == true)
            StartCoroutine("ChangeColor");
        if (isDefence == false) 
            StopCoroutine("ChangeColor");

        //射击
        if (Input.GetKey(KeyCode.Keypad1) && !isDestory && !isHurt)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootTime)
            {
                Shoot();
                shootTimer = 0;//发射完之后重新为shootTime赋值
            }
        }

        //超级时间检测
        EnterSupperTime();
        //思路：仅在非无敌状态下使用TakeHurt方法
        if (!isDefence)
        {
            TakeHurt();
        }
        AudioController();
    }

   
    


    //碰撞检测
    public void OnCollisionEnter2D(Collision2D collision)
    {
        dashTimer = 0;//和其他物体碰撞后冲刺时间重置
        if (collision.gameObject.tag == "IsGround")
            jumpFlag = JumpState.Ground;
        if (collision.gameObject.name == "Door")      
            Destroy(collision.gameObject);       
    }

    //静止
    void Idle()
    {
        isMoving = false;
    }

    //左右移动的方法
    void Move(Vector2 MoveDir)
    {
        isMoving = true;
        transform.rotation = Quaternion.Euler(0, MoveDir.x > 0 ? 0 : 180, 0);
        rigidbody2D.velocity = new Vector2(MoveDir.x * speed, this.rigidbody2D.velocity.y);
    }

    //跳跃的方法
    void Jump()
    {
        if (jumpFlag == JumpState.Ground)
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y + speed);
            jumpFlag = JumpState.Jump;
            m_audioSource.clip = JumpSound;
            m_audioSource.Play();
        }
        else if (jumpFlag == JumpState.Jump)
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y + speed);
            jumpFlag = JumpState.AirJump;
            m_audioSource.clip = JumpSound;
            m_audioSource.Play();
        }
        
    }

    //射击的方法
    public GameObject Shoot()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        GameObject go = GameObject.Instantiate(Bullet, this.transform.position, this.transform.rotation);
        go.GetComponent<SpriteRenderer>().color = new Color(r, g, b);
        
        if (right == true)
        {
            go.transform.position = this.transform.position + transform.right;
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed, 0);
        }
        if (right == false)
        {
            go.transform.position = this.transform.position + transform.right;
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(-bulletSpeed, 0);
        }
        return go;
    }
    //超级时间
    public void EnterSupperTime()
    {

        if (isSupper)
        {
            supperTimer += Time.deltaTime;

        }
        if (supperTimer > supperTime)
        {
            isSupper = false;
            bulletSpeed /= 3;
            shootTime *= 3;
            supperTimer = 0;
        }
    }



    //控制动画的方法
    void AnimatorController() 
    {

        if (isMoving == false && Mathf.Abs(rigidbody2D.velocity.y) < 0.1f && !isHurt && !isDestory)   
            m_animator.Play("Idle");
        if (isMoving == true && Mathf.Abs(rigidbody2D.velocity.y)< 0.1f && !isHurt && !isDestory)
            m_animator.Play("Run");
        if (Mathf.Abs(rigidbody2D.velocity.y) >= 0.1f && rigidbody2D.velocity.y > 0 && !isHurt && !isDestory)
            m_animator.Play("Jump");
        else if (Mathf.Abs(rigidbody2D.velocity.y) >= 0.1f && rigidbody2D.velocity.y < 0 && !isHurt && !isDestory)  
            m_animator.Play("Fall");
        if (isHurt == true)        
            m_animator.Play("Hurt");
        if (isDestory == true)
            m_animator.Play("Death");
    }
    //控制音效
    void AudioController()
    {
        
    }
    //被伤害（控制动画）
    public void TakeHurt()
    {
        if (isHurt == true)
        {
            hurtTimer += Time.deltaTime;
        }
        if (hurtTimer >= hurtTime)
        {
            hurtTimer = 0;
            isHurt = false;
            isDestory = true;
            this.TakeDamage(1);
            m_audioSource.PlayOneShot(DeathSound);
        }
        if (isDestory == true)
        {
            destoryTimer += Time.deltaTime;
            if (destoryTimer >= destroyTime)
            {
                isDestory = false;

                this.transform.position = reSetPoint.transform.position;
                this.rigidbody2D.velocity = new Vector2(0, 0);
                destoryTimer = 0;
                //无敌时间开始计时
                isDefence = true;
                if (HP <= 0)
                    Destroy(this.gameObject);
            }
        }
    }
    //被伤害（控制数值）
    public void TakeDamage(float damage)
    {
        this.HP -= damage;
        if (HP <= 0)
        {
            Globle.P1Score += 1;
        }

    }
    //无敌时间
    public void Defence()
    {
        if (isDefence == true)
        {
            defenceTimer += Time.deltaTime;
        }
        if (defenceTimer >= defenceTime)
        {
            isDefence = false;
            defenceTimer = 0;
        }
    }

    //携程
    //携程1.脚步
    IEnumerator FootStepPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            if (isMoving == true && Mathf.Abs(rigidbody2D.velocity.y) < 0.05f)
            {
                m_audioSource.Stop();
                m_audioSource.clip = FootStepSound;
                m_audioSource.Play();
            }

        }
    }
    //携程2.在无敌时间变色
    IEnumerator ChangeColor()
    {
        while (true)
        {
            float r, g, b;
            r = Random.Range(0, 1f);
            g = Random.Range(0, 1f);
            b = Random.Range(0, 1f);
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(r, g, b);
            yield return new WaitForSeconds(0.2f);
        }
    }

}
