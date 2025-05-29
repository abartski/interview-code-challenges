using OneBeyondApi.DTO;
using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface ICatalogueRepository
    {
        public List<BookStock> GetCatalogue();

        public List<OnLoan> GetOnLoan();

        public List<LoanFine> GetLoanFines();

        public List<LoanReservation> GetLoanReservations();

        public string GetLoanReservationDetails(Guid borrowerId, Guid bookStockId);

        public List<BookStock> SearchCatalogue(CatalogueSearch search);

        public Guid AddLoanFine(LoanFine loanFine);

        public Guid AddLoanReservation(LoanReservationCreateDto createDto);

        public LoanUpdateResultDto UpdateLoan(Guid bookStockId, LoanUpdateDto updateDto);

        public LoanUpdateResultDto ReturnOnLoan(Guid bookStockId);
    }
}
