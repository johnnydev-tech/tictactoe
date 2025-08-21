using Godot;

public partial class GameManager : Control
{
    private Cell[] cells;
    private Label statusLabel;
    private Label scoreLabel;
    private Button restartButton;
    
    private bool isPlayerXTurn = true;
    private bool gameEnded = false;
    private int playerXWins = 0;
    private int playerOWins = 0;
    
    private string[,] board = new string[3, 3];
    
    public override void _Ready()
    {
        // Pega as referências dos nós com novos caminhos
        statusLabel = GetNode<Label>("MainContainer/LeftPanel/GameInfo/StatusLabel");
        scoreLabel = GetNode<Label>("MainContainer/LeftPanel/GameInfo/ScoreLabel");
        restartButton = GetNode<Button>("MainContainer/LeftPanel/RestartButton");
        
        // Conecta o botão de reiniciar
        restartButton.Pressed += RestartGame;
        
        // Inicializa as células
        InitializeCells();
        
        // Inicia o jogo
        ResetBoard();
        
        // Configura o tema da interface
        SetupTheme();
    }
    
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            // Tecla R para reiniciar
            if (keyEvent.Keycode == Key.R)
            {
                RestartGame();
            }
            // Tecla Enter para confirmar célula focada
            else if (keyEvent.Keycode == Key.Enter || keyEvent.Keycode == Key.KpEnter)
            {
                // Encontra qual célula tem foco
                for (int i = 0; i < cells.Length; i++)
                {
                    if (cells[i].HasFocus())
                    {
                        OnCellClicked(i);
                        break;
                    }
                }
            }
        }
    }
    
    private void SetupTheme()
    {
        // Configura o título
        var title = GetNode<Label>("MainContainer/LeftPanel/Title");
        title.AddThemeFontSizeOverride("font_size", 32);
        title.AddThemeColorOverride("font_color", new Color(0.9f, 0.9f, 0.9f));
        
        // Configura as instruções
        var instructionsTitle = GetNode<Label>("MainContainer/LeftPanel/Instructions/InstructionsTitle");
        instructionsTitle.AddThemeFontSizeOverride("font_size", 20);
        instructionsTitle.AddThemeColorOverride("font_color", new Color(0.8f, 0.8f, 0.8f));
        
        // Configura as instruções individuais
        for (int i = 1; i <= 3; i++)
        {
            var instruction = GetNode<Label>($"MainContainer/LeftPanel/Instructions/InstructionsList/Instruction{i}");
            instruction.AddThemeFontSizeOverride("font_size", 16);
            instruction.AddThemeColorOverride("font_color", new Color(0.7f, 0.7f, 0.7f));
        }
        
        // Configura os labels de status
        statusLabel.AddThemeFontSizeOverride("font_size", 28);
        statusLabel.AddThemeColorOverride("font_color", new Color(0.9f, 0.9f, 0.9f));
        
        scoreLabel.AddThemeFontSizeOverride("font_size", 24);
        scoreLabel.AddThemeColorOverride("font_color", new Color(0.8f, 0.8f, 0.8f));
        
        // Configura o botão de reiniciar
        restartButton.AddThemeFontSizeOverride("font_size", 18);
    }
    
    private void InitializeCells()
    {
        var gameBoard = GetNode<GridContainer>("MainContainer/RightPanel/BoardContainer/GameBoard");
        cells = new Cell[9];
        
        GD.Print($"GameBoard found: {gameBoard != null}");
        GD.Print($"GameBoard children count: {gameBoard.GetChildCount()}");
        
        for (int i = 0; i < 9; i++)
        {
            cells[i] = gameBoard.GetChild<Cell>(i);
            cells[i].Initialize(i, this);
            GD.Print($"Cell {i} initialized: {cells[i] != null}");
        }
    }
    
    public void OnCellClicked(int cellIndex)
    {
        if (gameEnded) return;
        
        int row = cellIndex / 3;
        int col = cellIndex % 3;
        
        // Verifica se a célula está vazia
        if (!string.IsNullOrEmpty(board[row, col])) return;
        
        // Marca a célula
        string currentPlayer = isPlayerXTurn ? "X" : "O";
        board[row, col] = currentPlayer;
        cells[cellIndex].SetSymbol(currentPlayer);
        
        // Verifica se há um vencedor
        if (CheckWinner())
        {
            gameEnded = true;
            statusLabel.Text = $"Jogador {currentPlayer} venceu!";
            
            if (currentPlayer == "X")
                playerXWins++;
            else
                playerOWins++;
                
            UpdateScoreLabel();
        }
        else if (IsBoardFull())
        {
            gameEnded = true;
            statusLabel.Text = "Empate!";
        }
        else
        {
            // Troca o turno
            isPlayerXTurn = !isPlayerXTurn;
            statusLabel.Text = $"Vez do Jogador {(isPlayerXTurn ? "X" : "O")}";
        }
    }
    
    private bool CheckWinner()
    {
        // Verifica linhas
        for (int row = 0; row < 3; row++)
        {
            if (!string.IsNullOrEmpty(board[row, 0]) && 
                board[row, 0] == board[row, 1] && 
                board[row, 1] == board[row, 2])
            {
                HighlightWinningCells(new int[] { row * 3, row * 3 + 1, row * 3 + 2 });
                return true;
            }
        }
        
        // Verifica colunas
        for (int col = 0; col < 3; col++)
        {
            if (!string.IsNullOrEmpty(board[0, col]) && 
                board[0, col] == board[1, col] && 
                board[1, col] == board[2, col])
            {
                HighlightWinningCells(new int[] { col, col + 3, col + 6 });
                return true;
            }
        }
        
        // Verifica diagonal principal
        if (!string.IsNullOrEmpty(board[0, 0]) && 
            board[0, 0] == board[1, 1] && 
            board[1, 1] == board[2, 2])
        {
            HighlightWinningCells(new int[] { 0, 4, 8 });
            return true;
        }
        
        // Verifica diagonal secundária
        if (!string.IsNullOrEmpty(board[0, 2]) && 
            board[0, 2] == board[1, 1] && 
            board[1, 1] == board[2, 0])
        {
            HighlightWinningCells(new int[] { 2, 4, 6 });
            return true;
        }
        
        return false;
    }
    
    private void HighlightWinningCells(int[] winningCells)
    {
        foreach (int cellIndex in winningCells)
        {
            cells[cellIndex].SetWinning();
        }
    }
    
    private bool IsBoardFull()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (string.IsNullOrEmpty(board[row, col]))
                    return false;
            }
        }
        return true;
    }
    
    private void RestartGame()
    {
        ResetBoard();
    }
    
    private void ResetBoard()
    {
        // Limpa o tabuleiro
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                board[row, col] = "";
            }
        }
        
        // Reseta as células
        foreach (Cell cell in cells)
        {
            cell.Reset();
        }
        
        // Reseta o estado do jogo
        isPlayerXTurn = true;
        gameEnded = false;
        statusLabel.Text = "Vez do Jogador X";
    }
    
    private void UpdateScoreLabel()
    {
        scoreLabel.Text = $"X: {playerXWins} | O: {playerOWins}";
    }
}
