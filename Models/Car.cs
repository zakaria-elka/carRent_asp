//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Car
    {
        public int Id { get; set; }
        public string categories { get; set; }
        public string brand { get; set; }
        public int price { get; set; }
        public string Rnum { get; set; }
        public byte[] image { get; set; }
        public System.DateTime cdate { get; set; }
        public string carburant { get; set; }
        public bool reserve { get; set; }
        public string models { get; set; }
    }
}
