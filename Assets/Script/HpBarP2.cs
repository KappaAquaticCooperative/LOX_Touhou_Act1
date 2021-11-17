using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HpBarP2 : MonoBehaviour
{
    public float maxHp;
    public float nowHp;
    public Image bar;
    public Player2 player2;
    // Start is called before the first frame update
    void Start()
    {
        maxHp = player2.maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        nowHp = player2.HP;
        bar.fillAmount = nowHp / maxHp;
    }
}
