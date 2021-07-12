using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Scrolls
{
    public class KillScroll : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("KillScroll");
            Tooltip.SetDefault("Can synthesize the roulette of destiny of each difficulty");
            DisplayName.AddTranslation(GameCulture.Chinese, "魔神传承·刺客");
            Tooltip.AddTranslation(GameCulture.Chinese, "魔神之子使用可领悟弑灵戮神陨" +
                "\n魔神根据杀戮之道所创的魔教传教功法，练杀伐之意入体。" +
                "\n修炼此功法大成者" +
                "\n神念摄魂心为眼，杀意无形身化影。" +
                "\n山河万里随心至，动若崩雷众神惊！");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = 5;
            item.useAnimation = 20;
            item.useTime = 20;
            item.useStyle = 4;
            item.UseSound = SoundID.Item4;
            item.noUseGraphic = true;
        }

        public override bool UseItem(Player player)
        {
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

            if (modPlayer.PlayerClass == 0 || modPlayer.PlayerClass == 2)
            {
                if (Main.netMode == 0 || Main.netMode == 1)
                {
                    Main.NewText("已领悟刺客传承，请按L查看详情", 255, 255, 255);
                }
                modPlayer.PlayerClass = 2;
                return true;
            }
            else
            {
                Main.NewText("道心不坚，难成大器。", 255, 255, 255);
                return false;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("DemonScroll"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
