using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.validation
{
    public class MinmumValue : ValidationAttribute
    {
        private readonly int _length;

        public MinmumValue(int length = 10)
        {
            _length = length;
        }
        public override bool IsValid(object? value)
        {
            if (value is decimal val)
            {
                if (val > _length)
                    return true;
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be greater than 10.";
        }
    }
}
