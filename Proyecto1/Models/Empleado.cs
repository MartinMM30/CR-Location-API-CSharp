using System.ComponentModel.DataAnnotations;

namespace Proyecto1.Models
{
    public class Empleado
    {
        [Key] // Obligatorio para que no falle el scaffolding de Visual Studio
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

        [Required(ErrorMessage = "El salario mensual es obligatorio.")]
        [Range(0, 5000000, ErrorMessage = "El salario debe estar entre 0 y 5,000,000.")]
        [Display(Name = "Salario Mensual")]
        public decimal SalarioMensual { get; set; }

        [Required(ErrorMessage = "La fecha de ingreso es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Ingreso")]
        public DateTime FechaIngreso { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [Display(Name = "Categoría")]
        public CategoriaEmpleado Categoria { get; set; }

        [Required(ErrorMessage = "La provincia es obligatoria.")]
        public string Provincia { get; set; }

        [Required(ErrorMessage = "El cantón es obligatorio.")]
        [Display(Name = "Cantón")]
        public string Canton { get; set; }

        [Required(ErrorMessage = "El distrito es obligatorio.")]
        public string Distrito { get; set; }

        [Required(ErrorMessage = "La dirección exacta es obligatoria.")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "La dirección debe tener un máximo de 150 caracteres.")]
        [Display(Name = "Dirección Exacta")]
        public string DireccionExacta { get; set; }
    }
}
