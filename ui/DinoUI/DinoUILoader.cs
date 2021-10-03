using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;

namespace SummonHeart.ui.DinoUI
{
    public class DinoUILoader
    {
        public void Load()
        {
            if (!Main.dedServ)
            {
                dinoInterface = new UserInterface();
                dinoUIstate = new DinoUI();
                dinoUIstate.Activate();
                dinoInterface.SetState(dinoUIstate);
            }
        }

        public void UpdateUI(GameTime gameTime)
        {
            if (!Main.gameMenu && dinoUIstate.visible)
            {
                UserInterface userInterface = dinoInterface;
                if (userInterface == null)
                {
                    return;
                }
                userInterface.Update(gameTime);
            }
        }

        public void Unload()
        {
            dinoUIstate.UnloadUI();
        }

        public void ShowUI(Vector2 tileCenter)
        {
            dinoUIstate.visible = true;
            dinoUIstate.tileCenter = tileCenter;
            dinoInterface.SetState(dinoUIstate);
        }

        public void HideUI()
        {
            dinoUIstate.visible = false;
            dinoUIstate.PutSlotItemsInInventory();
            dinoUIstate.successBar.SuccessChance = 0f;
            dinoUIstate.successBar.FossilValue = 0f;
            dinoInterface.SetState(dinoUIstate);
        }

        public void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex((layer) => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("Drones: Drone Editor", delegate ()
                {
                    if (dinoUIstate.visible)
                    {
                        dinoInterface.Draw(Main.spriteBatch, new GameTime());
                    }
                    return true;
                }, (InterfaceScaleType)1));
            }
        }

        internal UserInterface dinoInterface;

        internal DinoUI dinoUIstate;
    }
}
