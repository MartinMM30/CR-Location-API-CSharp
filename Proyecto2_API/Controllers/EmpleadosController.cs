using Microsoft.AspNetCore.Mvc;
using Proyecto2_API.Models;

namespace Proyecto2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private readonly RepositorioMemoria _repositorio = RepositorioMemoria.Instancia;

        [HttpGet]
        public IActionResult ObtenerEmpleados() { return Ok(_repositorio.Empleados); }

        [HttpGet("buscar/{id}")]
        public IActionResult Buscar(string id)
        {
            var emp = _repositorio.Empleados.FirstOrDefault(e => e.Identificacion == id);
            if (emp == null) return NotFound("Empleado no encontrado.");
            return Ok(emp);
        }

        [HttpPost]
        public IActionResult Crear([FromBody] Empleado emp)
        {
            if (_repositorio.Empleados.Any(e => e.Identificacion == emp.Identificacion))
                return BadRequest("Ya existe un empleado con esta identificación.");

            _repositorio.Empleados.Add(emp);
            return Ok(emp);
        }

        [HttpPut("{id}")]
        public IActionResult Actualizar(string id, [FromBody] Empleado empMod)
        {
            if (id != empMod.Identificacion) return BadRequest("El ID no coincide.");
            var empOrg = _repositorio.Empleados.FirstOrDefault(e => e.Identificacion == id);
            if (empOrg == null) return NotFound("Empleado no encontrado.");

            empOrg.Nombre = empMod.Nombre;
            empOrg.PrimerApellido = empMod.PrimerApellido;
            empOrg.SegundoApellido = empMod.SegundoApellido;
            empOrg.FechaNacimiento = empMod.FechaNacimiento;
            empOrg.SalarioMensual = empMod.SalarioMensual;
            empOrg.FechaIngreso = empMod.FechaIngreso;
            empOrg.Categoria = empMod.Categoria;
            empOrg.Provincia = empMod.Provincia;
            empOrg.Canton = empMod.Canton;
            empOrg.Distrito = empMod.Distrito;
            empOrg.DireccionExacta = empMod.DireccionExacta;

            return Ok(empOrg);
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(string id)
        {
            var emp = _repositorio.Empleados.FirstOrDefault(e => e.Identificacion == id);
            if (emp == null) return NotFound("Empleado no encontrado.");
            _repositorio.Empleados.Remove(emp);
            return Ok("Empleado eliminado correctamente.");
        }
        [HttpGet("provincias")]
        public IActionResult ObtenerProvincias()
        {
            return Ok(_repositorio.ProvinciasYCantones);
        }


    }
}