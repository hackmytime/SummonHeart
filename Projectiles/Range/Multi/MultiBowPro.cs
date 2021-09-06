using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Extensions;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.Range.Multi
{
    public class MultiBowPro : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MultiBowPro");
            DisplayName.AddTranslation(GameCulture.Chinese, "弓分身");
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 52;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.frame = 1;
            projectile.timeLeft = 60 * 60 * 24 * 30;
            projectile.soundDelay = 0;
            projectile.friendly = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 6;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Player player = Main.player[projectile.owner];
            SummonHeartPlayer mp = player.getModPlayer();
            Texture2D Spectre = mod.GetTexture("Projectiles/Summon/NPC");
            Texture2D Weapon = Main.itemTexture[player.HeldItem.type];
            bool HasWeapon = false;
            if (Weapon != null)
                HasWeapon = true;
            Rectangle GetSpectreFrameRectan = new Rectangle(0, projectile.frame * 54, 40, 52);
            Rectangle GetWingFrameRectan = new Rectangle(0, projectile.frameCounter * 62, 86, 60);
            Vector2 origin = new Vector2(20, 26);
            Vector2 worigin = new Vector2(43, 30);
            Vector2 weaorigin = Weapon.Size() / 2f;
            Vector2 TruePos = origin - Main.screenPosition;
            Vector2 wTruePos = new Vector2(-23 + 32, 26) - Main.screenPosition;
            Vector2 weaTruePos = origin - Main.screenPosition + projectile.position;
            SpriteEffects effects = SpriteEffects.None;
            SpriteEffects weffects = SpriteEffects.None;
            float wRotation = projectile.rotation;
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
                spriteBatch.Draw(Weapon, projectile.oldPos[i] + TruePos, null, color, projectile.rotation, origin, 1f, effects, 0f);
            }
            spriteBatch.Draw(Weapon, projectile.position + TruePos, null, lightColor, projectile.rotation, origin, 1f, effects, 0f);
            return false;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.timeLeft = 60;
            if (player.dead || !player.active)
            {
                projectile.Kill();
                return;
            }
            Texture2D Weapon = Main.itemTexture[player.HeldItem.type];
            
            projectile.position.X = player.Center.X - Weapon.Width;
            projectile.position.Y = player.Center.Y - Weapon.Height - Weapon.Height * projectile.ai[1] * 1.5f;
            if (Main.player[projectile.owner].gravDir == -1f)
            {
                //projectile.position.Y = projectile.position.Y + 120f;
                projectile.rotation = 3.14f;
            }
            Vector2 vectorToCursor = Main.MouseWorld - projectile.Center;
            float distanceToCursor = vectorToCursor.Length();
            projectile.rotation = vectorToCursor.ToRotation();
        }
    }
}