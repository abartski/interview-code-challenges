namespace OneBeyondApi.Model
{
    public class LoanFine
    {
        public Guid Id { get; set; }
        public Guid? BorrowerId { get; set; }
        public Guid BookStockId { get; set; }
        public bool Active { get; set; }
    }
}
