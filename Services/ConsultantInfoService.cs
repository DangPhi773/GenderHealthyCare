using BusinessObjects.Models;
using BusinessObjects.ViewModels;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;
public class ConsultantInfoService(IConsultantInfoRepository repo) : IConsultantInfoService
{
    private readonly IConsultantInfoRepository _repo = repo;

    public async Task<List<ConsultantInfo>> GetAllConsultantInfosAsync()
    {
        var data = await _repo.GetAllConsultantInfosAsync();
        return data;
    }

    public async Task<ConsultantInfo?> GetConsultantInfoByIdAsync(int consultantId)
    {
        var data = await _repo.GetConsultantInfoByIdAsync(consultantId);
        return data;
    }
}
