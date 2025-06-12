using SiroccoDemo.Application.Services;
using SiroccoDemo.Domain.DTOs;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;
using System.Web.Http;

namespace SiroccoDemo.APIs.Controllers
{
    /// <summary>
    /// Account contact and note summary retrieval
    /// </summary>
    [RoutePrefix("api")]
    public class GetAccountContactNoteSummaryController : BaseController
    {
        private readonly IGetAccountContactNoteSummaryService _service;

        public GetAccountContactNoteSummaryController(IGetAccountContactNoteSummaryService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves a summary of all accounts including their contact information and associated notes.
        /// </summary>
        /// <returns>List of account summaries with contact and note information</returns>
        /// <response code="200">Account summaries retrieved successfully</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Route("account-contact-note-summary")]
        [SwaggerOperation("GetAccountContactNoteSummary")]
        [SwaggerResponse(200, "Account Summaries Retrieved Successfully", typeof(List<GetAccountContactNoteSummaryDTO>))]
        [SwaggerResponse(500, "Internal Server Error")]
        public IHttpActionResult GetAccountContactNoteSummary()
        {
            return ExecuteWithExceptionHandling(() => _service.GetAccountContactNoteSummary());
        }
    }
} 