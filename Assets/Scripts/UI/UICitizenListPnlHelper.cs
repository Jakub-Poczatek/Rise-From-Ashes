using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICitizenListPnlHelper : MonoBehaviour
{
    public Transform contentParent;
    public Button cancelBtn;
    private GameObject originalCitizenGO;
    private Dictionary<Citizen, GameObject> citizenList;

    // Start is called before the first frame update
    void Start()
    {
        Show();
        citizenList = new Dictionary<Citizen, GameObject>();
        originalCitizenGO = contentParent.GetChild(0).gameObject;
        contentParent.GetChild(0).gameObject.SetActive(false);
        UpdateCitizensList();
        Hide();
    }

    public void DisplayCitizensMenu()
    {
        UpdateCitizensList();
        Show();
    }

    private void UpdateCitizensList()
    {
        foreach(GameObject c in PopulationManagement.Instance.Citizens)
        {
            if (!citizenList.Keys.Contains(c.GetComponent<Citizen>()))
            {
                CreateCitizenEntry(c.GetComponent<Citizen>());
            }
            else UpdateCitizenEntry(c.GetComponent<Citizen>(), citizenList[c.GetComponent<Citizen>()]);
        }
    }


    private void CreateCitizenEntry(Citizen citizen)
    {
        GameObject citizenGO = Instantiate(originalCitizenGO);
        UpdateCitizenEntry(citizen, citizenGO);
        citizenList.Add(citizen, citizenGO);
    }

    private void UpdateCitizenEntry(Citizen citizen, GameObject citizenGO)
    {
        citizenGO.transform.parent = contentParent;
        citizenGO.transform.Find("CitizenTitle").GetComponent<TMP_Text>().text =
            citizen.citizenData.name + " - " + citizen.citizenData.occupation;
        citizenGO.transform.Find("CitizenDetails").GetComponent<TMP_Text>().text =
            "Happiness: " + citizen.citizenData.happiness + "    " +
            "Food: " + citizen.citizenData.Food + "    " +
            "Work - Rest Ratio: " + citizen.citizenData.WorkRestRatio.Item1 + ":" + citizen.citizenData.WorkRestRatio.Item2;
        citizenGO.transform.Find("HomeStatus").GetComponent<Toggle>().isOn = citizen.HouseBuilding != null;
        citizenGO.transform.Find("LocateBtn").GetComponent<Button>()
                .onClick.AddListener(() => LocateCitizen(citizen));
        citizenGO.SetActive(true);
    }

    private void LocateCitizen(Citizen citizen)
    {
        CameraMovement.Instance.SnapCamera(citizen.gameObject.transform.position);
        GameManager.Instance.uiController.citizenPanelHelper.DisplayCitizenMenu(citizen.citizenData);
        Hide();
    }

    private void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void DestroyCitizenEntry(Citizen citizen)
    {
        Destroy(citizenList[citizen]);
        citizenList.Remove(citizen);
    }
}
