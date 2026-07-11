using System;
using System.Collections.Generic;

namespace ContaVida.DataRefresh.DataAccess.DataAccess.ContaVidaProductionSource;

public partial class Relapse
{
    public Guid Id { get; set; }

    public int RelapseDay { get; set; }

    public int RelapseMonth { get; set; }

    public long RelapseYear { get; set; }

    public int? RelapseHour { get; set; }

    public int? RelapseMinute { get; set; }

    public Guid UserId { get; set; }

    public Guid PersonalProfileId { get; set; }

    public Guid EventCounterId { get; set; }

    public long PreviousYear { get; set; }

    public long PreviousMonth { get; set; }

    public long PreviousDay { get; set; }

    public long PreviousHour { get; set; }

    public long PreviousMinutes { get; set; }

    public DateTime CreationDate { get; set; }

    public string? RelapseMessage { get; set; }

    public int? RelapseReason { get; set; }

    public virtual EventCounter EventCounter { get; set; } = null!;

    public virtual PersonalProfile PersonalProfile { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
