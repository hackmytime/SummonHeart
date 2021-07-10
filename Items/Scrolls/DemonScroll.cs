using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SummonHeart.Items.Scrolls
{
    public class DemonScroll : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DemonScroll");
            Tooltip.SetDefault("Son of demon God, please choose the right way for you to inherit." +
                "\nThere are five ways of war, law, skill, spirit and killing");
            DisplayName.AddTranslation(GameCulture.Chinese, "魔神传承·抉择");
            Tooltip.AddTranslation(GameCulture.Chinese, "魔神之子，请选择你的道。" +
                "\n魔神传承共有战、法、技、灵、杀5条道");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = 5;
            item.useAnimation = 20;
            item.useTime = 20;
            item.useStyle = 4;
            item.UseSound = SoundID.Item4;
            item.noUseGraphic = true;
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
