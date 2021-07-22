using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.NPCs.Bosses
{
    // Token: 0x02000296 RID: 662
    public abstract class SummonHeartBoss : ModNPC
    {
        // Token: 0x06000FFD RID: 4093 RVA: 0x0009BE2C File Offset: 0x0009A02C
        public override void SetDefaults()
        {
            npc.knockBackResist = 0f;
            npc.boss = true;
            npc.scale = 1f;
            npc.aiStyle = -1;
            npc.npcSlots = 25f;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.lavaImmune = npc.buffImmune[24] = npc.buffImmune[39] = npc.buffImmune[31] = npc.buffImmune[69] = npc.buffImmune[70] = npc.buffImmune[20] = npc.buffImmune[44] = true;
            NPCID.Sets.TrailCacheLength[npc.type] = 8;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }

        // Token: 0x06000FFE RID: 4094 RVA: 0x000024F1 File Offset: 0x000006F1
        public virtual void OnKilled()
        {
        }

        // Token: 0x06000FFF RID: 4095 RVA: 0x0009BF40 File Offset: 0x0009A140
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                OnKilled();
            }
        }

        // Token: 0x06001000 RID: 4096 RVA: 0x0001748B File Offset: 0x0001568B
        public override bool CheckActive()
        {
            return false;
        }

        // Token: 0x170000A9 RID: 169
        // (get) Token: 0x06001001 RID: 4097 RVA: 0x0008243E File Offset: 0x0008063E
        // (set) Token: 0x06001002 RID: 4098 RVA: 0x0008244D File Offset: 0x0008064D
        protected float StateTimer
        {
            get
            {
                return npc.ai[0];
            }
            set
            {
                npc.ai[0] = value;
            }
        }

        // Token: 0x170000AA RID: 170
        // (get) Token: 0x06001003 RID: 4099 RVA: 0x0008245D File Offset: 0x0008065D
        // (set) Token: 0x06001004 RID: 4100 RVA: 0x0008246D File Offset: 0x0008066D
        protected short State
        {
            get
            {
                return (short)npc.ai[1];
            }
            set
            {
                npc.ai[1] = value;
            }
        }

        // Token: 0x06001005 RID: 4101 RVA: 0x0009BF58 File Offset: 0x0009A158
        protected void ChangeState(short stateID, bool keepVelocity = true, bool keepRotation = false, bool keep3 = false)
        {
            StateTimer = 0f;
            State = stateID;
            npc.ai[2] = 0f;
            if (!keep3)
            {
                npc.ai[3] = 0f;
            }
            if (!keepRotation)
            {
                npc.rotation = 0f;
            }
            if (!keepVelocity)
            {
                npc.velocity = Vector2.Zero;
            }
        }

        // Token: 0x06001006 RID: 4102 RVA: 0x0009BFC5 File Offset: 0x0009A1C5
        protected void SetFrame(int index)
        {
            SetFrame(index / Main.npcFrameCount[npc.type], index % Main.npcFrameCount[npc.type]);
            frameIndex = index;
        }

        // Token: 0x06001007 RID: 4103 RVA: 0x0009BFFA File Offset: 0x0009A1FA
        protected void SetFrame(int x, int y)
        {
            npc.frame = new Rectangle(x * frameWidth, y * frameHeight, frameWidth, frameHeight);
        }

        // Token: 0x040004C0 RID: 1216
        protected int frameWidth;

        // Token: 0x040004C1 RID: 1217
        protected int frameHeight;

        // Token: 0x040004C2 RID: 1218
        protected int frameIndex;
    }
}
