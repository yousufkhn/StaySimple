namespace StaySimple.BookingService.Services
{
    public class BookingValidationException : Exception
    {
        public BookingValidationException(string message) : base(message)
        {
        }
    }
}