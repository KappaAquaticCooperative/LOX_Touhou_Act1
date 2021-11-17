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
            player.isHurt = true;

        Player2 player2 = collision.gameObject.GetComponent<Player2>();
        if (player2)
            player2.isHurt = true;

        if(collision.gameObject.name== "face-block")
        {
            collision.gameObject.GetComponent<CerateObject>().TakeDamege(damage);
        }

        if (collision.gameObject.name == "JumpItem")
        {
            Destroy(collision.gameObject);
            CerateObject.exist -= 1;
        }

        if (collision.gameObject.tag != "sukima") 
        Destroy(this.gameObject);
    }






}
