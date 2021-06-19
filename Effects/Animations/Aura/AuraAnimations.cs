﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart;
using SummonHeart.Models;
using System;
using Terraria;

namespace SummonHeart.Effects.Animations.Aura
{
    public static class AuraAnimations
    {
        private static string GetAnimationSpriteName(string spriteName)
        {
            return string.Format("Effects/Animations/Aura/{0}", spriteName);
        }

        public static void DoChargeDust(SummonHeartPlayer modPlayer, AuraAnimationInfo auraInfo)
        {
            var position = auraInfo.GetAuraRotationAndPosition(modPlayer).Item2;

            for (int d = 0; d < 1; d++)
            {
                if (Main.rand.NextFloat() < 0.5f)
                {
                    Dust dust = Dust.NewDustDirect(position, auraInfo.GetWidth(), auraInfo.GetHeight(), 63, 0f, 0f, 0, new Color(255, 255, 255), 0.75f);
                    dust.noGravity = true;
                }
                if (Main.rand.NextFloat() < 0.25f)
                {
                    Dust dust = Dust.NewDustDirect(position, auraInfo.GetWidth(), auraInfo.GetHeight(), 63, 0f, 0f, 0, new Color(255, 255, 255), 1.5f);
                    dust.noGravity = true;
                }
            }
        }

        public static AuraAnimationInfo
            chargeAura = new AuraAnimationInfo(GetAnimationSpriteName("BaseAura"), 4, 3, BlendState.Additive, "Sounds/EnergyChargeStart", "Sounds/EnergyCharge", 22, true, false, new AuraAnimationInfo.DustDelegate(DoChargeDust), 0, null, 1),

            ssj1Aura = new AuraAnimationInfo(GetAnimationSpriteName("SSJ1Aura"), 4, 3, BlendState.Additive, "Sounds/SSJAscension", "Sounds/EnergyCharge", 22, true, false, null, 2, new AuraAnimationInfo.DustDelegate(DoSSJ1Dust), 1),
            assjAura = new AuraAnimationInfo(GetAnimationSpriteName("SSJ1Aura"), 4, 3, BlendState.Additive, "Sounds/SSJAscension", "Sounds/EnergyCharge", 22, true, false, null, 2, new AuraAnimationInfo.DustDelegate(DoSSJ1Dust), 1),
            ussjAura = new AuraAnimationInfo(GetAnimationSpriteName("SSJ1Aura"), 4, 3, BlendState.Additive, "Sounds/SSJAscension", "Sounds/EnergyCharge", 22, true, false, null, 2, new AuraAnimationInfo.DustDelegate(DoSSJ1Dust), 1),

            ssj2Aura = new AuraAnimationInfo(GetAnimationSpriteName("SSJ2Aura"), 4, 3, BlendState.Additive, "Sounds/SSJAscension", "Sounds/SSJ2", 510, true, false, null, 0, null, 1),
            ssj3Aura = new AuraAnimationInfo(GetAnimationSpriteName("SSJ3Aura"), 4, 3, BlendState.Additive, "Sounds/SSJAscension", "Sounds/SSJ3", 260, true, false, null, 0, null, 1),
            ssj4Aura = new AuraAnimationInfo(GetAnimationSpriteName("SSJ1Aura"), 4, 3, BlendState.Additive, "Sounds/SSJAscension", "Sounds/EnergyCharge", 22, true, false, null, 2, new AuraAnimationInfo.DustDelegate(DoSSJ1Dust), 1),

            mysticAura = new AuraAnimationInfo(GetAnimationSpriteName("MysticAura"), 4, 3, BlendState.Additive, "Sounds/SSJAscension", "Sounds/EnergyCharge", 22, true, false, null, 2, new AuraAnimationInfo.DustDelegate(DoSSJ1Dust), 1),

            ssjgAura = new AuraAnimationInfo(GetAnimationSpriteName("SSJGAura"), 8, 3, BlendState.AlphaBlend, "Sounds/SSJAscension", "Sounds/SSG", 340, true, false, new AuraAnimationInfo.DustDelegate(DoChargeDust), 0, null, 1),
            ssjbAura = new AuraAnimationInfo(GetAnimationSpriteName("SSJBAura"), 8, 3, BlendState.Additive, "Sounds/SSJAscension", "Sounds/SSB", 340, true, false, new AuraAnimationInfo.DustDelegate(DoChargeDust), 0, null, 1),
            ssjrAura = new AuraAnimationInfo(GetAnimationSpriteName("SSJRAura"), 8, 3, BlendState.Additive, "Sounds/SSJAscension", "Sounds/SSJR", 340, true, false, new AuraAnimationInfo.DustDelegate(DoChargeDust), 0, null, 1),

            uiOmenAura = new AuraAnimationInfo(GetAnimationSpriteName("UIOmenAura"), 15, 4, BlendState.Additive, "Sounds/SSJAscension", "Sounds/SSG", 340, true, false, new AuraAnimationInfo.DustDelegate(DoChargeDust), 0, null, 1),

