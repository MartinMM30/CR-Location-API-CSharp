using Microsoft.AspNetCore.Mvc;
using Proyecto2_API.Models;

namespace Proyecto2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservacionesController : ControllerBase
    {
        private readonly RepositorioMemoria _repositorio = RepositorioMemoria.Instancia;

        [HttpGet]
        public IActionResult ObtenerReservaciones() { return Ok(_repositorio.Reservaciones); }

        [HttpGet("buscar/{termino}")]
        public IActionResult Buscar(string termino)
        {
            var resultados = _repositorio.Reservaciones
                .Where(r => r.Codigo == termino || r.IdentificacionCliente == termino)
                .ToList();

            if (!resultados.Any()) return NotFound("No se encontraron reservaciones con ese criterio.");
            return Ok(resultados);
        }

        [HttpPost]
        public IActionResult Crear([FromBody] Reservacion res)
        {
            if (_repositorio.Reservaciones.Any(r => r.Codigo == res.Codigo))
                return BadRequest("El código de reservación ya existe.");
            if (!_repositorio.Clientes.Any(c => c.Identificacion == res.IdentificacionCliente))
                return BadRequest("El cliente no existe en el sistema.");
            if (!_repositorio.Habitaciones.Any(h => h.NumeroHabitacion == res.NumeroHabitacion))
                return BadRequest("La habitación no existe en el sistema.");
            if (res.FechaSalida <= res.FechaIngreso)
                return BadRequest("La fecha de salida debe ser posterior a la de ingreso.");

            // Lógica de Traslapes
            bool traslape = _repositorio.Reservaciones.Any(r =>
                r.NumeroHabitacion == res.NumeroHabitacion &&
                (r.Estado == EstadoReservacion.Reservada || r.Estado == EstadoReservacion.Confirmada) &&
                (res.FechaIngreso < r.FechaSalida && res.FechaSalida > r.FechaIngreso)
            );

            if (traslape) return BadRequest("La habitación no está disponible en esas fechas.");

            _repositorio.Reservaciones.Add(res);
            return Ok(res);
        }

        [HttpPut("{codigo}")]
        public IActionResult Actualizar(string codigo, [FromBody] Reservacion resMod)
        {
            if (codigo != resMod.Codigo) return BadRequest("El código no coincide.");
            var resOrg = _repositorio.Reservaciones.FirstOrDefault(r => r.Codigo == codigo);
            if (resOrg == null) return NotFound("Reservación no encontrada.");

            if (resMod.FechaSalida <= resMod.FechaIngreso)
                return BadRequest("La fecha de salida debe ser posterior a la de ingreso.");

            // Lógica de traslapes omitiendo la reservación actual
            bool traslape = _repositorio.Reservaciones.Any(r =>
                r.Codigo != codigo &&
                r.NumeroHabitacion == resMod.NumeroHabitacion &&
                (r.Estado == EstadoReservacion.Reservada || r.Estado == EstadoReservacion.Confirmada) &&
                (resMod.FechaIngreso < r.FechaSalida && resMod.FechaSalida > r.FechaIngreso)
            );

            if (traslape) return BadRequest("Las nuevas fechas chocan con otra reservación.");

            resOrg.FechaIngreso = resMod.FechaIngreso;
            resOrg.FechaSalida = resMod.FechaSalida;
            resOrg.CantidadPersonas = resMod.CantidadPersonas;
            resOrg.Estado = resMod.Estado;

            
            resOrg.SolicitudesEspeciales = resMod.SolicitudesEspeciales;
            resOrg.PorcentajeDescuento = resMod.PorcentajeDescuento;
            resOrg.TarifaReservacion = resMod.TarifaReservacion;
            resOrg.MontoTotal = resMod.MontoTotal;

            return Ok(resOrg);
        }

        [HttpDelete("{codigo}")]
        public IActionResult Eliminar(string codigo)
        {
            var res = _repositorio.Reservaciones.FirstOrDefault(r => r.Codigo == codigo);
            if (res == null) return NotFound("Reservación no encontrada.");
            _repositorio.Reservaciones.Remove(res);
            return Ok("Reservación eliminada correctamente.");
        }
    }
}