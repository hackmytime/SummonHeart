﻿using Microsoft.Xna.Framework;
using SummonHeart.Items.Weapons.Magic;
using SummonHeart.Utilities;
using System;
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
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            if (mp.PlayerClass == 6 && item.magic && player.altFunctionUse == 2)
            {
                if (mp.magicChargeCount <= 0)
                {
                    return false;
                }
                float v1 = item.shootSpeed * 1.5f;
                if (mp.magicChargeCount > 0)
                {
                    //计算伤害
                    int damage = (int)(item.damage * 30 * (player.allDamage - 1 + player.magicDamage + mp.handBloodGas / 20000));
                    Vector2 arrowVelocity = Vector2.Normalize(Main.MouseWorld - player.Center) * v1;
                    if (SummonHeartMod.itemSoundMap.ContainsKey(item))
                        Main.PlaySound(SummonHeartMod.itemSoundMap[item], player.position);
                    item.crit = 100;
                    int v = Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, arrowVelocity.X, arrowVelocity.Y, item.shoot, damage, item.knockBack, player.whoAmI, 0f, 0f);
                    Main.projectile[v].scale *= 3;
                    //Main.projectile[v].velocity *= 2;
                    mp.magicChargeCount--;
                    if(mp.magicChargeCount == 0)
                        mp.magicChargeActive = false;
                }
                /*float launchSpeed = -10f;
                Vector2 backstepVelocity = Vector2.Normalize(Main.MouseWorld - player.Center) * launchSpeed;
                player.velocity = backstepVelocity;
                for (int d = 0; d < 22; d++)
                {
                    Dust.NewDust(player.Center, 0, 0, 20, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default, 0.8f);
                }
                for (int d2 = 0; d2 < 12; d2++)
                {
                    Dust.NewDust(player.Center, 0, 0, 135, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default, 0.8f);
                }
                for (int d3 = 0; d3 < 88; d3++)
                {
                    Dust.NewDust(player.Center, 0, 0, 135, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default, 0.8f);
                }*/
            }
            //刺客
            if (mp.PlayerClass == 2 && item.thrown && player.altFunctionUse == 2)
            {
               /* if (mp.dashingLeft || mp.dashingRight)
                    return false;*/

                int killCost = (int)(mp.killResourceMax2 * 0.02f);
                if (mp.killResourceCurrent < killCost)
                {
                    CombatText.NewText(player.getRect(), Color.Red, "杀意不足，无法使用杀意闪");
                    return false;
                }
                Main.PlaySound(2, player.Center, 28);
                mp.killResourceCurrent -= killCost;
                CombatText.NewText(player.getRect(), Color.Red, "-" + killCost + "杀意值");
                //转换死气值
                float addDeath = killCost / 2;
                if (addDeath < 1)
                    addDeath = 1;
                mp.deathResourceCurrent += (int)addDeath;
                if (mp.deathResourceCurrent > mp.deathResourceMax)
                    mp.deathResourceCurrent = mp.deathResourceMax;
                /*if (player.direction == 1 && player.controlRight)
                {
                    player.controlRight = false;
                    mp.dashingRight = true;
                }
                else if (player.direction == -1 && player.controlLeft)
                {
                    player.controlLeft = false;
                    mp.dashingLeft = true;
                }
                else if (player.direction == 1)
                {
                    mp.dashingLeft = true;
                }
                else if (player.direction == -1)
                {
                    mp.dashingRight = true;
                }*/

                float launchSpeed = -player.maxRunSpeed * 2;
                
                Vector2 backstepVelocity = Vector2.Normalize(Main.MouseWorld - player.Center) * launchSpeed;
                player.velocity = backstepVelocity;
                for (int d = 0; d < 22; d++)
                {
                    Dust.NewDust(player.Center, 0, 0, MyDustId.BlueMagic, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default, 0.8f);
                }
                for (int d2 = 0; d2 < 12; d2++)
                {
                    Dust.NewDust(player.Center, 0, 0, MyDustId.BlueMagic, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default, 0.8f);
                }
                for (int d3 = 0; d3 < 88; d3++)
                {
                    Dust.NewDust(player.Center, 0, 0, MyDustId.BlueMagic, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default, 0.8f);
                }
            }
            return true;
        }

        public override void GrabRange(Item item, Player player, ref int grabRange)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            if (mp.MysteriousCrystal)
            {
                grabRange += 35 * 16;
                if (item.beingGrabbed)
                {
                    Vector2 vector = player.Center - item.Center;
                    item.velocity = (item.velocity * 4f + vector * (20f / vector.Length())) * 0.2f;
                }
            }
        }

        public override void HoldItem(Item item, Player player)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();

            float handMultiplier = SummonHeartConfig.Instance.handMultiplier;
            if (item.melee && mp.boughtbuffList[1])
            {
                if(mp.PlayerClass == 1 || mp.PlayerClass == 4)
                {
                    float curScale = (mp.handBloodGas / 500 * 0.01f + 0.5f) * handMultiplier;
                    curScale = 1f;
                    item.scale = curScale + 1f;
                }
            }

            if(mp.PlayerClass == 6 && item.magic && !item.channel)
            {
                if (Main.mouseLeft && !Main.mouseLeftRelease)
                {
                    mp.inMagicCharging = true;
                }
                else if (mp.autoAttack)
                {
                    if(mp.magicBook && player.statManaMax2 >= 200)
                        mp.inMagicCharging = true;
                }

                if (item.UseSound != null)
                {
                    if (!SummonHeartMod.itemSoundMap.ContainsKey(item))
                        SummonHeartMod.itemSoundMap.Add(item, item.UseSound);
                    item.UseSound = null;
                }
            }

            if (mp.inMagicCharging)
            {
                if (player.altFunctionUse != 2)
                {
                    //计算魔法消耗
                    item.mana = 0;
                    int magicCostCount = (int)mp.magicChargeCount;
                    magicCostCount *= magicCostCount;
                    if(magicCostCount < 10)
                    {
                        magicCostCount = 10;
                    }
                    magicCostCount /= 10;
                    if (player.statMana >= magicCostCount)
                    {
                        /*item.useTime = 0;
                        item.useAnimation = 0;*/
                        mp.magicChargeActive = true;
                        
                        if (mp.magicChargeCount == mp.magicChargeCountMax)
                        {
                            mp.magicCharge = 0;
                            return;
                        }
                        else
                        {
                            //计算增加的量
                            float addCharge = 2f;
                            if (mp.boughtbuffList[1])
                            {
                                addCharge += (mp.handBloodGas / 2500 + 20) * 0.01f;
                            }
                            mp.magicCharge += addCharge;
                            if(Main.time % 10 == 0)
                                player.statMana -= magicCostCount;
                            if (mp.magicCharge >= mp.magicChargeMax)
                            {
                                Main.PlaySound(SoundID.Item29, player.position);
                                mp.magicCharge = 0;
                                mp.magicChargeCount++;
                            }
                        }
                        if (mp.magicCharge >= 99f || mp.magicChargeCount == mp.magicChargeCountMax)
                        {
                            for (int d = 0; d < 22; d++)
                            {
                                Dust.NewDust(player.Center, 0, 0, 20, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default, 0.8f);
                            }
                            for (int d2 = 0; d2 < 12; d2++)
                            {
                                Dust.NewDust(player.Center, 0, 0, 135, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default, 0.8f);
                            }
                            for (int d3 = 0; d3 < 88; d3++)
                            {
                                Dust.NewDust(player.Center, 0, 0, 135, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default, 0.8f);
                            }
                        }
                        if (player.GetModPlayer<SummonHeartPlayer>().magicCharge < 100f)
                        {
                            for (int i = 0; i < 30; i++)
                            {
                                Vector2 offset = default;
                                double angle = Main.rand.NextDouble() * 2.0 * 3.141592653589793;
                                offset.X += (float)(Math.Sin(angle) * (100f - player.GetModPlayer<SummonHeartPlayer>().magicCharge));
                                offset.Y += (float)(Math.Cos(angle) * (100f - player.GetModPlayer<SummonHeartPlayer>().magicCharge));
                                Dust dust = Dust.NewDustPerfect(player.MountedCenter + offset, 20, new Vector2?(player.velocity), 200, default, 0.5f);
                                dust.fadeIn = 0.1f;
                                dust.noGravity = true;
                            }
                            Vector2 vector = new Vector2(Main.rand.Next(-28, 28) * -9.88f, Main.rand.Next(-28, 28) * -9.88f);
                            Dust dust2 = Main.dust[Dust.NewDust(player.MountedCenter + vector, 1, 1, 20, 0f, 0f, 255, new Color(0.8f, 0.4f, 1f), 0.8f)];
                            dust2.velocity = -vector / 12f;
                            dust2.velocity -= player.velocity / 8f;
                            dust2.noLight = true;
                            dust2.noGravity = true;
                            return;
                        }
                        Dust.NewDust(player.Center, 0, 0, 20, 0f + Main.rand.Next(-5, 5), 0f + Main.rand.Next(-5, 5), 150, default, 0.8f);
                        return;
                    }
                    else
                    {
                        //mp.inMagicCharging = false;
                        mp.magicCharge = 0;
                        mp.inMagicCharging = false;
                        if (player.statMana <= 0f)
                        {
                            /* if(SummonHeartMod.itemSoundMap.ContainsKey(item))
                                 Main.PlaySound(SummonHeartMod.itemSoundMap[item], player.position);
                             player.GetModPlayer<SummonHeartPlayer>().magicChargeActive = false;
                             player.GetModPlayer<SummonHeartPlayer>().magicCharge = 0f;
                             int v = Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, arrowVelocity.X, arrowVelocity.Y, item.shoot, 86 + (int)Math.Round(player.GetModPlayer<SummonHeartPlayer>().magicCharge / 10f), 4 + (int)Math.Round(player.GetModPlayer<SummonHeartPlayer>().magicCharge / 10f), player.whoAmI, 0f, 0f);
                             Main.projectile[v].scale *= 2;
                             Main.projectile[v].velocity *= 1.5f;*/
                        }
                    }
                }
            }
        }

        public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            if (item.fishingPole > 0)
            {
                int lures = mp.fishLureCount;
                for (int i = 0; i < lures; i++)
                {
                    Projectile.NewProjectile(position.X + (float)Main.rand.Next(5), position.Y + (float)Main.rand.Next(5), speedX + (float)Main.rand.Next(5), speedY + (float)Main.rand.Next(5), type, damage, knockBack, player.whoAmI, 0f, 0f);
                }
            }
            if (mp.PlayerClass == 6 && item.magic && !item.channel)
            {
                return false;
            }
            if (item.magic && mp.PlayerClass == 5 && mp.boughtbuffList[1] && !(item.modItem is DemonStaff))
            {
                if(player.statMana < mp.costMana)
                {
                    return false;
                }
                /*else if (item.modItem != null && item.modItem.Name == "DemonStaff")
                {
                    player.statMana -= 5;
                }*/
                else
                {
                    int maxPro = mp.handBloodGas / 33333 + 1;
                    player.statMana -= mp.costMana;
                    for (int i = 0; i < maxPro; i++)
                    {
                        int param = i / 2 + 1;
                        if (i % 2 == 0)
                        {
                            param *= -1;
                        }
                        Vector2 velocity = new Vector2(speedX, speedY).RotatedBy(MathHelper.Pi / 180 * 2 * param);
                        Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockBack, player.whoAmI);
                    }
                }
            }
            if (mp.SummonHeart && item.melee)
            {
                //if (item.melee || item.ranged || item.magic || item.thrown)
                if (mp.PlayerClass == 1 || mp.PlayerClass == 4)
                {
                    //第1个额外弹幕-3度角
                    int bloodGas = mp.handBloodGas;
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
            Player player = Main.LocalPlayer;
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();

            int num = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("ItemName"));
            if (num != -1)
            {
                string text;
                if (SummonHeartConfig.Instance.DisplayItemModNameConfig)
                {
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
                if (item.fishingPole > 0)
                {
                    text = "钓鱼等级 Lv" + mp.fishLureCount;
                    tooltips.Insert(num + 2, new TooltipLine(base.mod, "FishLevel", text));
                    tooltips[num + 2].overrideColor = Color.LightGreen;
                    text = "额外鱼线 " + mp.fishLureCount;
                    tooltips.Insert(num + 3, new TooltipLine(base.mod, "FishLureCount", text));
                    tooltips[num + 3].overrideColor = Color.LightGreen;
                    text = "完成钓鱼任务数 " + mp.fishCount;
                    tooltips.Insert(num + 4, new TooltipLine(base.mod, "FishCount", text));
                    tooltips[num + 4].overrideColor = Color.LightGreen;
                    int curCount = mp.nextFishCount;
                    if(mp.fishLureCount < 100)
                        text = "升级需要任务数达到 " + curCount;
                    else
                        text = "升级需要任务数达到 等级已满";
                    tooltips.Insert(num + 5, new TooltipLine(base.mod, "FishCount", text));
                    tooltips[num + 5].overrideColor = Color.LightGreen;
                }
            }
            int num2 = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("Damage"));
            if (num2 != -1)
            {
                if (mp.PlayerClass == 1 && mp.onDoubleDamage)
                {
                    int num4 = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("CritChance"));
                    if (num4 != -1)
                    {
                        int damage = (int)(item.damage + mp.damageResourceCurrent * 2);
                        string text2 = "双倍偿还+" + mp.damageResourceCurrent * 2 + "伤害";
                        tooltips.Insert(num2 + 1, new TooltipLine(base.mod, "KillMove", text2));
                        tooltips[num2 + 1].overrideColor = Color.LightGreen;
                    }
                }
                if (mp.PlayerClass == 2 && item.thrown)
                {
                    int num4 = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("CritChance"));
                    if (num4 != -1)
                    {
                        int damage = (int)(item.damage + mp.damageResourceCurrent * 2);
                        string text2 = "额外真实伤害+" + mp.addRealDamage;
                        tooltips.Insert(num2 + 1, new TooltipLine(base.mod, "KillMove", text2));
                        tooltips[num2 + 1].overrideColor = Color.LightGreen;
                    }
                }
            }
           
            if (mp.PlayerClass == 6 && item.magic && !item.channel)
            {
                int num4 = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("CritChance"));
                if (num4 != -1)
                {
                    string str = " " + mp.magicChargeCountMax;
                    tooltips[num4].text = (GameCulture.Chinese.IsActive ? "充能魔法储存上限" : "Power Magic MaxStack" + str);
                    tooltips[num4].overrideColor = Color.LightGreen;
                    //计算伤害
                    int damage = (int)(item.damage * 10 * (player.allDamage -1 + player.magicDamage + mp.handBloodGas / 20000));
                    string text2 = "左键充能，右键施放充能魔法" +
                   "\n充能魔法伤害 " + damage +
                   "\n充能魔法弹幕速度 2倍" +
                   "\n充能魔法弹幕大小 3倍";
                    tooltips[num4 + 1].text = text2;
                    tooltips[num4 + 1].overrideColor = Color.LightSkyBlue;
                }
                int num5 = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("Knockback"));
                if (num5 != -1)
                {
                    float addCharge = 1f;
                    if (mp.boughtbuffList[1])
                    {
                        addCharge += (mp.handBloodGas / 2500 + 20) * 0.01f;
                    }
                   
                    string str2 = 100 / addCharge / 60 + "秒 ";
                    tooltips[num5].text = str2 + (GameCulture.Chinese.IsActive ? "充能完成速度" : "Knockback");
                    tooltips[num5].overrideColor = Color.LightSkyBlue;
                }
                int num6 = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("UseMana"));
                if (num6 != -1)
                {
                    //计算魔法消耗
                    int magicCostCount = (int)mp.magicChargeCount;
                    magicCostCount *= magicCostCount;
                    if (magicCostCount < 10)
                    {
                        magicCostCount = 10;
                    }
                    tooltips[num6].text = "当前充能魔法消耗 " + magicCostCount;
                }
            }
            else if(mp.PlayerClass == 2 && item.thrown && !item.channel)
            {
                int num4 = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("CritChance"));
                if (num4 != -1)
                {
                    int damage = (int)(item.damage * 10 * (player.allDamage - 1 + player.magicDamage + mp.handBloodGas / 20000));
                    string text2 = "右键消耗2%最大杀意值施放杀意闪";
                    tooltips.Insert(num4-1, new TooltipLine(base.mod, "KillMove", text2));
                    tooltips[num4-1].overrideColor = Color.LightGreen;
                }
            }
            else
            {
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

                if (item.magic && mp.PlayerClass == 5 && mp.boughtbuffList[1])
                {
                    int num6 = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("UseMana"));
                    if (num6 != -1)
                    {
                        int str2 = (int)(player.manaCost + mp.costMana);
                        tooltips[num6].text = tooltips[num6].text + "(额外消耗" + mp.costMana + ")";
                    }
                }
            }
        }

        public override bool AltFunctionUse(Item item, Player player)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            if (mp.PlayerClass == 2 && item.thrown && !item.summon)
                return true;
            if (mp.PlayerClass == 6 && item.magic && !item.summon)
                return true;
            return base.AltFunctionUse(item, player);
        }

        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (context == "bossBag")
            {
                player.QuickSpawnItem(ItemID.LifeCrystal, Main.rand.Next(1, 2));
            }
            base.OpenVanillaBag(context, player, arg);
        }
    }
}
