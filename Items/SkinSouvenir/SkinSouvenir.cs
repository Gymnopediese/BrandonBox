using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BrandonBox.Items.SkinSouvenir
{	

	public class SkinSouvenir : ModItem
	{
		int hair;
		int skinVariant;
		Color hairColor;
		Color skinColor;
		Color eyeColor;
		Color shirtColor;
		Color underShirtColor;
		Color pantsColor;
		Color shoeColor;

		private bool  used = false;

		public Projectile projectile;
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

		public override bool ConsumeItem(Player player) {
			return false;
		}



		public override bool? UseItem(Player player) {
			if (used)
			{
				player.hair = hair;
				player.skinVariant = skinVariant;
				player.hairColor = hairColor;
				player.skinColor = skinColor;
				player.eyeColor = eyeColor;
				player.shirtColor = shirtColor;
				player.underShirtColor = underShirtColor;
				player.pantsColor = pantsColor;
				player.shoeColor = shoeColor;
				return true;
			}
			hair = player.hair;
			skinVariant = player.skinVariant;
			hairColor = player.hairColor;
			skinColor = player.skinColor;
			eyeColor = player.eyeColor;
			shirtColor = player.shirtColor;
			underShirtColor = player.underShirtColor;
			pantsColor = player.pantsColor;
			shoeColor = player.shoeColor;
			used = true;
			return true;
		}

		public override void SaveData(TagCompound tag)
		{
			if (!used) return;
			tag["used"] = used;
			tag["hair"] = hair;
			tag["skinVariant"] = skinVariant;
			tag["hairColor"] = hairColor.PackedValue;
			tag["skinColor"] = skinColor.PackedValue;
			tag["eyeColor"] = eyeColor.PackedValue;
			tag["shirtColor"] = shirtColor.PackedValue;
			tag["underShirtColor"] = underShirtColor.PackedValue;
			tag["pantsColor"] = pantsColor.PackedValue;
			tag["shoeColor"] = shoeColor.PackedValue;
		}

		public override void LoadData(TagCompound tag)
		{
			used = tag.GetBool("used");
			if (!used) return;
			hair = tag.GetInt("hair");
			skinVariant = tag.GetInt("skinVariant");
			hairColor = tag.Get<Color>("hairColor");
			skinColor = tag.Get<Color>("skinColor");
			eyeColor = tag.Get<Color>("eyeColor");
			shirtColor = tag.Get<Color>("shirtColor");
			underShirtColor = tag.Get<Color>("underShirtColor");
			pantsColor = tag.Get<Color>("pantsColor");
			shoeColor = tag.Get<Color>("shoeColor");
		}
	}

	// public class EyeCameraRecipe : Recipe.Recipe
	// {
		
	// 	public override string name { get; } = "EyeCamera";


	// 	public override void AddRecipes() {
	// 		var resultItem = ModContent.GetInstance<Items.EyeCamera.EyeCamera>();

	// 		// Start a new Recipe.
	// 		resultItem.CreateRecipe()
	// 			.AddIngredient(ModContent.ItemType<Items.WallCamera.Camera>())
	// 			.AddIngredient(ItemID.AdamantiteBar, 5)
	// 			// .AddTile(TileID.DemonAltar)
	// 			.AddCondition(Items.Recipe.RecipeLearner.learned(name))
	// 			.Register();

	// 		resultItem.CreateRecipe()
	// 			.AddIngredient(ModContent.ItemType<Items.WallCamera.Camera>())
	// 			.AddIngredient(ItemID.TitaniumBar, 5)
	// 			// .AddTile(TileID.DemonAltar)
	// 			.AddCondition(Items.Recipe.RecipeLearner.learned(name))
	// 			.Register();
	// 	}
	// }
}
