using BattleShipCore;
using System.ComponentModel;

namespace BattleShip
{
    public partial class frm_Main : Form
    {
        internal List<Row> field = new();
        internal BattleshipsGame game = new BattleshipsGame();
        internal int shots = 0;

        public frm_Main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization of new game
        /// </summary>
        private void InitializeNewGame()
        {
            game.InitializeGrid();
            game.PlaceShips();
            var gameMode = MessageBox.Show("Do you want play wirh visible enemy ships?", "Choose game mode", MessageBoxButtons.YesNo);
            field.Clear();
            if (gameMode == DialogResult.Yes)
            {
                for (int j = 0; j < 10; j++)
                {
                    field.Add(new Row
                    {
                        Name = (char)('A' + j),
                        C1 = game.GetGridCellValue(0, j),
                        C2 = game.GetGridCellValue(1, j),
                        C3 = game.GetGridCellValue(2, j),
                        C4 = game.GetGridCellValue(3, j),
                        C5 = game.GetGridCellValue(4, j),
                        C6 = game.GetGridCellValue(5, j),
                        C7 = game.GetGridCellValue(6, j),
                        C8 = game.GetGridCellValue(7, j),
                        C9 = game.GetGridCellValue(8, j),
                        C10 = game.GetGridCellValue(9, j),
                    });
                }
            }
            else
            {
                for (int j = 0; j < 10; j++)
                {
                    field.Add(new Row
                    {
                        Name = (char)('A' + j),
                        C1 = '-',
                        C2 = '-',
                        C3 = '-',
                        C4 = '-',
                        C5 = '-',
                        C6 = '-',
                        C7 = '-',
                        C8 = '-',
                        C9 = '-',
                        C10 = '-',
                    });
                }
            }
            gr_Main.DataSource = new BindingList<Row>(field);
            gr_Main.AutoResizeColumns();
        }

        /// <summary>
        /// double-click on cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gr_Main_DoubleClick(object sender, EventArgs e)
        {
            var row = gr_Main.SelectedCells[0].RowIndex;
            var column = (gr_Main.SelectedCells[0].ColumnIndex) - 1;

            var cellValue = game.GetGridCellValue(column, row);
            if (cellValue == 'X' || cellValue == 'O')
            {
                toolStripStatusLabel1.Text = "You have already shot here!";
                return;
            }
            shots++;

            if (game.ShootTarget(column, row))
            {
                gr_Main.SelectedCells[0].Value = 'X';
                toolStripStatusLabel1.Text = "Hit!";
                gr_Main.SelectedCells[0].Style.BackColor = Color.Red;
                gr_Main.SelectedCells[0].Style.SelectionBackColor = Color.Red;
                if (game.CheckAllShipsSunk())
                    MessageBox.Show($"You sank all the ships in {shots} shots. Congratulations! Game over!");
                else if (game.CheckShipDestroyed(column, row))
                    toolStripStatusLabel1.Text = "Hit! Ship destroyed!";
            }
            else
            {
                gr_Main.SelectedCells[0].Value = 'O';
                toolStripStatusLabel1.Text = "Double click on cell for shoot";
            }
        }

        private void frm_Main_Load(object sender, EventArgs e)
        {
            gr_Main.RowHeadersVisible = false;
            gr_Main.AutoGenerateColumns = true;
            InitializeNewGame();
        }

        /// <summary>
        /// new game button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitializeNewGame();
        }

        /// <summary>
        /// rules button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Battleship Game for playing agains computer. " +
                $"You need to shoot by double-click on the cell for shink all enemy ships with minumum shots." +
                $"Game has 2 modes - with hidden and with open ships on the map. Enjoy your time!");
        }
    }
}