using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExoSuitTest.Content.NPCs
{
    public class TestNPC : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 20;
            NPC.height = 20;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = 10;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            return true;
        }

        public override string GetChat()
        {
            return "Hello, I'm A Test!";
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Hi";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            Main.npcChatText = "Nice To Meet You!";
        }
    }
}
