using E_Commerce.Application.Comman;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{
    #region api attributes
    [Route("api/[controller]")]//to catch end points
    [ApiController]//enable some features
    #endregion
    public class ApiBaseController : ControllerBase
    {
        //return action result of value >cheak have value or not

        //return type of and end point                   
        public static ActionResult<T> ToActionResult<T>(Result<T> result)//convert result pattern to action result
        { //he sent Result unless it succecc or not /have data or not  
            if (result.IsSuccess)
            {
                return new OkObjectResult(result.Value);
            }
            else
            {
                return ToProblem(result.Errors);
            }
        }


        //void action result >cheak seccess or not
        public static ActionResult ToActionResult(Result result)
        {
            if (result.IsSuccess)
            {
                return new OkResult();
            }
            else
            {
                return ToProblem(result.Errors);
            }
        }

        //return problem details

        //ObjectResult used if i have 1 case //ActionResult used if i have many cases like [ok,notfound,ex]
        protected static ObjectResult ToProblem(IReadOnlyList<Error> errors)
        {
            var firstError = errors[0];

            var statusCode = firstError.ErrorType switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

            var problem = new ProblemDetails()
            {
                Status = statusCode,
                Title = firstError.Code,
                Detail = firstError.Description,

                //to send errors

                //dictionary of (key,value) to add any properties
                Extensions = { ["errors"] = errors }
            };

            return new ObjectResult(problem) { StatusCode = statusCode }; //ok>مش معني انه هيرجع نتيجه انها 200
        }

        protected string GetEmailFromToken()
        {
           return User.FindFirstValue(ClaimTypes.Email)?? throw new UnauthorizedAccessException("No Email Clim Found");
        }
    }

}
