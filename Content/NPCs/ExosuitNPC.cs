using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.ModLoader.UI.ModBrowser;
using Terraria.GameContent;
using Terraria.DataStructures;
using ExoSuitTest.Content.Players;
using Mono.Cecil.Cil;

namespace ExoSuitTest.Content.NPCs
{
    public class ExosuitNPC : ModNPC
    {
        // Texture (we are assuming all parts are on the same sprite sheet)
        private Texture2D bodyTexture;
        private Texture2D legTexture;
        private Texture2D headTexture;

        // Define frame data
        public struct ExosuitFrameData
        {
            public Rectangle BodySource;
            public Rectangle FrontArmSource;
            public Rectangle BackArmSource;
            public Rectangle LegSource;
            public Rectangle HeadSource;
        }

        private Dictionary<string, ExosuitFrameData[]> ExosuitAnimations;

        // Animation control
        private string currentAnimation = "Idle";
        private int frameCounter;
        private int frameSpeed = 4; // Higher = slower animation
        private int currentFrame;
        private string previousAnimation;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1; // We control frames manually
        }

        public override void SetDefaults()
        {
            NPC.frame.Width = 40;
            NPC.frame.Height = 56;
            NPC.friendly = true;
            NPC.width = 35;
            NPC.height = 40;
            NPC.damage = 1;
            NPC.defense = 5;
            NPC.lifeMax = 1000;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = -1; // Custom AI



            headTexture = ModContent.Request<Texture2D>("ExoSuitTest/Content/Textures/NPCs/ExosuitDefaultHelmet_Head").Value;
            bodyTexture = ModContent.Request<Texture2D>("ExoSuitTest/Content/Textures/NPCs/ExosuitDefaultChest_Body").Value;
            legTexture = ModContent.Request<Texture2D>("ExoSuitTest/Content/Textures/NPCs/ExosuitDefaultLegs_Legs").Value;
            if (ExosuitAnimations == null)
            {
                headTexture = ModContent.Request<Texture2D>("ExoSuitTest/Content/Textures/NPCs/ExosuitDefaultHelmet_Head").Value;
                bodyTexture = ModContent.Request<Texture2D>("ExoSuitTest/Content/Textures/NPCs/ExosuitDefaultChest_Body").Value;
                legTexture = ModContent.Request<Texture2D>("ExoSuitTest/Content/Textures/NPCs/ExosuitDefaultLegs_Legs").Value;

                if (bodyTexture == null)
                {
                    Main.NewText("Error: bodyTexture failed to load!", Color.Red);
                }
                else
                {
                    Main.NewText("Success: bodyTexture loaded correctly!", Color.Green);
                }

                SetupAnimations();
            }
        }

        public override void Load()
        {
            if (Main.dedServ)
                return;

            // Load your custom texture
            bodyTexture = ModContent.Request<Texture2D>("ExoSuitTest/Content/Textures/NPCs/ExosuitDefaultChest_Body").Value;

            if (bodyTexture == null)
            {
                Main.NewText("Error: bodyTexture failed to load!", Color.Red);
            }
            else
            {
                Main.NewText("Success: bodyTexture loaded correctly!", Color.Green);
            }

            // Setup animation frames
            SetupAnimations();
        }

