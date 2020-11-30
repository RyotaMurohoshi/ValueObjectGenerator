# ValueObjectGenerator

`ValueObjectGenerator` は、ValueObjectクラス(もしくは、Wrapperクラス)向けの`C# Source Generator`です。

このプロジェクトは、現在開発中です🚧.

## 導入方法

現在、開発中です。

## 使い方

```csharp
using ValueObjectGenerator;

[IntValueObject]
public partial class ProductId
{
}
```

上記のように `IntValueObject` 属性をクラスに付与します。そうすると次のようなコードが生成されます。

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

`ProductId` は ValueObjectクラス(もしくは、Wrapperクラス)です。

次のテープルは、属性とそれに対応する型を示しています。

| attribute  | type |
----|----
| StringValueObject | string |
| IntValueObject | int |
| LongValueObject | long |
| FloatValueObject | float |
| DoubleValueObject | double |

## 背景と開発動機

次の `Product` クラスは、2つのint型のプロパティ、`ProductId` と `ProductCategoryId`を持っています。
この型のインスタンスの利用シーンにおいて、いくつかの場所では `ProductId`が必要で, 他の場所では `ProductCategoryId`が必要でしょう。
ですが、どちらもint型なので、`ProductId` と `ProductCategoryId`を取り違えてしまうかもしれません.

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

この取り違え型を防ぐにはどうしたらいいでしょうか？一つの方法としては、次のような`ProductId`型と`CategoryId`型を作り、それらを利用することです。
これらの型を利用することで、コンパイラは`ProductId`プロパティと`ProductCategoryId`プロパティの取り違えを検出し、プログラム上のミスを防ぐことができます。
このようにValueObjectクラス(もしくは、Wrapperクラス)を作成し利用することで、int型やstring型のプロパティの取り違えやミスを防ぐことができます。

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

このようにValueObjectクラス(もしくは、Wrapperクラス)を作成し利用することでコンパイルエラーを防ぐことができます。しかし、これらのコードは非常に大量のボイラープレートで溢れています。これらのボイラープレートコードは、他の大切な意味のあるコードを読む際にとても邪魔です。

`C# Source Generator`はこのような、ポイラープレートコードの生成にとても向いています。`ValueObjectGenerator`はValueObjectクラス (もしくはWrapperクラス)を`C# Source Generator`を用いて、生成します.

次のコードは `ValueObjectGenerator` の利用例です. ValueObjectクラス(もしくは、Wrapperクラス)を定義するためにあった、大量のボイラープレートコードはなくなりました。

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

## 今後の計画

* IComparableのサポート
* JSON serializer/deserializer
* 他のValueタイプ
* 算術演算のサポート

## 開発者

日本の趣味個人ゲーム開発者の Ryota Murohoshi です。

* Posts:http://qiita.com/RyotaMurohoshi (日本語)
* Twitter:https://twitter.com/RyotaMurohoshi (日本語)

## ライセンス

MITライセンスです。
