//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 템플릿에서 생성되었습니다.
//
//     이 파일을 수동으로 변경하면 응용 프로그램에서 예기치 않은 동작이 발생할 수 있습니다.
//     이 파일을 수동으로 변경하면 코드가 다시 생성될 때 변경 내용을 덮어씁니다.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERPAPP.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblItem()
        {
            this.tblBOM = new HashSet<tblBOM>();
            this.tblOperation = new HashSet<tblOperation>();
            this.tblOrder = new HashSet<tblOrder>();
            this.tblPart = new HashSet<tblPart>();
        }
    
        public int ItemId { get; set; }
        public string ItemCode { get; set; }
        public string BrandCode { get; set; }
        public string ICateCode { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public byte[] ItemImage { get; set; }
        public Nullable<System.DateTime> RegDate { get; set; }
        public string RegID { get; set; }
        public Nullable<System.DateTime> ModDate { get; set; }
        public string ModID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBOM> tblBOM { get; set; }
        public virtual tblBrand tblBrand { get; set; }
        public virtual tblICate tblICate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblOperation> tblOperation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblOrder> tblOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPart> tblPart { get; set; }
    }
}
