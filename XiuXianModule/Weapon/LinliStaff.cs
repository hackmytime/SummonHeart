using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SummonHeart.XiuXianModule.Weapon
{
    public class LinliStaff : LinliDamageItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("FireBirdStaff");
            DisplayName.AddTranslation(GameCulture.Chinese, "弄焱诀");
            Tooltip.SetDefault("FireBirdStaff");
            Tooltip.AddTranslation(GameCulture.Chinese, "" +
                "以灵力引导天地火元素凝聚火鸟" +
                "\n火鸟会追踪敌人");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 100;
            item.width = 28;
            item.height = 36;
            item.knockBack = 8.15f;
            item.rare = 2;
            item.value = Item.sellPrice(8, 15, 0, 0);
            item.autoReuse = true;
            Item.staff[item.type] = true;
            item.UseSound = SoundID.Item20;
            item.useAnimation = 20;
            item.useTime = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            //item.shoot = ModContent.ProjectileType<MagicSword>();
            item.shoot = 706;
            item.shootSpeed = 10f;
            linliCost = 1;
            level = 3;
            baseDamage = 100;
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
