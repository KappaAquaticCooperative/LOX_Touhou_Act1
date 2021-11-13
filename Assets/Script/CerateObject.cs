using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerateObject : MonoBehaviour
{
    public GameObject jumpObject;
    public float creatTime=2;
    public float RX;
    public float HP;
    public static int exist=0;
    //к╫сп╠Да©
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
            GameObject ju = GameObject.Instantiate(jumpObject);
            ju.transform.position = new Vector3(RX, 5, -4.5f);
            ju.name = "JumpItem";
            ju.tag = "Pick";
            
            RX = Random.Range(-9, 11);
            

            creatTimer = 0;
            exist += 1;
            Debug.Log(exist);
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
