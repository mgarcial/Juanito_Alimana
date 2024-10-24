using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preferences : MonoBehaviour
{
    private const string CURRENT_LVL = "currLvl";
    private const string MAX_LVL = "maxLvl";

    public static int GetCurrentLvl() => PlayerPrefs.GetInt(CURRENT_LVL, 1);
    public static void SetCurrentLvl(int lvl) => PlayerPrefs.SetInt(CURRENT_LVL, lvl);
    public static void ClearMaxLevel() => PlayerPrefs.DeleteKey(MAX_LVL);
    public static int GetMaxLvl() => PlayerPrefs.GetInt(MAX_LVL, 1);
    public static void SetMaxLvl(int lvl) => PlayerPrefs.SetInt(MAX_LVL, lvl);
}
