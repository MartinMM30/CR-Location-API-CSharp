using System.ComponentModel.DataAnnotations;

namespace Proyecto2_API.Models
{
    public enum CategoriaEmpleado
    {
        Mesero, Salonero, Lavaplatos, Recepcionista, Administrador, Mantenimiento,
        Cocinero, Chef, Limpieza, Cocina, Seguridad, AtencionAlCliente,
        GuiaTuristico, EncargadoReservaciones
    }
    
    public enum TipoHabitacion
    {
        [Display(Name = "Start Junior")]
        StartJunior,

        [Display(Name = "Start Vista al Mar")]
        StartVistaAlMar,

        [Display(Name = "Master Start")]
        MasterStart
    }
    public enum EstadoReservacion
    {
        Reservada,
        Confirmada,
        Cancelada,
        Completada
    }
}


