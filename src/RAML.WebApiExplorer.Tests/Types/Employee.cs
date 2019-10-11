using System;

namespace RAML.WebApiExplorer.Tests.Types
{
    public class Employee : Person
    {
        public virtual int Id { get; set; }

        public virtual string Title { get; set; }
        public virtual DateTime BirthDate { get; set; }
        public virtual DateTime HireDate { get; set; }
        public virtual Employee ReportsTo { get; set; }
        
    }
}