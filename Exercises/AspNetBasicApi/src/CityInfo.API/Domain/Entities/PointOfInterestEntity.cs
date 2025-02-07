using System;
using CityInfo.API.Domain.Entities.Common;

namespace CityInfo.API.Domain.Entities;

public class PointOfInterestEntity : AuditableBaseEntity<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }

    public Guid CityId { get; set; }
    public CityEntity City { get; set; }
}