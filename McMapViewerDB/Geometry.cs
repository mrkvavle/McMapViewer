//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace McMapViewerDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Geometry
    {
        public Geometry()
        {
            this.Faces = new HashSet<Face>();
        }
    
        public int ID { get; set; }
        public int ChunkID { get; set; }
        public int MaterialID { get; set; }
    
        public virtual Chunk Chunk { get; set; }
        public virtual ICollection<Face> Faces { get; set; }
        public virtual Material Material { get; set; }
    }
}
