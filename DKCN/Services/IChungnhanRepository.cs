using DKCN.Models;

namespace DKCN.Services
{
    public interface IChungnhanRepository
    {
        Task AddAsync(ThongTinVayVon thongTinvayVon);
        Task UpdateAsync(ThongTinVayVon thongTinvayVon);
        ThongTinVayVon GetById(int id);
        Task DeleteAsync(int id);
        IEnumerable<ThongTinVayVon> GetAll();
        Task UpdateById(int id);
    }
}
