namespace Peliculas.Api.Dtos
{
    public class PaginacionDto
    {        
        public int Pagina { get; set; } = 1;

        private int registrosPorPagina = 50;

        private readonly int CantidadMaximaPorPagina = 50;

        public int RegistrosPorPagina
        {
            get { return registrosPorPagina; }
            set
            {
                registrosPorPagina = value > CantidadMaximaPorPagina ? CantidadMaximaPorPagina : value;
            }
        }
    }
}
