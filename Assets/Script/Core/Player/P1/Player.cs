using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    //复活点
    public Transform reSetPoint;
    //无敌时间
    public float defenceTime = 5;
    private float defenceTimer;
    public bool isDefence = false;
    //超级时间
    public float supperTime=5;
    private float supperTimer = 0;
    public bool isSupper = false;
    //受伤,HP为零时摧毁时间
    public float hurtTime;
    private float hurtTimer;
    public bool isHurt = false;
    public float destroyTime = 1;
    private float destoryTimer;
    public bool isDestory = false;

    //生命
    public float HP=3;
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
    public float dashTime=0.3f;  
    //子弹缓冲时间
    public float shootTime = 0.5f;
    public float shootTimer = 0;
    //可以跳跃  思路：值为1时进行第一次跳跃，2时二段跳（跳跃力低于第一段），3时无法跳跃，触发碰撞检测时回到1
    public JumpState jumpFlag = JumpState.Ground;
    //获取到的道具的数量
    public int itemNumber = 0;

    //私有变量
    private float dashTimer = 0;
    private bool isMoving = false;
    
    private Transform transform;
    private Rigidbody2D rigidbody2D;
    private Animator m_animator;
    private AudioSource m_audioSource;
    
    void Start()
    {
        transform = this.gameObject.GetComponent<Transform>();
        rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_audioSource = GetComponent<AudioSource>();

        shootTimer = shootTime;

        StartCoroutine(FootstepPlayer());
    }

    void Update()
    {
        //1.控制移动,测试完备
        Vector2 moveDir = Vector2.zero;
        if (Input.GetKey(KeyCode.D))
        {
            right = true;
            moveDir.x = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            right = false;
            moveDir.x = -1;
        }

        if (moveDir != Vector2.zero && isHurt == false && !isDestory) 
            Move(moveDir);
        else
            Idle();

        //2.控制跳跃
        if (Input.GetKeyDown(KeyCode.W) && isHurt == false && !isDestory)
            Jump();

        //3.控制攻击
        if (Input.GetKey(KeyCode.J) && isHurt == false && !isDestory) 
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootTime)
            {
                Shoot();
                shootTimer = 0;//发射完之后重新为shootTime赋值
            }
        }

        //4.控制动画
        AnimationController();

        //无敌时间
        Defence();

        //检测超级时间，用于计时结束后回到普通状态
        EnterSupperTime();
        //仅在不处于无敌时间时检测受伤
        if (isDefence == false)
        {
            TakeHurt();
        }

        //无敌时间中变色
        Defence();
        if (isDefence == true)
            StartCoroutine("ChangeColor");
        if (isDefence == false)
            StopCoroutine("ChangeColor");


    }

    //碰撞检测
    public void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "IsGround")
        {
            dashTimer = 0;
            jumpFlag = JumpState.Ground;
        }
        if (collision.gameObject.name == "Door")
            Destroy(collision.gameObject);
       
    }
    void Idle()
    {
        //1.应用视觉效果
        isMoving = false;
    }
    void Move(Vector2 MovingDir)
    {
        //1.左右移动
        transform.rotation = Quaternion.Euler(0, MovingDir.x >= 0 ? 0 : 180, 0);
        rigidbody2D.velocity = new Vector2(MovingDir.x * speed, rigidbody2D.velocity.y);
        //2.控制动画
        isMoving = true;
    }
    void Jump()
    {
        //1.跳跃处理
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
    public GameObject Shoot()
    {
        //随机颜色
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        GameObject go = GameObject.Instantiate(Bullet,this.transform.position,this.transform.rotation);
        go.GetComponent<SpriteRenderer>().color = new Color(r, g, b);
        go.transform.position = this.transform.position + transform.right;
       
            if (right == true)
                go.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed, 0);
            if (right == false)
                go.GetComponent<Rigidbody2D>().velocity = new Vector2(-bulletSpeed, 0);
        
        
        return go;
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
                destoryTimer = 0;
                isDestory = false;
                this.transform.position = reSetPoint.transform.position;
                this.rigidbody2D.velocity = new Vector2(0, 0);
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
            Globle.P2Score += 1;
            
        }
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
    //控制动画
    void AnimationController()
    {
        //1.左右移动
        if(!isMoving && Mathf.Abs(rigidbody2D.velocity.y) < 0.1f && !isHurt && !isDestory)
            m_animator.Play("Idle");
        else if(Mathf.Abs(rigidbody2D.velocity.y) < 0.1f && !isHurt && !isDestory)
            m_animator.Play("Run");
        //2.跳跃动画
        if (Mathf.Abs(rigidbody2D.velocity.y) >= 0.1f && rigidbody2D.velocity.y > 0 && !isHurt && !isDestory) 
            m_animator.Play("Jump");
        else if (Mathf.Abs(rigidbody2D.velocity.y) >= 0.1f && !isHurt && !isDestory) 
            m_animator.Play("Fall");
        //3.受伤动画
        if (isHurt == true)
        {
            m_animator.Play("Hurt");
        }
        //4.被摧毁
        if(isDestory==true)
        {
            
            m_animator.Play("Death");
        }
    }
    //携程
    IEnumerator FootstepPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            if (isMoving && Mathf.Abs(rigidbody2D.velocity.y) <= 0.05f)
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



}

public enum JumpState
{
    Ground,
    Jump,
    AirJump
}
