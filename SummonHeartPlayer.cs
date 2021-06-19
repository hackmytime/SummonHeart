using Microsoft.Xna.Framework;
using SummonHeart.Effects.Animations.Aura;
using SummonHeart.Models;
using SummonHeart.ui;
using SummonHeart.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static SummonHeart.SummonHeartMod;

namespace SummonHeart
{
    public class SummonHeartPlayer : ModPlayer
	{	
		public bool SummonHeart = false;
		public bool Berserked = false;
		public float AttackSpeed;
		public float tungstenPrevSizeSave;
		public bool FishSoul = false;
		public bool BattleCry = false;		
		public bool GlobalTeleporterUp = false;
		public bool llPet = false;
		public int BBP = 0;
		public int SummonCrit = 0;
		public int exp = 0;
		public float bodyDef = 0;

		public int eyeBloodGas = 0;
		public int handBloodGas = 0;
		public int bodyBloodGas = 0;
		public int footBloodGas = 0;

		public bool practiceEye = false;
		public bool practiceHand = false;
		public bool practiceBody = false;
		public bool practiceFoot = false;
		public bool soulSplit = false;

		public int HealCount = 0;
		private int healCD = 0;
		private int bodyHealCD = 0;

		public List<bool> boughtbuffList;

		// animation helper fields
		public AuraAnimationInfo currentAura;
		public int lightningFrameTimer = 500000;
		public int auraFrameTimer = 0;
		public int auraCurrentFrame = 0;

		public SummonHeartPlayer()
		{
			var size = SummonHeartMod.getBuffLength();
			boughtbuffList = new List<bool>();
			for(int i = 1; i <= size; i++)
            {
				boughtbuffList.Add(false);
            }
		}

        public override void PostUpdateMiscEffects()
        {
			player.statDefense += (int)bodyDef;
			//player.thorns = 1f;
			if(bodyDef > 96)
				player.noKnockback = true;

			//魔神之眼
			player.magicCrit += eyeBloodGas / 1000;
			player.meleeCrit += eyeBloodGas / 1000;
			player.rangedCrit += eyeBloodGas / 1000;
			player.thrownCrit += eyeBloodGas / 1000;

			//魔神之手
			player.allDamage += handBloodGas / 100 * 0.01f;
			AttackSpeed += handBloodGas / 500 * 0.01f;

			Item heldItem = player.HeldItem;
			if (heldItem.damage > 0 && heldItem.melee && heldItem.scale < 4.0f)
			{
				tungstenPrevSizeSave = heldItem.scale;
				float curScale = handBloodGas / 333 * 0.01f + 1;
				if (curScale < tungstenPrevSizeSave)
					curScale = tungstenPrevSizeSave;
				heldItem.scale = curScale;
			}else if (tungstenPrevSizeSave != -1)
			{
				heldItem.scale = tungstenPrevSizeSave;
			}


			//魔神之躯
			player.statLifeMax2 += bodyBloodGas / 100;
            if (boughtbuffList[2])
            {
				int heal = (int)(player.statLifeMax2 * (0.01 + bodyBloodGas / 10000 * 0.01f)) / 4;
				if(player.statLife < player.statLifeMax2 && bodyHealCD == 1)
				{
					player.statLife += heal;
					player.HealEffect(heal);
				}
            }

			//魔神之腿
			if (boughtbuffList[3])
				player.noFallDmg = true;
			player.wingTimeMax += footBloodGas / 1000 * 60;
			player.jumpSpeedBoost += footBloodGas / 200 * 0.01f;
		}

