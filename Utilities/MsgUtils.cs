using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Utilities
{
    public static class MsgUtils
    {
        public static void SyncAnglerQuestReward()
        {
            ModPacket packet = SummonHeartMod.Instance.GetPacket();
            packet.Write((byte)5);
            packet.Send();
        }

        public static void SyncTime()
        {
            ModPacket packet = SummonHeartMod.Instance.GetPacket();
            packet.Write((byte)6);
            packet.Send();
        }

        public static void SyncFallenStar()
        {
            ModPacket packet = SummonHeartMod.Instance.GetPacket();
            packet.Write((byte)7);
            packet.Send();
        }
    }
}
