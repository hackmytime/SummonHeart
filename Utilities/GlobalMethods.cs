using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace SummonHeart.Utilities
{
    /// <summary>
    /// This class contains global variables and functions that are used by all other classes
    /// </summary>
    internal class GlobalMethods
    {
        //========================| List of npcs for damage reduction |===============================================\\
        /// <summary>
        /// List of npcs that get damage reduction from explosives by 50% if its in expert mode
        /// </summary>
        public static List<int> DamageReducedNps = new List<int>
        {
            NPCID.EaterofWorldsBody,
            NPCID.EaterofWorldsHead,
            NPCID.EaterofWorldsTail,
            NPCID.TheDestroyer,
            NPCID.TheDestroyerBody,
            NPCID.TheDestroyerTail

        };

        public static void ExplosionDust(int Radius, Vector2 Center, Color color = default(Color), Color cloudColor = default(Color), int type = 1, Vector2 Direction = default(Vector2), bool shake = true, int dustType = 6, ArmorShaderData shader = null)
        {
            if (!new List<int>
            {
                1,
                2,
                3,
                4
            }.Contains(type))
            {
                type = 1;
            }
            switch (type)
            {
                case 1:
                    GlobalMethods.Type1Dust(Radius, Center, color, cloudColor, dustType);
                    break;
                case 2:
                    GlobalMethods.Type2Dust(Radius, Center, color, cloudColor, Direction);
                    break;
                case 3:
                    GlobalMethods.Type3Dust(Radius, Center, shader, color, cloudColor, dustType);
                    break;
            }
            Lighting.AddLight(Center, new Vector3((float)Radius / 2.3f, (float)Radius / 2.3f, (float)Radius / 2.3f));
            Lighting.maxX = Radius / 10;
            Lighting.maxY = Radius / 10;
        }

        private static void Type1Dust(int Radius, Vector2 Center, Color Color = default, Color cloudColor = default, int DustType = 6)
        {
            int dustAmount = (int)(Radius * 7);
            if (dustAmount > 40) dustAmount = 40;
            float scale = (100f / Radius) + (Radius / 100f);
            scale = scale / 10 + (Radius / 4);
            Vector2 startpoint = new Vector2(0f, -1f);
            startpoint *= 10; //How fast they all shoot out
            startpoint = startpoint.RotatedByRandom(MathHelper.Pi); //circle
            Vector2 fastBlast = startpoint;

            Vector2 startpoint2 = new Vector2(0f, -1f);
            startpoint2 *= 15 + (Radius / 6);
            startpoint2 = startpoint2.RotatedByRandom(MathHelper.Pi);

            //SPARKEL------------------------------------------------------------------------------------------------------------
            for (int i = 0; i < dustAmount + Radius; i++) //Main large outward part
            {
                Dust dust = Dust.NewDustPerfect(Center, DustType, startpoint, newColor: Color, Scale: scale);
                Dust dustTrail = Dust.NewDustPerfect(Center, DustType, startpoint, newColor: Color, Scale: scale / 1.9f);
                dust.noGravity = true;
                dustTrail.noGravity = true;
                dust.velocity *= Main.rand.NextFloat((Radius / 10f) + 1);
                dustTrail.velocity *= Main.rand.NextFloat((Radius / 10f) + .5f);
                dust.velocity.SafeNormalize(Center);

                startpoint = startpoint.RotatedBy(MathHelper.ToRadians(360 / dustAmount));
                dust.fadeIn = .8f;
                dustTrail.fadeIn = .4f;

                dust.velocity.Y -= 3f * 0.5f;
                dustTrail.velocity.Y -= 3f * 0.5f;
            }

            //smaller faster circle
            for (int i = 0; i < dustAmount + (Radius / 2); i++)
            {
                Dust dust;
                if (i % 2 == 0)
                {
                    dust = Dust.NewDustPerfect(Center, DustType, fastBlast, newColor: Color, Scale: scale / 3);
                }
                else
                {
                    dust = Dust.NewDustPerfect(Center, DustType, new Vector2(0, -1).RotatedByRandom(MathHelper.ToRadians(360)), newColor: Color, Scale: scale / 3);
                }

                dust.noGravity = true;
                dust.velocity *= Main.rand.NextFloat(10);
                dust.fadeIn = .2f;

                fastBlast = fastBlast.RotatedBy(MathHelper.ToRadians(360 / dustAmount));
            }

            if (Radius >= 15)
            {
                //Circular sparkle
                for (int i = 0; i < dustAmount + (Radius / 2); i++)
                {
                    Dust dust;
                    Dust dust2;
                    if (i % 2 == 0)
                    {
                        dust = Dust.NewDustPerfect(Center, DustType, startpoint2, newColor: Color, Scale: scale * .8f);
                    }
                    else
                    {
                        dust = Dust.NewDustPerfect(Center, DustType, new Vector2(0, -1).RotatedByRandom(MathHelper.ToRadians(360)), newColor: Color, Scale: scale * .5f);
                        dust.velocity *= Main.rand.NextFloat(8);
                    }

                    dust2 = Dust.NewDustPerfect(Center, DustType, startpoint2, newColor: Color, Scale: scale * .5f);
                    dust2.noGravity = true;
                    dust2.velocity /= 2;

                    dust.noGravity = true;
                    dust.fadeIn = .01f;

                    startpoint2 = startpoint2.RotatedBy(MathHelper.ToRadians(360 / dustAmount - 20));
                }
            }

            //Sparkle shoot
            int num833 = 0;
            for (int num834 = 1; num834 <= (Radius / 5) + 3; num834++)
            {
                float num835 = (float)Math.PI * 2f * Main.rand.NextFloat();
                for (float num836 = 0f; num836 < 1f; num836 += 0.09090909f)
                {
                    float f = (float)Math.PI * 2f * num836 + num835;
                    Vector2 spinningpoint2 = f.ToRotationVector2();
                    spinningpoint2 *= new Vector2(1f, 0.4f);
                    spinningpoint2 = spinningpoint2.RotatedBy((float)num833 - (float)Math.PI);
                    Vector2 value30 = ((float)num833 - (float)Math.PI / 2f).ToRotationVector2();
                    Vector2 position10 = Center + value30 * 16f * 0f;
                    Dust dust53 = Dust.NewDustPerfect(position10, DustType, spinningpoint2, Scale: scale * .3f);
                    dust53.fadeIn = 1.8f;
                    dust53.noGravity = true;
                    Dust dust = dust53;
                    dust.velocity *= (float)num834 * (Main.rand.NextFloat() * 2f + 0.2f);
                    dust = dust53;
                    dust.velocity += value30 * 0.8f * num834;
                    dust = dust53;
                    dust.velocity *= 2f;
                }
            }
            //SPARKEL-------------------------------------------------------------------------------------------------------------

            //GORE---------------------------------------------------------------------------------------------------------------- (adjust for small and large explosions)
            for (int num837 = 1; num837 <= (Radius / 5) + 5; num837++)
            {
                for (int num838 = -1; num838 <= 1; num838 += 2)
                {
                    for (int num839 = -1; num839 <= 1; num839 += 2)
                    {
                        Gore gore10 = Gore.NewGoreDirect(Center, Vector2.Zero, Main.rand.Next(61, 64), scale * .5f);
                        Gore gore = gore10;
                        gore.velocity *= (float)num837 / 3f + (Radius / 8);
                        gore = gore10;
                        gore.velocity += new Vector2(num838, num839);
                    }
                }
            }
            //GORE----------------------------------------------------------------------------------------------------------------

            //Black Smoke---------------------------------------------------------------------------------------------------------------------------
            float num830 = 3f;
            for (int num831 = 0; num831 < (Radius / 2) + 10; num831++)
            {
                Dust dust51 = Dust.NewDustDirect(Center, 1, 1, 31, 0f, 0f, 100, cloudColor, scale * .8f);
                Dust dust = dust51;
                dust.velocity *= 2f + (float)Main.rand.Next(Radius / 3) * 0.1f;
                dust51.velocity.Y -= num830 * 0.5f;
                dust51.color = Color.Black * .9f;
                if (Main.rand.Next(2) == 0)
                {
                    dust51.scale = 0.5f + Radius / 10;
                    dust51.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    dust51.color = Color.Black * 0.8f;
                }
            }

            //Black Smoke---------------------------------------------------------------------------------------------------------------------------
            
        }


        //============================================================================\\

        /// <summary>
        /// This function determines whether or not to break a tile. Overrides: -1 = Destroy anything, -2 = Don't break anything
        /// </summary>
        /// <param name="tileId"> The Id of the tile </param>
        /// <param name="pickPower"> pick power of the bomb </param>
        public static bool CanBreakTile(int tileId, int pickPower)
        {
            // Dynamic mod tile functionality at the bottom
            if (pickPower == -1)
                return true; // Override so an item can be set to ignore pickaxe power and destory everything
            if (pickPower <= -2)
                return false; // Override so an item can be set to not damage anything ever also catches invalid garbage
            if (tileId == TileID.Ebonstone || tileId == TileID.Crimstone)
            {
                return true;
            }
            if (tileId < 470)
            {
                // this is for all blocks which can be destroyed by any pickaxe
                if (Main.tileNoFail[tileId])
                {
                    return true;
                }

                if (tileId == TileID.DefendersForge || tileId == TileID.Containers || tileId == TileID.Containers2 || tileId == TileID.DemonAltar || tileId == TileID.FakeContainers || tileId == TileID.TrashCan || tileId == TileID.Dressers)
                {
                    return false;
                }

                if (tileId == TileID.DesertFossil && pickPower < 65)
                {
                    return false;
                }

                // Meteorite (Power 50)
                if (tileId == 37 && pickPower < 50)
                {
                    return false;
                }

                // Demonite & Crimtane Ores (Power 55)
                if ((tileId == TileID.Demonite || tileId == TileID.Crimtane) && pickPower < 55)
                {
                    return false;
                }

                // Obsidian & Ebonstone Hellstone Pearlstone and Crimstone Blocks (Power 65)
                if ((tileId == TileID.Obsidian || tileId == TileID.Ebonstone || tileId == TileID.Hellstone || tileId == TileID.Pearlstone || tileId == TileID.Crimstone) &&
                    pickPower < 65)
                {
                    return false;
                }

                // Dungeon Bricks (Power 65)
                // Separate from Obsidian block to allow for future functionality to better reflect base game mechanics
                if ((tileId == TileID.BlueDungeonBrick || tileId == TileID.GreenDungeonBrick || tileId == TileID.PinkDungeonBrick)
                    && pickPower < 65)
                {
                    return false;
                }

                // Cobalt & Palladium (Power 100)
                if ((tileId == TileID.Cobalt || tileId == TileID.Palladium)
                    && pickPower < 100)
                {
                    return false;
                }

                // Mythril & Orichalcum (Power 110)
                if ((tileId == TileID.Mythril || tileId == TileID.Orichalcum)
                    && pickPower < 110)
                {
                    return false;
                }

                // Adamantite & Titanium (Power 150)
                if ((tileId == TileID.Adamantite || tileId == TileID.Titanium)
                    && pickPower < 150)
                {
                    return false;
                }

                // Chlorophyte Ore (Power 200)
                if (tileId == TileID.Chlorophyte
                    && pickPower < 200)
                {
                    return false;
                }

                // Lihzahrd Brick (Power 210) todo add additional checks for Lihzahrd traps and the locked temple door
                if ((tileId == TileID.LihzahrdBrick || tileId == TileID.LihzahrdAltar || tileId == TileID.Traps)
                    && pickPower < 210)
                {
                    return false;
                }
            }
            // If the tile is modded, will need updating when tml is updated
            if (tileId > 469)
            {
                int tileResistance = GetModTile(tileId).minPick;
                if (tileResistance <= pickPower) return true;
                return false;
            }
            return true;    // Catch for anything which slipped through, defaults to true
        }

        //Same as type one but for rockets and wont go through the ground
        private static void Type2Dust(int Radius, Vector2 Center, Color color = default, Color cloudColor = default, Vector2 Direction = default)
        {
            int dustAmount = Radius * 7;
            if (dustAmount > 40) dustAmount = 40;
            float scale = 100f / Radius + Radius / 100f;
            scale = scale / 10 + Radius / 4;

            Vector2 startpoint = new Vector2(0, -1); //Change to the direction of the rocket
            startpoint *= 6; //How fast they all shoot out
            startpoint = startpoint.RotatedByRandom(MathHelper.Pi); //circle
            Vector2 fastBlast = startpoint;

            Vector2 startpoint2 = new Vector2(0, -1);
            startpoint2 *= 8 + Radius / 6;
            startpoint2 = startpoint2.RotatedByRandom(MathHelper.Pi);

            //SPARKEL------------------------------------------------------------------------------------------------------------
            //for (int i = 0; i < dustAmount + Radius; i++) //Main large outward part
            //{
            //    Dust dust = Dust.NewDustPerfect(Center, 6, startpoint, newColor: color, Scale: scale);
            //    Dust dustTrail = Dust.NewDustPerfect(Center, 6, startpoint, newColor: color, Scale: scale / 1.9f);

            //    dust.noGravity = true;
            //    dustTrail.noGravity = true;
            //    dust.velocity *= Main.rand.NextFloat((Radius / 10f) + 1);
            //    dustTrail.velocity *= Main.rand.NextFloat((Radius / 10f) + .5f);
            //    dust.velocity.SafeNormalize(Center);

            //    startpoint = startpoint.RotatedBy(MathHelper.ToRadians(360 / dustAmount));
            //    dust.fadeIn = .8f;
            //    dustTrail.fadeIn = .4f;

            //    dust.velocity.Y -= 3f * 0.5f;
            //    dustTrail.velocity.Y -= 3f * 0.5f;
            //}

            //smaller faster circle
            for (int i = 0; i < dustAmount + Radius / 2; i++)
            {
                Dust dust;
                if (i % 2 == 0)
                {
                    dust = Dust.NewDustPerfect(Center, 6, fastBlast, newColor: color, Scale: scale / 3);
                }
                else
                {
                    dust = Dust.NewDustPerfect(Center, 6, new Vector2(0, -1).RotatedByRandom(MathHelper.ToRadians(360)), newColor: color, Scale: scale / 3);
                }

                dust.noGravity = true;
                dust.velocity *= Main.rand.NextFloat(10);
                dust.fadeIn = .2f;

                fastBlast = fastBlast.RotatedBy(MathHelper.ToRadians(360 / dustAmount));
            }

            //Circular sparkle
            for (int i = 0; i < dustAmount + Radius / 2; i++)
            {
                Dust dust;
                Dust dust2;
                if (i % 2 == 0)
                {
                    dust = Dust.NewDustPerfect(Center, 6, startpoint2, newColor: color, Scale: scale * .8f);
                }
                else
                {
                    dust = Dust.NewDustPerfect(Center, 6, new Vector2(0, -1).RotatedByRandom(MathHelper.ToRadians(360)), newColor: color, Scale: scale * .3f);
                    dust.velocity *= Main.rand.NextFloat(8);
                }

                dust2 = Dust.NewDustPerfect(Center, 6, startpoint2, newColor: color, Scale: scale * .5f);
                dust2.noGravity = true;
                dust2.velocity /= 3;

                dust.noGravity = true;
                dust.fadeIn = .01f;
                dust.velocity /= 3;

                startpoint2 = startpoint2.RotatedBy(MathHelper.ToRadians(360 / dustAmount - 20));
            }


            //Sparkle shoot
            for (int num834 = 1; num834 <= Radius / 5 + 4; num834++)
            {
                for (float num836 = 0f; num836 < 1f; num836 += 0.09090909f)
                {
                    Vector2 dir = -Direction.RotatedByRandom(1.6);
                    Dust dust53 = Dust.NewDustPerfect(Center, 6, dir / 15, Scale: scale * .3f);
                    dust53.fadeIn = 1.8f;
                    dust53.noGravity = true;
                    Dust dust = dust53;
                    dust.velocity *= num834 * (Main.rand.NextFloat() * 2f + 0.2f);
                    dust = dust53;
                    dust.velocity *= 2f;

                }
            }
            //SPARKEL-------------------------------------------------------------------------------------------------------------

            //GORE---------------------------------------------------------------------------------------------------------------- (adjusted for small and large explosions)
            for (int num837 = 1; num837 <= Radius / 5 + 2; num837++)
            {
                for (int num838 = -1; num838 <= 1; num838 += 2)
                {
                    for (int num839 = -1; num839 <= 1; num839 += 2)
                    {
                        Vector2 dir = -Direction.RotatedByRandom(1.2);
                        Gore gore10 = Gore.NewGoreDirect(Center, dir / 15, Main.rand.Next(61, 64), scale * .3f);
                        Gore gore = gore10;
                        gore.velocity *= num837 / 3f + Radius / 8;
                        gore = gore10;
                        gore.velocity += new Vector2(num838, num839);
                    }
                }
            }
            //GORE----------------------------------------------------------------------------------------------------------------

            //Black Smoke---------------------------------------------------------------------------------------------------------------------------
            float num830 = 3f;
            for (int num831 = 0; num831 < Radius / 5 + 6; num831++)
            {
                Vector2 dir = -Direction.RotatedByRandom(1.2);
                dir = dir / 13;
                Dust dust51 = Dust.NewDustDirect(Center, 1, 1, 31, dir.X, dir.Y, 100, default, scale * .8f);
                Dust dust = dust51;
                dust.velocity *= 2f + Main.rand.Next(Radius / 3) * 0.1f;
                dust51.velocity.Y -= num830 * 0.5f;
                dust51.color = Color.Black * 0.9f;
                if (Main.rand.Next(2) == 0)
                {
                    dust51.scale = 0.5f + Radius / 10;
                    dust51.fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    dust51.color = Color.Black * 0.8f;
                }
            }

            //Black Smoke---------------------------------------------------------------------------------------------------------------------------


            //Debris---------------------------------------------------------------------------------------------------------------------------------------------------
            //for (int i = 0; i < 5 + Main.rand.Next(Radius / 3); i++)
            //{
            //    Vector2 explosionTop;

            //    if (i % 2 == 0) //Right side
            //    {
            //        explosionTop = RandomVector2(-MathHelper.Pi / 4, 0); //choose a random angle based off of 180
            //    }
            //    else //left side
            //    {
            //        explosionTop = RandomVector2(-MathHelper.Pi, (7 * MathHelper.Pi) / 4); //choose a random angle based off of 180

            //    }

            //    float speed = (Radius / 3) + Main.rand.NextFloat(2f);

            //    Dust dust = Dust.NewDustPerfect(Center, DustType<DebrisDust>(), explosionTop * speed, newColor: default(Color), Scale: scale); //starting color
            //    dust.noGravity = false;
            //}
            //Debris---------------------------------------------------------------------------------------------------------------------------------------------------
        }

        //Used for special bombs that need a shader
        private static void Type3Dust(int Radius, Vector2 Center, ArmorShaderData Shader, Color Color = default, Color cloudColor = default, int DustType = 6)
        {
            int dustAmount = Radius * 7;
            if (dustAmount > 40) dustAmount = 40;
            float scale = 100f / Radius + Radius / 100f;
            scale = scale / 10 + Radius / 4;
            Vector2 startpoint = new Vector2(0f, -1f);
            startpoint *= 10; //How fast they all shoot out
            startpoint = startpoint.RotatedByRandom(MathHelper.Pi); //circle
            Vector2 fastBlast = startpoint;

            Vector2 startpoint2 = new Vector2(0f, -1f);
            startpoint2 *= 15 + Radius / 6;
            startpoint2 = startpoint2.RotatedByRandom(MathHelper.Pi);

            //SPARKEL------------------------------------------------------------------------------------------------------------
            for (int i = 0; i < dustAmount + Radius; i++) //Main large outward part
            {
                Dust dust = Dust.NewDustPerfect(Center, DustType, startpoint, newColor: Color, Scale: scale);
                Dust dustTrail = Dust.NewDustPerfect(Center, DustType, startpoint, newColor: Color, Scale: scale / 1.9f);
                dust.shader = Shader;
                dustTrail.shader = Shader;

                dust.noGravity = true;
                dustTrail.noGravity = true;
                dust.velocity *= Main.rand.NextFloat(Radius / 10f + 1);
                dustTrail.velocity *= Main.rand.NextFloat(Radius / 10f + .5f);
                dust.velocity.SafeNormalize(Center);

                startpoint = startpoint.RotatedBy(MathHelper.ToRadians(360 / dustAmount));
                dust.fadeIn = .8f;
                dustTrail.fadeIn = .4f;

                dust.velocity.Y -= 3f * 0.5f;
                dustTrail.velocity.Y -= 3f * 0.5f;
            }

            //smaller faster circle
            for (int i = 0; i < dustAmount + Radius / 2; i++)
            {
                Dust dust;
                if (i % 2 == 0)
                {
                    dust = Dust.NewDustPerfect(Center, DustType, fastBlast, newColor: Color, Scale: scale / 3);
                }
                else
                {
                    dust = Dust.NewDustPerfect(Center, DustType, new Vector2(0, -1).RotatedByRandom(MathHelper.ToRadians(360)), newColor: Color, Scale: scale / 3);
                }
                dust.shader = Shader;
                dust.noGravity = true;
                dust.velocity *= Main.rand.NextFloat(10);
                dust.fadeIn = .2f;

                fastBlast = fastBlast.RotatedBy(MathHelper.ToRadians(360 / dustAmount));
            }

            if (Radius >= 15)
            {
                //Circular sparkle
                for (int i = 0; i < dustAmount + Radius / 2; i++)
                {
                    Dust dust;
                    Dust dust2;
                    if (i % 2 == 0)
                    {
                        dust = Dust.NewDustPerfect(Center, DustType, startpoint2, newColor: Color, Scale: scale * .8f);
                    }
                    else
                    {
                        dust = Dust.NewDustPerfect(Center, DustType, new Vector2(0, -1).RotatedByRandom(MathHelper.ToRadians(360)), newColor: Color, Scale: scale * .5f);
                        dust.velocity *= Main.rand.NextFloat(8);
                    }
                    dust.shader = Shader;
                    dust2 = Dust.NewDustPerfect(Center, DustType, startpoint2, newColor: Color, Scale: scale * .5f);
                    dust2.shader = Shader;
                    dust2.noGravity = true;
                    dust2.velocity /= 2;

                    dust.noGravity = true;
                    dust.fadeIn = .01f;

                    startpoint2 = startpoint2.RotatedBy(MathHelper.ToRadians(360 / dustAmount - 20));
                }
            }

            //Sparkle shoot
            int num833 = 0;
            for (int num834 = 1; num834 <= Radius / 5 + 3; num834++)
            {
                float num835 = (float)Math.PI * 2f * Main.rand.NextFloat();
                for (float num836 = 0f; num836 < 1f; num836 += 0.09090909f)
                {
                    float f = (float)Math.PI * 2f * num836 + num835;
                    Vector2 spinningpoint2 = f.ToRotationVector2();
                    spinningpoint2 *= new Vector2(1f, 0.4f);
                    spinningpoint2 = spinningpoint2.RotatedBy(num833 - (float)Math.PI);
                    Vector2 value30 = (num833 - (float)Math.PI / 2f).ToRotationVector2();
                    Vector2 position10 = Center + value30 * 16f * 0f;
                    Dust dust53 = Dust.NewDustPerfect(position10, DustType, spinningpoint2, Scale: scale * .3f);
                    dust53.shader = Shader;
                    dust53.fadeIn = 1.8f;
                    dust53.noGravity = true;
                    Dust dust = dust53;
                    dust.velocity *= num834 * (Main.rand.NextFloat() * 2f + 0.2f);
                    dust = dust53;
                    dust.velocity += value30 * 0.8f * num834;
                    dust = dust53;
                    dust.velocity *= 2f;
                }
            }
            //SPARKEL-------------------------------------------------------------------------------------------------------------

            //GORE---------------------------------------------------------------------------------------------------------------- (adjust for small and large explosions)
            for (int num837 = 1; num837 <= Radius / 5 + 5; num837++)
            {
                for (int num838 = -1; num838 <= 1; num838 += 2)
                {
                    for (int num839 = -1; num839 <= 1; num839 += 2)
                    {
                        Gore gore10 = Gore.NewGoreDirect(Center, Vector2.Zero, Main.rand.Next(61, 64), scale * .5f);
                        Gore gore = gore10;
                        gore.velocity *= num837 / 3f + Radius / 8;
                        gore = gore10;
                        gore.velocity += new Vector2(num838, num839);
                    }
                }
            }
            //GORE----------------------------------------------------------------------------------------------------------------

            //Black Smoke---------------------------------------------------------------------------------------------------------------------------
            float num830 = 3f;
            for (int num831 = 0; num831 < Radius / 2 + 10; num831++)
            {
                Dust dust51 = Dust.NewDustDirect(Center, 1, 1, 31, 0f, 0f, 100, cloudColor, scale * .8f);
                Dust dust = dust51;
                dust.velocity *= 2f + Main.rand.Next(Radius / 3) * 0.1f;
                dust51.velocity.Y -= num830 * 0.5f;
                dust51.color = Color.Black * .9f;
                if (Main.rand.Next(2) == 0)
                {
                    dust51.scale = 0.5f + Radius / 10;
                    dust51.fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    dust51.color = Color.Black * 0.8f;
                }
            }

            //Black Smoke---------------------------------------------------------------------------------------------------------------------------


            //Debris---------------------------------------------------------------------------------------------------------------------------------------------------
            //for (int i = 0; i < 5 + Main.rand.Next(Radius / 3); i++)
            //{
            //    Vector2 explosionTop;

            //    if (i % 2 == 0) //Right side
            //    {
            //        explosionTop = RandomVector2(-MathHelper.Pi / 4, 0); //choose a random angle based off of 180
            //    }
            //    else //left side
            //    {
            //        explosionTop = RandomVector2(-MathHelper.Pi, (7 * MathHelper.Pi) / 4); //choose a random angle based off of 180

            //    }

            //    float speed = (Radius / 3) + Main.rand.NextFloat(2f);

            //    Dust dust = Dust.NewDustPerfect(Center, DustType<DebrisDust>(), explosionTop * speed, newColor: default(Color), Scale: scale); //starting color
            //    dust.shader = Shader;
            //    dust.noGravity = false;
            //}
            //Debris---------------------------------------------------------------------------------------------------------------------------------------------------

        }


        /// <summary>
        /// This function converts a vector2 to an angle and puts it back as a vector2 at a random point between the two values.
        /// </summary>
        /// <param name="angle"> Starting angle </param>
        /// <param name="angleMin"> Minimum angle </param>
        public static Vector2 RandomVector2(float angle, float angleMin)
        {
            float random = Main.rand.NextFloat() * angle + angleMin;
            return new Vector2((float)Math.Cos(random), (float)Math.Sin(random));
        }
    }
}