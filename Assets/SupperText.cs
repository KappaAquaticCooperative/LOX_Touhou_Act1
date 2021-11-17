using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SupperText : MonoBehaviour
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
            if (player.isSupper == false)            
                this.gameObject.GetComponent<Text>().text = "N";            
            if (player.isSupper == true)
                this.gameObject.GetComponent<Text>().text = "Y";
        }
        if (player2)
        {
            if (player2.isSupper == false)
                this.gameObject.GetComponent<Text>().text = "N";
            if (player2.isSupper == true)
                this.gameObject.GetComponent<Text>().text = "Y";
        }
    }
}
