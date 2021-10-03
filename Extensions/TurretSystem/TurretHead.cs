using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Extensions.TurretSystem
{
    public abstract class TurretHead : ModNPC
    {
        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public int Direction
        {
            get
            {
                return (int)npc.ai[0];
            }
            set
            {
                npc.ai[0] = value;
            }
        }

        public float Rotation
        {
            get
            {
                return npc.ai[1];
            }
            set
            {
                npc.ai[1] = value;
            }
        }

        public override void SetDefaults()
        {
            npc.width = 48;
            npc.height = 48;
            npc.friendly = true;
            npc.damage = 0;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.knockBackResist = 0f;
            npc.netAlways = true;
            isAlive = true;
        }

        protected abstract int PickAmmo();

        public override void AI()
        {
            npc.velocity = Vector2.Zero;
            if (isAlive)
            {
                NPC target = SelectTarget();
                bool onTarget = false;
                if (target != null)
                {
                    Vector2 targetPos = target.Center;
                    onTarget = RotateTowards((float)Math.Atan2(targetPos.Y - GetMountOrigin().Y, targetPos.X - GetMountOrigin().X));
                }
                else
                {
                    RotateTowards(ShiftAngle(GetSpawnAngle()));
                }
                if (DetermineShoot(onTarget, target))
                {
                    ClientSideShootEffect(target);
                    if (Main.netMode == 0 || Main.netMode == 2)
                    {
                        ShootEffect(target);
                        Shoot(target);
                    }
                }
            }
        }

        public virtual string getHoverTooltip()
        {
            if (isAlive)
            {
                return string.Concat(new object[]
                {
                    npc.GivenOrTypeName,
                    " ",
                    npc.life,
                    "/",
                    npc.lifeMax,
                    "\n伤害: ",
                    shootDamage,
                    "\n攻速: ",
                    (60.0 / delayShoot).ToString("0.##"),
                    " 每秒\n攻击范围: ",
                    targetRange / 16,
                    " 格\n击退力度: ",
                    shootKnockback.ToString("0.##"),
                    "\n瞄准速度: ",
                    (3437.74 * rotationSpeed).ToString("0.##"),
                    "° 每秒\n防御: ",
                    npc.defense,
                    "\n"
                });
            }
            return "该炮塔已损坏";
        }

        public override bool CheckActive()
        {
            return false;
        }

        protected abstract Texture2D GetSpriteMap();

        protected virtual Vector2 GetMountOrigin()
        {
            return npc.position + GetRelativeMountOffset();
        }

        protected virtual Vector2 GetShootOrigin()
        {
            return GetMountOrigin() + new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * ShootOriginOffset;
        }

        protected virtual Vector2 GetRelativeMountOffset()
        {
            if (Direction == 0)
            {
                return new Vector2(npc.width - mountOffset.X, mountOffset.Y);
            }
            return mountOffset;
        }

        protected virtual bool DetermineShoot(bool onTarget, NPC targetNPC)
        {
            if (onTarget)
            {
                ShootCounter++;
                if (ShootCounter > delayShoot)
                {
                    ShootCounter = 0;
                    return true;
                }
            }
            return false;
        }

        protected virtual void ClientSideShootEffect(NPC targetNPC)
        {
        }

        protected virtual void ShootEffect(NPC targetNPC)
        {
        }

        protected virtual void Shoot(NPC targetNPC)
        {
            int projID = PickAmmo();
            int projIndex = Projectile.NewProjectile(GetShootOrigin().X, GetShootOrigin().Y, shootVelocity * (float)Math.Cos(Rotation), shootVelocity * (float)Math.Sin(Rotation), projID, shootDamage, shootKnockback, Main.myPlayer, 0f, 0f);
            Main.projectile[projIndex].friendly = true;
            Main.projectile[projIndex].hostile = false;
        }

        protected abstract int GetMaxRepairCost();

        public int GetRepairCost()
        {
            int cost = GetMaxRepairCost();
            if (!isAlive)
            {
                return cost;
            }
            return (int)(cost * (1f - npc.life / (float)npc.lifeMax) / 2f);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (isAlive)
            {
                Texture2D spriteMap = GetSpriteMap();
                SpriteEffects effect = GetEffect();
                Vector2 modifiedDrawOffset = GetDrawOffset();
                spriteBatch.Draw(spriteMap, npc.position - Main.screenPosition + GetRelativeMountOffset(), null, drawColor, Rotation, modifiedDrawOffset, npc.scale, effect, 0f);
                if (Main.player[Main.myPlayer].GetModPlayer<TurretPlayer>().drawTurretAngles)
                {
                    spriteBatch.Draw(Main.magicPixel, new Rectangle((int)(GetMountOrigin().X - Main.screenPosition.X), (int)(GetMountOrigin().Y - Main.screenPosition.Y), 75, 2), null, Color.Red, ShiftAngle(angleMax), new Vector2(0f, 0f), 0, 0f);
                    spriteBatch.Draw(Main.magicPixel, new Rectangle((int)(GetMountOrigin().X - Main.screenPosition.X), (int)(GetMountOrigin().Y - Main.screenPosition.Y), 75, 2), null, Color.Red, ShiftAngle(angleMin), new Vector2(0f, 0f), 0, 0f);
                }
            }
            return false;
        }

        public override void DrawEffects(ref Color drawColor)
        {
            if (!isAlive)
            {
                if (Main.rand.Next(30) == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6, 0f, 0f, 100, default, 1f);
                        Main.dust[dust].noLight = true;
                        Main.dust[dust].velocity *= 0.5f;
                    }
                    Vector2 vel = Vector2.Zero;
                    int type = Main.rand.Next(825, 828);
                    if (Main.windSpeed < 0f)
                    {
                        vel.X = -Main.windSpeed;
                    }
                    Gore.NewGore(npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)), vel, type, Main.rand.NextFloat() * 0.3f + 0.3f);
                    return;
                }
            }
            else if (npc.life < npc.lifeMax / 3 && Main.rand.Next(80) == 0)
            {
                Vector2 vel2 = Vector2.Zero;
                int type2 = Main.rand.Next(825, 828);
                if (Main.windSpeed < 0f)
                {
                    vel2.X = -Main.windSpeed;
                }
                Gore.NewGore(npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)), vel2, type2, Main.rand.NextFloat() * 0.3f + 0.3f);
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            if (isAlive)
            {
                return base.DrawHealthBar(hbPosition, ref scale, ref position);
            }
            return new bool?(false);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            Main.PlaySound(SoundID.NPCHit53, npc.Center);
            for (int d = 0; d < 5; d++)
            {
                int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6, 0f, 0f, 80, default, 1.25f);
                Main.dust[dust].noLight = true;
                Main.dust[dust].velocity *= 1f;
            }
        }

        public override bool CheckDead()
        {
            if (npc.life <= 0)
            {
                for (int i = 0; i < 60; i++)
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6, 0f, 0f, 100, default, 1f);
                    Main.dust[dust].noLight = true;
                    Main.dust[dust].velocity *= 0.5f;
                }
                npc.life = npc.lifeMax;
                Deactivate();
            }
            return false;
        }

        public void Deactivate()
        {
            npc.dontTakeDamage = true;
            npc.dontTakeDamageFromHostiles = true;
            isAlive = false;
        }

        public void Reactivate()
        {
            npc.dontTakeDamageFromHostiles = false;
            npc.dontTakeDamage = false;
            npc.life = npc.lifeMax;
            isAlive = true;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (isAlive)
            {
                return base.CanBeHitByProjectile(projectile);
            }
            return new bool?(false);
        }

        protected virtual SpriteEffects GetEffect()
        {
            if (Rotation > 1.5707963267948966 || Rotation < -1.5707963267948966)
            {
                return (SpriteEffects)2;
            }
            return 0;
        }

        protected virtual Vector2 GetDrawOffset()
        {
            if (Rotation > 1.5707963267948966 || Rotation < -1.5707963267948966)
            {
                return new Vector2(rotationOffset.X, GetSpriteMap().Height - rotationOffset.Y);
            }
            return rotationOffset;
        }

        protected virtual bool IsValidAngle(float angle)
        {
            angle = ShiftAngle(angle);
            return angle < angleMax && angle > angleMin;
        }

        protected float ShiftAngle(float angle)
        {
            if (Direction != 0 && Direction == 1)
            {
                if (angle < 0f)
                {
                    angle = -3.1415927f - angle;
                }
                else
                {
                    angle = 3.1415927f - angle;
                }
            }
            return angle;
        }

        protected virtual bool RotateTowards(float targetAngle)
        {
            float noRotDotProductResult = (float)(Math.Cos(Rotation) * Math.Cos(targetAngle) + Math.Sin(Rotation) * Math.Sin(targetAngle));
            float clockwiseDotProductResult = (float)(Math.Cos(Rotation + rotationSpeed) * Math.Cos(targetAngle) + Math.Sin(Rotation + rotationSpeed) * Math.Sin(targetAngle));
            float counterClockwiseDotProductResult = (float)(Math.Cos(Rotation - rotationSpeed) * Math.Cos(targetAngle) + Math.Sin(Rotation - rotationSpeed) * Math.Sin(targetAngle));
            if (noRotDotProductResult > clockwiseDotProductResult && noRotDotProductResult > counterClockwiseDotProductResult)
            {
                Rotation = targetAngle;
                Rotation = (float)(Rotation % 6.283185307179586);
                return true;
            }
            if (clockwiseDotProductResult > counterClockwiseDotProductResult)
            {
                Rotation += rotationSpeed;
            }
            else
            {
                Rotation -= rotationSpeed;
            }
            Rotation = (float)(Rotation % 6.283185307179586);
            return false;
        }

        protected virtual NPC SelectTarget()
        {
            NPC foundTarget = null;
            float angleDiff = 999999f;
            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
                if (target.CanBeChasedBy(this, false) && Vector2.Distance(target.Center, GetMountOrigin()) < targetRange && Collision.CanHitLine(GetMountOrigin(), 1, 1, target.position, target.width, target.height))
                {
                    Vector2 distance = target.Center - GetMountOrigin();
                    float angle = (float)Math.Atan2(distance.Y, distance.X);
                    if (IsValidAngle(angle) && angleDiff > Math.Abs(angle - Rotation))
                    {
                        angleDiff = Math.Abs(angle - Rotation);
                        foundTarget = target;
                    }
                }
            }
            return foundTarget;
        }

        public void InitRotation()
        {
            Rotation = GetSpawnAngle();
            if (Direction == 1)
            {
                Rotation = (float)(3.141592653589793 - Rotation);
            }
        }

        protected virtual float GetSpawnAngle()
        {
            return 0f;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(isAlive);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            isAlive = reader.ReadBoolean();
        }

        // Token: 0x04000027 RID: 39
        protected int delayShoot;

        // Token: 0x04000028 RID: 40
        protected int ShootCounter;

        // Token: 0x04000029 RID: 41
        protected float angleMax;

        // Token: 0x0400002A RID: 42
        protected float angleMin;

        // Token: 0x0400002B RID: 43
        protected int targetRange = 720;

        // Token: 0x0400002C RID: 44
        protected float shootVelocity = 14f;

        // Token: 0x0400002D RID: 45
        protected int shootDamage;

        // Token: 0x0400002E RID: 46
        protected float shootKnockback;

        // Token: 0x0400002F RID: 47
        protected float rotationSpeed = 0.04f;

        // Token: 0x04000030 RID: 48
        public const int FACE_RIGHT = 0;

        // Token: 0x04000031 RID: 49
        public const int FACE_LEFT = 1;

        // Token: 0x04000032 RID: 50
        public bool isAlive;

        // Token: 0x04000033 RID: 51
        public float ShootOriginOffset = 5f;

        // Token: 0x04000034 RID: 52
        protected Vector2 rotationOffset;

        // Token: 0x04000035 RID: 53
        protected Vector2 mountOffset;
    }
}
