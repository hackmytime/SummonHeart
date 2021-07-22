using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

// Token: 0x02000007 RID: 7
public static class SpriteBatchExtensions
{
	// Token: 0x06000021 RID: 33 RVA: 0x0000264F File Offset: 0x0000084F
	public static void BeginBlendState(this SpriteBatch spriteBatch, BlendState state, SamplerState samplerState = null, bool ui = false)
	{
		spriteBatch.End();
		spriteBatch.Begin((SpriteSortMode)(ui ? 0 : 1), state, samplerState ?? Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, ui ? Main.UIScaleMatrix : Main.GameViewMatrix.TransformationMatrix);
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002690 File Offset: 0x00000890
	public static void EndBlendState(this SpriteBatch spriteBatch, bool ui = false)
	{
		spriteBatch.End();
		spriteBatch.Begin((SpriteSortMode)(ui ? 0 : 1), BlendState.AlphaBlend, ui ? SamplerState.AnisotropicClamp : Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, ui ? Main.UIScaleMatrix : Main.GameViewMatrix.TransformationMatrix);
	}

	// Token: 0x06000023 RID: 35 RVA: 0x000026E3 File Offset: 0x000008E3
	public static void PushBlendState(this SpriteBatch spriteBatch, BlendState state, Action drawAction, SamplerState samplerState = null)
	{
		spriteBatch.PushBlendState(state, false, drawAction, samplerState);
	}

	// Token: 0x06000024 RID: 36 RVA: 0x000026F0 File Offset: 0x000008F0
	public static void PushBlendState(this SpriteBatch spriteBatch, BlendState state, bool isUI, Action drawAction, SamplerState samplerState = null)
	{
		spriteBatch.End();
		spriteBatch.Begin((SpriteSortMode)1, state, samplerState ?? Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, isUI ? Main.UIScaleMatrix : Main.GameViewMatrix.TransformationMatrix);
		drawAction();
		spriteBatch.End();
		spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, isUI ? SamplerState.AnisotropicClamp : Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, isUI ? Main.UIScaleMatrix : Main.GameViewMatrix.TransformationMatrix);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x0000277C File Offset: 0x0000097C
	public static void DrawDye(this SpriteBatch spriteBatch, Action drawAction)
	{
		spriteBatch.End();
		spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
		drawAction();
		spriteBatch.End();
		spriteBatch.Begin(0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
	}
}
