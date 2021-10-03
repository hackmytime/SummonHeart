using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SummonHeart.ui.DinoUI;
using Terraria;
using Terraria.UI;

namespace SummonHeart.ui
{
    public class ModUIHandler
    {
        public void Load()
        {
            dinoLoader = new DinoUILoader();
            dinoLoader.Load();
        }

        public void UpdateUI(GameTime gameTime)
        {
            dinoLoader.UpdateUI(gameTime);
        }

        public void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            dinoLoader.ModifyInterfaceLayers(layers);
        }

        // Token: 0x0600045D RID: 1117 RVA: 0x00016B74 File Offset: 0x00014D74
        public void Unload()
        {
            DinoUILoader dinoUILoader = dinoLoader;
            if (dinoUILoader != null)
            {
                dinoUILoader.Unload();
            }
            dinoLoader = null;
        }


        // Token: 0x040000F9 RID: 249
        public static DinoUILoader dinoLoader;
    }
}
