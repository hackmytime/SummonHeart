﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Extensions.TurretSystem;
using SummonHeart.Projectiles.Range;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.NPCs.Range
{
    public class TeslaTurretHead : TurretHead
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("原初炮塔");
        }

        public override void SetDefaults()
        {
            rotationOffset = new Vector2(15f, 13f);
            mountOffset = new Vector2(25f, 12f);
            angleMax = 3.1415927f;
            angleMin = -3.1415927f;
            shootDamage = 1;
            delayShoot = 30;
            rotationSpeed = 0f;
            targetRange = 400;
            npc.lifeMax = 1000;
            npc.defense = 10;
            base.SetDefaults();
        }

        protected override int PickAmmo()
        {
            return ModContent.ProjectileType<LightningBolt>();
            //return ModContent.ProjectileType<HeatLaserBeam>();
            //return ModContent.ProjectileType<Judgement>();
        }

        protected override Texture2D GetSpriteMap()
        {
            return Main.npcTexture[ModContent.NPCType<TeslaTurretHead>()];
        }

        // Token: 0x06000274 RID: 628 RVA: 0x0000E4A7 File Offset: 0x0000C6A7
        protected override int GetMaxRepairCost()
        {
            return Item.buyPrice(0, 0, 2, 50);
        }

        // Token: 0x06000275 RID: 629 RVA: 0x0000EAB0 File Offset: 0x0000CCB0
        protected override void Shoot(NPC targetNPC)
        {
            int projID = PickAmmo();
            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
                if (target.CanBeChasedBy(null, false))
                {
                    bool flag = Vector2.Distance(target.Center, GetMountOrigin()) < targetRange;
                    bool lineOfSight = Collision.CanHitLine(GetMountOrigin(), 5, 5, target.position, target.width, target.height);
                    if (flag && lineOfSight)
                    {
                        int projIndex = Projectile.NewProjectile(target.Center.X, target.Center.Y, 0f, 0f, projID, shootDamage, shootKnockback, Main.myPlayer, GetMountOrigin().X, GetMountOrigin().Y);
                        Main.projectile[projIndex].friendly = true;
                        Main.projectile[projIndex].netUpdate = true;
                    }
                }
            }
        }

        // Token: 0x06000276 RID: 630 RVA: 0x0000EB98 File Offset: 0x0000CD98
        protected override void ClientSideShootEffect(NPC targetNPC)
        {
            Main.PlaySound(SoundID.Item93, npc.position);
        }

        // Token: 0x06000277 RID: 631 RVA: 0x0000EBB0 File Offset: 0x0000CDB0
        protected override bool DetermineShoot(bool onTarget, NPC targetNPC)
        {
            if (targetNPC != null)
            {
                return base.DetermineShoot(true, targetNPC);
            }
            return base.DetermineShoot(onTarget, targetNPC);
        }
    }
}
