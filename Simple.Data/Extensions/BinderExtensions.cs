namespace Simple.Data.Extensions
{
    using System;
    using System.Dynamic;
    using System.Reflection;
    using Microsoft.CSharp.RuntimeBinder;


    internal static class BinderExtensions
    {
        private static readonly Type TypeOfICSharpInvokeMemberBinder;
        private static readonly FieldInfo FlagsField;
        private static readonly Func<InvokeMemberBinder, bool> ResultDiscardedGetter;
        private static bool _isMono = Type.GetType("Mono.Runtime") != null;

        static BinderExtensions()
        {
            // Microsoft are hiding the good stuff again. Not having that.
            try
            {
                // Get a different type if mono
                var typeToGet = _isMono
                    ? "Microsoft.CSharp.RuntimeBinder.CSharpInvokeMemberBinder"
                    : "Microsoft.CSharp.RuntimeBinder.ICSharpInvokeOrInvokeMemberBinder";

                TypeOfICSharpInvokeMemberBinder = Assembly.GetAssembly(typeof(CSharpArgumentInfo)).GetType(typeToGet);
                if (TypeOfICSharpInvokeMemberBinder != null)
                {
                    BindingFlags bindFlags = BindingFlags.Instance 
                        | BindingFlags.Public 
                        | BindingFlags.NonPublic
                        | BindingFlags.Static;
                    FlagsField = TypeOfICSharpInvokeMemberBinder.GetField("flags", bindFlags);
                    if (FlagsField != null)
                    {
                        ResultDiscardedGetter = GetResultDiscardedImpl;
                    }
                }
            }
            catch
            {
                ResultDiscardedGetter = null;
            }

            ResultDiscardedGetter = ResultDiscardedGetter ?? (_ => false);
        }

        public static bool HasSingleUnnamedArgument(this InvokeMemberBinder binder)
        {
            return binder.CallInfo.ArgumentCount == 1 &&
                   (binder.CallInfo.ArgumentNames.Count == 0 ||
                    string.IsNullOrWhiteSpace(binder.CallInfo.ArgumentNames[0]));
        }

        public static bool IsResultDiscarded(this InvokeMemberBinder binder)
        {
            return ResultDiscardedGetter(binder);
        }

        private static bool GetResultDiscardedImpl(InvokeMemberBinder binder)
        {
            if (!TypeOfICSharpInvokeMemberBinder.IsInstanceOfType(binder)) return false;

            try
            {
                return ((CSharpBinderFlags)FlagsField.GetValue(binder) & CSharpBinderFlags.ResultDiscarded) != 0;
            }
            catch (ArgumentException)
            {
                return true;
            }
            catch (TargetException)
            {
                return true;
            }
            catch (TargetParameterCountException)
            {
                return true;
            }
            catch (MethodAccessException)
            {
                return true;
            }
            catch (TargetInvocationException)
            {
                return true;
            }
        }
    }
}