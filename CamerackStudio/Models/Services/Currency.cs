using System;
using System.ComponentModel.DataAnnotations;

namespace CamerackStudio.Models.Services
{
    public class Currency
    {
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
        public class MaxDecimalValueAttribute : ValidationAttribute
        {
            private double maximum;
            public MaxDecimalValueAttribute(double maxVal)
                : base("Camerack does not support Images with Prices above N500,000.")
            {
                maximum = maxVal;
            }
            public override bool IsValid(object value)
            {
                var stringValue = value as string;
                double numericValue;
                if (stringValue == null)
                    return false;
                else if (!Double.TryParse(stringValue, out numericValue) || numericValue > maximum)
                {
                    return false;
                }
                return true;
            }
        }

        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
        public class MinDecimalValueAttribute : ValidationAttribute
        {
            private double minimum;
            public MinDecimalValueAttribute(double minVal)
                : base("Camerack does not support Images with Prices below 1000")
            {
                minimum = minVal;
            }
            public override bool IsValid(object value)
            {
                var stringValue = value as string;
                double numericValue;
                if (stringValue == null)
                    return false;
                else if (!Double.TryParse(stringValue, out numericValue) || numericValue < minimum)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
