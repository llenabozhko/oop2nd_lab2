namespace Lab2
{
  public class DecimalCommand(CalculatorEngine calculator) : CalculatorCommand(calculator)
  {
    public override void Execute() { calculator.AddDecimalPoint(); }
  }
}
