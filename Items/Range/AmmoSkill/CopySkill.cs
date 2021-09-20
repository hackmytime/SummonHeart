using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using SummonHeart.Items.Skill.Tools;
using SummonHeart.Items.Range.Tile;

namespace SummonHeart.Items.Range.AmmoSkill
{
    public class CopySkill : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("CopySkill");
            Tooltip.SetDefault("CopySkill");
            DisplayName.AddTranslation(GameCulture.Chinese, "辅助科技·射手武器复制科技");
            Tooltip.AddTranslation(GameCulture.Chinese, "消耗所持武器稀有度x30的金属元件+3000点灵魂之力，" +
                "\n复制1号物品栏的射手武器5把");
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
            Item heldItem = player.inventory[0];

            int costCount = 30;
            if(heldItem.rare > 0)
            {
                costCount *= heldItem.rare;
            }
            if (heldItem.rare == -12)
            {
                costCount = 300;
            }
            ItemCost[] costArr1 = new ItemCost[] {
                new ItemCost(ModContent.ItemType<MetalUnit>(), costCount)
            };

            {
                if (mp.PlayerClass != 7)
                {
                    CombatText.NewText(player.getRect(), Color.Red, "你是射手吗？学了炼金术吗？还想用科技？想啥呢？");
                }
                else if (!heldItem.ranged)
                {
                    CombatText.NewText(player.getRect(), Color.Red, "当前武器非射手武器无法复制");
                }
                else if (Builder.CanPayCost(costArr1, player))
                {
                    if (mp.CheckSoul(3000))
                    {
                        mp.BuySoul(3000);
                        Builder.PayCost(costArr1, player);
                        mp.player.QuickSpawnItem(heldItem, 1);
                        mp.player.QuickSpawnItem(heldItem, 1);
                        mp.player.QuickSpawnItem(heldItem, 1);
                        mp.player.QuickSpawnItem(heldItem, 1);
                        mp.player.QuickSpawnItem(heldItem, 1);
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
            recipe.AddIngredient(ModContent.ItemType<MetalUnit>(), 30);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
