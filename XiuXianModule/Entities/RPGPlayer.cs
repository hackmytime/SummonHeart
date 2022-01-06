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
using SummonHeart.Utilities;
using SummonHeart.Projectiles.XiuXian;
using SummonHeart.Extensions;
using SummonHeart.XiuXianModule.EnumType;
using SummonHeart.Projectiles.XiuXian.Weapon;
using SummonHeart.Projectiles.Range.Arrows;
using SummonHeart.Projectiles.Magic;

namespace SummonHeart.XiuXianModule.Entities
{
    internal class RPGPlayer : ModPlayer
    {
        public readonly static ushort ACTUALSAVEVERSION = 2;
        private float Exp = 0;
        private RPGStats Stats;

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
        public int lifeHeal = 0;
        public float baseLifeHeal = 0;
        public float lingli;
        public float lingliMax;
        private int linliHealCD = 0;
        public float danYaoMult = 1f;
        public int age = 0;
        public int life = 60 * 360 * 24;
        private int ageCD = 0;

        public float lingliDamageAdd;
        public float lingliDamageMult;
        public float baseDefMult;
        public float lingliDamageKnockback;
        public int lingliDamageCrit;

       

        //灵技
        public int IcicleCount = 0;
        public int IcicleCountMax = 108;
        private int icicleCD = 0;
        public bool onIceAttack;

        public override void ResetEffects()
        {
            //onIceAttack = false;
            danYaoMult = 1f;
            lifeHeal = 0;
            lingliDamageAdd = 0;
            baseLifeHeal = 0;
        }

        public void FrostEffect()
        {
            if (player.whoAmI == Main.myPlayer && onIceAttack)
            {
                if (icicleCD <= 0 && IcicleCount < IcicleCountMax && player.ownedProjectileCounts[ModContent.ProjectileType<FrostIcicle>()] < IcicleCountMax)
                {
                    IcicleCount+=3;
                    if (IcicleCount > IcicleCountMax)
                        IcicleCount = IcicleCountMax;

                    //kill all current ones
                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        Projectile proj = Main.projectile[i];

                        if (proj.active && proj.type == ModContent.ProjectileType<FrostIcicle>() && proj.owner == player.whoAmI)
                        {
                            proj.active = false;
                            proj.netUpdate = true;
                        }
                    }

                    //respawn in formation
                    int proCount = player.ownedProjectileCounts[ModContent.ProjectileType<FrostIcicle>()];
                    for (int k = 1; k <= getIceLevel(IcicleCount); k++)
                    {
                        int count = 0;
                        if (k < getIceLevel(IcicleCount))
                            count = 8 * k;
                        else
                            count = getIceLastLevelCount(IcicleCount);
                        for (int i = 0; i < count; i++)
                        {
                            int v = 360 / count;
                            float radians = i * v * (float)(Math.PI / 180);
                            Vector2 center = player.Center;
                            Projectile frost = SHUtils.NewProjectileDirectSafe(center, Vector2.Zero, ModContent.ProjectileType<FrostIcicle>(), 0, 0f, player.whoAmI, 5, radians);
                            frost.netUpdate = true;
                            frost.ai[0] = 40f * k;
                        }
                    }

                    float dustScale = 1.5f;

                    if (IcicleCount % 10 == 0)
                    {
                        dustScale = 3f;
                    }

                    //dust
                    /*for (int j = 0; j < 20; j++)
                    {
                        Vector2 vector6 = Vector2.UnitY * 5f;
                        vector6 = vector6.RotatedBy((j - (20 / 2 - 1)) * 6.28318548f / 20) + player.Center;
                        Vector2 vector7 = vector6 - player.Center;
                        int d = Dust.NewDust(vector6 + vector7, 0, 0, 15);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].scale = dustScale;
                        Main.dust[d].velocity = vector7;

                        if (IcicleCount % 10 == 0)
                        {
                            Main.dust[d].velocity *= 2;
                        }
                    }*/

                    icicleCD = 20;
                }

                if (icicleCD > 0)
                    icicleCD--;
            }

            if (IcicleCount >= 1 && !onIceAttack)
            {
                int dmg = 50;

                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile proj = Main.projectile[i];

                    if (proj.active && proj.type == ModContent.ProjectileType<FrostIcicle>() && proj.owner == player.whoAmI)
                    {
                        Vector2 vel = (Main.MouseWorld - proj.Center).SafeNormalize(-Vector2.UnitY) * 20f;

                        int p = Projectile.NewProjectile(proj.Center, vel, 116, dmg, 1f, player.whoAmI);
                        if (p != Main.maxProjectiles)
                        {
                            Main.projectile[p].timeLeft = 60 * 6;
                            Main.projectile[p].melee = false;
                            Main.projectile[p].magic = false;
                            Main.projectile[p].tileCollide = false;
                            Main.projectile[p].ignoreWater = true;
                            Main.projectile[p].penetrate = 6;
                            //Main.projectile[p].GetGlobalProjectile<FargoGlobalProjectile>().CanSplit = false;
                            //Main.projectile[p].GetGlobalProjectile<FargoGlobalProjectile>().FrostFreeze = true;
                        }

                        proj.Kill();
                    }
                }

