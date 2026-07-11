using System;
using System.Collections.Generic;

namespace ContaVida.DataRefresh.DataAccess.DataAccess.ContaVidaProductionSource;

public partial class User
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool AllowSysAdminAccess { get; set; }

    public DateTime CreationDate { get; set; }

    public bool IsSystemAdmin { get; set; }

    public virtual ICollection<CorrectLogin> CorrectLogins { get; set; } = new List<CorrectLogin>();

    public virtual ICollection<EventCounter> EventCounters { get; set; } = new List<EventCounter>();

    public virtual ICollection<PersonalProfile> PersonalProfiles { get; set; } = new List<PersonalProfile>();

    public virtual ICollection<Relapse> Relapses { get; set; } = new List<Relapse>();

    public virtual ICollection<ResetLoginPassword> ResetLoginPasswords { get; set; } = new List<ResetLoginPassword>();

    public virtual ICollection<SignUpRequest> SignUpRequests { get; set; } = new List<SignUpRequest>();
}
