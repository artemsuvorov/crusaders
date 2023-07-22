public class SawmillController : ResourceBuildingController
{
    private readonly Health health = new(100.0f);

    public override Cost Cost => new(wood: 10);

    protected override Health Health => health;

    protected override Resource Resource => Resource.Wood;
    protected override int Amount => 1;
}