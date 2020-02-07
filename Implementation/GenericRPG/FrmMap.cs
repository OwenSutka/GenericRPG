﻿using GameLibrary;
using GenericRPG.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GenericRPG
{
    public partial class FrmMap : Form
    {

        private Character character;
        private Map map;
        private Game game;

        private Random rand;
        private double encounterChance;

        public FrmMap()
        {
            InitializeComponent();
        }

        public void Reload(string level, string[] savelines = null)
        {
            game = Game.GetGame();
            grpMap.Controls.Clear(); // clear out the tiles

            map = new Map();
            character = map.LoadMap(level, grpMap,
              str => Resources.ResourceManager.GetObject(str) as Bitmap
            );
            Width = grpMap.Width + 20;
            Height = grpMap.Height + 40;

            if (savelines != null)
            {
                character.SetLevel(int.Parse(savelines[1]));
                //character.Health = float.Parse(savelines[3]);
                //character.Mana = float.Parse(savelines[4]);
                //character.XP = float.Parse(savelines[2]);
                //character.GetMoney(int.Parse(savelines[5]));
            }

            game.SetCharacter(character);

            rand = new Random();
            encounterChance = 0.15;
        }

        public void ChangeMap(string level, Character charact)
        {
            game = Game.GetGame();
            grpMap.Controls.Clear(); // clear out the tiles

            map = new Map();
            character = map.LoadMap(level, grpMap,
              str => Resources.ResourceManager.GetObject(str) as Bitmap
            );
            Width = grpMap.Width + 25;
            Height = grpMap.Height + 50;

            character.SetLevel(charact.Level);
            //character.Health = charact.Health;
            //character.Mana = charact.Mana;
            //character.XP = charact.XP;
            //character.GetMoney(charact.Wallet);

            game.SetCharacter(character);

            rand = new Random();
            encounterChance = 0.15;
        }

        private void FrmMap_Load(object sender, EventArgs e)
        {
            Reload("Resources/level1.txt");
        }

        bool isKeyRepeating = false;
        private void FrmMap_KeyDown(object sender, KeyEventArgs e)
        {
            // disallow input if we're in the middle of a fight
            if (game.State == GameState.FIGHTING) return;

            MoveDir dir = MoveDir.NO_MOVE;
            if (isKeyRepeating)
            {
                e.SuppressKeyPress = true;
            }
            else
            {
                isKeyRepeating = true;
            }
            Keys x = e.KeyCode;
            switch (x)
            {
                case Keys.Left:
                    dir = MoveDir.LEFT;
                    break;
                case Keys.Right:
                    dir = MoveDir.RIGHT;
                    break;
                case Keys.Up:
                    dir = MoveDir.UP;
                    break;
                case Keys.Down:
                    dir = MoveDir.DOWN;
                    break;

                case Keys.S:
                    // SAVE HERE
                    DialogResult result = MessageBox.Show("Do you want to save the current game state?", "SAVE GAME", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        Save.SaveGame();
                    }
                    break;
                case Keys.L:
                    DialogResult response = MessageBox.Show("You are about to load a saved game. Is this correct?", "SAVE GAME", MessageBoxButtons.YesNo);
                    if (response == DialogResult.Yes)
                    {
                        var savelines = GameLibrary.Load.LoadGame();
                        Reload(savelines[0], savelines);
                    }
                    break;

            }
            if (dir != MoveDir.NO_MOVE)
            {
                // tell the character to move and check if the move was valid
                switch (character.Move(dir))
                {

                    case Task.MOVE:
                        // check for enemy encounter
                        if (rand.NextDouble() < encounterChance)
                        {
                            encounterChance = 0.15;
                            Game.GetGame().ChangeState(GameState.FIGHTING);
                        }
                        else
                        {
                            encounterChance += 0.05;
                        }
                        if (game.State == GameState.FIGHTING)
                        {
                            FrmArena frmArena = new FrmArena();
                            frmArena.Show();
                        }
                        break;
                    case Task.LEAVE_LEVEL:
                        //this should give player option of which level to go to
                        int currentLevel = Game.GetGame().Level;
                        Game.GetGame().NextLevel();
                        if (currentLevel != Game.GetGame().Level)
                        {

                            ChangeMap("Resources/level" + Game.GetGame().Level + ".txt", Game.GetGame().Character);
                        }
                        break;
                    case Task.EXIT_GAME:
                        //ExitGame();
                        break;
                }


            }

        }
        public void FrmMap_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            isKeyRepeating = false;
        }
    }
    /*
    private void ExitGame()
    {
        DialogResult answer = MessageBox.Show("You are about to quit the game. Are you sure?", "QUIT GAME", MessageBoxButtons.YesNo);
        if (answer == DialogResult.Yes)
        {
            List<Form> openForms = new List<Form>();
            foreach (Form frm in Application.OpenForms)
            {
                openForms.Add(frm);
            }
            foreach (Form openForm in openForms)
            {
                if (openForm != this)
                    openForm.Close();
            }
            Application.Exit();
        }
    }
    */


}
