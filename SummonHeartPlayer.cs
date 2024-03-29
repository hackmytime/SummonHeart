﻿using Microsoft.Xna.Framework;
using SummonHeart.Buffs.XiuXian;
using SummonHeart.Effects.Animations.Aura;
using SummonHeart.Extensions;
using SummonHeart.Items.Material;
using SummonHeart.Items.Range;
using SummonHeart.Items.Weapons.Magic;
using SummonHeart.Models;
using SummonHeart.Projectiles.Summon;
using SummonHeart.Projectiles.XiuXian;
using SummonHeart.XiuXianModule.Entities;
using SummonHeart.ui;
using SummonHeart.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static SummonHeart.SummonHeartMod;

namespace SummonHeart
{
    public class SummonHeartPlayer : ModPlayer
	{	
		public bool SummonHeart = false;
		public bool killAnyBoss = false;
		public bool DropOresTwice = false;
		public bool MysteriousCrystal = false;
		public int PlayerClass = 0;
		public int deathCount = 0;
		public int buffMaxCount = 1;
		public int killNpcCount = 0;
		public int fishCount = 0;
		public int nextFishCount = 0;
		public bool autoAttack = false;
		public float AttackSpeed;
		public float tungstenPrevSizeSave;
		public bool FishSoul = false;
		public bool BattleCry = false;		
		public bool llPet = false;
		public bool chargeAttack = false;
		public bool showRadius = false;
		public int BBP = 0;
		public int SummonCrit = 0;
		public int exp;
		public int addLife = 0;
		public float bodyDef = 0;
		public int killResourceCurrent = 0;
		public int deathResourceCurrent = 0;
		public int deathResourceMax = 0;
		public int angerResourceCurrent = 0;
		public int angerResourceMax = 0;
		public int damageResourceCurrent = 0;
		public int damageResourceMax = 0;
		public bool onDoubleDamage = false;
		public bool onanger = false;
		public int killResourceMax;
		public int killResourceMax2;
		public int killResourceSkillCount = 0;
		public int killResourceSkillCountMax = 10;
		public int killResourceMulti;
		public bool inMagicCharging = false;
		public bool detonate = false;
		public bool magicChargeActive = false;
		public bool magicBook = false;
		public float magicCharge;
		public float magicChargeMax = 100;
		public float magicChargeCount = 0;
		public float magicChargeCountMax = 10;

		public bool bowChargeActive;
		public float bowCharge;
		public float bowChargeMax;
		public float bowChargeAttack;
		public int tornadoDamage;

		public Item powerArmor;

		public int eyeBloodGas = 0;
		public int handBloodGas = 0;
		public int bodyBloodGas = 0;
		public int footBloodGas = 0;

		public int eyeMax = 0;
		public int handMax = 0;
		public int bodyMax = 0;
		public int footMax = 0;

		public int bloodGasMax = 10000;

		public int addExp = 0;

		public bool practiceEye = false;
		public bool practiceHand = false;
		public bool practiceBody = false;
		public bool practiceFoot = false;
		public bool soulSplit = false;

		public int costMana;
		public bool accBuryTheLight;
		public int buryTheLightCooldown;
		public bool buryTheLightStarted;

		public int HealCount = 0;
		private int healCD = 0;
		private int bodyHealCD = 0;
		private int killHealCD = 0;
		private int damageCD = 0;

		public List<bool> boughtbuffList;

		public static int MaxExtraAccessories = 0;

		public Item[] ExtraAccessories;

		// animation helper fields
		public AuraAnimationInfo currentAura;
		public int lightningFrameTimer = 500000;
		public int auraFrameTimer = 0;
		public int auraCurrentFrame = 0;

		// 特效
		public Gradient oscGradient;
		public Color oscColor;
		public bool useOscColor;
		public bool useOscGradient;
		public bool colorsInitialized;

		//位移
		public int leftTimer;
		public bool dashingLeft;
		public int rightTimer;
		public bool dashingRight;
		public Vector2 RecallBackPos = Vector2.Zero;

		//属性
		public float MyAccelerationMult;
		public float MyMoveSpeedMult;
		public float MyCritDmageMult;
		public float myDamageReduceMult;
		public float enemyDamageReduceMult;
		public int addRealDamage;
		public int fishLureCount;
		public float manaExp;
		public float attackDamage;
		public int lifeSteal;

		//修仙
		public bool XiuLian;


		public SummonHeartPlayer()
		{
			var size = SummonHeartMod.getBuffLength();
			boughtbuffList = new List<bool>();
			for(int i = 1; i <= size; i++)
            {
				boughtbuffList.Add(false);
            }
		}

        public override void Initialize()
        {
			this.ExtraAccessories = new Item[50];
			for (int i = 0; i < 50; i++)
			{
				this.ExtraAccessories[i] = new Item();
				this.ExtraAccessories[i].SetDefaults(0, true);
			}
		}

        public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff)
        {
			for (int i = 0; i < MaxExtraAccessories; i++)
			{
				base.player.VanillaUpdateEquip(this.ExtraAccessories[i]);
			}
			for (int j = 0; j < MaxExtraAccessories; j++)
			{
				base.player.VanillaUpdateAccessory(base.player.whoAmI, this.ExtraAccessories[j], false, ref wallSpeedBuff, ref tileSpeedBuff, ref tileRangeBuff);
			}
		}

		public override void PostUpdateEquips()
		{
			ModPlayerEffects.UpdatePoints(player);
		}

		public override void ResetEffects()
		{
			XiuLian = false;
			SummonHeart = false;
			MysteriousCrystal = false;
			AttackSpeed = 1f;
			FishSoul = false;
			llPet = false;
			healCD++;
			if (healCD == 60)
			{
				healCD = 0;
				HealCount = player.statLifeMax2 / 2;
			}
			bodyHealCD++;
			if (bodyHealCD == 15)
			{
				bodyHealCD = 0;
			}
			killHealCD++;
			if (killHealCD == 12)
			{
				killHealCD = 0;
			}
			damageCD--;
			if (damageCD < 0)
				damageCD = 0;
			killResourceMax2 = killResourceMax;
			damageResourceMax = 9999;
			addRealDamage = 0;
			fishLureCount = this.getFishLevel();
			manaExp = 1;
			attackDamage = 0;
			lifeSteal = 0;
			if(buffMaxCount > 100)
            {
				buffMaxCount = 100;
            }

			eyeMax = SummonHeartConfig.Instance.eyeMax;
			handMax = SummonHeartConfig.Instance.handMax;
			bodyMax = SummonHeartConfig.Instance.bodyMax;
			footMax = SummonHeartConfig.Instance.footMax;

			MyAccelerationMult = 1f;
			MyMoveSpeedMult = 1f;
			MyCritDmageMult = 1f;
			costMana = (handBloodGas / 33333) * (handBloodGas / 33333) * 10 + 5;

			if (SummonHeartWorld.GoddessMode)
			{
				MaxExtraAccessories = 16;
			}
			else
			{
				MaxExtraAccessories = SummonHeartWorld.WorldLevel;
				if (SummonHeartWorld.WorldLevel == 5)
					MaxExtraAccessories += 3;
			}
			//inMagicCharging = false;
			magicBook = false;
			powerArmor = null;
			bowChargeActive = false;
			//刷新上限
			ModPlayerEffects.UpdateMax(player);
			//减伤倍率
			myDamageReduceMult = 1f;
			enemyDamageReduceMult = SummonHeartWorld.WorldLevel * 2;
			if (SummonHeartWorld.GoddessMode)
            {
				enemyDamageReduceMult *= 2;
			}
			//魔神的庇佑
			if (player.getDownedBossIndex() > 0)
				killAnyBoss = true;
			if(!killAnyBoss)
				player.AddBuff(mod.BuffType("DemonDefBuff"), 120);
		}

