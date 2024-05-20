using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BrandonBox
{
	class MyConditions
	{
		public static Condition hardmode = new Condition("is hardmode", () => Main.hardMode);
		public static Condition corruption = new Condition("is hardmode", () => !WorldGen.crimson);
		public static Condition crimpson = new Condition("is hardmode", () => WorldGen.crimson);
		public static Condition corruptionHardmode = new Condition("is hardmode", () => Main.hardMode && !WorldGen.crimson);
		public static Condition crimpsonHardmode = new Condition("is hardmode", () => Main.hardMode && WorldGen.crimson);
	}
}