        public override void PostUpdate()
        {
			currentAura = this.GetAuraEffectOnPlayer();
			IncrementAuraFrameTimers(currentAura);
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

		private void dealLevel()
		{
			int lvExp = 1;
			int exp = lvExp;
			int level = 0;
			while (exp <= BBP)
			{
				exp += lvExp;
				level++;
				lvExp += 1;
			}
			int needExp = exp - BBP;
			exp = needExp;
			SummonCrit = level;
			if (!Main.hardMode && SummonCrit > 299)
				SummonCrit = 299;
			if (Main.hardMode && SummonCrit > 499)
				SummonCrit = 500;
		}

		public override void ResetEffects()
        {
			SummonHeart = false;
			AttackSpeed = 1f;
			FishSoul = false;
			GlobalTeleporterUp = false;
			llPet = false;
			healCD++;
			if (healCD == 60)
            {
				healCD = 1;
				HealCount = SummonCrit;
            }
			bodyHealCD++;
			if(bodyHealCD == 15)
            {
				bodyHealCD = 1;
			}
		}

        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
			Item item = new Item();
			item.SetDefaults(ModLoader.GetMod("SummonHeart").ItemType("GuideNote"));
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
			clone.exp = exp;
			clone.bodyDef = bodyDef;
			clone.boughtbuffList = boughtbuffList;
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)player.whoAmI);
			packet.Write(BBP);
			packet.Write(SummonCrit);
			packet.Write(exp);
			packet.Write(bodyDef);
			packet.Write(eyeBloodGas);
			packet.Write(handBloodGas);
			packet.Write(bodyBloodGas);
			packet.Write(footBloodGas);
			packet.Write(practiceEye);
			packet.Write(practiceHand);
			packet.Write(practiceBody);
			packet.Write(practiceFoot);
			packet.Write(soulSplit);
			for (int i = 0; i < boughtbuffList.Count; i++)
			{
				packet.Write(boughtbuffList[i]);
			}
			packet.Send(toWho, fromWho);
		}

		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			SummonHeartPlayer clone = clientPlayer as SummonHeartPlayer;
			bool send = false;

			for (int i = 0; i < boughtbuffList.Count; i++)
			{
				if (clone.BBP != BBP || clone.SummonCrit != SummonCrit || clone.exp != exp)
				{
					send = true;
					break;
				}
				if (clone.boughtbuffList[i] != boughtbuffList[i])
				{
					send = true;
					break;
				}
			}
			if (send)
			{
				var packet = mod.GetPacket();
				packet.Write((byte)player.whoAmI);
				packet.Write(BBP);
				packet.Write(SummonCrit);
				packet.Write(exp);
				packet.Write(bodyDef);
				packet.Write(eyeBloodGas);
				packet.Write(handBloodGas);
				packet.Write(bodyBloodGas);
				packet.Write(footBloodGas);
				packet.Write(practiceEye);
				packet.Write(practiceHand);
				packet.Write(practiceBody);
				packet.Write(practiceFoot);
				packet.Write(soulSplit);
				for (int i = 0; i < boughtbuffList.Count; i++)
				{
					packet.Write(boughtbuffList[i]);
				}
				packet.Send();
			}
		}

		public override TagCompound Save()
		{
			var tagComp = new TagCompound();
			tagComp.Add("BBP", BBP);
			tagComp.Add("SummonCrit", SummonCrit);
			tagComp.Add("exp", exp);
			tagComp.Add("bodyDef", bodyDef);
			tagComp.Add("eyeBloodGas", eyeBloodGas);
			tagComp.Add("handBloodGas", handBloodGas);
			tagComp.Add("bodyBloodGas", bodyBloodGas);
			tagComp.Add("footBloodGas", footBloodGas);
			tagComp.Add("practiceEye", practiceEye);
			tagComp.Add("practiceHand", practiceHand);
			tagComp.Add("practiceBody", practiceBody);
			tagComp.Add("practiceFoot", practiceFoot);
			tagComp.Add("soulSplit", soulSplit);
			tagComp.Add("boughtbuffList", boughtbuffList);
			return tagComp;
		}
		
		public override void Load(TagCompound tag)
		{
			BBP = tag.GetInt("BBP");
			SummonCrit = tag.GetInt("SummonCrit");
			exp = tag.GetInt("exp");
			bodyDef = tag.GetFloat("bodyDef");
			eyeBloodGas = tag.GetInt("eyeBloodGas");
			handBloodGas = tag.GetInt("handBloodGas");
			bodyBloodGas = tag.GetInt("bodyBloodGas");
			footBloodGas = tag.GetInt("footBloodGas");
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
		}


		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (SummonHeartMod.AutoAttackKey.JustPressed)
			{
				Berserked = !Berserked;
				if(Berserked)
					Main.NewText($"自动使用武器: 开", Color.SkyBlue);
				else
					Main.NewText($"自动使用武器: 关", Color.SkyBlue);
			}

			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
			{
				Panel.visible = false;
			}

			if (SummonHeartMod.ShowUI.JustPressed)
			{
				Panel.visible = !Panel.visible;
			}
		}

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
			if (target.type == NPCID.TargetDummy || target.friendly)
				return;

			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			if (modPlayer.SummonHeart)
			{
				int heal = damage * modPlayer.SummonCrit / 5000;

				if (heal > modPlayer.SummonCrit / 25)
				{
					heal = modPlayer.SummonCrit / 25;
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
			if (modPlayer.SummonHeart)
			{
				int heal = damage * modPlayer.SummonCrit / 5000;

				if (heal > modPlayer.SummonCrit / 25)
				{
					heal = modPlayer.SummonCrit / 25;
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
			if (Berserked)
			{
				player.controlUseItem = true;
				player.releaseUseItem = true;
				player.HeldItem.autoReuse = true;
			}
			return true;
		}



        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
			if (bodyDef >= 1)
			{
				npc.StrikeNPC((int)(damage + bodyDef), npc.knockBackResist, -npc.direction);
				//Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("Returner2"), (int)bodyDef + npc.defense, 0, player.whoAmI);
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
			if (Berserked)
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

        public override float UseTimeMultiplier(Item item)
        {
			int useTime = item.useTime;
			int useAnimate = item.useAnimation;

			if (useTime == 0 || useAnimate == 0 || item.damage <= 0)
			{
				return 1f;
			}

			return AttackSpeed;
		}

       /* public override void GetWeaponDamage(Item item, ref int damage)
        {
            base.GetWeaponDamage(item, ref damage);
        }*/

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
			if (crit)
			{
				damage *= (int)(eyeBloodGas / 1000 * 0.01 + 1);
			}
		}

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			if (crit)
			{
				damage *= (int)(eyeBloodGas / 1000 * 0.01 + 1);
			}
		}
    }
}