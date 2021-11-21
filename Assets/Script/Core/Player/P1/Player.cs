using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    //�����
    public Transform reSetPoint;
    //�޵�ʱ��
    public float defenceTime = 5;
    private float defenceTimer;
    public bool isDefence = false;
    //����ʱ��
    public float supperTime=5;
    private float supperTimer = 0;
    public bool isSupper = false;
    //����,HPΪ��ʱ�ݻ�ʱ��
    public float hurtTime;
    private float hurtTimer;
    public bool isHurt = false;
    public float destroyTime = 1;
    private float destoryTimer;
    public bool isDestory = false;

    //����
    public float HP=3;
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
    public float dashTime=0.3f;  
    //�ӵ�����ʱ��
    public float shootTime = 0.5f;
    public float shootTimer = 0;
    //������Ծ  ˼·��ֵΪ1ʱ���е�һ����Ծ��2ʱ����������Ծ�����ڵ�һ�Σ���3ʱ�޷���Ծ��������ײ���ʱ�ص�1
    public JumpState jumpFlag = JumpState.Ground;
    //��ȡ���ĵ��ߵ�����
    public int itemNumber = 0;

    //˽�б���
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
        //1.�����ƶ�,�����걸
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

        //2.������Ծ
        if (Input.GetKeyDown(KeyCode.W) && isHurt == false && !isDestory)
            Jump();

        //3.���ƹ���
        if (Input.GetKey(KeyCode.J) && isHurt == false && !isDestory) 
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootTime)
            {
                Shoot();
                shootTimer = 0;//������֮������ΪshootTime��ֵ
            }
        }

        //4.���ƶ���
        AnimationController();

        //�޵�ʱ��
        Defence();

        //��ⳬ��ʱ�䣬���ڼ�ʱ������ص���ͨ״̬
        EnterSupperTime();
        //���ڲ������޵�ʱ��ʱ�������
        if (isDefence == false)
        {
            TakeHurt();
        }

        //�޵�ʱ���б�ɫ
        Defence();
        if (isDefence == true)
            StartCoroutine("ChangeColor");
        if (isDefence == false)
            StopCoroutine("ChangeColor");


    }

    //��ײ���
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
        //1.Ӧ���Ӿ�Ч��
        isMoving = false;
    }
    void Move(Vector2 MovingDir)
    {
        //1.�����ƶ�
        transform.rotation = Quaternion.Euler(0, MovingDir.x >= 0 ? 0 : 180, 0);
        rigidbody2D.velocity = new Vector2(MovingDir.x * speed, rigidbody2D.velocity.y);
        //2.���ƶ���
        isMoving = true;
    }
    void Jump()
    {
        //1.��Ծ����
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
        //�����ɫ
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
    
    //���˺���������ֵ��
    public void TakeDamage(float damage)
    {
        this.HP -= damage;
        if (HP <= 0)
        {
            Globle.P2Score += 1;
            
        }
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
    //���ƶ���
    void AnimationController()
    {
        //1.�����ƶ�
        if(!isMoving && Mathf.Abs(rigidbody2D.velocity.y) < 0.1f && !isHurt && !isDestory)
            m_animator.Play("Idle");
        else if(Mathf.Abs(rigidbody2D.velocity.y) < 0.1f && !isHurt && !isDestory)
            m_animator.Play("Run");
        //2.��Ծ����
        if (Mathf.Abs(rigidbody2D.velocity.y) >= 0.1f && rigidbody2D.velocity.y > 0 && !isHurt && !isDestory) 
            m_animator.Play("Jump");
        else if (Mathf.Abs(rigidbody2D.velocity.y) >= 0.1f && !isHurt && !isDestory) 
            m_animator.Play("Fall");
        //3.���˶���
        if (isHurt == true)
        {
            m_animator.Play("Hurt");
        }
        //4.���ݻ�
        if(isDestory==true)
        {
            
            m_animator.Play("Death");
        }
    }
    //Я��
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



}

public enum JumpState
{
    Ground,
    Jump,
    AirJump
}
