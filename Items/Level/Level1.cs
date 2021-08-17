using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Level
{
    public class Level1 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Level1");
            Tooltip.SetDefault("Change the World Level to Lv1");
            DisplayName.AddTranslation(GameCulture.Chinese, "命运轮盘·魔神之子");
            Tooltip.AddTranslation(GameCulture.Chinese, 
                "Lv1魔神之子(默认难度)" +
                "\n所有敌人2倍血量" +
                "\n所有敌人4倍攻击" +
                "\n所有敌人减伤倍率：2倍" +
                "\n所有敌人金钱掉落倍率：5倍" +
                "\n灵魂气血获取倍率：1倍" +
                "\n世界肉身总气血上限：40W" +
                "\n额外饰品栏：1个" +
                "\n评价：体验主角的待遇");
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
            item.consumable = true;
        }

        public override bool UseItem(Player player)
        {
            if(SummonHeartWorld.WorldLevel < 1)
            {
                if (Main.netMode == 0 || Main.netMode == 1)
                {
                    Main.NewText("当前世界难度为：魔神之子", 255, 255, 255);
                }
                SummonHeartWorld.WorldLevel = 1;
                return true;
            }
            else
            {
                Main.NewText("使用无效，当前世界难度已经设定过。", 255, 255, 255);
                return false;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("Level0"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
