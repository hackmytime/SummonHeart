using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Scrolls
{
    public class MagicScroll2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MagicScroll");
            Tooltip.SetDefault("Can synthesize the roulette of destiny of each difficulty");
            DisplayName.AddTranslation(GameCulture.Chinese, "魔神传承·法师·控法者");
            Tooltip.AddTranslation(GameCulture.Chinese, "魔神之子使用可领悟魔神淬法诀" +
                "\n魔神领悟道之本源用杀戮法则所创，万物皆有法力存于体内" +
                "\n击杀可炼化其血肉中蕴含的法力入体，大大提升肉身法力储量" +
                "\n炼至巅峰，成就不灭法体。可掌控充能叠加魔法，万法不侵。" +
                "\n有道是：" +
                "\n杀戮众生淬法体，月华灵眸破妄虚!" +
                "\n意起身随凌空至，掌过星流化荒芜！");
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

            if (modPlayer.PlayerClass == 0 || modPlayer.PlayerClass == 6)
            {
                if (Main.netMode == 0 || Main.netMode == 1)
                {
                    Main.NewText("已领悟控法者传承，请按L查看详情", 255, 255, 255);
                }
                modPlayer.PlayerClass = 6;
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
