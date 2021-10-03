using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Extensions
{
    // Token: 0x0200002E RID: 46
    public static class Helper
    {
        public static bool Is<T>(this Item item) where T : ModItem
        {
            return item != null && !item.IsAir && item.modItem is T;
        }
        // Token: 0x06000131 RID: 305 RVA: 0x0000F9F0 File Offset: 0x0000DBF0
        public static void Navigate(this Projectile p, Vector2 to, float speed, float smooth)
        {
            Vector2 move = to - p.position;
            Vector2 vel = move * (speed / move.Length());
            p.velocity += (vel - p.velocity) / smooth;
        }

        // Token: 0x06000132 RID: 306 RVA: 0x0000FA40 File Offset: 0x0000DC40
        public static void CreateDust(Rectangle hitbox, float speedX, float speedY, int DustType, int DustCount, float DustSize, Color color, int Alpha = 0, bool noGrav = false, bool noLight = false)
        {
            for (int DustCounter = DustCount; DustCounter > 0; DustCounter--)
            {
                Vector2 randSpeed = new Vector2(Main.rand.NextFloat(-speedX, speedX), Main.rand.NextFloat(-speedY, speedY)); ;
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustType, randSpeed.X, randSpeed.Y, Alpha, color, DustSize);
                Main.dust[dust].noGravity = noGrav;
                Main.dust[dust].noLight = noLight;
            }
        }

        // Token: 0x06000133 RID: 307 RVA: 0x0000FAD0 File Offset: 0x0000DCD0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NewDust(Rectangle rect, Vector2 speed, int type, int count = 1, int chance = 100, float size = 1f, Color color = default, int alpha = 0, bool noGrav = true, bool noLight = false, Action<Dust> callback = null)
        {
            for (int i = 0; i < count; i++)
            {
                if (Main.rand.Next(100) <= chance)
                {
                    Dust dust = Dust.NewDustDirect(rect.GetPosition(), rect.Width, rect.Height, type, speed.X, speed.Y, alpha, color, size);
                    dust.noGravity = noGrav;
                    dust.noLight = noLight;
                    if (callback != null)
                    {
                        callback(dust);
                    }
                }
            }
        }

        // Token: 0x06000134 RID: 308 RVA: 0x0000FB40 File Offset: 0x0000DD40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NewDust(Vector2 position, Vector2 speed, int type, int count = 1, int chance = 100, float size = 1f, int w = 1, int h = 1, Color color = default, int alpha = 0, bool noGrav = true, bool noLight = false, Action<Dust> callback = null)
        {
            NewDust(new Rectangle((int)position.X, (int)position.Y, w, h), speed, type, count, chance, size, color, alpha, noGrav, noLight, callback);
        }

        // Token: 0x06000135 RID: 309 RVA: 0x0000FB7C File Offset: 0x0000DD7C
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NPC GetNearestNPC(Vector2 position, Predicate<NPC> condition = null, float maxDist = 3.4028235E+38f)
        {
            float minDist = maxDist;
            NPC result = null;
            Predicate<NPC> predicate;
            if ((predicate = condition) == null)
            {
                predicate = (NPC npc) => !npc.friendly;
            }
            condition = predicate;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC npc2 = Main.npc[i];
                if (npc2.active && condition(npc2))
                {
                    float dist = Vector2.Distance(position, npc2.Center);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        result = npc2;
                    }
                }
            }
            return result;
        }

        // Token: 0x06000136 RID: 310 RVA: 0x0000FBF8 File Offset: 0x0000DDF8
        public static Player GetNearestPlayerDirect(Vector2 Point, bool Alive = false)
        {
            float NearestPlayerDist = -1f;
            Player NearestPlayer = null;
            foreach (Player player in Main.player)
            {
                if ((!Alive || player.active && !player.dead) && (NearestPlayerDist == -1f || player.Distance(Point) < NearestPlayerDist))
                {
                    NearestPlayerDist = player.Distance(Point);
                    NearestPlayer = player;
                }
            }
            return NearestPlayer;
        }

        // Token: 0x06000137 RID: 311 RVA: 0x0000FC5C File Offset: 0x0000DE5C
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Osc(float from, float to, float speed = 1f, float offset = 0f)
        {
            float dif = (to - from) / 2f;
            return from + dif + dif * (float)Math.Sin(Main.GlobalTime * speed + offset);
        }

        // Token: 0x06000138 RID: 312 RVA: 0x0000FC89 File Offset: 0x0000DE89
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Osc01(float speed = 1f, float offset = 0f)
        {
            return Osc(0f, 1f, speed, offset);
        }

        // Token: 0x06000139 RID: 313 RVA: 0x0000FC9C File Offset: 0x0000DE9C
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float OscSaw(float from, float to, float speed = 1f, float offset = 0f)
        {
            return from + (to - from) * (1f - Math.Abs((Main.GlobalTime * speed + offset) % 1f - 0.5f) / 0.5f);
        }

        // Token: 0x0600013A RID: 314 RVA: 0x0000FCCA File Offset: 0x0000DECA
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Saw(float from, float to, float speed = 1f, float offset = 0f)
        {
            return from + (to - from) * ((Main.GlobalTime * speed + offset) % 1f);
        }

        // Token: 0x0600013B RID: 315 RVA: 0x0000FCE1 File Offset: 0x0000DEE1
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Text(string translationText)
        {
            return Language.GetTextValue("Mods.Split." + translationText);
        }

        // Token: 0x0600013C RID: 316 RVA: 0x0000FCF3 File Offset: 0x0000DEF3
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Text(string translationText, params object[] format)
        {
            return string.Format(Text(translationText), format);
        }

        // Token: 0x0600013D RID: 317 RVA: 0x0000FD01 File Offset: 0x0000DF01
        public static void ChatText(string translationText, Color color = default)
        {
            Main.NewText(translationText, color, false);
        }

        // Token: 0x0600013E RID: 318 RVA: 0x0000FD0B File Offset: 0x0000DF0B
        public static string GetRandomText(string keyFilter)
        {
            LocalizedText localizedText = Language.SelectRandom(Lang.CreateDialogFilter("Mods.Split." + keyFilter), null);
            return Language.GetTextValue(localizedText != null ? localizedText.Key : null);
        }

        // Token: 0x0600013F RID: 319 RVA: 0x0000FD34 File Offset: 0x0000DF34
       /* public static void PlaySound(string sound, Vector2? position = null, float volume = 1f, float pitch = 0f, bool sync = false)
        {
            Vector2 pos = position ?? Main.LocalPlayer.Center;
            sound = "Sounds/" + sound;
            Main.PlaySound(50, (int)pos.X, (int)pos.Y, Split.Instance.GetSoundSlot(50, sound), volume, pitch);
            if (sync && Main.netMode == 1)
            {
                NetHelper.SendSoundMessage(sound, pos, volume, pitch);
            }
        }

        // Token: 0x06000140 RID: 320 RVA: 0x0000FDA8 File Offset: 0x0000DFA8
        public static NPC NewNPC(Vector2 position, string npc)
        {
            return Main.npc[NPC.NewNPC((int)position.X, (int)position.Y, Split.Instance.NPCType(npc), 0, 0f, 0f, 0f, 0f, 255)];
        }*/
    }
}
