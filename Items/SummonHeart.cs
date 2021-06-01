using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Items
{
    public class SummonHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("噬魂之心");
            Tooltip.SetDefault("泰拉远古魔神的心脏\n通过吞噬生物灵魂，获取魔神之力");
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {

			Player player = Main.player[Main.myPlayer];
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			string text1 = "已吞噬灵魂" + modPlayer.BBP;
			string text2 = "生命偷取增加" + modPlayer.SummonCrit / 50 + "%";
			string text3 = "攻击速度增加" + modPlayer.SummonCrit / 10 + "%";
			string text4 = "无视敌人护甲" + modPlayer.SummonCrit / 5 + "%";
			string text5 = "护甲穿透增加" + modPlayer.SummonCrit	/ 5;
			string text6 = "升级所需灵魂" + modPlayer.exp;
			string text7 = "当前等级Lv" + modPlayer.SummonCrit;
			TooltipLine line = new TooltipLine(mod, "text1", text1);
			TooltipLine line2 = new TooltipLine(mod, "text2", text2);
			TooltipLine line3 = new TooltipLine(mod, "text3", text3);
			TooltipLine line4 = new TooltipLine(mod, "text4", text4);
			TooltipLine line5 = new TooltipLine(mod, "text5", text5);
			TooltipLine line6 = new TooltipLine(mod, "text6", text6);
			TooltipLine line7 = new TooltipLine(mod, "text7", text7);
			line.overrideColor = Color.Red;
			line2.overrideColor = Color.LimeGreen;
			line3.overrideColor = Color.Brown;
			line4.overrideColor = Color.SkyBlue;
			line5.overrideColor = Color.Orange;
			line6.overrideColor = Color.Magenta;
			line7.overrideColor = Color.Red;
			
			tooltips.Insert(2, line);
			tooltips.Insert(3, line2);
			tooltips.Insert(4, line3);
			tooltips.Insert(5, line4);
			tooltips.Insert(6, line5);
			tooltips.Insert(7, line6);
			tooltips.Insert(8, line7);
			
		}

		public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.rare = 9;
            item.value = Item.sellPrice(0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			modPlayer.SummonHeart = true;
			player.maxMinions *= 5;
			modPlayer.AttackSpeed += modPlayer.SummonCrit / 10 * 0.01f;
			player.armorPenetration += modPlayer.SummonCrit / 5;
			/*player.magicCrit += modPlayer.SummonCrit / 10;
			player.meleeCrit += modPlayer.SummonCrit / 10;
			player.rangedCrit += modPlayer.SummonCrit / 10;
			player.thrownCrit += modPlayer.SummonCrit / 10;*/
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(29, 1);
			recipe.AddIngredient(23, 9);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
