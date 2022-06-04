using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MVCDemoApp.Models
{
    public class OrderCreateModel
    {
        public Order Order { get; set; }

        public List<SelectListItem> FoodItems { get; set; }

        public OrderCreateModel()
        {
            Order = new Order();
            FoodItems = new List<SelectListItem>();
        }
    }
}