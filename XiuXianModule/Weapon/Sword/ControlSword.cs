using Microsoft.Xna.Framework;
using SummonHeart.XiuXianModule.Entities;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SummonHeart.XiuXianModule.Weapon.Sword
{
    public class ControlSword : LinliDamageItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("FireBirdStaff");
            DisplayName.AddTranslation(GameCulture.Chinese, "御剑术");
            Tooltip.SetDefault("FireBirdStaff");
            Tooltip.AddTranslation(GameCulture.Chinese, "" +
                "以灵力凝聚灵力飞剑之阵" +
                "剑阵最多凝聚108把飞剑" +
                "\n道友看我御剑术");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 300;
            item.channel = true;
            item.width = 38;
            item.height = 38;
            item.knockBack = 5f;
            item.rare = 2;
            item.value = Item.sellPrice(8, 15, 0, 0);
            item.autoReuse = true;
            item.UseSound = SoundID.Item20;
            item.useAnimation = 6;
            item.useTime = 6;
            item.useStyle = ItemUseStyleID.HoldingOut;
            //item.shoot = ModContent.ProjectileType<MagicSword>();
            //item.shoot = 706;
            item.shootSpeed = 10f;
            linliCost = 1;
            level = 4;
            baseDamage = 300;
        }

        /*public override Vector2? HoldoutOrigin()
        {
            return new Vector2(0, 8);
        }
*/
        public override bool UseItem(Player player)
        {
            player.GetModPlayer<RPGPlayer>().onIceAttack = true;
            return base.UseItem(player);
        }

       

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return false;
        }

         public override void HoldItem(Player player)
         {
            var rp = player.GetModPlayer<RPGPlayer>();
            if (player.dead || !player.active || !player.channel || rp.lingli < linliCost)
             {
                 player.GetModPlayer<RPGPlayer>().onIceAttack = false;
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
