using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecializationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Specialization",
                table: "DoctorProfiles",
                newName: "SpecializationId");

            migrationBuilder.CreateTable(
                name: "Specializations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specializations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorProfiles_SpecializationId",
                table: "DoctorProfiles",
                column: "SpecializationId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorProfiles_Specializations_SpecializationId",
                table: "DoctorProfiles",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorProfiles_Specializations_SpecializationId",
                table: "DoctorProfiles");

            migrationBuilder.DropTable(
                name: "Specializations");

            migrationBuilder.DropIndex(
                name: "IX_DoctorProfiles_SpecializationId",
                table: "DoctorProfiles");

            migrationBuilder.RenameColumn(
                name: "SpecializationId",
                table: "DoctorProfiles",
                newName: "Specialization");
        }
    }
}
