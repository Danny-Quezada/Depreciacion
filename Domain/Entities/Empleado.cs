using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Empleado
    {
        public int Id { get; set; }
        public String Cedula { get; set; }

        public String Nombre { get; set; }
        public String Apellido { get; set; }
        public String Direccion { get; set; }
        public String Telefono { get; set; }
        public String Email { get;set }
        public Estado Estado { get; set; }

    }
}
