using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Extensions;
using SummonHeart.Items.Accessories;
using Terraria.GameContent.Events;
using SummonHeart.Utilities;

namespace SummonHeart.Items.Material
{
    public class DemonWorldBall : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DemonWorldBall");
            Tooltip.SetDefault("DemonLure, Consume 500 soul power and transfer it to the random treasure chest\n");
            DisplayName.AddTranslation(GameCulture.Chinese, "魔神的世界球");
            Tooltip.AddTranslation(GameCulture.Chinese, "左键使用消耗500灵魂之力，改变世界背景" +
                "\n右键使用消耗5000灵魂之力，今夜落星掉落速度提高100倍");
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
                if (mp.BBP < 5000)
                {
                    player.statLife = 1;
                    CombatText.NewText(player.getRect(), Color.Red, "灵魂之力不足，强行使用生命值减为1");
                }
                else if (Main.dayTime)
                {
                    CombatText.NewText(player.getRect(), Color.LawnGreen, "白天无法使用");
                }
                else
                {
                    CombatText.NewText(player.getRect(), Color.LightPink, "-5000灵魂之力，来欣赏流星雨吧");
                    mp.BBP -= 5000;
                    SummonHeartWorld.StarMulti = 100;
                    SummonHeartWorld.StarMultiTime = 60*60*12;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        MsgUtils.SyncFallenStar();
                    }
                }
            }
            else
            {
                if (mp.BBP < 500)
                {
                    player.statLife = 1;
                    CombatText.NewText(player.getRect(), Color.Red, "灵魂之力不足，强行使用生命值减为1");
                }
                else
                {
                    CombatText.NewText(player.getRect(), Color.Red, "-500灵魂之力");
                    mp.BBP -= 500;
                    WorldGen.RandomizeBackgrounds();
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
    }
}
