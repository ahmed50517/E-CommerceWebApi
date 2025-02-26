using E_Commerce.Models;
using E_Commerce.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    
    
    
    public class CategoryController : ControllerBase
    {
        private readonly IUnitofWork _unitofWork;
        public CategoryController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public IActionResult GetCategories()
        {
            var categories = _unitofWork.Categories.GetAll().ToList();
            return Ok(categories);
        }
        [HttpGet("{id:int}", Name = "Categorydetails")]

        [Authorize(Roles = "Admin")]

        public IActionResult GetByID([FromRoute] int id)
        {
            Category category = _unitofWork.Categories.Get(c => c.Id == id);
            return Ok(category);
        }
        [HttpGet]
        [Route("name/{name:alpha}")]
        [Authorize(Roles = "Admin")]

        public IActionResult GetByName(string name)
        {
            Category category = _unitofWork.Categories.Get(c => c.Name == name);
            return Ok(category);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult PostCategories(Category newcategory)
        {
            if (ModelState.IsValid)
            {
                _unitofWork.Categories.Add(newcategory);
                _unitofWork.Save();
                string url = Url.Link("Categorydetails", new { id = newcategory.Id });
                return Created(url, newcategory);
            }
            return BadRequest(ModelState);
        }
        [HttpPut("{id}")]
        [Authorize(Roles ="Admin")]
        public IActionResult PutCategory([FromRoute] int id, Category category)
        {
            if (ModelState.IsValid)
            {
                _unitofWork.Categories.Update(category);
                _unitofWork.Save();
                return StatusCode(StatusCodes.Status204NoContent);
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles= "Admin")]
        public IActionResult DeleteCategory(int id)
        {
            Category category = _unitofWork.Categories.Get(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _unitofWork.Categories.Remove(category);
            _unitofWork.Save();
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
