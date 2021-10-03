using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Extensions;
using SummonHeart.Items.Accessories;
using SummonHeart.Items.Range.Ammo.Bullet;

namespace SummonHeart.Items.Material
{
    public class DemonBullte : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DemonBullte");
            Tooltip.SetDefault("DemonBullte");
            DisplayName.AddTranslation(GameCulture.Chinese, "魔神的灵魂碎片·子弹兑换");
            Tooltip.AddTranslation(GameCulture.Chinese, "左键献祭十万灵魂之力获得9999发3级子弹" +
                "\n右键献祭百万灵魂之力获得999发4级劣质狙击弹和999发究极狙击弹");
        }

        public override void SetDefaults()
        {
            item.width = 64;
            item.height = 64;
            item.rare = -12;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.useAnimation = 20;
            item.useTime = 20;
            item.useStyle = 4;
            item.UseSound = SoundID.Item4;
        }

        public override bool UseItem(Player player)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            if (player.altFunctionUse == 2)
            {
                if (mp.BBP < 1000000)
                {
                    player.statLife = 1;
                    CombatText.NewText(player.getRect(), Color.Red, "灵魂之力不足，强行使用生命值减为1");
                }
                else
                {
                    CombatText.NewText(player.getRect(), Color.Red, "-100W灵魂之力");
                    mp.BBP -= 1000000;
                    player.QuickSpawnItem(ModContent.ItemType<MaxBullet>(), 999);
                    player.QuickSpawnItem(ModContent.ItemType<LowBullet>(), 999);
                }
            }
            else
            {
                if (mp.BBP < 100000)
                {
                    player.statLife = 1;
                    CombatText.NewText(player.getRect(), Color.Red, "灵魂之力不足，强行使用生命值减为1");
                }
                else
                {
                    CombatText.NewText(player.getRect(), Color.Red, "-10W灵魂之力");
                    mp.BBP -= 100000;
                    player.QuickSpawnItem(ModContent.ItemType<VTracingBullet>(), 9999);
                }
            }
            return true;
        }

        public override bool AltFunctionUse(Player player)
        {
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

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
