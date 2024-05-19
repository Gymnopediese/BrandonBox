// using BrandonBox.Content.Dusts;
// using BrandonBox.Content.EmoteBubbles;
// using BrandonBox.Content.Items;
using BrandonBox.Items.RainWand;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using static Terraria.Rain;

namespace BrandonBox.NPCs.RainMan
{
	/// <summary>
	/// The main focus of this NPC is to show how to make something similar to the vanilla bone merchant;
	/// which means that the NPC will act like any other town NPC but won't have a happiness button, won't appear on the minimap,
	/// and will spawn like an enemy NPC. If you want a traditional town NPC instead, see <see cref="ExamplePerson"/>.
	/// </summary>
	public class RainMan : ModNPC
	{
		private static Profiles.StackedNPCProfile NPCProfile;
		private static Asset<Texture2D> shimmerGun;

		public override void Load() {
			shimmerGun = ModContent.Request<Texture2D>(Texture + "_Shimmer_Gun");
		}

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 25; // The amount of frames the NPC has

			NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs.
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the npc that it tries to attack enemies.
			NPCID.Sets.PrettySafe[Type] = 300;
			NPCID.Sets.AttackType[Type] = 1; // Shoots a weapon.
			NPCID.Sets.AttackTime[Type] = 60; // The amount of time it takes for the NPC's attack animation to be over once it starts.
			NPCID.Sets.AttackAverageChance[Type] = 30;
			NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.
			NPCID.Sets.ShimmerTownTransform[NPC.type] = true; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

			//This sets entry is the most important part of this NPC. Since it is true, it tells the game that we want this NPC to act like a town NPC without ACTUALLY being one.
			//What that means is: the NPC will have the AI of a town NPC, will attack like a town NPC, and have a shop (or any other additional functionality if you wish) like a town NPC.
			//However, the NPC will not have their head displayed on the map, will de-spawn when no players are nearby or the world is closed, and will spawn like any other NPC.
			NPCID.Sets.ActsLikeTownNPC[Type] = true;

			// This prevents the happiness button
			NPCID.Sets.NoTownNPCHappiness[Type] = true;

			//To reiterate, since this NPC isn't technically a town NPC, we need to tell the game that we still want this NPC to have a custom/randomized name when they spawn.
			//In order to do this, we simply make this hook return true, which will make the game call the TownNPCName method when spawning the NPC to determine the NPC's name.
			NPCID.Sets.SpawnsWithCustomName[Type] = true;

		}

		public override void SetDefaults() {
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

		//Make sure to allow your NPC to chat, since being "like a town NPC" doesn't automatically allow for chatting.
		public override bool CanChat() {
			return true;
		}

		public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() {
			return new List<string>  {
            "Drizzle McSizzle",
            "Puddle Jumper",
            "Raindrop Robinson",
            "Misty Waters",
            "Thunderstorm Thompson",
            "Shower Singer",
            "Umbrella Ella",
            "Rainbow Randy",
            "Downpour Doug",
            "Mudslide Martha",
			"Rayan Rain",
        };
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (!ModContent.GetInstance<Systems.NPCsConfigs>().RainMan) return 0f;
			if (spawnInfo.Player.ZoneRain)
				return 0.05f;
			return 0f;
		}

		public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();
			
			chat.Add("Hey there! Looks like another rainy day in Terraria.");
			chat.Add("Did you know that raindrops aren't actually tear-shaped? They're more like tiny spheres!");
			chat.Add("I've heard that in some cultures, rain is considered a blessing from the gods.");
			chat.Add("Rainy days are perfect for cozying up indoors with a good book.");
			chat.Add("Take care! Don't forget your umbrella if you're heading out.");
			chat.Add("Need anything for this rainy weather? I've got umbrellas, raincoats, and waterproof boots!");
			chat.Add("If you ever need advice on dealing with rain-related challenges, just ask! I'm here to help.");
			chat.Add("I'm from a mysterious greyscale cube world.");
			chat.Add("Looks like it's raining cats and dogs out there!");
			return chat;
		}

		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			button = Language.GetTextValue("LegacyInterface.28"); //This is the key to the word "Shop"
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (firstButton) {
				shop = "Shop";
			}
		}

		public override void AddShops() {

			WeightedRandom<int> random = new WeightedRandom<int>();

			random.Add(ItemID.Umbrella, 0.4);
			random.Add(ItemID.RainHat, 0.3);
			random.Add(ItemID.RainCoat, 0.3);
			// random.Add(ItemID.RainSlicker, 1);
			// random.Add(ItemID.RainBoots, 1);
			random.Add(ItemID.UmbrellaHat, 0.5);
			random.Add(ItemID.Fish, 0.2);
			if (Main.LocalPlayer.anglerQuestsFinished > 35)
				random.Add(ItemID.BottomlessBucket, 0.1f);
			random.Add(ItemID.WaterWalkingPotion, 0.6f);
			random.Add(ItemID.WaterWalkingBoots, 0.05f);
			random.Add(ModContent.ItemType<RainWand>(), 0.1f);
			if (NPC.downedBoss3)
			{
				random.Add(ItemID.WaterCandle, 0.1f);
				random.Add(ItemID.WaterBolt, 0.1f);
			}
			random.Add(ItemID.WaterGun, 0.3f);
			random.Add(ItemID.Waterleaf, 0.5f);
			
			random.Add(ItemID.BottledWater, 1f);
			random.Add(ItemID.WaterBucket, 0.4f);
			random.Add(ItemID.WetBomb, 0.3f);
			random.Add(ItemID.AquaScepter, 0.2f);
			random.Add(ItemID.RainCloud, 0.4f);
			if (Main.hardMode) {
				random.Add(ItemID.HolyWater, 0.2f);
				random.Add(ItemID.UnholyWater, 0.2f);
				random.Add(ItemID.BloodWater, 0.2f);
				random.Add(ItemID.WaterfallBlock, 0.2f);
			}


			var item_amount = Main.rand.Next(3, 7);



			var shop = new NPCShop(Type);

			for (int i = 0; i < item_amount; i++) {
				shop.Add(random.Get());
			}

			shop.Register();
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback) {
			damage = 20;
			knockback = 2f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) {
			cooldown = 10;
			randExtraCooldown = 1;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay) {
			projType = ProjectileID.NanoBullet;
			attackDelay = 1;

			// This code progressively delays subsequent shots.
			if (NPC.localAI[3] > attackDelay) {
				attackDelay = 12;
			}
			if (NPC.localAI[3] > attackDelay) {
				attackDelay = 24;
			}
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) {
			multiplier = 10f;
			randomOffset = 0.2f;
		}

		public override void TownNPCAttackShoot(ref bool inBetweenShots) {
			if (NPC.localAI[3] > 1) {
				inBetweenShots = true;
			}
		}

		// public override void DrawTownAttackGun(ref Texture2D item, ref Rectangle itemFrame, ref float scale, ref int horizontalHoldoutOffset) {
		// 	if (!NPC.IsShimmerVariant) {
		// 		// If using an existing item, use this approach
		// 		int itemType = ModContent.ItemType<ExampleCustomAmmoGun>();
		// 		Main.GetItemDrawFrame(itemType, out item, out itemFrame);
		// 		horizontalHoldoutOffset = (int)Main.DrawPlayerItemPos(1f, itemType).X - 12;
		// 	}
		// 	else {
		// 		// This texture isn't actually an existing item, but can still be used.
		// 		item = shimmerGun.Value;
		// 		itemFrame = item.Frame();
		// 		horizontalHoldoutOffset = -2;
		// 	}
		// }
	}
}
