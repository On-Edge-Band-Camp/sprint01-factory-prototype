using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Displayer : MonoBehaviour
{
    Transporter transporter;
    SpriteRenderer sr;
    Sprite HoldingItem;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        transporter = transform.parent.GetComponent<Transporter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transporter.HoldingItem != null)
        {
            HoldingItem = transporter.HoldingItem.sprite;
        }
        else
        {
            HoldingItem = null;
        }
        sr.sprite = HoldingItem;
    }
}
