using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPDemoApp.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly IFoodData _foodData;
        private readonly IOrderData _orderData;

        public List<SelectListItem> FoodItems { get; set; }

        // NOTE: We can modify this object from a view in OnPost() method only
        [BindProperty]
        public Order Order { get; set; }

        public CreateModel(IFoodData foodData, IOrderData orderData)
        {
            _foodData = foodData;
            _orderData = orderData;
        }

        public async Task OnGet()
        {
            var food = await _foodData.GetFood();

            FoodItems = new List<SelectListItem>();

            food.ForEach(x =>
            {
                FoodItems.Add(new SelectListItem { Value = x.Id.ToString(), Text = x.Title });
            });
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            var food = await _foodData.GetFood();

            var foodItem = food.Where(x => x.Id == Order.FoodId).FirstOrDefault();

            if (foodItem == null)
                throw new ApplicationException("There is no such meal in the menu!");

            Order.Total = Order.Quantity * foodItem.Price;

            var id = await _orderData.CreateOrder(Order);

            return RedirectToPage("./Display", new { Id = id });
        }
    }
}
