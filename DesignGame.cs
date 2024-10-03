using QGameAssignment_3.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QGameAssignment_3
{
    public partial class DesignGame : Form
    {
        //Name: - Rial Parmar
        //Student Id: - 8884315
        //QGameAssignment - 3


        public DesignGame()
        {
            InitializeComponent();
        }

        // Enumeration representing different types of tiles in the game grid.
        public enum TileTypes
        {
            blank,
            wall,
            door,
            box
        }

        // Setting initial values for the game grid layout.
        private const int START_TOP = 100;
        private const int START_LEFT = 150;
        private const int WIDTH = 50;
        private const int HEIGHT = 50;
        private const int GAP = 2;

        // Variables to store the number of rows and columns in the game grid.
        CurrentTool currentTool = new CurrentTool();
        int rows;
        int columns;

        // Enable the toolbox controls for designing the game grid.
        private void EnableToolbox()
        {
            btnNone.Enabled = true;
            btnWall.Enabled = true;

            btnGreenBox.Enabled = true;
            btnGreenDoor.Enabled = true;
            btnRedBox.Enabled = true;
            btnRedDoor.Enabled = true;

            btnGenerate.Enabled = true;
            textBoxColumns.Enabled = true;
            textBoxRows.Enabled = true;
            btnNone.PerformClick();
            btnNone.Select();
        }

        // Enumeration representing different colors of tiles in the game grid.
        public enum TileColors
        {
            none,
            blank,
            black,
            red,
            green,
        }

        private void DesignGame_Load(object sender, EventArgs e)
        {

        }



        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // Event handler for the "Save" menu item click, responsible for saving the designed game grid.
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create a SaveFileDialog to allow the user to specify the file to save.
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "QGAME files (*.qgame)|*.qgame|Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                // Variables to count the number of different tiles in the game grid.
                int usedWalls = 0;
                int redDoors = 0;
                int greenDoors = 0;
                int redBoxes = 0;
                int greenBoxes = 0;

                foreach (Tile tile in this.Controls.OfType<Tile>().ToArray())
                {
                    // Update counts based on the type and color of each tile.
                    switch (tile.tileType)
                    {
                        case "wall":
                            usedWalls++;
                            break;
                        case "door":
                            if (tile.tileColor == "red")
                                redDoors++;
                            else if (tile.tileColor == "green")
                                greenDoors++;
                            break;
                        case "box":
                            if (tile.tileColor == "red")
                                redBoxes++;
                            else if (tile.tileColor == "green")
                                greenBoxes++;
                            break;
                    }
                }

                // Write the game grid data to the selected file.
                using (StreamWriter writer = new StreamWriter(savefile.FileName, false))
                {
                    writer.WriteLine($"rows::{rows}");
                    writer.WriteLine($"columns::{columns}");

                    foreach (Tile tile in this.Controls.OfType<Tile>().ToArray())
                    {
                        writer.WriteLine($"{tile.tileColor}::{tile.tileType}");
                    }
                }

                // Display a success message along with statistics about the saved design.
                MessageBox.Show($"File saved Successfully :)\nNumber of Walls: {usedWalls}\nRed Doors: {redDoors}\nGreen Doors: {greenDoors}\nRed Boxes: {redBoxes}\nGreen Boxes: {greenBoxes}", "Design Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBoxRows_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxColumns_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string errors = "";

            if (this.Controls.OfType<Tile>().Any())
            {
                DialogResult result = MessageBox.Show("Do you want to abandon the current level and start a new one?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }

                foreach (Tile tile in this.Controls.OfType<Tile>().ToArray())
                {
                    this.Controls.Remove(tile);
                }
            }



            if (!int.TryParse(textBoxRows.Text, out rows))
            {
                MessageBox.Show("Invalid Input!!!: Int's only for Rows", "QGame", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //errors += "Invalid Input!!!: Int's only for rows.\n";
            }
            if (!int.TryParse(textBoxColumns.Text, out columns))
            {
                MessageBox.Show("Invalid Input!!!: Int's only for Columns", "QGame", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (errors == "")
            {
                int x = START_LEFT;
                int y = START_TOP;

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        Tile tile = new Tile();
                        tile.Left = x;
                        tile.Top = y;

                        tile.Height = HEIGHT;
                        tile.Width = WIDTH;

                        tile.BackColor = Color.LightGray;
                        tile.BorderStyle = BorderStyle.FixedSingle;

                        tile.tileColor = TileColors.blank.ToString();
                        tile.tileType = TileTypes.blank.ToString();

                        tile.Click += new EventHandler(Tile_Click);

                        this.Controls.Add(tile);

                        x += WIDTH + GAP;

                        Application.DoEvents();
                    }
                    y += HEIGHT + GAP;
                    x = START_LEFT;
                }
                EnableToolbox();
            }
            else
            {
                MessageBox.Show(errors, "Error");
            }
        }

        private void ToolboxButtons_Click(object sender, EventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "btnNone":
                    currentTool.toolColor = TileColors.blank.ToString();
                    currentTool.toolType = TileTypes.blank.ToString();
                    break;
                case "btnWall":
                    currentTool.toolColor = TileColors.black.ToString();
                    currentTool.toolType = TileTypes.wall.ToString();
                    break;
                case "btnRedBox":
                    currentTool.toolColor = TileColors.red.ToString();
                    currentTool.toolType = TileTypes.box.ToString();
                    break;
                case "btnRedDoor":
                    currentTool.toolColor = TileColors.red.ToString();
                    currentTool.toolType = TileTypes.door.ToString();
                    break;
                case "btnGreenBox":
                    currentTool.toolColor = TileColors.green.ToString();
                    currentTool.toolType = TileTypes.box.ToString();
                    break;
                case "btnGreenDoor":
                    currentTool.toolColor = TileColors.green.ToString();
                    currentTool.toolType = TileTypes.door.ToString();
                    break;


                default:
                    MessageBox.Show(((Button)sender).Name);
                    break;
            }
        }

        private void Tile_Click(object sender, EventArgs e)
        {
            Tile tile = (Tile)sender;
            tile.tileColor = currentTool.toolColor;
            tile.tileType = currentTool.toolType;

            string imageName = ($"{tile.tileColor} {tile.tileType}");

            switch (imageName)
            {
                case "blank blank":
                    tile.BackColor = Color.LightGray;
                    tile.Image = null;
                    tile.SizeMode = PictureBoxSizeMode.StretchImage;
                    break;
                case "black wall":
                    tile.Image = Resources.black_wall;
                    tile.SizeMode = PictureBoxSizeMode.StretchImage;
                    break;
                case "red door":
                    tile.Image = Resources.red_door;
                    tile.SizeMode = PictureBoxSizeMode.StretchImage;
                    break;
                case "red box":
                    tile.Image = Resources.red_box;
                    tile.SizeMode = PictureBoxSizeMode.StretchImage;
                    break;
                case "green door":
                    tile.Image = Resources.green_door;
                    tile.SizeMode = PictureBoxSizeMode.StretchImage;
                    break;
                case "green box":
                    tile.Image = Resources.green_box;
                    tile.SizeMode = PictureBoxSizeMode.StretchImage;
                    break;
                default:
                    tile.BorderStyle = BorderStyle.FixedSingle;
                    tile.Image = null;
                    break;
            }
        }
    }
}
