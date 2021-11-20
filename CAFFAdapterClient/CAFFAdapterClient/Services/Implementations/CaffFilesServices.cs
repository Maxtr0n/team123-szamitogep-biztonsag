using CAFFAdapterClient.DataTransferObjects.CaffFiles;
using CAFFAdapterClient.Domain;
using CAFFAdapterClient.Infrastructure.Data;
using CAFFAdapterClient.Infrastructure.Exceptions;
using CAFFAdapterClient.ViewModels;
using CAFFAdapterClient.ViewModels.CaffFiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CAFFAdapterClient.Services
{
    public class CaffFilesServices : ICaffFilesServices
    {
        private readonly AppModelDbContext _dbContext;

        public CaffFilesServices(AppModelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TableViewModel<CaffFileRowViewModel>> GetAsync()
        {
            var items = _dbContext.CaffFiles
                .AsNoTracking()
                .Select(x => new CaffFileRowViewModel
                {
                    Id = x.Id
                })
                .ToList();

            var count = items.Count();

            return new TableViewModel<CaffFileRowViewModel>
            {
                Items = items,
                Count = count
            };
        }

        public async Task<ActionResult<CaffFileViewModel>> GetByIdAsync(int id)
        {
            var caff = await _dbContext.CaffFiles.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new DataNotFoundException();

            return new CaffFileViewModel
            {
                
            };
        }

        public async Task DeleteByIdAsync(int id)
        {
            var caff = await _dbContext.CaffFiles.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new DataNotFoundException();

            caff.IsDeleted = true;

            _dbContext.CaffFiles.Update(caff);

            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateAsync(CreateCaffFileDto dto)
        {
            // TODO: preview betöltés, itt kellene ráhívni a parserra
            var preview = new byte[0];

            var caff = new CaffFile()
            {
                File = dto.File,
                Preview = preview
            };

            _dbContext.CaffFiles.Add(caff);

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateCaffFileDto dto)
        {
            var caff = await _dbContext.CaffFiles.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new DataNotFoundException();

            _dbContext.CaffFiles.Update(caff);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<byte[]> GetPreviewByIdAsync(int id)
        {
            var caff = await _dbContext.CaffFiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new DataNotFoundException();

            return caff.Preview;
        }

        public async Task<byte[]> GetFileByIdAsync(int id)
        {
            var caff = await _dbContext.CaffFiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new DataNotFoundException();

            return caff.File;
        }
    }
}
