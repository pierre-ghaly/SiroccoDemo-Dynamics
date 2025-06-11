using SiroccoDemo.Application.Models;
using SiroccoDemo.Domain.DTOs;
using System.Collections.Generic;

namespace SiroccoDemo.Application.Services
{
    public interface IGetAccountContactNoteSummaryService
    {
        List<GetAccountContactNoteSummaryDTO> GetAccountContactNoteSummary(GetAccountContactNoteSummaryModel model);
    }
}
