using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Country
{
    public class SaveCountryViewModel
    {

        public  int Id { get; set; }

        [Required(ErrorMessage ="Se debe ingresar el nombre")]
        [MinLength(3, ErrorMessage = "El nombre del país debe tener una longitud mínima de 3 caracteres")]

        public required string Name { get; set; }


        [Required(ErrorMessage = "Se debe ingresar el código ISO")]
        [MinLength(2,ErrorMessage ="El código ISO debe tener una longitud mínima de 2 caracteres")]
        [MaxLength(3, ErrorMessage = "El código ISO debe tener una longitud máxima de 3 caracteres")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "El código solo debe contener letras.")]

        public required string IsoCode { get; set; }
        public bool IsEditMode { get; set; }
    }
}
