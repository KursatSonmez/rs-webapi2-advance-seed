using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        Task Add(IList<FileDto> modelList, Guid userID, bool isCommit = true);
        Task SendCloud(IList<FileDto> modelList, string localPath);
        Task<IList<FileListDto>> GetList(Guid refID, string screenCode);
        Task<FileDto> GetByID(Guid id, Guid? userID = null);
    }
    public class FileService : IFileService
    {
        private EntityUnitofWork<Guid> uow = null;
        public FileService(EntityUnitofWork<Guid> _uow)
        {
            uow = _uow;
        }

        /// <summary>
        /// The process of registering the file information in the database
        /// </summary>
        /// <param name="entityList"></param>
        /// <param name="userID"></param>
        /// <param name="isCommit"></param>
        /// <returns></returns>
        public async Task Add(IList<FileDto> modelList, Guid userID, bool isCommit = true)
        {
            foreach (var model in modelList)
            {
                Domain.File entity = Mapper.Map<Domain.File>(model);
                entity.ID = Guid.NewGuid();
                entity.CreateBy = userID;
                entity.CreateDT = DateTime.Now;

                uow.Repository<Domain.File>().Add(entity);
            }

            if (isCommit)
                await uow.SaveChangesAsync();
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

        public async Task<IList<FileListDto>> GetList(Guid refID, string screenCode)
        {
            var query = uow.Repository<Domain.File>().Query();

            if (!refID.IsNullOrEmpty())
                query = query.Where(x => x.RefID == refID);
            if (screenCode != null)
                query = query.Where(x => x.ScreenCode==screenCode);

            return await query.ProjectTo<FileListDto>().ToListAsync();
        }

        public async Task<FileDto> GetByID(Guid id, Guid? userID = null)
        {
            var query = uow.Repository<Domain.File>().Query().Where(x => x.ID == id);

            if (!userID.IsNullOrEmpty())
                query = query.Where(x => x.CreateBy == userID);

            return await query.ProjectTo<FileDto>().FirstOrDefaultAsync();
        }
    }
}