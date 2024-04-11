using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineSelectButton : MonoBehaviour
{
    public Tooltip MachineTooltip;

    public string MachineName;

    [TextArea]
    public string Description;

    // Start is called before the first frame update
    void Start()
    {
        MachineTooltip = FindObjectOfType<GameManager>().MachineTooltip;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTooltip()
    {
        if (MachineTooltip != null)
        {
            MachineTooltip.gameObject.SetActive(true);
            MachineTooltip.WriteTooltipInfo(MachineName, Description);
        }
    }

    public void HideToolTip()
    {
        MachineTooltip.gameObject.SetActive(false);
    }
}
