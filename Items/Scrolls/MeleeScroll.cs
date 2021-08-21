using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Scrolls
{
    public class MeleeScroll : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MeleeScroll");
            Tooltip.SetDefault("Can synthesize the roulette of destiny of each difficulty");
            DisplayName.AddTranslation(GameCulture.Chinese, "魔神传承·战士·泰坦");
            Tooltip.AddTranslation(GameCulture.Chinese, "魔神之子使用可领悟魔神炼体法" +
                "\n崩碎身躯把灵魂之力炼化溶于肉身，大幅提升肉身极限" +
                "\n肉身将拥有吞血肉凝练的逆天之力" +
                "\n泰坦被动：战者之心，死亡后增加1基础生命上限，0.2基础防御力。" +
                "\n增加的生命值最大不能超过玩家当前生命值，增加的防御力最大不能超过肉体强度" +
                "\n玩家肉体强度增加的防御力翻倍" +
                "\n转职为泰坦初始即可获得300基础生命上限，减少33%跳跃速度" +
                "\n泰坦魔神之躯的初始奖励提升为+300基础生命上限+50%独立伤害减免");
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

            if (modPlayer.PlayerClass == 0 || modPlayer.PlayerClass == 1)
            {
                if (Main.netMode == 0 || Main.netMode == 1)
                {
                    Main.NewText("已领悟战士·泰坦传承，请按L查看详情", 255, 255, 255);
                }
                modPlayer.PlayerClass = 1;
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
