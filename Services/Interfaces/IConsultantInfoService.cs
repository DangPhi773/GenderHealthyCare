using BusinessObjects.Models;
using BusinessObjects.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces;
public interface IConsultantInfoService
{
    Task<List<ConsultantInfo>> GetAllConsultantInfosAsync();
    Task<ConsultantInfo?> GetConsultantInfoByIdAsync(int consultantId);
    Task<bool> CreateConsultantInfoAsync(ConsultantInfo consultantInfo);
}
