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
namespace BrandonBox.NPCs.Archeologist
{
	// [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
	[AutoloadHead]
	public class Archeologist : ModNPC
	{
		public const string ShopName = "Shop";
		public int NumberOfTimesTalkedTo = 0;
		private static Profiles.StackedNPCProfile NPCProfile;
		private int originalType;
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 25; // The total amount of frames the NPC has

			NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
			NPCID.Sets.AttackFrameCount[Type] = 4; // The amount of frames in the attacking animation.
			NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the NPC that it tries to attack enemies.
			NPCID.Sets.AttackType[Type] = 0; // The type of attack the Town NPC performs. 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
			NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
			NPCID.Sets.AttackAverageChance[Type] = 30; // The denominator for the chance for a Town NPC to attack. Lower numbers make the Town NPC appear more aggressive.
			NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			originalType = NPC.type;

		}

		public override void AI()
        {
            // Call the base AI method to maintain the NPC's default behavior

			if (originalType == 0)
			{
				originalType = NPC.type;
				return;
			}


            // Check if it's nighttime
            if (!Main.dayTime)
            {
                // Change the NPC's appearance and behavior to werewolf
                NPC.type = NPCID.Werewolf;
				NPC.townNPC = false; // Sets NPC to be a Town NPC
				NPC.friendly = false;
				// NPCID.Sets.ExtraFramesCount[Type] = 6;
				AIType = NPCID.Werewolf;
				
            }
            else
            {
                // If it's daytime, revert the NPC's appearance and behavior back to its original state
                NPC.type = originalType;
				NPC.townNPC = true; // Sets NPC to be a Town NPC
				NPC.friendly = true;
				// NPCID.Sets.ExtraFramesCount[Type] = 9;
				// Main.npcFrameCount[Type] = 25;
				AIType = originalType;
            }
				base.AI();

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

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("Hailing from a mysterious greyscale cube world, the Example Person is here to help you understand everything about tModLoader."),

				// You can add multiple elements if you really wanted to
				// You can also use localization keys (see Localization/en-US.lang)
				new FlavorTextBestiaryInfoElement("Mods.ExampleMod.Bestiary.ExamplePerson")
			});
		}

		// The PreDraw hook is useful for drawing things before our sprite is drawn or running code before the sprite is drawn
		// Returning false will allow you to manually draw your NPC
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			// This code slowly rotates the NPC in the bestiary
			// (simply checking NPC.IsABestiaryIconDummy and incrementing NPC.Rotation won't work here as it gets overridden by drawModifiers.Rotation each tick)
			if (NPCID.Sets.NPCBestiaryDrawOffset.TryGetValue(Type, out NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers)) {
				drawModifiers.Rotation += 0.001f;

				// Replace the existing NPCBestiaryDrawModifiers with our new one with an adjusted rotation
				NPCID.Sets.NPCBestiaryDrawOffset.Remove(Type);
				NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			}

			return true;
		}

		public override void HitEffect(NPC.HitInfo hit) {
			int num = NPC.life > 0 ? 1 : 5;

			// for (int k = 0; k < num; k++) {
			//// 	Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Sparkle>());
			// }

			// Create gore when the NPC is killed.
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0) {
				string variant = "";
				int hatGore = NPC.GetPartyHatGore();
				int headGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Head").Type;
				int armGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Arm").Type;
				int legGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Leg").Type;

				// Spawn the gores. The positions of the arms and legs are lowered for a more natural look.
				if (hatGore > 0) {
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, hatGore);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, headGore, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs) { // Requirements for the town NPC to spawn.
			foreach (var player in Main.ActivePlayers)
				if (player.anglerQuestsFinished >= 100)
					return true;
			return false;
		}

		public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() {
			return new List<string>() {
    "Dr. Benjamin Stone",
    "Professor Alexander Carter",
    "Dr. Nicholas Evans",
    "Professor Matthew Johnson",
    "Dr. Daniel Foster",
    "Professor Michael Anderson",
    "Dr. Christopher Hayes",
    "Professor Jonathan Baker",
    "Dr. William Mitchell",
    "Professor David Peterson"
};
		}

		public override void FindFrame(int frameHeight) {
			/*npc.frame.Width = 40;
			if (((int)Main.time / 10) % 2 == 0)
			{
				npc.frame.X = 40;
			}
			else
			{
				npc.frame.X = 0;
			}*/
		}

		public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();


		chat.Add("I never asked for this curse, but I must find a way to control it.");
		chat.Add("The sands of time hold the key to my affliction.");
		chat.Add("If only I could uncover the secrets of the ancient temple...");
		chat.Add("The howls of the night haunt me...");
		chat.Add("I fear what I might become if I cannot find a remedy.");
		chat.Add("My research is my only hope...");
		chat.Add("Forgive my absence during the day, for I am consumed by my quest for a cure.");
		chat.Add("Every transformation takes a toll on my soul...");
		chat.Add("The curse is a constant reminder of my failures.");
		chat.Add("I am cursed to wander the desert of my own mind.");

			string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.


			return chosenChat;
		}

		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			button = Language.GetTextValue("LegacyInterface.28");
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (firstButton) {
				shop = ShopName; // Name of the shop tab we want to open.
			}
		}

		// Not completely finished, but below is what the NPC will sell
		public override void AddShops() {
			var npcShop = new NPCShop(Type, ShopName)
				.Add(ItemID.FuzzyCarrot)
				.Add(ItemID.AnglerHat)
				.Add(ItemID.AnglerVest)
				.Add(ItemID.AnglerPants)
				.Add(ItemID.GoldenFishingRod)
				.Add(ItemID.BottomlessBucket)
				.Add(ItemID.SuperAbsorbantSponge)
				.Add(ItemID.GoldenBugNet)
				.Add(ItemID.FishHook)
				.Add(ItemID.FishMinecart);

			if (Main.hardMode) {
				npcShop
				.Add(ItemID.HotlineFishingHook)
				.Add(ItemID.FinWings);

			}
			npcShop.Register(); // Name of this shop tab
		}

		public override void ModifyActiveShop(string shopName, Item[] items) {
			foreach (Item item in items) {
				// Skip 'air' items and null items.
				if (item == null || item.type == ItemID.None) {
					continue;
				}
			}
		}

		// public override void ModifyNPCLoot(NPCLoot npcLoot) {
		// 	npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ExampleCostume>()));
		// }

		// Make this Town NPC teleport to the King and/or Queen statue when triggered. Return toKingStatue for only King Statues. Return !toKingStatue for only Queen Statues. Return true for both.
		public override bool CanGoToStatue(bool toKingStatue) => true;

		// Make something happen when the npc teleports to a statue. Since this method only runs server side, any visual effects like dusts or gores have to be synced across all clients manually.
		public override void OnGoToStatue(bool toKingStatue) {
			if (Main.netMode == NetmodeID.Server) {
				ModPacket packet = Mod.GetPacket();
				// packet.Write((byte)ExampleMod.MessageType.ExampleTeleportToStatue);
				packet.Write((byte)NPC.whoAmI);
				packet.Send();
			}
			else {
				StatueTeleport();
			}
		}

		// Create a square of pixels around the NPC on teleport.
		public void StatueTeleport() {
			for (int i = 0; i < 30; i++) {
				Vector2 position = Main.rand.NextVector2Square(-20, 21);
				if (Math.Abs(position.X) > Math.Abs(position.Y)) {
					position.X = Math.Sign(position.X) * 20;
				}
				else {
					position.Y = Math.Sign(position.Y) * 20;
				}

				// Dust.NewDustPerfect(NPC.Center + position, ModContent.DustType<Sparkle>(), Vector2.Zero).noGravity = true;
			}
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