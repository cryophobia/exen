using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
	public class RasterizerState
	{
		public static readonly RasterizerState CullClockwise = new RasterizerState();
		public static readonly RasterizerState CullCounterClockwise = new RasterizerState();
		public static readonly RasterizerState CullNone = new RasterizerState();
	}
}
