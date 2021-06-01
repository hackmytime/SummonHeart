using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Items
{
    public class SummonHeartGlobalItem : GlobalItem
	{
        public override bool CanUseItem(Item item, Player player)
        {
            if (item.type == ItemID.SiltBlock || item.type == ItemID.SlushBlock || item.type == ItemID.DesertFossil
                || item.type == ItemID.Obsidian || item.type == ItemID.StoneBlock || item.type == ItemID.DirtBlock
                || item.type == ItemID.SandBlock || item.type == ItemID.MudBlock || item.type == ItemID.AshBlock)
            {
                item.useTime = 2;
                item.useAnimation = 3;
            }
            return base.CanUseItem(item, player);
        }

        public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
            if (modPlayer.SummonHeart)
            {
                if (item.melee || item.ranged || item.magic || item.thrown)
                {
                    //第1个额外弹幕-5度角
                    if (modPlayer.SummonCrit >= 0)
                    {
                        Vector2 velocity = new Vector2(speedX, speedY).RotatedBy(MathHelper.Pi / 180 * 5 * (-1));
                        Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockBack, player.whoAmI);
                    }
                    //第2个额外弹幕5度角
                    if (modPlayer.SummonCrit >= 150)
                    {
                        Vector2 velocity = new Vector2(speedX, speedY).RotatedBy(MathHelper.Pi / 180 * 5 * 1);
                        Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockBack, player.whoAmI);
                    }
                    //第3个额外弹幕-10度角
                    if (modPlayer.SummonCrit >= 300)
                    {
                        Vector2 velocity = new Vector2(speedX, speedY).RotatedBy(MathHelper.Pi / 180 * 5 * (-2));
                        Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockBack, player.whoAmI);
                    }
                    //第4个额外弹幕10度角
                    if (modPlayer.SummonCrit >= 450)
                    {
                        Vector2 velocity = new Vector2(speedX, speedY).RotatedBy(MathHelper.Pi / 180 * 5 * 2);
                        Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockBack, player.whoAmI);
                    }
                }
            }
            return base.Shoot(item, player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }


        /*public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            foreach (TooltipLine tooltipLine in tooltips)
            {
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "Speed")
                { 
                    double useTimes =  60.0 / item.useTime;
                    tooltipLine.text = Convert.ToDouble(useTimes).ToString("0.0") + " 每秒攻击次数";
                }
                if (tooltipLine.mod == "Terraria" && tooltipLine.Name == "Knockback")
                {
                    tooltipLine.text = Convert.ToDouble(item.knockBack).ToString("0.0") + " 击退力度";
                }
            }
            String modName = "【原版】";
            if (item.modItem != null)
            {
                modName = "【" + item.modItem.mod.DisplayName + "】";
            }
            TooltipLine line = new TooltipLine(mod, "ModName", modName);
            tooltips.Insert(1, line);
            base.ModifyTooltips(item, tooltips);
        }*/
    }
}
