using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Weapons.Magic
{
    public class FireBirdStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("FireBirdStaff");
            DisplayName.AddTranslation(GameCulture.Chinese, "火鸟法杖");
            Tooltip.SetDefault("FireBirdStaff");
            Tooltip.AddTranslation(GameCulture.Chinese, "" +
                "发射追踪魔法火鸟" +
                "\n曾经的传奇");
        }

        public override void SetDefaults()
        {
            item.damage = 81;
            item.width = 50;
            item.height = 55;
            item.knockBack = 8.15f;
            item.rare = -12;
            item.value = Item.sellPrice(8, 15, 0, 0);
            item.autoReuse = true;
            item.magic = true;
            item.mana = 8;
            item.noMelee = true;
            Item.staff[item.type] = true;
            item.UseSound = SoundID.Item20;
            item.useAnimation = 12;
            item.useTime = 12;
            item.useStyle = ItemUseStyleID.HoldingOut;
            //item.shoot = ModContent.ProjectileType<MagicSword>();
            item.shoot = 706;
            item.shootSpeed = 10f;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(0, 8);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HellstoneBar, 815);
            recipe.AddIngredient(ItemID.Feather, 815);
            recipe.AddIngredient(ItemID.IceFeather, 8);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
