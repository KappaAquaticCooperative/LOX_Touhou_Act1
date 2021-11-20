using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class P2HpBar : MonoBehaviour
{
    public float maxHp;
    public float nowHp;
    public Image bar;
    public Player2 player2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
            maxHp = player2.maxHP;
            nowHp = player2.HP;
            bar.fillAmount = nowHp / maxHp;
        
    }

}
