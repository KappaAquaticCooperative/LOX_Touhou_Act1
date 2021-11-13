using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    //生命
    public float HP = 3;

    //子弹
    public GameObject Bullet;
    public float bulletSpeed;

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
    public int jumpFlag = 1;

    //私有变量
    private float dashTimer = 0;
    
    private Transform transform;
    private Rigidbody2D rigidbody2D;

    void Start()
    {
        transform = this.gameObject.GetComponent<Transform>();
        rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
        shootTimer = shootTime;
    }


    void Update()
    {
        //控制方向（用于人物的转向）
        if (Input.GetKeyDown(KeyCode.RightArrow) && right == false)
        {
            transform.rotation = Quaternion.Euler(180, 180, 180);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && right == true)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }


        //左右移动   思路：设置速度值
        //减少漂移的方法:设置材质，增大摩擦力
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            right = false;
            rigidbody2D.velocity = new Vector2(-speed, rigidbody2D.velocity.y);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            right = true;
            rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
        }
        //跳跃   思路：给予向上的速度
        if (Input.GetKeyDown(KeyCode.UpArrow) && jumpFlag <= 2)
        {
            if (jumpFlag == 1)
            {
                rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.velocity += Vector2.up * 7;
            }
            //空中二段跳，跳跃力小于第一次
            if (jumpFlag == 2)
            {
                rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.velocity += Vector2.up * 4;
            }
            jumpFlag++;
        }
        //左右冲刺   思路：设置速度值     *参考则，空中冲刺时无视重力    //优化:在空中冲刺时限制冲刺时间
        if (Input.GetKey(KeyCode.RightShift) && Input.GetKey(KeyCode.RightArrow) && dashTimer < dashTime)
        {
            dashTimer += Time.deltaTime;
            rigidbody2D.velocity = new Vector2(2 * speed, 0);
        }
        if (Input.GetKey(KeyCode.RightShift) && Input.GetKey(KeyCode.LeftArrow) && dashTimer < dashTime)
        {
            dashTimer += Time.deltaTime;
            rigidbody2D.velocity = new Vector2(2 * -speed, 0);
        }

        //射击
        if (Input.GetKey(KeyCode.M))
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootTime)
            {
                Shoot();
                shootTimer = 0;//发射完之后重新为shootTime赋值
            }
        }

    }

   

    //碰撞检测
    public void OnCollisionEnter2D(Collision2D collision)
    {
        dashTimer = 0;//和其他物体碰撞后冲刺时间重置
        if (collision.gameObject.name != "Wall")
            jumpFlag = 1;
        if (collision.gameObject.name == "JumpItem")
        {
            shootTime -= 0.1f;
            Destroy(collision.gameObject);
            CerateObject.exist -= 1;

        }
        if (collision.gameObject.name == "Door")
        {
            Destroy(collision.gameObject);

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

    //生命减少的方法
    public void TakeDamage(float damage)
    {
        this.HP -= damage;
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }



}
