using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CatGirls.Tests;
using CatGirls.Controls;

namespace CatGirls
{
	public class CatGirlsGame : Microsoft.Xna.Framework.Game
	{
#if WINDOWS_PHONE
		public CatGirlsGame() : this(new Point(800, 480)) { }
#endif

		GraphicsDeviceManager graphics;

		public CatGirlsGame(Point size)
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.SupportedOrientations = DisplayOrientation.Portrait
					| DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
			
			graphics.PreferredBackBufferWidth = size.X;
			graphics.PreferredBackBufferHeight = size.Y;
#if WINDOWS_PHONE
			graphics.IsFullScreen = true;
#endif

			// Target 30 FPS
			TargetElapsedTime = TimeSpan.FromTicks(333333);

			IsMouseVisible = true;

			Content.RootDirectory = "Content";
		}


		SpriteBatch sb;
		SpriteFont font;

		protected override void LoadContent()
		{
			sb = new SpriteBatch(GraphicsDevice);

			// Load static content
			Test.LoadStaticContent(Content);
			Slider.LoadContent(Content);
			Button.LoadContent(Content);

			font = Content.Load<SpriteFont>("UIFont");
		
			menu = new TestMenu(GraphicsDevice, Content);
			menu.TestSelected += (type) => SetCurrentTest(type);

			base.LoadContent();
		}




		#region Test and Menu

		TestMenu menu;
		Test currentTest;

		void SetCurrentTest(Type testType)
		{
			if(testType.IsSubclassOf(typeof(Test)))
			{
				Test nextText = (Test)Activator.CreateInstance(testType);

				if(nextText != null)
				{
					ClearCurrentTest();
					
					currentTest = nextText;
					currentTest.Setup(this);
					currentTest.Initialize();
					currentTest.LoadContent();
					currentTest.BeginRun();
				}
			}
		}

		void ClearCurrentTest()
		{
			if(currentTest != null)
			{
				currentTest.EndRun();
				currentTest.UnloadContent();
				currentTest = null;
			}
		}

		#endregion


		#region Input Handling

		MouseState lastMouseState;

		void UpdateInput()
		{
			MouseState mouseState = Mouse.GetState();

			Point lastPoint = new Point(lastMouseState.X, lastMouseState.Y);
			Point point = new Point(mouseState.X, mouseState.Y);

			if(lastPoint != point)
			{
				OnMouseMove(point);
			}

			if(lastMouseState.LeftButton != mouseState.LeftButton)
			{
				if(mouseState.LeftButton == ButtonState.Pressed)
					OnMouseDown(point);
				else
					OnMouseUp(point);
			}

			lastMouseState = mouseState;
		}

		const int tapRadius = 3;
		Point mouseDownPoint;

		void OnMouseDown(Point point)
		{
			mouseDownPoint = point;

			if(currentTest != null)
				currentTest.MouseDown(point);
			else
				menu.MouseDown(point);
		}

		void OnMouseUp(Point point)
		{
			if(currentTest != null)
				currentTest.MouseUp(point);
			else
				menu.MouseUp(point);

			int dx = point.X - mouseDownPoint.X;
			int dy = point.Y - mouseDownPoint.Y;
			if((dx * dx) + (dy * dy) < tapRadius * tapRadius)
				OnTap(point);
		}

		void OnMouseMove(Point point)
		{
			if(currentTest != null)
				currentTest.MouseMove(point);
			else
				menu.MouseMove(point);
		}

		void OnTap(Point point)
		{
			if(currentTest != null)
			{
				if(currentTest.BackButton().Contains(point))
					ClearCurrentTest();
				else
					currentTest.Tap(point);
			}
			else
				menu.Tap(point);
		}

		#endregion


		public GameTime updateTime;

		protected override void Update(GameTime gameTime)
		{
			updateTime = gameTime;
			float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

			UpdateInput();

			if(currentTest != null)
			{
				currentTest.Update(seconds);
			}
			else
			{
				menu.Update(seconds);
			}

			base.Update(gameTime);
		}


		protected override void Draw(GameTime gameTime)
		{
			if(currentTest != null)
			{
				currentTest.Draw(sb);

				sb.Begin();
				sb.Draw(Test.BackIcon, currentTest.BackButton(), Color.White);
				sb.End();
			}
			else
			{
				GraphicsDevice.Clear(Color.White);
				menu.Draw(sb);
			}

			base.Draw(gameTime);
		}
	}
}
