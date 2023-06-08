public class SawmillController : ResourceBuildingController
{
    public override Cost Cost => new(wood: 10);

    protected override Resource Resource => Resource.Wood;
    protected override int Amount => 1;
}