using Peliculas.Api.Dtos;

namespace Peliculas.Api.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDto paginacion)
        {
            return queryable
                .Skip((paginacion.Pagina - 1) * paginacion.RegistrosPorPagina)
                .Take(paginacion.RegistrosPorPagina);
        }
    }
}
