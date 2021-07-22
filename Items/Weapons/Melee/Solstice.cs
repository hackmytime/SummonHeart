using System;
using Microsoft.Xna.Framework;
using SummonHeart.Extensions;
using SummonHeart.Projectiles.Melee;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Weapons.Melee
{
    // Token: 0x02000326 RID: 806
    internal class Solstice : ModItem
    {
        // Token: 0x060013C2 RID: 5058 RVA: 0x000B7A32 File Offset: 0x000B5C32
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solstice");
            DisplayName.AddTranslation(GameCulture.Russian, "Солнцестояние");
        }

        // Token: 0x060013C3 RID: 5059 RVA: 0x000B7A5C File Offset: 0x000B5C5C
        public override void SetDefaults()
        {
            item.damage = 81;
            item.useStyle = 3;
            item.useAnimation = 10;
            item.useTime = 10;
            item.shootSpeed = 1f;
            item.knockBack = 6.5f;
            item.width = item.height = 18;
            item.rare = 8;
            item.shoot = ModContent.ProjectileType<DragonLegacyRed>();
            item.value = Item.buyPrice(0, 65, 0, 0);
            item.noMelee = true;
            item.noUseGraphic = true;
            item.melee = true;
            item.autoReuse = true;
            base.item.glowMask = SplitGlowMask.Get("Solstice");
        }

        // Token: 0x060013C4 RID: 5060 RVA: 0x000B7B50 File Offset: 0x000B5D50
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = Main.MouseWorld + Main.rand.NextVector2(20f, 22f);
            NPC target;
            if ((target = Helper.GetNearestNPC(position, (npc) => npc.active && !npc.friendly && !npc.dontTakeDamage, 360f)) != null)
            {
                position = target.Center;
            }
            Vector2 vel = VectorHelper.VelocityToPoint(player.Center, position, 1f);
            float num;
            float num2;
            VectorHelper.VelocityToPoint(player.Center, position, 1f).RotatedByRandom(0.6).Deconstruct(out num, out num2);
            speedX = num;
            speedY = num2;
            position -= vel * 120f;
            return true;
        }
    }
}
