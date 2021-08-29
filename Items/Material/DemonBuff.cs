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
using System.Collections.Generic;

namespace SummonHeart.Items.Material
{
    public class DemonBuff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DemonTime");
            Tooltip.SetDefault("DemonLure, Consume 500 soul power and transfer it to the random treasure chest\n");
            DisplayName.AddTranslation(GameCulture.Chinese, "魔神的无限法则");
            Tooltip.AddTranslation(GameCulture.Chinese, "左键使用消耗当前灵魂法则上限x2000点灵魂之力，提高无限法则上限1" +
                "\n右键使用删除当前所有未启用的buff，每删除1个buff需要消耗2000点灵魂之力");
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
                //右键删除buff
                List<int> delList = new List<int>();
                foreach(var key in mp.infiniBuffDic.Keys)
                {
                    if (!mp.infiniBuffDic[key])
                        delList.Add(key);
                }
                int costSoul = 2000 * delList.Count;
                if (costSoul == 0)
                {
                    CombatText.NewText(player.getRect(), Color.Red, "没有要删除的buff");
                }
                else if (mp.BBP < costSoul)
                {
                    CombatText.NewText(player.getRect(), Color.Red, "灵魂之力不足");
                }
                else
                {
                    CombatText.NewText(player.getRect(), Color.LightGreen, $"-{costSoul}灵魂之力删除了{delList.Count}个buff");
                    mp.BBP -= costSoul;
                    foreach(var key in delList)
                    {
                        mp.infiniBuffDic.Remove(key);
                        player.ClearBuff(key);
                    }
                }
            }
            else
            {
                int costSoul = 2000 * mp.buffMaxCount;
                if (mp.buffMaxCount >= 100)
                {
                    CombatText.NewText(player.getRect(), Color.LightGreen, "灵魂法则已达上限100");
                }
                else if (mp.BBP < costSoul)
                {
                    player.statLife = 1;
                    CombatText.NewText(player.getRect(), Color.Red, $"灵魂之力不足{costSoul}点，强行使用生命值减为1");
                }
                else
                {
                    CombatText.NewText(player.getRect(), Color.Red, $"-{costSoul}灵魂之力");
                    mp.BBP -= costSoul;
                    mp.buffMaxCount++;
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
