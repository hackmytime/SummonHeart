using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Tools.Bomb
{
    public abstract class ExplosiveItem : ModItem
    {
        public override bool CloneNewInstances { get; } = true;

        /// <summary>
        /// If this is true then the mod will add this item to the disclaimer list. Returns false by default
        /// </summary>
        public bool toolTipDisclamer = false;

        /// <summary>
        /// If this is true then the mod will add this item to the Bombard tooltip list. Returns false by default
        /// </summary>
        public bool BombardTag = false;

        public ModItem ModItem
        {
            get;
            internal set;
        }

        public bool Explosive = true;

        public virtual void SafeSetDefaults()
        {

        }

        public sealed override void SetDefaults()
        {
            SafeSetDefaults();
            item.melee = false;
            item.ranged = false;
            item.magic = false;
            item.summon = false;
            item.thrown = false;
            DangerousSetDefaults();
        }

        public virtual void DangerousSetDefaults()
        {

        }
     
    }
}