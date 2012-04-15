using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
	public class SamplerState
	{
		public static readonly SamplerState AnisotropicClamp = new SamplerState();
		public static readonly SamplerState AnisotropicWrap = new SamplerState();	
		public static readonly SamplerState LinearClamp = new SamplerState();		
		public static readonly SamplerState LinearWrap = new SamplerState();		
		public static readonly SamplerState PointClamp = new SamplerState();		
		public static readonly SamplerState PointWrap = new SamplerState();
	}
}
