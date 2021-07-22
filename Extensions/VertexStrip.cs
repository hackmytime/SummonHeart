using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart;
using SummonHeart.Extensions;
using Terraria;

// Token: 0x02000008 RID: 8
public class VertexStrip
{
	// Token: 0x06000026 RID: 38 RVA: 0x000027E8 File Offset: 0x000009E8
	public void PrepareStrip(Vector2[] positions, float[] rotations, VertexStrip.StripColorFunction colorFunction, VertexStrip.StripHalfWidthFunction widthFunction, Vector2 offsetForAllPositions = default(Vector2), int? expectedVertexPairsAmount = null, bool includeBacksides = false)
	{
		int num = positions.Length;
		int num2 = num * 2;
		this._vertexAmountCurrentlyMaintained = num2;
		if (this._vertices.Length < num2)
		{
			Array.Resize<VertexStrip.CustomVertexInfo>(ref this._vertices, num2);
		}
		int num3 = num;
		if (expectedVertexPairsAmount != null)
		{
			num3 = expectedVertexPairsAmount.Value;
		}
		for (int i = 0; i < num; i++)
		{
			if (positions[i] == Vector2.Zero)
			{
				num = i - 1;
				this._vertexAmountCurrentlyMaintained = num * 2;
				break;
			}
			Vector2 pos = positions[i] + offsetForAllPositions;
			float rot = MathHelper.WrapAngle(rotations[i]);
			int indexOnVertexArray = i * 2;
			float progressOnStrip = (float)i / (float)(num3 - 1);
			this.AddVertex(colorFunction, widthFunction, pos, rot, indexOnVertexArray, progressOnStrip);
		}
		this.PrepareIndices(num, includeBacksides);
	}

	// Token: 0x06000027 RID: 39 RVA: 0x0000289E File Offset: 0x00000A9E
	private static float Distance(Vector2 Origin, Vector2 Target)
	{
		return Vector2.Distance(Origin, Target);
	}

