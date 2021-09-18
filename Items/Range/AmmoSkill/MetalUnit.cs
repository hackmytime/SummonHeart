using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Range.AmmoSkill
{
    public class MetalUnit : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MetalUnit");
            Tooltip.SetDefault("MetalUnit");
            DisplayName.AddTranslation(GameCulture.Chinese, "金属元件");
            Tooltip.AddTranslation(GameCulture.Chinese, "蕴含金属精华的炼金元件" +
                "\n造价高昂，品质保证");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = -12;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.CopperBar, 20);recipe.SetResult(this, 1);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.TinBar, 20);recipe.SetResult(this, 1);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.IronBar, 20);recipe.SetResult(this, 2);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.LeadBar, 20);recipe.SetResult(this, 2);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.SilverBar, 20);recipe.SetResult(this, 3);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.TungstenBar, 20);recipe.SetResult(this, 3);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.GoldBar, 20);recipe.SetResult(this, 4);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.PlatinumBar, 20);recipe.SetResult(this, 4);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.MeteoriteBar, 20);recipe.SetResult(this, 4);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.DemoniteBar, 20);recipe.SetResult(this, 5);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.CrimtaneBar, 20);recipe.SetResult(this, 5);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.HellstoneBar, 20);recipe.SetResult(this, 5);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.CobaltBar, 20);recipe.SetResult(this, 6);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.PalladiumBar, 20);recipe.SetResult(this, 6);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.MythrilBar, 20);recipe.SetResult(this, 6);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.OrichalcumBar, 20);recipe.SetResult(this, 6);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.AdamantiteBar, 20);recipe.SetResult(this, 6);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.TitaniumBar, 20);recipe.SetResult(this, 6);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.HallowedBar, 20);recipe.SetResult(this, 7);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.ChlorophyteBar, 20);recipe.SetResult(this, 8);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.LunarBar, 20);recipe.SetResult(this, 9);recipe.AddRecipe();
        }
    }
}
