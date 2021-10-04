using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using SummonHeart.Items.Range.Turret;
using SummonHeart.Items.Range.Armor;
using SummonHeart.Items.Range.AmmoSkill;

namespace SummonHeart.Items.Helpful
{
    public class HelpBag : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("HelpBag");
            Tooltip.SetDefault("HelpBag");
            DisplayName.AddTranslation(GameCulture.Chinese, "国庆大礼包");
            Tooltip.AddTranslation(GameCulture.Chinese, "国庆节礼物，右键打开");
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
            item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            player.QuickSpawnItem(ModContent.ItemType<MetalUnit>(), 101);
            player.QuickSpawnItem(ItemID.MasterBait, 10);
            player.QuickSpawnItem(ModContent.ItemType<LightTurretItem1>(), 1);
            player.QuickSpawnItem(ModContent.ItemType<PowerArmor3>(), 1);
        }
    }
}
