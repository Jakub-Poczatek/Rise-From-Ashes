using Codice.CM.Client.Differences;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicResourceHelper
{
    protected int resource;

    public BasicResourceHelper(int initialResource)
    {
        resource = initialResource;
    }

    public virtual int Resource
    {
        get => resource;
        private set
        {
            if (value < 0)
            {
                throw new ResourceException("Not enough resource");
            }
            else
                resource = value;
        }
    }

    public virtual void AdjustResource(int amount)
    {
        Resource += amount;
    }

    public virtual void CollectResource(float amount)
    {
        resource += Mathf.FloorToInt(amount);
    }
}
