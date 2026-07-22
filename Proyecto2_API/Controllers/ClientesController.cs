using Microsoft.AspNetCore.Mvc;
using Proyecto2_API.Models;

namespace Proyecto2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        // Conectamos con nuestro Singleton en memoria
        private readonly RepositorioMemoria _repositorio = RepositorioMemoria.Instancia;

        // ==========================================
        // LEER TODOS (GET)
        // ==========================================
        [HttpGet]
        public IActionResult ObtenerClientes()
        {
            try
            {
                // Devuelve código 200 y la lista en JSON
                return Ok(_repositorio.Clientes);
            }
            catch (Exception ex)
            {
                // Buenas prácticas: Manejo de errores solicitado en tutoría
                return StatusCode(500, "Error interno: " + ex.Message);
            }
        }

        // ==========================================
        // BUSCAR POR ID (GET)
        // ==========================================
        [HttpGet("buscar/{id}")]
        public IActionResult ObtenerClientePorId(string id)
        {
            try
            {
                var cliente = _repositorio.Clientes.FirstOrDefault(c => c.Identificacion == id);
                if (cliente == null)
                {
                    return NotFound("El cliente no existe."); // Error 404
                }
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno: " + ex.Message);
            }
        }

        // ==========================================
        // CREAR (POST)
        // ==========================================
        [HttpPost]
        public IActionResult CrearCliente([FromBody] Cliente nuevoCliente)
        {
            try
            {
                if (nuevoCliente == null) return BadRequest("Datos nulos.");

                // Validar que no exista
                if (_repositorio.Clientes.Any(c => c.Identificacion == nuevoCliente.Identificacion))
                {
                    return BadRequest("Ya existe un cliente con esta identificación."); // Error 400
                }

                _repositorio.Clientes.Add(nuevoCliente);
                return Ok(nuevoCliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno: " + ex.Message);
            }
        }

        // ==========================================
        // ACTUALIZAR (PUT)
        // ==========================================
        [HttpPut("{id}")]
        public IActionResult ActualizarCliente(string id, [FromBody] Cliente clienteModificado)
        {
            try
            {
                if (id != clienteModificado.Identificacion) return BadRequest("El ID no coincide.");

                var clienteOriginal = _repositorio.Clientes.FirstOrDefault(c => c.Identificacion == id);
                if (clienteOriginal == null) return NotFound("Cliente no encontrado.");

                // Actualizamos los datos
                clienteOriginal.Nombre = clienteModificado.Nombre;
                clienteOriginal.PrimerApellido = clienteModificado.PrimerApellido;
                clienteOriginal.SegundoApellido = clienteModificado.SegundoApellido;
                clienteOriginal.FechaNacimiento = clienteModificado.FechaNacimiento;

                return Ok(clienteOriginal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno: " + ex.Message);
            }
        }

        // ==========================================
        // ELIMINAR (DELETE)
        // ==========================================
        [HttpDelete("{id}")]
        public IActionResult EliminarCliente(string id)
        {
            try
            {
                // Regla de integridad: No eliminar si tiene reservaciones
                bool tieneReservaciones = _repositorio.Reservaciones.Any(r => r.IdentificacionCliente == id);
                if (tieneReservaciones)
                {
                    return BadRequest("Error de integridad: El cliente tiene reservaciones asociadas.");
                }

                var cliente = _repositorio.Clientes.FirstOrDefault(c => c.Identificacion == id);
                if (cliente == null) return NotFound("Cliente no encontrado.");

                _repositorio.Clientes.Remove(cliente);
                return Ok("Cliente eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno: " + ex.Message);
            }
        }
    }
}