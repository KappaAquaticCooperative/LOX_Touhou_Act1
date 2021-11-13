using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MpBarP2 : MonoBehaviour
{
    public float MaxMp;
    public float MP;
    public Image bar;


    

    public Player2 player;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        MaxMp = player.shootTime;
        if (player)
        {
            MP = player.shootTimer;
            bar.fillAmount = MP / MaxMp;
        }
    }
}
