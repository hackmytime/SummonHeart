using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SummonHeart.Utilities
{
    public static class SHUtils
    {
        public static void HealLife(this Player player, int amount, bool visible = true)
        {
            player.statLife += amount;
            if (player.statLife > player.statLifeMax2) player.statLife = player.statLifeMax2;
            if (visible) player.HealEffect(amount, true);
        }
        public static void HealMana(this Player player, int amount, bool visible = true)
        {
            player.statMana += amount;
            if (player.statMana > player.statManaMax2) player.statMana = player.statManaMax2;
            if (visible) player.ManaEffect(amount);
        }
       
        public static bool CanHitNPC(this Projectile projectile, NPC npc) => Collision.CanHit(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
        public static void FollowPlayer(this Projectile projectile, Vector2 place, float maxDist, float minDist, Vector2 velo, float speed = 0, bool positionLerp = false, float lerpAmtFar = 0.05f, float lerpAmtClose = 0.07f, float lerpAmtPos = 0.1f)
        {
            if (projectile.Distance(place) > maxDist) projectile.Center = place - projectile.DirectionTo(place) * maxDist;
            else if (projectile.Distance(place) < minDist) projectile.velocity = Vector2.Lerp(projectile.velocity, velo, lerpAmtClose);
            else projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(place) * speed, lerpAmtFar);
            if (positionLerp) projectile.Center = Vector2.Lerp(projectile.Center, place, lerpAmtPos);
        }
        
        public static bool AnyBossAlive
        {
            get
            {
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && npc.boss) return true;
                    if (npc.active && npc.type == NPCID.EaterofWorldsHead) return true;
                }
                return false;
            }
        }
       
        public static void DoAVisualBuff(this Player player, int type, int time = 0)
        {
            player.ClearBuff(type);
            player.AddBuff(type, time + 2);
        }
        public static Vector2 RandomRotate => MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
        public static int ownedProjectileCounts(this Player player, int type)
        {
            int amt = 0;
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.active && projectile.type == type && projectile.owner == player.whoAmI) amt++;
            }
            return amt;
        }
        public static void ownedProjectileKill(this Player player, int type)
        {
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.active && projectile.type == type && projectile.owner == player.whoAmI) projectile.Kill();
            }
        }
        public static int TransFloatToInt(float num)
        {
            int low = (int)num;
            int chance = (int)((num - low) * 100);
            if (Main.rand.Next(100) < chance) low++;
            return low;
        }
    }
}
