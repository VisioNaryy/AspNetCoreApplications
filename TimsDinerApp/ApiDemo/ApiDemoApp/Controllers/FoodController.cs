using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiDemoApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodData _foodData;
        private readonly IOrderData _orderData;

        public FoodController(IFoodData foodData, IOrderData orderData)
        {
            _foodData = foodData;
            _orderData = orderData;
        }

        [HttpGet]
        public async Task<List<Food>> Get()
        {
            return await _foodData.GetFood();
        }
    }
}