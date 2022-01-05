﻿using System;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using SummonHeart.Utilities;
using SummonHeart.XiuXianModule.EnumType;

namespace SummonHeart.XiuXianModule.Entities.Npc
{
    class NPCUtils
    {
        public static Dictionary<NPCModifier, int> modifierWeight = new Dictionary<NPCModifier, int>()
        {
            { NPCModifier.None,     200},
            { NPCModifier.Cluster,     10},
            { NPCModifier.Size,     200},
            { NPCModifier.Berserker,     100},
            { NPCModifier.Golden,     10},
            { NPCModifier.Vampire,     50},
             { NPCModifier.ArmorBreaker,     50},
             { NPCModifier.Dancer,     25},
        };

        public static Dictionary<NPCRank, float[]> NPCRankStats = new Dictionary<NPCRank, float[]>()
        {
            //Rank                                  HPMult,   DMGMult,    DefMult
            { NPCRank.Weak,         new float[3]    {0.8f,    0.9f,       0.5f  } },
            { NPCRank.Normal,       new float[3]    {1,       1,          1     } },
            { NPCRank.Alpha,        new float[3]    {1.2f,    1.05f,      1.1f  } },
            { NPCRank.Elite,        new float[3]    {1.4f,    1.10f,       1.2f  } },
            { NPCRank.Legendary,    new float[3]    {2,       1.2f,       1.4f  } },
            { NPCRank.Mythical,     new float[3]    {3.5f,       1.3f,          1.6f  } },
            { NPCRank.Godly,        new float[3]    {5,      1.4f,          1.8f  } },
            { NPCRank.DIO,          new float[3]    {10,      1.5f,           2f  } },

            //ASCENDED WORLD :
            { NPCRank.LimitBreaked,    new float[3]    {5,      2f,          3f  } },
            { NPCRank.Raised,          new float[3]    {10,      3.5f,          4f  } },
            { NPCRank.Ascended,        new float[3]    {20,      5f,          5f  } },
            { NPCRank.HighAscended,    new float[3]    {35,      6f,          6f  } },
            { NPCRank.PeakAscended,    new float[3]    {50,      7f,          7f  } },
            { NPCRank.Transcendental,  new float[3]    {200,      8.5f,          8.5f  } },
            { NPCRank.TransDimensional,new float[3]    {1000f,      10f,          10f  } },

            { NPCRank.DioAboveHeaven,  new float[3]    {9999999999999f,      100f,           20f  } }, //int max health, 100X damage, 20 time defense, Good luck XD
        };

        public static Dictionary<string, float[]> NPCSizeStats = new Dictionary<string, float[]>()
        {
            //Rank                              HPMult,     DMGMult,    DefMult     Size
            { "Mini",         new float[4]      {0.8f,       0.9f,       0.7f,       0.6f    } },
            { "Giant",        new float[4]      {1.8f,       1.05f,       1.05f,       1.2f    } },
            { "Colossus",     new float[4]      {2.2f,         1.10f,       1.10f,       1.5f    } },
            { "Titan",        new float[4]      {2.5f,         1.15f,       1.2f,       1.8f    } },

        };

