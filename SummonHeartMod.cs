using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SummonHeart.body;
using SummonHeart.costvalues;
using SummonHeart.ui;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
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

		// Hotkeys
		internal static ModHotKey AutoAttackKey;
		internal static ModHotKey ShowUI;

		internal Panel somethingUI;
		public UserInterface somethingInterface;
		public UserInterface tooltipInterface;
		public SummonHeartMod Instance;

		public SummonHeartMod()
		{
			modBuffValues = new List<BuffValue>();
			modBuffValues = VanilaBuffs.getVanilla();
			Instance = this;
		}

		public override void Load()
        {
			AutoAttackKey = RegisterHotKey("自动使用武器（再次点击停止使用）", "G");
			// this makes sure that the UI doesn't get opened on the server
			// the server can't see UI, can it? it's just a command prompt
			if (!Main.dedServ)
			{
				somethingUI = new Panel();
				somethingUI.Initialize();
				somethingInterface = new UserInterface();
				somethingInterface.SetState(somethingUI);
			}

			ShowUI = RegisterHotKey("魔神炼体法", Keys.L.ToString());
			
		}

        public override void Unload()
        {
			AutoAttackKey = null;
			ShowUI = null;
		}

      /* public override void PostSetupContent()
        {
			
			for (int i = 0; i < ItemLoader.ItemCount; i++)
			{
				ModItem mitem = ItemLoader.GetItem(i);
				
				if (mitem != null)
				{
					if (mitem.item.Name.Contains("Bar") && !mitem.item.accessory && mitem.mod.Name != "ThoriumMod")
					{
						var item = mitem.item;
						
						var bvalue = new BuffValue(item.type, item.defense, "防御+" + item.defense, item.Name);
                        if (!modBuffValues.Contains(bvalue))
                        {
							modBuffValues.Add(bvalue);
                        }
					}
				}
			}
		}*/

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
			// 同步噬魂之心
			byte playernumber = reader.ReadByte();
			SummonHeartPlayer summonHeartPlayer = Main.player[playernumber].GetModPlayer<SummonHeartPlayer>();
			summonHeartPlayer.BBP = reader.ReadInt32();
			summonHeartPlayer.SummonCrit = reader.ReadInt32();
			summonHeartPlayer.exp = reader.ReadInt32();
			summonHeartPlayer.bodyDef = reader.ReadSingle();
			summonHeartPlayer.eyeBloodGas = reader.ReadInt32();
			summonHeartPlayer.handBloodGas = reader.ReadInt32();
			summonHeartPlayer.bodyBloodGas = reader.ReadInt32();
			summonHeartPlayer.footBloodGas = reader.ReadInt32();
			summonHeartPlayer.practiceEye = reader.ReadBoolean();
			summonHeartPlayer.practiceHand = reader.ReadBoolean();
			summonHeartPlayer.practiceBody = reader.ReadBoolean();
			summonHeartPlayer.practiceFoot = reader.ReadBoolean();
			summonHeartPlayer.soulSplit = reader.ReadBoolean();

			for (int i = 0; i < getBuffLength(); i++)
			{
				summonHeartPlayer.boughtbuffList[i] = reader.ReadBoolean();
			}
			if (Main.netMode == NetmodeID.Server)
			{
				var packet = GetPacket();
				packet.Write((byte)playernumber);
				packet.Write(summonHeartPlayer.BBP);
				packet.Write(summonHeartPlayer.SummonCrit);
				packet.Write(summonHeartPlayer.exp);
				packet.Write(summonHeartPlayer.bodyDef);
				packet.Write(summonHeartPlayer.eyeBloodGas);
				packet.Write(summonHeartPlayer.handBloodGas);
				packet.Write(summonHeartPlayer.bodyBloodGas);
				packet.Write(summonHeartPlayer.footBloodGas);
				packet.Write(summonHeartPlayer.practiceEye);
				packet.Write(summonHeartPlayer.practiceHand);
				packet.Write(summonHeartPlayer.practiceBody);
				packet.Write(summonHeartPlayer.practiceFoot);
				packet.Write(summonHeartPlayer.soulSplit);
				for (int i = 0; i < getBuffLength(); i++)
				{
					packet.Write(summonHeartPlayer.boughtbuffList[i]);
				}
				packet.Send(-1, playernumber);
			}
		}


		public override void UpdateUI(GameTime gameTime)
        {
			// it will only draw if the player is not on the main menu
			if (!Main.gameMenu && Panel.visible)
			{
				somethingInterface?.Update(gameTime);
			}
			else
			{
				somethingUI.needValidate = true;
			}
		}

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1)
			{
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("SummonHeart", DrawSomethingUI, InterfaceScaleType.UI));
			}
		}

		private bool DrawSomethingUI()
		{
			// it will only draw if the player is not on the main menu
			if (!Main.gameMenu
				&& Panel.visible)
			{
				somethingInterface.Draw(Main.spriteBatch, new GameTime());
			}
			return true;
		}

    }
}