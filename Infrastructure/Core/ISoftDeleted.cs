using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Core
{
    public interface ISoftDeleted
    {
        bool IsDeleted { get; set; }
    }
}