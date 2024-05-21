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
namespace BrandonBox.Items.InfiniteSandBucket
{


	public class InfiniteSandBucket : ModItem
	{
		public override void SetDefaults() {
			// Vanilla has many useful methods like these, use them! This substitutes setting Item.createTile and Item.placeStyle as well as setting a few values that are common across all placeable items
			Item.DefaultToPlaceableTile(TileID.Sand);

			Item.placeStyle = TileID.WireBulb;
			Item.width = 16;
			Item.height = 16;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 1);
			Item.maxStack = 1;
			Item.consumable = false;
		}

		public override bool? UseItem ( Player player)
		{
			return true;
		}

	}


}
