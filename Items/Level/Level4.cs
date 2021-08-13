using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Level
{
    public class Level4 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Level4");
            Tooltip.SetDefault("Change the World Level to Lv4");
            DisplayName.AddTranslation(GameCulture.Chinese, "命运轮盘·逆天而行");
            Tooltip.AddTranslation(GameCulture.Chinese,
                "\nLv4逆天而行(sss级难度)" +
                "\n所有敌人8倍血量" +
                "\n所有敌人16倍攻击" +
                "\n所有敌人减伤倍率：4倍" +
                "\n所有敌人金钱掉落倍率：20倍" +
                "\n灵魂气血获取倍率：4倍" +
                "\n世界肉身总气血上限：70W" +
                "\n额外饰品栏：4个" +
                "\n评价：逆天而行，勇士走好");
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
                    Main.NewText("当前世界难度为：逆天而行", 255, 255, 255);
                }
                SummonHeartWorld.WorldLevel = 4;
                SummonHeartWorld.WorldBloodGasMax = 700000;
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
