using Microsoft.Xrm.Sdk;
using SiroccoDemo.Domain.Exceptions;
using System;

namespace SiroccoDemo.Plugins
{
    public abstract class PluginBase : IPlugin
    {
        protected internal IOrganizationService OrganizationService { get; private set; }
        protected internal IPluginExecutionContext Context { get; private set; }

        public void Execute(IServiceProvider serviceProvider)
        {
            Context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            OrganizationService = serviceFactory.CreateOrganizationService(Context.UserId);

            ExecuteWithExceptionHandling(() => ExtendedExecute());
        }

        protected void ExecuteWithExceptionHandling(Action action)
        {
            try
            {
                action();
            }
            catch (InvalidInputException ex)
            {
                var validationErrors = ex.ValidationErrors != null ? string.Join(", ", ex.ValidationErrors) : "None";
                throw new InvalidPluginExecutionException($"[{ex.ErrorCode}] {ex.Message}. Property: {ex.PropertyName}. Validation Errors: {validationErrors}");
            }
            catch (BusinessRuleException ex)
            {
                throw new InvalidPluginExecutionException($"[{ex.ErrorCode}] {ex.Message}. Rule: {ex.RuleName}. Entity: {ex.EntityType}");
            }
            catch (TransactionFailedException ex)
            {
                throw new InvalidPluginExecutionException($"[{ex.ErrorCode}] {ex.Message}. Operation: {ex.OperationName}. Transaction ID: {ex.TransactionId}");
            }
            catch (CrmException ex)
            {
                throw new InvalidPluginExecutionException($"[{ex.ErrorCode}] {ex.Message}. CRM Error: {ex.CrmErrorCode}. Entity: {ex.EntityType} ({ex.EntityId})");
            }
            catch (ExceptionBase ex)
            {
                throw new InvalidPluginExecutionException($"[{ex.ErrorCode}] {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException($"[UNEXPECTED_ERROR] An unexpected error occurred: {ex.Message}");
            }
        }

        protected T ExecuteWithExceptionHandling<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (InvalidInputException ex)
            {
                var validationErrors = ex.ValidationErrors != null ? string.Join(", ", ex.ValidationErrors) : "None";
                throw new InvalidPluginExecutionException($"[{ex.ErrorCode}] {ex.Message}. Property: {ex.PropertyName}. Validation Errors: {validationErrors}");
            }
            catch (BusinessRuleException ex)
            {
                throw new InvalidPluginExecutionException($"[{ex.ErrorCode}] {ex.Message}. Rule: {ex.RuleName}. Entity: {ex.EntityType}");
            }
            catch (TransactionFailedException ex)
            {
                throw new InvalidPluginExecutionException($"[{ex.ErrorCode}] {ex.Message}. Operation: {ex.OperationName}. Transaction ID: {ex.TransactionId}");
            }
            catch (CrmException ex)
            {
                throw new InvalidPluginExecutionException($"[{ex.ErrorCode}] {ex.Message}. CRM Error: {ex.CrmErrorCode}. Entity: {ex.EntityType} ({ex.EntityId})");
            }
            catch (ExceptionBase ex)
            {
                throw new InvalidPluginExecutionException($"[{ex.ErrorCode}] {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException($"[UNEXPECTED_ERROR] An unexpected error occurred: {ex.Message}");
            }
        }

        public abstract void ExtendedExecute();
    }
}
