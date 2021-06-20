using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items
{
    public class SummonHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("SoulEatingHeart");
			Tooltip.SetDefault("The heart of Terra ancient demons\nCan obtain the power of the devil By swallowing the soul of creatures");
			DisplayName.AddTranslation(GameCulture.Chinese, "噬魂之心");
            Tooltip.AddTranslation(GameCulture.Chinese, "泰拉远古魔神的心脏\n噬魂之心，吞噬万灵，铸我魔魂!");

			ModTranslation text = mod.CreateTranslation("Pip-Boy3000text1");
			text.SetDefault("Swallowed souls ");
			text.AddTranslation(GameCulture.Chinese, "已吞噬灵魂");
			mod.AddTranslation(text);

			text = mod.CreateTranslation("Pip-Boy3000text2");
			text.SetDefault("Life draining add ");
			text.AddTranslation(GameCulture.Chinese, "生命偷取增加");
			mod.AddTranslation(text);

			text = mod.CreateTranslation("Pip-Boy3000text3");
			text.SetDefault("All Weapon speed add ");
			text.AddTranslation(GameCulture.Chinese, "召唤栏位增加");
			mod.AddTranslation(text);

			text = mod.CreateTranslation("Pip-Boy3000text4");
			text.SetDefault("Ignore enemy armor rate ");
			text.AddTranslation(GameCulture.Chinese, "无视敌人护甲");
			mod.AddTranslation(text);

			text = mod.CreateTranslation("Pip-Boy3000text5");
			text.SetDefault("Armor penetration add ");
			text.AddTranslation(GameCulture.Chinese, "护甲穿透增加");
			mod.AddTranslation(text);

			text = mod.CreateTranslation("Pip-Boy3000text6");
			text.SetDefault("Level up need ");
			text.AddTranslation(GameCulture.Chinese, "升级所需灵魂");
			mod.AddTranslation(text);

			text = mod.CreateTranslation("Pip-Boy3000text7");
			text.SetDefault("Current Lv ");
			text.AddTranslation(GameCulture.Chinese, "当前等级Lv");
			mod.AddTranslation(text);
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {

			Player player = Main.player[Main.myPlayer];
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			string text1 = Language.GetTextValue("Mods.SummonHeart.Pip-Boy3000text1") + modPlayer.BBP;
			string text2 = Language.GetTextValue("Mods.SummonHeart.Pip-Boy3000text2") + modPlayer.SummonCrit / 50 + "%";
			string text3 = Language.GetTextValue("Mods.SummonHeart.Pip-Boy3000text3") + "5倍";
			string text4 = Language.GetTextValue("Mods.SummonHeart.Pip-Boy3000text4") + modPlayer.SummonCrit / 5 + "%";
			string text5 = Language.GetTextValue("Mods.SummonHeart.Pip-Boy3000text5") + modPlayer.SummonCrit	/ 5;
			string text6 = Language.GetTextValue("Mods.SummonHeart.Pip-Boy3000text6") + modPlayer.exp;
			string text7 = Language.GetTextValue("Mods.SummonHeart.Pip-Boy3000text7") + modPlayer.SummonCrit;
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
			//modPlayer.AttackSpeed += modPlayer.SummonCrit / 10 * 0.01f;
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
