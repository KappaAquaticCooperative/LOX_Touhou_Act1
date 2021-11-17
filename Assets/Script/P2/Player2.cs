using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    //是否移动
    public bool isMoving=false;
    //复活点
    public Transform reSetPoint;
    //超级时间
    public float supperTime = 5;
    private float supperTimer = 0;
    public bool isSupper = false;
    //受伤时间
    public float hurtTime;
    private float hurtTimer;
    public bool isHurt = false;
    //动画控制
    public Animator m_animator;
    //生命
    public float HP = 3;
    public float maxHP = 3;
    //子弹
    public GameObject Bullet;
    public float bulletSpeed;
    public float shootLimit;
    public float shootDecrease;
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
    //
    //私有变量
    private float dashTimer = 0;
    
    private Transform transform;
    private Rigidbody2D rigidbody2D;

    private void Awake()
    {
        
    }
    void Start()
    {
        transform = this.gameObject.GetComponent<Transform>();
        rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
        shootTimer = shootTime;
        m_animator = this.gameObject.GetComponent<Animator>();
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
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        if (MoveDir != Vector2.zero)
        {
            Move(MoveDir);
        }
        else
        {
            Idle();
        }

        AnimatorController();


        ////控制方向（用于人物的转向）
        //if (Input.GetKeyDown(KeyCode.RightArrow) && right == false)
        //{
        //    transform.rotation = Quaternion.Euler(180, 180, 180);
        //}
        //if (Input.GetKeyDown(KeyCode.LeftArrow) && right == true)
        //{
        //    transform.rotation = Quaternion.Euler(0, 180, 0);
        //}


        ////左右移动   思路：设置速度值
        ////减少漂移的方法:设置材质，增大摩擦力
        //if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    right = false;
        //    rigidbody2D.velocity = new Vector2(-speed, rigidbody2D.velocity.y);
        //}
        //else if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    right = true;
        //    rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
        //}
        ////跳跃   思路：给予向上的速度
        //if (Input.GetKeyDown(KeyCode.UpArrow) && jumpFlag <= 2)
        //{
        //    if (jumpFlag == 1)
        //    {
        //        rigidbody2D.velocity = Vector2.zero;
        //        rigidbody2D.velocity += Vector2.up * 7;
        //    }
        //    //空中二段跳，跳跃力小于第一次
        //    if (jumpFlag == 2)
        //    {
        //        rigidbody2D.velocity = Vector2.zero;
        //        rigidbody2D.velocity += Vector2.up * 4;
        //    }
        //    jumpFlag++;
        //}
        ////左右冲刺   思路：设置速度值     *参考则，空中冲刺时无视重力    //优化:在空中冲刺时限制冲刺时间
        //if (Input.GetKey(KeyCode.Keypad2) && Input.GetKey(KeyCode.RightArrow) && dashTimer < dashTime)
        //{
        //    dashTimer += Time.deltaTime;
        //    rigidbody2D.velocity = new Vector2(2 * speed, 0);
        //}
        //if (Input.GetKey(KeyCode.Keypad2) && Input.GetKey(KeyCode.LeftArrow) && dashTimer < dashTime)
        //{
        //    dashTimer += Time.deltaTime;
        //    rigidbody2D.velocity = new Vector2(2 * -speed, 0);
        //}

        //射击
        if (Input.GetKey(KeyCode.Keypad1))
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
        TakeHurt();
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
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y + speed);
            jumpFlag = JumpState.Jump;
        }
        else if (jumpFlag == JumpState.Jump)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y + speed);
            jumpFlag = JumpState.AirJump;
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
        if (isMoving == false && Mathf.Abs(rigidbody2D.velocity.y)<0.1f)
            m_animator.Play("Idle");
        if (isMoving == true && Mathf.Abs(rigidbody2D.velocity.y)<0.1f)
            m_animator.Play("Run");
        if (Mathf.Abs(rigidbody2D.velocity.y) >= 0.1f && rigidbody2D.velocity.y > 0)
            m_animator.Play("Jump");
        else if (Mathf.Abs(rigidbody2D.velocity.y) >= 0.1f && rigidbody2D.velocity.y < 0)  
            m_animator.Play("Fall");
        if (isHurt == true)
            m_animator.Play("Hurt");
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
            this.TakeDamage(1);
        }
    }
    //被伤害（控制数值）
    public void TakeDamage(float damage)
    {
        this.HP -= damage;
        this.transform.position = reSetPoint.transform.position;
        this.rigidbody2D.velocity = new Vector2(0, 0);
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }

    }



}
