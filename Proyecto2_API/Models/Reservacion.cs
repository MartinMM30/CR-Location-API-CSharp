using System.ComponentModel.DataAnnotations;

namespace Proyecto2_API.Models
{
    public class Reservacion
    {
        [Key]
        [Required(ErrorMessage = "El código es obligatorio.")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "El código debe tener entre 1 y 20 caracteres.")]
        [Display(Name = "Código de Reservación")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "La identificación del cliente es obligatoria.")]
        [Display(Name = "Identificación del Cliente")]
        public string IdentificacionCliente { get; set; }

        [Required(ErrorMessage = "El número de habitación es obligatorio.")]
        [Display(Name = "Número de Habitación")]
        public int NumeroHabitacion { get; set; }

        [Required(ErrorMessage = "La fecha de ingreso es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Ingreso")]
        public DateTime FechaIngreso { get; set; }

        [Required(ErrorMessage = "La fecha de salida es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Salida")]
        public DateTime FechaSalida { get; set; }

        [Required(ErrorMessage = "La cantidad de personas es obligatoria.")]
        [Range(1, 10, ErrorMessage = "La cantidad debe estar entre 1 y 10 personas.")]
        [Display(Name = "Cantidad de Personas")]
        public int CantidadPersonas { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [Display(Name = "Estado")]
        public EstadoReservacion Estado { get; set; }

        [Display(Name = "Fecha de la Reservación")]
        [DataType(DataType.Date)]
        public DateTime FechaReservacion { get; set; }

        [Display(Name = "Solicitudes Especiales")]
        [StringLength(300, ErrorMessage = "Máximo 300 caracteres.")]
        public string SolicitudesEspeciales { get; set; }

        [Display(Name = "Porcentaje de Descuento")]
        [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100.")]
        public decimal PorcentajeDescuento { get; set; }

        [Display(Name = "Tarifa de la Reservación")]
        public decimal TarifaReservacion { get; set; }

        [Display(Name = "Monto Total (con IVA)")]
        public decimal MontoTotal { get; set; }
    }
}
