using ApiDemoApp.Attributes;
using ApiDemoApp.Models;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDemoApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IFoodData _foodData;
        private readonly IOrderData _orderData;

        public OrderController(IFoodData foodData, IOrderData orderData)
        {
            _foodData = foodData;
            _orderData = orderData;
        }

        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(Order order)
        {
            var food = await _foodData.GetFood();

            var foodItem = food.Where(x => x.Id == order.FoodId).FirstOrDefault();

            if (foodItem != null)
                order.Total = order.Quantity * foodItem.Price;

            var id = await _orderData.CreateOrder(order);

            return Ok(id);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var order = await _orderData.GetOrderById(id);

            if (order != null)
            {
                var food = await _foodData.GetFood();

                var result = new
                {
                    Order = order,
                    ItemPurchased = food.Where(x => x.Id == order.FoodId).FirstOrDefault()?.Title
                };

                return Ok(result);
            }
            else
                return NotFound();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] OrderToUpdateDto orderToUpdate)
        {
            await _orderData.UpdateOrderName(orderToUpdate.Id, orderToUpdate.OrderName);

            return Ok();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderData.DeleteOrder(id);

            return Ok();
        }
    }
}