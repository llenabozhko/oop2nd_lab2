namespace Lab2
{
  public class InputCommand(CalculatorEngine calculator, string number) : CalculatorCommand(calculator)
  {
    private string number = number;

    public override void Execute() {calculator.ProcessNumber(number);}
  }
}
