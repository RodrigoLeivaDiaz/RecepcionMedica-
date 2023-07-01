using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecepcionMedica.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RecepcionMedica.Data
{
    public class MvcMedicoContext : IdentityDbContext<IdentityUser>
    {
        public MvcMedicoContext (DbContextOptions<MvcMedicoContext> options)
            : base(options)
        {
        }

        public DbSet<RecepcionMedica.Models.Medico> Medico { get; set; } = default!;

        public DbSet<RecepcionMedica.Models.Especialidad> Especialidad { get; set; } = default!;

        public DbSet<RecepcionMedica.Models.Paciente> Paciente { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        modelBuilder.Entity<Especialidad>()
        .HasMany(f => f.Medicos)
        .WithOne(f => f.Especialidad )
        .HasForeignKey(f => f.EspecialidadId);

        modelBuilder.Entity<Medico>()
        .HasMany(f => f.Pacientes)
        .WithOne(f => f.Medico)
        .HasForeignKey(f => f.MedicoId);

        base.OnModelCreating(modelBuilder);
        }          
    }
}
