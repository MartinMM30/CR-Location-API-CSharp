using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using Proyecto1.Models;

namespace Proyecto1.Controllers
{
    public class ReservacionController : Controller
    {
        private readonly string _apiUrl = "http://localhost:5101/api/Reservaciones";
        private readonly string _apiUrlClientes = "http://localhost:5101/api/Clientes";
        private readonly string _apiUrlHabitaciones = "http://localhost:5101/api/Habitaciones";

        // Función de apoyo para cargar los Dropdowns de Clientes y Habitaciones
        private async Task CargarListasDesplegables()
        {
            using var http = new HttpClient();
            var resCli = await http.GetAsync(_apiUrlClientes);
            if (resCli.IsSuccessStatusCode)
            {
                var clientes = JsonConvert.DeserializeObject<List<Cliente>>(await resCli.Content.ReadAsStringAsync()) ?? new();
                ViewBag.Clientes = new SelectList(clientes, "Identificacion", "Identificacion");
            }

            var resHab = await http.GetAsync(_apiUrlHabitaciones);
            if (resHab.IsSuccessStatusCode)
            {
                var habitaciones = JsonConvert.DeserializeObject<List<Habitacion>>(await resHab.Content.ReadAsStringAsync()) ?? new();
                ViewBag.Habitaciones = new SelectList(habitaciones, "NumeroHabitacion", "NumeroHabitacion");
            }
        }

        // Función para extraer la tarifa por noche desde la API de Habitaciones
        private async Task<decimal> ObtenerTarifa(int numeroHabitacion)
        {
            using var http = new HttpClient();
            var res = await http.GetAsync($"{_apiUrlHabitaciones}/buscar/{numeroHabitacion}");
            if (res.IsSuccessStatusCode)
            {
                var hab = JsonConvert.DeserializeObject<Habitacion>(await res.Content.ReadAsStringAsync());
                return hab?.TarifaPorNoche ?? 0;
            }
            return 0;
        }

        public async Task<IActionResult> Index()
        {
            List<Reservacion> reservaciones = new();
            using (var http = new HttpClient())
            {
                var response = await http.GetAsync(_apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    reservaciones = JsonConvert.DeserializeObject<List<Reservacion>>(apiResponse) ?? new();
                }
            }
            return View(reservaciones);
        }

        public async Task<IActionResult> Buscar(string terminoBusqueda)
        {
            if (string.IsNullOrEmpty(terminoBusqueda)) return RedirectToAction(nameof(Index));

            List<Reservacion> resultados = new();
            using (var http = new HttpClient())
            {
                var response = await http.GetAsync($"{_apiUrl}/buscar/{terminoBusqueda}");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    resultados = JsonConvert.DeserializeObject<List<Reservacion>>(apiResponse) ?? new();
                }
            }

            if (!resultados.Any())
            {
                ViewBag.MensajeError = "No se encontraron reservaciones con ese criterio.";
                return View("Index", new List<Reservacion>());
            }
            return View("Index", resultados);
        }

        public async Task<IActionResult> Create()
        {
            await CargarListasDesplegables();
            return View(new Reservacion { FechaReservacion = DateTime.Today, PorcentajeDescuento = 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservacion reservacion)
        {
            if (ModelState.IsValid)
            {
                int noches = (reservacion.FechaSalida - reservacion.FechaIngreso).Days;
                if (noches <= 0)
                {
                    ModelState.AddModelError(string.Empty, "La fecha de salida debe ser mayor a la de ingreso.");
                    await CargarListasDesplegables();
                    return View(reservacion);
                }

                
                // CÁLCULOS MATEMÁTICOS
                
                decimal tarifaNoche = await ObtenerTarifa(reservacion.NumeroHabitacion);

                // 1. Tarifa total
                reservacion.TarifaReservacion = tarifaNoche * noches;
                // 2. Descuento
                decimal montoDescuento = reservacion.TarifaReservacion * (reservacion.PorcentajeDescuento / 100m);
                decimal subtotal = reservacion.TarifaReservacion - montoDescuento;
                // 3. Monto Total con IVA
                reservacion.MontoTotal = subtotal + (subtotal * 0.13m);

                using var http = new HttpClient();
                var content = new StringContent(JsonConvert.SerializeObject(reservacion), Encoding.UTF8, "application/json");
                var response = await http.PostAsync(_apiUrl, content);

                if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));

                // Si hay traslapes, la API nos lo avisará y lo mostraremos
                ModelState.AddModelError(string.Empty, "Error: " + await response.Content.ReadAsStringAsync());
            }
            await CargarListasDesplegables();
            return View(reservacion);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            Reservacion? reservacion = null;
            using (var http = new HttpClient())
            {
                var response = await http.GetAsync($"{_apiUrl}/buscar/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var resultados = JsonConvert.DeserializeObject<List<Reservacion>>(await response.Content.ReadAsStringAsync());
                    reservacion = resultados?.FirstOrDefault();
                }
            }
            if (reservacion == null) return NotFound();

            await CargarListasDesplegables();
            return View(reservacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Reservacion reservacion)
        {
            if (id != reservacion.Codigo) return NotFound();

            if (ModelState.IsValid)
            {
                int noches = (reservacion.FechaSalida - reservacion.FechaIngreso).Days;
                if (noches <= 0)
                {
                    ModelState.AddModelError(string.Empty, "La fecha de salida debe ser mayor a la de ingreso.");
                    await CargarListasDesplegables();
                    return View(reservacion);
                }

                
                // CÁLCULOS MATEMÁTICOS
                
                decimal tarifaNoche = await ObtenerTarifa(reservacion.NumeroHabitacion);

                reservacion.TarifaReservacion = tarifaNoche * noches;
                decimal montoDescuento = reservacion.TarifaReservacion * (reservacion.PorcentajeDescuento / 100m);
                decimal subtotal = reservacion.TarifaReservacion - montoDescuento;
                reservacion.MontoTotal = subtotal + (subtotal * 0.13m);

                using var http = new HttpClient();
                var content = new StringContent(JsonConvert.SerializeObject(reservacion), Encoding.UTF8, "application/json");
                var response = await http.PutAsync($"{_apiUrl}/{id}", content);

                if (response.IsSuccessStatusCode) return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, "Error: " + await response.Content.ReadAsStringAsync());
            }
            await CargarListasDesplegables();
            return View(reservacion);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            Reservacion? reservacion = null;
            using (var http = new HttpClient())
            {
                var response = await http.GetAsync($"{_apiUrl}/buscar/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var resultados = JsonConvert.DeserializeObject<List<Reservacion>>(await response.Content.ReadAsStringAsync());
                    reservacion = resultados?.FirstOrDefault();
                }
            }
            if (reservacion == null) return NotFound();
            return View(reservacion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            using var http = new HttpClient();
            var response = await http.DeleteAsync($"{_apiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorIntegridad"] = await response.Content.ReadAsStringAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}