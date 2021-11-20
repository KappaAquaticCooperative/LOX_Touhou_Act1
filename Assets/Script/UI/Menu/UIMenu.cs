using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //从主菜单进入开始选择模式的页面,会清空计数板
    public void OnPlayClicked()
    {
        Globle.P1Score = 0;
        Globle.P2Score = 0;
        
        SceneManager.LoadSceneAsync(1);
        
    }
    //选择回到主菜单，会清空计数板
    public void ToMenuClicked()
    {
        SceneManager.LoadSceneAsync(0);
        Globle.P1Score = 0;
        Globle.P2Score = 0;
    }
    //在模式选择页面选择胜利条件
    public void LevelChooseW1()
    {
        Globle.gameMode = Globle.GameMode.win2;
    }
    public void LevelChooseW2()
    {
        Globle.gameMode = Globle.GameMode.win3;
    }
    public void LevelChooseW3()
    {
        Globle.gameMode = Globle.GameMode.win5;
    }
    //正式开始游戏，随机抽取地图
    public void StartPlayGame()
    {
        Debug.Log("模式："+Globle.gameMode);
        int a = Random.Range(4,7);
        SceneManager.LoadSceneAsync(a);
    }

}
