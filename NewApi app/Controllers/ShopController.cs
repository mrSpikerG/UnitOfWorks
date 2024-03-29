﻿using Microsoft.AspNetCore.Mvc;
using DataAccessEF;
using Domain;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.EntityFrameworkCore;
using NewApi_app.Cache;
using System.Collections.Generic;

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
                    var productSQL = this.Unit.Product.Get();

                    _cacheService.SetData("ShopItem", productSQL, DateTimeOffset.Now.AddHours(6));
                    return Ok(productSQL);
                }
                return Ok(productsCache);
            } catch {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public IActionResult GetWithCategory() {

            return Ok(this.Unit.Product.GetAdvancedItems());
        }

        [HttpGet]
        public IActionResult GetLastProducts() {

            List<ShopItem> list = (List<ShopItem>)this.Unit.Product.Get();
            return Ok(list.TakeLast(4));
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

                _cacheService.SetData("ShopItem", this.Unit.Product.Get(), DateTimeOffset.Now.AddHours(6));
                return Ok();
            } catch {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Manager)]
        public IActionResult Update(ShopItem item) {
            this.Unit.Product.Update(item);
            _cacheService.SetData("ShopItem", this.Unit.Product.Get(), DateTimeOffset.Now.AddHours(6));
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Manager)]
        public IActionResult UpdateWithCategory([FromQuery] ShopItem item, int categoryId) {
            if (item.Price < 1) {
                return BadRequest();
            }
            this.Unit.Product.Update(item);
            this._context.CategoryConnections.FirstOrDefault(x => x.PhoneId == item.Id).CategoryId = categoryId;
            this._context.SaveChanges();
            _cacheService.SetData("ShopItem", this.Unit.Product.Get(), DateTimeOffset.Now.AddHours(6));
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Manager)]
        public IActionResult Delete(int id) {
            this.Unit.Product.Delete(this.Unit.Product.FindById(id));
            _cacheService.SetData("ShopItem", this.Unit.Product.Get(), DateTimeOffset.Now.AddHours(6));
            return Ok();
        }

    }
}
