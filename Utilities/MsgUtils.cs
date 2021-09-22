using System;
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

        internal static void BuildHousePacket(int tileX, int tileY, int whoAmI)
        {
            ModPacket packet = SummonHeartMod.Instance.GetPacket();
            packet.Write((byte)8);
            packet.Write(tileX);
            packet.Write(tileY);
            packet.Write(whoAmI);
            packet.Send();
        }

        internal static void SpanNpcPacket(int whoAmI, short travellingMerchant)
        {
            ModPacket packet = SummonHeartMod.Instance.GetPacket();
            packet.Write((byte)9);
            packet.Write(whoAmI);
            packet.Write(travellingMerchant);
            packet.Send();
        }
    }
}
