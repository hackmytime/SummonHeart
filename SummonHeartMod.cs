using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SummonHeart.body;
using SummonHeart.Extensions;
using SummonHeart.Extensions.TurretSystem;
using SummonHeart.Items;
using SummonHeart.Items.Range.Tools;
using SummonHeart.ui;
using SummonHeart.ui.Bar;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Events;
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

		private ModUIHandler UIhandler = new ModUIHandler();

		internal static List<BuffValue> modBuffValues = new List<BuffValue>();

		public static HashSet<int> posionBuffSet = new HashSet<int>();
		public static HashSet<int> whiteBuffSet = new HashSet<int>();
		public static List<String> rejPassItemList = new List<String>();

		static Dictionary<Projectile, Vector2> oldProMap = new Dictionary<Projectile, Vector2>();
		public static Dictionary<Item, LegacySoundStyle> itemSoundMap = new Dictionary<Item, LegacySoundStyle>();

		public static int DustIDSlashFX;

		// Hotkeys
		internal static ModHotKey AutoAttackKey;
		internal static ModHotKey ShowUI;
		internal static ModHotKey KillSkillKey;
		internal static ModHotKey magicSkillKey;
		internal static ModHotKey TransKey;
		internal static ModHotKey BackDieKey;
		internal static ModHotKey BackHomeKey;
		internal static ModHotKey BackHomeBackKey;
		internal static ModHotKey DoubleDamageKey;
		internal static ModHotKey BuffKey;
		internal static ModHotKey TriggerExplosion;

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
		
		internal PowerArmor powerArmor;
		private UserInterface _powerArmorInterface;

		internal BowCharge bowCharge;
		private UserInterface bowChargeInterface;

		internal KillBar ExampleResourceBar;
		private UserInterface KillResourceBarUserInterface;

		internal DeathBar DeathResourceBar;
		private UserInterface DeathResourceBarInterface;
		
		internal AngerBar angerBar;
		private UserInterface angerBarInterface;
		
		internal DamageBar damageBar;
		private UserInterface damageBarInterface;
		
		internal PanelBuff panelBuff;
		private UserInterface panelBuffInterface;

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
			whiteBuffSet.Add(BuffID.ShadowDodge);
			whiteBuffSet.Add(BuffID.ParryDamageBuff);
			whiteBuffSet.Add(BuffID.MinecartLeft);
			whiteBuffSet.Add(BuffID.MinecartRight);
			whiteBuffSet.Add(BuffID.MinecartLeftWood);
			whiteBuffSet.Add(BuffID.MinecartRightWood);
			whiteBuffSet.Add(BuffID.MinecartLeftMech);
			whiteBuffSet.Add(BuffID.MinecartRightMech);
			whiteBuffSet.Add(BuffID.BasiliskMount);
			whiteBuffSet.Add(BuffID.BeeMount);
			whiteBuffSet.Add(BuffID.BunnyMount);
			whiteBuffSet.Add(BuffID.CuteFishronMount);
			whiteBuffSet.Add(BuffID.DrillMount);
			whiteBuffSet.Add(BuffID.PigronMount);
			whiteBuffSet.Add(BuffID.ScutlixMount);
			whiteBuffSet.Add(BuffID.SlimeMount);
			whiteBuffSet.Add(BuffID.TurtleMount);
			whiteBuffSet.Add(BuffID.UFOMount);
			whiteBuffSet.Add(BuffID.UnicornMount);
		}

		/*public override uint ExtraPlayerBuffSlots
		{
			get
			{
				return 200U;
			}
		}*/

		private void initposionBuffSet()
        {
			for (int i = 0; i < ItemLoader.ItemCount; i++)
			{
				var mitem = ItemLoader.GetItem(i);
				if (mitem != null)
				{
					var item = mitem.item;
					bool flag = item.buffType != 0 && !item.summon;
					if (flag)
						posionBuffSet.Add(mitem.item.buffType);
				}
			}
		}
		
		public override void Load()
        {
			AutoAttackKey = RegisterHotKey("自动使用武器（再次点击停止使用）", "G");
			ShowUI = RegisterHotKey("魔神炼体法", Keys.L.ToString());
			KillSkillKey = RegisterHotKey("刺客刺杀技能(可开关)", Keys.V.ToString());
			magicSkillKey = RegisterHotKey("控法者充能(可开关)", Keys.B.ToString());
			TransKey = RegisterHotKey("空间传送", Keys.Y.ToString());
			BackDieKey = RegisterHotKey("神秘水晶返回死亡点", Keys.Z.ToString());
			BackHomeKey = RegisterHotKey("神秘水晶快速回城", Keys.B.ToString());
			BackHomeBackKey = RegisterHotKey("神秘水晶返回快速回城点", Keys.C.ToString());
			DoubleDamageKey = RegisterHotKey("泰坦双倍偿还技能", Keys.K.ToString());
			BuffKey = RegisterHotKey("无限法则菜单", Keys.M.ToString());
			TriggerExplosion = RegisterHotKey("引爆工程炸弹", "Mouse2");
			// this makes sure that the UI doesn't get opened on the server
			// the server can't see UI, can it? it's just a command prompt
			if (Main.netMode != 2)
			{
				GFX.GFX.LoadGfx();
				this.UIhandler.Load();
			}
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
						//吸收伤害量
						damageBar = new DamageBar();
						damageBarInterface = new UserInterface();
						damageBarInterface.SetState(damageBar);
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
					
					powerArmor = new PowerArmor();
					_powerArmorInterface = new UserInterface();
					_powerArmorInterface.SetState(this.powerArmor);

					bowCharge = new BowCharge();
					bowChargeInterface = new UserInterface();
					bowChargeInterface.SetState(this.bowCharge);

					panelBuff = new PanelBuff();
					panelBuffInterface = new UserInterface();
					panelBuffInterface.SetState(panelBuff);
					
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

        public override void PostSetupContent()
        {
			initposionBuffSet();
		}

        public override void Unload()
        {
			this.UIhandler.Unload();
			this.UIhandler = null;
			AutoAttackKey = null;
			ShowUI = null;
			KillSkillKey = null;
			magicSkillKey = null;
			TransKey = null;
			BackDieKey = null;
			BackHomeKey = null;
			BackHomeBackKey = null;
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
			recipe = new ModRecipe(this);recipe.AddIngredient(ItemID.SandBlock, 200);recipe.AddTile(TileID.Furnaces);recipe.SetResult(ItemID.Glass, 100);recipe.AddRecipe();
			recipe = new ModRecipe(this);recipe.AddIngredient(ItemID.Sandstone, 200);recipe.AddTile(TileID.Furnaces);recipe.SetResult(ItemID.Glass, 100);recipe.AddRecipe();
			recipe = new ModRecipe(this);recipe.AddIngredient(ItemID.HardenedSand, 200);recipe.AddTile(TileID.Furnaces);recipe.SetResult(ItemID.Glass, 100);recipe.AddRecipe();
			recipe = new ModRecipe(this);recipe.AddIngredient(ItemID.MudBlock, 100);recipe.AddTile(TileID.Furnaces);recipe.SetResult(ItemID.DirtBlock, 100);recipe.AddRecipe();
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
			recipe = new ModRecipe(this);
			recipe.AddIngredient(ModLoader.GetMod("SummonHeart").ItemType("GuideNote"), 1);
			recipe.AddIngredient(ModLoader.GetMod("SummonHeart").ItemType("MagicScroll2"), 1);
			recipe.SetResult(ModLoader.GetMod("SummonHeart").ItemType("DemonStaff"));
			recipe.AddRecipe();
			recipe = new ModRecipe(this); recipe.AddIngredient(ItemID.CopperOre, 300); recipe.SetResult(ItemID.CopperBar, 100); recipe.AddRecipe();
			recipe = new ModRecipe(this); recipe.AddIngredient(ItemID.TinOre, 300); recipe.SetResult(ItemID.TinBar, 100); recipe.AddRecipe();
			recipe = new ModRecipe(this); recipe.AddIngredient(ItemID.IronOre, 300); recipe.SetResult(ItemID.IronBar, 100); recipe.AddRecipe();
			recipe = new ModRecipe(this); recipe.AddIngredient(ItemID.LeadOre, 300); recipe.SetResult(ItemID.LeadBar, 100); recipe.AddRecipe();
			recipe = new ModRecipe(this); recipe.AddIngredient(ItemID.SilverOre, 300); recipe.SetResult(ItemID.SilverBar, 100); recipe.AddRecipe();
			recipe = new ModRecipe(this); recipe.AddIngredient(ItemID.TungstenOre, 300); recipe.SetResult(ItemID.TungstenBar, 100); recipe.AddRecipe();
			recipe = new ModRecipe(this); recipe.AddIngredient(ItemID.GoldOre, 400); recipe.SetResult(ItemID.GoldBar, 100); recipe.AddRecipe();
			recipe = new ModRecipe(this); recipe.AddIngredient(ItemID.PlatinumOre, 400); recipe.SetResult(ItemID.PlatinumBar, 100); recipe.AddRecipe();
			recipe = new ModRecipe(this); recipe.AddIngredient(ItemID.Meteorite, 300); recipe.SetResult(ItemID.MeteoriteBar, 100); recipe.AddRecipe();
			recipe = new ModRecipe(this); recipe.AddIngredient(ItemID.DemoniteOre, 300); recipe.SetResult(ItemID.DemoniteBar, 100); recipe.AddRecipe();
			recipe = new ModRecipe(this); recipe.AddIngredient(ItemID.CrimtaneOre, 300); recipe.SetResult(ItemID.CrimtaneBar, 100); recipe.AddRecipe();
			recipe = new ModRecipe(this); recipe.AddIngredient(ItemID.Hellstone, 300); recipe.AddIngredient(ItemID.Obsidian, 100); recipe.SetResult(ItemID.HellstoneBar, 100); recipe.AddRecipe();
			recipe = new ModRecipe(this); recipe.AddIngredient(ItemID.CobaltOre, 300); recipe.SetResult(ItemID.CobaltBar, 100); recipe.AddRecipe();
			recipe = new ModRecipe(this); recipe.AddIngredient(ItemID.PalladiumOre, 300); recipe.SetResult(ItemID.PalladiumBar, 100); recipe.AddRecipe();
			recipe = new ModRecipe(this); recipe.AddIngredient(ItemID.MythrilOre, 400); recipe.SetResult(ItemID.MythrilBar, 100); recipe.AddRecipe();
			recipe = new ModRecipe(this); recipe.AddIngredient(ItemID.OrichalcumOre, 400); recipe.SetResult(ItemID.OrichalcumBar, 100); recipe.AddRecipe();
			recipe = new ModRecipe(this); recipe.AddIngredient(ItemID.AdamantiteOre, 400); recipe.SetResult(ItemID.AdamantiteBar, 100); recipe.AddRecipe();
			recipe = new ModRecipe(this); recipe.AddIngredient(ItemID.TitaniumOre, 400); recipe.SetResult(ItemID.TitaniumBar, 100); recipe.AddRecipe();
			recipe = new ModRecipe(this); recipe.AddIngredient(ItemID.ChlorophyteOre, 500); recipe.SetResult(ItemID.ChlorophyteBar, 100); recipe.AddRecipe();
			recipe = new ModRecipe(this); recipe.AddIngredient(ItemID.LunarOre, 400); recipe.SetResult(ItemID.LunarBar, 100); recipe.AddRecipe();
		}
		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			byte msgType = reader.ReadByte();
			switch (msgType)
			{
				case 0:
					{
						/*int playernumber = (int)reader.ReadByte();
						int tileX = reader.ReadInt32();
						int tileY = reader.ReadInt32();
						int houseType = reader.ReadInt32();
						Builder.BuildHouse(tileX, tileY, houseType, false);
						if (Main.netMode == 2)
						{
							ModPacket packet = base.GetPacket(256);
							packet.Write(0);
							packet.Write((byte)playernumber);
							packet.Write(tileX);
							packet.Write(tileY);
							packet.Write(houseType);
							packet.Send(-1, playernumber);
						}*/
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
				case 5:
					{
						Main.AnglerQuestSwap();
					}
					break;
				case 6:
					{
						Main.time = 54000.0;
						CultistRitual.delay = 0;
						CultistRitual.recheck = 0;
					}
					break;
				case 7:
					{
						SummonHeartWorld.StarMulti = 100;
						SummonHeartWorld.StarMultiTime = 60 * 60 * 12;
					}
					break;
				case 8:
					AutoHouseTool.HandleBuilding2(reader.ReadInt32(), reader.ReadInt32(), whoAmI);
					break;
				case 9:
					{
						NPC.SpawnOnPlayer(reader.ReadInt32(), reader.ReadInt16());
						NetMessage.SendData(61, -1, -1, null, reader.ReadInt16(), NPCID.TravellingMerchant, 0f, 0f, 0, 0, 0);
					}
					break;
				case 10:
					{
						if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
							byte playernumber = reader.ReadByte();
							SummonHeartPlayer modPlayer = Main.player[playernumber].GetModPlayer<SummonHeartPlayer>();
							modPlayer.detonate = true;
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
			this.UIhandler.UpdateUI(gameTime);
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

				//吸收伤害量
				if (damageBar == null)
				{
					damageBar = new DamageBar();
					damageBarInterface = new UserInterface();
					damageBarInterface.SetState(damageBar);
				}
				else
				{
					damageBarInterface?.Update(gameTime);
				}
			}
			else if (modPlayer.PlayerClass == 2)
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
			
			if (!Main.gameMenu && powerArmor != null)
			{
				_powerArmorInterface?.Update(gameTime);
			}
			else
			{
				if (powerArmor == null)
				{
					powerArmor = new PowerArmor();
					_powerArmorInterface = new UserInterface();
					_powerArmorInterface.SetState(this.powerArmor);
				}
			}
			
			if (!Main.gameMenu && bowCharge != null)
			{
				bowChargeInterface?.Update(gameTime);
			}
			else
			{
				if (bowCharge == null)
				{
					bowCharge = new BowCharge();
					bowChargeInterface = new UserInterface();
					bowChargeInterface.SetState(this.bowCharge);
				}
			}
			
			if (!Main.gameMenu && panelBuff != null && PanelBuff.visible)
			{
				panelBuffInterface?.Update(gameTime);
			}
			else
			{
				if (panelBuff == null)
				{
					panelBuff = new PanelBuff();
					panelBuffInterface = new UserInterface();
					panelBuffInterface.SetState(panelBuff);
				}
				panelBuff.needValidate = true;
			}
		}

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
			TurretPlayer.ModifyInterfaceLayers(layers);
			this.UIhandler.ModifyInterfaceLayers(layers);
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
				if (damageBar != null)
					damageBarInterface.Draw(Main.spriteBatch, new GameTime());
			}
			else if (modPlayer.PlayerClass == 2)
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
			if (!Main.gameMenu && powerArmor != null)
			{
				_powerArmorInterface.Draw(Main.spriteBatch, new GameTime());
			}
			if (!Main.gameMenu && bowCharge != null)
			{
				bowChargeInterface.Draw(Main.spriteBatch, new GameTime());
			}
			if (!Main.gameMenu && PanelBuff.visible)
			{
				panelBuffInterface.Draw(Main.spriteBatch, new GameTime());
			}
			return true;
		}

		internal static bool ClearEvents()
		{
			bool eventOccurring = false;
			bool canClearEvent = true;
			if (Main.invasionType != 0)
			{
				eventOccurring = true;
				if (canClearEvent)
				{
					Main.invasionType = 0;
				}
			}

			if (Main.pumpkinMoon)
			{
				eventOccurring = true;
				if (canClearEvent)
				{
					Main.pumpkinMoon = false;
				}
			}

			if (Main.snowMoon)
			{
				eventOccurring = true;
				if (canClearEvent)
				{
					Main.snowMoon = false;
				}
			}

			if (Main.eclipse)
			{
				eventOccurring = true;
				if (canClearEvent)
				{
					Main.eclipse = false;
				}
			}

			if (Main.bloodMoon)
			{
				eventOccurring = true;
				if (canClearEvent)
				{
					Main.bloodMoon = false;
				}
			}

			if (Main.raining)
			{
				eventOccurring = true;
				if (canClearEvent)
				{
					Main.raining = false;
				}
			}

			if (Main.slimeRain)
			{
				eventOccurring = true;
				if (canClearEvent)
				{
					Main.StopSlimeRain();
					Main.slimeWarningDelay = 1;
					Main.slimeWarningTime = 1;
				}
			}
			return eventOccurring && canClearEvent;
		}
	}
}