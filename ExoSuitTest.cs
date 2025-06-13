using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using Terraria;

using Microsoft.Xna.Framework;


namespace ExoSuitTest
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class ExoSuitTest : Mod
	{
        public static ModKeybind EnterExosuitKey;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                EnterExosuitKey = KeybindLoader.RegisterKeybind(this, "Enter Exosuit", "J");
            }

        }

        public override void Unload()
        {
            EnterExosuitKey = null;
        }

    }
}
