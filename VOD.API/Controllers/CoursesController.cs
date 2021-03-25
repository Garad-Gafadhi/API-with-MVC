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
    public class CoursesController : ControllerBase
    {
        #region Constructor

        public CoursesController(ICrudService crudService, LinkGenerator linkGenerator)
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

        // GET: api/<CourseController>
        [HttpGet]
        public async Task<IEnumerable<CourseDTO>> GetCourses()
        {
            var courses = await _crudService.GetAsync<Course, CourseDTO>();
            return courses;
        }

        // GET api/<CourseController>/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CourseDTO>> GetSingleCourse(int id, bool include = false)
        {
            try
            {
                var CourseDTO = await _crudService.GetSingleAsync<Course, CourseDTO>(i => i.Id.Equals(id), include);
                if (CourseDTO == null) return NotFound("Can not find resource");

                return CourseDTO;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CourseDTO>> PostCourse(CourseDTO model)
        {
            try
            {
                if (model == null) return BadRequest("No entity provided");
                var exist = await _crudService.AnyAsync<Instructor>(s => s.Id.Equals(model.InstructorId));
                if (!exist) return NotFound("Cant find related id in CourseDTO");

                var id = await _crudService.CreateAsync<CourseDTO, Course>(model);
                if (id < 1) return BadRequest("Unable to add the entity");

                var dto = await _crudService.GetSingleAsync<Course, CourseDTO>(s => s.Id.Equals(id), true);
                if (dto == null) return BadRequest("Cannot create entity");

                var uri = _linkGenerator.GetPathByAction("GetSingleCourse", "Courses", new {id});

                return Created(uri, dto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Server Failure!");
            }
        }


        // PUT api/<CourseController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CourseDTO>> PutCourse(int id, CourseDTO model)
        {
            try
            {
                if (model == null) return BadRequest("No entity provided");
                if (!id.Equals(model.Id)) return BadRequest("Differing Ids");

                var exist = await _crudService.AnyAsync<Instructor>(s => s.Id.Equals(model.InstructorId));
                if (!exist) return NotFound("Unable to find the related entity");

                exist = await _crudService.AnyAsync<Course>(a => a.Id.Equals(id));
                if (!exist) return NotFound("Unable to find the related entity");

                if (await _crudService.UpdateAsync<CourseDTO, Course>(model)) return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Failed to add the entity");
            }

            return BadRequest("Cannot update");
        }

        // DELETE api/<CourseController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CourseDTO>> DeleteCourse(int id)
        {
            try
            {
                var exist = await _crudService.AnyAsync<Course>(s => s.Id.Equals(id));
                if (!exist) return NotFound("Unable to find the related entity");

                if (await _crudService.DeleteAsync<Course>(i => i.Id.Equals(id))) return NoContent();
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