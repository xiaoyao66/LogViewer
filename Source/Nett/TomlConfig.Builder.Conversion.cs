﻿namespace Nett
{
    using System;
    using System.Collections.Generic;
    using Nett.Converters;

    public interface IConversionSettingsBuilder<TCustom, TToml>
        where TToml : TomlObject
    {
        IConversionSettingsBuilder<TCustom, TToml> FromToml(Func<ITomlRoot, TToml, TCustom> convert);

        IConversionSettingsBuilder<TCustom, TToml> FromToml(Func<TToml, TCustom> convert);
    }

    /// <summary>
    /// This class provides generic specializations for IConfigureConversionBuilder.
    /// </summary>
    /// <remarks>
    /// These specializations are used, so that the user supplying the conversion
    /// doesn't need to invoke the TOML object constructor directly which he cannot
    /// as it is internal.
    /// </remarks>
    public static class ConversionBuilderExtensions
    {
        public static IConversionSettingsBuilder<TCustom, TomlBool> ToToml<TCustom>(
            this IConversionSettingsBuilder<TCustom, TomlBool> cb, Func<TCustom, bool> conv)
        {
            ((TomlSettings.ConversionSettingsBuilder<TCustom, TomlBool>)cb).AddConverter(
                new TomlConverter<TCustom, TomlBool>((root, customValue) => new TomlBool(root, conv(customValue))));
            return cb;
        }

        public static IConversionSettingsBuilder<TCustom, TomlInt> ToToml<TCustom>(
            this IConversionSettingsBuilder<TCustom, TomlInt> cb, Func<TCustom, long> conv)
        {
            ((TomlSettings.ConversionSettingsBuilder<TCustom, TomlInt>)cb).AddConverter(
                new TomlConverter<TCustom, TomlInt>((root, customValue) => new TomlInt(root, conv(customValue))));
            return cb;
        }

        public static IConversionSettingsBuilder<TCustom, TomlFloat> ToToml<TCustom>(
            this IConversionSettingsBuilder<TCustom, TomlFloat> cb, Func<TCustom, double> conv)
        {
            ((TomlSettings.ConversionSettingsBuilder<TCustom, TomlFloat>)cb).AddConverter(
                new TomlConverter<TCustom, TomlFloat>((root, customValue) => new TomlFloat(root, conv(customValue))));
            return cb;
        }

        public static IConversionSettingsBuilder<TCustom, TomlString> ToToml<TCustom>(
            this IConversionSettingsBuilder<TCustom, TomlString> cb, Func<TCustom, string> conv)
        {
            ((TomlSettings.ConversionSettingsBuilder<TCustom, TomlString>)cb).AddConverter(
                new TomlConverter<TCustom, TomlString>((root, customValue) => new TomlString(root, conv(customValue))));
            return cb;
        }

        public static IConversionSettingsBuilder<TCustom, TomlOffsetDateTime> ToToml<TCustom>(
            this IConversionSettingsBuilder<TCustom, TomlOffsetDateTime> cb, Func<TCustom, DateTimeOffset> conv)
        {
            ((TomlSettings.ConversionSettingsBuilder<TCustom, TomlOffsetDateTime>)cb).AddConverter(
                new TomlConverter<TCustom, TomlOffsetDateTime>((root, customValue) => new TomlOffsetDateTime(root, conv(customValue))));
            return cb;
        }

        public static IConversionSettingsBuilder<TCustom, TomlDuration> ToToml<TCustom>(
            this IConversionSettingsBuilder<TCustom, TomlDuration> cb, Func<TCustom, TimeSpan> conv)
        {
            ((TomlSettings.ConversionSettingsBuilder<TCustom, TomlDuration>)cb).AddConverter(
                new TomlConverter<TCustom, TomlDuration>((root, customValue) => new TomlDuration(root, conv(customValue))));
            return cb;
        }

        public static IConversionSettingsBuilder<TCustom, TomlTable> ToToml<TCustom>(
            this IConversionSettingsBuilder<TCustom, TomlTable> cb, Action<TCustom, TomlTable> conv)
        {
            ((TomlSettings.ConversionSettingsBuilder<TCustom, TomlTable>)cb).AddConverter(
                new TomlConverter<TCustom, TomlTable>((root, customValue) =>
                {
                    var t = new TomlTable(root);
                    conv(customValue, t);
                    return t;
                }));

            return cb;
        }
    }

    public sealed partial class TomlSettings
    {
        private static readonly List<ITomlConverter> NumercialType = new List<ITomlConverter>()
        {
            // TOML -> CLR
            // TomlFloat -> *
            new TomlConverter<TomlFloat, long>((m, f) => Convert.ToInt64(f.Value)),
            new TomlConverter<TomlFloat, ulong>((m, f) => Convert.ToUInt64(f.Value)),
            new TomlConverter<TomlFloat, int>((m, f) => Convert.ToInt32(f.Value)),
            new TomlConverter<TomlFloat, uint>((m, f) => Convert.ToUInt32(f.Value)),
            new TomlConverter<TomlFloat, short>((m, f) => Convert.ToInt16(f.Value)),
            new TomlConverter<TomlFloat, ushort>((m, f) => Convert.ToUInt16(f.Value)),
            new TomlConverter<TomlFloat, char>((m, f) => (char)Convert.ToUInt16(f.Value)),
            new TomlConverter<TomlFloat, byte>((m, f) => Convert.ToByte(f.Value)),

            // TOML -> CLR
            // TomlInt -> *
            new TomlConverter<TomlInt, float>((m, i) => i.Value),
            new TomlConverter<TomlInt, double>((m, i) => i.Value),
        }
        .AddBidirectionalConverter<TomlInt, TomlFloat>((m, f) => new TomlInt(m, Convert.ToInt64(f.Value)), (m, i) => new TomlFloat(m, i.Value));

        private static readonly List<ITomlConverter> NumericalSize = new List<ITomlConverter>()
        {
            // TOML -> CLR
            // TomlFloat -> *
            new TomlConverter<TomlFloat, float>((m, f) => (float)f.Value),

            // TomlInt -> *
            new TomlConverter<TomlInt, ulong>((m, i) => Convert.ToUInt64(i.Value)),
            new TomlConverter<TomlInt, int>((m, i) => Convert.ToInt32(i.Value)),
            new TomlConverter<TomlInt, uint>((m, i) => Convert.ToUInt32(i.Value)),
            new TomlConverter<TomlInt, short>((m, i) => Convert.ToInt16(i.Value)),
            new TomlConverter<TomlInt, ushort>((m, i) => Convert.ToUInt16(i.Value)),
            new TomlConverter<TomlInt, char>((m, i) => Convert.ToChar(i.Value)),
            new TomlConverter<TomlInt, byte>((m, i) => Convert.ToByte(i.Value)),

            // CLR -> TOML
            // * -> TomlInt
            new TomlConverter<ulong, TomlInt>((m, v) => new TomlInt(m, Convert.ToInt64(v))),
            new TomlConverter<uint, TomlInt>((m, v) => new TomlInt(m, v)),
            new TomlConverter<int, TomlInt>((m, v) => new TomlInt(m, v)),
            new TomlConverter<short, TomlInt>((m, v) => new TomlInt(m, v)),
            new TomlConverter<ushort, TomlInt>((m, v) => new TomlInt(m, v)),
            new TomlConverter<char, TomlInt>((m, v) => new TomlInt(m, v)),
            new TomlConverter<byte, TomlInt>((m, v) => new TomlInt(m, v)),

            // * -> TomlFloat
            new TomlConverter<float, TomlFloat>((m, v) => new TomlFloat(m, v)),
        }
        .AddBidirectionalConverter<TomlOffsetDateTime, DateTime>(ToOffsetDateTime, (m, t) => t.Value.UtcDateTime);

        // Without these converters the library will not work correctly
        private static readonly List<ITomlConverter> EquivalentTypeConverters = new List<ITomlConverter>()
        {
            new TomlArrayToObjectConverter(),
            new TomlTableToDictionaryConverter(),
        }
        .AddBidirectionalConverter<TomlInt, long>((m, c) => new TomlInt(m, c), (m, t) => t.Value)
        .AddBidirectionalConverter<TomlFloat, double>((m, c) => new TomlFloat(m, c), (m, t) => t.Value)
        .AddBidirectionalConverter<TomlString, string>((m, c) => new TomlString(m, c), (m, t) => t.Value)
        .AddBidirectionalConverter<TomlOffsetDateTime, DateTimeOffset>((m, c) => new TomlOffsetDateTime(m, c), (m, t) => t.Value)
        .AddBidirectionalConverter<TomlLocalDateTime, DateTime>((m, c) => new TomlLocalDateTime(m, c), (m, t) => t.Value)
        .AddBidirectionalConverter<TomlLocalDateTime, DateTimeOffset>((m, c) => new TomlLocalDateTime(m, c.LocalDateTime), (m, t) => t.Value)
        .AddBidirectionalConverter<TomlLocalDate, DateTime>((m, c) => new TomlLocalDate(m, c), (m, t) => t.Value)
        .AddBidirectionalConverter<TomlLocalDate, DateTimeOffset>((m, c) => new TomlLocalDate(m, c.LocalDateTime), (m, t) => t.Value)
        .AddBidirectionalConverter<TomlDuration, TimeSpan>((m, c) => new TomlDuration(m, c), (m, t) => t.Value)
        .AddBidirectionalConverter<TomlLocalTime, TimeSpan>((m, c) => new TomlLocalTime(m, c), (m, t) => t.Value)
        .AddBidirectionalConverter<TomlBool, bool>((m, c) => new TomlBool(m, c), (m, t) => t.Value);

        private static readonly List<ITomlConverter> SerializeConverters = new List<ITomlConverter>()
        {
            new TomlToEnumConverter(),
            new EnumToTomlConverter(),
        }
        .AddBidirectionalConverter<TomlString, Guid>((m, c) => new TomlString(m, c.ToString("D")), (m, t) => Guid.Parse(t.Value));

        [Flags]
        public enum ConversionSets
        {
            None = 0,

            NumericalSize = 1 << 0,
            Serialize = 1 << 1,
            NumericalType = 1 << 2,

            All = NumericalSize | NumericalType | Serialize,
            Default = NumericalSize | Serialize,
        }

        private static TomlOffsetDateTime ToOffsetDateTime(ITomlRoot root, DateTime dt)
        {
            try
            {
                return new TomlOffsetDateTime(root, dt);
            }
            catch (ArgumentOutOfRangeException)
            {
                // Exc happens for DateTime.MinValue -> DateTimeOffset depending on timezone
                // (object not explicitly initialized)
                return new TomlOffsetDateTime(root, DateTimeOffset.MinValue);
            }
        }
    }

    internal static class RegisterConverterExtensions
    {
        public static List<ITomlConverter> AddBidirectionalConverter<TToml, TClr>(
            this List<ITomlConverter> converterlist,
            Func<ITomlRoot, TClr, TToml> toToml,
            Func<ITomlRoot, TToml, TClr> toClr)
            where TToml : TomlObject
        {
            converterlist.Add(new TomlConverter<TToml, TClr>(toClr));
            converterlist.Add(new TomlConverter<TClr, TToml>(toToml));
            return converterlist;
        }
    }
}
