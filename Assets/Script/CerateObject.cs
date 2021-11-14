using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerateObject : MonoBehaviour
{
    //被生成的两个物体和生成的时间
    public GameObject jumpObject;
    public GameObject heartObject;
    public float creatTime;
    //道具随机出现的x轴
    public float RX;
    //思路：这个物体存在时会随时间召唤道具，可以通过攻击摧毁该物体
    public float HP;
    //物体存在的上限
    //这里使用了静态变量，我认为便于调试（起码我认为是这样？实际上后面对这个变量的更改也挺方便的）
    public static int exist;
    //私有变量
    private float creatTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        RX = Random.Range(-9, 11);
    }

    // Update is called once per frame
    void Update()
    {
        creatTimer += Time.deltaTime;
        if (creatTimer >= creatTime && exist < 3)
        {
            int random = Random.Range(1, 4);
            

            if (random == 1 || random == 2)
            {
                GameObject ju = GameObject.Instantiate(jumpObject);
                ju.transform.position = new Vector3(RX, 5, -4.5f);
                ju.name = "JumpItem";
                ju.tag = "Pick";
            }
            if (random == 3)
            {
                GameObject hp = GameObject.Instantiate(heartObject);
                hp.transform.position = new Vector3(RX, 5, -4.5f);
                hp.name = "HeartItem";
                hp.tag = "Pick";
            }
            RX = Random.Range(-9, 11);
            

            creatTimer = 0;
            exist += 1;
            
        }

        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }

    }

   public void TakeDamege(float damage)
    {
        this.HP -= damage;
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }


}
