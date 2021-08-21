using System;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Prefix.Accessories
{
    public class AllDamagePrefix : ModPrefix
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

        public AllDamagePrefix()
        {
        }

        public AllDamagePrefix(byte AllDamage)
        {
            this.AllDamage = AllDamage;
        }

        public override bool Autoload(ref string name)
        {
            if (base.Autoload(ref name))
            {
                mod.AddPrefix("愤怒Lv1", new AllDamagePrefix(5));
                mod.AddPrefix("愤怒Lv2", new AllDamagePrefix(6));
                mod.AddPrefix("愤怒Lv3", new AllDamagePrefix(7));
                mod.AddPrefix("愤怒Lv4", new AllDamagePrefix(8));
            }
            return false;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<PrefixItem>().allDamage = AllDamage;
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= AllDamage / 2;
            return;
        }

        private readonly byte AllDamage;
    }
}
