using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    public string tipToShow = "default";
    private readonly float waitTime = 0.5f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        CancelInvoke();
        Invoke(nameof(ShowMessage), waitTime);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CancelInvoke();
        HoverTipManager.OnMouseFocusLoss();
    }

    private void ShowMessage()
    {
        HoverTipManager.OnMouseHover(tipToShow, Input.mousePosition);
    }
    
}
