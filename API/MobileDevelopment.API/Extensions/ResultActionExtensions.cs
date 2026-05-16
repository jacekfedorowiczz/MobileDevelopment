using Microsoft.AspNetCore.Mvc;
using MobileDevelopment.API.Models.Wrappers;

namespace MobileDevelopment.API.Extensions
{
    public static class ResultActionExtensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller)
        {
            return result.IsSuccess
                ? controller.Ok(result)
                : controller.BadRequest(result);
        }

        public static IActionResult ToActionResult(this Result result, ControllerBase controller)
        {
            return result.IsSuccess
                ? controller.Ok(result)
                : controller.BadRequest(result);
        }

        public static IActionResult ToCreatedAtActionResult<T>(
            this Result<T> result,
            ControllerBase controller,
            string actionName,
            object? routeValues)
        {
            return result.IsSuccess
                ? controller.CreatedAtAction(actionName, routeValues, result)
                : controller.BadRequest(result);
        }

        public static IActionResult ToNoContentResult(this Result result, ControllerBase controller)
        {
            return result.IsSuccess
                ? controller.NoContent()
                : controller.BadRequest(result);
        }
    }
}
