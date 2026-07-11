using System;
using System.Collections.Generic;

namespace ContaVida.DataRefresh.DataAccess.DataAccess.ContaVidaMirrorTarget;

public partial class ResetLoginPassword
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public virtual User User { get; set; } = null!;
}
