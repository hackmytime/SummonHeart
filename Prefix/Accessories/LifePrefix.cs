using System;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Prefix.Accessories
{
    public class LifePrefix : ModPrefix
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

        public LifePrefix()
        {
        }

        public LifePrefix(byte hp)
        {
            this.hp = hp;
        }

        public override bool Autoload(ref string name)
        {
            if (base.Autoload(ref name))
            {
                mod.AddPrefix("傲慢Lv1", new LifePrefix(1));
                mod.AddPrefix("傲慢Lv2", new LifePrefix(2));
                mod.AddPrefix("傲慢Lv3", new LifePrefix(3));
                mod.AddPrefix("傲慢Lv4", new LifePrefix(4));
            }
            return false;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<PrefixItem>().hp = (byte)(hp * 2);
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= hp;
            return;
        }

        private readonly byte hp;
    }
}
