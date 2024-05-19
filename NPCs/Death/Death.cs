// using ExampleMod.Common;
// using ExampleMod.Common.Configs;
// using ExampleMod.Content.Biomes;
// using ExampleMod.Content.Dusts;
// using ExampleMod.Content.EmoteBubbles;
// using ExampleMod.Content.Items;
// using ExampleMod.Content.Items.Accessories;
// using ExampleMod.Content.Items.Armor;
// using ExampleMod.Content.Projectiles;
// using ExampleMod.Content.Tiles;
// using ExampleMod.Content.Tiles.Furniture;
// using ExampleMod.Content.Walls;
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

namespace BrandonBox.NPCs.Death
{
	// [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
	[AutoloadHead]
	public class Death : ModNPC
	{
		public const string ShopName = "Shop";
		public int NumberOfTimesTalkedTo = 0;

		// private static int ShimmerHeadIndex;
		private static Profiles.StackedNPCProfile NPCProfile;

		public override void Load() {
			// Adds our Shimmer Head to the NPCHeadLoader.
			// ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
		}

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
			if (!ModContent.GetInstance<Systems.NPCsConfigs>().Death) return false;
			foreach (var player in Main.ActivePlayers)
				if (player.numberOfDeathsPVE + player.numberOfDeathsPVP >= 100)
					return true;
			return false;
		}

		public override bool CheckConditions(int left, int right, int top, int bottom) {
			int score = 0;
			for (int x = left -10; x <= right + 10; x++) {
				for (int y = top -10; y <= bottom + 10; y++) {
					if (Main.tile[x, y].TileType == TileID.Tombstones) {
						score++;
					}
				}
			}
			return score >= 4 * 7;
		}

		public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() {
			return new List<string>() {
				"Grimsley",
				"Mortimer",
				"Necron",
				"Grimm",
				"Reaperius",
				"Shade",
				"Bane",
				"Thanatos",
				"Void",
				"Darkmourne",
        };
		}


		public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();

			chat.Add("Eternity is just a concept for you, isn't it?");
			chat.Add("Resurrections? You must be the bane of my existence!");
			chat.Add("What's with mortals and their obsession with cheating death?");
			chat.Add("Do you ever get tired of dying? I know I do.");
			chat.Add("Every time you respawn, a part of my patience dies.");
			chat.Add("Life, death, respawn... it's all a tiresome cycle.");
			chat.Add("You again? I swear, you're more persistent than a cockroach!");
			chat.Add("Death comes for us all... except you, apparently.");
			chat.Add("One day you'll meet your end, and I won't be here to sell you anything!");
			chat.Add("I'm surrounded by the living... how utterly depressing.");
			chat.Add("I'm not sure if I should be impressed or annoyed by your constant revivals.");
			chat.Add("You're like a bad penny... you just keep turning up!");
			chat.Add("I've been killed by a lot of things, but never by kindness. What do you want?");
			string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.
			if (Main.rand.NextBool(4)) {
				chosenChat = "I've killed you " + (Main.LocalPlayer.numberOfDeathsPVE + Main.LocalPlayer.numberOfDeathsPVP) + " times. I'm keeping count.";
			}

			return chosenChat;
		}

		public override bool CanBeHitByNPC (NPC 	attacker	) {return false;}

		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			// button = Language.GetTextValue("LegacyInterface.28");
			button = "Kill me! (1 Copper)";
			// if (Main.LocalPlayer.HasItem(ItemID.HiveBackpack)) {
			// 	button = "Upgrade " + Lang.GetItemNameValue(ItemID.HiveBackpack);
			// }
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (Main.LocalPlayer.BuyItem(1))
				Main.LocalPlayer.KillMe(PlayerDeathReason.ByCustomReason("by the power of death itself!"), 9999, 0, false);
		}

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