using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
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

        public override void HoldItem(Item item, Player player)
        {
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

            float handMultiplier = SummonHeartConfig.Instance.handMultiplier;
            if (item.melee && modPlayer.boughtbuffList[1])
            {
                float curScale = (modPlayer.handBloodGas / 500 * 0.01f + 0.5f) * handMultiplier;
                curScale = 1f;
                //刺剑距离减半
                if (item.modItem != null && item.modItem.Name == "Raiden")
                {
                    curScale = 0.5f;
                }
                item.scale = curScale + 1f;
            }
        }

        public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
            if (modPlayer.SummonHeart)
            {
                //if (item.melee || item.ranged || item.magic || item.thrown)
                if (item.melee && modPlayer.PlayerClass == 1)
                {
                    //第1个额外弹幕-3度角
                    int bloodGas = modPlayer.handBloodGas;
                    if (bloodGas >= 0)
                    {
                        Vector2 velocity = new Vector2(speedX, speedY).RotatedBy(MathHelper.Pi / 180 * 3 * (-1));
                        Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockBack, player.whoAmI);
                    }
                    //第2个额外弹幕3度角
                    if (bloodGas >= 50000)
                    {
                        Vector2 velocity = new Vector2(speedX, speedY).RotatedBy(MathHelper.Pi / 180 * 3 * 1);
                        Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockBack, player.whoAmI);
                    }
                    //第3个额外弹幕-6度角
                    if (bloodGas >= 100000)
                    {
                        Vector2 velocity = new Vector2(speedX, speedY).RotatedBy(MathHelper.Pi / 180 * 3 * (-2));
                        Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockBack, player.whoAmI);
                    }
                    //第4个额外弹幕6度角
                    if (bloodGas >= 15000)
                    {
                        Vector2 velocity = new Vector2(speedX, speedY).RotatedBy(MathHelper.Pi / 180 * 3 * 2);
                        Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockBack, player.whoAmI);
                    }
                    //第5个额外弹幕-9度角
                    if (bloodGas >= 200000)
                    {
                        Vector2 velocity = new Vector2(speedX, speedY).RotatedBy(MathHelper.Pi / 180 * 3 * (-3));
                        Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockBack, player.whoAmI);
                    }
                    //第6个额外弹幕9度角
                    if (bloodGas >= 200000)
                    {
                        Vector2 velocity = new Vector2(speedX, speedY).RotatedBy(MathHelper.Pi / 180 * 3 * 3);
                        Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockBack, player.whoAmI);
                    }
                }
            }
            return base.Shoot(item, player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }


        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Player player = Main.player[Main.myPlayer];
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

            int num = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("ItemName"));
            if (num != -1)
            {
                string text;
                if (item.modItem == null)
                {
                    text = "【" + (GameCulture.Chinese.IsActive ? "原版" : "Vanilla") + "】";
                }
                else
                {
                    text = "【" + item.modItem.mod.DisplayName + "】";
                }
                tooltips.Insert(num + 1, new TooltipLine(base.mod, "SRC:ModBelongIdentifier", text));
            }
            int num2 = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("Damage"));
            if (num2 != -1)
            {
                if (item.summon)
                {
                    if(modPlayer.boughtbuffList[0])
                    {
                        string text2 = (modPlayer.eyeBloodGas / 1000 + 20) + "%" + (GameCulture.Chinese.IsActive ? "暴击率" : "Critical Strike Chance");
                        tooltips.Insert(num2 + 1, new TooltipLine(base.mod, "SRC:MinionCrit", text2));
                    }
                    else
                    {
                        string text2 = modPlayer.eyeBloodGas / 1000 + "%" + (GameCulture.Chinese.IsActive ? "暴击率" : "Critical Strike Chance");
                        tooltips.Insert(num2 + 1, new TooltipLine(base.mod, "SRC:MinionCrit", text2));
                    }
                }
            }
            int num4 = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("Speed"));
            if (num4 != -1)
            {
                string str = (60f / (float)item.useTime).ToString("f1") + " ";
                tooltips[num4].text = str + (GameCulture.Chinese.IsActive ? "每秒攻击次数" : "Attack Per Secend");
            }
            int num5 = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("Knockback"));
            if (num5 != -1)
            {
                string str2 = item.knockBack.ToString("f1") + " ";
                tooltips[num5].text = str2 + (GameCulture.Chinese.IsActive ? "击退力度" : "Knockback");
            }
        }
    }
}
