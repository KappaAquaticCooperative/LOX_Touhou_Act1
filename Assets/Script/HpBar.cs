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
    //���ڷ��֣��������������鶼����update����ں���Ļ���ֲ��������������Ժ���
    //�ֱ�ʹ��update��Fixedupdate���������������������У���֪�������Ƿ���Ϲ淶��
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
