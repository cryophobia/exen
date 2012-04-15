using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
	public class DepthStencilState
	{
		public static readonly DepthStencilState Default = new DepthStencilState();
		public static readonly DepthStencilState DepthRead = new DepthStencilState();
		public static readonly DepthStencilState None = new DepthStencilState();
	}
}
