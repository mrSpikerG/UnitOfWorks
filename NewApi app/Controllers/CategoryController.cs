using DataAccessEF;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public IActionResult Insert(Category category) {
            this.Unit.Category.Insert(category);
            return Ok();
        }

        [HttpPost]
        public IActionResult Update(Category category) {
            this.Unit.Category.Update(category);
            return Ok();
        }

        [HttpPost]
        public IActionResult Delete(Category category) {
            this.Unit.Category.Delete(category);
            return Ok();
        }

    }
}
