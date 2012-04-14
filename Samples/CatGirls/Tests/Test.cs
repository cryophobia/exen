using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using CatGirls.Controls;

namespace CatGirls.Tests
{
	abstract class Test
	{

		#region Setup

		protected Game Game { get; private set; }
		protected GraphicsDevice GraphicsDevice { get { return Game.GraphicsDevice; } }
		protected ContentManager Content { get { return Game.Content; } }
		public void Setup(Game game)
		{
			this.Game = game;
		}

		#endregion


		#region Static Content

		protected static SpriteFont UIFont { get; private set; }
		protected static Texture2D CatGirlIcon { get; private set; }
		protected static Texture2D CatGirl { get; private set; }
		public static Texture2D BackIcon { get; private set; }
		public static Texture2D WhiteBox { get; private set; }

		public static void LoadStaticContent(ContentManager content)
		{
			UIFont = content.Load<SpriteFont>("UIFont");
			CatGirlIcon = content.Load<Texture2D>("CatGirlIcon");
			CatGirl = content.Load<Texture2D>("CatGirl");
			BackIcon = content.Load<Texture2D>("Back");
			WhiteBox = content.Load<Texture2D>("WhiteBox");
		}

		#endregion


		#region Virtual API

		public virtual void Initialize() { }
		public virtual void LoadContent() { }
		public virtual void UnloadContent() { }
		public virtual void BeginRun() { }
		public virtual void EndRun() { }
		public virtual void Update(float seconds) { }

		public virtual void Draw(SpriteBatch sb)
		{
			if(inputList.Count > 0)
			{
				sb.Begin();
				foreach(Control c in inputList)
					c.Draw(sb);
				sb.End();
			}
		}

		#endregion


		#region Input

		List<Control> inputList = new List<Control>();
		protected void AddControl(Control control)
		{
			inputList.Add(control);
		}


		public virtual void Tap(Point point)
		{
			foreach(Control c in inputList)
				c.Tap(point);
		}

		public virtual void MouseMove(Point point)
		{
			foreach(Control c in inputList)
				c.MouseMove(point);
		}

		public virtual void MouseUp(Point point)
		{
			foreach(Control c in inputList)
				c.MouseUp(point);
		}

		public virtual void MouseDown(Point point)
		{
			foreach(Control c in inputList)
				c.MouseDown(point);
		}

		#endregion


		public virtual Rectangle BackButton()
		{
			return new Rectangle(GraphicsDevice.Viewport.Width - BackIcon.Width, 0, BackIcon.Width, BackIcon.Height);
		}
	}
}
