//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BolsaDePapel.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MusteriAdres
    {
        public int Id { get; set; }
        public int MusteriId { get; set; }
        public string Adres { get; set; }
        public string Adi { get; set; }
    
        public virtual Musteri Musteri { get; set; }
    }
}
