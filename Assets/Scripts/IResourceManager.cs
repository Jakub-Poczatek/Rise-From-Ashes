public interface IResourceManager
{
    int InitialGold { get; set; }
    float ResourceCalculationInterval { get; set; }

    void CalculateIncome();
    bool CanIBuyIt(int amount);
    void InceaseGold(int amount);
    bool SpendGold(int amount);
}