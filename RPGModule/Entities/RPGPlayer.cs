using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using System.Reflection;
using Terraria.UI.Chat;
using AnotherRpgMod.RPGModule;
using SummonHeart.Utilities;
using SummonHeart.RPGModule.Enum;

namespace SummonHeart.RPGModule.Entities
{

    class RPGPlayer : ModPlayer
    {
        public readonly static ushort ACTUALSAVEVERSION = 2;
        private int Exp = 0;
        private string basename = "";


        private DateTime LastLeech = DateTime.MinValue;

        private RPGStats Stats;
        float damageToApply = 0;

        private bool initiated = false;

        private bool XpLimitMessage = false;

        private int level = 1;
        private int armor;
        public int BaseArmor { get { return armor; } }
        public float m_virtualRes = 0;

        public long EquipedItemXp = 0;
        public long EquipedItemMaxXp = 1;

        public float ManaRegenBuffer = 0;
        public float ManaRegenPerSecond = 0;
        public int manaShieldDelay = 0;


        public float statMultiplier = 1;

        static public float MAINSTATSMULT = 0.0025f;
        static public float SECONDARYTATSMULT = 0.001f;
        public string baseName = "";


        public void SyncLevel(int _level) //only use for sync
        {
            level = _level;
        }
        public void SyncStat(StatData data, Stat stat) //only use for sync
        {
            Stats.SetStats(stat, level, data.GetLevel, data.GetXP);
        }

        public int GetStatImproved(Stat stat)
        {
            float VampireMultiplier = 1;
            float DayPerkMultiplier = 1;
            return Mathf.CeilInt(GetStat(stat)  * VampireMultiplier * DayPerkMultiplier);
        }

        public int GetStat(Stat s)
        {
            return Stats.GetStat(s);
        }

        public int GetStatXP(Stat s)
        {
            return Stats.GetStatXP(s);
        }
        public int GetStatXPMax(Stat s)
        {
            return Stats.GetStatXPMax(s);
        }
        public int GetAddStat(Stat s)
        {
            return Stats.GetLevelStat(s);
        }
        public int GetLevel()
        {
            return level;
        }
        public int GetExp()
        {
            return Exp;
        }

        private int totalPoints = 10;
        private int freePoints = 10;


        public int FreePtns { get { return freePoints; } }
        public int TotalPtns { get { return totalPoints; } }


        public float GetLifeLeechLeft { get { return HealthRegenLeech * LifeLeechDuration * player.statLifeMax2; } }
        private float HealthRegenLeech = 0.02f;

        private float LifeLeechDuration;
        public float LifeLeechMaxDuration = 3;

        public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
        {
            base.GetHealLife(item, quickHeal, ref healValue);
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(item, target, damage, knockback, crit);
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPCWithProj(proj, target, damage, knockback, crit);
        }

        public struct ManaShieldInfo
        {
            public float DamageAbsorbtion;
            public float ManaPerDamage;

            public ManaShieldInfo(float _Absortion, float _ManaPerDamage)
            {
                DamageAbsorbtion = _Absortion;
                ManaPerDamage = _ManaPerDamage;

            }
        }

        public void SpendPoints(Stat _stat, int ammount)
        {
            ammount = Mathf.Clamp(ammount, 1, freePoints);
            Stats.UpgradeStat(_stat, ammount);
            freePoints -= ammount;
        }

        public int GetNaturalStat(Stat s)
        {
            return Stats.GetNaturalStat(s);
        }

        public float GetDefenceMult()
        {
            return (GetStatImproved(Stat.灵根) * 0.0025f + GetStatImproved(Stat.魅力) * 0.006f) * statMultiplier + 1f;
        }

        public float GetHealthPerHeart()
        {
            return (GetStatImproved(Stat.灵根) * 0.65f + GetStatImproved(Stat.魅力) * 0.325f) * statMultiplier + 10;
        }
        public float GetManaPerStar()
        {
            return (GetStatImproved(Stat.悟性) * 0.2f + GetStatImproved(Stat.体质) * 0.05f) * statMultiplier + 10;
        }

