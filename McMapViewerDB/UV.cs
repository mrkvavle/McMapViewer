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
    
    public partial class UV
    {
        public int MapRevisionID { get; set; }
        public int OrderID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    
        public virtual FaceVertexUV FaceVertexUV { get; set; }
        public virtual FaceVertexUV FaceVertexUV1 { get; set; }
        public virtual FaceVertexUV FaceVertexUV2 { get; set; }
    }
}
