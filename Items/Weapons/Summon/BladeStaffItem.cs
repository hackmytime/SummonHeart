using System;
using Microsoft.Xna.Framework;
using SummonHeart.Buffs.Weapon;
using SummonHeart.Projectiles.Weapon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Items.Weapons.Summon
{
    public class BladeStaffItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blade Staff");
            Tooltip.SetDefault("Summons an Enchanted Dagger to fight for you\nIgnores a substantial amount of enemy defense\n'Don't let their small size fool you'");
            ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.mana = 10;
            item.damage = 6;
            item.useStyle = 1;
            item.shootSpeed = 10f;
            item.shoot = ModContent.ProjectileType<BladeStaffMinion>();
            item.buffType = ModContent.BuffType<BladeStaffBuff>();
            item.width = 26;
            item.height = 28;
            item.UseSound = SoundID.Item113;
            item.useAnimation = 36;
            item.useTime = 36;
            item.rare = 5;
            item.noMelee = true;
            item.knockBack = 0f;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.summon = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(item.buffType, 2, true);
            position = Main.MouseWorld;
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("GuideNote"), 1);
            recipe.AddIngredient(mod.GetItem("SummonScroll"), 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
