using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RPDemoApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RPDemoApp.Pages.Orders
{
    public class DisplayModel : PageModel
    {
        private readonly IFoodData _foodData;
        private readonly IOrderData _orderData;

        // NOTE: We can modify and use this property from a view in both OnGet() and OnPost() methods
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public OrderToUpdate OrderToUpdate { get; set; }

        public Order Order { get; set; }

        public string ItemPurchased { get; set; }

        public DisplayModel(IFoodData foodData, IOrderData orderData)
        {
            _foodData = foodData;
            _orderData = orderData;
        }

        public async Task<IActionResult> OnGet()
        {
            Order = await _orderData.GetOrderById(Id);

            if (Order != null)
            {
                var food = await _foodData.GetFood();

                ItemPurchased = food.Where(x => x.Id == Order.FoodId).FirstOrDefault()?.Title;
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            await _orderData.UpdateOrderName(OrderToUpdate.Id, OrderToUpdate.OrderName);

            return RedirectToPage("./Display", new { OrderToUpdate.Id });
        }
    }
}