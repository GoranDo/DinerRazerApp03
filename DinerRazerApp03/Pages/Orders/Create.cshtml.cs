using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DataLibrary.Data;
using DataLibrary.Models;
using DinerRazerApp03.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DinerRazerApp03.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly IFoodData foodData;
        private readonly IOrderData orderData;

        public List<SelectListItem> FoodItems { get; set; }
        [BindProperty]
        public OrderModel Order { get; set; }

        public CreateModel(IFoodData foodData, IOrderData orderData)
        {
            this.foodData = foodData;
            this.orderData = orderData;
        }

        public async Task OnGet()
        {
            var food = await orderData.GetFood();
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
            var food = await orderData.GetFood();
            Order.Total = Order.Quantity * food.Where(x => x.Id == Order.FoodId).First().Price;
            int id = await orderData.CreateOrder(Order);
            return RedirectToPage("./Display", new { Id = id });
        }


    }
}
