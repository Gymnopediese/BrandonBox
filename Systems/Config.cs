using System.ComponentModel;
using Terraria.ModLoader.Config;
using Terraria;
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
using Terraria.Graphics.CameraModifiers;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using BrandonBox.NPCs.Villager;

namespace BrandonBox.Systems
{

	// public class NPCsKiller : ModSystem {
	// 	public override void OnWorldLoad() {
	// 		// This is called when the world is loaded. This is a good place to set up any variables you need.
	// 		// This is called after all ModSystem classes have been loaded.
	// 	}


	// 	public override void PostWorldLoad() {
	// 		// This is called after all worlds have been loaded. This is a good place to do stuff that requires all worlds to be loaded.
	// 	}

	
	// }

	public class NPCsConfigs : ModConfig
	{
		// ConfigScope.ClientSide should be used for client side, usually visual or audio tweaks.
		// ConfigScope.ServerSide should be used for basically everything else, including disabling items or changing NPC behaviors
		public override ConfigScope Mode => ConfigScope.ServerSide;

		// The things in brackets are known as "Attributes".

		[Header("ToggleNPCs")] // Headers are like titles in a config. You only need to declare a header on the item it should appear over, not every item in the category. 
		// [Label("Warning")] // A label is the text displayed next to the option. This should usually be a short description of what it does. By default all ModConfig fields and properties have an automatic label translation key, but modders can specify a specific translation key.
		[Tooltip("If you save the config, all the existing instance of the Creeper will be removed !")] // A tooltip is a description showed when you hover your mouse over the option. It can be used as a more in-depth explanation of the option. Like with Label, a specific key can be provided.
		[DefaultValue(true)]
		public bool Creeper;
		[Tooltip("If you save the config, all the existing instance of the TheCursed will be removed !")]
		[DefaultValue(true)]
		public bool Cursed;

		[Tooltip("If you save the config, all the existing instance of the DaughterOfSun will be removed !")]
		[DefaultValue(true)]
		public bool DaughterOfSun;

		[Tooltip("If you save the config, all the existing instance of the Death will be removed !")]
		[DefaultValue(true)]
		public bool Death;


		[Tooltip("If you save the config, all the existing instance of the FisherMan will be removed !")]
		[DefaultValue(true)]
		public bool FisherMan;


		[Tooltip("If you save the config, all the existing instance of the Gravedigger will be removed !")]
		[DefaultValue(true)]
		public bool Gravedigger;




		[Tooltip("If you save the config, all the existing instance of the Pickpocket will be removed !")]
		[DefaultValue(true)]
		public bool Pickpocket;

		[Tooltip("If you save the config, all the existing instance of the Poacher will be removed !")]
		[DefaultValue(true)]
		public bool Poacher;

		[Tooltip("If you save the config, all the existing instance of the RainMan will be removed !")]
		[DefaultValue(true)]
		public bool RainMan;

		[Tooltip("If you save the config, all the existing instance of the Ronin will be removed !")]
		[DefaultValue(true)]
		public bool Ronin;


		[Tooltip("If you save the config, all the existing instance of the Traveler will be removed !")]
		[DefaultValue(true)]
		public bool Traveler;

		[Tooltip("If you save the config, all the existing instance of the TVGuy will be removed !")]
		[DefaultValue(true)]
		public bool TVGuy;

		[Tooltip("If you save the config, all the existing instance of the Villager will be removed !")]
		[DefaultValue(true)]
		public bool Villager;

		public override void OnChanged() {

			Dictionary<int, bool> npcs = new Dictionary<int, bool>();
			
			npcs[ModContent.NPCType<NPCs.Creeper.Creeper>()] = Creeper;
			npcs[ModContent.NPCType<NPCs.Cursed.Cursed>()] = Cursed;
			npcs[ModContent.NPCType<NPCs.DaughterOfSun.DaughterOfSun>()] = DaughterOfSun;
			npcs[ModContent.NPCType<NPCs.Death.Death>()] = Death;
			npcs[ModContent.NPCType<NPCs.FisherMan.FisherMan>()] = FisherMan;
			npcs[ModContent.NPCType<NPCs.Gravedigger.Gravedigger>()] = Gravedigger;
			npcs[ModContent.NPCType<NPCs.Pickpocket.Pickpocket>()] = Pickpocket;
			npcs[ModContent.NPCType<NPCs.Poacher.Poacher>()] = Poacher;
			npcs[ModContent.NPCType<NPCs.RainMan.RainMan>()] = RainMan;
			npcs[ModContent.NPCType<NPCs.Ronin.Ronin>()] = Ronin;
			npcs[ModContent.NPCType<NPCs.Traveler.Traveler>()] = Traveler;
			npcs[ModContent.NPCType<NPCs.TVGuy.TVGuy>()] = TVGuy;

			npcs[ModContent.NPCType<NPCs.Villager.Villager>()] = Villager;
			foreach (var npcid in npcs)
			{
				if (npcid.Value)
					continue;
				foreach (var npc in Main.npc)
					if (npc.type == npcid.Key)
						npc.active = false;
			}
		}
	}
}
