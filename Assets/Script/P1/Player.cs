using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //复活点
    public Transform reSetPoint;

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
    
    void Start()
    {
        transform = this.gameObject.GetComponent<Transform>();
        rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        
        shootTimer = shootTime;
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

        ////控制方向（用于人物的转向）
        //if (Input.GetKeyDown(KeyCode.D)&&right==false)
        //{

        //    transform.rotation = Quaternion.Euler(180, 180, 180);
        //} 
        //if (Input.GetKeyDown(KeyCode.A)&&right==true)
        //{            
        //    transform.rotation = Quaternion.Euler(0, 180, 0);
        //}

        ////左右移动   思路：设置速度值
        ////减少漂移的方法:设置材质，增大摩擦力
        //if (Input.GetKey(KeyCode.A))
        //{
        //    right = false;

        //    rigidbody2D.velocity = new Vector2(-speed, rigidbody2D.velocity.y);
        //    m_animator.Play("Move");

        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    right = true;

        //    rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
        //    m_animator.Play("Move");
        //    //transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        //    //transform.position += new Vector3(speed * Time.deltaTime, 0, 0); 
        //}
        ////跳跃   思路：给予向上的速度
        //if (Input.GetKeyDown(KeyCode.W)&&jumpFlag<=2)
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
        //if (Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.D)&&dashTimer<dashTime)
        //{
        //    dashTimer += Time.deltaTime;
        //    rigidbody2D.velocity = new Vector2(2 * speed, 0);
        //}
        //if (Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.A) && dashTimer < dashTime)
        //{
        //    dashTimer += Time.deltaTime;
        //    rigidbody2D.velocity = new Vector2(2 * -speed, 0);
        //}



        //检测超级时间，用于计时结束后回到普通状态
        EnterSupperTime();
        TakeHurt();
            
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
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y + speed);
            jumpFlag = JumpState.Jump;
        }
        else if (jumpFlag == JumpState.Jump)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y + speed);
            jumpFlag = JumpState.AirJump;
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
                
                if (HP <= 0)
                    Destroy(this.gameObject);
            }
        }
    }
    
    //被伤害（控制数值）
    public void TakeDamage(float damage)
    {
        this.HP -= damage;
        
      
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

}

public enum JumpState
{
    Ground,
    Jump,
    AirJump
}
