using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Items.BalloonBundle
{
    public class QuadrupleHorseshoeBalloonBundle : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("大马掌气球束");
            Tooltip.SetDefault("可让持有者五连跳\n增加跳跃高度\n免疫坠落伤害");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 60000;
            item.rare = 9;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.jumpBoost = true;
            player.doubleJumpCloud = true;
            player.doubleJumpBlizzard = true;
            player.doubleJumpSandstorm = true;
            player.doubleJumpFart = true;
            player.noFallDmg = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BundleofBalloons);
            recipe.AddIngredient(ItemID.FartInABalloon);
            recipe.AddIngredient(ItemID.LuckyHorseshoe);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
