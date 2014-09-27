/// <reference path="/js/libraries/three.js" />
var textures = [];
var textureList = ["alban_painting", "anvil_side", "anvil_top_1", "anvil_top_2", "anvil_top_3", "aztec2_painting", "aztec_painting", "beacon", "beacon_beam", "bedrock", "bed_foot_front", "bed_foot_side", "bed_foot_top", "bed_head_front", "bed_head_side", "bed_head_top", "bomb_painting", "book", "bookshelf", "brewing_stand", "brewing_stand_base", "brick", "cactus_bottom", "cactus_side", "cactus_top", "cake_bottom", "cake_inside", "cake_side", "cake_top", "carrot_crop_1", "carrot_crop_2", "carrot_crop_3", "carrot_crop_4", "cauldron_feet", "cauldron_inside", "cauldron_side", "cauldron_top", "chest", "chest_trapped", "clay", "coal_block", "coal_ore", "cobblestone", "cobblestone_mossy", "cobweb", "cocoa_plant_1", "cocoa_plant_2", "cocoa_plant_3", "command_side", "crops_1", "crops_2", "crops_3", "crops_4", "crops_5", "crops_6", "crops_7", "crops_8", "daylight_sensor_side", "daylight_sensor_top", "dead_shrub", "diamond_block", "diamond_ore", "dirt", "dirt_grass_side", "dirt_grass_top-desert", "dirt_grass_top-forest", "dirt_grass_top", "dirt_mycelium_side", "dirt_mycelium_top", "dirt_podzol_side", "dirt_podzol_top", "dirt_snow_side", "dispenser_front", "door_iron_bottom", "door_iron_top", "door_wood_bottom", "door_wood_top", "double_plant_fern_bottom", "double_plant_fern_top", "double_plant_grass_bottom", "double_plant_grass_top", "double_plant_paeonia_bottom", "double_plant_paeonia_top", "double_plant_rose_bottom", "double_plant_rose_top", "double_plant_sunflower_back", "double_plant_sunflower_bottom", "double_plant_sunflower_front", "double_plant_sunflower_top", "double_plant_syringa_bottom", "double_plant_syringa_top", "dropper_front", "emerald_block", "emerald_ore", "enchant_table_bottom", "enchant_table_side", "enchant_table_top", "enderchest", "enderdragon_egg", "endportal", "endportal_frame_side", "endportal_frame_top", "endstone", "eye_of_ender", "farmland_dry", "farmland_wet", "fern", "fire", "flowerpot", "flower_allium", "flower_blue_orchid", "flower_houstonia", "flower_oxeye_daisy", "flower_paeonia", "flower_red", "flower_tulip_orange", "flower_tulip_pink", "flower_tulip_red", "flower_tulip_white", "flower_yellow", "furnace_front", "furnace_lit_front", "furnace_side", "furnace_top", "glass", "glass_black", "glass_blue", "glass_brown", "glass_cyan", "glass_dark_gray", "glass_green", "glass_light_blue", "glass_light_gray", "glass_lime", "glass_magenta", "glass_orange", "glass_pane_side", "glass_pane_side_black", "glass_pane_side_blue", "glass_pane_side_brown", "glass_pane_side_cyan", "glass_pane_side_dark_gray", "glass_pane_side_green", "glass_pane_side_light_blue", "glass_pane_side_light_gray", "glass_pane_side_lime", "glass_pane_side_magenta", "glass_pane_side_orange", "glass_pane_side_pink", "glass_pane_side_purple", "glass_pane_side_red", "glass_pane_side_white", "glass_pane_side_yellow", "glass_pink", "glass_purple", "glass_red", "glass_white", "glass_yellow", "glowstone", "gold_block", "gold_ore", "gravel", "hardened_clay", "hatch", "hay_block_side", "hay_block_top", "hopper_inside", "hopper_outside", "hopper_top", "ice", "ice_packed", "iron_bars", "iron_block", "iron_ore", "jukebox_side", "jukebox_top", "kebab_painting", "ladder", "lapis_lazuli_block", "lapis_lazuli_ore", "largechest", "largechest_trapped", "lava", "lava_flowing", "leaves_acacia", "leaves_acacia_opaque", "leaves_big_oak", "leaves_big_oak_opaque", "leaves_birch", "leaves_birch_opaque", "leaves_jungle", "leaves_jungle_opaque", "leaves_oak", "leaves_oak_opaque", "leaves_spruce", "leaves_spruce_opaque", "lever", "lilypad", "log_acacia_side", "log_acacia_top", "log_big_oak_side", "log_big_oak_top", "log_birch_side", "log_birch_top", "log_jungle_side", "log_jungle_top", "log_oak_side", "log_oak_top", "log_spruce_side", "log_spruce_top", "melon_side", "melon_stem_1", "melon_stem_2", "melon_top", "mob_char", "mob_chicken", "mob_cow", "mob_creeper", "mob_pig", "mob_sheep", "mob_skeleton", "mob_skeleton_wither", "mob_squid", "mob_zombie", "mushroom_brown", "mushroom_brown_cap", "mushroom_inside", "mushroom_red", "mushroom_red_cap", "mushroom_stem", "netherrack", "netherwart_1", "netherwart_2", "netherwart_3", "nether_brick", "nether_quartz_ore", "noteblock", "obsidian", "piston_arm", "piston_arm_sticky", "piston_bottom", "piston_side", "piston_top", "plank_acacia", "plank_big_oak", "plank_birch", "plank_jungle", "plank_oak", "plank_spruce", "plant_painting", "portal", "potato_crop_1", "potato_crop_2", "potato_crop_3", "potato_crop_4", "pumpkin_front", "pumpkin_front_lit", "pumpkin_side", "pumpkin_side_lit", "pumpkin_stem_1", "pumpkin_stem_2", "pumpkin_top", "pumpkin_top_lit", "quartz_bottom", "quartz_side", "quartz_side_chiseled", "quartz_side_lines", "quartz_top", "quartz_top_chiseled", "quartz_top_lines", "rails", "rails_activator_off", "rails_activator_on", "rails_curved", "rails_detector", "rails_detector_off", "rails_detector_on", "rails_powered_off", "rails_powered_on", "redstone_block", "redstone_comparator_off", "redstone_comparator_on", "redstone_dust_off", "redstone_dust_on", "redstone_lamp_off", "redstone_lamp_on", "redstone_ore", "redstone_repeater_off", "redstone_repeater_on", "redstone_torch_off", "redstone_torch_on", "redstone_wire_off", "redstone_wire_on", "red_sand", "sand", "sandstone_bottom", "sandstone_side", "sandstone_side_carved", "sandstone_side_smooth", "sandstone_top", "sapling_acacia", "sapling_birch", "sapling_jungle", "sapling_oak", "sapling_roofed_oak", "sapling_spruce", "sign", "snow", "soulsand", "spawner", "sponge", "stained_clay_black", "stained_clay_blue", "stained_clay_brown", "stained_clay_cyan", "stained_clay_dark_gray", "stained_clay_green", "stained_clay_light_blue", "stained_clay_light_gray", "stained_clay_lime", "stained_clay_magenta", "stained_clay_orange", "stained_clay_pink", "stained_clay_purple", "stained_clay_red", "stained_clay_white", "stained_clay_yellow", "stone", "stone_brick", "stone_brick_circle", "stone_brick_cracked", "stone_brick_mossy", "stone_slab_side", "stone_slab_top", "sugarcane", "tall_grass", "tnt_bottom", "tnt_side", "tnt_top", "torch", "torch_flame", "tripwire", "tripwire_hook", "unknown", "vines", "wasteland_painting", "water", "water_flowing", "wool_black", "wool_blue", "wool_brown", "wool_cyan", "wool_dark_gray", "wool_dark_grey", "wool_green", "wool_light_blue", "wool_light_gray", "wool_lime", "wool_magenta", "wool_orange", "wool_pink", "wool_purple", "wool_red", "wool_white", "wool_yellow", "workbench_back", "workbench_front", "workbench_top.png"];

