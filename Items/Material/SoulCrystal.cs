using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework;
using SummonHeart.Extensions;

namespace SummonHeart.Items.Material
{
    public class SoulCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SoulCrystal");
            Tooltip.SetDefault("The son of a demon God is the soul of his own life. " +
                "He has boundless power and can be obtained when he cuts off his external incarnation\n");
            DisplayName.AddTranslation(GameCulture.Chinese, "本命神魂");
            Tooltip.AddTranslation(GameCulture.Chinese, "魔神之子本命神魂，带有高纬生命体气息，具有超脱伟力" +
                "\n泰拉世界生物不可直视，魔神之子斩身外化身时可获得" +
                "\n魔神之子吞噬众多史莱姆的灵魂力量后也可分离自身一部分灵魂合成");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = -12;
            item.value = Item.sellPrice(999, 0, 0, 0);
        }

        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Name == "ItemName" && line.mod == "Terraria")
            {
                SoulCrystal.EpicTooltipLine(line, ref yOffset);
                return false;
            }
            return true;
        }

        public static void EpicTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, line.font, line.text, new Vector2((float)line.X, (float)line.Y), Color.Lerp(Color.BlueViolet, Color.MediumVioletRed, Helper.Osc01(6f, 0f)), line.rotation, line.origin, line.baseScale, line.maxWidth, line.spread);
            Main.spriteBatch.PushBlendState(BlendState.Additive, true, delegate ()
            {
                Vector2 size = line.font.MeasureString(line.text);
                Main.spriteBatch.Draw(SummonHeartMod.Instance.GetTexture("Projectiles/Effects/LightTransparent"), new Vector2((float)line.X, (float)(line.Y - 4)) + size / 2f, null, Color.Lerp(Color.BlueViolet, Color.MediumVioletRed, Helper.Osc01(6f, 0f)).MultiplyAlpha(0.75f), 0f, Utils.Size(SummonHeartMod.Instance.GetTexture("Projectiles/Effects/LightTransparent")) / 2f, new Vector2(size.X / Helper.Osc(40f, 50f, 4f, 0f), 0.25f), 0, 0f);
                for (float i = -3.1415927f; i <= 3.1415927f; i += 1.5707964f)
                {
                    ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, line.font, line.text, new Vector2((float)line.X, (float)line.Y) + Utils.RotatedBy(Utils.ToRotationVector2(i), (double)(Main.GlobalTime * 2f), default(Vector2)) * Helper.Osc(0f, 3f, 12f, 0f), Color.SkyBlue.MultiplyAlpha(Helper.Osc(0.5f, 0.75f, 12f, 0f)), line.rotation + Main.rand.NextFloatRange(0.05f), line.origin, line.baseScale, line.maxWidth, line.spread);
                }
                for (float j = -3.1415927f; j <= 3.1415927f; j += 1.5707964f)
                {
                    ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, line.font, line.text, new Vector2((float)line.X, (float)line.Y) + Utils.RotatedBy(Utils.ToRotationVector2(j), (double)(-(double)Main.GlobalTime * 2f), default(Vector2)) * Helper.Osc(0f, 3f, 12f, 0.5f), Color.MediumVioletRed.MultiplyAlpha(Helper.Osc(0.5f, 0.75f, 12f, 0.5f)), line.rotation + Main.rand.NextFloatRange(0.05f), line.origin, line.baseScale, line.maxWidth, line.spread);
                }
            }, null);
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SlimeBanner, 250);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
