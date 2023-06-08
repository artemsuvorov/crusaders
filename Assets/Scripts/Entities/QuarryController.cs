public class QuarryController : ResourceBuildingController
{
    public override Cost Cost => new(wood: 10);

    protected override float RepeatRate => 3.0f;
    protected override Resource Resource => Resource.Stone;
    protected override int Amount => 1;
}