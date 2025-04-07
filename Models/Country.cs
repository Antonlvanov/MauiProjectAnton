using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiProjectAnton
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Capital { get; set; } = string.Empty;
        public int? Population { get; set; }
        public string? Flag { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
    }
}
