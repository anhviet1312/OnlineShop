using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopOnline.Data.Migrations
{
    public partial class change_deleteTag_Cascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTags_Tags_TagID",
                table: "ProductTags");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTags_Tags_TagID",
                table: "ProductTags",
                column: "TagID",
                principalTable: "Tags",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTags_Tags_TagID",
                table: "ProductTags");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTags_Tags_TagID",
                table: "ProductTags",
                column: "TagID",
                principalTable: "Tags",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
