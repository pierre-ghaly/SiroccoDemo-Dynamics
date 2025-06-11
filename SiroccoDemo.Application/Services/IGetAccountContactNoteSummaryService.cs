using SiroccoDemo.Application.Models;
using SiroccoDemo.Domain.DTOs;

namespace SiroccoDemo.Application.Services
{
    public interface IGetAccountContactNoteSummaryService
    {
        GetAccountContactNoteSummaryDTO GetAccountContactNoteSummary(GetAccountContactNoteSummaryModel model);
    }
}
