public interface IResourceManager
{
    void CalculateResources();
    bool CanIAffordIt(Cost cost);
    void EarnResources(Cost cost);
    bool Purchase(Cost cost);
    void PrepareResourceManager(BuildingManager buildingManager);
}