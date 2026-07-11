using System;
using System.Collections.Generic;

namespace ContaVida.DataRefresh.DataAccess.DataAccess.ContaVidaMirrorTarget;

public partial class SystemMaintenance
{
    public Guid Id { get; set; }

    public bool IsOnMaintenance { get; set; }
}
