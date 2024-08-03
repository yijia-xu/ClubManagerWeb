﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab5.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SportsClub",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fee = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SportsClub", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    NewsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Question = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SportClubId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.NewsId);
                    table.ForeignKey(
                        name: "FK_News_SportsClub_SportClubId",
                        column: x => x.SportClubId,
                        principalTable: "SportsClub",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    FanId = table.Column<int>(type: "int", nullable: false),
                    SportClubId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => new { x.FanId, x.SportClubId });
                    table.ForeignKey(
                        name: "FK_Subscription_Fan_FanId",
                        column: x => x.FanId,
                        principalTable: "Fan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscription_SportsClub_SportClubId",
                        column: x => x.SportClubId,
                        principalTable: "SportsClub",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_News_SportClubId",
                table: "News",
                column: "SportClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_SportClubId",
                table: "Subscription",
                column: "SportClubId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "Fan");

            migrationBuilder.DropTable(
                name: "SportsClub");
        }
    }
}