        public static float DELTATIME = 1f / 60f;
        //Generate/Get level and tier or NPC
        #region LevelGen
        //Get Base Level Based on Stat of npc
        public static int GetBaseLevel(NPC npc)
        {

            if (npc.type == NPCID.DungeonGuardian || npc.type == NPCID.SpikeBall | npc.type == NPCID.BlazingWheel)
                return 1;
            int maxLevel = GetMaxLevel();

            float health = npc.lifeMax;

            int damage = npc.damage;
            int def = npc.defense;
            if (npc.damage < 0)
                damage = 1;
            if (npc.defense < 0)
                def = 1;
            if (npc.defense > npc.damage)
                def = npc.damage / 2;

            int baselevel = Mathf.HugeCalc((int)(Mathf.Pow(npc.lifeMax / 20, 1.1f) + Mathf.Pow(damage * 0.4f, 1.2f) + Mathf.Pow(def, 1.4f)), -1);

            if (npc.boss)
            {


                if (Main.expertMode)
                {
                    baselevel = Mathf.HugeCalc((int)(health / 100 + Mathf.Pow(damage * 0.45f, 1.1f) + Mathf.Pow(def, 1.2f)), -1);
                }
                else
                {
                    baselevel = Mathf.HugeCalc((int)(health / 80 + Mathf.Pow(damage * 0.35f, 1.09f) + Mathf.Pow(def * 0.9f, 1.15f)), -1);
                }

                if ((npc.aiStyle == 6 || npc.aiStyle == 37) && npc.type > 100)
                {
                    if (Main.expertMode)
                    {
                        baselevel = Mathf.HugeCalc((int)(Mathf.Pow(health / 650, 0.5f) + Mathf.Pow(damage * 0.31f, 1.05f) + Mathf.Pow(def * 0.8f, 1.07f)), -1);
                    }
                    else
                    {
                        baselevel = Mathf.HugeCalc((int)(Mathf.Pow(health / 500, 0.5f) + Mathf.Pow(damage * 0.31f, 1.05f) + Mathf.Pow(def * 0.8f, 1.07f)), -1);
                    }
                }

            }

            if (Main.expertMode)
            {
                baselevel = (int)(baselevel * 0.8f);
            }

            baselevel = WorldManager.GetWorldLevelMultiplier(baselevel);
            if (baselevel < -1)
                return 0;
            if (Config.NPCConfig.LimitNPCGrowth)
            {
                baselevel = baselevel.Clamp(10, maxLevel);
            }


            baselevel = baselevel.Clamp(10, int.MaxValue);
            return baselevel;
        }

        public static int GetMaxLevel()
        {
            int maxLevel = Mathf.CeilInt(WorldManager.PlayerLevel + Config.NPCConfig.LimitNPCGrowthValue + WorldManager.PlayerLevel * Config.NPCConfig.LimitNPCGrowthPercent * 0.01f);
            if (maxLevel < 1)
                maxLevel = 1;
            return maxLevel;
        }

        internal static int InitLevel(NPC npc)
        {
            int level = 1;
            int baseLevel = 0;
            RPGPlayer mp = Main.LocalPlayer.GetModPlayer<RPGPlayer>();
            if (npc.boss)
            {
                if (npc.type == NPCID.KingSlime)
                {
                    //1、史莱姆王、结丹一重
                    baseLevel = 21;
                }
                if (npc.type == NPCID.EyeofCthulhu)
                {
                    //2、克眼、结丹三重
                    baseLevel = 23;
                }
                if (npc.type == NPCID.EaterofWorldsHead && !NPC.AnyNPCs(NPCID.EaterofWorldsTail))
                {
                    //3、世吞/克脑、结丹七重
                    baseLevel = 27;
                }
                if (npc.type == NPCID.EaterofWorldsTail && !NPC.AnyNPCs(NPCID.EaterofWorldsHead))
                {
                    //3、世吞/克脑、结丹七重
                    baseLevel = 27;
                }
                if (npc.type == NPCID.BrainofCthulhu)
                {
                    //3、世吞/克脑、结丹七重
                    baseLevel = 27;
                }
                if (npc.type == NPCID.QueenBee)
                {
                    //4、蜂王、结丹十重巅峰
                    baseLevel = 30;
                }
                if (npc.type == NPCID.SkeletronHead)
                {
                    //5、吴克、元婴一重
                    baseLevel = 31;
                }
                if (npc.type == NPCID.WallofFlesh)
                {
                    //6、肉山、元婴三重
                    baseLevel = 33;
                }
                if (npc.type == NPCID.SkeletronPrime)
                {
                    //7、新三王、元婴十重巅峰
                    baseLevel = 40;
                }
                if (npc.type == NPCID.Spazmatism && !NPC.AnyNPCs(NPCID.Retinazer))
                {
                    //7、新三王、元婴十重巅峰
                    baseLevel = 40;
                }
                if (npc.type == NPCID.Retinazer && !NPC.AnyNPCs(NPCID.Spazmatism))
                {
                    //7、新三王、元婴十重巅峰
                    baseLevel = 40;
                }
                if (npc.type == NPCID.TheDestroyer)
                {
                    //7、新三王、元婴十重巅峰
                    baseLevel = 40;
                }
                if (npc.type == NPCID.Plantera)
                {
                    //8、小花、化神一重
                    baseLevel = 41;
                }
                if (npc.type == NPCID.Golem)
                {
                    //9、石头人、化神三重
                    baseLevel = 43;
                }
                if (npc.type == NPCID.DukeFishron)
                {
                    //10、肉后小怪、化神七重
                    baseLevel = 47;
                }
                if (npc.type == NPCID.CultistBoss)
                {
                    //11、邪教徒、化神巅峰
                    baseLevel = 50;
                }
                if (npc.type == NPCID.MoonLordCore)
                {
                    //12、月总、合体一重
                    baseLevel = 51;
                }
                if (npc.type == NPCID.DD2Betsy)
                {
                    //12、天国飞龙
                    baseLevel = 40;
                }
            }
            else
            {
                //小怪
                baseLevel = mp.GetLevel();
                if (baseLevel > 30)
                    baseLevel = 30;
            }
            return baseLevel + mp.GetStat(Stat.道心);
        }

