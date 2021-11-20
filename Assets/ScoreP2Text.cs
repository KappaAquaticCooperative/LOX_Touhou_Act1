using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreP2Text : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        getText();
    }
    void getText()
    {

        this.GetComponent<Text>().text = Globle.P2Score.ToString();


    }
}
