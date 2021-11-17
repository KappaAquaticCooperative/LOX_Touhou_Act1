using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    //�Ƿ��ƶ�
    public bool isMoving=false;
    //�����
    public Transform reSetPoint;
    //����ʱ��
    public float supperTime = 5;
    private float supperTimer = 0;
    public bool isSupper = false;
    //����ʱ��
    public float hurtTime;
    private float hurtTimer;
    public bool isHurt = false;
    //��������
    public Animator m_animator;
    //����
    public float HP = 3;
    public float maxHP = 3;
    //�ӵ�
    public GameObject Bullet;
    public float bulletSpeed;
    public float shootLimit;
    public float shootDecrease;
    //����
    static public bool right = true;

    //�ٶ�
    public int speed;

    //������г�̵�ʱ��
    public float dashTime = 0.5f;

    //�ӵ�����ʱ��
    public float shootTime = 0.5f;
    public float shootTimer = 0;
    //������Ծ  ˼·��ֵΪ1ʱ���е�һ����Ծ��2ʱ����������Ծ�����ڵ�һ�Σ���3ʱ�޷���Ծ��������ײ���ʱ�ص�1
    public JumpState jumpFlag = JumpState.Ground;
    //
    //˽�б���
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


        ////���Ʒ������������ת��
        //if (Input.GetKeyDown(KeyCode.RightArrow) && right == false)
        //{
        //    transform.rotation = Quaternion.Euler(180, 180, 180);
        //}
        //if (Input.GetKeyDown(KeyCode.LeftArrow) && right == true)
        //{
        //    transform.rotation = Quaternion.Euler(0, 180, 0);
        //}


        ////�����ƶ�   ˼·�������ٶ�ֵ
        ////����Ư�Ƶķ���:���ò��ʣ�����Ħ����
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
        ////��Ծ   ˼·���������ϵ��ٶ�
        //if (Input.GetKeyDown(KeyCode.UpArrow) && jumpFlag <= 2)
        //{
        //    if (jumpFlag == 1)
        //    {
        //        rigidbody2D.velocity = Vector2.zero;
        //        rigidbody2D.velocity += Vector2.up * 7;
        //    }
        //    //���ж���������Ծ��С�ڵ�һ��
        //    if (jumpFlag == 2)
        //    {
        //        rigidbody2D.velocity = Vector2.zero;
        //        rigidbody2D.velocity += Vector2.up * 4;
        //    }
        //    jumpFlag++;
        //}
        ////���ҳ��   ˼·�������ٶ�ֵ     *�ο��򣬿��г��ʱ��������    //�Ż�:�ڿ��г��ʱ���Ƴ��ʱ��
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

        //���
        if (Input.GetKey(KeyCode.Keypad1))
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootTime)
            {
                Shoot();
                shootTimer = 0;//������֮������ΪshootTime��ֵ
            }
        }

        //����ʱ����
        EnterSupperTime();
        TakeHurt();
    }

   



    //��ײ���
    public void OnCollisionEnter2D(Collision2D collision)
    {
        dashTimer = 0;//������������ײ����ʱ������
        if (collision.gameObject.tag == "IsGround")
            jumpFlag = JumpState.Ground;
        if (collision.gameObject.name == "Door")      
            Destroy(collision.gameObject);       
    }

    //��ֹ
    void Idle()
    {
        isMoving = false;
    }

    //�����ƶ��ķ���
    void Move(Vector2 MoveDir)
    {
        isMoving = true;
        transform.rotation = Quaternion.Euler(0, MoveDir.x > 0 ? 0 : 180, 0);
        rigidbody2D.velocity = new Vector2(MoveDir.x * speed, this.rigidbody2D.velocity.y);
    }

    //��Ծ�ķ���
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

    //����ķ���
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
    //����ʱ��
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



    //���ƶ����ķ���
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

    //���˺������ƶ�����
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
    //���˺���������ֵ��
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
