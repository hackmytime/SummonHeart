using System;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Prefix.Accessories
{
    public class ExpPrefix : ModPrefix
    {
        public override float RollChance(Item item)
        {
            return 1f;
        }

        public override bool CanRoll(Item item)
        {
            return true;
        }

        public override PrefixCategory Category
        {
            get
            {
                return PrefixCategory.Accessory;
            }
        }

        public ExpPrefix()
        {
        }

        public ExpPrefix(byte value)
        {
            this.value = value;
        }

        public override bool Autoload(ref string name)
        {
            if (base.Autoload(ref name))
            {
                mod.AddPrefix("贪婪Lv1", new ExpPrefix(1));
                mod.AddPrefix("贪婪Lv2", new ExpPrefix(2));
                mod.AddPrefix("贪婪Lv3", new ExpPrefix(3));
                mod.AddPrefix("贪婪Lv4", new ExpPrefix(4));
            }
            return false;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<PrefixItem>().manaExp = (byte)(value * 5);
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= value;
            return;
        }

        private readonly byte value;
    }
}
