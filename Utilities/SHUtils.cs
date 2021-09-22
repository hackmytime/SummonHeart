using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using SummonHeart.Items.Range;

namespace SummonHeart.Utilities
{
    public static class SHUtils
    {
        public static void HealLife(this Player player, int amount, bool visible = true)
        {
            player.statLife += amount;
            if (player.statLife > player.statLifeMax2) player.statLife = player.statLifeMax2;
            if (visible) player.HealEffect(amount, true);
        }
        public static void HealMana(this Player player, int amount, bool visible = true)
        {
            player.statMana += amount;
            if (player.statMana > player.statManaMax2) player.statMana = player.statManaMax2;
            if (visible) player.ManaEffect(amount);
        }

        public static void Trans(this Player player, Vector2 pos)
        {
            player.Teleport(pos, 1, 0);
            NetMessage.SendData(65, -1, -1, null, 0, player.whoAmI, pos.X, pos.Y, 1, 0, 0);
            player.AddBuff(88, 100, true);
        }
       
        public static bool CanHitNPC(this Projectile projectile, NPC npc) => Collision.CanHit(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
        public static void FollowPlayer(this Projectile projectile, Vector2 place, float maxDist, float minDist, Vector2 velo, float speed = 0, bool positionLerp = false, float lerpAmtFar = 0.05f, float lerpAmtClose = 0.07f, float lerpAmtPos = 0.1f)
        {
            if (projectile.Distance(place) > maxDist) projectile.Center = place - projectile.DirectionTo(place) * maxDist;
            else if (projectile.Distance(place) < minDist) projectile.velocity = Vector2.Lerp(projectile.velocity, velo, lerpAmtClose);
            else projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(place) * speed, lerpAmtFar);
            if (positionLerp) projectile.Center = Vector2.Lerp(projectile.Center, place, lerpAmtPos);
        }
        
        public static bool AnyBossAlive
        {
            get
            {
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && npc.boss) return true;
                    if (npc.active && npc.type == NPCID.EaterofWorldsHead) return true;
                }
                return false;
            }
        }
       
        public static void DoAVisualBuff(this Player player, int type, int time = 0)
        {
            player.ClearBuff(type);
            player.AddBuff(type, time + 2);
        }
        public static Vector2 RandomRotate => MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
        public static List<Projectile> getOwnedProjectile(this Player player, int type)
        {
            List<Projectile> resulList = new List<Projectile>();
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.active && projectile.type == type && projectile.owner == player.whoAmI)
                    resulList.Add(Main.projectile[i]);
            }
            return resulList;
        }
        public static int ownedProjectileCounts(this Player player, int type)
        {
            int amt = 0;
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.active && projectile.type == type && projectile.owner == player.whoAmI) amt++;
            }
            return amt;
        }

        public static bool Sponge(Player player, int type)
        {
            if (Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType() == type)
            {
                int num234 = (int)Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType();
                int num235 = 0;
                int num2;
                for (int num236 = Player.tileTargetX - 1; num236 <= Player.tileTargetX + 1; num236 = num2 + 1)
                {
                    for (int num237 = Player.tileTargetY - 1; num237 <= Player.tileTargetY + 1; num237 = num2 + 1)
                    {
                        if ((int)Main.tile[num236, num237].liquidType() == num234)
                        {
                            num235 += (int)Main.tile[num236, num237].liquid;
                        }
                        num2 = num237;
                    }
                    num2 = num236;
                }
                if (Main.tile[Player.tileTargetX, Player.tileTargetY].liquid > 0 && num235 > 100)
                {
                    int liquidType = (int)Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType();
                   
                    Main.PlaySound(19, (int)player.position.X, (int)player.position.Y, 1, 1f, 0f);
                    int num238 = (int)Main.tile[Player.tileTargetX, Player.tileTargetY].liquid;
                    Main.tile[Player.tileTargetX, Player.tileTargetY].liquid = 0;
                    Main.tile[Player.tileTargetX, Player.tileTargetY].lava(false);
                    Main.tile[Player.tileTargetX, Player.tileTargetY].honey(false);
                    WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, false);
                    if (Main.netMode == 1)
                    {
                        NetMessage.sendWater(Player.tileTargetX, Player.tileTargetY);
                    }
                    else
                    {
                        Liquid.AddWater(Player.tileTargetX, Player.tileTargetY);
                    }
                    for (int num239 = Player.tileTargetX - 1; num239 <= Player.tileTargetX + 1; num239 = num2 + 1)
                    {
                        for (int num240 = Player.tileTargetY - 1; num240 <= Player.tileTargetY + 1; num240 = num2 + 1)
                        {
                            if (num238 < 256 && (int)Main.tile[num239, num240].liquidType() == num234)
                            {
                                int num241 = (int)Main.tile[num239, num240].liquid;
                                if (num241 + num238 > 255)
                                {
                                    num241 = 255 - num238;
                                }
                                num238 += num241;
                                Tile tile4 = Main.tile[num239, num240];
                                Tile tile5 = tile4;
                                tile5.liquid -= (byte)num241;
                                Main.tile[num239, num240].liquidType(liquidType);
                                if (Main.tile[num239, num240].liquid == 0)
                                {
                                    Main.tile[num239, num240].lava(false);
                                    Main.tile[num239, num240].honey(false);
                                }
                                WorldGen.SquareTileFrame(num239, num240, false);
                                if (Main.netMode == 1)
                                {
                                    NetMessage.sendWater(num239, num240);
                                }
                                else
                                {
                                    Liquid.AddWater(num239, num240);
                                }
                            }
                            num2 = num240;
                        }
                        num2 = num239;
                    }
                    return true;
                }
            }
            return false;
        }

        public static void ownedProjectileKill(this Player player, int type)
        {
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.active && projectile.type == type && projectile.owner == player.whoAmI) projectile.Kill();
            }
        }
        public static int ownedSummonProjectileCounts(this Player player)
        {
            int amt = 0;
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.active && projectile.minion && projectile.owner == player.whoAmI) amt++;
            }
            return amt;
        }
        public static void ownedSummonProjectileKill(this Player player, Projectile p)
        {
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile == p)
                    continue;
                if (projectile.active && projectile.minion && projectile.owner == player.whoAmI) projectile.Kill();
            }
        }
        public static int TransFloatToInt(float num)
        {
            int low = (int)num;
            int chance = (int)((num - low) * 100);
            if (Main.rand.Next(100) < chance) low++;
            return low;
        }

        internal static bool PlaceLiquid(Player player, int type)
        {
            if (Main.tile[Player.tileTargetX, Player.tileTargetY].active())
            {
                return false;
            }
            if (Math.Abs(player.position.X / 16f - Player.tileTargetX) > Player.tileRangeX + 20 * 16 || Math.Abs(player.position.Y / 16f - Player.tileTargetY) > Player.tileRangeY + 20 * 16)
            {
                return false;
            }
            Main.PlaySound(19, (int)player.position.X, (int)player.position.Y, 1, 1f, 0f);
            Main.tile[Player.tileTargetX, Player.tileTargetY].liquidType(type);
            Main.tile[Player.tileTargetX, Player.tileTargetY].liquid = byte.MaxValue;
            WorldGen.SquareTileFrame(Player.tileTargetX, Player.tileTargetY, true);
            if (Main.netMode == 1)
            {
                NetMessage.sendWater(Player.tileTargetX, Player.tileTargetY);
            }
            return true;
        }
    }
}
