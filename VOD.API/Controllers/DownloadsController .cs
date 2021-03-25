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
    public class DownloadsController : ControllerBase
    {
        #region Constructor

        public DownloadsController(ICrudService crudService, LinkGenerator linkGenerator)
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

        // GET: api/<DownloadController>
        [HttpGet]
        public async Task<IEnumerable<DownloadDTO>> GetDownloads()
        {
            var Downloads = await _crudService.GetAsync<Download, DownloadDTO>();
            return Downloads;
        }

        // GET api/<DownloadController>/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DownloadDTO>> GetSingleDownload(int id, bool include = false)
        {
            try
            {
                var DownloadDTO = await _crudService.GetSingleAsync<Download, DownloadDTO>(i => i.Id.Equals(id));
                if (DownloadDTO == null) return NotFound("Can not find resource");

                return DownloadDTO;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<DownloadDTO>> PostDownload(DownloadDTO model)
        {
            try
            {
                if (model == null) return BadRequest("No entity provided");

                var exist = await _crudService.AnyAsync<Course>(c => c.Id.Equals(model.CourseId));
                var exist1 = await _crudService.AnyAsync<Module>(m => m.Id.Equals(model.ModuleId));
                if (!exist && !exist1) return NotFound("Cant find related id in VideoDTO");

                var id = await _crudService.CreateAsync<DownloadDTO, Download>(model);
                if (id < 1) return BadRequest("Unable to add the entity");

                var Dto = await _crudService.GetSingleAsync<Download, DownloadDTO>(s => s.Id.Equals(id), true);
                if (Dto == null) return BadRequest("Cannot create entity");

                var uri = _linkGenerator.GetPathByAction("GetSingleDownload", "Downloads", new {id});

                return Created(uri, Dto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Server Failure!");
            }
        }


        // PUT api/<DownloadController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<DownloadDTO>> PutDownload(int id, DownloadDTO model)
        {
            try
            {
                if (model == null) return BadRequest("No entity provided");
                if (!id.Equals(model.Id)) return BadRequest("Differing Ids");
                var exist = await _crudService.AnyAsync<Course>(c => c.Id.Equals(model.CourseId));
                var exist1 = await _crudService.AnyAsync<Module>(m => m.Id.Equals(model.ModuleId));
                if (!exist && !exist1) return NotFound("Cant find related id in VideoDTO");
                if (await _crudService.UpdateAsync<DownloadDTO, Download>(model)) return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Failed to add the entity");
            }

            return BadRequest("Cannot update");
        }

        // DELETE api/<DownloadController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DownloadDTO>> DeleteDownload(int id)
        {
            try
            {
                var exist = await _crudService.AnyAsync<Download>(s => s.Id.Equals(id));
                if (!exist) return NotFound("Unable to find the related entity");

                if (await _crudService.DeleteAsync<Download>(i => i.Id.Equals(id))) return NoContent();
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