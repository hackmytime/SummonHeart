using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using SummonHeart.XiuXianModule.Entities;
using SummonHeart.Buffs.XiuXian.DanYao;

namespace SummonHeart.XiuXianModule.Items.Danyao.HuiFu
{
    public class BuTianDan : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("补天丹");
            Tooltip.SetDefault("七品丹药" +
                "\n每秒回复生命1W点" +
                "\n持续1分钟");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = 7;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.maxStack = 99;
            item.useAnimation = 20;
            item.useTime = 20;
            item.useStyle = 2;
            item.UseSound = SoundID.Item3;
            item.consumable = true;
        }

        public override bool UseItem(Player player)
        {
            RPGPlayer mp = player.GetModPlayer<RPGPlayer>();
            player.AddBuff(ModContent.BuffType<BuTianBuff>(), 3600 * 1);
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("LingShi4"), 9);
            recipe.AddIngredient(mod.GetItem("NeiDan7"), 1);
            int gem = 7;
            recipe.AddIngredient(ItemID.Amber, gem);
            recipe.AddIngredient(ItemID.Amethyst, gem);
            recipe.AddIngredient(ItemID.Topaz, gem);
            recipe.AddIngredient(ItemID.Sapphire, gem);
            recipe.AddIngredient(ItemID.Ruby, gem);
            recipe.AddIngredient(ItemID.Emerald, gem);
            recipe.AddIngredient(ItemID.Diamond, gem);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
