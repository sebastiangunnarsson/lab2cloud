using System;
using System.Diagnostics;
using System.IdentityModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Lab2.Helpers;
using Lab2.Models;

namespace Lab2.Controllers.V1
{
    [RoutePrefix("api/v1/vacations/{vacationId:int}/memories")]
    [Caching]
    public class MemoriesController : BaseController
    {

        private const string _accessKey = "AKIAJ3VSY5KQ4AWM4IIQ", _secret = "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";

        #region https://server/api/v1/vacations/12/memories

        //GET - Retrieves a list of memories for vacation #12
        [Route("")]
        public async Task<IHttpActionResult> Get(int vacationId)
        {
            var vacation = await _ctx.Vacations.FindAsync(vacationId);

            if (vacation == null)
                return BadRequest("Invalid vacationid");

            return Ok(vacation.Memories.Select(x => new
            {
                x.Description,
                x.Date,
                x.Position,
                TaggedUsers = x.TaggedUsers.Select(u => new
                {
                    u.Id,
                    u.Firstname,
                    u.Lastname
                }).ToList(),
                SearchTags = x.SearchTags.Select(t => t.Tag).ToList(),
                x.Id,
            }));
        }

        //POST - Creates a new memory for vacation #12
        [Route("")]
        public async Task<IHttpActionResult> Post(int vacationId, MemoryDTO model)
        {
            var vac = await _ctx.Vacations.FindAsync(vacationId);

            if (vac == null)
                return BadRequest("Invalid vacationid");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await AddTags(model);
            vac.Memories.Add(new Memory()
            {
                Date = model.Date.Value,
                Description = model.Description,
                Position = model.Position,
                TaggedUsers =
                    _ctx.Users.Where(u => model.TaggedUsers.Contains(u.UserName)).Select(x => x.VacationUser).ToList(),
                SearchTags = _ctx.SearchTags.Where(x => model.SearchTags.Contains(x.Tag)).ToList()
            });

            await _ctx.SaveChangesAsync();

            return Ok();
        }

        #endregion

        #region https://server/api/v1/vacations/12/memories/5



        //GET - Retrieve memory #5 for vacation #12
        [Route("{memoryId:int}")]
        public async Task<IHttpActionResult> Get(int vacationId, int memoryId)
        {
            var vac = await _ctx.Vacations.FindAsync(vacationId);

            if (vac == null)
                return BadRequest("Invalid vacationid");

            var mem = vac.Memories.FirstOrDefault(x => x.Id == memoryId);

            if (mem == null)
                return BadRequest("Invalid memoryid");

            return Ok(new
            {
                mem.Description,
                mem.Date,
                mem.Position,
                TaggedUsers = mem.TaggedUsers.Select(u => new
                {
                    u.Id,
                    u.Firstname,
                    u.Lastname
                }).ToList(),
                SearchTags = mem.SearchTags.Select(t => t.Tag).ToList(),
                mem.Id,
            });
        }

        //PUT - Updates memory #5 for vacation #12
        [Route("{memoryId:int}")]
        public async Task<IHttpActionResult> Put(int vacationId, int memoryId, MemoryDTO model)
        {
            var vac = await _ctx.Vacations.FindAsync(vacationId);

            if (vac == null)
                return BadRequest("Invalid vacationid");

            var mem = vac.Memories.FirstOrDefault(x => x.Id == memoryId);

            if (mem == null)
                return BadRequest("Invalid memoryid");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await AddTags(model);

            mem.Date = model.Date.Value;
            mem.Description = model.Description;
            mem.Position = model.Position;
            mem.TaggedUsers.Clear();
            mem.TaggedUsers =
                _ctx.Users.Where(u => model.TaggedUsers.Contains(u.UserName)).Select(x => x.VacationUser).ToList();

            mem.SearchTags.Clear();
            mem.SearchTags = _ctx.SearchTags.Where(t => model.SearchTags.Contains(t.Tag)).ToList();

            await _ctx.SaveChangesAsync();

            return Ok();
        }

        //PATCH - Partially updates memory #5 for vacation #12
        [Route("{memoryId:int}")]
        public async Task<IHttpActionResult> Patch(int vacationId, int memoryId, MemoryDTO model)
        {
            var vac = await _ctx.Vacations.FindAsync(vacationId);

            if (vac == null)
                return BadRequest("Invalid vacationid");

            var mem = vac.Memories.FirstOrDefault(x => x.Id == memoryId);

            if (mem == null)
                return BadRequest("Invalid memoryid");


            if (model.Date.HasValue)
                mem.Date = model.Date.Value;
            if (!string.IsNullOrEmpty(model.Description))
                mem.Description = model.Description;
            if (model.Position != null)
                mem.Position = model.Position;
            if (model.TaggedUsers != null)
            {
                mem.TaggedUsers.Clear();
                mem.TaggedUsers =
                    _ctx.Users.Where(u => model.TaggedUsers.Contains(u.UserName)).Select(x => x.VacationUser).ToList();
            }
            if (model.SearchTags != null)
            {
                await AddTags(model);
                mem.SearchTags.Clear();
                mem.SearchTags = _ctx.SearchTags.Where(t => model.SearchTags.Contains(t.Tag)).ToList();
            }

            await _ctx.SaveChangesAsync();

            return Ok();
        }

