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
    public GameObject cancelActionPnl;
    public GameObject buildingMenuPnl;
    public TMP_Text goldAmountTxt;
    //[SerializeField] Text woodAmountTxt;
    //[SerializeField] Text foodAmountTxt;

    // Start is called before the first frame update
    void Start()
    {
        cancelActionPnl.SetActive(false);
        buildingMenuPnl.SetActive(false);
        buildResidentialAreaBtn.onClick.AddListener(OnBuildAreaCallback);
        cancelActionBtn.onClick.AddListener(OnCancelActionCallback);
        openBuildMenuBtn.onClick.AddListener(OnOpenBuildMenu);
        demolishBtn.onClick.AddListener(OnDemolishHandler);
    }

    private void OnDemolishHandler()
    {
        OnDemolishActionHandler?.Invoke();
        cancelActionPnl.SetActive(true);
        buildingMenuPnl.SetActive(false);
    }

    private void OnOpenBuildMenu()
    {
        buildingMenuPnl.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBuildAreaCallback()
    {
        cancelActionPnl.SetActive(true);
        buildingMenuPnl.SetActive(false);
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