function loadTextures() {
	var i = textureList.length;

	while (i--) {

		var name = textureList[i];
		var loader = new THREE.ImageLoader();

		loader.load('tex/' + name + '.png', function (image) {
			var texture = new THREE.Texture();

			texture.anisotropy = 0;
			texture.premultiplyAlpha = false;
			texture.unpackAlignment = 0;
			texture.image = image;
			texture.needsUpdate = true;

			var textureName = texture.image.src.replace("http://localhost/tex/", "").replace(".png", "");
			textures.push({ texture: texture, name: textureName });
		});
	}
}

function getTexture(name) {
	var i = textures.length;

	while (i--) {
		//var nameArray = textures[i].name;//.split('/');
		//var tname= nameArray[nameArray.length];

		if (textures[i].name ==  name) {
			return textures[i].texture;
		};
	}
}

//THREE.TextureLoader.prototype.load = function (url, textureName) {

//		var scope = this;
//		scope.textureName = textureName;
//		var image = new Image();

//		image.addEventListener('load', function () {

//			var texture = new THREE.Texture(image);
//			texture.needsUpdate = true;

//			scope.dispatchEvent({ type: 'load', content: { texture: texture, textureName: scope.textureName } });

//		}, false);

//		image.addEventListener('error', function () {

//			scope.dispatchEvent({ type: 'error', message: 'Couldn\'t load URL [' + url + ']' });

//		}, false);

//		if (scope.crossOrigin) image.crossOrigin = scope.crossOrigin;

//		image.src = url;
//}

