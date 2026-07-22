using System.ComponentModel.DataAnnotations;

namespace Proyecto2_API.Models
{
    public class Cliente
    {
        [Key]
        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [RegularExpression(@"^(\d-\d{4}-\d{4}|\d{12}|[a-zA-Z0-9]{1,50})$", ErrorMessage = "Formato inválido. Debe ser Cédula (ej. 1-1111-0909), DIMEX (12 dígitos) o Pasaporte (alfanumérico hasta 50 caracteres).")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "La identificación debe tener entre 1 y 50 caracteres.")]
        [Display(Name = "Identificación")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [StringLength(75, MinimumLength = 3, ErrorMessage = "El primer apellido debe tener entre 3 y 75 caracteres.")]
        [Display(Name = "Primer Apellido")]
        public string PrimerApellido { get; set; }

        [Required(ErrorMessage = "El segundo apellido es obligatorio.")]
        [StringLength(75, MinimumLength = 3, ErrorMessage = "El segundo apellido debe tener entre 3 y 75 caracteres.")]
        [Display(Name = "Segundo Apellido")]
        public string SegundoApellido { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }
    }
}
