using RS.Core.Const;
using RS.Core.Service;
using RS.Core.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;

namespace RS.Core.Controllers
{
    [Authorize]
    [RoutePrefix("api/File")]
    public class FileController : ApiController
    {
        IFileService fileService = null;
        public FileController(IFileService _fileService)
        {
            fileService = _fileService;
        }

        [Route("Upload"), HttpPost]
        public async Task<IHttpActionResult> Upload(Guid refId,string screenCode)
        {
            #region Condition

            /// Checks whether the parameters are null.
            if (refId == null || screenCode == null)
                return BadRequest(Messages.FUE0002);

            /// Controls whether the screen code is in the <see cref="ScreenCodes"/> class.
            if (!typeof(ScreenCodes).GetFields().Any(x => x.Name == screenCode))
                return BadRequest(Messages.GNW0002);

            /// Checks the media type for the file-s to be uploaded.
            if (!Request.Content.IsMimeMultipartContent())
                return Content(HttpStatusCode.UnsupportedMediaType, Messages.FUW0001);
            #endregion

            /// `localPath` and `useCloud` is get from Web.Config.
            string localPath = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["fileServiceLocalPath"]);
            bool useCloud = Convert.ToBoolean(ConfigurationManager.AppSettings["useCloud"]);

            var provider = new MultipartFormDataStreamProvider(localPath);

            try
            {
                /// Loads the files into the local storage.
                await Request.Content.ReadAsMultipartAsync(provider);
                
                /// Check is exist valid file.
                if (provider.FileData.Count == 0)
                    return BadRequest(Messages.FUE0001);

                IList<FileDto> modelList = new List<FileDto>();

                foreach (MultipartFileData file in provider.FileData)
                {
                    string originalName = file.Headers.ContentDisposition.FileName;
                    if (originalName.StartsWith("\"") && originalName.EndsWith("\""))
                    {
                        originalName = originalName.Trim('"');
                    }
                    if (originalName.Contains(@"/") || originalName.Contains(@"\"))
                    {
                        originalName = Path.GetFileName(originalName);
                    }

                    FileDto fileDto = new FileDto
                    {
                        RefId = refId,
                        ScreenCode = screenCode,
                        OriginalName = Path.GetFileNameWithoutExtension(originalName),
                        StorageName = Path.GetFileName(file.LocalFileName),
                        Extension = Path.GetExtension(originalName).ToLower(),
                        Size = new FileInfo(file.LocalFileName).Length
                    };

                    modelList.Add(fileDto);
                }

                if (useCloud)
                    await fileService.SendCloud(modelList,localPath);

                await fileService.Add(modelList, IdentityClaimsValues.UserId<Guid>());

                return Ok(Messages.Ok);
            }
            catch (Exception exMessage)
            {
                return Content(HttpStatusCode.InternalServerError, exMessage);
            }
        }

        [Route("Download"), HttpGet]
        public async Task<IHttpActionResult> Download(Guid id)
        {
            var model = await fileService.GetById(id);

            if (model == null)
                return BadRequest(Messages.GNE0001);

            /// `localPath` is get from Web.Config.
            string localPath = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["fileServiceLocalPath"]);

            string root = localPath + "\\" + model.StorageName;

            byte[] fileData = File.ReadAllBytes(root);
            var stream = new MemoryStream(fileData, 0, fileData.Length);

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(stream.ToArray())
            };

            response.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment")
                {
                    FileName = model.OriginalName + "." + model.Extension,
                    Size=model.Size
                };

            response.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            IHttpActionResult result = ResponseMessage(response);
            return result;
        }

        [Route("Get"), HttpGet]
        [ResponseType(typeof(IEnumerable<FileListDto>))]
        public async Task<IHttpActionResult> GetList(Guid refId, string screenCode)
        {
            /// Controls whether the screen code is in the <see cref="ScreenCodes"/> class.
            if (!typeof(ScreenCodes).GetFields().Any(x => x.Name == screenCode))
                return BadRequest(Messages.GNW0002);

            var result = await fileService.GetList(refId,screenCode);

            if (result == null || result.Count <= 0)
                result = new List<FileListDto>();

            return Ok(result);
        }

    }
}