        private void SetupAnimations()
        {
            int frameWidth = 40; // <-- adjust based on your sprite size
            int frameHeight = 56;

            ExosuitAnimations = new Dictionary<string, ExosuitFrameData[]>();

            // Idle Animation
            ExosuitAnimations["Idle"] = new ExosuitFrameData[]
            {
            new ExosuitFrameData
            {
                BodySource = new Rectangle(0, 0, frameWidth, frameHeight), // first body
                FrontArmSource = new Rectangle(frameWidth * 2, 0, frameWidth, frameHeight), // front idle arm
                BackArmSource = new Rectangle(frameWidth * 2, frameHeight * 2, frameWidth, frameHeight), // back idle arm
                LegSource = new Rectangle(0,0, frameWidth, frameHeight),
                HeadSource = new Rectangle(0, 0, frameWidth, frameHeight),
            }
            };

            // Walking Animation (using frames 3,4,5,6)
            ExosuitAnimations["Walking"] = new ExosuitFrameData[]
            {
            new ExosuitFrameData
            {
                BodySource = new Rectangle(0, 0, frameWidth, frameHeight),
                FrontArmSource = new Rectangle(frameWidth * 3, frameHeight, frameWidth, frameHeight), // front walk 1
                BackArmSource = new Rectangle(frameWidth * 3, frameHeight * 3, frameWidth, frameHeight), // back walk 1
                LegSource = new Rectangle(0, frameHeight * 7, frameWidth, frameHeight),
                HeadSource = new Rectangle(0, frameHeight * 7, frameWidth, frameHeight),
            },
            new ExosuitFrameData
            {
                BodySource = new Rectangle(0, 0, frameWidth, frameHeight),
                FrontArmSource = new Rectangle(frameWidth * 4, frameHeight, frameWidth, frameHeight), // front walk 2
                BackArmSource = new Rectangle(frameWidth * 4, frameHeight * 3, frameWidth, frameHeight), // back walk 2
                LegSource = new Rectangle(0, frameHeight * 8, frameWidth, frameHeight),
                HeadSource = new Rectangle(0, frameHeight * 8, frameWidth, frameHeight),
            },
            new ExosuitFrameData
            {
                BodySource = new Rectangle(0, 0, frameWidth, frameHeight),
                FrontArmSource = new Rectangle(frameWidth * 5, frameHeight, frameWidth, frameHeight), // front walk 3
                BackArmSource = new Rectangle(frameWidth * 5, frameHeight * 3, frameWidth, frameHeight), // back walk 3
                LegSource = new Rectangle(0, frameHeight * 9, frameWidth, frameHeight),
                HeadSource = new Rectangle(0, frameHeight * 9, frameWidth, frameHeight),
            },
            new ExosuitFrameData
            {
                BodySource = new Rectangle(0, 0, frameWidth, frameHeight),
                FrontArmSource = new Rectangle(frameWidth * 6, frameHeight, frameWidth, frameHeight), // front walk 4
                BackArmSource = new Rectangle(frameWidth * 6, frameHeight * 3, frameWidth, frameHeight), // back walk 4
                LegSource = new Rectangle(0, frameHeight * 10, frameWidth, frameHeight),
                HeadSource = new Rectangle(0, frameHeight * 10, frameWidth, frameHeight),
            },
            new ExosuitFrameData
            {
                BodySource = new Rectangle(0, 0, frameWidth, frameHeight),
                FrontArmSource = new Rectangle(frameWidth * 3, frameHeight, frameWidth, frameHeight), // front walk 1
                BackArmSource = new Rectangle(frameWidth * 3, frameHeight * 3, frameWidth, frameHeight), // back walk 1
                LegSource = new Rectangle(0, frameHeight * 11, frameWidth, frameHeight),
                HeadSource = new Rectangle(0, frameHeight * 11, frameWidth, frameHeight),
            },
            new ExosuitFrameData
            {
                BodySource = new Rectangle(0, 0, frameWidth, frameHeight),
                FrontArmSource = new Rectangle(frameWidth * 4, frameHeight, frameWidth, frameHeight), // front walk 2
                BackArmSource = new Rectangle(frameWidth * 4, frameHeight * 3, frameWidth, frameHeight), // back walk 2
                LegSource = new Rectangle(0, frameHeight * 12, frameWidth, frameHeight),
                HeadSource = new Rectangle(0, frameHeight * 12, frameWidth, frameHeight),
            },
            new ExosuitFrameData
            {
                BodySource = new Rectangle(0, 0, frameWidth, frameHeight),
                FrontArmSource = new Rectangle(frameWidth * 5, frameHeight, frameWidth, frameHeight), // front walk 3
                BackArmSource = new Rectangle(frameWidth * 5, frameHeight * 3, frameWidth, frameHeight), // back walk 3
                LegSource = new Rectangle(0, frameHeight * 13, frameWidth, frameHeight),
                HeadSource = new Rectangle(0, frameHeight * 13, frameWidth, frameHeight),
            },
            new ExosuitFrameData
            {
                BodySource = new Rectangle(0, 0, frameWidth, frameHeight),
                FrontArmSource = new Rectangle(frameWidth * 6, frameHeight, frameWidth, frameHeight), // front walk 4
                BackArmSource = new Rectangle(frameWidth * 6, frameHeight * 3, frameWidth, frameHeight), // back walk 4
                LegSource = new Rectangle(0, frameHeight * 14, frameWidth, frameHeight),
                HeadSource = new Rectangle(0, frameHeight * 14, frameWidth, frameHeight),
            },
            new ExosuitFrameData
            {
                BodySource = new Rectangle(0, 0, frameWidth, frameHeight),
                FrontArmSource = new Rectangle(frameWidth * 5, frameHeight, frameWidth, frameHeight), // front walk 3
                BackArmSource = new Rectangle(frameWidth * 5, frameHeight * 3, frameWidth, frameHeight), // back walk 3
                LegSource = new Rectangle(0, frameHeight * 15, frameWidth, frameHeight),
                HeadSource = new Rectangle(0, frameHeight * 15, frameWidth, frameHeight),
            },
            new ExosuitFrameData
            {
                BodySource = new Rectangle(0, 0, frameWidth, frameHeight),
                FrontArmSource = new Rectangle(frameWidth * 6, frameHeight, frameWidth, frameHeight), // front walk 4
                BackArmSource = new Rectangle(frameWidth * 6, frameHeight * 3, frameWidth, frameHeight), // back walk 4
                LegSource = new Rectangle(0, frameHeight * 16, frameWidth, frameHeight),
                HeadSource = new Rectangle(0, frameHeight * 16, frameWidth, frameHeight),
            },
            new ExosuitFrameData
            {
                BodySource = new Rectangle(0, 0, frameWidth, frameHeight),
                FrontArmSource = new Rectangle(frameWidth * 3, frameHeight, frameWidth, frameHeight), // front walk 1
                BackArmSource = new Rectangle(frameWidth * 3, frameHeight * 3, frameWidth, frameHeight), // back walk 1
                LegSource = new Rectangle(0, frameHeight * 17, frameWidth, frameHeight),
                HeadSource = new Rectangle(0, frameHeight * 17, frameWidth, frameHeight),
            },
            new ExosuitFrameData
            {
                BodySource = new Rectangle(0, 0, frameWidth, frameHeight),
                FrontArmSource = new Rectangle(frameWidth * 4, frameHeight, frameWidth, frameHeight), // front walk 2
                BackArmSource = new Rectangle(frameWidth * 4, frameHeight * 3, frameWidth, frameHeight), // back walk 2
                LegSource = new Rectangle(0, frameHeight * 18, frameWidth, frameHeight),
                HeadSource = new Rectangle(0, frameHeight * 18, frameWidth, frameHeight),
            },
            new ExosuitFrameData
            {
                BodySource = new Rectangle(0, 0, frameWidth, frameHeight),
                FrontArmSource = new Rectangle(frameWidth * 5, frameHeight, frameWidth, frameHeight), // front walk 3
                BackArmSource = new Rectangle(frameWidth * 5, frameHeight * 3, frameWidth, frameHeight), // back walk 3
                LegSource = new Rectangle(0, frameHeight * 19, frameWidth, frameHeight),
                HeadSource = new Rectangle(0, frameHeight * 19, frameWidth, frameHeight),
            },
            };

            ExosuitAnimations["Jumping/Falling"] = new ExosuitFrameData[]
            {
                new ExosuitFrameData
                {
                    BodySource = new Rectangle(0, 0, frameWidth, frameHeight),
                    FrontArmSource = new Rectangle(frameWidth * 2, frameHeight, frameWidth, frameHeight),
                    BackArmSource = new Rectangle(frameWidth * 2, frameHeight * 3, frameWidth, frameHeight),
                    LegSource = new Rectangle(0, frameHeight * 5, frameWidth, frameHeight),
                    HeadSource = new Rectangle(0, frameHeight * 5, frameWidth, frameHeight),
                },
            };
        }
        

