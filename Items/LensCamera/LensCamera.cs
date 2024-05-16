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
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;


namespace BrandonBox.Items.LensCamera
{
	public class LensCamera : ModItem
	{

		public static string  target = "";
		public static int  type = -1;
		public bool consomable = false;
		public Projectile projectile;
		// public static Condition learned = Items.Recipe.RecipeLearner.learned(0);
		// public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MaxUses);
		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 26;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useTurn = true;
			Item.maxStack = 1;
			Item.consumable = true;
			Item.rare = ItemRarityID.Orange;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.value = Item.buyPrice(gold: 200);
		}

		public override void SaveData(TagCompound tag) {
			tag["target"] = target;
			tag["type"] = type;
		}

		public override void LoadData(TagCompound tag) {
			target = tag.GetString("target");
			type = tag.GetInt("type");
		}

		public override void NetSend(BinaryWriter writer) {
			writer.Write(target);
			writer.Write(type);

		}

		public override void NetReceive(BinaryReader reader) {
			target = reader.ReadString();
			type = reader.ReadInt32();
		}

		public override bool ConsumeItem(Player player) {
			return consomable;
		}


		public override bool? UseItem(Player player) {
			if (target == "")
			{
				NPC? t = null;
				var min = 999999999;
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.active && npc.townNPC && npc.type != ModContent.NPCType<NPCs.Villager.Villager>())
					{
						var distance = Vector2.Distance(npc.Center, player.Center);
						if ((int)distance < min)
						{
							min = (int)distance;
							t = npc;
						}
					}
				}

				if (min > 1000 || t == null)
				{
					Main.NewText("No NPC found.", Color.Blue);
					return false;
				}
				type =  t.type;
				target = t.FullName;
				Systems.KeybindSystem.cameras.Add(new Systems.CameraInfos(t.FullName, t.type));
				Main.NewText("Lenses applied to " + t.FullName, Color.Blue);
				return true;
			}
			if (NPC.AnyNPCs(type))
			{
				NPC t = Main.npc.FirstOrDefault(n => n.type == type);
				if (t.FullName != target)
					consomable = true;
				else
				{
					Main.NewText("Lens removed.", Color.Blue);
					Systems.KeybindSystem.cameras.RemoveAll( x => x.targettype == t.type);
				}
			}
			else
				consomable = true;
			target = "";
			type = -1;
			return true;
		}
	}

	public class LensCameraRecipe : Recipe.Recipe
	{
		
		public override string name { get; } = "LensCamera";


		public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<LensCamera>();

			// Start a new Recipe.
			resultItem.CreateRecipe()
				.AddIngredient(ModContent.ItemType<Items.WallCamera.Camera>(), 2)
				.AddIngredient(ItemID.Lens, 2)
				.AddIngredient(ItemID.MechanicalLens, 2)
				// .AddTile(TileID.DemonAltar)
				.AddCondition(Items.Recipe.RecipeLearner.learned(name))
				.Register();
		}
	}
}
