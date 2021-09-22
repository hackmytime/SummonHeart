using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SummonHeart.Extensions;

namespace SummonHeart.Items.Range.Loot
{
    public class Loot6 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Loot6");
            Tooltip.SetDefault("Loot6");
            DisplayName.AddTranslation(GameCulture.Chinese, "王级材料·6级生物材料");
            Tooltip.AddTranslation(GameCulture.Chinese, "用炼金术炼化敌人身躯形成的固态精华" +
                "\n吞噬+100000灵魂之力" +
                "\n用炼金术提纯压缩到极致的完美生物之精华，王级血肉精华");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = -12;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.maxStack = 1;
            item.useTime = 20;
            item.useStyle = 2;
            item.UseSound = SoundID.Item4;
            item.consumable = true;
        }

        public override bool UseItem(Player player)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            if (mp.BBP >= 5000000)
            {
                player.statLife = 1;
                CombatText.NewText(player.getRect(), Color.Red, "灵魂之力已满，无法吸收");
            }
            else
            {
                int addBBP = 100000;
                CombatText.NewText(player.getRect(), Color.LightGreen, $"+{addBBP}灵魂之力");
                mp.BBP += addBBP;
                if (mp.BBP > 5000000)
                    mp.BBP = 5000000;
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("Loot5"), 10);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Name == "ItemName" && line.mod == "Terraria")
            {
                Loot6.EpicTooltipLine(line, ref yOffset);
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
    }
}
