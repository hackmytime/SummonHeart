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

        [Label("所有敌人最大Hp和防御倍率")]
        [Increment(1)]
        [Range(5, 20)]
        [DefaultValue(5)]
        [Slider]
        public int hpDefMultiplier;

        [Label("所有敌人攻击倍率")]
        [Increment(1)]
        [Range(2, 10)]
        [DefaultValue(2)]
        [Slider]
        public int atkMultiplier;
    }
}