using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using SummonHeart.RPGModule.Enum;

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
                    Text = "道心：+3道源\n死亡时复活需要消耗的寿元大幅度提升\n试试钢铁道心开局";
                    break;
                case Stat.功法:
                    Text = "功法：提升功法品阶";
                    break;
                case Stat.体质:
                    Text = "体质：提升气血";
                    break;
                case Stat.力量:
                    Text = "力量：提升力量掌控境界";
                    break;
            }

            return Text;
        }
    }
}
