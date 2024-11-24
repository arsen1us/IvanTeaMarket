using CustomerChurmPrediction.Entities.CompanyEntity;
using CustomerChurmPrediction.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerChurmPrediction.Controllers
{
    [ApiController]
    [Route("/api/company")]
    public class CompanyController : Controller
    {
        ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        // Получить список компаний
        // GET: /api/company

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var companyList = await _companyService.FindAllAsync(default, default);
                if (companyList is not null)
                    return Ok(new { companyList = companyList });

                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // Получить по id
        // GET: /api/company/{companyId}

        [HttpGet]
        [Route("{companyId}")]
        public async Task<IActionResult> GetByIdAsync(string companyId)
        {
            try
            {
                var company = await _companyService.FindByIdAsync(companyId, default);
                if (company is not null)
                    return Ok(new { company = company });

                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Надо-не надо, пока не знаю
        // Получить список компаний, которые продают продукт в данной категории
        // GET: api/company/category/{categoryId}

        [HttpGet]
        [Route("category/{categoryId}")]
        public async Task<IActionResult> GetByCategoryIdAsync(string categoryId)
        {
            try
            {
                throw new Exception();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] CompanyAdd companyAdd)
        {
            if (companyAdd is null)
                return BadRequest();
            try
            {
                Company company = new Company
                {
                    Name = companyAdd.Name,
                    Story = companyAdd.Story,
                    Description = companyAdd.Description,
                    OwnerIds = companyAdd.OwnerIds
                };

                bool isSuccess = await _companyService.SaveOrUpdateAsync(company);
                if (isSuccess)
                    return Ok(new { company = company });
                return StatusCode(500);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut]
        [Route("{companyId}")]
        public async Task<IActionResult> UpdateAsync(string companyId, [FromBody] CompanyUpdate companyUpdate)
        {
            try
            {
                var company = await _companyService.FindByIdAsync(companyId, default);

                company.Name = companyUpdate.Name;
                company.Story = companyUpdate.Story;
                company.Description = companyUpdate.Description;
                company.OwnerIds = companyUpdate.OwnerIds;

                bool isSuccess = await _companyService.SaveOrUpdateAsync(company);
                if (isSuccess)
                    return Ok(new { company = company });
                return StatusCode(500);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{companyId}")]
        public async Task<IActionResult> DeleteAsync(string companyId)
        {
            if (string.IsNullOrEmpty(companyId))
                return BadRequest();
            try
            {
                long deletedCount = await _companyService.DeleteAsync(companyId);
                if (deletedCount > 0)
                    return Ok(new { deletedCound = deletedCount });
                return NotFound();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
