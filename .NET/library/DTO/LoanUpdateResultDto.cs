namespace OneBeyondApi.DTO
{
    public class LoanUpdateResultDto
    {
        public Guid BookStockId { get; set; }
        public DateTime? LoanEndDate { get; set; }
        public Guid? BorrowerId { get; set; }
    }
}
