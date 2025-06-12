using SiroccoDemo.Application.Services;
using SiroccoDemo.Domain.DTOs;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace SiroccoDemo.APIs.Controllers
{
    /// <summary>
    /// Account contact note summary
    /// </summary>
    [RoutePrefix("api")]
    public class GetAccountContactNoteSummaryController : ApiController
    {
        private readonly IGetAccountContactNoteSummaryService _service;

        public GetAccountContactNoteSummaryController(IGetAccountContactNoteSummaryService service)
        {
            _service = service;
        }

        /// <summary>
        /// Gets a summary of all accounts with their primary contacts, secondary contacts, and primary contact notes
        /// </summary>
        /// <returns>List of account summaries with contact and notes information</returns>
        /// <response code="200">Summary retrieved successfully</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Route("account-contact-note-summary")]
        [SwaggerOperation("GetAccountContactNoteSummary")]
        [SwaggerResponse(200, "Summary retrieved successfully", typeof(List<GetAccountContactNoteSummaryDTO>))]
        [SwaggerResponse(500, "Internal Server Error")]
        public IHttpActionResult GetAccountContactNoteSummary()
        {
            try
            {
                var result = _service.GetAccountContactNoteSummary();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("An error occurred while retrieving the summary", ex));
            }
        }
    }
} 