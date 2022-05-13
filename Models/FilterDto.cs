using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace testmvc.Models
{
    public class FilterDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public int CreatedBy { get; set; }

        public int? Countryid { get; set; }

        public int? Stateid { get; set; }

        public bool? IsActive { get; set; }
    }
}
