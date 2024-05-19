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
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
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

namespace BrandonBox.NPCs.DaughterOfSun
{
	// Here is where we save that our Town NPC has been rescued to the world.
	public class TownNPCGuideWorld : ModSystem {
		public static bool rescuedTutorialTownNPC = false;

		public override void SaveWorldData(TagCompound tag) {
			if (rescuedTutorialTownNPC) {
				tag["rescuedTutorialTownNPC"] = true;
			}
		}

		public override void LoadWorldData(TagCompound tag) {
			rescuedTutorialTownNPC = tag.ContainsKey("rescuedTutorialTownNPC");
		}

		public override void NetSend(BinaryWriter writer) {
			BitsByte flags = new BitsByte();
			flags[0] = rescuedTutorialTownNPC;
			writer.Write(flags);
		}

		public override void NetReceive(BinaryReader reader) {
			BitsByte flags = reader.ReadByte();
			rescuedTutorialTownNPC = flags[0];
		}
	}
	// This is how to make a rescuable Town NPC.
	public class DaughterOfSunTrapped : ModNPC
	{
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 1;

			// Hide this NPC from the bestiary.
			NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new(0) {
				Hide = true 
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, bestiaryData);
		}

		public override void SetDefaults() {
			// Notice NPC.townNPC is not set.
			NPC.friendly = true;
			NPC.width = 28;
			NPC.height = 32;
			NPC.aiStyle = 0; // aiStyle of 0 is used. The NPC will not move.
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			NPC.rarity = 1; // To make our NPC will show up on the Lifeform Analyzer.
		}

		public override bool CanChat() {
			return true;
		}

		public override void AI() {

			if (Main.netMode != NetmodeID.MultiplayerClient) {
				// Loop through every player on the server.
				for (int i = 0; i < Main.maxPlayers; i++) {
					// If the player is active (on the server) and are talking to this NPC...
					if (Main.player[i].active && Main.player[i].talkNPC == NPC.whoAmI) {
						NPC.Transform(ModContent.NPCType<DaughterOfSun>()); // Transform to our real Town NPC.																  
						Main.BestiaryTracker.Chats.RegisterChatStartWith(NPC); // Unlock the Town NPC in the Bestiary.																  
						Main.player[i].SetTalkNPC(NPC.whoAmI);  // Change who the player is talking to to the new Town NPC. 
						TownNPCGuideWorld.rescuedTutorialTownNPC = true; // Set our rescue bool to true.

						// We need to sync these changes in multiplayer.
						if (Main.netMode == NetmodeID.Server) {
							NetMessage.SendData(MessageID.SyncTalkNPC, -1, -1, null, i);
							NetMessage.SendData(MessageID.WorldData);
						}
					}
				}
			}
		}

