using System;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Prefix.Accessories
{
    public class RegenPrefix : ModPrefix
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

        public RegenPrefix()
        {
        }

        public RegenPrefix(byte value)
        {
            this.value = value;
        }

        public override bool Autoload(ref string name)
        {
            if (base.Autoload(ref name))
            {
                mod.AddPrefix("饕餮Lv1", new RegenPrefix(1));
                mod.AddPrefix("饕餮Lv2", new RegenPrefix(2));
                mod.AddPrefix("饕餮Lv3", new RegenPrefix(3));
                mod.AddPrefix("饕餮Lv4", new RegenPrefix(4));
            }
            return false;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<PrefixItem>().regen = (byte)(value * 5);
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= value;
            return;
        }

        private readonly byte value;
    }
}
