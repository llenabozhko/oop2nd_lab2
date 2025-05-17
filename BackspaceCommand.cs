namespace Lab2
{
  public class BackspaceCommand(CalculatorEngine calculator) : CalculatorCommand(calculator)
  {
    public override void Execute() { calculator.Backspace(); }
  }
}
