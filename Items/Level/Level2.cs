using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Level
{
    public class Level2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Level2");
            Tooltip.SetDefault("Change the World Level to Lv2");
            DisplayName.AddTranslation(GameCulture.Chinese, "命运轮盘·魔神之路");
            Tooltip.AddTranslation(GameCulture.Chinese,
                "\n所有敌人4倍血量" +
                "\n所有敌人8倍攻击" +
                "\n所有敌人减伤倍率：2倍" +
                "\n所有敌人金钱掉落倍率：10倍" +
                "\n灵魂气血获取倍率：2倍" +
                "\n世界肉身总气血上限：50W" +
                "\n额外饰品栏：2个" +
                "\n评价：硬核入门，你不正常");
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
            if(SummonHeartWorld.WorldLevel == 0)
            {
                if (Main.netMode == 0 || Main.netMode == 1)
                {
                    Main.NewText("当前世界难度为：魔神之路", 255, 255, 255);
                }
                SummonHeartWorld.WorldLevel = 2;
                SummonHeartWorld.WorldBloodGasMax = 500000;
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
