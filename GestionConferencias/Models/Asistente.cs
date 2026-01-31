using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionConferencias.Models;

public partial class Asistente
{
    public int AsistenteId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
    [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "Solo letras")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
    [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "Solo letras")]
    public string Apellido { get; set; } = null!;

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Debe tener exactamente 10 dígitos")]
    public string? Telefono { get; set; }

    public virtual ICollection<Registro> Registros { get; set; } = new List<Registro>();
}
