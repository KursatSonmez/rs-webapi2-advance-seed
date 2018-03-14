using AutoMapper;
using AutoMapper.QueryableExtensions;
using RS.Core.Domain;
using RS.Core.Lib;
using RS.Core.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RS.Core.Service
{
    public interface IFileService
    {
        Task Add(IList<FileDto> modelList, Guid userId, bool isCommit = true);
        Task SendCloud(IList<FileDto> modelList, string localPath);
        Task<IList<FileListDto>> GetList(Guid refId, string screenCode);
        Task<FileDto> GetById(Guid id, Guid? userId = null);
    }
    public class FileService : IFileService
    {
        private EntityUnitofWork<Guid> _uow = null;
        public FileService(EntityUnitofWork<Guid> uow)
        {
            _uow = uow;
        }

        /// <summary>
        /// The process of registering the file information in the database
        /// </summary>
        /// <param name="entityList"></param>
        /// <param name="userId"></param>
        /// <param name="isCommit"></param>
        /// <returns></returns>
        public async Task Add(IList<FileDto> modelList, Guid userId, bool isCommit = true)
        {
            foreach (var model in modelList)
            {
                SysFile entity = Mapper.Map<Domain.SysFile>(model);
                entity.Id = Guid.NewGuid();
                entity.CreateBy = userId;
                entity.CreateDT = DateTime.Now;

                _uow.Repository<SysFile>().Add(entity);
            }

            if (isCommit)
                await _uow.SaveChangesAsync();
        }

        public Task SendCloud(IList<FileDto> modelList,string localPath)
        {
            /// `ftpUserName`, `ftpPassword` and `storagePath` is get from Web.Config.
            string ftpUserName = ConfigurationManager.AppSettings["ftpUserName"];
            string ftpPassword = ConfigurationManager.AppSettings["ftpPassword"];
            string storagePath = ConfigurationManager.AppSettings["fileServiceStoragePath"];

            /// Uploaded files are sent to the cloud server.
            foreach (var model in modelList)
            {
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(storagePath + model.StorageName);

                req.UseBinary = true;
                req.Method = WebRequestMethods.Ftp.UploadFile;
                req.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                byte[] fileData = File.ReadAllBytes(localPath + "\\" + model.StorageName);
                req.ContentLength = fileData.Length;

                Stream reqStream = req.GetRequestStream();
                reqStream.Write(fileData, 0, fileData.Length);
                reqStream.Close();
            }

            return Task.CompletedTask;
        }

        public async Task<IList<FileListDto>> GetList(Guid refId, string screenCode)
        {
            var query = _uow.Repository<SysFile>().Query();

            if (!refId.IsNullOrEmpty())
                query = query.Where(x => x.RefId == refId);
            if (screenCode != null)
                query = query.Where(x => x.ScreenCode == screenCode);

            return await query.ProjectTo<FileListDto>().ToListAsync();
        }

        public async Task<FileDto> GetById(Guid id, Guid? userId = null)
        {
            var query = _uow.Repository<SysFile>().Query()
                .Where(x => x.Id == id);

            if (!userId.IsNullOrEmpty())
                query = query.Where(x => x.CreateBy == userId);

            return await query.ProjectTo<FileDto>().FirstOrDefaultAsync();
        }
    }
}