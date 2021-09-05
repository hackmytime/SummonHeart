using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using SummonHeart.Items.Skill.Tools;
using SummonHeart.Items.Range.Power;

namespace SummonHeart.Items.Range.Tile
{
    public class TileSkill1 : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("TileSkill1");
            Tooltip.SetDefault("TileSkill1");
            DisplayName.AddTranslation(GameCulture.Chinese, "材料科技Lv1");
            Tooltip.AddTranslation(GameCulture.Chinese, "可以利用炼金术提纯压缩材料的科技" +
                "\n左键使用炼金术压缩99个土块或者99个石块");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = -12;
            item.value = Item.sellPrice(9999, 0, 0, 0);
            item.useAnimation = 20;
            item.useTime = 20;
            item.useStyle = 4;
            item.UseSound = SoundID.Item4;
        }


        public override bool AltFunctionUse(Player player)
        {
            SkillBase skillBase = item.GetGlobalItem<SkillBase>();
            if (skillBase.skillUseCount >= skillBase.levelUpCount)
            {
                return true;
            }
            return base.AltFunctionUse(player);
        }

        public override bool UseItem(Player player)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            ItemCost[] costArr1 = new ItemCost[] {
                new ItemCost(ItemID.DirtBlock, 99)
            };
            ItemCost[] costArr2 = new ItemCost[] {
                new ItemCost(ItemID.StoneBlock, 99)
            };
            if (player.altFunctionUse == 2)
            {
                //处理升级
                item.TurnToAir();
                mp.player.QuickSpawnItem(ModContent.ItemType<PowerSkill2>(), 1);
                CombatText.NewText(player.getRect(), Color.LightGreen, "核心科技升级成功");
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
                    mp.player.QuickSpawnItem(ModContent.ItemType<WaterDirt>(), 1);
                }
                else if (Builder.CanPayCost(costArr2, player))
                {
                    Builder.PayCost(costArr2, player);
                    mp.player.QuickSpawnItem(ModContent.ItemType<WaterStone>(), 1);
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DirtBlock, 100);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
