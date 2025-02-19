using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.DTO;
using Infrastructure.Entity;
using Infrastructure.Repository;

namespace Business.Services
{
    public interface ICompanyService
    {
        Task<CompanyViewDto> GetCompanyById(Guid id);
        Task<IEnumerable<CompanyViewDto>> GetAllCompanies();
        Task<CompanyViewDto> CreateCompany(CompanyRequest companyRequest);
        Task<CompanyViewDto> UpdateCompany(Guid id, CompanyRequest companyRequest);
        Task<CompanyViewDto> GetCompanyByIsin(string ISIN);
    }
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepo _companyRepo;
        private readonly IMapper _mapper;
        public CompanyService(ICompanyRepo companyRepo, IMapper mapper)
        {
            _companyRepo = companyRepo ?? throw new ArgumentNullException(nameof(companyRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<CompanyViewDto> CreateCompany(CompanyRequest companyRequest)
        {
            var isIsinExists = await _companyRepo.CheckIfIsinExists(companyRequest.Isin);
            if (isIsinExists) throw new InvalidOperationException($"ISIN {companyRequest.Isin} already exists");
            var companyEntity = _mapper.Map<Company>(companyRequest);
            var createdCompany = await _companyRepo.CreateCompany(companyEntity);
            return _mapper.Map<CompanyViewDto>(createdCompany);
        }

        public async Task<IEnumerable<CompanyViewDto>> GetAllCompanies()
        {
            var companies = await _companyRepo.GetAllCompanies();
            return _mapper.Map<IEnumerable<CompanyViewDto>>(companies);
        }

        public async Task<CompanyViewDto> GetCompanyById(Guid id)
        {
            var company = await _companyRepo.GetCompanyById(id) ?? throw new KeyNotFoundException($"Company with id {id} not found");
            return _mapper.Map<CompanyViewDto>(company);
        }

        public async Task<CompanyViewDto> GetCompanyByIsin(string ISIN)
        {
            var company = await _companyRepo.GetCompanyByIsin(ISIN) ?? throw new KeyNotFoundException($"Company with ISIN {ISIN} not found");
            return _mapper.Map<CompanyViewDto>(company);
        }

        public async Task<CompanyViewDto> UpdateCompany(Guid id, CompanyRequest companyRequest)
        {
            var isIsinExists = await _companyRepo.CheckIfIsinExists(companyRequest.Isin, id);
            if (isIsinExists) throw new InvalidOperationException($"ISIN {companyRequest.Isin} already exists");
            var company = await _companyRepo.GetCompanyById(id) ?? throw new KeyNotFoundException($"Company with id {id} not found");
            _mapper.Map(companyRequest, company);
            var updatedCompany = await _companyRepo.UpdateCompany(company);
            return _mapper.Map<CompanyViewDto>(updatedCompany);

        }
    }
}