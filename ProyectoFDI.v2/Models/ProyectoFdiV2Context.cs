using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFDI.v2.Models;

public partial class ProyectoFdiV2Context : DbContext
{
    public ProyectoFdiV2Context()
    {
    }

    public ProyectoFdiV2Context(DbContextOptions<ProyectoFdiV2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<Competencium> Competencia { get; set; }

    public virtual DbSet<DeportistaModalidad> DeportistaModalidads { get; set; }

    public virtual DbSet<Deportistum> Deportista { get; set; }

    public virtual DbSet<DetalleCompetencium> DetalleCompetencia { get; set; }

    public virtual DbSet<Entrenador> Entrenadors { get; set; }

    public virtual DbSet<Genero> Generos { get; set; }

    public virtual DbSet<Juez> Juezs { get; set; }

    public virtual DbSet<Modalidad> Modalidads { get; set; }

    public virtual DbSet<Provincium> Provincia { get; set; }

    public virtual DbSet<Sede> Sedes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\Servidor;Initial Catalog=ProyectoFDI.v2;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCat).HasName("PK__categori__D54686DE82B4A573");

            entity.ToTable("categoria");

            entity.Property(e => e.IdCat).HasColumnName("id_cat");
            entity.Property(e => e.NombreCat)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nombre_cat");
        });

        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.IdClub).HasName("PK__club__6FA6EEEFF31FBD65");

            entity.ToTable("club");

            entity.Property(e => e.IdClub).HasColumnName("id_club");
            entity.Property(e => e.NombreClub)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nombre_club");
        });

        modelBuilder.Entity<Competencium>(entity =>
        {
            entity.HasKey(e => e.IdCom).HasName("PK__competen__D6967171FBDC9F5F");

            entity.ToTable("competencia");

            entity.Property(e => e.IdCom).HasColumnName("id_com");
            entity.Property(e => e.FechaFinCom)
                .HasColumnType("date")
                .HasColumnName("fechaFin_com");
            entity.Property(e => e.FechaInicioCom)
                .HasColumnType("date")
                .HasColumnName("fechaInicio_com");
            entity.Property(e => e.IdCat).HasColumnName("id_cat");
            entity.Property(e => e.IdGen).HasColumnName("id_gen");
            entity.Property(e => e.IdJuez).HasColumnName("id_juez");
            entity.Property(e => e.IdMod).HasColumnName("id_mod");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.NombreCom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_com");

            entity.HasOne(d => d.IdCatNavigation).WithMany(p => p.Competencia)
                .HasForeignKey(d => d.IdCat)
                .HasConstraintName("FK__competenc__id_ca__45F365D3");

            entity.HasOne(d => d.IdGenNavigation).WithMany(p => p.Competencia)
                .HasForeignKey(d => d.IdGen)
                .HasConstraintName("FK__competenc__id_ge__440B1D61");

            entity.HasOne(d => d.IdJuezNavigation).WithMany(p => p.Competencia)
                .HasForeignKey(d => d.IdJuez)
                .HasConstraintName("FK__competenc__id_ju__44FF419A");

            entity.HasOne(d => d.IdModNavigation).WithMany(p => p.Competencia)
                .HasForeignKey(d => d.IdMod)
                .HasConstraintName("FK__competenc__id_mo__47DBAE45");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.Competencia)
                .HasForeignKey(d => d.IdSede)
                .HasConstraintName("FK__competenc__id_se__46E78A0C");
        });

        modelBuilder.Entity<DeportistaModalidad>(entity =>
        {
            entity.HasKey(e => e.IdDepmod).HasName("PK__deportis__1CF328D093563263");

            entity.ToTable("deportista_modalidad");

            entity.Property(e => e.IdDepmod).HasColumnName("id_depmod");
            entity.Property(e => e.IdDep).HasColumnName("id_dep");
            entity.Property(e => e.IdMod).HasColumnName("id_mod");

            entity.HasOne(d => d.IdDepNavigation).WithMany(p => p.DeportistaModalidads)
                .HasForeignKey(d => d.IdDep)
                .HasConstraintName("FK__deportist__id_de__5DCAEF64");

            entity.HasOne(d => d.IdModNavigation).WithMany(p => p.DeportistaModalidads)
                .HasForeignKey(d => d.IdMod)
                .HasConstraintName("FK__deportist__id_mo__5CD6CB2B");
        });

        modelBuilder.Entity<Deportistum>(entity =>
        {
            entity.HasKey(e => e.IdDep).HasName("PK__deportis__D5EA635C9B3D2523");

            entity.ToTable("deportista");

            entity.Property(e => e.IdDep).HasColumnName("id_dep");
            entity.Property(e => e.ApellidosDep)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidos_dep");
            entity.Property(e => e.CedulaDep)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("cedula_dep");
            entity.Property(e => e.IdCat).HasColumnName("id_cat");
            entity.Property(e => e.IdClub).HasColumnName("id_club");
            entity.Property(e => e.IdEnt).HasColumnName("id_ent");
            entity.Property(e => e.IdGen).HasColumnName("id_gen");
            entity.Property(e => e.IdPro).HasColumnName("id_pro");
            entity.Property(e => e.IdUsu).HasColumnName("id_usu");
            entity.Property(e => e.NombresDep)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombres_dep");

            entity.HasOne(d => d.IdCatNavigation).WithMany(p => p.Deportista)
                .HasForeignKey(d => d.IdCat)
                .HasConstraintName("FK__deportist__id_ca__3B75D760");

            entity.HasOne(d => d.IdClubNavigation).WithMany(p => p.Deportista)
                .HasForeignKey(d => d.IdClub)
                .HasConstraintName("FK__deportist__id_cl__3D5E1FD2");

            entity.HasOne(d => d.IdEntNavigation).WithMany(p => p.Deportista)
                .HasForeignKey(d => d.IdEnt)
                .HasConstraintName("FK__deportist__id_en__3E52440B");

            entity.HasOne(d => d.IdGenNavigation).WithMany(p => p.Deportista)
                .HasForeignKey(d => d.IdGen)
                .HasConstraintName("FK__deportist__id_ge__3C69FB99");

            entity.HasOne(d => d.IdProNavigation).WithMany(p => p.Deportista)
                .HasForeignKey(d => d.IdPro)
                .HasConstraintName("FK__deportist__id_pr__3A81B327");

            entity.HasOne(d => d.IdUsuNavigation).WithMany(p => p.Deportista)
                .HasForeignKey(d => d.IdUsu)
                .HasConstraintName("FK__deportist__id_us__398D8EEE");
        });

        modelBuilder.Entity<DetalleCompetencium>(entity =>
        {
            entity.HasKey(e => e.IdDetalle).HasName("PK__detalle___4F1332DE2B69EBEA");

            entity.ToTable("detalle_competencia");

            entity.Property(e => e.IdDetalle).HasColumnName("id_detalle");
            entity.Property(e => e.ClasRes)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("clas_res");
            entity.Property(e => e.FinalRes)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("final_res");
            entity.Property(e => e.IdCom).HasColumnName("id_com");
            entity.Property(e => e.IdDep).HasColumnName("id_dep");
            entity.Property(e => e.Puesto).HasColumnName("puesto");

            entity.HasOne(d => d.IdComNavigation).WithMany(p => p.DetalleCompetencia)
                .HasForeignKey(d => d.IdCom)
                .HasConstraintName("FK__detalle_c__id_co__619B8048");

            entity.HasOne(d => d.IdDepNavigation).WithMany(p => p.DetalleCompetencia)
                .HasForeignKey(d => d.IdDep)
                .HasConstraintName("FK__detalle_c__id_de__60A75C0F");
        });

        modelBuilder.Entity<Entrenador>(entity =>
        {
            entity.HasKey(e => e.IdEnt).HasName("PK__entrenad__D52ABC751B02354A");

            entity.ToTable("entrenador");

            entity.Property(e => e.IdEnt).HasColumnName("id_ent");
            entity.Property(e => e.ApellidosEnt)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidos_ent");
            entity.Property(e => e.CedulaEnt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("cedula_ent");
            entity.Property(e => e.IdPro).HasColumnName("id_pro");
            entity.Property(e => e.IdUsu).HasColumnName("id_usu");
            entity.Property(e => e.NombresEnt)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombres_ent");

            entity.HasOne(d => d.IdProNavigation).WithMany(p => p.Entrenadors)
                .HasForeignKey(d => d.IdPro)
                .HasConstraintName("FK__entrenado__id_pr__32E0915F");

            entity.HasOne(d => d.IdUsuNavigation).WithMany(p => p.Entrenadors)
                .HasForeignKey(d => d.IdUsu)
                .HasConstraintName("FK__entrenado__id_us__31EC6D26");
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.IdGen).HasName("PK__genero__D7967175217FA7CB");

            entity.ToTable("genero");

            entity.Property(e => e.IdGen).HasColumnName("id_gen");
            entity.Property(e => e.NombreGen)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nombre_gen");
        });

        modelBuilder.Entity<Juez>(entity =>
        {
            entity.HasKey(e => e.IdJuez).HasName("PK__juez__0FA80749D2956F4B");

            entity.ToTable("juez");

            entity.Property(e => e.IdJuez).HasColumnName("id_juez");
            entity.Property(e => e.ApellidosJuez)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidos_juez");
            entity.Property(e => e.CedulaJuez)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("cedula_juez");
            entity.Property(e => e.IdPro).HasColumnName("id_pro");
            entity.Property(e => e.IdUsu).HasColumnName("id_usu");
            entity.Property(e => e.NombresJuez)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombres_juez");
            entity.Property(e => e.PrincipalJuez).HasColumnName("principal_juez");

            entity.HasOne(d => d.IdProNavigation).WithMany(p => p.Juezs)
                .HasForeignKey(d => d.IdPro)
                .HasConstraintName("FK__juez__id_pro__36B12243");

            entity.HasOne(d => d.IdUsuNavigation).WithMany(p => p.Juezs)
                .HasForeignKey(d => d.IdUsu)
                .HasConstraintName("FK__juez__id_usu__35BCFE0A");
        });

        modelBuilder.Entity<Modalidad>(entity =>
        {
            entity.HasKey(e => e.IdMod).HasName("PK__modalida__6C8843AAE0B7C907");

            entity.ToTable("modalidad");

            entity.Property(e => e.IdMod).HasColumnName("id_mod");
            entity.Property(e => e.DescripcionMod)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("descripcion_mod");
        });

        modelBuilder.Entity<Provincium>(entity =>
        {
            entity.HasKey(e => e.IdPro).HasName("PK__provinci__6FC9A86C64F1CFB5");

            entity.ToTable("provincia");

            entity.Property(e => e.IdPro).HasColumnName("id_pro");
            entity.Property(e => e.NombrePro)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre_pro");
        });

        modelBuilder.Entity<Sede>(entity =>
        {
            entity.HasKey(e => e.IdSede).HasName("PK__sede__D693504B931EB44F");

            entity.ToTable("sede");

            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.NombreSede)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre_sede");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsu).HasName("PK__usuario__6AE80FBB57EF0A22");

            entity.ToTable("usuario");

            entity.Property(e => e.IdUsu).HasColumnName("id_usu");
            entity.Property(e => e.ClaveUsu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("clave_usu");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("date")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.NombreUsu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_usu");
            entity.Property(e => e.RolesUsu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("roles_usu");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
