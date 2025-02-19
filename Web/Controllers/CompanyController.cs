using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Business.Services;
using FluentValidation;
using Infrastructure.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IValidator<IsinRequest> _isinValidator;

        public CompanyController(ICompanyService companyService, IValidator<IsinRequest> isinValidator)
        {
            _companyService = companyService ?? throw new ArgumentNullException(nameof(companyService));
            _isinValidator = isinValidator ?? throw new ArgumentNullException(nameof(isinValidator));
        }

        /// <summary>
        /// Get all companies
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns all companies</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CompanyViewDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await _companyService.GetAllCompanies();
            return Ok(companies);
        }

        /// <summary>
        /// Get company by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Returns company by id</response>
        /// <response code="404">Company not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CompanyViewDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCompanyById(Guid id)
        {
            var company = await _companyService.GetCompanyById(id);
            return Ok(company);
        }

        /// <summary>
        /// Get company by ISIN
        /// </summary>
        /// <param name="ISIN"></param>
        /// <returns></returns>
        /// <response code="200">Returns company by ISIN</response>
        /// <response code="404">Company not found</response>
        [HttpGet("isin/{ISIN}")]
        [ProducesResponseType(typeof(CompanyViewDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCompanyByIsin(string ISIN)
        {
            var validationResult = _isinValidator.Validate(new IsinRequest { Isin = ISIN });
            if (!validationResult.IsValid)
            {
                throw new ValidationException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }
            var company = await _companyService.GetCompanyByIsin(ISIN);
            return Ok(company);
        }

        /// <summary>
        /// Create company
        /// </summary>
        /// <param name="companyRequest"></param>
        /// <returns></returns>
        /// <response code="201">Returns created company</response>
        /// <response code="400">Bad request</response>
        [HttpPost]
        [ProducesResponseType(typeof(CompanyViewDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyRequest companyRequest)
        {
            var company = await _companyService.CreateCompany(companyRequest);
            return CreatedAtAction(nameof(GetCompanyById), new { id = company.Id }, company);
        }

        /// <summary>
        /// Update company
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyRequest"></param>
        /// <returns></returns>
        /// <response code="200">Returns updated company</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Company not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CompanyViewDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyRequest companyRequest)
        {
            var company = await _companyService.UpdateCompany(id, companyRequest);
            return Ok(company);
        }
    }
}