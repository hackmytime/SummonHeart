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
                "\n泰坦被动：伤害吸收，仇恨值增加20000点，泰坦吸收的所有伤害值会被保存起来，最大值为999999。" +
                "\n主动：双倍偿还，开启后，下一次武器攻击附加伤害吸收量双倍的真实伤害" +
                "\n玩家肉体强度增加的防御力翻倍，减伤倍率+1" +
                "\n转职为泰坦初始即可获得300基础生命上限，减少33%跳跃速度" +
                "\n泰坦魔神之躯的初始奖励提升为+300基础生命上限+4减伤倍率");
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
