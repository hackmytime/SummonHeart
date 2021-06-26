using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

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
            DisplayName.AddTranslation(GameCulture.Chinese, "女神之像【已无效】");
            Tooltip.AddTranslation(GameCulture.Chinese, "使用以切换女神模式（作弊模式),获得女神的祝福,无法取消"
            + "\n女神的祝福：10倍灵魂获取\n魔神之子被女神给与的力量所诱惑");
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
            /*if (!SummonHeartWorld.GoddessMode)
            {
                if (Main.netMode == 0 || Main.netMode == 1)
                {
                    Main.NewText("魔神之子被女神给与的力量所诱惑，获得了女神的祝福", 255, 255, 255);
                }
                SummonHeartWorld.GoddessMode = true;
                return true;
            }*/
           /* if (SummonHeartWorld.GoddessMode)
            {
                if (Main.netMode == 0 || Main.netMode == 1)
                {
                    Main.NewText(Language.GetTextValue("Mods.AlchemistNPC.Common.AntiBuffmodeisdisabled"), 255, 255, 255);
                }
                SummonHeartWorld.GoddessMode = false;
                return true;
            }*/
            return base.UseItem(player);
        }

        /*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }*/
    }
}
