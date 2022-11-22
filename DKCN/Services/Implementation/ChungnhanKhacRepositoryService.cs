using DKCN.DataAccess;
using DKCN.Models;
using Microsoft.EntityFrameworkCore;

namespace DKCN.Services.Implementation
{
    public class ChungnhanKhacRepositoryService : IChungnhanKhacRepository
    {
        private readonly ApplicationDbContext _context;
        public ChungnhanKhacRepositoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(ThongTinKhac thongTinvayVon)
        {
            await _context.AddAsync(thongTinvayVon);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var thongtin = GetById(id);
            _context.ThongTinKhac.Remove(thongtin);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<ThongTinKhac> GetAll()
        {
            var query = _context.ThongTinKhac.AsQueryable().AsNoTracking();
            return query;
        }

        public ThongTinKhac GetById(int id)
        {
            var result = _context.ThongTinKhac.FirstOrDefault(x => x.ID == id);
            return result;
        }

        public Task UpdateAsync(ThongTinKhac thongTinvayVon)
        {
            throw new NotImplementedException();
        }

        public Task UpdateById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
