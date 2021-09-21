using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class MusicoModel
    {
        public int Id { get; set; }
        [Display(Name = "PrimeiroNome")]
        public string Nome { get; set; }
        [Display(Name = "Ultimo Nome")]
        public string UltimoNome { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Data de nascimento")]
        public DateTime Nascimento { get; set; }
        [Display(Name = "toca quantos instrumentos")]
        [Required]
        public int QuantosInstrumentos { get; set; }
        [Display(Name = "Possui Banda")]
        public bool PossuiBanda { get; set; }

        [Display(Name = "Foto")]
        public string ImageUri { get; set; }

        [Display(Name = "Visualizações")]
        public int Visualizacao { get; set; }
    }
}
