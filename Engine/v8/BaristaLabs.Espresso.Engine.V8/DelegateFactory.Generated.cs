﻿// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// Microsoft Public License (MS-PL)
// 
// This license governs use of the accompanying software. If you use the
// software, you accept this license. If you do not accept the license, do not
// use the software.
// 
// 1. Definitions
// 
//   The terms "reproduce," "reproduction," "derivative works," and
//   "distribution" have the same meaning here as under U.S. copyright law. A
//   "contribution" is the original software, or any additions or changes to
//   the software. A "contributor" is any person that distributes its
//   contribution under this license. "Licensed patents" are a contributor's
//   patent claims that read directly on its contribution.
// 
// 2. Grant of Rights
// 
//   (A) Copyright Grant- Subject to the terms of this license, including the
//       license conditions and limitations in section 3, each contributor
//       grants you a non-exclusive, worldwide, royalty-free copyright license
//       to reproduce its contribution, prepare derivative works of its
//       contribution, and distribute its contribution or any derivative works
//       that you create.
// 
//   (B) Patent Grant- Subject to the terms of this license, including the
//       license conditions and limitations in section 3, each contributor
//       grants you a non-exclusive, worldwide, royalty-free license under its
//       licensed patents to make, have made, use, sell, offer for sale,
//       import, and/or otherwise dispose of its contribution in the software
//       or derivative works of the contribution in the software.
// 
// 3. Conditions and Limitations
// 
//   (A) No Trademark License- This license does not grant you rights to use
//       any contributors' name, logo, or trademarks.
// 
//   (B) If you bring a patent claim against any contributor over patents that
//       you claim are infringed by the software, your patent license from such
//       contributor to the software ends automatically.
// 
//   (C) If you distribute any portion of the software, you must retain all
//       copyright, patent, trademark, and attribution notices that are present
//       in the software.
// 
//   (D) If you distribute any portion of the software in source code form, you
//       may do so only under this license by including a complete copy of this
//       license with your distribution. If you distribute any portion of the
//       software in compiled or object code form, you may only do so under a
//       license that complies with this license.
// 
//   (E) The software is licensed "as-is." You bear the risk of using it. The
//       contributors give no express warranties, guarantees or conditions. You
//       may have additional consumer rights under your local laws which this
//       license cannot change. To the extent permitted under your local laws,
//       the contributors exclude the implied warranties of merchantability,
//       fitness for a particular purpose and non-infringement.
//       




