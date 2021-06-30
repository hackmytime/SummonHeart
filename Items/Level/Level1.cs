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
                "\n所有敌人5倍血防" +
                "\n所有敌人2倍攻击" +
                "\n灵魂气血获取倍率：1倍" +
                "\n世界肉身总气血上限：40W" +
                "\n单部位炼体气血上限：10W" +
                "\n初始肉身极限：10000" +
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
            if(SummonHeartWorld.WorldLevel == 0)
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
