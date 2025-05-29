using Microsoft.EntityFrameworkCore;
using OneBeyondApi.DTO;
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

        public List<LoanFine> GetLoanFines()
        {
            using (var context = new LibraryContext())
            {
                return context.LoanFines.ToList();
            }
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

        public Guid AddLoanFine(LoanFine loanFine)
        {
            using (var context = new LibraryContext())
            {
                context.LoanFines.Add(loanFine);
                context.SaveChanges();
                return loanFine.Id;
            }
        }

        public LoanUpdateResultDto UpdateLoan(Guid bookStockId, LoanUpdateDto updateDto)
        {
            using (var context = new LibraryContext())
            {
                if (updateDto == null)
                    throw new ArgumentNullException(nameof(updateDto), "Update data must be provided.");

                var bookStock = context.Catalogue
                    .Include(x => x.OnLoanTo)
                    .SingleOrDefault(x => x.Id == bookStockId);
                if (bookStock == null)
                    throw new ArgumentException("Book stock does not exist.", nameof(bookStockId));

                if (updateDto.BorrowerId != null)
                {
                    if (updateDto.LoanEndDate == null)
                        throw new InvalidOperationException("Cannot set a borrower without an end date.");

                    if (updateDto.LoanEndDate < DateTime.Now.Date)
                        throw new ArgumentException("Loan end date cannot be in the past.", nameof(updateDto.LoanEndDate));

                    var borrower = context.Borrowers.SingleOrDefault(b => b.Id == updateDto.BorrowerId.Value);
                    if (borrower == null)
                        throw new ArgumentException("Borrower does not exist.", nameof(updateDto.BorrowerId));

                    bookStock.LoanEndDate = updateDto.LoanEndDate;
                    bookStock.OnLoanTo = borrower;
                }
                else
                {
                    if (updateDto.LoanEndDate != null)
                        throw new InvalidOperationException("Cannot set a loan end date without a borrower.");

                    if (bookStock.LoanEndDate == null || bookStock.OnLoanTo == null)
                        throw new InvalidOperationException("Loan end date or borrower is already null.");

                    if (bookStock.LoanEndDate < DateTime.Now.Date)
                    {
                        var loanFine = new LoanFine
                        {
                            BorrowerId = bookStock.OnLoanTo.Id,
                            BookStockId = bookStockId,
                            Active = true
                        };

                        AddLoanFine(loanFine);
                    }

                    bookStock.LoanEndDate = null;
                    bookStock.OnLoanTo = null;

                    // explicitly clear the shadow FK for OnLoanTo (in-memory DB limitation workaround)
                    context.Entry(bookStock).Property("OnLoanToId").CurrentValue = null;
                }

                context.SaveChanges();

                return new LoanUpdateResultDto
                {
                    BookStockId = bookStock.Id,
                    LoanEndDate = bookStock.LoanEndDate,
                    BorrowerId = bookStock.OnLoanTo?.Id
                };
            }
        }

        public LoanUpdateResultDto ReturnOnLoan(Guid bookStockId)
        {
            using (var context = new LibraryContext())
            {
                var updateDto = new LoanUpdateDto
                {
                    LoanEndDate = null,
                    BorrowerId = null
                };

                var result = UpdateLoan(bookStockId, updateDto);

                if (result == null)
                    throw new ArgumentException("Loan could not be returned.", nameof(bookStockId));

                return result;
            }
        }
    }
}