        //Get Tier bonus from world
        public static int GetWorldTier(NPC npc, int baselevel)
        {

            int BonusLevel = WorldManager.GetWorldAdditionalLevel();
            int maxLevel = GetMaxLevel();
            if (BonusLevel + baselevel > maxLevel)
            {
                return maxLevel - baselevel;
            }
            return 0;
        }

        //Tier are bonus level either random or from world
        public static int GetTier(NPC npc, int baselevel)
        {
            if (baselevel < 0)
                baselevel = 0;
            int rand = Mathf.RandomInt(0, 4 + Mathf.CeilInt(baselevel * 0.1f));
            return Mathf.HugeCalc(rand + GetWorldTier(npc, baselevel + rand), -1);
        }
        public static int GetTierAlly(NPC npc, int baselevel)
        {
            return WorldManager.GetWorldAdditionalLevel();
        }

        //get actual rank of the monster
        public static NPCRank GetRank(int level, bool boss = false)
        {
            if (!WorldManager.ascended)
            {
                if (!Config.NPCConfig.NPCRarity)
                    return NPCRank.Normal;
                if (boss && !Config.NPCConfig.BossRarity)
                    return NPCRank.Normal;
                if (level < 1)
                    level = 1;
                float rarityBooster = (float)Math.Log(level + 1) + 1;
                int rn = Mathf.RandomInt(0, 1500 / (level / 50 + 1));

                if (rn <= 1)
                    return NPCRank.DIO;
                if (rn <= 3)
                    return NPCRank.Godly;
                if (rn <= 8)
                    return NPCRank.Mythical;
                if (rn < 15)
                    return NPCRank.Legendary;
                if (rn < 150)
                    return NPCRank.Elite;
                if (rn < 350)
                    return NPCRank.Alpha;
                if (rn < 1050)
                    return NPCRank.Normal;
                return NPCRank.Weak;

            }
            else
            {
                if (!Config.NPCConfig.NPCRarity)
                    return NPCRank.Raised;
                if (level < 1)
                    level = 1;
                int rn = Mathf.RandomInt(0, 4000 / (level / 1000 + 1));

                if (rn <= 1)
                    return NPCRank.DioAboveHeaven;
                if (rn <= 5)
                    return NPCRank.TransDimensional;
                if (rn <= 15)
                    return NPCRank.Transcendental;
                if (rn < 35)
                    return NPCRank.PeakAscended;
                if (rn < 150)
                    return NPCRank.HighAscended;
                if (rn < 500)
                    return NPCRank.Ascended;
                if (rn < 2000)
                    return NPCRank.Raised;
                return NPCRank.LimitBreaked;
            }

        }

