using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    public float speed;
    //思路：获取这两个参照物的X坐标，物体在之间移动，超过其中一方X坐标时方向相反
    public Transform moveGoal;
    public Transform moveBack;
    //移动方向判定
    public bool moveDirection=true;
    //设置平台移动的起点与终点
    private Vector3 goal;
    private Vector3 back;
    private Rigidbody2D rigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
        //goal = moveGoal.GetComponent<Transform>().position;
        goal = moveGoal.position;
        //back = moveBack.GetComponent<Transform>().position;
        back = moveBack.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveDirection == true)
        {
            rigidbody2D.velocity = new Vector2(speed, 0);
            if (this.transform.position.x >= goal.x)            
                moveDirection = false;
            
        }
        if (moveDirection == false)
        {
            rigidbody2D.velocity = new Vector2(-speed, 0);
            if (this.transform.position.x <= back.x)
                moveDirection = true;
        }

        
    }
}
