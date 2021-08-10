using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Utilities;

namespace SummonHeart.Items.Weapons.Sabres
{
    /// <summary>
    /// Stand (mostly) still to charge a slash, messes with hitboxes etc.
    /// drawstrike does quad damage, with added crit for a total of x8
    /// 35 * 8 == 280
    /// Draw Strike speed = 80 + 20 + 15 == 115
    /// Draw Strike DPS = 146
    /// hey, its me, jetstream sammy
    /// </summary>
    public class DemonSword : ModItem
    {
        public const int waitTime = 15;
        public const int chargeDamageMult = 2;

        private bool drawStrike;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DemonSword");
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
                "\n众生之怨：不受任何伤害暴击加成，无法附魔，减少2倍攻速加成" +
                "\n弑神之力：击杀任意生物增加攻击力，然受觉醒上限限制。" +
                "\n破灭法则：重击必定暴击，并且暴击伤害翻倍" +
                "\n魔剑觉醒：击杀强者摄其血肉灵魂修复剑身，可突破觉醒上限。" +
                "\n史莱姆王：觉醒上限突破至5%" +
                "\n克苏鲁之眼：觉醒上限突破至10%" +
                "\n世吞/克脑：觉醒上限突破至20%" +
                "\n蜂王：觉醒上限突破至30%" +
                "\n骷髅王：觉醒上限突破至40%" +
                "\n血肉之墙：觉醒上限突破至50%" +
                "\n任意新三王：觉醒上限突破至80%" +
                "\n世纪之花：觉醒上限突破至100%" +
                "\n猪鲨公爵：觉醒上限突破至120%" +
                "\n石巨人：觉醒上限突破至150%" +
                "\n邪教徒：觉醒上限突破至200%" +
                "\n月球领主：击杀强者可以一直突破，觉醒无上限");

        }
        public override void SetDefaults()
        {
            item.width = 46;
            item.height = 46;

            item.melee = true;
            item.damage = 1; //DPS 126
            item.knockBack = 3;
            item.autoReuse = true;

            item.useStyle = 1;
            item.UseSound = SoundID.Item1;

            item.useTime = waitTime;
            item.useAnimation = 22;

            item.rare = -12;
            item.value = Item.sellPrice(999, 0, 0, 0);
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
            recipe.AddIngredient(mod.GetItem("MeleeScroll"), 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        // Define if the player is still enough to use the special
        public bool hasDemonSwordSpecialCharge(Player player)
        {
            return player.itemTime == 0
                && Math.Abs(player.position.X - player.oldPosition.X) < 1f
                && Math.Abs(player.position.Y - player.oldPosition.Y) < 2f;
        }

        public override void HoldItem(Player player)
        {
            //bool specialCharge = hasDemonSwordSpecialCharge(player);
            bool specialCharge = false;
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
            WeaponSabres.HoldItemManager(player, item, mod.ProjectileType("DemonSwordSlash"),
                Color.Red, 1.0f, specialCharge ? 0f : 1f, customCharge, 12);

            // Blade sheen once fully charged
            /*if (true)
            {
                Vector2 dustPos = getBladeDustPos(player, Main.rand.NextFloat());
                Dust d = Dust.NewDustDirect(dustPos, 0, 0, 90);
                d.velocity = Vector2.Zero;
                d.noGravity = true;
                d.scale = 0.2f;
                d.fadeIn = 0.8f;
            }*/
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.player[Main.myPlayer];
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

            if (modPlayer.swordBlood == 0)
                modPlayer.swordBlood = 1;
            if (modPlayer.swordBloodMax < 100)
                modPlayer.swordBloodMax = 100;

            int num = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("CritChance"));
            if (num != -1)
            {
                string str = (modPlayer.swordBlood * 1.0f / 100f).ToString("0.00") + "%";
                tooltips[num].overrideColor = Color.LimeGreen;
                tooltips[num].text = str + (GameCulture.Chinese.IsActive ? "觉醒度" : "Arousal Level");
                string text;
                text = "击杀敌人+" + (modPlayer.swordBloodMax / 10000 + 1) + "攻击力";
                TooltipLine tooltipLine = new TooltipLine(base.mod, "SwordBloodMax", text);
                tooltipLine.overrideColor = Color.Red;
                tooltips.Insert(num + 1, tooltipLine);
                text = (modPlayer.swordBloodMax * 1.0f / 100f).ToString("0.00") + "%觉醒上限";
                tooltipLine = new TooltipLine(base.mod, "SwordBloodMax", text);
                tooltipLine.overrideColor = Color.Red;
                tooltips.Insert(num + 2, tooltipLine);
            }
        }

        public override void GetWeaponCrit(Player player, ref int crit)
        {
            crit = 0;
        }

        public override void GetWeaponDamage(Player player, ref int damage)
        {
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
            damage = modPlayer.swordBlood;
        }

        public Action<Player, bool> customCharge = CustomCharge;
        public static void CustomCharge(Player player, bool flashFrame)
        {
            /*if (flashFrame) //Charge burst
            {
                Vector2 dustPos = getBladeDustPos(player, 0f);
                //Main.PlaySound(25, player.position);
                for (int i = 0; i < 10; i++)
                {
                    Dust d = Dust.NewDustDirect(dustPos, 0, 0, 130, player.direction, 0f);
                    d.scale = 0.6f;
                }
            }*/
            /*if (player.itemTime > 0) // Charging sheen effect
            {
                float chargeNormal = (float)player.itemTime / player.HeldItem.useTime; // 1 -> 0
                Vector2 dustPos = getBladeDustPos(player, chargeNormal);
                Dust d = Dust.NewDustDirect(dustPos, 0, 0, 71);
                d.scale = 0.5f;
                d.velocity *= chargeNormal / 2f;
            }*/
        }
        private static Vector2 getBladeDustPos(Player player, float distanceNormal)
        {
            int width = 32;
            int height = 16;
            int offSetX = 0;
            int offSetY = 8;
            distanceNormal = MathHelper.Clamp(distanceNormal, 0f, 1f);
            Vector2 bladePosition = player.Center;
            bladePosition += new Vector2(
                (offSetX - distanceNormal * width) * player.direction - 4,
                (offSetY + distanceNormal * height) * player.gravDir - 4
                );
            return bladePosition;
        }

        /*public override bool HoldItemFrame(Player player) //called on player holding but not swinging
        {
            if (hasDemonSwordSpecialCharge(player)) //ready to slash
            {
                player.bodyFrame.Y = 4 * player.bodyFrame.Height;
                return true;
            }
            return false;
        }*/

        public override bool UseItemFrame(Player player)
        {
            WeaponSabres.UseItemFrame(player, 0.9f, item.isBeingGrabbed);
            return true;
        }

        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            int height = 100;
            int length = 100;
            if (item.noGrabDelay > 0)
            {
                length = 228;
                height = 140;
            }
            WeaponSabres.UseItemHitboxCalculate(player, item, ref hitbox, ref noHitbox, 0.9f, height, length);
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            Color colour = new Color(1f, 0f, 0f);
            WeaponSabres.OnHitFX(player, target, crit, colour, true);
        }

        //x6 damage + crit to make up for terrible (but cool) usage
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
            if (WeaponSabres.SabreIsChargedStriking(player, item))
            {
                damage *= chargeDamageMult;
                knockBack *= 1.5f;
                crit = true;
            }
        }
        public override void ModifyHitPvp(Player player, Player target, ref int damage, ref bool crit)
        {
            if (WeaponSabres.SabreIsChargedStriking(player, item))
            {
                damage *= chargeDamageMult;
                if ((player.Center - target.Center).Length() > 70)
                { crit = true; }
            }
        }
    }


    public class DemonSwordSlash : ModProjectile
    {
        public static Texture2D specialSlash;
        public static int specialProjFrames = 6;
        bool sndOnce = true;
        int chargeSlashDirection = -1;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("DemonSword");
            DisplayName.AddTranslation(GameCulture.Chinese, "快打");
            DisplayName.AddTranslation(GameCulture.Russian, "Хаяуси");
            Main.projFrames[projectile.type] = 5;
            if (Main.netMode == 2) return;
            specialSlash = mod.GetTexture("Items/Weapons/Sabres/" + GetType().Name + "_Special");
        }
        public override void SetDefaults()
        {
            projectile.width = 100;
            projectile.height = 100;
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
            }
            else
            {
                // Charged attack
                projectile.height = 228;
                projectile.width = 140;
                WeaponSabres.AISetChargeSlashVariables(player, chargeSlashDirection);
                WeaponSabres.NormalSlash(projectile, player);

                // Play charged sound
                if (sndOnce)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Attack06"), player.Center);
                    sndOnce = false;
                    SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
                    modPlayer.chargeAttack = false;
                }

                float pow = (specialProjFrames - SlashLogic) / 16f;
                Lighting.AddLight(new Vector2(projectile.Center.X + 70, projectile.Center.Y),
                    new Vector3(pow, pow * 0.2f, pow * 0.8f));
                Lighting.AddLight(new Vector2(projectile.Center.X - 70, projectile.Center.Y),
                    new Vector3(pow, pow * 0.2f, pow * 0.8f));
                Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y + 70),
                    new Vector3(pow, pow * 0.2f, pow * 0.8f));
                Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y - 70),
                    new Vector3(pow, pow * 0.2f, pow * 0.8f));

                FrameCheck += 0.5f;
            }
            projectile.damage = 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];
            int weaponItemID = mod.ItemType("DemonSword");
            Color lighting = Lighting.GetColor((int)(player.MountedCenter.X / 16), (int)(player.MountedCenter.Y / 16));
            return WeaponSabres.PreDrawSlashAndWeapon(spriteBatch, projectile, weaponItemID, lighting,
                SlashLogic == 0f ? specialSlash : null,
                SlashLogic == 0f ? new Color(1f, 1f, 1f, 0.1f) : lighting,
                specialProjFrames,
                SlashLogic == 0f ? chargeSlashDirection : SlashLogic);
        }
    }
}
