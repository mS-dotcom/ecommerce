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
    
    public partial class Sepet
    {
        public int ID { get; set; }
        public int UrunID { get; set; }
        public int Adet { get; set; }
        public int ToplamTutar { get; set; }
        public string ResimYol { get; set; }
        public int MusteriID { get; set; }
    
        public virtual Musteri Musteri { get; set; }
        public virtual Urun Urun { get; set; }
    }
}
