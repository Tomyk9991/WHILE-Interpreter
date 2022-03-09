using System.Runtime.CompilerServices;
using WHILE_Interpreter.Interpreter.Operators;

namespace WHILE_Interpreter.Interpreter;

public class Executor
{
    public TopLevelScope Program { get; set; }

    //representing the current visible variables
    private static VariablesList variables;
    
    public Executor(TopLevelScope scope)
    {
        this.Program = scope;
    }

    public void Run()
    {
        List<IStackable> currentScope = BuildScope();
        variables = new VariablesList();

        for (int i = 0; i < currentScope.Count; i++)
        {
            if (currentScope[i] is VariableToken variable)
                variables.AddOrUpdate(variable);

            if (currentScope[i] is AdditiveOperatorToken operatorToken)
                variables.Update(operatorToken);
        }

        Console.WriteLine();
        Console.WriteLine(variables);
    }

    private List<IStackable> BuildScope()
    {
        List<IStackable> scope = new List<IStackable>();

        while(this.Program.Stack.Count > 0)
            scope.Add(Program.Stack.Pop());

        scope.Reverse();

        return scope;
    }
    
    public static uint GetValueFromCurrentName(string value)
    {
        VariableToken token = variables.Find(t => t.Name.Value == value);

        if (token.Assignment is DigitToken t)
            return t.Value;
        
        //recursive search from name to name
        // x = 5;
        // y = x;
        // z = y;
        
        // assign x = 5;
        // assign y = x => search: x => 5 => y = 5;
        // assign z = y => search: y => 5 => x => 5;

        Console.WriteLine("searching recursively");
        return token.Assignment.Evaluate();
    }
}