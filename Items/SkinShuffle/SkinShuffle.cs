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
// using static Terraria.Rain;

namespace BrandonBox.Items.SkinShuffle
{
	public class SkinShuffle : ModItem
	{
		public static readonly int MaxUses = 10;

		public int useCount;

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MaxUses);

		List<Color> skinColors = new List<Color>()
		{
			// Fair/Light Skin: RGB(255, 240, 219)
			new Color(255, 240, 219),

			// Medium Skin: RGB(237, 208, 168)
			new Color(237, 208, 168),

			// Olive Skin: RGB(186, 146, 108)
			new Color(186, 146, 108),

			// Tan/Brown Skin: RGB(155, 112, 79)
			new Color(155, 112, 79),

			// Dark Brown/Deep Skin: RGB(97, 62, 40)
			new Color(97, 62, 40),

			// Ebony/Black Skin: RGB(50, 28, 14)
			new Color(50, 28, 14),

			// Reddish Indian Skin: RGB(221, 162, 147)
			new Color(221, 162, 147),

			// Yellowish Asian Skin: RGB(250, 213, 164)
			new Color(250, 213, 164)
		};


		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 26;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useTurn = true;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.rare = ItemRarityID.Orange;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.value = Item.buyPrice(gold: 1);
		}

		public override void NetSend(BinaryWriter writer) {
			writer.Write(useCount);
		}

		public override void NetReceive(BinaryReader reader) {
			useCount = reader.ReadInt32();
		}

		public override bool ConsumeItem(Player player) {

			return true;
		}



		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
			if (useCount == 0) {
				return;
			}

			// if (useCount % 2 == 0)
			// 	WorldGen.MakeRain();
			// else
			// 	Rain.MakeRain();
			// Vector2 spriteSize = frame.Size() * scale;

			// spriteBatch.Draw(TextureAssets.MagicPixel.Value,
			// 	position: new Vector2(position.X, position.Y + spriteSize.Y * 0.9f),
			// 	sourceRectangle: new Rectangle(0, 0, 1, 1),
			// 	Color.Red,
			// 	rotation: 0f,
			// 	origin: Vector2.Zero,
			// 	scale: new Vector2(spriteSize.X * (MaxUses - useCount) / MaxUses, 2f),
			// 	SpriteEffects.None,
			// 	layerDepth: 0f);
		}


		
		public override bool? UseItem(Player player) {

			player.hair = Main.rand.Next(HairID.Count);
			player.skinVariant = Main.rand.Next(10);
			// player.head = Main.rand.Next(HeadID.Count);
			player.hairColor = new Color((float)Main.rand.NextDouble(), (float)Main.rand.NextDouble(), (float)Main.rand.NextDouble());
			player.skinColor = skinColors[Main.rand.Next(skinColors.Count)];
			player.eyeColor = new Color((float)Main.rand.NextDouble(), (float)Main.rand.NextDouble(), (float)Main.rand.NextDouble());
			player.shirtColor = new Color((float)Main.rand.NextDouble(), (float)Main.rand.NextDouble(), (float)Main.rand.NextDouble());
			player.underShirtColor = new Color((float)Main.rand.NextDouble(), (float)Main.rand.NextDouble(), (float)Main.rand.NextDouble());
			player.pantsColor = new Color((float)Main.rand.NextDouble(), (float)Main.rand.NextDouble(), (float)Main.rand.NextDouble());
			player.shoeColor = new Color((float)Main.rand.NextDouble(), (float)Main.rand.NextDouble(), (float)Main.rand.NextDouble());

			return true;
		}
		// public override void OnStack(Item source, int numToTransfer) {
		// 	MergeUseCount(source);
		// }

		// public override void SplitStack(Item source, int numToTransfer) {
		// 	//Item is a clone of decrease, but useCount should not be cloned, so set it to 0 for the new item.
		// 	useCount = 0;

		// 	MergeUseCount(source);
		// }

		// private void MergeUseCount(Item source) {
		// 	var incoming = (ExampleMultiUseItem)source.ModItem;
		// 	useCount += incoming.useCount;
		// 	if (useCount >= MaxUses) {
		// 		Item.stack--;
		// 		useCount -= MaxUses;
		// 	}

		// 	incoming.useCount = 0;
		// }

		// public override void AddRecipes() {
		// 	CreateRecipe()
		// 		// .AddIngredient<ExampleItem>()
		// 		// .AddTile<Tiles.Furniture.ExampleWorkbench>()
		// 		.Register();
		// }
	}
}
