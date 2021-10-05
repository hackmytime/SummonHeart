using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SummonHeart.Extensions;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static SummonHeart.Utilities.GlobalMethods;

namespace SummonHeart.Projectiles.Range.Tools
{
    public class C4Projectile2 : ExplosiveProjectile
    {
        //Variables
        protected override string explodeSoundsLoc => "Sounds/Custom/Explosives/C4_";
        protected override string goreFileLoc => "Gores/Explosives/c4_gore";
        private enum C4State
        {
            Airborne,
            Frozen,
            Primed,
            Exploding
        };
        private C4State projState = C4State.Airborne;
        // private bool freeze;
        private SummonHeartPlayer c4Owner;
        private Vector2 positionToFreeze;
        private LegacySoundStyle indicatorSound;
        private LegacySoundStyle primedSound;
        private SoundEffectInstance indicatorSoundInstance;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("C4");
        }

        public override void SafeSetDefaults()
        {
            pickPower = 70;
            radius = 2;
            projectile.tileCollide = true;
            projectile.width = 32;
            projectile.height = 32;
            projectile.aiStyle = 16;
            projectile.friendly = true;
            projectile.damage = 400;
            projectile.penetrate = -1;
            projectile.timeLeft = int.MaxValue;
            //projectile.extraUpdates = 1;
            Terraria.ModLoader.SoundType customType = Terraria.ModLoader.SoundType.Custom;
            indicatorSound = mod.GetLegacySoundSlot(customType, explodeSoundsLoc + "timer");
            primedSound = mod.GetLegacySoundSlot(customType, explodeSoundsLoc + "time_to_explode");
            if (!Main.dedServ && indicatorSound != null || primedSound != null) //Checking for nulls might fix the error
            {
                indicatorSound = indicatorSound.WithPitchVariance(0f).WithVolume(0.5f);
                primedSound = primedSound.WithPitchVariance(0f).WithVolume(0.5f);
            }
            else if (indicatorSound != null || primedSound != null)
            {
                indicatorSound = mod.GetLegacySoundSlot(customType, explodeSoundsLoc + "timer");
                primedSound = mod.GetLegacySoundSlot(customType, explodeSoundsLoc + "time_to_explode");
            }
            explodeSounds = new LegacySoundStyle[4];
            for (int num = 1; num <= explodeSounds.Length; num++)
            {
                explodeSounds[num - 1] = mod.GetLegacySoundSlot(customType, explodeSoundsLoc + "Bomb_" + num);
            }
        }

        public override bool OnTileCollide(Vector2 old)
        {
            projectile.Kill();
            return true;
        }

        public override void PostAI()
        {
            switch (projState)
            {
                case C4State.Airborne:
                    if (projectile.owner == Main.myPlayer && c4Owner == null)
                    {
                        c4Owner = Main.player[projectile.owner].GetModPlayer<SummonHeartPlayer>();
                    }
                    break;
                case C4State.Frozen:
                    projectile.position = positionToFreeze;
                    projectile.velocity = Vector2.Zero;
                    if (indicatorSoundInstance == null)
                        indicatorSoundInstance = Main.PlaySound(indicatorSound, (int)projectile.Center.X, (int)projectile.Center.Y);
                    else if (indicatorSoundInstance.State != SoundState.Playing)    // else if needed to avoid a NullReferenceException
                        indicatorSoundInstance.Play();
                    if (c4Owner != null && c4Owner.detonate)
                    {
                        projState = C4State.Primed;
                        projectile.ai[1] = 55;
                        Main.PlaySound(primedSound, (int)projectile.position.X, (int)projectile.position.Y);
                    }
                    break;
                case C4State.Primed:
                    projectile.ai[1]--;
                    if (projectile.ai[1] < 1)
                    {
                        projState = C4State.Exploding;
                    }
                    break;
                case C4State.Exploding:
                    projectile.Kill();
                    break;
            }
        }

        public override void Kill(int timeLeft)
        {
            //Create Bomb Sound
            Main.PlaySound(explodeSounds[Main.rand.Next(explodeSounds.Length)], (int)projectile.Center.X, (int)projectile.Center.Y);
            this.DustEffects(default(Color), default(Color), 1, true, 6, null);
            Explosion();
            ExplosionDamage();
        }

        public override void Explosion()
        {
            Player player = Main.player[projectile.owner];
            pickPower = player.getMaxPickPowerInInventory();
            Vector2 position = projectile.Center;
            for (int x = -radius; x <= radius; x++) //Starts on the X Axis on the left
            {
                for (int y = -radius; y <= radius; y++) //Starts on the Y Axis on the top
                {
                    int xPosition = (int)(x + position.X / 16.0f);
                    int yPosition = (int)(y + position.Y / 16.0f);

                    Tile tile = Framing.GetTileSafely(xPosition, yPosition);

                    if (WorldGen.InWorld(xPosition, yPosition) && !WorldGen.TileEmpty(xPosition, yPosition) && tile.active()) //矩形
                    {
                        ushort tileP = tile.type;
                        if (!CanBreakTile(tileP, pickPower)) //Unbreakable CheckForUnbreakableTiles(tile) ||
                        {
                        }
                        else //Breakable
                        {
                            if (!TileID.Sets.BasicChest[Main.tile[xPosition, yPosition - 1].type] && !TileLoader.IsDresser(Main.tile[xPosition, yPosition - 1].type) && Main.tile[xPosition, yPosition - 1].type != 26)
                            {
                                int i = xPosition;
                                int j = yPosition;
                                int type = tile.type;
                                WorldGen.KillTile((int)(i), (int)(j), false, false, false);

                                if (Main.netMode == NetmodeID.MultiplayerClient) //update if in mp
                                {
                                    WorldGen.SquareTileFrame(i, j, true); //Updates Area
                                    NetMessage.SendData(MessageID.TileChange, -1, -1, null, 2, (float)i, (float)j, 0f, 0, 0, 0);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}