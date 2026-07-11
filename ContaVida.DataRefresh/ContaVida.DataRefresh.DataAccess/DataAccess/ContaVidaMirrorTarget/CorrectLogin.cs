using System;
using System.Collections.Generic;

namespace ContaVida.DataRefresh.DataAccess.DataAccess.ContaVidaMirrorTarget;

public partial class CorrectLogin
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string IpAddress { get; set; } = null!;

    public DateTime LoginDate { get; set; }

    public virtual User User { get; set; } = null!;
}
