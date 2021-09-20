using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Accessories
{
    public class FallenStarMagnet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("FallenStarMagnet");
            Tooltip.SetDefault("RFallenStarMagnet");
            DisplayName.AddTranslation(GameCulture.Chinese, "落星磁铁");
            Tooltip.AddTranslation(GameCulture.Chinese, "吸收全世界的落星");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.sellPrice(9, 0, 0, 0);
            item.rare = -12;
            item.holdStyle = 2;
            item.accessory = true;
        }

        public override void HoldItem(Player player)
        {
            for (int i = 0; i < 400; i++)
            {
                Item item = Main.item[i];
                if (item.type != ItemID.FallenStar)
                    continue;
                if (item.active && item.noGrabDelay == 0 && ItemLoader.CanPickup(item, player))
                {
                    item.beingGrabbed = true;
                    Vector2 vector = player.Center - item.Center;
                    item.velocity = (item.velocity * 4f + vector * (20f / vector.Length())) * 0.2f;
                }
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            for (int i = 0; i < 400; i++)
            {
                Item item = Main.item[i];
                if (item.type != ItemID.FallenStar)
                    continue;
                if (item.active && item.noGrabDelay == 0 && ItemLoader.CanPickup(item, player))
                {
                    item.beingGrabbed = true;
                    Vector2 vector = player.Center - item.Center;
                    item.velocity = (item.velocity * 4f + vector * (20f / vector.Length())) * 0.2f;
                }
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("MetalUnit"), 99);
            recipe.AddIngredient(ItemID.FallenStar, 9);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
