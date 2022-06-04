using DataLibrary.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCDemoApp.Controllers
{
    public class FoodController : Controller
    {
        private readonly IFoodData _foodData;
        private readonly IOrderData _orderData;

        public FoodController(IFoodData foodData, IOrderData orderData)
        {
            _foodData = foodData;
            _orderData = orderData;
        }

        public async Task<IActionResult> Index()
        {
            var food = await _foodData.GetFood();

            return View(food);
        }
    }
}