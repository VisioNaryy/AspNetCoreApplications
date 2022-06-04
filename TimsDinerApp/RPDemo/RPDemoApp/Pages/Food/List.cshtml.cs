using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RPDemoApp.Pages.Food
{
    public class ListModel : PageModel
    {
        private readonly IFoodData _foodData;
        private readonly IOrderData _orderData;

        public List<DataLibrary.Models.Food> Food { get; set; }

        public ListModel(IFoodData foodData, IOrderData orderData)
        {
            _foodData = foodData;
            _orderData = orderData;
        }
        public async Task OnGet()
        {
            Food = await _foodData.GetFood();
        }
    }
}
