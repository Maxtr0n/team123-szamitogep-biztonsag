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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
            var caffName = Guid.NewGuid();
            var inputPath = $"input-caff-files/{caffName}.caff";
            var outputPath = $"processed-caff-files/{caffName}";

            if (!Directory.Exists("input-caff-files"))
            {
                Directory.CreateDirectory("input-caff-files");
            }
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            await File.WriteAllBytesAsync(inputPath, dto.File);

            using (var process = new Process())
            {
                process.StartInfo.FileName = $@"{Directory.GetCurrentDirectory()}\Parser\caffparser.exe";
                process.StartInfo.Arguments = $"--if {inputPath} --of {outputPath}/{caffName}";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                var output = new StringBuilder();
                var error = new StringBuilder();
                process.OutputDataReceived += (sender, data) => output.Append("\n" + data.Data);
                process.ErrorDataReceived += (sender, data) => error.Append("\n" + data.Data);

                var startStatus = process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                await process.WaitForExitAsync();

                if (!output.ToString().Contains("Parsing ended successfully."))
                {
                    throw new BusinessLogicException("Nem sikerült a CAFF fájl feltöltése!");
                }
            }

            // var preview = new System.Net.WebClient().DownloadData("https://c.tenor.com/eFPFHSN4rJ8AAAAd/example.gif");
            var preview = await File.ReadAllBytesAsync($"{outputPath}/{caffName}.gif");
            var json = await File.ReadAllTextAsync($"{outputPath}/{caffName}.json");

            var caff = new CaffFile()
            {
                File = dto.File,
                Preview = preview,
                UserId = _userProvider.GetUserId(),
                Metadata = json
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
