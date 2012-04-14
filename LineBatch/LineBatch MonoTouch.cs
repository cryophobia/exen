using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using OpenTK.Graphics.ES11;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Krab.Graphics
{
	public class LineBatch : IDisposable
	{

		#region Public API

		GraphicsDevice device;
		public GraphicsDevice GraphicsDevice { get { return device; } }


		float widthToActualRadius = 0.5f;
		

		public void Begin()
		{
			device.SetClientProjectionMatrix();
			GL.MatrixMode(All.Modelview);
			GL.LoadIdentity();

			widthToActualRadius = 0.5f;

			BeginCommon();
		}

		/// <param name="inverseScaleLineWidth">
		/// Set to the scale of the transform matrix to make lines stay the same width during zooming.
		/// </param>
		public void Begin(Matrix transform, float inverseScaleLineWidth)
		{
			device.SetClientProjectionMatrix();

			GL.MatrixMode(All.Modelview);
			GL.LoadMatrix(ref transform.M11); // would you believe that this is legal?

			widthToActualRadius = 0.5f / inverseScaleLineWidth;

			BeginCommon();
		}

		public void End()
		{
			if(!inBatch || disposed)
				throw new InvalidOperationException("LineBatch is in incorrect state");

			// Finish off any unrendered lines
			RenderLines();

			inBatch = false;
		}


		Vector2 lastPosition;

		public void LineFrom(Vector2 start)
		{
			if(!inBatch || disposed)
				throw new InvalidOperationException("LineBatch is in incorrect state");

			lastPosition = start;
		}

		public void LineTo(Vector2 position, Color color, float width)
		{
			if(!inBatch || disposed)
				throw new InvalidOperationException("LineBatch is in incorrect state");

			if(color != lastColor || !CanAddLine)
			{
				RenderLines();
				lastColor = color;
			}

			Vector2 direction = VectorHelper.SafeNormalize(position - lastPosition);
			AddLine(lastPosition, position, direction, width * widthToActualRadius);

			lastPosition = position;
		}

		public void LineFromTo(Vector2 start, Vector2 position, Color color, float width)
		{
			LineFrom(start);
			LineTo(position, color, width);
		}

		#endregion



		#region Construct, Dispose, Finalize

		public LineBatch(GraphicsDevice graphicsDevice, ContentManager contentManager)
		{
			device = graphicsDevice;

			unsafe
			{
				vertexArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Vector2)) * MaxVertexCount);
				indexArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ushort)) * MaxIndexCount);
			}

			InitializeIndexArray();
		}

		bool disposed = false;

		public void Dispose()
		{
			disposed = true;

			unsafe
			{
				Marshal.FreeHGlobal((IntPtr)vertexArray);
				vertexArray = IntPtr.Zero;
				Marshal.FreeHGlobal((IntPtr)indexArray);
				indexArray = IntPtr.Zero;
			}

			GC.SuppressFinalize(this);
		}

		~LineBatch()
		{
			Dispose();
		}

		#endregion


		#region Vertex and Index Array Management

		const int MaxLinesPerBatch = 128; // This number has no special significance - selected arbitrarily

		private IntPtr vertexArray; // Vector2[MaxVertexCount]
		private IntPtr indexArray; // ushort[MaxIndexCount]

		#region Vertex Template

		// This implementation has a hard-coded number of segments per end-cap at 6:
		const int VerticesPerLine = 14;
		const int MaxVertexCount = VerticesPerLine * MaxLinesPerBatch;

		// The vertices are arranged in an anti-clockwise circle, starting at the top of the left cap.
		const float sr3d2 = 0.8660254f; // sqrt(3)/2
		static readonly Vector2[] template =
		{
			new Vector2(    0f,     1f), //  0  ( -7)
			new Vector2( -0.5f,  sr3d2), //  1  ( -8)
			new Vector2(-sr3d2,   0.5f), //  2  ( -9)
			new Vector2(   -1f,     0f), //  3  (-10)
			new Vector2(-sr3d2,  -0.5f), //  4  (-11)
			new Vector2( -0.5f, -sr3d2), //  5  (-12)
			new Vector2(     0,     -1), //  6  (-13)
			// Second half of circle is merely the first half with -X and -Y
		};

		#endregion

		#region Vertex Management

		private int vertexNumber;
		private int LineCount { get { return vertexNumber / VerticesPerLine; } }

		private bool CanAddLine { get { return vertexNumber < MaxVertexCount; } }

		private unsafe void AddLine(Vector2 start, Vector2 end, Vector2 direction, float radius)
		{
			Vector2 perpendicular = new Vector2(direction.Y, -direction.X);

			for(int i = 0; i < template.Length; i++)
			{
				Vector2 v = start + (template[i].X * direction + template[i].Y * perpendicular) * radius;
				((Vector2*)vertexArray)[vertexNumber++] = v;
						
			}
			for(int i = 0; i < template.Length; i++)
			{
				Vector2 v = end - (template[i].X * direction + template[i].Y * perpendicular) * radius;
				((Vector2*)vertexArray)[vertexNumber++] = v;
			}
		}

		private void ClearLines() { vertexNumber = 0; }

		#endregion

		#region Index Array Setup

		const int IndicesPerLine = 36; // 12 primatives per line = 36 indicies
		const int MaxIndexCount = MaxLinesPerBatch * IndicesPerLine;

		private unsafe void InitializeIndexArray()
		{
			// NOTE: this function assumes VerticesPerLine and IndicesPerLine

			ushort* ia = (ushort*)indexArray;
			for(int i = 0; i < MaxLinesPerBatch; i++)
			{
				ia[i*36 +  0] = (ushort)(i*14 +  3); //  1 
				ia[i*36 +  1] = (ushort)(i*14 +  2);
				ia[i*36 +  2] = (ushort)(i*14 +  4);
				ia[i*36 +  3] = (ushort)(i*14 +  2); //  2 
				ia[i*36 +  4] = (ushort)(i*14 +  1);
				ia[i*36 +  5] = (ushort)(i*14 +  4);
				ia[i*36 +  6] = (ushort)(i*14 +  1); //  3 
				ia[i*36 +  7] = (ushort)(i*14 +  5);
				ia[i*36 +  8] = (ushort)(i*14 +  4);
				ia[i*36 +  9] = (ushort)(i*14 +  1); //  4 
				ia[i*36 + 10] = (ushort)(i*14 +  0);
				ia[i*36 + 11] = (ushort)(i*14 +  5);
				ia[i*36 + 12] = (ushort)(i*14 +  0); //  5 
				ia[i*36 + 13] = (ushort)(i*14 +  6);
				ia[i*36 + 14] = (ushort)(i*14 +  5);
				ia[i*36 + 15] = (ushort)(i*14 +  0); //  6 
				ia[i*36 + 16] = (ushort)(i*14 + 13);
				ia[i*36 + 17] = (ushort)(i*14 +  6);
				ia[i*36 + 18] = (ushort)(i*14 + 13); //  7 
				ia[i*36 + 19] = (ushort)(i*14 +  7);
				ia[i*36 + 20] = (ushort)(i*14 +  6);
				ia[i*36 + 21] = (ushort)(i*14 + 13); //  8 
				ia[i*36 + 22] = (ushort)(i*14 + 12);
				ia[i*36 + 23] = (ushort)(i*14 +  7);
				ia[i*36 + 24] = (ushort)(i*14 + 12); //  9 
				ia[i*36 + 25] = (ushort)(i*14 +  8);
				ia[i*36 + 26] = (ushort)(i*14 +  7);
				ia[i*36 + 27] = (ushort)(i*14 + 12); // 10 
				ia[i*36 + 28] = (ushort)(i*14 + 11);
				ia[i*36 + 29] = (ushort)(i*14 +  8);
				ia[i*36 + 30] = (ushort)(i*14 + 11); // 11 
				ia[i*36 + 31] = (ushort)(i*14 +  9);
				ia[i*36 + 32] = (ushort)(i*14 +  8);
				ia[i*36 + 33] = (ushort)(i*14 + 11); // 12 
				ia[i*36 + 34] = (ushort)(i*14 + 10);
				ia[i*36 + 35] = (ushort)(i*14 +  9);
			}
		}

		#endregion

		#endregion



		#region MonoTouch Rendering

		public void BeginCommon()
		{
			if(inBatch || disposed)
				throw new InvalidOperationException("LineBatch is in incorrect state");

			GL.Disable(All.DepthTest);
			GL.Disable(All.CullFace);
			GL.EnableClientState(All.VertexArray);

			device.RenderState.AlphaBlendEnable = true;
			device.RenderState.SetBlendState(Blend.SourceAlpha, Blend.InverseSourceAlpha);

			inBatch = true;
		}

		bool inBatch = false;
		Color lastColor;

		private unsafe void RenderLines()
		{
			if(LineCount == 0)
				return;

			// Expects GL.Disable(All.Texture2D) and GL.DisableClientState(All.TextureCoordArray)
			// (these are enabled and then disabled in SpriteBatch

			// Set the glColor to apply alpha to the image
			Vector4 color = lastColor.ToVector4();
			GL.Color4(color.X, color.Y, color.Z, color.W);

			// Set up the VertexPointer to point to the vertices we have defined
			GL.VertexPointer(2, All.Float, 8, vertexArray);

			// Draw the vertices to the screen
			GL.DrawElements(All.Triangles, LineCount * IndicesPerLine, All.UnsignedShort, indexArray);

			ClearLines();
		}

		#endregion

	}
}
