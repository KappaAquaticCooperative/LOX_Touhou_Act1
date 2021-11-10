using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed=2;
    public float damage=1;
    

    private Transform transform;


    void Start()
    {
        
    }


    void Update()
    {
        
        
            
            
        
       
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player)
            player.TakeDamage(damage);
        Player2 player2 = collision.gameObject.GetComponent<Player2>();
        if (player2)
            player2.TakeDamage(damage);

        Destroy(this.gameObject);
    }






}