using Godot;

public partial class TicTacToe : Control
{
    private Button[] buttons = new Button[9];
    private Label statusLabel;
    private Label scoreLabel;
    private Button restartButton;
    private Button resetScoreButton;
    private Timer autoRestartTimer;
    
    private bool isPlayerX = true;
    private bool gameEnded = false;
    private int xWins = 0;
    private int oWins = 0;
    private int ties = 0;
    private string[] board = new string[9];
    
    public override void _Ready()
    {
        GD.Print("=== JOGO DA VELHA - LAYOUT MINIMALISTA ===");
        
        // Elementos da interface - Layout com MarginContainer
        statusLabel = GetNode<Label>("MainHBox/LeftPanel/LeftMargin/LeftContent/Status");
        scoreLabel = GetNode<Label>("MainHBox/LeftPanel/LeftMargin/LeftContent/Score");
        restartButton = GetNode<Button>("MainHBox/LeftPanel/LeftMargin/LeftContent/ButtonsContainer/RestartButton");
        resetScoreButton = GetNode<Button>("MainHBox/LeftPanel/LeftMargin/LeftContent/ButtonsContainer/ResetScoreButton");
        
        GD.Print("✅ Interface com MarginContainer configurada");
        
        // Botões do tabuleiro
        var gridContainer = GetNode<GridContainer>("MainHBox/RightPanel/CenterContainer/GridContainer");
        
        for (int i = 0; i < 9; i++)
        {
            buttons[i] = gridContainer.GetChild<Button>(i);
            
            // Configura o botão
            buttons[i].SetCustomMinimumSize(new Vector2(140, 140));
            buttons[i].AddThemeFontSizeOverride("font_size", 56);
            
            // Conecta o clique
            int index = i;
            buttons[i].Pressed += () => {
                GD.Print($">>> CLIQUE NO BOTÃO {index + 1} <<<");
                MakeMove(index);
            };
        }
        
        GD.Print("✅ Tabuleiro configurado");
        
        // Conecta os botões
        restartButton.Pressed += () => {
            GD.Print(">>> REINICIAR JOGO <<<");
            RestartGame();
        };
        
        resetScoreButton.Pressed += () => {
            GD.Print(">>> ZERAR PLACAR <<<");
            ResetScore();
        };
        
        // Cria e configura o timer de reinício automático
        autoRestartTimer = new Timer();
        autoRestartTimer.WaitTime = 3.0f; // 3 segundos
        autoRestartTimer.OneShot = true; // Executa apenas uma vez
        autoRestartTimer.Timeout += () => {
            GD.Print(">>> REINÍCIO AUTOMÁTICO <<<");
            RestartGame();
        };
        AddChild(autoRestartTimer);
        
        // Configura o tema moderno e bonito
        SetupModernTheme();
        
        // Inicia o jogo
        ResetGame();
        
        GD.Print("=== LAYOUT MINIMALISTA PRONTO ===");
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
            buttons[index].AddThemeColorOverride("font_color", Colors.LightBlue);
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
            UpdateStatus();
            string nextPlayer = isPlayerX ? "X" : "O";
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
        UpdateStatus();
        
        GD.Print("✅ Jogo resetado - Jogador X começa");
    }
    
    private void UpdateStatus()
    {
        if (gameEnded)
        {
            // Status de fim de jogo em branco
            statusLabel.AddThemeColorOverride("font_color", Colors.White);
        }
        else
        {
            // Cor baseada no jogador atual
            if (isPlayerX)
            {
                statusLabel.Text = "Vez do Jogador X";
                statusLabel.AddThemeColorOverride("font_color", Colors.Red);
            }
            else
            {
                statusLabel.Text = "Vez do Jogador O";
                statusLabel.AddThemeColorOverride("font_color", Colors.LightBlue);
            }
        }
    }
    
    private void UpdateScore()
    {
        if (ties > 0)
        {
            scoreLabel.Text = $"X: {xWins}  |  O: {oWins}  |  Empates: {ties}";
        }
        else
        {
            scoreLabel.Text = $"X: {xWins}  |  O: {oWins}";
        }
        GD.Print($"📊 Placar: X={xWins}, O={oWins}, Empates={ties}");
    }
    
    private void ResetScore()
    {
        xWins = 0;
        oWins = 0;
        ties = 0;
        UpdateScore();
        GD.Print("🗑️ Placar zerado!");
    }
    
