using Azure.Core;
using DataAccessEF;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewApi_app.Cache;
using System.Linq;

namespace NewApi_app.Controllers {
    
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CategoryController : Controller {

        private UnitOfWorks Unit;
        private readonly ICacheService _cacheService;
        public CategoryController(ShopContext context, ICacheService cacheService) {
            this.Unit = new UnitOfWorks(context);
            this._cacheService = cacheService;
        }

        [HttpGet]
        public IActionResult Get() {
            try {
                List<Category> productsCache = _cacheService.GetData<List<Category>>("Category");
                if (productsCache == null) {
                    var productSQL = this.Unit.Category.Get().Cast<object>().ToList();
                    if (productSQL.Count > 0) {
                        _cacheService.SetData("Category", productSQL, DateTimeOffset.Now.AddMinutes(5));
                    }
                }
                return Ok(productsCache);
            } catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public IActionResult GetCategoryCount(int id) {
            return Ok(this.Unit.CategoryConnection.Get().Cast<CategoryConnection>().Where(x => x.CategoryId == id).Count());
        }

        [HttpPost]
        [Authorize(Roles =UserRoles.Manager)]
        public IActionResult Insert(string name,string image) {
            this.Unit.Category.Insert(new Category() {Image=image,Name=name });
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Manager)]
        public IActionResult Update(Category category) {
            this.Unit.Category.Update(category);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Manager)]
        public IActionResult Delete(int id) {
            try {
                this.Unit.Category.Delete(this.Unit.Category.FindById(id));

                return Ok();
            }catch {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }
    }
}
