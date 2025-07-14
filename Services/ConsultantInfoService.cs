using BusinessObjects.Models;
using BusinessObjects.Models.Request;
using Microsoft.EntityFrameworkCore;
using Repositories;
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
    public Task<bool> UpdateConsultantInfoAsync(ConsultantInfo info) =>
        _repo.UpdateConsultantInfoAsync(info);

    public Task<bool> DeleteConsultantInfoAsync(int consultantId) =>
        _repo.DeleteConsultantInfoAsync(consultantId);
    //public async Task<bool> AddConsultantInfoAsync(ConsultantInfo consultantInfo)
    //{
    //    return await _repo.AddConsultantInfoAsync(consultantInfo);
    //}

    public async Task<bool> CreateConsultantInfoAsync(ConsultantInfo info)
    {
        if (info == null)
        {
            Console.WriteLine("ConsultantInfoService: ❌ Không thể tạo ConsultantInfo vì thông tin không hợp lệ.");
            return false;
        }
        var result = await _repo.AddConsultantInfoAsync(info);
        return result;
    }
}
