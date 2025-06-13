using System;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria;
using ExoSuitTest.Content.NPCs;
using Terraria.DataStructures;
using System.Security.Cryptography.X509Certificates;
using Terraria.Cinematics;
using Terraria.GameContent;

namespace ExoSuitTest.Content.Players
{
    public class ExosuitPlayer : ModPlayer
    {
        public bool inExosuit = false;
        public NPC ExosuitNPC;
        public bool cameraOverride = false;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (ExoSuitTest.EnterExosuitKey.JustPressed)
            {
                Main.NewText("It's pressed.", Color.Green);
                if (!inExosuit)
                {
                    const float enterRadius = 64f;

                    foreach (NPC npc in Main.npc)
                    {
                        Main.NewText("Just see if this is going.", Color.Orange);
                        if (npc.active && npc.type == ModContent.NPCType<ExosuitNPC>())
                        {
                            Main.NewText("Found Exosuit NPC.", Color.LightBlue);

                            float distance = Vector2.Distance(Player.Center, npc.Center);
                            Main.NewText($"Distance: {distance}", Color.Gray);

                            if (distance <= enterRadius)
                            {
                                Main.NewText("In Range.", Color.Green);
                                inExosuit = true;
                                ExosuitNPC = npc;
                                Player.Center = npc.Center;
                                SoundEngine.PlaySound(SoundID.Mech, Player.position);
                                break;
                            }
                        }

                    }
                }
                else
                {
                    inExosuit = false;
                    ExosuitNPC = null;
                    SoundEngine.PlaySound(SoundID.Mech, Player.position);
                }
            }
        }
        public override void PostUpdate()
        {
            if (inExosuit && ExosuitNPC != null && ExosuitNPC.active)
            {
                Vector2 visualOffset = new Vector2(0f, 6f);
                Vector2 exoOrigin = new Vector2(0f, 12f);
                Player.Center = ExosuitNPC.Center + new Vector2(5f, Player.height / 2f);
                Player.velocity = Vector2.Zero;
                Player.noItems = true;
                Player.width = 0;
                Player.height = 0;
                Player.gravity = 0;
                cameraOverride = !cameraOverride;
            }
        }


        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            if (inExosuit)
            {
                drawInfo.drawPlayer.invis = true;
            }
            if (inExosuit && ExosuitNPC != null && ExosuitNPC.active)
            {
                // Draw a semi-transparent red rectangle over the player's hitbox
                Rectangle playerHitbox = Player.Hitbox;

                Main.spriteBatch.Draw(
                    TextureAssets.MagicPixel.Value,
                    new Rectangle(
                        playerHitbox.X - (int)Main.screenPosition.X,
                        playerHitbox.Y - (int)Main.screenPosition.Y,
                        playerHitbox.Width,
                        playerHitbox.Height
                    ),
                    Color.Blue * 0.5f
                );

            }
        }

        

        public override void ResetEffects()
        {
            if (!inExosuit)
            {
                Player.noItems = false;
                Player.ghost = false;
            }
        }
    }
}
