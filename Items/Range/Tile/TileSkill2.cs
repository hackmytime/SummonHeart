using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using SummonHeart.Items.Skill.Tools;
using SummonHeart.Items.Range.Power;

namespace SummonHeart.Items.Range.Tile
{
    public class TileSkill2 : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("TileSkill2");
            Tooltip.SetDefault("TileSkill2");
            DisplayName.AddTranslation(GameCulture.Chinese, "材料科技Lv2");
            Tooltip.AddTranslation(GameCulture.Chinese, "可以利用炼金术提纯压缩玻璃的科技" +
                "\n左键使用炼金术提存压缩10个玻璃");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = -12;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.useAnimation = 10;
            item.useTime = 10;
            item.useStyle = 4;
            item.UseSound = SoundID.Item4;
        }


        public override bool UseItem(Player player)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            ItemCost[] costArr1 = new ItemCost[] {
                new ItemCost(ItemID.Glass, 10)
            };
           
            if (player.altFunctionUse == 2)
            {
            }
            else
            {
                if (mp.PlayerClass != 7)
                {
                    CombatText.NewText(player.getRect(), Color.Red, "你是射手吗？学了炼金术吗？还想用科技？想啥呢？");
                }
                else if (Builder.CanPayCost(costArr1, player))
                {
                    Builder.PayCost(costArr1, player);
                    mp.player.QuickSpawnItem(ModContent.ItemType<WaterGlass>(), 1);
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Glass, 50);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
