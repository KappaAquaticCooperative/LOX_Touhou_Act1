using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P1Itemtext : MonoBehaviour
{
    public Player player;
    public Player2 player2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            this.gameObject.GetComponent<Text>().text = player.itemNumber.ToString();
        }
        if (player2)
        {
            this.gameObject.GetComponent<Text>().text = player2.itemNumber.ToString();
        }

    }
}
