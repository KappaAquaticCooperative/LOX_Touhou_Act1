using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MpBarP1 : MonoBehaviour
{
    public float MaxMp;
    public float MP;
    public Image bar;

    public Player player;


    // Start is called before the first frame update
    void Start()
    {
        MaxMp = player.shootTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            MP = player.shootTimer;
            bar.fillAmount = MP / MaxMp;
        }
    }
}
