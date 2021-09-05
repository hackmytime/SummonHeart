﻿using Microsoft.Xna.Framework;
using SummonHeart.Extensions;
using SummonHeart.Items.Skill.Tools;
using System;
using System.Collections.Generic;
using System.Threading;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Tools
{
    public class AutoHouseTool : ModItem
    {
        private Player owner
        {
            get
            {
                return Main.player[item.owner];
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.AddTranslation(GameCulture.Chinese, "自动房屋建筑装置");
            Tooltip.SetDefault("在鼠标位置为起点以玩家朝向为方向自动建筑房屋，一次最多建造10个\n1个房屋需要消耗76木头+2凝胶");
        }

        public override void SetDefaults()
        {
            item.height = 38;
            item.width = 38;
            item.maxStack = 1;
            item.rare = 2;
            item.value = 100000;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 1;
        }

        public override void HoldItem(Player player)
        {
            
            base.HoldItem(player);
        }

        public override bool UseItem(Player player)
        {
            Vector2 mousePosition = Main.MouseWorld;
            SummonHeartPlayer mp = player.SH();

            int tileX = (int)(mousePosition.X / 16f);
            int tileY = (int)(mousePosition.Y / 16f);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                if (player.whoAmI != Main.LocalPlayer.whoAmI)
                {
                    return true;
                }
                Vector2 mouseWorld = Main.MouseWorld;
                tileX = (int)(mouseWorld.X / 16f);
                tileY = (int)(mouseWorld.Y / 16f);
                if (Builder.BuildHouse(tileX, tileY, 0, true))
                {
                    Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>().houseTileX = tileX;
                    Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>().houseTileY = tileY;
                    Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>().houesType = 0;
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    int direction = player.direction;
                    int newTileX = tileX + i * 4 * direction;
                    if (direction == -1)
                        newTileX -= 4;
                    if (Builder.BuildHouse(newTileX, tileY, 0, true))
                    {
                        mp.houseTileX = newTileX;
                        mp.houseTileY = tileY;
                        mp.houesType = 0;
                    }
                }
            }
            return true;
        }

        private Item GetBuildItem()
        {
            foreach (Item item2 in owner.inventory)
            {
                if (item2.stack > 0 && item2.createTile != -1)
                {
                    return item2;
                }
            }
            return null;
        }

        private void buildTile(Item item2, int i, int j)
        {
            if (!Main.tile[i, j].active())
            {
                int stack = num;
                num = stack - 1;
                if (stack > 0)
                {
                    stack = item2.stack;
                    item2.stack = stack - 1;
                    if (stack > 0)
                    {
                        if (!WorldGen.PlaceTile(i, j, item2.createTile, false, false, -1, item2.placeStyle))
                        {
                            item2.stack++;
                        }
                        buildTile(item2, i + owner.direction, j);
                    }
                }
            }
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ItemID.Wood, 99);
            modRecipe.AddTile(TileID.WorkBenches);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }

        private int num;
    }
}