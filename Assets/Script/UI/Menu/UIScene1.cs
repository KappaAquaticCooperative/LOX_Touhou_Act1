using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIScene1 : MonoBehaviour
{
    public Player p1;
    public Player2 p2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (p1.HP <= 0 || p2.HP <= 0)
        {
            
            SceneManager.LoadScene(2);
        }   
    }


    
}
