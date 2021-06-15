using Terraria;
using static Terraria.ModLoader.ModContent;

namespace SummonHeart.costvalues
{
    public class MoneyCostValue : CostValue
    {
        public int cost;

        public MoneyCostValue(int cost)
        {
            this.cost = cost;
        }

        public bool CheckBuy()
        {
            Main.LocalPlayer.CanBuyItem(cost);

            return  Main.LocalPlayer.CanBuyItem(cost);
        }

        public void Buy()
        {
            if (true)
            {
                Main.LocalPlayer.BuyItem(cost);
            }
        }

        public string UIInfo()
        {
            return "";
        }
    }
}
