using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoorType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "warehouse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouse", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "shelves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShelfLevel = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shelves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shelves_warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "warehouse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shelf_material",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShelfId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shelf_material", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shelf_material_materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_shelf_material_shelves_ShelfId",
                        column: x => x.ShelfId,
                        principalTable: "shelves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    ShelfId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_transactions_materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transactions_shelves_ShelfId",
                        column: x => x.ShelfId,
                        principalTable: "shelves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transactions_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_shelf_material_MaterialId",
                table: "shelf_material",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_shelf_material_ShelfId",
                table: "shelf_material",
                column: "ShelfId");

            migrationBuilder.CreateIndex(
                name: "IX_shelves_WarehouseId",
                table: "shelves",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_MaterialId",
                table: "transactions",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_ShelfId",
                table: "transactions",
                column: "ShelfId");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_UserId",
                table: "transactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_RoleId",
                table: "users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shelf_material");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "materials");

            migrationBuilder.DropTable(
                name: "shelves");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "warehouse");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
