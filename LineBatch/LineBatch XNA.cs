using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Krab.XNA;
using Microsoft.Xna.Framework.Content;

namespace Krab.Graphics
{
	public class LineBatch : IDisposable
	{
		RoundLineManager rlm;

		LineInstanceData instanceData;
		int instanceCount;


		#region Public API

		public GraphicsDevice GraphicsDevice { get; private set; }


		public LineBatch(GraphicsDevice graphicsDevice, ContentManager contentManager)
		{
			GraphicsDevice = graphicsDevice;
			rlm = new RoundLineManager(graphicsDevice, contentManager);

			instanceData.Initialize();
			instanceCount = 0;
		}

		public void Dispose()
		{
			rlm.Dispose();
		}


		public void Begin()
		{
			instanceCount = 0;
			Viewport viewport = GraphicsDevice.Viewport;

			// 2f to convert width to radius
			rlm.Begin(Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, -1, 1), 2f);
		}

		/// <param name="inverseScaleLineWidth">
		/// Set to the scale of the transform matrix to make lines stay the same width during zooming.
		/// </param>
		public void Begin(Matrix transform, float inverseScaleLineWidth)
		{
			instanceCount = 0;
			Viewport viewport = GraphicsDevice.Viewport;

			// mutliply by 2f is to convert width to radius
			rlm.Begin(transform * Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, -1, 1), inverseScaleLineWidth * 2f);
		}

		public void End()
		{
			if(instanceCount > 0)
				FinishBatch();
			rlm.End();
		}


		Vector2 lastPosition;

		public void LineFrom(Vector2 start)
		{
			lastPosition = start;
		}

		public void LineTo(Vector2 position, Color color, float width)
		{
			if(instanceCount == LineInstanceData.Count)
				FinishBatch();

			instanceData.SetElement(instanceCount++, lastPosition, position, color, width);
			lastPosition = position;
		}

		public void LineFromTo(Vector2 start, Vector2 position, Color color, float width)
		{
			LineFrom(start);
			LineTo(position, color, width);
		}

		#endregion


		private void FinishBatch()
		{
			rlm.Draw(instanceData, instanceCount);
			instanceCount = 0;
		}

	}
}
