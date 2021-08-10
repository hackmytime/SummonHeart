using Microsoft.Xna.Framework;
using SummonHeart.Effects.Animations.Aura;
using SummonHeart.Extensions;
using SummonHeart.Items.Weapons.Magic;
using SummonHeart.Models;
using SummonHeart.Projectiles.Summon;
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
		public bool eatGodSoul = false;
		public int PlayerClass = 0;
		public int deathCount = 0;
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
		public bool onanger = false;
		public int killResourceMax;
		public int killResourceMax2;
		public int killResourceSkillCount = 0;
		public int killResourceSkillCountMax = 10;
		public int killResourceMulti;
		public bool inMagicCharging = false;
		public bool magicChargeActive = false;
		public bool magicBook = false;
		public float magicCharge;
		public float magicChargeMax = 100;
		public float magicChargeCount = 0;
		public float magicChargeCountMax = 10;

		public int eyeBloodGas = 0;
		public int handBloodGas = 0;
		public int bodyBloodGas = 0;
		public int footBloodGas = 0;

		public int eyeMax = 0;
		public int handMax = 0;
		public int bodyMax = 0;
		public int footMax = 0;

		public int bloodGasMax = 10000;

		public int swordBlood = 1;
		public int shortSwordBlood = 1;
		public int flySwordBlood = 1;
		public int magicSwordBlood = 1;
		public int swordBloodMax = 100;

		public bool practiceEye = false;
		public bool practiceHand = false;
		public bool practiceBody = false;
		public bool practiceFoot = false;
		public bool soulSplit = false;

		public Projectile eyeProjectile;
		public float MyAccelerationMult;
		public float MyMoveSpeedMult;
		public float MyCritDmageMult;
		public int costMana;
		public bool accBuryTheLight;
		public int buryTheLightCooldown;
		public bool buryTheLightStarted;

		public int HealCount = 0;
		private int healCD = 0;
		private int bodyHealCD = 0;
		private int killHealCD = 0;

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

        public override void ResetEffects()
		{
			SummonHeart = false;
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
			killResourceMax2 = killResourceMax;
			

			eyeMax = SummonHeartConfig.Instance.eyeMax;
			handMax = SummonHeartConfig.Instance.handMax;
			bodyMax = SummonHeartConfig.Instance.bodyMax;
			footMax = SummonHeartConfig.Instance.footMax;

			MyAccelerationMult = 1f;
			MyMoveSpeedMult = 1f;
			MyCritDmageMult = 1f;
			costMana = (handBloodGas / 33333) * (handBloodGas / 33333) * 10 + 5;

			if (eatGodSoul)
			{
				MaxExtraAccessories = 24;
			}
			else
			{
				MaxExtraAccessories = SummonHeartWorld.WorldLevel;
				if (SummonHeartWorld.WorldLevel == 5)
					MaxExtraAccessories += 3;
			}
			inMagicCharging = false;
			magicBook = false;
			//刷新上限
			ModPlayerEffects.UpdateMax(player);

			
		}

		public override void PreUpdate()
		{
			if (player.HasItemInAcc(mod.ItemType("MysteriousCrystal")) != -1 && base.player.respawnTimer > 300 && !player.AnyBossAlive())
			{
				player.respawnTimer = 120;
			}

			if (Main.netMode != 2)
			{
				ModPlayerEffects.UpdateColors(player);
			}
		}

		/*private void AccBuryTheLight()
		{
			NPC target = Helper.GetNearestNPC(base.player.position, (NPC npc) => !npc.friendly && npc.active && !npc.dontTakeDamage, 600f);
			if (target == null)
			{
				this.buryTheLightStarted = false;
				this.buryTheLightCooldown = 10;
				return;
			}
			if (this.buryTheLightCooldown <= 0)
			{
				this.buryTheLightCooldown = 10;
				if (!this.buryTheLightStarted)
				{
					this.buryTheLightStarted = true;
					Main.PlaySound(50, (int)base.player.position.X, (int)base.player.position.Y, base.mod.GetSoundSlot((SoundType)50, "Sounds/Items/buryTheLight"), 0.4f, Utils.NextFloat(Main.rand, 0f, 0.15f));
				}
				float mult = 1f + (base.player.allDamage - 1f + (base.player.rangedDamage - 1f) + (base.player.meleeDamage - 1f) + (base.player.magicDamage - 1f));
				int damage = (int)(12f * mult) + Math.Min(60, (int)((float)target.defense * 0.5f));
				Vector2 vel = VectorHelper.VelocityToPoint(base.player.Center, target.Center, 1f);
				Projectile.NewProjectileDirect(target.Center - vel * 60f, vel, ModContent.ProjectileType<DragonLegacyBlue>(), (int)((float)damage * 1.25f), 6.6f, base.player.whoAmI, 0f, 0f).netUpdate = true;
				return;
			}
			this.buryTheLightCooldown--;
		}*/
	
		public override void PostUpdate()
		{
			currentAura = this.GetAuraEffectOnPlayer();
			IncrementAuraFrameTimers(currentAura);
		}

		public override void PostUpdateMiscEffects()
        {
			if(player.endurance > 0.8f)
            {
				player.endurance = 0.8f;
			}
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
		}

        public override void GetHealMana(Item item, bool quickHeal, ref int healValue)
        {
            base.GetHealMana(item, quickHeal, ref healValue);
        }

		private void EffectMagic2()
		{
			int allBlood = this.getAllBloodGas();
			player.statManaMax2 += allBlood / 80;

			if (magicBook)
				player.statManaMax2 *= 2;

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
				MyCritDmageMult += eyeBloodGas / 1000 * 0.01f;
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
				player.wingTimeMax += (footBloodGas / 2222 + 10) * 60;
				player.jumpSpeedBoost += (footBloodGas / 1000 + 60) * 0.01f;
				if (footBloodGas >= 200000)
				{
					player.wingTime = footBloodGas / 1000 * 60;
				}
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
				MyCritDmageMult += eyeBloodGas / 1000 * 0.01f;
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
				player.HealMana(heal);
			}

			//魔神之腿
			if (boughtbuffList[3])
			{
				player.noFallDmg = true;
				MyMoveSpeedMult += (footBloodGas / 5000 + 20) * 0.01f;
				MyAccelerationMult += (footBloodGas / 5000 + 20) * 0.01f;
				player.wingTimeMax += (footBloodGas / 2222 + 10) * 60;
				player.jumpSpeedBoost += (footBloodGas / 1000 + 60) * 0.01f;
				if (footBloodGas >= 200000)
				{
					player.wingTime = footBloodGas / 1000 * 60;
				}
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
            }

			// 手
			if (boughtbuffList[1])
			{

			}

			// 躯
			if (boughtbuffList[2])
			{
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
			int addMax = shortSwordBlood;
			if (addMax > 10000)
				addMax = 10000;
			killResourceMax = 100 + addMax;
			killResourceMulti = 10;
			killResourceSkillCountMax = 10;
			//被动
			int allBlood = this.getAllBloodGas();
			int deathAdd = deathCount * 10;
			if(deathAdd > 40000)
            {
				deathAdd = 40000;
            }
			killResourceMax += allBlood / 20 + deathAdd;

			//魔神之手
			if (boughtbuffList[1])
            {
				AttackSpeed += (handBloodGas / 4000 + 30) * 0.01f;
				player.thrownVelocity += (handBloodGas / 4000 + 30) * 0.01f;
				killResourceSkillCountMax = (handBloodGas / 5000 + 10);
				//隐藏路线：叛道者
				if (shortSwordBlood <= 1)
					killResourceMax2 *= 4;
			}
			deathResourceMax = killResourceMax2;
			int heal = 1;
			if (boughtbuffList[2])
            {
				//神通流
                heal = (int)(killResourceMax2 * (0.01 + eyeBloodGas / 100000 * 0.01f)) / 4;
				heal += (bodyBloodGas / 400 + 15) / 4;
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
				player.wingTimeMax += (footBloodGas / 1000 + 10) * 60;
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
			MyMoveSpeedMult -= 0.2f;
			player.jumpSpeedBoost -= 0.33f;

			player.statDefense += (int)bodyDef * 2;
			int addDef = deathCount / 5;
			if (addDef > bodyDef)
				addDef = (int)bodyDef;
			player.statDefense += addDef;
			//魔神之眼
			if (boughtbuffList[0])
            {
				player.meleeCrit += eyeBloodGas / 2222 + 10;
				MyCritDmageMult += eyeBloodGas / 2000 * 0.01f;
			}

			//魔神之手
			if (boughtbuffList[1])
			{
				AttackSpeed += (handBloodGas / 1111 + 20) * 0.01f;
			}

			//魔神之躯
			if (boughtbuffList[2])
			{
				player.noKnockback = true;
				player.statLifeMax2 += (bodyBloodGas / 200 + 300);
				//计算被动
				addLife = deathCount;
				if (addLife > player.statLifeMax2)
					addLife = player.statLifeMax2;
				player.statLifeMax2 += addLife;
				int heal = (int)(player.statLifeMax2 * (0.01 + bodyBloodGas / 20000 * 0.01f)) / 4;
				if (player.statLife < player.statLifeMax2 && bodyHealCD == 1)
				{
					if (heal < 1)
						heal = 1;
					player.statLife += heal;
					player.HealEffect(heal);
				}
            }
            else
            {
				//计算被动
				addLife = deathCount;
				if (addLife > player.statLifeMax2)
					addLife = player.statLifeMax2;
				player.statLifeMax2 += addLife;
			}

			//魔神之腿
			if (boughtbuffList[3])
			{
				player.noFallDmg = true;
				MyMoveSpeedMult += (footBloodGas / 10000 + 20) * 0.01f;
				MyAccelerationMult += (footBloodGas / 10000 + 20) * 0.01f;
				player.wingTimeMax += (footBloodGas / 2222 + 10) * 60;
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
			angerResourceMax = 100 + deathCount;
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
				MyCritDmageMult += eyeBloodGas / 1000 * 0.01f;
			}

			//魔神之手
			if (boughtbuffList[1])
			{
				//player.meleeDamage += handBloodGas / 200 * 0.01f;
				AttackSpeed += (handBloodGas / 1111 + 20) * 0.01f;
			}

			//魔神之躯
			if (boughtbuffList[2])
			{
				player.noKnockback = true;
				player.statLifeMax2 += bodyBloodGas / 200;
			}

			//魔神之腿
			if (boughtbuffList[3])
			{
				player.noFallDmg = true;
				MyMoveSpeedMult += (footBloodGas / 5000 + 20) * 0.01f;
				MyAccelerationMult += (footBloodGas / 5000 + 20) * 0.01f;
				player.wingTimeMax += (footBloodGas / 2222 + 10) * 60;
				player.jumpSpeedBoost += (footBloodGas / 1000 + 60) * 0.01f;
				if (footBloodGas >= 200000)
				{
					player.wingTime = footBloodGas / 1000 * 60;
				}
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
			if (Main.hardMode && SummonCrit > 499)
				SummonCrit = 500;
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
			item.SetDefaults(ModLoader.GetMod("SummonHeart").ItemType("LlPet"));
			item.stack = 1;
			items.Add(item);
			
			item = new Item();
			item.SetDefaults(ModLoader.GetMod("SummonHeart").ItemType("MysteriousCrystal"));
			item.stack = 1;
			items.Add(item);
			item = new Item();
			item.SetDefaults(ItemID.QueenSpiderStaff);
			item.stack = 1;
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
			item = new Item();
			item.SetDefaults(ItemID.WaterBucket);
			item.stack = 2;
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
		}

        public override void clientClone(ModPlayer clientClone)
		{
			SummonHeartPlayer clone = clientClone as SummonHeartPlayer;
			clone.BBP = BBP;
			clone.SummonCrit = SummonCrit;
			clone.PlayerClass = PlayerClass;
			//clone.deathCount = deathCount;
			clone.bodyDef = bodyDef;
			clone.eyeBloodGas = eyeBloodGas;
			clone.handBloodGas = handBloodGas;
			clone.bodyBloodGas = bodyBloodGas;
			clone.footBloodGas = footBloodGas;
			clone.bloodGasMax = bloodGasMax;
			clone.swordBlood = swordBlood;
			clone.shortSwordBlood = shortSwordBlood;
			clone.flySwordBlood = flySwordBlood;
			clone.magicSwordBlood = magicSwordBlood;
			clone.swordBloodMax = swordBloodMax;
			clone.practiceEye = practiceEye;
			clone.practiceHand = practiceHand;
			clone.practiceBody = practiceBody;
			clone.practiceFoot = practiceFoot;
			clone.soulSplit = soulSplit;
			//clone.boughtbuffList = boughtbuffList;
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)0);
			packet.Write((byte)player.whoAmI);
			packet.Write(BBP);
			packet.Write(SummonCrit);
			packet.Write(PlayerClass);
			//packet.Write(deathCount);
			packet.Write(bodyDef);
			packet.Write(eyeBloodGas);
			packet.Write(handBloodGas);
			packet.Write(bodyBloodGas);
			packet.Write(footBloodGas);
			packet.Write(bloodGasMax);
			packet.Write(swordBlood);
			packet.Write(shortSwordBlood);
			packet.Write(flySwordBlood);
			packet.Write(magicSwordBlood);
			packet.Write(swordBloodMax);
			packet.Write(practiceEye);
			packet.Write(practiceHand);
			packet.Write(practiceBody);
			packet.Write(practiceFoot);
			packet.Write(soulSplit);
			/*for (int i = 0; i < boughtbuffList.Count; i++)
			{
				packet.Write(boughtbuffList[i]);
			}*/
			packet.Send(toWho, fromWho);
		}

		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			SummonHeartPlayer clone = clientPlayer as SummonHeartPlayer;
			bool send = false;

			if (clone.BBP != BBP || clone.SummonCrit != SummonCrit
					|| clone.bodyDef != bodyDef || clone.PlayerClass != PlayerClass || clone.deathCount != deathCount
					|| clone.eyeBloodGas != eyeBloodGas || clone.handBloodGas != handBloodGas
					|| clone.bodyBloodGas != bodyBloodGas || clone.footBloodGas != footBloodGas
					|| clone.bloodGasMax != bloodGasMax || clone.swordBlood != swordBlood
					|| clone.shortSwordBlood != shortSwordBlood || clone.magicSwordBlood != magicSwordBlood
					|| clone.flySwordBlood != flySwordBlood || clone.swordBloodMax != swordBloodMax
					|| clone.practiceEye != practiceEye || clone.practiceHand != practiceHand
					|| clone.practiceBody != practiceBody || clone.practiceFoot != practiceFoot
					|| clone.soulSplit != soulSplit)
			{
				send = true;
			}
			/*for (int i = 0; i < boughtbuffList.Count; i++)
			{ 
				if (clone.boughtbuffList[i] != boughtbuffList[i])
				{
					send = true;
				}
			}*/

			if (send)
			{
				var packet = mod.GetPacket();
				packet.Write((byte)0);
				packet.Write((byte)player.whoAmI);
				packet.Write(BBP);
				packet.Write(SummonCrit);
				packet.Write(PlayerClass);
				//packet.Write(deathCount);
				packet.Write(bodyDef);
				packet.Write(eyeBloodGas);
				packet.Write(handBloodGas);
				packet.Write(bodyBloodGas);
				packet.Write(footBloodGas);
				packet.Write(bloodGasMax);
				packet.Write(swordBlood);
				packet.Write(shortSwordBlood);
				packet.Write(flySwordBlood);
				packet.Write(magicSwordBlood);
				packet.Write(swordBloodMax);
				packet.Write(practiceEye);
				packet.Write(practiceHand);
				packet.Write(practiceBody);
				packet.Write(practiceFoot);
				packet.Write(soulSplit);
				/*for (int i = 0; i < boughtbuffList.Count; i++)
				{
					packet.Write(boughtbuffList[i]);
				}*/
				packet.Send();
			}
		}

		public override TagCompound Save()
		{
			var tagComp = new TagCompound();
			tagComp.Add("eatGodSoul", eatGodSoul);
			tagComp.Add("BBP", BBP);
			tagComp.Add("SummonCrit", SummonCrit);
			tagComp.Add("exp", exp);
			tagComp.Add("PlayerClass", PlayerClass);
			tagComp.Add("deathCount", deathCount);
			tagComp.Add("bodyDef", bodyDef);
			tagComp.Add("eyeBloodGas", eyeBloodGas);
			tagComp.Add("handBloodGas", handBloodGas);
			tagComp.Add("bodyBloodGas", bodyBloodGas);
			tagComp.Add("footBloodGas", footBloodGas);
			tagComp.Add("bloodGasMax", bloodGasMax);
			tagComp.Add("swordBlood", swordBlood);
			tagComp.Add("shortSwordBlood", shortSwordBlood);
			tagComp.Add("flySwordBlood", flySwordBlood);
			tagComp.Add("magicSwordBlood", magicSwordBlood);
			tagComp.Add("swordBloodMax", swordBloodMax);
			tagComp.Add("killResourceCurrent", killResourceCurrent);
			tagComp.Add("deathResourceCurrent", deathResourceCurrent);
			tagComp.Add("practiceEye", practiceEye);
			tagComp.Add("practiceHand", practiceHand);
			tagComp.Add("practiceBody", practiceBody);
			tagComp.Add("practiceFoot", practiceFoot);
			tagComp.Add("soulSplit", soulSplit);
			tagComp.Add("boughtbuffList", boughtbuffList);
			tagComp["ExtraAccessories"] = this.ExtraAccessories.Select(new Func<Item, TagCompound>(ItemIO.Save)).ToList<TagCompound>();
			return tagComp;
		}
		
		public override void Load(TagCompound tag)
		{
			eatGodSoul = tag.GetBool("eatGodSoul");
			BBP = tag.GetInt("BBP");
			SummonCrit = tag.GetInt("SummonCrit");
			exp = tag.GetInt("exp");
			PlayerClass = tag.GetInt("PlayerClass");
			deathCount = tag.GetInt("deathCount");
			bodyDef = tag.GetFloat("bodyDef");
			eyeBloodGas = tag.GetInt("eyeBloodGas");
			handBloodGas = tag.GetInt("handBloodGas");
			bodyBloodGas = tag.GetInt("bodyBloodGas");
			footBloodGas = tag.GetInt("footBloodGas");
			bloodGasMax = tag.GetInt("bloodGasMax");
			swordBlood = tag.GetInt("swordBlood");
			shortSwordBlood = tag.GetInt("shortSwordBlood");
			flySwordBlood = tag.GetInt("flySwordBlood");
			magicSwordBlood = tag.GetInt("magicSwordBlood");
			swordBloodMax = tag.GetInt("swordBloodMax");
			killResourceCurrent = tag.GetInt("killResourceCurrent");
			deathResourceCurrent = tag.GetInt("deathResourceCurrent");
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
		}

		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (SummonHeartMod.AutoAttackKey.JustPressed)
			{
				autoAttack = !autoAttack;
				if (autoAttack)
                {
					Main.NewText($"自动使用武器: 开", Color.SkyBlue);
					inMagicCharging = true;
                }
                else
                {
					Main.NewText($"自动使用武器: 关", Color.SkyBlue);
					inMagicCharging = false;
                }
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
			if (SummonHeartMod.TransKey.JustPressed)
			{
				if (PlayerClass == 5)
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
			/*if (SummonHeartMod.ExtraAccessaryKey.JustPressed)
			{
				PanelGodSoul.visible = !PanelGodSoul.visible;
			}*/
		}

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
			if (target.type == NPCID.TargetDummy || target.friendly)
				return;
			
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			if (PlayerClass == 4 && boughtbuffList[2])
			{
				int heal = (int)(damage * (bodyBloodGas / 100000 * 0.01f + 0.01));

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
		}

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
			if (target.type == NPCID.TargetDummy || target.friendly)
				return;
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			if (PlayerClass == 4 && boughtbuffList[2])
			{
				int heal = (int)(damage * (bodyBloodGas / 100000 * 0.01f + 0.01));

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
			if (PlayerClass == 1 && boughtbuffList[2])
			{
				damage = (int)(damage * (1 - 0.5 - bodyBloodGas / 13333 * 0.01f));
				if (damage < 1)
					damage = 1;
			}
			if (PlayerClass == 2)
			{
				//判断死气值是否够减
				int defDmage = 0;
                if (deathResourceCurrent >= damage)
                {
					defDmage = damage;
                }
                else
                {
					defDmage = deathResourceCurrent;
                }
				deathResourceCurrent -= defDmage;
				if (defDmage > 0)
					CombatText.NewText(player.getRect(), Color.DarkGray, "-" + defDmage + "死气值");
				if (deathResourceCurrent <= 0)
				{
					deathResourceCurrent = 0;
				}
				if (crit)
					defDmage /= 2;
				damage -= defDmage;
			}
			if (PlayerClass == 3 && boughtbuffList[2])
			{
				damage = (int)(damage * 0.04f);
				if (damage < 1)
					damage = 1;
			}
			if (PlayerClass == 4 && boughtbuffList[2])
			{
				damage = (int)(damage * (1 - bodyBloodGas / 5000 * 0.01f));
				if (damage < 1)
					damage = 1;
			}
			if (PlayerClass == 5 && boughtbuffList[2])
			{
				int manaDamage = (int)(damage * (0.75f + bodyBloodGas / 10000 * 0.01f));
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
		//允许您修改 NPC 弹幕对该玩家造成的伤害等
		public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
			if (PlayerClass == 1 && boughtbuffList[2])
			{
				damage = (int)(damage * (1 - 0.5 - bodyBloodGas / 13333 * 0.01f));
				if (damage < 1)
					damage = 1;
			}
			if (PlayerClass == 2)
			{
				//判断死气值是否够减
				int defDmage = 0;
				if (deathResourceCurrent >= damage)
				{
					defDmage = damage;
				}
				else
				{
					defDmage = deathResourceCurrent;
				}
				deathResourceCurrent -= defDmage;
				if(defDmage > 0)
					CombatText.NewText(player.getRect(), Color.DarkGray, "-" + defDmage + "死气值");
				if (deathResourceCurrent <= 0)
				{
					deathResourceCurrent = 0;
				}
				if (crit)
					defDmage /= 2;
				damage -= defDmage;
			}
			if (PlayerClass == 3 && boughtbuffList[2])
			{
				damage = (int)(damage * (1 - 0.2 - bodyBloodGas / 5000 * 0.01f));
				if (damage < 1)
					damage = 1;
			}
			if (PlayerClass == 4 && boughtbuffList[2])
			{
				damage = (int)(damage * (1 - bodyBloodGas / 5000 * 0.01f));
				if (damage < 1)
					damage = 1;
			}
			if (PlayerClass == 5 && boughtbuffList[2])
			{
				int manaDamage = (int)(damage * (0.75f + bodyBloodGas / 10000 * 0.01f));
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
					heartAdd += handBloodGas / 200 * 0.01f;
				}
			}
			if (PlayerClass == 2 && item.thrown)
			{
				//刺客
				if (boughtbuffList[1])
				{
					heartAdd += handBloodGas / 200 * 0.01f;
				}
			}
			if (PlayerClass == 3 && item.summon)
            {
                //召唤师
                if (boughtbuffList[1])
                {
					heartAdd += handBloodGas / 200 * 0.01f;
				}
            }
			if (PlayerClass == 5 && item.magic)
			{
				//法师
				if (boughtbuffList[1])
				{
					heartAdd += handBloodGas / 200 * 0.01f;
				}
			}
			add = heartAdd * baseAdd;
			base.ModifyWeaponDamage(item, ref add, ref mult, ref flat);
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
			if(PlayerClass == 4)
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
			if (PlayerClass == 1)
			{
				string text = $"{player.name}拥有战者之心，不畏死亡。已从死亡之中获得力量，生命上限+1，防御力+0.2";
				Main.NewText(text, Color.Green);
			}
			if (PlayerClass == 2)
            {
				swordBloodMax += 2;
				string text = $"{player.name}身为刺客，向死而生，杀意上限+10点";
				Main.NewText(text, Color.Green);
			}
            base.Kill(damage, hitDirection, pvp, damageSource);
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
			if (crit && PlayerClass == 4 && !onanger)
			{
				angerResourceCurrent += 3;
				if (angerResourceCurrent > angerResourceMax)
					angerResourceCurrent = angerResourceMax;
			}
		}

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			if (crit && PlayerClass == 4 && !onanger)
			{
				angerResourceCurrent += 3;
				if (angerResourceCurrent > angerResourceMax)
					angerResourceCurrent = angerResourceMax;
			}
		}

        public override void PostUpdateRunSpeeds()
        {
            ModPlayerEffects.PostUpdateRunSpeeds(player);
        }
    }
}