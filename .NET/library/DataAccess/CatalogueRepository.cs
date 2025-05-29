using Microsoft.EntityFrameworkCore;
using OneBeyondApi.Model;

namespace OneBeyondApi.DataAccess
{
    public class CatalogueRepository : ICatalogueRepository
    {
        public CatalogueRepository()
        {
        }
        public List<BookStock> GetCatalogue()
        {
            using (var context = new LibraryContext())
            {
                var list = context.Catalogue
                    .Include(x => x.Book)
                    .ThenInclude(x => x.Author)
                    .Include(x => x.OnLoanTo)
                    .ToList();
                return list;
            }
        }

        public List<OnLoan> GetOnLoan()
        {
            var result = SearchCatalogue(new CatalogueSearch { OnLoan = true });

            return result?
                .GroupBy(x => x.OnLoanTo!)
                .Select(group => new OnLoan
                {
                    Borrower = group.Key,
                    BookNames = group.Select(item => item.Book.Name).ToList()
                })
                .ToList() ?? [];
        }

        public List<BookStock> SearchCatalogue(CatalogueSearch search)
        {
            using (var context = new LibraryContext())
            {
                var list = context.Catalogue
                    .Include(x => x.Book)
                    .ThenInclude(x => x.Author)
                    .Include(x => x.OnLoanTo)
                    .AsQueryable();

                if (search != null)
                {
                    if (!string.IsNullOrEmpty(search.Author)) {
                        list = list.Where(x => x.Book.Author.Name.Contains(search.Author));
                    }
                    if (!string.IsNullOrEmpty(search.BookName)) {
                        list = list.Where(x => x.Book.Name.Contains(search.BookName));
                    }
                    if (search.OnLoan.HasValue)
                    {
                        bool isOnLoan = search.OnLoan.Value;
                        list = list.Where(x => x.OnLoanTo != null == isOnLoan);
                    }
                }

                return list.ToList();
            }
        }
    }
}
