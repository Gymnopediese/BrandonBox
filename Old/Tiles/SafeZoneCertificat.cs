
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
namespace BrandonBox.Tiles
{
	// Simple 3x3 tile that can be placed on a wall
	public class SafeZoneCertificat : ModTile
	{
		public static int uniqueAnimationFrame = 0;
		public static int _type;
		public override void SetStaticDefaults() {
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.FramesOnKillWall[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(TileID.Painting2X3, 0));
			TileObjectData.addTile(Type);
			// AddMapEntry(new Color(120, 85, 60), Language.GetText("MapObject.Trophy"));
			DustType = 7;
			LocalizedText name = CreateMapEntryName();
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
			AddMapEntry(new Color(238, 145, 105), name);
			_type = Type;
			
		}
				// This method allows you to determine how much light this block emits
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
			r = 0.6f;
			g = 0.6f;
			b = 1f;
		}

		public static int animationFrameWidth = 32;

		public static int tileCageFrameIndex = 0;

		public override void AnimateTile(ref int frame, ref int frameCounter) {
			if (++frameCounter >= 4) {
				frameCounter = 0;
				frame = ++frame % 8;
			}
		}

		public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset) {
			var tile = Main.tile[i, j];
			if (tile.TileFrameY < 56) {
				frameYOffset = Main.tileFrame[type] * 56;
			}
			else {
				frameYOffset = 252;
			}
		}

	}
}
