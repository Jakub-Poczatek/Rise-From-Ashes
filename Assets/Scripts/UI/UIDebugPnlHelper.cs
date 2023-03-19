using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDebugPnlHelper : MonoBehaviour
{
    public TMP_Text playerState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDisplay(string playerState)
    {
        this.playerState.text = "PlayerState: " + playerState;
    }
}
