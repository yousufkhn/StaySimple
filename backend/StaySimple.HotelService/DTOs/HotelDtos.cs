namespace StaySimple.HotelService.DTOs
{
    // how are they different from the class based dtos we make? these records are immutable, clean (downside is we can't apply validations)
    //  we set their values using contructors
    public record HotelDto(int Id, string Name, string City, string Address, string Description, double Rating, string ImageUrl);
    public record RoomDto(int Id, int HotelId, string HotelName, string RoomType, decimal PricePerNight, int Capacity, bool IsAvailable);
    public record CreateHotelDto(string Name, string City, string Address, string Description, double Rating, string ImageUrl);
    public record CreateRoomDto(int HotelId, string RoomType, decimal PricePerNight, int Capacity, bool IsAvailable);
    public record RoomAvailabilityResult(bool RoomExists,bool IsAvailable);

}
