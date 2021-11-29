using CAFFAdapterClient.DataTransferObjects.CaffFiles;
using CAFFAdapterClient.DataTransferObjects.Comment;
using CAFFAdapterClient.Domain;
using CAFFAdapterClient.Framework.Providers;
using CAFFAdapterClient.Infrastructure.Data;
using CAFFAdapterClient.Infrastructure.Exceptions;
using CAFFAdapterClient.ViewModels;
using CAFFAdapterClient.ViewModels.CaffFiles;
using CAFFAdapterClient.ViewModels.Comments;
using CAFFAdapterClient.ViewModels.GifFiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
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
                CreatedAt = caff.CreatedAt,
                Metadata = caff.Metadata,
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
                throw new BusinessLogicException("Can not remove CAFF file!");
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
                    throw new BusinessLogicException("Can not parse the CAFF file!");
                }
            }

            // var preview = new System.Net.WebClient().DownloadData("https://c.tenor.com/eFPFHSN4rJ8AAAAd/example.gif");
            var preview = await File.ReadAllBytesAsync($"{outputPath}/{caffName}.gif");
            var json = await File.ReadAllTextAsync($"{outputPath}/{caffName}.json");

            var caff = new CaffFile()
            {
                File = dto.File,
                Description = dto.Description,
                Preview = preview,
                UserId = _userProvider.GetUserId(),
                Metadata = json,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.CaffFiles.Add(caff);

            await _dbContext.SaveChangesAsync();

            return caff.Id;
        }

        public async Task<byte[]> PreviewAsync(CreateCaffFileDto dto)
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
                    throw new BusinessLogicException("Can not parse the CAFF file!");
                }
            }

            // var preview = new System.Net.WebClient().DownloadData("https://c.tenor.com/eFPFHSN4rJ8AAAAd/example.gif");
            return await File.ReadAllBytesAsync($"{outputPath}/{caffName}.gif");
        }

        public async Task UpdateAsync(int id, UpdateCaffFileDto dto)
        {
            var caff = await _dbContext.CaffFiles.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new DataNotFoundException();

            caff.Description = dto.Description;

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
                throw new BusinessLogicException("Can not remove the comment!");
            }

            comment.IsDeleted = true;

            _dbContext.Comments.Update(comment);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<TableViewModel<GifViewModel>> getGifs(bool byUserId)
        {
            List<CaffFile> caffFiles = null;
            if (byUserId)
            {
                caffFiles = _dbContext.CaffFiles
                .Where(x => x.UserId == _userProvider.GetUserId())                
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
            } else
            {
                caffFiles = _dbContext.CaffFiles               
               .OrderByDescending(x => x.CreatedAt)
               .ToList();
            }
           
            var items = new List<GifViewModel>();

            foreach (var caffFile in caffFiles)
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == caffFile.UserId);

                var newGifViewModel = new GifViewModel();
                newGifViewModel.Id = caffFile.Id;
                newGifViewModel.Username = user.FirstName + " " + user.LastName;
                newGifViewModel.Description = caffFile.Description;
                newGifViewModel.Base64Encode = "data:image/gif;base64," + Convert.ToBase64String(caffFile.Preview);
                newGifViewModel.Metadata = caffFile.Metadata;
                items.Add(newGifViewModel);
            }

            var tableViewModel = new TableViewModel<GifViewModel>();
            tableViewModel.Count = items.Count();
            tableViewModel.Items = items;

            return tableViewModel;
        }

        public async Task<TableViewModel<CommentByGifViewModel>> getCommentsByGifId(int gifId)
        {
            var comments = _dbContext.Comments
                .Where(x => x.CaffId == gifId && x.IsDeleted == false)
                .Include(x => x.User)
                .OrderBy(x => x.CreatedAt)
                .ToList();

            Console.WriteLine("COUNT: " + comments.Count());

            var items = new List<CommentByGifViewModel>();

            foreach (var comment in comments)
            {
                var newCommentViewModel = new CommentByGifViewModel();
                newCommentViewModel.CommentId = comment.Id;
                newCommentViewModel.UserId = comment.UserId;
                newCommentViewModel.Username = comment.User.FirstName + ' ' + comment.User.LastName;
                newCommentViewModel.Comment = comment.Message;
                items.Add(newCommentViewModel);
            }

            var tableViewModel = new TableViewModel<CommentByGifViewModel>();
            tableViewModel.Count = items.Count();
            tableViewModel.Items = items;

            return tableViewModel;
        }

        public async Task<int> createCaffForSeed(CreateCaffSeedDto createCaffSeedDto)
        {
            var caff = new CaffFile()
            {
                File = createCaffSeedDto.Caff,
                Preview = createCaffSeedDto.Gif,
                UserId = createCaffSeedDto.UserId,
                Description = createCaffSeedDto.Description,
                Metadata = createCaffSeedDto.Json
            };

            await _dbContext.CaffFiles.AddAsync(caff);
            await _dbContext.SaveChangesAsync();

            return caff.Id;
        }

        public async Task addCommentForSeed(CommentSeedDto commentSeedDto)
        {
            var caff = await _dbContext.CaffFiles.FirstOrDefaultAsync(x => x.Id == commentSeedDto.CaffId)
                ?? throw new DataNotFoundException();

            await _dbContext.Comments.AddAsync(new Comment()
            {
                CreatedAt = DateTime.UtcNow,
                UserId = commentSeedDto.UserId,
                Message = commentSeedDto.Message,
                CaffId = commentSeedDto.CaffId
            });

            await _dbContext.SaveChangesAsync();
        }
    }
}
