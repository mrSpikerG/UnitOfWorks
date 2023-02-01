using Azure.Core;
using DataAccessEF;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace NewApi_app.Controllers {
    
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CategoryController : Controller {

        private UnitOfWorks Unit;
        public CategoryController(ShopContext context) {
            this.Unit = new UnitOfWorks(context);
        }

        [HttpGet]
        public IActionResult Get() {
            return Ok(this.Unit.Category.Get());
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
