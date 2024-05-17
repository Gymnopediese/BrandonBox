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

namespace BrandonBox.NPCs.Gravedigger
{

	public class GravediggerTaxCollection : ModSystem
	{
		public static int Taxes = 0;

		public override void PreUpdateTime() {
			if ((Main.time + 1800) % 3600 == 0)
			{
				if (!NPC.AnyNPCs(ModContent.NPCType<Gravedigger>()))
				{
					Taxes = 0;
					return;
				}
				for (int i = 0; i < Main.npc.Length; i++)
				{
					if (Main.npc[i].townNPC && !Main.npc[i].homeless && Main.npc[i].type != ModContent.NPCType<Gravedigger>() && Main.npc[i].type == ModContent.NPCType<NPCs.TheCursed.TheCursed>())
					{
						if (NPC.AnyNPCs(ModContent.NPCType<NPCs.TheCursed.TheCursed>()))
							Taxes += 2000;
						else
							Taxes += 200;
					}
				}
			}
		}
		public override void SaveWorldData(TagCompound tag) {
			tag["Taxes"] = Taxes;
		}

		public override void LoadWorldData(TagCompound tag) {
			Taxes = tag.GetInt("Taxes");
		}
	}
	// [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
	[AutoloadHead]
	public class Gravedigger : ModNPC
	{
		public const string ShopName = "Shop";
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
			return (NPC.AnyNPCs(NPCID.TaxCollector));
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
			return new List<string>(){
"M",
};
		}

		public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();
			chat.Add("Don't talk to me.");
			string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.
			return chosenChat;
		}

		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			button = "Zombie Cure (10 gold)";

			string tax = "";
			int ttax = GravediggerTaxCollection.Taxes;
			if (ttax % 100 != 0)
				tax = ttax % 100 + " copper";
			ttax /= 100;
			if (ttax % 100 != 0)
				tax = ttax % 100 + " silver " + tax;
			ttax /= 100;
			if (ttax % 100 != 0)
				tax = ttax % 100 + " gold " + tax;
			ttax /= 100;
			if (ttax % 100 != 0)
				tax = ttax % 100 + " platinum " + tax;
			if (GravediggerTaxCollection.Taxes == 0)
				button2 = "No Taxes";
			else
				button2 = "Collect Taxes (" + tax + ")";
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (!firstButton)
			{
				Main.LocalPlayer.QuickSpawnItemDirect(null, ItemID.CopperCoin, (int)GravediggerTaxCollection.Taxes % 100);
				GravediggerTaxCollection.Taxes /= 100;
				Main.LocalPlayer.QuickSpawnItemDirect(null, ItemID.SilverCoin, (int)GravediggerTaxCollection.Taxes % 100);
				GravediggerTaxCollection.Taxes /= 100;
				Main.LocalPlayer.QuickSpawnItemDirect(null, ItemID.GoldCoin, (int)GravediggerTaxCollection.Taxes % 100);
				GravediggerTaxCollection.Taxes /= 100;
				Main.LocalPlayer.QuickSpawnItemDirect(null, ItemID.PlatinumCoin, (int)GravediggerTaxCollection.Taxes % 100);
				GravediggerTaxCollection.Taxes /= 100;
			}
			else if (Main.LocalPlayer.BuyItem(100000))
				Main.LocalPlayer.QuickSpawnItemDirect(null , ModContent.ItemType<ZombieCure>(), 1);
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