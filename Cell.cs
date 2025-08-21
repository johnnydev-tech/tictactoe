using Godot;

public partial class Cell : Button
{
    private int cellIndex;
    private GameManager gameManager;
    private bool isOccupied = false;
    
    public override void _Ready()
    {
        // Configura tamanho responsivo baseado no container pai
        SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        SizeFlagsVertical = Control.SizeFlags.ExpandFill;
        
        // Define tamanho mínimo para manter proporção
        SetCustomMinimumSize(new Vector2(100, 100));
        
        // Configura a fonte responsiva
        AddThemeFontSizeOverride("font_size", 64);
        AddThemeColorOverride("font_color", new Color(0.2f, 0.2f, 0.2f));
        
        // Permite foco com Tab
        FocusMode = Control.FocusModeEnum.All;
        
        // Conecta sinais
        Pressed += OnCellPressed;
        FocusEntered += OnFocusEntered;
        
        GD.Print($"Cell {cellIndex} initialized");
    }
    
    public override void _GuiInput(InputEvent @event)
    {
        // Permite Enter para selecionar quando tem foco
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            if (keyEvent.Keycode == Key.Enter || keyEvent.Keycode == Key.KpEnter)
            {
                if (HasFocus())
                {
                    OnCellPressed();
                }
            }
        }
    }
    
    public void Initialize(int index, GameManager manager)
    {
        cellIndex = index;
        gameManager = manager;
        GD.Print($"Cell {cellIndex} initialized with manager");
    }
    
    private void OnCellPressed()
    {
        GD.Print($"Cell {cellIndex} pressed, occupied: {isOccupied}");
        if (!isOccupied && gameManager != null)
        {
            gameManager.OnCellClicked(cellIndex);
        }
    }
    
    private void OnFocusEntered()
    {
        if (!isOccupied)
        {
            // Destaque visual quando tem foco
            Modulate = new Color(1.1f, 1.1f, 1.1f);
        }
    }
    
    public override void _OnFocusExited()
    {
        if (!isOccupied)
        {
            // Remove destaque quando perde foco
            Modulate = Color.White;
        }
    }
    
    public void SetSymbol(string symbol)
    {
        Text = symbol;
        isOccupied = true;
        
        // Muda a cor baseada no símbolo
        if (symbol == "X")
        {
            AddThemeColorOverride("font_color", new Color(0.8f, 0.2f, 0.2f)); // Vermelho
        }
        else if (symbol == "O")
        {
            AddThemeColorOverride("font_color", new Color(0.2f, 0.2f, 0.8f)); // Azul
        }
        
        // Remove foco após selecionar
        ReleaseFocus();
        Modulate = Color.White;
        
        GD.Print($"Cell {cellIndex} set to {symbol}");
    }
    
    public void SetWinning()
    {
        Modulate = new Color(0.4f, 0.8f, 0.4f);
        GD.Print($"Cell {cellIndex} set as winning");
    }
    
    public void Reset()
    {
        Text = "";
        isOccupied = false;
        Modulate = Color.White;
        AddThemeColorOverride("font_color", new Color(0.2f, 0.2f, 0.2f));
        ReleaseFocus();
        GD.Print($"Cell {cellIndex} reset");
    }
}
