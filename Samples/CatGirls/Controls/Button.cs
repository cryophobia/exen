using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CatGirls.Controls
{
	class Button : Control
	{
		static Texture2D image;
		public static void LoadContent(ContentManager content)
		{
			image = content.Load<Texture2D>("CatGirlIcon");
		}

		public Button(Point topLeft, Color color)
		{
			this.Rectangle = new Rectangle(topLeft.X, topLeft.Y, image.Width, image.Height);
			this.Color = color;
		}

		public Rectangle Rectangle { get; private set; }

		public Color Color { get; set; }

		public event Action Clicked;

		public override void Tap(Point point)
		{
			if(Rectangle.Contains(point) && Clicked != null)
				Clicked();
		}

		public override void Draw(SpriteBatch sb)
		{
			sb.Draw(image, Rectangle, Color);
		}
	}
}
