using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;

namespace BrandonBox.NPCs.Villager
{
	[AutoloadHead]
	public class Villager : ModNPC
	{
		public static readonly int SPRITE_COUNT = 1000;
		public const string ShopName = "Shop";
		private string _texture;
		private string _head_texture;
		private int _num;
		bool spawned = false;
		private Profiles.StackedNPCProfile NPCProfile;
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 25; // The total amount of frames the NPC has
			NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
			NPCID.Sets.AttackFrameCount[Type] = 4; // The amount of frames in the attacking animation.
			NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the NPC that it tries to attack enemies.
			NPCID.Sets.HatOffsetY[Type] = 4; 
			NPCID.Sets.TownCritter[Type] = true; 
			NPCID.Sets.NoTownNPCHappiness[Type] = true;
			NPCID.Sets.ActsLikeTownNPC[Type] = true;
			
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
			OnSpawn(null);
			if (!ModContent.GetInstance<Systems.NPCsConfigs>().Villager)
				NPC.active = false;
		}

		public override void OnSpawn (IEntitySource source)
		{
			if (spawned)
				return;
			_num = Main.rand.Next(SPRITE_COUNT);
			_texture = Texture.Replace("Villager/Villager", "Villager/Villager" + _num);
			_head_texture = HeadTexture.Replace("Villager/Villager", "Villager/Villager" + _num);
			NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(_texture, NPCHeadLoader.GetHeadSlot(_head_texture), _texture)
			);
		}

		public override ITownNPCProfile TownNPCProfile() {
			return NPCProfile;
		}

		public override List<string> SetNPCNameList() {
			return new List<string>()  {
            "Alex",
            "Jordan",
            "Casey",
            "Taylor",
            "Riley",
            "Charlie",
            "Jamie",
            "Dakota",
            "Avery",
            "Harper",
            "Rowan",
            "Quinn",
            "Finley",
            "Skyler",
            "Peyton",
            "Sage",
            "Emerson",
            "Reese",
            "Armani",
            "Phoenix",
            "Blair",
            "Elliot",
            "Cameron",
            "Aubrey",
            "Morgan",
            "Sawyer",
            "Tatum",
            "Adrian",
            "Angel",
            "Bailey",
            "Blake",
            "Brook",
            "Carey",
            "Devon",
            "Hayden",
            "Jesse",
            "Kim",
            "Mackenzie",
            "Parker",
            "Robin",
            "Shay",
            "Terry",
            "Shannon",
            "Casey",
            "Aspen",
            "Dominique",
            "Elisha",
            "Jody",
            "Justice",
            "Lane",
            "Marley",
            "Micah",
            "Nicky",
            "Noel",
            "Peyton",
            "Remy",
            "Rory",
            "Sam",
            "Sidney",
            "Terry",
            "Tyler"
        };
		}

		public override string GetChat() {

			
			var chat = new string[]{
            "Hey there, how's it going?",
            "Nice weather we're having today, huh?",
            "Just heading out to do some errands.",
            "Got any plans for the day?",
            "I'm thinking of trying out that new recipe I found.",
            "Did you catch the game last night?",
            "Need any help with anything?",
            "I'm so tired, didn't get much sleep last night.",
            "Got some fresh bread from the bakery, want some?",
            "I heard there's a sale at the market today.",
            "Just taking a stroll, enjoying the sunshine.",
            "I think I left my keys at home again.",
            "Have you seen my cat? I swear he's always running off.",
            "Thinking of redecorating my living room, any ideas?",
            "I can't believe it's already May, time flies!",
            "Forgot to water my plants again, hope they're not dead.",
            "Thinking of going for a swim later, wanna join?",
            "Have you met the new neighbor yet?",
            "I should really clean out my closet, it's a mess.",
            "Do you know if the library is open today?",
            "I'm craving some ice cream, wanna go grab a cone?",
            "Just finished a good book, looking for recommendations.",
            "I need to get my car serviced, it's making a weird noise.",
            "Got any plans for the weekend?",
            "I heard there's a movie marathon at the theater tonight.",
            "Trying to cut down on caffeine, but I can't resist my morning coffee.",
            "Thinking of starting a new hobby, any suggestions?",
            "I should probably start exercising more, but it's so hard to get motivated.",
            "Just got a new puzzle, wanna help me solve it?",
            "Do you know if the park is crowded today?",
            "Thinking of getting a haircut, what do you think?",
            "I have so much laundry to do, it never ends.",
            "Do you believe in ghosts?",
            "I'm thinking of adopting a pet, but I'm not sure if I'm ready for the responsibility.",
            "Got any plans for the holidays?",
            "Just got back from vacation, already missing the beach.",
            "Thinking of trying out that new restaurant downtown, wanna come with?",
            "I need to buy a gift for my mom's birthday, any ideas?",
            "Just finished watching a good movie, looking for recommendations.",
            "Thinking of starting a garden, but I'm not sure where to begin.",
            "I'm so hungry, what should I have for lunch?",
            "Got any plans for the evening?",
            "Just finished my workout, feeling great!",
            "Thinking of going for a hike this weekend, wanna join?",
            "I'm so excited for the concert next week, it's gonna be amazing!",
            "Got any fun weekend plans?",
            "Just got a new video game, wanna play?",
            "Thinking of redecorating my bedroom, any suggestions?",
            "I need to go grocery shopping, but I'm too lazy to leave the house.",
            "Thinking of getting a tattoo, what do you think?",
            "I'm so bored, need something to do.",
            "Just got a new book, wanna borrow it?",
            "Thinking of taking up painting, but I'm not sure if I'm any good.",
            "I'm so stressed out, need to find a way to relax.",
            "Thinking of going vegan, but I'm not sure if I can give up cheese.",
            "I need to call my mom, it's been ages since we talked.",
            "Thinking of going camping this summer, wanna come?",
            "I'm so tired of this rainy weather, when will it end?",
            "Thinking of getting a new car, any recommendations?",
            "I'm so excited for the new season of my favorite show, can't wait to binge-watch it.",
            "Thinking of learning a new language, but I'm not sure which one.",
            "I'm so sick of my job, need to find something new.",
            "Thinking of going to the beach this weekend, wanna come?",
            "I'm so hungry, what should I make for dinner?",
            "Thinking of getting a dog, but I'm not sure if I'm ready for the responsibility.",
            "I'm so bored, need some entertainment.",
            "Thinking of going to a concert next month, wanna come?",
            "I'm so tired, didn't get much sleep last night.",
            "Thinking of going for a bike ride this afternoon, wanna join?",
            "I'm so excited for the new museum exhibit, it looks amazing!",
            "Thinking of taking up yoga, but I'm not sure if I'm flexible enough.",
            "I'm so stressed out, need to find a way to unwind.",
            "Thinking of going on a road trip this summer, wanna come?",
            "I'm so sick of this cold weather, when will spring arrive?",
            "Thinking of getting a new phone, but I'm not sure which one to choose.",
            "I'm so excited for the new restaurant opening, the menu looks delicious!",
            "Thinking of starting a book club, wanna join?",
            "I'm so hungry, need to grab a snack.",
            "Thinking of going to a music festival this summer, wanna come?",
            "I'm so tired, need to take a nap.",
            "Thinking of taking up photography, but I'm not sure if I have an eye for it.",
            "I'm so bored, need something fun to do.",
            "Thinking of going to the farmer's market this weekend, wanna come?",
            "I'm so excited for the new movie coming out, it looks epic!",
            "Thinking of going for a run this morning, wanna join?",
            "I'm so stressed out, need to find a way to relax.",
            "Thinking of going on a picnic this afternoon, wanna come?",
            "I'm so sick of this traffic, need to find a quicker route.",
            "Thinking of getting a new laptop, but I'm not sure which one to get.",
            "I'm so excited for the new season of my favorite TV show, can't wait to binge-watch it.",
            "Thinking of going to the beach this weekend, wanna come?",
            "I'm so hungry, what should I have for breakfast?",
            "Thinking of taking up painting, but I'm not sure if I'm any good.",
            "I'm so bored, need something to do.",
            "Thinking of going to a concert next month, wanna come?",
            "I'm so tired, didn't get much sleep last night.",
            "Thinking of going for a bike ride this afternoon, wanna join?",
            "I'm so excited for the new museum exhibit, it looks amazing!",
            "Thinking of taking up yoga, but I'm not sure if I'm flexible enough.",
            "I'm so stressed out, need to find a way to unwind.",
            "Thinking of going on a road trip this summer, wanna come?",
            "I'm so sick of this cold weather, when will spring arrive?",
            "Thinking of getting a new phone, but I'm not sure which one to choose.",
            "I'm so excited for the new restaurant opening, the menu looks delicious!",
            "Thinking of starting a book club, wanna join?",
            "I'm so hungry, need to grab a snack.",
            "Thinking of going to a music festival this summer, wanna come?",
            "I'm so tired, need to take a nap.",
            "Thinking of taking up photography, but I'm not sure if I have an eye for it.",
            "I'm so bored, need something fun to do.",
            "Thinking of going to the farmer's market this weekend, wanna come?",
            "I'm so excited for the new movie coming out, it looks epic!",
            "Thinking of going for a run this morning, wanna join?",
            "I'm so stressed out, need to find a way to relax.",
            "Thinking of going on a picnic this afternoon, wanna come?",
            "I'm so sick of this traffic, need to find a quicker route.",
            "Thinking of getting a new laptop, but I'm not sure which one to get.",
            "I'm so excited for the new season of my favorite TV show, can't wait to binge-watch it.",
            "Thinking of going to the beach this weekend, wanna come?",
            "I'm so hungry, what should I have for breakfast?",
			"Did you ever felt like you're being watched?",
			"Did you ever felt like we were in a simulation?",
			"Did you ever felt like we were in a 2D Crafting Game?",
        };
			
			setMain();


			return chat[Main.rand.Next(chat.Length)];
		}

		public void setMain()
		{
			int i = 0;
			for (; i < Main.maxNPCs; i++)
				if (Main.npc[i].type == ModContent.NPCType<Villager>())
					break ;
			if (i == Main.npc.Length || i == Main.LocalPlayer.talkNPC)
				return;
			var temp = Main.npc[i];
			Main.npc[i] = Main.npc[Main.LocalPlayer.talkNPC];
			Main.npc[Main.LocalPlayer.talkNPC] = temp;
			Main.LocalPlayer.SetTalkNPC(i);
		}
		public override void Load() {
			for (int i = 0; i < SPRITE_COUNT; i++)
				Mod.AddNPCHeadTexture(Type, Texture.Replace("Villager/Villager", "Villager/Villager" + i + "_Head"));
		}
		public override void LoadData(TagCompound tag) {
			try
			{
				_texture = tag.GetString("Texture");
				_head_texture = tag.GetString("HeadTexture");
				_num = tag.GetInt("Num");
				NPCProfile = new Profiles.StackedNPCProfile(
					new Profiles.DefaultNPCProfile(_texture, NPCHeadLoader.GetHeadSlot(_head_texture), _texture)
				);
			}
			catch (System.Exception e)
			{
				OnSpawn(null);
			}
		}
		public override void SaveData(TagCompound tag) {
			tag["Texture"] = _texture;
			tag["HeadTexture"] = _head_texture;
			tag["Num"] = _num;
		}
		public override bool CheckConditions(int left, int right, int top, int bottom) {
			int score = 0;
			for (int x = left; x <= right; x++) {
				for (int y = top; y <= bottom; y++) {
					if (Main.tile[x, y].TileType == TileID.Beds)
					{
						score++;
					}
				}
			}
			return score >= 1;
		}
	}
}