        public void ApplyReduction(ref int damage, bool heal = false)
        {
            if (m_virtualRes > 0)
                CombatText.NewText(player.getRect(), new Color(50, 26, 255, 1), "(" + damage + ")");
            damage = (int)(damage * (1 - m_virtualRes));

        }

        public void ApplyReduction(ref float damage, bool heal = false)
        {
            if (m_virtualRes > 0)
                CombatText.NewText(player.getRect(), new Color(50, 26, 255, 1), "(" + damage + ")");
            damage = (float)(damage * (1 - m_virtualRes));

        }

        public float GetArmorPenetrationMult()
        {
            return 1f + Stats.GetStat(Stat.功法) * 0.01f;
        }

        public int GetArmorPenetrationAdd()
        {
            return Mathf.FloorInt(Stats.GetStat(Stat.功法) * 0.1f);
        }



        public override void PostUpdateEquips()
        {

            if (Config.gpConfig.RPGPlayer)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    m_virtualRes = 0;
                    armor = player.statDefense;

                    player.statLifeMax2 = Mathf.Clamp((int)(GetHealthMult() * player.statLifeMax2 * GetHealthPerHeart() / 20) + 10, 10, int.MaxValue);
                    /*
                if (Main.netMode == 1)
                {
                    player.statLifeMax2 = Mathf.Clamp((int)(GetHealthMult() * player.statLifeMax2 * GetHealthPerHeart() / 20) + 10,10,Int16.MaxValue);
                    if (player.statLifeMax2>= Int16.MaxValue)
                    {
                        float HealthVirtualMult = Mathf.Clamp((int)(GetHealthMult() * player.statLifeMax2 * GetHealthPerHeart() / 20) + 10, 10, int.MaxValue) / player.statLifeMax2;
                        m_virtualRes = 1-(1f / HealthVirtualMult);
                    }
                            
                }
                else
                {
                    player.statLifeMax2 = Mathf.Clamp((int)(GetHealthMult() * player.statLifeMax2 * GetHealthPerHeart() / 20) + 10, 10, int.MaxValue);
                }
                */
                    player.statManaMax2 = (int)(player.statManaMax2 * GetManaPerStar() / 20) + 10;
                    player.statDefense = (int)(GetDefenceMult() * player.statDefense * GetArmorMult());
                    player.meleeDamage *= GetDamageMult(DamageType.Melee, 2);
                    player.thrownDamage *= GetDamageMult(DamageType.Throw, 2);
                    player.rangedDamage *= GetDamageMult(DamageType.Ranged, 2);
                    player.magicDamage *= GetDamageMult(DamageType.Magic, 2);
                    player.minionDamage *= GetDamageMult(DamageType.Summon, 2);




                    player.armorPenetration = Mathf.FloorInt(player.armorPenetration * GetArmorPenetrationMult());
                    player.armorPenetration += GetArmorPenetrationAdd();

                    manaShieldDelay = Mathf.Clamp(manaShieldDelay - 1, 0, manaShieldDelay);
                    player.manaCost *= (float)Math.Sqrt(GetDamageMult(DamageType.Magic, 2));


                    ManaRegenPerSecond = Mathf.FloorInt(GetManaRegen());
                    float manaRegenTick = ManaRegenPerSecond / 60 + ManaRegenBuffer;
                    int manaRegenThisTick = Mathf.Clamp(Mathf.FloorInt(manaRegenTick), 0, int.MaxValue);
                    ManaRegenBuffer = Mathf.Clamp(manaRegenTick - manaRegenThisTick, 0, int.MaxValue);
                    player.statMana = Mathf.Clamp(player.statMana + manaRegenThisTick, player.statMana, player.statManaMax2);
                }
            }
        }

        

        public void ResetStats()
        {
            totalPoints+=1000;
            Stats.Reset(level);
            freePoints = totalPoints;
        }

        public float GetCriticalChanceBonus()
        {
            float X = Mathf.Pow(GetStatImproved(Stat.悟性) * statMultiplier + GetStatImproved(Stat.功法) * statMultiplier, 0.8f) * 0.05f;
            Mathf.Clamp(X, 0, 75);
            return X;
        }
        public float GetCriticalDamage()
        {
            float X = Mathf.Pow(GetStatImproved(Stat.道心) * statMultiplier + GetStatImproved(Stat.气运) * statMultiplier, 0.8f) * 0.005f;
            return 1.4f + X;
        }

       
        public float GetBonusHeal()
        {
            if (Config.gpConfig.RPGPlayer)
                return GetHealthPerHeart() / 20;
            return 1;
        }
        public float GetBonusHealMana()
        {
            if (Config.gpConfig.RPGPlayer)
                return (float)Math.Sqrt(GetManaPerStar() / 20);
            return 1;
        }

        public float GetHealthRegen()
        {
            float RegenMultiplier = 1f;
            return (GetStatImproved(Stat.灵根) + GetStatImproved(Stat.魅力)) * 0.02f * statMultiplier * RegenMultiplier;
        }
        public float GetManaRegen()
        {
            float RegenMultiplier = 1f;
            if (player.manaRegenDelay > 0)
                RegenMultiplier *= 0.5f;

            return (GetStatImproved(Stat.体质) + GetStatImproved(Stat.力量)) * 0.02f * statMultiplier * RegenMultiplier;
        }



        public bool HaveRangedWeapon()
        {
            if (player.HeldItem != null && player.HeldItem.damage > 0 && player.HeldItem.maxStack <= 1)
            {
                return player.HeldItem.ranged;
            }
            return false;
        }
        public bool HaveBow()
        {
            if (HaveRangedWeapon())
            {
                if (player.HeldItem.useAmmo == 40)
                    return true;
            }
            return false;
        }
        public float GetHealthMult()
        {
            float mult = 1;
            return mult;
        }
        public float GetArmorMult()
        {
            float mult = 1;
            return mult;
        }
        public float GetDamageMult(DamageType type, int skill = 0)
        {
            switch (type)
            {
                case DamageType.Magic:
                    return (GetStatImproved(Stat.体质) * MAINSTATSMULT + GetStatImproved(Stat.力量) * SECONDARYTATSMULT) * statMultiplier + 0.8f;
                case DamageType.Ranged:
                    return (GetStatImproved(Stat.道心) * MAINSTATSMULT + GetStatImproved(Stat.功法) * SECONDARYTATSMULT) * statMultiplier + 0.8f;
                case DamageType.Summon:
                    return (GetStatImproved(Stat.力量) * MAINSTATSMULT + GetStatImproved(Stat.悟性) * SECONDARYTATSMULT) * statMultiplier + 0.8f;
                case DamageType.Throw:
                    return (GetStatImproved(Stat.功法) * MAINSTATSMULT + GetStatImproved(Stat.气运) * SECONDARYTATSMULT) * statMultiplier + 0.8f;
                case DamageType.Symphonic:
                    return (GetStatImproved(Stat.道心) * SECONDARYTATSMULT + GetStatImproved(Stat.悟性) * SECONDARYTATSMULT) * statMultiplier + 0.8f;
                case DamageType.Radiant:
                    return (GetStatImproved(Stat.体质) * SECONDARYTATSMULT + GetStatImproved(Stat.力量) * SECONDARYTATSMULT) * statMultiplier + 0.8f;
                case DamageType.KI:
                    return (GetStatImproved(Stat.力量) * MAINSTATSMULT + GetStatImproved(Stat.气运) * SECONDARYTATSMULT) * statMultiplier + 0.8f;
                default:
                    return (GetStatImproved(Stat.气运) * MAINSTATSMULT + GetStatImproved(Stat.道心) * SECONDARYTATSMULT) * statMultiplier + 0.8f;
            }
        }


        public void CheckExp()
        {
            int actualLevelGained = 0;
            while (Exp >= XPToNextLevel())
            {

                actualLevelGained++;
                Exp -= XPToNextLevel();
                LevelUp();
                if (actualLevelGained > 5)
                {
                    Exp = 0;
                }
            }
        }

        int ReduceExp(int xp, int _level)
        {
            int exp = xp;
            if (_level <= level - 5)
            {
                float expMult = 1 - (level - _level) * 0.1f;
                exp = (int)(exp * expMult);
            }

            if (exp < 1)
                exp = 1;

            return exp;
        }
        public void AddXp(int exp, int _level)
        {
            if (Config.gpConfig.RPGPlayer)
            {



                exp = ReduceExp(exp, _level);

                if (level >= 1000)
                {

                    if (!XpLimitMessage)
                    {
                        XpLimitMessage = true;
                        Main.NewText("You character has Reached the limit of his mortal body and is unable to gain more power !");
                    }
                    exp = 0;
                }

                if (exp >= XPToNextLevel() * 0.1f)
                {
                    CombatText.NewText(player.getRect(), new Color(50, 26, 255), exp + " XP !!");
                }
                else
                {
                    CombatText.NewText(player.getRect(), new Color(127, 159, 255), exp + " XP");
                }



                Exp += exp;
                CheckExp();
            }
        }
        public void commandLevelup()
        {
            LevelUp(true);
        }

        public void ResetLevel()
        {
            totalPoints = 0;
            freePoints = 0;
            level = 1;
            Stats.Reset(1);
        }

        public void RecalculateStat()
        {
            int _level = level;
            level = 0;
            totalPoints = 0;
            freePoints = 0;
            Stats = new RPGStats();
            for (int i = 0; i < _level; i++)
            {
                LevelUp(true);
            }

        }

        public float GetLifeLeech(int damage)
        {
            float value = 0;
            value *= player.statLifeMax2;
            return value;
        }
        public float GetManaLeech()
        {
            float value = 0;
            return value;
        }


        private void LevelUpMessage(int pointsToGain)
        {
            CombatText.NewText(player.getRect(), new Color(255, 25, 100), "LEVEL UP !!!!");
            CombatText.NewText(player.getRect(), new Color(255, 125, 255), "+1 SKILL POINTS", true);
            CombatText.NewText(player.getRect(), new Color(150, 100, 200), "+" + pointsToGain + " Ability points", true);
            Main.NewText(player.name + " Is now level : " + level.ToString() + " .Congratulation !", 255, 223, 63);
        }

        private void LevelUp(bool silent = false)
        {
            int pointsToGain = 5 + Mathf.FloorInt(Mathf.Pow(level, 0.5f));
            totalPoints += pointsToGain;
            freePoints += pointsToGain;
            Stats.OnLevelUp();
            level++;
            if (!silent)
                LevelUpMessage(pointsToGain);

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                SendClientChanges(this);
            }
        }
        public int XPToNextLevel()
        {
            return 15 * level + 5 * Mathf.CeilInt(Mathf.Pow(level, 1.8f)) + 40;
        }

        public int[] ConvertStatToInt()
        {
            int[] convertedStats = new int[8];
            for (int i = 0; i < 8; i++)
            {
                convertedStats[i] = GetAddStat((Stat)i);
            }
            return convertedStats;
        }

        public int[] ConvertStatXPToInt()
        {
            int[] convertedStats = new int[8];
            for (int i = 0; i < 8; i++)
            {
                convertedStats[i] = GetStatXP((Stat)i);
            }
            return convertedStats;
        }

        void LoadStats(int[] _level, int[] _xp)
        {
            if (_xp.Length != 8) //if save is not correct , will try to port
            {

                RecalculateStat();
                if (_level.Length != 8) //if port don't work
                {
                    return;
                }
                for (int i = 0; i < 8; i++)
                {
                    Stats.UpgradeStat((Stat)i, _level[i]);
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    Stats.SetStats((Stat)i, level + 3, _level[i], _xp[i]);
                }
            }

        }

      


        public override TagCompound Save()
        {
            if (Stats == null)
            {
                Stats = new RPGStats();
            }
            return new TagCompound {
                {"Exp", Exp},
                {"level", level},
                {"Stats", ConvertStatToInt()},
                {"StatsXP", ConvertStatXPToInt()},
                {"totalPoints", totalPoints},
                {"freePoints", freePoints},
            };
        }
        public override void Initialize()
        {
            if (Stats == null)
                Stats = new RPGStats();

        }

        public override void Load(TagCompound tag)
        {
            Exp = tag.GetInt("Exp");
            level = tag.GetInt("level");
            LoadStats(tag.GetIntArray("Stats"), tag.GetIntArray("StatsXP"));
            totalPoints = tag.GetInt("totalPoints");
            freePoints = tag.GetInt("freePoints");
            initiated = true;
        }

    }
}
