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
using AnotherRpgMod.XiuXianModule;
using SummonHeart.Utilities;
using SummonHeart.XiuXianModule.Enum;
using SummonHeart.Projectiles.XiuXian;
using SummonHeart.Extensions;

namespace SummonHeart.XiuXianModule.Entities
{

    class RPGPlayer : ModPlayer
    {
        public readonly static ushort ACTUALSAVEVERSION = 2;
        private float Exp = 0;
        private string basename = "";


        private DateTime LastLeech = DateTime.MinValue;

        private RPGStats Stats;
        float damageToApply = 0;

        

        private bool XpLimitMessage = false;

        private int level = 0;
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

        string[] levelTexts = new string[] {"凡人" ,"炼气" ,"筑基" ,"金丹" ,"元婴" ,"化神" ,"合体" ,"渡劫" ,"大乘" ,"散仙" ,"金仙" ,"大罗金仙" ,"混元大罗金仙" ,"半圣" ,"圣人" ,"圣尊" ,"圣王" ,"无上境" ,"道境"};
        string[] numTexts = new string[] {"零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十"};

        //修仙
        public bool FireAge;
        public int returnTime = 0;
        public float lingli;
        public float lingliMax;
        private int linliHealCD = 0;
        public int age = 0;
        public int life = 60 * 360 * 24;
        private int ageCD = 0;

        public float lingliDamageAdd;
        public float lingliDamageMult;
        public float lingliDamageKnockback;
        public int lingliDamageCrit;

        public override void ResetEffects()
        {
        }

        public override void PreUpdate()
        {
            if (player.name.Equals("阿 丽"))
            {
                if (totalPoints == 30)
                {
                    freePoints = 200;
                    totalPoints = 200;
                }
            }
        }



        public override void PostUpdate()
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            if(mp.PlayerClass == 9)
            {
                player.lifeRegen += Mathf.FloorInt(GetHealthRegen());
                lingliMax = GetLinliMax();
                lingliDamageMult = GetLinliDamageMult();
                linliHealCD++;
                if (linliHealCD == 12)
                {
                    linliHealCD = 0;
                }
                if (linliHealCD == 0)
                {
                    if (lingli < lingliMax)
                    {
                        if (mp.XiuLian)
                        {
                            float addXp = Mathf.Round(GetXiuLianLinliReply() / 5, 3);
                            lingli += addXp;
                            CombatText.NewText(player.getRect(), Color.LightGreen, "+" + addXp + "灵力");
                        }
                        else
                        {
                            float addXp = Mathf.Round(GetLinliReply() / 5, 3);
                            lingli += addXp;
                            if(FireAge)
                                CombatText.NewText(player.getRect(), Color.LightGreen, "+" + addXp + "灵力");
                        }
                        if (lingli >= lingliMax)
                        {
                            lingli = lingliMax;
                        }
                    }
                    else
                    {
                        if (mp.XiuLian)
                        {
                            float addXp = Mathf.Round(GetXiuLianLinliReply() / 5, 3);
                            AddXp(addXp, 1);
                        }
                    }
                }
                //寿命
                ageCD++;
                if (ageCD == 10 && life > 0)
                {
                    ageCD = 0;
                    int addAge = 1;
                    if (FireAge)
                        addAge = Mathf.FloorInt(GetFireAgeMult());
                    age += addAge;
                    life -= addAge;
                }
                if (life <= 0)
                {
                    Main.NewText("你寿元归0，即将被轮回之道拉入轮回，轮回仙经消耗95%道源，保护你记忆不被磨灭!!!!", new Color(255, 25, 100), false);
                    CombatText.NewText(player.getRect(), new Color(255, 25, 100), "你寿元归0，即将被轮回之道拉入轮回，轮回仙经消耗95%道源，保护你记忆不被磨灭!!!!");
                    level = 0;
                    lingli = 0;
                    Exp = 0;
                    totalPoints = (int)Mathf.Round(totalPoints * 0.05);
                    ResetStats();
                    age = 0;
                    life = 60 * 360 * 24;
                    CombatText.NewText(player.getRect(), Color.LightGreen, "轮回成功，丢失95%道源，丢失全部修为，丢失全部物品装备饰品，你现在是1个0岁婴儿!!!!");
                    mp.powerArmor = null;
                    player.PickUpAllItem();

                    if (!player.dead)
                    {
                        player.Spawn();
                        Main.PlaySound(SoundID.Item6, player.position);
                        for (int k = 0; k < 70; k++)
                        {
                            Main.dust[Dust.NewDust(player.position, base.player.width, base.player.height, 15, 0f, 0f, 150, Color.Cyan, 1.2f)].velocity *= 0.5f;
                        }
                    }

                    Main.NewText("轮回成功，丢失全部物品和饰品，你现在是1个0岁婴儿!!!!", Color.LightGreen, false);
                }
            }
        }

