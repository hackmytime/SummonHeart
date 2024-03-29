using Microsoft.Xna.Framework;
using SummonHeart.Extensions;
using SummonHeart.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Projectiles.Range.Tools
{
    public abstract class ExplosiveProjectile : ModProjectile
    {
        /// <summary>
        /// If this is true then the mod will add this projectile to the trinket avoid list. Returns false by default
        /// </summary>
        public bool IgnoreTrinkets = false;

        public bool InflictDamageSelf = true;
        public readonly bool Explosive = true;              // This marks the item as part of the explosive class
        public int radius = 0;                                  // Radius of the explosion
        public int pickPower = 0;                           // Strength of the explosion
        internal bool crit = false;                         // If it crits (dont edit this it is used internally Left internal so other bombs can edit the crit chances 
        protected LegacySoundStyle[] explodeSounds;         // The sounds that are played as the bomb explodes
        protected abstract string explodeSoundsLoc { get; } // Where the explosion sound files are located (relative to project dir)
        protected abstract string goreFileLoc { get; }      // Where the explosion gore sprites are located (relative to project dir)

        private bool firstTick;
        private bool firstTickPreAI;

        public virtual void SafeSetDefaults()
        {
        }

        public sealed override void SetDefaults()
        {
            //constants throughout all bombs
            SafeSetDefaults();
            projectile.melee = false;
            projectile.ranged = false;
            projectile.magic = false;
            projectile.thrown = false;
            projectile.minion = false;
            projectile.netUpdate = true;
            DangerousSetDefaults();

        }

        public override void AI()
        {
            if (!firstTick && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NetMessage.SendData(MessageID.SyncProjectile, number: projectile.whoAmI);
                firstTick = true;
            }
        }


        public virtual void DangerousSetDefaults()
        {
            // Does nothing, this should be used to override the values locked by SetDefaults
            // Only use if you need to since those values are set to ensure the bombs function as intended
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            return;
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            return;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            return;
        }

        public virtual void DustEffects(Color color = default, Color color2 = default, int type = 1, bool shake = true, int dustType = 6, ArmorShaderData shader = null)
        {
            GlobalMethods.ExplosionDust(radius, projectile.Center, color, color2, type, shake: shake, dustType: dustType, shader: shader);
        }

        //Explosion
        #region Explosion
        /// <summary>
        /// Takes the projectiles radius attribute in place of passing variables
        /// Creates a circular explosion in the radius defined
        /// Efficient but most blocks don't drop due to optimization methods
        /// </summary>
        public virtual void Explosion()
        {
            // x and y are the tile offset of the current tile relative to the player
            // i and j are the true tile cords relative to 0,0 in the world
            Player player = Main.player[projectile.owner];

            if (pickPower < -1) return;

            Vector2 position = new Vector2(projectile.Center.X / 16f, projectile.Center.Y / 16f);    // Converts to tile cords for convenience

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    int i = (int)(x + position.X);
                    int j = (int)(y + position.Y);
                    if (!WorldGen.InWorld(i, j)) continue;
                    if (Math.Sqrt(x * x + y * y) <= radius + 0.5) //Circle
                    {
                        Tile tile = Framing.GetTileSafely(i, j);

                        if (!WorldGen.TileEmpty(i, j) && tile.active())
                        {
                            if (!GlobalMethods.CanBreakTile(tile.type, pickPower)) continue;
                            //if (!CanBreakTiles) continue;
                            // Using KillTile is laggy, use ClearTile when working with larger tile sets    (also stops sound spam)
                            // But it must be done on outside tiles to ensure propper updates so use it only on outermost tiles
                            if (Math.Abs(x) >= radius - 1 || Math.Abs(y) >= radius - 1 || Terraria.ID.TileID.Sets.Ore[tile.type])
                            {
                                int type = tile.type;
                                WorldGen.KillTile((int)(i), (int)(j), false, false, false);

                                if (Main.netMode == NetmodeID.MultiplayerClient) //update if in mp
                                {
                                    WorldGen.SquareTileFrame(i, j, true); //Updates Area
                                    NetMessage.SendData(MessageID.TileChange, -1, -1, null, 2, (float)i, (float)j, 0f, 0, 0, 0);
                                }

                                if (player.SH().DropOresTwice) //chance to drop 2 ores
                                {
                                    WorldGen.PlaceTile(i, j, type);
                                    WorldGen.KillTile((int)(i), (int)(j), false, false, false);

                                    if (Main.netMode == NetmodeID.MultiplayerClient)
                                    {
                                        WorldGen.SquareTileFrame(i, j, true); //Updates Area
                                        NetMessage.SendData(MessageID.TileChange, -1, -1, null, 2, (float)i, (float)j, 0f, 0, 0, 0);
                                    }
                                }
                            }
                            else
                            {
                                if (!TileID.Sets.BasicChest[Main.tile[i, j - 1].type] && !TileLoader.IsDresser(Main.tile[i, j - 1].type) && Main.tile[i, j - 1].type != 26)
                                {
                                    tile.ClearTile();
                                    tile.active(false);

                                    if (Main.netMode == NetmodeID.MultiplayerClient)
                                    {
                                        WorldGen.SquareTileFrame(i, j, true); //Updates Area
                                        NetMessage.SendData(MessageID.TileChange, -1, -1, null, 2, (float)i, (float)j, 0f, 0, 0, 0);
                                    }
                                }

                                if (tile.liquid == Tile.Liquid_Water || tile.liquid == Tile.Liquid_Lava || tile.liquid == Tile.Liquid_Honey)
                                {
                                    WorldGen.SquareTileFrame(i, j, true);
                                }

                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Cycles through every npc and player, checking the distance, and deals damage accordingly
        /// Damage is not dealt if Blast Shielding is equipped
        /// </summary>
        public virtual void ExplosionDamage()
        {
            foreach (NPC npc in Main.npc)
            {
                float dist = Vector2.Distance(npc.Center, base.projectile.Center);
                if (dist / 16f <= (float)this.radius)
                {
                    int dir = (dist > 0f) ? 1 : -1;
                    if (!GlobalMethods.DamageReducedNps.Contains(npc.type))
                    {
                        npc.StrikeNPC(base.projectile.damage, base.projectile.knockBack, dir, this.crit, false, false);
                    }
                    else
                    {
                        npc.StrikeNPC(base.projectile.damage - (int)((float)base.projectile.damage * 0.5f), base.projectile.knockBack, dir, this.crit, false, false);
                    }
                }
            }
            foreach (Player player in Main.player)
            {
                if (player == null || player.whoAmI == 255 || !player.active)
                {
                    return;
                }
                if (this.CanHitPlayer(player))
                {
                    float dist2 = Vector2.Distance(player.Center, base.projectile.Center);
                    int dir2 = (dist2 > 0f) ? 1 : -1;
                    if (dist2 / 16f <= (float)this.radius && Main.netMode == 0 && this.InflictDamageSelf)
                    {
                        player.Hurt(PlayerDeathReason.ByProjectile(player.whoAmI, base.projectile.whoAmI), (int)((double)base.projectile.damage * (this.crit ? 1.5 : 1.0)), dir2, false, false, false, -1);
                        player.hurtCooldowns[0] += 15;
                    }
                    else if (Main.netMode != 1 && dist2 / 16f <= (float)this.radius && player.whoAmI == base.projectile.owner && this.InflictDamageSelf)
                    {
                        NetMessage.SendPlayerHurt(base.projectile.owner, PlayerDeathReason.ByProjectile(player.whoAmI, base.projectile.whoAmI), (int)((double)base.projectile.damage * (this.crit ? 1.5 : 1.0)), dir2, this.crit, true, 0, -1, -1);
                    }
                }
            }
        }
        #endregion
    }
}