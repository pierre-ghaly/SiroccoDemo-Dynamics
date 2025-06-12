using SiroccoDemo.Domain.Exceptions;
using System;
using System.Net;
using System.Web.Http;

namespace SiroccoDemo.APIs.Controllers
{
    public abstract class BaseController : ApiController
    {
        protected IHttpActionResult ExecuteWithExceptionHandling<T>(Func<T> action)
        {
            try
            {
                var result = action();
                return Ok(result);
            }
            catch (InvalidInputException ex)
            {
                return Content(HttpStatusCode.BadRequest, new
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
                return Content(HttpStatusCode.BadRequest, new
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
                return Content(HttpStatusCode.InternalServerError, new
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
                return Content(HttpStatusCode.InternalServerError, new
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
                return Content(HttpStatusCode.InternalServerError, new
                {
                    ErrorCode = ex.ErrorCode,
                    Message = ex.Message,
                    OccurredAt = ex.OccurredAt
                });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    ErrorCode = "UNEXPECTED_ERROR",
                    Message = "An unexpected error occurred",
                    OccurredAt = DateTime.UtcNow,
                    Details = ex.Message
                });
            }
        }
    }
} 