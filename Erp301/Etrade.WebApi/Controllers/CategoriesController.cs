using Etrade.DAL.Abstract;
using Etrade.Entities.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Etrade.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryDAL categoryDAL;

        public CategoriesController(ICategoryDAL categoryDAL)
        {
            this.categoryDAL = categoryDAL;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(categoryDAL.GetAll());
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = categoryDAL.Get(id);
            if (category == null)
            {
                return BadRequest();//NotFound();
            }
            return Ok(category);
        }
        [HttpPost]
        public IActionResult Post([FromBody] Category category)
        {
            if (ModelState.IsValid)
            {
                categoryDAL.Add(category);
                //return Ok();
                return CreatedAtAction("Get", new { id = category.Id }, category);
            }
            return BadRequest();
        }
        [HttpPut]
        public IActionResult Put([FromBody] Category category)
        {
            if (ModelState.IsValid)
            {
                categoryDAL.Update(category);
                return Ok(category);
            }
            return BadRequest();
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var category = categoryDAL.Get(id);
            if (category == null)
                return NotFound("Kategori Bulunamadı");

            categoryDAL.Delete(category);
            return Ok();
        }
    }
}
