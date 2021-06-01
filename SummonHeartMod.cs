using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart
{
	public class SummonHeartMod : Mod
	{
		// Hotkeys
		internal static ModHotKey AutoAttackKey;

		public override void Load()
        {
			AutoAttackKey = RegisterHotKey("自动使用武器（再次点击停止使用）", "G");
        }

        public override void Unload()
        {
			AutoAttackKey = null;
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
			byte playernumber = reader.ReadByte();
			SummonHeartPlayer summonHeartPlayer = Main.player[playernumber].GetModPlayer<SummonHeartPlayer>();
			summonHeartPlayer = Main.player[playernumber].GetModPlayer<SummonHeartPlayer>();

			summonHeartPlayer.BBP = reader.ReadInt32();
			summonHeartPlayer.SummonCrit = reader.ReadInt32();
			summonHeartPlayer.exp = reader.ReadInt32();

			if (Main.netMode == NetmodeID.Server)
			{
				var packet = GetPacket();
				packet.Write(playernumber);
				packet.Write(summonHeartPlayer.BBP);
				packet.Write(summonHeartPlayer.SummonCrit);
				packet.Write(summonHeartPlayer.exp);
				packet.Send(-1, playernumber);
			}
		}
	}
}