using System;
using System.Collections.Generic;

namespace GestionConferencias.Models;

public partial class Registro
{
    public int RegistroId { get; set; }

    public int ConferenciaId { get; set; }

    public int AsistenteId { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual Asistente? Asistente { get; set; }

    public virtual Conferencia? Conferencia { get; set; }
}
