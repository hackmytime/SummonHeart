using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SummonHeart.Buffs.Weapon;
using SummonHeart.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace SummonHeart.Projectiles.Weapon
{
    public class DemonFlySwordMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DemonFlySwordMinion");
            Main.projFrames[projectile.type] = 1;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 60;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = false;
        }

        public sealed override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 10;
            projectile.height = 20;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.minionSlots = 3f;
            projectile.timeLeft = 60;
            projectile.localNPCHitCooldown = 10;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        public override bool? CanCutTiles()
        {
            return new bool?(false);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(Color.White);
        }

        private void AI_GetMyGroupIndexAndFillBlackListMod(List<int> blackListedTargets, out int index, out int totalIndexesInGroup)
        {
            index = 0;
            totalIndexesInGroup = 0;
            for (int num = 0; num < 1000; num++)
            {
                Projectile i = Main.projectile[num];
                if (i.active && i.owner == projectile.owner && i.type == projectile.type && (i.type != 759 || i.frame == Main.projFrames[i.type] - 1))
                {
                    if (projectile.whoAmI > num)
                    {
                        index++;
                    }
                    totalIndexesInGroup++;
                }
            }
        }

        public static float GetLerpValueMod(float from, float to, float t, bool clamped = false)
        {
            if (clamped)
            {
                if (from < to)
                {
                    if (t < from)
                    {
                        return 0f;
                    }
                    if (t > to)
                    {
                        return 1f;
                    }
                }
                else
                {
                    if (t < to)
                    {
                        return 1f;
                    }
                    if (t > from)
                    {
                        return 0f;
                    }
                }
            }
            return (t - from) / (to - from);
        }

        public bool CanHitWithOwnBodyMod(Entity ent)
        {
            return projectile.Distance(ent.Center) <= 1000f && (Collision.CanHit(projectile.position, projectile.width, projectile.height, ent.position, ent.width, ent.height) || Collision.CanHitLine(projectile.Center + new Vector2(projectile.direction * projectile.width / 2, (float)(-(float)projectile.height / 3)), 0, 0, ent.Center + new Vector2(0f, (float)(-(float)ent.height / 3)), 0, 0) || Collision.CanHitLine(projectile.Center + new Vector2(projectile.direction * projectile.width / 2, (float)(-(float)projectile.height / 3)), 0, 0, ent.Center, 0, 0) || Collision.CanHitLine(projectile.Center + new Vector2(projectile.direction * projectile.width / 2, 0f), 0, 0, ent.Center + new Vector2(0f, ent.height / 3), 0, 0));
        }

        public bool CanHitWithMeleeWeaponMod(Entity ent)
        {
            return projectile.Distance(ent.Center) <= 1000f && (Collision.CanHit(Main.player[projectile.owner].position, Main.player[projectile.owner].width, Main.player[projectile.owner].height, ent.position, ent.width, ent.height) || Collision.CanHitLine(Main.player[projectile.owner].Center + new Vector2(Main.player[projectile.owner].direction * Main.player[projectile.owner].width / 2, Main.player[projectile.owner].gravDir * -Main.player[projectile.owner].height / 3f), 0, 0, ent.Center + new Vector2(0f, (float)(-(float)ent.height / 3)), 0, 0) || Collision.CanHitLine(Main.player[projectile.owner].Center + new Vector2(Main.player[projectile.owner].direction * Main.player[projectile.owner].width / 2, Main.player[projectile.owner].gravDir * -Main.player[projectile.owner].height / 3f), 0, 0, ent.Center, 0, 0) || Collision.CanHitLine(Main.player[projectile.owner].Center + new Vector2(Main.player[projectile.owner].direction * Main.player[projectile.owner].width / 2, 0f), 0, 0, ent.Center + new Vector2(0f, ent.height / 3), 0, 0));
        }

        private bool IsInRangeOfMeOrMyOwnerMod(Entity entity, float maxDistance, out float myDistance, out float playerDistance, out bool closerIsMe)
        {
            myDistance = Vector2.Distance(entity.Center, projectile.Center);
            if (myDistance < maxDistance && !CanHitWithOwnBodyMod(entity))
            {
                myDistance = float.PositiveInfinity;
            }
            playerDistance = Vector2.Distance(entity.Center, Main.player[projectile.owner].Center);
            if (playerDistance < maxDistance && !CanHitWithMeleeWeaponMod(entity))
            {
                playerDistance = float.PositiveInfinity;
            }
            closerIsMe = myDistance < playerDistance;
            if (closerIsMe)
            {
                return myDistance <= maxDistance;
            }
            return playerDistance <= maxDistance;
        }

        private void Minion_FindTargetInRangeMod(int startAttackRange, ref int attackTarget, bool skipIfCannotHitWithOwnBody, Func<Entity, int, bool> customEliminationCheck = null)
        {
            float zero2 = startAttackRange;
            float i = zero2;
            float damage = zero2;
            NPC x = projectile.OwnerMinionAttackTargetNPC;
            float num8;
            float num9;
            bool flag4;
            if (x != null && x.CanBeChasedBy(projectile, false) && IsInRangeOfMeOrMyOwnerMod(x, zero2, out num8, out num9, out flag4))
            {
                attackTarget = x.whoAmI;
                return;
            }
            if (attackTarget >= 0)
            {
                return;
            }
            for (int flag3 = 0; flag3 < 200; flag3++)
            {
                NPC num5 = Main.npc[flag3];
                float num6;
                float num7;
                bool value;
                if (num5.CanBeChasedBy(projectile, false) && IsInRangeOfMeOrMyOwnerMod(num5, zero2, out num6, out num7, out value) && (!skipIfCannotHitWithOwnBody || CanHitWithOwnBodyMod(num5)) && (customEliminationCheck == null || customEliminationCheck(num5, attackTarget)))
                {
                    attackTarget = flag3;
                    float num10 = value ? num6 : num7;
                    if (i > num6)
                    {
                        i = num6;
                    }
                    if (damage > num7)
                    {
                        damage = num7;
                    }
                    zero2 = Math.Max(i, damage);
                }
            }
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            Vector2 vector = player.Top + new Vector2(-10f, -20f);
            int BladeMinionBuff = ModContent.BuffType<BladeStaffBuff>();
            if (player.HasBuff(BladeMinionBuff))
            {
                projectile.timeLeft = 2;
            }
            if (projectile.ai[0] == 0f)
            {
                int index;
                int totalIndexesInGroup;
                AI_GetMyGroupIndexAndFillBlackListMod(null, out index, out totalIndexesInGroup);
                float num2 = 6.2831855f / totalIndexesInGroup;
                float num3 = totalIndexesInGroup * 0.66f;
                Vector2 value = new Vector2(30f, 6f) / 5f * (totalIndexesInGroup - 1);
                Vector2 value2 = Utils.RotatedBy(Vector2.UnitY, num2 * index + Main.GlobalTime % num3 / num3 * 6.2831855f, default(Vector2));
                vector += value2 * value;
                vector.Y += player.gfxOffY;
                vector = Utils.Floor(vector);
            }
            if (projectile.ai[0] == 0f)
            {
                Vector2 velocity = vector - projectile.Center;
                float num4 = 10f;
                float lerpValue = GetLerpValueMod(200f, 600f, velocity.Length(), true);
                num4 += lerpValue * 30f;
                if (velocity.Length() >= 3000f)
                {
                    projectile.Center = vector;
                }
                projectile.velocity = velocity;
                if (projectile.velocity.Length() > num4)
                {
                    projectile.velocity *= num4 / projectile.velocity.Length();
                }
                int startAttackRange = 800;
                int attackTarget = -1;
                Minion_FindTargetInRangeMod(startAttackRange, ref attackTarget, false, null);
                if (attackTarget != -1)
                {
                    projectile.ai[0] = 60f;
                    projectile.ai[1] = attackTarget;
                    projectile.netUpdate = true;
                }
                float targetAngle = Utils.ToRotation(Utils.SafeNormalize(projectile.velocity, Vector2.UnitY)) + 1.5707964f;
                if (velocity.Length() < 40f)
                {
                    targetAngle = Utils.ToRotation(Vector2.UnitY) + 1.5707964f;
                }
                projectile.rotation = Utils.AngleLerp(projectile.rotation, targetAngle, 0.2f);
                return;
            }
            if (projectile.ai[0] == -1f)
            {
                if (projectile.ai[1] == 0f)
                {
                    Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 1, 1f, 0f);
                    for (int i = 0; i < 2; i++)
                    {
                        Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 15, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f, 0, default, 1.4f);
                        if (Main.rand.Next(3) != 0)
                        {
                            dust.scale *= 1.3f;
                            dust.velocity *= 1.1f;
                        }
                        dust.noGravity = true;
                        dust.fadeIn = 0f;
                    }
                    projectile.velocity += Utils.NextVector2CircularEdge(Main.rand, 4f, 4f);
                }
                projectile.ai[1] += 1f;
                projectile.rotation += projectile.velocity.X * 0.1f + projectile.velocity.Y * 0.05f;
                projectile.velocity *= 0.92f;
                if (projectile.ai[1] >= 9f)
                {
                    projectile.ai[0] = 0f;
                    projectile.ai[1] = 0f;
                }
                return;
            }
            NPC nPC = null;
            int num5 = (int)projectile.ai[1];
            if (Utils.IndexInRange<NPC>(Main.npc, num5) && Main.npc[num5].CanBeChasedBy(projectile, false))
            {
                nPC = Main.npc[num5];
            }
            if (nPC == null)
            {
                projectile.ai[0] = -1f;
                projectile.ai[1] = 0f;
                projectile.netUpdate = true;
            }
            else if (player.Distance(nPC.Center) >= 900f)
            {
                projectile.ai[0] = 0f;
                projectile.ai[1] = 0f;
                projectile.netUpdate = true;
            }
            else
            {
                Vector2 velocity2 = nPC.Center - projectile.Center;
                float num6 = 16f;
                projectile.velocity = velocity2;
                if (projectile.velocity.Length() > num6)
                {
                    projectile.velocity *= num6 / projectile.velocity.Length();
                }
                float targetAngle2 = Utils.ToRotation(Utils.SafeNormalize(projectile.velocity, Vector2.UnitY)) + 1.5707964f;
                projectile.rotation = Utils.AngleLerp(projectile.rotation, targetAngle2, 0.4f);
            }
            float num7 = 0.1f;
            float num8 = projectile.width * 5;
            for (int j = 0; j < 1000; j++)
            {
                if (j != projectile.whoAmI && Main.projectile[j].active && Main.projectile[j].owner == projectile.owner && Main.projectile[j].type == projectile.type && Math.Abs(projectile.position.X - Main.projectile[j].position.X) + Math.Abs(projectile.position.Y - Main.projectile[j].position.Y) < num8)
                {
                    if (projectile.position.X < Main.projectile[j].position.X)
                    {
                        Projectile projectile = base.projectile;
                        projectile.velocity.X = projectile.velocity.X - num7;
                    }
                    else
                    {
                        Projectile projectile2 = projectile;
                        projectile2.velocity.X = projectile2.velocity.X + num7;
                    }
                    if (projectile.position.Y < Main.projectile[j].position.Y)
                    {
                        Projectile projectile3 = projectile;
                        projectile3.velocity.Y = projectile3.velocity.Y - num7;
                    }
                    else
                    {
                        Projectile projectile4 = projectile;
                        projectile4.velocity.Y = projectile4.velocity.Y + num7;
                    }
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            oldArmorPenetration = Main.player[projectile.owner].armorPenetration;
            Main.player[projectile.owner].armorPenetration += 25;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.player[projectile.owner].armorPenetration = oldArmorPenetration;
            oldArmorPenetration = 0;
            target.immune[projectile.owner] = 0;
            if (projectile.ai[0] > 0f)
            {
                projectile.ai[0] = -1f;
                projectile.ai[1] = 0f;
                projectile.netUpdate = true;
            }
        }

        private static Conditions.IsSolid _cachedConditions_solid = new Conditions.IsSolid();

        private static NotNull _cachedConditions_notNull = new NotNull();

        private int oldArmorPenetration;
    }
}
