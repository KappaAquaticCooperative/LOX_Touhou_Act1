using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //����
    public float HP=3;
    //�ӵ�
    public GameObject Bullet;
    public float bulletSpeed;
    //����
    static public bool right = true;
    //�ٶ�
    public int speed;
    //������г�̵�ʱ��
    public float dashTime=0.3f;  
    //�ӵ�����ʱ��
    public float shootTime = 0.5f;
    public float shootTimer = 0;
    //������Ծ  ˼·��ֵΪ1ʱ���е�һ����Ծ��2ʱ����������Ծ�����ڵ�һ�Σ���3ʱ�޷���Ծ��������ײ���ʱ�ص�1
    public int jumpFlag=1;
    //˽�б���
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
        //���Ʒ��������ӵ��ķ���
        if (Input.GetKeyDown(KeyCode.D))
        {
            right = true;
        } 
        if (Input.GetKeyDown(KeyCode.A))
        {
            right = false;
            
        }

        //�����ƶ�   ˼·�������ٶ�ֵ
        //����Ư�Ƶķ���:���ò��ʣ�����Ħ����
        if (Input.GetKey(KeyCode.A))
        {
            rigidbody2D.velocity = new Vector2(-speed, rigidbody2D.velocity.y);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);
        }
        //��Ծ   ˼·���������ϵ���
        if (Input.GetKeyDown(KeyCode.W)&&jumpFlag<=2)
        {
            if (jumpFlag == 1)
            {
                rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.velocity = rigidbody2D.velocity + Vector2.up * 10;
            }
            //���ж���������Ծ��С�ڵ�һ��
            if (jumpFlag == 2)
            {
                rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.AddForce(new Vector2(0, 250));
            }
            jumpFlag++;
        }
        //���ҳ��   ˼·�������ٶ�ֵ     *�ο��򣬿��г��ʱ��������    //�Ż�:�ڿ��г��ʱ���Ƴ��ʱ��
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D)&&dashTimer<dashTime)
        {
            dashTimer += Time.deltaTime;
            rigidbody2D.velocity = new Vector2(2 * speed, 0);
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A) && dashTimer < dashTime)
        {
            dashTimer += Time.deltaTime;
            rigidbody2D.velocity = new Vector2(2 * -speed, 0);
        }

        //���
        if (Input.GetKey(KeyCode.J))
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootTime)
            {
                Shoot();
                shootTimer = 0;//������֮������ΪshootTime��ֵ
            }
        }

    }
    
    //����ª�ģ���ײ���
    //δ�ɹ����Ż��������ڿ��ź��һ���Զ�����
    //if (collision.gameObject.name == "Door")
    //    {
    //        collision.gameObject.SetActive(false);
    //        for (doorTimer = doorTime;doorTimer<=5;doorTimer+=Time.deltaTime)
    //        {
    //            if (doorTimer >= 4.5)
    //            {
    //                collision.gameObject.SetActive(true);
    //            }
    //        }
    //    }

    //��ײ���
    public void OnCollisionEnter2D(Collision2D collision)
    {
        dashTimer = 0;
        if (collision.gameObject.name != "Wall")
        jumpFlag = 1;
        if (collision.gameObject.name == "JumpItem")
            Destroy(collision.gameObject);
        if (collision.gameObject.name == "Door")
            Destroy(collision.gameObject);
       
    }
   //����ķ���
   public GameObject Shoot()
    {
        
        GameObject go = GameObject.Instantiate(Bullet,this.transform.position,this.transform.rotation);
        if (right == true)
        {
            go.transform.position = this.transform.position + transform.right;
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed, 0);
        }
        if (right == false)
        {
            go.transform.position = this.transform.position - transform.right;
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(-bulletSpeed, 0);
        }
        return go;
    }

    //�������ٵķ���
    public void TakeDamage(float damage)
    {
        this.HP -= damage;

        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }



}
