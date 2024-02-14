using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionDisplay : MonoBehaviour
{
    public TMPro.TextMeshPro displayField;

    public void OnConnectionSuccess(int sessionNumber)
    {
        if(sessionNumber < 0)
        {
            displayField.text = $"Local Session: {sessionNumber}";
        }
        else
        {
            displayField.text = $"Server Session: {sessionNumber}";
        }
        
    }

    public void OnConnectionFail(string errorMessage)
    {
        displayField.text = errorMessage;
    }
}
