using System;
using System.Windows.Forms;

namespace GameLibrary
{
    // Lil structy boi to determine position of a character. Actually good coding progress. Nice!
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public struct Position
    {
        public int row;
        public int col;

        public Position(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////



    // this is the character class. Takes from the Mortal class
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public class Character : Mortal
    {
        // getter and setter for the picturebox of the character and the XP and the bool of should the character level up
        public PictureBox Pic { get; private set; }
        public float XP { get; private set; }
        public bool ShouldLevelUp { get; private set; }

        // variables for the map and the position struct
        public Position pos;
        public Map map;


        // creates the character and initializes variables. Need to split from map because dependencies are INSANE in this program
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Character(PictureBox pb, Position pos, Map map) : base("Player 1", 1)
        {
            Pic = pb;
            this.pos = pos;
            this.map = map;
            ShouldLevelUp = false;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        



        // how to add experience to the character
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void GainXP(float amount)
        {
            XP += amount;
            // every 100 experience points you gain a level
            if (XP / 100 >= Level)
            {
                Level = Level + 1;
                XP = 0;
                this.LevelUp();

                //ShouldLevelUp = true;
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        // How to level up the character
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void LevelUp()
        {
            base.LevelUp();
            ShouldLevelUp = false;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        // Reset character to the top left corner
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void BackToStart()
        {
            pos.row = map.CharacterStartRow;
            pos.col = map.CharacterStartCol;
            Position topleft = map.RowColToTopLeft(pos);
            Pic.Left = topleft.col;
            Pic.Top = topleft.row;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        // Reset all stats to zero
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void ResetStats()
        {
            base.ResetStats();
            XP = 0;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        // the function that moves the character on the map. Make sure is strong enough to do this right
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Task Move(MoveDir dir)
        {
            Position newPos = pos;
            switch (dir)
            {
                case MoveDir.UP:
                    newPos.row--;
                    break;
                case MoveDir.DOWN:
                    newPos.row++;
                    break;
                case MoveDir.LEFT:
                    newPos.col--;
                    break;
                case MoveDir.RIGHT:
                    newPos.col++;
                    break;
            }
            if (map.IsNextLevel(newPos))
            {
     
                return Task.LEAVE_LEVEL;
            }
            else if (map.IsValidPos(newPos))
            {
                pos = newPos;
                Position topleft = map.RowColToTopLeft(pos);
                Pic.Left = topleft.col;
                Pic.Top = topleft.row;
                return Task.MOVE;
            }
            return Task.NO_TASK;//false
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
