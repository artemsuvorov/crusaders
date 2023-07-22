public class QuarryController : ResourceBuildingController
{
    private readonly Health health = new(100.0f);

    public override Cost Cost => new(wood: 10);

    protected override Health Health => health;

    protected override float RepeatRate => 3.0f;
    protected override Resource Resource => Resource.Stone;
    protected override int Amount => 1;
}