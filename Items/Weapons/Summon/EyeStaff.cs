using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Weapons.Summon
{
    public class EyeStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons 2 mini Eye of Cthulhus to dash at your enemies!");
            DisplayName.AddTranslation(GameCulture.Chinese, "疯狗克眼杖");
            Tooltip.AddTranslation(GameCulture.Chinese, "召唤师流下了幸福的眼泪");
        }

        public override void SetDefaults()
        {
            item.damage = 81;
            item.summon = true;
            item.useTime = 21;
            item.useAnimation = 23;
            item.useStyle = 1;
            item.knockBack = 8.15f;
            item.rare = -12;
            item.value = Item.sellPrice(8, 15, 0, 0);
            item.UseSound = SoundID.Item44;
            item.autoReuse = false;
            item.shoot = 388;
            item.shootSpeed = 10f;
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ItemID.Lens, 815);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
