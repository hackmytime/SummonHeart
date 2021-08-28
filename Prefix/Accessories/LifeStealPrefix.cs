using System;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Prefix.Accessories
{
    public class LifeStealPrefix : ModPrefix
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

        public LifeStealPrefix()
        {
        }

        public LifeStealPrefix(byte value)
        {
            this.value = value;
        }

        public override bool Autoload(ref string name)
        {
            if (base.Autoload(ref name))
            {
                mod.AddPrefix("色欲Lv1", new LifeStealPrefix(1));
                mod.AddPrefix("色欲Lv2", new LifeStealPrefix(2));
                mod.AddPrefix("色欲Lv3", new LifeStealPrefix(3));
                mod.AddPrefix("色欲Lv4", new LifeStealPrefix(4));
            }
            return false;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<PrefixItem>().lifeSteal = value;
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= value;
            return;
        }

        private readonly byte value;
    }
}
