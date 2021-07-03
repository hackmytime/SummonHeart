using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Effects.Animations.Aura;
using SummonHeart.Extensions;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Utilities
{
    public static class AnimationHelper
    {
        public static void SetSpriteBatchForPlayerLayerCustomDraw(BlendState blendState, SamplerState samplerState)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, blendState, samplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public static void ResetSpriteBatchForPlayerDrawLayers(SamplerState samplerState)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, samplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public static readonly PlayerLayer auraEffect = new PlayerLayer("DBZMOD", "AuraEffects", null, delegate (PlayerDrawInfo drawInfo)
        {
            if (Main.netMode == NetmodeID.Server)
                return;

            Player player = drawInfo.drawPlayer;
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

            if (drawInfo.shadow != 0f)
            {
                return;
            }

            Models.AuraAnimationInfo aura = modPlayer.GetAuraEffectOnPlayer();

            if (aura != null)
            {
                // we don't do player draw data, we do a custom draw.                
                modPlayer.DrawAura(aura);
            }
        });


        public static DrawData LightningEffectDrawData(PlayerDrawInfo drawInfo, string lightningTexture)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = ModLoader.GetMod("SummonHeart");
            SummonHeartPlayer modPlayer = drawPlayer.GetModPlayer<SummonHeartPlayer>();
            int frame = modPlayer.lightningFrameTimer / 5;
            Texture2D texture = mod.GetTexture(lightningTexture);
            int frameSize = texture.Height / 3;
            int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
            int drawY = (int)(drawInfo.position.Y + drawPlayer.height / 0.6f - Main.screenPosition.Y);
            return new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, frameSize * frame, texture.Width, frameSize), Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
        }

        public static readonly PlayerLayer lightningEffects = new PlayerLayer("DBZMOD", "LightningEffects", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Main.playerDrawData.Add(LightningEffectDrawData(drawInfo, "Dusts/LightningRed"));
        });
    }
}
