# ValueObjectGenerator

`ValueObjectGenerator` is C# source generator is for ValueObjects (ie.Wrapper classes).

This project is under the develop🚧.

## Install

Under the develop.

## Usage

```csharp
using ValueObjectGenerator;

[IntValueObject]
public partial class ProductId
{
}
```

With `IntValueObject` attribute, below code is generated.

```csharp
public partial class ProductId: IEquatable<ProductId>
{
    public int Value { get; }

    public ProductId(int value)
    {
        Value = value;
    }

    public override bool Equals(object obj) => ReferenceEquals(this, obj) || obj is ProductId other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
    public static bool operator ==(ProductId left, ProductId right) => Equals(left, right);
    public static bool operator !=(ProductId left, ProductId right) => !Equals(left, right);

    public bool Equals(ProductId other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public static explicit operator ProductId(int value) => new ProductId(value);
    public static explicit operator int(ProductId value) => value.Value;
}
```

`ProductId` is a `ValueObject` class (ie.Wrapper classes).

Below table shows all supporting Attributes and corresponding Types.

| attribute  | type |
----|----
| StringValueObject | string |
| IntValueObject | int |
| LongValueObject | long |
| FloatValueObject | float |
| DoubleValueObject | double |

## Motivation

Next `Product` class has 2 int type properties, `ProductId` and `ProductCategoryId`.
Some methods need `ProductId` as an argument, the other methods need `ProductCategoryId` as an argument.
Sometime, developers may mistake between `ProductId` and `ProductCategoryId`.

```csharp
public class Product
{
    public Product(string name, int productId, int productCategoryId)
    {
        Name = name;
        ProductId = productId;
        ProductCategoryId = productCategoryId;
    }

    public string Name { get; }
    public int ProductId { get; }
    public int ProductCategoryId { get; }
}
```

To avoid this mistake, we should create `ProductId` class and `CategoryId` class, and use them.
With these classes, compiler can detect wrong usage of `ProductId` property and `ProductCategoryId` property.

```csharp
public sealed class ProductId: IEquatable<ProductId>
{
    public int Value { get; }

    public ProductId(int value)
    {
        Value = value;
    }

    public override bool Equals(object obj) => ReferenceEquals(this, obj) || obj is ProductId other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
    public static bool operator ==(ProductId left, ProductId right) => Equals(left, right);
    public static bool operator !=(ProductId left, ProductId right) => !Equals(left, right);

    public bool Equals(ProductId other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public static explicit operator ProductId(int value) => new ProductId(value);
    public static explicit operator int(ProductId value) => value.Value;
}

public class CategroyId: IEquatable<CategroyId>
{
    public int Value { get; }

    public CategroyId(int value)
    {
        Value = value;
    }

    public override bool Equals(object obj) => ReferenceEquals(this, obj) || obj is CategroyId other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
    public static bool operator ==(CategroyId left, CategroyId right) => Equals(left, right);
    public static bool operator !=(CategroyId left, CategroyId right) => !Equals(left, right);

    public bool Equals(CategroyId other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public static explicit operator CategroyId(int value) => new CategroyId(value);
    public static explicit operator int(CategroyId value) => value.Value;
}


public class Product
{
    public Product(string name, ProductId productId, CategroyId productCategoryId)
    {
        Name = name;
        ProductId = productId;
        ProductCategoryId = productCategoryId;
    }

    public string Name { get; }
    public ProductId ProductId { get; }
    public CategroyId ProductCategoryId { get; }
}
```

There are many boilerplate code in `ProductId` and `CategoryId` classes.  These boilerplate code is so noisy to read other important meaningful code.

`C# Source Generator` can generate these boilerplate code with smart way. `ValueObjectGenerator` generate ValueObject classes (ie.Wrapper classes) with `C# Source Generator`.

Below code is example of `ValueObjectGenerator`. There are no boilerplate code to create ValueObjects (ie.Wrapper classes ) like `ProductId` and `CategoryId` classes.

```csharp
[StringValueObject]
public class ProductName { }

[IntValueObject]
public class ProductId { }

[IntValueObject]
public class CategoryId { }

public class Product
{
    public Product(ProductName name, ProductId productId, CategoryId productCategoryId)
    {
        Name = name;
        ProductId = productId;
        ProductCategoryId = productCategoryId;
    }

    public ProductName Name { get; }
    public ProductId ProductId { get; }
    public CategoryId ProductCategoryId { get; }
}
```

## Plans

* JSON serializer/deserializer.
* other value types.

## Author

Ryota Murohoshi is game Programmer in Japan.

* Posts:http://qiita.com/RyotaMurohoshi (JPN)
* Twitter:https://twitter.com/RyotaMurohoshi (JPN)

## License

This library is under MIT License.
