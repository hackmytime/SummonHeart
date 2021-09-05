using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using SummonHeart.Items.Skill.Tools;
using SummonHeart.Items.Range.Tile;

namespace SummonHeart.Items.Range.AmmoSkill
{
    public class AmmonSkill : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("AmmonSkill");
            Tooltip.SetDefault("AmmonSkill");
            DisplayName.AddTranslation(GameCulture.Chinese, "2级科技·追踪元件制造");
            Tooltip.AddTranslation(GameCulture.Chinese, "制作追踪元件的科技" +
                "\n左键使用炼金术消耗200灵魂之力和1个液态玻璃制造一个追踪元件");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 32;
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
            ItemCost[] costArr1 = new ItemCost[] {
                new ItemCost(ModContent.ItemType<WaterGlass>(), 1)
            };

            {
                if (mp.PlayerClass != 7)
                {
                    CombatText.NewText(player.getRect(), Color.Red, "你是射手吗？学了炼金术吗？还想用科技？想啥呢？");
                }
                else if (Builder.CanPayCost(costArr1, player))
                {
                    if (mp.CheckSoul(200))
                    {
                        mp.BuySoul(200);
                        Builder.PayCost(costArr1, player);
                        mp.player.QuickSpawnItem(ModContent.ItemType<TracingUnit>(), 1);
                    }
                    else
                    {
                        CombatText.NewText(player.getRect(), Color.Red, "灵魂之力不足");
                    }
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Glass, 100);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
