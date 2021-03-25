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
    public class InstructorsController : ControllerBase
    {
        #region Constructor

        public InstructorsController(ICrudService crudService, LinkGenerator linkGenerator)
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

        // GET: api/<InstructorsController>
        [HttpGet]
        public async Task<IEnumerable<InstructorDTO>> GetInstructors()
        {
            var instructors = await _crudService.GetAsync<Instructor, InstructorDTO>();
            return instructors;
        }

        // GET api/<InstructorsController>/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<InstructorDTO>> GetSingleInstructor(int id, bool include = false)
        {
            try
            {
                var InstructorDTO = await _crudService.GetSingleAsync<Instructor, InstructorDTO>(i => i.Id.Equals(id));
                if (InstructorDTO == null) return NotFound("Can not find resource");

                return InstructorDTO;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<InstructorDTO>> PostInstructor(InstructorDTO model)
        {
            try
            {
                if (model == null) return BadRequest("No entity provided");

                var id = await _crudService.CreateAsync<InstructorDTO, Instructor>(model);
                if (id < 1) return BadRequest("Unable to add the entity");

                var dto = await _crudService.GetSingleAsync<Instructor, InstructorDTO>(s => s.Id.Equals(id), true);
                if (dto == null) return BadRequest("Cannot create entity");

                var uri = _linkGenerator.GetPathByAction("GetSingleInstructor", "Instructors", new {id});

                return Created(uri, dto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Server Failure!");
            }
        }


        // PUT api/<InstructorsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<InstructorDTO>> PutInstructor(int id, InstructorDTO model)
        {
            try
            {
                if (model == null) return BadRequest("No entity provided");
                if (!id.Equals(model.Id)) return BadRequest("Differing Ids");

                if (await _crudService.UpdateAsync<InstructorDTO, Instructor>(model)) return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Failed to add the entity");
            }

            return BadRequest("Cannot update");
        }

        // DELETE api/<InstructorsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<InstructorDTO>> DeleteInstructor(int id)
        {
            try
            {
                var exist = await _crudService.AnyAsync<Instructor>(s => s.Id.Equals(id));
                if (!exist) return NotFound("Unable to find the related entity");

                if (await _crudService.DeleteAsync<Instructor>(i => i.Id.Equals(id))) return NoContent();
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