using BusinessLogicLayer.CoreLogic;
using DataAccessLayer.Entities;
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
        public CategoryController(ICategoryService cartService)
        {
            _categoryService = cartService;
        }

        /// <summary>
        /// Get a list of Categories.
        /// </summary>
        /// <returns>A list of Categories.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), 200)]
        public IEnumerable<Category> Get() => _categoryService.List();

        /// <summary>
        /// Finds a Category by its Id
        /// </summary>
        /// <param name="id">Number</param>
        /// <returns>An Categories</returns>
        [HttpGet("{id}", Name = "GetCategory")]
        public ActionResult<Category> Get(int id)
        {
            var result = _categoryService.Get(id);
            if (result != default)
                return Ok(_categoryService.Get(id));
            else
                return NotFound();
        }

        /// <summary>
        /// Inserts a Category
        /// </summary>
        /// <returns>The number of affected rows in DB</returns>
        [HttpPost]
        public ActionResult<Category> Insert(Category dto)
        {
            var result = _categoryService.Upsert(dto);
            if (result != 0)
                return CreatedAtAction("Insert", _categoryService.Get(dto.Id));
            else
                return BadRequest();
        }

        /// <summary>
        /// Updates a Category
        /// </summary>
        /// <returns>The number of affected rows in DB</returns>
        [HttpPut]
        public ActionResult<Category> Update(Category dto)
        {
            var result = _categoryService.Upsert(dto);
            if (result > 0)
                return NoContent();
            else
                return NotFound();
        }

        /// <summary>
        /// Deletes a Category by its Id
        /// </summary>
        /// <returns>The number of affected rows in DB</returns>
        [HttpDelete("{id}")]
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
