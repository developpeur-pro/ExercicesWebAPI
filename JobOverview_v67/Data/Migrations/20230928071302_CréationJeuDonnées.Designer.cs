﻿// <auto-generated />
using System;
using JobOverview_v67.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JobOverview_v67.Data.Migrations
{
    [DbContext(typeof(ContexteJobOverview))]
    [Migration("20230928071302_CréationJeuDonnées")]
    partial class CréationJeuDonnées
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JobOverview.Entities.Filiere", b =>
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

            modelBuilder.Entity("JobOverview.Entities.Logiciel", b =>
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

            modelBuilder.Entity("JobOverview.Entities.Module", b =>
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

            modelBuilder.Entity("JobOverview.Entities.Release", b =>
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

                    b.HasKey("Numero", "NumeroVersion", "CodeLogiciel");

                    b.HasIndex("NumeroVersion", "CodeLogiciel");

                    b.ToTable("Releases");
                });

            modelBuilder.Entity("JobOverview.Entities.Version", b =>
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

            modelBuilder.Entity("JobOverview.Entities.Logiciel", b =>
                {
                    b.HasOne("JobOverview.Entities.Filiere", null)
                        .WithMany()
                        .HasForeignKey("CodeFiliere")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("JobOverview.Entities.Module", b =>
                {
                    b.HasOne("JobOverview.Entities.Logiciel", null)
                        .WithMany()
                        .HasForeignKey("CodeLogiciel")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("JobOverview.Entities.Module", null)
                        .WithMany()
                        .HasForeignKey("CodeModuleParent", "CodeLogicielParent")
                        .OnDelete(DeleteBehavior.NoAction);
                });

            modelBuilder.Entity("JobOverview.Entities.Release", b =>
                {
                    b.HasOne("JobOverview.Entities.Version", null)
                        .WithMany()
                        .HasForeignKey("NumeroVersion", "CodeLogiciel")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("JobOverview.Entities.Version", b =>
                {
                    b.HasOne("JobOverview.Entities.Logiciel", null)
                        .WithMany()
                        .HasForeignKey("CodeLogiciel")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
