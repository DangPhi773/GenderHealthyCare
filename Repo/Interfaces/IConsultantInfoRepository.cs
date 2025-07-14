using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces;
public interface IConsultantInfoRepository
{
    Task<List<ConsultantInfo>> GetAllConsultantInfosAsync();
    Task<ConsultantInfo?> GetConsultantInfoByIdAsync(int consultantId);
    Task<bool> AddConsultantInfoAsync(ConsultantInfo consultantInfo);
}
