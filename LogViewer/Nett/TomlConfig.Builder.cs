﻿namespace Nett
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Extensions;
    using Nett.Mapping;
    using Util;
    using static System.Diagnostics.Debug;

    public sealed partial class TomlSettings
    {
        public class CreateInstanceContext
        {
            public string Key { get; }

            public IEnumerable<string> KeyChain { get; }

            public CreateInstanceContext(string key)
            {
                this.Key = key;
                this.KeyChain = Enumerable.Empty<string>();
            }

            public CreateInstanceContext(IEnumerable<string> keyChain)
            {
                this.KeyChain = keyChain.ToList();
                this.Key = this.KeyChain.Any() ? this.KeyChain.Last() : null;
            }
        }

        public interface IMappingBuilder<TCustom>
        {
            ITypeSettingsBuilder<TCustom> ToKey(string key);
        }

        public interface ITypeSettingsBuilder<TCustom>
        {
            ITypeSettingsBuilder<TCustom> CreateInstance(Func<TCustom> func);

            ITypeSettingsBuilder<TCustom> CreateInstance(Func<CreateInstanceContext, TCustom> creator);

            ITypeSettingsBuilder<TCustom> IgnoreProperty<TProperty>(Expression<Func<TCustom, TProperty>> accessor);

            ITypeSettingsBuilder<TCustom> TreatAsInlineTable();

            ITypeSettingsBuilder<TCustom> WithConversionFor<TToml>(Action<IConversionSettingsBuilder<TCustom, TToml>> conv)
                where TToml : TomlObject;

            ITypeSettingsBuilder<TCustom> Include<TMember>(Expression<Func<TCustom, TMember>> selector);

            ITypeSettingsBuilder<TCustom> Include(string memberName, BindingFlags bindFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            IMappingBuilder<TCustom> Map<TMember>(Expression<Func<TCustom, TMember>> selector);

            IMappingBuilder<TCustom> Map(string memberName, BindingFlags bindFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public interface IPropertyMappingBuilder
        {
            /// <summary>
            /// Use a custom target property selector implementation for these settings.
            /// </summary>
            /// <param name="custom">Custom target selector implementation.</param>
            /// <exception cref="ArgumentNullException">If <paramref name="custom"/> is <b>null</b>.</exception>
            /// <returns>Fluent configuration builder continuation object.</returns>
            IPropertyMappingBuilder UseTargetPropertySelector(ITargetPropertySelector custom);

            /// <summary>
            /// Choose which standard selector implementation to use for these settings.
            /// </summary>
            /// <param name="standardSelectors">Lamba which's parameter allows to choose one of the standard selectors.</param>
            /// <exception cref="ArgumentNullException">If <paramref name="standardSelectors"/> is <b>null</b>.</exception>
            /// <returns>Fluent configuration builder continuation object.</returns>
            IPropertyMappingBuilder UseTargetPropertySelector(
                Func<TargetPropertySelectors, ITargetPropertySelector> standardSelectors);

            /// <summary>
            /// Use a custom key generator implementation for these settings.
            /// </summary>
            /// <param name="generator">Custom key generator implementation.</param>
            /// <exception cref="ArgumentNullException">If <paramref name="generator"/> is <b>null</b>.</exception>
            /// <returns>Fluent configuration builder continuation object.</returns>
            IPropertyMappingBuilder UseKeyGenerator(IKeyGenerator generator);

            /// <summary>
            /// Choose which standard key generator implementation to use for these settings.
            /// </summary>
            /// <param name="standardGenerators">Lambda which's parameter allows to choose one of the standard selectors.</param>
            /// <exception cref="ArgumentNullException">If <paramref name="standardGenerators"/> is <b>null</b>.</exception>
            /// <returns>Fluent configuration builder continuation object.</returns>
            IPropertyMappingBuilder UseKeyGenerator(Func<KeyGenerators, IKeyGenerator> standardGenerators);

            /// <summary>
            /// Set a custom callback that gets invoked when a target property is not found when a TOML table gets mapped to a
            /// .Net object.
            /// </summary>
            /// <param name="whenNotFound">The callback to invoke.
            /// Argument 1: The TOML key chain that could not be mapped from highest to deepest. Always contains at least one element.
            ///   The root itself is not part of the key chain.
            /// Argument 2: The target object on that no property matching they key was found.
            /// Argument 3: The value associated with that key in the TOML table.
            /// </param>
            /// <returns>Fluent configuration builder continuation object.</returns>
            IPropertyMappingBuilder OnTargetPropertyNotFound(Action<string[], object, TomlObject> whenNotFound);
        }

        public interface ITomlSettingsBuilder
        {
            ITomlSettingsBuilder AllowImplicitConversions(ConversionSets sets);

            ITomlSettingsBuilder Apply(Action<ITomlSettingsBuilder> batch);

            ITomlSettingsBuilder ConfigureType<T>(Action<ITypeSettingsBuilder<T>> ct);

            /// <summary>
            /// Configures the property mapping settings that define how TOML rows are mapped to corresponding CLR object
            /// properties and vice versa.
            /// </summary>
            /// <param name="configureMapping">The configuration lambda.</param>
            /// <returns>Fluent configuration builder continuation object.</returns>
            ITomlSettingsBuilder ConfigurePropertyMapping(Action<IPropertyMappingBuilder> configureMapping);
        }

        internal sealed class ConversionSettingsBuilder<TCustom, TToml> : IConversionSettingsBuilder<TCustom, TToml>
            where TToml : TomlObject
        {
            private readonly List<ITomlConverter> converters;

            public ConversionSettingsBuilder(List<ITomlConverter> converters)
            {
                Assert(converters != null);

                this.converters = converters;
            }

            public IConversionSettingsBuilder<TCustom, TToml> FromToml(Func<ITomlRoot, TToml, TCustom> convert)
            {
                this.AddConverter(new TomlConverter<TToml, TCustom>(convert));
                return this;
            }

            public IConversionSettingsBuilder<TCustom, TToml> FromToml(Func<TToml, TCustom> convert)
            {
                this.AddConverterInternal(new TomlConverter<TToml, TCustom>((_, tToml) => convert(tToml)));
                return this;
            }

            public IConversionSettingsBuilder<TCustom, TToml> ToToml(Func<ITomlRoot, TCustom, TToml> convert)
            {
                this.AddConverterInternal(new TomlConverter<TCustom, TToml>(convert));
                return this;
            }

            internal void AddConverter(ITomlConverter converter) => this.converters.Add(converter);

            private void AddConverterInternal(ITomlConverter converter)
            {
                this.converters.Insert(0, converter);
            }
        }

        internal sealed class PropertyMappingBuilder : IPropertyMappingBuilder
        {
            private readonly TomlSettings settings;

            public PropertyMappingBuilder(TomlSettings settings)
            {
                this.settings = settings;
            }

            public IPropertyMappingBuilder UseKeyGenerator(IKeyGenerator generator)
            {
                this.settings.keyGenerator = generator.CheckNotNull(nameof(generator));
                return this;
            }

            public IPropertyMappingBuilder UseKeyGenerator(Func<KeyGenerators, IKeyGenerator> selectStandardGenerator)
                 => this.UseKeyGenerator(selectStandardGenerator(KeyGenerators.Instance));

            public IPropertyMappingBuilder UseTargetPropertySelector(ITargetPropertySelector custom)
            {
                this.settings.mappingPropertySelector = custom.CheckNotNull(nameof(custom));
                return this;
            }

            public IPropertyMappingBuilder UseTargetPropertySelector(Func<TargetPropertySelectors, ITargetPropertySelector> selectStandardRule)
                => this.UseTargetPropertySelector(selectStandardRule(TargetPropertySelectors.Instance));

            public IPropertyMappingBuilder OnTargetPropertyNotFound(Action<string[], object, TomlObject> whenNotFound)
            {
                this.settings.whenTargetPropertyNotFoundCallback = whenNotFound.CheckNotNull(nameof(whenNotFound));
                return this;
            }
        }

        internal sealed class TomlSettingsBuilder : ITomlSettingsBuilder, IExpSettingsBuilder
        {
            private readonly TomlSettings settings = new TomlSettings();
            private readonly List<ITomlConverter> userConverters = new List<ITomlConverter>();

            private ConversionSets allowedConversions;

            public TomlSettingsBuilder(TomlSettings settings)
            {
                Assert(settings != null);

                this.settings = settings;
                this.AllowImplicitConversions(ConversionSets.Default);
            }

            public ITomlSettingsBuilder AllowImplicitConversions(ConversionSets sets)
            {
                this.allowedConversions = sets;
                return this;
            }

            public ITomlSettingsBuilder Apply(Action<ITomlSettingsBuilder> batch)
            {
                batch(this);
                return this;
            }

            public ITomlSettingsBuilder ConfigurePropertyMapping(Action<IPropertyMappingBuilder> configureMapping)
            {
                configureMapping(new PropertyMappingBuilder(this.settings));
                return this;
            }

            public ITomlSettingsBuilder ConfigureType<T>(Action<ITypeSettingsBuilder<T>> ct)
            {
                ct(new TypeSettingsBuilder<T>(this.settings, this.userConverters));
                return this;
            }

            public void SetupConverters()
            {
                this.SetupDefaultConverters();
                this.SetupUserConverters();
            }

            public void SetupDefaultConverters()
            {
                this.settings.converters.AddRange(EquivalentTypeConverters);

                if (this.allowedConversions.HasFlag(ConversionSets.NumericalSize))
                {
                    this.settings.converters.AddRange(NumericalSize);
                }

                if (this.allowedConversions.HasFlag(ConversionSets.Serialize))
                {
                    this.settings.converters.AddRange(SerializeConverters);
                }

                if (this.allowedConversions.HasFlag(ConversionSets.NumericalType))
                {
                    this.settings.converters.AddRange(NumercialType);
                }
            }

            void IExpSettingsBuilder.EnableExperimentalFeature(ExperimentalFeature feature, bool enable)
            {
                this.settings.featureFlags[feature] = enable;
            }

            private void SetupUserConverters()
            {
                this.settings.converters.AddRange(this.userConverters);
            }
        }

        internal sealed class TypeSettingsBuilder<TCustom> : ITypeSettingsBuilder<TCustom>
        {
            private readonly TomlSettings settings;
            private readonly List<ITomlConverter> converters;

            public TypeSettingsBuilder(TomlSettings settings, List<ITomlConverter> converters)
            {
                Assert(settings != null);
                Assert(converters != null);

                this.settings = settings;
                this.converters = converters;
            }

            public ITypeSettingsBuilder<TCustom> CreateInstance(Func<TCustom> activator)
                => this.CreateInstance(_ => activator());

            public ITypeSettingsBuilder<TCustom> CreateInstance(Func<CreateInstanceContext, TCustom> activator)
            {
                this.settings.activators.Add(typeof(TCustom), (ctx) => activator(ctx));
                return this;
            }

            public ITypeSettingsBuilder<TCustom> IgnoreProperty<TProperty>(Expression<Func<TCustom, TProperty>> accessor)
            {
                var properties = this.settings.ignoredMembers.AddIfNeeded(typeof(TCustom), def: new HashSet<SerializationMember>());
                var propertyInfo = ReflectionUtil.GetPropertyInfo(accessor);
                properties.Add(new SerializationMember(propertyInfo));
                return this;
            }

            public ITypeSettingsBuilder<TCustom> Include<TMember>(Expression<Func<TCustom, TMember>> selector)
            {
                var member = ReflectionUtil.GetSerMemberInfo(selector);
                var key = member.GetKey();
                this.settings.explicitMembers.Add(new SerializationInfo(member, key), key.Value);
                return this;
            }

            public ITypeSettingsBuilder<TCustom> Include(string memberName, BindingFlags bindFlags)
            {
                var sm = ReflectionUtil.GetSerMemberInfo(typeof(TCustom), memberName, bindFlags);
                var key = sm.GetKey();
                this.settings.explicitMembers.Add(new SerializationInfo(sm, key), key.Value);
                return this;
            }

            public IMappingBuilder<TCustom> Map<TMember>(Expression<Func<TCustom, TMember>> selector)
            {
                var sm = ReflectionUtil.GetSerMemberInfo(selector);
                return new MappingBuilder(this, sm);
            }

            public IMappingBuilder<TCustom> Map(string memberName, BindingFlags bindFlags)
            {
                var sm = ReflectionUtil.GetSerMemberInfo(typeof(TCustom), memberName, bindFlags);
                return new MappingBuilder(this, sm);
            }

            public ITypeSettingsBuilder<TCustom> TreatAsInlineTable()
            {
                this.settings.inlineTableTypes.Add(typeof(TCustom));
                return this;
            }

            public ITypeSettingsBuilder<TCustom> WithConversionFor<TToml>(Action<IConversionSettingsBuilder<TCustom, TToml>> conv)
                where TToml : TomlObject
            {
                conv(new ConversionSettingsBuilder<TCustom, TToml>(this.converters));
                return this;
            }

            private sealed class MappingBuilder : IMappingBuilder<TCustom>
            {
                private readonly SerializationMember serMember;
                private readonly TypeSettingsBuilder<TCustom> typeBuilder;

                public MappingBuilder(TypeSettingsBuilder<TCustom> typeBuilder, SerializationMember serMember)
                {
                    this.typeBuilder = typeBuilder;
                    this.serMember = serMember;
                }

                public ITypeSettingsBuilder<TCustom> ToKey(string key)
                {
                    this.typeBuilder.settings.explicitMembers.Add(new SerializationInfo(this.serMember, new TomlKey(key)), key);
                    return this.typeBuilder;
                }
            }
        }
    }
}
