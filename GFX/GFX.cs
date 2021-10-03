using System;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SummonHeart.GFX
{
    public static class GFX
    {
        public static void LoadGfx()
        {
            Mod loader = SummonHeartMod.Instance;
            FieldInfo[] fields = typeof(GFX).GetFields(BindingFlags.Static | BindingFlags.Public);
            FieldInfo[] directoryFields = typeof(GFX).GetFields(BindingFlags.Static | BindingFlags.NonPublic);
            foreach (FieldInfo fi in fields)
            {
                bool foundTexture = false;
                for (int index = 0; index < directoryFields.Length; index++)
                {
                    if (("_" + fi.Name).Equals(directoryFields[index].Name))
                    {
                        foundTexture = true;
                        fi.SetValue(null, loader.GetTexture((string)directoryFields[index].GetValue(null)));
                    }
                }
                if (!foundTexture)
                {
                    SummonHeartMod.Instance.Logger.Error("failed to load texture " + fi.Name);
                    fi.SetValue(null, loader.GetTexture("GFX/UIArt/Nothing"));
                }
            }
        }

        // Token: 0x060007BE RID: 1982 RVA: 0x0002332C File Offset: 0x0002152C
        public static void UnloadGfx()
        {
            FieldInfo[] fields = typeof(GFX).GetFields(BindingFlags.Static | BindingFlags.Public);
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i].SetValue(null, null);
            }
        }

        // Token: 0x040001A9 RID: 425
        private const string _CHASSIS_DIRECTORY = "GFX/ChassisArt/";

        // Token: 0x040001AA RID: 426
        private const string _WEAPON_DIRECTORY = "GFX/WeaponArt/";

        // Token: 0x040001AB RID: 427
        private const string _UI_DIRECTORY = "GFX/UIArt/";

        // Token: 0x040001AC RID: 428
        private const string _EFFECTS_DIRECTORY = "GFX/Effects/";

        // Token: 0x040001AD RID: 429
        private const string _ICON_DIRECTORY = "GFX/MechIcons/";

        // Token: 0x040001AE RID: 430
        private const string _NOTHING = "GFX/UIArt/Nothing";

        // Token: 0x040001AF RID: 431
        public static Texture2D NOTHING;

        // Token: 0x040001B0 RID: 432
        private const string _BIG_CHASSIS = "GFX/ChassisArt/BigChassis";

        // Token: 0x040001B1 RID: 433
        public static Texture2D BIG_CHASSIS;

        // Token: 0x040001B2 RID: 434
        private const string _SMALL_CHASSIS = "GFX/ChassisArt/SmallChassis";

        // Token: 0x040001B3 RID: 435
        public static Texture2D SMALL_CHASSIS;

        // Token: 0x040001B4 RID: 436
        private const string _GREEN_CHASSIS = "GFX/ChassisArt/GreenChassis";

        // Token: 0x040001B5 RID: 437
        public static Texture2D GREEN_CHASSIS;

        // Token: 0x040001B6 RID: 438
        private const string _WHITE_CHASSIS = "GFX/ChassisArt/WhiteChassis";

        // Token: 0x040001B7 RID: 439
        public static Texture2D WHITE_CHASSIS;

        // Token: 0x040001B8 RID: 440
        private const string _HUNTER_KILLER_CHASSIS = "GFX/ChassisArt/HunterKillerChassis";

        // Token: 0x040001B9 RID: 441
        public static Texture2D HUNTER_KILLER_CHASSIS;

        // Token: 0x040001BA RID: 442
        private const string _FLAME_CHASSIS = "GFX/ChassisArt/FlameChassis";

        // Token: 0x040001BB RID: 443
        public static Texture2D FLAME_CHASSIS;

        // Token: 0x040001BC RID: 444
        private const string _SIMPLE_SHOOTER = "GFX/WeaponArt/SimpleShooter";

        // Token: 0x040001BD RID: 445
        public static Texture2D SIMPLE_SHOOTER;

        // Token: 0x040001BE RID: 446
        private const string _ROCKET_SHOOTER = "GFX/WeaponArt/RocketShooter";

        // Token: 0x040001BF RID: 447
        public static Texture2D ROCKET_SHOOTER;

        // Token: 0x040001C0 RID: 448
        private const string _MICRO_MISSILE_LAUNCHER = "GFX/WeaponArt/MicroMissileLauncher";

        // Token: 0x040001C1 RID: 449
        public static Texture2D MICRO_MISSILE_LAUNCHER;

        // Token: 0x040001C2 RID: 450
        private const string _LIGHTNING_ORB = "GFX/WeaponArt/LightningOrb";

        // Token: 0x040001C3 RID: 451
        public static Texture2D LIGHTNING_ORB;

        // Token: 0x040001C4 RID: 452
        private const string _CHEM_SPRAYER = "GFX/WeaponArt/ChemSprayer";

        // Token: 0x040001C5 RID: 453
        public static Texture2D CHEM_SPRAYER;

        // Token: 0x040001C6 RID: 454
        private const string _BASIC_SHOTGUN = "GFX/WeaponArt/BasicShotgun";

        // Token: 0x040001C7 RID: 455
        public static Texture2D BASIC_SHOTGUN;

        // Token: 0x040001C8 RID: 456
        private const string _LIGHT_PLASMA = "GFX/WeaponArt/LightPlasma";

        // Token: 0x040001C9 RID: 457
        public static Texture2D LIGHT_PLASMA;

        // Token: 0x040001CA RID: 458
        private const string _GAUSS_RIFLE = "GFX/WeaponArt/GaussRifle";

        // Token: 0x040001CB RID: 459
        public static Texture2D GAUSS_RIFLE;

        // Token: 0x040001CC RID: 460
        private const string _FLAME_THROWER = "GFX/WeaponArt/FlameThrower";

        // Token: 0x040001CD RID: 461
        public static Texture2D FLAME_THROWER;

        // Token: 0x040001CE RID: 462
        private const string _FLAK_CANNON = "GFX/WeaponArt/FlakCannon";

        // Token: 0x040001CF RID: 463
        public static Texture2D FLAK_CANNON;

        // Token: 0x040001D0 RID: 464
        private const string _HEXXED_FLAME_THROWER = "GFX/WeaponArt/HexxedFlameThrower";

        // Token: 0x040001D1 RID: 465
        public static Texture2D HEXXED_FLAME_THROWER;

        // Token: 0x040001D2 RID: 466
        private const string _ASSAULT_CANNON = "GFX/WeaponArt/AssaultCannon";

        // Token: 0x040001D3 RID: 467
        public static Texture2D ASSAULT_CANNON;

        // Token: 0x040001D4 RID: 468
        private const string _SHARD_CANNON = "GFX/WeaponArt/ShardCannon";

        // Token: 0x040001D5 RID: 469
        public static Texture2D SHARD_CANNON;

        // Token: 0x040001D6 RID: 470
        private const string _INFO_BUTTON = "GFX/UIArt/InfoButtonTexture";

        // Token: 0x040001D7 RID: 471
        public static Texture2D INFO_BUTTON;

        // Token: 0x040001D8 RID: 472
        private const string _DRONE_UI_BACKING = "GFX/UIArt/DroneUIBacking";

        // Token: 0x040001D9 RID: 473
        public static Texture2D DRONE_UI_BACKING;

        // Token: 0x040001DA RID: 474
        private const string _ACTIVATOR_UI = "GFX/UIArt/ActivatorUI";

        // Token: 0x040001DB RID: 475
        public static Texture2D ACTIVATOR_UI;

        // Token: 0x040001DC RID: 476
        private const string _ACTIVATOR_UI_OUTLINE = "GFX/UIArt/ActivatorUIOutline";

        // Token: 0x040001DD RID: 477
        public static Texture2D ACTIVATOR_UI_OUTLINE;

        // Token: 0x040001DE RID: 478
        private const string _DRONE_INFO_BUTTON = "GFX/UIArt/DroneInfoButton";

        // Token: 0x040001DF RID: 479
        public static Texture2D DRONE_INFO_BUTTON;

        // Token: 0x040001E0 RID: 480
        private const string _DRONE_INV_SLOT = "GFX/UIArt/DroneInvSlot";

        // Token: 0x040001E1 RID: 481
        public static Texture2D DRONE_INV_SLOT;

        // Token: 0x040001E2 RID: 482
        private const string _DRONE_ATTACHMENT_SLOT = "GFX/UIArt/DroneAttachmentSlot";

        // Token: 0x040001E3 RID: 483
        public static Texture2D DRONE_ATTACHMENT_SLOT;

        // Token: 0x040001E4 RID: 484
        private const string _DINO_SLOT = "GFX/UIArt/DinoSlot";

        // Token: 0x040001E5 RID: 485
        public static Texture2D DINO_SLOT;

        // Token: 0x040001E6 RID: 486
        private const string _DINO_CHANCE_BAR = "GFX/UIArt/DinoChanceBar";

        // Token: 0x040001E7 RID: 487
        public static Texture2D DINO_CHANCE_BAR;

        // Token: 0x040001E8 RID: 488
        private const string _DINO_EXPORT_BUTTON = "GFX/UIArt/DinoExportButton";

        // Token: 0x040001E9 RID: 489
        public static Texture2D DINO_EXPORT_BUTTON;

        // Token: 0x040001EA RID: 490
        private const string _SHELF = "GFX/UIArt/Shelf";

        // Token: 0x040001EB RID: 491
        public static Texture2D SHELF;

        // Token: 0x040001EC RID: 492
        private const string _LIGHTNING_CENTER_SEGMENT = "GFX/Effects/LightningCenterSegment";

        // Token: 0x040001ED RID: 493
        public static Texture2D LIGHTNING_CENTER_SEGMENT;

        // Token: 0x040001EE RID: 494
        private const string _LIGHTNING_END_PIECE = "GFX/Effects/LightningEndPiece";

        // Token: 0x040001EF RID: 495
        public static Texture2D LIGHTNING_END_PIECE;

        // Token: 0x040001F0 RID: 496
        private const string _ARROW = "GFX/Effects/Arrow";

        // Token: 0x040001F1 RID: 497
        public static Texture2D ARROW;

        // Token: 0x040001F2 RID: 498
        private const string _MECH_ARROW = "GFX/Effects/MechArrow";

        // Token: 0x040001F3 RID: 499
        public static Texture2D MECH_ARROW;

        // Token: 0x040001F4 RID: 500
        private const string _AUTO_MECH_ICON = "GFX/MechIcons/AutoMech_Icon";

        // Token: 0x040001F5 RID: 501
        public static Texture2D AUTO_MECH_ICON;

        // Token: 0x040001F6 RID: 502
        private const string _BULK_MECH_ICON = "GFX/MechIcons/BulkMech_Icon";

        // Token: 0x040001F7 RID: 503
        public static Texture2D BULK_MECH_ICON;

        // Token: 0x040001F8 RID: 504
        private const string _OSTRICH_MECH_ICON = "GFX/MechIcons/OstrichMech_Icon";

        // Token: 0x040001F9 RID: 505
        public static Texture2D OSTRICH_MECH_ICON;

        // Token: 0x040001FA RID: 506
        private const string _STANDARDA1_MECH_ICON = "GFX/MechIcons/StandardA1Mech_Icon";

        // Token: 0x040001FB RID: 507
        public static Texture2D STANDARDA1_MECH_ICON;

        // Token: 0x040001FC RID: 508
        private const string _STANDARD_MECH_ICON = "GFX/MechIcons/StandardMech_Icon";

        // Token: 0x040001FD RID: 509
        public static Texture2D STANDARD_MECH_ICON;
    }
}
