using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class CompanyRequest
    {
        public Guid? Id { get; set; }
        public required string Name { get; set; }
        public required string StockTicker { get; set; }
        public required string Exchange { get; set; }
        public required string Isin { get; set; }
        public string? Website { get; set; }
    }
}