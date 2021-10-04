﻿using System;
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

        internal static void BombPacket(int whoAmI)
        {
            ModPacket packet = SummonHeartMod.Instance.GetPacket();
            packet.Write((byte)10);
            packet.Write((byte)whoAmI);
            packet.Send(-1, -1);
        }
        internal static void TurretShootPacket(int i, int projID, int shootDamage, float shootKnockback, float x, float y)
        {
            ModPacket packet = SummonHeartMod.Instance.GetPacket();
            packet.Write((byte)11);
            packet.Write(i);
            packet.Write(projID);
            packet.Write(shootDamage);
            packet.Write(shootKnockback);
            packet.Send(-1, -1);
        }
    }
}
