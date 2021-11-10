using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    //移动速度
    public float speed=3;

    public GameObject bg1;
    public GameObject bg2;

    //私有变量
    private Transform transform1;
    private Transform transform2;
    private Vector2 start1;
    private Vector2 start2;
    // Start is called before the first frame update
    void Start()
    {
        transform1 = bg1.GetComponent<Transform>();
        transform2 = bg2.GetComponent<Transform>();
        start1 = transform1.position;
        start2 = transform2.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform1.Translate(-transform.right * speed * Time.deltaTime, 0);
        transform2.Translate(-transform.right * speed * Time.deltaTime, 0);
        if (transform2.position.x < start1.x)
        {
            transform1.position = start1;
            transform2.position = start2;            
        }
    }
}
