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

namespace BrandonBox.NPCs.Traveler
{

	public class VanillaItemAsCurrency : CustomCurrencySingleCoin
{
	public VanillaItemAsCurrency VanillaItemAsCurrencySystem; // Set in the mod class
	public int VanillaItemAsCurrencyID; // Set in the mod class

	public int Value; // Set in the mod class

	public VanillaItemAsCurrency(int coinItemID, long currencyCap, string CurrencyTextKey, int Value) : base(coinItemID, currencyCap)
	{
		this.CurrencyTextKey = CurrencyTextKey; // The name of the currency as a localization key.
		CurrencyTextColor = Color.LightBlue; // The color that the price line will be.
		CurrencyDrawScale = 1f; 
		this.Value = Value;
	}

	public override bool TryPurchasing(	long 	price,
										List< Item[]> 	inv,
										List< Point > 	slotCoins,
										List< Point > 	slotsEmpty,
										List< Point > 	slotEmptyBank,
										List< Point > 	slotEmptyBank2,
										List< Point > 	slotEmptyBank3,
										List< Point > 	slotEmptyBank4 
										)	
	{
		return Main.LocalPlayer.BuyItem(Value) && base.TryPurchasing(price, inv, slotCoins, slotsEmpty, slotEmptyBank, slotEmptyBank2, slotEmptyBank3, slotEmptyBank);
	}

	public override void GetPriceText(string[] lines, ref int currentLine, long price)
	{
		Color lineColor = CurrencyTextColor * (Main.mouseTextColor / 255f);
		lines[currentLine++] = $"[c/{lineColor.R:X2}{lineColor.G:X2}{lineColor.B:X2}:{Lang.tip[50].Value} {price} {Language.GetTextValue(CurrencyTextKey)}]";
		lines[currentLine++] = $"[c/{lineColor.R:X2}{lineColor.G:X2}{lineColor.B:X2}: + {NPCs.Gravedigger.Gravedigger.MoneyToText(Value)} ]";
	}

}

	[AutoloadHead]
	public class Traveler : ModNPC
	{
		public static bool crimson = false;
		public static List<VanillaItemAsCurrency> itemsMoney = new List<VanillaItemAsCurrency>();
		public static List<int> itemsMoneyIDs = new List<int>();
		public const string ShopName = "Shop";
		private Profiles.StackedNPCProfile NPCProfile;