        private async Task AddTags(MemoryDTO model)
        {
            foreach (var tag in model.SearchTags)
            {
                if (!_ctx.SearchTags.Any(x => x.Tag == tag))
                    _ctx.SearchTags.Add(new SearchTag()
                    {
                        Tag = tag
                    });
            }
            await _ctx.SaveChangesAsync();
        }

        //DELETE - Deletes memory #5 for vacation #12
        [Route("{memoryId:int}")]
        public async Task<IHttpActionResult> Delete(int vacationId, int memoryId)
        {
            var vac = await _ctx.Vacations.FindAsync(vacationId);

            if (vac == null)
                return BadRequest("Invalid vacationid");

            var mem = vac.Memories.FirstOrDefault(x => x.Id == memoryId);

            if (mem == null)
                return BadRequest("Invalid memoryid");

            vac.Memories.Remove(mem);

            await _ctx.SaveChangesAsync();
            return Ok();
        }

        #endregion


        #region https://server/api/v1/vacations/12/memories/5/media

        //POST - Upload media to server
        [Route("{memoryId:int}/media")]
        [HttpPost]
        public async Task<IHttpActionResult> Upload(int vacationId, int memoryId)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }


            var vac = await _ctx.Vacations.FindAsync(vacationId);

            if (vac == null)
                return BadRequest("Invalid vacationid");

            var mem = vac.Memories.FirstOrDefault(x => x.Id == memoryId);

            if (mem == null)
                return BadRequest("Invalid memoryid");


            if (mem.Media != null)
                mem.Media = null;

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            await Request.Content.ReadAsMultipartAsync(provider);

            foreach (var file in provider.FileData)
            {
                var filename = file.Headers.ContentDisposition.FileName.Replace("\"", "");

                var putRequest = new PutObjectRequest
                {
                    BucketName = "cloudlabsg-media",
                    Key = Guid.NewGuid().ToString() + "_" + filename
                };

                using (var s3 = new AmazonS3Client(_accessKey, _secret, RegionEndpoint.EUWest1))
                using (var fs = new FileStream(file.LocalFileName, FileMode.Open))
                {
                    putRequest.InputStream = fs;
                    var s3Response = await s3.PutObjectAsync(putRequest);
                }


                Media media = null;

                MediaToolkit.Model.MediaFile f = new MediaToolkit.Model.MediaFile(file.LocalFileName);
                using (MediaToolkit.Engine engine = new MediaToolkit.Engine())
                {
                    engine.GetMetadata(f);
                }

                switch (file.Headers.ContentType.MediaType.Split('/')[0])
                {
                    case "video":
                        media = new Video()
                        {
                            Duration = f.Metadata.Duration.TotalSeconds,
                            VideoBitrate = (double)f.Metadata.VideoData.BitRateKbs.GetValueOrDefault(),
                            AudioBitrate = f.Metadata.AudioData.BitRateKbs,
                            FrameRate = f.Metadata.VideoData.Fps,
                            SamplingRate = f.Metadata.AudioData.SampleRate,
                            Memory = mem,
                        };
                        break;


                    case "audio":
                        media = new Sound()
                        {
                            Bitrate = f.Metadata.AudioData.BitRateKbs,
                            Duration = f.Metadata.Duration.TotalSeconds,
                            SamplingRate = f.Metadata.AudioData.SampleRate,
                        };
                        break;

                    case "image":
                        using (var pic = System.Drawing.Image.FromFile(file.LocalFileName))
                        {
                            media = new Picture()
                            {
                                Height = pic.Height,
                                Width = pic.Width,
                            };
                        }
                        break;

                    default:
                        break;
                }

                media.Url = putRequest.Key;
                media.Type = file.Headers.ContentType.MediaType;
                mem.Media = media;

                System.IO.File.Delete(file.LocalFileName);
                CacheHelper.Delete(putRequest.Key);
            }

            await _ctx.SaveChangesAsync();
            return Ok();
        }


        //GET - Download the media from the server
        [Route("{memoryId:int}/media")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Download(int vacationId, int memoryId)
        {

            var vac = await _ctx.Vacations.FindAsync(vacationId);

            if (vac == null)
                throw new BadRequestException();

            var mem = vac.Memories.FirstOrDefault(x => x.Id == memoryId);

            if (mem == null)
                throw new BadRequestException();

            var getRequest = new GetObjectRequest
            {
                BucketName = "cloudlabsg-media",
                Key = mem.Media.Url
            };

            MemoryStream stream;
            byte[] data = CacheHelper.Get<byte[]>(getRequest.Key);
            if (data == null)
            {
                using (var s3 = new AmazonS3Client(_accessKey, _secret, RegionEndpoint.EUWest1))
                {
                    using (var response =(await s3.GetObjectAsync(getRequest)).ResponseStream)
                    {
                        stream = new MemoryStream();
                        await response.CopyToAsync(stream);
                    }

                }
                
            }
            else
            {
                stream = new MemoryStream(data);
                
            }

            stream.Seek(0, SeekOrigin.Begin);

            HttpResponseMessage msg = new HttpResponseMessage(HttpStatusCode.OK);
            HttpContext.Current.Response.AppendHeader("content-disposition", String.Format("attachment;FileName=\"{0}\"", getRequest.Key));
            msg.Content = new StreamContent(stream);
            msg.Content.Headers.ContentType = new MediaTypeHeaderValue(mem.Media.Type);

            CacheHelper.Save(getRequest.Key, stream.ToArray());

            return ResponseMessage(msg);

        }
        #endregion

    }
}