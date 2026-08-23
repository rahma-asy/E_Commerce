using E_Commerce.Application.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.ObjectPool;
using System.Runtime.CompilerServices;
using System.Text;

namespace E_Commerce.API.Attributes
{
    // IActionFilter have 2 metthods OnActionExecuted and OnActionExecuting
    //IAsyncActionFilter have task async OnActionExecution
    public class RedisCasheAttribute : ActionFilterAttribute //it is implement IActionFilter and IAsyncActionFilter only override whet you need  
    {
        private readonly int _durationInSeconds;

        public RedisCasheAttribute(int durationInSeconds =60)
        {
            _durationInSeconds = durationInSeconds;
        }
        //context have all data about current request   [params,parameters,httpcontext[requst ,response,path and URL],user,route vales]                                        
        // next is function responsible for exucute controller's action[اللي جاي هنا بسببه كاني بقوله روح كمل]
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //get cashe service from container
            var cacheService=context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            //so we need cache key to check   [https://localhost:7243/api/Products? or https://localhost:7243/api/Products?typeid=2&typeid=2& ...............]
            var cacheKey=CreateCacheKey(context.HttpContext.Request);
            var data=  await cacheService.GetDataAsync(cacheKey);

            //if data exist in cashe =>get data from cashe and skip endpoint

            if (!string.IsNullOrEmpty(data)) //i have data
            {
                //will create response
                context.Result = new ContentResult()
                {
                    Content = data,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return; //will not go to end pount
            }

            //if data not exist in cashe => execute endpoint+ store result in cashe if result is 200ok+ have data

            var executedContext = await next.Invoke();//will go to end point and return action executed [end pointبعد تنفيذ ال   ]
            if (executedContext.Result is OkObjectResult { Value:not null} ok) //OkObjectResult ckeck result type and  casting in ok
{
              await  cacheService.SetDataAsync(cacheKey, ok.Value,TimeSpan.FromSeconds(_durationInSeconds));
}           
        
        }
        //helper method
        private static string CreateCacheKey(HttpRequest request) 
        {
            //base[https://localhost:7243] not important we need resourse[/api/Products? ]
            var key = new StringBuilder();//to save memory[we have lot of cocatinations so us mutable]

            key.Append(request.Path);// api/Products?typeid=2 
            //check if there is any params[at least one key-value pair ]
            if (request.Query.Any())
            {
                key.Append("?");// api/Products?
                foreach (var (k, v) in request.Query.OrderBy(x=>x.Key))
                {
                    key.Append(k).Append("=").Append(v).Append("&");
                }
            }
            return key.ToString();  
        }
    }
}
