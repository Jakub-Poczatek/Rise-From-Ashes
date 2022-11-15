using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private Action OnBuildAreaHandler;
    private Action OnCancelActionHandler;

    public Button buildResidentialAreaBtn;
    public Button cancelActionBtn;
    public GameObject cancelActionPnl;

    // Start is called before the first frame update
    void Start()
    {
        cancelActionPnl.SetActive(false);
        buildResidentialAreaBtn.onClick.AddListener(OnBuildAreaCallback);
        cancelActionBtn.onClick.AddListener(OnCancelActionCallback);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBuildAreaCallback()
    {
        cancelActionPnl.SetActive(true);
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
}
