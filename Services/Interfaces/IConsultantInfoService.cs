using BusinessObjects.Models;
using BusinessObjects.ViewModels;
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
    Task<bool> UpdateConsultantInfoAsync(ConsultantInfo info);
    Task<bool> DeleteConsultantInfoAsync(int consultantId);
    //Task<bool> AddConsultantInfoAsync(ConsultantInfo info);
}
