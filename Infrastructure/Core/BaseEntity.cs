using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Core
{
    public class BaseEntity : Audit, ISoftDeleted
    {
        public bool IsDeleted { get; set; }
    }
}