using WHILE_Interpreter.Interpreter.Logging;
using WHILE_Interpreter.Interpreter.Operators;

namespace WHILE_Interpreter.Interpreter;

public class RunTime
{
    public TopLevelScope Program { get; set; }
    public ILogger Logger { get; set; }
    
    //representing the current visible variables
    private static VariablesList variables;
    
    public RunTime(TopLevelScope scope, ILogger logger)
    {
        this.Program = scope;
        this.Logger = logger;
    }

    public void Run()
    {
        List<IStackable> currentScope = BuildScope();
        variables = new VariablesList();

        foreach (var stackable in currentScope)
        {
            if (stackable is VariableToken variable)
                variables.AddOrUpdate(variable);

            if (stackable is AdditiveOperatorToken operatorToken)
                variables.Update(operatorToken);

            // if (stackable is WhileToken whileToken)
            // {
            //     whileToken.
            // }
        }

        
        this.Logger.Log("Variable states:");
        this.Logger.Log(variables);
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