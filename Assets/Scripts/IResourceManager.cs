public interface IResourceManager
{
    int InitialGold { get; }
    float ResourceCalculationInterval { get; }

    void CalculateIncome();
    bool CanIBuyIt(int amount);
    void InceaseGold(int amount);
    bool SpendGold(int amount);
}