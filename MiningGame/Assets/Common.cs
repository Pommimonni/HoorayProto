using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public static class Common  {
    public static GameMaster gameMaster;
    public static PlayerInformation playerInfo;
    public static UsefulFunctions usefulFunctions;
    public static GameSettings gameSettings;
    public static WallGrid wallGrid;
    public static HoleShapeHandler holeShapeHandler;
    public static SpriteCropper spriteCropper;
    public static Effects effects;
    public static InfoGUI playerInfoGUI;
    public static LevelLoader levelLoader;
    public static BombSummoner bonusBombSummoner;
    public static BonusRoundWinChances bonusRoundChances;
    public static LauriWrapper lauriWrapper;
    public static Gems gemSkins;

    public static string cullingMaskNameInTexture = "_Mask";

    public static ControlGameMusics controlGameMusic;

    public static Vector3 mapleftBottom;
    public static Vector3 mapRightTop;
    public static Vector3 mapMIddle;

    public static Color defaultRedTextColor=Color.white;

    public static DisplayScript displayScript;

    public static float AdjustBet(float amount)
    {
        return amount * RoundSettings.bet;
    }


}
