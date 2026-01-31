using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestionConferencias.Models;

public partial class ConferenciasDbContext : IdentityDbContext<IdentityUser>
{


    public ConferenciasDbContext(DbContextOptions<ConferenciasDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asistente> Asistentes { get; set; }

    public virtual DbSet<Conferencia> Conferencias { get; set; }

    public virtual DbSet<Registro> Registros { get; set; }

}
