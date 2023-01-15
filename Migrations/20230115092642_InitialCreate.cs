using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscountAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    discountId = table.Column<string>(type: "text", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    discountName = table.Column<string>(type: "text", nullable: true),
                    startDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    endDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    discountType = table.Column<string>(type: "text", nullable: true),
                    discountValue = table.Column<float>(type: "real", nullable: false),
                    timerId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.discountId);
                });

            migrationBuilder.CreateTable(
                name: "DiscountProducts",
                columns: table => new
                {
                    discountId = table.Column<string>(type: "text", nullable: false),
                    productId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountProducts", x => new { x.discountId, x.productId });
                    table.ForeignKey(
                        name: "FK_DiscountProducts_Discounts_discountId",
                        column: x => x.discountId,
                        principalTable: "Discounts",
                        principalColumn: "discountId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscountProducts");

            migrationBuilder.DropTable(
                name: "Discounts");
        }
    }
}
