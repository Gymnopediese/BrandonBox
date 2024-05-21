

from glob import glob
import cv2
from PIL import Image
import imageio

        
        

def sprite_sheet_to_gif(sprite_sheet_path, sprite_width, sprite_height, output_gif_path, duration=100):
	# Open the sprite sheet image
	sprite_sheet = Image.open(sprite_sheet_path)
	sprite_sheet_width, sprite_sheet_height = sprite_sheet.size

	# Calculate the number of sprites in the sheet
	cols = sprite_sheet_width // sprite_width
	rows = sprite_sheet_height // sprite_height

	# Extract individual sprites from the sheet
	sprites = []
	for row in range(rows):
		for col in range(cols):
			left = col * sprite_width
			upper = row * sprite_height
			right = left + sprite_width
			lower = upper + sprite_height
			sprite = sprite_sheet.crop((left, upper, right, lower))
			sprites.append(sprite)
	sprites[0].save(output_gif_path.replace(".gif", "_Body.png"))
	sprites = sprites[1:16]
 
	sprites[0].save(fp=output_gif_path, format='GIF', append_images=sprites[1:],
             save_all=True, duration=100, loop=10, disposal = 2)

def main():
    
	folders = glob("NPCs/*/")
	for folder in folders:
		name = folder.split("/")[-2]
		print(folder, name)
		try:
			sprite_sheet_to_gif(f"{folder}{name}.png", 44, 54, f"{folder}{name}.gif")
		# exit(1)
		except:
			pass
	others = ["NPCs/Traveler/TravelerCrimson.png"]
	for folder in others:
		name = folder.split("/")[-1]
		print(folder, name)
		try:
			sprite_sheet_to_gif(folder, 44, 54, folder.replace(".png", ".gif"))
		# exit(1)
		except:
			pass

	
	sprite_sheet_to_gif("NPCs/DaughterOfSun/DaughterOfSun.png", 50, 60, "NPCs/DaughterOfSun/DaughterOfSun.gif")
	

main()