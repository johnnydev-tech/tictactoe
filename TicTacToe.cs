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
    private int ties = 0;
    private string[] board = new string[9];
    
    public override void _Ready()
    {
        GD.Print("=== JOGO DA VELHA INICIADO ===");
        
        // Encontra os elementos da interface
        statusLabel = GetNode<Label>("VBoxContainer/Status");
        scoreLabel = GetNode<Label>("VBoxContainer/HBoxContainer/Score");
        restartButton = GetNode<Button>("VBoxContainer/HBoxContainer/RestartButton");
        
        GD.Print("Labels encontrados com sucesso");
        
        // Encontra os botões do tabuleiro
        var gridContainer = GetNode<GridContainer>("VBoxContainer/GridContainer");
        
        for (int i = 0; i < 9; i++)
        {
            buttons[i] = gridContainer.GetChild<Button>(i);
            
            // Configura o botão
            buttons[i].SetCustomMinimumSize(new Vector2(120, 120));
            buttons[i].AddThemeFontSizeOverride("font_size", 48);
            
            // Conecta o clique - MÉTODO MAIS SIMPLES
            int index = i; // Cópia local para o closure
            buttons[i].Pressed += () => {
                GD.Print($">>> CLIQUE NO BOTÃO {index + 1} <<<");
                MakeMove(index);
            };
            
            GD.Print($"Botão {i + 1} configurado");
        }
        
        // Conecta o botão de reiniciar
        restartButton.Pressed += () => {
            GD.Print(">>> REINICIAR JOGO <<<");
            RestartGame();
        };
        
        // Inicia o jogo
        ResetGame();
        
        GD.Print("=== JOGO PRONTO PARA JOGAR ===");
    }
    
    private void MakeMove(int index)
    {
        GD.Print($"MakeMove chamado para posição {index}");
        
        // Verifica se o movimento é válido
        if (gameEnded)
        {
            GD.Print("Jogo já terminou!");
            return;
        }
        
        if (!string.IsNullOrEmpty(board[index]))
        {
            GD.Print($"Posição {index} já ocupada com: {board[index]}");
            return;
        }
        
        // Faz a jogada
        string currentPlayer = isPlayerX ? "X" : "O";
        board[index] = currentPlayer;
        
        // Atualiza o botão IMEDIATAMENTE
        buttons[index].Text = currentPlayer;
        
        // Define a cor
        if (isPlayerX)
        {
            buttons[index].AddThemeColorOverride("font_color", Colors.Red);
        }
        else
        {
            buttons[index].AddThemeColorOverride("font_color", Colors.Blue);
        }
        
        GD.Print($"Posição {index} marcada com '{currentPlayer}'");
        GD.Print($"Texto do botão agora é: '{buttons[index].Text}'");
        
        // Verifica vitória
        if (CheckWin())
        {
            gameEnded = true;
            statusLabel.Text = $"� JOGADOR {currentPlayer} VENCEU! �";
            
            if (isPlayerX)
                xWins++;
            else
                oWins++;
                
            UpdateScore();
            GD.Print($"� VITÓRIA DO JOGADOR {currentPlayer}! 🎉");
        }
        else if (CheckTie())
        {
            gameEnded = true;
            statusLabel.Text = "🤝 EMPATE! 🤝";
            ties++;
            UpdateScore();
            GD.Print("⚖️ JOGO EMPATADO!");
        }
        else
        {
            // Alterna o jogador
            isPlayerX = !isPlayerX;
            string nextPlayer = isPlayerX ? "X" : "O";
            statusLabel.Text = $"Vez do Jogador {nextPlayer}";
            GD.Print($"🔄 Agora é a vez do Jogador {nextPlayer}");
        }
    }
    
    private bool CheckWin()
    {
        // Combinações vencedoras
        int[][] winPatterns = {
            new int[] {0, 1, 2}, new int[] {3, 4, 5}, new int[] {6, 7, 8}, // Linhas
            new int[] {0, 3, 6}, new int[] {1, 4, 7}, new int[] {2, 5, 8}, // Colunas  
            new int[] {0, 4, 8}, new int[] {2, 4, 6}  // Diagonais
        };
        
        foreach (var pattern in winPatterns)
        {
            int a = pattern[0];
            int b = pattern[1]; 
            int c = pattern[2];
            
            if (!string.IsNullOrEmpty(board[a]) && 
                board[a] == board[b] && 
                board[b] == board[c])
            {
                // Destaca os botões vencedores
                buttons[a].Modulate = Colors.Gold;
                buttons[b].Modulate = Colors.Gold;
                buttons[c].Modulate = Colors.Gold;
                return true;
            }
        }
        
        return false;
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
        GD.Print("🔄 REINICIANDO JOGO...");
        ResetGame();
    }
    
    private void ResetGame()
    {
        // Limpa o tabuleiro
        for (int i = 0; i < 9; i++)
        {
            board[i] = "";
            buttons[i].Text = " ";
            buttons[i].AddThemeColorOverride("font_color", Colors.Black);
            buttons[i].Modulate = Colors.White;
        }
        
        // Reseta o estado
        isPlayerX = true;
        gameEnded = false;
        statusLabel.Text = "Vez do Jogador X";
        
        GD.Print("✅ Jogo resetado - Jogador X começa");
    }
    
    private void UpdateScore()
    {
        scoreLabel.Text = $"X: {xWins} | O: {oWins} | Empates: {ties}";
        GD.Print($"📊 Placar: X={xWins}, O={oWins}, Empates={ties}");
    }
}
