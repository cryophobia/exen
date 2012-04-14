using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CatGirls
{
	static class FrameworkExtensions
	{
		//
		// XNA
		//

		#region Vector2

		public static Vector2 Floor(this Vector2 vector)
		{
			return new Vector2((float)Math.Floor(vector.X), (float)Math.Floor(vector.Y));
		}

		public static Point AsPoint(this Vector2 v)
		{
			return new Point((int)v.X, (int)v.Y);
		}

		#endregion


		#region Point

		public static Vector2 AsVector2(this Point p)
		{
			return new Vector2(p.X, p.Y);
		}

		#endregion


		#region Texture2D

		public static Vector2 Center(this Texture2D texture)
		{
			return new Vector2(texture.Width/2f, texture.Height/2f);
		}

		public static Vector2 Size(this Texture2D texture)
		{
			return new Vector2(texture.Width, texture.Height);
		}

		#endregion


		//
		// .NET
		//

		#region Single (float)

		public static float Floor(this float v)
		{
			return (float)Math.Floor(v);
		}

		#endregion

	}
}
