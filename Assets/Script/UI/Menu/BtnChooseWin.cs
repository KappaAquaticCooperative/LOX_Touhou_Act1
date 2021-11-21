using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BtnChooseWin : MonoBehaviour
{
    public GameObject button;
    public bool changed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }
    void ChangeColor()
    {
        if (changed == true)
            this.GetComponent<Image>().color = Color.blue;
        if (changed == false)
            this.GetComponent<Image>().color = Color.green;
    }
}
