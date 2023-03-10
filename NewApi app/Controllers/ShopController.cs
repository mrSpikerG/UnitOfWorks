using Microsoft.AspNetCore.Mvc;
using DataAccessEF;
using Domain;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.EntityFrameworkCore;
using NewApi_app.Cache;

namespace NewApi_app.Properties {

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ShopController : Controller {

        private UnitOfWorks Unit;
        private ShopContext _context;
        private readonly ICacheService _cacheService;
        public ShopController(ShopContext context, ICacheService cacheService) {
            this.Unit = new UnitOfWorks(context);
            this._context = context;
            this._cacheService = cacheService;
        }


        [HttpGet]
        public IActionResult Get() {
            try {
                List<ShopItem> productsCache = _cacheService.GetData<List<ShopItem>>("ShopItem");
                if (productsCache == null) {
                    var productSQL = this._context.ShopItems.ToList();
                    if (productSQL.Count > 0) {
                        _cacheService.SetData("ShopItem", productSQL, DateTimeOffset.Now.AddMinutes(5));
                    }
                }
                return Ok(productsCache);
            } catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public IActionResult GetCategory(int id) {
            return Ok(this.Unit.CategoryConnection.GetCategoryByProduct(id));
        }

        [HttpGet]
        public IActionResult GetByPage(int page, int count, int categoryId, decimal minCost, decimal maxCost) {

            if (minCost == -1) {
                minCost = decimal.MinValue;
            }
            if (maxCost == -1) {
                maxCost = decimal.MaxValue;
            }

            if (page <= 0 || count <= 0) {
                return StatusCode(StatusCodes.Status400BadRequest);
            }


            return Ok(this.Unit.Product.GetItems(page - 1, count, categoryId, minCost, maxCost));
        }

        [HttpGet]
        public IActionResult GetPages(int count, int categoryId, decimal minCost, decimal maxCost) {

            if (minCost == -1) {
                minCost = decimal.MinValue;
            }
            if (maxCost == -1) {
                maxCost = decimal.MaxValue;
            }

            if (count <= 0) {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            return Ok(this.Unit.Product.GetPages(count, categoryId, minCost, maxCost));
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Manager)]
        public IActionResult Insert(string name, string image, decimal price, int categoryId) {

            try {
                if (this.Unit.Category.FindById(categoryId) == null) {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                this.Unit.Product.Insert(new ShopItem() { Name = name, Image = image, Price = price });
                this.Unit.CategoryConnection.Insert(new CategoryConnection() { PhoneId = this.Unit.Product.GetLastByName(name), CategoryId = categoryId });
                return Ok();
            } catch {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Manager)]
        public IActionResult Update(ShopItem item) {
            this.Unit.Product.Update(item);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Manager)]
        public IActionResult Delete(int id) {
            this.Unit.Product.Delete(this.Unit.Product.FindById(id));
            return Ok();
        }

    }
}
