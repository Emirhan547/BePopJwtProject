using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.Business.Dtos.Common
{
    public abstract class BaseDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
