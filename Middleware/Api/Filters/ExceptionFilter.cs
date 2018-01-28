using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Infrastructure.CrossCutting.Logger;
using System.Net;
using Infrastructure.CrossCutting.Configuration;
using Infrastructure.CrossCutting.Helper;

namespace Middleware.Api.Filters
{
    public class ExcetionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            //bool grayLogAble = AppSettings.Get<bool>("Able");
            //if (grayLogAble)
                //Save GrayLog
                //PostToGrayLog(context);

            int statusCode = (int)HttpStatusCode.BadRequest;
            if (context.Exception is UnauthorizedAccessException)
                statusCode = (int)HttpStatusCode.Unauthorized;

            context.HttpContext.Response.StatusCode = statusCode;
            context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(context.Exception.Message);

            base.OnException(context);
        }

    }
}