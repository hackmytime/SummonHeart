using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Models;
using SummonHeart.Utilities;
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

        public static int getPower(this SummonHeartPlayer modPlayer)
        {
            int power = 0;
            int x = 1;
            if (Main.hardMode)
            {
                x = 2;
            }
            if (NPC.downedMoonlord)
            {
                x = 10;
            }
            power = modPlayer.eyeBloodGas + modPlayer.handBloodGas + modPlayer.bodyBloodGas + modPlayer.footBloodGas;
            power += modPlayer.player.statLifeMax2 * x;
            power += modPlayer.player.statDefense * 30;
            power += modPlayer.SummonCrit * 20 * x;
            power += modPlayer.killResourceMax2 * 10;
            Item item = modPlayer.player.HeldItem;
            if (item.damage > 0)
            {
                power += (int)(item.damage * 5 * (60f / (float)item.useTime));
            }
           
            return power;
        }

        public static int getAllBloodGas(this SummonHeartPlayer modPlayer)
        {
            int all = 0;
            all = modPlayer.eyeBloodGas + modPlayer.handBloodGas + modPlayer.bodyBloodGas + modPlayer.footBloodGas;
            return all;
        }

        public static int HasItemInAcc(this Player player, int type)
        {
            for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
            {
                if (player.armor[i].type == type)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}