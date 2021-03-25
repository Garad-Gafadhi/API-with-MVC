using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using VOD.Common.DTOModels;
using VOD.Common.Entities;
using VOD.Database.Service;

namespace VOD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        #region Constructor

        public VideosController(ICrudService crudService, LinkGenerator linkGenerator)
        {
            _crudService = crudService;

            _linkGenerator = linkGenerator;
        }

        #endregion

        #region Properties

        private readonly ICrudService _crudService;

        private readonly LinkGenerator _linkGenerator;

        #endregion

        #region Methods

        // GET: api/<VideoController>
        [HttpGet]
        public async Task<IEnumerable<VideoDTO>> GetVideos()
        {
            var videos = await _crudService.GetAsync<Video, VideoDTO>();
            return videos;
        }

        // GET api/<VideoController>/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<VideoDTO>> GetSingleVideo(int id, bool include = false)
        {
            try
            {
                var VideoDTO = await _crudService.GetSingleAsync<Video, VideoDTO>(i => i.Id.Equals(id), include);
                if (VideoDTO == null) return NotFound("Can not find resource");

                return VideoDTO;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<VideoDTO>> PostVideo(VideoDTO model)
        {
            var id = await _crudService.CreateAsync<VideoDTO, Video>(model);
            try
            {
                if (model == null) return BadRequest("No entity provided");
                var exist = await _crudService.AnyAsync<Course>(s => s.Id.Equals(model.CourseId));
                var exist1 = await _crudService.AnyAsync<Module>(m => m.Id.Equals(model.CourseId));
                if (!exist && !exist1) return NotFound("Cant find related id in VideoDTO");

                if (id < 1) return BadRequest("Unable to add the entity");

                var dto = await _crudService.GetSingleAsync<Video, VideoDTO>(s => s.Id.Equals(id), true);
                if (dto == null) return BadRequest("Cannot create entity");

                var uri = _linkGenerator.GetPathByAction("GetSingleVideo", "Videos", new {id});

                return Created(uri, dto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Server Failure!");
            }
        }


        // PUT api/<VideoController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<VideoDTO>> PutVideo(int id, VideoDTO model)
        {
            try
            {
                if (model == null) return BadRequest("No entity provided");
                if (!id.Equals(model.Id)) return BadRequest("Differing Ids");

                var exist = await _crudService.AnyAsync<Course>(s => s.Id.Equals(model.CourseId));
                var exist1 = await _crudService.AnyAsync<Module>(m => m.Id.Equals(model.ModuleId));
                if (!exist1) return NotFound("Cant find related id in VideoDTO");

                exist1 = await _crudService.AnyAsync<Video>(a => a.Id.Equals(id)); //xtra control

                if (await _crudService.UpdateAsync<VideoDTO, Video>(model)) return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Failed to add the entity");
            }

            return BadRequest("Cannot update");
        }

        // DELETE api/<VideoController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<VideoDTO>> DeleteVideo(int id)
        {
            try
            {
                var exist = await _crudService.AnyAsync<Video>(s => s.Id.Equals(id));
                if (!exist) return NotFound("Unable to find the related entity");

                if (await _crudService.DeleteAsync<Video>(i => i.Id.Equals(id))) return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Failed to add the entity");
            }

            return BadRequest("Cannot delete this item!");
        }

        #endregion
    }
}