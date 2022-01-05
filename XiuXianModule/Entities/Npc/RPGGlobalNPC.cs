using Microsoft.Xna.Framework;
using SummonHeart.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.XiuXianModule.Entities.Npc
{
    class RPGGlobalNPC : GlobalNPC
    {

        public bool StatsCreated = false;
        public bool DontCreateStats = false;
        public bool StatsFrame0 = false;
        public Dictionary<string, string> specialBuffer = new Dictionary<string, string>();
        private int level = -1;
        public int getLevel { get { return level; } }
        private int tier = 0;
        public int getTier { get { return tier; } }
        public int life = 3;
        int baseDamage = 0;
        public float dontTakeDamageTime = 0;
        public float debuffDamage = 0;

        public int getRank { get { return (int)Rank; } }

        private NPCRank Rank = NPCRank.Normal;
        public NPCModifier modifier = 0;

        string[] levelTexts = new string[] { "凡人", "炼气", "筑基", "金丹", "元婴", "化神", "合体", "渡劫", "大乘", "散仙", "金仙", "大罗金仙", "混元大罗金仙", "半圣", "圣人", "圣尊", "圣王", "无上境", "道境" };
        string[] numTexts = new string[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };

        public string GetLevelText()
        {
            if (level == 0)
                return levelTexts[0];
            int a = level / 10 + 1;
            int b = level % 10;
            if (b == 0)
            {
                a--;
                b = 10;
            }
            return levelTexts[a] + numTexts[b] + "重";
        }

        public override void SetDefaults(NPC npc)
        {
            base.SetDefaults(npc);
        }

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        private float VauraMemory = 0;

        public void ApplyCleave(NPC npc, int damage)
        {
            npc.life = Mathf.Clamp(npc.life - damage, 0, npc.lifeMax);
            Dust.NewDust(npc.Center - npc.Size * 0.5f, npc.width, npc.height, 90, 0f, 0f, 0, new Color(255, 50, 0), 0.1f);
            if (npc.life == 0)
                npc.checkDead();
        }

        public override void TownNPCAttackProj(NPC npc, ref int projType, ref int attackDelay)
        {
            base.TownNPCAttackProj(npc, ref projType, ref attackDelay);
        }
        public int ApplyVampricAura(NPC npc, float damage)
        {
            int value = 0;
            VauraMemory += damage;
            value = Mathf.FloorInt(VauraMemory);
            VauraMemory -= value;
            npc.life = Mathf.Clamp(npc.life - value, 0, npc.lifeMax);
            Dust.NewDust(npc.Center - npc.Size * 0.5f, npc.width, npc.height, 90, 0f, 0f, 0, new Color(255, 50, 0), 0.1f);
            if (npc.life == 0)
                npc.checkDead();
            return value;
        }

        #region Buffers
        public override void ModifyHitPlayer(NPC npc, Player target, ref int damage, ref bool crit)
        {
            if (HaveModifier(NPCModifier.Vampire))
            {
                npc.HealEffect(Mathf.RoundInt(damage * 0.5f), true);
            }

            if (HaveModifier(NPCModifier.ArmorBreaker))
            {
                float def = target.statDefense;

                float mult = 0.5f;
                if (Main.expertMode)
                    mult = 0.75f;

                damage += Mathf.RoundInt(def * 0.3f * mult);

            }

            base.ModifyHitPlayer(npc, target, ref damage, ref crit);
        }

        public string GetBufferProperty(string property)
        {
            try
            {
                return specialBuffer[property];
            }
            catch (Exception exception)
            {
                SummonHeartMod.Instance.Logger.Error(
                        "[" + GetType().FullName + "] GetBufferProperty(" +
                        property + "): " + exception.Message
                    );

                return null;
            }
        }

        public bool HaveBufferProperty(string property)
        {
            if (specialBuffer.ContainsKey(property))
                return true;
            return false;
        }

        public void SetBufferProperty(string property, string value)
        {
            if (specialBuffer.ContainsKey(property))
                specialBuffer[property] = value;
            else
                specialBuffer.Add(property, value);
        }


        #endregion


        public bool HaveModifier(NPCModifier _modifier)
        {
            return (_modifier & modifier) == _modifier;
        }


        public void SetLevelTier(int level, int tier, byte rank)
        {
            this.level = level;
            this.tier = tier;

            Rank = (NPCRank)rank;
        }

        public void SetStats(NPC npc)
        {
            npc = NPCUtils.SetNPCStats(npc, level, tier, Rank);
            baseDamage = npc.damage;

        }
        public void SetInit(NPC npc)
        {

            if (Main.netMode == NetmodeID.Server)
            {
                Main.npcLifeBytes[npc.type] = 4; //Sadly have to UN-optimise Health of ennemy, because it caused npc to dispear if health was over 128 (for small )
            }


            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (level < 0)
                {
                    if (!Config.NPCConfig.NPCProgress)
                    {
                        level = 0;
                        tier = 0;
                    }
                    else
                    {
                        level = NPCUtils.InitLevel(npc);
                        if (npc.townNPC || npc.damage == 0)
                            tier = Mathf.CeilInt(NPCUtils.GetTierAlly(npc, level) * Config.NPCConfig.NpclevelMultiplier);
                        else if (Config.NPCConfig.NPCProgress)
                            tier = Mathf.CeilInt(NPCUtils.GetTier(npc, level) * Config.NPCConfig.NpclevelMultiplier);
                    }
                    if (!npc.townNPC && !(npc.damage == 0) && !npc.dontCountMe)
                    {
                        Rank = NPCUtils.GetRank(level + tier, npc.boss);
                        modifier = NPCUtils.GetModifier(Rank, npc);
                        if (HaveModifier(NPCModifier.Size))
                        {
                            int maxrng = Main.expertMode ? 50 : 70;
                            int rn = Mathf.RandomInt(0, maxrng);
                            if (npc.boss)
                                rn += 1;
                            if (rn < 1)
                            {
                                SetBufferProperty("size", "Growther");
                                SetBufferProperty("GrowtherStep", "Mini");
                            }
                            else if (rn < 3)
                                SetBufferProperty("size", "Titan");
                            else if (rn < 6)
                                SetBufferProperty("size", "Colossus");
                            else if (rn < 20)
                                SetBufferProperty("size", "Giant");
                            else
                                SetBufferProperty("size", "Mini");
                        }
                    }
                }
            }
        }

        private int InitLevel(NPC npc)
        {
            return Mathf.CeilInt(NPCUtils.GetBaseLevel(npc) * Config.NPCConfig.NpclevelMultiplier);
        }

        private void Effect(NPC npc)
        {
            if (HaveModifier(NPCModifier.Golden))
            {
                Dust.NewDust(npc.Center - npc.Size * 0.5f, npc.width, npc.height, 244, 0f, 0f, 0, new Color(255, 255, 255), 0.05f);
                Lighting.AddLight(npc.Center, 1f, .8f, .5f);
            }


            if (HaveModifier(NPCModifier.Berserker))
            {
                Dust.NewDust(npc.Center - npc.Size * 0.5f, npc.width, npc.height, 90, 0f, 0f, 0, new Color(255, 255, 255), 0.5f);
                Lighting.AddLight(npc.Center, 0.3f, .0f, .0f);
            }

            if (HaveModifier(NPCModifier.Cluster))
            {
                Dust.NewDust(npc.Center - npc.Size * 0.5f, npc.width, npc.height, 91, 0f, 0f, 0, new Color(255, 255, 255), 0.5f);
                Lighting.AddLight(npc.Center, 0.2f, .1f, .3f);
            }

            if (npc.HasBuff(BuffID.Venom))
            {
                int num41 = Dust.NewDust(npc.position, npc.width, npc.height, 46, 0f, 0f, 120, default(Color), 0.2f);
                Main.dust[num41].noGravity = true;
                Main.dust[num41].fadeIn = 1.9f;
            }
        }

        public override void PostAI(NPC npc)
        {
            Effect(npc);
            if (npc.dontTakeDamage && dontTakeDamageTime > 0)
            {
                dontTakeDamageTime -= NPCUtils.DELTATIME;
                if (dontTakeDamageTime <= 0)
                {
                    npc.dontTakeDamage = false;
                    dontTakeDamageTime = 0;
                }
            }


            if (!StatsCreated && Main.netMode != NetmodeID.MultiplayerClient)
            {
                StatsCreated = true;
                SetInit(npc);
                SetStats(npc);
                //MPDebug.Log(mod,"Server Side : \n"+ npc.GetGivenOrTypeNetName()+ " ID : "+ npc.whoAmI + "\nLvl."+ (getLevel+getTier)+"\nHealth : " + npc.life + " / " + npc.lifeMax + "\nDamage : " + npc.damage + "\nDef : " + npc.defense + "\nTier : " + getRank + "\n");
                MsgUtils.SendNpcSpawn(mod, npc, tier, level, this);
                //NetMessage.SendData(23, -1, -1, null, npc.whoAmI);
                npc.GivenName = NPCUtils.GetNpcNameChange(npc, tier, level, Rank);

            }

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                if (!StatsFrame0)
                {
                    StatsFrame0 = true;
                }

                else if (!StatsCreated)
                {
                    StatsCreated = true;
                    MsgUtils.AskNpcInfo(mod, npc);
                }
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {

                debuffDamage += NPCUtils.UpdateDOT(npc);
                int applydamage = 0;

                if (debuffDamage > 0)
                {
                    applydamage = Mathf.FloorInt(debuffDamage);
                    debuffDamage -= applydamage;
                    npc.life -= applydamage;
                    npc.lifeRegen -= applydamage;
                    if (npc.life <= 0)
                        npc.checkDead();
                }


                if (HaveModifier(NPCModifier.Berserker))
                {
                    npc.damage = Mathf.RoundInt((float)baseDamage * (2 - (float)npc.life / (float)npc.lifeMax));
                }

                //MsgUtils.SendNpcUpdate(mod,npc);

            }


            if (npc.life < 0)
                npc.life = 0;
        }


        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {

            if (HaveModifier(NPCModifier.Dancer))
            {
                if (Mathf.Random(0, 1) < 0.2f)
                {
                    damage = 0;
                    Main.PlaySound(SoundID.DoubleJump, npc.position);
                }
            }

            base.OnHitByItem(npc, player, item, damage, knockback, crit);
            MsgUtils.SendNpcUpdate(mod, npc);
            //NetMessage.SendData(23, -1, -1, null, npc.whoAmI);
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            if (HaveModifier(NPCModifier.Dancer))
            {
                if (Mathf.Random(0, 1) < 0.2f)
                {
                    damage = 0;
                    Main.PlaySound(SoundID.DoubleJump, npc.position);
                }
            }
            base.OnHitByProjectile(npc, projectile, damage, knockback, crit);
            MsgUtils.SendNpcUpdate(mod, npc);
            //NetMessage.SendData(23, -1, -1, null, npc.whoAmI);
        }

        public override bool CheckDead(NPC npc)
        {
            if (HaveBufferProperty("GrowtherStep"))
            {
                switch (GetBufferProperty("GrowtherStep"))
                {
                    case "Mini":
                        SetBufferProperty("GrowtherStep", "Normal");
                        break;
                    case "Normal":
                        SetBufferProperty("GrowtherStep", "Giant");
                        break;
                    case "Giant":
                        SetBufferProperty("GrowtherStep", "Colossus");
                        break;
                    case "Colossus":
                        SetBufferProperty("GrowtherStep", "Titan");
                        break;
                    case "Titan":
                        return false;
                }
                npc = NPCUtils.SizeShiftMult(npc, GetBufferProperty("GrowtherStep"));
                npc.life = npc.lifeMax;
                MsgUtils.SendNpcUpdate(mod, npc);
                return false;
                //NetMessage.SendData(23, -1, -1, null, npc.whoAmI);
            }
            if(life > 1)
            {
                life--;
                npc.life = npc.lifeMax;
                MsgUtils.SendNpcUpdate(mod, npc);
                return false;
            }
            return true;
        }
       /* public override bool PreNPCLoot(NPC npc)
        {
            if (npc.type == NPCID.MoonLordCore)
            {
                if (!NPC.downedMoonlord)
                {
                }
            }
            if (npc.type == NPCID.EyeofCthulhu)
            {
                if (!NPC.downedBoss1)
                {
                    npc.DropItemInstanced(npc.position, npc.Size, mod.ItemType("AlchemistCharmTier1"));
                }
            }
            if (true)
            {
                if (npc.type == NPCID.KingSlime)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("KingSlimeBooster"));
                }
                if (npc.type == NPCID.EyeofCthulhu)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("EyeOfCthulhuBooster"));
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BrokenBooster1"));
                }
                if (npc.type == NPCID.EaterofWorldsHead && !NPC.AnyNPCs(NPCID.EaterofWorldsTail))
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("EaterOfWorldsBooster"));
                }
                if (npc.type == NPCID.EaterofWorldsTail && !NPC.AnyNPCs(NPCID.EaterofWorldsHead))
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("EaterOfWorldsBooster"));
                }
                if (npc.type == NPCID.BrainofCthulhu)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BrainOfCthulhuBooster"));
                }
                if (npc.type == NPCID.QueenBee)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("QueenBeeBooster"));
                }
                if (npc.type == NPCID.SkeletronHead)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SkeletronBooster"));
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BrokenBooster2"));
                }
                if (npc.type == NPCID.WallofFlesh)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("WoFBooster"));
                }
                if (npc.type == NPCID.SkeletronPrime)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PrimeBooster"));
                }
                if (npc.type == NPCID.Spazmatism && !NPC.AnyNPCs(NPCID.Retinazer))
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TwinsBooster"));
                }
                if (npc.type == NPCID.Retinazer && !NPC.AnyNPCs(NPCID.Spazmatism))
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TwinsBooster"));
                }
                if (npc.type == NPCID.TheDestroyer)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DestroyerBooster"));
                }
                if (npc.type == NPCID.Plantera)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PlanteraBooster"));
                }
                if (npc.type == NPCID.Golem)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GolemBooster"));
                }
                if (npc.type == NPCID.DukeFishron)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FishronBooster"));
                }
                if (npc.type == NPCID.CultistBoss)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CultistBooster"));
                }
                if (npc.type == NPCID.MoonLordCore)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MoonLordBooster"));
                }
                if (npc.type == NPCID.DD2DarkMageT1 || npc.type == NPCID.DD2DarkMageT3)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DarkMageBooster"));
                }
                if (npc.type == NPCID.DD2OgreT2 || npc.type == NPCID.DD2OgreT3)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("OgreBooster"));
                }
                if (npc.type == NPCID.DD2Betsy)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BetsyBooster"));
                }
                if (npc.type == 471)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GSummonerBooster"));
                }
                if (npc.type == 243)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("IceGolemBooster"));
                }
                if (npc.type == 170 || npc.type == 171 || npc.type == 180)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PigronBooster"));
                }
                if (npc.type == 395)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MartianSaucerBooster"));
                }
            }
            return base.PreNPCLoot(npc);
        }*/

        public override void NPCLoot(NPC npc)
        {

            if (HaveModifier(NPCModifier.Cluster) && !HaveBufferProperty("clustered"))
            {
                int clusterRN = Mathf.RandomInt(4, 8);
                for (int i = 0; i < clusterRN; i++)
                    NPCUtils.SpawnSized(npc, specialBuffer);

            }

            if (npc.damage == 0) return;
            if (npc.townNPC) return;
            

            Player player = Array.Find(Main.player, p => p.active);
            if (Main.netMode == NetmodeID.SinglePlayer)
                player = Main.LocalPlayer; //if local , well it's simple
            else if (Main.player[npc.target].active)
                player = Main.player[npc.target];
            else
                return;
            int XPToDrop = NPCUtils.GetExp(npc);
            if (npc.rarity > 0)
            {
                XPToDrop = (int)(XPToDrop * 1.5f);
            }
            if (npc.boss)
            {
                XPToDrop = XPToDrop * 2;
                WorldManager.OnBossDefeated(npc);
            }
        }
    }
}