using System;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace BaristaLabs.Espresso.Engine.V8
{
    internal static partial class DelegateFactory
    {
        private const int maxArgCount = 16;

        
        [ExcludeFromCodeCoverage]
        private class ProcShim<TDelegate> : ProcShim
        {
            private static readonly MethodInfo method = typeof(ProcShim<TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = true;
            private readonly object target;
            private readonly Delegate del;

            public ProcShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public void InvokeTarget()
            {
                if (allByValue)
                {
                    Invoke(() => ((dynamic)target)());
                }
                else
                {
                    
                    try
                    {
                        Invoke(() => ((dynamic)target)());
                    }
                    finally
                    {
                        
                    }
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class ProcShim<T1, TDelegate> : ProcShim
        {
            private static readonly MethodInfo method = typeof(ProcShim<T1, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1));
            private readonly object target;
            private readonly Delegate del;

            public ProcShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public void InvokeTarget(T1 a1)
            {
                if (allByValue)
                {
                    Invoke(() => ((dynamic)target)(a1));
                }
                else
                {
                    var v1 = GetArgValue(a1);
                    try
                    {
                        Invoke(() => ((dynamic)target)(ref v1));
                    }
                    finally
                    {
                        SetArgValue(a1, v1);
                    }
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class ProcShim<T1, T2, TDelegate> : ProcShim
        {
            private static readonly MethodInfo method = typeof(ProcShim<T1, T2, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2));
            private readonly object target;
            private readonly Delegate del;

            public ProcShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public void InvokeTarget(T1 a1, T2 a2)
            {
                if (allByValue)
                {
                    Invoke(() => ((dynamic)target)(a1, a2));
                }
                else
                {
                    var v1 = GetArgValue(a1);
                    var v2 = GetArgValue(a2);
                    try
                    {
                        Invoke(() => ((dynamic)target)(ref v1, ref v2));
                    }
                    finally
                    {
                        SetArgValue(a1, v1);
                        SetArgValue(a2, v2);
                    }
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class ProcShim<T1, T2, T3, TDelegate> : ProcShim
        {
            private static readonly MethodInfo method = typeof(ProcShim<T1, T2, T3, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3));
            private readonly object target;
            private readonly Delegate del;

            public ProcShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public void InvokeTarget(T1 a1, T2 a2, T3 a3)
            {
                if (allByValue)
                {
                    Invoke(() => ((dynamic)target)(a1, a2, a3));
                }
                else
                {
                    var v1 = GetArgValue(a1);
                    var v2 = GetArgValue(a2);
                    var v3 = GetArgValue(a3);
                    try
                    {
                        Invoke(() => ((dynamic)target)(ref v1, ref v2, ref v3));
                    }
                    finally
                    {
                        SetArgValue(a1, v1);
                        SetArgValue(a2, v2);
                        SetArgValue(a3, v3);
                    }
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class ProcShim<T1, T2, T3, T4, TDelegate> : ProcShim
        {
            private static readonly MethodInfo method = typeof(ProcShim<T1, T2, T3, T4, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
            private readonly object target;
            private readonly Delegate del;

            public ProcShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public void InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4)
            {
                if (allByValue)
                {
                    Invoke(() => ((dynamic)target)(a1, a2, a3, a4));
                }
                else
                {
                    var v1 = GetArgValue(a1);
                    var v2 = GetArgValue(a2);
                    var v3 = GetArgValue(a3);
                    var v4 = GetArgValue(a4);
                    try
                    {
                        Invoke(() => ((dynamic)target)(ref v1, ref v2, ref v3, ref v4));
                    }
                    finally
                    {
                        SetArgValue(a1, v1);
                        SetArgValue(a2, v2);
                        SetArgValue(a3, v3);
                        SetArgValue(a4, v4);
                    }
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class ProcShim<T1, T2, T3, T4, T5, TDelegate> : ProcShim
        {
            private static readonly MethodInfo method = typeof(ProcShim<T1, T2, T3, T4, T5, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
            private readonly object target;
            private readonly Delegate del;

            public ProcShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public void InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5)
            {
                if (allByValue)
                {
                    Invoke(() => ((dynamic)target)(a1, a2, a3, a4, a5));
                }
                else
                {
                    var v1 = GetArgValue(a1);
                    var v2 = GetArgValue(a2);
                    var v3 = GetArgValue(a3);
                    var v4 = GetArgValue(a4);
                    var v5 = GetArgValue(a5);
                    try
                    {
                        Invoke(() => ((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5));
                    }
                    finally
                    {
                        SetArgValue(a1, v1);
                        SetArgValue(a2, v2);
                        SetArgValue(a3, v3);
                        SetArgValue(a4, v4);
                        SetArgValue(a5, v5);
                    }
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class ProcShim<T1, T2, T3, T4, T5, T6, TDelegate> : ProcShim
        {
            private static readonly MethodInfo method = typeof(ProcShim<T1, T2, T3, T4, T5, T6, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
            private readonly object target;
            private readonly Delegate del;

            public ProcShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public void InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6)
            {
                if (allByValue)
                {
                    Invoke(() => ((dynamic)target)(a1, a2, a3, a4, a5, a6));
                }
                else
                {
                    var v1 = GetArgValue(a1);
                    var v2 = GetArgValue(a2);
                    var v3 = GetArgValue(a3);
                    var v4 = GetArgValue(a4);
                    var v5 = GetArgValue(a5);
                    var v6 = GetArgValue(a6);
                    try
                    {
                        Invoke(() => ((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6));
                    }
                    finally
                    {
                        SetArgValue(a1, v1);
                        SetArgValue(a2, v2);
                        SetArgValue(a3, v3);
                        SetArgValue(a4, v4);
                        SetArgValue(a5, v5);
                        SetArgValue(a6, v6);
                    }
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class ProcShim<T1, T2, T3, T4, T5, T6, T7, TDelegate> : ProcShim
        {
            private static readonly MethodInfo method = typeof(ProcShim<T1, T2, T3, T4, T5, T6, T7, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
            private readonly object target;
            private readonly Delegate del;

            public ProcShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public void InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7)
            {
                if (allByValue)
                {
                    Invoke(() => ((dynamic)target)(a1, a2, a3, a4, a5, a6, a7));
                }
                else
                {
                    var v1 = GetArgValue(a1);
                    var v2 = GetArgValue(a2);
                    var v3 = GetArgValue(a3);
                    var v4 = GetArgValue(a4);
                    var v5 = GetArgValue(a5);
                    var v6 = GetArgValue(a6);
                    var v7 = GetArgValue(a7);
                    try
                    {
                        Invoke(() => ((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7));
                    }
                    finally
                    {
                        SetArgValue(a1, v1);
                        SetArgValue(a2, v2);
                        SetArgValue(a3, v3);
                        SetArgValue(a4, v4);
                        SetArgValue(a5, v5);
                        SetArgValue(a6, v6);
                        SetArgValue(a7, v7);
                    }
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class ProcShim<T1, T2, T3, T4, T5, T6, T7, T8, TDelegate> : ProcShim
        {
            private static readonly MethodInfo method = typeof(ProcShim<T1, T2, T3, T4, T5, T6, T7, T8, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));
            private readonly object target;
            private readonly Delegate del;

            public ProcShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public void InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8)
            {
                if (allByValue)
                {
                    Invoke(() => ((dynamic)target)(a1, a2, a3, a4, a5, a6, a7, a8));
                }
                else
                {
                    var v1 = GetArgValue(a1);
                    var v2 = GetArgValue(a2);
                    var v3 = GetArgValue(a3);
                    var v4 = GetArgValue(a4);
                    var v5 = GetArgValue(a5);
                    var v6 = GetArgValue(a6);
                    var v7 = GetArgValue(a7);
                    var v8 = GetArgValue(a8);
                    try
                    {
                        Invoke(() => ((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, ref v8));
                    }
                    finally
                    {
                        SetArgValue(a1, v1);
                        SetArgValue(a2, v2);
                        SetArgValue(a3, v3);
                        SetArgValue(a4, v4);
                        SetArgValue(a5, v5);
                        SetArgValue(a6, v6);
                        SetArgValue(a7, v7);
                        SetArgValue(a8, v8);
                    }
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class ProcShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, TDelegate> : ProcShim
        {
            private static readonly MethodInfo method = typeof(ProcShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9));
            private readonly object target;
            private readonly Delegate del;

            public ProcShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public void InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9)
            {
                if (allByValue)
                {
                    Invoke(() => ((dynamic)target)(a1, a2, a3, a4, a5, a6, a7, a8, a9));
                }
                else
                {
                    var v1 = GetArgValue(a1);
                    var v2 = GetArgValue(a2);
                    var v3 = GetArgValue(a3);
                    var v4 = GetArgValue(a4);
                    var v5 = GetArgValue(a5);
                    var v6 = GetArgValue(a6);
                    var v7 = GetArgValue(a7);
                    var v8 = GetArgValue(a8);
                    var v9 = GetArgValue(a9);
                    try
                    {
                        Invoke(() => ((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, ref v8, ref v9));
                    }
                    finally
                    {
                        SetArgValue(a1, v1);
                        SetArgValue(a2, v2);
                        SetArgValue(a3, v3);
                        SetArgValue(a4, v4);
                        SetArgValue(a5, v5);
                        SetArgValue(a6, v6);
                        SetArgValue(a7, v7);
                        SetArgValue(a8, v8);
                        SetArgValue(a9, v9);
                    }
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class ProcShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TDelegate> : ProcShim
        {
            private static readonly MethodInfo method = typeof(ProcShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10));
            private readonly object target;
            private readonly Delegate del;

            public ProcShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public void InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10)
            {
                if (allByValue)
                {
                    Invoke(() => ((dynamic)target)(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10));
                }
                else
                {
                    var v1 = GetArgValue(a1);
                    var v2 = GetArgValue(a2);
                    var v3 = GetArgValue(a3);
                    var v4 = GetArgValue(a4);
                    var v5 = GetArgValue(a5);
                    var v6 = GetArgValue(a6);
                    var v7 = GetArgValue(a7);
                    var v8 = GetArgValue(a8);
                    var v9 = GetArgValue(a9);
                    var v10 = GetArgValue(a10);
                    try
                    {
                        Invoke(() => ((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, ref v8, ref v9, ref v10));
                    }
                    finally
                    {
                        SetArgValue(a1, v1);
                        SetArgValue(a2, v2);
                        SetArgValue(a3, v3);
                        SetArgValue(a4, v4);
                        SetArgValue(a5, v5);
                        SetArgValue(a6, v6);
                        SetArgValue(a7, v7);
                        SetArgValue(a8, v8);
                        SetArgValue(a9, v9);
                        SetArgValue(a10, v10);
                    }
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class ProcShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TDelegate> : ProcShim
        {
            private static readonly MethodInfo method = typeof(ProcShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11));
            private readonly object target;
            private readonly Delegate del;

            public ProcShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public void InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11)
            {
                if (allByValue)
                {
                    Invoke(() => ((dynamic)target)(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11));
                }
                else
                {
                    var v1 = GetArgValue(a1);
                    var v2 = GetArgValue(a2);
                    var v3 = GetArgValue(a3);
                    var v4 = GetArgValue(a4);
                    var v5 = GetArgValue(a5);
                    var v6 = GetArgValue(a6);
                    var v7 = GetArgValue(a7);
                    var v8 = GetArgValue(a8);
                    var v9 = GetArgValue(a9);
                    var v10 = GetArgValue(a10);
                    var v11 = GetArgValue(a11);
                    try
                    {
                        Invoke(() => ((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, ref v8, ref v9, ref v10, ref v11));
                    }
                    finally
                    {
                        SetArgValue(a1, v1);
                        SetArgValue(a2, v2);
                        SetArgValue(a3, v3);
                        SetArgValue(a4, v4);
                        SetArgValue(a5, v5);
                        SetArgValue(a6, v6);
                        SetArgValue(a7, v7);
                        SetArgValue(a8, v8);
                        SetArgValue(a9, v9);
                        SetArgValue(a10, v10);
                        SetArgValue(a11, v11);
                    }
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class ProcShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TDelegate> : ProcShim
        {
            private static readonly MethodInfo method = typeof(ProcShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12));
            private readonly object target;
            private readonly Delegate del;

            public ProcShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public void InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11, T12 a12)
            {
                if (allByValue)
                {
                    Invoke(() => ((dynamic)target)(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12));
                }
                else
                {
                    var v1 = GetArgValue(a1);
                    var v2 = GetArgValue(a2);
                    var v3 = GetArgValue(a3);
                    var v4 = GetArgValue(a4);
                    var v5 = GetArgValue(a5);
                    var v6 = GetArgValue(a6);
                    var v7 = GetArgValue(a7);
                    var v8 = GetArgValue(a8);
                    var v9 = GetArgValue(a9);
                    var v10 = GetArgValue(a10);
                    var v11 = GetArgValue(a11);
                    var v12 = GetArgValue(a12);
                    try
                    {
                        Invoke(() => ((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, ref v8, ref v9, ref v10, ref v11, ref v12));
                    }
                    finally
                    {
                        SetArgValue(a1, v1);
                        SetArgValue(a2, v2);
                        SetArgValue(a3, v3);
                        SetArgValue(a4, v4);
                        SetArgValue(a5, v5);
                        SetArgValue(a6, v6);
                        SetArgValue(a7, v7);
                        SetArgValue(a8, v8);
                        SetArgValue(a9, v9);
                        SetArgValue(a10, v10);
                        SetArgValue(a11, v11);
                        SetArgValue(a12, v12);
                    }
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class ProcShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TDelegate> : ProcShim
        {
            private static readonly MethodInfo method = typeof(ProcShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13));
            private readonly object target;
            private readonly Delegate del;

            public ProcShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public void InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11, T12 a12, T13 a13)
            {
                if (allByValue)
                {
                    Invoke(() => ((dynamic)target)(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13));
                }
                else
                {
                    var v1 = GetArgValue(a1);
                    var v2 = GetArgValue(a2);
                    var v3 = GetArgValue(a3);
                    var v4 = GetArgValue(a4);
                    var v5 = GetArgValue(a5);
                    var v6 = GetArgValue(a6);
                    var v7 = GetArgValue(a7);
                    var v8 = GetArgValue(a8);
                    var v9 = GetArgValue(a9);
                    var v10 = GetArgValue(a10);
                    var v11 = GetArgValue(a11);
                    var v12 = GetArgValue(a12);
                    var v13 = GetArgValue(a13);
                    try
                    {
                        Invoke(() => ((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, ref v8, ref v9, ref v10, ref v11, ref v12, ref v13));
                    }
                    finally
                    {
                        SetArgValue(a1, v1);
                        SetArgValue(a2, v2);
                        SetArgValue(a3, v3);
                        SetArgValue(a4, v4);
                        SetArgValue(a5, v5);
                        SetArgValue(a6, v6);
                        SetArgValue(a7, v7);
                        SetArgValue(a8, v8);
                        SetArgValue(a9, v9);
                        SetArgValue(a10, v10);
                        SetArgValue(a11, v11);
                        SetArgValue(a12, v12);
                        SetArgValue(a13, v13);
                    }
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class ProcShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TDelegate> : ProcShim
        {
            private static readonly MethodInfo method = typeof(ProcShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14));
            private readonly object target;
            private readonly Delegate del;

            public ProcShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public void InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11, T12 a12, T13 a13, T14 a14)
            {
                if (allByValue)
                {
                    Invoke(() => ((dynamic)target)(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14));
                }
                else
                {
                    var v1 = GetArgValue(a1);
                    var v2 = GetArgValue(a2);
                    var v3 = GetArgValue(a3);
                    var v4 = GetArgValue(a4);
                    var v5 = GetArgValue(a5);
                    var v6 = GetArgValue(a6);
                    var v7 = GetArgValue(a7);
                    var v8 = GetArgValue(a8);
                    var v9 = GetArgValue(a9);
                    var v10 = GetArgValue(a10);
                    var v11 = GetArgValue(a11);
                    var v12 = GetArgValue(a12);
                    var v13 = GetArgValue(a13);
                    var v14 = GetArgValue(a14);
                    try
                    {
                        Invoke(() => ((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, ref v8, ref v9, ref v10, ref v11, ref v12, ref v13, ref v14));
                    }
                    finally
                    {
                        SetArgValue(a1, v1);
                        SetArgValue(a2, v2);
                        SetArgValue(a3, v3);
                        SetArgValue(a4, v4);
                        SetArgValue(a5, v5);
                        SetArgValue(a6, v6);
                        SetArgValue(a7, v7);
                        SetArgValue(a8, v8);
                        SetArgValue(a9, v9);
                        SetArgValue(a10, v10);
                        SetArgValue(a11, v11);
                        SetArgValue(a12, v12);
                        SetArgValue(a13, v13);
                        SetArgValue(a14, v14);
                    }
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class ProcShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TDelegate> : ProcShim
        {
            private static readonly MethodInfo method = typeof(ProcShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15));
            private readonly object target;
            private readonly Delegate del;

            public ProcShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public void InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11, T12 a12, T13 a13, T14 a14, T15 a15)
            {
                if (allByValue)
                {
                    Invoke(() => ((dynamic)target)(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15));
                }
                else
                {
                    var v1 = GetArgValue(a1);
                    var v2 = GetArgValue(a2);
                    var v3 = GetArgValue(a3);
                    var v4 = GetArgValue(a4);
                    var v5 = GetArgValue(a5);
                    var v6 = GetArgValue(a6);
                    var v7 = GetArgValue(a7);
                    var v8 = GetArgValue(a8);
                    var v9 = GetArgValue(a9);
                    var v10 = GetArgValue(a10);
                    var v11 = GetArgValue(a11);
                    var v12 = GetArgValue(a12);
                    var v13 = GetArgValue(a13);
                    var v14 = GetArgValue(a14);
                    var v15 = GetArgValue(a15);
                    try
                    {
                        Invoke(() => ((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, ref v8, ref v9, ref v10, ref v11, ref v12, ref v13, ref v14, ref v15));
                    }
                    finally
                    {
                        SetArgValue(a1, v1);
                        SetArgValue(a2, v2);
                        SetArgValue(a3, v3);
                        SetArgValue(a4, v4);
                        SetArgValue(a5, v5);
                        SetArgValue(a6, v6);
                        SetArgValue(a7, v7);
                        SetArgValue(a8, v8);
                        SetArgValue(a9, v9);
                        SetArgValue(a10, v10);
                        SetArgValue(a11, v11);
                        SetArgValue(a12, v12);
                        SetArgValue(a13, v13);
                        SetArgValue(a14, v14);
                        SetArgValue(a15, v15);
                    }
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class ProcShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TDelegate> : ProcShim
        {
            private static readonly MethodInfo method = typeof(ProcShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16));
            private readonly object target;
            private readonly Delegate del;

            public ProcShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public void InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11, T12 a12, T13 a13, T14 a14, T15 a15, T16 a16)
            {
                if (allByValue)
                {
                    Invoke(() => ((dynamic)target)(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16));
                }
                else
                {
                    var v1 = GetArgValue(a1);
                    var v2 = GetArgValue(a2);
                    var v3 = GetArgValue(a3);
                    var v4 = GetArgValue(a4);
                    var v5 = GetArgValue(a5);
                    var v6 = GetArgValue(a6);
                    var v7 = GetArgValue(a7);
                    var v8 = GetArgValue(a8);
                    var v9 = GetArgValue(a9);
                    var v10 = GetArgValue(a10);
                    var v11 = GetArgValue(a11);
                    var v12 = GetArgValue(a12);
                    var v13 = GetArgValue(a13);
                    var v14 = GetArgValue(a14);
                    var v15 = GetArgValue(a15);
                    var v16 = GetArgValue(a16);
                    try
                    {
                        Invoke(() => ((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, ref v8, ref v9, ref v10, ref v11, ref v12, ref v13, ref v14, ref v15, ref v16));
                    }
                    finally
                    {
                        SetArgValue(a1, v1);
                        SetArgValue(a2, v2);
                        SetArgValue(a3, v3);
                        SetArgValue(a4, v4);
                        SetArgValue(a5, v5);
                        SetArgValue(a6, v6);
                        SetArgValue(a7, v7);
                        SetArgValue(a8, v8);
                        SetArgValue(a9, v9);
                        SetArgValue(a10, v10);
                        SetArgValue(a11, v11);
                        SetArgValue(a12, v12);
                        SetArgValue(a13, v13);
                        SetArgValue(a14, v14);
                        SetArgValue(a15, v15);
                        SetArgValue(a16, v16);
                    }
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        
        [ExcludeFromCodeCoverage]
        private class FuncShim<TResult, TDelegate> : FuncShim<TResult>
        {
            private static readonly MethodInfo method = typeof(FuncShim<TResult, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = true;
            private readonly object target;
            private readonly Delegate del;

            public FuncShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public TResult InvokeTarget()
            {
                if (allByValue)
                {
                    return Invoke(() => (TResult)((dynamic)target)());
                }

                
                try
                {
                    return Invoke(() => (TResult)((dynamic)target)());
                }
                finally
                {
                    
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class FuncShim<T1, TResult, TDelegate> : FuncShim<TResult>
        {
            private static readonly MethodInfo method = typeof(FuncShim<T1, TResult, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1));
            private readonly object target;
            private readonly Delegate del;

            public FuncShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public TResult InvokeTarget(T1 a1)
            {
                if (allByValue)
                {
                    return Invoke(() => (TResult)((dynamic)target)(a1));
                }

                var v1 = GetArgValue(a1);
                try
                {
                    return Invoke(() => (TResult)((dynamic)target)(ref v1));
                }
                finally
                {
                    SetArgValue(a1, v1);
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class FuncShim<T1, T2, TResult, TDelegate> : FuncShim<TResult>
        {
            private static readonly MethodInfo method = typeof(FuncShim<T1, T2, TResult, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2));
            private readonly object target;
            private readonly Delegate del;

            public FuncShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public TResult InvokeTarget(T1 a1, T2 a2)
            {
                if (allByValue)
                {
                    return Invoke(() => (TResult)((dynamic)target)(a1, a2));
                }

                var v1 = GetArgValue(a1);
                var v2 = GetArgValue(a2);
                try
                {
                    return Invoke(() => (TResult)((dynamic)target)(ref v1, ref v2));
                }
                finally
                {
                    SetArgValue(a1, v1);
                    SetArgValue(a2, v2);
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class FuncShim<T1, T2, T3, TResult, TDelegate> : FuncShim<TResult>
        {
            private static readonly MethodInfo method = typeof(FuncShim<T1, T2, T3, TResult, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3));
            private readonly object target;
            private readonly Delegate del;

            public FuncShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public TResult InvokeTarget(T1 a1, T2 a2, T3 a3)
            {
                if (allByValue)
                {
                    return Invoke(() => (TResult)((dynamic)target)(a1, a2, a3));
                }

                var v1 = GetArgValue(a1);
                var v2 = GetArgValue(a2);
                var v3 = GetArgValue(a3);
                try
                {
                    return Invoke(() => (TResult)((dynamic)target)(ref v1, ref v2, ref v3));
                }
                finally
                {
                    SetArgValue(a1, v1);
                    SetArgValue(a2, v2);
                    SetArgValue(a3, v3);
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class FuncShim<T1, T2, T3, T4, TResult, TDelegate> : FuncShim<TResult>
        {
            private static readonly MethodInfo method = typeof(FuncShim<T1, T2, T3, T4, TResult, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
            private readonly object target;
            private readonly Delegate del;

            public FuncShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public TResult InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4)
            {
                if (allByValue)
                {
                    return Invoke(() => (TResult)((dynamic)target)(a1, a2, a3, a4));
                }

                var v1 = GetArgValue(a1);
                var v2 = GetArgValue(a2);
                var v3 = GetArgValue(a3);
                var v4 = GetArgValue(a4);
                try
                {
                    return Invoke(() => (TResult)((dynamic)target)(ref v1, ref v2, ref v3, ref v4));
                }
                finally
                {
                    SetArgValue(a1, v1);
                    SetArgValue(a2, v2);
                    SetArgValue(a3, v3);
                    SetArgValue(a4, v4);
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class FuncShim<T1, T2, T3, T4, T5, TResult, TDelegate> : FuncShim<TResult>
        {
            private static readonly MethodInfo method = typeof(FuncShim<T1, T2, T3, T4, T5, TResult, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
            private readonly object target;
            private readonly Delegate del;

            public FuncShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public TResult InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5)
            {
                if (allByValue)
                {
                    return Invoke(() => (TResult)((dynamic)target)(a1, a2, a3, a4, a5));
                }

                var v1 = GetArgValue(a1);
                var v2 = GetArgValue(a2);
                var v3 = GetArgValue(a3);
                var v4 = GetArgValue(a4);
                var v5 = GetArgValue(a5);
                try
                {
                    return Invoke(() => (TResult)((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5));
                }
                finally
                {
                    SetArgValue(a1, v1);
                    SetArgValue(a2, v2);
                    SetArgValue(a3, v3);
                    SetArgValue(a4, v4);
                    SetArgValue(a5, v5);
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class FuncShim<T1, T2, T3, T4, T5, T6, TResult, TDelegate> : FuncShim<TResult>
        {
            private static readonly MethodInfo method = typeof(FuncShim<T1, T2, T3, T4, T5, T6, TResult, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
            private readonly object target;
            private readonly Delegate del;

            public FuncShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public TResult InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6)
            {
                if (allByValue)
                {
                    return Invoke(() => (TResult)((dynamic)target)(a1, a2, a3, a4, a5, a6));
                }

                var v1 = GetArgValue(a1);
                var v2 = GetArgValue(a2);
                var v3 = GetArgValue(a3);
                var v4 = GetArgValue(a4);
                var v5 = GetArgValue(a5);
                var v6 = GetArgValue(a6);
                try
                {
                    return Invoke(() => (TResult)((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6));
                }
                finally
                {
                    SetArgValue(a1, v1);
                    SetArgValue(a2, v2);
                    SetArgValue(a3, v3);
                    SetArgValue(a4, v4);
                    SetArgValue(a5, v5);
                    SetArgValue(a6, v6);
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class FuncShim<T1, T2, T3, T4, T5, T6, T7, TResult, TDelegate> : FuncShim<TResult>
        {
            private static readonly MethodInfo method = typeof(FuncShim<T1, T2, T3, T4, T5, T6, T7, TResult, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
            private readonly object target;
            private readonly Delegate del;

            public FuncShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public TResult InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7)
            {
                if (allByValue)
                {
                    return Invoke(() => (TResult)((dynamic)target)(a1, a2, a3, a4, a5, a6, a7));
                }

                var v1 = GetArgValue(a1);
                var v2 = GetArgValue(a2);
                var v3 = GetArgValue(a3);
                var v4 = GetArgValue(a4);
                var v5 = GetArgValue(a5);
                var v6 = GetArgValue(a6);
                var v7 = GetArgValue(a7);
                try
                {
                    return Invoke(() => (TResult)((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7));
                }
                finally
                {
                    SetArgValue(a1, v1);
                    SetArgValue(a2, v2);
                    SetArgValue(a3, v3);
                    SetArgValue(a4, v4);
                    SetArgValue(a5, v5);
                    SetArgValue(a6, v6);
                    SetArgValue(a7, v7);
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class FuncShim<T1, T2, T3, T4, T5, T6, T7, T8, TResult, TDelegate> : FuncShim<TResult>
        {
            private static readonly MethodInfo method = typeof(FuncShim<T1, T2, T3, T4, T5, T6, T7, T8, TResult, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));
            private readonly object target;
            private readonly Delegate del;

            public FuncShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public TResult InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8)
            {
                if (allByValue)
                {
                    return Invoke(() => (TResult)((dynamic)target)(a1, a2, a3, a4, a5, a6, a7, a8));
                }

                var v1 = GetArgValue(a1);
                var v2 = GetArgValue(a2);
                var v3 = GetArgValue(a3);
                var v4 = GetArgValue(a4);
                var v5 = GetArgValue(a5);
                var v6 = GetArgValue(a6);
                var v7 = GetArgValue(a7);
                var v8 = GetArgValue(a8);
                try
                {
                    return Invoke(() => (TResult)((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, ref v8));
                }
                finally
                {
                    SetArgValue(a1, v1);
                    SetArgValue(a2, v2);
                    SetArgValue(a3, v3);
                    SetArgValue(a4, v4);
                    SetArgValue(a5, v5);
                    SetArgValue(a6, v6);
                    SetArgValue(a7, v7);
                    SetArgValue(a8, v8);
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class FuncShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult, TDelegate> : FuncShim<TResult>
        {
            private static readonly MethodInfo method = typeof(FuncShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9));
            private readonly object target;
            private readonly Delegate del;

            public FuncShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public TResult InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9)
            {
                if (allByValue)
                {
                    return Invoke(() => (TResult)((dynamic)target)(a1, a2, a3, a4, a5, a6, a7, a8, a9));
                }

                var v1 = GetArgValue(a1);
                var v2 = GetArgValue(a2);
                var v3 = GetArgValue(a3);
                var v4 = GetArgValue(a4);
                var v5 = GetArgValue(a5);
                var v6 = GetArgValue(a6);
                var v7 = GetArgValue(a7);
                var v8 = GetArgValue(a8);
                var v9 = GetArgValue(a9);
                try
                {
                    return Invoke(() => (TResult)((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, ref v8, ref v9));
                }
                finally
                {
                    SetArgValue(a1, v1);
                    SetArgValue(a2, v2);
                    SetArgValue(a3, v3);
                    SetArgValue(a4, v4);
                    SetArgValue(a5, v5);
                    SetArgValue(a6, v6);
                    SetArgValue(a7, v7);
                    SetArgValue(a8, v8);
                    SetArgValue(a9, v9);
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class FuncShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult, TDelegate> : FuncShim<TResult>
        {
            private static readonly MethodInfo method = typeof(FuncShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10));
            private readonly object target;
            private readonly Delegate del;

            public FuncShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public TResult InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10)
            {
                if (allByValue)
                {
                    return Invoke(() => (TResult)((dynamic)target)(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10));
                }

                var v1 = GetArgValue(a1);
                var v2 = GetArgValue(a2);
                var v3 = GetArgValue(a3);
                var v4 = GetArgValue(a4);
                var v5 = GetArgValue(a5);
                var v6 = GetArgValue(a6);
                var v7 = GetArgValue(a7);
                var v8 = GetArgValue(a8);
                var v9 = GetArgValue(a9);
                var v10 = GetArgValue(a10);
                try
                {
                    return Invoke(() => (TResult)((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, ref v8, ref v9, ref v10));
                }
                finally
                {
                    SetArgValue(a1, v1);
                    SetArgValue(a2, v2);
                    SetArgValue(a3, v3);
                    SetArgValue(a4, v4);
                    SetArgValue(a5, v5);
                    SetArgValue(a6, v6);
                    SetArgValue(a7, v7);
                    SetArgValue(a8, v8);
                    SetArgValue(a9, v9);
                    SetArgValue(a10, v10);
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class FuncShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult, TDelegate> : FuncShim<TResult>
        {
            private static readonly MethodInfo method = typeof(FuncShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11));
            private readonly object target;
            private readonly Delegate del;

            public FuncShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public TResult InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11)
            {
                if (allByValue)
                {
                    return Invoke(() => (TResult)((dynamic)target)(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11));
                }

                var v1 = GetArgValue(a1);
                var v2 = GetArgValue(a2);
                var v3 = GetArgValue(a3);
                var v4 = GetArgValue(a4);
                var v5 = GetArgValue(a5);
                var v6 = GetArgValue(a6);
                var v7 = GetArgValue(a7);
                var v8 = GetArgValue(a8);
                var v9 = GetArgValue(a9);
                var v10 = GetArgValue(a10);
                var v11 = GetArgValue(a11);
                try
                {
                    return Invoke(() => (TResult)((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, ref v8, ref v9, ref v10, ref v11));
                }
                finally
                {
                    SetArgValue(a1, v1);
                    SetArgValue(a2, v2);
                    SetArgValue(a3, v3);
                    SetArgValue(a4, v4);
                    SetArgValue(a5, v5);
                    SetArgValue(a6, v6);
                    SetArgValue(a7, v7);
                    SetArgValue(a8, v8);
                    SetArgValue(a9, v9);
                    SetArgValue(a10, v10);
                    SetArgValue(a11, v11);
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class FuncShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult, TDelegate> : FuncShim<TResult>
        {
            private static readonly MethodInfo method = typeof(FuncShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12));
            private readonly object target;
            private readonly Delegate del;

            public FuncShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public TResult InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11, T12 a12)
            {
                if (allByValue)
                {
                    return Invoke(() => (TResult)((dynamic)target)(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12));
                }

                var v1 = GetArgValue(a1);
                var v2 = GetArgValue(a2);
                var v3 = GetArgValue(a3);
                var v4 = GetArgValue(a4);
                var v5 = GetArgValue(a5);
                var v6 = GetArgValue(a6);
                var v7 = GetArgValue(a7);
                var v8 = GetArgValue(a8);
                var v9 = GetArgValue(a9);
                var v10 = GetArgValue(a10);
                var v11 = GetArgValue(a11);
                var v12 = GetArgValue(a12);
                try
                {
                    return Invoke(() => (TResult)((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, ref v8, ref v9, ref v10, ref v11, ref v12));
                }
                finally
                {
                    SetArgValue(a1, v1);
                    SetArgValue(a2, v2);
                    SetArgValue(a3, v3);
                    SetArgValue(a4, v4);
                    SetArgValue(a5, v5);
                    SetArgValue(a6, v6);
                    SetArgValue(a7, v7);
                    SetArgValue(a8, v8);
                    SetArgValue(a9, v9);
                    SetArgValue(a10, v10);
                    SetArgValue(a11, v11);
                    SetArgValue(a12, v12);
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class FuncShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult, TDelegate> : FuncShim<TResult>
        {
            private static readonly MethodInfo method = typeof(FuncShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13));
            private readonly object target;
            private readonly Delegate del;

            public FuncShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public TResult InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11, T12 a12, T13 a13)
            {
                if (allByValue)
                {
                    return Invoke(() => (TResult)((dynamic)target)(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13));
                }

                var v1 = GetArgValue(a1);
                var v2 = GetArgValue(a2);
                var v3 = GetArgValue(a3);
                var v4 = GetArgValue(a4);
                var v5 = GetArgValue(a5);
                var v6 = GetArgValue(a6);
                var v7 = GetArgValue(a7);
                var v8 = GetArgValue(a8);
                var v9 = GetArgValue(a9);
                var v10 = GetArgValue(a10);
                var v11 = GetArgValue(a11);
                var v12 = GetArgValue(a12);
                var v13 = GetArgValue(a13);
                try
                {
                    return Invoke(() => (TResult)((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, ref v8, ref v9, ref v10, ref v11, ref v12, ref v13));
                }
                finally
                {
                    SetArgValue(a1, v1);
                    SetArgValue(a2, v2);
                    SetArgValue(a3, v3);
                    SetArgValue(a4, v4);
                    SetArgValue(a5, v5);
                    SetArgValue(a6, v6);
                    SetArgValue(a7, v7);
                    SetArgValue(a8, v8);
                    SetArgValue(a9, v9);
                    SetArgValue(a10, v10);
                    SetArgValue(a11, v11);
                    SetArgValue(a12, v12);
                    SetArgValue(a13, v13);
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class FuncShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult, TDelegate> : FuncShim<TResult>
        {
            private static readonly MethodInfo method = typeof(FuncShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14));
            private readonly object target;
            private readonly Delegate del;

            public FuncShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public TResult InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11, T12 a12, T13 a13, T14 a14)
            {
                if (allByValue)
                {
                    return Invoke(() => (TResult)((dynamic)target)(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14));
                }

                var v1 = GetArgValue(a1);
                var v2 = GetArgValue(a2);
                var v3 = GetArgValue(a3);
                var v4 = GetArgValue(a4);
                var v5 = GetArgValue(a5);
                var v6 = GetArgValue(a6);
                var v7 = GetArgValue(a7);
                var v8 = GetArgValue(a8);
                var v9 = GetArgValue(a9);
                var v10 = GetArgValue(a10);
                var v11 = GetArgValue(a11);
                var v12 = GetArgValue(a12);
                var v13 = GetArgValue(a13);
                var v14 = GetArgValue(a14);
                try
                {
                    return Invoke(() => (TResult)((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, ref v8, ref v9, ref v10, ref v11, ref v12, ref v13, ref v14));
                }
                finally
                {
                    SetArgValue(a1, v1);
                    SetArgValue(a2, v2);
                    SetArgValue(a3, v3);
                    SetArgValue(a4, v4);
                    SetArgValue(a5, v5);
                    SetArgValue(a6, v6);
                    SetArgValue(a7, v7);
                    SetArgValue(a8, v8);
                    SetArgValue(a9, v9);
                    SetArgValue(a10, v10);
                    SetArgValue(a11, v11);
                    SetArgValue(a12, v12);
                    SetArgValue(a13, v13);
                    SetArgValue(a14, v14);
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class FuncShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult, TDelegate> : FuncShim<TResult>
        {
            private static readonly MethodInfo method = typeof(FuncShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15));
            private readonly object target;
            private readonly Delegate del;

            public FuncShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public TResult InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11, T12 a12, T13 a13, T14 a14, T15 a15)
            {
                if (allByValue)
                {
                    return Invoke(() => (TResult)((dynamic)target)(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15));
                }

                var v1 = GetArgValue(a1);
                var v2 = GetArgValue(a2);
                var v3 = GetArgValue(a3);
                var v4 = GetArgValue(a4);
                var v5 = GetArgValue(a5);
                var v6 = GetArgValue(a6);
                var v7 = GetArgValue(a7);
                var v8 = GetArgValue(a8);
                var v9 = GetArgValue(a9);
                var v10 = GetArgValue(a10);
                var v11 = GetArgValue(a11);
                var v12 = GetArgValue(a12);
                var v13 = GetArgValue(a13);
                var v14 = GetArgValue(a14);
                var v15 = GetArgValue(a15);
                try
                {
                    return Invoke(() => (TResult)((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, ref v8, ref v9, ref v10, ref v11, ref v12, ref v13, ref v14, ref v15));
                }
                finally
                {
                    SetArgValue(a1, v1);
                    SetArgValue(a2, v2);
                    SetArgValue(a3, v3);
                    SetArgValue(a4, v4);
                    SetArgValue(a5, v5);
                    SetArgValue(a6, v6);
                    SetArgValue(a7, v7);
                    SetArgValue(a8, v8);
                    SetArgValue(a9, v9);
                    SetArgValue(a10, v10);
                    SetArgValue(a11, v11);
                    SetArgValue(a12, v12);
                    SetArgValue(a13, v13);
                    SetArgValue(a14, v14);
                    SetArgValue(a15, v15);
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        [ExcludeFromCodeCoverage]
        private class FuncShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult, TDelegate> : FuncShim<TResult>
        {
            private static readonly MethodInfo method = typeof(FuncShim<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult, TDelegate>).GetMethod("InvokeTarget");
            private static readonly bool allByValue = GetAllByValue(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16));
            private readonly object target;
            private readonly Delegate del;

            public FuncShim(V8ScriptEngine engine, object target)
                : base(engine)
            {
                this.target = GetCompatibleTarget(typeof(TDelegate), target);
                del = Delegate.CreateDelegate(typeof(TDelegate), this, method);
            }

            // ReSharper disable UnusedMember.Local

            public TResult InvokeTarget(T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11, T12 a12, T13 a13, T14 a14, T15 a15, T16 a16)
            {
                if (allByValue)
                {
                    return Invoke(() => (TResult)((dynamic)target)(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16));
                }

                var v1 = GetArgValue(a1);
                var v2 = GetArgValue(a2);
                var v3 = GetArgValue(a3);
                var v4 = GetArgValue(a4);
                var v5 = GetArgValue(a5);
                var v6 = GetArgValue(a6);
                var v7 = GetArgValue(a7);
                var v8 = GetArgValue(a8);
                var v9 = GetArgValue(a9);
                var v10 = GetArgValue(a10);
                var v11 = GetArgValue(a11);
                var v12 = GetArgValue(a12);
                var v13 = GetArgValue(a13);
                var v14 = GetArgValue(a14);
                var v15 = GetArgValue(a15);
                var v16 = GetArgValue(a16);
                try
                {
                    return Invoke(() => (TResult)((dynamic)target)(ref v1, ref v2, ref v3, ref v4, ref v5, ref v6, ref v7, ref v8, ref v9, ref v10, ref v11, ref v12, ref v13, ref v14, ref v15, ref v16));
                }
                finally
                {
                    SetArgValue(a1, v1);
                    SetArgValue(a2, v2);
                    SetArgValue(a3, v3);
                    SetArgValue(a4, v4);
                    SetArgValue(a5, v5);
                    SetArgValue(a6, v6);
                    SetArgValue(a7, v7);
                    SetArgValue(a8, v8);
                    SetArgValue(a9, v9);
                    SetArgValue(a10, v10);
                    SetArgValue(a11, v11);
                    SetArgValue(a12, v12);
                    SetArgValue(a13, v13);
                    SetArgValue(a14, v14);
                    SetArgValue(a15, v15);
                    SetArgValue(a16, v16);
                }
            }

            // ReSharper restore UnusedMember.Local

            #region DelegateShim overrides

            public override Delegate Delegate
            {
                get { return del; }
            }

            #endregion
        }
        
        private static readonly Type[] procTemplates =
        {
            
            typeof(Action),
            
            typeof(Action</*T1*/>),
            
            typeof(Action</*T1*/, /*T2*/>),
            
            typeof(Action</*T1*/, /*T2*/, /*T3*/>),
            
            typeof(Action</*T1*/, /*T2*/, /*T3*/, /*T4*/>),
            
            typeof(Action</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/>),
            
            typeof(Action</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/>),
            
            typeof(Action</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/>),
            
            typeof(Action</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/>),
            
            typeof(Action</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/>),
            
            typeof(Action</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/>),
            
            typeof(Action</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/>),
            
            typeof(Action</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*T12*/>),
            
            typeof(Action</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*T12*/, /*T13*/>),
            
            typeof(Action</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*T12*/, /*T13*/, /*T14*/>),
            
            typeof(Action</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*T12*/, /*T13*/, /*T14*/, /*T15*/>),
            
            typeof(Action</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*T12*/, /*T13*/, /*T14*/, /*T15*/, /*T16*/>),
            
        };

        private static readonly Type[] funcTemplates =
        {
            
            typeof(Func</*TResult*/>),
            
            typeof(Func</*T1*/, /*TResult*/>),
            
            typeof(Func</*T1*/, /*T2*/, /*TResult*/>),
            
            typeof(Func</*T1*/, /*T2*/, /*T3*/, /*TResult*/>),
            
            typeof(Func</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*TResult*/>),
            
            typeof(Func</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*TResult*/>),
            
            typeof(Func</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*TResult*/>),
            
            typeof(Func</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*TResult*/>),
            
            typeof(Func</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*TResult*/>),
            
            typeof(Func</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*TResult*/>),
            
            typeof(Func</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*TResult*/>),
            
            typeof(Func</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*TResult*/>),
            
            typeof(Func</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*T12*/, /*TResult*/>),
            
            typeof(Func</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*T12*/, /*T13*/, /*TResult*/>),
            
            typeof(Func</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*T12*/, /*T13*/, /*T14*/, /*TResult*/>),
            
            typeof(Func</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*T12*/, /*T13*/, /*T14*/, /*T15*/, /*TResult*/>),
            
            typeof(Func</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*T12*/, /*T13*/, /*T14*/, /*T15*/, /*T16*/, /*TResult*/>),
            
        };

        private static readonly Type[] procShimTemplates =
        {
            
            typeof(ProcShim</*TDelegate*/>),
            
            typeof(ProcShim</*T1*/, /*TDelegate*/>),
            
            typeof(ProcShim</*T1*/, /*T2*/, /*TDelegate*/>),
            
            typeof(ProcShim</*T1*/, /*T2*/, /*T3*/, /*TDelegate*/>),
            
            typeof(ProcShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*TDelegate*/>),
            
            typeof(ProcShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*TDelegate*/>),
            
            typeof(ProcShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*TDelegate*/>),
            
            typeof(ProcShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*TDelegate*/>),
            
            typeof(ProcShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*TDelegate*/>),
            
            typeof(ProcShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*TDelegate*/>),
            
            typeof(ProcShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*TDelegate*/>),
            
            typeof(ProcShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*TDelegate*/>),
            
            typeof(ProcShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*T12*/, /*TDelegate*/>),
            
            typeof(ProcShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*T12*/, /*T13*/, /*TDelegate*/>),
            
            typeof(ProcShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*T12*/, /*T13*/, /*T14*/, /*TDelegate*/>),
            
            typeof(ProcShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*T12*/, /*T13*/, /*T14*/, /*T15*/, /*TDelegate*/>),
            
            typeof(ProcShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*T12*/, /*T13*/, /*T14*/, /*T15*/, /*T16*/, /*TDelegate*/>),
            
        };

        private static readonly Type[] funcShimTemplates =
        {
            
            typeof(FuncShim</*TResult*/, /*TDelegate*/>),
            
            typeof(FuncShim</*T1*/, /*TResult*/, /*TDelegate*/>),
            
            typeof(FuncShim</*T1*/, /*T2*/, /*TResult*/, /*TDelegate*/>),
            
            typeof(FuncShim</*T1*/, /*T2*/, /*T3*/, /*TResult*/, /*TDelegate*/>),
            
            typeof(FuncShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*TResult*/, /*TDelegate*/>),
            
            typeof(FuncShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*TResult*/, /*TDelegate*/>),
            
            typeof(FuncShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*TResult*/, /*TDelegate*/>),
            
            typeof(FuncShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*TResult*/, /*TDelegate*/>),
            
            typeof(FuncShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*TResult*/, /*TDelegate*/>),
            
            typeof(FuncShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*TResult*/, /*TDelegate*/>),
            
            typeof(FuncShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*TResult*/, /*TDelegate*/>),
            
            typeof(FuncShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*TResult*/, /*TDelegate*/>),
            
            typeof(FuncShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*T12*/, /*TResult*/, /*TDelegate*/>),
            
            typeof(FuncShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*T12*/, /*T13*/, /*TResult*/, /*TDelegate*/>),
            
            typeof(FuncShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*T12*/, /*T13*/, /*T14*/, /*TResult*/, /*TDelegate*/>),
            
            typeof(FuncShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*T12*/, /*T13*/, /*T14*/, /*T15*/, /*TResult*/, /*TDelegate*/>),
            
            typeof(FuncShim</*T1*/, /*T2*/, /*T3*/, /*T4*/, /*T5*/, /*T6*/, /*T7*/, /*T8*/, /*T9*/, /*T10*/, /*T11*/, /*T12*/, /*T13*/, /*T14*/, /*T15*/, /*T16*/, /*TResult*/, /*TDelegate*/>),
            
        };
    }
}
