using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MpBar : MonoBehaviour
{
    public float MaxMp;
    public float MP;
    public Image bar;

    public Player player;
    public Player2 player2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //MP����Ų�������HP�����������⣬������û��
        if (player)
        {
            MaxMp = player.shootTime;
            MP = player.shootTimer;
            bar.fillAmount = MP / MaxMp;
        }
        if(player2)
        {
            MaxMp = player2.shootTime;
            MP = player2.shootTimer;
            bar.fillAmount = MP / MaxMp;
        }
    }
}
