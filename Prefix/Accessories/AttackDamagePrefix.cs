using System;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Prefix.Accessories
{
    public class AttackDamagePrefix : ModPrefix
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

        public AttackDamagePrefix()
        {
        }

        public AttackDamagePrefix(byte value)
        {
            this.value = value;
        }

        public override bool Autoload(ref string name)
        {
            if (base.Autoload(ref name))
            {
                mod.AddPrefix("嫉妒Lv1", new AttackDamagePrefix(1));
                mod.AddPrefix("嫉妒Lv2", new AttackDamagePrefix(2));
                mod.AddPrefix("嫉妒Lv3", new AttackDamagePrefix(3));
                mod.AddPrefix("嫉妒Lv4", new AttackDamagePrefix(4));
            }
            return false;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<PrefixItem>().attackDamage = value;
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= value;
            return;
        }

        private readonly byte value;
    }
}
