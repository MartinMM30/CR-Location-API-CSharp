using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Proyecto1.Models;

namespace Proyecto1.Controllers
{
    public class ClienteController : Controller
    {
        // URL base de API
        private readonly string _apiUrl = "http://localhost:5101/api/Clientes";

   
        // LEER TODOS (GET)
        
        public async Task<IActionResult> Index()
        {
            List<Cliente> clientes = new List<Cliente>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_apiUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        // El "??" evita la advertencia CS8600 si el JSON viene vacío
                        clientes = JsonConvert.DeserializeObject<List<Cliente>>(apiResponse) ?? new List<Cliente>();
                    }
                }
            }
            return View(clientes);
        }

        
        // BUSCAR (GET)
        
        public async Task<IActionResult> Buscar(string terminoBusqueda)
        {
            if (string.IsNullOrEmpty(terminoBusqueda)) return RedirectToAction(nameof(Index));

            Cliente? cliente = null; 
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{_apiUrl}/buscar/{terminoBusqueda}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        cliente = JsonConvert.DeserializeObject<Cliente>(apiResponse);
                    }
                }
            }

            if (cliente == null)
            {
                ViewBag.MensajeError = "No se encontró ningún cliente con esa identificación.";
                return View("Index", new List<Cliente>());
            }

            return View("Index", new List<Cliente> { cliente });
        }

        
        // CREAR (GET y POST)
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(cliente), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync(_apiUrl, content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            string errorResponse = await response.Content.ReadAsStringAsync();
                            ModelState.AddModelError(string.Empty, "Error de la API: " + errorResponse);
                        }
                    }
                }
            }
            return View(cliente);
        }

        
        // ACTUALIZAR (GET y POST)
        
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            Cliente? cliente = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{_apiUrl}/buscar/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        cliente = JsonConvert.DeserializeObject<Cliente>(apiResponse);
                    }
                }
            }

            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Cliente cliente)
        {
            if (id != cliente.Identificacion) return NotFound();

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(cliente), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync($"{_apiUrl}/{id}", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            string errorResponse = await response.Content.ReadAsStringAsync();
                            ModelState.AddModelError(string.Empty, "Error de la API: " + errorResponse);
                        }
                    }
                }
            }
            return View(cliente);
        }

        // ELIMINAR (GET y POST)
        
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            Cliente? cliente = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{_apiUrl}/buscar/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        cliente = JsonConvert.DeserializeObject<Cliente>(apiResponse);
                    }
                }
            }

            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{_apiUrl}/{id}"))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        TempData["ErrorIntegridad"] = errorResponse;
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}