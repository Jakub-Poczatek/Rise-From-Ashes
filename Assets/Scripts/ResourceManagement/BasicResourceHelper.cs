public class BasicResourceHelper
{
    protected float resource;

    public BasicResourceHelper(float initialResource)
    {
        resource = initialResource;
    }

    public virtual float Resource
    {
        get => resource;
        private set
        {
            if (value < 0)
            {
                resource = 0;
            }
            else
                resource = value;
        }
    }

    public virtual void AdjustResource(float amount)
    {
        Resource += amount;
    }
}
