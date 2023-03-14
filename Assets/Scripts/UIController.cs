using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private Action<string> OnBuildSingleStructureHandler;
    private Action<string> OnBuildRoadHandler;
    private Action<string> OnBuildResidentialHandler;
    private Action OnCancelActionHandler;
    private Action OnDemolishActionHandler;
    private Action OnConfirmActionHandler;
    private Action OnCitizenAssignHandler;

    public Button cancelActionBtn;
    public Button openBuildMenuBtn;
    public Button demolishBtn;
    public Button confirmActionBtn;
    public Button closeBuildMenuBtn;
    public GameObject cancelActionPnl;
    public GameObject buildingMenuPnl;
    public GameObject resourceGenStructsPnl;
    public GameObject roadStructsPnl;
    public GameObject residentialStructsPnl;
    public GameObject structureButtonPrefab;

    public TMP_Text goldAmountTxt;
    public TMP_Text foodAmountTxt;
    public TMP_Text woodAmountTxt;
    public TMP_Text stoneAmountTxt;
    public TMP_Text metalAmountTxt;

    public UIStructInfoPnlHelper structPanelHelper;
    public UICitizenInfoPnlHelper citizenPanelHelper;

    /*public TMP_Text infoPnlStructName;
    public TMP_Text infoPnlStructCost;
    public TMP_Text infoPnlStructLevel;
    public TMP_Text infoPnlStructType;
    public TMP_Text infoPnlStructIncome;*/

    public StructureRepository structureRepository;

    // Start is called before the first frame update
    void Start()
    {
        cancelActionPnl.SetActive(false);
        buildingMenuPnl.SetActive(false);
        citizenPanelHelper.Hide();

        //buildResidentialAreaBtn.onClick.AddListener(OnBuildAreaCallback);
        cancelActionBtn.onClick.AddListener(OnCancelActionCallback);
        confirmActionBtn.onClick.AddListener(OnConfirmActionCallback);
        openBuildMenuBtn.onClick.AddListener(OnOpenBuildMenu);
        demolishBtn.onClick.AddListener(OnDemolishHandler);
        closeBuildMenuBtn.onClick.AddListener(OnCloseMenuHandler);

        citizenPanelHelper.assignBtn.onClick.AddListener(OnCitizenAssignCallback);
    }

    public void ToggleCitizenInteraction(bool toggle, CitizenData citizenData = null)
    {
        if (toggle)
        {
            citizenPanelHelper.DisplayCitizenMenu(citizenData);
            citizenPanelHelper.Show();
        }
        else citizenPanelHelper.Hide();
    }

    public void DisplayStructureInfo(StructureBase structure)
    {
        structPanelHelper.DisplayStructureInfo(structure);
    }

    private void OnCloseMenuHandler()
    {
        buildingMenuPnl.SetActive(false);
    }

    private void OnDemolishHandler()
    {
        OnDemolishActionHandler?.Invoke();
        cancelActionPnl.SetActive(true);
        OnCloseMenuHandler();
    }

    private void OnOpenBuildMenu()
    {
        buildingMenuPnl.SetActive(true);
        prepareBuildMenu();
    }

    private void prepareBuildMenu()
    {
        CreateButtonsInPanel(resourceGenStructsPnl.transform, structureRepository.GetResourceGenStructNames(), OnBuildSingleStructureCallback);
        CreateButtonsInPanel(roadStructsPnl.transform, new List<string>() { structureRepository.GetRoadStructName() }, OnBuildRoadCallback);
        CreateButtonsInPanel(residentialStructsPnl.transform, new List<string>() { structureRepository.GetResidentialStructName() }, OnBuildResidentialCallback);
    }

    private void CreateButtonsInPanel(Transform panelTransform, List<string> dataToShow, Action<string> callback)
    {
        if (dataToShow.Count > panelTransform.childCount)
        {
            int diff = dataToShow.Count - panelTransform.childCount;
            for (int i = 0; i < diff; i++)
            {
                Instantiate(structureButtonPrefab, panelTransform);
            }
        }

        for (int i = 0; i < panelTransform.childCount; i++)
        {
            var button = panelTransform.GetChild(i).GetComponent<Button>();
            if (button != null)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = dataToShow[i];
                button.onClick.AddListener(() => callback(button.GetComponentInChildren<TextMeshProUGUI>().text));
            }
        }
    }

    private void OnCitizenAssignCallback()
    {
        OnCitizenAssignHandler?.Invoke();
        citizenPanelHelper.Hide();
        cancelActionPnl.gameObject.SetActive(true);
    }

    private void OnBuildSingleStructureCallback(string structureName)
    {
        PrepareUIForBuilding();
        OnBuildSingleStructureHandler?.Invoke(structureName);
    }

    private void OnBuildRoadCallback(string structureName)
    {
        PrepareUIForBuilding();
        OnBuildRoadHandler?.Invoke(structureName);
    }

    private void OnBuildResidentialCallback(string structureName)
    {
        PrepareUIForBuilding();
        OnBuildResidentialHandler?.Invoke(structureName);
    }

    private void PrepareUIForBuilding()
    {
        cancelActionPnl.SetActive(true);
        OnCloseMenuHandler();
    }

    private void OnConfirmActionCallback()
    {
        cancelActionPnl.SetActive(false);
        OnConfirmActionHandler?.Invoke();
    }

    private void OnCancelActionCallback()
    {
        cancelActionPnl.SetActive(false);
        OnCancelActionHandler?.Invoke();
    }

    public void AddListenerOnCitizenAssignEvent(Action listener)
    {
        OnCitizenAssignHandler += listener;
    }

    public void AddListenerOnBuildSingleStructureEvent(Action<string> listener)
    {
        OnBuildSingleStructureHandler += listener;
    }

    public void RemoveListenerOnBuildSingleStructureEvent(Action<string> listener)
    {
        OnBuildSingleStructureHandler -= listener;
    }

    public void AddListenerOnBuildRoadEvent(Action<string> listener)
    {
        OnBuildRoadHandler += listener;
    }

    public void RemoveListenerOnBuildRoadEvent(Action<string> listener)
    {
        OnBuildRoadHandler -= listener;
    }

    public void AddListenerOnBuildResidentialEvent(Action<string> listener)
    {
        OnBuildResidentialHandler += listener;
    }

    public void RemoveListenerOnBuildResidentialEvent(Action<string> listener)
    {
        OnBuildResidentialHandler -= listener;
    }

    public void AddListenerOnCancelActionEvent(Action listener)
    {
        OnCancelActionHandler += listener;
    }

    public void RemoveListenerOnCancelActionEvent(Action listener)
    {
        OnCancelActionHandler -= listener;
    }

    public void AddListenerOnConfirmActionEvent(Action listener)
    {
        OnConfirmActionHandler += listener;
    }

    public void RemoveListenerOnConfirmActionEvent(Action listener)
    {
        OnConfirmActionHandler -= listener;
    }

    public void AddListenerOnDemolishActionEvent(Action listener)
    {
        OnDemolishActionHandler += listener;
    }

    public void RemoveListenerOnDemolishActionEvent(Action listener)
    {
        OnDemolishActionHandler -= listener;
    }

    public void UpdateResourceValues(Cost cost)
    {
        goldAmountTxt.text = cost.gold.ToString();
        foodAmountTxt.text = cost.food.ToString();
        woodAmountTxt.text = cost.wood.ToString();
        stoneAmountTxt.text = cost.stone.ToString();
        metalAmountTxt.text = cost.metal.ToString();
    }

    public void HideStructureInfo()
    {
        structPanelHelper.Hide();
    }
}
