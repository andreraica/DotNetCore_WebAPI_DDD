using Infrastructure.CrossCutting.Configuration;
using Infrastructure.CrossCutting.Helper;
using Infrastructure.CrossCutting.Logger;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;

namespace Middleware.Api.Filters
{
    public class LogsRequestsAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //bool grayLogAble = AppSettings.Get<bool>("Able");
            //if (grayLogAble)
            //    PostToGrayLog(context);
        }

        private async void PostToGrayLog(ActionExecutingContext context)
        {
            try
            {
                //POST
            }
            catch
            {
                // N�o fazer nada por enquanto. Apenas para n�o estourar erro 502 na aplica��o.
            }
        }

        public dynamic ControllerNameActionName(ActionExecutingContext context)
        {
            var ControllerAction = new
            {
                Controller = ((Microsoft.AspNetCore.Mvc.ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName,
                Action = ((Microsoft.AspNetCore.Mvc.ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName,
            };
            return ControllerAction;
        }

    }

}