        #endregion

        public static int GetExp(NPC npc)
        {
            return Mathf.CeilInt(Math.Pow(npc.lifeMax / 8 + npc.damage * 1.2 + npc.defense * 1.7f, 0.9f));
        }

        private static NPCModifier AddRandomModifier(List<NPCModifier> pool)
        {
            int totalWeight = 0;
            for (int i = 0; i < pool.Count; i++)
                totalWeight += modifierWeight[pool[i]];

            int rn = Mathf.RandomInt(0, totalWeight);
            int checkingWeight = 0;
            for (int i = 0; i < pool.Count; i++)
            {
                if (rn < checkingWeight + modifierWeight[pool[i]])
                    return pool[i];
                checkingWeight += modifierWeight[pool[i]];

            }
            return pool[pool.Count - 1];

        }

        public static NPCModifier GetModifier(NPCRank rank, NPC npc)
        {
            if (npc.dontCountMe)
                return NPCModifier.None;
            if (!Config.NPCConfig.NPCModifier)
                return NPCModifier.None;

            if (npc.boss && !Config.NPCConfig.BossModifier)
                return NPCModifier.None;

            int maxModifier = 1;
            switch (rank)
            {
                case NPCRank.Weak:
                    return 0;
                case NPCRank.Normal:
                    maxModifier = Mathf.Random(0, 3) < 1 ? 0 : 1;
                    break;
                case NPCRank.Alpha:
                    maxModifier = 1;
                    break;
                case NPCRank.Elite:
                    maxModifier = Mathf.Random(0, 3) < 1 ? 1 : 2;
                    break;
                case NPCRank.Legendary:
                    maxModifier = 2;
                    break;
                case NPCRank.Mythical:
                    maxModifier = 3;
                    break;
                case NPCRank.Godly:
                    maxModifier = 4;
                    break;
                case NPCRank.DIO:
                    maxModifier = (Enum.GetValues(typeof(NPCModifier)) as NPCModifier[]).Length;
                    break;
            }
            NPCModifier modifiers = 0;
            //if npc.aiStyle == 3 



            List<NPCModifier> modifiersPool = (Enum.GetValues(typeof(NPCModifier)) as NPCModifier[]).ToList();
            if (npc.boss)
            {
                modifiersPool.Remove(NPCModifier.Size);
                if (maxModifier == (Enum.GetValues(typeof(NPCModifier)) as NPCModifier[]).Length)
                    maxModifier -= 1;
                if (!Config.NPCConfig.BossClustered)
                {
                    modifiersPool.Remove(NPCModifier.Cluster);
                    if (maxModifier + 1 == (Enum.GetValues(typeof(NPCModifier)) as NPCModifier[]).Length)
                        maxModifier -= 1;
                }
            }



            else if (npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsTail)
            {
                modifiersPool.Remove(NPCModifier.Cluster);
                modifiersPool.Remove(NPCModifier.Size);
                if (maxModifier == (Enum.GetValues(typeof(NPCModifier)) as NPCModifier[]).Length)
                    maxModifier -= 2;
            }

            for (int i = 0; i < maxModifier; i++)
            {
                NPCModifier modid = AddRandomModifier(modifiersPool);
                modifiers = modifiers | modid;
                modifiersPool.Remove(modid);
            }

            return modifiers;
        }