		public void setSkin(bool crimson)
		{
			if (crimson)
			{
				NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture), Texture)
				);
				return;
			}
			var _head_texture = HeadTexture.Replace("Traveler/Traveler", "Traveler/TravelerCrimson"); 
			var _texture = Texture.Replace("Traveler/Traveler", "Traveler/TravelerCrimson"); 
			NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(_texture, NPCHeadLoader.GetHeadSlot(_head_texture), _texture)
			);
		}
		public override void LoadData(TagCompound tag) {
			crimson = tag.GetBool("crimson");
			setSkin(crimson);
		}

		public override void SaveData(TagCompound tag) {
			tag["crimson"] = WorldGen.crimson;
		}

		public override void OnSpawn(IEntitySource source) {
			crimson = WorldGen.crimson;
			setSkin(crimson);
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


		List<int> idsca = new List<int>() {
				ItemID.Vertebrae,
				ItemID.ViciousMushroom,

				ItemID.CrimsonHeart,
				ItemID.TheUndertaker,
				ItemID.PanicNecklace,
				ItemID.CrimsonRod,
				ItemID.TheRottedFork,

				ItemID.BloodWater,
				ItemID.DartPistol,
				ItemID.TendonHook,
				ItemID.FetidBaghnakhs,
				ItemID.SoulDrain,
				ItemID.FleshKnuckles,

				ItemID.Ichor,
				ItemID.RedSolution,
				ItemID.CrimsonKey,
			};

		List<int> idsco = new List<int>() {
				ItemID.RottenChunk,
				ItemID.VileMushroom,
				
				ItemID.ShadowOrb,
				ItemID.Musket,
				ItemID.BandofStarpower,
				ItemID.Vilethorn,
				ItemID.BallOHurt,

				ItemID.UnholyWater,
				ItemID.DartRifle,
				ItemID.WormHook,
				ItemID.ChainGuillotines,
				ItemID.ClingerStaff,
				ItemID.PutridScent,
				ItemID.CursedFlame,
				ItemID.PurpleSolution,

				ItemID.CorruptionKey,
		};

		List<int> prices = new List<int>() {
			Item.buyPrice(0, 1, 0, 0),
			Item.buyPrice(0, 0, 5, 0),

			Item.buyPrice(0, 2, 0, 0),
			Item.buyPrice(0, 2, 0, 0),
			Item.buyPrice(0, 2, 0, 0),
			Item.buyPrice(0, 2, 0, 0),
			Item.buyPrice(0, 2, 0, 0),

			Item.buyPrice(0, 1,  0, 0),
			Item.buyPrice(0, 15, 0, 0),
			Item.buyPrice(0, 15, 0, 0),
			Item.buyPrice(0, 15, 0, 0),
			Item.buyPrice(0, 15, 0, 0),
			Item.buyPrice(0, 15, 0, 0),

			Item.buyPrice(0, 0, 50, 0),
			Item.buyPrice(0, 0, 15, 0),
			Item.buyPrice(1, 50, 0, 0),
		};

		public void loadCripsonItems()
		{			
			int i = -1;
			foreach (var id in idsca)
			{
				itemsMoney.Add(new VanillaItemAsCurrency(id, 9999L, new Item(id).Name, prices[++i]));
				itemsMoneyIDs.Add(CustomCurrencyManager.RegisterCurrency(itemsMoney[itemsMoney.Count - 1]));
			}
		}

		

		public void loadCorruptionItems()
		{
			int i = -1;
			foreach (var id in idsco)
			{
				itemsMoney.Add(new VanillaItemAsCurrency(id, 9999L, new Item(id).Name, prices[++i]));
				itemsMoneyIDs.Add(CustomCurrencyManager.RegisterCurrency(itemsMoney[itemsMoney.Count - 1]));
			}	
		}

		public override void Load()
		{
			Mod.AddNPCHeadTexture(Type, Texture.Replace("Traveler/Traveler", "Traveler/TravelerCrimson_Head"));

			loadCripsonItems();
			loadCorruptionItems();

			

		}


		public override bool CanTownNPCSpawn(int numTownNPCs) { 
			if (!ModContent.GetInstance<Systems.NPCsConfigs>().Traveler) return false;
			return NPC.downedBoss2;
		}

		public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() {
			if (crimson)
				return new List<string>(){
					"Ezra",
					"Liora",
					"Selene",
					"Elara",
					"Kael",
				};

			return new List<string>(){
			"Thorne",
			"Jasper",
			"Tamsin",
			"Ronan",
			"Caius"
		};
		}

		public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();


			chat.Add("Wonders from afar, eager for a trade?");
			chat.Add("Greetings, adventurer! Care to see what treasures I've brought?");
			chat.Add("Exotic goods for your local finds. Let's barter!");
			chat.Add("I've traveled long and far, what unique items do you have?");
			chat.Add("New lands, new items. Fancy a trade?");
			chat.Add("Trade with me, and you'll see wonders from distant realms.");
			chat.Add("Fresh from lands unknown, ready to exchange?");
			chat.Add("Your world is full of curiosities! Show me what you have.");
			chat.Add("I bring rare treasures from beyond the horizon.");
			chat.Add("Let's make a deal! My goods are unlike any you've seen.");
			if (crimson)
			{
				chat.Add("This red hue is quite the sight, isn't it?");
				chat.Add("The corruption is a dangerous place, but it has its own beauty.");
			}
			else
			{
				chat.Add("The crimson is a dangerous place, but it has its own beauty.");
				chat.Add("This purple hue is quite the sight, isn't it?");
			}
			string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.


			return chosenChat;
		}

		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			// if (TravelerAllowed.itemsIDs.Count != 0)
				button = Language.GetTextValue("LegacyInterface.28");
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			// if (firstButton && TravelerAllowed.itemsIDs.Count != 0) {
				shop = ShopName; // Name of the shop tab we want to open.
			// }
		}


		public override void AddShops() {

			var shop = new NPCShop(Type, ShopName);

			for (int i = 0; i < 7; i++)
			{
				shop.Add(new Item(idsco[i]) { shopSpecialCurrency = itemsMoneyIDs[i], shopCustomPrice = 1}, MyConditions.crimpson);
				shop.Add(new Item(idsca[i]) { shopSpecialCurrency = itemsMoneyIDs[idsca.Count + i], shopCustomPrice = 1}, MyConditions.corruption);
			}

			for (int i = 7; i < 8 + 5; i++)
			{
				shop.Add(new Item(idsco[i]) { shopSpecialCurrency = itemsMoneyIDs[i], shopCustomPrice = 1}, MyConditions.crimpsonHardmode);
				shop.Add(new Item(idsca[i]) { shopSpecialCurrency = itemsMoneyIDs[idsca.Count + i], shopCustomPrice = 1}, MyConditions.corruptionHardmode);
			}


			shop.Add(new Item(ItemID.ScourgeoftheCorruptor) { shopSpecialCurrency = itemsMoneyIDs[idsco.Count - 1], shopCustomPrice = 1}, MyConditions.planteraCrimson);
			shop.Add(new Item(ItemID.VampireKnives) { shopSpecialCurrency = itemsMoneyIDs[2 * idsca.Count - 1], shopCustomPrice = 1}, MyConditions.planteraCorruption);
			shop.Register();
			
		}


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