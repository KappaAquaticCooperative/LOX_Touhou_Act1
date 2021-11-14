using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HPText : MonoBehaviour
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
        if (!player2)
        {
            this.gameObject.GetComponent<Text>().text = player.HP.ToString();
        }
        
    }
    private void FixedUpdate()
    {
        if (!player)
        {
            this.gameObject.GetComponent<Text>().text = player2.HP.ToString();
        }
    }
}
