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
        statusLabel = GetNode<Label>("MainHBox/LeftPanel/LeftMargin/LeftContent/Status");
        scoreLabel = GetNode<Label>("MainHBox/LeftPanel/LeftMargin/LeftContent/Score");
        restartButton = GetNode<Button>("MainHBox/LeftPanel/LeftMargin/LeftContent/ButtonsContainer/RestartButton");
        resetScoreButton = GetNode<Button>("MainHBox/LeftPanel/LeftMargin/LeftContent/ButtonsContainer/ResetScoreButton");
        
        var gridContainer = GetNode<GridContainer>("MainHBox/RightPanel/CenterContainer/GridContainer");
        
        for (int i = 0; i < 9; i++)
        {
            buttons[i] = gridContainer.GetChild<Button>(i);
            buttons[i].SetCustomMinimumSize(new Vector2(140, 140));
            buttons[i].AddThemeFontSizeOverride("font_size", 56);
            
            int index = i;
            buttons[i].Pressed += () => MakeMove(index);
        }
        
        restartButton.Pressed += () => RestartGame();
        resetScoreButton.Pressed += () => ResetScore();
        
        autoRestartTimer = new Timer();
        autoRestartTimer.WaitTime = 3.0f;
        autoRestartTimer.OneShot = true;
        autoRestartTimer.Timeout += () => RestartGame();
        AddChild(autoRestartTimer);
        
        SetupModernTheme();
        ResetGame();
    }
    
    private void MakeMove(int index)
    {
        if (gameEnded || !string.IsNullOrEmpty(board[index]))
            return;
        
        string currentPlayer = isPlayerX ? "X" : "O";
        board[index] = currentPlayer;
        buttons[index].Text = currentPlayer;
        
        if (isPlayerX)
            buttons[index].AddThemeColorOverride("font_color", Colors.Red);
        else
            buttons[index].AddThemeColorOverride("font_color", Colors.LightBlue);
        
        if (CheckWin())
        {
            gameEnded = true;
            statusLabel.Text = $"� JOGADOR {currentPlayer} VENCEU! �";
            
            if (isPlayerX)
                xWins++;
            else
                oWins++;
                
            UpdateScore();
        }
        else if (CheckTie())
        {
            gameEnded = true;
            statusLabel.Text = "🤝 EMPATE! 🤝";
            ties++;
            UpdateScore();
        }
        else
        {
            isPlayerX = !isPlayerX;
            UpdateStatus();
        }
    }
    
    private bool CheckWin()
    {
        int[][] winPatterns = {
            new int[] {0, 1, 2}, new int[] {3, 4, 5}, new int[] {6, 7, 8},
            new int[] {0, 3, 6}, new int[] {1, 4, 7}, new int[] {2, 5, 8},
            new int[] {0, 4, 8}, new int[] {2, 4, 6}
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
        ResetGame();
    }
    
    private void ResetGame()
    {
        for (int i = 0; i < 9; i++)
        {
            board[i] = "";
            buttons[i].Text = " ";
            buttons[i].AddThemeColorOverride("font_color", Colors.Black);
            buttons[i].Modulate = Colors.White;
        }
        
        isPlayerX = true;
        gameEnded = false;
        UpdateStatus();
    }
    
    private void UpdateStatus()
    {
        if (gameEnded)
        {
            statusLabel.AddThemeColorOverride("font_color", Colors.White);
        }
        else
        {
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
    }
    
    private void ResetScore()
    {
        xWins = 0;
        oWins = 0;
        ties = 0;
        UpdateScore();
    }
    
    private void SetupModernTheme()
    {
        var leftMargin = GetNode<MarginContainer>("MainHBox/LeftPanel/LeftMargin");
        leftMargin.AddThemeConstantOverride("margin_left", 40);
        leftMargin.AddThemeConstantOverride("margin_right", 30);
        leftMargin.AddThemeConstantOverride("margin_top", 20);
        leftMargin.AddThemeConstantOverride("margin_bottom", 20);
        
        var title = GetNode<Label>("MainHBox/LeftPanel/LeftMargin/LeftContent/Title");
        title.AddThemeFontSizeOverride("font_size", 36);
        title.AddThemeColorOverride("font_color", Colors.White);
        
        statusLabel.AddThemeFontSizeOverride("font_size", 28);
        
        scoreLabel.AddThemeFontSizeOverride("font_size", 24);
        scoreLabel.AddThemeColorOverride("font_color", Colors.LightGray);
        
        var restartNormal = new StyleBoxFlat();
        restartNormal.BgColor = new Color(0.2f, 0.5f, 0.8f, 1.0f);
        restartNormal.BorderColor = new Color(0.4f, 0.7f, 1.0f, 1.0f);
        restartNormal.BorderWidthTop = 3;
        restartNormal.BorderWidthBottom = 1;
        restartNormal.BorderWidthLeft = 2;
        restartNormal.BorderWidthRight = 2;
        restartNormal.SetCornerRadiusAll(8);
        restartButton.AddThemeStyleboxOverride("normal", restartNormal);
        
        var restartHover = new StyleBoxFlat();
        restartHover.BgColor = new Color(0.3f, 0.6f, 0.9f, 1.0f);
        restartHover.BorderColor = new Color(0.5f, 0.8f, 1.0f, 1.0f);
        restartHover.BorderWidthTop = 3;
        restartHover.BorderWidthBottom = 1;
        restartHover.BorderWidthLeft = 2;
        restartHover.BorderWidthRight = 2;
        restartHover.SetCornerRadiusAll(8);
        restartButton.AddThemeStyleboxOverride("hover", restartHover);
        
        var restartPressed = new StyleBoxFlat();
        restartPressed.BgColor = new Color(0.15f, 0.4f, 0.7f, 1.0f);
        restartPressed.BorderColor = new Color(0.1f, 0.3f, 0.6f, 1.0f);
        restartPressed.BorderWidthTop = 1;
        restartPressed.BorderWidthBottom = 3;
        restartPressed.BorderWidthLeft = 2;
        restartPressed.BorderWidthRight = 2;
        restartPressed.SetCornerRadiusAll(8);
        restartButton.AddThemeStyleboxOverride("pressed", restartPressed);
        
        restartButton.AddThemeFontSizeOverride("font_size", 22);
        restartButton.AddThemeColorOverride("font_color", Colors.White);
        
        var resetNormal = new StyleBoxFlat();
        resetNormal.BgColor = new Color(0.8f, 0.5f, 0.2f, 1.0f);
        resetNormal.BorderColor = new Color(1.0f, 0.7f, 0.4f, 1.0f);
        resetNormal.BorderWidthTop = 3;
        resetNormal.BorderWidthBottom = 1;
        resetNormal.BorderWidthLeft = 2;
        resetNormal.BorderWidthRight = 2;
        resetNormal.SetCornerRadiusAll(8);
        resetScoreButton.AddThemeStyleboxOverride("normal", resetNormal);
        
        var resetHover = new StyleBoxFlat();
        resetHover.BgColor = new Color(0.9f, 0.6f, 0.3f, 1.0f);
        resetHover.BorderColor = new Color(1.0f, 0.8f, 0.5f, 1.0f);
        resetHover.BorderWidthTop = 3;
        resetHover.BorderWidthBottom = 1;
        resetHover.BorderWidthLeft = 2;
        resetHover.BorderWidthRight = 2;
        resetHover.SetCornerRadiusAll(8);
        resetScoreButton.AddThemeStyleboxOverride("hover", resetHover);
        
        var resetPressed = new StyleBoxFlat();
        resetPressed.BgColor = new Color(0.7f, 0.4f, 0.15f, 1.0f);
        resetPressed.BorderColor = new Color(0.6f, 0.3f, 0.1f, 1.0f);
        resetPressed.BorderWidthTop = 1;
        resetPressed.BorderWidthBottom = 3;
        resetPressed.BorderWidthLeft = 2;
        resetPressed.BorderWidthRight = 2;
        resetPressed.SetCornerRadiusAll(8);
        resetScoreButton.AddThemeStyleboxOverride("pressed", resetPressed);
        
        resetScoreButton.AddThemeFontSizeOverride("font_size", 22);
        resetScoreButton.AddThemeColorOverride("font_color", Colors.White);
    }
}
