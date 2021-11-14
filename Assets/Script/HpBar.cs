using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HpBar : MonoBehaviour
{
    public float maxHp;
    public float nowHp;
    public Image bar;
    public Player player;
    public Player2 player2;
    // Start is called before the first frame update
    void Start()
    {
      
   
    }

    // Update is called once per frame
    //现在发现，如果把两个代码块都放在update里，排在后面的会出现不归零的情况，调试后发现
    //分别使用update和Fixedupdate可以让这两个都正常运行（不知这样做是否符合规范）
    void Update()
    {
      
        if (!player2)
        {
            maxHp = player.maxHP;
            nowHp = player.HP;
            bar.fillAmount = nowHp / maxHp;
        }
        
    }
    private void FixedUpdate()
    {
        if (!player)
        {
            maxHp = player2.maxHP;
            nowHp = player2.HP;
            bar.fillAmount = nowHp / maxHp;
        }
    }
}
