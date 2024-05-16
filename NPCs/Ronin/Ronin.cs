
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

namespace BrandonBox.NPCs.Ronin
{
	// [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
	[AutoloadHead]
	public class Ronin : ModNPC
	{
		public static bool japaneseHouse = false;
		public const string ShopName = "Shop";
		public int NumberOfTimesTalkedTo = 0;
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
			// Connects this NPC with a custom emote.


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

		public override bool CanTownNPCSpawn(int numTownNPCs) { // Requirements for the town NPC to spawn.
			foreach (var player in Main.ActivePlayers)
				if (NPC.downedSlimeKing)
					return true;
			return false;
		}

		// ! trop styleeee
		// Example Person needs a house built out of ExampleMod tiles. You can delete this whole method in your townNPC for the regular house conditions.
		public override bool CheckConditions(int left, int right, int top, int bottom) {
			int score = 0;
			for (int x = left; x <= right; x++) {
				for (int y = top; y <= bottom; y++) {
					int type = Main.tile[x, y].TileType;
					if (type == TileID.DynastyWood || type == TileID.BlueDynastyShingles || type == TileID.RedDynastyShingles)
					{
						score++;
					}

					if (Main.tile[x, y].WallType == WallID.BlueDynasty || Main.tile[x, y].WallType == WallID.WhiteDynasty) {
						score++;
					}
				}
			}
			// Main.NewText("score" + " " + score + " " + (right - left) * (bottom - top) / 2);
			japaneseHouse = (score >= (right - left) * (bottom - top) / 2);

			return true;
		}

		public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() {
			return new List<string>(){
	"Ayame",
	"Haruka",
	"Kiyomi",
	"Mariko",
	"Sakura",
	"Yukiko",
	"Emi",
	"Hinata",
	"Kaede",
	"Reiko"
};
		}

		public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();
			chat.Add("In the silence of the night, I am the whisper of the wind.");
			chat.Add("Honor binds me, but duty drives me. Yet, beneath it all, lies a soul yearning for freedom.");
			chat.Add("The path of the blade is fraught with peril, but the heart of the warrior knows no fear.");
			chat.Add("Through the storm of battle, I find clarity. Yet, within the tempest, lies the echo of a forgotten promise.");
			chat.Add("To know oneself is to conquer all foes. But who am I, beneath this mask of steel and silence? Perhaps even I do not know.");
			chat.Add("Fear not the darkness, for I am its shadow. But beware, for shadows hold secrets darker still.");
			chat.Add("My blade knows no master but honor. Yet, beneath its polished steel lies a tale of sacrifice and redemption.");
			chat.Add("To defeat a foe, one must first understand oneself. In my journey, I have faced demons both within and without.");
			chat.Add("The path of the warrior is solitude, yet I am never alone. For the spirits of my ancestors walk beside me, whispering their wisdom.");
			chat.Add("In battle, the true self is revealed. But who am I, beneath this mask of steel and silence? Perhaps only the winds of fate hold the answer.");

			chat.Add("My father, a shadow in the annals of history, wielded a blade that sung songs of valor and sacrifice.");
			chat.Add("In the echoes of battle, I hear my father's voice, a whisper of guidance from beyond the veil.");
			chat.Add("With each step I take, I honor the memory of my father, his legacy a flame that burns within my soul.");
			chat.Add("The path I walk was forged by my father's footsteps, a trail of courage and resilience.");
			chat.Add("In the stillness of the night, I commune with the spirit of my father, finding solace in his eternal presence.");

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

			var isJapanese = new Condition("japanesHouse", () => japaneseHouse);
			var isJapaneseBosse = new Condition("japanesHouse", () => NPC.downedBoss3 && japaneseHouse);
			var npcShop = new NPCShop(Type, ShopName)
				.Add(42)
				.Add(ItemID.Katana)
				.Add(ItemID.DynastyWood, isJapanese)
				.Add(ItemID.BlueDynastyShingles, isJapanese)
				.Add(ItemID.RedDynastyShingles, isJapanese)
				.Add(ItemID.Muramasa, isJapaneseBosse);

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

		public override void LoadData(TagCompound tag) {
			japaneseHouse = tag.GetBool("japaneseHouse");
		}

		public override void SaveData(TagCompound tag) {
			tag["japaneseHouse"] = japaneseHouse;
		}
	}
}