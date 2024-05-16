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
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BrandonBox.NPCs.Creeper
{
	public class CreeperAllowed : ModPlayer
	{
		public static bool allowed = false;

		public override bool PreKill(	double 	damage,
			int 	hitDirection,
			bool 	pvp,
			ref bool 	playSound,
			ref bool 	genDust,
			ref PlayerDeathReason 	damageSource 
		)
		{
			if (damageSource.GetDeathText("non").ToString().Contains(new Item(580).Name));
				allowed = true;
			return true;
		}

		public override void LoadData(TagCompound tag) {
			allowed = tag.GetBool("allowed");
		}

		public override void SaveData(TagCompound tag) {
			tag["allowed"] = allowed;
		}
	}
	// [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
	[AutoloadHead]
	public class Creeper : ModNPC
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
			foreach (var player in Main.ActivePlayers) {
				if (CreeperAllowed.allowed)
					return true;
			}
			return false;
		}

		public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() {
			return new List<string>() {
				"Creepy",
				"Creep",
				"Creepster",
				"Creepo",
				"Creepto",
				"Creeper",
				"Creeptomaniac",
			};
		}

		public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();

			chat.Add("*creeper noises*");
			string chosenChat = chat;
			return chosenChat;
		}

		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			button = Language.GetTextValue("LegacyInterface.28");
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (firstButton) {
				shop = ShopName;
			}
		}

		public override void AddShops() {
			var npcShop = new NPCShop(Type, ShopName);
			npcShop.Add(new Item(ItemID.DirtBlock) { shopCustomPrice = 500 });
			npcShop.Add(new Item(ItemID.StoneBlock) { shopCustomPrice = 500 });
			npcShop.Add(new Item(ItemID.SandBlock) { shopCustomPrice = 500 });
			npcShop.Add(new Item(ItemID.Wood) { shopCustomPrice = 500 });
			npcShop.Add(new Item(ItemID.IronBar) { shopCustomPrice = 10000 });
			npcShop.Add(new Item(ItemID.GoldBar) { shopCustomPrice = 50000 });
			npcShop.Add(new Item(ItemID.Diamond) { shopCustomPrice = 100000 });
			npcShop.Register(); // Name of this shop tab
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
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) {
			multiplier = 12f;
			randomOffset = 2f;
		}
	}
}