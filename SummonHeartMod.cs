using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SummonHeart.body;
using SummonHeart.Extensions;
using SummonHeart.Items;
using SummonHeart.ui;
using SummonHeart.ui.Bar;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace SummonHeart
{
    public class SummonHeartMod : Mod
	{
	/*	public static SummonHeartMod instance;*/

		internal static List<BuffValue> modBuffValues = new List<BuffValue>();

		static List<Projectile> slowProList = new List<Projectile>();
		public static List<String> rejPassItemList = new List<String>();

		static Dictionary<Projectile, Vector2> oldProMap = new Dictionary<Projectile, Vector2>();
		public static Dictionary<Item, LegacySoundStyle> itemSoundMap = new Dictionary<Item, LegacySoundStyle>();

		public static int DustIDSlashFX;

		// Hotkeys
		internal static ModHotKey AutoAttackKey;
		internal static ModHotKey ShowUI;
		internal static ModHotKey KillSkillKey;
		internal static ModHotKey TransKey;
		//internal static ModHotKey ExtraAccessaryKey;

		internal PanelMelee PanelMeleeUI;
		internal PanelKill PanelKillUI;
		internal PanelSummon panelSummonUI;
		public UserInterface somethingInterface;

		internal PanelMelee2 melee2UI;
		public UserInterface melee2Interface;
		
		internal PanelMagic magicUI;
		public UserInterface magicInterface;

		internal PanelMagic2 magic2UI;
		public UserInterface magic2Interface;

		/*internal PanelGodSoul godSoulUI;
		public UserInterface godSoulInterface;*/

		internal MagicCharge magicCharge;
		private UserInterface _magicChargeUserInterface;

		internal KillBar ExampleResourceBar;
		private UserInterface KillResourceBarUserInterface;

		internal DeathBar DeathResourceBar;
		private UserInterface DeathResourceBarInterface;
		
		internal AngerBar angerBar;
		private UserInterface angerBarInterface;

		public static SummonHeartMod Instance;

		public SummonHeartMod()
		{
			modBuffValues = new List<BuffValue>();
			modBuffValues = VanilaBuffs.getVanilla();
			Instance = this;
			rejPassItemList.Add("UnlimitedManaAccessory");
			rejPassItemList.Add("UnlimitedManaAccessory1");
			rejPassItemList.Add("UnlimitedManaAccessory2");
			rejPassItemList.Add("UnlimitedManaAccessory3");
			rejPassItemList.Add("UnlimitedManaAccessory4");
			rejPassItemList.Add("UnlimitedManaAccessory5");
		}

		public static void addSlowMap(Projectile p)
		{
			//判断是否包含
			if (!slowProList.Contains(p))
            {
				slowProList.Add(p);
                Vector2 velocity = new Vector2();
				velocity = p.velocity;
				oldProMap[p] = velocity;
			}
		}

		public static void deleteSlowMap(Projectile p)
		{
			//判断是否包含
			if (slowProList.Contains(p))
			{
				p.velocity = oldProMap[p];
				slowProList.Remove(p);
				oldProMap.Remove(p);
			}
		}

		private void updateMap()
		{
			Player player = Main.player[Main.myPlayer];
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			//遍历value
			List<Projectile> delList = new List<Projectile>();
			foreach (Projectile p in slowProList)
			{
				//判断p是否存活
				if (!p.active)
				{
					delList.Add(p);
				}
				else
				{
					Vector2 vector2 = oldProMap[p];
					p.velocity = vector2 *= (1 - (modPlayer.eyeBloodGas / 5000 * 0.01f + 0.2f));
				}
			}

			//删除死亡的弹幕
			foreach (Projectile p in delList)
			{
				slowProList.Remove(p);
			}
		}

		public override void Load()
        {
			AutoAttackKey = RegisterHotKey("自动使用武器（再次点击停止使用）", "G");
			ShowUI = RegisterHotKey("魔神炼体法", Keys.L.ToString());
			KillSkillKey = RegisterHotKey("刺客刺杀技能(可开关)", Keys.V.ToString());
			TransKey = RegisterHotKey("空间传送", Keys.Y.ToString());
			//ExtraAccessaryKey = RegisterHotKey("额外饰品栏带单", Keys.Q.ToString());
			// this makes sure that the UI doesn't get opened on the server
			// the server can't see UI, can it? it's just a command prompt
			if (!Main.dedServ)
			{
				try
				{
					Ref<Effect> trailRef = new Ref<Effect>(base.GetEffect("Effects/Trail"));
					GameShaders.Misc["SummonHeart:CircleReveal"] = new MiscShaderData(new Ref<Effect>(base.GetEffect("Effects/CircleReveal")), "CircleReveal");
					GameShaders.Misc["SummonHeart:SoftTrail"] = new MiscShaderData(trailRef, "SoftTrail");
					GameShaders.Misc["SummonHeart:SwordTrail"] = new MiscShaderData(trailRef, "SwordTrail");
					GameShaders.Misc["SummonHeart:SolidTrail"] = new MiscShaderData(trailRef, "RacketTrail");
					SplitGlowMask.Load();
					Player player = Main.player[Main.myPlayer];
					SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
					if (modPlayer.PlayerClass == 1)
					{
						PanelMeleeUI = new PanelMelee();
						PanelMeleeUI.Initialize();
						somethingInterface = new UserInterface();
						somethingInterface.SetState(PanelMeleeUI);
					}
					else if (modPlayer.PlayerClass == 2)
					{
						PanelKillUI = new PanelKill();
						PanelKillUI.Initialize();
						somethingInterface = new UserInterface();
						somethingInterface.SetState(PanelKillUI);
						ExampleResourceBar = new KillBar();
						ExampleResourceBar.Activate();
						KillResourceBarUserInterface = new UserInterface();
						KillResourceBarUserInterface.SetState(ExampleResourceBar);
						DeathResourceBar = new DeathBar();
						DeathResourceBar.Activate();
						DeathResourceBarInterface = new UserInterface();
						DeathResourceBarInterface.SetState(DeathResourceBar);
					}
					else if (modPlayer.PlayerClass == 3)
					{
						panelSummonUI = new PanelSummon();
						panelSummonUI.Initialize();
						somethingInterface = new UserInterface();
						somethingInterface.SetState(panelSummonUI);
					}else if (modPlayer.PlayerClass == 4)
                    {
						//狂战
						melee2UI = new PanelMelee2();
						melee2UI.Initialize();
						melee2Interface = new UserInterface();
						melee2Interface.SetState(melee2UI);
						//怒气条
						angerBar = new AngerBar();
						angerBar.Activate();
						angerBarInterface = new UserInterface();
						angerBarInterface.SetState(angerBar);
					}
					else if (modPlayer.PlayerClass == 5)
					{
						magicUI = new PanelMagic();
						magicUI.Initialize();
						magicInterface = new UserInterface();
						magicInterface.SetState(magicUI);
					}
					else if (modPlayer.PlayerClass == 6)
					{
						magic2UI = new PanelMagic2();
						magic2UI.Initialize();
						magic2Interface = new UserInterface();
						magic2Interface.SetState(magic2UI);
					}
					/*godSoulUI = new PanelGodSoul();
					godSoulUI.Initialize();
					godSoulInterface = new UserInterface();
					godSoulInterface.SetState(godSoulUI);*/

					magicCharge = new MagicCharge();
					_magicChargeUserInterface = new UserInterface();
					_magicChargeUserInterface.SetState(this.magicCharge);
				}
				catch (Exception ex)
				{
					//处理异常
				}
				finally
				{
					//清理
				}
			}
		}

        public override void Unload()
        {
			AutoAttackKey = null;
			ShowUI = null;
			KillSkillKey = null;
			TransKey = null;
			//ExtraAccessaryKey = null;
		}

		public override void PostSetupContent()
		{
			DustIDSlashFX = GetDust("SlashDust").Type;
			for (int i = 0; i < ItemLoader.ItemCount; i++)
			{
				var mitem = ItemLoader.GetItem(i);
				if(mitem != null && mitem.item.magic && !mitem.item.channel)
                {
                    string name = mitem.item.Name;
                }
			}
		}

		public static int getBuffLength()
		{
			int c = 0;
			c = modBuffValues.Count;
			return c;
		}

		public static BuffValue getBuff(int index)
		{
			int c = index;
			
				if (c < modBuffValues.Count)
				{
					return modBuffValues[c];
				}

			throw new Exception("Index out of range " + index);
		}

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
			updateMap();
			foreach (NPC npc2 in Main.npc)
			{
				if (npc2.active && npc2.type != 0)
				{
					Rectangle rectangle;
                    rectangle = new Rectangle((int)npc2.position.X - 15, (int)npc2.position.Y - 15, npc2.width + 30, npc2.height + 30);
					if (rectangle.Contains((int)Main.MouseWorld.X, (int)Main.MouseWorld.Y) && ((!npc2.townNPC && !npc2.friendly) || Main.LocalPlayer.dead))
					{
						string arg = "666666";
						if (npc2.friendly && npc2.dontTakeDamage)
						{
							arg = "449944";
						}
						if (npc2.friendly && !npc2.dontTakeDamage)
						{
							arg = "00FF00";
						}
						if (!npc2.friendly && npc2.dontTakeDamage)
						{
							arg = "994444";
						}
						if (!npc2.friendly && !npc2.dontTakeDamage)
						{
							arg = "FF0000";
						}
						if (!npc2.friendly && npc2.damage == 0 && (npc2.type < 396 || npc2.type > 398))
						{
							arg = "FFFF00";
						}
						string[] array = new string[]
						{
							string.Format("[c/{0}:{1}]", arg, npc2.GivenOrTypeName),
							string.Format("\n【{0}】", (npc2.modNPC == null) ? (GameCulture.Chinese.IsActive ? "原版" : "Vanilla") : npc2.modNPC.mod.Name),
							string.Format("\n{0}:{1}", GameCulture.Chinese.IsActive ? "伤害" : "Damage", npc2.damage.ToString()),
							string.Format("\n{0}:{1}", GameCulture.Chinese.IsActive ? "防御" : "Defense", npc2.defense.ToString()),
							string.Format("\n{0}:{1}/{2}", GameCulture.Chinese.IsActive ? "生命" : "Life", npc2.life.ToString(), npc2.lifeMax.ToString()),
							string.Format("\n{0}:{1}", GameCulture.Chinese.IsActive ? "战力" : "Power", npc2.getPower()),
							string.Format("\n{0}:{1}", GameCulture.Chinese.IsActive ? "评价" : "PowerLevel", npc2.getPowerLevelText())
						};
						string text = string.Concat(array);
						Main.instance.MouseTextHackZoom(text);
						Main.mouseText = true;
						Main.player[Main.myPlayer].showItemIcon = false;
						return;
					}
				}
			}
		}

       

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.Silk, 2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ItemID.Leather);
			recipe.AddRecipe();
			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(893);
			recipe.AddRecipe();
			recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.StoneBlock, 10);
			recipe.needWater = true;
			recipe.needLava = true;
			recipe.SetResult(ItemID.Obsidian, 5);
			recipe.AddRecipe();
			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModLoader.GetMod("SummonHeart").ItemType("GuideNote"), 1);
			recipe.AddIngredient(ModLoader.GetMod("SummonHeart").ItemType("MeleeScroll2"), 1);
			recipe.SetResult(ModLoader.GetMod("SummonHeart").ItemType("DemonSword"));
			recipe.AddRecipe();
			
			/*Mod Calamity = ModLoader.GetMod("CalamityMod");
			if(Calamity != null)
            {
				recipe = new ModRecipe(this);
				recipe.AddRecipeGroup(RecipeGroupID.IronBar, 10);
				recipe.AddIngredient(ModLoader.GetMod("CalamityMod").ItemType("PurifiedGel"), 7);
				recipe.AddTile(TileID.Anvils);
				recipe.SetResult(ModLoader.GetMod("CalamityMod").ItemType("P90"));
				recipe.AddRecipe();
			}*/
		}
		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			byte msgType = reader.ReadByte();
			switch (msgType)
			{
				case 0:
                    {
						// 同步噬魂之心
						byte playernumber = reader.ReadByte();
						SummonHeartPlayer summonHeartPlayer = Main.player[playernumber].GetModPlayer<SummonHeartPlayer>();
						summonHeartPlayer.BBP = reader.ReadInt32();
						summonHeartPlayer.SummonCrit = reader.ReadInt32();
						summonHeartPlayer.exp = reader.ReadInt32();
						summonHeartPlayer.PlayerClass = reader.ReadInt32();
						//summonHeartPlayer.deathCount = reader.ReadInt32();
						summonHeartPlayer.bodyDef = reader.ReadSingle();
						summonHeartPlayer.eyeBloodGas = reader.ReadInt32();
						summonHeartPlayer.handBloodGas = reader.ReadInt32();
						summonHeartPlayer.bodyBloodGas = reader.ReadInt32();
						summonHeartPlayer.footBloodGas = reader.ReadInt32();
						summonHeartPlayer.bloodGasMax = reader.ReadInt32();
						summonHeartPlayer.swordBlood = reader.ReadInt32();
						summonHeartPlayer.shortSwordBlood = reader.ReadInt32();
						summonHeartPlayer.flySwordBlood = reader.ReadInt32();
						summonHeartPlayer.magicSwordBlood = reader.ReadInt32();
						summonHeartPlayer.swordBloodMax = reader.ReadInt32();
						summonHeartPlayer.practiceEye = reader.ReadBoolean();
						summonHeartPlayer.practiceHand = reader.ReadBoolean();
						summonHeartPlayer.practiceBody = reader.ReadBoolean();
						summonHeartPlayer.practiceFoot = reader.ReadBoolean();
						summonHeartPlayer.soulSplit = reader.ReadBoolean();

						/*for (int i = 0; i < getBuffLength(); i++)
						{
							summonHeartPlayer.boughtbuffList[i] = reader.ReadBoolean();
						}*/
						if (Main.netMode == NetmodeID.Server)
						{
							var packet = GetPacket();
							packet.Write((byte)playernumber);
							packet.Write(summonHeartPlayer.BBP);
							packet.Write(summonHeartPlayer.SummonCrit);
							packet.Write(summonHeartPlayer.exp);
							packet.Write(summonHeartPlayer.PlayerClass);
							//packet.Write(summonHeartPlayer.deathCount);
							packet.Write(summonHeartPlayer.bodyDef);
							packet.Write(summonHeartPlayer.eyeBloodGas);
							packet.Write(summonHeartPlayer.handBloodGas);
							packet.Write(summonHeartPlayer.bodyBloodGas);
							packet.Write(summonHeartPlayer.footBloodGas);
							packet.Write(summonHeartPlayer.bloodGasMax);
							packet.Write(summonHeartPlayer.swordBlood);
							packet.Write(summonHeartPlayer.shortSwordBlood);
							packet.Write(summonHeartPlayer.flySwordBlood);
							packet.Write(summonHeartPlayer.magicSwordBlood);
							packet.Write(summonHeartPlayer.swordBloodMax);
							packet.Write(summonHeartPlayer.practiceEye);
							packet.Write(summonHeartPlayer.practiceHand);
							packet.Write(summonHeartPlayer.practiceBody);
							packet.Write(summonHeartPlayer.practiceFoot);
							packet.Write(summonHeartPlayer.soulSplit);
							/*for (int i = 0; i < getBuffLength(); i++)
							{
								packet.Write(summonHeartPlayer.boughtbuffList[i]);
							}*/
							packet.Send(-1, playernumber);
						}
					}
					break;

				case 1:
                    {
						byte npc = reader.ReadByte();
						Main.npc[npc].life = reader.ReadInt32();
					}
                    break;

				case 2: //update SoulSplit
                    {
						/*byte npc = reader.ReadByte();
						*/
						if (Main.netMode == NetmodeID.MultiplayerClient)
						{
							int npc = reader.ReadByte();
							Main.npc[npc].lifeRegen = reader.ReadInt32();
						}
					}
					break;
				case 3:
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
				case 4:
                    {
						if (Main.netMode == NetmodeID.MultiplayerClient)
						{
							byte playernumber = reader.ReadByte();
							int npc = reader.ReadByte();
							Main.player[playernumber].doKillNpcExp(Main.npc[npc]);
						}
					}
					break;

				default:
					Logger.WarnFormat("MyMod: Unknown Message type: {0}", msgType);
					break;
			}
		}


		public override void UpdateUI(GameTime gameTime)
        {
			// it will only draw if the player is not on the main menu
			Player player = Main.player[Main.myPlayer];
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			
			if (modPlayer.PlayerClass == 1)
            {
				if (!Main.gameMenu && PanelMelee.visible)
				{
					somethingInterface?.Update(gameTime);
				}
				else
				{
					if (PanelMeleeUI == null)
					{
						PanelMeleeUI = new PanelMelee();
						PanelMeleeUI.Initialize();
						somethingInterface = new UserInterface();
						somethingInterface.SetState(PanelMeleeUI);
					}
					PanelMeleeUI.needValidate = true;
				}
            }else if (modPlayer.PlayerClass == 2)
			{
				if (!Main.gameMenu && PanelKill.visible)
				{
					somethingInterface?.Update(gameTime);
				}
				else
				{
                    if (PanelKillUI == null)
                    {
						PanelKillUI = new PanelKill();
						PanelKillUI.Initialize();
						somethingInterface = new UserInterface();
						somethingInterface.SetState(PanelKillUI);
					}
					PanelKillUI.needValidate = true;
				}
					
				if (ExampleResourceBar == null)
				{
					ExampleResourceBar = new KillBar();
					ExampleResourceBar.Activate();
					KillResourceBarUserInterface = new UserInterface();
					KillResourceBarUserInterface.SetState(ExampleResourceBar);
					DeathResourceBar = new DeathBar();
					DeathResourceBar.Activate();
					DeathResourceBarInterface = new UserInterface();
					DeathResourceBarInterface.SetState(DeathResourceBar);
				}
                else
                {
					KillResourceBarUserInterface?.Update(gameTime);
					DeathResourceBarInterface?.Update(gameTime);
				}
			}else if (modPlayer.PlayerClass == 3)
			{
				if (!Main.gameMenu && PanelSummon.visible)
				{
					somethingInterface?.Update(gameTime);
				}
				else
				{
					if (panelSummonUI == null)
					{
						panelSummonUI = new PanelSummon();
						panelSummonUI.Initialize();
						somethingInterface = new UserInterface();
						somethingInterface.SetState(panelSummonUI);
					}
					panelSummonUI.needValidate = true;
				}
			}
			else if (modPlayer.PlayerClass == 4)
			{
				if (!Main.gameMenu && PanelMelee2.visible)
				{
					melee2Interface?.Update(gameTime);
				}
				else
				{
					if (melee2UI == null)
					{
						melee2UI = new PanelMelee2();
						melee2UI.Initialize();
						melee2Interface = new UserInterface();
						melee2Interface.SetState(melee2UI);
					}
					melee2UI.needValidate = true;
				}

				if (angerBar == null)
				{
					angerBar = new AngerBar();
					angerBar.Activate();
					angerBarInterface = new UserInterface();
					angerBarInterface.SetState(angerBar);
				}
				else
				{
					angerBarInterface?.Update(gameTime);
				}
			}
			else if (modPlayer.PlayerClass == 5)
			{
				if (!Main.gameMenu && PanelMagic.visible)
				{
					magicInterface?.Update(gameTime);
				}
				else
				{
					if (magicUI == null)
					{
						magicUI = new PanelMagic();
						magicUI.Initialize();
						magicInterface = new UserInterface();
						magicInterface.SetState(magicUI);
					}
					magicUI.needValidate = true;
				}
			}
			else if (modPlayer.PlayerClass == 6)
			{
				if (!Main.gameMenu && PanelMagic2.visible)
				{
					magic2Interface?.Update(gameTime);
				}
				else
				{
					if (magic2UI == null)
					{
						magic2UI = new PanelMagic2();
						magic2UI.Initialize();
						magic2Interface = new UserInterface();
						magic2Interface.SetState(magic2UI);
					}
					magic2UI.needValidate = true;
				}
			}

			/*if (!Main.gameMenu && PanelGodSoul.visible)
			{
				godSoulInterface?.Update(gameTime);
			}
			else
			{
				if (godSoulUI == null)
				{
					godSoulUI = new PanelGodSoul();
					godSoulUI.Initialize();
					godSoulInterface = new UserInterface();
					godSoulInterface.SetState(godSoulUI);
				}
				godSoulUI.needValidate = true;
			}*/

			if (!Main.gameMenu && magicCharge != null)
			{
				_magicChargeUserInterface?.Update(gameTime);
			}
			else
			{
				if (magicCharge == null)
				{
					magicCharge = new MagicCharge();
					_magicChargeUserInterface = new UserInterface();
					_magicChargeUserInterface.SetState(this.magicCharge);
				}
			}
		}

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1)
			{
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("SummonHeart", DrawSomethingUI, InterfaceScaleType.UI));
			}

			int MouseTextIndex = layers.FindIndex((GameInterfaceLayer layer) => layer.Name.Equals("Vanilla: Inventory"));
			if (MouseTextIndex != -1)
			{
				layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer("SummonHeart: Extra Accessories", delegate ()
				{
					((AllItemsMenu)base.GetGlobalItem("AllItemsMenu")).DrawUpdateExtraAccessories(Main.spriteBatch);
					return true;
				}, (InterfaceScaleType)1));
			}
		}

		private bool DrawSomethingUI()
		{
			// it will only draw if the player is not on the main menu
			Player player = Main.player[Main.myPlayer];
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

			if (modPlayer.PlayerClass == 1)
            {
				if (!Main.gameMenu && PanelMelee.visible)
				{
					somethingInterface.Draw(Main.spriteBatch, new GameTime());
				}
            }else if (modPlayer.PlayerClass == 2)
			{
				if (!Main.gameMenu && PanelKill.visible)
				{
					somethingInterface.Draw(Main.spriteBatch, new GameTime());
				}
				if(KillResourceBarUserInterface != null)
					KillResourceBarUserInterface.Draw(Main.spriteBatch, new GameTime());
				if(DeathResourceBarInterface != null)
					DeathResourceBarInterface.Draw(Main.spriteBatch, new GameTime());
			}else if (modPlayer.PlayerClass == 3)
			{
				if (!Main.gameMenu && PanelSummon.visible)
				{
					somethingInterface.Draw(Main.spriteBatch, new GameTime());
				}
			}
			else if (modPlayer.PlayerClass == 4)
			{
				if (!Main.gameMenu && PanelMelee2.visible)
				{
					melee2Interface.Draw(Main.spriteBatch, new GameTime());
				}
				if (angerBar != null)
					angerBarInterface.Draw(Main.spriteBatch, new GameTime());
			}
			else if (modPlayer.PlayerClass == 5)
			{
				if (!Main.gameMenu && PanelMagic.visible)
				{
					magicInterface.Draw(Main.spriteBatch, new GameTime());
				}
			}
			else if (modPlayer.PlayerClass == 6)
			{
				if (!Main.gameMenu && PanelMagic2.visible)
				{
					magic2Interface.Draw(Main.spriteBatch, new GameTime());
				}
			}

			/*if (!Main.gameMenu && PanelGodSoul.visible)
			{
				godSoulInterface.Draw(Main.spriteBatch, new GameTime());
			}*/
			if (!Main.gameMenu && magicCharge != null)
			{
				_magicChargeUserInterface.Draw(Main.spriteBatch, new GameTime());
			}
			return true;
		}

    }
}