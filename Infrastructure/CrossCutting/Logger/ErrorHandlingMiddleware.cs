using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Infrastructure.CrossCutting.Logger
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other scoped dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                //Write in log
                //PostExceptionToGrayLog(context, ex);
            }
        }
        
    }
}
