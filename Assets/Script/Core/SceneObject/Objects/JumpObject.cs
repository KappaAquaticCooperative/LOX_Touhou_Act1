using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class JumpObject : MonoBehaviour
{
    //��Ƶ
    public AudioClip jumpItemSound;
    //�������������ٵĸı�̶�
    public float shootDecrease;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�������С������ʱ�������ã������ٸ�����
        Player player = collision.gameObject.GetComponent<Player>();
        Player2 player2 = collision.gameObject.GetComponent<Player2>();
        if (player)
        {
            if (player.shootTime > player.shootLimit)
            {
                player.shootTime -= player.shootDecrease;
                player.itemNumber += 1;
            }
            AudioSource.PlayClipAtPoint(jumpItemSound, transform.position);
            Destroy(this.gameObject);
            CerateObject.exist -= 1;
        }
        if (player2)
        {
            if (player2.shootTime > player2.shootLimit)
            {
                player2.shootTime -= player2.shootDecrease;
                player2.itemNumber += 1;
            }
            AudioSource.PlayClipAtPoint(jumpItemSound, transform.position);
            Destroy(this.gameObject);
            CerateObject.exist -= 1;
        }
    }
    
}
