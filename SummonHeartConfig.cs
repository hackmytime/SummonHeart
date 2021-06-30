using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace SummonHeart
{
    public class SummonHeartConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public static SummonHeartConfig Instance;

        [DefaultValue(true)]
        [Label("炼体特效")]
        public bool EffectVisualConfig;

        [Label("魔神之手攻击范围倍率")]
        [Range(0, 1f)]
        [DefaultValue(1)]
        [Slider]
        public float handMultiplier;

       /* [Label("敌人攻击倍率(每1倍+1W炼体世界上限)")]
        [Increment(1)]
        [Range(2, 10)]
        [DefaultValue(2)]
        [Slider]
        public int atkMultiplier;*/
    }
}