using System.Collections.Generic;

public enum FactionName
{
    English,
    French,
    German,
    Muslim
}

public class Faction
{
    private readonly HashSet<EntityController> entities = new();

    public FactionName Name { get; private set; }

    public Faction(FactionName name)
    {
        Name = name;
    }

    public void AddAlly(EntityController entity)
    {
        entities.Add(entity);
        entity.Died += () => entities.Remove(entity);
    }

    public bool ConstainsAlly(EntityController entity)
    {
        return entities.Contains(entity);
    }
}
