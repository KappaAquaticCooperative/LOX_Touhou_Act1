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

    //�����˵����뿪ʼѡ��ģʽ��ҳ��,����ռ�����
    public void OnPlayClicked()
    {
        Globle.P1Score = 0;
        Globle.P2Score = 0;
        
        SceneManager.LoadSceneAsync(1);
        
    }
    //ѡ��ص����˵�������ռ�����
    public void ToMenuClicked()
    {
        SceneManager.LoadSceneAsync(0);
        Globle.P1Score = 0;
        Globle.P2Score = 0;
    }
    //��ģʽѡ��ҳ��ѡ��ʤ������
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
    //��ʽ��ʼ��Ϸ�������ȡ��ͼ
    public void StartPlayGame()
    {
        Debug.Log("ģʽ��"+Globle.gameMode);
        int a = Random.Range(4,7);
        SceneManager.LoadSceneAsync(a);
    }

}
