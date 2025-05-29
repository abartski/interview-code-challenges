namespace OneBeyondApi.DTO
{
    public class LoanReservationCreateDto
    {
        public Guid? BorrowerId { get; set; }
        public Guid BookStockId { get; set; }
    }
}
