
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
namespace BrandonBox.NPCs.TVGuy
{
	public class TVGuyAllowed : ModSystem
	{
		public static bool allowed = false;

		public override void LoadWorldData(TagCompound tag) {
			allowed = tag.GetBool("allowed");
		}
		
		public override void SaveWorldData(TagCompound tag) {
			tag["allowed"] = allowed;
		}

		public override void NetSend(BinaryWriter writer) {
			BitsByte flags = new BitsByte();
			flags[0] = allowed;
			writer.Write(flags);
		}

		public override void NetReceive(BinaryReader reader) {
			BitsByte flags = reader.ReadByte();
			allowed = flags[0];
		}


	}

	// [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
	[AutoloadHead]
	public class TVGuy : ModNPC
	{
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

			// Set Example Person's biome and neighbor preferences with the NPCHappiness hook. You can add happiness text and remarks with localization (See an example in ExampleMod/Localization/en-US.lang).
			// NOTE: The following code uses chaining - a style that works due to the fact that the SetXAffection methods return the same NPCHappiness instance they're called on.
			NPC.Happiness
				//// .SetBiomeAffection<ExampleSurfaceBiome>(AffectionLevel.Love) // Example Person likes the Example Surface Biome
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Hate) // Loves living near the dryad.
				.SetNPCAffection(NPCID.Guide, AffectionLevel.Hate) // Likes living near the guide.
				.SetNPCAffection(NPCID.Merchant, AffectionLevel.Hate) // Dislikes living near the merchant.
				.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate) // Hates living near the demolitionist.
				.SetNPCAffection(NPCID.Nurse, AffectionLevel.Hate) // Hates living near the nurse.
				.SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Hate) // Hates living near the arms dealer.
				.SetNPCAffection(NPCID.Clothier, AffectionLevel.Hate) // Hates living near the clothier.
				.SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Hate) // Hates living near the goblin tinkerer.
				.SetNPCAffection(NPCID.Wizard, AffectionLevel.Hate) // Hates living near the wizard.
				.SetNPCAffection(NPCID.Mechanic, AffectionLevel.Hate) // Hates living near the mechanic.
				.SetNPCAffection(NPCID.Painter, AffectionLevel.Hate) // Hates living near the painter.
				.SetNPCAffection(NPCID.Angler, AffectionLevel.Hate) // Hates living near the angler.
				.SetNPCAffection(NPCID.DD2Bartender, AffectionLevel.Hate) // Hates living near the tavernkeep.
				.SetNPCAffection(NPCID.Mechanic, AffectionLevel.Love) // Hates living near
				.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Hate); // Hates living near

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
			if (TVGuyAllowed.allowed)
				return true;
			foreach (var player in Main.ActivePlayers)
			{
				if (player.HasItem(ItemID.Meteorite) || player.HasItem(ItemID.MeteoriteBar))
				{
					TVGuyAllowed.allowed = true;
					return true;
				}
			}
			return false;
		}

		// ! trop styleeee
		// Example Person needs a house built out of ExampleMod tiles. You can delete this whole method in your townNPC for the regular house conditions.
		public override bool CheckConditions(int left, int right, int top, int bottom) {
			int score = 0;
			int x = (left + right) / 2 * 16;
			int y = (top + bottom) / 2 * 16;
			foreach (var npc in Main.npc)
				if (!npc.homeless && npc.townNPC && npc.Center.Distance(new Vector2(x, y)) < 3000) 
						return false;
			return true;
		}

		public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() {
			return new List<string>(){
				"Tom",
				"Dave",
				"Steve",
				"John",
				"Mike",
				"Rob",
				"Chris",
				"Matt",
				"Jim",
				"Dan"
			};
		}

		public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();
			if (NPC.downedBoss3)
			{
				chat.Add("I'm working a new Camera system, it's going to be great!");
				chat.Add("My camera is near perfect, I just need some wire... Do you know where I can find some?");
				chat.Add("I'm working on a new camera, but I need some wire to finish it.");
			}
			chat.Add("The truth is out there, hidden in the shadows.");
			chat.Add("Keep your eyes open, they're among us.");
			chat.Add("The government denies it, but I've seen the proof.");
			chat.Add("Don't be fooled by their smiles, they're hiding something sinister.");
			chat.Add("The stars whisper secrets, but only the vigilant can hear.");
			chat.Add("Every corner holds a clue, every shadow a mystery.");
			chat.Add("Mark my words, the invasion is imminent.");
			chat.Add("The meteorite was just the beginning, the aliens are coming.");
			chat.Add("Trust me, I've seen things you wouldn't believe.");
			if (NPC.AnyNPCs(NPCID.Guide))
			{
				chat.Add("The guide's knowledge is vast, but he's keeping secrets from you.");
				chat.Add("Don't trust the guide, he's in on it.");
			}
			if (NPC.AnyNPCs(NPCID.Merchant))
			{
				chat.Add("The merchant's prices are a front, he's really a spy.");
				chat.Add("The merchant's wares are stolen, I've seen the evidence.");
				chat.Add("The merchant's wares are laced with mind-controlling substances, I've seen the effects firsthand.");
			}
			if (NPC.AnyNPCs(NPCID.Nurse))
			{
				chat.Add("The nurse's needles inject more than just medicine, they're tracking devices.");
				chat.Add("I've caught the nurse exchanging secrets with the underworld.");
			}
			if (NPC.AnyNPCs(NPCID.ArmsDealer))
				chat.Add("The arms dealer's weapons are more than they seem, they're alien technology.");
			if (NPC.AnyNPCs(NPCID.Dryad))
				chat.Add("The dryad speaks to the plants, but they know more than she lets on.");
			if (NPC.AnyNPCs(NPCID.Demolitionist))
				chat.Add("The demolitionist's bombs are more than just explosives, they're a warning.");
			if (NPC.AnyNPCs(NPCID.Clothier))
				chat.Add("The clothier's clothes are more than just fabric, they're disguises.");
			if (NPC.AnyNPCs(NPCID.GoblinTinkerer))
				chat.Add("The goblin tinkerer's gadgets are more than just tools, they're weapons.");
			if (NPC.AnyNPCs(NPCID.Wizard))
				chat.Add("The wizard's spells aren't just for show, they're protecting us from unseen threats.");
			if (NPC.AnyNPCs(NPCID.Mechanic))
			// 	chat.Add("The mechanic's machines aren't just for convenience, they're watching our every move.");
			// if (NPC.AnyNPCs(NPCID.Painter))
				chat.Add("The painter's art holds hidden messages, decipher them if you dare.");
			if (NPC.AnyNPCs(NPCID.Angler))
				chat.Add("Even the angler's fishy tales hold grains of truth, if you know where to look.");
			if (NPC.AnyNPCs(NPCID.DD2Bartender))
				chat.Add("The tavernkeep's brews are brewed with ingredients not of this earth.");
			if (NPC.AnyNPCs(NPCID.PartyGirl))
				chat.Add("The party girl is not the age she pretend to be...");
			string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.


			return chosenChat;
		}

		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			if (NPC.downedBoss3)
				button = Language.GetTextValue("LegacyInterface.28");
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (firstButton) {
				shop = ShopName; // Name of the shop tab we want to open.
			}
		}

		// Not completely finished, but below is what the NPC will sell
		public override void AddShops() {
			var npcShop = new NPCShop(Type, ShopName);
				// .Add<TVRemote>();

			npcShop.Add(ModContent.ItemType<Items.WallCamera.WallCameraRecipe>(), new Condition("", () => !Items.Recipe.RecipeLearner.learnedRecipes.Contains("WallCamera")));
			npcShop.Add(ModContent.ItemType<Items.LensCamera.LensCameraRecipe>(), new Condition("", () => !Items.Recipe.RecipeLearner.learnedRecipes.Contains("LensCamera")));


			if (Main.hardMode) {
				npcShop.Add(ModContent.ItemType<Items.EyeCamera.EyeCamera>(), new Condition("", () => !Items.Recipe.RecipeLearner.learnedRecipes.Contains("EyeCamera")));
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
		// Let the NPC "talk about" minion boss
		// public override int? PickEmote(Player closestPlayer, List<int> emoteList, WorldUIAnchor otherAnchor) {
		// 	// By default this NPC will have a chance to use the Minion Boss Emote even if Minion Boss is not downed yet
		// 	int type = ModContent.EmoteBubbleType<MinionBossEmote>();
		// 	// If the NPC is talking to the Demolitionist, it will be more likely to react with angry emote
		// 	if (otherAnchor.entity is NPC { type: NPCID.Demolitionist }) {
		// 		type = EmoteID.EmotionAnger;
		// 	}

		// 	// Make the selection more likely by adding it to the list multiple times
		// 	for (int i = 0; i < 4; i++) {
		// 		emoteList.Add(type);
		// 	}

		// 	// Use this or return null if you don't want to override the emote selection totally
		// 	return base.PickEmote(closestPlayer, emoteList, otherAnchor);
		// }
	}
}