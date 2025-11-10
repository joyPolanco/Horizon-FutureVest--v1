using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class OperationResultDto
    {
        public required bool WasSuccessful { get; set; }

        public required string ResultMessage
        {
            get; set;
        }

    }
}
