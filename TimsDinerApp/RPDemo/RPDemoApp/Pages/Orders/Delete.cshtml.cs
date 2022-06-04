using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RPDemoApp.Pages.Orders
{
    public class DeleteModel : PageModel
    {
        private readonly IFoodData _foodData;
        private readonly IOrderData _orderData;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Order Order { get; set; }

        public DeleteModel(IFoodData foodData, IOrderData orderData)
        {
            _foodData = foodData;
            _orderData = orderData;
        }

        public async Task OnGet()
        {
            Order = await _orderData.GetOrderById(Id);
        }

        public async Task<IActionResult> OnPost()
        {
            await _orderData.DeleteOrder(Id);

            return RedirectToPage("./Create");
        }
    }
}
