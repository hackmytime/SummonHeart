using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Tools.Bomb
{
    public class BunnyiteItem : ExplosiveItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bunnyite");
            Tooltip.SetDefault("Spawns in 200 bunnies");
            DisplayName.AddTranslation(GameCulture.Chinese, "2级科技·兔子炸弹");
            Tooltip.AddTranslation(GameCulture.Chinese, "爆炸生成200只兔子");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 0;
            item.width = 20;
            item.height = 20;
            item.maxStack = 999;
            item.consumable = true;
            item.useStyle = 1;
            item.rare = 5;
            item.UseSound = SoundID.Item1;
            item.useAnimation = 20;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.noUseGraphic = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("BunnyiteProjectile");
            item.shootSpeed = 5f;
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ItemID.Dynamite, 30);
            modRecipe.AddIngredient(ItemID.Bunny, 10);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
    }
}
