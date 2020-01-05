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

    public partial class Customer : BaseViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            this.OutputInfoes = new HashSet<OutputInfo>();
        }

        private int _id;
        private string _displayName;
        private string _address;
        private string _phone;
        private string _email;
        private string _moreInfo;
        private Nullable<DateTime> _contractDate;

        public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }
        public string DisplayName { get => _displayName; set { _displayName = value; OnPropertyChanged(); } }
        public string Address { get => _address; set { _address = value; OnPropertyChanged(); } }
        public string Phone { get => _phone; set { _phone = value; OnPropertyChanged(); } }
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }
        public string MoreInfo { get => _moreInfo; set { _moreInfo = value; OnPropertyChanged(); } }
        public Nullable<System.DateTime> ContractDate { get => _contractDate; set { _contractDate = value; OnPropertyChanged(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OutputInfo> OutputInfoes { get; set; }
    }
}