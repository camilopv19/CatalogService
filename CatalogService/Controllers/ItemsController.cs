using BusinessLogicLayer;
using BusinessLogicLayer.CoreLogic;
using BusinessLogicLayer.Messaging;
using DataAccessLayer.Entities;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    /// <Summary>
    /// Items API.
    /// </Summary>
    [Route("api/Items")]
    [ApiController]
    public class ItemsController : Controller
    {
        private readonly IItemService _itemService;
        private readonly TelemetryClient telemetryClient;

#pragma warning disable 1591
        public ItemsController(IItemService itemService, TelemetryClient telemetryClient)
        {
            _itemService = itemService;
            this.telemetryClient = telemetryClient;
        }

        /// <summary>
        /// Get a default, hardcoded Item by dictionary search
        /// </summary>
        /// <returns>Same Item</returns>
        [HttpGet("GetItem")]
        public ActionResult<Item> GetItem([FromQuery] Dictionary<string, string> queryParams)
        {
            // Hardcoded response for reference
            var response = new Item(){
                Name = $"{queryParams.First().Key} = {queryParams.First().Value}",
                Description = $"{queryParams.ElementAt(1).Key} = {queryParams.ElementAt(1).Value}",
                Image = $"{string.Join(", ", queryParams.Keys)}",
                CategoryId = 0,
                Price = 99.9m,
                Amount = 10
            };
            telemetryClient.TrackEvent("GetMockItem");
            return Ok(response);
        }

        /// <summary>
        /// Get a list of all Items. Allowed roles: Manager, Buyer
        /// </summary>
        /// <returns>A list of all Items.</returns>
        [HttpGet("all")] // You can customize the route as needed
        [ProducesResponseType(typeof(IEnumerable<Item>), 200)]
        [Authorize(Roles = "Manager, Buyer")]
        public ActionResult<ItemResponse> GetAll()
        {
            var items = _itemService.List();
            if (items != null)
            {
                telemetryClient.TrackEvent("GetAllItems");
                return Ok(items);
            }
            else
                return NotFound();
        }

        /// <summary>
        /// Get a list of Items. 
        /// CategoryId as optional filter.
        /// Page as optional pagination (20 items per page).
        /// Allowed roles: Manager, Buyer
        /// </summary>
        /// <returns>A list of Items.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Item>), 200)]
        [Authorize(Roles = "Manager, Buyer")]
        public ActionResult<ItemResponse> Get(int? categoryId, int? page)
        {
            var items = _itemService.List(categoryId, page);
            if (items != null)
            {
                telemetryClient.TrackEvent("Get Item by CategoryId, paginated");
                return Ok(items);
            }
            else
                return NotFound();
        }

        /// <summary>
        /// Finds an Item by its Id. Allowed roles: Manager, Buyer
        /// </summary>
        /// <param name="id">Integer.</param>
        /// <returns>An Item</returns>
        [HttpGet("{id}", Name = "GetItems")]
        [Authorize(Roles = "Manager, Buyer")]
        public ActionResult<Item> Get(int id)
        {
            var result = _itemService.Get(id);
            if (result != default)
            {
                telemetryClient.TrackEvent("Get Item by Id");
                return Ok(_itemService.Get(id));
            }
            else
                return NotFound();
        }

        /// <summary>
        /// Inserts an Item. Only Manager
        /// </summary>
        /// <returns>The number of affected rows in DB</returns>
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult<Item> Insert(Item dto)
        {
            var result = _itemService.Upsert(dto);
            if (result != 0)
            {
                telemetryClient.TrackEvent("Insert item");
                return CreatedAtAction("Insert", _itemService.Get(dto.Id));
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// Updates an Item. Only Manager
        /// </summary>
        /// <returns>The number of affected rows in DB</returns>
        [HttpPut]
        [Authorize(Roles = "Manager")]
        public ActionResult<string> Update(Item dto)
        {
            var result = _itemService.Upsert(dto);
            if (result > 0)
            {
                telemetryClient.TrackEvent("Update item");
                var msgBroker = new MessageService();
                msgBroker.Publish(dto);
                return msgBroker.Publish(dto);
            }
            else
                return NotFound();
        }

        /// <summary>
        /// Deletes an Item by its Id. Only Manager
        /// </summary>
        /// <returns>The number of affected rows in DB</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public ActionResult<Item> Delete(int id)
        {
            var result = _itemService.Delete(id);
            if (result > 0)
            {
                telemetryClient.TrackEvent("Delete item");
                return NoContent();
            }
            else
                return NotFound();
        }
    }
}
