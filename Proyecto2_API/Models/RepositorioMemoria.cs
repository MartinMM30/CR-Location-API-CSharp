namespace Proyecto2_API.Models
{
    public class RepositorioMemoria
    {
        private static RepositorioMemoria _instancia;

        public List<Cliente> Clientes { get; set; }
        public List<Empleado> Empleados { get; set; }
        public List<Habitacion> Habitaciones { get; set; }
        public List<Reservacion> Reservaciones { get; set; }

        public Dictionary<string, List<string>> ProvinciasYCantones { get; set; }

        private RepositorioMemoria()
        {
            Clientes = new List<Cliente>();
            Empleados = new List<Empleado>();
            Habitaciones = new List<Habitacion>();
            Reservaciones = new List<Reservacion>();

            ProvinciasYCantones = new Dictionary<string, List<string>>
            {
                { "San José", new List<string> { "San José", "Escazú", "Desamparados", "Puriscal", "Tarrazú", "Aserrí", "Mora", "Goicoechea", "Santa Ana", "Alajuelita", "Vásquez de Coronado", "Acosta", "Tibás", "Moravia", "Montes de Oca", "Turrubares", "Dota", "Curridabat", "Pérez Zeledón", "León Cortés" } },
                { "Alajuela", new List<string> { "Alajuela", "San Ramón", "Grecia", "San Mateo", "Atenas", "Naranjo", "Palmares", "Poás", "Orotina", "San Carlos", "Zarcero", "Sarchí", "Upala", "Los Chiles", "Guatuso", "Río Cuarto" } },
                { "Cartago", new List<string> { "Cartago", "Paraíso", "La Unión", "Jiménez", "Turrialba", "Alvarado", "Oreamuno", "El Guarco" } },
                { "Heredia", new List<string> { "Heredia", "Barva", "Santo Domingo", "Santa Bárbara", "San Rafael", "San Isidro", "Belén", "Flores", "San Pablo", "Sarapiquí" } },
                { "Guanacaste", new List<string> { "Liberia", "Nicoya", "Santa Cruz", "Bagaces", "Carrillo", "Cañas", "Abangares", "Tilarán", "Nandayure", "La Cruz", "Hojancha" } },
                { "Puntarenas", new List<string> { "Puntarenas", "Esparza", "Buenos Aires", "Montes de Oro", "Osa", "Quepos", "Golfito", "Coto Brus", "Parrita", "Corredores", "Garabito", "Puerto Jiménez", "Monteverde" } },
                { "Limón", new List<string> { "Limón", "Pococí", "Siquirres", "Talamanca", "Matina", "Guácimo" } }
            };

            
            // REGISTROS POR DEFECTO 
            
            CargarDatosPorDefecto();
        }

        private void CargarDatosPorDefecto()
        {
            // 1. Cliente por defecto
            Clientes.Add(new Cliente
            {
                Identificacion = "1-1111-1111",
                Nombre = "Juan",
                PrimerApellido = "Pérez",
                SegundoApellido = "Mora",
                FechaNacimiento = new DateTime(1990, 5, 15)
            });

            // 2. Empleado por defecto
            Empleados.Add(new Empleado
            {
                Identificacion = "2-2222-2222",
                Nombre = "María",
                PrimerApellido = "Salas",
                SegundoApellido = "Gómez",
                FechaNacimiento = new DateTime(1985, 10, 20),
                SalarioMensual = 500000,
                FechaIngreso = new DateTime(2020, 1, 15),
                Categoria = CategoriaEmpleado.Administrador,
                Provincia = "Guanacaste",
                Canton = "Liberia",
                Distrito = "Centro",
                DireccionExacta = "Frente al parque central"
            });

            // 3. Habitación por defecto
            Habitaciones.Add(new Habitacion
            {
                NumeroHabitacion = 101,
                Tipo = TipoHabitacion.MasterStart,
                TarifaPorNoche = 150,
                TieneTVSatelital = true,
                PendientesMantenimiento = "Ninguno"
            });

            // 4. Reservación por defecto
            Reservaciones.Add(new Reservacion
            {
                Codigo = "RES-001",
                IdentificacionCliente = "1-1111-1111",
                NumeroHabitacion = 101,
                FechaIngreso = DateTime.Today.AddDays(5),
                FechaSalida = DateTime.Today.AddDays(10), // 5 Noches
                CantidadPersonas = 2,
                Estado = EstadoReservacion.Confirmada,

                
                FechaReservacion = DateTime.Today,
                SolicitudesEspeciales = "Cerca de la piscina",
                PorcentajeDescuento = 10,
                TarifaReservacion = 750,  // 5 noches * $150
                MontoTotal = 762.75m      // Subtotal con descuento (675) + 13% IVA
            });
        }

        public static RepositorioMemoria Instancia
        {
            get
            {
                if (_instancia == null) _instancia = new RepositorioMemoria();
                return _instancia;
            }
        }
    }
}



/* * DECLARACIÓN DE USO DE INTELIGENCIA ARTIFICIAL
 * Herramienta: Gemini (Google)
 * Propósito: Apoyo en la depuración de código, análisis de errores de compilación, 
 * estructuración de expresiones regulares para validación de formatos (Cédula/DIMEX)
 * y optimización de la estructura de diccionarios para el manejo en memoria.
 * Todo el código fue analizado, adaptado y comprendido por mi persona para cumplir 
 * con la arquitectura solicitada en el Proyecto 2.
 */