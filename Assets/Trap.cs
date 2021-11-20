using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
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
        Player2 player2 = collision.gameObject.GetComponent<Player2>();
        
        if (player)
        {
            player.TakeDamage(1);
            player.transform.position = player.reSetPoint.position;
        }
        if (player2)
        {
            player2.TakeDamage(1);
            player2.transform.position = player2.reSetPoint.position;
        }

    }
}
