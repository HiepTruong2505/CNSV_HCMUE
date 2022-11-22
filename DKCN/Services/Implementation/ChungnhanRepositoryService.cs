using DKCN.DataAccess;
using DKCN.Models;
using Microsoft.EntityFrameworkCore;

namespace DKCN.Services.Implementation
{
    public class ChungnhanRepositoryService : IChungnhanRepository
    {
        private readonly ApplicationDbContext _context;
        public ChungnhanRepositoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ThongTinVayVon thongTinvayVon)
        {
            await _context.ThongTinVayVon.AddAsync(thongTinvayVon);
            await _context.SaveChangesAsync();
        }

        public  ThongTinVayVon GetById(int id)
        {
            return _context.ThongTinVayVon.FirstOrDefault(u => u.ID == id);
        }

        public async Task DeleteAsync(int id)
        {
            var thongtin = GetById(id);
            _context.ThongTinVayVon.Remove(thongtin);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<ThongTinVayVon> GetAll()
        {
            var query = _context.ThongTinVayVon.AsQueryable().AsNoTracking();
            return query;
        }

        

        public Task UpdateAsync(ThongTinVayVon thongTinvayVon)
        {
            throw new NotImplementedException();
        }

        public Task UpdateById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
