using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Context;
using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public interface ICompanyRepo
    {
        Task<IEnumerable<Company>> GetAllCompanies();
        Task<Company?> GetCompanyById(Guid id);
        Task<Company> CreateCompany(Company company);
        Task<Company> UpdateCompany(Company company);
        Task<bool> CheckIfIsinExists(string ISIN, Guid? id = null);
        Task<Company?> GetCompanyByIsin(string ISIN);
    }
    public class CompanyRepo : ICompanyRepo
    {
        private readonly GLContext _context;
        public CompanyRepo(GLContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> CheckIfIsinExists(string ISIN, Guid? id = null)
        {
            return await _context.Companies.AnyAsync(x => x.Isin == ISIN && (!id.HasValue || x.Id != id.Value));
        }

        public async Task<Company> CreateCompany(Company company)
        {
            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<IEnumerable<Company>> GetAllCompanies()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<Company?> GetCompanyById(Guid id)
        {
            return await _context.Companies.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Company?> GetCompanyByIsin(string ISIN)
        {
            return await _context.Companies.FirstOrDefaultAsync(x => x.Isin == ISIN);
        }

        public async Task<Company> UpdateCompany(Company company)
        {
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
            return company;
        }
    }
}