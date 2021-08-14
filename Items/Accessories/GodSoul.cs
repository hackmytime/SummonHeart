using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Extensions;

namespace SummonHeart.Items.Accessories
{
    public class GodSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SoulCrystal");
            Tooltip.SetDefault("The son of a demon God is the soul of his own life. " +
                "He has boundless power and can be obtained when he cuts off his external incarnation\n");
            DisplayName.AddTranslation(GameCulture.Chinese, "魔神·神格？");
            Tooltip.AddTranslation(GameCulture.Chinese, "女神模式专属饰品，女神赠予的神格。" +
                "\n吞噬获得神格之力：额外饰品栏上限增加至24" +
                "\n泰拉世界生物不可直视，仅魔神之子开启女神模式时可获得");
        }

        public override void SetDefaults()
        {
            item.width = 64;
            item.height = 64;
            item.rare = -12;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.consumable = true;
			item.useAnimation = 20;
			item.useTime = 20;
			item.useStyle = 4;
			item.UseSound = SoundID.Item4;
		}

        public override bool UseItem(Player player)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
           
            mp.eatGodSoul = true;
            int extraAccessories = 14 + SummonHeartWorld.WorldLevel * 2;
            string text = player.name + "吞噬了魔神·神格？获得神格之力，额外饰品栏增加至" + extraAccessories;
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(text, new Color(175, 75, 255));
            }
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), new Color(175, 75, 255));
            }
            return true;
        }

        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Name == "ItemName" && line.mod == "Terraria")
            {
                GodSoul.EpicTooltipLine(line, ref yOffset);
                return false;
            }
            return true;
        }

        public static void EpicTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, line.font, line.text, new Vector2(line.X, line.Y), Color.Lerp(Color.BlueViolet, Color.MediumVioletRed, Helper.Osc01(6f, 0f)), line.rotation, line.origin, line.baseScale, line.maxWidth, line.spread);
            Main.spriteBatch.PushBlendState(BlendState.Additive, true, delegate ()
            {
                Vector2 size = line.font.MeasureString(line.text);
                Main.spriteBatch.Draw(SummonHeartMod.Instance.GetTexture("Projectiles/Effects/LightTransparent"), new Vector2(line.X, line.Y - 4) + size / 2f, null, Color.Lerp(Color.BlueViolet, Color.MediumVioletRed, Helper.Osc01(6f, 0f)).MultiplyAlpha(0.75f), 0f, SummonHeartMod.Instance.GetTexture("Projectiles/Effects/LightTransparent").Size() / 2f, new Vector2(size.X / Helper.Osc(40f, 50f, 4f, 0f), 0.25f), 0, 0f);
                for (float i = -3.1415927f; i <= 3.1415927f; i += 1.5707964f)
                {
                    ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, line.font, line.text, new Vector2(line.X, line.Y) + i.ToRotationVector2().RotatedBy(Main.GlobalTime * 2f, default) * Helper.Osc(0f, 3f, 12f, 0f), Color.SkyBlue.MultiplyAlpha(Helper.Osc(0.5f, 0.75f, 12f, 0f)), line.rotation + Main.rand.NextFloatRange(0.05f), line.origin, line.baseScale, line.maxWidth, line.spread);
                }
                for (float j = -3.1415927f; j <= 3.1415927f; j += 1.5707964f)
                {
                    ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, line.font, line.text, new Vector2(line.X, line.Y) + j.ToRotationVector2().RotatedBy((double)(-(double)Main.GlobalTime * 2f), default) * Helper.Osc(0f, 3f, 12f, 0.5f), Color.MediumVioletRed.MultiplyAlpha(Helper.Osc(0.5f, 0.75f, 12f, 0.5f)), line.rotation + Main.rand.NextFloatRange(0.05f), line.origin, line.baseScale, line.maxWidth, line.spread);
                }
            }, null);
        }
	}
}
