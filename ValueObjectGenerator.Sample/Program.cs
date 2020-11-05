using System;
using ValueObjectGenerator;

namespace ValueObjectGeneratorSample
{
    [StringValueObject]
    public partial class UserName
    {
    }

    [StringValueObject(PropertyName = "StringValue")]
    public partial class CustomizedPropertyName
    {
    }

    [IntValueObject]
    public partial class ProductId
    {
    }

    [IntValueObject]
    public partial class CategoryId
    {
    }

    [LongValueObject]
    public partial class ConsumeId
    {
    }

    [FloatValueObject]
    public partial class Scale
    {
    }

    [DoubleValueObject]
    public partial class Rate
    {
    }


    [StringValueObject]
    public partial struct StructStringValueObject
    {
    }

    public static class Program
    {
        public static void SampleStringValueObject()
        {
            Console.WriteLine("---StringValueObject Sample---");
            var userName = new UserName("Ryota");
            var otherUserName = userName;

            Console.WriteLine("var userName = new UserName(\"Ryota\");");
            Console.WriteLine("var otherUserName = userName;");
            Console.WriteLine();
            Console.WriteLine($"userName: {userName}");
            Console.WriteLine($"userName.Value: {userName.Value}");
            Console.WriteLine();
            Console.WriteLine($"userName == otherUserName: {userName == otherUserName}");
            Console.WriteLine($"userName.Equals(otherUserName): {userName.Equals(otherUserName)}");
            Console.WriteLine($"userName == new UserName(\"Ryota\"): {userName == new UserName("Ryota")}");
            Console.WriteLine($"userName.Equals(new UserName(\"Ryota\")): {userName.Equals(new UserName("Ryota"))}");
            Console.WriteLine();
            Console.WriteLine($"userName == new UserName(\"Taro\"): {userName == new UserName("Taro")}");
            Console.WriteLine($"userName.Equals(new UserName(\"Taro\"): {userName.Equals(new UserName("Taro"))}");
            Console.WriteLine($"userName.Equals(null): {userName.Equals(null)}");
            Console.WriteLine($"userName.Equals(\"\"): {userName.Equals("")}");
            Console.WriteLine();
        }

        public static void SampleIntValueObject()
        {
            Console.WriteLine("---IntValueObject Sample---");
            var productId = new ProductId(1);
            var otherProductId = productId;
            var categoryId = new CategoryId(1);

            Console.WriteLine("var productId = new ProductId(1);");
            Console.WriteLine("var otherProductId = productId;");
            Console.WriteLine("var categoryId = new CategoryId(1);");
            Console.WriteLine();
            Console.WriteLine($"productId: {productId}");
            Console.WriteLine($"productId.Value: {productId.Value}");
            Console.WriteLine();
            Console.WriteLine($"productId == otherProductId: {productId == otherProductId}");
            Console.WriteLine($"productId.Equals(otherProductId): {productId.Equals(otherProductId)}");
            Console.WriteLine($"productId == new ProductId(1): {productId == new ProductId(1)}");
            Console.WriteLine($"productId.Equals(new ProductId(1)): {productId.Equals(new ProductId(1))}");
            Console.WriteLine();
            Console.WriteLine($"productId == new ProductId(2): {productId == new ProductId(2)}");
            Console.WriteLine($"productId.Equals(new ProductId(2)): {productId.Equals(new ProductId(2))}");
            Console.WriteLine($"productId.Equals(null): {productId.Equals(null)}");
            Console.WriteLine($"productId.Equals(1): {productId.Equals(1)}");
            Console.WriteLine();
            Console.WriteLine("productId == categoryId: Compile Error");
            Console.WriteLine($"productId.Equals(categoryId): {productId.Equals(categoryId)}");
            Console.WriteLine("productId == new CategoryId(1):  Compile Error");
            Console.WriteLine($"productId.Equals(new CategoryId(1)): {productId.Equals(new CategoryId(1))}");
            Console.WriteLine();
        }

        public static void SampleLongValueObject()
        {
            Console.WriteLine("---LongValueObject Sample---");
            var consumeId = new ConsumeId(1L);
            var otherConsumeId = consumeId;

            Console.WriteLine("var consumeId = new ConsumeId(1L);");
            Console.WriteLine("var otherConsumeId = consumeId;");
            Console.WriteLine();
            Console.WriteLine($"consumeId: {consumeId}");
            Console.WriteLine($"consumeId.Value: {consumeId.Value}");
            Console.WriteLine();
            Console.WriteLine($"consumeId == otherConsumeId: {consumeId == otherConsumeId}");
            Console.WriteLine($"consumeId.Equals(otherConsumeId): {consumeId.Equals(otherConsumeId)}");
            Console.WriteLine($"consumeId == new ConsumeId(1L): {consumeId == new ConsumeId(1L)}");
            Console.WriteLine($"consumeId.Equals(new ConsumeId(1L)): {consumeId.Equals(new ConsumeId(1L))}");
            Console.WriteLine();
            Console.WriteLine($"consumeId == new ConsumeId(2L): {consumeId == new ConsumeId(2L)}");
            Console.WriteLine($"consumeId.Equals(new ConsumeId(2L): {consumeId.Equals(new ConsumeId(2L))}");
            Console.WriteLine($"consumeId.Equals(null): {consumeId.Equals(null)}");
            Console.WriteLine($"consumeId.Equals(1L): {consumeId.Equals(1L)}");
            Console.WriteLine();
        }

