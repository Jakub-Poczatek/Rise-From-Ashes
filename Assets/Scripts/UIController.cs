using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private Action OnBuildAreaHandler;
    private Action OnCancelActionHandler;
    private Action OnDemolishActionHandler;

    public Button buildResidentialAreaBtn;
    public Button cancelActionBtn;
    public Button openBuildMenuBtn;
    public Button demolishBtn;
    public Button closeBuildMenuBtn;
    public GameObject cancelActionPnl;
    public GameObject buildingMenuPnl;
    public GameObject resourceGenStructsPnl;
    public GameObject roadStructsPnl;
    public GameObject structureButtonPrefab;
    public TMP_Text goldAmountTxt;

    public StructureRepository structureRepository;
    //[SerializeField] Text woodAmountTxt;
    //[SerializeField] Text foodAmountTxt;

    // Start is called before the first frame update
    void Start()
    {
        cancelActionPnl.SetActive(false);
        buildingMenuPnl.SetActive(false);
        //buildResidentialAreaBtn.onClick.AddListener(OnBuildAreaCallback);
        cancelActionBtn.onClick.AddListener(OnCancelActionCallback);
        openBuildMenuBtn.onClick.AddListener(OnOpenBuildMenu);
        demolishBtn.onClick.AddListener(OnDemolishHandler);
        closeBuildMenuBtn.onClick.AddListener(OnCloseMenuHandler);
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
        CreateButtonsInPanel(resourceGenStructsPnl.transform, structureRepository.GetResourceGenStructNames());
        CreateButtonsInPanel(roadStructsPnl.transform, new List<string>() { structureRepository.GetRoadStructName() });
    }

    private void CreateButtonsInPanel(Transform panelTransform, List<string> dataToShow)
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
                button.onClick.AddListener(OnBuildAreaCallback);
            }
        }
        /*foreach (Transform t in panelTransform)
        {
            var button = t.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(OnBuildAreaCallback);
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBuildAreaCallback()
    {
        cancelActionPnl.SetActive(true);
        OnCloseMenuHandler();
        OnBuildAreaHandler?.Invoke();
    }

    private void OnCancelActionCallback()
    {
        cancelActionPnl.SetActive(false);
        OnCancelActionHandler?.Invoke();
    }

    public void AddListenerOnBuildAreaEvent(Action listener)
    {
        OnBuildAreaHandler += listener;
    }

    public void RemoveListenerOnBuildAreaEvent(Action listener)
    {
        OnBuildAreaHandler -= listener;
    }

    public void AddListenerOnCancelActionEvent(Action listener)
    {
        OnCancelActionHandler += listener;
    }

    public void RemoveListenerOnCancelActionEvent(Action listener)
    {
        OnCancelActionHandler -= listener;
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
