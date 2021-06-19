using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Models;
using SummonHeart.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Extensions
{
    /// <summary>
    ///     A class housing all the player/ModPlayer/MyPlayer based extensions
    /// </summary>
    public static class PlayerExtensions
    {
        public static void DrawAura(this SummonHeartPlayer modPlayer, AuraAnimationInfo aura)
        {
            Player player = modPlayer.player;
            Texture2D texture = aura.GetTexture();
            Rectangle textureRectangle = new Rectangle(0, aura.GetHeight() * modPlayer.auraCurrentFrame, texture.Width, aura.GetHeight());
            float scale = aura.GetAuraScale(modPlayer);
            Tuple<float, Vector2> rotationAndPosition = aura.GetAuraRotationAndPosition(modPlayer);
            float rotation = rotationAndPosition.Item1;
            Vector2 position = rotationAndPosition.Item2;

            AnimationHelper.SetSpriteBatchForPlayerLayerCustomDraw(aura.blendState, player.GetPlayerSamplerState());

            // custom draw routine
            Main.spriteBatch.Draw(texture, position - Main.screenPosition, textureRectangle, Color.White, rotation, new Vector2(aura.GetWidth(), aura.GetHeight()) * 0.5f, scale, SpriteEffects.None, 0f);

            AnimationHelper.ResetSpriteBatchForPlayerDrawLayers(player.GetPlayerSamplerState());
        }

        public static SamplerState GetPlayerSamplerState(this Player player)
        {
            return player.mount.Active ? Main.MountedSamplerState : Main.DefaultSamplerState;
        }
    }
}