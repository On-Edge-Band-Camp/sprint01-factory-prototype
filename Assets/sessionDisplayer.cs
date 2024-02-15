using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class sessionDisplayer : MonoBehaviour
{
    public TMP_Text Text;

    public void OnConnectionSucess(int sessionNum)
    {
        if(sessionNum < 0)
        { 
            Text.text = $"Session: {sessionNum}";
        }

        else
        {
            Text.text = $"Session: {sessionNum}";
        }
    }

    public void OnConnectionFail(string errorMsg)
    {
        Text.text = errorMsg;
    }
}
