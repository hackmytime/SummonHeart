using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Tools
{
    public class AutoBuildingTool : ModItem
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
            DisplayName.AddTranslation(GameCulture.Chinese, "自动建筑装置");
            Tooltip.SetDefault("在鼠标位置为起点自动放置物块，一次最多100个\n放置的物块为背包内符合放置要求的第一个");
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

        public override bool CanUseItem(Player player)
        {
            return !Main.tile[(int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16].active() && GetBuildItem() != null && base.CanUseItem(player);
        }

        public override bool UseItem(Player player)
        {
            num = 100;
            Item item2 = GetBuildItem();
            buildTile(item2, (int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16);
            if (item2.stack == 0)
            {
                item2.active = false;
            }
            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex((t) => t.Name.Equals("Tooltip1"));
            string text = "现在放置：";
            Item item2 = GetBuildItem();
            text += item2 == null ? "没有符合条件的物品" : item2.Name;
            TooltipLine tipLine = new TooltipLine(mod, "BuildingRod", text);
            tooltips.Insert(index + 1, tipLine);
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
            modRecipe.AddIngredient(ItemID.IronBar, 99);
            modRecipe.AddTile(TileID.WorkBenches);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }

        private int num;
    }
}
