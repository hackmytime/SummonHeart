using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Helpful.Plant
{
    public class Fertilizer : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 99;
            item.useTurn = true;
            item.useAnimation = 17;
            item.useTime = 17;
            item.useStyle = 2;
            item.consumable = true;
            item.value = 15;
            item.rare = 0;
            item.autoReuse = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fertilizer");
            Tooltip.SetDefault("");
            DisplayName.AddTranslation(GameCulture.Chinese, "1级科技造物·金坷垃");
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(mod, "点击树苗或植物，使其快速生长","点击树苗或植物，使其快速生长")
            {
                overrideColor = new Color?(new Color(0, 150, 0))
            });
            tooltips.Add(new TooltipLine(mod, "也可以用来在泥土上种草，在草块上种植物", "也可以用来在泥土上种草，在草块上种植物")
            {
                overrideColor = new Color?(new Color(0, 150, 0))
            });
            tooltips.Add(new TooltipLine(mod, "而且可以在沙地上种植仙人掌", "而且可以在沙地上种植仙人掌")
            {
                overrideColor = new Color?(new Color(0, 150, 0))
            });
            foreach (TooltipLine tooltipLine in tooltips)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "ItemName")
                {
                    tooltipLine.overrideColor = new Color?(new Color(100, 200, 0));
                }
            }
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(2, 5);
            modRecipe.AddIngredient(mod.GetItem("Loot1"), 5);
            modRecipe.needWater = true;
            modRecipe.SetResult(this, 15);
            modRecipe.AddRecipe();
        }

        public override bool UseItem(Player player)
        {
            if (Math.Abs(Main.MouseWorld.X - player.position.X) < 100f && Math.Abs(Main.MouseWorld.Y - player.position.Y) < 70f && Main.tile[Player.tileTargetX, Player.tileTargetY].type == 20 && Main.tile[Player.tileTargetX - 1, Player.tileTargetY].type != 5 && Main.tile[Player.tileTargetX + 1, Player.tileTargetY].type != 5 && Main.tile[Player.tileTargetX - 2, Player.tileTargetY].type != 5 && Main.tile[Player.tileTargetX + 2, Player.tileTargetY].type != 5)
            {
                Main.PlaySound(6, -1, -1, 1, 1f, 0f);
                int num = Dust.NewDust(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), 16, 16, mod.DustType("FertDust"), 0f, 0f, 0, default, 1f);
                Main.dust[num].scale = 3f;
                Main.dust[num].position = Main.MouseWorld - Main.dust[num].scale * new Vector2(4f, 4f);
                if (Main.rand.Next(0, 15) == 1)
                {
                    WorldGen.GrowEpicTree(Player.tileTargetX, Player.tileTargetY);
                    WorldGen.TreeGrowFXCheck(Player.tileTargetX, Player.tileTargetY);
                    NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 99, 0);
                    return true;
                }
                if (Main.rand.Next(0, 30) < 8)
                {
                    WorldGen.GrowTree(Player.tileTargetX, Player.tileTargetY);
                    WorldGen.GrowPalmTree(Player.tileTargetX, Player.tileTargetY);
                    WorldGen.TreeGrowFXCheck(Player.tileTargetX, Player.tileTargetY);
                    NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 99, 0);
                    return true;
                }
                return true;
            }
            else
            {
                if (Math.Abs(Main.MouseWorld.X - player.position.X) < 100f && Math.Abs(Main.MouseWorld.Y - player.position.Y) < 70f && Main.tile[Player.tileTargetX, Player.tileTargetY].type == 82)
                {
                    Main.PlaySound(6, -1, -1, 1, 1f, 0f);
                    int num2 = Dust.NewDust(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), 16, 16, mod.DustType("FertDust"), 0f, 0f, 0, default, 1f);
                    Main.dust[num2].scale = 3f;
                    Main.dust[num2].position = Main.MouseWorld - Main.dust[num2].scale * new Vector2(4f, 4f);
                    if (Main.rand.Next(0, 3) < 1)
                    {
                        if (Main.tile[Player.tileTargetX, Player.tileTargetY].frameX == 0)
                        {
                            Main.tile[Player.tileTargetX, Player.tileTargetY].type = 83;
                            NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 99, 0);
                        }
                        if (Main.tile[Player.tileTargetX, Player.tileTargetY].frameX == 18)
                        {
                            Main.tile[Player.tileTargetX, Player.tileTargetY].type = 83;
                            NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 99, 0);
                        }
                        if (Main.tile[Player.tileTargetX, Player.tileTargetY].frameX == 36)
                        {
                            Main.tile[Player.tileTargetX, Player.tileTargetY].type = 83;
                            NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 99, 0);
                        }
                        if (Main.tile[Player.tileTargetX, Player.tileTargetY].frameX == 54)
                        {
                            Main.tile[Player.tileTargetX, Player.tileTargetY].type = 83;
                            NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 99, 0);
                        }
                        if (Main.tile[Player.tileTargetX, Player.tileTargetY].frameX == 72)
                        {
                            Main.tile[Player.tileTargetX, Player.tileTargetY].type = 83;
                            NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 99, 0);
                        }
                        if (Main.tile[Player.tileTargetX, Player.tileTargetY].frameX == 90)
                        {
                            Main.tile[Player.tileTargetX, Player.tileTargetY].type = 83;
                            NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 99, 0);
                        }
                        if (Main.tile[Player.tileTargetX, Player.tileTargetY].frameX == 108)
                        {
                            Main.tile[Player.tileTargetX, Player.tileTargetY].type = 84;
                            NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 99, 0);
                        }
                    }
                    return true;
                }
                if (Math.Abs(Main.MouseWorld.X - player.position.X) < 100f && Math.Abs(Main.MouseWorld.Y - player.position.Y) < 70f && Main.tile[Player.tileTargetX, Player.tileTargetY].type == 83 && Main.tile[Player.tileTargetX, Player.tileTargetY].frameX == 108)
                {
                    Main.PlaySound(6, -1, -1, 1, 1f, 0f);
                    int num3 = Dust.NewDust(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), 16, 16, mod.DustType("FertDust"), 0f, 0f, 0, default, 1f);
                    Main.dust[num3].scale = 3f;
                    Main.dust[num3].position = Main.MouseWorld - Main.dust[num3].scale * new Vector2(4f, 4f);
                    if (Main.rand.Next(1, 4) == 1)
                    {
                        Main.tile[Player.tileTargetX, Player.tileTargetY].type = 84;
                        NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 99, 0);
                    }
                    return true;
                }
                if (Math.Abs(Main.MouseWorld.X - player.position.X) < 100f && Math.Abs(Main.MouseWorld.Y - player.position.Y) < 70f && Main.tile[Player.tileTargetX, Player.tileTargetY].type == 53 && Main.tile[Player.tileTargetX, Player.tileTargetY].active() && !Main.tile[Player.tileTargetX, Player.tileTargetY - 1].active() && !Main.tile[Player.tileTargetX - 1, Player.tileTargetY - 1].active() && !Main.tile[Player.tileTargetX + 1, Player.tileTargetY - 1].active() && !Main.tile[Player.tileTargetX - 2, Player.tileTargetY - 1].active() && !Main.tile[Player.tileTargetX + 2, Player.tileTargetY - 1].active())
                {
                    Main.PlaySound(6, -1, -1, 1, 1f, 0f);
                    int num4 = Dust.NewDust(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), 16, 16, mod.DustType("FertDust"), 0f, 0f, 0, default, 1f);
                    Main.dust[num4].scale = 3f;
                    Main.dust[num4].position = Main.MouseWorld - Main.dust[num4].scale * new Vector2(4f, 4f);
                    if (Main.rand.Next(0, 3) == 0)
                    {
                        WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY - 1, 80, true, false, -1, 0);
                        int num5 = Main.rand.Next(-8, -3);
                        for (int i = -2; i > num5; i--)
                        {
                            WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY + i, 80, true, false, -1, 0);
                        }
                        for (int j = Main.rand.Next(num5 + 1, -1); j > Main.rand.Next(num5 + 1, num5 + 3); j--)
                        {
                            WorldGen.PlaceTile(Player.tileTargetX - 1, Player.tileTargetY + j, 80, true, false, -1, 0);
                        }
                        for (int k = Main.rand.Next(num5 + 1, -1); k > Main.rand.Next(num5 + 1, num5 + 3); k--)
                        {
                            WorldGen.PlaceTile(Player.tileTargetX + 1, Player.tileTargetY + k, 80, true, false, -1, 0);
                        }
                        if (Main.rand.Next(0, 100) < 30)
                        {
                            WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY + num5 + 1, 227, true, false, -1, 6);
                        }
                        NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 99, 0);
                        NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY - 1, 99, 0);
                    }
                    return true;
                }
                if (Math.Abs(Main.MouseWorld.X - player.position.X) < 100f && Math.Abs(Main.MouseWorld.Y - player.position.Y) < 70f && (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 2 || Main.tile[Player.tileTargetX, Player.tileTargetY].type == 0) && Main.tile[Player.tileTargetX, Player.tileTargetY].active() && !Main.tile[Player.tileTargetX, Player.tileTargetY - 1].active())
                {
                    Main.PlaySound(6, -1, -1, 1, 1f, 0f);
                    int num6 = Dust.NewDust(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), 16, 16, mod.DustType("FertDust"), 0f, 0f, 0, default, 1f);
                    Main.dust[num6].scale = 3f;
                    Main.dust[num6].position = Main.MouseWorld - Main.dust[num6].scale * new Vector2(4f, 4f);
                    if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 0)
                    {
                        WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, 2, true, false, -1, 0);
                        NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 99, 0);
                    }
                    if (Main.rand.Next(0, 10) == 0)
                    {
                        dyePlants(0);
                    }
                    else if (Main.rand.Next(0, 20) == 0)
                    {
                        worldGen(0, 185, 0, 12);
                    }
                    else if (Main.rand.Next(0, 25) == 0)
                    {
                        worldGen(0, 20, 0, 3);
                    }
                    else if (Main.rand.Next(0, 5) == 0)
                    {
                        worldGen(0, 3, 6, 23);
                    }
                    else
                    {
                        worldGen(0, 3, 0, 6);
                    }
                    for (int l = 0; l < Main.rand.Next(1, 6); l++)
                    {
                        if ((Main.tile[Player.tileTargetX + l, Player.tileTargetY].type == 2 || Main.tile[Player.tileTargetX + l, Player.tileTargetY].type == 0) && Main.tile[Player.tileTargetX + l, Player.tileTargetY].active() && !Main.tile[Player.tileTargetX + l, Player.tileTargetY - 1].active())
                        {
                            if (Main.tile[Player.tileTargetX + l, Player.tileTargetY].type == 0)
                            {
                                WorldGen.PlaceTile(Player.tileTargetX + l, Player.tileTargetY, 2, true, false, -1, 0);
                                NetMessage.SendTileSquare(-1, Player.tileTargetX + l, Player.tileTargetY, 99, 0);
                            }
                            if (Main.rand.Next(0, 25) == 0)
                            {
                                dyePlants(l);
                            }
                            else if (Main.rand.Next(0, 20) == 0)
                            {
                                worldGen(l, 185, 0, 12);
                            }
                            else if (Main.rand.Next(0, 25) == 0)
                            {
                                worldGen(l, 20, 0, 3);
                            }
                            else if (Main.rand.Next(0, 5) == 0)
                            {
                                worldGen(l, 3, 6, 23);
                            }
                            else
                            {
                                worldGen(l, 3, 0, 6);
                            }
                        }
                    }
                    for (int m = 0; m > Main.rand.Next(-6, -1); m--)
                    {
                        if ((Main.tile[Player.tileTargetX + m, Player.tileTargetY].type == 2 || Main.tile[Player.tileTargetX + m, Player.tileTargetY].type == 0) && Main.tile[Player.tileTargetX + m, Player.tileTargetY].active() && !Main.tile[Player.tileTargetX + m, Player.tileTargetY - 1].active())
                        {
                            if (Main.tile[Player.tileTargetX + m, Player.tileTargetY].type == 0)
                            {
                                WorldGen.PlaceTile(Player.tileTargetX + m, Player.tileTargetY, 2, true, false, -1, 0);
                                NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 99, 0);
                            }
                            if (Main.rand.Next(0, 25) == 0)
                            {
                                dyePlants(m);
                            }
                            else if (Main.rand.Next(0, 20) == 0)
                            {
                                worldGen(m, 185, 0, 12);
                            }
                            else if (Main.rand.Next(0, 25) == 0)
                            {
                                worldGen(m, 20, 0, 3);
                            }
                            else if (Main.rand.Next(0, 5) == 0)
                            {
                                worldGen(m, 3, 6, 23);
                            }
                            else
                            {
                                worldGen(m, 3, 0, 6);
                            }
                        }
                    }
                    return true;
                }
                if (TileLoader.IsSapling(Main.tile[Player.tileTargetX, Player.tileTargetY].type) && Main.tile[Player.tileTargetX - 1, Player.tileTargetY].type != 5 && Main.tile[Player.tileTargetX + 1, Player.tileTargetY].type != 5 && Main.tile[Player.tileTargetX - 2, Player.tileTargetY].type != 5 && Main.tile[Player.tileTargetX + 2, Player.tileTargetY].type != 5)
                {
                    Main.PlaySound(6, -1, -1, 1, 1f, 0f);
                    int num7 = Dust.NewDust(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), 16, 16, mod.DustType("FertDust"), 0f, 0f, 0, default, 1f);
                    Main.dust[num7].scale = 3f;
                    Main.dust[num7].position = Main.MouseWorld - Main.dust[num7].scale * new Vector2(4f, 4f);
                    if (Main.rand.Next(0, 5) == 3)
                    {
                        WorldGen.GrowTree(Player.tileTargetX, Player.tileTargetY);
                        WorldGen.TreeGrowFXCheck(Player.tileTargetX, Player.tileTargetY);
                    }
                    return true;
                }
                if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == 59 && Main.tile[Player.tileTargetX, Player.tileTargetY].active() && !Main.tile[Player.tileTargetX, Player.tileTargetY - 1].active())
                {
                    Main.PlaySound(6, -1, -1, 1, 1f, 0f);
                    int num8 = Dust.NewDust(new Vector2(Main.MouseWorld.X, Main.MouseWorld.Y), 16, 16, mod.DustType("FertDust"), 0f, 0f, 0, default, 1f);
                    Main.dust[num8].scale = 3f;
                    Main.dust[num8].position = Main.MouseWorld - Main.dust[num8].scale * new Vector2(4f, 4f);
                    if (Main.rand.Next(0, 5) == 0)
                    {
                        int num9 = Main.rand.Next(0, 9);
                        if (num9 < 7)
                        {
                            WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, 60, true, false, -1, 0);
                            NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 99, 0);
                        }
                        else
                        {
                            WorldGen.PlaceTile(Player.tileTargetX, Player.tileTargetY, 70, true, false, -1, 0);
                            NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 99, 0);
                        }
                    }
                    return true;
                }
                return false;
            }
        }

        public virtual void worldGen(int x, int t, int r1, int r2)
        {
            WorldGen.PlaceTile(Player.tileTargetX + x, Player.tileTargetY - 1, t, true, false, -1, 0);
            Main.tile[Player.tileTargetX + x, Player.tileTargetY - 1].frameX = (short)(WorldGen.genRand.Next(r1, r2) * 18);
            NetMessage.SendTileSquare(-1, Player.tileTargetX + x, Player.tileTargetY, 99, 0);
        }

        public virtual void dyePlants(int x)
        {
            int num = Main.rand.Next(0, 50);
            if (num == 0)
            {
                Main.rand.Next(0, 4);
                if (num == 0)
                {
                    WorldGen.PlaceTile(Player.tileTargetX + x, Player.tileTargetY - 1, 227, true, false, -1, 8);
                    NetMessage.SendTileSquare(-1, Player.tileTargetX + x, Player.tileTargetY - 1, 99, 0);
                    return;
                }
                if (num == 1)
                {
                    WorldGen.PlaceTile(Player.tileTargetX + x, Player.tileTargetY - 1, 227, true, false, -1, 9);
                    NetMessage.SendTileSquare(-1, Player.tileTargetX + x, Player.tileTargetY - 1, 99, 0);
                    return;
                }
                if (num == 2)
                {
                    WorldGen.PlaceTile(Player.tileTargetX + x, Player.tileTargetY - 1, 227, true, false, -1, 10);
                    NetMessage.SendTileSquare(-1, Player.tileTargetX + x, Player.tileTargetY - 1, 99, 0);
                    return;
                }
                if (num == 3)
                {
                    WorldGen.PlaceTile(Player.tileTargetX + x, Player.tileTargetY - 1, 227, true, false, -1, 11);
                    NetMessage.SendTileSquare(-1, Player.tileTargetX + x, Player.tileTargetY - 1, 99, 0);
                    return;
                }
            }
            else
            {
                if (Main.rand.Next(0, 2) == 0)
                {
                    WorldGen.PlaceTile(Player.tileTargetX + x, Player.tileTargetY - 1, 227, true, false, -1, 3);
                    NetMessage.SendTileSquare(-1, Player.tileTargetX + x, Player.tileTargetY - 1, 99, 0);
                    return;
                }
                WorldGen.PlaceTile(Player.tileTargetX + x, Player.tileTargetY - 1, 227, true, false, -1, 4);
                NetMessage.SendTileSquare(-1, Player.tileTargetX + x, Player.tileTargetY - 1, 99, 0);
            }
        }
    }
}
