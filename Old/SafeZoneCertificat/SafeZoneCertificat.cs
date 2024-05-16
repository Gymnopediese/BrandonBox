using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
// using BrandonBox.Tiles;

namespace BrandonBox.Items.SafeZoneCertificat
{
	public class SafeZoneCertificat : ModItem
	{
		public override void SetDefaults() {
			// ModContent.TileType<Tiles.Furniture.ExampleWorkbench>() retrieves the id of the tile that this item should place when used.
			// DefaultToPlaceableTile handles setting various Item values that placeable items use
			// Hover over DefaultToPlaceableTile in Visual Studio to read the documentation!
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.SafeZoneCertificat>());
			Item.width = 32; // The item texture's width
			Item.height = 52; // The item texture's height
			Item.value = 150;
		}

		// public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup) {
		// 	itemGroup = ContentSamples.CreativeHelper.ItemGroup.CraftingObjects;
		// }

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		// public override void AddRecipes() {
		// 	CreateRecipe()
		// 		.AddIngredient(ItemID.WorkBench)
		// 		.AddIngredient<ExampleItem>(10)
		// 		.Register();
		// }
	}
}
