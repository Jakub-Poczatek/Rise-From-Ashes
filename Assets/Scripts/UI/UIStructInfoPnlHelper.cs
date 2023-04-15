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
    private Structure currentStructure;

    public Structure CurrentStructure { get => currentStructure; set => currentStructure = value; }

    // Start is called before the first frame update
    void Start()
    {
        citizenButtons = new GameObject[5];
        for (int i = 0; i < citizenButtons.Length; i++)
        {
            citizenButtons[i] = Instantiate(citizenList.transform.GetChild(0).gameObject);
            citizenButtons[i].transform.SetParent(citizenList.transform);
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
        resourceGenParent.SetActive(false);
        structureName.text = structure.StructureName;
        structureLevel.text = "Level " + structure.StructureLevel;
        citizenListLbl.text = "List of Citizens " + structure.Citizens.Count + "/" + structure.MaxCitizenCapacity;
        HideCitizenList();
        ClearButtonListeners();
        DisplayCitizenList(structure);

        if (structure.GetType() == typeof(WorkableStructure))
            DisplayWorkableStructure((WorkableStructure) structure);

        currentStructure = structure;

        upgradeBtn.gameObject.GetComponent<HoverTip>().tipToShow = structure.UpgradeCost.ToString();
        Show();
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
            int index = i;
            citizenButtons[i].SetActive(true);
            citizenButtons[i].GetComponentInChildren<TMP_Text>().text = structure.Citizens.ElementAt(i).Key.GetComponent<Citizen>().citizenData.name;
            if (structure.Citizens.ElementAt(i).Value)
                citizenButtons[i].GetComponentInChildren<Button>().GetComponent<Image>().color = new Color(0.0078f, 0.3922f, 0.2510f);
            else citizenButtons[i].GetComponentInChildren<Button>().GetComponent<Image>().color = new Color(0.3922f, 0.0078f, 0.1490f);
            citizenButtons[i].GetComponentInChildren<Button>().onClick.AddListener(() => 
                LocateCitizen(structure.Citizens.ElementAtOrDefault(index).Key.GetComponent<Citizen>()));
        }
    }

    private void ClearButtonListeners()
    {
        foreach (GameObject cb in citizenButtons)
        {
            cb.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        }
    }

    private void LocateCitizen(Citizen citizen)
    {
        CameraMovement.Instance.SnapCamera(citizen.gameObject.transform.position);
        GameManager.Instance.uiController.citizenPanelHelper.DisplayCitizenMenu(citizen.citizenData);
        Hide();
    }

    private void HideCitizenList()
    {
        foreach (GameObject cb in citizenButtons)
            cb.SetActive(false);
    }
}
