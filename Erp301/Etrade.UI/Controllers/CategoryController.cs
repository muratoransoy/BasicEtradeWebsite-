using Etrade.Entities.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Etrade.UI.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private HttpClient httpClient;
        public CategoryController()
        {
                httpClient= new HttpClient();
        }
        public async Task<IActionResult> Index()
        {
            var responseMessage = await httpClient.GetAsync("http://localhost:5085/api/Categories");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonString = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<Category>>(jsonString);
                return View(values);
            }
            return NotFound("Kategori listesi alınamadı");
        }
        [Authorize(Roles ="create,admin")]
        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id","Name")]Category category) 
        {
            var JsonObject = JsonConvert.SerializeObject(category);
            var stringContent = new StringContent(JsonObject,Encoding.UTF8,"application/json");
            var responeMessage = await httpClient.PostAsync("http://localhost:5085/api/Categories",stringContent);
            if(responeMessage.IsSuccessStatusCode)
                return RedirectToAction("Index");
            return View(category);
        }
        [Authorize(Roles = "create,admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var responseMessage = await httpClient.GetAsync("http://localhost:5085/api/Categories/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonString = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<Category>(jsonString);
                return View(value);
            }
            return NotFound("Kategori alınamadı");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            var JsonObject = JsonConvert.SerializeObject(category);
            var stringContent = new StringContent(JsonObject, Encoding.UTF8, "application/json");
            var responeMessage = await httpClient.PutAsync("http://localhost:5085/api/Categories/", stringContent);
            if (responeMessage.IsSuccessStatusCode)
                return RedirectToAction("Index");
            return View(category);
        }
        public async Task<IActionResult> Details(int id)
        {
            var responseMessage = await httpClient.GetAsync("http://localhost:5085/api/Categories/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonString = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<Category>(jsonString);
                return View(value);
            }
            return NotFound("Kategori alınamadı");
        }
        public async Task<IActionResult> Delete(int id)
        {
           
            var responeMessage = await httpClient.DeleteAsync("http://localhost:5085/api/Categories?id="+id);
            if (responeMessage.IsSuccessStatusCode)
         
                return RedirectToAction("Index");
       
           return NotFound();
        
        }
    }
}
