using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //反射冷却
    public float workTime=0.1f;
    public float workTimer=0;

    public float speed=2;
    public float damage=1;
    //判断子弹是否转向
    public bool changed = false;

    //摧毁动画相关
    public float destroyTime = 1;
    private float destoryTimer;
    public bool isDestory = false;
    //生命周期
    public float disappearTime = 10f;
    public float disappearTimer;
    //音效
    public AudioClip bulletSound;
    //私有变量
    private Transform transform;
    private Rigidbody2D rigidbody2D;
    private Animator m_animator;
    private AudioSource m_audioSource;
    void Start()
    {
        workTimer = 0;
        transform = GetComponent<Transform>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_audioSource = GetComponent<AudioSource>();
        destoryTimer = 0;
        changed = false;
    }


    void Update()
    {
        workTimer += Time.deltaTime;
        disappearTimer += Time.deltaTime;
        if (disappearTimer >= disappearTime)
        {
            isDestory = true;
            
        }
        Destroy();
        AnimatorController();

    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player && player.isDestory == false && this.isDestory == false && player.isDefence == false)  
            player.isHurt = true;

        Player2 player2 = collision.gameObject.GetComponent<Player2>();
        if (player2 && player2.isDestory == false && this.isDestory == false && player2.isDefence == false)  
            player2.isHurt = true;

        if(collision.gameObject.name== "face-block")
        {
            collision.gameObject.GetComponent<CerateObject>().TakeDamege(damage);
        }

        if (collision.gameObject.name == "JumpItem")
        {
            Destroy(collision.gameObject);
            CerateObject.exist -= 1;
        }

        if (collision.gameObject.tag != "sukima")
        {
            isDestory = true;
            AudioSource.PlayClipAtPoint(bulletSound, transform.position);   
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Mirror" && collision.transform.rotation.z > 0 && workTimer >= workTime) 
        {

            //rigidbody2D.velocity = 0.5f*Vector3.Reflect(this.transform.position,new Vector3(0,1.8f,0));
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.y, rigidbody2D.velocity.x);
            if (changed == false && workTimer >= workTime)
            {
                transform.rotation = Quaternion.Euler(0, 0, -90);
                changed = true;
                workTimer = 0;
            }
            if (changed == true && workTimer >= workTime)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180);
                changed = false;
                workTimer = 0;
            }
        }
        if (collision.tag == "Mirror" && collision.transform.rotation.z < 0 && workTimer >= workTime)
        {
            rigidbody2D.velocity = new Vector2(-rigidbody2D.velocity.y, -rigidbody2D.velocity.x);
            if (changed == false && workTimer >= workTime)
            {
                transform.rotation = Quaternion.Euler(0, 0, -90);
                changed = true;
                workTimer = 0;
            }
            if (changed == true && workTimer >= workTime)
            {
                transform.rotation = Quaternion.Euler(0, 180, -180);
                changed = false;
                workTimer = 0;
            }
        }
        //if (collision.tag == "Mirror" && changed == true)
        //{
        //    rigidbody2D.ve
        //}
    }


    void AnimatorController()
    {
        //if (isDestory == false)
        //{
        //    m_animator.Play("Bullet");
        //}
        if (isDestory == true)
        {
            m_animator.Play("Death");
            
        }
    }
    private void Destroy()
    {
        if (isDestory == true)
        {
            destoryTimer += Time.deltaTime;
            
            if (destoryTimer >= destroyTime)
            {
                isDestory = false;
                Destroy(this.gameObject);
                destoryTimer = 0;
            }
        }
    }


}
