namespace SummonHeart.costvalues
{
    public interface CostValue
    {
        bool CheckBuy();
        void Buy();
        string UIInfo();
    }
}
