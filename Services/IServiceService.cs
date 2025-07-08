using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Services.Interfaces
{
    public interface IServiceService
    {
        Task<Service?> GetServiceById(int id);
        Task<IEnumerable<Service>> GetAllServices();
    }
}
