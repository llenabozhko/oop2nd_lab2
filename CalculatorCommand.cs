namespace Lab2
{
  public abstract class CalculatorCommand(CalculatorEngine calculator) : ICommand
  {
    protected CalculatorEngine calculator = calculator;
    protected string oldDisplayValue = calculator.DisplayValue;
    protected string oldCalculationExpression = calculator.CalculationExpression;
    protected bool oldIsNewOperation = calculator.IsNewOperation;

    public abstract void Execute();

    public virtual void Undo()
    {
      calculator.SetDisplayValue(oldDisplayValue);
      calculator.SetCalculationExpression(oldCalculationExpression);
      calculator.SetNewOperation(oldIsNewOperation);
    }
  }
}
