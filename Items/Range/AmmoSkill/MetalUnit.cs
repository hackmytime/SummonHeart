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
            ModRecipe recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.CopperBar, 100);recipe.SetResult(this, 10);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.TinBar, 100);recipe.SetResult(this, 10);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.IronBar, 100);recipe.SetResult(this, 20);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.LeadBar, 100);recipe.SetResult(this, 20);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.SilverBar, 100);recipe.SetResult(this, 30);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.TungstenBar, 100);recipe.SetResult(this, 30);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.GoldBar, 100);recipe.SetResult(this, 40);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.PlatinumBar, 100);recipe.SetResult(this, 40);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.MeteoriteBar, 100);recipe.SetResult(this, 40);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.DemoniteBar, 100);recipe.SetResult(this, 50);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.CrimtaneBar, 100);recipe.SetResult(this, 50);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.HellstoneBar, 100);recipe.SetResult(this, 50);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.CobaltBar, 100);recipe.SetResult(this, 60);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.PalladiumBar, 100);recipe.SetResult(this, 60);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.MythrilBar, 100);recipe.SetResult(this, 60);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.OrichalcumBar, 100);recipe.SetResult(this, 60);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.AdamantiteBar, 100);recipe.SetResult(this, 60);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.TitaniumBar, 100);recipe.SetResult(this, 60);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.HallowedBar, 100);recipe.SetResult(this, 70);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.ChlorophyteBar, 100);recipe.SetResult(this, 80);recipe.AddRecipe();
            recipe = new ModRecipe(mod);recipe.AddIngredient(ItemID.LunarBar, 100);recipe.SetResult(this, 90);recipe.AddRecipe();
        }
    }
}
