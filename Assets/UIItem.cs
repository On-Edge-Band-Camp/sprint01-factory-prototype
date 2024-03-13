using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItem : MonoBehaviour
{
    public SOItem soitem;
    public Image image;
    Animator ac;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        ac = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItem(SOItem soitem)
    {
        ac = GetComponent<Animator>();
        this.soitem = soitem;
        image.sprite = soitem.Sprite;
        image.enabled = true;
        ac.Play("Pop");
    }

    public void updateCount(int itemCount)
    {
        GetComponentInChildren<TMP_Text>().text = itemCount.ToString();
        ac.Play("Pop");
    }

    public void Hide()
    {
        image = GetComponent<Image>();
        image.enabled = false;
        GetComponentInChildren<TMP_Text>().text = " ";
    }

    public void Clear()
    {
        soitem = null;
        image.sprite = null;
        image.enabled = false;
        GetComponentInChildren<TMP_Text>().text = " ";
    }
}
