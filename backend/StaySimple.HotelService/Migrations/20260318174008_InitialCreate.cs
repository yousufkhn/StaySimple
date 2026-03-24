using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StaySimple.HotelService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hotels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelId = table.Column<int>(type: "int", nullable: false),
                    RoomType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PricePerNight = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "Address", "City", "Description", "ImageUrl", "Name", "Rating" },
                values: new object[,]
                {
                    { 1, "123 Marine Drive, Mumbai", "Mumbai", "A luxurious 5-star hotel overlooking the Arabian Sea with world-class amenities.", "https://images.unsplash.com/photo-1566073771259-6a8506099945?w=800", "The Grand Palace", 4.7999999999999998 },
                    { 2, "45 Palace Road, Jaipur", "Jaipur", "Experience royal Rajasthani hospitality in this beautifully restored heritage property.", "https://images.unsplash.com/photo-1551882547-ff40c63fe5fa?w=800", "Royal Heritage Inn", 4.5 },
                    { 3, "78 Mall Road, Manali", "Manali", "A serene mountain retreat with stunning Himalayan views and cozy accommodations.", "https://images.unsplash.com/photo-1542314831-068cd1dbfeeb?w=800", "Mountain View Resort", 4.2999999999999998 },
                    { 4, "22 Baga Beach Road, Goa", "Goa", "A beachfront paradise with private beach access and vibrant nightlife nearby.", "https://images.unsplash.com/photo-1520250497591-112f2f40a3f4?w=800", "Sunset Beach Hotel", 4.5999999999999996 },
                    { 5, "99 Connaught Place, Delhi", "Delhi", "Modern business hotel in the heart of New Delhi, perfect for work and leisure.", "https://images.unsplash.com/photo-1564501049412-61c2a3083791?w=800", "City Central Hotel", 4.2000000000000002 },
                    { 6, "12 Lake Pichola Road, Udaipur", "Udaipur", "A romantic lakeside hotel with stunning views of Lake Pichola and City Palace.", "https://images.unsplash.com/photo-1582719508461-905c673771fd?w=800", "Lakeside Retreat", 4.7000000000000002 }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Capacity", "HotelId", "IsAvailable", "PricePerNight", "RoomType" },
                values: new object[,]
                {
                    { 1, 1, 1, true, 3500m, "Single" },
                    { 2, 2, 1, true, 5500m, "Double" },
                    { 3, 4, 1, true, 12000m, "Suite" },
                    { 4, 1, 2, true, 2500m, "Single" },
                    { 5, 2, 2, true, 4000m, "Double" },
                    { 6, 3, 2, true, 7500m, "Deluxe" },
                    { 7, 1, 3, true, 2000m, "Single" },
                    { 8, 2, 3, true, 3500m, "Double" },
                    { 9, 4, 3, true, 6000m, "Suite" },
                    { 10, 1, 4, true, 3000m, "Single" },
                    { 11, 2, 4, true, 5000m, "Double" },
                    { 12, 3, 4, true, 8500m, "Deluxe" },
                    { 13, 1, 5, true, 2200m, "Single" },
                    { 14, 2, 5, true, 3800m, "Double" },
                    { 15, 2, 6, true, 4500m, "Double" },
                    { 16, 4, 6, true, 9500m, "Suite" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HotelId",
                table: "Rooms",
                column: "HotelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Hotels");
        }
    }
}
