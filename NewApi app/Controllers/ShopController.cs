using Microsoft.AspNetCore.Mvc;
using DataAccessEF;
using Domain;
using Microsoft.AspNetCore.Authorization;
using System.Data;

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

        [HttpGet]
        public IActionResult GetCategory(int id) {
            return Ok(this.Unit.CategoryConnection.GetCategoryByProduct(id));
        }

        [HttpGet]
        public IActionResult GetByPage(int page,int count,int categoryId) {

            if(page<=0 || count <= 0) {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return Ok(this.Unit.Product.GetItems(page-1,count, categoryId));
        }
          
        [HttpGet]
        public IActionResult GetPages(int count,int categoryId) {
            if (count <= 0) {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            return Ok(this.Unit.Product.GetPages(count, categoryId));
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Manager)]
        public IActionResult Insert(ShopItem item) {
            this.Unit.Product.Insert(item);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Manager)]
        public IActionResult Update(ShopItem item) {
            this.Unit.Product.Update(item);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Manager)]
        public IActionResult Delete(ShopItem item) {
            this.Unit.Product.Delete(item);
            return Ok();
        }
        
    }
}
