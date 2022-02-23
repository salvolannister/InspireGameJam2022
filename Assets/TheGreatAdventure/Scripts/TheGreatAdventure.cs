using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script is written using the information learned in the unity professional programmer course
 *
 */

/// <summary>
/// This script is in charge of managing the states of the game
/// </summary>
public class TheGreatAdventure : Manager<TheGreatAdventure>
{

    //TODO: manage game Over
    //TODO: manage coins
    //TODO: manage game event on game state change 
    //TODO: manage reload of first level, when dies

    public static int SCORE { get; private set; }
    static private eGameState _GAME_STATE = eGameState.mainMenu;

    
    // System.Flags changes how eGameStates are viewed in the Inspector and lets multiple 
    //  values be selected simultaneously (similar to how Physics Layers are selected).
    // It's only valid for the game to ever be in one state, but I've added System.Flags
    //  here to demonstrate it and to make the ActiveOnlyDuringSomeGameStates script easier
    //  to view and modify in the Inspector.
    // When you use System.Flags, you still need to set each enum value so that it aligns 
    //  with a power of 2. You can also define enums that combine two or more values,
    //  for example the all value below that combines all other possible values.
    [System.Flags]
    public enum eGameState
    {
        // Decimal      // Binary
        none = 0,       // 00000000
        mainMenu = 1,   // 00000001
        preLevel = 2,   // 00000010
        level = 4,      // 00000100
        postLevel = 8,  // 00001000
        gameOver = 16,  // 00010000
        all = 0xFFFFFFF // 11111111111111111111111111111111
    }

    void Start()
    {

    }

    void Update()
    {

    }

    static public void GameOver()
    {
        // SAVE
        //SaveGameManager.CheckHighScore(SCORE);
        //SaveGameManager.Save();
        instance.EndGame();
    }

    private void EndGame()
    {
        // change game state 
        // show game over 
        ReloadScene();
    }

    void ReloadScene()
    {
        // Reload the scene to restart the game
        // Note: This exposes a long-time Unity bug where reloading the scene 
        //  during gameplay within the Editor causes the lighting to all go 
        //  dark and the engine to think that it needs to rebuild the lighting.
        //  This bug does not cause any issues outside of the Editor.
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
