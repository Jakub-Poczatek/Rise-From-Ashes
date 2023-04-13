using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverTipManager : MonoBehaviour
{
    public RectTransform tipParent;
    private TMP_Text tipTxt;

    public static Action<string, Vector2> OnMouseHover;
    public static Action OnMouseFocusLoss;

    private HoverTipManager() {}
    public static HoverTipManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        tipTxt = tipParent.GetComponentInChildren<TMP_Text>();
        HideTip();
    }

    private void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseFocusLoss += HideTip;
    }

    private void OnDisable()
    {
        OnMouseHover -= ShowTip;
        OnMouseFocusLoss -= HideTip;
    }

    private void ShowTip(string tip, Vector2 mousePos)
    {
        tipTxt.text = tip;
        tipParent.gameObject.SetActive(true);
        tipParent.transform.position = new Vector2(mousePos.x - tipTxt.preferredWidth / 2, mousePos.y + tipTxt.preferredHeight / 2);
    }

    public void HideTip()
    {
        tipTxt.text = default;
        tipParent.gameObject.SetActive(false);
    }
}
