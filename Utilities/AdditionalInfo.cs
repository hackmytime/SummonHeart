using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using SummonHeart.XiuXianModule.EnumType;

namespace SummonHeart.Utilities
{
    class AdditionalInfo
    {
        static public string GetAdditionalStatInfo(Stat stat)
        {
            string Text = "";
            switch (stat)
            {
                case Stat.灵根:
                    Text = "灵根: 提升功法修炼速度";
                    break;
                case Stat.悟性:
                    Text = "悟性: 提升灵技修炼速度";
                    break;
                case Stat.魅力:
                    Text = "魅力：降低购买价值和重铸价格";
                    break;
                case Stat.气运:
                    Text = "气运：提升物品掉率";
                    break;
                case Stat.道心:
                    Text = "道心：每次点击都送4道源作为挑战奖励\n每1点道心提升燃元秘术10%的效果\n死亡时复活需要消耗的寿元提升0-10倍\n试试肝帝证道难度开局（点满道心）\n键盘砸烂，不寄刀片";
                    break;
                case Stat.功法:
                    Text = "功法：提升功法品阶\n灵气吸收指的是修炼状态下的速度\n平常状态的自动回复只有10分之一效果";
                    break;
                case Stat.体质:
                    Text = "体质：提升气血、回血、防御和灵防";
                    break;
                case Stat.力量:
                    Text = "力量：提升力量掌控境界";
                    break;
                default:
                    Text = "左键+1，右键+20，滚轮可以爽快加";
                    break;
            }

            return Text;
        }
    }
}
