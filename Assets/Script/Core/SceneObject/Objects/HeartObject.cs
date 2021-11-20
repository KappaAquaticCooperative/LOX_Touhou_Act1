using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartObject : MonoBehaviour
{
    //音频
    public AudioClip HeartItemSound;
    //该物体对玩家生命的改变程度
    
    public float HPchange=1;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
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
            AudioSource.PlayClipAtPoint(HeartItemSound, transform.position,1f);

            player.HP += HPchange;    
        }
        Player2 player2 = collision.gameObject.GetComponent<Player2>();
        if (player2&&player2.HP<=player2.maxHP-HPchange)
        {
            AudioSource.PlayClipAtPoint(HeartItemSound, transform.position);
            player2.HP += HPchange;     
        }
        Destroy(this.gameObject);
        CerateObject.exist -= 1;
    }
}
