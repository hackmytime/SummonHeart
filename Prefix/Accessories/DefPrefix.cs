using System;
using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Prefix.Accessories
{
    public class DefPrefix : ModPrefix
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

        public DefPrefix()
        {
        }

        public DefPrefix(byte myDamageReduceMult)
        {
            this.myDamageReduceMult = myDamageReduceMult;
        }

        public override bool Autoload(ref string name)
        {
            if (base.Autoload(ref name))
            {
                mod.AddPrefix("怠惰Lv1", new DefPrefix(1));
                mod.AddPrefix("怠惰Lv2", new DefPrefix(2));
                mod.AddPrefix("怠惰Lv3", new DefPrefix(3));
                mod.AddPrefix("怠惰Lv4", new DefPrefix(4));
            }
            return false;
        }

        public override void Apply(Item item)
        {
            item.GetGlobalItem<PrefixItem>().myDamageReduceMult = myDamageReduceMult;
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= myDamageReduceMult;
            return;
        }

        private readonly byte myDamageReduceMult;
    }
}
