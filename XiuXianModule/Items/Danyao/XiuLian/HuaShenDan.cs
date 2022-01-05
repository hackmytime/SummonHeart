using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using SummonHeart.XiuXianModule.Entities;
using SummonHeart.Buffs.XiuXian.DanYao;

namespace SummonHeart.XiuXianModule.Items.Danyao.XiuLian
{
    public class HuaShenDan : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("化神丹");
            Tooltip.SetDefault("六品丹药" +
                "\n可用于元婴巅峰修士突破化神境界" +
                "\n可用于化神修士日常修炼" +
                "\n服用后灵力吸收速度翻32倍" +
                "\n持续6分钟");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.rare = 6;
            item.value = Item.sellPrice(9999, 0, 0, 0);
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
            if (mp.GetLevel() < 40)
            {
                CombatText.NewText(player.getRect(), Color.Gold, "境界过低，此丹药对你来过太过强大，强行服用恐怕爆体而亡");
                return false;
            }
            else if (mp.GetLevel() > 50)
            {
                CombatText.NewText(player.getRect(), Color.Gold, "境界过高，此丹药对你已经无用，无法吸收");
                return false;
            }
            else
            {
                player.AddBuff(ModContent.BuffType<HuaShenBuff>(), 3600 * 6);
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.GetItem("LingShi3"), 99);
            recipe.AddIngredient(mod.GetItem("NeiDan6"), 1);
            int gem = 9;
            recipe.AddIngredient(ItemID.Amber, gem);
            recipe.AddIngredient(ItemID.Amethyst, gem);
            recipe.AddIngredient(ItemID.Topaz, gem);
            recipe.AddIngredient(ItemID.Sapphire, gem);
            recipe.AddIngredient(ItemID.Ruby, gem);
            recipe.AddIngredient(ItemID.Emerald, gem);
            recipe.AddIngredient(ItemID.Diamond, gem);
            recipe.AddIngredient(ItemID.HallowedBar, gem);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
