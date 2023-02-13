using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStructInfoPnlHelper : MonoBehaviour
{
    public TMP_Text infoPnlStructName, infoPnlStructCost, infoPnlStructLevel, 
        infoPnlStructType, infoPnlStructIncome;

    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void DisplayStructureInfo(StructureBase structure)
    {
        Show();
        HideElement(infoPnlStructType.gameObject);
        HideElement(infoPnlStructIncome.gameObject);
        SetText(infoPnlStructName, structure.name);
        SetText(infoPnlStructCost, "-" + structure.maintenanceGoldCost.ToString());

        if (structure.GetType() == typeof(ResourceGenStruct))
            DisplayResourceGenStruct((ResourceGenStruct) structure);
    }

    public void DisplayResourceGenStruct(ResourceGenStruct structure)
    {
        SetText(infoPnlStructType, structure.resourceType.ToString());
        SetText(infoPnlStructIncome, "+" + structure.resourceGenAmount.ToString());
    }

    private void HideElement(GameObject element)
    {
        element.transform.parent.gameObject.SetActive(false);
    }

    private void ShowElement(GameObject element)
    {
        element.transform.parent.gameObject.SetActive(true);
    }

    private void SetText(TMP_Text element, string value)
    {
        ShowElement(element.gameObject);
        element.text = value;
    }
}
