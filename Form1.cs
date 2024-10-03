namespace QGameAssignment_3
{
    public partial class Form1 : Form
    {
        //Name: - Rial Parmar
        //Student Id: - 8884315
        //QGameAssignment - 3
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Event handler for the "Design" button click
        private void btnDesign_Click(object sender, EventArgs e)
        {
            // Create a new instance of the DesignGame form
            DesignGame designGame = new DesignGame();
            designGame.Show();
        }

        // Event handler for the "Play" button click
        private void btnPlay_Click(object sender, EventArgs e)
        {
            // Create a new instance of the PlayGame form
            PlayGame playGame = new PlayGame();
            playGame.Show();
        }

        // Event handler for the "Exit" button click
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}