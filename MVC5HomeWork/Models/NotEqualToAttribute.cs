using System;
using System.ComponentModel.DataAnnotations;

namespace MVC5HomeWork.Models
{
    internal class NotEqualToAttribute : DataTypeAttribute
    {
        public string OtherProperty { get; private set; }

        public NotEqualToAttribute(string otherProperty) : base(otherProperty)
        {
            OtherProperty = otherProperty;
        }

        public override bool IsValid(object value)
        {
            return value.ToString() != OtherProperty;
        }
    }
}