using System;
using System.Drawing;

namespace Microsoft.Xna.Framework
{
	internal static class PointFExtensions
	{
		public static Vector2 AsXnaVector2(this PointF p)
		{
			return new Vector2(p.X, p.Y);
		}

		public static Microsoft.Xna.Framework.Point AsXnaPoint(this PointF p)
		{
			return new Microsoft.Xna.Framework.Point((int)p.X, (int)p.Y);
		}
	}
}
