using EcommerceProject.Data.Base;
using EcommerceProject.Data.ViewModel;
using EcommerceProject.Models;
using System.Threading.Tasks;

namespace EcommerceProject.Data.Services
{
    /* Interfaz del servicio de las peliculas con un CRUD de ellas */
    public interface IMovieServices : IEntityBaseRepository<Movie>
    {
        Task<Movie> GetMovieByIdAsync(int id);
        Task<NewMoviewDropDownsVM> GetNewMovieDropdownsValues();
        Task AddNewMovieAsync(NewMovieVM data);
        Task UpdateMovieAsync(NewMovieVM data);
    }
}
