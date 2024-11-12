using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp6
{
    public class Smartphone
    {
        public int Id { get; set; } // Первичный ключ
        public string Name { get; set; }
        public string Cost { get; set; }
        public string CPU { get; set; }
        public string Battery { get; set; }

        // Пустой конструктор, необходимый для EF
        public Smartphone() { }

        public Smartphone(string name, string cost, string cpu, string battery)
        {
            Name = name;
            Cost = cost;
            CPU = cpu;
            Battery = battery;
        }
    }
}
