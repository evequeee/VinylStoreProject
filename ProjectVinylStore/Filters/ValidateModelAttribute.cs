using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectVinylStore.Business.DTOs;

namespace ProjectVinylStore.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var validationErrors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                var errorResponse = new ErrorResponse
                {
                    Message = "One or more validation errors occurred.",
                    ErrorCode = "VALIDATION_ERROR",
                    StatusCode = 400,
                    ValidationErrors = validationErrors,
                    TraceId = context.HttpContext.TraceIdentifier
                };

                var apiResponse = ApiResponse.ErrorResult(errorResponse);
                context.Result = new BadRequestObjectResult(apiResponse);
            }
        }
    }
}