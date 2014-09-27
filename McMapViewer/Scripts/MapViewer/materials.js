function Materials() {
}

Materials.GetMat = function (meshName) {
	var material = window.mapViewer.materialsCreator.create(meshName);
	return material;
}

Materials.GetMaterials = function (callback) {

	var mtlLoader = new THREE.MTLLoader("/maps/");

	mtlLoader.load("/maps/minecraft.mtl", function (materials) {
		window.mapViewer.materialsCreator = materials;
		window.mapViewer.materialsCreator.preload(Materials.fixMaterials);
		
		callback();
	});
}

Materials.fixMaterials=function(){

	var mats = window.mapViewer.materialsCreator.materials;
	
	for (var mat in mats) {
		mats[mat] = Materials.fixMaterial(mats[mat]);
	}
}

Materials.fixMaterial = function (material) {
	var that = this,
		isTransparent = that.isTransparent(material.name),
		isLuminant = that.isLuminant(material.name),
		isLiquid = that.isLiquid(material.name);

	if (material.name != "" && material.name != "unknown") {

		material.map.magFilter = THREE.NearestFilter;
		material.map.minFilter = THREE.NearestMipMapLinearFilter;//THREE.NearestMipMapLinearFilter;
		material.flipSided = false;
		material.map.anisotropy = 0;
		material.emissive = new THREE.Color(0x555555);
		material.shading = THREE.FlatShading;
		material.specular = new THREE.Color(0x111111);
		material.shininess = 0;
		material.ansitropy = 0;
		material.depthWrite = true;
		material.fog = true;
		material.combine = THREE.AddOperation;
		material.wrapAround = true;

		if (isLiquid) {
			material.transparent = true;
			material.depthWrite = true;
		}
		else if (isTransparent) {
			material.transparent = true;
			material.side = THREE.DoubleSide;
			material.depthWrite = true;
		}
	}

	return material;
}

Materials.isLiquid = function (name) {
	var mats = Materials.GetMats();

	var i = mats.liquid.length;
	while (i--) {
		if (name == mats.liquid[i]) {
			return true;
		}
	}

	return false;
}

Materials.isTransparent = function (name) {
	var mats = Materials.GetMats();

	var i = mats.transparent.length;
	while (i--) {
		if (name == mats.transparent[i]) {
			return true;
		}
	}

	return false;
}

Materials.isLuminant = function (name) {
	var mats = Materials.GetMats();

	var i = mats.luminant.length;
	while (i--) {
		if (name == mats.luminant[i]) {
			return true;
		}
	}

	return false;
}

