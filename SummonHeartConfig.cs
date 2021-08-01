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

        [DefaultValue(false)]
        [Label("突破21亿32位系统上限，非64位tml不要开")]
        public bool OverLoadConfig;

        [Label("魔神之手攻击范围倍率")]
        [Range(0, 1f)]
        [DefaultValue(1)]
        [Slider]
        public float handMultiplier;

        [Label("自定义额外饰品栏X坐标位置")]
        [Increment(1)]
        [Range(0, 1000)]
        [DefaultValue(20)]
        [Slider]
        public int accX;

        [Label("自定义额外饰品栏Y坐标位置")]
        [Increment(1)]
        [Range(0, 800)]
        [DefaultValue(306)]
        [Slider]
        public int accY;

        [Label("自定义魔神之眼气血上限")]
        [Increment(1000)]
        [Range(0, 200000)]
        [DefaultValue(200000)]
        [Slider]
        public int eyeMax;

        [Label("自定义魔神之手气血上限")]
        [Increment(1000)]
        [Range(0, 200000)]
        [DefaultValue(200000)]
        [Slider]
        public int handMax;

        [Label("自定义魔神之躯气血上限")]
        [Increment(1000)]
        [Range(0, 200000)]
        [DefaultValue(200000)]
        [Slider]
        public int bodyMax;

        [Label("自定义魔神之腿气血上限")]
        [Increment(1000)]
        [Range(0, 200000)]
        [DefaultValue(200000)]
        [Slider]
        public int footMax;
    }
}