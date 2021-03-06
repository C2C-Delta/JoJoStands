using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.NPCs
{
    public class JoJoGlobalNPC : GlobalNPC
    {
        public bool frozenInTime = false;
        public bool affectedbyBtz = false;
        public bool taggedByButterfly = false;
        public bool applyingForesightPositions = false;
        public bool foresightResetIndex = false;
        public bool taggedWithPhantomMarker = false;
        public bool grabbedByHermitPurple = false;
        public int foresightSaveTimer = 0;
        public int foresightPositionIndex = 0;
        public int foresightPositionIndexMax = 0;
        public int BtZSaveTimer = 0;
        public int aiStyleSave = 0;
        public int lifeRegenIncrement = 0;
        public int lockRegenCounter = 0;
        public bool forceDeath = false;
        public int indexPosition = 0;
        public bool spawnedByDeathLoop = false;
        public int deathTimer = 0;
        public float kingCrimsonDonutMultiplier = 1f;
        public Vector2 playerPositionOnSkip = Vector2.Zero;
        public Vector2[] BtZPositions = new Vector2[400];
        public Vector2[] foresightPosition = new Vector2[400];
        public Rectangle[] foresightFrames = new Rectangle[400];
        public Vector2[] foresightRotations = new Vector2[400];      //although this is a Vector2, I'm storing rotations in X and Direction in Y

        public override bool InstancePerEntity
        {
            get { return true; }
        }

        public override void NPCLoot(NPC npc)
        {
            Player player = Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)];
            if (Main.hardMode && (player.position.Y < (Main.worldSurface * 0.35) * 16f) && Main.rand.NextFloat(0, 101) < 7f)
            {
                Item.NewItem(npc.getRect(), mod.ItemType("SoulofTime"), Main.rand.Next(1, 3));      //mininum amount = 1, maximum amount = 3
            }
            if (Main.dayTime && Main.rand.NextFloat(0, 101) < 10f)
            {
                Item.NewItem(npc.getRect(), mod.ItemType("SunDroplet"), Main.rand.Next(1, 3));
            }
            if (Main.hardMode && (npc.type == NPCID.Zombie || npc.type == NPCID.GoblinArcher || npc.type == NPCID.GoblinPeon || npc.type == NPCID.GoblinScout || npc.type == NPCID.GoblinSorcerer || npc.type == NPCID.GoblinSummoner || npc.type == NPCID.GoblinThief || npc.type == NPCID.GoblinTinkerer || npc.type == NPCID.GoblinWarrior || npc.townNPC) && Main.rand.NextFloat(0, 101) <= 4f)
            {
                Item.NewItem(npc.getRect(), mod.ItemType("Hand"));
            }
            if (player.ZoneOverworldHeight && !player.ZoneCorrupt && !player.ZoneCrimson && !player.ZoneDungeon && !player.ZoneJungle && !player.ZoneSnow && !player.ZoneDesert && !player.ZoneBeach && !player.ZoneHoly)
            {
                if (Main.dayTime && Main.rand.NextFloat(0, 101) <= 4f)
                {
                    Item.NewItem(npc.getRect(), mod.ItemType("WillToFight"));
                }
                if (!Main.dayTime && Main.rand.NextFloat(0, 101) <= 4f)
                {
                    Item.NewItem(npc.getRect(), mod.ItemType("WillToProtect"));
                }
            }
            if (player.ZoneUnderworldHeight && Main.rand.NextFloat(0, 101) <= 4f)
            {
                Item.NewItem(npc.getRect(), mod.ItemType("WillToDestroy"));
            }
            if ((player.ZoneCorrupt || player.ZoneCrimson) && Main.rand.NextFloat(0, 101) <= 4f)
            {
                Item.NewItem(npc.getRect(), mod.ItemType("WillToControl"));
            }
            if (player.ZoneDungeon && Main.rand.NextFloat(0, 101) <= 4f)
            {
                Item.NewItem(npc.getRect(), mod.ItemType("WillToEscape"));
            }
            if (player.ZoneJungle && Main.rand.NextFloat(0, 101) <= 4f)
            {
                Item.NewItem(npc.getRect(), mod.ItemType("WillToChange"));
            }
            if (npc.type == NPCID.WallofFlesh)
            {
                if (Main.rand.NextFloat(0, 101) < 50f)
                {
                    Item.NewItem(npc.getRect(), mod.ItemType("StandEmblem"));
                }
                else
                {
                    Item.NewItem(npc.getRect(), mod.ItemType("HamonEmblem"));
                }
            }
        }

        public override bool SpecialNPCLoot(NPC npc)
        {
            if (taggedByButterfly)      //increases the drop chances of loot by calling it again when called, cause it's gonna normally call NPCLoot and call it again here
            {
                npc.NPCLoot();
                npc.value = 0;
            }
            return base.SpecialNPCLoot(npc);
        }

        public override void GetChat(NPC npc, ref string chat)
        {
            if (MyPlayer.SecretReferences)
            {
                if (npc.type == mod.NPCType("MarineBiologist") && Main.rand.Next(0, 101) <= 3)      //Placement contributor reference
                {
                    chat = "I knew a guy who loved to name things. He�s still around, and he�s probably still naming everything he can find. I wonder what kind of Placement he went through in the dimension shift...";
                }
                if (npc.type == mod.NPCType("MarineBiologist") && Main.rand.Next(0, 101) <= 3)      //Nekro contributor reference
                {
                    chat = "There was a man with splt personalities named Nekro and Sektor, they named about 10 of the stands you have access to, kinda reminds me of a friend from Egypt...";
                }
                if (npc.type == mod.NPCType("MarineBiologist") && Main.rand.Next(0, 101) <= 5)      //Techno contributor reference
                {
                    chat = "Some weirdo with an afro once zoomed past me at the speed of a train, with his Stand carrying him in a box. Gramps seemed to approve. Who knows where that lunatic is now.";
                }
                if (npc.type == NPCID.Nurse && Main.rand.Next(0, 100) <= 5)     //ciocollata reference
                {
                    chat = "I heard there was a surgeon fired for killing patients and recording it, there are some sick people in this world.";
                }
                if (npc.type == NPCID.Demolitionist && Main.rand.Next(0, 100) <= 10)        //obviously, a Killer Queen reference
                {
                    chat = Main.LocalPlayer.name + " do you know what a 'Killer Queen' is? I heard it can make anything explode...";
                }
                if (npc.type == NPCID.Guide && Main.rand.Next(0, 100) <= 4)     //Betty contributor reference
                {
                    chat = "Hey " + Main.LocalPlayer.name + ", the other day one small girl calling herself 'The Dead Princess' came to me asking for a new name to her list... I'm not sure what was she talking about...";
                }
                if (npc.type == NPCID.Mechanic && Main.rand.Next(0, 100) <= 5)      //Phil contributer reference
                {
                    chat = "I've heard of someone named Phil selling something called 'Flex Tape' that can fix everything, mind if you get some for me?";
                }
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Painter)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("IWouldntLose"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("OfficersRegret"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("QuietLife"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("ShotintheDark"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("BloodForTheKing"));
                nextSlot++;
            }
            if (type == NPCID.TravellingMerchant && ((Main.hardMode && Main.rand.Next(0, 101) >= 90) || NPC.downedPlantBoss))
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("ViralPearlRing"));
            }
        }

        public override bool PreAI(NPC npc)
        {
            MyPlayer player = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
            if (player.TheWorldEffect || frozenInTime)
            {
                npc.velocity.X *= 0f;
                npc.velocity.Y *= 0f;               //negative X is to the left, negative Y is UP
                npc.frameCounter = 1;
                if (!npc.noGravity)
                {
                    npc.velocity.Y -= 0.3f;     //the default gravity value, so that if enemies have gravity enabled, this velocity counters that gravity
                }
                npc.netUpdate = true;
                return false;
            }
            if (player.BackToZero)
            {
                if (!affectedbyBtz)
                {
                    BtZSaveTimer--;
                    if (BtZSaveTimer <= 0)
                    {
                        if (indexPosition < 999)
                        {
                            indexPosition += 1;
                            BtZPositions[indexPosition] = npc.position;
                            BtZSaveTimer = 30;
                        }
                    }
                }
                if (affectedbyBtz)
                {
                    BtZSaveTimer--;
                    if (BtZSaveTimer <= 0)
                    {
                        if (indexPosition > 1)
                        {
                            indexPosition -= 1;
                            npc.position = BtZPositions[indexPosition];
                            if (npc.position == BtZPositions[indexPosition])
                            {
                                System.Array.Clear(BtZPositions, indexPosition, 1);
                            }
                            BtZSaveTimer = 5;
                        }
                        npc.netUpdate = true;
                    }
                    npc.velocity = Vector2.Zero;
                    npc.AddBuff(BuffID.Confused, 600);
                    return false;
                }
            }
            if (!player.BackToZero)
            {
                if (npc.HasBuff(mod.BuffType("AffectedByBtZ")))
                {
                    int buffIndex = npc.FindBuffIndex(mod.BuffType("AffectedByBtZ"));
                    npc.DelBuff(buffIndex);
                }
                BtZSaveTimer = 0;
                indexPosition = 0;
                affectedbyBtz = false;
            }
            if (player.TimeSkipPreEffect)
            {
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    if (Main.player[i].active && playerPositionOnSkip == Vector2.Zero && Main.player[i].GetModPlayer<MyPlayer>().TimeSkipPreEffect)
                    {
                        playerPositionOnSkip = Main.player[i].position;
                    }
                }
            }
            if (player.TimeSkipEffect && !npc.townNPC && !npc.friendly && !npc.boss)
            {
                if (playerPositionOnSkip == Vector2.Zero)
                {
                    playerPositionOnSkip = Main.player[Buffs.ItemBuff.PreTimeSkip.userIndex].position;
                }
                if (aiStyleSave == 0 && npc.aiStyle != 0)
                {
                    aiStyleSave = npc.aiStyle;
                    npc.aiStyle = 0;
                }
                if (npc.aiStyle == 0)
                {
                    npc.velocity /= 2;
                    if (npc.direction == -1)
                    {
                        npc.spriteDirection = 1;
                    }
                    if (npc.direction == 1)
                    {
                        npc.spriteDirection = -1;
                    }
                    if (playerPositionOnSkip.X > npc.position.X)
                    {
                        npc.velocity.X = 1f;
                        npc.direction = 1;
                    }
                    if (playerPositionOnSkip.X < npc.position.X)
                    {
                        npc.velocity.X = -1f;
                        npc.direction = -1;
                    }
                    if (npc.noGravity)
                    {
                        if (playerPositionOnSkip.Y > npc.position.Y)
                        {
                            npc.velocity.X = -1f;
                        }
                        if (playerPositionOnSkip.Y < npc.position.Y)
                        {
                            npc.velocity.X = 1f;
                        }
                    }
                    if (!npc.noGravity)
                    {
                        int num17 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                        int num18 = (int)((npc.position.Y + (float)npc.height - 16f) / 16f);
                        if (WorldGen.SolidTile((int)(npc.Center.X / 16f) + npc.direction, (int)(npc.Center.Y / 16f)) && !Collision.SolidTilesVersatile(num17 - npc.direction * 2, num17 - npc.direction, num18 - 5, num18 - 1) && !Collision.SolidTiles(num17, num17, num18 - 5, num18 - 3) && npc.ai[1] == 0f)
                        {
                            npc.velocity.Y = -6f;
                            npc.netUpdate = true;
                        }
                    }
                }
            }
            if (player.TimeSkipEffect && npc.boss)
            {
                npc.defense /= 2;
            }
            if (!player.TimeSkipEffect && npc.aiStyle == 0)
            {
                playerPositionOnSkip = Vector2.Zero;
                npc.aiStyle = aiStyleSave;
                aiStyleSave = 0;
            }
            if (player.Foresight && !npc.immortal)
            {
                applyingForesightPositions = true;
                if (foresightSaveTimer > 0)
                {
                    foresightSaveTimer--;
                }
                if (foresightSaveTimer <= 1)
                {
                    foresightPosition[foresightPositionIndex] = npc.position;
                    foresightFrames[foresightPositionIndex] = npc.frame;
                    foresightRotations[foresightPositionIndex].X = npc.rotation;
                    foresightRotations[foresightPositionIndex].Y = npc.direction;
                    foresightPositionIndex++;       //second so that something saves in [0] and goes up from there
                    foresightPositionIndexMax++;
                    foresightSaveTimer = 5;
                }
            }
            if (!player.Foresight && applyingForesightPositions)
            {
                if (!foresightResetIndex)
                {
                    foresightPositionIndex = 0;
                    foresightResetIndex = true;
                }
                npc.velocity = Vector2.Zero;
                npc.position = foresightPosition[foresightPositionIndex];
                npc.frame = foresightFrames[foresightPositionIndex];
                npc.rotation = foresightRotations[foresightPositionIndex].X;
                npc.direction = (int)foresightRotations[foresightPositionIndex].Y;
                if (foresightSaveTimer > 0)
                {
                    foresightSaveTimer--;
                }
                if (foresightSaveTimer <= 1)
                {
                    foresightPositionIndex++;
                    foresightSaveTimer = 5;
                    if (foresightPositionIndex >= 1)
                    {
                        if (foresightPosition[foresightPositionIndex - 1] != Vector2.Zero)
                        {
                            foresightPosition[foresightPositionIndex - 1] = Vector2.Zero;
                        }
                        if (foresightRotations[foresightPositionIndex - 1].X != 0f)
                        {
                            foresightRotations[foresightPositionIndex - 1].X = 0f;
                        }
                        if (foresightRotations[foresightPositionIndex - 1].Y != 0f)
                        {
                            foresightRotations[foresightPositionIndex - 1].Y = 0f;
                        }
                    }
                }
                if (foresightPositionIndex >= foresightPositionIndexMax)
                {
                    applyingForesightPositions = false;
                    foresightPositionIndex = 0;
                    foresightPositionIndexMax = 0;
                    foresightResetIndex = false;
                }
                /*if (foresightPositionIndex >= 49)       //a failsafe to prevent Index Out of Bounds in extended multiplayer timeskips
                {
                    foresightPositionIndex = 0;
                    foresightPositionIndexMax = 0;
                }*/
                return false;
            }
            if (spawnedByDeathLoop)
            {
                deathTimer++;
                if ((deathTimer >= 30 && Buffs.ItemBuff.DeathLoop.Looping10x) || (deathTimer >= 60 && Buffs.ItemBuff.DeathLoop.Looping3x))
                {
                    if (npc.immortal || npc.hide)
                    {
                        npc.StrikeNPCNoInteraction(999999999, 0f, 1, false, true, false);
                    }
                    if (!npc.immortal)
                    {
                        npc.StrikeNPCNoInteraction(npc.lifeMax + 10, 0f, 1, false, true, false);
                    }
                    deathTimer = 0;
                }
            }
            if (npc.HasBuff(mod.BuffType("Locked")))
            {
                npc.lifeRegen = -4;
                npc.velocity *= 0.95f;
                lockRegenCounter++;
                npc.defense = (int)(npc.defense * 0.95);
                if (lockRegenCounter >= 60)    //increases lifeRegen damage every second
                {
                    lockRegenCounter = 0;
                    lifeRegenIncrement += 2;
                    npc.StrikeNPC(lifeRegenIncrement, 0f, 1);
                }
            }

            if (npc.HasBuff(mod.BuffType("RedBindDebuff")))
            {
                npc.velocity.X = 0f;
                if (npc.velocity.Y > 8f)
                    npc.velocity.Y += 0.3f;
                return false;
            }
            if (npc.HasBuff(mod.BuffType("Stolen")))
                return false;
            if (grabbedByHermitPurple)
                return false;
            return true;
        }

        public override bool CheckDead(NPC npc)
        {
            for (int i = 0; i < 255; i++)
            {
                Player player = Main.player[i];
                if (player.active && npc.boss && player.GetModPlayer<MyPlayer>().DeathLoop && Buffs.ItemBuff.DeathLoop.LoopNPC == 0)
                {
                    Buffs.ItemBuff.DeathLoop.LoopNPC = npc.type;
                    Buffs.ItemBuff.DeathLoop.deathPositionX = npc.position.X;
                    Buffs.ItemBuff.DeathLoop.deathPositionY = npc.position.Y;
                    Buffs.ItemBuff.DeathLoop.Looping3x = true;
                    Buffs.ItemBuff.DeathLoop.Looping10x = false;
                }
                if (player.active && !npc.boss && player.GetModPlayer<MyPlayer>().DeathLoop && Buffs.ItemBuff.DeathLoop.LoopNPC == 0 && !npc.friendly && npc.lifeMax > 5)
                {
                    Buffs.ItemBuff.DeathLoop.LoopNPC = npc.type;
                    Buffs.ItemBuff.DeathLoop.deathPositionX = npc.position.X;
                    Buffs.ItemBuff.DeathLoop.deathPositionY = npc.position.Y;
                    Buffs.ItemBuff.DeathLoop.Looping3x = false;
                    Buffs.ItemBuff.DeathLoop.Looping10x = true;
                }
            }
            return base.CheckDead(npc);
        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            MyPlayer player = Main.LocalPlayer.GetModPlayer<MyPlayer>();
            if (player.Foresight || applyingForesightPositions)
            {
                for (int i = 0; i < foresightPositionIndexMax; i++)
                {
                    SpriteEffects effects = SpriteEffects.None;
                    if (foresightRotations[i].Y == 1)
                    {
                        effects = SpriteEffects.FlipHorizontally;
                    }
                    if (foresightPosition[i] != Vector2.Zero)
                    {
                        spriteBatch.Draw(Main.npcTexture[npc.type], (foresightPosition[i] - Main.screenPosition) + new Vector2(-5f, 30f), foresightFrames[i], Color.DarkRed, foresightRotations[i].X, foresightFrames[i].Size() / 2f, npc.scale, effects, 0f);
                    }
                }
            }
            return base.PreDraw(npc, spriteBatch, drawColor);
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
        {
            if (npc.HasBuff(mod.BuffType("RedBindDebuff")))
            {
                Texture2D texture = mod.GetTexture("Extras/BoundByRedBind");
                spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, Color.White, npc.rotation, npc.Size / 2f, npc.scale, SpriteEffects.None, 0f);
            }
        }

        public override void OnHitPlayer(NPC npc, Player target, int damage, bool crit)
        {
            MyPlayer mPlayer = target.GetModPlayer<MyPlayer>();
            int standSlotType = mPlayer.StandSlot.Item.type;
            if (target.HasBuff(mod.BuffType("BacktoZero")))     //only affects the ones with the buff, everyone's bool should turn on and save positions normally
            {
                npc.AddBuff(mod.BuffType("AffectedByBtZ"), 2);
                npc.StrikeNPC(damage, npc.knockBackResist, -npc.direction);
            }
            if (mPlayer.StandSlot.Item.type == mod.ItemType("DollyDaggerT1"))
            {
                npc.StrikeNPC((int)(npc.damage * 0.35f), 4f, -npc.direction);       //Dolly dagger is reflecting 35% of damage here, 70% in tier 2
            }
            if (mPlayer.StandSlot.Item.type == mod.ItemType("DollyDaggerT2"))
            {
                npc.StrikeNPC((int)(npc.damage * 0.7f), 6f, -npc.direction);
            }
            if (target.GetModPlayer<MyPlayer>().Vampire)
            {
                npc.AddBuff(BuffID.Frostburn, 240);
            }
            if (npc.boss)
            {
                if (standSlotType == mod.ItemType("LockT1"))
                {
                    npc.AddBuff(mod.BuffType("Locked"), 3 * 60);
                }
                if (standSlotType == mod.ItemType("LockT2"))
                {
                    npc.AddBuff(mod.BuffType("Locked"), 6 * 60);
                }
                if (standSlotType == mod.ItemType("LockT3"))
                {
                    npc.AddBuff(mod.BuffType("Locked"), 9 * 60);
                }
                if (standSlotType == mod.ItemType("LockT4"))
                {
                    npc.AddBuff(mod.BuffType("Locked"), 12 * 60);
                }
            }
            else
            {
                if (standSlotType == mod.ItemType("LockT1"))
                {
                    npc.AddBuff(mod.BuffType("Locked"), 5 * 60);
                }
                if (standSlotType == mod.ItemType("LockT2"))
                {
                    npc.AddBuff(mod.BuffType("Locked"), 10 * 60);
                }
                if (standSlotType == mod.ItemType("LockT3"))
                {
                    npc.AddBuff(mod.BuffType("Locked"), 15 * 60);
                }
                if (standSlotType == mod.ItemType("LockT4"))
                {
                    npc.AddBuff(mod.BuffType("Locked"), 20 * 60);
                }
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (forceDeath)
            {
                npc.lifeRegen = -4;
            }
        }
    }
}