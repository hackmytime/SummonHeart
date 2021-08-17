using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Level
{
    public class Level5 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Level5");
            Tooltip.SetDefault("Change the World Level to Lv5");
            DisplayName.AddTranslation(GameCulture.Chinese, "命运轮盘·？？？？");
            Tooltip.AddTranslation(GameCulture.Chinese,
                "Lv5？？？？(？？？级难度)" +
                "\n所有敌人6倍血量" +
                "\n所有敌人30倍攻击" +
                "\n所有敌人减伤倍率：10倍" +
                "\n所有敌人金钱掉落倍率：30倍" +
                "\n灵魂气血获取倍率：5倍" +
                "\n世界肉身总气血上限：80W" +
                "\n初始肉身极限：60000" +
                "\n额外饰品栏：8个" +
                "\n评价：......");
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
            if(SummonHeartWorld.WorldLevel < 5)
            {
                if (Main.netMode == 0 || Main.netMode == 1)
                {
                    Main.NewText("当前世界难度为：？？？？", 255, 255, 255);
                }
                if (ModLoader.GetMod("Luiafk") != null)
                {
                    player.QuickSpawnItem(ModLoader.GetMod("Luiafk").ItemType("TimeChanger"), 1);
                    Main.NewText("神秘的声音：勇气可嘉，送你一件礼物，减少你的自闭概率", 255, 255, 255);
                }
                SummonHeartWorld.WorldLevel = 5;
                SummonHeartWorld.WorldBloodGasMax = 800000;
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
