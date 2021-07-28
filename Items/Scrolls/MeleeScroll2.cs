using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Scrolls
{
    public class MeleeScroll2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MeleeScroll");
            Tooltip.SetDefault("Can synthesize the roulette of destiny of each difficulty");
            DisplayName.AddTranslation(GameCulture.Chinese, "魔神传承·战士·狂战");
            Tooltip.AddTranslation(GameCulture.Chinese, "魔神之子使用可领悟魔神炼体法" +
                "\n崩碎身躯把灵魂之力炼化溶于肉身，大幅提升肉身极限" +
                "\n肉身将拥有吞血肉凝练的逆天之力。" +
                "\n狂战被动：无尽怒火，解锁怒气系统，死亡后增加1怒气上限。" +
                "\n怒气系统，暴击时+3怒气值，初始上限100，每一点怒气值增加1%近战暴击率。" +
                "\n怒气值超过100时，每1点怒气值增加0.5%近战暴击伤害" +
                "\n死亡时，如果怒气值满，则触发被动，不灭怒火，每秒消耗25点怒气值，免疫一切伤害，归0时死亡" +
                "\n不灭怒火状态下，暴击无法增加怒气" +
                "\n怒气值上限500，最多无敌20秒，最多增加200%暴击伤害" +
                "\n转职为狂战初始+20%移动速度，+20%攻击速度，+33%跳跃速度");
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
                    Main.NewText("已领悟战士·狂战传承，请按L查看详情", 255, 255, 255);
                }
                modPlayer.PlayerClass = 4;
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
