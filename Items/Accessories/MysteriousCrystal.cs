using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Accessories
{
    public class MysteriousCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MysteriousCrystal");
            Tooltip.SetDefault("Resurrection time reduced to 5 seconds");
            DisplayName.AddTranslation(GameCulture.Chinese, "神秘水晶");
            Tooltip.AddTranslation(GameCulture.Chinese, "复活时间减为2秒(Boss存活时无效)" +
                "\n装备可自动拾取金币到储蓄罐" +
                "\n装备可自动拾取35码范围内的物品" +
                "\n装备可快速回城，需设置快捷键使用，默认B" +
                "\n装备可返回死亡点，可设置快捷键使用，默认Z" +
                "\n左键使用消耗500灵魂之力召唤旅商" +
                "\n给挑战者的礼物");
        }

        public override void SetDefaults()
        {
            item.rare = 4;
            item.width = 32;
            item.height = 32;
            item.value = 4 * 100000;
            item.accessory = true;
            item.useStyle = 4;
            item.UseSound = SoundID.Item4;
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override bool UseItem(Player player)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            if (mp.BBP < 500)
            {
                CombatText.NewText(player.getRect(), Color.Red, "灵魂之力不足，无法召唤");
            }
            else
            {
                CombatText.NewText(player.getRect(), Color.Red, "-500灵魂之力");
                mp.BBP -= 500;
                NPC.SpawnOnPlayer(player.whoAmI, NPCID.TravellingMerchant);
            }
            return true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            mp.MysteriousCrystal = true;
        }
    }
}