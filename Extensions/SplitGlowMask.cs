using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Terraria;

namespace SummonHeart.Extensions
{
    // Token: 0x0200001F RID: 31
    public static class SplitGlowMask
    {
        // Token: 0x06000097 RID: 151 RVA: 0x00007EC0 File Offset: 0x000060C0
        public static short Get(string ident)
        {
            short index;
            if (glowMasks != null && glowMasks.TryGetValue(ident, out index))
            {
                return index;
            }
            return -1;
        }

        // Token: 0x06000098 RID: 152 RVA: 0x00007EE6 File Offset: 0x000060E6
        private static string LoadJson()
        {
            return Encoding.UTF8.GetString(SummonHeartMod.Instance.GetFileBytes("Data/Glowmasks.json"));
        }

        // Token: 0x06000099 RID: 153 RVA: 0x00007F01 File Offset: 0x00006101
        private static string UnwrapPath(KeyValuePair<string, string> pair)
        {
            return pair.Value.Replace("$", pair.Key).Replace("^", "Glow");
        }

        // Token: 0x0600009A RID: 154 RVA: 0x00007F2C File Offset: 0x0000612C
        public static void Load()
        {
            glowMasks = new Dictionary<string, short>();
            Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(LoadJson());
            List<Texture2D> masks = Main.glowMaskTexture.ToList();
            short index = (short)masks.Count;
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                Texture2D texture = SummonHeartMod.Instance.GetTexture(UnwrapPath(pair));
                texture.Name = "SplitMod_" + pair.Key;
                masks.Add(texture);
                Dictionary<string, short> dictionary2 = glowMasks;
                string key = pair.Key;
                short num = index;
                index = (short)(num + 1);
                dictionary2.Add(key, num);
            }
            Main.glowMaskTexture = masks.ToArray();
        }

        // Token: 0x0600009B RID: 155 RVA: 0x00007FF0 File Offset: 0x000061F0
        public static void Unload()
        {
            if (Main.glowMaskTexture != null)
            {
                Main.glowMaskTexture = Main.glowMaskTexture.Where(delegate (Texture2D tex)
                {
                    bool? flag;
                    if (tex == null)
                    {
                        flag = null;
                    }
                    else
                    {
                        string name = tex.Name;
                        flag = name != null ? new bool?(!name.StartsWith("SplitMod_")) : null;
                    }
                    return flag ?? true;
                }).ToArray();
            }
            Dictionary<string, short> dictionary = glowMasks;
            if (dictionary != null)
            {
                dictionary.Clear();
            }
            glowMasks = null;
        }

        // Token: 0x0400008D RID: 141
        private static Dictionary<string, short> glowMasks;
    }
}
