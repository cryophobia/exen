// RoundLine.cs
// Originally By Michael D. Anderson
// Based on Version 3.00, Mar 12 2009
// Modified by Andrew Russell
//
// A class to efficiently draw thick lines with rounded ends.

#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion


namespace Krab.XNA
{
	internal struct LineInstanceData
	{
		public static int Count = RoundLineManager.LinesPerBatch;

		public Vector4[] positionData;
		public Vector4[] colorData;
		public Vector4[] radiusData; // Packed 4 radii per Vector4

		public void Initialize()
		{
			// The number of lines per batch must be evenly divisble by four
			Debug.Assert(Count % 4 == 0);

			positionData = new Vector4[Count];
			colorData = new Vector4[Count];
			radiusData = new Vector4[Count/4];
		}

		public void SetElement(int i, Vector2 start, Vector2 end, Color color, float radius)
		{
			Debug.Assert(i < Count);

			positionData[i] = new Vector4(start.X, start.Y, end.X, end.Y);
			colorData[i] = color.ToVector4();

			switch(i%4)
			{
				case 0: radiusData[i/4].X = radius; break;
				case 1: radiusData[i/4].Y = radius; break;
				case 2: radiusData[i/4].Z = radius; break;
				case 3: radiusData[i/4].W = radius; break;
			}
		}
	}

	public class LineBuffer
	{
		const int SubLength = RoundLineManager.LinesPerBatch;

		internal List<LineInstanceData> subBuffers = new List<LineInstanceData>();
		internal int count = 0;

		private void Extend()
		{
			LineInstanceData sb = new LineInstanceData();
			sb.Initialize();
			subBuffers.Add(sb);
		}

		public void Add(Vector2 start, Vector2 end, Color color, float radius)
		{
			while(count + 1 > subBuffers.Count * SubLength)
				Extend();

			int i = count % SubLength;
			int j = count / SubLength;

			subBuffers[j].SetElement(i, start, end, color, radius);

			++count;
		}

		public void Add(Vector2 point, Color color, float radius)
		{
			Add(point, point, color, radius);
		}

		public void Clear()
		{
			count = 0;
		}
	}


	// A vertex type for drawing RoundLines, including an instance index
	struct RoundLineVertex : IVertexType
	{
		public Vector3 pos;
		public Vector2 rhoTheta;
		public Vector2 scaleTrans;
		public float index;

		public RoundLineVertex(Vector3 pos, Vector2 norm, Vector2 tex, float index)
		{
			this.pos = pos;
			this.rhoTheta = norm;
			this.scaleTrans = tex;
			this.index = index;
		}

		public static VertexDeclaration VertexDeclaration = new VertexDeclaration
		(
			new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
			new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.Normal, 0),
			new VertexElement(20, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
			new VertexElement(28, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 1)
		);

		VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }
	}


	/// <summary>
	/// Class to handle drawing a list of RoundLines.
	/// </summary>
	public class RoundLineManager : IDisposable
	{
		public GraphicsDevice GraphicsDevice { get { return device; } }


		public RoundLineManager(GraphicsDevice graphicsDevice, ContentManager content)
		{
			this.device = graphicsDevice;

			effect = content.Load<Effect>("RoundLine");
			effect.CurrentTechnique = effect.Techniques[0];
			effectPass = effect.CurrentTechnique.Passes[0];

			viewProjMatrixParameter = effect.Parameters["viewProj"];
			instancePositionParameter = effect.Parameters["instancePosition"];
			instanceColorParameter = effect.Parameters["instanceColor"];
			instanceRadiusParameter = effect.Parameters["instanceRadius"];
			inverseScaleRadiusParameter = effect.Parameters["inverseScaleRadius"];

			CreateRoundLineMesh();
		}

		public void Dispose()
		{
			if(vb != null)
				vb.Dispose();
			vb = null;

			if(ib != null)
				ib.Dispose();
			ib = null;
		}



		private GraphicsDevice device;
		private Effect effect;
		private EffectPass effectPass;
		private EffectParameter viewProjMatrixParameter;
		private EffectParameter inverseScaleRadiusParameter;
		private EffectParameter instancePositionParameter;
		private EffectParameter instanceRadiusParameter;
		private EffectParameter instanceColorParameter;
		private VertexBuffer vb;
		private IndexBuffer ib;
		internal const int LinesPerBatch = 104; // Must match the value in RoundLine.fx
		private int numVertices;
		private int numIndices;
		private int numPrimitivesPerInstance;
		private int numPrimitives;


		/// <summary>
		/// Create a mesh for a RoundLine.
		/// </summary>
		/// <remarks>
		/// The RoundLine mesh has 3 sections:
		/// 1.  Two quads, from 0 to 1 (left to right)
		/// 2.  A half-disc, off the left side of the quad
		/// 3.  A half-disc, off the right side of the quad
		///
		/// The X and Y coordinates of the "normal" encode the rho and theta of each vertex
		/// The "texture" encodes whether to scale and translate the vertex horizontally by length and radius
		/// </remarks>
		private void CreateRoundLineMesh()
		{
			const int primsPerCap = 6; // A higher primsPerCap produces rounder endcaps at the cost of more vertices
			const int verticesPerCap = primsPerCap * 2 + 2;
			const int primsPerCore = 4;
			const int verticesPerCore = 8;

			numVertices = (verticesPerCore + verticesPerCap + verticesPerCap) * LinesPerBatch;
			numPrimitivesPerInstance = primsPerCore + primsPerCap + primsPerCap;
			numPrimitives = numPrimitivesPerInstance * LinesPerBatch;
			numIndices = 3 * numPrimitives;
			short[] indices = new short[numIndices];
			RoundLineVertex[] tri = new RoundLineVertex[numVertices];

			int iv = 0;
			int ii = 0;
			int iVertex;
			int iIndex;
			for (int instance = 0; instance < LinesPerBatch; instance++)
			{
				// core vertices
				const float pi2 = MathHelper.PiOver2;
				const float threePi2 = 3 * pi2;
				iVertex = iv;
				tri[iv++] = new RoundLineVertex(new Vector3(0.0f, -1.0f, 0), new Vector2(1, threePi2), new Vector2(0, 0), instance);
				tri[iv++] = new RoundLineVertex(new Vector3(0.0f, -1.0f, 0), new Vector2(1, threePi2), new Vector2(0, 1), instance);
				tri[iv++] = new RoundLineVertex(new Vector3(0.0f, 0.0f, 0), new Vector2(0, threePi2), new Vector2(0, 1), instance);
				tri[iv++] = new RoundLineVertex(new Vector3(0.0f, 0.0f, 0), new Vector2(0, threePi2), new Vector2(0, 0), instance);
				tri[iv++] = new RoundLineVertex(new Vector3(0.0f, 0.0f, 0), new Vector2(0, pi2), new Vector2(0, 1), instance);
				tri[iv++] = new RoundLineVertex(new Vector3(0.0f, 0.0f, 0), new Vector2(0, pi2), new Vector2(0, 0), instance);
				tri[iv++] = new RoundLineVertex(new Vector3(0.0f, 1.0f, 0), new Vector2(1, pi2), new Vector2(0, 1), instance);
				tri[iv++] = new RoundLineVertex(new Vector3(0.0f, 1.0f, 0), new Vector2(1, pi2), new Vector2(0, 0), instance);

				// core indices
				indices[ii++] = (short)(iVertex + 0);
				indices[ii++] = (short)(iVertex + 1);
				indices[ii++] = (short)(iVertex + 2);
				indices[ii++] = (short)(iVertex + 2);
				indices[ii++] = (short)(iVertex + 3);
				indices[ii++] = (short)(iVertex + 0);

				indices[ii++] = (short)(iVertex + 4);
				indices[ii++] = (short)(iVertex + 6);
				indices[ii++] = (short)(iVertex + 5);
				indices[ii++] = (short)(iVertex + 6);
				indices[ii++] = (short)(iVertex + 7);
				indices[ii++] = (short)(iVertex + 5);

				// left halfdisc
				iVertex = iv;
				iIndex = ii;
				for (int i = 0; i < primsPerCap + 1; i++)
				{
					float deltaTheta = MathHelper.Pi / primsPerCap;
					float theta0 = MathHelper.PiOver2 + i * deltaTheta;
					float theta1 = theta0 + deltaTheta / 2;
					// even-numbered indices are at the center of the halfdisc
					tri[iVertex + 0] = new RoundLineVertex(new Vector3(0, 0, 0), new Vector2(0, theta1), new Vector2(0, 0), instance);

					// odd-numbered indices are at the perimeter of the halfdisc
					float x = (float)Math.Cos(theta0);
					float y = (float)Math.Sin(theta0);
					tri[iVertex + 1] = new RoundLineVertex(new Vector3(x, y, 0), new Vector2(1, theta0), new Vector2(1, 0), instance);

					if (i < primsPerCap)
					{
						// indices follow this pattern: (0, 1, 3), (2, 3, 5), (4, 5, 7), ...
						indices[iIndex + 0] = (short)(iVertex + 0);
						indices[iIndex + 1] = (short)(iVertex + 1);
						indices[iIndex + 2] = (short)(iVertex + 3);
						iIndex += 3;
						ii += 3;
					}
					iVertex += 2;
					iv += 2;
				}

				// right halfdisc
				for (int i = 0; i < primsPerCap + 1; i++)
				{
					float deltaTheta = MathHelper.Pi / primsPerCap;
					float theta0 = 3 * MathHelper.PiOver2 + i * deltaTheta;
					float theta1 = theta0 + deltaTheta / 2;
					float theta2 = theta0 + deltaTheta;
					// even-numbered indices are at the center of the halfdisc
					tri[iVertex + 0] = new RoundLineVertex(new Vector3(0, 0, 0), new Vector2(0, theta1), new Vector2(0, 1), instance);

					// odd-numbered indices are at the perimeter of the halfdisc
					float x = (float)Math.Cos(theta0);
					float y = (float)Math.Sin(theta0);
					tri[iVertex + 1] = new RoundLineVertex(new Vector3(x, y, 0), new Vector2(1, theta0), new Vector2(1, 1), instance);

					if (i < primsPerCap)
					{
						// indices follow this pattern: (0, 1, 3), (2, 3, 5), (4, 5, 7), ...
						indices[iIndex + 0] = (short)(iVertex + 0);
						indices[iIndex + 1] = (short)(iVertex + 1);
						indices[iIndex + 2] = (short)(iVertex + 3);
						iIndex += 3;
						ii += 3;
					}
					iVertex += 2;
					iv += 2;
				}
			}

			vb = new VertexBuffer(device, RoundLineVertex.VertexDeclaration, numVertices, BufferUsage.WriteOnly);
			vb.SetData(tri);

			ib = new IndexBuffer(device, IndexElementSize.SixteenBits, numIndices, BufferUsage.WriteOnly);
			ib.SetData(indices);
		}

		
		/// <summary>
		/// Draw a list of Lines.
		/// </summary>
		public void Draw(LineBuffer lines, Matrix viewProjMatrix, float inverseScaleRadius)
		{
			Begin(viewProjMatrix, inverseScaleRadius);

			for(int i = 0; i < lines.count; i += LinesPerBatch)
			{
				Draw(lines.subBuffers[i/LinesPerBatch], Math.Min(LinesPerBatch, lines.count - i));
			}

			End();
		}



		internal void Begin(Matrix viewProjMatrix, float inverseScaleRadius)
		{
			device.SetVertexBuffer(vb);
			device.Indices = ib;

			viewProjMatrixParameter.SetValue(viewProjMatrix);
			inverseScaleRadiusParameter.SetValue(inverseScaleRadius);
		}

		internal void Draw(LineInstanceData instanceData, int instanceCount)
		{
			instancePositionParameter.SetValue(instanceData.positionData);
			instanceColorParameter.SetValue(instanceData.colorData);
			instanceRadiusParameter.SetValue(instanceData.radiusData);
			effectPass.Apply();

			device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, numVertices, 0,
					numPrimitivesPerInstance * instanceCount);
		}

		internal void End()
		{
		}

	}
}
