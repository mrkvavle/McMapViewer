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
    
    public partial class MapRevision
    {
        public MapRevision()
        {
            this.Chunks = new HashSet<Chunk>();
        }
    
        public int ID { get; set; }
        public int MapID { get; set; }
        public Nullable<bool> IsActiveMap { get; set; }
    
        public virtual ICollection<Chunk> Chunks { get; set; }
        public virtual Map Map { get; set; }
    }
}
