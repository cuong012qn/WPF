//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyKho_MVVM.Models
{
    using System;
    using System.Collections.Generic;
    using QuanLyKho_MVVM.ViewModels;

    public partial class Unit : BaseViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Unit()
        {
            this.Objects = new HashSet<Object>();
        }

        private int _id;
        private string _displayName;

        public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }
        public string DisplayName { get => _displayName; set { _displayName = value; OnPropertyChanged(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        private ICollection<Object> objects;
        public virtual ICollection<Object> Objects { get => objects; set { objects = value; OnPropertyChanged(); } }
    }
}