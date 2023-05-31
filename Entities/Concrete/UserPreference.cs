using Core.Entities;

namespace Entities.Concrete;

public partial class UserPreference : IEntity
{
    public int UserId { get; set; }

    public string HistoricalPlaces { get; set; }

    public string Restaurants { get; set; }

    public string Cafes { get; set; }

    public string Beaches { get; set; }

    public string Parks { get; set; }
}