        public static string GetNpcNameChange(NPC npc, int tier, int level, NPCRank rank)
        {

            string name = npc.GivenOrTypeName;
            if (npc.townNPC)
                return name;
            if (name == "")
                name = npc.GivenName;
            if (name == "")
                name = npc.TypeName;
            if (name == "")
                return name;



            string sufix = " the ";
            string Prefix = "";
            /*
        if (Config.gpConfig.NPCProgress)
            Prefix+= "Lvl. " + (tier + level) + " ";
        if (WorldManager.GetWorldAdditionalLevel() > 0)
            Prefix += "(+" + GetWorldTier(npc,level) + ") ";
            */
            switch (rank)
            {
                case NPCRank.Weak:
                    Prefix += "Weak ";
                    break;
                case NPCRank.Alpha:
                    Prefix += "Alpha ";
                    break;
                case NPCRank.Elite:
                    Prefix += "Elite ";
                    break;
                case NPCRank.Legendary:
                    Prefix += "Legendary ";
                    break;
                case NPCRank.Mythical:
                    Prefix += "Mythical ";
                    break;
                case NPCRank.Godly:
                    Prefix += "Godly ";
                    break;
                case NPCRank.DIO:
                    Prefix += "Kono Dio Da ";
                    break;
            }

            RPGGlobalNPC anpc = npc.GetGlobalNPC<RPGGlobalNPC>();
            if (anpc.HaveModifier(NPCModifier.Dancer))
                sufix += "Dancing ";
            if (anpc.HaveModifier(NPCModifier.Cluster))
                sufix += "Clustered ";
            if (anpc.HaveModifier(NPCModifier.Golden))
                sufix += "Golden ";
            if (anpc.HaveModifier(NPCModifier.ArmorBreaker))
                sufix += "Armor Breaker ";
            if (anpc.HaveModifier(NPCModifier.Berserker))
                sufix += "Berserker ";
            if (anpc.HaveModifier(NPCModifier.Vampire))
                sufix += "Vampire ";

            if (anpc.HaveModifier(NPCModifier.Size))
            {
                string size = anpc.GetBufferProperty("size");
                Prefix += size + " ";
            }
            if (sufix == " the ")
                sufix = "";
            return Prefix + name + sufix;

        }

        public static NPC SetRankStat(NPC npc, NPCRank rank)
        {
            if (rank == NPCRank.Normal)
                return npc;
            if (rank == NPCRank.Weak)
            {
                npc.lifeMax = Mathf.CeilInt(npc.lifeMax * NPCRankStats[rank][0]);
                if (npc.damage > 0)
                    npc.damage = Mathf.CeilInt(npc.damage * NPCRankStats[rank][1]);
                if (npc.defense > 0)
                    npc.defense = Mathf.CeilInt(npc.defense * NPCRankStats[rank][2]);
            }
            else
            {
                npc.lifeMax = Mathf.HugeCalc(Mathf.CeilInt(npc.lifeMax * NPCRankStats[rank][0]), npc.lifeMax);
                if (npc.damage > 0)
                    npc.damage = Mathf.HugeCalc(Mathf.CeilInt(npc.damage * NPCRankStats[rank][1]), npc.damage);
                if (npc.defense > 0)
                    npc.defense = Mathf.HugeCalc(Mathf.CeilInt(npc.defense * NPCRankStats[rank][2]), npc.defense);
            }
            npc.life = npc.lifeMax;

            return npc;
        }

        public static NPC SetSizeStat(NPC npc, string size)
        {
            if (size == "Growther")
                size = npc.GetGlobalNPC<RPGGlobalNPC>().GetBufferProperty("GrowtherStep");

            if (size == "Normal")
                return npc;
            if (size == "Mini")
            {
                npc.lifeMax = Mathf.CeilInt(npc.lifeMax * NPCSizeStats[size][0]);
                if (npc.damage > 0)
                    npc.damage = Mathf.CeilInt(npc.damage * NPCSizeStats[size][1]);
                if (npc.defense > 0)
                    npc.defense = Mathf.CeilInt(npc.defense * NPCSizeStats[size][2]);
            }
            else
            {
                npc.lifeMax = Mathf.HugeCalc(Mathf.CeilInt(npc.lifeMax * NPCSizeStats[size][0]), npc.lifeMax);
                if (npc.damage > 0)
                    npc.damage = Mathf.HugeCalc(Mathf.CeilInt(npc.damage * NPCSizeStats[size][1]), npc.damage);
                if (npc.defense > 0)
                    npc.defense = Mathf.HugeCalc(Mathf.CeilInt(npc.defense * NPCSizeStats[size][2]), npc.defense);
            }


            npc.scale *= NPCSizeStats[size][3];

            npc.life = npc.lifeMax;

            return npc;
        }

