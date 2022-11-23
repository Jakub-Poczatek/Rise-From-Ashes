using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private float timer;
    private float oneSecond = 1;
    private float goldAmount = 100;
    private float goldAmountChange = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > oneSecond)
        {
            timer -= oneSecond;
            goldAmount += goldAmountChange;
        }
    }

    public float GoldAmount { get => goldAmount; set => goldAmount = value; }
    public float GoldAmountChange { get => goldAmountChange; set => goldAmountChange = value; }
}
