using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.UI.Chat;

namespace SummonHeart.Extensions.TurretSystem
{
    public class TurretPlayer : ModPlayer
    {
        public override void PreUpdate()
        {
            if (Main.dedServ || Main.myPlayer != player.whoAmI)
            {
                return;
            }
            Vector2 mousePos = Main.MouseWorld;
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (!Main.blockMouse && !Main.LocalPlayer.mouseInterface && mousePos.Between(npc.TopLeft, npc.BottomRight))
                {
                    ModNPC modNPC = npc.modNPC;
                    if (modNPC != null && npc.active && typeof(TurretHead).IsAssignableFrom(modNPC.GetType()))
                    {
                        overrideTooltipString = ((TurretHead)modNPC).getHoverTooltip();
                        return;
                    }
                }
            }
            overrideTooltipString = null;
        }

        public static void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex((layer) => layer.Name.Equals("Vanilla: Mouse Text"));
            if (index != -1 && !string.IsNullOrEmpty(overrideTooltipString))
            {
                layers.Find((x) => x.Name == "Vanilla: Mouse Over").Active = false;
                layers.Insert(index, new LegacyGameInterfaceLayer("Paradigm: Mouse Text", delegate ()
                {
                    TextSnippet[] text = ChatManager.ParseMessage(overrideTooltipString, Color.White).ToArray();
                    float x = ChatManager.GetStringSize(Main.fontMouseText, text, Vector2.One, -1f).X;
                    Vector2 pos = Main.MouseScreen + new Vector2(12f, 28f);
                    if (pos.Y > (float)(Main.screenHeight - 30))
                    {
                        pos.Y = (float)(Main.screenHeight - 30);
                    }
                    if (pos.X > (float)Main.screenWidth - x)
                    {
                        pos.X = (float)Main.screenWidth - x;
                    }
                    int hoveredSnippet;
                    ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, text, pos, 0f, Vector2.Zero, Vector2.One, out hoveredSnippet, -1f, 2f);
                    return true;
                }, (InterfaceScaleType)1));
            }
        }

       /* public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (ParadigmMod.DrawTurretAngles.JustPressed)
            {
                drawTurretAngles = !drawTurretAngles;
            }
        }*/

        // Token: 0x06000069 RID: 105 RVA: 0x000041A8 File Offset: 0x000023A8
        public static void Unload()
        {
            overrideTooltipString = null;
        }

        // Token: 0x0600006A RID: 106 RVA: 0x000041B0 File Offset: 0x000023B0
        public override TagCompound Save()
        {
            TagCompound tagCompound = new TagCompound();
            tagCompound["drawTurretAngles"] = drawTurretAngles;
            return tagCompound;
        }

        // Token: 0x0600006B RID: 107 RVA: 0x000041CD File Offset: 0x000023CD
        public override void Load(TagCompound tag)
        {
            drawTurretAngles = tag.GetBool("drawTurretAngles");
        }

        // Token: 0x04000036 RID: 54
        public static string overrideTooltipString;

        // Token: 0x04000037 RID: 55
        public bool drawTurretAngles;
    }
}
