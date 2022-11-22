using DKCN.Models;

namespace DKCN.Services
{
    public interface IChungnhanKhacRepository
    {
        Task AddAsync(ThongTinKhac thongTinvayVon);
        Task UpdateAsync(ThongTinKhac thongTinvayVon);
        ThongTinKhac GetById(int id);
        Task DeleteAsync(int id);
        IEnumerable<ThongTinKhac> GetAll();
        Task UpdateById(int id);
    }
}
