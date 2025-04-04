using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayrollService.Core.Domain.Entities;

namespace PayrollService.Core.RepositoryContracts.SeparateRepository
{
    public interface ISalariesBaseRepository : IRepository<EmailLog>
    {
        
    }
}
