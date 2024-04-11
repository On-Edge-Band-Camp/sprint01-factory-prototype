using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMovementEffect : MonoBehaviour
{
    public int effectChoice;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(effectChoice == 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(Mathf.Cos(Time.time)) * 0.05f + 1,
                Mathf.Abs(Mathf.Sin(Time.time)) * 0.03f + 1, 1);
        }else if(effectChoice == 1)
        {
            transform.localScale = new Vector3(Mathf.Abs(Mathf.Cos(Time.time * 40)) * 0.02f + 1,
                Mathf.Abs(Mathf.Sin(Time.time * 50)) * 0.05f + 1, 1);
        }

    }
}
