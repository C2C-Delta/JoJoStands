﻿using Terraria;
using Terraria.ModLoader;

namespace JoJoStands.Buffs.ItemBuff
{
    public class CooledOut : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Cooled Out");
            Description.SetDefault("You lowered your body temperature.");
            Main.buffNoTimeDisplay[Type] = false;
        }
    }
}