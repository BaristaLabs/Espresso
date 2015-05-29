namespace BaristaLabs.Espresso.Common
{
    using System;

    /// <summary>
    /// Represents a JavaScript Engine
    /// </summary>
    public interface IJavaScriptContext : IDisposable
    {
        /// <summary>
        /// Gets the name of the JavaScript engine
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets the version of the JavaScript engine
        /// </summary>
        string Version
        {
            get;
        }

        /// <summary>
        /// Returns the result of evaluating the specified expression
        /// </summary>
        /// <param name="expression">The JavaScript Expression to Evaluate</param>
        /// <returns>Result of the evaluation</returns>
        object Evaluate(IScriptSource source);

        /// <summary>
        /// Returns the strongly-typed result of a expression evaluation.
        /// </summary>
        /// <typeparam name="T">Type of result</typeparam>
        /// <param name="expression">The JavaScript expression to Evaluate</param>
        /// <returns>Strongly-typed result.</returns>
        T Evaluate<T>(IScriptSource source);

        /// <summary>
        /// Executes the specified code.
        /// </summary>
        /// <param name="code">Code</param>
        void Execute(IScriptSource source);
        
        /// <summary>
        /// Returns the result of calling with the specified name.
        /// </summary>
        /// <param name="name">Function name</param>
        /// <param name="args">Function arguments</param>
        /// <returns>Result of the function execution</returns>
        object Call(string name, params object[] args);

        /// <summary>
        /// Calls a function
        /// </summary>
        /// <typeparam name="T">Type of function result</typeparam>
        /// <param name="functionName">Function name</param>
        /// <param name="args">Function arguments</param>
        /// <returns>Result of the execution</returns>
        T Call<T>(string name, params object[] args);

        /// <summary>
        /// Returns a value that indicates if an object with the specified name has been defined.
        /// </summary>
        /// <param name="name">Name of the object</param>
        /// <returns>A value that indicates if the object is defined.</returns>
        bool IsDefined(string name);

        /// <summary>
        /// Returns the value of the variable with the specified name.
        /// </summary>
        /// <param name="variableName">Variable name</param>
        /// <returns>Value of variable</returns>
        object GetVariable(string variableName);

        /// <summary>
        /// Gets a value of variable
        /// </summary>
        /// <typeparam name="T">Type of variable</typeparam>
        /// <param name="variableName">Variable name</param>
        /// <returns>Value of variable</returns>
        T GetVariable<T>(string variableName);

        /// <summary>
        /// Sets a value of variable
        /// </summary>
        /// <param name="variableName">Variable name</param>
        /// <param name="value">Value of variable</param>
        void SetVariable(string variableName, object value);

        /// <summary>
        /// Removes a variable
        /// </summary>
        /// <param name="variableName">Variable name</param>
        void Remove(string variableName);
    }
}