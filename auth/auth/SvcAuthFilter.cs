using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Auth.Core;
using Auth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Auth
{
    public class SvcAuthFilter : ActionFilterAttribute
    {
        private readonly SvcTokenStorage tokenStorage;

        public SvcAuthFilter(SvcTokenStorage tokenStorage)
        {
            this.tokenStorage = tokenStorage;
        }

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var argument in context.ActionArguments.Values.Where(v => v is SecuredModel))
            {
                SecuredModel model = argument as SecuredModel;
                if (model == null)
                {
                    context.Result = new StatusCodeResult((int) HttpStatusCode.Unauthorized);
                }
                else
                {
                    var current = await tokenStorage.GetSvcToken(model.SvcId);
                    if (!current.Success || current.Result.Token != model.SvcToken)
                    {
                        context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                    }
                }
                
            }
            base.OnActionExecuting(context);
        }
    }
}
