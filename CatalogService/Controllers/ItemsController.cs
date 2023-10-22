using BusinessLogicLayer;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    /// <Summary>
    /// Category API.
    /// </Summary>
    [Route("api/Items")]
    [ApiController]
    public class ItemsController : Controller
    {
        private readonly IItemService _itemService;

        #pragma warning disable 1591
        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        /// <summary>
        /// Get a list of Items.
        /// </summary>
        /// <returns>A list of Items.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Item>), 200)]
        public IEnumerable<Item> Get() => _itemService.List();

        /// <summary>
        /// Finds an Item by its Id
        /// </summary>
        /// <param name="id">Integer.</param>
        /// <returns>An Item</returns>
        [HttpGet("{id}", Name = "GetItems")]
        public ActionResult<Item> Get(int id)
        {
            var result = _itemService.Get(id);
            if (result != default)
                return Ok(_itemService.Get(id));
            else
                return NotFound();
        }

        /// <summary>
        /// Inserts an Item
        /// </summary>
        /// <returns>The number of affected rows in DB</returns>
        [HttpPost]
        public ActionResult<Item> Insert(Item dto)
        {
            var result = _itemService.Upsert(dto);
            if (result != 0)
                return CreatedAtAction("Insert", _itemService.Get(dto.Id));
            else
                return BadRequest();
        }

        /// <summary>
        /// Updates an Item
        /// </summary>
        /// <returns>The number of affected rows in DB</returns>
        [HttpPut]
        public ActionResult<Item> Update(Item dto)
        {
            var result = _itemService.Upsert(dto);
            if (result > 0)
                return NoContent();
            else
                return NotFound();
        }

        /// <summary>
        /// Deletes an Item by its Id
        /// </summary>
        /// <returns>The number of affected rows in DB</returns>
        [HttpDelete("{id}")]
        public ActionResult<Item> Delete(int id)
        {
            var result = _itemService.Delete(id);
            if (result > 0)
                return NoContent();
            else
                return NotFound();
        }
    }
}
