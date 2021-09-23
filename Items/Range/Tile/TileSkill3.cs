using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using SummonHeart.Items.Skill.Tools;
using SummonHeart.Items.Range.Power;
using SummonHeart.Items.Range.AmmoSkill;

namespace SummonHeart.Items.Range.Tile
{
    public class TileSkill3 : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("TileSkill3");
            Tooltip.SetDefault("TileSkill3");
            DisplayName.AddTranslation(GameCulture.Chinese, "材料科技Lv3");
            Tooltip.AddTranslation(GameCulture.Chinese, "可以利用炼金术提纯压缩岩浆的科技" +
                "\n左键使用炼金术消耗1号物品栏内次元岩浆桶内20格岩浆");
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
            Item baseItem = player.inventory[0];
            if (player.altFunctionUse == 2)
            {
            }
            else
            {
                if (mp.PlayerClass != 7)
                {
                    CombatText.NewText(player.getRect(), Color.Red, "你是射手吗？学了炼金术吗？还想用科技？想啥呢？");
                }
                else
                {
                    if(baseItem.netID == 0)
                    {
                        CombatText.NewText(player.getRect(), Color.Red, "1号栏位没有物品");
                        return true;
                    }
                    BucketGItem bg = baseItem.GetGlobalItem<BucketGItem>();
                    if(bg.liquidType == 2)
                    {
                        if(bg.liquidCount >= 20)
                        {
                            bg.liquidCount -= 20;
                            mp.player.QuickSpawnItem(ModContent.ItemType<HotUnit>(), 1);
                        }
                        else
                        {
                            CombatText.NewText(player.getRect(), Color.Red, "次元岩浆桶内岩浆不足");
                        }
                    }
                    else
                    {
                        CombatText.NewText(player.getRect(), Color.Red, "1号栏位不是次元岩浆桶");
                    }
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LavaBucket, 30);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
