using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIEndMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WinGame();
    }
    //回到菜单
    public void OnClickToMenu()
    {
        Globle.P1Score = 0;
        Globle.P2Score = 0;
        SceneManager.LoadSceneAsync(0);
        
        
    }
    //选择界面
    public void OnClickToNext()
    {
        SceneManager.LoadSceneAsync(1);
        
        

    }
    //判断总体胜负
    public void WinGame()
    {
        if (Globle.gameMode == Globle.GameMode.win2)
        {
            if (Globle.P1Score == 2||Globle.P2Score==2)
                SceneManager.LoadSceneAsync(3);
        }
        if (Globle.gameMode == Globle.GameMode.win3)
        {
            if (Globle.P1Score == 3 || Globle.P2Score == 3)
                SceneManager.LoadSceneAsync(3);
        }
        if (Globle.gameMode == Globle.GameMode.win5)
        {
            if (Globle.P1Score == 5 || Globle.P2Score == 5)
                SceneManager.LoadSceneAsync(3);
        }

    }
}
