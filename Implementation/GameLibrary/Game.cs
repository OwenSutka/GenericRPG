using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    // This is the Enum that allows the developer to choose the game state. May need to check back here to add to this
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public enum GameState
    {
        LOADING,
        TITLE_SCREEN,
        UPGRADE_SCREEN,
        FIGHTING,
        ON_MAP,
        DEAD
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////


    // The game class. Where this all actually happens. Will be used to change screens etc.
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class Game
    {
        private int NumLevels = 5;
        // Initialize the game object
        private static Game game;

        // getters and setters for the character initialization and the game state initialization
        public Character Character { get; private set; }
        public GameState State { get; private set; }

        public int Level { get; private set; }

        // Starts that bad boi up
        private Game()
        {
            State = GameState.LOADING;
            Level = 1;
        }

        // Creates the game object on startup if not already initialized
        public static Game GetGame()
        {
            if (game == null)
                game = new Game();
            return game;
        }

        // Changes the game state to reflect the new goal
        public void ChangeState(GameState newState)
        {
            State = newState;
        }

        // Initializes the new character (getter setter)
        public void SetCharacter(Character character)
        {
            Character = character;
        }

    public void NextLevel()
        {
            if (Level < NumLevels)
            {
                Level++;

            }
        }

        public void PreviousLevel()
        {
            if (Level > 1)
            {
                Level--;
            }
        }
    }
}
