using System.ComponentModel.DataAnnotations;

namespace RAML.WebApiExplorer.Tests.Types
{
    public class AnnotatedObject
    {
        public virtual string FirstName { get; set; }

        [Required]
        public Person Person { get; set; }

        [Required]
        public virtual string LastName { get; set; }
        
        public virtual string Address { get; set; }
        
        [MaxLength(255)]
        public virtual string City { get; set; }

        [MinLength(2)]
        public virtual string State { get; set; }
        public virtual string Country { get; set; }
        
        [RegularExpression("[a-z]{0,2}[0-9]{4,10}")]
        public virtual string PostalCode { get; set; }
        
        [Phone]
        public virtual string Phone { get; set; }
        public virtual string Fax { get; set; }

        [EmailAddress]
        public virtual string Email { get; set; }

        [Url]
        public virtual string Url { get; set; }

        [Range(18,120)]
        public int Age { get; set; }

        [Range(20.5, 300.5)]
        public decimal Weight { get; set; }
    }
}