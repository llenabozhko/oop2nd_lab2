using System;

namespace Lab2
{
  public class ScientificCommand(CalculatorEngine calculator, string operation) : CalculatorCommand(calculator)
  {
    private readonly string operation = operation;

    public override void Execute()
    {
      if (operation == "π")
        calculator.ProcessNumber(Math.PI.ToString());
      else if (operation == "e")
        calculator.ProcessNumber(Math.E.ToString());
      else
        calculator.PerformScientificOperation(operation, out _, out _);
    }
  }

}
