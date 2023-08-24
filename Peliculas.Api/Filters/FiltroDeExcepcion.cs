using Microsoft.AspNetCore.Mvc.Filters;

namespace Peliculas.Api.Filters
{
    public class FiltroDeExcepcion: ExceptionFilterAttribute
    {
        private readonly ILogger<FiltroDeExcepcion> _logger;

        public FiltroDeExcepcion(ILogger<FiltroDeExcepcion> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context);
        }
    }
}
