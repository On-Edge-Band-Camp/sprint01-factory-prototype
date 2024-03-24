using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    [SerializeField] TMP_Text ToolTipName;
    [SerializeField] TMP_Text TooltipDescription;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WriteTooltipInfo(string name, string description)
    {
        ToolTipName.text = name;
        TooltipDescription.text = description;
    }
}
