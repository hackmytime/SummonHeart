using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace SummonHeart.Items.Range.Loot
{
    public class Loot5 : LootItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Loot5");
            Tooltip.SetDefault("Loot5");
            DisplayName.AddTranslation(GameCulture.Chinese, "5级生物材料");
            Tooltip.AddTranslation(GameCulture.Chinese, "用炼金术炼化敌人身躯形成的固态精华" +
                "\n吞噬+10000灵魂之力" +
                "\n蕴含超量生物之精华");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = 9;
            item.value = Item.sellPrice(100, 0, 0, 0);
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
                int addBBP = 10000;
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
            recipe.AddIngredient(mod.GetItem("Loot4"), 10);
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("Loot4"), 100);
            recipe.SetResult(this, 10);
            recipe.AddRecipe();
        }
    }
}