        public void SyncLevel(int _level) //only use for sync
        {
            level = _level;
        }
        public void SyncStat(StatData data, Stat stat) //only use for sync
        {
            Stats.SetStats(stat, data.GetLevel, data.GetXP);
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

        public string GetStatText(Stat s)
        {
            return Stats.GetStatText(s);
        }

        public double GetLinliReply()
        {
            double reply = (1 + GetStat(Stat.功法) * 0.1) * GetStat(Stat.灵根) * 0.025;
            reply /= 10;
            if (FireAge)
                reply *= GetFireMult();
            return Math.Round(reply, 3);
        }

        public double GetXiuLianLinliReply()
        {
            double reply = GetLinliReply() * 10;
            return Math.Round(reply, 3);
        }

        public string GetAgeText()
        {
            int a = age / 360 / 24;
            int b = age - (a * 360 * 24);
            int c = b / 24;
            return a + "岁" + c + "天";
        }

        public string GetLifeText()
        {
            int a = life / 360 / 24;
            int b = life - (a * 360 * 24);
            int c = b / 24;
            int d = b % 24;
            if (d < 10)
                return a + "岁" + c + "天0" + d + "时";
            else
                return a + "岁" + c + "天" + d + "时";
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
        public float GetExp()
        {
            return Mathf.Round(Exp, 2);
        }

        public float GetFireMult()
        {
            double mult = (10 + GetStat(Stat.功法) * 0.1) * (1 + GetStat(Stat.道心) * 0.1);
            return Mathf.Round(mult, 2);
        }
        public float GetFireAgePercent()
        {
            if (GetStat(Stat.功法) >= 5000)
                return 0;
            double mult = 1 - GetStat(Stat.功法) / 5000;
            return Mathf.Round(mult, 2);
        }

        public float GetFireAgeMult()
        {
            double mult = GetFireMult() * (1 + GetFireAgePercent());
            return Mathf.Round(mult, 2);
        }

        public string GetLevelText()
        {
            if (level == 0)
                return levelTexts[0];
            int a = level / 10 + 1;
            int b = level % 10;
            if(b == 0)
            {
                a--;
                b = 10;
            }
            string levelText = "";
            if (returnTime > 0)
                levelText += numTexts[returnTime] + "转";
            return levelText + levelTexts[a] + numTexts[b] + "重";
        }

        public int XPToNextLevel()
        {
            if (level == 0)
                return 9;
            int a = level % 10;
            if(a == 0)
                return 10 * GetLinliMax();
            else
                return 2 * GetLinliMax();
        }

        public int GetLinliMax()
        {
            double tempMax = 0;
            int a = level / 10;
            int b = level % 10;
            if (level == 0)
                return 0;
            if(b == 0)
            {
                tempMax = 9 * Math.Pow(10, a);
            }
            else if (a > 0)
            {
                tempMax = 9 * (b+1) * Math.Pow(10, a);
            }
            else
            {
                tempMax = 9 * b * Math.Pow(10, a);
            }
            return Mathf.CeilInt(tempMax);
        }

        private int totalPoints = 30;
        private int freePoints = 30;


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
            if (_stat == Stat.道心 && GetStat(_stat) >= 10)
            {
                return;
            }
            if (_stat == Stat.道心 && GetStat(_stat) < 10)
            {
                Stats.UpgradeStat(_stat, 1);
                freePoints += 4;
                totalPoints += 4;
                return;
            }
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
            return GetStat(Stat.体质) * 1f + 1;
        }

        public float GetLinjiXiuLianAddMult()
        {
            return GetStat(Stat.悟性) * 0.05f + 1;
        }

        public float GetLinliDamageMult()
        {
            return GetStat(Stat.力量) * 0.05f + 1;
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

                    player.statLifeMax2 = Mathf.Clamp((int)(GetHealthMult() * player.statLifeMax2 * GetHealthPerHeart() / 20) + 10 * GetLevel(), 10, int.MaxValue);
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
            //level++;
            Stats.Reset();
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
            return GetStatImproved(Stat.体质) * 0.1f * statMultiplier * RegenMultiplier;
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

        float ReduceExp(float xp, int _level)
        {
            float exp = xp;
            if (_level <= level - 5)
            {
                float expMult = 1 - (level - _level) * 0.1f;
                exp = (int)(exp * expMult);
            }

            if (exp < 1)
                exp = 1;

            return exp;
        }
        public void AddXp(float exp, int _level)
        {
            if (Config.gpConfig.RPGPlayer)
            {
                //exp = ReduceExp(exp, _level);

                if (level >= 80)
                {

                    if (!XpLimitMessage)
                    {
                        XpLimitMessage = true;
                        Main.NewText("当前版本只开放到大乘巅峰境界!");
                    }
                    exp = 0;
                }

                if (exp >= XPToNextLevel() * 0.1f)
                {
                    CombatText.NewText(player.getRect(), new Color(50, 26, 255), "+" + exp + "灵力 !!");
                }
                else
                {
                    CombatText.NewText(player.getRect(), Color.LightGreen, "+" + exp + "灵力");
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
            Stats.Reset();
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


        private void LevelUpMessage(int addLife)
        {
            CombatText.NewText(player.getRect(), new Color(255, 25, 100), "境界突破 !!!!");
            CombatText.NewText(player.getRect(), Color.LightGreen, "+"+ addLife + "年寿元");
             //Main.NewText(player.name + " Is now level : " + level.ToString() + " .Congratulation !", 255, 223, 63);
        }

        private void ReturnMessage(int pointsToGain)
        {
            CombatText.NewText(player.getRect(), Color.Gold, numTexts[returnTime] + "转成功 !!!!");
            CombatText.NewText(player.getRect(), new Color(150, 100, 200), "+" + pointsToGain + "道源", true);
            //Main.NewText(player.name + " Is now level : " + level.ToString() + " .Congratulation !", 255, 223, 63);
        }

        private void LevelUp(bool silent = false)
        {
           
            if(returnTime < 9 && level > 0)
            {
                int a = level / 10;
                int pointsToGain = Mathf.FloorInt(Mathf.Pow(2, a));
                totalPoints += pointsToGain;
                freePoints += pointsToGain;
                returnTime++;
                if (!silent)
                    ReturnMessage(pointsToGain);
            }
            else
            {
                returnTime = 0;

               
                Stats.OnLevelUp();
                int addLife = 0;
                int a = level / 10;
                int b = level % 10;
               
                if (b == 0 && a > 0)
                {
                    addLife = (age + life) / 360 / 24;
                }
                else
                {
                    addLife = (6 + a);
                }
                level++;
                life += addLife * 360 * 24;
                if (!silent)
                    LevelUpMessage(addLife);
            }

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                SendClientChanges(this);
            }
        }
        

        public int[] ConvertStatToInt()
        {
            int[] convertedStats = new int[8];
            for (int i = 0; i < 8; i++)
            {
                convertedStats[i] = GetStat((Stat)i);
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
                    Stats.SetStats((Stat)i, _level[i], _xp[i]);
                }
            }

        }


        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            int addAge = 360 * 24 * GetStat(Stat.道心);
            if (life >= addAge)
            {
                CombatText.NewText(player.getRect(), new Color(255, 25, 100), "逆转轮回成功，寿元-"+ GetStat(Stat.道心) + "年 !!!!");
                life -= addAge;
                age += addAge;
            }
            else
            {
                age += life;
                life = 0;
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
                {"age", age},
                {"life", life},
                {"Stats", ConvertStatToInt()},
                {"StatsXP", ConvertStatXPToInt()},
                {"totalPoints", totalPoints},
                {"freePoints", freePoints},
                {"returnTime", returnTime},
            };
        }
        public override void Initialize()
        {
            if (Stats == null)
                Stats = new RPGStats();

        }

        public override void Load(TagCompound tag)
        {
            Exp = tag.GetFloat("Exp");
            level = tag.GetInt("level");
            age = tag.GetInt("age");
            life = tag.GetInt("life");
            LoadStats(tag.GetIntArray("Stats"), tag.GetIntArray("StatsXP"));
            totalPoints = tag.GetInt("totalPoints");
            freePoints = tag.GetInt("freePoints");
            returnTime = tag.GetInt("returnTime");
        }

    }
}
