using System;
using System.Collections.Generic;

namespace ContaVida.DataRefresh.DataAccess.DataAccess.ContaVidaMirrorTarget;

public partial class PersonalProfile
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string LastName1 { get; set; } = null!;

    public string? LastName2 { get; set; }

    public Guid UserId { get; set; }

    public string? Pohone { get; set; }

    public string? Address { get; set; }

    public string? DefaultPetPhotos { get; set; }

    public DateTime CreationDate { get; set; }

    public int CounterLimit { get; set; }

    public int RelapseLimit { get; set; }

    public virtual ICollection<EventCounter> EventCounters { get; set; } = new List<EventCounter>();

    public virtual ICollection<Relapse> Relapses { get; set; } = new List<Relapse>();

    public virtual User User { get; set; } = null!;
}
