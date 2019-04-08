// Template: Models (ApiModel.t4) version 3.0

// This code was generated by AMF Server Scaffolder

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace MoviesWebApiSample.MoviesV1.Models
{
    public partial class Movie
    {
        

        [Required]
        [MaxLength(0)]
        [MinLength(0)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [MinLength(0)]
        public string Name { get; set; }

        [Required]
        [MaxLength(0)]
        [MinLength(0)]
        public string Director { get; set; }

        [Required]
        [MaxLength(0)]
        [MinLength(0)]
        public string Genre { get; set; }

        [Required]
        [MaxLength(0)]
        [MinLength(0)]
        public string Cast { get; set; }

        [MaxLength(0)]
        [MinLength(0)]
        [Range(1.00,double.MaxValue)]
        public decimal Duration { get; set; }

        [MaxLength(0)]
        [MinLength(0)]
        public string Storyline { get; set; }

        [Required]
        [MaxLength(0)]
        [MinLength(0)]
        public string Language { get; set; }

        [MaxLength(0)]
        [MinLength(0)]
        public bool Rented { get; set; }
    } // end class

} // end Models namespace
