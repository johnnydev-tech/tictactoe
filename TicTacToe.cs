using Godot;

public partial class TicTacToe : Control
{
    private Button[] buttons = new Button[9];
    private Label statusLabel;
    private Label scoreLabel;
    private Button restartButton;
    
    private bool isPlayerX = true;
    private bool gameEnded = false;
    private int xWins = 0;
    private int oWins = 0;
    private string[] board = new string[9];
    
    public override void _Ready()
    {
        GD.Print("TicTacToe script started");
        
        // Encontra os elementos da interface
        statusLabel = GetNode<Label>("VBoxContainer/Status");
        scoreLabel = GetNode<Label>("VBoxContainer/HBoxContainer/Score");
        restartButton = GetNode<Button>("VBoxContainer/HBoxContainer/RestartButton");
        
        // Encontra os botões do tabuleiro
        var gridContainer = GetNode<GridContainer>("VBoxContainer/GridContainer");
        
        for (int i = 0; i < 9; i++)
        {
            buttons[i] = gridContainer.GetChild<Button>(i);
            
            // Configura cada botão
            buttons[i].AddThemeFontSizeOverride("font_size", 48);
            
            // Conecta o clique usando closure para capturar o índice
            int index = i; // Importante: cria uma cópia local
            buttons[i].Pressed += () => OnButtonPressed(index);
            
            GD.Print($"Button {i} configured");
        }
        
        // Conecta o botão de reiniciar
        restartButton.Pressed += RestartGame;
        
        // Inicia o jogo
        ResetGame();
        
        GD.Print("TicTacToe setup complete");
    }
    
    private void OnButtonPressed(int index)
    {
        GD.Print($"Button {index} pressed");
        
        if (gameEnded || !string.IsNullOrEmpty(board[index]))
        {
            GD.Print("Invalid move");
            return;
        }
        
        // Marca o botão
        string symbol = isPlayerX ? "X" : "O";
        board[index] = symbol;
        buttons[index].Text = symbol;
        
        // Muda a cor
        if (isPlayerX)
        {
            buttons[index].AddThemeColorOverride("font_color", Colors.Red);
        }
        else
        {
            buttons[index].AddThemeColorOverride("font_color", Colors.Blue);
        }
        
        GD.Print($"Set button {index} to {symbol}");
        
        // Verifica vitória
        if (CheckWin())
        {
            gameEnded = true;
            statusLabel.Text = $"Jogador {symbol} venceu!";
            
            if (isPlayerX)
                xWins++;
            else
                oWins++;
                
            UpdateScore();
            GD.Print($"{symbol} won!");
        }
        else if (CheckTie())
        {
            gameEnded = true;
            statusLabel.Text = "Empate!";
            GD.Print("Game tied!");
        }
        else
        {
            // Troca o jogador
            isPlayerX = !isPlayerX;
            statusLabel.Text = $"Vez do {(isPlayerX ? "X" : "O")}";
            GD.Print($"Turn changed to {(isPlayerX ? "X" : "O")}");
        }
    }
    
    private bool CheckWin()
    {
        // Linhas
        for (int i = 0; i < 3; i++)
        {
            if (CheckLine(i * 3, i * 3 + 1, i * 3 + 2))
                return true;
        }
        
        // Colunas
        for (int i = 0; i < 3; i++)
        {
            if (CheckLine(i, i + 3, i + 6))
                return true;
        }
        
        // Diagonais
        if (CheckLine(0, 4, 8) || CheckLine(2, 4, 6))
            return true;
        
        return false;
    }
    
    private bool CheckLine(int a, int b, int c)
    {
        return !string.IsNullOrEmpty(board[a]) && 
               board[a] == board[b] && 
               board[b] == board[c];
    }
    
    private bool CheckTie()
    {
        for (int i = 0; i < 9; i++)
        {
            if (string.IsNullOrEmpty(board[i]))
                return false;
        }
        return true;
    }
    
    private void RestartGame()
    {
        ResetGame();
        GD.Print("Game restarted");
    }
    
    private void ResetGame()
    {
        // Limpa o tabuleiro
        for (int i = 0; i < 9; i++)
        {
            board[i] = "";
            buttons[i].Text = " ";
            buttons[i].AddThemeColorOverride("font_color", Colors.Black);
        }
        
        // Reseta o estado
        isPlayerX = true;
        gameEnded = false;
        statusLabel.Text = "Vez do X";
        
        GD.Print("Game reset");
    }
    
    private void UpdateScore()
    {
        scoreLabel.Text = $"X: {xWins} | O: {oWins}";
    }
}
