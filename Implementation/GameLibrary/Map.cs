using System.IO;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Collections.Generic;


namespace GameLibrary
{
    // This is the map class and is used to show the map to the screen. Only need one map object at a time
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class Map
    {
        // Initialize a bunch of variables and constants to be used
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Constants
        private const int TOP_PAD = 10;
        private const int BOUNDARY_PAD = 5;
        private const int BLOCK_SIZE = 50;
        // Variables
        private int[,] layout;
        public double encounterChance;
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        // getters and setters for where the character will begin initially and the number of rows and columns in the map
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int CharacterStartRow { get; private set; }
        public int CharacterStartCol { get; private set; }
        private int NumRows { get { return layout.GetLength(0); } }
        private int NumCols { get { return layout.GetLength(1); } }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        // NEED TO BREAK UP AND SIMPLIFY. Cherry thought it would be cool to do 20 things at once. This program is a mess
        // and needs some work. We need to just load the map (calls an enum), send that to a display function, which can
        // then call the character (if not there initialize) and then place it on the map at the coords specified.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void LoadMap2(string mapFile, GroupBox grpBox, Func<string, Bitmap> LoadImg)
        {
            int top = TOP_PAD;
            int left = BOUNDARY_PAD;

            PictureBox pb = null;
            List<string> mapLines = new List<string>();

            // read from map file
            using (FileStream fs = new FileStream(mapFile, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        mapLines.Add(line);
                        line = sr.ReadLine();
                    }
                }
            }

            layout = new int[mapLines.Count, mapLines[0].Length];


        }
        public Character LoadMap(string mapFile, GroupBox grpMap, Func<string, Bitmap> LoadImg)
        {
            // declare and initialize locals
            int top = TOP_PAD;
            int left = BOUNDARY_PAD;
            Character character = null;
            PictureBox characterPB = CreateMapCell('z', LoadImg);
            List<string> mapLines = new List<string>();

            // read from map file
            using (FileStream fs = new FileStream(mapFile, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        mapLines.Add(line);
                        line = sr.ReadLine();
                    }
                }
            }

            // load map file into layout and create PictureBox objects
            layout = new int[mapLines.Count, mapLines[0].Length];
            int i = 0;
            foreach (string mapLine in mapLines)
            {
                int j = 0;
                foreach (char c in mapLine)
                {
                    char val = c;
                    layout[i, j] = (val == '3' ? 1 : 0);
                    PictureBox pb = CreateMapCell(val, LoadImg);
                    if (val == '1')
                    {
                        CharacterStartRow = i;
                        CharacterStartCol = j;
                        characterPB.Top = top;
                        characterPB.Left = left;
                        pb = characterPB;
                    }
                    if (pb != null)
                    {
                        pb.Top = top;
                        pb.Left = left;
                        grpMap.Controls.Add(pb);
                    }
                    left += BLOCK_SIZE;
                    j++;
                }
                left = BOUNDARY_PAD;
                top += BLOCK_SIZE;
                i++;
            }

            //grpMap.Controls.Add(characterPB);

            // resize Group
            grpMap.Width = NumCols * BLOCK_SIZE + BOUNDARY_PAD * 2;
            grpMap.Height = NumRows * BLOCK_SIZE + TOP_PAD + BOUNDARY_PAD;
            grpMap.Top = 5;
            grpMap.Left = 5;

            // initialize for game
            Game.GetGame().ChangeState(GameState.ON_MAP);

            // return Character object from reading map

            character = new Character(characterPB, new Position(CharacterStartRow, CharacterStartCol), this);
            return character;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        // This places the texture into each cell.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private PictureBox CreateMapCell(char legendValue, Func<string, Bitmap> LoadImg)
        {
            PictureBox result = null;
            switch (legendValue)
            {
                // walkable
                // wall
                case '1':
                    result = new PictureBox()
                    {
                        // make all this a function which can process everything better
                        BackgroundImage = LoadImg("StartingPoint"),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Width = BLOCK_SIZE,
                        Height = BLOCK_SIZE
                    };
                    break;

                // character
                case '2':
                    result = new PictureBox()
                    {
                        BackgroundImage = LoadImg("Ramp"),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Width = BLOCK_SIZE,
                        Height = BLOCK_SIZE
                    };
                    break;

                case 'c':
                    result = new PictureBox()
                    {
                        BackgroundImage = LoadImg("Ramp2"),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Width = BLOCK_SIZE,
                        Height = BLOCK_SIZE
                    };
                    break;

                // next level
                case '3':
                    result = new PictureBox()
                    {
                        BackgroundImage = LoadImg("Wall1ForGenericRPG"),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Width = BLOCK_SIZE,
                        Height = BLOCK_SIZE
                    };
                    break;

                // boss
                case '4':
                    result = new PictureBox()
                    {
                        BackgroundImage = LoadImg("Linoleum1"),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Width = BLOCK_SIZE,
                        Height = BLOCK_SIZE
                    };
                    break;

                // quit
                case '5':
                    result = new PictureBox()
                    {
                        BackgroundImage = LoadImg("Linoleum2"),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Width = BLOCK_SIZE,
                        Height = BLOCK_SIZE
                    };
                    break;
                // quit
                case '6':
                    result = new PictureBox()
                    {
                        BackgroundImage = LoadImg("Linoleum1"),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Width = BLOCK_SIZE,
                        Height = BLOCK_SIZE
                    };
                    break;
                // quit
                case '7':
                    result = new PictureBox()
                    {
                        BackgroundImage = LoadImg("Desk2"),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Width = BLOCK_SIZE,
                        Height = BLOCK_SIZE
                    };
                    break;
                // quit
                case '8':
                    result = new PictureBox()
                    {
                        BackgroundImage = LoadImg("Desk1"),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Width = BLOCK_SIZE,
                        Height = BLOCK_SIZE
                    };
                    break;
                // quit
                case '9':
                    result = new PictureBox()
                    {
                        BackgroundImage = LoadImg("Chair"),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Width = BLOCK_SIZE,
                        Height = BLOCK_SIZE
                    };
                    break;
                case 'a':
                    result = new PictureBox()
                    {
                        BackgroundImage = LoadImg("Desk3"),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Width = BLOCK_SIZE,
                        Height = BLOCK_SIZE
                    };
                    break;
                case 'b':
                    result = new PictureBox()
                    {
                        BackgroundImage = LoadImg("Desk4"),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Width = BLOCK_SIZE,
                        Height = BLOCK_SIZE
                    };
                    break;
                // quit
                case '0':
                    result = new PictureBox()
                    {
                        BackgroundImage = LoadImg("quitgame"),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Width = BLOCK_SIZE,
                        Height = BLOCK_SIZE
                    };
                    break;
                case 'z':
                    result = new PictureBox()
                    {
                        BackgroundImage = LoadImg("character"),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Width = BLOCK_SIZE,
                        Height = BLOCK_SIZE
                    };
                    break;
            }
            return result;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////





        // Again cherry tries to do too much at once. Needs to be a simple check if it is a valid position. Can then add more functions after
        // if needed, but right now this needs a clean up.
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool IsValidPos(Position pos)
        {
            if (pos.row < 0 || pos.row >= NumRows ||
                pos.col < 0 || pos.col >= NumCols ||
                layout[pos.row, pos.col] == '3')
            {
                return false;
            }
            return true;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        // Resets your position to the top left. not that great, could be worked on but I don't know why I would need this
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Position RowColToTopLeft(Position p)
        {
            return new Position(p.row * BLOCK_SIZE + TOP_PAD, p.col * BLOCK_SIZE + BOUNDARY_PAD);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
