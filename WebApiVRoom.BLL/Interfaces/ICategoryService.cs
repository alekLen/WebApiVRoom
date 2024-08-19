using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiVRoom.BLL.DTO;

namespace WebApiVRoom.BLL.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDTO> GetCategory(int id);
        Task<IEnumerable<CategoryDTO>> GetAllCategories();
        Task<IEnumerable<CategoryDTO>> GetAllPaginated(int pageNumber, int pageSize);
        Task<CategoryDTO> GetCategoryByName(string name);
        Task AddCategory(CategoryDTO categoryDTO);
        Task<CategoryDTO> UpdateCategory(CategoryDTO categoryDTO);
        Task DeleteCategory(int id);
    }
}
