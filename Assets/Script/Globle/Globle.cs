using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globle : MonoBehaviour
{
    public static int P1Score = 0;
    public static int P2Score = 0;
    public static GameMode gameMode = GameMode.win2;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public enum GameMode
    {
        win2,
        win3,
        win5,
    }
    

}

