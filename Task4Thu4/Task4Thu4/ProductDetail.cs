//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Task4Thu4
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductDetail
    {
        public int ID { get; set; }
        public int IDProduct { get; set; }
        public int IDTax { get; set; }
        public int IDDiscount { get; set; }
        public double WholePrice { get; set; }
        public double RetailPrice { get; set; }
        public double InputPrice { get; set; }
    
        public virtual Discount Discount { get; set; }
        public virtual Product Product { get; set; }
        public virtual Tax Tax { get; set; }
    }
}