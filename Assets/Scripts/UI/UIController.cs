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
    public Button openChallengeMenuBtn;
    public Button openCitizenListMenuBtn;
    public GameObject cancelConfirmActionPnl;
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
    public TMP_Text happinessAmountTxt;
    public TMP_Text citizenSpawnTimeTxt;

    public UIStructInfoPnlHelper structPanelHelper;
    public UICitizenInfoPnlHelper citizenPanelHelper;
    public UIDebugPnlHelper debugPanelHelper;
    public UIChallengePnlHelper challengePanelHelper;
    public UICitizenListPnlHelper citizenListPanelHelper;

    /*public TMP_Text infoPnlStructName;
    public TMP_Text infoPnlStructCost;
    public TMP_Text infoPnlStructLevel;
    public TMP_Text infoPnlStructType;
    public TMP_Text infoPnlStructIncome;*/

    public StructureRepository structureRepository;

    // Start is called before the first frame update
    void Start()
    {
        cancelConfirmActionPnl.SetActive(false);
        PrepareBuildMenu();
        buildingMenuPnl.SetActive(false);
        citizenPanelHelper.Hide();

        //buildResidentialAreaBtn.onClick.AddListener(OnBuildAreaCallback);
        cancelActionBtn.onClick.AddListener(OnCancelActionCallback);
        confirmActionBtn.onClick.AddListener(OnConfirmActionCallback);
        openBuildMenuBtn.onClick.AddListener(() => ToggleBuildPanel(true));
        closeBuildMenuBtn.onClick.AddListener(() => ToggleBuildPanel(false));
        demolishBtn.onClick.AddListener(OnDemolishHandler);

        citizenPanelHelper.assignBtn.onClick.AddListener(OnCitizenAssignCallback);
        citizenPanelHelper.cancelBtn.onClick.AddListener(() => ToggleCitizenInteractionPanel(false));
        citizenPanelHelper.foodDecreaseBtn.onClick.AddListener(() => citizenPanelHelper.UpdateFood(-1));
        citizenPanelHelper.foodIncreaseBtn.onClick.AddListener(() => citizenPanelHelper.UpdateFood(1));
        citizenPanelHelper.sleepDecreaseBtn.onClick.AddListener(() => citizenPanelHelper.UpdateSleep(-5));
        citizenPanelHelper.sleepIncreaseBtn.onClick.AddListener(() => citizenPanelHelper.UpdateSleep(5));

        structPanelHelper.upgradeBtn.onClick.AddListener(() =>
        {
            structPanelHelper.CurrentStructure.Upgrade();
            ToggleStructureInteractionPanel(true, structPanelHelper.CurrentStructure);
        });
        structPanelHelper.cancelBtn.onClick.AddListener(() => ToggleStructureInteractionPanel(false));

        openChallengeMenuBtn.onClick.AddListener(() => ToggleChallengesPanel(true));
        challengePanelHelper.cancelBtn.onClick.AddListener(() => ToggleChallengesPanel(false));

        openCitizenListMenuBtn.onClick.AddListener(() => ToggleCitizenListPanel(true));
        citizenListPanelHelper.cancelBtn.onClick.AddListener(() => ToggleCitizenListPanel(false));
    }

    public void ToggleCitizenInteractionPanel(bool toggle, CitizenData citizenData = null)
    {
        if (toggle)
        {
            ToggleCancelConfirmPanel(false);
            ToggleBuildPanel(false);
            ToggleStructureInteractionPanel(false);
            ToggleChallengesPanel(false);
            ToggleCitizenListPanel(false);
            citizenPanelHelper.DisplayCitizenMenu(citizenData);
        }
        else citizenPanelHelper.Hide();
    }

    public void ToggleCancelConfirmPanel(bool toggle)
    {
        if (toggle)
        {
            ToggleBuildPanel(false);
        }
        cancelConfirmActionPnl.SetActive(toggle);
    }

    private void ToggleBuildPanel(bool toggle)
    {
        if (toggle)
        {
            ToggleCitizenInteractionPanel(false);
            ToggleCancelConfirmPanel(false);
            ToggleStructureInteractionPanel(false);
            ToggleChallengesPanel(false);
            ToggleCitizenListPanel(false);
        }
        buildingMenuPnl.SetActive(toggle);
    }

    public void ToggleStructureInteractionPanel(bool toggle, Structure structure = null)
    {
        if (toggle)
        {
            ToggleCitizenInteractionPanel(false);
            ToggleCancelConfirmPanel(false);
            ToggleBuildPanel(false);
            ToggleChallengesPanel(false);
            ToggleCitizenListPanel(false);
            structPanelHelper.DisplayStructureInfo(structure);
        }
        else structPanelHelper.Hide();
    }

    public void ToggleChallengesPanel(bool toggle)
    {
        if (toggle)
        {
            ToggleCitizenInteractionPanel(false);
            ToggleCancelConfirmPanel(false);
            ToggleBuildPanel(false);
            ToggleStructureInteractionPanel(false);
            ToggleCitizenListPanel(false);
            challengePanelHelper.DisplayChallengesMenu();
        }
        else challengePanelHelper.Hide();
    }

    public void ToggleCitizenListPanel(bool toggle)
    {
        if (toggle)
        {
            ToggleCitizenInteractionPanel(false);
            ToggleCancelConfirmPanel(false);
            ToggleBuildPanel(false);
            ToggleStructureInteractionPanel(false);
            ToggleChallengesPanel(false);
            citizenListPanelHelper.DisplayCitizensMenu();
        }
        else citizenListPanelHelper.Hide();
    }

    private void OnDemolishHandler()
    {
        OnDemolishActionHandler?.Invoke();
        cancelConfirmActionPnl.SetActive(true);
        ToggleBuildPanel(false);
    }

    private void PrepareBuildMenu()
    {
        CreateButtonsInPanel(resourceGenStructsPnl.transform, structureRepository.GetResourceGenStructNames(), OnBuildSingleStructureCallback, StructureType.ResourceGenStructure);
        CreateButtonsInPanel(roadStructsPnl.transform, new List<string>() { structureRepository.GetRoadStructName() }, OnBuildRoadCallback, StructureType.RoadStructure);
        CreateButtonsInPanel(residentialStructsPnl.transform, new List<string>() { structureRepository.GetResidentialStructName() }, OnBuildResidentialCallback, StructureType.ResidentialStructure);
    }

    private void CreateButtonsInPanel(Transform panelTransform, List<string> dataToShow, Action<string> callback, StructureType structureType)
    {
        int diff = dataToShow.Count - panelTransform.childCount;
        for (int i = 0; i < diff; i++)
        {
            Instantiate(structureButtonPrefab, panelTransform);
        }

        for (int i = 0; i < panelTransform.childCount; i++)
        {
            var button = panelTransform.GetChild(i).GetComponent<Button>();
            if (button != null)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = dataToShow[i];
                button.onClick.AddListener(() => callback(button.GetComponentInChildren<TextMeshProUGUI>().text));
                panelTransform.GetChild(i).GetComponent<HoverTip>().tipToShow = structureRepository.GetStructureByName(dataToShow[i], structureType).buildCost.ToString();
            }
        }
    }

    private void OnCitizenAssignCallback()
    {
        OnCitizenAssignHandler?.Invoke();
        citizenPanelHelper.Hide();
        cancelConfirmActionPnl.gameObject.SetActive(true);
    }

    private void OnBuildSingleStructureCallback(string structureName)
    {
        ToggleCancelConfirmPanel(true);
        OnBuildSingleStructureHandler?.Invoke(structureName);
    }

    private void OnBuildRoadCallback(string structureName)
    {
        ToggleCancelConfirmPanel(true);
        OnBuildRoadHandler?.Invoke(structureName);
    }

    private void OnBuildResidentialCallback(string structureName)
    {
        ToggleCancelConfirmPanel(true);
        OnBuildResidentialHandler?.Invoke(structureName);
    }

    private void OnConfirmActionCallback()
    {
        cancelConfirmActionPnl.SetActive(false);
        OnConfirmActionHandler?.Invoke();
    }

    private void OnCancelActionCallback()
    {
        cancelConfirmActionPnl.SetActive(false);
        OnCancelActionHandler?.Invoke();
    }

    public void UpdateResourceValues(Cost cost, Cost previousCost, float happiness)
    {
        goldAmountTxt.text = Math.Round(cost.gold, 1) + " (" + Math.Round(cost.gold - previousCost.gold, 1) + ")";
        foodAmountTxt.text = Math.Round(cost.food, 1) + " (" + Math.Round(cost.food - previousCost.food, 1) + ")";
        woodAmountTxt.text = Math.Round(cost.wood, 1) + " (" + Math.Round(cost.wood - previousCost.wood, 1) + ")";
        stoneAmountTxt.text = Math.Round(cost.stone, 1) + " (" + Math.Round(cost.stone - previousCost.stone, 1) + ")";
        metalAmountTxt.text = Math.Round(cost.metal, 1) + " (" + Math.Round(cost.metal - previousCost.metal, 1) + ")"; ;
        happinessAmountTxt.text = Math.Round(happiness) + "%";
        citizenSpawnTimeTxt.text = Math.Round(PopulationManagement.Instance.GetTimeUntilNewCitizen()) + " sec";
    }

    public void UpdateDebugDisplay(string playerState)
    {
        debugPanelHelper.UpdateDisplay(playerState);
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

}