        internal object SH()
        {
            throw new NotImplementedException();
        }

        public override void PreUpdate()
		{
			if (player.AnyBossAlive() && SummonHeartWorld.GoddessMode)
			{
				player.respawnTimer = 60*60*5;
            }
			
			if (player.HasItemInAcc(mod.ItemType("MysteriousCrystal")) != -1 && base.player.respawnTimer > 120 && !player.AnyBossAlive())
			{
				player.respawnTimer = 120;
			}

			if (player.HasItemInAcc(mod.ItemType("MysteriousCrystal")) != -1)
			{
				player.UpdateCoins();
			}

			if (XiuLian)
			{
				/*player.immune = true;
				player.immuneTime = 2;
				player.hurtCooldowns[0] = 2;
				player.hurtCooldowns[1] = 2;
				player.stealth = 1;*/

				if (player.ownedProjectileCounts[ModContent.ProjectileType<XiuLianProj>()] <= 0)
					Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<XiuLianProj>(), 0, 0, Main.myPlayer);
			}

			if (Main.netMode != 2)
			{
				ModPlayerEffects.UpdateColors(player);
			}

            if (player.name.Equals("魔崽子"))
            {
				DropOresTwice = true;
            }
            else
            {
				DropOresTwice = false;
			}

			if(player.active && !player.dead && PlayerClass != 9)
            {
				if(infiniBuffDic.Keys.Count < buffMaxCount)
                {
					for (int i = 0; i < player.CountBuffs(); i++)
					{
						int type = player.buffType[i];
						if (!infiniBuffDic.Keys.Contains(type) && !SummonHeartMod.whiteBuffSet.Contains(type))
						{
							ModBuff modBuff = BuffLoader.GetBuff(type);
							if (Main.debuff[type] == false)
							{
								if (modBuff == null)
								{
									if (infiniBuffDic.Keys.Count < buffMaxCount)
										infiniBuffDic.Add(type, true);
								}
								else if (SummonHeartMod.posionBuffSet.Contains(type))
								{
									if (infiniBuffDic.Keys.Count < buffMaxCount)
										infiniBuffDic.Add(type, true);
								}
							}
						}
					}
				}

				foreach(var type in infiniBuffDic.Keys)
				{
					int buffId = type;
                    if (infiniBuffDic[type])
                    {
						player.AddBuff(buffId, 10, true);
						Main.buffNoTimeDisplay[type] = true;
						Main.persistentBuff[type] = true;
                    }
                    else
                    {
						if (player.HasBuff(buffId))
							player.ClearBuff(buffId);
					}
				}
			}
		}

        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
			int team = 1;
			if (player.team != team)
			{
				player.team = team;
				NetMessage.SendData(45, -1, -1, null, Main.myPlayer, 0f, 0f, 0f, 0, 0, 0);
			}
		}

        public override void PostUpdate()
		{
			currentAura = this.GetAuraEffectOnPlayer();
			IncrementAuraFrameTimers(currentAura);
			
            //特效
            if (XiuLian || (onDoubleDamage && damageResourceCurrent > 0))
            {
				int dustId = MyDustId.BlueMagic;
				for (int d = 0; d < 2; d++)
                {
					Vector2 vector = new Vector2(Main.rand.Next(-12, 12) * -9.88f, Main.rand.Next(-12, 12) * -9.88f);
					Dust dust2 = Main.dust[Dust.NewDust(player.MountedCenter + vector, 1, 1, 20, 0f, 0f, 255, new Color(0.8f, 0.4f, 1f), 0.8f)];
					dust2.velocity = -vector / 12f;
					dust2.velocity -= player.velocity / 8f;
					dust2.noLight = true;
					dust2.noGravity = true;
                }
			}
		}

		public override void PostUpdateMiscEffects()
        {
			if(player.endurance > 0.9f)
            {
				player.endurance = 0.9f;
			}
			//计算减伤倍率
			myDamageReduceMult += player.endurance * 10;
			player.endurance = 0f;
			if (PlayerClass == 1)
            {
				//战士·泰坦
				EffectMelee();
            }else if(PlayerClass == 2)
			{
				//刺客
				EffectKill();
			}
			else if (PlayerClass == 3)
			{
				//召唤
				EffectSummon();
			}
			else if (PlayerClass == 4)
			{
				//战士·狂战
				EffectMelee2();
			}
			else if (PlayerClass == 5)
			{
				//法师·法神
				EffectMagic();
			}
			else if (PlayerClass == 6)
			{
				//法师·控法者
				EffectMagic2();
			}
			else if (PlayerClass == 7)
			{
				//射手
				EffectRange();
			}
			//无限飞行
			if (footBloodGas >= 200000)
			{
				player.wingTime = 200 * 60;
			}

			EffectPowerArmor();
		}

        private void EffectPowerArmor()
        {
			//能量护甲
			Item item = player.GetItemInAcc(mod.ItemType("PowerArmor1"));
			if (item != null)
			{
				powerArmor = item;
			}
			item = player.GetItemInAcc(mod.ItemType("PowerArmor2"));
			if (item != null)
			{
				powerArmor = item;
			}
			item = player.GetItemInAcc(mod.ItemType("PowerArmor3"));
			if (item != null)
			{
				powerArmor = item;
			}
			item = player.GetItemInAcc(mod.ItemType("PowerArmor4"));
			if (item != null)
			{
				powerArmor = item;
			}
			item = player.GetItemInAcc(mod.ItemType("PowerArmor5"));
			if (item != null)
			{
				powerArmor = item;
			}
			item = player.GetItemInAcc(mod.ItemType("PowerArmor6"));
			if (item != null)
			{
				powerArmor = item;
			}
		}

        private void EffectRange()
        {
			if (ModLoader.GetMod("Luiafk") != null 
				|| ModLoader.GetMod("AlchemistNPC") != null
				|| ModLoader.GetMod("AlchemistNPCLite") != null
				|| ModLoader.GetMod("UsefulNPCs") != null
				|| ModLoader.GetMod("HelpfulNPCs") != null
			)
			{
				if (player.active && !player.dead && Main.time % 60 == 0)
				{
					player.AddBuff(BuffID.Frozen, 60, true);
					player.AddBuff(BuffID.Darkness, 60, true);
					player.AddBuff(BuffID.Blackout, 60, true);
					player.AddBuff(BuffID.Cursed, 60, true);
					player.AddBuff(BuffID.Cursed, 60, true);
					player.AddBuff(BuffID.OgreSpit, 60, true);
					player.AddBuff(BuffID.Webbed, 60, true);
					player.AddBuff(BuffID.Obstructed, 60, true);
					player.AddBuff(199, 60, true);
				}
				if ((int)Main.time % 60 < 1)
				{
					Main.NewText("射手职业禁止安装Luiafk、ANPC、ANPCLite、UsefulNPCs、HelpfulNPCs这几个获取无限资源的模组", Color.Red, false);
					Main.NewText("你违反了规定，受到了魔神的诅咒，请尽快卸载以上模组", Color.Red, false);
				}
			}

			// 眼
			/*if (player.HeldItem != null)
			{
				if (player.ownedProjectileCounts(mod.ProjectileType("MultiBowPro")) < 1)
				{
					for(int i = 1; i <= 5; i++)
                    {
                        int p = Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("MultiBowPro"), 0, 0f, player.whoAmI);
						Main.projectile[p].ai[1] = i;
                    }
				}
            }
            else
            {
				if (player.ownedProjectileCounts(mod.ProjectileType("MultiBowPro")) > 0)
				{
					player.ownedProjectileKill(mod.ProjectileType("MultiBowPro"));
				}
			}*/
		}

        public override void GetHealMana(Item item, bool quickHeal, ref int healValue)
        {
            base.GetHealMana(item, quickHeal, ref healValue);
        }

		private void EffectMagic2()
		{
			int allBlood = this.getAllBloodGas();
			player.statManaMax2 += allBlood / 40;

			player.manaRegen = 0;
			player.manaRegenCount = 0;
			player.manaRegenDelay = 99999999;
			if (player.manaCost == 0)
			{
				player.magicDamage = 0;
				if ((int)Main.time % 60 < 1)
				{
					Main.NewText("控法者禁止使用无限魔力，否则魔法攻击力变为0", Color.Red, false);
				}
			}

			//魔神之眼
			if (boughtbuffList[0])
			{
				MyCritDmageMult += eyeBloodGas / 500 * 0.01f + 1;
			}

			//魔神之手
			if (boughtbuffList[1])
			{
				magicChargeCountMax = handBloodGas / 2500 + 20;
            }
            else
            {
				magicChargeCountMax = 10;
			}

			//魔神之躯
			int heal = 2;
			if (boughtbuffList[2])
			{
				player.noKnockback = true;
				//计算被动
				heal = (int)(player.statManaMax2 * (0.01 + bodyBloodGas / 100000 * 0.01f)) / 4;
				if (heal < 2)
					heal = 2;
			}
			if (player.statMana < player.statManaMax2 && bodyHealCD == 1)
			{
				if (magicBook)
					heal *= 2;
				player.HealMana(heal);
			}

			//魔神之腿
			if (boughtbuffList[3])
			{
				player.noFallDmg = true;
				MyMoveSpeedMult += (footBloodGas / 5000 + 20) * 0.01f;
				MyAccelerationMult += (footBloodGas / 5000 + 20) * 0.01f;
				player.wingTimeMax += (footBloodGas / 2222 + 5) * 60;
				player.jumpSpeedBoost += (footBloodGas / 1000 + 60) * 0.01f;
				
			}
		}

		private void EffectMagic()
        {
            int allBlood = this.getAllBloodGas();
			player.statManaMax2 += allBlood / 20;
			player.manaRegen = 0;
			player.manaRegenCount = 0;
			player.manaRegenDelay = 99999999;
            if (player.manaCost == 0)
            {
                Mod Luiafk = ModLoader.GetMod("Luiafk");

				if (Luiafk != null)
                {
					player.manaCost = 9999999;
					if ((int)Main.time % 60 < 1)
					{
						Main.NewText("法神禁止使用无限魔力，否则魔法消耗增加9999999", Color.Red, false);
					}
                }
                else
                {
					player.manaCost = 500;
				}
			}

			//魔神之眼
			if (boughtbuffList[0])
			{
				player.magicCrit += eyeBloodGas / 2222 + 10;
				MyCritDmageMult += eyeBloodGas / 500 * 0.01f + 1;
			}

			//魔神之躯
			int heal = 2;
			if (boughtbuffList[2])
			{
				player.noKnockback = true;
				//计算被动
				heal = (int)(player.statManaMax2 * (0.01 + bodyBloodGas / 100000 * 0.01f)) / 4;
				if (heal < 2)
					heal = 2;
			}
			if (player.statMana < player.statManaMax2 && bodyHealCD == 1)
			{
				if (magicBook)
					heal *= 2;
				player.HealMana(heal);
			}

			//魔神之腿
			if (boughtbuffList[3])
			{
				player.noFallDmg = true;
				MyMoveSpeedMult += (footBloodGas / 5000 + 20) * 0.01f;
				MyAccelerationMult += (footBloodGas / 5000 + 20) * 0.01f;
				player.wingTimeMax += (footBloodGas / 2222 + 5) * 60;
				player.jumpSpeedBoost += (footBloodGas / 1000 + 60) * 0.01f;
				
			}
		}

        public override void ModifyManaCost(Item item, ref float reduce, ref float mult)
        {
            base.ModifyManaCost(item, ref reduce, ref mult);
        }

        private void EffectSummon()
		{
			// 眼
			if (boughtbuffList[0])
            {
				if (practiceEye && player.ownedProjectileCounts(mod.ProjectileType("Overgrowth")) < 1)
				{
					Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("Overgrowth"), 0, 0f, player.whoAmI);
                }
                if(!practiceEye && player.ownedProjectileCounts(mod.ProjectileType("Overgrowth")) > 0)
                {
					player.ownedProjectileKill(mod.ProjectileType("Overgrowth"));
                }
				MyCritDmageMult += eyeBloodGas / 500 * 0.01f + 1;
			}

			// 手
			if (boughtbuffList[1])
			{

			}

			// 躯
			if (boughtbuffList[2])
			{
				myDamageReduceMult = 99f;
                if (player.whoAmI == Main.myPlayer)
                {
					if (practiceBody && player.ownedProjectileCounts(ModContent.ProjectileType<EmpyreanSpectre>()) < 1)
						Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<EmpyreanSpectre>(), 0, 0f, player.whoAmI);
					if (!practiceBody && player.ownedProjectileCounts(ModContent.ProjectileType<EmpyreanSpectre>()) >= 1)
						player.ownedProjectileKill(ModContent.ProjectileType<EmpyreanSpectre>());
                }
				player.noFallDmg = true;
				MyMoveSpeedMult += 0.66f;
				MyAccelerationMult += 0.66f;
				player.maxMinions += (bodyBloodGas / 10000) + 3;
				player.statLifeMax2 /= 2;
				player.statManaMax2 += bodyBloodGas / 400 + 200;
			}

			// 腿
			if (boughtbuffList[3])
			{
				player.wingTimeMax += (footBloodGas / 1000 + 10) * 60;
				player.jumpSpeedBoost += (footBloodGas / 400 + 100) * 0.01f;
			}
		}

		private void EffectKill()
        {
			killResourceMax = 100;
			killResourceSkillCountMax = 10;
			//被动
			killResourceMax += killNpcCount;
			//计算上限
			int allBlood = this.getAllBloodGas();
			int curMax = 99999 + allBlood;
			if (allBlood >= 800000)
				curMax += 100000;
			if (killResourceMax > curMax)
				killResourceMax = curMax;
			deathResourceMax = killResourceMax / 100;
			killResourceMulti = 3 + killResourceMax / 44444;
			addRealDamage = killResourceMax / 99;
			//魔神之手
			if (boughtbuffList[1])
            {
				AttackSpeed += (handBloodGas / 4000 + 30) * 0.01f;
				player.thrownVelocity += (handBloodGas / 4000 + 30) * 0.01f;
				killResourceSkillCountMax = (handBloodGas / 5000 + 10);
			}
		
			int heal = 1;
			if (boughtbuffList[2])
            {
				//神通流
                heal = (int)(killResourceMax2 * (0.05 + eyeBloodGas / 50000 * 0.01f)) / 4;
				heal += (bodyBloodGas / 200 + 15) / 4;
				if (heal < 1)
					heal = 1;
            }
            else
            {
				heal = 2;
            }
			if (killResourceCurrent < killResourceMax2 && killHealCD == 0)
			{
				killResourceCurrent += heal;
				if (killResourceCurrent > killResourceMax2)
					killResourceCurrent = killResourceMax2;
			}
			
			//魔神之腿
			if (boughtbuffList[3])
			{
				player.noFallDmg = true;
				MyMoveSpeedMult += (footBloodGas / 5000 + 33) * 0.01f;
				MyAccelerationMult += (footBloodGas / 5000 + 33) * 0.01f;
				player.wingTimeMax += (footBloodGas / 1000 + 5) * 60;
				player.jumpSpeedBoost += (footBloodGas / 500 + 100) * 0.01f;
			}

			//位移
			if (this.dashingLeft)
			{
				this.dashingRight = false;
				this.rightTimer = 0;
				this.leftTimer++;
				if (this.leftTimer == 1)
				{
					base.player.controlLeft = true;
					base.player.releaseLeft = true;
					return;
				}
				if (this.leftTimer >= 2)
				{
					base.player.controlLeft = true;
					base.player.releaseLeft = true;
					this.leftTimer = 0;
					this.dashingLeft = false;
					return;
				}
			}
			else if (this.dashingRight)
			{
				this.dashingLeft = false;
				this.leftTimer = 0;
				this.rightTimer++;
				if (this.rightTimer == 1)
				{
					base.player.controlRight = true;
					base.player.releaseRight = true;
				}
				if (this.rightTimer >= 2)
				{
					base.player.controlRight = true;
					base.player.releaseRight = true;
					this.rightTimer = 0;
					this.dashingRight = false;
				}
			}
		}

        private void EffectMelee()
        {
			//泰坦
			player.statLifeMax2 += 300;
			player.jumpSpeedBoost -= 0.33f;
			player.aggro += 20000;
			player.statDefense += (int)bodyDef * 2;
			myDamageReduceMult += 1;
			//吸收伤害上限
			int allBlood = this.getAllBloodGas();
			damageResourceMax = 99999 + allBlood;
			if (allBlood >= 800000)
				damageResourceMax += 100000;
			//魔神之眼
			if (boughtbuffList[0])
            {
				player.meleeCrit += eyeBloodGas / 2222 + 10;
				MyCritDmageMult += eyeBloodGas / 1000 * 0.01f + 0.5f;
			}

			//魔神之手
			if (boughtbuffList[1])
			{
				AttackSpeed += (handBloodGas / 3333 + 10) * 0.01f;
			}

			//魔神之躯
			if (boughtbuffList[2])
			{
				myDamageReduceMult += (bodyBloodGas / 200 + 400) * 0.01f;
				player.noKnockback = true;
				player.statLifeMax2 += (bodyBloodGas / 200 + 300);
			
				int heal = (int)(player.statLifeMax2 * (0.01 + bodyBloodGas / 20000 * 0.01f)) / 4;
				if (player.statLife < player.statLifeMax2 && bodyHealCD == 1)
				{
					if (heal < 1)
						heal = 1;
					player.statLife += heal;
					player.HealEffect(heal);
				}
            }

			//魔神之腿
			if (boughtbuffList[3])
			{
				player.noFallDmg = true;
				MyMoveSpeedMult += (footBloodGas / 10000 + 20) * 0.01f;
				MyAccelerationMult += (footBloodGas / 10000 + 20) * 0.01f;
				player.wingTimeMax += (footBloodGas / 2222 + 5) * 60;
				player.jumpSpeedBoost += (footBloodGas / 1333 + 50) * 0.01f;
				/*if (footBloodGas >= 150000)
				{
					player.wingTime = footBloodGas / 1000 * 60;
				}*/
			}
		}

		private void EffectMelee2()
		{
			//狂战
			angerResourceMax = 100 + deathCount * 4;
			if (angerResourceMax > 500)
				angerResourceMax = 500;
			player.statDefense += (int)bodyDef;
			player.meleeCrit += angerResourceCurrent;
			if(angerResourceCurrent > 100)
            {
				MyCritDmageMult += (angerResourceCurrent - 100) / 2 * 0.01f;
			}
			//初始奖励
			MyMoveSpeedMult += 0.2f;
			MyAccelerationMult += 0.2f;
			AttackSpeed += 0.2f;
			player.jumpSpeedBoost += 0.33f;
			if (onanger && Main.time % 12 == 0)
			{
				angerResourceCurrent -= 5;
				CombatText.NewText(player.getRect(), Color.Red, "-" + 5 + "怒气值");
				if (angerResourceCurrent <= 0)
				{
					angerResourceCurrent = 0;
					player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " 燃尽怒火而死."), 7777, 0);
				}
			}

			//魔神之眼
			if (boughtbuffList[0])
			{
				player.meleeCrit += eyeBloodGas / 2222 + 10;
				MyCritDmageMult += eyeBloodGas / 500 * 0.01f + 1;
			}

			//魔神之手
			if (boughtbuffList[1])
			{
				//player.meleeDamage += handBloodGas / 200 * 0.01f;
				AttackSpeed += (handBloodGas / 2222 + 20) * 0.01f;
			}

			//魔神之躯
			if (boughtbuffList[2])
			{
				myDamageReduceMult += (bodyBloodGas / 200 + 100) * 0.01f;
				player.noKnockback = true;
				player.statLifeMax2 += bodyBloodGas / 200;
			}

			//魔神之腿
			if (boughtbuffList[3])
			{
				player.noFallDmg = true;
				MyMoveSpeedMult += (footBloodGas / 5000 + 33) * 0.01f;
				MyAccelerationMult += (footBloodGas / 5000 + 33) * 0.01f;
				player.wingTimeMax += (footBloodGas / 1000 + 5) * 60;
				player.jumpSpeedBoost += (footBloodGas / 500 + 100) * 0.01f;
				
			}
		}

		public struct SoundData
		{
			public int Type;
			public int x;
			public int y;
			public int Style;
			public float volumeScale;
			public float pitchOffset;
			public SoundData(int Type)
			{ this.Type = Type; x = -1; y = -1; Style = 1; volumeScale = 1f; pitchOffset = 0f; }
		}
		public static void ItemFlashFX(Player player, int dustType = 45, SoundData sDat = default(SoundData))
		{
			if (sDat.Type == 0) { sDat = new SoundData(25); }
			if (player.whoAmI == Main.myPlayer)
			{ 
				Main.PlaySound(sDat.Type, sDat.x, sDat.y, sDat.Style, sDat.volumeScale, sDat.pitchOffset); 
			}
			for (int i = 0; i < 5; i++)
			{
				int d = Dust.NewDust(
					player.position, player.width, player.height, dustType, 0f, 0f, 255,
					default(Color), (float)Main.rand.Next(20, 26) * 0.1f);
				Main.dust[d].noLight = true;
				Main.dust[d].noGravity = true;
				Main.dust[d].velocity *= 0.5f;
			}
		}
		

		public bool GetPratice(int currentBuffIndex)
		{
			bool praticeBool = false;
			if (currentBuffIndex == 0)
			{
				praticeBool = practiceEye;
			}
			else if (currentBuffIndex == 1)
			{
				praticeBool = practiceHand;
			}
			else if (currentBuffIndex == 2)
			{
				praticeBool = practiceBody;
			}
			else if (currentBuffIndex == 3)
			{
				praticeBool = practiceFoot;
			}
			else if (currentBuffIndex == 4)
			{
				praticeBool = soulSplit;
			}
			return praticeBool;
		}

		public void SetPratice(int index, bool flag)
		{
			if (index == 0)
			{
				practiceEye = flag;
			}
			else if (index == 1)
			{
				practiceHand = flag;
			}
			else if (index == 2)
			{
				practiceBody = flag;
			}
			else if (index == 3)
			{
				practiceFoot = flag;
			}
			else if (index == 4)
			{
				soulSplit = flag;
			}
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
				SendClientChanges(this);
			}
		}

		public bool CheckSoul(int count)
		{
			return BBP >= count;
		}

		public void BuySoul(int count)
		{
			BBP -= count;
			dealLevel();
		}

		public void dealLevel()
		{
			int lvExp = 1;
			int curExp = lvExp;
			int level = 0;
			while (curExp <= BBP)
			{
				curExp += lvExp;
				level++;
				lvExp += 10;
			}
			int needExp = curExp - BBP;
			this.exp = needExp;
			SummonCrit = level;
			if (!Main.hardMode && SummonCrit > 299)
				SummonCrit = 299;
			if (!NPC.downedMoonlord && Main.hardMode && SummonCrit > 499)
				SummonCrit = 500;
			if (NPC.downedMoonlord && SummonCrit >= 5000)
				SummonCrit = 5000;

		}

		public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
			Item item = new Item();
			item.SetDefaults(ModLoader.GetMod("SummonHeart").ItemType("GuideNote"));
			item.stack = 1;
			items.Add(item);
			item = new Item();
			item.SetDefaults(ModLoader.GetMod("SummonHeart").ItemType("Level0"));
			item.stack = 1;
			items.Add(item);
			item = new Item();
			item.SetDefaults(ModLoader.GetMod("SummonHeart").ItemType("DemonScroll"));
			item.stack = 1;
			items.Add(item);
			item = new Item();
			item.SetDefaults(ItemID.LifeCrystal);
			item.stack = 1;
			items.Add(item);
			
			item = new Item();
			item.SetDefaults(ModLoader.GetMod("SummonHeart").ItemType("DemonLure"));
			item.stack = 1;
			items.Add(item);
			item = new Item();
			item.SetDefaults(ModLoader.GetMod("SummonHeart").ItemType("DemonWorldBall"));
			item.stack = 1;
			items.Add(item);
			item = new Item();
			item.SetDefaults(ModLoader.GetMod("SummonHeart").ItemType("DemonTime"));
			item.stack = 1;
			items.Add(item);
			item = new Item();
			item.SetDefaults(ModLoader.GetMod("SummonHeart").ItemType("MysteriousCrystal"));
			item.stack = 1;
			items.Add(item);
			item = new Item();
			item.SetDefaults(ModLoader.GetMod("SummonHeart").ItemType("HelpBag"));
			item.stack = 1;
			items.Add(item);
			item = new Item();
			item.SetDefaults(ItemID.ApprenticeBait);
			item.stack = 10;
			items.Add(item);
			item = new Item();
			item.SetDefaults(ItemID.ManaFlower);
			item.stack = 1;
			items.Add(item);
			item = new Item();
			item.SetDefaults(ItemID.LesserManaPotion);
			item.stack = 100;
			items.Add(item);
			item = new Item();
			item.SetDefaults(ItemID.Torch);
			item.stack = 99;
			items.Add(item);
			if (ModLoader.GetMod("Luiafk") != null)
			{
				item = new Item();
				item.SetDefaults(ModLoader.GetMod("Luiafk").ItemType("ToolTime"));
				item.stack = 1;
				items.Add(item);
			}
			if (ModLoader.GetMod("MagicStorage") != null)
			{
				item = new Item();
				item.SetDefaults(ModLoader.GetMod("MagicStorage").ItemType("StorageHeart"));
				item.stack = 1;
				items.Add(item);
				item = new Item();
				item.SetDefaults(ModLoader.GetMod("MagicStorage").ItemType("CraftingAccess"));
				item.stack = 1;
				items.Add(item);
				item = new Item();
				item.SetDefaults(ModLoader.GetMod("MagicStorage").ItemType("StorageUnit"));
				item.stack = 16;
				items.Add(item);
			}
			if (ModLoader.GetMod("MagicStorageExtra") != null)
			{
				item = new Item();
				item.SetDefaults(ModLoader.GetMod("MagicStorageExtra").ItemType("StorageHeart"));
				item.stack = 1;
				items.Add(item);
				item = new Item();
				item.SetDefaults(ModLoader.GetMod("MagicStorageExtra").ItemType("CraftingAccess"));
				item.stack = 1;
				items.Add(item);
				item = new Item();
				item.SetDefaults(ModLoader.GetMod("MagicStorageExtra").ItemType("StorageUnit"));
				item.stack = 16;
				items.Add(item);
			}
		}

		public override TagCompound Save()
		{
			var tagComp = new TagCompound();
			tagComp.Add("killAnyBoss", killAnyBoss);
			tagComp.Add("BBP", BBP);
			tagComp.Add("SummonCrit", SummonCrit);
			tagComp.Add("exp", exp);
			tagComp.Add("PlayerClass", PlayerClass);
			tagComp.Add("deathCount", deathCount);
			tagComp.Add("killNpcCount", killNpcCount);
			tagComp.Add("fishCount", fishCount);
			tagComp.Add("buffMaxCount", buffMaxCount);
			tagComp.Add("bodyDef", bodyDef);
			tagComp.Add("eyeBloodGas", eyeBloodGas);
			tagComp.Add("handBloodGas", handBloodGas);
			tagComp.Add("bodyBloodGas", bodyBloodGas);
			tagComp.Add("footBloodGas", footBloodGas);
			tagComp.Add("bloodGasMax", bloodGasMax);
			tagComp.Add("killResourceCurrent", killResourceCurrent);
			tagComp.Add("deathResourceCurrent", deathResourceCurrent);
			tagComp.Add("damageResourceCurrent", damageResourceCurrent);
			tagComp.Add("practiceEye", practiceEye);
			tagComp.Add("practiceHand", practiceHand);
			tagComp.Add("practiceBody", practiceBody);
			tagComp.Add("practiceFoot", practiceFoot);
			tagComp.Add("soulSplit", soulSplit);
			tagComp.Add("boughtbuffList", boughtbuffList);
			tagComp["ExtraAccessories"] = this.ExtraAccessories.Select(new Func<Item, TagCompound>(ItemIO.Save)).ToList<TagCompound>();
			int i = 0;
			tagComp.Add("buff_count", this.infiniBuffDic.Count);
			foreach (KeyValuePair<int, bool> kv in this.infiniBuffDic)
			{
				tagComp.Set("buff_type_" + i, kv.Key);
				tagComp.Set("buff_time_" + i, kv.Value);
				i++;
			}
			return tagComp;
		}
		
		public override void Load(TagCompound tag)
		{
			killAnyBoss = tag.GetBool("killAnyBoss");
			BBP = tag.GetInt("BBP");
			SummonCrit = tag.GetInt("SummonCrit");
			exp = tag.GetInt("exp");
			PlayerClass = tag.GetInt("PlayerClass");
			deathCount = tag.GetInt("deathCount");
			killNpcCount = tag.GetInt("killNpcCount");
			fishCount = tag.GetInt("fishCount");
			buffMaxCount = tag.GetInt("buffMaxCount");
			bodyDef = tag.GetFloat("bodyDef");
			eyeBloodGas = tag.GetInt("eyeBloodGas");
			handBloodGas = tag.GetInt("handBloodGas");
			bodyBloodGas = tag.GetInt("bodyBloodGas");
			footBloodGas = tag.GetInt("footBloodGas");
			bloodGasMax = tag.GetInt("bloodGasMax");
			killResourceCurrent = tag.GetInt("killResourceCurrent");
			deathResourceCurrent = tag.GetInt("deathResourceCurrent");
			damageResourceCurrent = tag.GetInt("damageResourceCurrent");
			practiceEye = tag.GetBool("practiceEye");
			practiceHand = tag.GetBool("practiceHand");
			practiceBody = tag.GetBool("practiceBody");
			practiceFoot = tag.GetBool("practiceFoot");
			soulSplit = tag.GetBool("soulSplit");
			boughtbuffList = tag.Get<List<bool>>("boughtbuffList");

			while (boughtbuffList.Count < modBuffValues.Count)
			{
				boughtbuffList.Add(false);
			}
            tag.GetList<TagCompound>("ExtraAccessories").Select(new Func<TagCompound, Item>(ItemIO.Load)).ToList<Item>().CopyTo(this.ExtraAccessories);

			this.infiniBuffDic = new Dictionary<int, bool>();
			int buffCount = tag.GetInt("buff_count");
			for (int i = 0; i < buffCount; i++)
			{
				this.infiniBuffDic[tag.GetInt("buff_type_" + i)] = tag.GetBool("buff_time_" + i);
			}
		}

		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (SummonHeartMod.GoldKey.JustPressed && PlayerClass == 9)
			{
				if (XiuLian)
				{
					CombatText.NewText(player.getRect(), Color.Red, "已停止修炼");
					player.ClearBuff(ModContent.BuffType<GoldenStasis>());
				}
				else
				{
					if (!player.HasBuff(ModContent.BuffType<GoldenStasis>()) && !player.HasBuff(ModContent.BuffType<GoldenStasisCD>()))
					{
						int duration = 3600;

						duration *= 60 *24;

						player.AddBuff(ModContent.BuffType<GoldenStasis>(), duration);
						player.AddBuff(ModContent.BuffType<GoldenStasisCD>(), 3600);

						Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Zhonyas").WithVolume(1f), player.Center);
						CombatText.NewText(player.getRect(), Color.LightGreen, "进入修炼状态");
                    }
                    else
                    {
						CombatText.NewText(player.getRect(), Color.Red, "暂时无法进入修炼状态");
                    }
				}
			}

			if (SummonHeartMod.FireAgeKey.JustPressed && PlayerClass == 9)
			{
				var Char = Main.player[Main.myPlayer].GetModPlayer<RPGPlayer>();
				if (Char.FireAge)
				{
					Char.FireAge = false;
					CombatText.NewText(player.getRect(), Color.Red, "已关闭轮回仙经·燃元秘术");
				}
				else
				{
					Main.PlaySound(29, (int)player.position.X, (int)player.position.Y, 104, 1f, 0f);
					Char.FireAge = true;
					CombatText.NewText(player.getRect(), Color.LightGreen, "已开启轮回仙经·燃元秘术");
				}
			}

			if (XiuLian)
			{
				return;
			}
			if (SummonHeartMod.AutoAttackKey.JustPressed)
			{
				autoAttack = !autoAttack;
				if (autoAttack)
                {
					Main.NewText($"自动使用武器: 开", Color.SkyBlue);
                }
                else
                {
					Main.NewText($"自动使用武器: 关", Color.SkyBlue);
                }
			}

			if (SummonHeartMod.TriggerExplosion.JustReleased)
			{
				this.detonate = true;
				if (Main.netMode != NetmodeID.SinglePlayer)
					MsgUtils.BombPacket(player.whoAmI);
			}
			else
			{
				this.detonate = false;
			}

			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
			{
				PanelMelee.visible = false;
				PanelKill.visible = false;
				PanelSummon.visible = false;
				PanelMelee2.visible = false;
				PanelMagic.visible = false;
				PanelMagic2.visible = false;
				PanelGodSoul.visible = false;
				PanelBuff.visible = false;
				PanelXiuXian.visible = false;
			}

			if (SummonHeartMod.ShowUI.JustPressed)
			{
				if(PlayerClass == 0)
                {
					Main.NewText($"你暂未获得任何传承，请先使用职业传承书选择你的道", Color.Red);
				}
				else if(PlayerClass == 1)
                {
					PanelMelee.visible = !PanelMelee.visible;
                }else if(PlayerClass == 2)
                {
					PanelKill.visible = !PanelKill.visible;
				}
				else if (PlayerClass == 3)
				{
					PanelSummon.visible = !PanelSummon.visible;
				}
				else if (PlayerClass == 4)
				{
					PanelMelee2.visible = !PanelMelee2.visible;
				}
				else if (PlayerClass == 5)
				{
					PanelMagic.visible = !PanelMagic.visible;
				}
				else if (PlayerClass == 6)
				{
					PanelMagic2.visible = !PanelMagic2.visible;
				}
				else if (PlayerClass == 9)
				{
					PanelXiuXian.visible = !PanelXiuXian.visible;
				}
			}
			if (SummonHeartMod.KillSkillKey.JustPressed)
			{
				if(PlayerClass == 2)
                {
					showRadius = !showRadius;
					if (showRadius)
                    {
						Main.NewText($"已开启刺杀技能", Color.White);
					}
                    else
                    {
						Main.NewText($"已关闭刺杀技能", Color.White);
					}
                }
				else
                {
					Main.NewText($"只有刺客才能使用刺杀技能", Color.Red);
				}
			}
			if (SummonHeartMod.magicSkillKey.JustPressed)
			{
				if (PlayerClass == 6)
				{
					inMagicCharging = !inMagicCharging;
					if (inMagicCharging)
					{
						Main.NewText($"已开启自动充能", Color.White);
					}
					else
					{
						Main.NewText($"已关闭自动技能", Color.White);
					}
				}
				else
                {
					Main.NewText($"只有控法才能使用充能", Color.Red);
				}
			}
			if (SummonHeartMod.TransKey.JustPressed)
			{
				if (PlayerClass == 5 || PlayerClass == 6)
				{
                    if (!boughtbuffList[3])
                    {
						Main.NewText($"未修炼魔神之腿，无法使用空间传送", Color.Red);
						return;
					}
					int costMana = player.statManaMax2 / 20;
					if(costMana < 100)
                    {
						costMana = 100;
                    }
					if (player.statMana < costMana)
					{
						Main.NewText($"魔法值不足{costMana}，无法使用空间传送", Color.Red);
						return;
					}
					Vector2 cursorPosition = new Vector2(Main.mouseX, Main.mouseY);
					cursorPosition.X -= Main.screenWidth / 2;
					cursorPosition.Y -= Main.screenHeight / 2;
					Vector2 vector = player.Center + cursorPosition;
					player.Trans(vector);
					player.HealMana(-costMana);
				}
				else
				{
					Main.NewText($"只有法师才能使用空间传送技能", Color.Red);
				}
			}
			if (SummonHeartMod.BackDieKey.JustPressed)
			{
				if (player.HasItemInAcc(mod.ItemType("MysteriousCrystal")) != -1 && player.statLife > 0)
				{
					if(player.lastDeathPostion.X == 0 && player.lastDeathPostion.Y == 0)
                    {
						CombatText.NewText(player.getRect(), Color.Red, "没有死亡点，无法传送");
					}
                    else
                    {
						Vector2 vector = new Vector2(player.lastDeathPostion.X - 16f, player.lastDeathPostion.Y - 24f);
						player.Teleport(vector, 0, 0);
                    }
                }
                else
                {
					Main.NewText($"你未装备神秘水晶", Color.Red);
				}
			}
			if (SummonHeartMod.BackHomeKey.JustPressed)
			{
				if (player.HasItemInAcc(mod.ItemType("MysteriousCrystal")) != -1 && player.statLife > 0)
				{
					this.RecallBackPos = base.player.position;
					player.Spawn();
					Main.PlaySound(SoundID.Item6, player.position);
					for (int k = 0; k < 70; k++)
					{
						Main.dust[Dust.NewDust(player.position, base.player.width, base.player.height, 15, 0f, 0f, 150, Color.Cyan, 1.2f)].velocity *= 0.5f;
					}
				}
                else
                {
					Main.NewText($"你未装备神秘水晶", Color.Red);
				}
			}
			if (SummonHeartMod.BackHomeBackKey.JustPressed)
			{
				if (player.HasItemInAcc(mod.ItemType("MysteriousCrystal")) != -1 && player.statLife > 0)
				{
					if (RecallBackPos == Vector2.Zero)
					{
						CombatText.NewText(player.getRect(), Color.Red, "没有回城点，无法返回");
					}
					else
					{
						player.noFallDmg = true;
						player.Teleport(this.RecallBackPos, 0, 0);
						this.RecallBackPos = Vector2.Zero;
					}
				}
				else
				{
					Main.NewText($"你未装备神秘水晶", Color.Red);
				}
			}
			if (SummonHeartMod.DoubleDamageKey.JustPressed)
			{
				if (PlayerClass == 1)
				{
					onDoubleDamage = !onDoubleDamage;
					if (onDoubleDamage)
					{
						CombatText.NewText(player.getRect(), new Color(0, 255, 0), "已开启双倍偿还");
					}
					else
					{
						CombatText.NewText(player.getRect(), Color.Red, "已关闭双倍偿还");
					}
				}
			}
			if (SummonHeartMod.BuffKey.JustPressed)
			{
				PanelBuff.visible = !PanelBuff.visible;
			}
		}

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
			if (target.type == NPCID.TargetDummy || target.friendly)
				return;

            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

			int heal = lifeSteal;
			if (PlayerClass == 4 && boughtbuffList[2])
			{
				heal += (int)(damage * (bodyBloodGas / 100000 * 0.01f + 0.01));
			}
			if (heal > player.statLifeMax2 / 4)
			{
				heal = player.statLifeMax2 / 4;
			}
			if (heal > HealCount)
			{
				heal = HealCount;
			}
			if (heal > 0 && HealCount > 0)
			{
				HealCount -= heal;
				player.statLife += heal;
				player.HealEffect(heal);
			}
		}

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (target.type == NPCID.TargetDummy || target.friendly)
                return;

			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

			int heal = lifeSteal;
			if (PlayerClass == 4 && boughtbuffList[2])
			{
				heal += (int)(damage * (bodyBloodGas / 100000 * 0.01f + 0.01));
			}
			if (heal > player.statLifeMax2 / 4)
			{
				heal = player.statLifeMax2 / 4;
			}
			if (heal > HealCount)
			{
				heal = HealCount;
			}
			if (heal > 0 && HealCount > 0)
			{
				HealCount -= heal;
				player.statLife += heal;
				player.HealEffect(heal);
			}
		}

        public override bool PreItemCheck()
        {
			if (autoAttack)
			{
				player.controlUseItem = true;
				player.releaseUseItem = true;
				player.HeldItem.autoReuse = true;
			}
			return true;
		}
		//允许您修改 NPC 对该玩家造成的伤害等
		public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
			int oldDamage = damage;
			if(powerArmor != null)
            {
				int costPowerArmor = 0;
				PowerArmorBase powerArmorBase = powerArmor.GetGlobalItem<PowerArmorBase>();
				if (powerArmorBase.powerArmorCount >= damage)
					costPowerArmor = damage;
				else
					costPowerArmor = powerArmorBase.powerArmorCount;

				if (costPowerArmor > 0)
				{
					damage -= costPowerArmor;
					CombatText.NewText(player.getRect(), Color.DarkGray, "-" + costPowerArmor + "护盾值");
					powerArmorBase.powerArmorCount -= costPowerArmor;
					if (powerArmorBase.powerArmorCount <= 0)
					{
						powerArmor.TurnToAir();
						powerArmor = null;
						CombatText.NewText(player.getRect(), Color.LightBlue, "-" + costPowerArmor + "能量护甲已损坏");
					}
				}
			}

			if (PlayerClass == 5 || PlayerClass == 6)
			{
				if (boughtbuffList[2])
                {
					int manaDamage = (int)(damage * 0.95f);
					int leftCount = 0;
					int costMana = manaDamage;
					if(player.statMana < manaDamage)
					{
						leftCount = manaDamage - player.statMana;
						costMana = player.statMana;
					}
					player.HealMana(costMana * -1);
					damage -= manaDamage;
					damage += +leftCount;
					if (damage < 1)
						damage = 1;
                }
			}
			
			damage = (int)Math.Ceiling(damage / myDamageReduceMult);

			if (PlayerClass == 1 && damageCD == 0)
			{
				damageCD = 6;
				int addDamage = oldDamage - damage;
				if(crit)
					addDamage *= 2;
				damageResourceCurrent += addDamage;
				if (damageResourceCurrent > damageResourceMax)
					damageResourceCurrent = damageResourceMax;
			}
		}
		//允许您修改 NPC 弹幕对该玩家造成的伤害等
		public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
			int oldDamage = damage;

			if (powerArmor != null)
			{
				int costPowerArmor = 0;
				PowerArmorBase powerArmorBase = powerArmor.GetGlobalItem<PowerArmorBase>();
				if (powerArmorBase.powerArmorCount >= damage)
					costPowerArmor = damage;
				else
					costPowerArmor = powerArmorBase.powerArmorCount;

				if (costPowerArmor > 0)
				{
					damage -= costPowerArmor;
					CombatText.NewText(player.getRect(), Color.DarkGray, "-" + costPowerArmor + "护盾值");
					powerArmorBase.powerArmorCount -= costPowerArmor;
					if (powerArmorBase.powerArmorCount <= 0)
                    {
						powerArmor.TurnToAir();
						powerArmor = null;
						CombatText.NewText(player.getRect(), Color.LightBlue, "-" + costPowerArmor + "能量护甲已损坏");
					}
				}
			}

			if (PlayerClass == 5 || PlayerClass == 6)
			{
                if (boughtbuffList[2])
                {
					int manaDamage = (int)(damage * 0.95f);
					int leftCount = 0;
					int costMana = manaDamage;
					if (player.statMana < manaDamage)
					{
						leftCount = manaDamage - player.statMana;
						costMana = player.statMana;
					}
					player.HealMana(costMana * -1);
					damage -= manaDamage;
					damage += +leftCount;
					if (damage < 1)
						damage = 1;
                }
			}
			
			damage = (int)Math.Ceiling(damage / myDamageReduceMult);

			if (PlayerClass == 1 && damageCD == 0)
			{
				damageCD = 10;
				int addDamage = oldDamage - damage;
				if (crit)
					addDamage *= 2;
				damageResourceCurrent += addDamage;
				if (damageResourceCurrent > damageResourceMax)
					damageResourceCurrent = damageResourceMax;
			}
			if (PlayerClass == 3 && boughtbuffList[2])
			{
				damage *= 30;
			}
		}

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
			if(eyeBloodGas + handBloodGas + bodyBloodGas + footBloodGas > 0 && SummonHeartConfig.Instance.EffectVisualConfig)
            {
				//handle lightning effects
				AnimationHelper.lightningEffects.visible = true;
				layers.Add(AnimationHelper.lightningEffects);

				AnimationHelper.auraEffect.visible = true;
				// capture the back layer index, which should always exist before the hook fires.
				var index = layers.FindIndex(x => x.Name == "MiscEffectsBack");
				layers.Insert(index, AnimationHelper.auraEffect);
            }

			if (XiuLian) //dont draw player during betsy dash
				while (layers.Count > 0)
					layers.RemoveAt(0);
		}

        public override void PostItemCheck()
        {
			if (autoAttack)
			{
				player.controlUseItem = true;
				player.releaseUseItem = true;
				player.HeldItem.autoReuse = true;
			}
		}

		public void IncrementAuraFrameTimers(AuraAnimationInfo aura)
		{
			if (aura == null)
				return;
			// doubled frame timer while charging.
			// auraFrameTimer++;

			auraFrameTimer++;
			if (auraFrameTimer >= aura.frameTimerLimit)
			{
				auraFrameTimer = 0;
				auraCurrentFrame++;
			}
			if (auraCurrentFrame >= aura.frames)
			{
				auraCurrentFrame = 0;
			}
		}

		public void KillResourceCountMsg()
		{
			if (!Main.LocalPlayer.Equals(player) || Main.netMode == 2) return;

			if (Main.netMode == 1)
			{
				int heal = 5 * SummonHeartWorld.WorldLevel;
				if (boughtbuffList[1])
				{
					heal += (eyeBloodGas / 400);
				}
				CombatText.NewText(player.getRect(), new Color(0, 255, 0), "+" + heal + "杀意值");
				killResourceCurrent += heal;
				if (killResourceCurrent > killResourceMax2)
					killResourceCurrent = killResourceMax2;
			}
		}

		public override float UseTimeMultiplier(Item item)
        {
			int useTime = item.useTime;
			int useAnimate = item.useAnimation;

			if (useTime == 0 || useAnimate == 0 || item.damage <= 0)
			{
				return 1f;
			}

			if (item.modItem != null && item.modItem.Name == "DemonSword")
			{
				return AttackSpeed / 2 + 0.5f;
			}
			if (item.modItem != null && item.modItem.Name == "Raiden")
			{
				return AttackSpeed / 2 + 0.5f;
			}
			if (item.modItem is DemonStaff)
			{
				return 1f;
			}
			return AttackSpeed;
		}


        public override void ModifyWeaponDamage(Item item, ref float add, ref float mult, ref float flat)
        {
			float baseAdd = add;
			float heartAdd = 1f;
			if(PlayerClass == 1 && item.melee)
            {
				//战士
				if (boughtbuffList[1])
				{
					heartAdd += (handBloodGas / 500 + 100) * 0.01f;
				}
			}
			if (PlayerClass == 2 && item.thrown)
			{
				//刺客
				if (boughtbuffList[1])
				{
					heartAdd += (handBloodGas / 500 + 100) * 0.01f;
				}
			}
			if (PlayerClass == 3 && item.summon)
            {
                //召唤师
                if (boughtbuffList[1])
                {
					heartAdd += (handBloodGas / 500 + 100) * 0.01f;
				}
            }
			if (PlayerClass == 4 && item.melee)
			{
				//狂战
				if (boughtbuffList[1])
				{
					heartAdd += (handBloodGas / 500 + 100) * 0.01f;
				}
			}
			if (PlayerClass == 5 && item.magic)
			{
				//法师
				if (boughtbuffList[1])
				{
					heartAdd += (handBloodGas / 500 + 100) * 0.01f;
				}
			}
			add = heartAdd * baseAdd;
			base.ModifyWeaponDamage(item, ref add, ref mult, ref flat);
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
			if (PlayerClass == 2)
			{
				deathResourceCurrent -= (int)damage;
				CombatText.NewText(player.getRect(), Color.DarkGray, "-" + damage + "死气值");
				if (deathResourceCurrent <= 0)
				{
					deathResourceCurrent = 0;
					return true;
				}
				else
				{
					player.statLife = 1;
					return false;
				}
			}
			else if (PlayerClass == 4)
			{
				if (angerResourceCurrent >= angerResourceMax)
					onanger = true;

                if (onanger)
                {
					CombatText.NewText(player.getRect(), Color.Red, "触发被动无尽怒火免疫伤害");
					if (angerResourceCurrent <= 0)
					{
						deathResourceCurrent = 0;
						return true;
					}
					else
					{
						player.statLife = 1;
						return false;
					}
				}
			}
			return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
			onanger = false;
			angerResourceCurrent = 0;
			deathCount++;
			if (PlayerClass == 1 && damageResourceCurrent > 0)
			{
				damageResourceCurrent /= 2;
				string text = $"{player.name}已死亡。丢失1半的伤害吸收量，伤害吸收量-{damageResourceCurrent}";
				Main.NewText(text, Color.Green);
			}
			/*if (PlayerClass == 2)
            {
				string text = $"{player.name}身为刺客，向死而生，杀意上限+10点";
				Main.NewText(text, Color.Green);
			}*/
            base.Kill(damage, hitDirection, pvp, damageSource);
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
			if (PlayerClass == 4 && !onanger)
			{
				angerResourceCurrent += 1;
				if (angerResourceCurrent > angerResourceMax)
					angerResourceCurrent = angerResourceMax;
			}
		}

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			if (PlayerClass == 4 && !onanger)
			{
				angerResourceCurrent += 1;
				if (angerResourceCurrent > angerResourceMax)
					angerResourceCurrent = angerResourceMax;
			}
		}

        public override void PostUpdateRunSpeeds()
        {
            ModPlayerEffects.PostUpdateRunSpeeds(player);
        }

        public override void AnglerQuestReward(float rareMultiplier, List<Item> rewardItems)
        {
			fishCount++;
			base.AnglerQuestReward(rareMultiplier, rewardItems);
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				MsgUtils.SyncAnglerQuestReward();
			}
		}

		public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk)
		{
            if (SummonHeartConfig.Instance.FishOtherItem)
            {
				if (Main.rand.Next(100) <= 5)
				{
					int itemID = 0;
					bool itemChosen = false;
					while (!itemChosen)
					{
						itemID = Main.rand.Next(ItemLoader.ItemCount);
						Item item = new Item();
						item.SetDefaults(itemID, false);
						int rarity = item.rare;
						int finshLevel = this.getFishLevel();
						int curRarity = finshLevel / 20;
						if (rarity == curRarity)
						{
							itemChosen = true;
						}
					}
					caughtType = itemID;
				}
            }
		}

		public IDictionary<int, bool> infiniBuffDic = new Dictionary<int, bool>();
	}
}