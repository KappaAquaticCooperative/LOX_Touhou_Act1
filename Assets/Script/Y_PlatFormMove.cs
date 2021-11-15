using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_PlatFormMove : MonoBehaviour
{
    public float speed;
    //˼·����ȡ�������������Y���꣬������֮���ƶ�����������һ��Y����ʱ�����෴
    public Transform moveGoal;
    public Transform moveBack;
    //�ƶ������ж�
    public bool moveDirection = true;
    //����ƽ̨�ƶ���������յ�
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
