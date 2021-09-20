using Microsoft.Xna.Framework;
using SummonHeart.Extensions;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static SummonHeart.Utilities.GlobalMethods;

namespace SummonHeart.Projectiles.Range.Tools
{
    public class TheLevelerProjectile : ExplosiveProjectile
    {
        protected override string explodeSoundsLoc => "Sounds/Custom/Explosives/The_Leveler_";
        protected override string goreFileLoc => "Gores/Explosives/the-leveler_gore";

        internal static bool CanBreakWalls;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Leveler");
            //Tooltip.SetDefault("");
        }

        public override void SafeSetDefaults()
        {
            pickPower = 64;
            radius = 20;
            projectile.tileCollide = true; //checks to see if the projectile can go through tiles
            projectile.width = 10;   //This defines the hitbox width
            projectile.height = 32; //This defines the hitbox height
            projectile.aiStyle = 16;  //How the projectile works, 16 is the aistyle Used for: Grenades, Dynamite, Bombs, Sticky Bomb.
            projectile.friendly = true; //Tells the game whether it is friendly to players/friendly npcs or not
            projectile.penetrate = -1; //Tells the game how many enemies it can hit before being destroyed
            projectile.timeLeft = 120; //The amount of time the projectile is alive for
            projectile.damage = 0;

            drawOffsetX = -15;
            drawOriginOffsetY = -15;
            explodeSounds = new LegacySoundStyle[4];
            for (int num = 1; num <= explodeSounds.Length; num++)
            {
                explodeSounds[num - 1] = mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, explodeSoundsLoc + num);
            }
        }

        public override void AI()
        {
            projectile.rotation = 0;
        }

        public override bool OnTileCollide(Vector2 old)
        {
            projectile.Kill();
            return true;
        }

        public override void Kill(int timeLeft)
        {
            //Create Bomb Sound
            Main.PlaySound(explodeSounds[Main.rand.Next(explodeSounds.Length)], (int)projectile.Center.X, (int)projectile.Center.Y);

            //Create Bomb Damage
            ExplosionDamage();

            //Create Bomb Explosion
            Explosion();
        }

        public override void Explosion()
        {
            Player player = Main.player[projectile.owner];
            pickPower = player.getMaxPickPowerInInventory();
            Vector2 position = projectile.Center;

            int x = 0;
            int y = 0;

            int width = 100; //Explosion Width
            int height = 10; //Explosion Height

            for (y = height - 1; y >= 0; y--)
            {
                for (x = -width; x < width; x++)
                {
                    int xPosition = (int)(x + position.X / 16.0f);
                    int yPosition = (int)(-y + position.Y / 16.0f);

                    if (!WorldGen.InWorld(xPosition, yPosition)) continue;

                    Tile tile = Framing.GetTileSafely(xPosition, yPosition);

                    if (WorldGen.InWorld(xPosition, yPosition) && tile.active()) //Circle
                    {
                        if (!CanBreakTile(tile.type, pickPower)) //Unbreakable CheckForUnbreakableTiles(tile) ||
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

                            if (tile.liquid == Tile.Liquid_Water || tile.liquid == Tile.Liquid_Lava || tile.liquid == Tile.Liquid_Honey)
                            {
                                WorldGen.SquareTileFrame(xPosition, yPosition, true);
                            }

                            if (Main.netMode == NetmodeID.MultiplayerClient)
                            {
                                WorldGen.SquareTileFrame(xPosition, yPosition, true); //Updates Area
                                NetMessage.SendData(MessageID.TileChange, -1, -1, null, 2, xPosition, yPosition, 0f, 0, 0, 0);
                            }
                        }

                        if (true)
                        {
                            WorldGen.KillWall(xPosition, yPosition, false);
                            WorldGen.KillWall(xPosition + 1, yPosition + 1, false); //get the last bit
                        }
                    }
                }
                width++; //Increments width to make stairs on each end
            }
        }
    }
}