using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagList
{
    //Player attack tags
    public static string PlayerBasicAttack = "BasicAttack";
    public static string PlayerDashAttack = "DashAttack";
    public static string PlayerChargeAttack = "ChargeAttack";
    public static string PlayerLightning = "PlayerLightning";
    public static string PlayerFire = "PlayerFire";

    //Enemy attack tags
    public static string ExampleEnemyAttack = "ExampleEnemyAttack";
    public static string BasicEnemySword = "BasicEnemySword";

    //Fire boss attack tags
    public static string FireBossSlam = "FireBossSlam";
    public static string FireBossSpin = "FireBossSpin";
    public static string FireBossFlamethrower = "FireBossFlamethrower";
    public static string Firepit = "Firepit";

    //Lightning boss attack tags
    public static string LightningBossShell = "LightningBossShell";
    public static string LightningAOEAttack = "LightningAOEAttack";
    public static string LightningBeamAttack = "LightningBeamAttack";

    //Wind boss attack tags
    public static string WindSlam = "WindSlam";
    public static string WindBoomerang = "WindBoomerang";
    public static string WindSpear = "WindSpear";
    public static string WindSmallTornado = "WindSmallTornado";
    public static string WindBigTornado = "WindBigTornado";

    //Final boss attack tags
    public static string FinalTounge = "FinalTounge";
    public static string FinalBeam = "FinalBeam";
}

public class DataList
{
    //Player Sword Data
    public static SwordType currentSwordType = SwordType.SteelSword;
    public static int[,] attackDamage = new int[,] { { 3, 6, 9 }, { 5, 8, 11 } };
    public enum SwordType
    {
        SteelSword = 0,
        FireSword = 1
    }
    public enum AttackType
    {
        BasicAttack = 0,
        DashAttack = 1,
        ChargeAttack = 2
    }

    //Player Stat Data
    public static float PlayerMaxHealth = 10;
    public static float PlayerMaxMagic = 10;
}

public class PPrefsList
{
    public const string DefeatedLightning = "DefeatedLightning";
    public const string DefeatedFire = "DefeatedFire";
    public const string DefeatedWind = "DefeatedWind";
    public const string DefeatedFinal = "DefeatedFinal";

    public const string MaxHealth = "MaxHealth";
    public const string MaxMagic = "MaxMagic";

    public const string SwordType = "SwordType";

    public static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }

    public static int GetValue(string key, int defaultValue)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetInt(key);
        }
        else
        {
            return defaultValue;
        }
    }

    public static bool GetBool(string key, bool defaultValue)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return (PlayerPrefs.GetInt(key) == 1 ? true : false);
        }
        else
        {
            return defaultValue;
        }
    }

    public static void ResetData()
    {
        PlayerPrefs.SetInt(DefeatedFinal, 0);
        PlayerPrefs.SetInt(DefeatedLightning, 0);
        PlayerPrefs.SetInt(DefeatedWind, 0);
        PlayerPrefs.SetInt(DefeatedFire, 0);
        PlayerPrefs.SetInt(MaxHealth, 10);
        PlayerPrefs.SetInt(MaxMagic, 10);
        PlayerPrefs.SetInt(SwordType, 0);
    }
}