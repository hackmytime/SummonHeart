using SummonHeart;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Prefix
{
    public class PrefixItem : GlobalItem
    {
        public byte mana;
        public byte critDamage;
        public byte hp;
        public byte regen;
        public byte allDamage;
        public byte myDamageReduceMult;

        public PrefixItem()
        {
            mana = 0;
            critDamage = 0;
            hp = 0;
            regen = 0;
            allDamage = 0;
            myDamageReduceMult = 0;
        }

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            PrefixItem prefixItem = (PrefixItem)base.Clone(item, itemClone);
            prefixItem.mana = mana;
            prefixItem.critDamage = critDamage;
            prefixItem.hp = hp;
            prefixItem.regen = regen;
            prefixItem.allDamage = allDamage;
            prefixItem.myDamageReduceMult = myDamageReduceMult;
            return prefixItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!item.social && item.prefix > 0)
            {
                int bonus = mana - Main.cpItem.GetGlobalItem<PrefixItem>().mana;
                if (bonus > 0)
                {
                    tooltips.Add(new TooltipLine(mod, "ManaPrefix", "+" + bonus + " Mana")
                    {
                        isModifier = true
                    });
                }
            }
            if (!item.social && item.prefix > 0)
            {
                int bonus2 = critDamage - Main.cpItem.GetGlobalItem<PrefixItem>().critDamage;
                if (bonus2 > 0)
                {
                    tooltips.Add(new TooltipLine(mod, "CritDamagePrefix", "+" + bonus2 + "% 伤害")
                    {
                        isModifier = true
                    });
                }
            }
            if (!item.social && item.prefix > 0)
            {
                int bonus3 = hp - Main.cpItem.GetGlobalItem<PrefixItem>().hp;
                if (bonus3 > 0)
                {
                    tooltips.Add(new TooltipLine(mod, "HpPrefix", "+" + bonus3 + " Max Life")
                    {
                        isModifier = true
                    });
                }
            }
            if (!item.social && item.prefix > 0)
            {
                int bonus4 = regen - Main.cpItem.GetGlobalItem<PrefixItem>().regen;
                if (bonus4 > 0)
                {
                    tooltips.Add(new TooltipLine(mod, "RegenPrefix", "+" + Math.Round(bonus4 / 2.0, 1) + " Life Regen")
                    {
                        isModifier = true
                    });
                }
            } 
            if (!item.social && item.prefix > 0)
            {
                int bonus4 = allDamage - Main.cpItem.GetGlobalItem<PrefixItem>().allDamage;
                if (bonus4 > 0)
                {
                    tooltips.Add(new TooltipLine(mod, "allDamagePrefix", "+" + bonus4 + "%伤害")
                    {
                        isModifier = true
                    });
                }
            }
            if (!item.social && item.prefix > 0)
            {
                int bonus4 = myDamageReduceMult - Main.cpItem.GetGlobalItem<PrefixItem>().myDamageReduceMult;
                if (bonus4 > 0)
                {
                    tooltips.Add(new TooltipLine(mod, "allDamagePrefix", "+" + bonus4 * 0.1f + "减伤倍率")
                    {
                        isModifier = true
                    });
                }
            }
        }

        public override bool NewPreReforge(Item item)
        {
            mana = 0;
            critDamage = 0;
            hp = 0;
            regen = 0;
            allDamage = 0;
            myDamageReduceMult = 0;
            return base.NewPreReforge(item);
        }

        public override void UpdateEquip(Item item, Player player)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            if (item.GetGlobalItem<PrefixItem>().mana > 0)
            {
                player.statManaMax2 += mana;
            }
            if (item.GetGlobalItem<PrefixItem>().critDamage > 0)
            {
                player.GetModPlayer<SummonHeartPlayer>().MyCritDmageMult += critDamage;
            }
            if (item.GetGlobalItem<PrefixItem>().hp > 0)
            {
                player.statLifeMax2 += hp;
            }
            if (item.GetGlobalItem<PrefixItem>().regen > 0)
            {
                player.lifeRegen += regen;
            }
            if (item.GetGlobalItem<PrefixItem>().allDamage > 0)
            {
                var addDmage = allDamage * 0.01f;
                player.allDamage += addDmage;
            }
            if (item.GetGlobalItem<PrefixItem>().myDamageReduceMult > 0)
            {
                mp.myDamageReduceMult += myDamageReduceMult * 0.1f;
            }
            base.UpdateEquip(item, player);
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(mana);
            writer.Write(critDamage);
            writer.Write(hp);
            writer.Write(regen);
            writer.Write(allDamage);
            writer.Write(myDamageReduceMult);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            mana = reader.ReadByte();
            critDamage = reader.ReadByte();
            hp = reader.ReadByte();
            regen = reader.ReadByte();
            allDamage = reader.ReadByte();
            myDamageReduceMult = reader.ReadByte();
        }
    }
}
