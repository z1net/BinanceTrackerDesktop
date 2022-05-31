﻿using BinanceTrackerDesktop.Formatters.Currency;
using BinanceTrackerDesktop.Formatters.Currency.Crypto;
using BinanceTrackerDesktop.Formatters.ValueString;
using System.Reflection;

namespace BinanceTrackerDesktop.Formatters.Utilities;

public sealed class FormatterUtility<TFormatterType> where TFormatterType : new()
{
    private static IList<TemporaryFormatter> formatters = new List<TemporaryFormatter>
    {
        new TemporaryFormatter(Activator.CreateInstance<ValueStringFormatter>(), typeof(ValueStringFormatter)),
        new TemporaryFormatter(Activator.CreateInstance<BasedOnUserDataCurrencyFormatter>(), typeof(BasedOnUserDataCurrencyFormatter)),
        new TemporaryFormatter(Activator.CreateInstance<BasedOnUserDataCryptocurrencyFormatter>(), typeof(BasedOnUserDataCryptocurrencyFormatter)),
    };



    public static object Format(object argument)
    {
        if (argument == null)
        {
            throw new ArgumentNullException(nameof(argument));
        }

        TemporaryFormatter formatter = null;
        if ((formatter = formatters.FirstOrDefault(f => f.Type.Equals(typeof(TFormatterType)))) == null)
        {
            formatters.Add(formatter = new TemporaryFormatter(Activator.CreateInstance<TFormatterType>(), typeof(TFormatterType)));
        }

        return formatter.Format(argument);
    }



    private sealed class TemporaryFormatter : IFormatter<object>
    {
        public readonly object Target;

        public readonly Type Type;

        public readonly MethodInfo TypeMethodInfo;



        public TemporaryFormatter(object target, Type type)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            Target = target;
            Type = type;
            TypeMethodInfo = type.GetMethod(nameof(Format));
        }



        public object Format(object argument)
        {
            return TypeMethodInfo.Invoke(Target, new object[]
            {
                argument
            });
        }
    }
}