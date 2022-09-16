namespace Lock_Picking;

internal partial class MainForm : Form
{
    private const int GameFieldWidth = 726;
    private const int GameFieldHeight = 726;

    private readonly Game game;

    public MainForm()
    {
        InitializeComponent();
        SetStyle(ControlStyles.DoubleBuffer |
                 ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.UserPaint, true);

        Width = GameFieldWidth;
        Height = GameFieldHeight;

        game = new Game(GameFieldWidth, GameFieldHeight);
    }

    private void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Space)
            game.IsKeyPressed = true;
    }

    private void MainForm_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Space)
            game.IsKeyPressed = false;
    }

    private void timer_Tick(object sender, EventArgs e)
    {
        game.Update();
        Refresh();
    }

    private void MainForm_Paint(object sender, PaintEventArgs e)
    {
        game.Draw(e.Graphics);
    }

    private void MainForm_MouseMove(object sender, MouseEventArgs e)
    {

        if (!game.IsKeyPressed)
        {
            if (e.X < Width / 3)
                game.LockpickAngle = 0;
            else if (e.X > Width * 2 / 3)
                game.LockpickAngle = 180;
            else
                game.LockpickAngle = 180 * (e.X - Width / 3) / (Width / 3);
        }
    }
}