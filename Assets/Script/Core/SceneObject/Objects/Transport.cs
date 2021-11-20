using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transport : MonoBehaviour
{
    //定位到另一个传送门
    public GameObject another;
    //缓冲时间
    public float workTime = 1;
    private float workTimer = 1;
    //音效
    public AudioClip sukimaSound;

    private AudioSource m_audioSource;
    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        workTimer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (workTimer >= workTime)
        {
            workTimer = 0;
            another.gameObject.GetComponent<Transport>().workTimer = 0;
            collision.gameObject.GetComponent<Transform>().position = another.gameObject.GetComponent<Transform>().position;
            another.GetComponent<AudioSource>().clip = sukimaSound;
            another.GetComponent<AudioSource>().Play();
        }
    }


}
