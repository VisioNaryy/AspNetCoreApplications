using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCDemoApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MVCDemoApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IFoodData _foodData;
        private readonly IOrderData _orderData;

        public OrderController(IFoodData foodData, IOrderData orderData)
        {
            _foodData = foodData;
            _orderData = orderData;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var food = await _foodData.GetFood();

            var model = new OrderCreateModel();

            food.ForEach(x =>
            {
                model.FoodItems.Add(new SelectListItem { Value = x.Id.ToString(), Text = x.Title });
            });

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if(ModelState.IsValid == false)
            {
                return View();
            }

            var food = await _foodData.GetFood();

            var foodItem = food.Where(x => x.Id == order.FoodId).FirstOrDefault();

            if (foodItem != null)
                order.Total = order.Quantity * foodItem.Price;

            var id = await _orderData.CreateOrder(order);

            return RedirectToAction("Display", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Display(int id)
        {
            var orderDisplay = new OrderDisplayModel();

            var order = await _orderData.GetOrderById(id);

            if (order != null)
            {
                orderDisplay.Order = order;

                var food = await _foodData.GetFood();

                orderDisplay.ItemPurchased = food.Where(x => x.Id == orderDisplay.Order.FoodId).FirstOrDefault()?.Title;
            }

            return View(orderDisplay);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, string orderName)
        {
            await _orderData.UpdateOrderName(id, orderName);

            return RedirectToAction("Display", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderData.GetOrderById(id);

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Order order)
        {
            await _orderData.DeleteOrder(order.Id);

            return RedirectToAction("Create");
        }
    }
}