        public static NPC SizeShiftMult(NPC npc, string size)
        {
            if (size == "Mini")
            {
                npc.lifeMax = Mathf.HugeCalc(Mathf.CeilInt(npc.lifeMax / NPCSizeStats["Mini"][0]), npc.lifeMax);
                if (npc.damage > 0)
                    npc.damage = Mathf.HugeCalc(Mathf.CeilInt(npc.damage / NPCSizeStats["Mini"][1]), npc.damage);
                if (npc.defense > 0)
                    npc.defense = Mathf.HugeCalc(Mathf.CeilInt(npc.defense / NPCSizeStats["Mini"][2]), npc.defense);

                npc.scale /= NPCSizeStats["Mini"][3];
            }
            else if (size == "Normal")
            {
                npc.lifeMax = Mathf.HugeCalc(Mathf.CeilInt(npc.lifeMax * NPCSizeStats["Giant"][0]), npc.lifeMax);
                if (npc.damage > 0)
                    npc.damage = Mathf.HugeCalc(Mathf.CeilInt(npc.damage * NPCSizeStats["Giant"][1]), npc.damage);
                if (npc.defense > 0)
                    npc.defense = Mathf.HugeCalc(Mathf.CeilInt(npc.defense * NPCSizeStats["Giant"][2]), npc.defense);

                npc.scale *= NPCSizeStats["Giant"][3];
            }
            else if (size == "Giant")
            {

                npc.lifeMax = Mathf.HugeCalc(Mathf.CeilInt(npc.lifeMax / NPCSizeStats["Giant"][0] * NPCSizeStats["Colossus"][0]), npc.lifeMax);
                if (npc.damage > 0)
                    npc.damage = Mathf.HugeCalc(Mathf.CeilInt(npc.damage / NPCSizeStats["Giant"][1] * NPCSizeStats["Colossus"][1]), npc.damage);
                if (npc.defense > 0)
                    npc.defense = Mathf.HugeCalc(Mathf.CeilInt(npc.defense / NPCSizeStats["Giant"][2] * NPCSizeStats["Colossus"][2]), npc.defense);

                npc.scale *= NPCSizeStats["Colossus"][3] / NPCSizeStats["Giant"][3];
            }
            else if (size == "Colossus")
            {
                npc.lifeMax = Mathf.HugeCalc(Mathf.CeilInt(npc.lifeMax / NPCSizeStats["Colossus"][0] * NPCSizeStats["Titan"][0]), npc.lifeMax);
                if (npc.damage > 0)
                    npc.damage = Mathf.HugeCalc(Mathf.CeilInt(npc.damage / NPCSizeStats["Colossus"][1] * NPCSizeStats["Titan"][1]), npc.damage);
                if (npc.defense > 0)
                    npc.defense = Mathf.HugeCalc(Mathf.CeilInt(npc.defense / NPCSizeStats["Colossus"][2] * NPCSizeStats["Titan"][2]), npc.defense);

                npc.scale *= NPCSizeStats["Titan"][3] / NPCSizeStats["Colossus"][3];
            }

            return npc;
        }

