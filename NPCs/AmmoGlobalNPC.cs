using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.NPCs
{
    public class AmmoGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override void ResetEffects(NPC npc)
        {
            apMarked = false;
            apStuck = false;
            apCold = false;
            apDrugged = false;
            apDruggedShouldTint = false;
            apClouded = false;
            apCactus = false;
            apSlime = false;
        }

        public override void SetDefaults(NPC npc)
        {
            apMarked = false;
            apAlreadyGrantedMulti = false;
            apStuck = false;
            apCold = false;
            apCactus = false;
        }

        public override void HitEffect(NPC npc, int hitDirection, double damage)
        {
            if (apMarked && !apAlreadyGrantedMulti)
            {
                apAlreadyGrantedMulti = true;
                npc.takenDamageMultiplier += 0.15f;
                npc.netUpdate = true;
            }
        }

        public override void NPCLoot(NPC npc)
        {
            if (npc.boss && !Main.expertMode)
            {
                if (Main.hardMode)
                {
                    int id = Item.NewItem(npc.getRect(), mod.ItemType("AmmoBoxPlus"), 1, false, 0, false, false);
                    if (Main.netMode != 0)
                    {
                        NetMessage.SendData(21, -1, -1, null, id, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
                else
                {
                    int id2 = Item.NewItem(npc.getRect(), mod.ItemType("AmmoBox"), 1, false, 0, false, false);
                    if (Main.netMode != 0)
                    {
                        NetMessage.SendData(21, -1, -1, null, id2, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
            }
            if (Main.rand.Next(10) == 0 && apExtraHeartTick > 0)
            {
                int id3 = Item.NewItem(npc.getRect(), 58, 1, false, 0, false, false);
                int id4 = Item.NewItem(npc.getRect(), 58, 1, false, 0, false, false);
                if (Main.netMode != 0)
                {
                    NetMessage.SendData(21, -1, -1, null, id3, 0f, 0f, 0f, 0, 0, 0);
                    NetMessage.SendData(21, -1, -1, null, id4, 0f, 0f, 0f, 0, 0, 0);
                }
            }
            if (Main.rand.Next(10) == 0 && apExtraManaTick > 0)
            {
                int id5 = Item.NewItem(npc.getRect(), 184, 1, false, 0, false, false);
                int id6 = Item.NewItem(npc.getRect(), 184, 1, false, 0, false, false);
                if (Main.netMode != 0)
                {
                    NetMessage.SendData(21, -1, -1, null, id5, 0f, 0f, 0f, 0, 0, 0);
                    NetMessage.SendData(21, -1, -1, null, id6, 0f, 0f, 0f, 0, 0, 0);
                }
            }
        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            if (npc.realLife != -1 && npc.realLife != npc.whoAmI)
            {
                return true;
            }
            if (apMarked || apStuck)
            {
                float elSizeX = 32f;
                float elSizeY = 56f;
                float spacer = npc.type == 551 ? 60 : 36;
                Vector2 distToNpc = npc.Top - Main.screenPosition;
                if (apMarked && !apStuck)
                {
                    Vector2 finalPos = distToNpc - new Vector2(0.5f * elSizeX, elSizeY + spacer);
                    spriteBatch.Draw(mod.GetTexture("UI/Marked"), finalPos, new Color(255, 255, 255, 255));
                }
                else if (!apMarked && apStuck)
                {
                    Vector2 finalPos = distToNpc - new Vector2(0.5f * elSizeX, elSizeY + spacer);
                    spriteBatch.Draw(mod.GetTexture("UI/Iced"), finalPos, new Color(255, 255, 255, 255));
                }
                else if (apMarked && apStuck)
                {
                    Vector2 finalPos = distToNpc - new Vector2(0.5f * elSizeX, elSizeY + spacer);
                    spriteBatch.Draw(mod.GetTexture("UI/FrozenMark"), finalPos, new Color(255, 255, 255, 255));
                }
            }
            if (apDrugged)
            {
                int radius = Math.Max(npc.width, npc.height) * 2;
                for (int i = 0; i < 360; i++)
                {
                    if (i < 90 || i > 180 && i < 270)
                    {
                        double x = radius * Math.Cos(i * 3.141592653589793 / 180.0);
                        double y = radius * Math.Sin(i * 3.141592653589793 / 180.0);
                        Dust.NewDust(new Vector2((float)x, (float)y) + npc.Center, 8, 8, 260, 0f, 0f, 0, Color.Blue, 1f);
                    }
                    else if (i > 90 && i < 180 || i > 270)
                    {
                        double x2 = radius * Math.Sin(i * 3.141592653589793 / 180.0);
                        double y2 = radius * Math.Cos(i * 3.141592653589793 / 180.0);
                        Dust.NewDust(new Vector2((float)x2, (float)y2) + npc.Center, 8, 8, 260, 0f, 0f, 0, Color.Blue, 1f);
                    }
                }
            }
            if (apClouded)
            {
                Dust.NewDustDirect(npc.Center, 2, 2, 32, 0f, 0f, 0, default, 1f);
            }
            return true;
        }

        public override bool CheckDead(NPC npc)
        {
            if (npc.GetGlobalNPC<AmmoGlobalNPC>().apStuck && npc.realLife != -1)
            {
                int index = 0;
                foreach (NPC i in Main.npc)
                {
                    if (i.whoAmI == npc.realLife || i.realLife == npc.realLife)
                    {
                        i.StrikeNPC(i.life, 0f, 1, false, true, false);
                    }
                    index++;
                }
                return true;
            }
            return true;
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (apStuck || apStuck && apCold)
            {
                drawColor.B = byte.MaxValue;
                drawColor.R = 140;
            }
            else if (apCold)
            {
                drawColor.B = byte.MaxValue;
                drawColor.R = 200;
            }
            if (apDruggedShouldTint)
            {
                drawColor = Color.Purple;
            }
            if (apClouded)
            {
                drawColor = Color.Yellow;
            }
        }

        public override bool PreAI(NPC npc)
        {
            apYiYaTick = apYiYaTick > 0 ? apYiYaTick - 1 : 0;
            apDruggedCooldown = apDruggedCooldown > 0 ? apDruggedCooldown - 1 : 0;
            apDruggedTick = apDruggedTick > 0 ? apDruggedTick - 1 : 0;
            if (apDrugged && apDruggedTick == 0)
            {
                int index = 0;
                int appliedCount = 0;
                foreach (NPC i in Main.npc)
                {
                    float num = Math.Abs(npc.Center.X - i.Center.X);
                    float b = Math.Abs(npc.Center.Y - i.Center.Y);
                    double num2 = Math.Sqrt(num * num + b * b);
                    double reach = Math.Max(npc.width, npc.height) * 1.7f;
                    if (num2 < reach && i.active && !i.friendly && i.whoAmI != npc.whoAmI && !i.GetGlobalNPC<AmmoGlobalNPC>().apDrugged && !i.dontTakeDamage)
                    {
                        if (i.realLife == npc.whoAmI)
                        {
                            i.StrikeNPC(npc.damage / 8, 1f, 0, false, false, false);
                        }
                        else
                        {
                            i.StrikeNPC(npc.damage / 2, 1f, 0, false, false, false);
                        }
                        Main.npc[index].GetGlobalNPC<AmmoGlobalNPC>().apDruggedShouldTint = true;
                        Main.npc[index].netUpdate = true;
                        appliedCount++;
                    }
                    index++;
                }
                if (appliedCount > 0)
                {
                    apDruggedTick = 120;
                }
            }
            apCactusCooldown = apCactusCooldown > 0 ? apCactusCooldown - 1 : 0;
            if (apCactus)
            {
                int index2 = 0;
                foreach (NPC j in Main.npc)
                {
                    if (Collision.CheckAABBvAABBCollision(npc.Hitbox.BottomLeft(), new Vector2(npc.Hitbox.Width, npc.Hitbox.Height), j.Hitbox.BottomLeft(), new Vector2(j.Hitbox.Width, j.Hitbox.Height)) && j.active && !j.friendly && j.whoAmI != npc.whoAmI && !j.GetGlobalNPC<AmmoGlobalNPC>().apCactus && j.GetGlobalNPC<AmmoGlobalNPC>().apCactusCooldown == 0 && !j.dontTakeDamage)
                    {
                        if (j.position.X > npc.position.X)
                        {
                            j.StrikeNPC(npc.damage / 4, 9f, 1, false, false, false);
                        }
                        else
                        {
                            j.StrikeNPC(npc.damage / 4, 9f, -1, false, false, false);
                        }
                        Main.npc[index2].GetGlobalNPC<AmmoGlobalNPC>().apCactusCooldown = 300;
                        Main.npc[index2].netUpdate = true;
                    }
                    index2++;
                }
            }
            npc.velocity = apStuck ? new Vector2(0f, 0f) : npc.velocity;
            if (npc.type == 222 && apStuck)
            {
                npc.ai[0] = 2f;
                npc.ai[1] = 0f;
            }
            return !apStuck;
        }

        public override void AI(NPC npc)
        {
            apExtraManaTick = apExtraManaTick > 0 ? apExtraManaTick - 1 : 0;
            apExtraHeartTick = apExtraHeartTick > 0 ? apExtraHeartTick - 1 : 0;
            if (apSlime || apCold)
            {
                float slimeVelocityBossMulti = 0.993f;
                float slimeVelocityNpcMulti = 0.97f;
                float iceVelocityBossMulti = 0.96f;
                float iceVelocityNpcMulti = 0.93f;
                if (Main.netMode == 2)
                {
                    if (npc.type == 222 && npc.ai[0] == 0f && npc.ai[1] % 2f != 0f)
                    {
                        return;
                    }
                    if (npc.boss)
                    {
                        if (apSlime)
                        {
                            npc.velocity *= slimeVelocityBossMulti;
                        }
                        else if (apCold)
                        {
                            if (npc.type == 222)
                            {
                                npc.velocity *= 0.98f;
                            }
                            else
                            {
                                npc.velocity *= iceVelocityBossMulti;
                            }
                        }
                    }
                    else if (apSlime)
                    {
                        npc.velocity *= slimeVelocityNpcMulti;
                    }
                    else if (apCold)
                    {
                        npc.velocity *= iceVelocityNpcMulti;
                    }
                    npc.netUpdate = true;
                    ModPacket packet = mod.GetPacket(256);
                    packet.Write(6);
                    packet.Write(npc.whoAmI);
                    packet.WriteVector2(npc.velocity);
                    packet.Send(-1, -1);
                    npc.netUpdate = true;
                    npc.netUpdate2 = true;
                    return;
                }
                else if (Main.netMode == 0)
                {
                    if (npc.type == 222 && npc.ai[0] == 0f && npc.ai[1] % 2f != 0f)
                    {
                        return;
                    }
                    if (npc.boss)
                    {
                        if (apSlime)
                        {
                            npc.velocity *= slimeVelocityBossMulti;
                            return;
                        }
                        if (apCold)
                        {
                            if (npc.type == 222)
                            {
                                npc.velocity *= 0.98f;
                                return;
                            }
                            npc.velocity *= iceVelocityBossMulti;
                            return;
                        }
                    }
                    else
                    {
                        if (apSlime)
                        {
                            npc.velocity *= slimeVelocityNpcMulti;
                            return;
                        }
                        if (apCold)
                        {
                            npc.velocity *= iceVelocityNpcMulti;
                        }
                    }
                }
            }
        }
       

        internal bool apStuck;

        internal bool apMarked;

        internal bool apAlreadyGrantedMulti;

        internal bool apAlreadyDroppedOre;

        internal bool apCold;

        internal bool apStuckLimit;

        internal bool apDrugged;

        internal bool apDruggedShouldTint;

        internal int apDruggedCooldown;

        internal int apDruggedTick;

        internal bool apClouded;

        internal bool apCactus;

        internal int apCactusCooldown;

        internal bool apSlime;

        internal int apYiYaTick;

        internal bool apYing;

        internal bool apYang;

        internal int apExtraHeartTick;

        internal int apExtraManaTick;
    }
}
