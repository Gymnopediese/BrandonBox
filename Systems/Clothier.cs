using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BrandonBox.Systems
{
	class Clothier : GlobalNPC
	{
		public override void ModifyShop(NPCShop shop) {

			if (shop.NpcType == NPCID.Clothier) {
				shop.Add<Items.SkinShuffle.SkinShuffle>();
			}

		}
	}
}
