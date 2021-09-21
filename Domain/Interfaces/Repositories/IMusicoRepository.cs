using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IMusicoRepository
    {
        Task<IEnumerable<MusicoModel>> GetAllAsync();
        Task<MusicoModel> GetByIdAsync(int id);
        Task CreateAsync(MusicoModel musicoModel);
        Task EditAsync(MusicoModel musicoModel);
        Task DeleteAsync(int id);
    }
}
