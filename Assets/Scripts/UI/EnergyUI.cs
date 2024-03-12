using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyUI : MonoBehaviour
{
    public PlayerResources resources;
    public TMP_Text text;

    public bool isCost;
    public Machine machine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCost)
        {
            text.text = machine.energyCost.ToString();
        }
        else
        {
            text.text = $"Energy: {resources.Energy}";
        }
    }
}
