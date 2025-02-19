using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Infrastructure.DTO;
using Infrastructure.Repository;

namespace Business.Validation
{

    public class IsinRequestValidation : AbstractValidator<IsinRequest>
    {
        public IsinRequestValidation()
        {
            RuleFor(x => x.Isin)
                .NotEmpty().WithMessage("ISIN must not be empty")
                .Length(12).WithMessage("ISIN must be 12 characters long")
                .Matches("^[A-Z]{2}[A-Z0-9]{9}[0-9]$").WithMessage("ISIN must be in the correct format")
                .Must(IsinValidatorHelper.ValidateISIN).WithMessage("ISIN must be valid according to the ISIN standard");
        }
    }

    public class CompanyAddValidation : AbstractValidator<CompanyRequest>
    {
        private readonly ICompanyRepo _companyRepo;
        public CompanyAddValidation(ICompanyRepo companyRepo)
        {
            _companyRepo = companyRepo;

            RuleFor(x => x.Name).NotEmpty().MinimumLength(3);
            RuleFor(x => x.StockTicker).NotEmpty().MinimumLength(3);
            RuleFor(x => x.Exchange).NotEmpty().MinimumLength(3);
            RuleFor(x => x.Website)
                .Matches(@"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$")
                .When(x => !string.IsNullOrWhiteSpace(x.Website))
                .WithMessage("Website must be a valid URL");

            RuleFor(x => x.Isin)
                .NotEmpty().WithMessage("ISIN must not be empty")
                .Length(12).WithMessage("ISIN must be 12 characters long")
                .Matches("^[A-Z]{2}[A-Z0-9]{9}[0-9]$").WithMessage("ISIN must be in the correct format")
                .Must(IsinValidatorHelper.ValidateISIN).WithMessage("ISIN must be valid according to the ISIN standard");

        }
    }

    public static class IsinValidatorHelper
    {
        public static bool ValidateISIN(string isin)
        {
            if (string.IsNullOrWhiteSpace(isin) || isin.Length != 12)
                return false;

            string numericISIN = ConvertToNumeric(isin.Substring(0, 11)) + isin[11];
            return IsValidModulus10(numericISIN);
        }

        private static string ConvertToNumeric(string input)
        {
            return string.Concat(input.Select(c =>
                char.IsLetter(c) ? (c - 'A' + 10).ToString() : c.ToString()));
        }

        private static bool IsValidModulus10(string numericString)
        {
            int sum = 0;
            bool doubleDigit = false;

            for (int i = numericString.Length - 1; i >= 0; i--)
            {
                int digit = numericString[i] - '0';
                if (doubleDigit)
                {
                    digit *= 2;
                    if (digit > 9) digit -= 9;
                }
                sum += digit;
                doubleDigit = !doubleDigit;
            }

            return sum % 10 == 0;
        }
    }
}