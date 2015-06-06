namespace BaristaLabs.Espresso.Engine.V8
{
    using Common;
    using Extensions;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public sealed class V8ScriptEngine : IScriptEngine
    {
        #region data
        private ScriptAccess defaultAccess;
        private readonly V8ScriptEngineFlags engineFlags;
        private readonly V8ContextProxy proxy;
        private readonly object script;
        private DisposedFlag disposedFlag = new DisposedFlag();

        private const int continuationInterval = 2000;
        private bool inContinuationTimerScope;

        private readonly HostItemCollateral hostItemCollateral;
        private readonly IUniqueNameManager nameManager = new UniqueNameManager();
        private readonly IUniqueNameManager documentNameManager = new UniqueFileNameManager();
        private List<string> documentNames;

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new V8 script engine instance.
        /// </summary>
        /// <remarks>
        /// A separate V8 runtime is created for the new script engine instance.
        /// </remarks>
        public V8ScriptEngine()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new V8 script engine instance with the specified name.
        /// </summary>
        /// <param name="name">A name to associate with the instance. Currently this name is used only as a label in presentation contexts such as debugger user interfaces.</param>
        /// <remarks>
        /// A separate V8 runtime is created for the new script engine instance.
        /// </remarks>
        public V8ScriptEngine(string name)
            : this(name, null)
        {
        }

        /// <summary>
        /// Initializes a new V8 script engine instance with the specified resource constraints.
        /// </summary>
        /// <param name="constraints">Resource constraints for the V8 runtime (see remarks).</param>
        /// <remarks>
        /// A separate V8 runtime is created for the new script engine instance.
        /// </remarks>
        public V8ScriptEngine(V8RuntimeConstraints constraints)
            : this(null, constraints)
        {
        }

        /// <summary>
        /// Initializes a new V8 script engine instance with the specified name and resource constraints.
        /// </summary>
        /// <param name="name">A name to associate with the instance. Currently this name is used only as a label in presentation contexts such as debugger user interfaces.</param>
        /// <param name="constraints">Resource constraints for the V8 runtime (see remarks).</param>
        /// <remarks>
        /// A separate V8 runtime is created for the new script engine instance.
        /// </remarks>
        public V8ScriptEngine(string name, V8RuntimeConstraints constraints)
            : this(name, constraints, V8ScriptEngineFlags.None)
        {
        }

        /// <summary>
        /// Initializes a new V8 script engine instance with the specified options.
        /// </summary>
        /// <param name="flags">A value that selects options for the operation.</param>
        /// <remarks>
        /// A separate V8 runtime is created for the new script engine instance.
        /// </remarks>
        public V8ScriptEngine(V8ScriptEngineFlags flags)
            : this(flags, 0)
        {
        }

        /// <summary>
        /// Initializes a new V8 script engine instance with the specified options and debug port.
        /// </summary>
        /// <param name="flags">A value that selects options for the operation.</param>
        /// <param name="debugPort">A TCP/IP port on which to listen for a debugger connection.</param>
        /// <remarks>
        /// A separate V8 runtime is created for the new script engine instance.
        /// </remarks>
        public V8ScriptEngine(V8ScriptEngineFlags flags, int debugPort)
            : this(null, null, flags, debugPort)
        {
        }

        /// <summary>
        /// Initializes a new V8 script engine instance with the specified name and options.
        /// </summary>
        /// <param name="name">A name to associate with the instance. Currently this name is used only as a label in presentation contexts such as debugger user interfaces.</param>
        /// <param name="flags">A value that selects options for the operation.</param>
        /// <remarks>
        /// A separate V8 runtime is created for the new script engine instance.
        /// </remarks>
        public V8ScriptEngine(string name, V8ScriptEngineFlags flags)
            : this(name, flags, 0)
        {
        }

        /// <summary>
        /// Initializes a new V8 script engine instance with the specified name, options, and debug port.
        /// </summary>
        /// <param name="name">A name to associate with the instance. Currently this name is used only as a label in presentation contexts such as debugger user interfaces.</param>
        /// <param name="flags">A value that selects options for the operation.</param>
        /// <param name="debugPort">A TCP/IP port on which to listen for a debugger connection.</param>
        /// <remarks>
        /// A separate V8 runtime is created for the new script engine instance.
        /// </remarks>
        public V8ScriptEngine(string name, V8ScriptEngineFlags flags, int debugPort)
            : this(name, null, flags, debugPort)
        {
        }

        /// <summary>
        /// Initializes a new V8 script engine instance with the specified resource constraints and options.
        /// </summary>
        /// <param name="constraints">Resource constraints for the V8 runtime (see remarks).</param>
        /// <param name="flags">A value that selects options for the operation.</param>
        /// <remarks>
        /// A separate V8 runtime is created for the new script engine instance.
        /// </remarks>
        public V8ScriptEngine(V8RuntimeConstraints constraints, V8ScriptEngineFlags flags)
            : this(constraints, flags, 0)
        {
        }

        /// <summary>
        /// Initializes a new V8 script engine instance with the specified resource constraints, options, and debug port.
        /// </summary>
        /// <param name="constraints">Resource constraints for the V8 runtime (see remarks).</param>
        /// <param name="flags">A value that selects options for the operation.</param>
        /// <param name="debugPort">A TCP/IP port on which to listen for a debugger connection.</param>
        /// <remarks>
        /// A separate V8 runtime is created for the new script engine instance.
        /// </remarks>
        public V8ScriptEngine(V8RuntimeConstraints constraints, V8ScriptEngineFlags flags, int debugPort)
            : this(null, constraints, flags, debugPort)
        {
        }

        /// <summary>
        /// Initializes a new V8 script engine instance with the specified name, resource constraints, and options.
        /// </summary>
        /// <param name="name">A name to associate with the instance. Currently this name is used only as a label in presentation contexts such as debugger user interfaces.</param>
        /// <param name="constraints">Resource constraints for the V8 runtime (see remarks).</param>
        /// <param name="flags">A value that selects options for the operation.</param>
        /// <remarks>
        /// A separate V8 runtime is created for the new script engine instance.
        /// </remarks>
        public V8ScriptEngine(string name, V8RuntimeConstraints constraints, V8ScriptEngineFlags flags)
            : this(name, constraints, flags, 0)
        {
        }

        /// <summary>
        /// Initializes a new V8 script engine instance with the specified name, resource constraints, options, and debug port.
        /// </summary>
        /// <param name="name">A name to associate with the instance. Currently this name is used only as a label in presentation contexts such as debugger user interfaces.</param>
        /// <param name="constraints">Resource constraints for the V8 runtime (see remarks).</param>
        /// <param name="flags">A value that selects options for the operation.</param>
        /// <param name="debugPort">A TCP/IP port on which to listen for a debugger connection.</param>
        /// <remarks>
        /// A separate V8 runtime is created for the new script engine instance.
        /// </remarks>
        public V8ScriptEngine(string name, V8RuntimeConstraints constraints, V8ScriptEngineFlags flags, int debugPort)
            : this(null, name, constraints, flags, debugPort)
        {
        }

        internal V8ScriptEngine(V8Runtime runtime, string name, V8RuntimeConstraints constraints, V8ScriptEngineFlags flags, int debugPort)
        {
            Name = nameManager.GetUniqueName(name, GetType().GetRootName());

            using (var localRuntime = (runtime != null) ? null : new V8Runtime(name, constraints))
            {
                var activeRuntime = runtime ?? localRuntime;
                hostItemCollateral = activeRuntime.HostItemCollateral;

                engineFlags = flags;
                proxy = V8ContextProxy.Create(activeRuntime.IsolateProxy, Name, flags.HasFlag(V8ScriptEngineFlags.EnableDebugging), flags.HasFlag(V8ScriptEngineFlags.DisableGlobalMembers), debugPort);
                script = GetRootItem();

                var engineInternal = Evaluate(
                    MiscHelpers.FormatInvariant("{0} [internal]", GetType().Name),
                    false,
                    @"
                        EngineInternal = (function () {

                            function convertArgs(args) {
                                var result = [];
                                var count = args.Length;
                                for (var i = 0; i < count; i++) {
                                    result.push(args[i]);
                                }
                                return result;
                            }

                            function construct(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) {
                                return new this(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
                            }

                            return {

                                getCommandResult: function (value) {
                                    if (value != null) {
                                        if (((typeof(value) == 'object') && !value.hasOwnProperty('{c2cf47d3-916b-4a3f-be2a-6ff567425808}')) || (typeof(value) == 'function')) {
                                            if (typeof(value.toString) == 'function') {
                                                return value.toString();
                                            }
                                        }
                                    }
                                    return value;
                                },

                                invokeConstructor: function (constructor, args) {
                                    if (typeof(constructor) != 'function') {
                                        throw new Error('Function expected');
                                    }
                                    return construct.apply(constructor, convertArgs(args));
                                },

                                invokeMethod: function (target, method, args) {
                                    if (typeof(method) != 'function') {
                                        throw new Error('Function expected');
                                    }
                                    return method.apply(target, convertArgs(args));
                                },

                                getStackTrace: function () {
                                    try {
                                        throw new Error('[stack trace]');
                                    }
                                    catch (exception) {
                                        return exception.stack;
                                    }
                                    return '';
                                }
                            };
                        })();
                    "
                );

                ((IDisposable)engineInternal).Dispose();
            }
        }

        #endregion

        #region public members

        public string Name
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the default script access setting for all members of exposed objects.
        /// </summary>
        /// <remarks>
        /// Use <see cref="DefaultScriptUsageAttribute"/>, <see cref="ScriptUsageAttribute"/>, or
        /// their subclasses to override this property for individual types and members. Note that
        /// this property has no effect on the method binding algorithm. If a script-based call is
        /// bound to a method that is blocked by this property, it will be rejected even if an
        /// overload exists that could receive the call.
        /// </remarks>
        public ScriptAccess DefaultAccess
        {
            get { return defaultAccess; }
            set
            {
                defaultAccess = value;
                OnAccessSettingsChanged();
            }
        }

        public bool FormatCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a soft limit for the size of the V8 runtime's heap.
        /// </summary>
        /// <remarks>
        /// This property is specified in bytes. When it is set to the default value, heap size
        /// monitoring is disabled, and scripts with memory leaks or excessive memory usage
        /// can cause unrecoverable errors and process termination.
        /// <para>
        /// A V8 runtime unconditionally terminates the process when it exceeds its resource
        /// constraints (see <see cref="V8RuntimeConstraints"/>). This property enables external
        /// heap size monitoring that can prevent termination in some scenarios. To be effective,
        /// it should be set to a value that is significantly lower than
        /// <see cref="V8RuntimeConstraints.MaxOldSpaceSize"/>. Note that enabling heap size
        /// monitoring results in slower script execution.
        /// </para>
        /// <para>
        /// Exceeding this limit causes the V8 runtime to interrupt script execution and throw an
        /// exception. To re-enable script execution, set this property to a new value.
        /// </para>
        /// </remarks>
        public UIntPtr MaxRuntimeHeapSize
        {
            get
            {
                VerifyNotDisposed();
                return proxy.MaxRuntimeHeapSize;
            }

            set
            {
                VerifyNotDisposed();
                proxy.MaxRuntimeHeapSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum time interval between consecutive heap size samples.
        /// </summary>
        /// <remarks>
        /// This property is effective only when heap size monitoring is enabled (see
        /// <see cref="MaxRuntimeHeapSize"/>).
        /// </remarks>
        public TimeSpan RuntimeHeapSizeSampleInterval
        {
            get
            {
                VerifyNotDisposed();
                return proxy.RuntimeHeapSizeSampleInterval;
            }

            set
            {
                VerifyNotDisposed();
                proxy.RuntimeHeapSizeSampleInterval = value;
            }
        }

        internal ScriptFrame CurrentScriptFrame
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the maximum amount by which the V8 runtime is permitted to grow the stack during script execution.
        /// </summary>
        /// <remarks>
        /// This property is specified in bytes. When it is set to the default value, no stack
        /// usage limit is enforced, and scripts with unchecked recursion or other excessive stack
        /// usage can cause unrecoverable errors and process termination.
        /// <para>
        /// Note that the V8 runtime does not monitor stack usage while a host call is in progress.
        /// Monitoring is resumed when control returns to the runtime.
        /// </para>
        /// </remarks>
        public UIntPtr MaxRuntimeStackUsage
        {
            get
            {
                VerifyNotDisposed();
                return proxy.MaxRuntimeStackUsage;
            }

            set
            {
                VerifyNotDisposed();
                proxy.MaxRuntimeStackUsage = value;
            }
        }

        /// <summary>
        /// Creates a compiled script.
        /// </summary>
        /// <param name="code">The script code to compile.</param>
        /// <returns>A compiled script that can be executed multiple times without recompilation.</returns>
        public V8Script Compile(string code)
        {
            return Compile(null, code);
        }

        ICompiledScript IScriptEngine.Compile(string code)
        {
            return Compile(null, code);
        }

        /// <summary>
        /// Creates a compiled script with an associated document name.
        /// </summary>
        /// <param name="documentName">A document name for the compiled script. Currently this name is used only as a label in presentation contexts such as debugger user interfaces.</param>
        /// <param name="code">The script code to compile.</param>
        /// <returns>A compiled script that can be executed multiple times without recompilation.</returns>
        public V8Script Compile(string documentName, string code)
        {
            VerifyNotDisposed();

            return ScriptInvoke(() =>
            {
                var uniqueName = documentNameManager.GetUniqueName(documentName, "Script Document");
                return proxy.Compile(uniqueName, FormatCode ? MiscHelpers.FormatCode(code) : code);
            });
        }

        ICompiledScript IScriptEngine.Compile(string documentName, string code)
        {
            return Compile(documentName, code);
        }

        /// <summary>
        /// Evaluates script code.
        /// </summary>
        /// <param name="code">The script code to evaluate.</param>
        /// <returns>The result value.</returns>
        /// <remarks>
        /// In some script languages the distinction between statements and expressions is
        /// significant but ambiguous for certain syntactic elements. This method always
        /// interprets the specified script code as an expression.
        /// <para>
        /// If a debugger is attached, it will present the specified script code to the user as a
        /// document with an automatically selected name. This document will be discarded after
        /// execution.
        /// </para>
        /// <para>
        /// For information about the types of result values that script code can return, see
        /// <see cref="Evaluate(string, bool, string)"/>.
        /// </para>
        /// </remarks>
        public object Evaluate(string code)
        {
            return Evaluate(null, code);
        }

        /// <summary>
        /// Evaluates script code with an associated document name.
        /// </summary>
        /// <param name="documentName">A document name for the script code. Currently this name is used only as a label in presentation contexts such as debugger user interfaces.</param>
        /// <param name="code">The script code to evaluate.</param>
        /// <returns>The result value.</returns>
        /// <remarks>
        /// In some script languages the distinction between statements and expressions is
        /// significant but ambiguous for certain syntactic elements. This method always
        /// interprets the specified script code as an expression.
        /// <para>
        /// If a debugger is attached, it will present the specified script code to the user as a
        /// document with the specified name. This document will be discarded after execution.
        /// </para>
        /// <para>
        /// For information about the types of result values that script code can return, see
        /// <see cref="Evaluate(string, bool, string)"/>.
        /// </para>
        /// </remarks>
        public object Evaluate(string documentName, string code)
        {
            return Evaluate(documentName, true, code);
        }

        /// <summary>
        /// Evaluates script code with an associated document name, optionally discarding the document after execution.
        /// </summary>
        /// <param name="documentName">A document name for the script code. Currently this name is used only as a label in presentation contexts such as debugger user interfaces.</param>
        /// <param name="discard"><c>True</c> to discard the script document after execution, <c>false</c> otherwise.</param>
        /// <param name="code">The script code to evaluate.</param>
        /// <returns>The result value.</returns>
        /// <remarks>
        /// In some script languages the distinction between statements and expressions is
        /// significant but ambiguous for certain syntactic elements. This method always
        /// interprets the specified script code as an expression.
        /// <para>
        /// If a debugger is attached, it will present the specified script code to the user as a
        /// document with the specified name. Discarding this document removes it from view but
        /// has no effect on the script engine.
        /// </para>
        /// <para>
        /// The following table summarizes the types of result values that script code can return.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Type</term>
        ///         <term>Returned&#xA0;As</term>
        ///         <description>Remarks</description>
        ///     </listheader>
        ///     <item>
        ///         <term><b>String</b></term>
        ///         <term><see href="http://msdn.microsoft.com/en-us/library/system.string.aspx">System.String</see></term>
        ///         <description>N/A</description>
        ///     </item>
        ///     <item>
        ///         <term><b>Boolean</b></term>
        ///         <term><see href="http://msdn.microsoft.com/en-us/library/system.boolean.aspx">System.Boolean</see></term>
        ///         <description>N/A</description>
        ///     </item>
        ///     <item>
        ///         <term><b>Number</b></term>
        ///         <term><see href="http://msdn.microsoft.com/en-us/library/system.int32.aspx">System.Int32</see>&#xA0;or&#xA0;<see href="http://msdn.microsoft.com/en-us/library/system.double.aspx">System.Double</see></term>
        ///         <description>
        ///         Other numeric types are possible. The exact conversions between script and .NET
        ///         numeric types are defined by the script engine.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term><b>Null&#xA0;Reference</b></term>
        ///         <term><c>null</c></term>
        ///         <description>N/A</description>
        ///     </item>
        ///     <item>
        ///         <term><b>Undefined</b></term>
        ///         <term><see cref="Undefined"/></term>
        ///         <description>
        ///         This represents JavaScript's
        ///         <see href="http://msdn.microsoft.com/en-us/library/ie/dae3sbk5(v=vs.94).aspx">undefined</see>,
        ///         VBScript's
        ///         <see href="http://msdn.microsoft.com/en-us/library/f8tbc79x(v=vs.85).aspx">Empty</see>,
        ///         etc.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term><b>Void</b></term>
        ///         <term><see cref="VoidResult"/></term>
        ///         <description>
        ///         This is returned when script code forwards the result of a host method that returns no value.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term><b>Host&#xA0;Object</b></term>
        ///         <term>Native&#xA0;.NET&#xA0;type</term>
        ///         <description>
        ///         This includes all .NET types not mentioned above, including value types (enums,
        ///         structs, etc.), and instances of all other classes. Script code can only create
        ///         these objects by invoking a host method or constructor. They are returned to
        ///         the host in their native .NET form.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term><b>Script&#xA0;Object</b></term>
        ///         <term><see href="http://msdn.microsoft.com/en-us/library/system.dynamic.dynamicobject.aspx">System.Dynamic.DynamicObject</see></term>
        ///         <description>
        ///         This includes all native script objects that have no .NET representation. C#'s
        ///         <see href="http://msdn.microsoft.com/en-us/library/dd264741.aspx">dynamic</see>
        ///         keyword provides a convenient way to access them.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term>Other</term>
        ///         <term>Unspecified</term>
        ///         <description>
        ///         This includes host types and other ClearScript-specific objects intended for
        ///         script code use only. It may also include language-specific values that the
        ///         ClearScript library does not support. 
        ///         </description>
        ///     </item>
        /// </list>
        /// </para>
        /// </remarks>
        public object Evaluate(string documentName, bool discard, string code)
        {
            return Evaluate(documentName, discard, code, true);
        }

        internal object Evaluate(string documentName, bool discard, string code, bool marshalResult)
        {
            var result = Execute(documentName, code, true, discard);
            if (marshalResult)
            {
                result = MarshalToHost(result, false);
            }

            return result;
        }

        object IScriptEngine.Evaluate(ICompiledScript compiledScript)
        {
            var v8CompiledScript = compiledScript as V8Script;
            if (v8CompiledScript == null)
                throw new InvalidOperationException("Compiled script must be an instance of a V8Script.");

            return Evaluate(v8CompiledScript);
        }

        // ReSharper disable ParameterHidesMember

        /// <summary>
        /// Evaluates a compiled script.
        /// </summary>
        /// <param name="script">The compiled script to evaluate.</param>
        /// <returns>The result value.</returns>
        /// <remarks>
        /// For information about the types of result values that script code can return, see
        /// <see cref="ScriptEngine.Evaluate(string, bool, string)"/>.
        /// </remarks>
        public object Evaluate(V8Script script)
        {
            return Execute(script, true);
        }

        /// <summary>
        /// Executes script code.
        /// </summary>
        /// <param name="code">The script code to execute.</param>
        /// <remarks>
        /// In some script languages the distinction between statements and expressions is
        /// significant but ambiguous for certain syntactic elements. This method always
        /// interprets the specified script code as a statement.
        /// <para>
        /// If a debugger is attached, it will present the specified script code to the user as a
        /// document with an automatically selected name. This document will not be discarded
        /// after execution.
        /// </para>
        /// </remarks>
        public void Execute(string code)
        {
            Execute(null, code);
        }

        /// <summary>
        /// Executes script code with an associated document name.
        /// </summary>
        /// <param name="documentName">A document name for the script code. Currently this name is used only as a label in presentation contexts such as debugger user interfaces.</param>
        /// <param name="code">The script code to execute.</param>
        /// <remarks>
        /// In some script languages the distinction between statements and expressions is
        /// significant but ambiguous for certain syntactic elements. This method always
        /// interprets the specified script code as a statement.
        /// <para>
        /// If a debugger is attached, it will present the specified script code to the user as a
        /// document with the specified name. This document will not be discarded after execution.
        /// </para>
        /// </remarks>
        public void Execute(string documentName, string code)
        {
            Execute(documentName, false, code);
        }

        /// <summary>
        /// Executes script code with an associated document name, optionally discarding the document after execution.
        /// </summary>
        /// <param name="documentName">A document name for the script code. Currently this name is used only as a label in presentation contexts such as debugger user interfaces.</param>
        /// <param name="discard"><c>True</c> to discard the script document after execution, <c>false</c> otherwise.</param>
        /// <param name="code">The script code to execute.</param>
        /// <remarks>
        /// In some script languages the distinction between statements and expressions is
        /// significant but ambiguous for certain syntactic elements. This method always
        /// interprets the specified script code as a statement.
        /// <para>
        /// If a debugger is attached, it will present the specified script code to the user as a
        /// document with the specified name. Discarding this document removes it from view but
        /// has no effect on the script engine.
        /// </para>
        /// </remarks>
        public void Execute(string documentName, bool discard, string code)
        {
            Execute(documentName, code, false, discard);
        }

        void IScriptEngine.Execute(ICompiledScript compiledScript)
        {
            var v8CompiledScript = compiledScript as V8Script;
            if (v8CompiledScript == null)
                throw new InvalidOperationException("Compiled script must be an instance of a V8Script.");

            Execute(v8CompiledScript);
        }

        /// <summary>
        /// Executes a compiled script.
        /// </summary>
        /// <param name="script">The compiled script to execute.</param>
        /// <remarks>
        /// This method is similar to <see cref="Evaluate(V8Script)"/> with the exception that it
        /// does not marshal a result value to the host. It can provide a performance advantage
        /// when the result value is not needed.
        /// </remarks>
        public void Execute(V8Script script)
        {
            Execute(script, false);
        }

        // ReSharper restore ParameterHidesMember

        /// <summary>
        /// Returns memory usage information for the V8 runtime.
        /// </summary>
        /// <returns>A <see cref="V8RuntimeHeapInfo"/> object containing memory usage information for the V8 runtime.</returns>
        public IRuntimeHeapInfo GetRuntimeHeapInfo()
        {
            VerifyNotDisposed();
            return proxy.GetRuntimeHeapInfo();
        }

        #endregion

        #region internal members

        private object GetRootItem()
        {
            return MarshalToHost(ScriptInvoke(() => proxy.GetRootItem()), false);
        }

        private void BaseScriptInvoke(Action action)
        {
            var previousScriptFrame = CurrentScriptFrame;
            CurrentScriptFrame = new ScriptFrame();

            try
            {
                action();
            }
            finally
            {
                CurrentScriptFrame = previousScriptFrame;
            }
        }

        private T BaseScriptInvoke<T>(Func<T> func)
        {
            var previousScriptFrame = CurrentScriptFrame;
            CurrentScriptFrame = new ScriptFrame();

            try
            {
                return func();
            }
            finally
            {
                CurrentScriptFrame = previousScriptFrame;
            }
        }

        private void VerifyNotDisposed()
        {
            if (disposedFlag.IsSet())
            {
                throw new ObjectDisposedException(ToString());
            }
        }

        // ReSharper disable ParameterHidesMember

        private object Execute(V8Script script, bool evaluate)
        {
            MiscHelpers.VerifyNonNullArgument(script, "script");
            VerifyNotDisposed();

            return MarshalToHost(ScriptInvoke(() =>
            {
                if (inContinuationTimerScope || (ContinuationCallback == null))
                {
                    return proxy.Execute(script, evaluate);
                }

                var state = new Timer[] { null };
                using (state[0] = new Timer(unused => OnContinuationTimer(state[0]), null, Timeout.Infinite, Timeout.Infinite))
                {
                    inContinuationTimerScope = true;
                    try
                    {
                        state[0].Change(continuationInterval, Timeout.Infinite);
                        return proxy.Execute(script, evaluate);
                    }
                    finally
                    {
                        inContinuationTimerScope = false;
                    }
                }
            }), false);
        }



        // ReSharper restore ParameterHidesMember

        private void OnContinuationTimer(Timer timer)
        {
            try
            {
                var callback = ContinuationCallback;
                if ((callback != null) && !callback())
                {
                    Interrupt();
                }
                else
                {
                    timer.Change(continuationInterval, Timeout.Infinite);
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }

        #endregion

        #region ScriptEngine

        /// <summary>
        /// Gets or sets a callback that can be used to halt script execution.
        /// </summary>
        /// <remarks>
        /// During script execution the script engine periodically invokes this callback to
        /// determine whether it should continue. If the callback returns <c>false</c>, the script
        /// engine terminates script execution and throws an exception.
        /// </remarks>
        public ContinuationCallback ContinuationCallback
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the script engine's recommended file name extension for script files.
        /// </summary>
        /// <remarks>
        /// <see cref="V8ScriptEngine"/> instances return "js" for this property.
        /// </remarks>
        public string FileNameExtension
        {
            get { return "js"; }
        }

        /// <summary>
        /// Allows the host to access script resources directly.
        /// </summary>
        /// <remarks>
        /// The value of this property is an object that is bound to the script engine's root
        /// namespace. It dynamically supports properties and methods that correspond to global
        /// script objects and functions.
        /// </remarks>
        public dynamic Script
        {
            get
            {
                VerifyNotDisposed();
                return script;
            }
        }

        /// <summary>
        /// Exposes a host object to script code.
        /// </summary>
        /// <param name="itemName">A name for the new global script item that will represent the object.</param>
        /// <param name="target">The object to expose.</param>
        /// <remarks>
        /// For information about the mapping between host members and script-callable properties
        /// and methods, see <see cref="AddHostObject(string, HostItemFlags, object)"/>.
        /// </remarks>
        public void AddHostObject(string itemName, object target)
        {
            AddHostObject(itemName, HostItemFlags.None, target);
        }

        /// <summary>
        /// Exposes a host object to script code with the specified options.
        /// </summary>
        /// <param name="itemName">A name for the new global script item that will represent the object.</param>
        /// <param name="flags">A value that selects options for the operation.</param>
        /// <param name="target">The object to expose.</param>
        /// <remarks>
        /// Once a host object is exposed to script code, its members are accessible via the script
        /// language's native syntax for member access. The following table provides details about
        /// the mapping between host members and script-accessible properties and methods.
        /// <para>
        /// <list type="table">
        ///     <listheader>
        ///         <term>Member&#xA0;Type</term>
        ///         <term>Exposed&#xA0;As</term>
        ///         <description>Remarks</description>
        ///     </listheader>
        ///     <item>
        ///         <term><b>Constructor</b></term>
        ///         <term>N/A</term>
        ///         <description>
        ///         To invoke a constructor from script code, call
        ///         <see cref="HostFunctions.newObj{T}">HostFunctions.newObj(T)</see>.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term><b>Property/Field</b></term>
        ///         <term><b>Property</b></term>
        ///         <description>N/A</description>
        ///     </item>
        ///     <item>
        ///         <term><b>Method</b></term>
        ///         <term><b>Method</b></term>
        ///         <description>
        ///         Overloaded host methods are merged into a single script-callable method. At
        ///         runtime the correct host method is selected based on the argument types.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term><b>Generic&#xA0;Method</b></term>
        ///         <term><b>Method</b></term>
        ///         <description>
        ///         The ClearScript library supports dynamic C#-like type inference when invoking
        ///         generic methods. However, some methods require explicit type arguments. To call
        ///         such a method from script code, you must place the required number of
        ///         <see cref="AddHostType(string, HostItemFlags, Type)">host type objects</see>
        ///         at the beginning of the argument list. Doing so for methods that do not require
        ///         explicit type arguments is optional.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term><b>Extension&#xA0;Method</b></term>
        ///         <term><b>Method</b></term>
        ///         <description>
        ///         Extension methods are available if the type that implements them has been
        ///         exposed in the current script engine.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term><b>Indexer</b></term>
        ///         <term><b>Property</b></term>
        ///         <description>
        ///         Indexers appear as properties named "Item" that accept one or more index values
        ///         as arguments. In addition, objects that implement <see cref="IList"/> expose
        ///         properties with numeric names that match their valid indices. This includes
        ///         one-dimensional host arrays and other collections. Multidimensional host arrays
        ///         do not expose functional indexers; you must use
        ///         <see href="http://msdn.microsoft.com/en-us/library/system.array.getvalue.aspx">Array.GetValue</see>
        ///         and
        ///         <see href="http://msdn.microsoft.com/en-us/library/system.array.setvalue.aspx">Array.SetValue</see>
        ///         instead.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <term><b>Event</b></term>
        ///         <term><b>Property</b></term>
        ///         <description>
        ///         Events are exposed as read-only properties of type <see cref="EventSource{T}"/>.
        ///         </description>
        ///     </item>
        /// </list>
        /// </para>
        /// </remarks>
        public void AddHostObject(string itemName, HostItemFlags flags, object target)
        {
            MiscHelpers.VerifyNonNullArgument(target, "target");
            AddHostItem(itemName, flags, target);
        }

        /// <summary>
        /// Exposes a host object to script code with the specified type restriction.
        /// </summary>
        /// <typeparam name="T">The type whose members are to be made accessible from script code.</typeparam>
        /// <param name="itemName">A name for the new global script item that will represent the object.</param>
        /// <param name="target">The object to expose.</param>
        /// <remarks>
        /// This method can be used to restrict script access to the members of a particular
        /// interface or base class.
        /// <para>
        /// For information about the mapping between host members and script-callable properties
        /// and methods, see <see cref="AddHostObject(string, HostItemFlags, object)"/>.
        /// </para>
        /// </remarks>
        public void AddRestrictedHostObject<T>(string itemName, T target)
        {
            AddRestrictedHostObject(itemName, HostItemFlags.None, target);
        }

        /// <summary>
        /// Exposes a host object to script code with the specified type restriction and options.
        /// </summary>
        /// <typeparam name="T">The type whose members are to be made accessible from script code.</typeparam>
        /// <param name="itemName">A name for the new global script item that will represent the object.</param>
        /// <param name="flags">A value that selects options for the operation.</param>
        /// <param name="target">The object to expose.</param>
        /// <remarks>
        /// This method can be used to restrict script access to the members of a particular
        /// interface or base class.
        /// <para>
        /// For information about the mapping between host members and script-callable properties
        /// and methods, see <see cref="AddHostObject(string, HostItemFlags, object)"/>.
        /// </para>
        /// </remarks>
        public void AddRestrictedHostObject<T>(string itemName, HostItemFlags flags, T target)
        {
            AddHostItem(itemName, flags, HostItem.Wrap(this, target, typeof(T)));
        }

        /// <summary>
        /// Exposes a host type to script code with a default name.
        /// </summary>
        /// <param name="type">The type to expose.</param>
        /// <remarks>
        /// This method uses <paramref name="type"/>'s name for the new global script item that
        /// will represent it.
        /// <para>
        /// Host types are exposed to script code in the form of objects whose properties and
        /// methods are bound to the type's static members and nested types. If the type has
        /// generic parameters, the corresponding object will be invocable with type arguments to
        /// yield a specific type.
        /// </para>
        /// <para>
        /// For more information about the mapping between host members and script-callable
        /// properties and methods, see <see cref="AddHostObject(string, HostItemFlags, object)"/>.
        /// </para>
        /// </remarks>
        public void AddHostType(Type type)
        {
            AddHostType(HostItemFlags.None, type);
        }

        /// <summary>
        /// Exposes a host type to script code with a default name and the specified options.
        /// </summary>
        /// <param name="flags">A value that selects options for the operation.</param>
        /// <param name="type">The type to expose.</param>
        /// <remarks>
        /// This method uses <paramref name="type"/>'s name for the new global script item that
        /// will represent it.
        /// <para>
        /// Host types are exposed to script code in the form of objects whose properties and
        /// methods are bound to the type's static members and nested types. If the type has
        /// generic parameters, the corresponding object will be invocable with type arguments to
        /// yield a specific type.
        /// </para>
        /// <para>
        /// For more information about the mapping between host members and script-callable
        /// properties and methods, see <see cref="AddHostObject(string, HostItemFlags, object)"/>.
        /// </para>
        /// </remarks>
        public void AddHostType(HostItemFlags flags, Type type)
        {
            AddHostType(type.GetRootName(), flags, type);
        }

        /// <summary>
        /// Exposes a host type to script code.
        /// </summary>
        /// <param name="itemName">A name for the new global script item that will represent the type.</param>
        /// <param name="type">The type to expose.</param>
        /// <remarks>
        /// Host types are exposed to script code in the form of objects whose properties and
        /// methods are bound to the type's static members and nested types. If the type has
        /// generic parameters, the corresponding object will be invocable with type arguments to
        /// yield a specific type.
        /// <para>
        /// For more information about the mapping between host members and script-callable
        /// properties and methods, see <see cref="AddHostObject(string, HostItemFlags, object)"/>.
        /// </para>
        /// </remarks>
        public void AddHostType(string itemName, Type type)
        {
            AddHostType(itemName, HostItemFlags.None, type);
        }

        /// <summary>
        /// Exposes a host type to script code with the specified options.
        /// </summary>
        /// <param name="itemName">A name for the new global script item that will represent the type.</param>
        /// <param name="flags">A value that selects options for the operation.</param>
        /// <param name="type">The type to expose.</param>
        /// <remarks>
        /// Host types are exposed to script code in the form of objects whose properties and
        /// methods are bound to the type's static members and nested types. If the type has
        /// generic parameters, the corresponding object will be invocable with type arguments to
        /// yield a specific type.
        /// <para>
        /// For more information about the mapping between host members and script-callable
        /// properties and methods, see <see cref="AddHostObject(string, HostItemFlags, object)"/>.
        /// </para>
        /// </remarks>
        public void AddHostType(string itemName, HostItemFlags flags, Type type)
        {
            MiscHelpers.VerifyNonNullArgument(type, "type");
            AddHostItem(itemName, flags, HostType.Wrap(type));
        }

        /// <summary>
        /// Exposes a host type to script code. The type is specified by name.
        /// </summary>
        /// <param name="itemName">A name for the new global script item that will represent the type.</param>
        /// <param name="typeName">The fully qualified name of the type to expose.</param>
        /// <param name="typeArgs">Optional generic type arguments.</param>
        /// <remarks>
        /// Host types are exposed to script code in the form of objects whose properties and
        /// methods are bound to the type's static members and nested types. If the type has
        /// generic parameters, the corresponding object will be invocable with type arguments to
        /// yield a specific type.
        /// <para>
        /// For more information about the mapping between host members and script-callable
        /// properties and methods, see <see cref="AddHostObject(string, HostItemFlags, object)"/>.
        /// </para>
        /// </remarks>
        public void AddHostType(string itemName, string typeName, params Type[] typeArgs)
        {
            AddHostType(itemName, HostItemFlags.None, typeName, typeArgs);
        }

        /// <summary>
        /// Exposes a host type to script code with the specified options. The type is specified by name.
        /// </summary>
        /// <param name="itemName">A name for the new global script item that will represent the type.</param>
        /// <param name="flags">A value that selects options for the operation.</param>
        /// <param name="typeName">The fully qualified name of the type to expose.</param>
        /// <param name="typeArgs">Optional generic type arguments.</param>
        /// <remarks>
        /// Host types are exposed to script code in the form of objects whose properties and
        /// methods are bound to the type's static members and nested types. If the type has
        /// generic parameters, the corresponding object will be invocable with type arguments to
        /// yield a specific type.
        /// <para>
        /// For more information about the mapping between host members and script-callable
        /// properties and methods, see <see cref="AddHostObject(string, HostItemFlags, object)"/>.
        /// </para>
        /// </remarks>
        public void AddHostType(string itemName, HostItemFlags flags, string typeName, params Type[] typeArgs)
        {
            AddHostItem(itemName, flags, TypeExtensions.ImportType(typeName, null, false, typeArgs));
        }

        /// <summary>
        /// Exposes a host type to script code. The type is specified by type name and assembly name.
        /// </summary>
        /// <param name="itemName">A name for the new global script item that will represent the type.</param>
        /// <param name="typeName">The fully qualified name of the type to expose.</param>
        /// <param name="assemblyName">The name of the assembly that contains the type to expose.</param>
        /// <param name="typeArgs">Optional generic type arguments.</param>
        /// <remarks>
        /// Host types are exposed to script code in the form of objects whose properties and
        /// methods are bound to the type's static members and nested types. If the type has
        /// generic parameters, the corresponding object will be invocable with type arguments to
        /// yield a specific type.
        /// <para>
        /// For more information about the mapping between host members and script-callable
        /// properties and methods, see <see cref="AddHostObject(string, HostItemFlags, object)"/>.
        /// </para>
        /// </remarks>
        public void AddHostType(string itemName, string typeName, string assemblyName, params Type[] typeArgs)
        {
            AddHostType(itemName, HostItemFlags.None, typeName, assemblyName, typeArgs);
        }

        /// <summary>
        /// Exposes a host type to script code with the specified options. The type is specified by
        /// type name and assembly name.
        /// </summary>
        /// <param name="itemName">A name for the new global script item that will represent the type.</param>
        /// <param name="flags">A value that selects options for the operation.</param>
        /// <param name="typeName">The fully qualified name of the type to expose.</param>
        /// <param name="assemblyName">The name of the assembly that contains the type to expose.</param>
        /// <param name="typeArgs">Optional generic type arguments.</param>
        /// <remarks>
        /// Host types are exposed to script code in the form of objects whose properties and
        /// methods are bound to the type's static members and nested types. If the type has
        /// generic parameters, the corresponding object will be invocable with type arguments to
        /// yield a specific type.
        /// <para>
        /// For more information about the mapping between host members and script-callable
        /// properties and methods, see <see cref="AddHostObject(string, HostItemFlags, object)"/>.
        /// </para>
        /// </remarks>
        public void AddHostType(string itemName, HostItemFlags flags, string typeName, string assemblyName, params Type[] typeArgs)
        {
            AddHostItem(itemName, flags, TypeExtensions.ImportType(typeName, assemblyName, true, typeArgs));
        }

        /// <summary>
        /// Gets a string representation of the script call stack.
        /// </summary>
        /// <returns>The script call stack formatted as a string.</returns>
        /// <remarks>
        /// This method returns an empty string if the script engine is not executing script code.
        /// The stack trace text format is defined by the script engine.
        /// </remarks>
        public string GetStackTrace()
        {
            string stackTrace = Script.EngineInternal.getStackTrace();
            var lines = stackTrace.Split('\n');
            return string.Join("\n", lines.Skip(2));
        }

        /// <summary>
        /// Interrupts script execution and causes the script engine to throw an exception.
        /// </summary>
        /// <remarks>
        /// This method can be called safely from any thread.
        /// </remarks>
        public void Interrupt()
        {
            VerifyNotDisposed();
            proxy.Interrupt();
        }

        /// <summary>
        /// Invokes a global function or procedure.
        /// </summary>
        /// <param name="funcName">The name of the global function or procedure to invoke.</param>
        /// <param name="args">Optional invocation arguments.</param>
        /// <returns>The return value if a function was invoked, an undefined value otherwise.</returns>
        public object Invoke(string funcName, params object[] args)
        {
            MiscHelpers.VerifyNonBlankArgument(funcName, "funcName", "Invalid function name");
            return ((IDynamic)Script).InvokeMethod(funcName, args ?? MiscHelpers.GetEmptyArray<object>());
        }

        /// <summary>
        /// Performs garbage collection.
        /// </summary>
        /// <param name="exhaustive"><c>True</c> to perform exhaustive garbage collection, <c>false</c> to favor speed over completeness.</param>
        public void CollectGarbage(bool exhaustive)
        {
            VerifyNotDisposed();
            proxy.CollectGarbage(exhaustive);
        }
        #endregion

        #region ScriptEngine overrides (internal members)

        internal void AddHostItem(string itemName, HostItemFlags flags, object item)
        {
            VerifyNotDisposed();

            var globalMembers = flags.HasFlag(HostItemFlags.GlobalMembers);
            if (globalMembers && engineFlags.HasFlag(V8ScriptEngineFlags.DisableGlobalMembers))
            {
                throw new InvalidOperationException("GlobalMembers support is disabled in this script engine");
            }

            MiscHelpers.VerifyNonNullArgument(itemName, "itemName");
            Debug.Assert(item != null);

            ScriptInvoke(() =>
            {
                var marshaledItem = MarshalToScript(item, flags);
                if (!(marshaledItem is HostItem))
                {
                    throw new InvalidOperationException("Invalid host item");
                }

                proxy.AddGlobalItem(itemName, marshaledItem, globalMembers);
            });
        }

        internal object PrepareResult<T>(T result, ScriptMemberFlags flags)
        {
            return PrepareResult(result, typeof(T), flags);
        }

        internal object PrepareResult(object result, Type type, ScriptMemberFlags flags)
        {
            var wrapNull = flags.HasFlag(ScriptMemberFlags.WrapNullResult);
            if (wrapNull && (result == null))
            {
                return HostObject.WrapResult(null, type, true);
            }

            //Always wrap the result.
            return HostObject.WrapResult(result, type, wrapNull);
        }

        internal object MarshalToScript(object obj)
        {
            var hostItem = obj as HostItem;
            return MarshalToScript(obj, (hostItem != null) ? hostItem.Flags : HostItemFlags.None);
        }

        internal object[] MarshalToScript(object[] args)
        {
            return args.Select(MarshalToScript).ToArray();
        }

        internal object MarshalToScript(object obj, HostItemFlags flags)
        {
            if (obj == null)
            {
                return DBNull.Value;
            }

            if (obj is Undefined)
            {
                return null;
            }

            if (obj is Nonexistent)
            {
                return obj;
            }

            var hostItem = obj as HostItem;
            if (hostItem != null)
            {
                if ((hostItem.Engine == this) && (hostItem.Flags == flags))
                {
                    return obj;
                }

                obj = hostItem.Target;
            }

            var hostTarget = obj as HostTarget;
            if (hostTarget != null)
            {
                obj = hostTarget.Target;
            }

            var scriptItem = obj as ScriptItem;
            if (scriptItem != null)
            {
                if (scriptItem.Engine == this)
                {
                    return scriptItem.Unwrap();
                }
            }

            return HostItem.Wrap(this, hostTarget ?? obj, flags);
        }

        internal object[] MarshalToHost(object[] args, bool preserveHostTargets)
        {
            return args.Select(arg => MarshalToHost(arg, preserveHostTargets)).ToArray();
        }

        internal object MarshalToHost(object obj, bool preserveHostTarget)
        {
            if (obj == null)
            {
                return Undefined.Value;
            }

            if (obj is DBNull)
            {
                return null;
            }

            object result;
            if (MiscHelpers.TryMarshalPrimitiveToHost(obj, out result))
            {
                return result;
            }

            var hostTarget = obj as HostTarget;
            if (hostTarget != null)
            {
                return preserveHostTarget ? hostTarget : hostTarget.Target;
            }

            var hostItem = obj as HostItem;
            if (hostItem != null)
            {
                return preserveHostTarget ? hostItem.Target : hostItem.Unwrap();
            }

            if (obj is ScriptItem)
            {
                return obj;
            }

            return V8ScriptItem.Wrap(this, obj);
        }

        internal object Execute(string documentName, string code, bool evaluate, bool discard)
        {
            VerifyNotDisposed();

            return ScriptInvoke(() =>
            {
                var uniqueName = documentNameManager.GetUniqueName(documentName, "Script Document");
                if (discard)
                {
                    uniqueName += " [temp]";
                }
                else if (documentNames != null)
                {
                    documentNames.Add(uniqueName);
                }

                if (inContinuationTimerScope || (ContinuationCallback == null))
                {
                    return proxy.Execute(uniqueName, FormatCode ? MiscHelpers.FormatCode(code) : code, evaluate, discard);
                }

                var state = new Timer[] { null };
                using (state[0] = new Timer(unused => OnContinuationTimer(state[0]), null, Timeout.Infinite, Timeout.Infinite))
                {
                    inContinuationTimerScope = true;
                    try
                    {
                        state[0].Change(continuationInterval, Timeout.Infinite);
                        return proxy.Execute(uniqueName, FormatCode ? MiscHelpers.FormatCode(code) : code, evaluate, discard);
                    }
                    finally
                    {
                        inContinuationTimerScope = false;
                    }
                }
            });
        }

        internal HostItemCollateral HostItemCollateral
        {
            get { return hostItemCollateral; }
        }

        internal void OnAccessSettingsChanged()
        {
            ScriptInvoke(() => proxy.OnAccessSettingsChanged());
        }

        internal void ScriptInvoke(Action action)
        {
            VerifyNotDisposed();
            proxy.InvokeWithLock(() => BaseScriptInvoke(action));
        }

        internal T ScriptInvoke<T>(Func<T> func)
        {
            VerifyNotDisposed();
            var result = default(T);
            proxy.InvokeWithLock(() => result = BaseScriptInvoke(func));
            return result;
        }

        #endregion

        #region IDisposable
        /// <summary>
        /// Releases all resources used by the script engine.
        /// </summary>
        /// <remarks>
        /// Call <c>Dispose()</c> when you are finished using the script engine. <c>Dispose()</c>
        /// leaves the script engine in an unusable state. After calling <c>Dispose()</c>, you must
        /// release all references to the script engine so the garbage collector can reclaim the
        /// memory that the script engine was occupying.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the script engine and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><c>True</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        /// <remarks>
        /// This method is called by the public <see cref="ScriptEngine.Dispose()"/> method and the
        /// <see cref="ScriptEngine.Finalize">Finalize</see> method.
        /// <see cref="ScriptEngine.Dispose()"/> invokes the protected <c>Dispose(Boolean)</c>
        /// method with the <paramref name="disposing"/> parameter set to <c>true</c>.
        /// <see cref="ScriptEngine.Finalize">Finalize</see> invokes <c>Dispose(Boolean)</c> with
        /// <paramref name="disposing"/> set to <c>false</c>.
        /// </remarks>
        internal void Dispose(bool disposing)
        {
            if (disposedFlag.Set())
            {
                if (disposing)
                {
                    ((IDisposable)script).Dispose();
                    proxy.Dispose();
                }
            }
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the script engine is reclaimed by garbage collection.
        /// </summary>
        /// <remarks>
        /// This method overrides <see cref="System.Object.Finalize"/>. Application code should not
        /// call this method; an object's <c>Finalize()</c> method is automatically invoked during
        /// garbage collection, unless finalization by the garbage collector has been disabled by a
        /// call to <see cref="System.GC.SuppressFinalize"/>.
        /// </remarks>
        ~V8ScriptEngine()
        {
            Dispose(false);
        }
        #endregion

        #region synchronized invocation

        internal void SyncInvoke(Action action)
        {
            action();
        }

        internal T SyncInvoke<T>(Func<T> func)
        {
            return func();
        }

        #endregion

        #region extension method table

        private readonly ExtensionMethodTable extensionMethodTable = new ExtensionMethodTable();

        internal void ProcessExtensionMethodType(Type type)
        {
            if (extensionMethodTable.ProcessType(type, DefaultAccess))
            {
                bindCache.Clear();
            }
        }

        internal ExtensionMethodSummary ExtensionMethodSummary
        {
            get { return extensionMethodTable.Summary; }
        }

        #endregion

        #region bind cache

        private readonly Dictionary<BindSignature, object> bindCache = new Dictionary<BindSignature, object>();

        internal void CacheBindResult(BindSignature signature, object result)
        {
            bindCache.Add(signature, result);
        }

        internal bool TryGetCachedBindResult(BindSignature signature, out object result)
        {
            return bindCache.TryGetValue(signature, out result);
        }

        #endregion

        #region host-side invocation
        [ThreadStatic]
        private static V8ScriptEngine currentEngine;

        internal void HostInvoke(Action action)
        {
            var previousEngine = currentEngine;
            currentEngine = this;

            try
            {
                action();
            }
            finally
            {
                currentEngine = previousEngine;
            }
        }

        internal T HostInvoke<T>(Func<T> func)
        {
            var previousEngine = currentEngine;
            currentEngine = this;

            try
            {
                return func();
            }
            finally
            {
                currentEngine = previousEngine;
            }
        }

        #endregion

        #region host item cache
        private static readonly object nullHostObjectProxy = new object();
        private readonly ConditionalWeakTable<object, List<WeakReference>> hostObjectHostItemCache = new ConditionalWeakTable<object, List<WeakReference>>();
        private readonly ConditionalWeakTable<Type, List<WeakReference>> hostTypeHostItemCache = new ConditionalWeakTable<Type, List<WeakReference>>();

        internal HostItem GetOrCreateHostItem(HostTarget target, HostItemFlags flags, HostItem.CreateFunc createHostItem)
        {
            var hostObject = target as HostObject;
            if (hostObject != null)
            {
                return GetOrCreateHostItemForHostObject(hostObject, hostObject.Target, flags, createHostItem);
            }

            var hostType = target as HostType;
            if (hostType != null)
            {
                return GetOrCreateHostItemForHostType(hostType, flags, createHostItem);
            }

            var hostMethod = target as HostMethod;
            if (hostMethod != null)
            {
                return GetOrCreateHostItemForHostObject(hostMethod, hostMethod, flags, createHostItem);
            }

            var hostVariable = target as HostVariableBase;
            if (hostVariable != null)
            {
                return GetOrCreateHostItemForHostObject(hostVariable, hostVariable, flags, createHostItem);
            }

            var hostIndexedProperty = target as HostIndexedProperty;
            if (hostIndexedProperty != null)
            {
                return GetOrCreateHostItemForHostObject(hostIndexedProperty, hostIndexedProperty, flags, createHostItem);
            }

            return createHostItem(this, target, flags);
        }

        private HostItem GetOrCreateHostItemForHostObject(HostTarget hostTarget, object target, HostItemFlags flags, HostItem.CreateFunc createHostItem)
        {
            var cacheEntry = hostObjectHostItemCache.GetOrCreateValue(target ?? nullHostObjectProxy);

            List<WeakReference> activeWeakRefs = null;
            var staleWeakRefCount = 0;

            foreach (var weakRef in cacheEntry)
            {
                var hostItem = weakRef.Target as HostItem;
                if (hostItem == null)
                {
                    staleWeakRefCount++;
                }
                else
                {
                    if ((hostItem.Target.Type == hostTarget.Type) && (hostItem.Flags == flags))
                    {
                        return hostItem;
                    }

                    if (activeWeakRefs == null)
                    {
                        activeWeakRefs = new List<WeakReference>(cacheEntry.Count);
                    }

                    activeWeakRefs.Add(weakRef);
                }
            }

            if (staleWeakRefCount > 4)
            {
                cacheEntry.Clear();
                if (activeWeakRefs != null)
                {
                    cacheEntry.Capacity = activeWeakRefs.Count + 1;
                    cacheEntry.AddRange(activeWeakRefs);
                }
            }

            var newHostItem = createHostItem(this, hostTarget, flags);
            cacheEntry.Add(new WeakReference(newHostItem));
            return newHostItem;
        }

        private HostItem GetOrCreateHostItemForHostType(HostType hostType, HostItemFlags flags, HostItem.CreateFunc createHostItem)
        {
            if (hostType.Types.Length != 1)
            {
                return createHostItem(this, hostType, flags);
            }

            var cacheEntry = hostTypeHostItemCache.GetOrCreateValue(hostType.Types[0]);

            List<WeakReference> activeWeakRefs = null;
            var staleWeakRefCount = 0;

            foreach (var weakRef in cacheEntry)
            {
                var hostItem = weakRef.Target as HostItem;
                if (hostItem == null)
                {
                    staleWeakRefCount++;
                }
                else
                {
                    if (hostItem.Flags == flags)
                    {
                        return hostItem;
                    }

                    if (activeWeakRefs == null)
                    {
                        activeWeakRefs = new List<WeakReference>(cacheEntry.Count);
                    }

                    activeWeakRefs.Add(weakRef);
                }
            }

            if (staleWeakRefCount > 4)
            {
                cacheEntry.Clear();
                if (activeWeakRefs != null)
                {
                    cacheEntry.Capacity = activeWeakRefs.Count + 1;
                    cacheEntry.AddRange(activeWeakRefs);
                }
            }

            var newHostItem = createHostItem(this, hostType, flags);
            cacheEntry.Add(new WeakReference(newHostItem));
            return newHostItem;
        }

        #endregion

        #region shared host target member data

        internal readonly HostTargetMemberData SharedHostMethodMemberData = new HostTargetMemberData();
        internal readonly HostTargetMemberData SharedHostIndexedPropertyMemberData = new HostTargetMemberData();

        private readonly ConditionalWeakTable<Type, List<WeakReference>> sharedHostObjectMemberDataCache = new ConditionalWeakTable<Type, List<WeakReference>>();

        internal HostTargetMemberData GetSharedHostObjectMemberData(HostObject target, Type targetAccessContext, ScriptAccess targetDefaultAccess)
        {
            var cacheEntry = sharedHostObjectMemberDataCache.GetOrCreateValue(target.Type);

            List<WeakReference> activeWeakRefs = null;
            var staleWeakRefCount = 0;

            foreach (var weakRef in cacheEntry)
            {
                var memberData = weakRef.Target as SharedHostObjectMemberData;
                if (memberData == null)
                {
                    staleWeakRefCount++;
                }
                else
                {
                    if ((memberData.AccessContext == targetAccessContext) && (memberData.DefaultAccess == targetDefaultAccess))
                    {
                        return memberData;
                    }

                    if (activeWeakRefs == null)
                    {
                        activeWeakRefs = new List<WeakReference>(cacheEntry.Count);
                    }

                    activeWeakRefs.Add(weakRef);
                }
            }

            if (staleWeakRefCount > 4)
            {
                cacheEntry.Clear();
                if (activeWeakRefs != null)
                {
                    cacheEntry.Capacity = activeWeakRefs.Count + 1;
                    cacheEntry.AddRange(activeWeakRefs);
                }
            }

            var newMemberData = new SharedHostObjectMemberData(targetAccessContext, targetDefaultAccess);
            cacheEntry.Add(new WeakReference(newMemberData));
            return newMemberData;
        }

        #endregion

        #region Nested type: ScriptFrame

        internal class ScriptFrame
        {
            public Exception HostException { get; set; }

            public IScriptEngineException ScriptError { get; set; }

            public IScriptEngineException PendingScriptError { get; set; }

            public bool InterruptRequested { get; set; }
        }

        #endregion

        #region unit test support

        internal void EnableDocumentNameTracking()
        {
            documentNames = new List<string>();
        }

        internal IEnumerable<string> GetDocumentNames()
        {
            return documentNames;
        }

        #endregion
    }
}
