using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupperItem : MonoBehaviour
{
    public float colorTime=1;
    public float existTime = 5;
    public float r;
    public float g;
    public float b;
    
    //私有变量
    private float existTimer = 0;
    public float colorTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        ChangeColor(r, g, b);
    }

    // Update is called once per frame
    void Update()
    {
        colorTimer += Time.deltaTime;
        if (colorTimer >= colorTime)
            ChangeColor(r,g,b);
        existTimer += Time.deltaTime;
        if (existTimer >= existTime)
            Destroy(this.gameObject);


    }
    public void ChangeColor(float r,float g,float b)
    {
        r = Random.Range(0f, 1f);
        g = Random.Range(0f, 1f);
        b = Random.Range(0f, 1f);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(r, g, b);
        colorTimer = 0;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        Player2 player2 = collision.gameObject.GetComponent<Player2>();
        //用于强化玩家
        if (player)
        {
            player.shootTime /= 3;
            player.bulletSpeed *= 3;
            player.isSupper = true;
        }
        if (player2) 
        { 
            player2.shootTime /= 3;
            player2.bulletSpeed *= 3;
            player2.isSupper = true;
        }


        Destroy(this.gameObject);
    }


}
