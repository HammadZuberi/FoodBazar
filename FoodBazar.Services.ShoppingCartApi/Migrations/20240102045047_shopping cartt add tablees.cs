using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodBazar.Services.ShoppingCartApi.Migrations
{
    /// <inheritdoc />
    public partial class shoppingcarttaddtablees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cartHeaders",
                columns: table => new
                {
                    CartHeaderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CouponCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cartHeaders", x => x.CartHeaderId);
                });

            migrationBuilder.CreateTable(
                name: "cartDetails",
                columns: table => new
                {
                    CartDeatailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartHeaderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cartDetails", x => x.CartDeatailsId);
                    table.ForeignKey(
                        name: "FK_cartDetails_cartHeaders_CartHeaderId",
                        column: x => x.CartHeaderId,
                        principalTable: "cartHeaders",
                        principalColumn: "CartHeaderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cartDetails_CartHeaderId",
                table: "cartDetails",
                column: "CartHeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cartDetails");

            migrationBuilder.DropTable(
                name: "cartHeaders");
        }
    }
}
