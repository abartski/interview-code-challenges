using OneBeyondApi.DTO;
using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface ICatalogueRepository
    {
        public List<BookStock> GetCatalogue();

        public List<OnLoan> GetOnLoan();

        public List<BookStock> SearchCatalogue(CatalogueSearch search);

        public LoanUpdateResultDto UpdateLoan(Guid bookStockId, LoanUpdateDto updateDto);

        public LoanUpdateResultDto ReturnOnLoan(Guid bookStockId);
    }
}
