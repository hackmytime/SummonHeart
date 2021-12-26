using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace SummonHeart.Items.Range.Loot
{
    public class Loot1 : LootItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Loot1");
            Tooltip.SetDefault("Loot1");
            DisplayName.AddTranslation(GameCulture.Chinese, "1级生物材料");
            Tooltip.AddTranslation(GameCulture.Chinese, "用炼金术炼化敌人身躯形成的固态精华" +
                "\n吞噬+1灵魂之力" +
                "\n蕴含各种实体物质");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = 5;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.maxStack = 99990;
            item.useAnimation = 20;
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
                int addBBP = 1;
                CombatText.NewText(player.getRect(), Color.LightGreen, $"+{addBBP}灵魂之力");
                mp.BBP += addBBP;
                if (mp.BBP > 5000000 * 200)
                    mp.BBP = 5000000 * 200;
            }
            return true;
        }
    }
}
