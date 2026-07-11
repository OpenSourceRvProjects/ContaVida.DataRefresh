using System;
using System.Collections.Generic;

namespace ContaVida.DataRefresh.DataAccess.DataAccess.ContaVidaProductionSource;

public partial class SystemMaintenance
{
    public Guid Id { get; set; }

    public bool IsOnMaintenance { get; set; }
}
