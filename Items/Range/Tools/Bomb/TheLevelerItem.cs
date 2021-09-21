using SummonHeart.Items.Range.AmmoSkill;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Tools.Bomb
{
    public class TheLevelerItem : ExplosiveItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("TheLevelerItem");
            Tooltip.SetDefault("TheLevelerItem");
            DisplayName.AddTranslation(GameCulture.Chinese, "2级科技·地表工程炸弹");
            Tooltip.AddTranslation(GameCulture.Chinese, "用于平整地形，爆炸范围200x10" +
                "\n会破坏墙壁" +
                "\n无爆炸伤害");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 0;
            item.width = 10;
            item.height = 32;
            item.maxStack = 999;
            item.consumable = true;
            item.useStyle = 1;
            item.rare = 7;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 20;
            item.useTime = 100;
            item.value = Item.sellPrice(1, 0, 0, 0);
            item.noUseGraphic = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("TheLevelerProjectile");
            item.shootSpeed = 5f;
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ItemID.Dynamite, 30);
            modRecipe.AddIngredient(ItemID.Gel, 1);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
