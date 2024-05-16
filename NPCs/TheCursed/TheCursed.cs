using BrandonBox.Items.ZombieCure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.DataStructures;

namespace BrandonBox.NPCs.TheCursed
{

	public class TheCursedCollection : ModSystem
	{
		public static int counter = 9999;

		public override void PreUpdateTime() {
			if ((Main.time + 1800) % 3600 == 0)
				counter+=1;

			if (counter >= 24 * 3 && NPC.AnyNPCs(ModContent.NPCType<TheCursed>()))
			{
				int i = 0;
				WeightedRandom<NPC> villagers = new WeightedRandom<NPC>();
				for (; i < Main.npc.Length; i++)
					if (Main.npc[i].type == ModContent.NPCType<NPCs.Villager.Villager>())
						villagers.Add(Main.npc[i]);
				if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Villager.Villager>()))
				{
					NPC villager = villagers;
					villager.SimpleStrikeNPC(9999, 0);
				}
				else
				{
					foreach (var player in Main.player)
						if (player.active)
							player.KillMe(PlayerDeathReason.ByCustomReason(player.name +" was killed by the anger of the cursed.") , 99999, 0);
					Main.npc[NPC.FindFirstNPC(ModContent.NPCType<TheCursed>())].SimpleStrikeNPC(9999, 0);
				}
				counter = 0;
			}
		}
		public override void SaveWorldData(TagCompound tag) {
			tag["counter"] = counter;
		}

		public override void LoadWorldData(TagCompound tag) {
			counter = tag.GetInt("counter");
		}
	}
	// [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
	[AutoloadHead]
	public class TheCursed : ModNPC
	{
		private static Profiles.StackedNPCProfile NPCProfile;
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 25; // The total amount of frames the NPC has

			NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
			NPCID.Sets.AttackFrameCount[Type] = 4; // The amount of frames in the attacking animation.
			NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the NPC that it tries to attack enemies.
			NPCID.Sets.AttackType[Type] = 0; // The type of attack the Town NPC performs. 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
			NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
			NPCID.Sets.AttackAverageChance[Type] = 30; // The denominator for the chance for a Town NPC to attack. Lower numbers make the Town NPC appear more aggressive.
			NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.

		}

		public override void SetDefaults() {
			NPC.townNPC = true; // Sets NPC to be a Town NPC
			NPC.friendly = true; // NPC Will not attack player
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			

			AnimationType = NPCID.Guide;
		}


		public override bool CanTownNPCSpawn(int numTownNPCs) { // Requirements for the town NPC to spawn.
			return (NPC.AnyNPCs(ModContent.NPCType<NPCs.Villager.Villager>()) && NPC.AnyNPCs(ModContent.NPCType<NPCs.Gravedigger.Gravedigger>()) && (NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3));
		}

public override bool CheckConditions(int left, int right, int top, int bottom) {
			int score = 0;
			for (int x = left -10; x <= right + 10; x++) {
				for (int y = top -10; y <= bottom + 10; y++) {
					if (Main.tile[x, y].TileType == TileID.WaterCandle) {
						score++;
					}
				}
			}
			return score >= 3;
		}
		

		public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() {
			return new List<string>(){
"Harold Graves",
"Samuel Stone",
"William Diggs",
"George Graveley",
"Thomas Tombstone",
"Edward Earth",
"Arthur Ashes",
"Frederick Foss",
"Charles Coffin",
"Joseph Bones"
};
		}

		public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();
			string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.
			return chosenChat;
		}
		public override bool CanGoToStatue(bool toKingStatue) => true;

		public override void TownNPCAttackStrength(ref int damage, ref float knockback) {
			damage = 20;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) {
			cooldown = 30;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay) {
			// projType = ModContent.ProjectileType<SparklingBall>();
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) {
			multiplier = 12f;
			randomOffset = 2f;
			// SparklingBall is not affected by gravity, so gravityCorrection is left alone.
		}
	}
}