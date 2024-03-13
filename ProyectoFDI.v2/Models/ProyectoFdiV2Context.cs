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

    public virtual DbSet<VistaCompetencium> VistaCompetencia { get; set; }

    public virtual DbSet<VistaPuntajesDeportista> VistaPuntajesDeportistas { get; set; }

    public virtual DbSet<VistaViasResultado> VistaViasResultados { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //=> optionsBuilder.UseSqlServer("Data Source=(localdb)\\proyectofdi.database.windows.net;Initial Catalog=ProyectoFDI.v2;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        => optionsBuilder.UseSqlServer("Data Source=MSI;Initial Catalog=ProyectoFDI.v2;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.IdCat).HasName("PK__categori__D54686DE2D646EAA");

            entity.ToTable("categoria");

            entity.Property(e => e.IdCat).HasColumnName("id_cat");
            entity.Property(e => e.NombreCat)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nombre_cat");
        });

        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.IdClub).HasName("PK__club__6FA6EEEF9A75DC5D");

            entity.ToTable("club");

            entity.Property(e => e.IdClub).HasColumnName("id_club");
            entity.Property(e => e.NombreClub)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nombre_club");
        });

        modelBuilder.Entity<Competencium>(entity =>
        {
            entity.HasKey(e => e.IdCom).HasName("PK__competen__D69671710CF1F0A7");

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
                .HasConstraintName("FK__competenc__id_ca__59063A47");

            entity.HasOne(d => d.IdGenNavigation).WithMany(p => p.Competencia)
                .HasForeignKey(d => d.IdGen)
                .HasConstraintName("FK__competenc__id_ge__571DF1D5");

            entity.HasOne(d => d.IdJuezNavigation).WithMany(p => p.Competencia)
                .HasForeignKey(d => d.IdJuez)
                .HasConstraintName("FK__competenc__id_ju__5812160E");

            entity.HasOne(d => d.IdModNavigation).WithMany(p => p.Competencia)
                .HasForeignKey(d => d.IdMod)
                .HasConstraintName("FK__competenc__id_mo__5629CD9C");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.Competencia)
                .HasForeignKey(d => d.IdSede)
                .HasConstraintName("FK__competenc__id_se__5535A963");
        });

        modelBuilder.Entity<DeportistaModalidad>(entity =>
        {
            entity.HasKey(e => e.IdDepmod).HasName("PK__deportis__1CF328D0586FD878");

            entity.ToTable("deportista_modalidad");

            entity.Property(e => e.IdDepmod).HasColumnName("id_depmod");
            entity.Property(e => e.IdDep).HasColumnName("id_dep");
            entity.Property(e => e.IdMod).HasColumnName("id_mod");

            entity.HasOne(d => d.IdDepNavigation).WithMany(p => p.DeportistaModalidads)
                .HasForeignKey(d => d.IdDep)
                .HasConstraintName("FK__deportist__id_de__60A75C0F");

            entity.HasOne(d => d.IdModNavigation).WithMany(p => p.DeportistaModalidads)
                .HasForeignKey(d => d.IdMod)
                .HasConstraintName("FK__deportist__id_mo__5FB337D6");
        });

        modelBuilder.Entity<Deportistum>(entity =>
        {
            entity.HasKey(e => e.IdDep).HasName("PK__deportis__D5EA635C57F7FE9A");

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
                .HasConstraintName("FK__deportist__id_ca__5BE2A6F2");

            entity.HasOne(d => d.IdClubNavigation).WithMany(p => p.Deportista)
                .HasForeignKey(d => d.IdClub)
                .HasConstraintName("FK__deportist__id_cl__5DCAEF64");

            entity.HasOne(d => d.IdEntNavigation).WithMany(p => p.Deportista)
                .HasForeignKey(d => d.IdEnt)
                .HasConstraintName("FK__deportist__id_en__5EBF139D");

            entity.HasOne(d => d.IdGenNavigation).WithMany(p => p.Deportista)
                .HasForeignKey(d => d.IdGen)
                .HasConstraintName("FK__deportist__id_ge__5CD6CB2B");

            entity.HasOne(d => d.IdProNavigation).WithMany(p => p.Deportista)
                .HasForeignKey(d => d.IdPro)
                .HasConstraintName("FK__deportist__id_pr__5AEE82B9");

            entity.HasOne(d => d.IdUsuNavigation).WithMany(p => p.Deportista)
                .HasForeignKey(d => d.IdUsu)
                .HasConstraintName("FK__deportist__id_us__59FA5E80");
        });

        modelBuilder.Entity<DetalleCompetenciaDificultad>(entity =>
        {
            entity.HasKey(e => e.IdDetalleDificultad).HasName("PK__detalle___433E45E6B6F03480");

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
                .HasConstraintName("FK__detalle_c__id_co__6477ECF3");

            entity.HasOne(d => d.IdDepNavigation).WithMany(p => p.DetalleCompetenciaDificultads)
                .HasForeignKey(d => d.IdDep)
                .HasConstraintName("FK__detalle_c__id_de__6383C8BA");
        });

        modelBuilder.Entity<DetalleCompetencium>(entity =>
        {
            entity.HasKey(e => e.IdDetalle).HasName("PK__detalle___4F1332DE13B51728");

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
                .HasConstraintName("FK__detalle_c__id_co__628FA481");

            entity.HasOne(d => d.IdDepNavigation).WithMany(p => p.DetalleCompetencia)
                .HasForeignKey(d => d.IdDep)
                .HasConstraintName("FK__detalle_c__id_de__619B8048");
        });

        modelBuilder.Entity<Entrenador>(entity =>
        {
            entity.HasKey(e => e.IdEnt).HasName("PK__entrenad__D52ABC752AD7145A");

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
                .HasConstraintName("FK__entrenado__id_pr__66603565");

            entity.HasOne(d => d.IdUsuNavigation).WithMany(p => p.Entrenadors)
                .HasForeignKey(d => d.IdUsu)
                .HasConstraintName("FK__entrenado__id_us__656C112C");
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.IdGen).HasName("PK__genero__D7967175BBCBCC80");

            entity.ToTable("genero");

            entity.Property(e => e.IdGen).HasColumnName("id_gen");
            entity.Property(e => e.NombreGen)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nombre_gen");
        });

        modelBuilder.Entity<Juez>(entity =>
        {
            entity.HasKey(e => e.IdJuez).HasName("PK__juez__0FA807492A02A374");

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
                .HasConstraintName("FK__juez__id_pro__68487DD7");

            entity.HasOne(d => d.IdUsuNavigation).WithMany(p => p.Juezs)
                .HasForeignKey(d => d.IdUsu)
                .HasConstraintName("FK__juez__id_usu__6754599E");
        });

        modelBuilder.Entity<Modalidad>(entity =>
        {
            entity.HasKey(e => e.IdMod).HasName("PK__modalida__6C8843AADF0162C3");

            entity.ToTable("modalidad");

            entity.Property(e => e.IdMod).HasColumnName("id_mod");
            entity.Property(e => e.DescripcionMod)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("descripcion_mod");
        });

        modelBuilder.Entity<Provincium>(entity =>
        {
            entity.HasKey(e => e.IdPro).HasName("PK__provinci__6FC9A86C6EC6A4C9");

            entity.ToTable("provincia");

            entity.Property(e => e.IdPro).HasColumnName("id_pro");
            entity.Property(e => e.NombrePro)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre_pro");
        });

        modelBuilder.Entity<PuntajeBloque>(entity =>
        {
            entity.HasKey(e => e.IdBloPts).HasName("PK__puntaje___B78A6B49B28E2609");

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
                .HasConstraintName("FK__puntaje_b__id_co__693CA210");

            entity.HasOne(d => d.IdDepNavigation).WithMany(p => p.PuntajeBloques)
                .HasForeignKey(d => d.IdDep)
                .HasConstraintName("FK__puntaje_b__id_de__6A30C649");
        });

        modelBuilder.Entity<ResultadoBloque>(entity =>
        {
            entity.HasKey(e => e.IdResBloque).HasName("PK__resultad__35024A277A36721C");

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
                .HasConstraintName("FK__resultado__id_co__6B24EA82");

            entity.HasOne(d => d.IdDepNavigation).WithMany(p => p.ResultadoBloques)
                .HasForeignKey(d => d.IdDep)
                .HasConstraintName("FK__resultado__id_de__6C190EBB");
        });

        modelBuilder.Entity<Sede>(entity =>
        {
            entity.HasKey(e => e.IdSede).HasName("PK__sede__D693504BF95DFB59");

            entity.ToTable("sede");

            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.NombreSede)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre_sede");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsu).HasName("PK__usuario__6AE80FBB3F734BB7");

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

        modelBuilder.Entity<VistaCompetencium>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VistaCompetencia");

            entity.Property(e => e.ActivoCom).HasColumnName("activo_com");
            entity.Property(e => e.DescripcionModalidad)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("descripcion_modalidad");
            entity.Property(e => e.FechaFinCom)
                .HasColumnType("date")
                .HasColumnName("fechaFin_com");
            entity.Property(e => e.FechaInicioCom)
                .HasColumnType("date")
                .HasColumnName("fechaInicio_com");
            entity.Property(e => e.Genero)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("genero");
            entity.Property(e => e.IdCom).HasColumnName("id_com");
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nombre_categoria");
            entity.Property(e => e.NombreCom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_com");
            entity.Property(e => e.NombreDeSede)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre_de_sede");
            entity.Property(e => e.NombreDelJuez)
                .HasMaxLength(101)
                .IsUnicode(false)
                .HasColumnName("nombre_del_juez");
        });

        modelBuilder.Entity<VistaPuntajesDeportista>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vista_puntajes_deportistas");

            entity.Property(e => e.Etapa)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("etapa");
            entity.Property(e => e.IdCom).HasColumnName("id_com");
            entity.Property(e => e.IdDep).HasColumnName("id_dep");
            entity.Property(e => e.IdVw).HasColumnName("id_vw");
            entity.Property(e => e.IntentosTops).HasColumnName("intentos_tops");
            entity.Property(e => e.IntentosZonas).HasColumnName("intentos_zonas");
            entity.Property(e => e.NombreDep)
                .IsRequired()
                .HasMaxLength(101)
                .IsUnicode(false)
                .HasColumnName("nombre_dep");
            entity.Property(e => e.TopsRealizados).HasColumnName("tops_realizados");
            entity.Property(e => e.ZonasRealizadas).HasColumnName("zonas_realizados");
        });

        modelBuilder.Entity<VistaViasResultado>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vista_vias_resultado");

            entity.Property(e => e.Clasificacion1)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("clasificacion_1");
            entity.Property(e => e.Clasificacion2)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("clasificacion_2");
            entity.Property(e => e.Deportista)
                .IsRequired()
                .HasMaxLength(101)
                .IsUnicode(false)
                .HasColumnName("deportista");
            entity.Property(e => e.Final)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("final");
            entity.Property(e => e.IdCompe).HasColumnName("id_compe");
            entity.Property(e => e.PuestoClasificacion).HasColumnName("puesto_clasificacion");
            entity.Property(e => e.PuestoFinal).HasColumnName("puesto_final");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
