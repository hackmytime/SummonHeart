using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using SummonHeart.Items.Accessories;
using Microsoft.Xna.Framework;
using SummonHeart.Extensions;

namespace SummonHeart.Items
{
    public class Goddess : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Goddess");
            Tooltip.SetDefault("Use to toggle Goddess mode(Cheet Mod) Can't cancel\n" +
                "The blessing of the goddess:swallowing  10x the soul of creatures\n" +
                "The son of the demon God is seduced by the power given by the goddess");
            DisplayName.AddTranslation(GameCulture.Chinese, "女神之像");
            Tooltip.AddTranslation(GameCulture.Chinese, "使用以切换女神模式，获得女神的祝福，此模式无法取消"
            + "\n女神的祝福：赠予玩家魔神·神格？" +
            "\n负面效果：" +
            "\n1、所有泰拉瑞亚世界原生生物也会获得女神的祝福buff，每秒恢复自身1%*世界难度的生命【boss类单位上限为0.5%】" +
            "\n2、后续剧情女神获得史诗级增强");
        }

        public override void SetDefaults()
        {
            item.width = 96;
            item.height = 151;
            item.rare = 5;
            item.useAnimation = 20;
            item.useTime = 20;
            item.useStyle = 4;
            item.UseSound = SoundID.Item4;
        }

        public override bool UseItem(Player player)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartWorld.GoddessMode)
            {
                if (!player.GetModPlayer<SummonHeartPlayer>().eatGodSoul && player.HasItemInInventory(mod.ItemType("GodSoul")) == -1)
                {
                    Item.NewItem(player.Center, ModContent.ItemType<GodSoul>());
                }
                string text = player.name + "当前世界女神模式已开启，魔神之子获得了女神的祝福";
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    Main.NewText(text, new Color(175, 75, 255));
                }
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), new Color(175, 75, 255));
                }
                SummonHeartWorld.GoddessMode = true;
                return true;
            }
            else
            {
                if (!player.GetModPlayer<SummonHeartPlayer>().eatGodSoul && player.HasItemInInventory(mod.ItemType("GodSoul")) == -1)
                {
                    Item.NewItem(player.Center, ModContent.ItemType<GodSoul>());
                }
            }
            
            return base.UseItem(player);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
