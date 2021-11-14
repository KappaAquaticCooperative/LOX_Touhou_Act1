using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerateObject : MonoBehaviour
{
    //�����ɵ�������������ɵ�ʱ��
    public GameObject jumpObject;
    public GameObject heartObject;
    public float creatTime;
    //����������ֵ�x��
    public float RX;
    //˼·������������ʱ����ʱ���ٻ����ߣ�����ͨ�������ݻٸ�����
    public float HP;
    //������ڵ�����
    //����ʹ���˾�̬����������Ϊ���ڵ��ԣ���������Ϊ��������ʵ���Ϻ������������ĸ���Ҳͦ����ģ�
    public static int exist;
    //˽�б���
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
