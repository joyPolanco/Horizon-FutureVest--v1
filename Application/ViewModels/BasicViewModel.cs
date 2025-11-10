using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class BasicViewModel<T>
    {

        
        public required T Id { get; set; }

        public required string Name { get; set; }
    }
}
