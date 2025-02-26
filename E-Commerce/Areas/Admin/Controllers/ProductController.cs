using System.Linq.Expressions;
using System.Security.Claims;
using E_Commerce.DTO;
using E_Commerce.Models;
using E_Commerce.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitofWork _unitofWork;
        public ProductController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        [HttpGet]

        public IActionResult GetProducts()
        {
            var products = _unitofWork.Products.GetAll().ToList();
            return Ok(products);
        }
        [HttpGet("{id:int}", Name = "Productdetails")]


        public IActionResult GetProductByID([FromRoute] int id)
        {
            Product product = _unitofWork.Products.Get(c => c.Id == id);
            return Ok(product);
        }
        [HttpGet]
        [Route("name/{name:alpha}")]

        public IActionResult GetProductByName(string name)
        {
            Product product = _unitofWork.Products.Get(c => c.Title.ToLower() == name.ToLower());
            return Ok(product);
        }
        [HttpGet("category/{categoryname:alpha}")]
        public IActionResult GetProductByCategory(string categoryname)
        {
          var category=  _unitofWork.Categories.Get( c=>c.Name.ToLower()==categoryname.ToLower());
            if (category == null)
            {
                return NotFound();
            }
            var products = _unitofWork.Products.GetAllByfilter(p => p.CategoryId == category.Id).ToList();
            return Ok(products);

        }
        [HttpGet("pricerange/{pricerange:int}")]
        public IActionResult GetProductByPriceRange(int pricerange)
        {
            var products = _unitofWork.Products.GetAllByfilter(p => p.Price <= pricerange).ToList();
            return Ok(products);
        }
        [HttpGet("availability/{productname:alpha}")]
        public IActionResult GetProductByAvailability(string productname)
        {
            var product = _unitofWork.Products.Get(c => c.Title.ToLower() == productname.ToLower());
            if (product == null)
            {
                return NotFound();
            }
            if (product.IsAvailable)
            {
                return Ok("Product is available");
            }
            return Ok("Product is not available");
        }

            [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult PostProduct(Product newproduct)
        {
            if (ModelState.IsValid)
            {
                _unitofWork.Products.Add(newproduct);
                _unitofWork.Save();
                string url = Url.Link("Productdetails", new { id = newproduct.Id });
                return Created(url, newproduct);
            }
            return BadRequest(ModelState);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult PutProduct([FromRoute] int id, Product product)
        {
            if (ModelState.IsValid)
            {
                _unitofWork.Products.Update(product);
                _unitofWork.Save();
                return StatusCode(StatusCodes.Status204NoContent);
            }
            return BadRequest(ModelState);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteProduct(int id)
        {
            Product product = _unitofWork.Products.Get(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _unitofWork.Products.Remove(product);
            _unitofWork.Save();
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