        public static NPC SetModifierStat(NPC npc)
        {
            RPGGlobalNPC ArNpc = npc.GetGlobalNPC<RPGGlobalNPC>();
            if (ArNpc.HaveModifier(NPCModifier.Golden))
            {

                npc.lifeMax = Mathf.HugeCalc((int)(npc.lifeMax * 3f), npc.lifeMax);
                if (npc.damage > 0)
                    npc.damage = Mathf.HugeCalc((int)(npc.damage * 1.5f), npc.damage);
                npc.value += Item.buyPrice(0, 1, 50, 0);
                npc.value *= 2 * ArNpc.getRank;
            }

            if (ArNpc.HaveModifier(NPCModifier.Size))
                npc = SetSizeStat(npc, ArNpc.GetBufferProperty("size"));

            if (ArNpc.HaveModifier(NPCModifier.Vampire))
                npc.lifeMax = Mathf.HugeCalc((int)(npc.lifeMax * 1.5f), npc.lifeMax);
            if (npc.damage > 0)
                npc.damage = Mathf.HugeCalc((int)(npc.damage * 1.3f), npc.damage);

            if (ArNpc.HaveModifier(NPCModifier.Berserker))
                npc.color = Color.Lerp(npc.color, new Color(1.0f, 0.0f, 0.0f), 0.3f);
            if (ArNpc.HaveModifier(NPCModifier.Golden))
                npc.color = Color.Lerp(npc.color, new Color(1.0f, 0.8f, 0.5f), 0.8f);



            npc.life = npc.lifeMax;

            return npc;

        }



        static public void SpawnSized(NPC npc, Dictionary<string, string> buffer)
        {
            int n = NPC.NewNPC(
               (int)npc.position.X,
               (int)npc.position.Y,
               npc.type
               );
            Main.npc[n].SetDefaults(Main.npc[n].netID);
            Main.npc[n].velocity.X = Mathf.RandomInt(-8, 8);
            Main.npc[n].velocity.Y = Mathf.RandomInt(-20, -2);
            Main.npc[n].GetGlobalNPC<RPGGlobalNPC>().SetBufferProperty("clustered", "true");
        }

        public static float UpdateDOT(NPC npc)
        {
            float DoTDamage = 0;
            int life = npc.life;
            if (life > 1)
            {
                if (npc.HasBuff(BuffID.OnFire))
                {
                    DoTDamage += (Mathf.Logx(npc.lifeMax, 1.01f) * 0.25f * DELTATIME).Clamp(0, npc.lifeMax * 0.003f * DELTATIME);
                }
                if (npc.HasBuff(BuffID.Burning))
                {
                    DoTDamage += (Mathf.Logx(npc.lifeMax, 1.01f) * 0.05f * DELTATIME).Clamp(0, npc.lifeMax * 0.002f * DELTATIME);
                }
                if (npc.HasBuff(BuffID.Frostburn))
                {

                    DoTDamage += (Mathf.Logx(npc.lifeMax, 1.01f) * 0.25f * DELTATIME).Clamp(0, npc.lifeMax * 0.004f * DELTATIME);
                }
                if (npc.HasBuff(BuffID.Venom))
                {

                    DoTDamage += (Mathf.Logx(npc.lifeMax, 1.01f) * 0.25f * DELTATIME).Clamp(0, npc.lifeMax * 0.005f * DELTATIME);
                }
            }
            return DoTDamage;
        }

        public static int GetLinliMax(int level)
        {
            double tempMax = 0;
            int a = level / 10;
            int b = level % 10;
            if (level == 0)
                return 0;
            if (b == 0)
            {
                tempMax = 9 * Math.Pow(10, a);
            }
            else if (a > 0)
            {
                tempMax = 9 * (b + 1) * Math.Pow(10, a);
            }
            else
            {
                tempMax = 9 * b * Math.Pow(10, a);
            }
            return Mathf.CeilInt(tempMax);
        }