	// Token: 0x06000028 RID: 40 RVA: 0x000028A8 File Offset: 0x00000AA8
	public void PrepareStripWithProceduralPadding(Vector2[] positions, float[] rotations, VertexStrip.StripColorFunction colorFunction, VertexStrip.StripHalfWidthFunction widthFunction, Vector2 offsetForAllPositions = default(Vector2), bool includeBacksides = false, bool tryStoppingOddBug = true)
	{
		int num = positions.Length;
		this._temporaryPositionsCache.Clear();
		this._temporaryRotationsCache.Clear();
		int num2 = 0;
		while (num2 < num && !(positions[num2] == Vector2.Zero))
		{
			Vector2 vector = positions[num2];
			float num3 = MathHelper.WrapAngle(rotations[num2]);
			this._temporaryPositionsCache.Add(vector);
			this._temporaryRotationsCache.Add(num3);
			if (num2 + 1 < num && positions[num2 + 1] != Vector2.Zero)
			{
				Vector2 vector2 = positions[num2 + 1];
				float num4 = MathHelper.WrapAngle(rotations[num2 + 1]);
				int num5 = (int)(Math.Abs(MathHelper.WrapAngle(num4 - num3)) / 0.2617994f);
				if (num5 != 0)
				{
					float num6 = VertexStrip.Distance(vector, vector2);
					Vector2 value = vector + Utils.ToRotationVector2(num3) * num6;
					Vector2 value2 = vector2 + Utils.ToRotationVector2(num4) * -num6;
					int num7 = num5 + 2;
					float num8 = 1f / (float)num7;
					Vector2 target = vector;
					for (float num9 = num8; num9 < 1f; num9 += num8)
					{
						Vector2 vector3 = Vector2.CatmullRom(value, vector, vector2, value2, num9);
						float item = MathHelper.WrapAngle(Utils.ToRotation(vector3.DirectionTo(target)));
						this._temporaryPositionsCache.Add(vector3);
						this._temporaryRotationsCache.Add(item);
						target = vector3;
					}
				}
			}
			num2++;
		}
		int count = this._temporaryPositionsCache.Count;
		Vector2 zero = Vector2.Zero;
		int num10 = 0;
		while (num10 < count && (!tryStoppingOddBug || !(this._temporaryPositionsCache[num10] == zero)))
		{
			Vector2 pos = this._temporaryPositionsCache[num10] + offsetForAllPositions;
			float rot = this._temporaryRotationsCache[num10];
			int indexOnVertexArray = num10 * 2;
			float progressOnStrip = (float)num10 / (float)(count - 1);
			this.AddVertex(colorFunction, widthFunction, pos, rot, indexOnVertexArray, progressOnStrip);
			num10++;
		}
		this._vertexAmountCurrentlyMaintained = count * 2;
		this.PrepareIndices(count, includeBacksides);
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00002AB4 File Offset: 0x00000CB4
	private void PrepareIndices(int vertexPaidsAdded, bool includeBacksides)
	{
		int num = vertexPaidsAdded - 1;
		int num2 = 6 + Utils.ToInt(includeBacksides) * 6;
		int num3 = num * num2;
		this._indicesAmountCurrentlyMaintained = num3;
		if (this._indices.Length < num3)
		{
			Array.Resize<short>(ref this._indices, num3);
		}
		short num4 = 0;
		while ((int)num4 < num)
		{
			short num5 = (short)((int)num4 * num2);
			int num6 = (int)(num4 * 2);
			this._indices[(int)num5] = (short)num6;
			this._indices[(int)(num5 + 1)] = (short)(num6 + 1);
			this._indices[(int)(num5 + 2)] = (short)(num6 + 2);
			this._indices[(int)(num5 + 3)] = (short)(num6 + 2);
			this._indices[(int)(num5 + 4)] = (short)(num6 + 1);
			this._indices[(int)(num5 + 5)] = (short)(num6 + 3);
			if (includeBacksides)
			{
				this._indices[(int)(num5 + 6)] = (short)(num6 + 2);
				this._indices[(int)(num5 + 7)] = (short)(num6 + 1);
				this._indices[(int)(num5 + 8)] = (short)num6;
				this._indices[(int)(num5 + 9)] = (short)(num6 + 2);
				this._indices[(int)(num5 + 10)] = (short)(num6 + 3);
				this._indices[(int)(num5 + 11)] = (short)(num6 + 1);
			}
			num4 += 1;
		}
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00002BD0 File Offset: 0x00000DD0
	private void AddVertex(VertexStrip.StripColorFunction colorFunction, VertexStrip.StripHalfWidthFunction widthFunction, Vector2 pos, float rot, int indexOnVertexArray, float progressOnStrip)
	{
		while (indexOnVertexArray + 1 >= this._vertices.Length)
		{
			Array.Resize<VertexStrip.CustomVertexInfo>(ref this._vertices, this._vertices.Length * 2);
		}
		Color color = colorFunction(progressOnStrip);
		float scaleFactor = widthFunction(progressOnStrip);
		Vector2 value = Utils.ToRotationVector2(MathHelper.WrapAngle(rot - 1.5707964f)) * scaleFactor;
		this._vertices[indexOnVertexArray].Position = pos + value;
		this._vertices[indexOnVertexArray + 1].Position = pos - value;
		this._vertices[indexOnVertexArray].TexCoord = new Vector2(progressOnStrip, 1f);
		this._vertices[indexOnVertexArray + 1].TexCoord = new Vector2(progressOnStrip, 0f);
		this._vertices[indexOnVertexArray].Color = color;
		this._vertices[indexOnVertexArray + 1].Color = color;
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002CC8 File Offset: 0x00000EC8
	public void DrawTrail()
	{
		if (this._vertexAmountCurrentlyMaintained < 3)
		{
			return;
		}
		Main.instance.GraphicsDevice.DrawUserIndexedPrimitives<VertexStrip.CustomVertexInfo>(0, this._vertices, 0, this._vertexAmountCurrentlyMaintained, this._indices, 0, this._indicesAmountCurrentlyMaintained / 3);
	}

	// Token: 0x0400001A RID: 26
	private VertexStrip.CustomVertexInfo[] _vertices = new VertexStrip.CustomVertexInfo[1];

	// Token: 0x0400001B RID: 27
	private int _vertexAmountCurrentlyMaintained;

	// Token: 0x0400001C RID: 28
	private short[] _indices = new short[1];

	// Token: 0x0400001D RID: 29
	private int _indicesAmountCurrentlyMaintained;

	// Token: 0x0400001E RID: 30
	private List<Vector2> _temporaryPositionsCache = new List<Vector2>();

	// Token: 0x0400001F RID: 31
	private List<float> _temporaryRotationsCache = new List<float>();

	// Token: 0x02000009 RID: 9
	// (Invoke) Token: 0x0600002E RID: 46
	public delegate Color StripColorFunction(float progressOnStrip);

	// Token: 0x0200000A RID: 10
	// (Invoke) Token: 0x06000032 RID: 50
	public delegate float StripHalfWidthFunction(float progressOnStrip);

	// Token: 0x0200000B RID: 11
	private struct CustomVertexInfo : IVertexType
	{
		// Token: 0x06000035 RID: 53 RVA: 0x00002D41 File Offset: 0x00000F41
		public CustomVertexInfo(Vector2 position, Color color, Vector2 texCoord)
		{
			this.Position = position;
			this.Color = color;
			this.TexCoord = texCoord;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002D58 File Offset: 0x00000F58
		public VertexDeclaration VertexDeclaration
		{
			get
			{
				return VertexStrip.CustomVertexInfo._vertexDeclaration;
			}
		}

		// Token: 0x04000020 RID: 32
		public Vector2 Position;

		// Token: 0x04000021 RID: 33
		public Color Color;

		// Token: 0x04000022 RID: 34
		public Vector2 TexCoord;

		// Token: 0x04000023 RID: 35
		private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[]
		{
			new VertexElement(0, (VertexElementFormat)1, 0, 0),
			new VertexElement(8, (VertexElementFormat)4, (VertexElementUsage)1, 0),
			new VertexElement(12, (VertexElementFormat)1, (VertexElementUsage)2, 0)
		});
	}
}
