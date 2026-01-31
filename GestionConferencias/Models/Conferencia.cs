using System;
using System.Collections.Generic;

namespace GestionConferencias.Models;

public partial class Conferencia
{
    public int ConferenciaId { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public string Ubicacion { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Registro> Registros { get; set; } = new List<Registro>();
}
