﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;

namespace JoJoStands.Projectiles.PlayerStands.GratefulDead
{
    public class GratefulDeadStandT4 : StandClass
    {
        public override float shootSpeed => 16f;
        public bool grabFrames = false;
        public bool secondaryFrames = false;
        public bool activatedGas = false;
        public override float maxDistance => 98f;
        public override int punchDamage => 90;
        public override int punchTime => 10;
        public override int halfStandHeight => 34;
        public override float fistWhoAmI => 8f;
        public override float tierNumber => 1f;
        public override int standOffset => -4;
        public override int standType => 1;
        public override string poseSoundName => "OnceWeDecideToKillItsDone";
        public override string spawnSoundName => "The Grateful Dead";
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public int updateTimer = 0;


        public override void AI()
        {
            SelectAnimation();
            UpdateStandInfo();
            updateTimer++;
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            projectile.frameCounter++;
            if (modPlayer.StandOut)
            {
                projectile.timeLeft = 2;
            }
            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }

            if (!modPlayer.StandAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer && !secondaryFrames && !grabFrames)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (!attackFrames && !secondaryFrames && !grabFrames)
                {
                    StayBehind();
                }
                if (Main.mouseRight && projectile.owner == Main.myPlayer && shootCount <= 0 && !grabFrames)
                {
                    projectile.velocity = Main.MouseWorld - projectile.position;
                    projectile.velocity.Normalize();
                    projectile.velocity *= 5f;
                    float mouseDistance = Vector2.Distance(Main.MouseWorld, projectile.Center);
                    if (mouseDistance > 40f)
                    {
                        projectile.velocity = player.velocity + projectile.velocity;
                    }
                    if (mouseDistance <= 40f)
                    {
                        projectile.velocity = Vector2.Zero;
                    }
                    secondaryFrames = true;
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc.active)
                        {
                            if (projectile.Distance(npc.Center) <= 30f && !npc.boss && !npc.immortal && !npc.hide)
                            {
                                projectile.ai[0] = npc.whoAmI;
                                grabFrames = true;
                            }
                        }
                    }
                    LimitDistance();
                }
                if (grabFrames && projectile.ai[0] != -1f)
                {
                    projectile.velocity = Vector2.Zero;
                    NPC npc = Main.npc[(int)projectile.ai[0]];
                    npc.direction = -projectile.direction;
                    npc.position = projectile.position + new Vector2(5f * projectile.direction, -2f - npc.height / 3f);
                    npc.velocity = Vector2.Zero;
                    npc.AddBuff(mod.BuffType("Old2"), 2);
                    if (!npc.active)
                    {
                        grabFrames = false;
                        projectile.ai[0] = -1f;
                        shootCount += 30;
                    }
                    LimitDistance();
                }
                if (!Main.mouseRight && (grabFrames || secondaryFrames))
                {
                    grabFrames = false;
                    projectile.ai[0] = -1f;
                    shootCount += 30;
                    secondaryFrames = false;
                }
            }
            if (SpecialKeyPressed() && shootCount <= 0)
            {
                shootCount += 30;
                activatedGas = !activatedGas;
                if (activatedGas)
                {
                    Main.NewText("Gas Spread: On");
                }
                else
                {
                    Main.NewText("Gas Spread: Off");
                }
            }
            if (activatedGas)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active)
                    {
                        float distance = Vector2.Distance(player.Center, npc.Center);
                        if (distance < (30f * 16f) && !npc.immortal && !npc.hide)
                        {
                            npc.AddBuff(mod.BuffType("Old"), 2);
                        }
                    }
                }
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player otherPlayer = Main.player[i];
                    if (otherPlayer.active)
                    {
                        float distance = Vector2.Distance(player.Center, otherPlayer.Center);
                        if (distance < (30f * 16f) && otherPlayer.whoAmI != player.whoAmI)
                        {
                            otherPlayer.AddBuff(mod.BuffType("Old"), 2);
                        }
                    }
                }
                player.AddBuff(mod.BuffType("Old"), 2);
            }
            if (modPlayer.StandAutoMode)
            {
                BasicPunchAI();
            }
        }
        public override bool PreDrawExtras(SpriteBatch spriteBatch)
        {
            Player player = Main.player[projectile.owner];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            if (MyPlayer.RangeIndicators)
            {
                Texture2D texture = mod.GetTexture("Extras/RangeIndicator");
                spriteBatch.Draw(texture, player.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.Red * ((MyPlayer.RangeIndicatorAlpha * 3.9215f) / 1000f), 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), ((30f * 16f) + mPlayer.standRangeBoosts) / 160f, SpriteEffects.None, 0);
            }
            return base.PreDrawExtras(spriteBatch);
        }

        public override void SendExtraStates(BinaryWriter writer)
        {
            writer.Write(grabFrames);
        }

        public override void ReceiveExtraStates(BinaryReader reader)
        {
            grabFrames = reader.ReadBoolean();
        }


        public override void SelectAnimation()
        {
            if (attackFrames)
            {
                normalFrames = false;
                PlayAnimation("Attack");
            }
            if (normalFrames)
            {
                PlayAnimation("Idle");
            }
            if (secondaryFrames)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Secondary");
            }
            if (grabFrames)
            {
                normalFrames = false;
                attackFrames = false;
                secondaryFrames = false;
                PlayAnimation("Grab");

            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                PlayAnimation("Pose");
            }
        }

        public override void PlayAnimation(string animationName)
        {
            if (Main.netMode != NetmodeID.Server)
                standTexture = mod.GetTexture("Projectiles/PlayerStands/GratefulDead/GratefulDead_" + animationName);

            if (animationName == "Idle")
            {
                AnimationStates(animationName, 4, 12, true);
            }
            if (animationName == "Attack")
            {
                AnimationStates(animationName, 4, newPunchTime, true);
            }
            if (animationName == "Secondary")
            {
                AnimationStates(animationName, 1, 1, true);
            }
            if (animationName == "Grab")
            {
                AnimationStates(animationName, 3, 12, true, true, 2, 2);
            }
            if (animationName == "Pose")
            {
                AnimationStates(animationName, 1, 12, true);
            }
        }

    }
}