using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items
{
    public class BattleCry : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("BattleCry");
            Tooltip.SetDefault("Increase spawn rates by 20x on use" +
                               "\nUse it again to decrease them");
            DisplayName.AddTranslation(GameCulture.Chinese, "战争号角");
            Tooltip.AddTranslation(GameCulture.Chinese, "20x刷怪率" +
                               "\n再次使用可关闭");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.sellPrice(0, 0, 2);
            item.rare = ItemRarityID.Pink;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = ItemUseStyleID.HoldingUp;
        }

        public override bool UseItem(Player player)
        {

            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
            modPlayer.BattleCry = !modPlayer.BattleCry;

            string text = "战争号角 " + (modPlayer.BattleCry ? "开启!" : "关闭!");
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(text, new Color(175, 75, 255));
            }
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), new Color(175, 75, 255));
            }

            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}