using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace BrandonBox.NPCs.Protector
{
	public class ProtectorPay : ModSystem
	{
		public static bool paid = false;
		public override void PreUpdateTime() {
			if (!NPC.AnyNPCs(ModContent.NPCType<Protector>()))
				return;
			if (Main.dayTime && Main.time == 0)
			{
				if (paid == false)
				{
					NPC protector = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<Protector>())];
					protector.active = false;
					Main.NewText("The Protector has left, you couldn't pay the tax...", Color.Gold);
				}
				paid = false;
			}
			else if (Main.dayTime == false && Main.time % 3600 == 0 && paid == false)
			{
				foreach (Player player in Main.player)
				{
					if (player.BuyItem(50000))
					{
						paid = true;
						Main.NewText($"The Protector has been paid by {player.name}!", Color.Gold);
						break;
					}

				}
			}
		}
		public override void SaveWorldData(TagCompound tag) {
			tag["paid"] = paid;
		}

		public override void LoadWorldData(TagCompound tag) {
			paid = tag.GetBool("paid");
		}
	}
	public class ProtectorPowers : GlobalNPC
	{
		public override bool CanBeHitByNPC(NPC npc, NPC attacker)
		{
			if (npc.townNPC && NPC.AnyNPCs(ModContent.NPCType<Protector>()))
				return false;
			return base.CanBeHitByNPC(npc, attacker);
		}
	}

	// [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
	[AutoloadHead]
	public class Protector : ModNPC
	{
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

			if (!ModContent.GetInstance<Systems.NPCsConfigs>().Protector) return false;
			foreach (Player player in Main.player)
			{
				if (player.CountItem(ItemID.PlatinumCoin) >= 100)
				{
					Main.NewText("enought");
					return true;
				}
			}
			Main.NewText("not enohgru");
			return false;
		}

		public override bool CheckConditions(int left, int right, int top, int bottom) {
			int score = 0;
			for (int x = left; x <= right; x++)
			{
				for (int y = top; y <= bottom; y++) 
				{
					if (Main.tile[x, y].TileType == TileID.GoldCoinPile)
						score++;
				}
			}
			// Main.NewText($"{score} {left} {right} {top} {bottom} {Main.LocalPlayer.position}");
			Main.NewText(score + " " + left + " "  + right + " " + top + " " + right + " " + Main.LocalPlayer.position / 16);
			return score >= 100;
		}

		public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() {
			return [
	"Aurum the Greedy",
	"Opulentus",
	"Gildor",
	"Coinus",
	"Luxor",
	"Plutarch",
	"Treasureius",
	"Avaritus",
	"Goldrend",
	"Midas"];
		}

		public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();
			chat.Add("You are but an insect to me, but your gold is welcome.");
			chat.Add("Bow before me and your riches shall flourish.");
			chat.Add("In my presence, wealth flows like a river.");
			chat.Add("You dare summon me? Prove your worth with platinum.");
			chat.Add("Serve me gold, and I shall protect your pitiful existence.");
			chat.Add("Your survival is bought by the weight of your coin.");
			chat.Add("Remember, mortal, my protection isn't cheap.");
			chat.Add("Without your tribute, I vanish like a whisper in the wind.");
			chat.Add("Gold speaks louder than words, remember that.");
			chat.Add("My power is as vast as your treasure is heavy.");

			string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.


			return chosenChat;
		}

		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			// button = Language.GetTextValue("LegacyInterface.28");
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			// if (firstButton) {
			// 	shop = ShopName; // Name of the shop tab we want to open.
			// }
		}



		// Make this Town NPC teleport to the King and/or Queen statue when triggered. Return toKingStatue for only King Statues. Return !toKingStatue for only Queen Statues. Return true for both.
		public override bool CanGoToStatue(bool toKingStatue) => true;

		// Make something happen when the npc teleports to a statue. Since this method only runs server side, any visual effects like dusts or gores have to be synced across all clients manually.
		
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