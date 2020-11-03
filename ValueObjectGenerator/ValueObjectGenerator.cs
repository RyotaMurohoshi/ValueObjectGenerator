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
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
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
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
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
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
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
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
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

            foreach (ClassDeclarationSyntax candidateClass in receiver.CandidateClasses)
            {
                var model = compilation.GetSemanticModel(candidateClass.SyntaxTree);
                var classSymbol = ModelExtensions.GetDeclaredSymbol(model, candidateClass);
                if (classSymbol.GetAttributes().Any(ad =>
                    ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default)))
                {
                    var classSource = ProcessClass(valueType, classSymbol, attributeSymbol);
                    context.AddSource(
                        $"{classSymbol.ContainingNamespace.ToDisplayString()}_{classSymbol.Name}_value_object",
                        SourceText.From(classSource, Encoding.UTF8));
                }
            }
        }

        internal static string ProcessClass(string valueType, ISymbol classSymbol, ISymbol attributeSymbol)
        {
            if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
                return null; //TODO: issue a diagnostic that it must be top level

            var defaultPropertyName = "Value";
            var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            var attributeData = classSymbol.GetAttributes().Single(ad =>
                ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default));
            var overridenNameOpt =
                attributeData.NamedArguments.SingleOrDefault(kvp => kvp.Key == "PropertyName").Value;
            var propertyName = overridenNameOpt.IsNull ? defaultPropertyName : overridenNameOpt.Value.ToString();
            var source = new StringBuilder($@"
namespace {namespaceName}
{{
    public partial class {classSymbol.Name} : System.IEquatable<{classSymbol.Name}>
    {{
        public {valueType} {propertyName} {{ get; }}
        public {classSymbol.Name}({valueType} value)
        {{
            {propertyName} = value;
        }}
        public override bool Equals(object obj) => ReferenceEquals(this, obj) || obj is {classSymbol.Name} other && Equals(other);
        public override int GetHashCode() => {propertyName}.GetHashCode();
        public override string ToString() => {propertyName}.ToString();
        public static bool operator ==({classSymbol.Name} left, {classSymbol.Name} right) => Equals(left, right);
        public static bool operator !=({classSymbol.Name} left, {classSymbol.Name} right) => !Equals(left, right);
        public bool Equals({classSymbol.Name} other)
        {{
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return {propertyName} == other.{propertyName};
        }}
        public static explicit operator {classSymbol.Name}({valueType} value) => new {classSymbol.Name}(value);
        public static explicit operator {valueType}({classSymbol.Name} value) => value.{propertyName};
    }}
}}");
            return source.ToString();
        }
    }

    internal class SyntaxReceiver : ISyntaxReceiver
    {
        internal IList<ClassDeclarationSyntax> CandidateClasses { get; } = new List<ClassDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax
                && classDeclarationSyntax.AttributeLists.Count > 0)
                CandidateClasses.Add(classDeclarationSyntax);
        }
    }
}
