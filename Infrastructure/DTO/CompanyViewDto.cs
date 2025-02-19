using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class CompanyViewDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? StockTicker { get; set; }
        public string? Exchange { get; set; }
        public string? Isin { get; set; }
        public string? Website { get; set; }
    }
}