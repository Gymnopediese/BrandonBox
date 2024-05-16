using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
// using static Terraria.Rain;
using BrandonBox.Items.Recipe;
using Terraria.DataStructures;
using Terraria.ObjectData;
namespace BrandonBox.Items.WallCamera
{
	public class CameraTile : ModTile
		{
		public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true;
			
			TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(TileID.WireBulb, 0));

			TileObjectData.addTile(Type);
			AddMapEntry(new Color(120, 85, 60), Language.GetText("MapObject.Trophy"));
			DustType = 7;
		}

		public override void 	KillTile (int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			Systems.KeybindSystem.cameras.RemoveAll(x => new Vector2((int)x.position.X, (int)x.position.Y) == new Vector2(i, j));
			Systems.PlayerModifier.SetCamera();
		}

		public override void AnimateTile(ref int frame, ref int frameCounter) {
			if (++frameCounter >= 10) {
				frameCounter = 0;
				frame = ++frame % 13;
			}
		}

		public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset) {
			var tile = Main.tile[i, j];
			if (tile.TileFrameY < 18) {
				frameYOffset = Main.tileFrame[type] * 18;
			}
			else {
				frameYOffset = 252;
			}
		}
	}

	public class Camera : ModItem
	{
		public override void SetDefaults() {
			// Vanilla has many useful methods like these, use them! This substitutes setting Item.createTile and Item.placeStyle as well as setting a few values that are common across all placeable items
			Item.DefaultToPlaceableTile(ModContent.TileType<CameraTile>());

			Item.width = 16;
			Item.height = 16;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 1);
		}

		public override void OnConsumeItem(	Player 	player	)	{
			Systems.KeybindSystem.cameras.Add(new Systems.CameraInfos(Main.MouseWorld / 16));
		}
	}

	public class WallCameraRecipe : Recipe.Recipe
	{
		
		public override string name { get; } = "WallCamera";

		public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.WallCamera.Camera>();

			// Start a new Recipe.
			resultItem.CreateRecipe()
				.AddIngredient(ItemID.SuspiciousLookingEye)
				.AddIngredient(ItemID.Lens, 3)
				.AddIngredient(ItemID.IronBar, 5)
				.AddIngredient(ItemID.Wire, 20)
				.AddTile(TileID.DemonAltar)
				.AddCondition(Items.Recipe.RecipeLearner.learned(name))
				.Register();
		}
	}
}