        public static NPC SetNPCStats(NPC npc, int level, int tier, NPCRank rank)
        {
            if (npc == null)
                return npc;
            int lingliMax = GetLinliMax(level);
            npc.lifeMax = Mathf.HugeCalc(npc.lifeMax + lingliMax, npc.lifeMax);
            if (npc.damage > 0)
                npc.damage += Mathf.HugeCalc(Mathf.FloorInt(npc.damage + lingliMax / 10), npc.damage);
            if (npc.defense > 0)
                npc.defense = Mathf.HugeCalc(Mathf.FloorInt(npc.defense + lingliMax / 8), npc.defense);

            if (npc.defense < 0)
                npc.defense = 0;
            if (npc.damage < 0)
                npc.damage = 0;

            npc.life = npc.lifeMax;

            return npc;

            if (!Config.NPCConfig.NPCProgress)
            {
                npc = SetRankStat(npc, rank);
                npc = SetModifierStat(npc);
                return npc;
            }

            if (npc.townNPC || npc.damage == 0)
            {
                npc.lifeMax = Mathf.HugeCalc(Mathf.FloorInt(npc.lifeMax * (0.5f + (tier + level) * 0.1f)), npc.lifeMax);
                if (npc.damage > 0)
                    npc.damage = Mathf.HugeCalc(Mathf.FloorInt(npc.damage * (0.75f + level * 0.03f + tier * 0.06f)), npc.damage);
                if (npc.defense > 0)
                    npc.defense = Mathf.HugeCalc(Mathf.FloorInt(npc.defense * (0.8f + level * 0.012f + tier * 0.02f)), npc.defense);

                if (npc.defense < 0)
                    npc.defense = 0;
                if (npc.damage < 0)
                    npc.damage = 0;

                npc.life = npc.lifeMax;

                return npc;
            }
            else
            {
                float power = 1.0f;
                if (Main.hardMode)
                    power = 1.15f;

                if (npc.boss)
                {
                    if (npc.damage > 0)
                    {
                        if (Mathf.HugeCalc(Mathf.FloorInt(npc.damage * (0.35f + level * 0.04f + tier * 0.06f) * Config.NPCConfig.NpcDamageMultiplier * 0.75f), 1) < 250000)
                            npc.damage = Mathf.HugeCalc(Mathf.FloorInt(npc.damage * (0.35f + level * 0.04f + tier * 0.06f) * Config.NPCConfig.NpcDamageMultiplier * 0.75f), 1);
                        else
                        {
                            npc.damage = Mathf.FloorInt(250000 * Mathf.Logx(1 + level * 0.10f + tier * 0.30f, 7.5f) * Config.NPCConfig.NpcDamageMultiplier);
                        }
                    }

                    npc.lifeMax = Mathf.HugeCalc(Mathf.FloorInt(Mathf.Pow(npc.lifeMax * (level * 0.05f + tier * 0.075), 1.1f) * Config.NPCConfig.BossHealthMultiplier * Config.NPCConfig.NpcHealthMultiplier), 1);
                }
                else
                {
                    if (npc.damage > 0)
                        npc.damage = Mathf.HugeCalc(Mathf.FloorInt(npc.damage * (0.75f + level * 0.035f + tier * 0.05f) * Config.NPCConfig.NpcDamageMultiplier), 1);
                    npc.lifeMax = Mathf.HugeCalc(Mathf.FloorInt(Mathf.Pow(npc.lifeMax * (level * 0.20f + tier * 0.35f), power) * Config.NPCConfig.NpcHealthMultiplier), 1);
                }

                npc.value = npc.value * (1 + (level + tier) * 0.001f) * (1 + (int)rank * 0.1f);
                if (npc.defense > 0)
                    npc.defense = Mathf.HugeCalc(Mathf.FloorInt(npc.defense * (1 + level * 0.01f + tier * 0.02f)), npc.defense);
                if (npc.defense > 5)
                    npc.defense -= 5;
                else
                    npc.defense = 0;

                npc.life = npc.lifeMax;

                npc = SetRankStat(npc, rank);
                npc = SetModifierStat(npc);
            }
            return npc;
        }
    }
}
