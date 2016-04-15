using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace MVC5HomeWork.Models
{
    internal class PhoneNumberValidatableAttribute : DataTypeAttribute
    {
        public PhoneNumberValidatableAttribute() : base(DataType.Text)
        {
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            Regex rgx = new Regex(@"^\d{4}-\d{6}$");
            return rgx.IsMatch(value.ToString());
        }
    }
}