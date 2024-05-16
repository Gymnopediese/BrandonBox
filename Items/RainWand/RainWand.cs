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

namespace BrandonBox.Items.RainWand
{
	public class RainWand : ModItem
	{
		public static readonly int MaxUses = 10;

		public int useCount;

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MaxUses);

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

			useCount++;
			if (useCount == MaxUses) {
				useCount = 0;
				return true;
			}
			return false;
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
			if (Main.raining){
				Main.StopRain();
			}
			else{
				Main.StartRain();
			}
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
