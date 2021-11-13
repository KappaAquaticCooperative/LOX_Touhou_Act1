using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transport : MonoBehaviour
{
    //��λ����һ��������
    public GameObject another;
    //����ʱ��
    public float workTime = 1;
    private float workTimer = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        workTimer += Time.deltaTime;
        
        Debug.Log(workTimer);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (workTimer >= workTime)
        {
            workTimer = 0;
            another.gameObject.GetComponent<Transport>().workTimer = 0;
            collision.gameObject.GetComponent<Transform>().position = another.gameObject.GetComponent<Transform>().position;

        }
    }


}
