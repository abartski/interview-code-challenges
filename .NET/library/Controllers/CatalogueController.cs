using Microsoft.AspNetCore.Mvc;
using OneBeyondApi.DataAccess;
using OneBeyondApi.DTO;
using OneBeyondApi.Model;

namespace OneBeyondApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogueController : ControllerBase
    {
        private readonly ILogger<CatalogueController> _logger;
        private readonly ICatalogueRepository _catalogueRepository;

        public CatalogueController(ILogger<CatalogueController> logger, ICatalogueRepository catalogueRepository)
        {
            _logger = logger;
            _catalogueRepository = catalogueRepository;   
        }

        [HttpGet]
        [Route("GetCatalogue")]
        public IList<BookStock> Get()
        {
            return _catalogueRepository.GetCatalogue();
        }

        [HttpGet]
        [Route("GetOnLoan")]
        public IList<OnLoan> GetOnLoan()
        {
            return _catalogueRepository.GetOnLoan();
        }

        [HttpGet]
        [Route("GetLoanFines")]
        public IList<LoanFine> GetLoanFines()
        {
            return _catalogueRepository.GetLoanFines();
        }

        [HttpPost]
        [Route("SearchCatalogue")]
        public IList<BookStock> Post(CatalogueSearch search)
        {
            return _catalogueRepository.SearchCatalogue(search);
        }

        [HttpPost]
        [Route("AddLoanFine")]
        public Guid PostLoanFine(LoanFine loanFine)
        {
            return _catalogueRepository.AddLoanFine(loanFine);
        }

        [HttpPatch]
        [Route("{bookStockId}/Loan")]
        public IActionResult UpdateLoan(Guid bookStockId, [FromBody] LoanUpdateDto updateDto)
        {
            var result = _catalogueRepository.UpdateLoan(bookStockId, updateDto);

            return Ok(result);
        }

        [HttpPatch]
        [Route("{bookStockId}/Loan/Return")]
        public IActionResult ReturnOnLoan(Guid bookStockId)
        {
            var result = _catalogueRepository.ReturnOnLoan(bookStockId);

            return Ok(result);
        }
    }
}