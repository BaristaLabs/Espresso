namespace BaristaLabs.Espresso.Common
{
    using System;

    /// <summary>
    /// Represents a JavaScript engine.
    /// </summary>
    public interface IJavaScriptEngine : IDisposable
    {
        /// <summary>
        /// Gets or sets a callback that can be used to halt script execution.
        /// </summary>
        /// <remarks>
        /// During script execution the script engine periodically invokes this callback to
        /// determine whether it should continue. If the callback returns <c>false</c>, the script
        /// engine terminates script execution and throws an exception.
        /// </remarks>
        ContinuationCallback ContinuationCallback
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name associated with the script engine instance.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets the script engine's recommended file name extension for script files.
        /// </summary>
        /// <remarks>
        /// <see cref="V8ScriptEngine"/> instances return "js" for this property.
        /// </remarks>
        string FileNameExtension
        {
            get;
        }

        /// <summary>
        /// Enables or disables script code formatting.
        /// </summary>
        /// <remarks>
        /// When this property is set to <c>true</c>, the script engine may format script code
        /// before executing or compiling it. This is intended to facilitate interactive debugging.
        /// The formatting operation currently includes stripping leading and trailing blank lines
        /// and removing global indentation.
        /// </remarks>
        bool FormatCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a soft limit for the size of the Script Engine runtime's heap.
        /// </summary>
        UIntPtr MaxRuntimeHeapSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum amount by which the runtime is permitted to grow the stack during script execution.
        /// </summary>
        UIntPtr MaxRuntimeStackUsage
        {
            get;
            set;
        }

        /// <summary>
        /// Allows the host to access script resources directly.
        /// </summary>
        /// <remarks>
        /// The value of this property is an object that is bound to the script engine's root
        /// namespace. It dynamically supports properties and methods that correspond to global
        /// script objects and functions.
        /// </remarks>
        dynamic Script
        {
            get;
        }

        void AddHostObject(string itemName, object target);

        void AddHostType(Type type);

        void AddHostType(string itemName, Type type);

        /// <summary>
        /// Performs garbage collection.
        /// </summary>
        /// <param name="exhaustive"><c>True</c> to perform exhaustive garbage collection, <c>false</c> to favor speed over completeness.</param>
        void CollectGarbage(bool exhaustive);

        /// <summary>
        /// Creates a compiled script.
        /// </summary>
        /// <param name="code">The script code to compile.</param>
        /// <returns>A compiled script that can be executed multiple times without recompilation.</returns>
        ICompiledScript Compile(string code);

        /// <summary>
        /// Creates a compiled script with an associated document name.
        /// </summary>
        /// <param name="documentName">A document name for the compiled script. Currently this name is used only as a label in presentation contexts such as debugger user interfaces.</param>
        /// <param name="code">The script code to compile.</param>
        /// <returns>A compiled script that can be executed multiple times without recompilation.</returns>
        ICompiledScript Compile(string documentName, string code);

        object Evaluate(string code);

        object Evaluate(string documentName, string code);

        object Evaluate(ICompiledScript compiledScript);

        void Execute(string code);

        void Execute(string documentName, string code);

        void Execute(ICompiledScript compiledScript);

        /// <summary>
        /// Returns memory usage information for the script engine runtime.
        /// </summary>
        /// <returns>A <see cref="IRuntimeHeapInfo"/> object containing memory usage information for the script engine runtime.</returns>
        IRuntimeHeapInfo GetRuntimeHeapInfo();

        /// <summary>
        /// Gets a string representation of the script call stack.
        /// </summary>
        /// <returns>The script call stack formatted as a string.</returns>
        /// <remarks>
        /// This method returns an empty string if the script engine is not executing script code.
        /// The stack trace text format is defined by the script engine.
        /// </remarks>
        string GetStackTrace();

        object Invoke(string functionName, params object[] args);

        /// <summary>
        /// Interrupts script execution and causes the script engine to throw an exception.
        /// </summary>
        /// <remarks>
        /// This method can be called safely from any thread.
        /// </remarks>
        void Interrupt();
    }
}
