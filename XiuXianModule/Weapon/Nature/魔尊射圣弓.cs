using Microsoft.Xna.Framework;
using SummonHeart.XiuXianModule.Entities;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SummonHeart.XiuXianModule.Weapon.Nature
{
    public class 魔尊射圣弓 : LinliDamageItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("FireBirdStaff");
            DisplayName.AddTranslation(GameCulture.Chinese, "魔尊射圣弓");
            Tooltip.SetDefault("FireBirdStaff");
            Tooltip.AddTranslation(GameCulture.Chinese, "" +
                "以灵力引导天地火元素凝聚火鸟" +
                "\n火鸟会追踪敌人");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 100;
            item.channel = true;
            item.width = 108;
            item.height = 144;
            item.knockBack = 5f;
            item.rare = 2;
            item.value = Item.sellPrice(8, 15, 0, 0);
            item.autoReuse = true;
            item.UseSound = SoundID.Item20;
            item.useAnimation = 20;
            item.useTime = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            //item.shoot = ModContent.ProjectileType<MagicSword>();
            //item.shoot = 706;
            item.shootSpeed = 10f;
            linliCost = 1;
            level = 3;
            baseDamage = 100;
        }

        /*public override Vector2? HoldoutOrigin()
        {
            return new Vector2(0, 8);
        }
*/
        public override bool UseItem(Player player)
        {
          
            return base.UseItem(player);
        }

        public override void HoldItem(Player player)
        {
            if (Main.mouseLeft && !Main.mouseLeftRelease)
            {
                player.GetModPlayer<RPGPlayer>().FrostEffect();
            }
            else if (player.GetModPlayer<SummonHeartPlayer>().autoAttack)
            {
                player.GetModPlayer<RPGPlayer>().FrostEffect();
            }
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
