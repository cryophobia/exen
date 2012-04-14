using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#if MONOTOUCH || ANDROID
using OpenTK.Graphics.ES11;
#endif

namespace BlankGame
{
	public class BlankGameGame : Game
	{
		protected GraphicsDeviceManager graphics;

		public BlankGameGame()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 320;
			graphics.PreferredBackBufferHeight = 480;

			IsMouseVisible = true;
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

#if MONOTOUCH || ANDROID
			GL.MatrixMode (All.Projection);
			GL.LoadIdentity ();
			GL.Ortho (-1.0f, 1.0f, -1.5f, 1.5f, -1.0f, 1.0f);
			GL.MatrixMode (All.Modelview);
			GL.Rotate (3.0f, 0.0f, 0.0f, 1.0f);

			GL.VertexPointer (2, All.Float, 0, square_vertices);
			GL.EnableClientState (All.VertexArray);
			GL.ColorPointer (4, All.UnsignedByte, 0, square_colors);
			GL.EnableClientState (All.ColorArray);

			GL.DrawArrays (All.TriangleStrip, 0, 4);
#endif

			base.Draw(gameTime);
		}
		
		
		float[] square_vertices = {
			-0.5f, -0.5f,
			0.5f, -0.5f,
			-0.5f, 0.5f, 
			0.5f, 0.5f,
		};

		byte[] square_colors = {
			255, 255,   0, 255,
			0,   255, 255, 255,
			0,     0,    0,  0,
			255,   0,  255, 255,
		};

	}
}
