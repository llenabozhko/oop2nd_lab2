namespace Lab2
{
  public class CalculateCommand(CalculatorEngine calculator) : CalculatorCommand(calculator)
  {
    private double firstOperand;
    private double secondOperand;
    private string operatorSymbol;

    public override void Execute()
    {
      calculator.Calculate(out _, out _, out firstOperand, out secondOperand, out operatorSymbol);
    }
  }
}