        public override void AI()
        {
            NPC.TargetClosest();
            NPC.velocity.X = 0;
            if (!NPC.collideY)
            {
                NPC.velocity.Y += 0.3f;
                if (NPC.velocity.Y > 3f)
                    NPC.velocity.Y = 3f;
            }
            else
            {
                NPC.velocity.Y = 0f;
            }

            //tracking for player in suit
            //player control
            if (Main.myPlayer != Main.LocalPlayer.whoAmI) return;

            Player player = Main.LocalPlayer;
            var modPlayer = player.GetModPlayer<ExosuitPlayer>();

            if (modPlayer.inExosuit)
            {
                if (player.controlLeft)
                {
                    currentAnimation = "Walking";
                    NPC.velocity.X = -3f;

                }
                else if (player.controlRight)
                {
                    currentAnimation = "Walking";
                    NPC.velocity.X = +3f;

                }
                if (player.controlJump && NPC.velocity.Y == 0)
                {
                    currentAnimation = "Jumping/Falling";
                    NPC.velocity.Y = -6f;
                }
            }




            //will need to take out this block of code for animations to work for player control
            if (NPC.HasValidTarget && NPC.Distance(Main.player[NPC.target].Center) >= 200f)
            {
                // Move towards player
                if (NPC.Center.X < Main.player[NPC.target].Center.X)
                    NPC.velocity.X = 3f;
                else
                    NPC.velocity.X = -3f;

                currentAnimation = "Walking";
            }
            else
            {
                currentAnimation = "Idle";
            }
            //this block up


            Vector2 ahead = NPC.position + new Vector2(NPC.velocity.X * 10, 0);
            bool hittingWall = Collision.SolidCollision(ahead, NPC.width, NPC.height);

            if (hittingWall)
            {
                NPC.velocity.Y = -6f;
                currentAnimation = "Jumping/Falling";
            }
            
            if (NPC.velocity.Y != 0)
            {
                currentAnimation = "Jumping/Falling";
            }



            if (currentAnimation != previousAnimation)
            {
                currentFrame = 0;
                frameCounter = 0;
                previousAnimation = currentAnimation;
            }

            // Animate
            frameCounter++;
            if (frameCounter >= frameSpeed)
            {
                frameCounter = 0;
                currentFrame++;
                if (currentFrame >= ExosuitAnimations[currentAnimation].Length)
                    currentFrame = 0;
            }

            if (NPC.velocity.X > 0)
            {
                NPC.spriteDirection = 1;
            }
            else if (NPC.velocity.X < 0)
            {
                NPC.spriteDirection = -1;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (bodyTexture == null)
                return true;

            if (!ExosuitAnimations.ContainsKey(currentAnimation))
            {
                Main.NewText($"Missing animation key: {currentAnimation}", Color.Red);
                return true;
            }

            var animationFrames = ExosuitAnimations[currentAnimation];
            if (currentFrame < 0 || currentFrame >= animationFrames.Length)
            {
                Main.NewText($"Invalid frame index {currentFrame} for animation '{currentAnimation}' with length {animationFrames.Length}", Color.Red);
                currentFrame = 0; // Optional: reset or clamp
                return true;
            }

            var frameData = animationFrames[currentFrame];

            //var frameData = ExosuitAnimations[currentAnimation][currentFrame];
            Vector2 drawPos = NPC.Center - screenPos;
            SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            float rotation = 0f;
            Vector2 origin = new Vector2(frameData.BodySource.Width / 2f, frameData.BodySource.Height / 2f);
            float scale = 1f;

            Vector2 offset = new Vector2(0f, -6f);
            Vector2 backArmOffset = new Vector2(0f, 8f);
            Vector2 headOffset = new Vector2(0f, 1f);

            // Draw order: Back Arm → Body → Front Arm
            spriteBatch.Draw(legTexture, drawPos + offset, frameData.LegSource, drawColor, rotation, origin, scale, effects, 0f);
            spriteBatch.Draw(bodyTexture, drawPos + offset, frameData.BackArmSource, drawColor, rotation, origin, scale, effects, 0f);
            spriteBatch.Draw(bodyTexture, drawPos + offset, frameData.BodySource, drawColor, rotation, origin, scale, effects, 0f);
            spriteBatch.Draw(headTexture, drawPos + headOffset + offset, frameData.HeadSource, drawColor, rotation, origin, scale, effects, 0f);
            spriteBatch.Draw(bodyTexture, drawPos + offset, frameData.FrontArmSource, drawColor, rotation, origin, scale, effects, 0f);


            spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(
                (int)Entity.position.X - (int)screenPos.X,
                (int)Entity.position.Y - (int)screenPos.Y,
                Entity.width,
                Entity.height
                ), Color.Red * 0.5f);


            return false; // We handled drawing
        }

    }
}
