using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using SummonHeart.XiuXianModule.Entities;
using SummonHeart.Buffs.XiuXian.DanYao;

namespace SummonHeart.XiuXianModule.Items.Danyao.XiuLian
{
    public class JuQiDan : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("聚气丹");
            Tooltip.SetDefault("一品丹药" +
                "\n可用于所有修士加快灵力吸收" +
                "\n服用后灵力吸收速度加快50%" +
                "\n持续1分钟");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = 1;
            item.value = Item.sellPrice(1, 0, 0, 0);
            item.maxStack = 99;
            item.useAnimation = 20;
            item.useTime = 20;
            item.useStyle = 2;
            item.UseSound = SoundID.Item3;
            item.consumable = true;
        }

        public override bool UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<XisuiBuff>(), 3600 * 1);
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("LingShi1"), 9);
            recipe.AddIngredient(mod.GetItem("NeiDan1"), 1);
            int gem = 1;
            recipe.AddIngredient(ItemID.Amber, gem);
            recipe.AddIngredient(ItemID.Amethyst, gem);
            recipe.AddIngredient(ItemID.Topaz, gem);
            recipe.AddIngredient(ItemID.Sapphire, gem);
            recipe.AddIngredient(ItemID.Ruby, gem);
            recipe.AddIngredient(ItemID.Emerald, gem);
            recipe.AddIngredient(ItemID.Diamond, gem);
            recipe.AddIngredient(ItemID.SilverBar, gem);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
