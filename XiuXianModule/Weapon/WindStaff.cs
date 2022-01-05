using Microsoft.Xna.Framework;
using SummonHeart.Projectiles.Range.Arrows;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SummonHeart.XiuXianModule.Weapon
{
    public class WindStaff : LinliDamageItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("FireBirdStaff");
            DisplayName.AddTranslation(GameCulture.Chinese, "风暴术");
            Tooltip.SetDefault("FireBirdStaff");
            Tooltip.AddTranslation(GameCulture.Chinese, "" +
                "以灵力引导天地风元素凝聚风暴之箭" +
                "\n注意安全,筑基以下境界会被风暴吹走");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 300;
            item.width = 28;
            item.height = 36;
            item.knockBack = 8.15f;
            item.rare = 5;
            item.value = Item.sellPrice(8, 15, 0, 0);
            item.autoReuse = true;
            Item.staff[item.type] = true;
            item.UseSound = SoundID.Item20;
            item.useAnimation = 300;
            item.useTime = 300;
            item.useStyle = ItemUseStyleID.HoldingOut;
            //item.shoot = ModContent.ProjectileType<MagicSword>();
            item.shoot = ModContent.ProjectileType<TornadoArrowPro>();
            item.shootSpeed = 12f;
            linliCost = 10;
            level = 4;
            baseDamage = 300;
        }

        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(0, 8);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("GuideNote"), 1);
            recipe.AddIngredient(mod.GetItem("XiuXianScroll"), 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
