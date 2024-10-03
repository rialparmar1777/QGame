using QGameAssignment_3.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QGameAssignment_3
{
    //QGame Project 
    //It has some minor errors but tried to make it work perfectly
    public partial class PlayGame : Form
    {
        public PlayGame()
        {
            InitializeComponent();
            DoubleBuffered = true;
            DisableControls();
        }

        // Enumeration representing different types of tiles in the game grid.
        public enum TileTypes
        {
            blank,
            wall,
            door,
            box
        }

        // Enumeration representing different types of colors in the game grid.
        public enum TileColors
        {
            blank,
            black,
            red,
            green,
            blue,
            yellow
        }

        // Setting all the initial values for the game grid layout.
        private const int START_TOP = 30;
        private const int START_LEFT = 175;
        private const int WIDTH = 50;
        private const int HEIGHT = 50;
        private const int SPACE = 2;
        int totalBoxes;

        // Enable game controls to allow player interaction.
        public void EnableControls()
        {
            btnUp.Enabled = true;
            btnDown.Enabled = true;
            btnLeft.Enabled = true;
            btnRight.Enabled = true;
            loadGameToolStripMenuItem.Enabled = true;
        }

        // Represents the currently selected tile in the game grid.
        Tile selectedTile = new Tile();
        int rows;
        int columns;
        int moves = 0;
        bool correctDoor = false;
        bool isBox = false;

        // Disable game controls to prevent player interaction.
        public void DisableControls()
        {
            btnUp.Enabled = false;
            btnDown.Enabled = false;
            btnLeft.Enabled = false;
            btnRight.Enabled = false;
            loadGameToolStripMenuItem.Enabled = true;
            textBoxRB.Enabled = false;
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // Event handler for the "Load Game" menu item click
        private void loadGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create an OpenFileDialog to allow the user to choose a file.
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "QGAME files (*.qgame)|*.qgame|Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openfile.ShowDialog() == DialogResult.OK)
            {
                var levelList = new List<string>();
                string record;
                try
                {
                    using (StreamReader reader = new StreamReader(openfile.FileName))
                    {
                        while ((record = reader.ReadLine()) != null)
                        {
                            levelList.Add(record);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Error: Something went wrong.");
                }
                string[] loadArray = levelList.ToArray();

                int x = START_LEFT;
                int y = START_TOP;

                rows = int.Parse(Regex.Match(loadArray[0], @"\d+").Value);
                columns = int.Parse(Regex.Match(loadArray[1], @"\d+").Value);
                int loadCounter = 1;

                totalBoxes = 0;
                int remainingBoxes = 0;

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        loadCounter++;

                        Tile tile = new Tile();

                        tile.Left = x;
                        tile.Top = y;

                        tile.Height = HEIGHT;
                        tile.Width = WIDTH;

                        tile.SizeMode = PictureBoxSizeMode.StretchImage;
                        tile.BorderStyle = BorderStyle.FixedSingle;

                        tile.Click += new EventHandler(Tile_Click);

                        switch (loadArray[loadCounter])
                        {
                            // Set properties based on the loaded data
                            case "blank::blank":
                                tile.BackColor = Color.LightGray;
                                tile.tileColor = TileColors.blank.ToString();
                                tile.tileType = TileTypes.blank.ToString();
                                break;
                            case "black::wall":
                                tile.Image = Resources.black_wall;
                                tile.tileColor = TileColors.black.ToString();
                                tile.tileType = TileTypes.wall.ToString();
                                break;
                            case "red::box":
                                tile.Image = Resources.red_box;
                                tile.tileColor = TileColors.red.ToString();
                                tile.tileType = TileTypes.box.ToString();
                                totalBoxes++;
                                break;
                            case "red::door":
                                tile.Image = Resources.red_door;
                                tile.tileColor = TileColors.red.ToString();
                                tile.tileType = TileTypes.door.ToString();
                                break;
                            case "green::box":
                                tile.Image = Resources.green_box;
                                tile.tileColor = TileColors.green.ToString();
                                tile.tileType = TileTypes.box.ToString();
                                totalBoxes++;
                                break;
                            case "green::door":
                                tile.Image = Resources.green_door;
                                tile.tileColor = TileColors.green.ToString();
                                tile.tileType = TileTypes.door.ToString();
                                break;

                            default:
                                tile.BackColor = Color.LightGray;
                                tile.BorderStyle = BorderStyle.FixedSingle;
                                tile.tileColor = TileColors.blank.ToString();
                                tile.tileType = TileTypes.blank.ToString();
                                break;
                        }

                        this.Controls.Add(tile);

                        x += WIDTH + SPACE;

                        Application.DoEvents();
                    }
                    y += HEIGHT + SPACE;
                    x = START_LEFT;
                }

                // Remove blank tiles
                foreach (Tile tile in this.Controls.OfType<Tile>().ToArray())
                {
                    if (tile.tileType == TileTypes.blank.ToString())
                    {
                        this.Controls.Remove(tile);
                        Application.DoEvents();
                    }
                }

                remainingBoxes = totalBoxes;
                textBoxRB.Text = remainingBoxes.ToString();

                EnableControls();
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void textBoxRB_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxMoves_TextChanged(object sender, EventArgs e)
        {

        }

        // Check if the grid in the specified direction is valid
        public bool GetGrid(string direction)
        {
            foreach (Tile tile in this.Controls.OfType<Tile>().ToArray())
            {
                switch (direction)
                {
                    // Check upward movement
                    case "up":
                        if (selectedTile.Left == tile.Left && selectedTile.Top == (tile.Bottom + SPACE) && !tile.selected)
                        {
                            if (tile.tileType == TileTypes.door.ToString() && selectedTile.tileColor == tile.tileColor)
                            {
                                correctDoor = true;
                            }
                            return true;
                        }
                        break;
                    // Check downward movement
                    case "down":
                        if (selectedTile.Left == tile.Left && selectedTile.Bottom == (tile.Top - SPACE) && !tile.selected)
                        {
                            if (tile.tileType == TileTypes.door.ToString() && selectedTile.tileColor == tile.tileColor)
                            {
                                correctDoor = true;
                            }
                            return true;
                        }
                        break;
                    // Check leftward movement
                    case "left":
                        if (selectedTile.Left == (tile.Right + SPACE) && selectedTile.Top == tile.Top && !tile.selected)
                        {
                            if (tile.tileType == TileTypes.door.ToString() && selectedTile.tileColor == tile.tileColor)
                            {
                                correctDoor = true;
                            }
                            return true;
                        }
                        break;
                    // Check rightward movement
                    case "right":
                        if (selectedTile.Right == (tile.Left - SPACE) && selectedTile.Top == tile.Top && !tile.selected)
                        {
                            if (tile.tileType == TileTypes.door.ToString() && selectedTile.tileColor == tile.tileColor)
                            {
                                correctDoor = true;
                            }
                            return true;
                        }
                        break;
                }
            }
            correctDoor = false;
            return false;
        }

        // Check if the player has won the game
        public bool CheckWinMessage()
        {
            foreach (Tile tile in this.Controls.OfType<Tile>().ToArray())
            {
                if (tile.tileType == TileTypes.box.ToString())
                {
                    return false;
                }
            }
            textBoxRB.Text = "0";
            MessageBox.Show(
    "╔════════════════════════╗\n" +
    "║  Congratulations!! 🏆  ║\n" +
    "║  Game End 🎮          ║\n" +
    "╚════════════════════════╝",
    "QGame",
    MessageBoxButtons.OK,
    MessageBoxIcon.Information
);

            ResetForm();
            return true;
        }

        // Event handler for clicking on a tile
        private void Tile_Click(object sender, EventArgs e)
        {
            foreach (Tile tile in this.Controls.OfType<Tile>().ToArray())
            {
                tile.selected = false;
            }

            selectedTile.Padding = new System.Windows.Forms.Padding(all: 0);
            selectedTile.BackColor = Color.Transparent;

            if (((Tile)sender).tileType == TileTypes.box.ToString())
            {
                isBox = true;

                selectedTile.Padding = new System.Windows.Forms.Padding(all: 0);

                selectedTile = (Tile)sender;
                selectedTile.selected = true;

                selectedTile.Padding = new System.Windows.Forms.Padding(all: 2);
                selectedTile.BackColor = Color.Gray;
                selectedTile.BorderStyle = BorderStyle.FixedSingle;
            }
            else
            {
                isBox = false;
            }
        }

        // Event handler for form load
        private void DesignGame_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            DisableControls();
        }

        private void ResetForm()
        {
            moves = 0;
            textBoxMoves.Text = "0";
            totalBoxes = 0;
            textBoxRB.Text = "0";

            foreach (Tile tile in this.Controls.OfType<Tile>().ToArray())
            {
                this.Controls.Remove(tile);
            }



            DisableControls();
        }

        private void PlayGame_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            DisableControls();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (isBox)
            {
                lblMoves.Select();
                moves++;
                textBoxMoves.Text = moves.ToString();
                DisableControls();
                while (!GetGrid("up"))
                {
                    selectedTile.Top -= SPACE;
                }
                EnableControls();
                if (correctDoor)
                {
                    this.Controls.Remove(selectedTile);
                    totalBoxes = totalBoxes - 1;

                    textBoxRB.Text = totalBoxes.ToString();
                }
            }
            else
            {
                MessageBox.Show("Please select a box.");
            }
            CheckWinMessage();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (isBox)
            {
                lblMoves.Select();
                moves++;
                textBoxMoves.Text = moves.ToString();
                DisableControls();
                while (!GetGrid("down"))
                {
                    selectedTile.Top += SPACE;
                }
                EnableControls();
                if (correctDoor)
                {
                    this.Controls.Remove(selectedTile);
                    totalBoxes = totalBoxes - 1;

                    textBoxRB.Text = totalBoxes.ToString();
                }
            }
            else
            {
                MessageBox.Show("Please select a box.");
            }
            CheckWinMessage();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (isBox)
            {
                lblMoves.Select();
                moves++;
                textBoxMoves.Text = moves.ToString();
                DisableControls();
                while (!GetGrid("left"))
                {
                    selectedTile.Left -= SPACE;
                }
                EnableControls();
                if (correctDoor)
                {
                    this.Controls.Remove(selectedTile);
                    totalBoxes = totalBoxes - 1;

                    textBoxRB.Text = totalBoxes.ToString();
                }
            }
            else
            {
                MessageBox.Show("Please select a box.");
            }
            CheckWinMessage();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (isBox)
            {
                lblMoves.Select();
                moves++;
                textBoxMoves.Text = moves.ToString();
                DisableControls();
                while (!GetGrid("right"))
                {
                    selectedTile.Left += SPACE;
                }
                EnableControls();
                if (correctDoor)
                {
                    this.Controls.Remove(selectedTile);
                    textBoxRB.Text = totalBoxes.ToString();
                }
            }
            else
            {
                MessageBox.Show("Please select a box.");
            }
            CheckWinMessage();
        }
    }
}
