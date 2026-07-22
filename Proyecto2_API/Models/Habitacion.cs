using System.ComponentModel.DataAnnotations;

namespace Proyecto2_API.Models
{
    public class Habitacion
    {
        [Key]
        [Required(ErrorMessage = "El número de habitación es obligatorio.")]
        [Range(1, 500, ErrorMessage = "El número de habitación debe estar entre 1 y 500.")]
        [Display(Name = "Número de Habitación")]
        public int NumeroHabitacion { get; set; }

        [Required(ErrorMessage = "El tipo de habitación es obligatorio.")]
        [Display(Name = "Tipo de Habitación")]
        public TipoHabitacion Tipo { get; set; }

        [Required(ErrorMessage = "La tarifa es obligatoria.")]
        [Range(50, 800, ErrorMessage = "La tarifa por noche debe estar entre $50 y $800.")]
        [Display(Name = "Tarifa por Noche ($)")]
        public decimal TarifaPorNoche { get; set; }

        [Required(ErrorMessage = "Debe indicar si tiene TV Satelital.")]
        [Display(Name = "TV Satelital")]
        public bool TieneTVSatelital { get; set; }

        [StringLength(500, ErrorMessage = "El texto de mantenimiento no puede superar los 500 caracteres.")]
        [Display(Name = "Pendientes de Mantenimiento")]
        public string PendientesMantenimiento { get; set; }
    }
}
