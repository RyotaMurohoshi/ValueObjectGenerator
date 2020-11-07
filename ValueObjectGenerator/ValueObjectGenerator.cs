using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace ValueObjectGenerator
{
    [Generator]
    public class StringValueObjectGenerator : ISourceGenerator
    {
        private const string attributeText = @"
using System;
namespace ValueObjectGenerator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class StringValueObjectAttribute : Attribute
    {
        public string PropertyName { get; set; }
    }
}
";

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            Executor.Execute("string",
                "StringValueObjectAttribute",
                "ValueObjectGenerator.StringValueObjectAttribute",
                attributeText,
                context
            );
        }
    }

    [Generator]
    public class IntValueObjectGenerator : ISourceGenerator
    {
        private const string attributeText = @"
using System;
namespace ValueObjectGenerator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class IntValueObjectAttribute : Attribute
    {
        public string PropertyName { get; set; }
    }
}
";

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            Executor.Execute(
                "int",
                "IntValueObjectAttribute",
                "ValueObjectGenerator.IntValueObjectAttribute",
                attributeText,
                context
            );
        }
    }

    [Generator]
    public class LongValueObjectGenerator : ISourceGenerator
    {
        private const string attributeText = @"
using System;
namespace ValueObjectGenerator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class LongValueObjectAttribute : Attribute
    {
        public string PropertyName { get; set; }
    }
}
";

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            Executor.Execute(
                "long",
                "LongValueObjectAttribute",
                "ValueObjectGenerator.LongValueObjectAttribute",
                attributeText,
                context
            );
        }
    }

    [Generator]
    public class FloatValueObjectGenerator : ISourceGenerator
    {
        private const string attributeText = @"
using System;
namespace ValueObjectGenerator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class FloatValueObjectAttribute : Attribute
    {
        public string PropertyName { get; set; }
    }
}
";

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            Executor.Execute(
                "float",
                "FloatValueObjectAttribute",
                "ValueObjectGenerator.FloatValueObjectAttribute",
                attributeText,
                context
            );
        }
    }

    [Generator]
    public class DoubleFloat : ISourceGenerator
    {
        private const string attributeText = @"
using System;
namespace ValueObjectGenerator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class DoubleValueObjectAttribute : Attribute
    {
        public string PropertyName { get; set; }
    }
}
";

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            Executor.Execute(
                "double",
                "DoubleValueObjectAttribute",
                "ValueObjectGenerator.DoubleValueObjectAttribute",
                attributeText,
                context
            );
        }
    }

    internal static class Executor
    {
        internal static void Execute(
            string valueType,
            string hintName,
            string metaDataName,
            string attributeText,
            GeneratorExecutionContext context)
        {
            context.AddSource(hintName, SourceText.From(attributeText, Encoding.UTF8));

            if (!(context.SyntaxReceiver is SyntaxReceiver receiver)) return;

            var options = (context.Compilation as CSharpCompilation).SyntaxTrees[0].Options as CSharpParseOptions;
            var compilation = context.Compilation.AddSyntaxTrees(
                CSharpSyntaxTree.ParseText(SourceText.From(attributeText, Encoding.UTF8), options));
            var attributeSymbol = compilation.GetTypeByMetadataName(metaDataName);

            foreach (var candidate in receiver.CandidateTypes)
            {
                var model = compilation.GetSemanticModel(candidate.typeDeclarationSyntax.SyntaxTree);
                var typeSymbol = ModelExtensions.GetDeclaredSymbol(model, candidate.typeDeclarationSyntax);
                if (typeSymbol.GetAttributes().Any(ad =>
                    ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default)))
                {
                    var classSource = ProcessType(valueType, typeSymbol, attributeSymbol, candidate.isStruct);
                    context.AddSource(
                        $"{typeSymbol.ContainingNamespace.ToDisplayString()}_{typeSymbol.Name}_value_object",
                        SourceText.From(classSource, Encoding.UTF8));
                }
            }
        }

        internal static string ProcessType(string valueType, ISymbol typeSymbol, ISymbol attributeSymbol,
            bool isStruct)
        {
            if (!typeSymbol.ContainingSymbol.Equals(typeSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
                return null;

            var defaultPropertyName = "Value";
            var namespaceName = typeSymbol.ContainingNamespace.ToDisplayString();
            var attributeData = typeSymbol.GetAttributes().Single(ad =>
                ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default));
            var overridenNameOpt =
                attributeData.NamedArguments.SingleOrDefault(kvp => kvp.Key == "PropertyName").Value;
            var propertyName = overridenNameOpt.IsNull ? defaultPropertyName : overridenNameOpt.Value.ToString();
            var source = new StringBuilder($@"
namespace {namespaceName}
{{
    public {(isStruct ? "" : "sealed")} partial {(isStruct ? "struct" : "class")} {typeSymbol.Name} : System.IEquatable<{typeSymbol.Name}>
    {{
        public {valueType} {propertyName} {{ get; }}
        public {typeSymbol.Name}({valueType} value)
        {{
            {propertyName} = value;
        }}
        public override bool Equals(object obj) => ReferenceEquals(this, obj) || obj is {typeSymbol.Name} other && Equals(other);
        public override int GetHashCode() => {propertyName}.GetHashCode();
        public override string ToString() => {propertyName}.ToString();
        public static bool operator ==({typeSymbol.Name} left, {typeSymbol.Name} right) => Equals(left, right);
        public static bool operator !=({typeSymbol.Name} left, {typeSymbol.Name} right) => !Equals(left, right);
        public bool Equals({typeSymbol.Name} other)
        {{
            {(isStruct ? "" : "if (ReferenceEquals(null, other)) return false;")}
            {(isStruct ? "" : "if (ReferenceEquals(this, other)) return true;")}
            return {propertyName} == other.{propertyName};
        }}
        public static explicit operator {typeSymbol.Name}({valueType} value) => new {typeSymbol.Name}(value);
        public static explicit operator {valueType}({typeSymbol.Name} value) => value.{propertyName};
    }}
}}");
            return source.ToString();
        }
    }

    internal class SyntaxReceiver : ISyntaxReceiver
    {
        internal IList<(TypeDeclarationSyntax typeDeclarationSyntax, bool isStruct)> CandidateTypes { get; } =
            new List<(TypeDeclarationSyntax, bool)>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax
                && classDeclarationSyntax.AttributeLists.Count > 0)
                CandidateTypes.Add((classDeclarationSyntax, false));

            if (syntaxNode is StructDeclarationSyntax structDeclarationSyntax
                && structDeclarationSyntax.AttributeLists.Count > 0)
                CandidateTypes.Add((structDeclarationSyntax, true));
        }
    }
}