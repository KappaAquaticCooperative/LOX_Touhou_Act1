using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeColor());
    }

    // Update is called once per frame
    void Update()
    {
        if (Globle.P1Score > Globle.P2Score)
            this.gameObject.GetComponent<Text>().text = "P1";
        if (Globle.P1Score < Globle.P2Score)
            this.gameObject.GetComponent<Text>().text = "p2";
    }
    //×ÖÌå±äÉ«
    IEnumerator ChangeColor()
    {
        while (true)
        {
            float r, g, b;
            r = Random.Range(0, 1f);
            g = Random.Range(0, 1f);
            b = Random.Range(0, 1f);
            this.gameObject.GetComponent<Text>().color = new Color(r, g, b);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
