namespace OneBeyondApi.Model
{
    public class LoanReservation
    {
        public Guid Id { get; set; }
        public Guid? BorrowerId { get; set; }
        public BookStock? BookStock { get; set; }
        public int QueueNumber { get; set; }
    }
}
