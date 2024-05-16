
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
// using Terraria.Projectile;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.DataStructures;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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



namespace BrandonBox.Systems
{
	public struct CameraInfos
	{
		public CameraInfos(Vector2 position)
		{
			this.position = position;
			targetname = "";
			targettype = -1;
		}

		public CameraInfos(string targetname, int targettype)
		{
			this.targetname = targetname;
			this.targettype = targettype;
			position = new Vector2(-1, -1);
		}


		public CameraInfos(string targetname, int targettype, Vector2 position)
		{
			this.targetname = targetname;
			this.targettype = targettype;
			this.position = position;
		}

		public Vector2 position { get; }
		public string targetname { get; }
		public int targettype { get; }

		// public override string ToString() => $"({X}, {Y})";
	}

	public class Cam : ICameraModifier
	{
		public string UniqueIdentity { get; set; } = "CameraModifier";
		public bool Finished { get; set; } = false;
		public int count = 0;

		public Vector2? target;
		public Projectile? projectile;
		public NPC? npc;

		public Cam(NPC npc) {
			this.npc = npc;
			Main.instance.CameraModifiers.Add(this);
		}
		public Cam(Projectile projectile) {
			this.projectile = projectile;
			Main.instance.CameraModifiers.Add(this);
		}
		

		public Cam(Vector2 target) {
			this.target = target;
			Main.instance.CameraModifiers.Add(this);
		}

		public void finish() {Finished = true;}

		public void Update (ref CameraInfo cameraPosition) {
			if (npc != null)
				cameraPosition.CameraPosition = npc.Center -  new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
			else if (projectile != null)
				cameraPosition.CameraPosition = projectile.Center -  new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
			else if (target != null)
				cameraPosition.CameraPosition = (target ?? Vector2.Zero) -  new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
		}

		
	}

	public class KeybindSystem : ModSystem
	{
		public static ModKeybind NextCamera { get; private set; }	
		public static ModKeybind PreviousCamera { get; private set; }
		public static int camera = 0;
		public static Cam? modifier = null;

		public static List<CameraInfos> cameras = new List<CameraInfos>();

		public override void Load() {
			NextCamera = KeybindLoader.RegisterKeybind(Mod, "NextCamera", "I");
			PreviousCamera = KeybindLoader.RegisterKeybind(Mod, "PreviousCamera", "O");
		}

		// Please see ExampleMod.cs' Unload() method for a detailed explanation of the unloading process.
		public override void Unload() {
			// Not required if your AssemblyLoadContext is unloading properly, but nulling out static fields can help you figure out what's keeping it loaded.
			NextCamera = null;
			PreviousCamera = null;
		}


		public override void SaveWorldData(TagCompound tag) {
			tag["camera"] = camera;
			for (int i = 0; i < cameras.Count; i++)
			{
				tag["cameraTN" + i] = cameras[i].targetname;
				tag["cameraTT" + i] = cameras[i].targettype;
				tag["cameraP" + i] = cameras[i].position;
			}
		}

		public override void LoadWorldData(TagCompound tag) {
			camera = tag.GetInt("camera");
			cameras = new List<CameraInfos>();
			for (int i = 0; tag.ContainsKey("cameraTN" + i); i++)
				cameras.Add(new CameraInfos(tag.GetString("cameraTN" + i), tag.GetInt("cameraTT" + i), tag.Get<Vector2>("cameraP" + i)));
		}
	}
	public class PlayerModifier : ModPlayer
	{
		public override void ProcessTriggers(TriggersSet triggersSet) {
			if (KeybindSystem.NextCamera.JustPressed)
				KeybindSystem.camera++;
			else if (KeybindSystem.PreviousCamera.JustPressed)
				KeybindSystem.camera--;
			else
				return;	

			SetCamera();
		}

		public static void skipDestroyedCameras()
		{
			while (KeybindSystem.cameras.Count > 0
				&& ((KeybindSystem.cameras[KeybindSystem.camera - 1].targetname == "" && Main.tile[(int)KeybindSystem.cameras[KeybindSystem.camera - 1].position.X , (int)KeybindSystem.cameras[KeybindSystem.camera - 1].position.Y ].TileType != ModContent.TileType<Items.WallCamera.CameraTile>())
				|| (KeybindSystem.cameras[KeybindSystem.camera - 1].targetname != "" && (!NPC.AnyNPCs((int)KeybindSystem.cameras[KeybindSystem.camera - 1].targettype) || Main.npc[NPC.FindFirstNPC((int)KeybindSystem.cameras[KeybindSystem.camera - 1].targettype)].FullName != KeybindSystem.cameras[KeybindSystem.camera - 1].targetname))))
			{
				KeybindSystem.cameras.RemoveAt(KeybindSystem.camera - 1);
				KeybindSystem.camera = Math.Min(KeybindSystem.camera, KeybindSystem.cameras.Count);
			}
		}

		public static void SetCamera ()
		{
			if (KeybindSystem.camera == -1)
				KeybindSystem.camera = KeybindSystem.cameras.Count;
			KeybindSystem.camera %= (KeybindSystem.cameras.Count + 1); 

			if (KeybindSystem.cameras.Count == 0)
			{
				if (KeybindSystem.modifier != null)
					KeybindSystem.modifier.finish();
				Main.NewText("You have no cameras", Color.Blue);
				return ;
			}
			// Main.NewText("brooo" + KeybindSystem.camera + " " + KeybindSystem.cameras.Count + " " + KeybindSystem.cameras[KeybindSystem.camera - 1]);
			
			if (KeybindSystem.camera > 0) {
				skipDestroyedCameras();
				if (KeybindSystem.camera > 0)
				{
					if (KeybindSystem.cameras[KeybindSystem.camera - 1].targetname != "")
					{
						NPC t = Main.npc[NPC.FindFirstNPC((int)KeybindSystem.cameras[KeybindSystem.camera - 1].targettype)];
						KeybindSystem.modifier = new Cam(t);
						Main.NewText("Looking at Camera Lenses of " + t.FullName + " " + KeybindSystem.camera + " / " + KeybindSystem.cameras.Count, Color.Blue);
						return ;
					}
					else
					{
						KeybindSystem.modifier = new Cam(KeybindSystem.cameras[KeybindSystem.camera - 1].position * 16);
						Main.NewText("Looking at Camera " + KeybindSystem.camera + " / " + KeybindSystem.cameras.Count, Color.Blue);
						return ;
					}
				}
			}
			Main.NewText("Back to player", Color.Blue);
			if (KeybindSystem.modifier != null)
				KeybindSystem.modifier.finish();
			KeybindSystem.modifier = null;
		}
	}	
}
