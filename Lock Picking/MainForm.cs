namespace Lock_Picking
{
    partial class MainForm : Form
    {
        private const int GameFieldWidth = 726;
        private const int GameFieldHeight = 726;

        private int mouseX;

        private Game game;

        public MainForm()
        {
            InitializeComponent();

            Width = GameFieldWidth;
            Height = GameFieldHeight;

            game = new Game(GameFieldWidth, GameFieldHeight);
        }

        private void pictureGameField_Paint(object sender, PaintEventArgs e)
        {
            game.Draw(e.Graphics);
        }

        private void pictureGameField_MouseMove(object sender, MouseEventArgs e)
        {
            if(!game.isKeyPressed) 
                mouseX = e.X;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space) 
                game.isKeyPressed=true;
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
                game.isKeyPressed=false;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            game.Update(mouseX);
            pictureGameField.Refresh();
        }

    }
}