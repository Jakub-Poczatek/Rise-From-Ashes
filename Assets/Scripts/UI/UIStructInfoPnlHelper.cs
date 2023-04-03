using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStructInfoPnlHelper : MonoBehaviour
{
    public TMP_Text structureName, structureLevel, citizenListLbl, resourceType, 
        resourceGenAmount;
    public Button upgradeBtn, demolishBtn, cancelBtn;
    public GameObject citizenList, resourceGenParent;

    private GameObject[] citizenButtons;

    // Start is called before the first frame update
    void Start()
    {
        citizenButtons = new GameObject[5];
        for (int i = 0; i < citizenButtons.Length; i++)
        {
            citizenButtons[i] = Instantiate(citizenList.transform.GetChild(0).gameObject);
            citizenButtons[i].transform.parent = citizenList.transform;
            citizenButtons[i].SetActive(false);
        }
        Destroy(citizenList.transform.GetChild(0).gameObject);
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        HideCitizenList();
        gameObject.SetActive(false);
    }

    public void DisplayStructureInfo(Structure structure)
    {
        Show();
        resourceGenParent.SetActive(false);
        structureName.text = structure.StructureName;
        structureLevel.text = "Level " + structure.StructureLevel;
        citizenListLbl.text = "List of Citizens " + structure.Citizens.Count + "/" + structure.MaxCitizenCapacity;
        DisplayCitizenList(structure);

        if (structure.GetType() == typeof(WorkableStructure))
            DisplayWorkableStructure((WorkableStructure) structure);
    }

    private void DisplayWorkableStructure(WorkableStructure structure)
    {
        resourceGenParent.SetActive(true);
        resourceType.text = structure.ResourceType.ToString();
        resourceGenAmount.text = structure.GenAmount.ToString();
    }

    private void DisplayCitizenList(Structure structure)
    {
        for (int i = 0; i < structure.Citizens.Count; i++)
        {
            citizenButtons[i].SetActive(true);
            citizenButtons[i].GetComponentInChildren<TMP_Text>().text = structure.Citizens.ElementAt(i).Key.GetComponent<Citizen>().citizenData.name;
        }
    }

    private void HideCitizenList()
    {
        foreach (GameObject cb in citizenButtons)
            cb.SetActive(false);
    }
}
