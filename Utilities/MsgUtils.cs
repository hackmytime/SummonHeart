using SummonHeart.Extensions;
using SummonHeart.Items.Range.Tools;
using SummonHeart.Projectiles.Range.Bullet;
using SummonHeart.XiuXianModule.Entities;
using SummonHeart.XiuXianModule.Entities.Npc;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Utilities
{
    public static class MsgUtils
    {
		
		public static void SyncNpcVariables(Mod mod, NPC npc)
		{
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)Message.SyncNpcVariables);
			packet.Write((byte)npc.whoAmI);
			packet.Write(npc.life);
			packet.Send();
		}

		public static void SyncNpcData(Mod mod, NPC npc)
		{
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)Message.SyncNpcData);
			packet.Write((byte)npc.whoAmI);
			packet.Write(npc.lifeRegen);
			packet.Send();
		}

		public static void SyncKillResourceCount(Mod mod, Player player, int killResourceCount)
		{
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)Message.SyncKillResourceCount);
			packet.Write((byte)player.whoAmI);
			packet.Write(killResourceCount);
			packet.Send();

		}
		public static void SyncPlayerNpcVar(Mod mod, Player player, NPC npc)
		{
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)Message.SyncPlayerNpcVar);
			packet.Write((byte)player.whoAmI);
			packet.Write((byte)npc.whoAmI);
			packet.Send();
		}
		public static void SyncAnglerQuestReward()
        {
            ModPacket packet = SummonHeartMod.Instance.GetPacket();
            packet.Write((byte)Message.SyncAnglerQuestReward);
            packet.Send();
        }

        public static void SyncTime()
        {
            ModPacket packet = SummonHeartMod.Instance.GetPacket();
            packet.Write((byte)Message.SyncTime);
            packet.Send();
        }

        public static void SyncFallenStar()
        {
            ModPacket packet = SummonHeartMod.Instance.GetPacket();
            packet.Write((byte)Message.SyncFallenStar);
            packet.Send();
        }

        internal static void BuildHousePacket(int tileX, int tileY, int whoAmI)
        {
            ModPacket packet = SummonHeartMod.Instance.GetPacket();
            packet.Write((byte)Message.BuildHousePacket);
            packet.Write(tileX);
            packet.Write(tileY);
            packet.Write(whoAmI);
            packet.Send();
        }

        internal static void SpanNpcPacket(int whoAmI, short travellingMerchant)
        {
            ModPacket packet = SummonHeartMod.Instance.GetPacket();
            packet.Write((byte)Message.SpanNpcPacket);
            packet.Write(whoAmI);
            packet.Write(travellingMerchant);
            packet.Send();
        }

        internal static void BombPacket(int whoAmI)
        {
            ModPacket packet = SummonHeartMod.Instance.GetPacket();
            packet.Write((byte)Message.BombPacket);
            packet.Write((byte)whoAmI);
            packet.Send(-1, -1);
        }
        internal static void TurretShootPacket(int i, int projID, int shootDamage, float shootKnockback, float x, float y)
        {
            ModPacket packet = SummonHeartMod.Instance.GetPacket();
            packet.Write((byte)Message.TurretShootPacket);
            packet.Write(i);
            packet.Write(projID);
            packet.Write(shootDamage);
            packet.Write(shootKnockback);
            packet.Send(-1, -1);
        }
		static public string ParseBuffer(Dictionary<string, string> buffer)
		{
			string parsed = "";

			if (buffer.Count == 0)
				return parsed;
			foreach (KeyValuePair<string, string> entry in buffer)
			{
				parsed += entry.Key + ":" + entry.Value + ",";
			}

			return parsed.Remove(parsed.Length - 1);
		}
		internal static void SendNpcSpawn(Mod mod, NPC npc, int Tier, int Level, RPGGlobalNPC ARPGNPC)
		{

			if (Main.netMode == NetmodeID.Server)
			{

				ModPacket packet = mod.GetPacket();

				packet.Write((byte)Message.SyncNPCSpawn);
				packet.Write(npc.whoAmI);
				packet.Write(Level);
				packet.Write(Tier);
				packet.Write(ARPGNPC.getRank);
				packet.Write((int)ARPGNPC.modifier);
				packet.Write((string)ParseBuffer(ARPGNPC.specialBuffer));
				packet.Write(WorldManager.BossDefeated);

				packet.Send();
			}
		}

		static public void AskNpcInfo(Mod mod, NPC npc)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				ModPacket packet = mod.GetPacket();
				packet.Write((byte)Message.AskNpc);
				packet.Write(npc.whoAmI);
				packet.Send();
			}
		}

		static public void SendNpcUpdate(Mod mod, NPC npc, int ignore = -1)
		{
			if (Main.netMode == NetmodeID.Server)
			{
				ModPacket packet = mod.GetPacket();
				packet.Write((byte)Message.SyncNPCUpdate);
				packet.Write(npc.whoAmI);
				packet.Write(npc.life);
				packet.Write(npc.lifeMax);
				packet.Send();

			}
			else if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				ModPacket packet = mod.GetPacket();
				packet.Write((byte)Message.AskNpc);
				packet.Write(npc.whoAmI);
				packet.Send();
			}
		}

		static public void HandlePacket(BinaryReader reader, int whoAmI)
        {
			Message msg = (Message)reader.ReadByte();
			switch (msg)
			{
				case Message.SyncNpcVariables:
					{
						byte npc = reader.ReadByte();
						Main.npc[npc].life = reader.ReadInt32();
					}
					break;

				case Message.SyncNpcData: //update SoulSplit
					{
						if (Main.netMode == NetmodeID.MultiplayerClient)
						{
							int npc = reader.ReadByte();
							Main.npc[npc].lifeRegen = reader.ReadInt32();
						}
					}
					break;
				case Message.SyncKillResourceCount:
					{
						if (Main.netMode == NetmodeID.MultiplayerClient)
						{
							byte playernumber = reader.ReadByte();
							int heal = reader.ReadInt32();
							SummonHeartPlayer modPlayer = Main.player[playernumber].GetModPlayer<SummonHeartPlayer>();
							modPlayer.KillResourceCountMsg();
						}
					}
					break;
				case Message.SyncPlayerNpcVar:
					{
						if (Main.netMode == NetmodeID.MultiplayerClient)
						{
							byte playernumber = reader.ReadByte();
							int npc = reader.ReadByte();
							Main.player[playernumber].doKillNpcExp(Main.npc[npc]);
						}
					}
					break;
				case Message.SyncAnglerQuestReward:
					{
						Main.AnglerQuestSwap();
					}
					break;
				case Message.SyncTime:
					{
						Main.time = 54000.0;
						CultistRitual.delay = 0;
						CultistRitual.recheck = 0;
					}
					break;
				case Message.SyncFallenStar:
					{
						SummonHeartWorld.StarMulti = 100;
						SummonHeartWorld.StarMultiTime = 60 * 60 * 12;
					}
					break;
				case Message.BuildHousePacket:
					AutoHouseTool.HandleBuilding2(reader.ReadInt32(), reader.ReadInt32(), whoAmI);
					break;
				case Message.SpanNpcPacket:
					{
						NPC.SpawnOnPlayer(reader.ReadInt32(), reader.ReadInt16());
						NetMessage.SendData(61, -1, -1, null, reader.ReadInt16(), NPCID.TravellingMerchant, 0f, 0f, 0, 0, 0);
					}
					break;
				case Message.BombPacket:
					{
						if (Main.netMode == NetmodeID.MultiplayerClient)
						{
							byte playernumber = reader.ReadByte();
							SummonHeartPlayer modPlayer = Main.player[playernumber].GetModPlayer<SummonHeartPlayer>();
							modPlayer.detonate = true;
						}
					}
					break;
				case Message.TurretShootPacket:
					{
						if (Main.netMode == NetmodeID.MultiplayerClient)
						{
							if (Main.LocalPlayer.whoAmI != 0)
								return;
							/*byte playernumber = reader.ReadByte();
							SummonHeartPlayer modPlayer = Main.player[playernumber].GetModPlayer<SummonHeartPlayer>();
							modPlayer.detonate = true;*/
							int i = reader.ReadInt32();
							int projID = reader.ReadInt32();
							int shootDamage = reader.ReadInt32();
							int shootKnockback = reader.ReadInt32();
							NPC target = Main.npc[i];
							int projIndex = Projectile.NewProjectile(target.Center.X, target.Center.Y, 0f, 0f, ModContent.ProjectileType<BoltBulletPro>(), shootDamage, shootKnockback, Main.myPlayer);
							Main.projectile[projIndex].netUpdate = true;
						}
					}
					break;
				case Message.SyncLevel:
					int playerId = reader.ReadInt32();
					int amount = reader.ReadInt32();
					int statLife = reader.ReadInt32();
					int statLifeMax2 = reader.ReadInt32();
					RPGPlayer p = Main.player[playerId].GetModPlayer<RPGPlayer>();

					if (playerId != Main.myPlayer)
					{
						if (Main.netMode != NetmodeID.SinglePlayer)
							p.SyncLevel(amount);
						if (Main.netMode != NetmodeID.Server)
						{
							Main.player[playerId].statLife = statLife;
							Main.player[playerId].statLifeMax2 = statLifeMax2;
							WorldManager.instance.NetUpdateWorld();
						}
					}

					WorldManager.PlayerLevel = Math.Max(WorldManager.PlayerLevel, p.GetLevel());

					break;
				case Message.AddXP:
					int exp = reader.ReadInt32();
					int level = reader.ReadInt32();
					Main.LocalPlayer.GetModPlayer<RPGPlayer>().AddXp(exp, level);
					break;
				/*case Message.SyncNPCSpawn:
					if (Main.netMode == NetmodeID.MultiplayerClient)
					{


						NPC npc = Main.npc[(int)tags[DataTag.npcId]];

						if (npc.GetGlobalNPC<ARPGGlobalNPC>() == null)
							AnotherRpgMod.Instance.Logger.Info("Sync NPC Spawn | name :" + npc.GivenName);

						//npc.SetDefaults(npc.type);
						if (npc.GetGlobalNPC<ARPGGlobalNPC>().StatsCreated == true)
							return;
						int tier = (int)tags[DataTag.tier];
						int level = (int)tags[DataTag.level];
						NPCRank rank = (NPCRank)tags[DataTag.rank];

						NPCModifier modifiers = (NPCModifier)tags[DataTag.modifiers];
						if (npc == null || npc.GetGlobalNPC<ARPGGlobalNPC>() == null)
							return;
						AnotherRpgMod.Instance.Logger.Info(npc.GivenOrTypeName + "\nTier : " + tier + "   Level : " + level + "   rank : " + rank + "   Modifier  : " + modifiers + " \n Buffer : " + (string)tags[DataTag.buffer]);

						Dictionary<string, string> bufferStack = Unparse((string)tags[DataTag.buffer]);

						WorldManager.BossDefeated = (int)tags[DataTag.WorldTier];

						npc.GetGlobalNPC<ARPGGlobalNPC>().StatsCreated = true;
						npc.GetGlobalNPC<ARPGGlobalNPC>().modifier = modifiers;
						npc.GetGlobalNPC<ARPGGlobalNPC>().SetLevelTier(level, tier, (byte)rank);
						npc.GetGlobalNPC<ARPGGlobalNPC>().specialBuffer = bufferStack;

						npc.GetGlobalNPC<ARPGGlobalNPC>().SetStats(npc);

						npc.GivenName = NPCUtils.GetNpcNameChange(npc, tier, level, rank);
						npc.life = npc.lifeMax;


						//AnotherRpgMod.Instance.Logger.Info("NPC created with id : " + npc.whoAmI);
						//AnotherRpgMod.Instance.Logger.Info( "Client Side : \n" + npc.GetGivenOrTypeNetName() + "\nLvl." + (npc.GetGlobalNPC<ARPGGlobalNPC>().getLevel + npc.GetGlobalNPC<ARPGGlobalNPC>().getTier) + "\nHealth : " + npc.life + " / " + npc.lifeMax + "\nDamage : " + npc.damage + "\nDef : " + npc.defense + "\nTier : " + npc.GetGlobalNPC<ARPGGlobalNPC>().getRank + "\n\n");

					}
					break;*/

				case Message.SyncNPCUpdate:
					if (Main.netMode == NetmodeID.MultiplayerClient)
					{
						int npcId = reader.ReadInt32();
						int npcLife = reader.ReadInt32();
						int npcMaxLife = reader.ReadInt32();
						NPC npcu = Main.npc[npcId];

						if (npcu.lifeMax != npcMaxLife || npcu.life != npcLife)
						{
						}
						Main.npc[npcId].lifeMax = npcMaxLife;
						Main.npc[npcId].life = npcLife;
					}
					break;
            }
		}
    }
}
