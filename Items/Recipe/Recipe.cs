using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using static Terraria.ModLoader.ModContent;

namespace BrandonBox.Items.Recipe
{
	public class RecipeLearner : ModSystem
	{
		public static List<string> learnedRecipes = new List<string>();
		public static Condition learned(string name)
		{
			return new Condition("item " + name + " learned", () => learnedRecipes.Contains(name));
		}
		public override void SaveWorldData(TagCompound tag) {
			for (int i = 0; i < learnedRecipes.Count; i++) {
				tag["learnedRecipe" + i] = learnedRecipes[i];
			}
		}

		public override void LoadWorldData(TagCompound tag) {
			for (int i = 0; tag.ContainsKey("learnedRecipe" + i); i++)
				learnedRecipes.Add(tag.GetString("learnedRecipe" + i));
		}

	}
	public abstract class Recipe : ModItem
	{

		// public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs("salut");

		public abstract string name { get; }
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
			Item.value = Item.buyPrice(gold: 10);
		}


		public override bool? UseItem(Player player) {
			// Main.NewText("ConsumeItem" + " " + id + " " + RecipeLearner.learnedRecipes.Contains(id));
			if (!RecipeLearner.learnedRecipes.Contains(name)) {
				RecipeLearner.learnedRecipes.Add(name);
				Main.NewText("You learned to create a" + " " + name + "!");
				return true;
			}
			return false;
		}
	}
}
