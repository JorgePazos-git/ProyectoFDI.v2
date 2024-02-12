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

    public virtual DbSet<DetalleCompetenciaDificultad> DetalleCompetenciaDificultads { get; set; }

    public virtual DbSet<DetalleCompetencium> DetalleCompetencia { get; set; }

    public virtual DbSet<Entrenador> Entrenadors { get; set; }

    public virtual DbSet<Genero> Generos { get; set; }

    public virtual DbSet<Juez> Juezs { get; set; }

    public virtual DbSet<Modalidad> Modalidads { get; set; }

    public virtual DbSet<Provincium> Provincia { get; set; }

    public virtual DbSet<PuntajeBloque> PuntajeBloques { get; set; }

    public virtual DbSet<ResultadoBloque> ResultadoBloques { get; set; }

    public virtual DbSet<Sede> Sedes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=proyectofdi.database.windows.net;Initial Catalog=ProyectoFDI.v2;User ID=proyectofdi;Password=Allistar123.;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCat).HasName("PK__categori__D54686DE494DEF25");

            entity.ToTable("categoria");

            entity.Property(e => e.IdCat).HasColumnName("id_cat");
            entity.Property(e => e.NombreCat)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nombre_cat");
        });

        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.IdClub).HasName("PK__club__6FA6EEEFF7500A2D");

            entity.ToTable("club");

            entity.Property(e => e.IdClub).HasColumnName("id_club");
            entity.Property(e => e.NombreClub)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nombre_club");
        });

        modelBuilder.Entity<Competencium>(entity =>
        {
            entity.HasKey(e => e.IdCom).HasName("PK__competen__D6967171FCA22FB8");

            entity.ToTable("competencia");

            entity.Property(e => e.IdCom).HasColumnName("id_com");
            entity.Property(e => e.ActivoCom).HasColumnName("activo_com");
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
                .HasConstraintName("FK__competenc__id_ca__7F2BE32F");

            entity.HasOne(d => d.IdGenNavigation).WithMany(p => p.Competencia)
                .HasForeignKey(d => d.IdGen)
                .HasConstraintName("FK__competenc__id_ge__7D439ABD");

            entity.HasOne(d => d.IdJuezNavigation).WithMany(p => p.Competencia)
                .HasForeignKey(d => d.IdJuez)
                .HasConstraintName("FK__competenc__id_ju__7E37BEF6");

            entity.HasOne(d => d.IdModNavigation).WithMany(p => p.Competencia)
                .HasForeignKey(d => d.IdMod)
                .HasConstraintName("FK__competenc__id_mo__01142BA1");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.Competencia)
                .HasForeignKey(d => d.IdSede)
                .HasConstraintName("FK__competenc__id_se__00200768");
        });

        modelBuilder.Entity<DeportistaModalidad>(entity =>
        {
            entity.HasKey(e => e.IdDepmod).HasName("PK__deportis__1CF328D067FE08EE");

            entity.ToTable("deportista_modalidad");

            entity.Property(e => e.IdDepmod).HasColumnName("id_depmod");
            entity.Property(e => e.IdDep).HasColumnName("id_dep");
            entity.Property(e => e.IdMod).HasColumnName("id_mod");

            entity.HasOne(d => d.IdDepNavigation).WithMany(p => p.DeportistaModalidads)
                .HasForeignKey(d => d.IdDep)
                .HasConstraintName("FK__deportist__id_de__7A672E12");

            entity.HasOne(d => d.IdModNavigation).WithMany(p => p.DeportistaModalidads)
                .HasForeignKey(d => d.IdMod)
                .HasConstraintName("FK__deportist__id_mo__797309D9");
        });

        modelBuilder.Entity<Deportistum>(entity =>
        {
            entity.HasKey(e => e.IdDep).HasName("PK__deportis__D5EA635C6A1D0BA3");

            entity.ToTable("deportista");

            entity.Property(e => e.IdDep).HasColumnName("id_dep");
            entity.Property(e => e.ActivoDep).HasColumnName("activo_dep");
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
                .HasConstraintName("FK__deportist__id_ca__73BA3083");

            entity.HasOne(d => d.IdClubNavigation).WithMany(p => p.Deportista)
                .HasForeignKey(d => d.IdClub)
                .HasConstraintName("FK__deportist__id_cl__75A278F5");

            entity.HasOne(d => d.IdEntNavigation).WithMany(p => p.Deportista)
                .HasForeignKey(d => d.IdEnt)
                .HasConstraintName("FK__deportist__id_en__76969D2E");

            entity.HasOne(d => d.IdGenNavigation).WithMany(p => p.Deportista)
                .HasForeignKey(d => d.IdGen)
                .HasConstraintName("FK__deportist__id_ge__74AE54BC");

            entity.HasOne(d => d.IdProNavigation).WithMany(p => p.Deportista)
                .HasForeignKey(d => d.IdPro)
                .HasConstraintName("FK__deportist__id_pr__72C60C4A");

            entity.HasOne(d => d.IdUsuNavigation).WithMany(p => p.Deportista)
                .HasForeignKey(d => d.IdUsu)
                .HasConstraintName("FK__deportist__id_us__71D1E811");
        });

        modelBuilder.Entity<DetalleCompetenciaDificultad>(entity =>
        {
            entity.HasKey(e => e.IdDetalleDificultad).HasName("PK__detalle___433E45E6AB3BBF49");

            entity.ToTable("detalle_competencia_dificultad");

            entity.Property(e => e.IdDetalleDificultad).HasColumnName("id_detalle_dificultad");
            entity.Property(e => e.Clas1Res)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("clas1_res");
            entity.Property(e => e.Clas2Res)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("clas2_res");
            entity.Property(e => e.FinalRes)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("final_res");
            entity.Property(e => e.IdCom).HasColumnName("id_com");
            entity.Property(e => e.IdDep).HasColumnName("id_dep");
            entity.Property(e => e.Puesto).HasColumnName("puesto");
            entity.Property(e => e.PuestoInicialRes).HasColumnName("puesto_inicial_res");

            entity.HasOne(d => d.IdComNavigation).WithMany(p => p.DetalleCompetenciaDificultads)
                .HasForeignKey(d => d.IdCom)
                .HasConstraintName("FK__detalle_c__id_co__634EBE90");

            entity.HasOne(d => d.IdDepNavigation).WithMany(p => p.DetalleCompetenciaDificultads)
                .HasForeignKey(d => d.IdDep)
                .HasConstraintName("FK__detalle_c__id_de__625A9A57");
        });

        modelBuilder.Entity<DetalleCompetencium>(entity =>
        {
            entity.HasKey(e => e.IdDetalle).HasName("PK__detalle___4F1332DEBB81259D");

            entity.ToTable("detalle_competencia");

            entity.Property(e => e.IdDetalle).HasColumnName("id_detalle");
            entity.Property(e => e.ClasRes)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("clas_res");
            entity.Property(e => e.CuartosRes)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("cuartos_res");
            entity.Property(e => e.FinalRes)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("final_res");
            entity.Property(e => e.IdCom).HasColumnName("id_com");
            entity.Property(e => e.IdDep).HasColumnName("id_dep");
            entity.Property(e => e.OctavosRes)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("octavos_res");
            entity.Property(e => e.Puesto).HasColumnName("puesto");
            entity.Property(e => e.SemiRes)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("semi_res");

            entity.HasOne(d => d.IdComNavigation).WithMany(p => p.DetalleCompetencia)
                .HasForeignKey(d => d.IdCom)
                .HasConstraintName("FK__detalle_c__id_co__04E4BC85");

            entity.HasOne(d => d.IdDepNavigation).WithMany(p => p.DetalleCompetencia)
                .HasForeignKey(d => d.IdDep)
                .HasConstraintName("FK__detalle_c__id_de__03F0984C");
        });

        modelBuilder.Entity<Entrenador>(entity =>
        {
            entity.HasKey(e => e.IdEnt).HasName("PK__entrenad__D52ABC75C7982C56");

            entity.ToTable("entrenador");

            entity.Property(e => e.IdEnt).HasColumnName("id_ent");
            entity.Property(e => e.ActivoEnt).HasColumnName("activo_ent");
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
                .HasConstraintName("FK__entrenado__id_pr__6B24EA82");

            entity.HasOne(d => d.IdUsuNavigation).WithMany(p => p.Entrenadors)
                .HasForeignKey(d => d.IdUsu)
                .HasConstraintName("FK__entrenado__id_us__6A30C649");
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.IdGen).HasName("PK__genero__D79671759B1036DC");

            entity.ToTable("genero");

            entity.Property(e => e.IdGen).HasColumnName("id_gen");
            entity.Property(e => e.NombreGen)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nombre_gen");
        });

        modelBuilder.Entity<Juez>(entity =>
        {
            entity.HasKey(e => e.IdJuez).HasName("PK__juez__0FA80749CA28D854");

            entity.ToTable("juez");

            entity.Property(e => e.IdJuez).HasColumnName("id_juez");
            entity.Property(e => e.ActivoJuez).HasColumnName("activo_juez");
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
                .HasConstraintName("FK__juez__id_pro__6EF57B66");

            entity.HasOne(d => d.IdUsuNavigation).WithMany(p => p.Juezs)
                .HasForeignKey(d => d.IdUsu)
                .HasConstraintName("FK__juez__id_usu__6E01572D");
        });

        modelBuilder.Entity<Modalidad>(entity =>
        {
            entity.HasKey(e => e.IdMod).HasName("PK__modalida__6C8843AA80073099");

            entity.ToTable("modalidad");

            entity.Property(e => e.IdMod).HasColumnName("id_mod");
            entity.Property(e => e.DescripcionMod)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("descripcion_mod");
        });

        modelBuilder.Entity<Provincium>(entity =>
        {
            entity.HasKey(e => e.IdPro).HasName("PK__provinci__6FC9A86C1889B546");

            entity.ToTable("provincia");

            entity.Property(e => e.IdPro).HasColumnName("id_pro");
            entity.Property(e => e.NombrePro)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre_pro");
        });

        modelBuilder.Entity<PuntajeBloque>(entity =>
        {
            entity.HasKey(e => e.IdBloPts).HasName("PK__puntaje___B78A6B496770F08D");

            entity.ToTable("puntaje_bloque");

            entity.Property(e => e.IdBloPts).HasColumnName("id_blo_pts");
            entity.Property(e => e.Etapa)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("etapa");
            entity.Property(e => e.IdCom).HasColumnName("id_com");
            entity.Property(e => e.IdDep).HasColumnName("id_dep");
            entity.Property(e => e.IntentosTops).HasColumnName("intentos_tops");
            entity.Property(e => e.IntentosZonas).HasColumnName("intentos_zonas");
            entity.Property(e => e.NumeroBloque).HasColumnName("numero_bloque");

            entity.HasOne(d => d.IdComNavigation).WithMany(p => p.PuntajeBloques)
                .HasForeignKey(d => d.IdCom)
                .HasConstraintName("FK__puntaje_b__id_co__1B9317B3");

            entity.HasOne(d => d.IdDepNavigation).WithMany(p => p.PuntajeBloques)
                .HasForeignKey(d => d.IdDep)
                .HasConstraintName("FK__puntaje_b__id_de__1C873BEC");
        });

        modelBuilder.Entity<ResultadoBloque>(entity =>
        {
            entity.HasKey(e => e.IdResBloque).HasName("PK__resultad__35024A27D835018F");

            entity.ToTable("resultado_bloque");

            entity.Property(e => e.IdResBloque).HasColumnName("id_res_bloque");
            entity.Property(e => e.Etapa)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("etapa");
            entity.Property(e => e.IdCom).HasColumnName("id_com");
            entity.Property(e => e.IdDep).HasColumnName("id_dep");
            entity.Property(e => e.Puesto).HasColumnName("puesto");

            entity.HasOne(d => d.IdComNavigation).WithMany(p => p.ResultadoBloques)
                .HasForeignKey(d => d.IdCom)
                .HasConstraintName("FK__resultado__id_co__1F63A897");

            entity.HasOne(d => d.IdDepNavigation).WithMany(p => p.ResultadoBloques)
                .HasForeignKey(d => d.IdDep)
                .HasConstraintName("FK__resultado__id_de__2057CCD0");
        });

        modelBuilder.Entity<Sede>(entity =>
        {
            entity.HasKey(e => e.IdSede).HasName("PK__sede__D693504B17045EBE");

            entity.ToTable("sede");

            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.NombreSede)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre_sede");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsu).HasName("PK__usuario__6AE80FBB4C621344");

            entity.ToTable("usuario");

            entity.Property(e => e.IdUsu).HasColumnName("id_usu");
            entity.Property(e => e.ActivoUsu).HasColumnName("activo_usu");
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
