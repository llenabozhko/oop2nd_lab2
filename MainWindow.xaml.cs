using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Lab2
{
  public partial class MainWindow : Window
  {
    private CalculatorEngine calculator;
    private CommandManager commandManager;
    private bool isScientificMode = false;

    public MainWindow()
    {
      InitializeComponent();
      calculator = new CalculatorEngine();
      commandManager = new CommandManager();
      KeyDown += MainWindow_KeyDown;
      UpdateDisplay();
    }

    private void UpdateDisplay()
    {
      ResultDisplay.Text = calculator.DisplayValue;
      CalculationDisplay.Text = calculator.CalculationExpression;
    }

    private void MainWindow_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.D1:
          ProcessNumberInput("1");
          break;
        case Key.D2:
          ProcessNumberInput("2");
          break;
        case Key.D3:
          ProcessNumberInput("3");
          break;
        case Key.D4:
          ProcessNumberInput("4");
          break;
        case Key.D5:
          ProcessNumberInput("5");
          break;
        case Key.D6:
          ProcessNumberInput("6");
          break;
        case Key.D7:
          ProcessNumberInput("7");
          break;
        case Key.D8:
          if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            ProcessOperator("*");
          else
            ProcessNumberInput("8");
          break;
        case Key.D9:
          ProcessNumberInput("9");
          break;
        case Key.D0:
          ProcessNumberInput("0");
          break;
        case Key.OemPlus:
          ProcessOperator("+");
          break;
        case Key.OemMinus:
          ProcessOperator("-");
          break;
        case Key.OemQuestion:
          ProcessOperator("/");
          break;
        case Key.Enter:
          CalculateResult();
          break;
        case Key.Escape:
          Clear_Click(null, null);
          break;
        case Key.Back:
          Backspace_Click(null, null);
          break;
        case Key.Decimal:
        case Key.OemPeriod:
        case Key.OemComma:
          Decimal_Click(null, null);
          break;
        case Key.Z:
          if ((Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) == (ModifierKeys.Control | ModifierKeys.Shift))
          {
            Redo_Click(null, null);
          }
          else if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
          {
            Undo_Click(null, null);
          }
          break;
      }
    }

    private void Number_Click(object sender, RoutedEventArgs e)
    {
      string number = ((Button)sender).Content.ToString();
      if (number == "π" || number == "e")
      {
        Scientific_Click(sender, e);
      }
      else
      {
        ProcessNumberInput(number);
      }
    }

    private void Decimal_Click(object sender, RoutedEventArgs e)
    {
      if (calculator.IsInErrorState)
      {
        calculator.ResetErrorState();
      }

      var command = new DecimalCommand(calculator);
      commandManager.ExecuteCommand(command);
      UpdateDisplay();
    }

    private void Operator_Click(object sender, RoutedEventArgs e)
    {
      string op = ((Button)sender).Content.ToString();
      ProcessOperator(op);
    }

    private void Equals_Click(object sender, RoutedEventArgs e)
    {
      CalculateResult();
    }

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
      var command = new ClearCommand(calculator);
      commandManager.ExecuteCommand(command);
      UpdateDisplay();
    }

    private void Backspace_Click(object sender, RoutedEventArgs e)
    {
      if (calculator.IsInErrorState)
      {
        calculator.ResetErrorState();
        UpdateDisplay();
        return;
      }

      if (!calculator.IsNewOperation && calculator.DisplayValue.Length > 0)
      {
        var command = new BackspaceCommand(calculator);
        commandManager.ExecuteCommand(command);
        UpdateDisplay();
      }
    }

    private void Undo_Click(object sender, RoutedEventArgs e)
    {
      commandManager.Undo();
      UpdateDisplay();
    }

    private void Redo_Click(object sender, RoutedEventArgs e)
    {
      commandManager.Redo();
      UpdateDisplay();
    }

    private void ToggleScientific_Click(object sender, RoutedEventArgs e)
    {
      isScientificMode = !isScientificMode;

      if (isScientificMode)
      {
        ExtraColumn.Width = new GridLength(1, GridUnitType.Star);
        Width += 80;
        MenuButton.Visibility = Visibility.Collapsed;
        CollapseButton.Visibility = Visibility.Visible;

        foreach (UIElement element in ButtonsGrid.Children)
        {
          if (Grid.GetColumn(element) == 4)
            element.Visibility = Visibility.Visible;
        }
      }
      else
      {
        ExtraColumn.Width = new GridLength(0);
        Width -= 80;
        MenuButton.Visibility = Visibility.Visible;
        CollapseButton.Visibility = Visibility.Collapsed;

        foreach (UIElement element in ButtonsGrid.Children)
        {
          if (Grid.GetColumn(element) == 4)
            element.Visibility = Visibility.Collapsed;
        }
      }
    }

    private void Scientific_Click(object sender, RoutedEventArgs e)
    {
      if (calculator.IsInErrorState)
      {
        calculator.ResetErrorState();
        UpdateDisplay();
      }

      string operation = ((Button)sender).Content.ToString();
      try
      {
        var command = new ScientificCommand(calculator, operation);
        commandManager.ExecuteCommand(command);
        UpdateDisplay();
      }
      catch
      {
        calculator.SetErrorState();
        UpdateDisplay();
      }
    }
    private void ProcessNumberInput(string number)
    {
      if (calculator.IsInErrorState)
        calculator.ResetErrorState();

      var command = new InputCommand(calculator, number);
      commandManager.ExecuteCommand(command);
      UpdateDisplay();
    }

    private void ProcessOperator(string op)
    {
      if (calculator.IsInErrorState)
        calculator.ResetErrorState();

      try
      {
        if (!string.IsNullOrEmpty(calculator.CalculationExpression) && !calculator.IsNewOperation)
          CalculateResult();

        var command = new OperatorCommand(calculator, op);
        commandManager.ExecuteCommand(command);
        UpdateDisplay();
      }
      catch
      {
        calculator.SetErrorState();
        UpdateDisplay();
      }
    }

    private void CalculateResult()
    {
      if (calculator.IsInErrorState || string.IsNullOrEmpty(calculator.CalculationExpression) || calculator.IsNewOperation)
        return;

      try
      {
        var command = new CalculateCommand(calculator);
        commandManager.ExecuteCommand(command);
        UpdateDisplay();
      }
      catch
      {
        calculator.SetErrorState();
        UpdateDisplay();
      }
    }
  }
}
