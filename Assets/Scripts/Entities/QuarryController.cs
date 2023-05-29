public class QuarryController : ResourceBuildingController
{
    public override Cost Cost => new(wood: 10);

    protected override Resource Resource => Resource.Stone;
    protected override int Amount => 8;
}