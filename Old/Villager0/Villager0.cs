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
using BrandonBox.Items.VillagePermit;
// using BrandonBox.Tiles;
namespace BrandonBox.NPCs.Villager.Villager0
{
	// [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
	[AutoloadHead]
	public class Villager0 : ModNPC
	{

		private Profiles.StackedNPCProfile NPCProfile;
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 25; // The total amount of frames the NPC has
			NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
			NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the NPC that it tries to attack enemies.
			NPCID.Sets.HatOffsetY[Type] = 4; 
		}

		public override void SetDefaults() {
			NPC.townNPC = true; // Sets NPC to be a Town NPC
			NPC.friendly = true; // NPC Will not attack player
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			// NPC.Texture = ModContent.Request<Texture2D>(Texture.Replace("Villager/Villager", "Villager/Villager" + Main.rand.Next(6))).Value;
			AnimationType = NPCID.Guide;

		}

		public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() {
			return new List<string>(){ "randomboy" };
		}

		public override void FindFrame(int frameHeight) {

		}

		public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();
			
			chat.Add("I am a villager.");
			string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.

			return chosenChat;
		}

		public override bool CheckConditions(int left, int right, int top, int bottom) {
			int score = 0;
			for (int x = left; x <= right; x++) {
				for (int y = top; y <= bottom; y++) {
					if (Main.tile[x, y].TileType == Tiles.SafeZoneCertificat._type) {
						score++;
					}
				}
			}
			return score >= 2;
		}

	
		public override bool CanGoToStatue(bool toKingStatue) => true;

	}
}