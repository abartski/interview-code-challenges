using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public interface ICatalogueRepository
    {
        public List<BookStock> GetCatalogue();

        public List<OnLoan> GetOnLoan();

        public List<BookStock> SearchCatalogue(CatalogueSearch search);
    }
}
