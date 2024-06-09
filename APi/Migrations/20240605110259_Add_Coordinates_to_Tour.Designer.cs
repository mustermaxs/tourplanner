﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Tourplanner;

#nullable disable

namespace Api.Migrations
{
    [DbContext(typeof(TourContext))]
    [Migration("20240605110259_Add_Coordinates_to_Tour")]
    partial class Add_Coordinates_to_Tour
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.3.24172.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Tourplanner.Entities.Map", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TourId")
                        .HasColumnType("integer");

                    b.Property<int>("Zoom")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Maps");
                });

            modelBuilder.Entity("Tourplanner.Entities.Tile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("MapId")
                        .HasColumnType("integer");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<int>("X")
                        .HasColumnType("integer");

                    b.Property<int>("Y")
                        .HasColumnType("integer");

                    b.Property<int>("Zoom")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MapId");

                    b.ToTable("Tiles");
                });

            modelBuilder.Entity("Tourplanner.Entities.TourLogs.TourLog", b =>
                {
                    b.Property<int>("TourLogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TourLogId"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<float>("Difficulty")
                        .HasColumnType("real");

                    b.Property<float>("Duration")
                        .HasColumnType("real");

                    b.Property<float>("Rating")
                        .HasColumnType("real");

                    b.Property<int>("TourId")
                        .HasColumnType("integer");

                    b.HasKey("TourLogId");

                    b.HasIndex("TourId");

                    b.ToTable("TourLogs");
                });

            modelBuilder.Entity("Tourplanner.Entities.Tours.Tour", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<float>("ChildFriendliness")
                        .HasColumnType("real");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<float>("Distance")
                        .HasColumnType("real");

                    b.Property<float>("EstimatedTime")
                        .HasColumnType("real");

                    b.Property<string>("From")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<int?>("MapId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("character varying(60)");

                    b.Property<float>("Popularity")
                        .HasColumnType("real");

                    b.Property<string>("To")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<int>("TransportType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MapId")
                        .IsUnique();

                    b.ToTable("Tours");
                });

            modelBuilder.Entity("Tourplanner.Entities.Map", b =>
                {
                    b.OwnsOne("Tourplanner.Entities.Bbox", "Bbox", b1 =>
                        {
                            b1.Property<int>("MapId")
                                .HasColumnType("integer");

                            b1.Property<double>("MaxX")
                                .HasColumnType("double precision");

                            b1.Property<double>("MaxY")
                                .HasColumnType("double precision");

                            b1.Property<double>("MinX")
                                .HasColumnType("double precision");

                            b1.Property<double>("MinY")
                                .HasColumnType("double precision");

                            b1.HasKey("MapId");

                            b1.ToTable("Maps");

                            b1.WithOwner()
                                .HasForeignKey("MapId");
                        });

                    b.Navigation("Bbox")
                        .IsRequired();
                });

            modelBuilder.Entity("Tourplanner.Entities.Tile", b =>
                {
                    b.HasOne("Tourplanner.Entities.Map", "Map")
                        .WithMany("Tiles")
                        .HasForeignKey("MapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Map");
                });

            modelBuilder.Entity("Tourplanner.Entities.TourLogs.TourLog", b =>
                {
                    b.HasOne("Tourplanner.Entities.Tours.Tour", "Tour")
                        .WithMany("TourLogs")
                        .HasForeignKey("TourId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tour");
                });

            modelBuilder.Entity("Tourplanner.Entities.Tours.Tour", b =>
                {
                    b.HasOne("Tourplanner.Entities.Map", "Map")
                        .WithOne("Tour")
                        .HasForeignKey("Tourplanner.Entities.Tours.Tour", "MapId");

                    b.OwnsOne("Tourplanner.Entities.Tours.Coordinates", "Coordinates", b1 =>
                        {
                            b1.Property<int>("TourId")
                                .HasColumnType("integer");

                            b1.Property<double>("Lattitude")
                                .HasColumnType("double precision");

                            b1.Property<double>("Longitude")
                                .HasColumnType("double precision");

                            b1.HasKey("TourId");

                            b1.ToTable("Tours");

                            b1.WithOwner()
                                .HasForeignKey("TourId");
                        });

                    b.Navigation("Coordinates")
                        .IsRequired();

                    b.Navigation("Map");
                });

            modelBuilder.Entity("Tourplanner.Entities.Map", b =>
                {
                    b.Navigation("Tiles");

                    b.Navigation("Tour")
                        .IsRequired();
                });

            modelBuilder.Entity("Tourplanner.Entities.Tours.Tour", b =>
                {
                    b.Navigation("TourLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
