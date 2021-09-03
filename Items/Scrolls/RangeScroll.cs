using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Scrolls
{
    public class RangeScroll : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MagicScroll");
            Tooltip.SetDefault("Can synthesize the roulette of destiny of each difficulty");
            DisplayName.AddTranslation(GameCulture.Chinese, "魔神传承·射手");
            Tooltip.AddTranslation(GameCulture.Chinese, "魔神之子使用可领悟魔神炼金术" +
                "\n魔神炼器之法，包含技之道的精髓" +
                "\n结合魔神之子高维生物投影记忆带来的现代科技" +
                "\n产生了无法预估的变故" +
                "\n有道是：" +
                "\n以凡人之身比肩神明者" +
                "\n唯科技之道是也" +
                "\n提示：科技之道很肝，非肝帝者慎重选择。");
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
            item.consumable = true;
        }

        public override bool UseItem(Player player)
        {
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

            if (modPlayer.PlayerClass == 0 || modPlayer.PlayerClass == 5)
            {
                if (Main.netMode == 0 || Main.netMode == 1)
                {
                    Main.NewText("已领悟射手传承，开局什么都没有，祝你好运。", 255, 255, 255);
                }
                modPlayer.PlayerClass = 7;
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
