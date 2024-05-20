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
namespace BrandonBox.NPCs.Pickpocket
{

	public class PickpocketPlayer : ModPlayer
	{
		public override void PostBuyItem(NPC vendor, Item[] shopInventory, Item item)
		{
			if (vendor.type != ModContent.NPCType<Pickpocket>())
				return;
			
			for (var i = 0; i < shopInventory.Length; i++)
			{
				if (shopInventory[i].type != item.type)
					continue;
				PickpocketAllowed.itemsIDs.Remove(item.type);
				shopInventory[i] = new Item();
				item.value *= 4 / 5;
				return;
			}
		}
	}

	public class PickpocketAllowed : ModSystem
	{
		public static List<int> itemsIDs = new List<int>();
		public static bool allowed = false;

		public override void PreUpdateTime() {
			if (!(Main.time == 0 && Main.dayTime))
			 	return ;

			itemsIDs.Clear();
			
			List<Item> items = new List<Item>();
			int count = 0;
			foreach (var shop in NPCShopDatabase.AllShops)
			{
				if (!NPC.AnyNPCs(shop.NpcType) || shop.NpcType == ModContent.NPCType<Pickpocket>())
					continue;
				foreach (var entry in shop.ActiveEntries)
				{
					if (entry.Item.value < 10000)
						continue;
					foreach (var condition in entry.Conditions)
						if (!condition.Predicate())
							goto ConditionNotMet;
					items.Add(entry.Item);
					count += 1;
					ConditionNotMet:;
				}
			}


			var min = Math.Max(Math.Min((int)(items.Count / 20), 4), 1);
			var max = Math.Max(min + 1,Math.Min((int)(items.Count / 10), 10));
			var amount = Main.rand.Next(min, max);
			for (var i = 0; i < amount && i < count; i++)
			{
				var index = Main.rand.Next(items.Count);
				var item = items[index].type;
				items.RemoveAt(index);
				itemsIDs.Add(item);
			}
		}

		public override void LoadWorldData(TagCompound tag) {
			allowed = tag.GetBool("allowed");
			var count = tag.GetInt("count");
			for (var i = 0; i < count; i++)
				itemsIDs.Add(tag.GetInt("item" + i));
		}

		public override void SaveWorldData(TagCompound tag) {
			tag["allowed"] = allowed;
			tag["count"] = itemsIDs.Count;
			for (var i = 0; i < itemsIDs.Count; i++)
				tag["item" + i] = itemsIDs[i];

		}
	}
	// [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
	[AutoloadHead]
	public class Pickpocket : ModNPC
	{
		public const string ShopName = "Shop";
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
			NPCID.Sets.NoTownNPCHappiness[Type] = true; // Makes the NPC not have a happiness value.
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
			if (!ModContent.GetInstance<Systems.NPCsConfigs>().Pickpocket) return false;
			if (PickpocketAllowed.allowed) return true;
			foreach (var player in Main.ActivePlayers)
			{
				if (player.active
					&& (player.HasItem(ItemID.GreenCap)
					|| player.HasItem(ItemID.DyeTradersScimitar)
					|| player.HasItem(ItemID.PainterPaintballGun)
					|| player.HasItem(ItemID.JimsCap)
					|| player.HasItem(ItemID.AleThrowingGlove)
					|| player.HasItem(ItemID.StylistKilLaKillScissorsIWish)
					|| player.HasItem(ItemID.RedHat)
					|| player.HasItem(ItemID.CombatWrench)
					|| player.HasItem(ItemID.PartyGirlGrenade)))
				{
					PickpocketAllowed.allowed = true;
					return true;
				}
			}
			return false;
		}

		public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() {
			return new List<string>(){
				"Mei Ling",
				"Xiao Li",
				"Ling Hua",
				"Jia Jia",
				"Yue Min",
				"Hua Chen",
				"An Mei",
				"Xiang Yu",
				"Lan Fen",
				"Zhen Zhen"
			};
		}

		public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();

			if (PickpocketAllowed.itemsIDs.Count != 0)
			{
				chat.Add("I've got some great deals for you today!");
				chat.Add("Oh, these? Handmade, of course!");
				chat.Add("I don't know where you heard that... these are all crafted by me!");
				chat.Add("Such a bargain, right? Took me ages to make!");
				chat.Add("Why would I steal? I'm just a humble artisan!");
				chat.Add("Every stitch and thread, my own work!");
				chat.Add("How could you think these are stolen? They're my pride and joy!");
				chat.Add("You're getting an incredible deal, but it's hard for me too!");
				chat.Add("Don't listen to rumors, my items are 100% authentic!");
				chat.Add("Trust me, these took forever to create!");
				chat.Add("It's tough to sell them this cheap, but I want to share my work!");
			}
			else 
			{
				chat.Add("I'm all out of stock for today, come back tomorrow!");
				chat.Add("I've sold out for today, but I'll have more tomorrow!");
				chat.Add("Everyday's I have new items, come back tomorrow!");
			}
			string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.


			return chosenChat;
		}

		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			if (PickpocketAllowed.itemsIDs.Count != 0)
				button = Language.GetTextValue("LegacyInterface.28");
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (firstButton && PickpocketAllowed.itemsIDs.Count != 0) {
				shop = ShopName; // Name of the shop tab we want to open.
			}
		}

		public override void AddShops() {
			new NPCShop(Type, ShopName).Register(); // Name of this shop tab
		}

		public override void ModifyActiveShop(string shopName, Item[] items) {
			for (var i = 0; i < items.Length; i++) {
				if (i >= PickpocketAllowed.itemsIDs.Count)
				{
					items[i] = new Item();
					continue;
				}
				var item = new Item(PickpocketAllowed.itemsIDs[i]);
				item.isAShopItem = true;
				item.shopCustomPrice = (int)(item.value * 4 / 5);
				item.buy = true;
				items[i] = item;
			}
		}

		// public override void ModifyNPCLoot(NPCLoot npcLoot) {
		// 	npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ExampleCostume>()));
		// }

		// Make this Town NPC teleport to the King and/or Queen statue when triggered. Return toKingStatue for only King Statues. Return !toKingStatue for only Queen Statues. Return true for both.
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