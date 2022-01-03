using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using SummonHeart.ui;

namespace SummonHeart.Items.Scrolls
{
    public class XiuXianScroll : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("XiuXianScroll");
            Tooltip.SetDefault("Can synthesize the roulette of destiny of each difficulty");
            DisplayName.AddTranslation(GameCulture.Chinese, "修仙者");
            Tooltip.AddTranslation(GameCulture.Chinese, "你是高维宇宙仙道势力入侵者" +
                "\n传承『轮回仙经』" +
                "\n凡人→炼气→筑基→金丹→元婴→化神→合体→渡劫→大乘→散仙→金仙→大罗金仙→混元大罗金仙→半圣→圣人→圣尊→圣王→无上境→道境" +
                "\n有道是：" +
                "\n仙路尽头谁为峰" +
                "\n答案将由你谱写" +
                "\n获得传承后，会给你30点道源，请打开天赋界面，决定初始加点，无法更改" +
                "\n提示：修仙需有灵根，无灵根无法修仙" +
                "\n提示：点击修炼小人可以开关天赋界面"
                );
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

            if (modPlayer.PlayerClass == 0 || modPlayer.PlayerClass == 9)
            {
                if (Main.netMode == 0 || Main.netMode == 1)
                {
                    Main.NewText("已领悟『轮回仙经』获得30点初始道源，请打开天赋界面，决定初始加点，无法更改", 255, 255, 255);
                }
                modPlayer.PlayerClass = 9;
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
