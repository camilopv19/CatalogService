using BusinessLogicLayer.CoreLogic;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    /// <Summary>
    /// Categories API.
    /// </Summary>
    [Route("api/Categories")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

#pragma warning disable 1591
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get a list of Categories. Allowed roles: Manager, Buyer
        /// </summary>
        /// <returns>A list of Categories.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), 200)]
        //[Authorize]
        [Authorize(Roles ="Manager, Buyer")]
        public ActionResult<Category> Get() => Ok(_categoryService.List());

        /// <summary>
        /// Finds a Category by its Id. Allowed roles: Manager, Buyer
        /// </summary>
        /// <param name="id">Number</param>
        /// <returns>An Categories</returns>
        [HttpGet("{id}", Name = "GetCategory")]
        [Authorize(Roles = "Manager, Buyer")]
        public ActionResult<Category> Get(int id)
        {
            var result = _categoryService.Get(id);
            if (result != default)
                return Ok(_categoryService.Get(id));
            else
                return NotFound();
        }

        /// <summary>
        /// Inserts a Category. Only Manager
        /// </summary>
        /// <returns>The number of affected rows in DB</returns>
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult<Category> Insert(Category dto)
        {
            var result = _categoryService.Upsert(dto);
            if (result != 0)
                return CreatedAtAction("Insert", _categoryService.Get(dto.Id));
            else
                return BadRequest();
        }

        /// <summary>
        /// Updates a Category. Only Manager
        /// </summary>
        /// <returns>The number of affected rows in DB</returns>
        [HttpPut]
        [Authorize(Roles = "Manager")]
        public ActionResult<Category> Update(Category dto)
        {
            var result = _categoryService.Upsert(dto);
            if (result > 0)
                return NoContent();
            else
                return NotFound();
        }

        /// <summary>
        /// Deletes a Category by its Id. Only Manager
        /// </summary>
        /// <returns>The number of affected rows in DB</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public ActionResult<Category> Delete(int id)
        {
            var result = _categoryService.Delete(id);
            if (result > 0)
                return NoContent();
            else
                return NotFound();
        }
    }
}
