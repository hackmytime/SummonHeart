using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace SummonHeart.Items.Range.Loot
{
    public class Loot2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Loot2");
            Tooltip.SetDefault("Loot2");
            DisplayName.AddTranslation(GameCulture.Chinese, "2级生物材料");
            Tooltip.AddTranslation(GameCulture.Chinese, "用炼金术炼化敌人身躯形成的固态精华" +
                "\n吞噬+10灵魂之力" +
                "\n蕴含生物之精华");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = 6;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.maxStack = 9999;
            item.useTime = 20;
            item.useStyle = 2;
            item.UseSound = SoundID.Item4;
            item.consumable = true;
        }

        public override bool UseItem(Player player)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            if (mp.BBP >= 5000000 * 200)
            {
                player.statLife = 1;
                CombatText.NewText(player.getRect(), Color.Red, "灵魂之力已满，无法吸收");
            }
            else
            {
                int addBBP = 10;
                CombatText.NewText(player.getRect(), Color.LightGreen, $"+{addBBP}灵魂之力");
                mp.BBP += addBBP;
                if (mp.BBP > 5000000 * 200)
                    mp.BBP = 5000000 * 200;
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("Loot1"), 10);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("Loot1"), 1000);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }
}
