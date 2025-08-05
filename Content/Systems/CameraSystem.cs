using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ExoSuitTest.Content.Players;
using Terraria.ModLoader;

namespace ExoSuitTest.Content.Systems
{
    internal class CameraSystem : ModSystem
    {

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            Player player = Main.LocalPlayer;
            var modPlayer = player.GetModPlayer<ExosuitPlayer>();

            

            if (modPlayer.cameraOverride)
            {
               



            }
        }
    }
}
