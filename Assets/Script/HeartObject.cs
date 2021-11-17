using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartObject : MonoBehaviour
{
    //该物体对玩家生命的改变程度
    public float HPchange=1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player&&player.HP<=player.maxHP-HPchange)
        {
            player.HP += HPchange;
        }
        Player2 player2 = collision.gameObject.GetComponent<Player2>();
        if (player2&&player2.HP<=player2.maxHP-HPchange)
        {
            player2.HP += HPchange;
        }
        Destroy(this.gameObject);
        CerateObject.exist -= 1;
    }
}