    private void SetupModernTheme()
    {
        // === CONFIGURAR MARGINCONTAINER PARA ESPAÇAMENTO ===
        var leftMargin = GetNode<MarginContainer>("MainHBox/LeftPanel/LeftMargin");
        leftMargin.AddThemeConstantOverride("margin_left", 40);
        leftMargin.AddThemeConstantOverride("margin_right", 30);
        leftMargin.AddThemeConstantOverride("margin_top", 20);
        leftMargin.AddThemeConstantOverride("margin_bottom", 20);
        
        // === FONTES MAIORES SEM BACKGROUND CONFUSO ===
        
        // Título limpo (sem container)
        var title = GetNode<Label>("MainHBox/LeftPanel/LeftMargin/LeftContent/Title");
        title.AddThemeFontSizeOverride("font_size", 36);
        title.AddThemeColorOverride("font_color", Colors.White);
        
        // Status do jogo limpo (cor será definida dinamicamente)
        statusLabel.AddThemeFontSizeOverride("font_size", 28);
        
        // Placar limpo
        scoreLabel.AddThemeFontSizeOverride("font_size", 24);
        scoreLabel.AddThemeColorOverride("font_color", Colors.LightGray);
        
        // === BOTÕES 3D ESTILIZADOS ===
        
        // Botão Reiniciar - Estilo 3D azul
        var restartNormal = new StyleBoxFlat();
        restartNormal.BgColor = new Color(0.2f, 0.5f, 0.8f, 1.0f);
        restartNormal.BorderColor = new Color(0.4f, 0.7f, 1.0f, 1.0f);
        restartNormal.BorderWidthTop = 3;
        restartNormal.BorderWidthBottom = 1;
        restartNormal.BorderWidthLeft = 2;
        restartNormal.BorderWidthRight = 2;
        restartNormal.CornerRadiusTopLeft = 8;
        restartNormal.CornerRadiusTopRight = 8;
        restartNormal.CornerRadiusBottomLeft = 8;
        restartNormal.CornerRadiusBottomRight = 8;
        restartButton.AddThemeStyleboxOverride("normal", restartNormal);
        
        var restartHover = new StyleBoxFlat();
        restartHover.BgColor = new Color(0.3f, 0.6f, 0.9f, 1.0f);
        restartHover.BorderColor = new Color(0.5f, 0.8f, 1.0f, 1.0f);
        restartHover.BorderWidthTop = 3;
        restartHover.BorderWidthBottom = 1;
        restartHover.BorderWidthLeft = 2;
        restartHover.BorderWidthRight = 2;
        restartHover.CornerRadiusTopLeft = 8;
        restartHover.CornerRadiusTopRight = 8;
        restartHover.CornerRadiusBottomLeft = 8;
        restartHover.CornerRadiusBottomRight = 8;
        restartButton.AddThemeStyleboxOverride("hover", restartHover);
        
        var restartPressed = new StyleBoxFlat();
        restartPressed.BgColor = new Color(0.15f, 0.4f, 0.7f, 1.0f);
        restartPressed.BorderColor = new Color(0.1f, 0.3f, 0.6f, 1.0f);
        restartPressed.BorderWidthTop = 1;
        restartPressed.BorderWidthBottom = 3;
        restartPressed.BorderWidthLeft = 2;
        restartPressed.BorderWidthRight = 2;
        restartPressed.CornerRadiusTopLeft = 8;
        restartPressed.CornerRadiusTopRight = 8;
        restartPressed.CornerRadiusBottomLeft = 8;
        restartPressed.CornerRadiusBottomRight = 8;
        restartButton.AddThemeStyleboxOverride("pressed", restartPressed);
        
        restartButton.AddThemeFontSizeOverride("font_size", 22);
        restartButton.AddThemeColorOverride("font_color", Colors.White);
        
        // Botão Zerar Placar - Estilo 3D laranja
        var resetNormal = new StyleBoxFlat();
        resetNormal.BgColor = new Color(0.8f, 0.5f, 0.2f, 1.0f);
        resetNormal.BorderColor = new Color(1.0f, 0.7f, 0.4f, 1.0f);
        resetNormal.BorderWidthTop = 3;
        resetNormal.BorderWidthBottom = 1;
        resetNormal.BorderWidthLeft = 2;
        resetNormal.BorderWidthRight = 2;
        resetNormal.CornerRadiusTopLeft = 8;
        resetNormal.CornerRadiusTopRight = 8;
        resetNormal.CornerRadiusBottomLeft = 8;
        resetNormal.CornerRadiusBottomRight = 8;
        resetScoreButton.AddThemeStyleboxOverride("normal", resetNormal);
        
        var resetHover = new StyleBoxFlat();
        resetHover.BgColor = new Color(0.9f, 0.6f, 0.3f, 1.0f);
        resetHover.BorderColor = new Color(1.0f, 0.8f, 0.5f, 1.0f);
        resetHover.BorderWidthTop = 3;
        resetHover.BorderWidthBottom = 1;
        resetHover.BorderWidthLeft = 2;
        resetHover.BorderWidthRight = 2;
        resetHover.CornerRadiusTopLeft = 8;
        resetHover.CornerRadiusTopRight = 8;
        resetHover.CornerRadiusBottomLeft = 8;
        resetHover.CornerRadiusBottomRight = 8;
        resetScoreButton.AddThemeStyleboxOverride("hover", resetHover);
        
        var resetPressed = new StyleBoxFlat();
        resetPressed.BgColor = new Color(0.7f, 0.4f, 0.15f, 1.0f);
        resetPressed.BorderColor = new Color(0.6f, 0.3f, 0.1f, 1.0f);
        resetPressed.BorderWidthTop = 1;
        resetPressed.BorderWidthBottom = 3;
        resetPressed.BorderWidthLeft = 2;
        resetPressed.BorderWidthRight = 2;
        resetPressed.CornerRadiusTopLeft = 8;
        resetPressed.CornerRadiusTopRight = 8;
        resetPressed.CornerRadiusBottomLeft = 8;
        resetPressed.CornerRadiusBottomRight = 8;
        resetScoreButton.AddThemeStyleboxOverride("pressed", resetPressed);
        
        resetScoreButton.AddThemeFontSizeOverride("font_size", 22);
        resetScoreButton.AddThemeColorOverride("font_color", Colors.White);
        
        GD.Print("🎨 Tema moderno 3D com MarginContainer aplicado!");
    }
}
