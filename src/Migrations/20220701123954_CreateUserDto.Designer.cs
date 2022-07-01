﻿// <auto-generated />
using System;
using AuthenticationApi.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AuthenticationApi.Migrations
{
    [DbContext(typeof(AuthenticationContext))]
    [Migration("20220701123954_CreateUserDto")]
    partial class CreateUserDto
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.14");

            modelBuilder.Entity("AuthenticationApi.Repositories.Dtos.UserDto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("UsersSecurity");
                });
#pragma warning restore 612, 618
        }
    }
}
