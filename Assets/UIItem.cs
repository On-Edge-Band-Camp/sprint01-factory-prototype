using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItem : MonoBehaviour
{
    public GameItem Item;
    public Image image;
    public Animator ac;
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItem(GameItem item)
    {
        Debug.Log("SetItem");
        Item = item;
        image.sprite = item.Sprite;
    }

    public void updateCount(int itemCount)
    {
        Debug.Log("updateCount");
        image.enabled = true;
        text.text = itemCount.ToString();
        ac.Play("Pop");
    }

    public void Hide()
    {
        image.enabled = false;
        text.text = " ";
    }

    public void Clear()
    {
        Item = null;
        image.sprite = null;
        image.enabled = false;
        text.text = " ";
    }
}
