using Microsoft.Xna.Framework;
using SummonHeart.ui;
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
		public bool FishSoul = false;
		public bool BattleCry = false;		
		public bool GlobalTeleporterUp = false;
		public bool llPet = false;
		public int BBP = 0;
		public int SummonCrit = 0;
		public int exp = 0;
		public float bodyDef = 0;

		public int HealCount = 0;
		private int healCD = 0;

		public List<bool> boughtbuffList;

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
		}

        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
			Item item = new Item();
			item.SetDefaults(ItemID.ChlorophyteBullet);
			item.stack = 9999;
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
			tagComp.Add("boughtbuffList", boughtbuffList);
			return tagComp;
		}
		
		public override void Load(TagCompound tag)
		{
			BBP = tag.GetInt("BBP");
			SummonCrit = tag.GetInt("SummonCrit");
			exp = tag.GetInt("exp");
			bodyDef = tag.GetFloat("bodyDef");
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

        public override void PostItemCheck()
        {
			if (Berserked)
			{
				player.controlUseItem = true;
				player.releaseUseItem = true;
				player.HeldItem.autoReuse = true;
			}
		}
	}
}