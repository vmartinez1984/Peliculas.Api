using Microsoft.EntityFrameworkCore;

namespace Peliculas.Api.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertarParametrosDePaginacionEnCabecera<T>(this HttpContext context, IQueryable<T> queryable)
        {
            if(context == null) throw new ArgumentNullException(nameof(context));

            double cantidad = await queryable.CountAsync();

            context.Response.Headers.Add("cantidadTotalDeRegistros", cantidad.ToString());
        }
    }
}
