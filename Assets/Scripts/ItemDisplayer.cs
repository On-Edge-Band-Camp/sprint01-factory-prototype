using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDisplayer : MonoBehaviour
{
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateSprite(GameItem item)
    {
        if (item == null)
        {
            sr.sprite = null;
        }
        else
        {
            sr.sprite = item.Sprite;
        }
    }
}
