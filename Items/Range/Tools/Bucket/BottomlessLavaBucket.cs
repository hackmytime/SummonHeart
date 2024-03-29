﻿using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Tools.Bucket
{
    public class BottomlessLavaBucket : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("+2 range\nContains an endless amount of lava");
            DisplayName.AddTranslation(GameCulture.Chinese, "科技造物·次元岩浆桶");
            Tooltip.AddTranslation(GameCulture.Chinese, "左键装岩浆，右键放岩浆" +
                "\n范围+25" +
                "\n1次可装25格液体" +
                "\n容量9999格");
        }

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 40;
            item.useTime = 1;
            item.useAnimation = 4;
            item.useStyle = 1;
            item.value = 500000;
            item.rare = 7;
            item.autoReuse = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void HoldItem(Player player)
        {
            BucketGItem bg = item.GetGlobalItem<BucketGItem>();
            bg.liquidType = 2;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("MetalUnit"), 50);
            recipe.AddIngredient(mod.GetItem("Power3"), 1);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
