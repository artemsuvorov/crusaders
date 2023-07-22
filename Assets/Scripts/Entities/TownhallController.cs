public class TownhallController : UnitBuildingController
{
    private readonly Health health = new(200.0f);

    public override Cost Cost => new();

    protected override Health Health => health;
}