		public override string GetChat() {
			// Make the Town NPC say something unique when first rescued.
			return Language.GetTextValue("Thanks for rescuing me!");
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (!ModContent.GetInstance<Systems.NPCsConfigs>().DaughterOfSun) return 0f;
			if (!TownNPCGuideWorld.rescuedTutorialTownNPC && !NPC.AnyNPCs(ModContent.NPCType<DaughterOfSunTrapped>()) && !NPC.AnyNPCs(ModContent.NPCType<DaughterOfSun>())) {
				if (spawnInfo.SpawnTileType == TileID.Sunplate || spawnInfo.SpawnTileType == TileID.Cloud) {//&& Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].WallType == WallID.DiscWall
					return 1f;
				}
			}
			return 0f;
		}

	}
	// [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
	[AutoloadHead]
	public class DaughterOfSun : ModNPC
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
			// NPCID.Sets.ShimmerTownTransform[NPC.type] = true; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

			// NPCID.Sets.ShimmerTownTransform[Type] = true; // Allows for this NPC to have a different texture after touching the Shimmer liquid.

			// Connects this NPC with a custom emote.
			// This makes it when the NPC is in the world, other NPCs will "talk about him".
			// By setting this you don't have to override the PickEmote method for the emote to appear.
			//// NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<ExamplePersonEmote>();

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
				// Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
				// If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			// Set Example Person's biome and neighbor preferences with the NPCHappiness hook. You can add happiness text and remarks with localization (See an example in ExampleMod/Localization/en-US.lang).
			// NOTE: The following code uses chaining - a style that works due to the fact that the SetXAffection methods return the same NPCHappiness instance they're called on.
			NPC.Happiness
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like) // Example Person prefers the forest.
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike) // Example Person dislikes the snow.
				//// .SetBiomeAffection<ExampleSurfaceBiome>(AffectionLevel.Love) // Example Person likes the Example Surface Biome
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Love) // Loves living near the dryad.
				.SetNPCAffection(NPCID.Guide, AffectionLevel.Like) // Likes living near the guide.
				.SetNPCAffection(NPCID.Merchant, AffectionLevel.Dislike) // Dislikes living near the merchant.
				.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate) // Hates living near the demolitionist.
			; // < Mind the semicolon!

			// This creates a "profile" for ExamplePerson, which allows for different textures during a party and/or while the NPC is shimmered.
			// NPCProfile = new Profiles.StackedNPCProfile(
			// 	new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture), Texture + "_Party"),
			// 	new Profiles.DefaultNPCProfile(Texture + "_Shimmer", ShimmerHeadIndex, Texture + "_Shimmer_Party")
			// );
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


		public override bool CanTownNPCSpawn(int numTownNPCs) {
			if (!ModContent.GetInstance<Systems.NPCsConfigs>().DaughterOfSun) return false;
			return (TownNPCGuideWorld.rescuedTutorialTownNPC);
		}

		// ! trop styleeee
		// Example Person needs a house built out of ExampleMod tiles. You can delete this whole method in your townNPC for the regular house conditions.
		public override bool CheckConditions(int left, int right, int top, int bottom) {
			int score = 0;
			for (int x = left; x <= right; x++) {
				for (int y = top; y <= bottom; y++) {
					int type = Main.tile[x, y].TileType;
					if (type == TileID.Cloud) {
						score++;
					}
					if (Main.tile[x, y].WallType == WallID.Cloud) {
						score++;
					}
				}
			}

			return (score >= ((right - left) * (bottom - top)) / 2);
		}

		public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() {
			return new List<string>() {
				"Soleil",
				"Aurora",
				"Helia",
				"Sunniva",
				"Radiance",
				"Solara",
				"Aurelia",
				"Lumina",
				"Sunny",
				"Sunbeam",
			};
		}

		public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();

			chat.Add("I do love a good sunny day!");
			chat.Add("Isn't it wonderful when the sun is shining?");
			chat.Add("Sunlight just makes everything better, don't you think?");
			chat.Add("Let's brighten things up around here!");
			chat.Add("Ah, the warmth of the sun is so comforting.");
			chat.Add("I hope you're enjoying the sunshine as much as I am!");
			chat.Add("Daytime adventures are the best kind!");
			chat.Add("I heard gold has a special glow in the sunlight!");
			chat.Add("Isn't the sky just lovely when it's clear?");
			chat.Add("Thank you for bringing some sunshine into my day!");
			if (Main.IsItDay())
				chat.Add("The sun is shining so brightly today!");
			string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.
			return chosenChat;
		}

		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			if (Main.dayTime)
				button = "Bring Sunshine (50 Gold)";
			else
				button = "Remove Sunshine (50 Gold)";
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (Main.LocalPlayer.BuyItem(500000)){
				if (!Main.dayTime)
				{
					Main.dayTime = true;
					Main.time = 0;
					Main.NewText("You bought some sunshine!");
					// Main.SkipToTime(0, true);
				}
				else
				{
					Main.dayTime = false;
					Main.time = 0;
					Main.NewText("You bought some darkness!");
					// Main.SkipToTime(19500, false);
				}
			}
			// els
		}

		public override bool CanGoToStatue(bool toKingStatue) => false;


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