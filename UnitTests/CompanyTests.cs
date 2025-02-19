using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Services;
using Business.Validation;
using FluentValidation.TestHelper;
using Infrastructure.DTO;
using Infrastructure.Entity;
using Infrastructure.Repository;
using Moq;
using Xunit;

namespace UnitTests
{
    public class CompanyTests
    {
        private readonly Mock<ICompanyRepo> _companyRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ICompanyService _companyService;
        private readonly CompanyAddValidation _companyAddValidation;

        public CompanyTests()
        {
            _companyRepoMock = new Mock<ICompanyRepo>();
            _mapperMock = new Mock<IMapper>();
            _companyService = new CompanyService(_companyRepoMock.Object, _mapperMock.Object);
            _companyAddValidation = new CompanyAddValidation(
                _companyRepoMock.Object
            );
        }

        [Fact]
        public async Task CreateCompany_WhenCompanyRequestIsValid_ReturnsCompanyViewDto()
        {
            // Arrange
            var companyRequest = new CompanyRequest
            {
                Name = "Test Company",
                StockTicker = "TST",
                Exchange = "NYSE",
                Website = "http://testcompany.com",
                Isin = "US1234567890"
            };

            var companyEntity = new Company
            {
                Id = Guid.NewGuid(),
                Name = companyRequest.Name,
                StockTicker = companyRequest.StockTicker,
                Exchange = companyRequest.Exchange,
                Website = companyRequest.Website,
                Isin = companyRequest.Isin
            };

            var companyViewDto = new CompanyViewDto
            {
                Id = companyEntity.Id,
                Name = companyEntity.Name,
                StockTicker = companyEntity.StockTicker,
                Exchange = companyEntity.Exchange,
                Website = companyEntity.Website,
                Isin = companyEntity.Isin
            };

            _mapperMock.Setup(m => m.Map<Company>(companyRequest)).Returns(companyEntity);
            _companyRepoMock.Setup(r => r.CreateCompany(It.IsAny<Company>())).ReturnsAsync(companyEntity);
            _mapperMock.Setup(m => m.Map<CompanyViewDto>(companyEntity)).Returns(companyViewDto);

            // Act
            var result = await _companyService.CreateCompany(companyRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(companyViewDto.Id, result.Id);
            Assert.Equal(companyViewDto.Name, result.Name);
            Assert.Equal(companyViewDto.StockTicker, result.StockTicker);
            Assert.Equal(companyViewDto.Exchange, result.Exchange);
            Assert.Equal(companyViewDto.Website, result.Website);
            Assert.Equal(companyViewDto.Isin, result.Isin);
        }

        [Fact]
        public async Task CreateCompany_WhenIsinInvalid_ReturnsValidationError()
        {
            // Arrange
            var companyRequest = new CompanyRequest
            {
                Name = "Test Company",
                StockTicker = "TST",
                Exchange = "NYSE",
                Website = "http://testcompany.com",
                Isin = "US123456789"
            };

            // Act
            await _companyService.CreateCompany(companyRequest);

            // Assert
            var valid = await _companyAddValidation.TestValidateAsync(companyRequest);
            valid.ShouldHaveValidationErrorFor(x => x.Isin);
        }
    }
}