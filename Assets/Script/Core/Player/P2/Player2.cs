using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    //�Ƿ��ƶ�
    public bool isMoving=false;
    //�����
    public Transform reSetPoint;
    //�޵�ʱ��
    public float defenceTime = 5;
    private float defenceTimer;
    public bool isDefence = false;
    //����ʱ��
    public float supperTime = 5;
    private float supperTimer = 0;
    public bool isSupper = false;
    //����ʱ��
    public float hurtTime;
    private float hurtTimer;
    public bool isHurt = false;
    public float destroyTime = 1;
    private float destoryTimer;
    public bool isDestory = false;
    
    //����
    public float HP = 3;
    public float maxHP = 3;
    //�ӵ�
    public GameObject Bullet;
    public float bulletSpeed;
    public float shootLimit;
    public float shootDecrease;
    //��Ч
    public AudioClip FootStepSound;
    public AudioClip JumpSound;
    public AudioClip DeathSound;
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
    //��ȡ���ĵ�������
    public int itemNumber = 0;
    //˽�б���
    private float dashTimer = 0;
    //��������
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

        //��������
        AnimatorController();

        //�޵�ʱ���б�ɫ
        Defence();
        if (isDefence == true)
            StartCoroutine("ChangeColor");
        if (isDefence == false) 
            StopCoroutine("ChangeColor");

        //���
        if (Input.GetKey(KeyCode.Keypad1) && !isDestory && !isHurt)
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
        //˼·�����ڷ��޵�״̬��ʹ��TakeHurt����
        if (!isDefence)
        {
            TakeHurt();
        }
        AudioController();
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
    //������Ч
    void AudioController()
    {
        
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
                //�޵�ʱ�俪ʼ��ʱ
                isDefence = true;
                if (HP <= 0)
                    Destroy(this.gameObject);
            }
        }
    }
    //���˺���������ֵ��
    public void TakeDamage(float damage)
    {
        this.HP -= damage;
        if (HP <= 0)
        {
            Globle.P1Score += 1;
        }

    }
    //�޵�ʱ��
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

    //Я��
    //Я��1.�Ų�
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
    //Я��2.���޵�ʱ���ɫ
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
