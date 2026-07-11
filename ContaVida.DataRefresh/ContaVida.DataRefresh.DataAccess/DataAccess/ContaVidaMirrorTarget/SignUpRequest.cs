using System;
using System.Collections.Generic;

namespace ContaVida.DataRefresh.DataAccess.DataAccess.ContaVidaMirrorTarget;

public partial class SignUpRequest
{
    public Guid Id { get; set; }

    public string Ip { get; set; } = null!;

    public Guid? UserId { get; set; }

    public string RequestObject { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public virtual User? User { get; set; }
}
