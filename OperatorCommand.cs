namespace Lab2
{
  public class OperatorCommand(CalculatorEngine calculator, string operatorSymbol) : CalculatorCommand(calculator)
  {
    private readonly string operatorSymbol = operatorSymbol;

        public override void Execute() { calculator.ApplyOperator(operatorSymbol); }
  }
}
