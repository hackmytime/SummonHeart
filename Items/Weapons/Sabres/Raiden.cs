using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using System.Linq;
using Terraria.Utilities;
using SummonHeart.Utilities;

namespace SummonHeart.Items.Weapons.Sabres
{
    public static class RaidenUtils
    {
        public const int focusTime = 60;
        public const int focusTargets = 30;
        private const float focusRadius = 400f;
        private const float focusRadiusReductionSpeedFactor = 15f;
        private const int dustId = MyDustId.BlueMagic;//106

        public static int DustAmount(Player player) { return player.whoAmI == Main.myPlayer ? 32 : 2; }

        public static float GetFocusRadius(Player player)
        {
            float speedFactor = Math.Max(Math.Abs(player.velocity.X), Math.Abs(player.velocity.Y / 2));
            return Math.Max(64, focusRadius);
        }

        public static void DrawDustRadius(Player player, float radius, int amount = 4)
        {
            // Range display dust;
            for (int i = 0; i < amount; i++)
            {
                Vector2 offset = new Vector2();
                double angle = Main.rand.NextDouble() * 2d * Math.PI;
                offset.X += (float)(Math.Sin(angle) * radius);
                offset.Y += (float)(Math.Cos(angle) * radius);

                Dust d = Dust.NewDustPerfect(player.Center + offset, dustId, player.velocity, 200, default, 0.3f);
                d.fadeIn = 0.5f;
                d.noGravity = true;
            }
        }

        public static void DrawOrderedTargets(Player player, List<NPC> targets)
        {
            // Display targets for client
            if (player.whoAmI == Main.myPlayer)
            {
                Vector2 last = player.Center;
                for (int i = 0; i < targets.Count; i++)
                {
                    DrawDustToBetweenVectors(last, targets[i].Center, dustId, 5, 0.35f);
                    last = targets[i].Center;
                }

                // Crosshair
                if (targets.Count > 0)
                {
                    int velo = (int)(Main.rand.NextFloatDirection() * 16f);

                    for (int y = -1; y <= 1; y++)
                    {
                        for (int x = -1; x <= 1; x++)
                        {
                            Dust d = Dust.NewDustPerfect(last + new Vector2(velo * 4, y),
                                182, new Vector2(-velo, 0));
                            d.scale = 0.5f;
                            d.noGravity = true;

                            d = Dust.NewDustPerfect(last + new Vector2(x, velo * 4),
                                182, new Vector2(0, -velo));
                            d.scale = 0.5f;
                            d.noGravity = true;
                        }
                    }
                }
            }
        }

        public static List<NPC> GetTargettableNPCs(Vector2 center, Vector2 targetCentre, float radius, int limit = 15)
        {
            Player player = Main.player[Main.myPlayer];
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

            Dictionary<NPC, float> targets = new Dictionary<NPC, float>();
            foreach (NPC npc in Main.npc)
            {
                if (!npc.active || npc.life == 0) continue;
                if (npc.CanBeChasedBy() || npc.type == NPCID.TargetDummy)
                {
                    float distance = (center - npc.Center).Length();
                    if (distance <= radius)
                    {
                        float distanceToFocus = (targetCentre - npc.Center).Length();
                        if(modPlayer.shortSwordBlood >= 5000)
                        {
                            targets.Add(npc, distanceToFocus);
                        }
                        else if(Collision.CanHit(center - new Vector2(12, 12), 24, 24, npc.position, npc.width, npc.height))
                        {
                            targets.Add(npc, distanceToFocus);
                        }
                    }
                }
            }

            // Sort the list by distance (inner to outer)
            List<NPC> targetList = new List<NPC>(targets.Count);
            var ie = targets.OrderBy(pair => pair.Value).Take(targets.Count);

            foreach (KeyValuePair<NPC, float> kvp in ie)
            {
                if (limit > 0) { targetList.Add(kvp.Key); }
                limit--;
            }

            // Invert (outer to inner)
            targetList.Reverse();

            return targetList;
        }
        public static void DrawDustToBetweenVectors(Vector2 start, Vector2 end, int dust, int amount = 5, float scale = 0.5f, bool noGravity = true)
        {
            Vector2 dustDisplace = new Vector2(-4, -4);
            Vector2 difference = end - start;
            for (int i = 0; i < amount; i++)
            {
                Dust d = Dust.NewDustPerfect(start + difference * Main.rand.NextFloat() + dustDisplace, dust, difference * 0.01f, 0, default, scale);
                d.noGravity = noGravity;
            }
        }
    }

    /// <summary>
    /// Yo it's like, a homing weapon or something.
    /// </summary>
    public class Raiden : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DemonShortSword");
            DisplayName.AddTranslation(GameCulture.Chinese, "魔剑·弑神·传承武器");

