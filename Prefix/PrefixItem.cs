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
        public byte manaExp;
        public byte attackDamage;
        public byte hp;
        public byte regen;
        public byte allDamage;
        public byte myDamageReduceMult;
        public byte lifeSteal;

        public PrefixItem()
        {
            manaExp = 0;
            attackDamage = 0;
            hp = 0;
            regen = 0;
            allDamage = 0;
            myDamageReduceMult = 0;
            lifeSteal = 0;
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
            prefixItem.manaExp = manaExp;
            prefixItem.attackDamage = attackDamage;
            prefixItem.hp = hp;
            prefixItem.regen = regen;
            prefixItem.allDamage = allDamage;
            prefixItem.myDamageReduceMult = myDamageReduceMult;
            prefixItem.lifeSteal = lifeSteal;
            return prefixItem;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if(Main.cpItem.netID != 0 && Main.cpItem.GetGlobalItem<PrefixItem>() != null)
            {
                if (!item.social && item.prefix > 0)
                {
                    int bonus = manaExp - Main.cpItem.GetGlobalItem<PrefixItem>().manaExp;
                    if (bonus > 0)
                    {
                        tooltips.Add(new TooltipLine(mod, "manaExpPrefix", "+" + bonus + "%经验加成")
                        {
                            isModifier = true
                        });
                    }
                }
                if (!item.social && item.prefix > 0)
                {
                    int bonus2 = attackDamage - Main.cpItem.GetGlobalItem<PrefixItem>().attackDamage;
                    if (bonus2 > 0)
                    {
                        tooltips.Add(new TooltipLine(mod, "attackDamagePrefix", "攻击造成敌方攻击力" + bonus2 + "%的额外伤害")
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
                        tooltips.Add(new TooltipLine(mod, "HpPrefix", "+" + bonus3 + "%生命上限")
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
                        tooltips.Add(new TooltipLine(mod, "RegenPrefix", "+" + bonus4 + "生命回复")
                        {
                            isModifier = true
                        });
                    }
                }
                if (!item.social && item.prefix > 0)
                {
                    int bonus5 = allDamage - Main.cpItem.GetGlobalItem<PrefixItem>().allDamage;
                    if (bonus5 > 0)
                    {
                        tooltips.Add(new TooltipLine(mod, "allDamagePrefix", "+" + bonus5 + "%伤害")
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
                if (!item.social && item.prefix > 0)
                {
                    int bonus4 = lifeSteal - Main.cpItem.GetGlobalItem<PrefixItem>().lifeSteal;
                    if (bonus4 > 0)
                    {
                        tooltips.Add(new TooltipLine(mod, "allDamagePrefix", "+" + bonus4 + "生命偷取")
                        {
                            isModifier = true
                        });
                    }
                }
            }
        }

        public override bool NewPreReforge(Item item)
        {
            manaExp = 0;
            attackDamage = 0;
            hp = 0;
            regen = 0;
            allDamage = 0;
            myDamageReduceMult = 0;
            lifeSteal = 0;
            return base.NewPreReforge(item);
        }

        public override void UpdateEquip(Item item, Player player)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            if (item.GetGlobalItem<PrefixItem>().manaExp > 0)
            {
                mp.manaExp += manaExp * 0.01f;
            }
            if (item.GetGlobalItem<PrefixItem>().attackDamage > 0)
            {
                mp.attackDamage += attackDamage * 0.01f;
            }
            if (item.GetGlobalItem<PrefixItem>().hp > 0)
            {
                var addHp = hp * 0.01f;
                int addHpNum = (int)(player.statLifeMax2 * addHp);
                player.statLifeMax2 += addHpNum;
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
            if (item.GetGlobalItem<PrefixItem>().lifeSteal > 0)
            {
                mp.lifeSteal += lifeSteal;
            }
            base.UpdateEquip(item, player);
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(manaExp);
            writer.Write(attackDamage);
            writer.Write(hp);
            writer.Write(regen);
            writer.Write(allDamage);
            writer.Write(myDamageReduceMult);
            writer.Write(lifeSteal);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            manaExp = reader.ReadByte();
            attackDamage = reader.ReadByte();
            hp = reader.ReadByte();
            regen = reader.ReadByte();
            allDamage = reader.ReadByte();
            myDamageReduceMult = reader.ReadByte();
            lifeSteal = reader.ReadByte();
        }
    }
}
