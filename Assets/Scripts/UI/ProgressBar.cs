using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{
    public MachineDetails MachineRefrence;
    public Slider slider;
    public UIItem UIItem;

    // Update is called once per frame
    void Update()
    {
        slider.value = MachineRefrence.machine.currentProcessInPercent;
        if (slider.value >= 1 && UIItem != null)
        {
            UIItem.BigPop();
        }
    }
}