                IcicleCount = 0;
            }
        }


        public int XpForLevel(int level)
        {
            return 8 * level;
        }
        public int getIceLevel(int i)
        {
            int level = 1;
            while (i > XpForLevel(level))
            {
                i -= XpForLevel(level);
                level = level + 1;
            }
            return level;
        }

        public int getIceLastLevelCount(int i)
        {
            int level = 1;
            while (i > XpForLevel(level))
            {
                i -= XpForLevel(level);
                level = level + 1;
            }
            return i;
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

        public override void PostUpdateMiscEffects()
        {
            FrostEffect();
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            if (mp.PlayerClass == 9)
            {
                lingliMax = GetLinliMax();
                lingliDamageMult = GetLinliDamageMult();
                baseDefMult = GetBaseDefMult();
                baseLifeHeal = GetBaseLifeHeal();
                //player.statLifeMax2 = Mathf.Clamp((int)(GetHealthMult() * player.statLifeMax2 * GetHealthPerHeart() / 20) + 10 * GetLevel(), 10, int.MaxValue);
                player.statLifeMax2 = Mathf.FloorInt(lingliMax / 2 + GetHealthAdd());
                //player.statDefense = (int)(GetDefenceMult() * player.statDefense * GetArmorMult());
                lingliDamageAdd += Mathf.FloorInt(lingliMax / 20 * lingliDamageMult);
                player.statDefense += Mathf.FloorInt(lingliDamageAdd * 0.8 * baseDefMult);
                if(level > 10)
                {
                    player.noKnockback = true;
                    player.noFallDmg = true;
                }

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
                            if (FireAge)
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
                    //回血
                    int heal = lifeHeal + (int)baseLifeHeal / 5;
                    if (player.statLife < player.statLifeMax2)
                    {
                        if (heal < 1)
                            heal = 1;
                        player.statLife += heal;
                        player.HealEffect(heal);
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
            double reply = (1 + GetStat(Stat.功法) * 0.1) * GetStat(Stat.灵根) * 0.025 * danYaoMult;
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
                return a + "年" + c + "天0" + d + "时";
            else
                return a + "年" + c + "天" + d + "时";
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

        public float XPToNextLevel()
        {
            if (level == 0)
                return 9;
            int a = level % 10;
            if(a == 0)
                return 20 * GetLinliMax();
            else
                return 10 * GetLinliMax();
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
                freePoints += 1;
                totalPoints += 1;
                return;
            }
            ammount = Mathf.Clamp(ammount, 1, freePoints);
            Stats.UpgradeStat(_stat, ammount);
            freePoints -= ammount;
        }

        public float GetDefenceMult()
        {
            return (GetStatImproved(Stat.灵根) * 0.0025f + GetStatImproved(Stat.魅力) * 0.006f) * statMultiplier + 1f;
        }

        public float GetHealthAdd()
        {
            return GetStat(Stat.体质) * 10f;
        }

        public float GetBaseLifeHeal()
        {
            return GetStat(Stat.体质) * 2f;
        }

        public float GetBaseDefMult()
        {
            return GetStat(Stat.体质) * 0.1f + 1;
        }

        public int GetItemDropMult()
        {
            return Mathf.FloorInt(GetStat(Stat.气运) * 0.05f);
        }

        public float GetLinjiXiuLianAddMult()
        {
            return GetStat(Stat.悟性) * 0.05f + 1;
        }

        public float GetLinliDamageMult()
        {
            return GetStat(Stat.力量) * 0.1f + 1;
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

       
       
        public float GetBonusHealMana()
        {
            if (Config.gpConfig.RPGPlayer)
                return (float)Math.Sqrt(GetManaPerStar() / 20);
            return 1;
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


        private void LevelUpMessage(int addLife, int pointsToGain)
        {
            CombatText.NewText(player.getRect(), new Color(255, 25, 100), "境界突破 !!!!");
            CombatText.NewText(player.getRect(), Color.LightGreen, "+"+ addLife + "年寿元");
            CombatText.NewText(player.getRect(), new Color(150, 100, 200), "+" + pointsToGain + "道源", true);
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
                int a = level / 10;
                int b = level % 10;
                
                int pointsToGain = Mathf.FloorInt(Mathf.Pow(2, a));
                if (b == 0)
                    pointsToGain *= 100;
                else
                    pointsToGain *= b;
                totalPoints += pointsToGain;
                freePoints += pointsToGain;
                Stats.OnLevelUp();
                int addLife = 0;
               
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
                    LevelUpMessage(addLife, pointsToGain);
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

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            SendClientChanges(this);
            if (WorldManager.instance != null)
                WorldManager.instance.NetUpdateWorld();
            base.SyncPlayer(toWho, fromWho, newPlayer);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            if (clientPlayer == null || level <= 0)
                return;

            if (Config.gpConfig.RPGPlayer)
            {
                ModPacket packet = mod.GetPacket();
                packet.Write((byte)Message.SyncLevel);
                packet.Write(player.whoAmI);
                packet.Write(level);
                packet.Write(player.statLife);
                packet.Write(player.statLifeMax2);
                packet.Send();
            }
            base.SendClientChanges(clientPlayer);
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
                {"lingli", lingli},
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
            lingli = tag.GetFloat("lingli");
            LoadStats(tag.GetIntArray("Stats"), tag.GetIntArray("StatsXP"));
            totalPoints = tag.GetInt("totalPoints");
            freePoints = tag.GetInt("freePoints");
            returnTime = tag.GetInt("returnTime");
        }

    }
}
