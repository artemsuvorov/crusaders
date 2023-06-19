using System.Collections.Generic;

public enum FactionName
{
    None,
    English,
    French,
    German,
    Muslim
}

public class Faction
{
    private readonly HashSet<EntityController> entities = new();

    public FactionName Name { get; private set; }

    public bool HasTownhall => Townhall is not null;
    public TownhallController Townhall { get; private set; }

    public Faction(FactionName name)
    {
        Name = name;
    }

    public void AddAlly(EntityController entity)
    {
        entities.Add(entity);
        entity.Died += e => entities.Remove(entity);
        entity.FactionName = Name;

        var townhall = entity.GetComponent<TownhallController>();
        if (townhall is not null)
            Townhall = townhall;
    }

    public bool ConstainsAlly(EntityController entity)
    {
        return entities.Contains(entity);
    }
}
