using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Microsoft.Xna.Framework
{
	public partial class GraphicsDeviceManager : IGraphicsDeviceService, IDisposable, IGraphicsDeviceManager
	{
		public static readonly int DefaultBackBufferWidth = 800;
		public static readonly int DefaultBackBufferHeight = 600;


		public GraphicsDevice GraphicsDevice { get; private set; }


		#region Device Setup

		public void CreateDevice()
		{
			GraphicsDevice = new GraphicsDevice(game);
			ApplyChanges();
			OnDeviceCreated(this, EventArgs.Empty);
		}

		public void ApplyChanges()
		{
			GraphicsDevice.Viewport = new Viewport()
			{
				X = 0,
				Y = 0,
				Width = PreferredBackBufferWidth,
				Height = PreferredBackBufferHeight,
			};
			GraphicsDevice.Root.Width = PreferredBackBufferWidth;
			GraphicsDevice.Root.Height = PreferredBackBufferHeight;
			GraphicsDevice.UpdatePresentationParameters();

			// Setup touch info
			TouchPanel.DisplayWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
			TouchPanel.DisplayHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
			TouchPanel.DisplayOrientation = GraphicsDevice.PresentationParameters.DisplayOrientation;
		}

		#endregion


		#region Drawing

		public bool BeginDraw()
		{
			return true;
		}

		public void EndDraw()
		{
			GraphicsDevice.EndSpriteFrame();
		}

		#endregion

	}
}
