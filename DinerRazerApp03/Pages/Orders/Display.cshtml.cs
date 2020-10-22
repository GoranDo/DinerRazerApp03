using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLibrary.Data;
using DataLibrary.Models;
using DinerRazerApp03.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DinerRazerApp03.Pages.Orders
{
    public class DisplayModel : PageModel
    {
        private readonly IFoodData foodData;
        private readonly IOrderData orderData;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public OrderUpdateModel UpdateModel { get; set; }

        public OrderModel Order { get; set; }
        public string ItemPurchased { get; set; }

        public DisplayModel(IFoodData foodData, IOrderData orderData)
        {
            this.foodData = foodData;
            this.orderData = orderData;
        }
        public async Task<IActionResult> OnGet()
        {
            Order = await orderData.GetOrderById(Id);
            var food = await foodData.GetFood();
            ItemPurchased = food.Where(x => x.Id == Order.FoodId).FirstOrDefault()?.Title;
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }
            await orderData.UpdateOrderName(UpdateModel.Id, UpdateModel.OrderName);

            return RedirectToPage("./Display", new { UpdateModel.Id });
        }
    }
}
