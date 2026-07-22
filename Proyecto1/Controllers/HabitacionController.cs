using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Proyecto1.Models;

namespace Proyecto1.Controllers
{
    public class HabitacionController : Controller
    {
        private readonly string _apiUrl = "http://localhost:5101/api/Habitaciones";

        public async Task<IActionResult> Index()
        {
            List<Habitacion> habitaciones = new();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_apiUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        habitaciones = JsonConvert.DeserializeObject<List<Habitacion>>(apiResponse) ?? new List<Habitacion>();
                    }
                }
            }
            return View(habitaciones);
        }

        public async Task<IActionResult> Buscar(int terminoBusqueda)
        {
            if (terminoBusqueda <= 0) return RedirectToAction(nameof(Index));

            Habitacion? habitacion = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{_apiUrl}/buscar/{terminoBusqueda}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        habitacion = JsonConvert.DeserializeObject<Habitacion>(apiResponse);
                    }
                }
            }

            if (habitacion == null)
            {
                ViewBag.MensajeError = "No se encontró ninguna habitación con ese número.";
                return View("Index", new List<Habitacion>());
            }
            return View("Index", new List<Habitacion> { habitacion });
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Habitacion habitacion)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(habitacion), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync(_apiUrl, content))
                    {
                        if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError(string.Empty, "Error de la API: " + errorResponse);
                    }
                }
            }
            return View(habitacion);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            Habitacion? habitacion = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{_apiUrl}/buscar/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        habitacion = JsonConvert.DeserializeObject<Habitacion>(apiResponse);
                    }
                }
            }
            if (habitacion == null) return NotFound();
            return View(habitacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Habitacion habitacion)
        {
            if (id != habitacion.NumeroHabitacion) return NotFound();
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(habitacion), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync($"{_apiUrl}/{id}", content))
                    {
                        if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError(string.Empty, "Error de la API: " + errorResponse);
                    }
                }
            }
            return View(habitacion);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Habitacion? habitacion = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{_apiUrl}/buscar/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        habitacion = JsonConvert.DeserializeObject<Habitacion>(apiResponse);
                    }
                }
            }
            if (habitacion == null) return NotFound();
            return View(habitacion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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