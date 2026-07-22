using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using Proyecto1.Models;

namespace Proyecto1.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly string _apiUrl = "http://localhost:5101/api/Empleados";

        // Función de apoyo para cargar Provincias desde la API
        private async Task CargarProvincias()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{_apiUrl}/provincias");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var diccionarioProvincias = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(apiResponse);

                    if (diccionarioProvincias != null)
                    {
                        //Enviamos una lista de textos (strings) para que la vista los lea sin problema
                        ViewBag.Provincias = diccionarioProvincias.Keys.ToList();
                    }
                }
            }
        }

        // Endpoint para que el JavaScript de la vista cargue los Cantones dinámicamente
        [HttpGet]
        public async Task<JsonResult> ObtenerCantones(string provincia)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{_apiUrl}/provincias");
                if (response.IsSuccessStatusCode)
                {
                    var dic = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(await response.Content.ReadAsStringAsync());
                    if (dic != null && !string.IsNullOrEmpty(provincia) && dic.ContainsKey(provincia))
                    {
                        return Json(dic[provincia]);
                    }
                }
            }
            return Json(new List<string>());
        }
       
        [HttpGet]
        public JsonResult ObtenerDistritos(string canton)
        {
            if (!string.IsNullOrEmpty(canton))
            {
                return Json(new List<string> {
                    "Centro",
                    "San José",
                    "San Juan",
                    "San Pedro",
                    "El Carmen"
                });
            }
            return Json(new List<string>());
        }

        // Obtener todos los empleados para la tabla principal
        public async Task<IActionResult> Index()
        {
            List<Empleado> empleados = new();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(_apiUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        empleados = JsonConvert.DeserializeObject<List<Empleado>>(apiResponse) ?? new List<Empleado>();
                    }
                }
            }
            return View(empleados);
        }

        // Buscar empleado por su número de identificación
        public async Task<IActionResult> Buscar(string terminoBusqueda)
        {
            if (string.IsNullOrEmpty(terminoBusqueda)) return RedirectToAction(nameof(Index));

            Empleado? empleado = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{_apiUrl}/buscar/{terminoBusqueda}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        empleado = JsonConvert.DeserializeObject<Empleado>(apiResponse);
                    }
                }
            }

            if (empleado == null)
            {
                ViewBag.MensajeError = "No se encontró ningún empleado.";
                return View("Index", new List<Empleado>());
            }
            return View("Index", new List<Empleado> { empleado });
        }

        // Mostrar el formulario de creación de empleado
        public async Task<IActionResult> Create()
        {
            await CargarProvincias();
            return View();
        }

        // Enviar los datos del nuevo empleado a la API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(empleado), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync(_apiUrl, content))
                    {
                        if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError(string.Empty, "Error de la API: " + errorResponse);
                    }
                }
            }
            await CargarProvincias();
            return View(empleado);
        }

        // Mostrar el formulario para editar un empleado existente
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            Empleado? empleado = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{_apiUrl}/buscar/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        empleado = JsonConvert.DeserializeObject<Empleado>(apiResponse);
                    }
                }
            }
            if (empleado == null) return NotFound();

            await CargarProvincias();
            return View(empleado);
        }

        // Enviar los datos modificados a la API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Empleado empleado)
        {
            if (id != empleado.Identificacion) return NotFound();
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(empleado), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync($"{_apiUrl}/{id}", content))
                    {
                        if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError(string.Empty, "Error de la API: " + errorResponse);
                    }
                }
            }
            await CargarProvincias();
            return View(empleado);
        }

        // Mostrar la pantalla de confirmación antes de eliminar
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();
            Empleado? empleado = null;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{_apiUrl}/buscar/{id}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        empleado = JsonConvert.DeserializeObject<Empleado>(apiResponse);
                    }
                }
            }
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        // Ejecutar la eliminación del registro en la API
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