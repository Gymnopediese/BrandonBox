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
using BrandonBox.NPCs.Villager;
namespace BrandonBox.Items.ZombieCure
{
	public class ZombieCure : ModItem
	{

		// public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MaxUses);
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
			Item.value = Item.buyPrice(gold: 200);
		}
		public override bool ConsumeItem(Player player) {

			return true;
		}



		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
			// if (useCount == 0) {
			// 	return;
			// }
		}


		public override bool? UseItem(Player player) {

			NPC? zombie = null;
			double distance = 10000000;

			foreach (var npc in Main.npc) {
				if (npc.active && NPCID.Sets.Zombies[npc.type] && npc.Distance(player.Center) < distance) {
					zombie = npc;
					distance = npc.Distance(player.Center);
				}
			}
			if (zombie != null && distance < 1000) {
				// zombie.SimpleStrikeNPC(9999, 0);
				// NPC.NewNPC(null, (int)zombie.Center.X, (int)zombie.Center.Y, ModContent.NPCType<Villager>());
				zombie.Transform(ModContent.NPCType<Villager>());
				return true;
			}
			return false;
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
