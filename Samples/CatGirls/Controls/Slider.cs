using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CatGirls.Controls
{
	class Slider : Control
	{
		static Texture2D background, handle;
		public static void LoadContent(ContentManager content)
		{
			background = content.Load<Texture2D>("gradient");
			handle = content.Load<Texture2D>("handle");
		}

		public Slider(Rectangle rectangle, Color color, float initialValue)
		{
			this.Rectangle = rectangle;

			inputStartRectangle = new Rectangle(rectangle.X,
					rectangle.Y - inputMargin, rectangle.Width,
					rectangle.Height + inputMargin * 2);
			inputRectangle = new Rectangle(rectangle.X - inputMargin,
					rectangle.Y - inputMargin, rectangle.Width + inputMargin * 2,
					rectangle.Height + inputMargin * 2);
			
			this.Color = color;
			this.value = initialValue;
		}

		const int inputMargin = 64;
		public Rectangle Rectangle { get; private set; }
		Rectangle inputStartRectangle;
		Rectangle inputRectangle;

		public Color Color { get; set; }

		bool mouseWentDownInside;

		float oldValue;
		float value;
		public float Value { get { return value; } }


		#region Input

		void UpdateValue(Point point)
		{
			if(inputRectangle.Contains(point))
			{
				int p = Rectangle.Height - (point.Y - Rectangle.Y);
				if(p < 0)
					p = 0;
				if(p > Rectangle.Height)
					p = Rectangle.Height;
				value = (float)p/(float)Rectangle.Height;
			}
			else
			{
				value = oldValue;
			}
		}

		public override void MouseDown(Point point)
		{
			if(inputStartRectangle.Contains(point))
			{
				mouseWentDownInside = true;
				UpdateValue(point);
			}
		}

		public override void MouseUp(Point point)
		{
			if(mouseWentDownInside)
			{
				oldValue = value;
			}
			mouseWentDownInside = false;
		}

		public override void MouseMove(Point point)
		{
			if(mouseWentDownInside)
			{
				UpdateValue(point);
			}
		}

		#endregion


		public override void Draw(SpriteBatch sb)
		{
			sb.Draw(background, Rectangle, Color);

			Vector2 position = new Vector2(Rectangle.X + Rectangle.Width/2,
				Rectangle.Y + Rectangle.Height - (float)Math.Floor((value * Rectangle.Height)));
			sb.Draw(handle, position, null, Color.White, 0, handle.Center().Floor(), 1f, SpriteEffects.None, 0f);
		}
	}
}
