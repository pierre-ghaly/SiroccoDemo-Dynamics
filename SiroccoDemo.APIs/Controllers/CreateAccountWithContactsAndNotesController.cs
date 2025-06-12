using SiroccoDemo.Application.Models;
using SiroccoDemo.Application.Services;
using SiroccoDemo.Domain.DTOs;
using SiroccoDemo.Domain.Exceptions;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Web.Http;

namespace SiroccoDemo.APIs.Controllers
{
    /// <summary>
    /// Account with contacts and notes creation
    /// </summary>
    [RoutePrefix("api")]
    public class CreateAccountWithContactsAndNotesController : ApiController
    {
        private readonly ICreateAccountWithContactsAndNotesService _service;

        public CreateAccountWithContactsAndNotesController(ICreateAccountWithContactsAndNotesService service)
        {
            _service = service;
        }

        /// <summary>
        /// Creates an account with optional primary contact, secondary contacts, account notes, and primary contact notes.
        /// Only the Account field is required. All other fields are optional.
        /// Note: Primary contact notes cannot be provided without a primary contact.
        /// All operations are executed in a single transaction - if any step fails, everything is rolled back.
        /// </summary>
        /// <param name="model">The account creation model. Only Account field is required, all others are optional.</param>
        /// <returns>Created account details with generated IDs</returns>
        /// <response code="200">Account created successfully</response>
        /// <response code="400">Invalid input data or business rule violation. Returns structured error with ErrorCode, Message, and specific details.</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Route("create-account-with-contacts-and-notes")]
        [SwaggerOperation("CreateAccountWithContactsAndNotes")]
        [SwaggerResponse(200, "Account created successfully", typeof(CreateAccountWithContactsAndNotesDTO))]
        [SwaggerResponse(400, "Bad Request - Invalid input data")]
        [SwaggerResponse(500, "Internal Server Error")]
        public IHttpActionResult CreateAccountWithContactsAndNotes([FromBody] CreateAccountWithContactsAndNotesModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Request body cannot be null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = _service.CreateAccountWithContactsAndNotes(model);
                return Ok(result);
            }
            catch (InvalidInputException ex)
            {
                return Content(System.Net.HttpStatusCode.BadRequest, new
                {
                    ErrorCode = ex.ErrorCode,
                    Message = ex.Message,
                    PropertyName = ex.PropertyName,
                    ValidationErrors = ex.ValidationErrors,
                    OccurredAt = ex.OccurredAt
                });
            }
            catch (BusinessRuleException ex)
            {
                return Content(System.Net.HttpStatusCode.BadRequest, new
                {
                    ErrorCode = ex.ErrorCode,
                    Message = ex.Message,
                    RuleName = ex.RuleName,
                    EntityType = ex.EntityType,
                    OccurredAt = ex.OccurredAt
                });
            }
            catch (TransactionFailedException ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new
                {
                    ErrorCode = ex.ErrorCode,
                    Message = ex.Message,
                    OperationName = ex.OperationName,
                    TransactionId = ex.TransactionId,
                    OccurredAt = ex.OccurredAt
                });
            }
            catch (CrmException ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new
                {
                    ErrorCode = ex.ErrorCode,
                    Message = ex.Message,
                    CrmErrorCode = ex.CrmErrorCode,
                    EntityType = ex.EntityType,
                    EntityId = ex.EntityId,
                    OccurredAt = ex.OccurredAt
                });
            }
            catch (ExceptionBase ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new
                {
                    ErrorCode = ex.ErrorCode,
                    Message = ex.Message,
                    OccurredAt = ex.OccurredAt
                });
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new
                {
                    ErrorCode = "UNEXPECTED_ERROR",
                    Message = "An unexpected error occurred while creating the account",
                    OccurredAt = DateTime.UtcNow,
                    Details = ex.Message
                });
            }
        }
    }
} 