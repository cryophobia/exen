using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using ExEnSilver.Renderer;
using Krab.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SWM=System.Windows.Media;
using System.Diagnostics;
using Krab.Silverlight;
using Microsoft.Xna.Framework.Content;

namespace Krab.Silverlight
{
	public class LineSprite : Sprite
	{
		Polyline polyline;

		SWM.SolidColorBrush brush;


		#region Performance Notes

		// Notes about Caching
		//
		// With Caching:
		// - The Silverlight Renderer is fast enough
		// - Getting rendered data to the GPU is fast enough
		// - Reallocating the surface on the GPU is SLOW
		// - Reallocation is not necessary if the rendered area does not change size
		// - The rendered area is very likely to change size!
		//
		// - I don't know how to prevent the rendered area from changing size
		//     (maybe could try caching the canvas with a clip, but... effort!)
		//
		// Conclusion: don't cache!
		// - What does not change size is the Silverlight control itself
		// - Therefore: simply don't use caching and let Silverlight make a full-plugin surface
		//   - Big Benefit: Silverlight will automatically group together all uncached operations onto the one surface
		//   - This will use excess texture memory if the plugin is bigger than the game (eg: iPhone border) - but not much
		//   - Can cause multiple full-plugin surfaces unless careful with keeping cached things together in Z-order
		//     (In CSA: this means not caching hands (rendered on CPU anyway) and body (tiny!))
		//
		// Because this is uncached, GraphicsDevice.HintClipEnable should not be used.
		//   (see performance notes for that function)
		//


		// Notes about render transform
		//
		// - Takes about a 10% performance hit (actually not bad)
		// - It is possible to just transform the points - but that is effort
		//   (also doesn't exactly match XNA for things like skew matrices)

		#endregion


		public LineSprite(bool withTransform)
		{
			polyline = new Polyline();

			polyline.Stroke = brush = new SWM.SolidColorBrush(Color.Black.ToSilverlightColor());
			polyline.StrokeLineJoin = SWM.PenLineJoin.Round;
			polyline.StrokeStartLineCap = SWM.PenLineCap.Round;
			polyline.StrokeEndLineCap = SWM.PenLineCap.Round;

			base.Create(polyline, false, withTransform); // Cannot cache line rendering
		}



		public static int EncodeWidth(float width) { return (int)(width * 8f); }
		public static double DecodeWidth(int encodedWidth) { return (double)encodedWidth / 8.0; }

		private Color color = Color.Black;
		public Color Color { get { return color; } }

		private int encodedWidth;
		public int EncodedWidth { get { return encodedWidth; } }

		public void SetStyle(Color color, int encodedWidth)
		{
			brush.Color = color.ToSilverlightColor();
			polyline.StrokeThickness = DecodeWidth(encodedWidth);
			this.color = color;
			this.encodedWidth = encodedWidth;
		}

		public void SetTransform(ref SWM.Matrix matrix)
		{
			transform.Matrix = matrix;
		}


		public void Clear()
		{
			polyline.Points.Clear();
		}

		public void Add(Vector2 point)
		{
			// Note: reflector tells me that modifing points in the middle of the list will be slow
			polyline.Points.Add(new System.Windows.Point(point.X, point.Y));
		}

	}
}


namespace Krab.Graphics
{
	public class LineBatch : IDisposable
	{

		#region Public API

		public GraphicsDevice GraphicsDevice { get; private set; }


		public LineBatch(GraphicsDevice graphicsDevice, ContentManager contentManager)
		{
			GraphicsDevice = graphicsDevice;
		}

		public void Dispose()
		{
			// TODO: Implement this
		}


		public void Begin()
		{
			hasTransform = false;
			CheckForBufferResetCondition();
		}

		public void Begin(Matrix transform)
		{
			hasTransform = true;
			Debug.Assert(transform.IsAffineMatrix);
			this.transform = transform.AsSilverlightMatrix;
			CheckForBufferResetCondition();
		}

		public void End()
		{
			CompleteCurrentSprite();
		}


		Vector2 lastPosition;

		public void LineFrom(Vector2 start)
		{
			CompleteCurrentSprite();
			lastPosition = start;
		}

		public void LineTo(Vector2 position, Color color, float width)
		{
			int encodedWidth = LineSprite.EncodeWidth(width);

			if(current == null || current.Color != color || current.EncodedWidth != encodedWidth)
			{
				StartNewSprite(color, encodedWidth);
				current.Add(lastPosition);
			}

			current.Add(position);
			lastPosition = position;
		}

		public void LineFromTo(Vector2 start, Vector2 position, Color color, float width)
		{
			LineFrom(start);
			LineTo(position, color, width);
		}

		#endregion



		#region Sprite Management

		bool hasTransform;
		SWM.Matrix transform;


		List<LineSprite> untransformedBuffer = new List<LineSprite>();
		int untransformedBufferPosition = 0;

		List<LineSprite> transformedBuffer = new List<LineSprite>();
		int transformedBufferPosition = 0;

		
		int bufferPositionsWereResetOnFrame = 0;

		void CheckForBufferResetCondition()
		{
			if(GraphicsDevice.CurrentSpriteFrameNumber > bufferPositionsWereResetOnFrame)
			{
				transformedBufferPosition = 0;
				untransformedBufferPosition = 0;

				bufferPositionsWereResetOnFrame = GraphicsDevice.CurrentSpriteFrameNumber;
			}
		}


		LineSprite current = null;

		void StartNewSprite(Color color, int encodedWidth)
		{
			if(current != null)
				CompleteCurrentSprite();

			// Where to get sprite from
			int i;
			List<LineSprite> list;
			if(hasTransform)
			{
				i = transformedBufferPosition;
				list = transformedBuffer;
			}
			else
			{
				i = untransformedBufferPosition;
				list = untransformedBuffer;
			}

			// Get sprite or create it
			if(i == list.Count)
			{
				current = new LineSprite(hasTransform);
				list.Add(current);
				GraphicsDevice.AddSprite(current);
			}
			else
			{
				current = list[i];
			}

			// Set the style, transform 
			// And advance the position in the list of lines
			current.SetStyle(color, encodedWidth);
			if(hasTransform)
			{
				current.SetTransform(ref transform);
				transformedBufferPosition++;
			}
			else
			{
				untransformedBufferPosition++;
			}

			// Remove old data from the sprite
			current.Clear();
		}

		void CompleteCurrentSprite()
		{
			if(current != null)
			{
				GraphicsDevice.DrawSprite(current);
				current = null;
			}
		}

		#endregion


	}
}
