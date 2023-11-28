﻿// <auto-generated />
using System;
using JobOverview_v94.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JobOverview_v94.Data.Migrations
{
    [DbContext(typeof(ContexteJobOverview))]
    [Migration("20231117171655_AjoutTaches")]
    partial class AjoutTaches
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JobOverview_v94.Entities.Activite", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Titre")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Code");

                    b.ToTable("Activites");
                });

            modelBuilder.Entity("JobOverview_v94.Entities.ActiviteMetier", b =>
                {
                    b.Property<string>("CodeActivite")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CodeMetier")
                        .HasColumnType("varchar(20)");

                    b.HasKey("CodeActivite", "CodeMetier");

                    b.HasIndex("CodeMetier");

                    b.ToTable("ActivitesMetiers");
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Equipe", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CodeFiliere")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CodeService")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(60)
                        .IsUnicode(false)
                        .HasColumnType("varchar(60)");

                    b.HasKey("Code");

                    b.HasIndex("CodeFiliere");

                    b.HasIndex("CodeService");

                    b.ToTable("Equipes");
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Filiere", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Code");

                    b.ToTable("Filieres");

                    b.HasData(
                        new
                        {
                            Code = "BIOV",
                            Nom = "Biologie végétale"
                        },
                        new
                        {
                            Code = "BIOH",
                            Nom = "Biologie humaine"
                        },
                        new
                        {
                            Code = "BIOA",
                            Nom = "Support animale"
                        });
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Logiciel", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CodeFiliere")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(60)
                        .IsUnicode(false)
                        .HasColumnType("varchar(60)");

                    b.HasKey("Code");

                    b.HasIndex("CodeFiliere");

                    b.ToTable("Logiciels");

                    b.HasData(
                        new
                        {
                            Code = "GENOMICA",
                            CodeFiliere = "BIOH",
                            Nom = "Genomica"
                        },
                        new
                        {
                            Code = "ANATOMIA",
                            CodeFiliere = "BIOH",
                            Nom = "Anatomia"
                        });
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Metier", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CodeService")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Titre")
                        .IsRequired()
                        .HasMaxLength(60)
                        .IsUnicode(false)
                        .HasColumnType("varchar(60)");

                    b.HasKey("Code");

                    b.HasIndex("CodeService");

                    b.ToTable("Metiers", (string)null);
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Module", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CodeLogiciel")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CodeLogicielParent")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CodeModuleParent")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(60)
                        .IsUnicode(false)
                        .HasColumnType("varchar(60)");

                    b.HasKey("Code", "CodeLogiciel");

                    b.HasIndex("CodeLogiciel");

                    b.HasIndex("CodeModuleParent", "CodeLogicielParent");

                    b.ToTable("Modules");

                    b.HasData(
                        new
                        {
                            Code = "SEQUENCAGE",
                            CodeLogiciel = "GENOMICA",
                            Nom = "Séquençage"
                        },
                        new
                        {
                            Code = "MARQUAGE",
                            CodeLogiciel = "GENOMICA",
                            CodeLogicielParent = "GENOMICA",
                            CodeModuleParent = "SEQUENCAGE",
                            Nom = "Marquage"
                        },
                        new
                        {
                            Code = "SEPARATION",
                            CodeLogiciel = "GENOMICA",
                            CodeLogicielParent = "GENOMICA",
                            CodeModuleParent = "SEQUENCAGE",
                            Nom = "Séparation"
                        },
                        new
                        {
                            Code = "ANALYSE",
                            CodeLogiciel = "GENOMICA",
                            CodeLogicielParent = "GENOMICA",
                            CodeModuleParent = "SEQUENCAGE",
                            Nom = "Analyse"
                        },
                        new
                        {
                            Code = "POLYMORPHISME",
                            CodeLogiciel = "GENOMICA",
                            Nom = "Polymorphisme génétique"
                        },
                        new
                        {
                            Code = "VAR_ALLELE",
                            CodeLogiciel = "GENOMICA",
                            Nom = "Variations alléliques"
                        },
                        new
                        {
                            Code = "UTILS_ROLES",
                            CodeLogiciel = "GENOMICA",
                            Nom = "Utilisateurs et rôles"
                        },
                        new
                        {
                            Code = "PARAMETRES",
                            CodeLogiciel = "GENOMICA",
                            Nom = "Paramètres"
                        },
                        new
                        {
                            Code = "MICRO",
                            CodeLogiciel = "ANATOMIA",
                            Nom = "Anatomie microscopique"
                        },
                        new
                        {
                            Code = "PATHO",
                            CodeLogiciel = "ANATOMIA",
                            Nom = "Anatomie pathologique"
                        },
                        new
                        {
                            Code = "FONC",
                            CodeLogiciel = "ANATOMIA",
                            Nom = "Anatomie fonctionnelle"
                        },
                        new
                        {
                            Code = "RADIO",
                            CodeLogiciel = "ANATOMIA",
                            Nom = "Anatomie radiologique"
                        },
                        new
                        {
                            Code = "TOPO",
                            CodeLogiciel = "ANATOMIA",
                            Nom = "Anatomie topographique"
                        });
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Personne", b =>
                {
                    b.Property<string>("Pseudo")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CodeEquipe")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CodeMetier")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Manager")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<decimal>("TauxProductivite")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(3,2)")
                        .HasDefaultValue(1m);

                    b.HasKey("Pseudo");

                    b.HasIndex("CodeEquipe");

                    b.HasIndex("CodeMetier");

                    b.HasIndex("Manager");

                    b.ToTable("Personnes");
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Release", b =>
                {
                    b.Property<short>("Numero")
                        .HasColumnType("smallint");

                    b.Property<float>("NumeroVersion")
                        .HasColumnType("real");

                    b.Property<string>("CodeLogiciel")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("DatePubli")
                        .HasColumnType("datetime2");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Numero", "NumeroVersion", "CodeLogiciel");

                    b.HasIndex("NumeroVersion", "CodeLogiciel");

                    b.ToTable("Releases");
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Service", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Code");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Tache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CodeActivite")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CodeLogiciel")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CodeModule")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<decimal>("DureePrevue")
                        .HasColumnType("decimal(3, 1)");

                    b.Property<decimal>("DureeRestante")
                        .HasColumnType("decimal(3, 1)");

                    b.Property<float>("NumVersion")
                        .HasColumnType("real");

                    b.Property<string>("Personne")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Titre")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Id");

                    b.HasIndex("CodeActivite");

                    b.HasIndex("Personne");

                    b.HasIndex("CodeModule", "CodeLogiciel");

                    b.HasIndex("NumVersion", "CodeLogiciel");

                    b.ToTable("Taches");
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Travail", b =>
                {
                    b.Property<DateTime>("DateTravail")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdTache")
                        .HasColumnType("int");

                    b.Property<decimal>("Heures")
                        .HasColumnType("decimal(3, 1)");

                    b.Property<decimal>("TauxProductivite")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(3, 2)")
                        .HasDefaultValue(1m);

                    b.HasKey("DateTravail", "IdTache");

                    b.HasIndex("IdTache");

                    b.ToTable("Travaux");
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Version", b =>
                {
                    b.Property<float>("Numero")
                        .HasColumnType("real");

                    b.Property<string>("CodeLogiciel")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("DateOuverture")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateSortiePrevue")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateSortieReelle")
                        .HasColumnType("datetime2");

                    b.Property<short>("Millesime")
                        .HasColumnType("smallint");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Numero", "CodeLogiciel");

                    b.HasIndex("CodeLogiciel");

                    b.ToTable("Versions");

                    b.HasData(
                        new
                        {
                            Numero = 1f,
                            CodeLogiciel = "GENOMICA",
                            DateOuverture = new DateTime(2022, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateSortiePrevue = new DateTime(2023, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateSortieReelle = new DateTime(2023, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Millesime = (short)2023
                        },
                        new
                        {
                            Numero = 2f,
                            CodeLogiciel = "GENOMICA",
                            DateOuverture = new DateTime(2022, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateSortiePrevue = new DateTime(2024, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Millesime = (short)2024
                        },
                        new
                        {
                            Numero = 4.5f,
                            CodeLogiciel = "ANATOMIA",
                            DateOuverture = new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateSortiePrevue = new DateTime(2022, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateSortieReelle = new DateTime(2022, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Millesime = (short)2022
                        },
                        new
                        {
                            Numero = 5f,
                            CodeLogiciel = "ANATOMIA",
                            DateOuverture = new DateTime(2022, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateSortiePrevue = new DateTime(2023, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateSortieReelle = new DateTime(2023, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Millesime = (short)2023
                        },
                        new
                        {
                            Numero = 5.5f,
                            CodeLogiciel = "ANATOMIA",
                            DateOuverture = new DateTime(2023, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateSortiePrevue = new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Millesime = (short)2024
                        });
                });

            modelBuilder.Entity("JobOverview_v94.Entities.ActiviteMetier", b =>
                {
                    b.HasOne("JobOverview_v94.Entities.Activite", null)
                        .WithMany()
                        .HasForeignKey("CodeActivite")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JobOverview_v94.Entities.Metier", null)
                        .WithMany()
                        .HasForeignKey("CodeMetier")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Equipe", b =>
                {
                    b.HasOne("JobOverview_v94.Entities.Filiere", null)
                        .WithMany("Equipes")
                        .HasForeignKey("CodeFiliere")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("JobOverview_v94.Entities.Service", "Service")
                        .WithMany()
                        .HasForeignKey("CodeService")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Service");
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Logiciel", b =>
                {
                    b.HasOne("JobOverview_v94.Entities.Filiere", null)
                        .WithMany()
                        .HasForeignKey("CodeFiliere")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Metier", b =>
                {
                    b.HasOne("JobOverview_v94.Entities.Service", null)
                        .WithMany()
                        .HasForeignKey("CodeService")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Module", b =>
                {
                    b.HasOne("JobOverview_v94.Entities.Logiciel", null)
                        .WithMany("Modules")
                        .HasForeignKey("CodeLogiciel")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("JobOverview_v94.Entities.Module", null)
                        .WithMany("SousModules")
                        .HasForeignKey("CodeModuleParent", "CodeLogicielParent")
                        .OnDelete(DeleteBehavior.NoAction);
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Personne", b =>
                {
                    b.HasOne("JobOverview_v94.Entities.Equipe", null)
                        .WithMany("Personnes")
                        .HasForeignKey("CodeEquipe")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("JobOverview_v94.Entities.Metier", "Métier")
                        .WithMany()
                        .HasForeignKey("CodeMetier")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("JobOverview_v94.Entities.Personne", null)
                        .WithMany()
                        .HasForeignKey("Manager")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Métier");
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Release", b =>
                {
                    b.HasOne("JobOverview_v94.Entities.Version", null)
                        .WithMany("Releases")
                        .HasForeignKey("NumeroVersion", "CodeLogiciel")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Tache", b =>
                {
                    b.HasOne("JobOverview_v94.Entities.Activite", null)
                        .WithMany()
                        .HasForeignKey("CodeActivite")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("JobOverview_v94.Entities.Personne", null)
                        .WithMany()
                        .HasForeignKey("Personne")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("JobOverview_v94.Entities.Module", null)
                        .WithMany()
                        .HasForeignKey("CodeModule", "CodeLogiciel")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("JobOverview_v94.Entities.Version", null)
                        .WithMany()
                        .HasForeignKey("NumVersion", "CodeLogiciel")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Travail", b =>
                {
                    b.HasOne("JobOverview_v94.Entities.Tache", null)
                        .WithMany("Travaux")
                        .HasForeignKey("IdTache")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Version", b =>
                {
                    b.HasOne("JobOverview_v94.Entities.Logiciel", null)
                        .WithMany()
                        .HasForeignKey("CodeLogiciel")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Equipe", b =>
                {
                    b.Navigation("Personnes");
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Filiere", b =>
                {
                    b.Navigation("Equipes");
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Logiciel", b =>
                {
                    b.Navigation("Modules");
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Module", b =>
                {
                    b.Navigation("SousModules");
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Tache", b =>
                {
                    b.Navigation("Travaux");
                });

            modelBuilder.Entity("JobOverview_v94.Entities.Version", b =>
                {
                    b.Navigation("Releases");
                });
#pragma warning restore 612, 618
        }
    }
}
