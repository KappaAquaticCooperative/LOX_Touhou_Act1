using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_PlatFormMove : MonoBehaviour
{
    public float speed;
    //思路：获取这两个参照物的Y坐标，物体在之间移动，超过其中一方Y坐标时方向相反
    public Transform moveGoal;
    public Transform moveBack;
    //移动方向判定
    public bool moveDirection = true;
    //设置平台移动的起点与终点
    private Vector3 goal;
    private Vector3 back;
    private Rigidbody2D rigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
        goal = moveGoal.position;
        back = moveBack.position;
    }
    // Update is called once per frame
    void Update()
    {

        if (moveDirection == true)
        {
            rigidbody2D.velocity = new Vector2(0, speed);
            if (this.transform.position.y >= goal.y)           
                moveDirection = false;           
        }
        if (moveDirection == false)
        {
            rigidbody2D.velocity = new Vector2(0, -speed);
            if (this.transform.position.y <= back.y)
                moveDirection = true;
        }
    }
}
