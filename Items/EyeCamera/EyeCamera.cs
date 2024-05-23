using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BrandonBox.Items.EyeCamera
{	
	public class EyeBallCamera : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.IsAGolfBall[Type] = true; // Allows the projectile to be placed on the tee.
			ProjectileID.Sets.TrailingMode[Type] = 0; // Creates a trail behind the golf ball.
			ProjectileID.Sets.TrailCacheLength[Type] = 20; // Sets the length of the trail.
		}

		public override void SetDefaults() {
			Projectile.netImportant = true; // Indicates that this projectile will be synced to a joining player (by default, any projectiles active before the player joins (besides pets) are not synced over).
			Projectile.width = 26; // The width of the projectile's hitbox.
			Projectile.height = 26; // The height of the projectile's hitbox.
			Projectile.friendly = true; // Setting this to anything other than true causes an index out of bounds error.
			Projectile.penetrate = -1; // Number of times the projectile can penetrate enemies. -1 sets it to infinite penetration.
			Projectile.aiStyle = 149; // 149 is the golf ball AI.
			Projectile.tileCollide = false; // Tile Collision is set to false, as it's handled in the AI.
		}
	}

	public class EyeCamera : ModItem
	{

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
			if (used) {
				Systems.KeybindSystem.modifier.Finished = true;
				used = !used;
				projectile.active = false;
				return true;
			}
			projectile = Projectile.NewProjectileDirect(null, player.Center, (Main.MouseScreen - (new Vector2(Main.screenWidth/ 2, Main.screenHeight/2))) / 16, ModContent.ProjectileType<EyeBallCamera>(), 0, 0, player.whoAmI);
			Systems.KeybindSystem.modifier = new Systems.Cam(projectile);
			Main.instance.CameraModifiers.Add(Systems.KeybindSystem.modifier);
			// ModContent.GetTexture("YourMod/Items/YourItemUsed");
			used = !used;
			return true;
		}
	}

	public class EyeCameraRecipe : Recipe.Recipe
	{
		
		public override string name { get; } = "EyeCamera";


		public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.EyeCamera.EyeCamera>();

			// Start a new Recipe.
			resultItem.CreateRecipe()
				.AddIngredient(ModContent.ItemType<Items.WallCamera.Camera>())
				.AddIngredient(ItemID.AdamantiteBar, 5)
				// .AddTile(TileID.DemonAltar)
				.AddCondition(Items.Recipe.RecipeLearner.learned(name))
				.Register();

			resultItem.CreateRecipe()
				.AddIngredient(ModContent.ItemType<Items.WallCamera.Camera>())
				.AddIngredient(ItemID.TitaniumBar, 5)
				// .AddTile(TileID.DemonAltar)
				.AddCondition(Items.Recipe.RecipeLearner.learned(name))
				.Register();
		}
	}
}
