﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Progressterra.Context;

namespace Progressterra.Migrations
{
    [DbContext(typeof(ProgressterraContext))]
    [Migration("20190908194732_AddHeaders")]
    partial class AddHeaders
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Progressterra.Context.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Available");

                    b.Property<DateTime>("EventTime");

                    b.Property<long?>("ResponseTime");

                    b.Property<int>("ServiceId");

                    b.HasKey("Id");

                    b.HasIndex("ServiceId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Progressterra.Context.Header", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int>("ServiceId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("ServiceId");

                    b.ToTable("Headers");
                });

            modelBuilder.Entity("Progressterra.Context.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("Progressterra.Context.Event", b =>
                {
                    b.HasOne("Progressterra.Context.Service", "Service")
                        .WithMany("Events")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Progressterra.Context.Header", b =>
                {
                    b.HasOne("Progressterra.Context.Service", "Service")
                        .WithMany("Headers")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
