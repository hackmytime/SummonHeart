using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Scrolls
{
    public class SummonScroll : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("KillScroll");
            Tooltip.SetDefault("Can synthesize the roulette of destiny of each difficulty");
            DisplayName.AddTranslation(GameCulture.Chinese, "魔神传承·召唤");
            Tooltip.AddTranslation(GameCulture.Chinese, "魔神之子使用可领悟魔神练灵诀" +
                "\n魔神根据灵魂法则所创，可练灵魂之力入体。" +
                "\n凝魂躯，斩化身，练灵眼，控万灵。" +
                "\n有道是：" +
                "\n神魂之路道法妙，魂躯初凝挡万劫!" +
                "\n神魂之眸慑八荒，掌控万灵夷四方！");
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

            if (modPlayer.PlayerClass == 0 || modPlayer.PlayerClass == 3)
            {
                if (Main.netMode == 0 || Main.netMode == 1)
                {
                    Main.NewText("已领悟召唤传承，请按L查看详情", 255, 255, 255);
                }
                modPlayer.PlayerClass = 3;
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