            Tooltip.SetDefault(
              "Refining eight realms · the peak of martial arts · cast by the immortal right arm of the ancient demon God before he died" +
               "\nThe guardian weapon of the devil's son can only be summoned by blood essence" +
               "\nResentment of all living beings: critical hit bonus without any damage reduced by 4 times attack speed bonus and 2 times attack range bonus" +
               "\nPower of killing gods: kill any creature + 1 attack power, but limited by the upper limit of awakening." +
               "\nSword awakening: kill the strong, capture their flesh and soul, repair the sword body and increase the upper limit of awakening.");
            Tooltip.AddTranslation(GameCulture.Chinese, "" +
                "炼体八境·武道巅峰·远古魔神临死之前碎裂不朽右臂所铸造" +
                "\n魔神之子的护道传承武器，唯魔神之子可用精血召唤使用" +
                "\n众生之怨：不受任何伤害暴击攻击范围加成，无法附魔，减少3倍攻速加成" +
                "\n弑神之力：击杀任意生物+1攻击力，然受觉醒上限限制。" +
                "\n魔剑觉醒：击杀强者摄其血肉灵魂修复剑身，可突破觉醒上限。" +
                "\n空间法则：自身蕴含魔神所悟空间法则之力，剑出必中！");
        }
        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 40;

            item.melee = true;
            item.damage = 1;
            item.knockBack = 5;
            item.autoReuse = true;

            item.useStyle = 1;
            item.UseSound = SoundID.Item1;
            item.useTime = 60 / 4;
            item.useAnimation = 17;

            item.rare = -12;
            item.value = Item.sellPrice(0, 99, 0, 0);
        }

        public override bool AllowPrefix(int pre)
        {
            return false;
        }

        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return new bool?(false);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("GuideNote"), 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.player[Main.myPlayer];
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

            if (modPlayer.shortSwordBlood == 0)
                modPlayer.shortSwordBlood = 1;
            if (modPlayer.swordBloodMax < 100)
                modPlayer.swordBloodMax = 100;

            int num = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("CritChance"));
            if (num != -1)
            {
                string str = (modPlayer.shortSwordBlood * 1.0f / 100f).ToString("0.00") + "%";
                tooltips[num].overrideColor = Color.LimeGreen;
                tooltips[num].text = str + (GameCulture.Chinese.IsActive ? "觉醒度" : "Arousal Level");
                string text;
                text = (modPlayer.swordBloodMax * 1.0f / 100f).ToString("0.00") + "%觉醒上限";
                TooltipLine tooltipLine = new TooltipLine(base.mod, "SwordBloodMax", text);
                tooltipLine.overrideColor = Color.Red;
                tooltips.Insert(num + 1, tooltipLine);
            }
        }

        public override void GetWeaponCrit(Player player, ref int crit)
        {
            crit = 0;
        }

        public override void GetWeaponDamage(Player player, ref int damage)
        {
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
            damage = modPlayer.shortSwordBlood;
        }

        public override void HoldItem(Player player)
        {
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

            WeaponSabres.HoldItemManager(player, item, mod.ProjectileType("RaidenSlash"),
                Color.Red, 1f, false ? 0f : 1f, customCharge, 8);

            if (modPlayer.showRadius)
            {
                float radius = RaidenUtils.GetFocusRadius(player);

                RaidenUtils.DrawDustRadius(player, radius, RaidenUtils.DustAmount(player));

                if (Main.myPlayer == player.whoAmI)
                {
                    Vector2 mouse = new Vector2(Main.screenPosition.X + Main.mouseX, Main.screenPosition.Y + Main.mouseY);
                    List<NPC> targets = RaidenUtils.GetTargettableNPCs(player.Center, mouse, radius, RaidenUtils.focusTargets);
                    RaidenUtils.DrawOrderedTargets(player, targets);
                }
            }
        }

        public Action<Player, bool> customCharge = CustomCharge;
        public static void CustomCharge(Player player, bool flashFrame)
        {
            /*float chargeSize = player.itemTime * 4f; //roughly 15 * 4
            Vector2 dustPosition = player.Center + new Vector2(
                Main.rand.NextFloatDirection() * chargeSize,
                Main.rand.NextFloatDirection() * chargeSize
                );

            // Charging dust
            Dust d = Dust.NewDustPerfect(dustPosition, 235);
            d.scale = Main.rand.Next(70, 85) * 0.01f;
            d.fadeIn = player.whoAmI + 1;*/
        }

        public override bool HoldItemFrame(Player player) //called on player holding but not swinging
        {
            if (player.itemTime == 0 && false) //ready to slash targets
            {
                player.bodyFrame.Y = 4 * player.bodyFrame.Height;
                return true;
            }
            return false;
        }

        public override bool UseItemFrame(Player player)
        {
            //WeaponSabres.UseItemFrame(player, 0.9f, item.isBeingGrabbed);
            return true;
        }

        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            int height = 94;
            int length = 104;
            WeaponSabres.UseItemHitboxCalculate(player, item, ref hitbox, ref noHitbox, 0.9f, height, length);
            if (WeaponSabres.SabreIsChargedStriking(player, item))
            {
                player.meleeDamage += 2f;
                noHitbox = player.itemAnimation < player.itemAnimationMax - 10;
            }
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            Color colour = new Color(0.4f, 0.8f, 0.1f);
            WeaponSabres.OnHitFX(player, target, crit, colour, true);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            float rotation = player.itemRotation + MathHelper.PiOver4;
            if (item.isBeingGrabbed) rotation -= MathHelper.PiOver2; // Reverse slash (upward) flip
            if (player.direction < 0) { rotation *= -1f; rotation += MathHelper.Pi; }
            Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            int dustId = MyDustId.YellowFx;//159
            Dust d = Dust.NewDustDirect(hitbox.Location.ToVector2() - new Vector2(4, 4), hitbox.Width, hitbox.Height, dustId);
            d.color = Color.Red;
            d.scale = 0.5f;
            d.fadeIn = 0.5f;
            d.position -= direction * 25f;
            d.velocity = direction * 5f;
        }
    }

    public class RaidenSlash : ModProjectile
    {
        public const int specialProjFrames = 9;
        bool sndOnce = true;
        int chargeSlashDirection = 1;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Raiden");
            DisplayName.AddTranslation(GameCulture.Chinese, "雷电");
            Main.projFrames[projectile.type] = specialProjFrames;
        }
        public override void SetDefaults()
        {
            projectile.width = 104;
            projectile.height = 94;
            projectile.aiStyle = -1;
            projectile.timeLeft = 60;

            projectile.friendly = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;

            projectile.penetrate = -1;
        }

        public override bool? CanCutTiles() { return false; }
        public float FrameCheck
        {
            get { return projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }
        public float SlashLogic
        {
            get { return (int)projectile.ai[1]; }
            set { projectile.ai[1] = value; }
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (WeaponSabres.AINormalSlash(projectile, SlashLogic))
            {
                FrameCheck += 1f;
                targets = null;
            }
            else
            {
                // Charged attack
                WeaponSabres.AISetChargeSlashVariables(player, chargeSlashDirection);
                WeaponSabres.NormalSlash(projectile, player);

                // Play charged sound & set targets
                if (sndOnce)
                {
                    Vector2 mouse;
                    if (Main.myPlayer == player.whoAmI)
                    { mouse = new Vector2(Main.screenPosition.X + Main.mouseX, Main.screenPosition.Y + Main.mouseY); }
                    else
                    { mouse = player.Center + new Vector2(player.direction * 256); } // an estimation
                    targets = RaidenUtils.GetTargettableNPCs(player.Center, mouse, RaidenUtils.GetFocusRadius(player), RaidenUtils.focusTargets);
                    if (targets.Count > 0)
                    {
                        Main.PlaySound(SoundID.Item71, projectile.Center); sndOnce = false;
                        SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
                        modPlayer.chargeAttack = false;
                        modPlayer.showRadius = false;

                        // Set up initial ending position as where we started
                        if (player.whoAmI == Main.myPlayer)
                        { endingPositionCenter = player.Center; }

                        // Set ending slash direction
                        if (targets.Last().Center.X > player.Center.X)
                        { player.direction = 1; }
                        else
                        { player.direction = -1; }
                    }
                }

                if (targets != null && targets.Count > 0)
                {
                    ChargeSlashAI(player);
                }
                else
                {
                    SlashLogic = 1;
                    WeaponSabres.AINormalSlash(projectile, SlashLogic);
                    FrameCheck += 1f;
                    targets = null;
                }
            }
            projectile.damage = 0;
        }

        /// <summary> As there are 9 frames, gotta fit the number of targets within this window (roughly)</summary>
        public int totalTargetTime = 30;
        public List<NPC> targets = null;
        private Vector2? endingPositionCenter = null;
        public void ChargeSlashAI(Player player)
        {
            if (targets.Count == 1) totalTargetTime = 5;
            if (targets.Count == 2) totalTargetTime = 10;
            if (targets.Count == 3) totalTargetTime = 15;
            if (targets.Count == 4) totalTargetTime = 20;
            if (targets.Count == 5) totalTargetTime = 25;

            setAnimationAndImmunities(player);

            float countf = targets.Count;
            int framesPerTarget = (int)Math.Max(1, (float)totalTargetTime / countf);
            float oneFrame = specialProjFrames / (framesPerTarget * countf); // calculates ro roughly 9 / 30 = 0.3

            // Get current frame, and current target in attack
            int i = (int)(FrameCheck / oneFrame);

            if (i >= framesPerTarget * countf)
            {
                // End frame
                player.Center = (Vector2)endingPositionCenter;
                player.velocity = new Vector2(projectile.direction * -7.5f, player.gravDir * -2);
                projectile.timeLeft = 0;

                return;
            }
            else
            {
                // Set camera lerp
                if (player.whoAmI == Main.myPlayer)
                { Main.SetCameraLerp(0.1f, 10); }

                // Get the target position, or wait if the target is invalid
                int iTarget = (int)MathHelper.Clamp((float)i / framesPerTarget, 0, countf - 1);
                NPC target = targets[iTarget];
                Vector2 targetBottom;
                if (target == null || !target.active || target.dontTakeDamage)
                { targetBottom = player.Bottom; target = null; }
                else
                {
                    // Set target
                    targetBottom = target.Bottom;
                    // and rotate slash
                    Vector2 toTarget = targetBottom - player.Bottom;
                    projectile.rotation = (float)Math.Atan2(toTarget.Y, toTarget.X);
                }

                Vector2 oldBottom = new Vector2(player.Bottom.X, player.Bottom.Y);
                Vector2 vecHeight = new Vector2(0, Player.defaultHeight / -2);

                // Tweening if there is time
                if (framesPerTarget > 1)
                {
                    Vector2 dist = (targetBottom - player.Bottom) * (1f / Math.Max(1, framesPerTarget / 2f));
                    player.Bottom = player.Bottom + dist;
                    player.velocity.Y = -player.gravDir * 1.5f;

                    int distFactor = (int)(dist.Length() / 4f);
                    RaidenUtils.DrawDustToBetweenVectors(oldBottom + vecHeight, player.Bottom + vecHeight, 15, distFactor, 2f);
                }

                int framesToNextKeyframe = Math.Max(0, ((iTarget + 1) * framesPerTarget - 1) - i);

                //// Snap to target on key frames, assuming they can be reached
                if (framesToNextKeyframe == 0)
                {
                    player.Bottom = targetBottom;

                    RaidenUtils.DrawDustToBetweenVectors(oldBottom + vecHeight, player.Bottom + vecHeight, 15, 2, 2f);
                }

                // Set slash
                WeaponSabres.RecentreSlash(projectile, player);


                // Clientside unstick code, don't bother for others in MP
                if (player.whoAmI == Main.myPlayer)
                { UpdateValidEndingPosition(player); }
                else { endingPositionCenter = player.Center; }

                FrameCheck += oneFrame;
            }
        }

        private void UpdateValidEndingPosition(Player player)
        {
            // Check if the player is inside tiles
            bool stuck = false;
            try
            {
                Point targetTileBottom = player.Bottom.ToTileCoordinates();
                for (int y = targetTileBottom.Y - 1; y >= targetTileBottom.Y - 3; y--)
                {
                    int x = targetTileBottom.X;

                    //Main.NewText("check X " + x + " ? pX" + player.position.ToTileCoordinates().X);//DEBUG
                    Dust.NewDustPerfect(new Vector2(x * 16 + 8, y * 16 + 8), 127);//DEBUG

                    if (Main.tile[x, y] != null && Main.tile[x, y].active() && Main.tileSolid[Main.tile[x, y].type])
                    {
                        stuck = true; break;
                    }
                    if (stuck) break;
                }
            }
            catch { stuck = true; }

            // If we're fine, we can update the position
            if (!stuck)
            { endingPositionCenter = player.Center; }
        }

        private void setAnimationAndImmunities(Player player)
        {
            // freeze in swing
            player.itemAnimation = player.itemAnimationMax - 2;
            player.itemTime = player.itemAnimation;
            player.attackCD = 0;

            // Set immunities
            player.immune = true;
            player.immuneTime = Math.Max(player.immuneTime, 30); //half second
            player.immuneNoBlink = true;
            player.fallStart = (int)(player.position.Y / 16f);
            player.fallStart2 = player.fallStart;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];
            int weaponItemID = mod.ItemType("Raiden");
            Color lighting = Lighting.GetColor((int)(player.MountedCenter.X / 16), (int)(player.MountedCenter.Y / 16));
            return WeaponSabres.PreDrawSlashAndWeapon(spriteBatch, projectile, weaponItemID, lighting,
                null,//SlashLogic == 0f ? specialSlash : null,
                lighting,
                specialProjFrames,
                SlashLogic == 0f ? chargeSlashDirection : SlashLogic,
                SlashLogic == 0f);
        }
    }
}
