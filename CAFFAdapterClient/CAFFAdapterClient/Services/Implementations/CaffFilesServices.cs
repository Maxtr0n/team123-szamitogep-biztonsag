using CAFFAdapterClient.DataTransferObjects.CaffFiles;
using CAFFAdapterClient.Domain;
using CAFFAdapterClient.Framework.Providers;
using CAFFAdapterClient.Infrastructure.Data;
using CAFFAdapterClient.Infrastructure.Exceptions;
using CAFFAdapterClient.ViewModels;
using CAFFAdapterClient.ViewModels.CaffFiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CAFFAdapterClient.Services
{
    public class CaffFilesServices : ICaffFilesServices
    {
        private readonly AppModelDbContext _dbContext;
        private readonly IUserProvider _userProvider;

        public CaffFilesServices(
            AppModelDbContext dbContext,
            IUserProvider userProvider)
        {
            _dbContext = dbContext;
            _userProvider = userProvider;
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
            var caff = await _dbContext.CaffFiles
                .Include(x => x.Comments.Where(y => !y.IsDeleted))
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new DataNotFoundException();

            return new CaffFileViewModel
            {
                Description = caff.Description,
                Comments = caff.Comments.Select(x => new CommentViewModel
                {
                    Id = x.Id,
                    CreationAt = x.CreatedAt,
                    Message = x.Message,
                    UserId = x.UserId,
                    UserFirstName = x.User?.FirstName,
                    UserLastName = x.User?.LastName
                })
            };
        }

        public async Task DeleteByIdAsync(int id)
        {
            var caff = await _dbContext.CaffFiles.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new DataNotFoundException();

            if (_userProvider.GetUserRole() != Domain.Enums.UserRoles.Admin && caff.UserId != _userProvider.GetUserId())
            {
                throw new BusinessLogicException("Az adott CAFF fájl nem törölhető!");
            }

            caff.IsDeleted = true;

            _dbContext.CaffFiles.Update(caff);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CreateAsync(CreateCaffFileDto dto)
        {
            // TODO: preview betöltés, itt kellene ráhívni a parserra
            var preview = new byte[0];
            preview = new System.Net.WebClient().DownloadData("https://c.tenor.com/eFPFHSN4rJ8AAAAd/example.gif");

            var caff = new CaffFile()
            {
                File = dto.File,
                Preview = preview,
                UserId = _userProvider.GetUserId()
            };

            _dbContext.CaffFiles.Add(caff);

            await _dbContext.SaveChangesAsync();

            return caff.Id;
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

        public async Task AddCommentAsync(int caffId, AddComment dto)
        {
            var caff = await _dbContext.CaffFiles.FirstOrDefaultAsync(x => x.Id == caffId)
                ?? throw new DataNotFoundException();

            _dbContext.Comments.Add(new Comment()
            {
                CreatedAt = DateTime.UtcNow,
                UserId = _userProvider.GetUserId(),
                Message = dto.Message,
                CaffId = caff.Id
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCommentByIdAsync(int id, int cid)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == cid && x.CaffId == id)
                  ?? throw new DataNotFoundException();

            if (_userProvider.GetUserRole() != Domain.Enums.UserRoles.Admin && comment.UserId != _userProvider.GetUserId())
            {
                throw new BusinessLogicException("Az adott komment nem törölhető!");
            }

            comment.IsDeleted = true;

            _dbContext.Comments.Update(comment);

            await _dbContext.SaveChangesAsync();
        }
    }
}
