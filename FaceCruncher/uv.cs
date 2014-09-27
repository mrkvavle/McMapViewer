using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FaceCruncher
{
	public class UV
	{
		public double X;
		public double Y;
		public UV(double x, double y ) 
		{
			X = x;
			Y = y;
		}
		public override string ToString(){
			return	String.Format("{0:0.0000}", X) + " " + 
					String.Format("{0:0.0000}", Y);
		}
	}
}