        public static void SampleFloatValueObject()
        {
            Console.WriteLine("---FloatValueObject Sample---");
            var scale = new Scale(0.5F);
            var otherScale = scale;

            Console.WriteLine("var scale = new Scale(0.5F);");
            Console.WriteLine("var otherScale = scale;");
            Console.WriteLine();
            Console.WriteLine($"scale: {scale}");
            Console.WriteLine($"scale.Value: {scale.Value}");
            Console.WriteLine();
            Console.WriteLine($"scale == otherScale: {scale == otherScale}");
            Console.WriteLine($"scale.Equals(otherScale): {scale.Equals(otherScale)}");
            Console.WriteLine($"scale == new Scale(1L): {scale == new Scale(1L)}");
            Console.WriteLine($"scale.Equals(new Scale(1L)): {scale.Equals(new Scale(1L))}");
            Console.WriteLine();
            Console.WriteLine($"scale == new Scale(2L): {scale == new Scale(2L)}");
            Console.WriteLine($"scale.Equals(new Scale(2L): {scale.Equals(new Scale(2L))}");
            Console.WriteLine($"scale.Equals(null): {scale.Equals(null)}");
            Console.WriteLine($"scale.Equals(1L): {scale.Equals(0.5F)}");
            Console.WriteLine();
        }

        public static void SampleDoubleValueObject()
        {
            Console.WriteLine("---DoubleValueObject Sample---");
            var rate = new Rate(0.25);
            var otherRate = rate;

            Console.WriteLine("var rate = new Rate(0.25);");
            Console.WriteLine("var otherRate = rate;");
            Console.WriteLine();
            Console.WriteLine($"rate: {rate}");
            Console.WriteLine($"rate.Value: {rate.Value}");
            Console.WriteLine();
            Console.WriteLine($"rate == otherRate: {rate == otherRate}");
            Console.WriteLine($"rate.Equals(otherRate): {rate.Equals(otherRate)}");
            Console.WriteLine($"rate == new Rate(0.25): {rate == new Rate(0.25)}");
            Console.WriteLine($"rate.Equals(new Rate(0.25)): {rate.Equals(new Rate(0.25))}");
            Console.WriteLine();
            Console.WriteLine($"rate == new Rate(0.1): {rate == new Rate(0.1)}");
            Console.WriteLine($"rate.Equals(new Rate(0.1): {rate.Equals(new Rate(0.1))}");
            Console.WriteLine($"rate.Equals(null): {rate.Equals(null)}");
            Console.WriteLine($"rate.Equals(0.25): {rate.Equals(0.25)}");
            Console.WriteLine();
        }

        public static void SampleCustomizedPropertyName()
        {
            Console.WriteLine("---CustomizedPropertyName Sample---");
            Console.WriteLine("var fieldName = new CustomizedPropertyName(\"CustomizedPropertyName\");");

            var fieldName = new CustomizedPropertyName("CustomizedPropertyName");
            Console.WriteLine($"fieldName.StringValue : {fieldName.StringValue}");
            Console.WriteLine();
        }

        public static void SampleStructStringValueObject()
        {
            Console.WriteLine("---StructStringValueObject Sample---");
            var structStringValueObject = new StructStringValueObject("Ryota");
            var otherStructStringValueObject = structStringValueObject;

            Console.WriteLine("var structStringValueObject = new StructStringValueObject(\"Ryota\")");
            Console.WriteLine("var otherStructStringValueObject = structStringValueObject;");
            Console.WriteLine();
            Console.WriteLine($"structStringValueObject: {structStringValueObject}");
            Console.WriteLine($"structStringValueObject.Value: {structStringValueObject.Value}");
            Console.WriteLine();
            Console.WriteLine($"structStringValueObject == otherStructStringValueObject: {structStringValueObject == otherStructStringValueObject}");
            Console.WriteLine($"structStringValueObject.Equals(otherStructStringValueObject): {structStringValueObject.Equals(otherStructStringValueObject)}");
            Console.WriteLine($"structStringValueObject == new StructStringValueObject(\"Ryota\"): {structStringValueObject == new StructStringValueObject("Ryota")}");
            Console.WriteLine($"structStringValueObject.Equals(new StructStringValueObject(\"Ryota\")): {structStringValueObject.Equals(new StructStringValueObject("Ryota"))}");
            Console.WriteLine();
            Console.WriteLine($"structStringValueObject == new StructStringValueObject(\"Taro\"): {structStringValueObject == new StructStringValueObject("Taro")}");
            Console.WriteLine($"structStringValueObject.Equals(new StructStringValueObject(\"Taro\"): {structStringValueObject.Equals(new StructStringValueObject("Taro"))}");
            Console.WriteLine($"structStringValueObject.Equals(null): {structStringValueObject.Equals(null)}");
            Console.WriteLine($"structStringValueObject.Equals(\"\"): {structStringValueObject.Equals("")}");
            Console.WriteLine();
            Console.WriteLine($"ReferenceEquals(structStringValueObject, otherStructStringValueObject): {ReferenceEquals(structStringValueObject, otherStructStringValueObject)}");
            Console.WriteLine();
        }

        public static void Main(string[] args)
        {
            SampleStringValueObject();
            SampleIntValueObject();
            SampleLongValueObject();
            SampleFloatValueObject();
            SampleDoubleValueObject();
            SampleCustomizedPropertyName();
            SampleStructStringValueObject();
        }
    }
}