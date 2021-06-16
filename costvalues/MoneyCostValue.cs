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
            SummonHeartPlayer summonHeartPlayer = Main.player[Main.myPlayer].GetModPlayer<SummonHeartPlayer>();
            return summonHeartPlayer.CheckSoul(cost);
        }

        public void Buy()
        {
            if (true)
            {
                SummonHeartPlayer summonHeartPlayer = Main.player[Main.myPlayer].GetModPlayer<SummonHeartPlayer>();
                summonHeartPlayer.BuySoul(cost);
            }
        }

        public string UIInfo()
        {
            return "";
        }
    }
}
