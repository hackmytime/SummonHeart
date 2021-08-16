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
                "\n使用返回死亡点，可设置快捷键使用，默认Z" +
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
        }

        public override bool CanUseItem(Player player)
        {
            return player.showLastDeath;
        }

        public override bool UseItem(Player player)
        {
            Vector2 vector = new Vector2(player.lastDeathPostion.X - 16f, player.lastDeathPostion.Y - 24f);
            player.Teleport(vector, 0, 0);
            return base.ConsumeItem(player);
        }

        /*public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (Main.time % 10.0 == 0.0)
            {
                int startX = (int)(player.Center.X / 16f) - 50;
                int startY = (int)(player.Center.Y / 16f) - 50;
                int endX = startX + 100;
                int endY = startY + 100;
                for (int i = startX; i < endX; i++)
                {
                    for (int j = startY; j < endY; j++)
                    {
                        if (WorldGen.InWorld(i, j, 0))
                        {
                            //Main.Map.Update(i, j, (byte)(200 - 2 * (byte)Vector2.Distance(player.Center / 16f, new Vector2(i, j))));
                            Main.Map.Update(i, j, 100);
                        }
                    }
                }
                Main.refreshMap = true;
            }
        }*/
    }
}