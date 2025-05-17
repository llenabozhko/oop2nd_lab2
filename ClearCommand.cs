namespace Lab2
{
  public class ClearCommand(CalculatorEngine calculator) : CalculatorCommand(calculator)
  {
    public override void Execute() { calculator.Clear(); }
  }
}
