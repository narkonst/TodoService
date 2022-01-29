using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace TodoApiDTO.Filters
{
    internal class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        private readonly int _code;
        private readonly Type _type;

        public ExceptionHandlerAttribute(Type type, int code)
        {
            _type = type;
            _code = code;
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() != _type)
            {
                return;
            }

            var result = new ObjectResult(context.Exception.Message)
            {
                StatusCode = _code
            };

            context.Result = result;
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);

            return Task.FromResult<object>(null);
        }
    }
}