            createFalseUIAura = new AuraAnimationInfo(GetAnimationSpriteName("FalseUIAura"), 15, 4, BlendState.Additive, "Sounds/SSJAscension", "Sounds/EnergyCharge", 22, true, false, new AuraAnimationInfo.DustDelegate(DoFalseUIDust), 0, null, 1),
            createKaiokenAura = new AuraAnimationInfo(GetAnimationSpriteName("KaiokenAura"), 4, 3, BlendState.Additive, "Sounds/KaioAuraStart", "Sounds/EnergyCharge", 22, true, true, null, 0, null, 0),
            createSuperKaiokenAura = new AuraAnimationInfo(GetAnimationSpriteName("SuperKaiokenAura"), 4, 3, BlendState.Additive, "Sounds/KaioAuraStart", "Sounds/EnergyCharge", 22, true, true, null, 0, null, 0),

            lssjAura = new AuraAnimationInfo(GetAnimationSpriteName("LSSJAura"), 4, 3, BlendState.Additive, "Sounds/SSJAscension", "Sounds/SSJ2", 510, true, false, null, 0, null, 1),

            soulStealerAura = new AuraAnimationInfo(GetAnimationSpriteName("SoulStealerAura"), 8, 3, BlendState.Additive, "Sounds/SoulStealerAscension", "Sounds/SoulStealer", 100, true, false, new AuraAnimationInfo.DustDelegate(DoChargeDust), 0, null, 1);

        public static void DoSSJ1Dust(SummonHeartPlayer modPlayer, AuraAnimationInfo aura)
        {
            const float aurawidth = 3.0f;

            for (int i = 0; i < 20; i++)
            {
                float xPos = (Vector2.UnitX * 5.0f + Vector2.UnitX * (Main.rand.Next(-10, 10) * aurawidth)).X;
                float yPos = (Vector2.UnitY * modPlayer.player.height - Vector2.UnitY * Main.rand.Next(0, modPlayer.player.height)).Y - 0.5f;

                Dust tDust = Dust.NewDustDirect(modPlayer.player.position + new Vector2(xPos, yPos), 1, 1, 87, 0f, 0f, 0, new Color(0, 0, 0, 0), 0.4f * Main.rand.Next(1, 4));

                if (Math.Abs((tDust.position - (modPlayer.player.position + Vector2.UnitX * 7.0f)).X) < 10)
                {
                    tDust.scale *= 0.75f;
                }

                Vector2 dir = -(tDust.position - (modPlayer.player.position + Vector2.UnitX * 5.0f - Vector2.UnitY * modPlayer.player.height));
                dir.Normalize();

                tDust.velocity = new Vector2(dir.X * 2.0f, -1 * Main.rand.Next(1, 5));
                tDust.noGravity = true;
            }
        }

        public static void DoFalseUIDust(SummonHeartPlayer modPlayer, AuraAnimationInfo aura)
        {
            //blue dust
            if (Main.rand.NextFloat() < 1f)
            {
                Dust dust;
                Vector2 center = aura.GetCenter(modPlayer);
                Vector2 position = center + new Vector2(-15, -20);
                dust = Dust.NewDustDirect(position, 42, 58, 187, 0f, -5.526316f, 0, new Color(255, 255, 255), 0.8552632f);
                dust.noGravity = true;
                dust.fadeIn = 0.5131579f;
            }
            //white dust
            if (Main.rand.NextFloat() < 0.5263158f)
            {
                Dust dust;
                Vector2 center = aura.GetCenter(modPlayer);
                Vector2 position = center + new Vector2(-17, -10);
                dust = Dust.NewDustDirect(position, 26, 52, 63, 0f, -7.368421f, 0, new Color(255, 255, 255), 0.8552632f);
                dust.noGravity = true;
                dust.fadeIn = 0.7894737f;
            }
        }

        public static void DoFabulousDust(SummonHeartPlayer modPlayer, AuraAnimationInfo aura)
        {
            var position = aura.GetAuraRotationAndPosition(modPlayer).Item2;
            if (Main.rand.NextFloat() < 0.5f)
            {
                Dust dust = Dust.NewDustDirect(position, aura.GetWidth(), aura.GetHeight(), 91, 0f, 0f, 0, new Color(Main.DiscoColor.R, Main.DiscoColor.G, Main.DiscoColor.B), 0.75f);
                dust.noGravity = true;
            }
            if (Main.rand.NextFloat() < 0.25f)
            {
                Dust dust = Dust.NewDustDirect(position, aura.GetWidth(), aura.GetHeight(), 91, 0f, 0f, 0, new Color(Main.DiscoColor.R, Main.DiscoColor.G, Main.DiscoColor.B), 0.25f);
                dust.noGravity = true;
            }
        }

        /// <summary>
        ///     Return the aura effect currently active on the player.
        /// </summary>
        /// <param name="player">The player being checked</param>
        public static AuraAnimationInfo GetAuraEffectOnPlayer(this SummonHeartPlayer player)
        {
            if (player.player.dead)
                return null;

            return ssjrAura;
        }
    }
}
