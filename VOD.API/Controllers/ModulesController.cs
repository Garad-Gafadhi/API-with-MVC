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
    public class ModulesController : ControllerBase
    {
        #region Constructor

        public ModulesController(ICrudService crudService, LinkGenerator linkGenerator)
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

        // GET: api/<ModulesController>
        [HttpGet]
        public async Task<IEnumerable<ModuleDTO>> GetModules()
        {
            var modules = await _crudService.GetAsync<Module, ModuleDTO>();
            return modules;
        }

        // GET api/<ModuleController>/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ModuleDTO>> GetSingleModule(int id, bool include = false)
        {
            try
            {
                var ModuleDTO = await _crudService.GetSingleAsync<Module, ModuleDTO>(i => i.Id.Equals(id), include);
                if (ModuleDTO == null) return NotFound("Can not find resource");

                return ModuleDTO;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ModuleDTO>> PostModule(ModuleDTO model)
        {
            try
            {
                if (model == null) return BadRequest("No entity provided");


                var id = await _crudService.CreateAsync<ModuleDTO, Module>(model);
                if (id < 1) return BadRequest("Unable to add the entity");

                var dto = await _crudService.GetSingleAsync<Module, ModuleDTO>(s => s.Id.Equals(id), true);
                if (dto == null) return BadRequest("Cannot create entity");

                var uri = _linkGenerator.GetPathByAction("GetSingleModule", "Modules", new {id});

                return Created(uri, dto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Server Failure!");
            }
        }


        // PUT api/<ModuleController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ModuleDTO>> PutModule(int id, ModuleDTO model)
        {
            try
            {
                if (model == null) return BadRequest("No entity provided");
                if (!id.Equals(model.Id)) return BadRequest("Differing Ids");

                var exist1 = await _crudService.AnyAsync<Course>(c => c.Id.Equals(model.CourseId));
                if (!exist1) return NotFound("Cant find related id in VideoDTO");

                if (await _crudService.UpdateAsync<ModuleDTO, Module>(model)) return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Failed to add the entity");
            }

            return BadRequest("Cannot update");
        }

        // DELETE api/<ModuleController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ModuleDTO>> DeleteModule(int id)
        {
            try
            {
                var exist = await _crudService.AnyAsync<Module>(s => s.Id.Equals(id));
                if (!exist) return NotFound("Unable to find the related entity");

                if (await _crudService.DeleteAsync<Module>(i => i.Id.Equals(id))) return NoContent();
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