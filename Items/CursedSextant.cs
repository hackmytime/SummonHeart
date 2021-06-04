using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items
{
    public class CursedSextant : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("CursedSextant");
            Tooltip.SetDefault("Starts the Blood Moon");
            DisplayName.AddTranslation(GameCulture.Chinese, "血色六分仪");
            Tooltip.AddTranslation(GameCulture.Chinese, "召唤血月");
        }

        public override void SetDefaults()
        {
            item.width = item.height = 20;
            item.maxStack = 20;
            item.value = Item.sellPrice(silver: 2);
            item.rare = ItemRarityID.Blue;
            item.useAnimation = item.useTime = 30;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.consumable = true;
        }

        public override bool CanUseItem(Player player) => !Main.dayTime && !Main.bloodMoon;

        public override bool UseItem(Player player)
        {
            Main.bloodMoon = true;

            NetMessage.SendData(MessageID.WorldData);
            Main.NewText("The Blood Moon is rising...", new Color(175, 75, 255));

            return true;
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(1329, 30);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}