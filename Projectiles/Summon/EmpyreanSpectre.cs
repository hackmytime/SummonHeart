using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;
using static System.Math;
using SummonHeart.Utilities;
using SummonHeart.Projectiles.Melee;
using SummonHeart.Extensions;
using System;
using SummonHeart.Items.Weapons.Sabres;

namespace SummonHeart.Projectiles.Summon
{
    public class EmpyreanSpectre : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Empyrean Spectre");
            DisplayName.AddTranslation(GameCulture.Chinese, "皇天幻影");
            Main.projFrames[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 52;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.frame = WalkingFrame1;
            projectile.timeLeft = 60 * 60 * 24 * 30;
            projectile.soundDelay = 0;
            projectile.friendly = true;
            Role = 0;
            WeaponRotation = 0f;
            WeaponPosition = Vector2.Zero;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (player.dead || !player.active)
            {
                projectile.Kill();
                return;
            }
            if (projectile.timeLeft < 1800) projectile.timeLeft = 1800;
            Item item = player.inventory[player.selectedItem];
            if (!item.IsAir)
            {
                Role = Melee;
                /*if (item.melee) Role = Melee;
                else if (item.ranged) Role = Ranged;
                else if (item.magic) Role = Magic;
                else if (item.summon) Role = Summon;*/
            }
            ref float CurrentStatus = ref projectile.ai[0];
            ref float ShootCD = ref projectile.ai[1];
            ref Vector2 velocity = ref projectile.velocity;
            if (CurrentStatus == 0) CurrentStatus = Returning;
            if (ShootCD > 0) ShootCD--;
            #region FindTarget
            int Target = -1;
            if (player.HasMinionAttackTargetNPC)
            {
                Target = player.MinionAttackTargetNPC;
                NPC See = Main.npc[Target];
                if (projectile.Distance(See.Center) < 800) Target = See.whoAmI;
            }
            else
            {
                float MinDistance = 800;
                foreach (NPC See in Main.npc)
                {
                    if (See.active && !See.friendly && !See.dontTakeDamage && projectile.Distance(See.Center) < MinDistance && See.CanBeChasedBy())
                    {
                        MinDistance = projectile.Distance(See.Center);
                        Target = See.whoAmI;
                    }
                }
                if (Target == -1)
                {
                    foreach (NPC See in Main.npc)
                    {
                        if (See.active && !See.friendly && !See.dontTakeDamage && projectile.Distance(See.Center) < MinDistance)
                        {
                            MinDistance = projectile.Distance(See.Center);
                            Target = See.whoAmI;
                        }
                    }
                }
            }
            #endregion
            if (CurrentStatus > NoTargetFound || Role == None)
            {
                #region TurningBack
                if (CurrentStatus != Returned)
                {
                    CurrentStatus = Returning;
                    Vector2 BackPos = Vector2.UnitY * -64 + player.Center;
                    Vector2 BackVelo = BackPos - projectile.Center;
                    BackVelo.Normalize();
                    velocity = BackVelo * 17f;
                    if (projectile.Distance(BackPos) < 32f || projectile.Distance(BackPos) > 16f * 480f)
                    {
                        CurrentStatus = Returned;
                        ClearWeapon();
                        SpawnDust(projectile.Center, 6f);
                        SpawnDust(BackPos, 6f);
                    }
                }
                else
                {
                    Vector2 BackPos = MathHelper.ToRadians(-90).ToRotationVector2() * 64;
                    BackPos = player.Center + BackPos;
                    velocity = player.velocity;
                    if (velocity != Vector2.Zero) velocity.Normalize();
                    projectile.Center = BackPos;
                }
                if (Target != -1 && Role != None) CurrentStatus = ChasingEnermy;
                #endregion
            }
            else
            {
                if (Target == -1)
                {
                    CurrentStatus = Returning;
                    ClearWeapon();
                }
                else
                {
                    NPC npc = Main.npc[Target];
                    float Speed1 = 17f;
                    float Speed2 = 3f;
                    float dir = projectile.AngleTo(npc.Center);
                    float range = 32f;
                    float rot = 114514f;
                    switch (Role)
                    {
                        case 1:
                            {
                                if (projectile.Distance(npc.Center) < 160f)
                                {
                                    projectile.friendly = true;
                                    projectile.damage = 0;
                                    if (CurrentStatus == ChasingEnermy)
                                    {
                                        CurrentStatus = AlreadyChasedOn;
                                        SpawnWeapon();
                                    }
                                    velocity = Vector2.Lerp(velocity, npc.velocity, 0.1f);
                                    if (projectile.Distance(npc.Center) < 80f)
                                    {
                                        velocity *= 0.985f;
                                    }
                                }
                                else velocity = Vector2.Lerp(velocity, projectile.DirectionTo(npc.Center) * Speed1, 0.06f);
                                if (HasWeapon)
                                {
                                    /* dir = MathHelper.ToRadians(MathHelper.Lerp(30, -90, ShootCD / 13));
                                     range = 16f;
                                     if (projectile.spriteDirection == -1) dir = MathHelper.Pi - dir;*/
                                    /*projectile.friendly = false;
                                    projectile.damage = 100;

                                    f = (float)(1f - (Main.time % 60) / 60f);
                                    timeLeft--;
                                    if (timeLeft < 0)
                                        timeLeft = 0;
                                    if (center == default)
                                    {
                                        center = projectile.Center;
                                        projectile.ai[0] = (float)Main.rand.Choose(-1, 1);
                                        if (altSound > 3)
                                        {
                                            altSound = 1;
                                        }
                                        Main.PlaySound(50, (int)projectile.position.X, (int)projectile.position.Y, mod.GetSoundSlot(SoundType.Custom, string.Format("Sounds/Items/buryTheLightHit{0}", altSound++)), 0.8f, 0f);
                                    }
                                    if (Main.netMode != 2)
                                    {
                                        color = Main.player[projectile.owner].GetModPlayer<SummonHeartPlayer>().ColorOrOther(new Color(200, 255, 248));
                                    }
                                    Vector2 vel = projectile.velocity.RotatedBy((Ease(f) - 0.5f) * projectile.ai[0] * 2.3561945f, default);
                                    projectile.rotation = vel.ToRotation() - 1.5707964f;
                                    projectile.Center = center + vel * (64f + 64f * (1f - Ease(f)));*/


                                    if (ShootCD == 0 && player.whoAmI == Main.myPlayer)
                                    {
                                        ShootCD = 20;
                                        Vector2 vel = VectorHelper.VelocityToPoint(projectile.Center, npc.Center, 1f);
                                        Projectile.NewProjectileDirect(npc.Center - vel * 60f, vel, ModContent.ProjectileType<DragonLegacyBlue>(), player.HeldItem.damage, 6.6f, player.whoAmI, 0f, 0f).netUpdate = true;

                                        /*Vector2 velocity2 = projectile.DirectionTo(npc.Center) * 24f;
                                        int amt = Main.rand.Next(4, 6);
                                        for (int i = 0; i < amt; i++)
                                        {
                                            velocity2 += new Vector2(Main.rand.Next(-20, 21), Main.rand.Next(-20, 21)) * 0.05f;
                                            float damageMult = 0.5f;
                                            int type = ProjectileType<DragonLegacyRed>();
                                            switch (i)
                                            {
                                                case 1:
                                                    type = ProjectileType<MeldGreatswordMediumProjectile>();
                                                    damageMult = 0.65f;
                                                    break;
                                                case 2:
                                                    type = ProjectileType<MeldGreatswordBigProjectile>();
                                                    damageMult = 0.8f;
                                                    break;
                                            }
                                            Projectile.NewProjectile(projectile.Center, velocity2, type, (int)(45 * player.MeleeDamage() * damageMult), 5.25f, player.whoAmI);

                                        }*/
                                    }
                                }
                                break;
                            }
                        case 2:
                            {
                                if (CurrentStatus == ChasingEnermy)
                                {
                                    CurrentStatus = AlreadyChasedOn;
                                    SpawnWeapon();
                                }
                                Vector2 RangedGo = projectile.DirectionTo((player.Center * 3 + npc.Center) / 4f);
                                velocity = Vector2.Lerp(velocity, RangedGo * 12f, 0.05f);
                                if (HasWeapon)
                                {
                                    range = 20f;
                                    if (ShootCD == 0 && player.whoAmI == Main.myPlayer)
                                    {
                                       /* Projectile.NewProjectile(projectile.Center, projectile.DirectionTo(npc.Center) * 9f, ProjectileType<CosmicFire>(),
                                            (int)(40 * player.RangedDamage()), 3.5f, player.whoAmI);*/
                                        ShootCD = 6;
                                    }
                                }
                                break;
                            }
                        case 3:
                            {
                                projectile.velocity = Vector2.Zero;
                                if (projectile.Distance(npc.Center) < 240)
                                {
                                    if (CurrentStatus == ChasingEnermy)
                                    {
                                        CurrentStatus = AlreadyChasedOn;
                                        SpawnWeapon();
                                    }
                                }
                                else
                                {
                                    Vector2 RandPos = Vector2.UnitY * Main.rand.Next(32, 160);
                                    RandPos = npc.Center + RandPos.RotatedByRandom(MathHelper.TwoPi);
                                    SpawnDust(projectile.Center, 6f);
                                    projectile.Center = RandPos;
                                    SpawnDust(RandPos, 6f);
                                }
                                if (HasWeapon)
                                {
                                    if (ShootCD == 0 && player.whoAmI == Main.myPlayer)
                                    {
                                       /* Vector2 velo = Vector2.Normalize(projectile.DirectionTo(npc.Center) * 4f + SHUtils.RandomRotate) * 17f;
                                        float rLerp = Main.rand.Next(10, 160) * 0.001f;
                                        if (Main.rand.Next(2) == 0) rLerp *= -1f;
                                        float rDir = Main.rand.Next(10, 160) * 0.001f;
                                        if (Main.rand.Next(2) == 0) rDir *= -1f;
                                        int type = ProjectileType<CosmicTentacle>();
                                        float damage2 = 55 * player.MagicDamage();
                                        if (Main.rand.Next(7) == 0)
                                        {
                                            type = ProjectileType<BrimstoneTentacle>();
                                            damage2 *= 1.5f;
                                        }
                                        Projectile.NewProjectile(projectile.Center, velo, type, (int)damage2, 3.5f, player.whoAmI, rDir, rLerp);
                                        ShootCD = 5;*/
                                    }
                                }
                                break;
                            }
                        case 4:
                            {
                                if (projectile.Distance(npc.Center) < 20 * 16)
                                {
                                    velocity = Vector2.Lerp(velocity, npc.velocity, 0.05f);
                                    if (CurrentStatus == ChasingEnermy)
                                    {
                                        CurrentStatus = AlreadyChasedOn;
                                        SpawnWeapon();
                                    }
                                }
                                else velocity = Vector2.Lerp(velocity, projectile.DirectionTo(npc.Center) * Speed1, 0.05f);
                                if (HasWeapon)
                                {
                                    /*if (projectile.spriteDirection != -1) dir = MathHelper.Pi;
                                    rot = 0f;
                                    if (ShootCD == 0 && player.whoAmI == Main.myPlayer && player.ownedProjectileCounts(ProjectileType<LanternFlame>()) < 35)
                                    {
                                        ShootCD = Main.rand.Next(5, 9);
                                        float startOffsetX = Main.rand.Next(15, 200) * (Main.rand.NextBool() ? -1f : 1f);
                                        float startOffsetY = Main.rand.Next(15, 200) * (Main.rand.NextBool() ? -1f : 1f);
                                        Vector2 Pos = new Vector2(projectile.Center.X + startOffsetX, projectile.Center.Y + startOffsetY);
                                        Projectile.NewProjectile(Pos, Vector2.Zero, ProjectileType<LanternFlame>(),
                                               (int)(32 * player.MinionDamage()), 5f, player.whoAmI, 0f, 0f);
                                    }*/
                                }
                                break;
                            }
                        case 5:
                            {
                                if (projectile.Distance(npc.Center) < 320)
                                {
                                    velocity = Vector2.Lerp(velocity, npc.velocity, 0.06f);
                                    if (projectile.Distance(npc.Center) > 160)
                                        velocity = Vector2.Lerp(velocity, projectile.DirectionTo(npc.Center) * Speed2, 0.05f);
                                    if (CurrentStatus == ChasingEnermy)
                                    {
                                        CurrentStatus = AlreadyChasedOn;
                                        SpawnWeapon();
                                    }
                                }
                                else velocity = Vector2.Lerp(velocity, projectile.DirectionTo(npc.Center) * Speed1, 0.05f);
                                if (HasWeapon)
                                {
                                    if (ShootCD == 0 && player.whoAmI == Main.myPlayer)
                                    {
                                       /* Projectile disk = Projectile.NewProjectileDirect(projectile.Center, projectile.DirectionTo(npc.Center) * 20F,
                                            ProjectileType<DestructionStar>(), (int)(750 * player.RogueDamage()), 10f, player.whoAmI);
                                        disk.penetrate = 1;*/
                                        ShootCD = 30;
                                    }
                                }
                                break;
                            }
                    }
                    if (rot == 114514f) rot = dir;
                    WeaponPosition = dir.ToRotationVector2() * range;
                    WeaponRotation = rot;
                }
            }
            #region VisualEffect
            ref int Frame = ref projectile.frame;
            ref int SideFrame = ref projectile.frameCounter;
            ref float SideFrameCounter = ref projectile.knockBack;
            ref int ShadowEffect = ref projectile.soundDelay;
            ShadowEffect += 2;
            if (ShadowEffect > 40 || projectile.soundDelay < 0) ShadowEffect = 0;
            if (CurrentStatus > NoTargetFound)
            {
                SideFrame = 0;
                if (Frame >= 2) Frame = WalkingFrame1;
                SideFrameCounter++;
                if (SideFrameCounter >= 7)
                {
                    SideFrameCounter = 0;
                    if (Frame == WalkingFrame1) Frame = WalkingFrame2;
                    else if (Frame == WalkingFrame2) Frame = WalkingFrame1;
                }
                projectile.spriteDirection = player.direction;
            }
            else
            {
                if (velocity.Y > 0.1f)
                {
                    SideFrameCounter = 0;
                    SideFrame = 2;
                    Frame = FlyingFrameDown;
                }
                else
                {
                    SideFrameCounter++;
                    if (SideFrameCounter >= 6)
                    {
                        SideFrameCounter = 0;
                        SideFrame++;
                    }
                    if (SideFrame >= 4) SideFrame = 0;
                    Frame = FlyingFrameUp;
                }
                float TryToLerp = Abs(velocity.X);
                TryToLerp = MathHelper.ToRadians(25 - 25 / (1 + TryToLerp / 5f));
                if (projectile.Center.X > Main.npc[Target].Center.X) projectile.spriteDirection = -1;
                else projectile.spriteDirection = 1;
                if (velocity.X > 0) projectile.rotation = TryToLerp;
                else projectile.rotation = -TryToLerp;
            }
            #endregion
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            /*Texture2D Spectre = mod.GetTexture("Projectiles/Summon/EmpyreanSpectre");
            Texture2D Wing = mod.GetTexture("Projectiles/Summon/EmpyreanSpectre_Wing");*/
            Texture2D Spectre = mod.GetTexture("Projectiles/Summon/NPC");
            Texture2D Wing = mod.GetTexture("Projectiles/Summon/Wings");
            Texture2D weapon = mod.GetTexture("Projectiles/Melee/DragonLegacyBlue");
            Texture2D Weapon = Main.itemTexture[GetWeaponType()];
           /* int weaponType = GetWeaponType();
            if(weaponType == 1)
            {
                Weapon = weapon;
            }*/
            Rectangle GetSpectreFrameRectan = new Rectangle(0, projectile.frame * 54, 40, 52);
            Rectangle GetWingFrameRectan = new Rectangle(0, projectile.frameCounter * 62, 86, 60);
            Vector2 origin = new Vector2(20, 26);
            Vector2 worigin = new Vector2(43, 30);
            Vector2 weaorigin = Weapon.Size() / 2f;
            Vector2 TruePos = origin - Main.screenPosition;
            Vector2 wTruePos = new Vector2(-23 + 32, 26) - Main.screenPosition;
            Vector2 weaTruePos = origin - Main.screenPosition + WeaponPosition;
            SpriteEffects effects = SpriteEffects.None;
            SpriteEffects weffects = SpriteEffects.None;
            float wRotation = WeaponRotation;
            if (projectile.spriteDirection < 0)
            {
                effects = SpriteEffects.FlipHorizontally;
                weffects = SpriteEffects.FlipVertically;
                wTruePos = new Vector2(28, 26) - Main.screenPosition;
            }
            Color color = Color.Lerp(Main.DiscoColor, lightColor, 0.5f);
            color.R = 0;
            color.A = 0;
            for (int i = 0; i < 5; i++)
            {
                color *= 0.8f;
                spriteBatch.Draw(Wing, projectile.oldPos[i] + wTruePos, GetWingFrameRectan, color, projectile.rotation, worigin, 1f, effects, 0f);
                spriteBatch.Draw(Spectre, projectile.oldPos[i] + TruePos, GetSpectreFrameRectan, color, projectile.rotation, origin, 1f, effects, 0f);
                if (HasWeapon) spriteBatch.Draw(Weapon, projectile.oldPos[i] + weaTruePos, null, color, WeaponRotation, weaorigin, 1f, weffects, 0f);
            }
            spriteBatch.Draw(Wing, projectile.position + wTruePos, GetWingFrameRectan, lightColor, projectile.rotation, worigin, 1f, effects, 0f);
            spriteBatch.Draw(Spectre, projectile.position + TruePos, GetSpectreFrameRectan, lightColor, projectile.rotation, origin, 1f, effects, 0f);
            if (HasWeapon) spriteBatch.Draw(Weapon, projectile.position + weaTruePos, null, lightColor, wRotation, weaorigin, 1f, weffects, 0f);
            return false;
        }
        public override bool CanDamage() => false;
        private void SpawnDust(Vector2 Center, float Speed)
        {
            for (int i = 0; i < 18; i++)
            {
                Vector2 DustRotate = (MathHelper.TwoPi * i / 18).ToRotationVector2();
                Dust dust = Dust.NewDustPerfect(Center, 62, DustRotate * Speed / 2, 100, default, 2f);
                dust.noGravity = true;
                dust.noLight = false;
                dust.scale = 2f;
            }
        }
        private void ClearWeapon()
        {
            SpawnDust(WeaponPosition, 14f);
            HasWeapon = false;
            Role = None;
            WeaponRotation = 0f;
            WeaponPosition = Vector2.Zero;
        }
        private void SpawnWeapon()
        {
            HasWeapon = true;
            SpawnDust(projectile.Center, 14f);
        }
        private int GetWeaponType()
        {
            switch (Role)
            {
                case 1: return ItemType<DemonSword>();
                case 2: return ItemType<DemonSword>();
                case 3: return ItemType<DemonSword>();
                case 4: return ItemType<DemonSword>();
            }
            return 0;
        }

        private static float Ease(float f)
        {
            if (f < 1f)
            {
                return 1f - (float)Math.Pow(2.0, -10f * f);
            }
            return 1f;
        }
        public bool HasWeapon;
        public int Role;
        public float WeaponRotation;
        public Vector2 WeaponPosition;
        public override bool CloneNewInstances => true;

        private const int WalkingFrame1 = 0;
        private const int WalkingFrame2 = 1;
        private const int FlyingFrameDown = 2;
        private const int FlyingFrameUp = 3;
        private const int Returning = 23333;
        private const int Returned = 66666;
        private const int NoTargetFound = 10000;
        private const int ChasingEnermy = 1234;
        private const int AlreadyChasedOn = 4321;
        private const int None = 0;
        private const int Melee = 1;
        private const int Ranged = 2;
        private const int Magic = 3;
        private const int Summon = 4;
        private const int Rogue = 5;

        private static int altSound = 1;
        private Vector2 center;
        private Color color;
        private float f;
        private int timeLeft = 60;

    }
}