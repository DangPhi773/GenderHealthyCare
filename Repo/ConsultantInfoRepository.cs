using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories;
public class ConsultantInfoRepository(ConsultantInfoDAO dao) : IConsultantInfoRepository
{
    private readonly ConsultantInfoDAO _dao = dao;

    public async Task<List<ConsultantInfo>> GetAllConsultantInfosAsync()
    {
        var data = await _dao.GetAllConsultantInfosAsync();
        return data;
    }

    public async Task<ConsultantInfo?> GetConsultantInfoByIdAsync(int consultantId)
    {
        var data = await _dao.GetConsultantInfoByIdAsync(consultantId);
        return data;
    }
    public async Task<bool> AddConsultantInfoAsync(ConsultantInfo info)
    {
        var result = await _dao.CreateConsultantInfoAsync(info);
        return result;
    }
}
