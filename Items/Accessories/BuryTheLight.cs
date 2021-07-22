using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Extensions;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace SummonHeart.Items.Accessories
{
    // Token: 0x02000580 RID: 1408
    public class BuryTheLight : ModItem
    {
        // Token: 0x06001D9B RID: 7579 RVA: 0x000E2C08 File Offset: 0x000E0E08
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bury the Light");
            DisplayName.AddTranslation(GameCulture.Russian, "Погребенный свет");
            Tooltip.SetDefault("An epic sword that attacks whenever you do\nIts damage is affected by all class bonuses\nDoes not appear if placed in the hidden slot");
            Tooltip.AddTranslation(GameCulture.Russian, "Эпический меч, атакующий вслед за вами\nНа его урон влияют все классовые бонусы\nНе появляется, если помещён в скрытый слот");
        }

        // Token: 0x06001D9C RID: 7580 RVA: 0x000E2C60 File Offset: 0x000E0E60
        public override void SetDefaults()
        {
            item.width = item.height = 72;
            item.accessory = true;
            item.knockBack = 4.5f;
            item.value = Item.sellPrice(0, 22, 0, 0);
            item.rare = -12;
        }

        // Token: 0x06001D9D RID: 7581 RVA: 0x000E2CC6 File Offset: 0x000E0EC6
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Name == "ItemName" && line.mod == "Terraria")
            {
                EpicTooltipLine(line, ref yOffset);
                return false;
            }
            return true;
        }

        // Token: 0x06001D9E RID: 7582 RVA: 0x000E2CF8 File Offset: 0x000E0EF8
        public static void EpicTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, line.font, line.text, new Vector2(line.X, line.Y), Color.Lerp(Color.BlueViolet, Color.MediumVioletRed, Helper.Osc01(6f, 0f)), line.rotation, line.origin, line.baseScale, line.maxWidth, line.spread);
            Main.spriteBatch.PushBlendState(BlendState.Additive, true, delegate ()
            {
                Vector2 size = line.font.MeasureString(line.text);
                Main.spriteBatch.Draw(SummonHeartMod.Instance.GetTexture("Projectiles/Effects/LightTransparent"), new Vector2((float)line.X, (float)(line.Y - 4)) + size / 2f, null, Color.Lerp(Color.BlueViolet, Color.MediumVioletRed, Helper.Osc01(6f, 0f)).MultiplyAlpha(0.75f), 0f, Utils.Size(SummonHeartMod.Instance.GetTexture("Projectiles/Effects/LightTransparent")) / 2f, new Vector2(size.X / Helper.Osc(40f, 50f, 4f, 0f), 0.25f), 0, 0f);
                for (float i = -3.1415927f; i <= 3.1415927f; i += 1.5707964f)
                {
                    ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, line.font, line.text, new Vector2((float)line.X, (float)line.Y) + i.ToRotationVector2().RotatedBy((double)(Main.GlobalTime * 2f), default(Vector2)) * Helper.Osc(0f, 3f, 12f, 0f), Color.SkyBlue.MultiplyAlpha(Helper.Osc(0.5f, 0.75f, 12f, 0f)), line.rotation + Main.rand.NextFloatRange(0.05f), line.origin, line.baseScale, line.maxWidth, line.spread);
                }
                for (float j = -3.1415927f; j <= 3.1415927f; j += 1.5707964f)
                {
                    ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, line.font, line.text, new Vector2((float)line.X, (float)line.Y) + j.ToRotationVector2().RotatedBy((double)(-(double)Main.GlobalTime * 2f), default(Vector2)) * Helper.Osc(0f, 3f, 12f, 0.5f), Color.MediumVioletRed.MultiplyAlpha(Helper.Osc(0.5f, 0.75f, 12f, 0.5f)), line.rotation + Main.rand.NextFloatRange(0.05f), line.origin, line.baseScale, line.maxWidth, line.spread);
                }
            }, null);
        }

        // Token: 0x06001D9F RID: 7583 RVA: 0x000E2DC2 File Offset: 0x000E0FC2
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
            {
                player.GetModPlayer<SummonHeartPlayer>().accBuryTheLight = true;
            }
        }
    }
}
