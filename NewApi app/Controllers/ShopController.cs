using Microsoft.AspNetCore.Mvc;
using DataAccessEF;

namespace NewApi_app.Properties {
    
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ShopController : Controller {

        private UnitOfWorks Unit;
        public ShopController(ShopContext context) {
            this.Unit = new UnitOfWorks(context);
        }
 
        
        [HttpGet]
        public IActionResult Get() {
            return Ok(this.Unit.Product.Get());
        }

        [HttpPost]
        public IActionResult Insert(ShopItem item) {
            this.Unit.Product.Insert(item);
            return Ok();
        }

        [HttpPost]
        public IActionResult Update(ShopItem item) {
            this.Unit.Product.Update(item);
            return Ok();
        }

        [HttpPost]
        public IActionResult Delete(ShopItem item) {
            this.Unit.Product.Delete(item);
            return Ok();
        }
        
    }
}
