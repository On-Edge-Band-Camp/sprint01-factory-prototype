using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomMenu : MonoBehaviour
{
    Animator AC;
    public GameObject OpenButton;
    public GameObject CloseButton;

    // Start is called before the first frame update
    void Start()
    {
        AC = GetComponent<Animator>();
        AC.SetBool("Open", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        AC.SetBool("Open", true);
        CloseButton.SetActive(true);
        OpenButton.SetActive(false);
    }

    public void Close()
    {
        AC.SetBool("Open", false);
        OpenButton.SetActive(true);
        CloseButton.SetActive(false);
    }
}
