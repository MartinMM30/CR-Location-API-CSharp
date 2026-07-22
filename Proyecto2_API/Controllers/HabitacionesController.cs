using Microsoft.AspNetCore.Mvc;
using Proyecto2_API.Models;

namespace Proyecto2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HabitacionesController : ControllerBase
    {
        private readonly RepositorioMemoria _repositorio = RepositorioMemoria.Instancia;

        [HttpGet]
        public IActionResult ObtenerHabitaciones() { return Ok(_repositorio.Habitaciones); }

        [HttpGet("buscar/{id}")]
        public IActionResult Buscar(int id)
        {
            var hab = _repositorio.Habitaciones.FirstOrDefault(h => h.NumeroHabitacion == id);
            if (hab == null) return NotFound("Habitación no encontrada.");
            return Ok(hab);
        }

        [HttpPost]
        public IActionResult Crear([FromBody] Habitacion hab)
        {
            if (_repositorio.Habitaciones.Any(h => h.NumeroHabitacion == hab.NumeroHabitacion))
                return BadRequest("Ya existe esta habitación.");

            _repositorio.Habitaciones.Add(hab);
            return Ok(hab);
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Habitacion habMod)
        {
            if (id != habMod.NumeroHabitacion) return BadRequest("El ID no coincide.");
            var habOrg = _repositorio.Habitaciones.FirstOrDefault(h => h.NumeroHabitacion == id);
            if (habOrg == null) return NotFound("Habitación no encontrada.");

            habOrg.Tipo = habMod.Tipo;
            habOrg.TarifaPorNoche = habMod.TarifaPorNoche;
            habOrg.TieneTVSatelital = habMod.TieneTVSatelital;
            habOrg.PendientesMantenimiento = habMod.PendientesMantenimiento;

            return Ok(habOrg);
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            // Regla de integridad
            if (_repositorio.Reservaciones.Any(r => r.NumeroHabitacion == id))
                return BadRequest("Error de integridad: La habitación tiene reservaciones asociadas.");

            var hab = _repositorio.Habitaciones.FirstOrDefault(h => h.NumeroHabitacion == id);
            if (hab == null) return NotFound("Habitación no encontrada.");

            _repositorio.Habitaciones.Remove(hab);
            return Ok("Habitación eliminada correctamente.");
        }
    }
}