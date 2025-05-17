using System;

namespace Lab2
{
  public class CalculatorEngine
  {
    public string DisplayValue { get; private set; } = "0";
    public string CalculationExpression { get; private set; } = "";
    public bool IsNewOperation { get; private set; } = true;
    public bool IsInErrorState { get; private set; } = false;

    public void SetDisplayValue(string value) { DisplayValue = value; }

    public void SetCalculationExpression(string expression) { CalculationExpression = expression; }

    public void SetNewOperation(bool value) { IsNewOperation = value; }

    public void AddDecimalPoint()
    {
      if (IsNewOperation)
      {
        DisplayValue = "0,";
        IsNewOperation = false;
      }
      else if (!DisplayValue.Contains(","))
      {
        DisplayValue += ",";
      }
    }

    public void Backspace()
    {
      if (DisplayValue.Length == 1 || (DisplayValue.Length == 2 && DisplayValue[0] == '-'))
      {
        DisplayValue = "0";
        IsNewOperation = true;
      }
      else
      {
        DisplayValue = DisplayValue[..^1];
      }
    }

    public void Clear()
    {
      DisplayValue = "0";
      CalculationExpression = "";
      IsNewOperation = true;
    }

    public void ProcessNumber(string number)
    {
      if (IsNewOperation)
      {
        DisplayValue = number;
        IsNewOperation = false;
      }
      else
      {
        string currentValue = DisplayValue.Replace(",", "").Replace("-", "");
        if (currentValue.Length >= 24)
          return;

        if (DisplayValue == "0" && number != ",")
          DisplayValue = number;
        else
          DisplayValue += number;
      }

      // Check if the number is followed by π or e
      if (number == "π" || number == "e")
      {
        ApplyOperator("*");
      }
    }

    public void ApplyOperator(string op)
    {
      if (DisplayValue == "π")
        DisplayValue = Math.PI.ToString();
      else if (DisplayValue == "e")
        DisplayValue = Math.E.ToString();

      CalculationExpression = $"{DisplayValue} {op}";
      IsNewOperation = true;
    }



    public void PerformScientificOperation(string operation, out string oldDisplay, out string oldExpression)
    {
      oldDisplay = DisplayValue;
      oldExpression = CalculationExpression;
      double input;

      if (!double.TryParse(DisplayValue, out input))
      {
        throw new ArgumentException("Bad input!");
      }

      double result = 0;
      string newExpression = "";

      switch (operation)
      {
        case "π":
          result = Math.PI;
          newExpression = "π";
          break;
        case "e":
          result = Math.E;
          newExpression = "e";
          break;
        case "√":
          if (input < 0)
            throw new ArgumentException("√ must be processed on n >= 0");
          result = Math.Sqrt(input);
          newExpression = $"√({input})";
          break;
        case "x²":
          result = Math.Pow(input, 2);
          newExpression = $"({input})²";
          break;
        case "ln":
          if (input <= 0)
            throw new ArgumentException("ln must be processed with n > 0");
          result = Math.Log(input);
          newExpression = $"ln({input})";
          break;
        default:
          throw new ArgumentException("Error");
      }

      DisplayValue = result.ToString();
      CalculationExpression = newExpression;
      IsNewOperation = true;
    }

    public void Calculate(out string oldDisplay, out string oldExpression,
                      out double firstOperand, out double secondOperand, out string op)
    {
      oldDisplay = DisplayValue;
      oldExpression = CalculationExpression;

      string[] parts = CalculationExpression.Split(' ');

      if (!double.TryParse(parts[0], out firstOperand))
      {
        if (parts[0] == "π")
          firstOperand = Math.PI;
        else if (parts[0] == "e")
          firstOperand = Math.E;
        else
          throw new ArgumentException("something went wrong");
      }

      op = parts[1];

      if (!double.TryParse(DisplayValue, out secondOperand))
      {
        if (DisplayValue == "π")
          secondOperand = Math.PI;
        else if (DisplayValue == "e")
          secondOperand = Math.E;
        else
          throw new ArgumentException("something went wrong");
      }

      double result = 0;

      switch (op)
      {
        case "+":
          result = firstOperand + secondOperand;
          break;
        case "-":
          result = firstOperand - secondOperand;
          break;
        case "*":
          result = firstOperand * secondOperand;
          break;
        case "/":
          if (secondOperand == 0)
            throw new DivideByZeroException();
          result = firstOperand / secondOperand;
          break;
        default:
          throw new ArgumentException("Error");
      }

      CalculationExpression = $"{CalculationExpression} {DisplayValue} =";
      DisplayValue = result.ToString();
      IsNewOperation = true;
    }




    public void ResetErrorState()
    {
      if (IsInErrorState)
      {
        DisplayValue = "0";
        CalculationExpression = "";
        IsNewOperation = true;
        IsInErrorState = false;
      }
    }

    public void SetErrorState()
    {
      DisplayValue = "Error";
      CalculationExpression = "";
      IsNewOperation = true;
      IsInErrorState = true;
    }


  }
}
