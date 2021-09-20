using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Models;
using SummonHeart.NPCs;
using SummonHeart.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Extensions
{
    /// <summary>
    ///     A class housing all the player/ModPlayer/MyPlayer based extensions
    /// </summary>
    public static class PlayerExtensions
    {
		public static SummonHeartPlayer getModPlayer(this Player player) => player.GetModPlayer<SummonHeartPlayer>();
		public static void DrawAura(this SummonHeartPlayer modPlayer, AuraAnimationInfo aura)
        {
            Player player = modPlayer.player;
            Texture2D texture = aura.GetTexture();
            Rectangle textureRectangle = new Rectangle(0, aura.GetHeight() * modPlayer.auraCurrentFrame, texture.Width, aura.GetHeight());
            float scale = aura.GetAuraScale(modPlayer);
            Tuple<float, Vector2> rotationAndPosition = aura.GetAuraRotationAndPosition(modPlayer);
            float rotation = rotationAndPosition.Item1;
            Vector2 position = rotationAndPosition.Item2;

            AnimationHelper.SetSpriteBatchForPlayerLayerCustomDraw(aura.blendState, player.GetPlayerSamplerState());

            // custom draw routine
            Main.spriteBatch.Draw(texture, position - Main.screenPosition, textureRectangle, Color.White, rotation, new Vector2(aura.GetWidth(), aura.GetHeight()) * 0.5f, scale, SpriteEffects.None, 0f);

            AnimationHelper.ResetSpriteBatchForPlayerDrawLayers(player.GetPlayerSamplerState());
        }

        public static SamplerState GetPlayerSamplerState(this Player player)
        {
            return player.mount.Active ? Main.MountedSamplerState : Main.DefaultSamplerState;
        }

        public static int getPower(this SummonHeartPlayer modPlayer)
        {
            int power = 0;
            int x = 1;
            if (Main.hardMode)
            {
                x = 2;
            }
            if (NPC.downedMoonlord)
            {
                x = 5;
            }
            power = modPlayer.eyeBloodGas + modPlayer.handBloodGas + modPlayer.bodyBloodGas + modPlayer.footBloodGas;
            power += modPlayer.player.statLifeMax2 * x;
            power += modPlayer.player.statDefense * 30;
            power += modPlayer.SummonCrit * 20 * x;
            power += modPlayer.killResourceMax2 * 10;
            Item item = modPlayer.player.HeldItem;
            if (item.damage > 0)
            {
                power += (int)(item.damage * 5 * (60f / (float)item.useTime));
            }
           
            return power;
        }

        public static int getAllBloodGas(this SummonHeartPlayer modPlayer)
        {
            int all = 0;
            all = modPlayer.eyeBloodGas + modPlayer.handBloodGas + modPlayer.bodyBloodGas + modPlayer.footBloodGas;
            return all;
        }

		

		public static int getFishLevel(this SummonHeartPlayer mp)
		{
			int fishLevel = 0;
            int fishCount = mp.fishCount;
			int levelCount = 0;
			for(fishLevel = 0; fishLevel < 100; fishLevel++)
            {
				int curCount = 1 + fishLevel;
				levelCount += curCount;
				if (fishCount < levelCount)
					break;
            }
			mp.nextFishCount = levelCount;
			return fishLevel;
		}

		public static int HasItemInAcc(this Player player, int type)
        {
            for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
            {
                if (player.armor[i].type == type)
                {
                    return i;
                }
            }
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
			for (int i = 0; i < SummonHeartPlayer.MaxExtraAccessories; i++)
			{
				if (mp.ExtraAccessories[i].type == type)
				{
					return i;
				}
			}
			return -1;
        }

		public static Item GetItemInAcc(this Player player, int type)
		{
			for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
			{
				if (player.armor[i].type == type)
				{
					return player.armor[i];
				}
			}
			SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
			for (int i = 0; i < SummonHeartPlayer.MaxExtraAccessories; i++)
			{
				if (mp.ExtraAccessories[i].type == type)
				{
					return mp.ExtraAccessories[i];
				}
			}
			return null;
		}

		public static int HasItemInInventory(this Player player, int type)
		{
			SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
			for (int i = 0; i < player.inventory.Length; i++)
			{
				if (player.inventory[i].type == type)
				{
					return i;
				}
			}
			return -1;
		}

		public static int getMaxPickPowerInInventory(this Player player)
		{
			int pickPower = -1;
			SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
			for (int i = 0; i < player.inventory.Length; i++)
			{
				if (player.inventory[i].pick > pickPower)
				{
					pickPower = player.inventory[i].pick;
				}
			}
			return pickPower;
		}

		public static void CostItem(this Player player, int type, int count)
		{
			foreach (var v in player.inventory)
			{
				if (v != null)
				{
					if (v.type == type)
					{
						if (v.stack <= count)
						{
							count -= v.stack;
							v.TurnToAir();
						}
						else
						{
							v.stack -= count;
							break;
						}
					}
				}
			}
		}

		public static void doKillNpcExp(this Player player, NPC npc)
        {
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			if (npc.boss)
				modPlayer.killAnyBoss = true;

			int addKillCount = (int)modPlayer.manaExp;
			modPlayer.killNpcCount += addKillCount;
			
			if(modPlayer.PlayerClass == 5 && modPlayer.boughtbuffList[0])
            {
				int healMana = modPlayer.eyeBloodGas / 1000 + 10;
				player.HealMana(healMana);
            }

			if (modPlayer.SummonHeart)
			{
				int addExp = 0;
				int addBloodGas = 0;
				int powerLevel = npc.getPowerLevel();

				if (npc.boss)
				{
					if (powerLevel == -1)
					{
						addExp = 1;
					}
					else
					{
                        int downedBossIndex = player.getDownedBossIndex();
						addExp = 100 + 100 * downedBossIndex;
					}
				}
				else
				{
					addExp = 1;
				}
				//越阶战斗奖励
				if (npc.boss && powerLevel > 0)
				{
					if (powerLevel >= 10)
						powerLevel = 10;
					if(modPlayer.PlayerClass == 3)
                    {
						//召唤职业最大5倍
						if (powerLevel >= 5)
							powerLevel = 5;
					}
					addExp *= (powerLevel + 1);
				}

				//处理灵魂
				//处理难度额外灵魂
				int hardMulti = SummonHeartWorld.WorldLevel;
				if (hardMulti > 0 && !npc.boss)
				{
					addExp *= hardMulti;
				}
				//处理贪婪词缀增加
				addExp = (int)(addExp * modPlayer.manaExp);
				modPlayer.BBP += addExp;
				if (modPlayer.BBP > 5000000)
					modPlayer.BBP = 5000000;

				if (npc.boss)
				{
					string text = "";
					if (powerLevel == -1)
					{
						text = $"{player.name}的战力碾压{npc.FullName}，可惜，其血肉灵魂已于{player.name}无用！灵魂之力+{addExp}";
					}
					if (powerLevel == 0)
					{
						text = $"{player.name}吞噬了{npc.FullName}的灵魂，灵魂之力+{addExp}";
					}
					if (powerLevel > 0)
					{
						text = $"{player.name}越级吞噬了{npc.getPowerLevelText()}强者{npc.FullName}的灵魂，获得额外{powerLevel}倍灵魂之力，+{addExp}灵魂之力";
					}

					Main.NewText(text, Color.Green);
				}

				/*//处理突破
				//最大血气上限【规则】
				int WORLDMAXBLOODGAS = SummonHeartWorld.WorldBloodGasMax;
				//int MAXBLOODGAS = 200000;
				if (powerLevel > 0 && npc.getPower() > modPlayer.bloodGasMax)
				{
					//突破的数值=（敌人战力-玩家肉身极限）/  5 * (阶位)
					int addMax = (npc.getPower() - modPlayer.bloodGasMax) / 5;

					if (powerLevel >= 5)
						addMax = npc.getPower() - modPlayer.bloodGasMax;
					else
					{
						addMax *= (powerLevel);
					}
					modPlayer.bloodGasMax += addMax;
					//判断是否超过世界上限
					if (modPlayer.bloodGasMax > WORLDMAXBLOODGAS)
						modPlayer.bloodGasMax = WORLDMAXBLOODGAS;
					string text = $"{player.name}越级斩杀了{npc.getPowerLevelText()}强者{npc.FullName}，于生死之间突破肉身极限，+{addMax}肉身极限";
					Main.NewText(text, Color.Green);
				}*/

				//修炼开始
				int bloodGasMax = modPlayer.bloodGasMax;

				if (modPlayer.getAllBloodGas() < SummonHeartWorld.WorldBloodGasMax)
				{
					addBloodGas = addExp;

					//修炼魔神之眼
					if (modPlayer.practiceEye && modPlayer.eyeBloodGas < bloodGasMax)
					{
						if (modPlayer.eyeBloodGas < modPlayer.eyeMax)
						{
							//判断是否超上限
							if (modPlayer.CheckSoul(addExp))
							{
								modPlayer.BuySoul(addExp);
								modPlayer.eyeBloodGas += addBloodGas;
								if (modPlayer.eyeBloodGas > bloodGasMax)
									modPlayer.eyeBloodGas = bloodGasMax;
								if (modPlayer.eyeBloodGas > modPlayer.eyeMax)
									modPlayer.eyeBloodGas = modPlayer.eyeMax;
								if (npc.boss)
								{
									string text = "";
									if (powerLevel == 0)
									{
										text = $"{player.name}修炼魔神之眼消耗{addExp}灵魂之力吞噬了{npc.FullName}的气血，魔神之眼气血+{addBloodGas}";
									}
									if (powerLevel > 0)
									{
										text = $"{player.name}修炼魔神之眼消耗{addExp}灵魂之力越级吞噬了{npc.getPowerLevelText()}强者{npc.FullName}的气血，额外吸收{powerLevel}倍气血，魔神之眼气血+{addBloodGas}";
									}
									Main.NewText(text, Color.Green);
								}
							}
						}
					}
					//修炼魔神之手
					if (modPlayer.practiceHand && modPlayer.handBloodGas < bloodGasMax)
					{
						int handMaxBloodGas = modPlayer.handMax;
						
						if (modPlayer.handBloodGas < modPlayer.handMax)
						{
							
							if (modPlayer.CheckSoul(addExp))
							{
								modPlayer.BuySoul(addExp);
								modPlayer.handBloodGas += addBloodGas;
								if (modPlayer.handBloodGas > bloodGasMax)
									modPlayer.handBloodGas = bloodGasMax;
								if (modPlayer.handBloodGas > handMaxBloodGas)
									modPlayer.handBloodGas = handMaxBloodGas;
								if (npc.boss)
								{
									string text = "";
									if (powerLevel == 0)
									{
										text = $"{player.name}修炼魔神之手消耗{addExp}灵魂之力吞噬了{npc.FullName}的气血，魔神之手气血+{addBloodGas}";
									}
									if (powerLevel > 0)
									{
										text = $"{player.name}修炼魔神之手消耗{addExp}灵魂之力越级吞噬了{npc.getPowerLevelText()}强者{npc.FullName}的气血，额外吸收{powerLevel}倍气血，魔神之手气血+{addBloodGas}";
									}
									Main.NewText(text, Color.Green);
								}
							}
						}
					}
					//修炼魔神之躯
					if (modPlayer.practiceBody)
					{
						int bodyMaxBloodGas = modPlayer.bodyMax;
						int bodyMax = bloodGasMax;
					
						if (modPlayer.bodyBloodGas < bodyMaxBloodGas && modPlayer.bodyBloodGas < bodyMax)
						{
							if (modPlayer.CheckSoul(addExp))
							{
								modPlayer.BuySoul(addExp);
								modPlayer.bodyBloodGas += addBloodGas;
								if (modPlayer.bodyBloodGas > bodyMaxBloodGas)
									modPlayer.bodyBloodGas = bodyMaxBloodGas;
								if (modPlayer.bodyBloodGas > bodyMax)
									modPlayer.bodyBloodGas = bodyMax;
								if (npc.boss)
								{
									string text = "";
									if (powerLevel == 0)
									{
										text = $"{player.name}修炼魔神之躯消耗{addExp}灵魂之力吞噬了{npc.FullName}的气血，魔神之躯气血+{addBloodGas}";
									}
									if (powerLevel > 0)
									{
										text = $"{player.name}修炼魔神之躯消耗{addExp}灵魂之力越级吞噬了{npc.getPowerLevelText()}强者{npc.FullName}的气血，额外吸收{powerLevel}倍气血，魔神之躯气血+{addBloodGas}";
									}
									Main.NewText(text, Color.Green);
								}
							}
						}
					}
					//修炼魔神之腿
					if (modPlayer.practiceFoot && modPlayer.footBloodGas < bloodGasMax)
					{
						if (modPlayer.footBloodGas < modPlayer.footMax)
						{
							if (modPlayer.CheckSoul(addExp))
							{
								modPlayer.BuySoul(addExp);
								modPlayer.footBloodGas += addBloodGas;
								if (modPlayer.footBloodGas > modPlayer.footMax)
									modPlayer.footBloodGas = modPlayer.footMax;
								if (modPlayer.footBloodGas > bloodGasMax)
									modPlayer.footBloodGas = bloodGasMax;
								if (npc.boss)
								{
									string text = "";
									if (powerLevel == 0)
									{
										text = $"{player.name}修炼魔神之腿消耗{addExp}灵魂之力吞噬了{npc.FullName}的气血，魔神之腿气血+{addBloodGas}";
									}
									if (powerLevel > 0)
									{
										text = $"{player.name}修炼魔神之腿消耗{addExp}灵魂之力越级吞噬了{npc.getPowerLevelText()}强者{npc.FullName}的气血，额外吸收{powerLevel}倍气血，魔神之腿气血+{addBloodGas}";
									}
									Main.NewText(text, Color.Green);
								}
							}
						}
					}
				}
				modPlayer.dealLevel();
			}
		}

		public static SummonHeartPlayer SH(this Player player) => player.GetModPlayer<SummonHeartPlayer>();
		public static SummonHeartGlobalNPC SummonHeart(this NPC npc) => npc.GetGlobalNPC<SummonHeartGlobalNPC>();


		public static Color ColorOrOther(this SummonHeartPlayer modPlayer, Color other)
		{
			if (!modPlayer.useOscColor)
			{
				return other;
			}
			return modPlayer.oscColor;
		}

		public static bool AnyBossAlive(this Player player)
		{
			for (int i = 0; i < Main.npc.Length; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.active && npc.boss) return true;
				if (npc.active && npc.type == NPCID.EaterofWorldsHead) return true;
			}
			return false;
		}

		public static bool CheckItemsFromOtherMods(this Player player)
		{
			foreach (Item item in player.inventory)
			{
				if (item.modItem != null && SummonHeartMod.rejPassItemList.Contains(item.modItem.Name))
				{
					return true;
				}
			}
			return false;
		}

		///<摘要>
		///通过免疫和摄像机lerp管理破折号的逻辑。
		///也接管了sparket.frame，所以在一个普通的斜杠ai之后调用它。
		///</summary>
		///<param name=“dashFrameDuration”>要冲刺的帧数
		///<param name=“dashSpeed”>短跑速度，例如player.maxRunSpeed*5f</param>
		///<param name=“freezeFrame”>要在例如2处冻结的帧
		///<param name=“dashEndVelocity”>结束速度，或为空以使用短划线速度，例如preDashVelocity</param>
		///<returns>如果当前在破折号中，则为True</returns>
		public static bool AIDashSlash(this Player player, Projectile projectile, float dashFrameDuration, float dashSpeed, int freezeFrame, ref Vector2? dashEndVelocity)
		{
			if (player.dead || !player.active)
			{
				projectile.timeLeft = 0;
				return false;
			}
			if (freezeFrame < 1) freezeFrame = 1;

			bool dashing = false;
			if ((int)projectile.ai[0] < dashFrameDuration)
			{
				// Fine-tuned tilecollision
				player.armorEffectDrawShadow = true;
				Vector2 projVel = projectile.velocity;
				if (player.gravDir < 0) projVel.Y = -projVel.Y;
				for (int i = 0; i < 4; i++)
				{
					player.position += Collision.TileCollision(player.position, projVel * dashSpeed / 4,
						player.width, player.height, false, false, (int)player.gravDir);
				}

				if (player.velocity.Y == 0)
				{ player.velocity = new Vector2(0, (projectile.velocity * dashSpeed).Y); }
				else
				{ player.velocity = new Vector2(0, player.gravDir * player.gravity); }

				// Prolong mid-slash player animation
				if (player.direction < 0) projectile.position.X += projectile.width;

				float dist = Math.Max(0, projectile.width - projectile.height); // total distance covered by the moving hitbox

				Vector2 direction = new Vector2(
					(float)Math.Cos(projectile.rotation),
					(float)Math.Sin(projectile.rotation));
				direction.Y *= player.gravDir;
				Vector2 centre = player.MountedCenter;
				Vector2 playerOffset = player.Size.X * projectile.scale * direction;

				projectile.Center = centre
					+ direction * (dist + projectile.height) / 2
					- playerOffset;
				if (player.itemAnimation <= player.itemAnimationMax - freezeFrame)
				{ player.itemAnimation = player.itemAnimationMax - freezeFrame; }

				// Set immunities
				player.immune = true;
				player.immuneTime = Math.Max(player.immuneTime, 6);
				player.immuneNoBlink = true;

				dashing = true;
			}
			else if ((int)projectile.ai[0] >= dashFrameDuration && dashEndVelocity != new Vector2(float.MinValue, float.MinValue))
			{
				if (dashEndVelocity == null)
				{
					Vector2 projVel = projectile.velocity.SafeNormalize(Vector2.Zero);
					if (player.gravDir < 0) projVel.Y = -projVel.Y;
					float speed = dashSpeed / 4f;
					if (speed < player.maxFallSpeed)
					{ player.velocity = projVel * speed; }
					else
					{ player.velocity = projVel * player.maxFallSpeed; }

					// Reset fall damage
					player.fallStart = (int)(player.position.Y / 16f);
					player.fallStart2 = player.fallStart;
				}
				else
				{
					player.velocity = (Vector2)dashEndVelocity;
				}

				// Set the vector to a "reset" state
				dashEndVelocity = new Vector2(float.MinValue, float.MinValue);
			}

			// Trigger lerp by offsetting camera
			if (projectile.timeLeft == 60)
			{
				Main.SetCameraLerp(0.1f, 10);
				Main.screenPosition -= projectile.velocity * 2;
			}

			// Set new projectile frame
			projectile.frame = (int)Math.Max(0, projectile.ai[0] - dashFrameDuration);

			return dashing;
		}

		public static string getDownedBoss(this Player player)
		{
			string[] bossTips = new string[]
			{
				"未吞噬任何Boss：25基础攻击",
				"已吞噬史莱姆王：26基础攻击",
				"已吞噬克苏鲁之眼：28基础攻击",
				"已吞噬世吞/克脑：30基础攻击",
				"已吞噬蜂王：35基础攻击",
				"已吞噬骷髅王：42基础攻击",
				"已吞噬血肉之墙：56基础攻击",
				"已吞噬任意新三王：70基础攻击",
				"已吞噬世纪之花：90基础攻击",
				"已吞噬猪鲨公爵：120基础攻击",
				"已吞噬石巨人：140基础攻击",
				"已吞噬邪教徒：160基础攻击",
				"已吞噬月球领主：200基础攻击"
			};
			int downedIndex = 0;
			//1、2W - king slime5 %
			if (NPC.downedSlimeKing)
			{
				downedIndex = 1;
			}
			//2、3W - bigEye10
			if (NPC.downedBoss1)
			{
				downedIndex = 2;
			}
			//3、4W - 世吞 / 克脑20
			if (NPC.downedBoss2)
			{
				downedIndex = 3;
			}
			//4、6W - 蜂王30
			if (NPC.downedQueenBee)
			{
				downedIndex = 4;
			}
			//5、7W - 吴克40
			if (NPC.downedBoss3)
			{
				downedIndex = 5;
			}
			//6、8W - 肉山50
			if (Main.hardMode)
			{
				downedIndex = 6;
			}
			//7、10W-新三王80
			if (NPC.downedMechBossAny)
			{
				downedIndex = 7;
			}
			//8、12W - 小花100
			if (NPC.downedPlantBoss)
			{
				downedIndex = 8;
			}
			//9、14W - 小怪120
			if (NPC.downedFishron)
			{
				downedIndex = 9;
			}
			//10、16W - 石头150
			if (NPC.downedGolemBoss)
			{
				downedIndex = 10;
			}
			//11、18W - 教徒200
			if (NPC.downedAncientCultist)
			{
				downedIndex = 11;
			}

			//12、20W - 月总无上限*/
			if (NPC.downedMoonlord)
			{
				downedIndex = 12;
			}
			return bossTips[downedIndex];
		}

		public static int getDownedBossDmage(this Player player)
		{
			int[] bossTips = new int[]
			{
				25,
				26,
				28,
				30,
				35,
				42,
				56,
				70,
				90,
				120,
				140,
				160,
				200
			};
			int downedIndex = player.getDownedBossIndex();
			return bossTips[downedIndex];
		}

		public static int getDownedBossIndex(this Player player)
		{
			int downedIndex = 0;
			//1、2W - king slime5 %
			if (NPC.downedSlimeKing)
			{
				downedIndex = 1;
			}
			//2、3W - bigEye10
			if (NPC.downedBoss1)
			{
				downedIndex = 2;
			}
			//3、4W - 世吞 / 克脑20
			if (NPC.downedBoss2)
			{
				downedIndex = 3;
			}
			//4、6W - 蜂王30
			if (NPC.downedQueenBee)
			{
				downedIndex = 4;
			}
			//5、7W - 吴克40
			if (NPC.downedBoss3)
			{
				downedIndex = 5;
			}
			//6、8W - 肉山50
			if (Main.hardMode)
			{
				downedIndex = 6;
			}
			//7、10W-新三王80
			if (NPC.downedMechBossAny)
			{
				downedIndex = 7;
			}
			//8、12W - 小花100
			if (NPC.downedPlantBoss)
			{
				downedIndex = 8;
			}
			//9、14W - 小怪120
			if (NPC.downedFishron)
			{
				downedIndex = 9;
			}
			//10、16W - 石头150
			if (NPC.downedGolemBoss)
			{
				downedIndex = 10;
			}
			//11、18W - 教徒200
			if (NPC.downedAncientCultist)
			{
				downedIndex = 11;
			}

			//12、20W - 月总无上限*/
			if (NPC.downedMoonlord)
			{
				downedIndex = 12;
			}
			return downedIndex;
		}

		public static void UpdateCoins(this Player p)
		{
			if (p.whoAmI == Main.myPlayer)
			{
				long num = 0L;
				long num2 = 0L;
				long num3 = 0L;
				long num4 = 0L;
				int num5 = 0;
				int num6 = 0;
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				int num7 = 0;
				int num8 = CalculateSlots(p, ref num, ref num2, ref num3, ref num4, ref num5);
				if (num8 == -1)
				{
					return;
				}
				for (int i = 0; i < 40; i++)
				{
					Item item = p.bank.item[i];
					if (item.IsAir || (item.type >= 71 && item.type <= 74))
					{
						num6++;
						if (num6 >= num5)
						{
							break;
						}
					}
				}
				if (num6 >= num5)
				{
					for (int j = 50; j < 54; j++)
					{
						Item item = p.inventory[j];
						if (item.type >= 71 && item.type <= 74)
						{
							p.inventory[j].TurnToAir();
						}
					}
					for (int k = 0; k < 40; k++)
					{
						Item item = p.bank.item[k];
						if (item.type >= 71 && item.type <= 74)
						{
							p.bank.item[k].TurnToAir();
						}
					}
					for (int l = 39; l >= 0; l--)
					{
						if (p.bank.item[l].IsAir)
						{
							if (num7 + 1 < num8)
							{
								p.bank.item[l] = new Item();
								p.bank.item[l].SetDefaults(74, false);
								p.bank.item[l].stack = 999;
								num4 -= 999L;
								num7++;
							}
							else if (num7 + 1 == num8)
							{
								p.bank.item[l] = new Item();
								p.bank.item[l].SetDefaults(74, false);
								p.bank.item[l].stack = (int)num4;
								num7++;
							}
							else if (!flag3 && num3 > 0L)
							{
								p.bank.item[l] = new Item();
								p.bank.item[l].SetDefaults(73, false);
								p.bank.item[l].stack = (int)num3;
								flag3 = true;
							}
							else if (!flag2 && num2 > 0L)
							{
								p.bank.item[l] = new Item();
								p.bank.item[l].SetDefaults(72, false);
								p.bank.item[l].stack = (int)num2;
								flag2 = true;
							}
							else if (!flag && num > 0L)
							{
								p.bank.item[l] = new Item();
								p.bank.item[l].SetDefaults(71, false);
								p.bank.item[l].stack = (int)num;
								break;
							}
						}
					}
					if (Main.playerInventory)
					{
						Recipe.FindRecipes();
					}
				}
			}
		}

		private static int CalculateSlots(Player player, ref long copper, ref long silver, ref long gold, ref long platinum, ref int slots)
		{
			int num = 0;
			for (int i = 50; i < 54; i++)
			{
				Item item = player.inventory[i];
				if (item.type >= 71 && item.type <= 74)
				{
					copper += (long)((double)item.stack * Math.Pow(100.0, (double)(item.type - 71)));
				}
			}
			if (copper > 0L)
			{
				for (int j = 0; j < 40; j++)
				{
					Item item = player.bank.item[j];
					if (item.type >= 71 && item.type <= 74)
					{
						copper += (long)((double)item.stack * Math.Pow(100.0, (double)(item.type - 71)));
					}
				}
				ValueCalc(ref copper, ref silver, ref gold, ref platinum);
				if (platinum > 0L)
				{
					num = ((platinum % 999L == 0L) ? ((int)(platinum / 999L)) : ((int)(platinum / 999L + 1L)));
				}
				slots = ((gold > 0L) ? 1 : 0) + ((silver > 0L) ? 1 : 0) + ((copper > 0L) ? 1 : 0) + num;
				return num;
			}
			return -1;
		}

		private static void ValueCalc(ref long copper, ref long silver, ref long gold, ref long platinum)
		{
			platinum = copper / 1000000L;
			copper -= platinum * 1000000L;
			gold = copper / 10000L;
			copper -= gold * 10000L;
			silver = copper / 100L;
			copper -= silver * 100L;
		}
	}
}