Materials.GetMats = function () {
	materials = {
		opaque: [
			"anvil_side",
			"anvil_top_1",
			"anvil_top_2",
			"anvil_top_3",
			"bed_foot_front",
			"bed_foot_side",
			"bed_foot_top",
			"bed_head_front",
			"bed_head_side",
			"bed_head_top",
			"bedrock",
			"book",
			"bookshelf",
			"brick",
			"cake_bottom",
			"cake_inside",
			"cake_side",
			"cake_top",
			"chest",
			"chest_trapped",
			"clay",
			"coal_block",
			"coal_ore",
			"cobblestone",
			"cobblestone_mossy",
			"command_side",
			"daylight_sensor_side",
			"daylight_sensor_top",
			"diamond_ore",
			"diamond_block",
			"dirt",
			"dirt_grass_side",
			"dirt_grass_top",
			"dirt_grass_top-desert",
			"dirt_grass_top-forest",
			"dirt_mycelium_side",
			"dirt_mycelium_top",
			"dirt_podzol_side",
			"dirt_podzol_top",
			"dirt_snow_side",
			"dispenser_front",
			"door_iron_bottom",
			"door_iron_top",
			"door_wood_bottom",
			"door_wood_top",
			"dropper_front",
			"emerald_ore",
			"emerald_block",
			"enchant_table_bottom",
			"enchant_table_side",
			"enchant_table_top",
			"enderchest",
			"endestone",
			"enderdragon_egg",
			"eye_of_ender",
			"farmland_dry",
			"farmland_wet",
			"furnace_front",
			"furnace_lit_front",
			"furnace_side",
			"furnace_top",
			"glowstone",
			"gold_block",
			"gold_ore",
			"gravel",
			"hardened_clay",
			"hatch",
			"hay_block_side",
			"hay_block_top",
			"hopper_inside",
			"hopper_outside",
			"hopper_top",
			"iron_block",
			"iron_ore",
			"jukebox_side",
			"jukebox_top",
			"lapis_lazuli_block",
			"lapis_lazuli_ore",
			"largechest",
			"largechest_trapped",
			"melon_side",
			"melon_top",
			"mob_char",
			"mob_creeper",
			"mob_skeleton",
			"mob_skeleton_wither",
			"mob_zombie",
			"nether_brick",
			"nether_quartz_ore",
			"netherrack",
			"noteblock",
			"obsidian",
			"piston_bottom",
			"piston_side",
			"piston_top",
			"plank_acacia",
			"plank_big_oak",
			"plank_birch",
			"plank_jungle",
			"plank_oak",
			"plank_spruce",
			"pumpkin_front",
			"pumpkin_side",
			"pumpkin_top",
			"quartz_bottom",
			"quartz_side",
			"quartz_side_chiseled",
			"quartz_side_lines",
			"quartz_top",
			"quartz_top_chiseled",
			"quartz_top_lines",
			"red_sand",
			"redstone_block",
			"redstone_comparator_off",
			"redstone_comparator_on",
			"redstone_lamp_off",
			"redstone_ore",
			"redstone_repeater_off",
			"redstone_repeater_on",
			"sand",
			"sandstone_bottom",
			"sandstone_side",
			"sandstone_side_carved",
			"sandstone_side_smooth",
			"sandstone_top",
			"snow",
			"soulsand",
			"sponge",
			"stained_clay_black",
			"stained_clay_blue",
			"stained_clay_brown",
			"stained_clay_cyan",
			"stained_clay_dark_gray",
			"stained_clay_green",
			"stained_clay_light_blue",
			"stained_clay_light_gray",
			"stained_clay_lime",
			"stained_clay_magenta",
			"stained_clay_orange",
			"stained_clay_pink",
			"stained_clay_purple",
			"stained_clay_red",
			"stained_clay_white",
			"stained_clay_yellow",
			"stone",
			"stone_brick",
			"stone_brick_circle",
			"stone_brick_cracked",
			"stone_brick_mossy",
			"stone_slab_side",
			"stone_slab_top",
			"tnt_bottom",
			"tnt_side",
			"tnt_top",
			"log_acacia_side",
			"log_acacia_top",
			"log_big_oak_side",
			"log_big_oak_top",
			"log_birch_side",
			"log_birch_top",
			"log_jungle_side",
			"log_jungle_top",
			"log_oak_side",
			"log_oak_top",
			"log_spruce_side",
			"log_spruce_top",
			"wool_black",
			"wool_blue",
			"wool_brown",
			"wool_cyan",
			"wool_dark_gray",
			"wool_green",
			"wool_light_blue",
			"wool_light_gray",
			"wool_lime",
			"wool_magenta",
			"wool_orange",
			"wool_pink",
			"wool_purple",
			"wool_red",
			"wool_white",
			"wool_yellow",
			"workbench_front",
			"workbench_back",
			"workbench_top",
			"kebab_painting",
			"aztec_painting",
			"alban_painting",
			"aztec2_painting",
			"bomb_painting",
			"plant_painting",
			"wasteland_painting",
			"mob_pig",
			"mob_cow",
			"mob_chicken",
			"mob_sheep",
			"mob_squid"
		],

		transparent: [
			"beacon",
			"beacon_beam",
			"brewing_stand",
			"brewing_stand_base",
			"cactus_bottom",
			"cactus_side",
			"cactus_top",
			"carrot_crop_1",
			"carrot_crop_2",
			"carrot_crop_3",
			"carrot_crop_4",
			"cauldron_feet",
			"cauldron_inside",
			"cauldron_side",
			"cauldron_top",
			"cobweb",
			"cocoa_plant_1",
			"cocoa_plant_2",
			"cocoa_plant_3",
			"crops_1",
			"crops_2",
			"crops_3",
			"crops_4",
			"crops_5",
			"crops_6",
			"crops_7",
			"crops_8",
			"dead_shrub",
			"double_plant_fern_bottom",
			"double_plant_fern_top",
			"double_plant_grass_bottom",
			"double_plant_grass_top",
			"double_plant_paeonia_bottom",
			"double_plant_paeonia_top",
			"double_plant_rose_bottom",
			"double_plant_rose_top",
			"double_plant_sunflower_back",
			"double_plant_sunflower_bottom",
			"double_plant_sunflower_front",
			"double_plant_sunflower_top",
			"double_plant_syringa_bottom",
			"double_plant_syringa_top",
			"endportal",
			"endportal_frame_side",
			"endportal_frame_top",
			"fern",
			"fire",
			"flower_allium",
			"flower_blue_orchid",
			"flower_houstonia",
			"flower_oxeye_daisy",
			"flower_paeonia",
			"flower_red",
			"flower_tulip_orange",
			"flower_tulip_pink",
			"flower_tulip_red",
			"flower_tulip_white",
			"flower_yellow",
			"flowerpot",
			"glass",
			"glass_black",
			"glass_blue",
			"glass_brown",
			"glass_cyan",
			"glass_dark_gray",
			"glass_green",
			"glass_light_blue",
			"glass_light_gray",
			"glass_lime",
			"glass_magenta",
			"glass_orange",
			"glass_pink",
			"glass_purple",
			"glass_red",
			"glass_white",
			"glass_yellow",
			"glass_pane_side",
			"glass_pane_side_black",
			"glass_pane_side_blue",
			"glass_pane_side_brown",
			"glass_pane_side_cyan",
			"glass_pane_side_dark_gray",
			"glass_pane_side_green",
			"glass_pane_side_light_blue",
			"glass_pane_side_light_gray",
			"glass_pane_side_lime",
			"glass_pane_side_magenta",
			"glass_pane_side_orange",
			"glass_pane_side_pink",
			"glass_pane_side_purple",
			"glass_pane_side_red",
			"glass_pane_side_white",
			"glass_pane_side_yellow",
			"ice",
			"ice_packed",
			"iron_bars",
			"ladder",
			"leaves_acacia",
			"leaves_big_oak",
			"leaves_birch",
			"leaves_jungle",
			"leaves_oak",
			"leaves_spruce",
			"lever",
			"lilypad",
			"mushroom_brown",
			"mushroom_brown_cap",
			"mushroom_inside",
			"mushroom_red",
			"mushroom_red_cap",
			"mushroom_stem",
			"melon_stem_1",
			"melon_stem_2",
			"netherwart_1",
			"netherwart_2",
			"netherwart_3",
			"piston_arm_sticky",
			"piston_arm",
			"potato_crop_1",
			"potato_crop_2",
			"potato_crop_3",
			"potato_crop_4",
			"portal",
			"pumpkin_stem_1",
			"pumpkin_stem_2",
			"rails",
			"rails_activator_off",
			"rails_activator_on",
			"rails_curved",
			"rails_detector_off",
			"rails_detector_on",
			"rails_powered_off",
			"rails_powered_on",
			"redstone_wire_off",
			"redstone_wire_on",
			"redstone_torch_off",
			"redstone_torch_on",
			"redstone_dust_off",
			"redstone_dust_on",
			"sapling_acacia",
			"sapling_birch",
			"sapling_jungle",
			"sapling_oak",
			"sapling_roofed_oak",
			"sapling_spruce",
			"sign",
			"spawner",
			"sugarcane",
			"tall_grass",
			"torch",
			"torch_flame",
			"tripwire",
			"tripwire_hook",
			"vines"
		],
		liquid: [
			"lava",
			"lava_flowing",
			"water",
			"water_flowing"
		],
		luminant: [
			"beacon_beam",
			"fire",
			"glowstone",
			"lava",
			"lava_flowing",
			"pumpkin_front_lit",
			"pumpkin_side_lit",
			"pumpkin_top_lit",
			"redstone_lamp_on",
			"redstone_torch_on",
			"redstone_wire_on",
			"torch",
			"torch_flame"
		]
	}

	return materials;
}

