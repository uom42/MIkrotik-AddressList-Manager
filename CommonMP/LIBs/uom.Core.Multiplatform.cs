#define UOM_TEST_DEF

#nullable enable

global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Collections.Specialized;
global using System.ComponentModel;
global using System.Diagnostics;
global using System.Globalization;
global using System.IO;
global using System.Linq;
global using System.Net;
global using System.Numerics;
global using System.Reflection;
global using System.Runtime.CompilerServices;
global using System.Runtime.InteropServices;
global using System.Security;
global using System.Security.Cryptography;
global using System.Security.Principal;
global using System.Text;
global using System.Text.RegularExpressions;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Xml;
global using System.Xml.Linq;

global using uom.Extensions;

global using static uom.constants;
global using static uom.Extensions.Extensions_DebugAndErrors;

using System.Buffers;
using System.Buffers.Text;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;

using Vanara.Extensions;


//using Windows.Foundation.Metadata;
//using Microsoft.Win32;



#if UOM_TEST_DEF
//Console.WriteLine("Visual Studio 7");
#endif


#region Code Snippets



/*
 
 
 
 <PropertyGroup Condition="$([MSBuild]::IsOSPlatform('Windows'))">
		<DefineConstants>WINDOWS;WINFORMS</DefineConstants>
	</PropertyGroup>
	<PropertyGroup Condition="$([MSBuild]::IsOSPlatform('Linux'))">
		<DefineConstants>LINUX</DefineConstants>
	</PropertyGroup>
	<PropertyGroup Condition="$([MSBuild]::IsOSPlatform('FreeBSD'))">
		<DefineConstants>FREEBSD</DefineConstants>
	</PropertyGroup>
	<PropertyGroup Condition="$([MSBuild]::IsOSPlatform('OSX'))">
		<DefineConstants>MAC</DefineConstants>
	</PropertyGroup>
 
 
 */




#region NET Versions
//https://docs.microsoft.com/ru-ru/dotnet/standard/frameworks

#region .NET Framework
//NETFRAMEWORK, NET48, NET472, NET471, NET47, NET462, NET461, NET46, NET452, NET451, NET45, NET40, NET35, NET20
//NET48_OR_GREATER, NET472_OR_GREATER, NET471_OR_GREATER, NET47_OR_GREATER, NET462_OR_GREATER, NET461_OR_GREATER, NET46_OR_GREATER, NET452_OR_GREATER, NET451_OR_GREATER, NET45_OR_GREATER, NET40_OR_GREATER, NET35_OR_GREATER, NET20_OR_GREATER
#endregion
#region .NET Standard
//NETSTANDARD, NETSTANDARD2_1, NETSTANDARD2_0, NETSTANDARD1_6, NETSTANDARD1_5, NETSTANDARD1_4, NETSTANDARD1_3, NETSTANDARD1_2, NETSTANDARD1_1, NETSTANDARD1_0
//NETSTANDARD2_1_OR_GREATER, NETSTANDARD2_0_OR_GREATER, NETSTANDARD1_6_OR_GREATER, NETSTANDARD1_5_OR_GREATER, NETSTANDARD1_4_OR_GREATER, NETSTANDARD1_3_OR_GREATER, NETSTANDARD1_2_OR_GREATER, NETSTANDARD1_1_OR_GREATER, NETSTANDARD1_0_OR_GREATER
#endregion
#region .NET 5+ (и .NET Core)
//NET, NET6_0, NET6_0_ANDROID, NET6_0_IOS, NET6_0_MACOS, NET6_0_MACCATALYST, NET6_0_TVOS, NET6_0_WINDOWS, NET5_0, NETCOREAPP, NETCOREAPP3_1, NETCOREAPP3_0, NETCOREAPP2_2, NETCOREAPP2_1, NETCOREAPP2_0, NETCOREAPP1_1, NETCOREAPP1_0
//NET6_0_OR_GREATER, NET6_0_ANDROID_OR_GREATER, NET6_0_IOS_OR_GREATER, NET6_0_MACOS_OR_GREATER, NET6_0_MACCATALYST_OR_GREATER, NET6_0_TVOS_OR_GREATER, NET6_0_WINDOWS_OR_GREATER, NET5_0_OR_GREATER, NETCOREAPP_OR_GREATER, NETCOREAPP3_1_OR_GREATER, NETCOREAPP3_0_OR_GREATER, NETCOREAPP2_2_OR_GREATER, NETCOREAPP2_1_OR_GREATER, NETCOREAPP2_0_OR_GREATER, NETCOREAPP1_1_OR_GREATER, NETCOREAPP1_0_OR_GREATER
#endregion
#region Versions samples
#if NET6_0_WINDOWS || NET5_0_OR_GREATER || NET6_0_OR_GREATER || NET6_0_ANDROID || NET6_0_MACOS || NET6_0_IOS
#endif
#if NET48_OR_GREATER
#endif
#endregion
#endregion

//	Using tuples to swap values
//	(li.BackColor, li.ForeColor) = (li.ForeColor, li.BackColor);

#region Limit T to strings / integers etc:
/*
https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters

  where T : operators( +, -, /, * )

Beginning with C# 7.3, you can use closer approximation - the unmanaged constraint to specify that a type parameter is a non-pointer, non-nullable unmanaged type.
A type is an unmanaged type if it's any of the following types:

sbyte, byte, short, ushort, int, uint, long, ulong, char, float, double, decimal, or bool
Any enum type
Any pointer type
Any user-defined struct type that contains fields of unmanaged types only and, in C# 7.3 and earlier, is not a constructed type (a type that includes at least one type argument)
where T : unmanaged, IComparable, IEquatable<T>
*/

#endregion

#region NULLables
/*
 
	https://docs.microsoft.com/ru-ru/dotnet/csharp/language-reference/attributes/nullable-analysis 
	
System.Diagnostics.CodeAnalysis.AllowNullAttribute

[return: MaybeNull]
public T Find<T>(IEnumerable<T> sequence, Func<T, bool> predicate)

public static void ThrowWhenNull([NotNull] object? value, string valueExpression = "")
{
   _ = value ?? throw new ArgumentNullException(nameof(value), valueExpression);
   other logic elided
}

#nullable disable warnings
See All # Constants: https://docs.microsoft.com/ru-ru/dotnet/csharp/language-reference/preprocessor-directives
*/
#endregion

#region Class destructor
/*
		/// <summary>Destructor</summary>
		~KeyboardHook()
		{
		}
 */
#endregion

//if (e is MethodCallExpression { Method.Name: "MethodName" })

//	object? value = key?.GetValue("AppsUseLightTheme");
//	return value is int i && i > 0;

#region Comments https://learn.microsoft.com/ru-ru/dotnet/csharp/language-reference/xmldoc/recommended-tags#para

/*

/// <inheritdoc cref="ContextMenu_UnRegisterAction" />
/// <inheritdoc />

//https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// <summary>
/// Return Human-Readable bytes order (Sample: 192.168.1.1 => [192,168,1,1])
/// <c>!!! Don't use for any arifmetic calculations! Use only for Saving/Restoring IP !!!</c>
/// To Math calculations with IP use <see cref="eToUInt32CalculableOrder"/> instead!!!
/// <example>
/// For example:
/// <code>
/// Point p = new Point(3,5);
/// p.Translate(-1,3);
/// </code>
/// results in <c>p</c>'s having the value (2,8).
/// </example>
/// </summary>
/// <returns>for 192.168.1.1 => [192,168,1,1]</returns>

------------------------------------------------

  /// <summary>
    /// Every class and member should have a one sentence
    /// summary describing its purpose.
    /// </summary>
    /// <remarks>
    /// You can expand on that one sentence summary to
    /// provide more information for readers. In this case,
    /// the <c>ExampleClass</c> provides different C#
    /// elements to show how you would add documentation
    ///comments for most elements in a typical class.
    /// <para>
    /// The remarks can add multiple paragraphs, so you can
    /// write detailed information for developers that use
    /// your work. You should add everything needed for
    /// readers to be successful. This class contains
    /// examples for the following:
    /// </para>
    /// <list type="table">
    /// <item>
    /// <term>Summary</term>
    /// <description>
    /// This should provide a one sentence summary of the class or member.
    /// </description>
    /// </item>
    /// <item>
    /// <term>Remarks</term>
    /// <description>
    /// This is typically a more detailed description of the class or member
    /// </description>
    /// </item>
    /// <item>
    /// <term>para</term>
    /// <description>
    /// The para tag separates a section into multiple paragraphs
    /// </description>
    /// </item>
    /// <item>
    /// <term>list</term>
    /// <description>
    /// Provides a list of terms or elements
    /// </description>
    /// </item>
    /// <item>
    /// <term>returns, param</term>
    /// <description>
    /// Used to describe parameters and return values
    /// </description>
    /// </item>
    /// <item>
    /// <term>value</term>
    /// <description>Used to describe properties</description>
    /// </item>
    /// <item>
    /// <term>exception</term>
    /// <description>
    /// Used to describe exceptions that may be thrown
    /// </description>
    /// </item>
    /// <item>
    /// <term>c, cref, see, seealso</term>
    /// <description>
    /// These provide code style and links to other
    /// documentation elements
    /// </description>
    /// </item>
    /// <item>
    /// <term>example, code</term>
    /// <description>
    /// These are used for code examples
    /// </description>
    /// </item>
    /// </list>
    /// <para>
    /// The list above uses the "table" style. You could
    /// also use the "bullet" or "number" style. Neither
    /// would typically use the "term" element.
    /// <br/>
    /// Note: paragraphs are double spaced. Use the *br*
    /// tag for single spaced lines.
    /// </para>
    /// </remarks>
    public class ExampleClass
    {
        /// <value>
        /// The <c>Label</c> property represents a label
        /// for this instance.
        /// </value>
        /// <remarks>
        /// The <see cref="Label"/> is a <see langword="string"/>
        /// that you use for a label.
        /// <para>
        /// Note that there isn't a way to provide a "cref" to
        /// each accessor, only to the property itself.
        /// </para>
        /// </remarks>
        public string? Label
        {
            get;
            set;
        }

        /// <summary>
        /// Adds two integers and returns the result.
        /// </summary>
        /// <returns>
        /// The sum of two integers.
        /// </returns>
        /// <param name="left">
        /// The left operand of the addition.
        /// </param>
        /// <param name="right">
        /// The right operand of the addition.
        /// </param>
        /// <example>
        /// <code>
        /// int c = Math.Add(4, 5);
        /// if (c > 10)
        /// {
        ///     Console.WriteLine(c);
        /// }
        /// </code>
        /// </example>
        /// <exception cref="System.OverflowException">
        /// Thrown when one parameter is
        /// <see cref="Int32.MaxValue">MaxValue</see> and the other is
        /// greater than 0.
        /// Note that here you can also use
        /// <see href="https://learn.microsoft.com/dotnet/api/system.int32.maxvalue"/>
        ///  to point a web page instead.
        /// </exception>
        /// <see cref="ExampleClass"/> for a list of all
        /// the tags in these examples.
        /// <seealso cref="ExampleClass.Label"/>
        public static int Add(int left, int right)
        {
            if ((left == int.MaxValue && right > 0) || (right == int.MaxValue && left > 0))
                throw new System.OverflowException();

            return left + right;
        }
    }

    /// <summary>
    /// This is an example of a positional record.
    /// </summary>
    /// <remarks>
    /// There isn't a way to add XML comments for properties
    /// created for positional records, yet. The language
    /// design team is still considering what tags should
    /// be supported, and where. Currently, you can use
    /// the "param" tag to describe the parameters to the
    /// primary constructor.
    /// </remarks>
    /// <param name="FirstName">
    /// This tag will apply to the primary constructor parameter.
    /// </param>
    /// <param name="LastName">
    /// This tag will apply to the primary constructor parameter.
    /// </param>
    public record Person(string FirstName, string LastName);
}




 */



#endregion

#region Binary variable definitions
/*
 
int myValue = 0b0010_0110_0000_0011;

[Flags]
enum Days
{
	None = 0,
	Sunday = 0b0000001,
	Monday = 0b0000010,   // 2
	Tuesday = 0b0000100,   // 4
	Wednesday = 0b0001000,   // 8
	Thursday = 0b0010000,   // 16
	Friday = 0b0100000,   // etc.
	Saturday = 0b1000000,
	Weekend = Saturday | Sunday,
	Weekdays = Monday | Tuesday | Wednesday | Thursday | Friday
}

[Flags]
enum Days2
{
	None = 0,
	Sunday = 1,
	Monday = 1 << 1,   // 2
	Tuesday = 1 << 2,   // 4
	Wednesday = 1 << 3,   // 8
	Thursday = 1 << 4,   // 16
	Friday = 1 << 5,   // etc.
	Saturday = 1 << 6,
	Weekend = Saturday | Sunday,
	Weekdays = Monday | Tuesday | Wednesday | Thursday | Friday
}

[Flags]
enum Scenery
{
	Trees = 0x001, // 000000000001
	Grass = 0x002, // 000000000010
	Flowers = 0x004, // 000000000100
	Cactus = 0x008, // 000000001000
	Birds = 0x010, // 000000010000
	Bushes = 0x020, // 000000100000
	Shrubs = 0x040, // 000001000000
	Trails = 0x080, // 000010000000
	Ferns = 0x100, // 000100000000
	Rocks = 0x200, // 001000000000
	Animals = 0x400, // 010000000000
	Moss = 0x800, // 100000000000
}

*/

#endregion
#region Object Comparing, inline type conversions, Template comparsions https://docs.microsoft.com/ru-ru/dotnet/csharp/language-reference/operators/patterns#relational-patterns
/*	


	if (input is not (float or double))
	{
		return;
	}


	public override bool Equals(object? obj)	
		=> obj is LocalMemory memory && EqualityComparer<IntPtr>.Default.Equals(handle, memory.handle);


	static bool IsLetter(char c) 
		=> c is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z');

	static bool IsConferenceDay(DateTime date) 
		=> date is { Year: 2020, Month: 5, Day: 19 or 20 or 21 };

	static string GetCalendarSeason(DateTime date) 
		=> date.Month switch
	{
		3 or 4 or 5 => "spring",
		6 or 7 or 8 => "summer",
		9 or 10 or 11 => "autumn",
		12 or 1 or 2 => "winter",
		_ => throw new ArgumentOutOfRangeException(nameof(date), $"Date with unexpected month: {date.Month}."),
	};

	static string Classify(double measurement) 
		=> measurement switch
	{
		< -40.0 => "Too low",
		>= -40.0 and < 0 => "Low",
		>= 0 and < 10.0 => "Acceptable",
		>= 10.0 and < 20.0 => "High",
		>= 20.0 => "Too high",
		double.NaN => "Unknown",
	};

	static string Classify(Point point) => point switch
	{
		(0, 0) => "Origin",
		(1, 0) => "positive X basis end",
		(0, 1) => "positive Y basis end",
		_ => "Just a point",
	};


	static decimal GetGroupTicketPriceDiscount(int groupSize, DateTime visitDate)
		=> (groupSize, visitDate.DayOfWeek) switch
		{
			(<= 0, _) => throw new ArgumentException("Group size must be positive."),
			(_, DayOfWeek.Saturday or DayOfWeek.Sunday) => 0.0m,
			(>= 5 and < 10, DayOfWeek.Monday) => 20.0m,
			(>= 10, DayOfWeek.Monday) => 30.0m,
			(>= 5 and < 10, _) => 12.0m,
			(>= 10, _) => 15.0m,
			_ => 0.0m,
		};




		var numbers = new List<int> { 1, 2, 3 };
		if (SumAndCount(numbers) is (Sum: var sum, Count: > 0))
		{
			Console.WriteLine($"Sum of [{string.Join(" ", numbers)}] is {sum}");  // output: Sum of [1 2 3] is 6
		}

		static (double Sum, int Count) SumAndCount(IEnumerable<int> numbers)
		{
			int sum = 0;
			int count = 0;
			foreach (int number in numbers)
			{
				sum += number;
				count++;
			}
			return (sum, count);
		}




		public record Point2D(int X, int Y);
		public record Point3D(int X, int Y, int Z);
		static string PrintIfAllCoordinatesArePositive(object point) => point switch
		{
			Point2D (> 0, > 0) p => p.ToString(),
			Point3D (> 0, > 0, > 0) p => p.ToString(),
			_ => string.Empty,
		};

		В предыдущем примере используются позиционные записи, которые неявно обеспечивают выполнение метода Deconstruct.
		Используйте шаблон свойства в позиционном шаблоне, как показано в следующем примере:

		public record WeightedPoint(int X, int Y)
		{
			public double Weight { get; set; }
		}

		static bool IsInDomain(WeightedPoint point) => point is (>= 0, >= 0) { Weight: >= 0.0 };	

		Можно объединить два предыдущих варианта, как показано в следующем примере:
		if (input is WeightedPoint (> 0, > 0) { Weight: > 0.0 } p)
		{
			// ..
		}


		**************** Шаблон var
		Начиная с C# 7.0, шаблонvar можно использовать для сопоставления любого выражения, включая null, и присвоения результата сопоставления новой локальной переменной, как показано в следующем примере:

		static bool IsAcceptable(int id, int absLimit) =>
			SimulateDataFetch(id) is var results 
			&& results.Min() >= -absLimit 
			&& results.Max() <= absLimit;

		static int[] SimulateDataFetch(int id)
		{
			var rand = new Random();
			return Enumerable
					   .Range(start: 0, count: 5)
					   .Select(s => rand.Next(minValue: -10, maxValue: 11))
					   .ToArray();
		}


		Шаблон var полезно использовать, если в логическом выражении вам требуется временная переменная для хранения результатов промежуточных вычислений. Шаблон var также можно использовать, если требуется реализовать дополнительные проверки в условиях регистра when выражения или оператора switch, как показано в следующем примере:

		public record Point(int X, int Y);
		static Point Transform(Point point) => point switch
		{
			var (x, y) when x < y => new Point(-x, y),
			var (x, y) when x > y => new Point(x, -y),
			var (x, y) => new Point(x, y),
		};

		static void TestTransform()
		{
			Console.WriteLine(Transform(new Point(1, 2)));  // output: Point { X = -1, Y = 2 }
			Console.WriteLine(Transform(new Point(5, 2)));  // output: Point { X = 5, Y = -2 }
		}    
 */
#endregion

#region stackalloc vs fixed
/*		
	Выражение stackalloc выделяет блок памяти в стеке. Выделенный в стеке блок памяти, который создает этот метод,
	автоматически удаляется по завершении выполнения метода. 
	Вы не можете явным образом освободить память, выделенную stackalloc. 
	Выделенный в стеке блок памяти не подвергается сборке мусора, поэтому его не нужно закреплять с помощью инструкции fixed.

	Объем доступной памяти в стеке ограничен. 
	При выделении слишком большого объема памяти в стеке возникает исключение StackOverflowException. 
	Чтобы избежать этого, следуйте приведенным ниже правилам.

	Ограничьте объем памяти, выделенный stackalloc. 
	Например, если предполагаемый размер буфера меньше определенного предела, то выделяется память в стеке. 
	В противном случае используйте массив требуемой длины, как показано в следующем коде:

	const int MaxStackLimit = 1024;
	Span<byte> buffer = inputLength <= MaxStackLimit ? stackalloc byte[MaxStackLimit] : new byte[inputLength];

	!!! Мы рекомендуем везде, где это возможно, использовать для работы с выделенной в стеке памятью типы Span<T> или ReadOnlySpan<T>.!!!

	int length = 3;
	Span<int> numbers = stackalloc int[length];
	for (var i = 0; i < length; i++)
	{
		numbers[i] = i;
	}

	Старайтесь не использовать stackalloc в циклах. Выделяйте блок памяти за пределами цикла и используйте его повторно внутри цикла.


	Span<int> numbers = stackalloc[] { 1, 2, 3, 4, 5, 6 };
	var ind = numbers.IndexOfAny(stackalloc[] { 2, 4, 6, 8 });
	Console.WriteLine(ind);  // output: 1


	unsafe
	{
		int length = 3;
		int* numbers = stackalloc int[length];
		for (var i = 0; i < length; i++)
		{
			numbers[i] = i;
		}
	}


	Содержимое только что выделенной памяти не определено. 
	Его следует инициализировать перед использованием. 
	Например, вы можете использовать метод Span<T>.Clear, который задает для всех элементов значение по умолчанию типа T.

	Начиная с версии C# 7.3, вы можете использовать синтаксис инициализатора массива, 
	чтобы определить содержимое для только что выделенной памяти. 
	В следующем примере показано несколько способов сделать это:

	Span<int> first = stackalloc int[3] { 1, 2, 3 };
	Span<int> second = stackalloc int[] { 1, 2, 3 };
	ReadOnlySpan<int> third = stackalloc[] { 1, 2, 3 };
	В выражении stackalloc T[E]T должен иметь неуправляемый тип, а E — неотрицательное значение int.

	Безопасность
	При использовании stackalloc в среде CLR автоматически включается контроль переполнения буфера. Если буфер переполнен, процесс незамедлительно прерывается — это позволяет минимизировать риск исполнения вредоносного кода.

 public static unsafe void Main()
    {
        int[] array = CreateInt32Array();

        // Create a span, pin it, and print its elements.
        Span<int> span = array.AsSpan();
        fixed (int* spanPtr = span)
        {
            Console.WriteLine($"Span contains {span.Length} elements:");
            for (int i = 0; i < span.Length; i++)
            {
                Console.WriteLine(spanPtr[i]);
            }
            Console.WriteLine();
        }

        // Create a read-only span, pin it, and print its elements.
        ReadOnlySpan<int> readonlyspan = array.AsSpan();
        fixed (int* readonlyspanPtr = readonlyspan)
        {
            Console.WriteLine($"ReadOnlySpan contains {readonlyspan.Length} elements:");
            for (int i = 0; i < readonlyspan.Length; i++)
            {
                Console.WriteLine(readonlyspanPtr[i]);
            }
            Console.WriteLine();
        }
    }

    private static int[] CreateInt32Array()
    {
        return new int[] { 100, 200, 300, 400, 500 };
    }



*/
#endregion
#region Unsafe Pointers	*

/*
 * 
unsafe
{
byte[] bytes = { 1, 2, 3 };
fixed (byte* pointerToFirst = bytes)
{
 Console.WriteLine($"The address of the first array element: {(long)pointerToFirst:X}.");
 Console.WriteLine($"The value of the first array element: {*pointerToFirst}.");
}
}
// Output is similar to:
// The address of the first array element: 2173F80B5C8.
// The value of the first array element: 1.



unsafe
{
int[] numbers = { 10, 20, 30 };
fixed (int* toFirst = &numbers[0], toLast = &numbers[^1])
{
 Console.WriteLine(toLast - toFirst);  // output: 2
}
}


unsafe
{
int[] numbers = { 10, 20, 30, 40, 50 };
Span<int> interior = numbers.AsSpan()[1..^1];
fixed (int* p = interior)
{
 for (int i = 0; i < interior.Length; i++)
 {
	 Console.Write(p[i]);
 }
 // output: 203040
}
}


unsafe
{
var message = "Hello!";
fixed (char* p = message)
{
 Console.WriteLine(*p);  // output: H
}
}
*/
#endregion
#region VOLATILE (Multithreading)
/*
	https://docs.microsoft.com/ru-ru/dotnet/csharp/language-reference/keywords/volatile
	public volatile int sharedStorage;
*/
#endregion

#region with
//https://docs.microsoft.com/ru-ru/dotnet/csharp/language-reference/operators/with-expression

#endregion
#region SWITCH

/*
 * 
 object numericValue = Type.GetTypeCode(typeof(T)) switch
				{
					TypeCode.Int16 => Int16.Parse(stringValue, style),
					TypeCode.Int32 => Int32.Parse(stringValue, style),
					TypeCode.Int64 => Int64.Parse(stringValue, style),

					TypeCode.UInt16 => UInt16.Parse(stringValue, style),
					TypeCode.UInt32 => UInt32.Parse(stringValue, style),
					TypeCode.UInt64 => UInt64.Parse(stringValue, style),

					TypeCode.Decimal => Decimal.Parse(stringValue, style),
					TypeCode.Double => Double.Parse(stringValue, style),
					TypeCode.Single => Single.Parse(stringValue, style),

					TypeCode.Byte => Byte.Parse(stringValue, style),

					_ => defaultValue
				};

  var ttt = direction switch
    {
        Direction.Up    => Orientation.North,
        Direction.Right => Orientation.East,
        Direction.Down  => Orientation.South,
        Direction.Left  => Orientation.West,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), $"Not expected direction value: {direction}"),
    }; 

	static Point Transform(Point point) => point switch
	{
		{ X: 0, Y: 0 }                    => new Point(0, 0),
		{ X: var x, Y: var y } when x < y => new Point(x + y, y),
		{ X: var x, Y: var y } when x > y => new Point(x - y, y),
		{ X: var x, Y: var y }            => new Point(2 * x, 2 * y),
	};

	static int GetSourceLabel<T>(IEnumerable<T> source) => source switch
	{
		Array array => 1,
		ICollection<T> collection => 2,
		_ => 3,
	};

	public static decimal CalculateToll(this Vehicle vehicle) => vehicle switch
    {
        Car _ => 2.00m,
        Truck _ => 7.50m,
        null => throw new ArgumentNullException(nameof(vehicle)),
        _ => throw new ArgumentException("Unknown type of a vehicle", nameof(vehicle)),
    };

	public static decimal CalculateToll(this Vehicle vehicle) => vehicle switch
	{
		Car => 2.00m,
		Truck => 7.50m,
		null => throw new ArgumentNullException(nameof(vehicle)),
		_ => throw new ArgumentException("Unknown type of a vehicle", nameof(vehicle)),
	};

	static string Classify(double measurement) => measurement switch
	{
		< -4.0 => "Too low",
		> 10.0 => "Too high",
		double.NaN => "Unknown",
		_ => "Acceptable",
	};

	static string GetCalendarSeason(DateTime date) => date.Month switch
	{
		>= 3 and < 6 => "spring",
		>= 6 and < 9 => "summer",
		>= 9 and < 12 => "autumn",
		12 or (>= 1 and < 3) => "winter",
		_ => throw new ArgumentOutOfRangeException(nameof(date), $"Date with unexpected month: {date.Month}."),
	};

	static string GetCalendarSeason(DateTime date) => date.Month switch
	{
		3 or 4 or 5 => "spring",
		6 or 7 or 8 => "summer",
		9 or 10 or 11 => "autumn",
		12 or 1 or 2 => "winter",
		_ => throw new ArgumentOutOfRangeException(nameof(date), $"Date with unexpected month: {date.Month}."),
	};

	static string TakeFive(object input) => input switch
	{
		string { Length: >= 5 } s => s.Substring(0, 5),
		string s => s,

		ICollection<char> { Count: >= 5 } symbols => new string(symbols.Take(5).ToArray()),
		ICollection<char> symbols => new string(symbols.ToArray()),

		null => throw new ArgumentNullException(nameof(input)),
		_ => throw new ArgumentException("Not supported input type."),
	};


	balance += record switch
	{
		[_, "DEPOSIT", _, var amount]     => decimal.Parse(amount),
		[_, "WITHDRAWAL", .., var amount] => -decimal.Parse(amount),
		[_, "INTEREST", var amount]       => decimal.Parse(amount),
		[_, "FEE", var fee]               => decimal.Parse(fee),
		_                                 => throw new InvalidOperationException($"Record {record} is not in the expected format!"),
	};
	Console.WriteLine($"Record: {record}, New balance: {balance:C}");
 */
#endregion
#region Шаблоны списков c# 11
/*	
 *	https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching

Начиная с C# 11, можно сопоставить массив или список с последовательностью шаблонов, соответствующих элементам. Можно применить любой из следующих шаблонов:

Любой шаблон можно применить к любому элементу, чтобы убедиться, что отдельный элемент соответствует определенным характеристикам.
Шаблон отмены (_) соответствует одному элементу.
Шаблон диапазона (..) может соответствовать нулю или нескольким элементам последовательности. В шаблоне списка допускается не более одного шаблона диапазона.
Шаблон var может захватывать один элемент или диапазон элементов.
Эти правила демонстрируются с помощью следующих объявлений массива:


int[] one = { 1 };
int[] odd = { 1, 3, 5 };
int[] even = { 2, 4, 6 };
int[] fib = { 1, 1, 2, 3, 5 };

Можно сопоставить всю последовательность, указав все элементы и используя значения:
Console.WriteLine(odd is [1, 3, 5]); // true
Console.WriteLine(even is [1, 3, 5]); // false (values)
Console.WriteLine(one is [1, 3, 5]); // false (length)

Некоторые элементы можно сопоставить в последовательности известной длины с помощью шаблона отмены (_) в качестве заполнителя:
Console.WriteLine(odd is [1, _, _]); // true
Console.WriteLine(odd is [_, 3, _]); // true
Console.WriteLine(even is [_, _, 5]); // false (last value)

Вы можете указать любое количество значений или заполнителей в любом месте последовательности. Если вы не обеспокоены длиной, можно использовать шаблон диапазона , чтобы соответствовать нулю или нескольким элементам:
Console.WriteLine(odd is [1, .., 3, _]); // true
Console.WriteLine(fib is [1, .., 3, _]); // true
Console.WriteLine(odd is [1, _, 5, ..]); // true
Console.WriteLine(fib is [1, _, 5, ..]); // false

В предыдущих примерах использовался шаблон константы , чтобы определить, является ли элемент заданным числом. Любой из этих шаблонов можно заменить другим шаблоном, например реляционным шаблоном:
Console.WriteLine(odd is [_, >1, ..]); // true
Console.WriteLine(even is [_, >1, ..]); // true
Console.WriteLine(fib is [_, > 1, ..]); // false

Шаблоны списков являются ценным инструментом, если данные не соответствуют обычной структуре. Сопоставление шаблонов можно использовать для проверки фигуры и значений данных вместо преобразования их в набор объектов.
Рассмотрим следующий фрагмент текстового файла, содержащего банковские транзакции:

Выходные данные
04-01-2020, DEPOSIT,    Initial deposit,            2250.00
04-15-2020, DEPOSIT,    Refund,                      125.65
04-18-2020, DEPOSIT,    Paycheck,                    825.65
04-22-2020, WITHDRAWAL, Debit,           Groceries,  255.73
05-01-2020, WITHDRAWAL, #1102,           Rent, apt, 2100.00
05-02-2020, INTEREST,                                  0.65
05-07-2020, WITHDRAWAL, Debit,           Movies,      12.57
04-15-2020, FEE,                                       5.55
Это формат CSV, но некоторые строки имеют больше столбцов, чем другие. Еще хуже для обработки один столбец в типе WITHDRAWAL содержит текст, созданный пользователем, и может содержать запятую в тексте. Шаблон списка, включающий шаблон отмены, шаблон констант и шаблон var для записи данных о значении в следующем формате:

decimal balance = 0m;
foreach (var record in ReadRecords())
{
	balance += record switch
	{
		[_, "DEPOSIT", _, var amount]     => decimal.Parse(amount),
		[_, "WITHDRAWAL", .., var amount] => -decimal.Parse(amount),
		[_, "INTEREST", var amount]       => decimal.Parse(amount),
		[_, "FEE", var fee]               => decimal.Parse(fee),
		_                                 => throw new InvalidOperationException($"Record {record} is not in the expected format!"),
	};
	Console.WriteLine($"Record: {record}, New balance: {balance:C}");
}
В предыдущем примере принимается строковый массив, где каждый элемент является одним полем в строке. Ключи выражения switch во втором поле, определяющее тип транзакции и количество оставшихся столбцов. Каждая строка гарантирует, что данные заданы в правильном формате. Шаблон отмены (_) пропускает первое поле с датой транзакции. Второе поле соответствует типу транзакции. Оставшиеся совпадения элементов пропускают поле с суммой. Последнее совпадение использует шаблон var для записи строкового представления суммы. Выражение вычисляет сумму для добавления или вычитания из баланса.
Шаблоны списка позволяют сопоставлять фигуру последовательности элементов данных. 
Вы используете шаблоны отмены и диапазона для сопоставления расположения элементов. 
Любой другой шаблон соответствует характеристикам отдельных элементов.

*/
#endregion

#region EventHandler
/*
	public event EventHandler<string> LineRead = delegate { };	
*/
#endregion

#region YIELD - In IEnumerations RETURN
/*
	public static IEnumerable<int> AllIndexesOf(this string str, string searchstring)
	{
		int minIndex = str.IndexOf(searchstring);
		while (minIndex != -1)
		{
			yield return minIndex;
			minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
		}
	}
*/
#endregion
#region CATCH WHEN
/*
	var client = new HttpClient();
	var streamTask = client.GetStringAsync("https://localHost:10000");
	try
	{
		var responseText = await streamTask;
		return responseText;
	}
	catch (HttpRequestException e) when (e.Message.Contains("301"))
	{
		return "Site Moved";
	}
	catch (HttpRequestException e) when (e.Message.Contains("404"))
	{
		return "Page Not Found";
	}
	catch		(Exception ex) when(ex is FormatException || ex is OverflowException)
	{
	}
	catch (HttpRequestException e)
	{
		return e.Message;
	} 
*/
#endregion
#region List Ranges And Indexes https://docs.microsoft.com/ru-ru/dotnet/csharp/whats-new/tutorials/ranges-indexes
/* 
	string[] words = new string[]
	{                // index from start    index from end
		"The",      // 0                   ^9
		"quick",    // 1                   ^8
		"brown",    // 2                   ^7
		"fox",      // 3                   ^6
		"jumped",   // 4                   ^5
		"over",     // 5                   ^4
		"the",      // 6                   ^3
		"lazy",     // 7                   ^2
		"dog"       // 8                   ^1
	};              // 9 (or words.Length) ^0
	Console.WriteLine($"The last word is {words[^1]}");
*/
#endregion

#region LINQ GroupBy Sample
/*
*** VB
Dim lExtList = (From FI In Me._lTotalFoundFilesOnDisk
	Group By FileExt = FI.Extension.ToLower Into Files = Group, Count()
	Order By FileExt).ToList


*** C#

			from ri in rx.Matches(file).Select(m => MacRecordInfo.Parse(m))
				group ri by ri.MAC into grp
				orderby grp.Key
				select new
				{
					MAC = grp.Key,
					InfoList = from p in grp select p.Info
				};



var workBooks =
					from s in Sources.Cast<SourceOfficeDocument>()
					group s by s.File.FullName.ToLower() into g
					select new
					{
						file = g.Key,
						workbooks = from p in g select p
					};

var groupByLastNamesQuery =
	from student in students
	group student by student.LastName into newGroup
	orderby newGroup.Key
	select newGroup;

foreach (var nameGroup in groupByLastNamesQuery)
{
	Console.WriteLine($"Key: {nameGroup.Key}");
	foreach (var student in nameGroup)
	{
		Console.WriteLine($"\t{student.LastName}, {student.FirstName}");
	}
}

 */


//Also see .ToLookup() for any lists
//https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.tolookup?view=netframework-4.8&f1url=%3FappId%3DDev16IDEF1%26l%3DEN-US%26k%3Dk(System.Linq.Enumerable.ToLookup%60%602);k(TargetFrameworkMoniker-.NETFramework,Version%253Dv4.8);k(DevLang-csharp)%26rd%3Dtrue
#endregion
#region P-LINQ With Cancelation

/*
//https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-speed-up-small-loop-bodies
			

//How to: Speed Up Small Loop Bodies
			//https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/understanding-speedup-in-plinq
 


  static void Main()
        {
            int[] source = Enumerable.Range(1, 10000000).ToArray();
            using CancellationTokenSource cts = new();

            // Start a new asynchronous task that will cancel the
            // operation from another thread. Typically you would call
            // Cancel() in response to a button click or some other
            // user interface event.
            Task.Factory.StartNew(() =>
            {
                UserClicksTheCancelButton(cts);
            });

            int[] results = null;
            try
            {
                results =
                    (from num in source.AsParallel().WithCancellation(cts.Token)
                     where num % 3 == 0
                     orderby num descending
                     select num).ToArray();
            }
            catch (OperationCanceledException e)
            {
                WriteLine(e.Message);
            }
            catch (AggregateException ae)
            {
                if (ae.InnerExceptions != null)
                {
                    foreach (Exception e in ae.InnerExceptions)
                    {
                        WriteLine(e.Message);
                    }
                }
            }

            foreach (var item in results ?? Array.Empty<int>())
            {
                WriteLine(item);
            }
            WriteLine();
            ReadKey();
        }

        static void UserClicksTheCancelButton(CancellationTokenSource cts)
        {
            // Wait between 150 and 500 ms, then cancel.
            // Adjust these values if necessary to make
            // cancellation fire while query is still executing.
            Random rand = new();
            Thread.Sleep(rand.Next(150, 500));
            cts.Cancel();
        }
 */
#endregion
#region SIMD, a parallel processing at hardware level in C#.
/*
 
	//using System.Runtime.InteropservicesMemoryMarshal.Cast. 
	allow us to map data from one type to another, without actually copying bytes around.
   
	public void SimpleSumVectorsNoCopy()
        {
            int numVectors = left.Length / floatSlots;
            int ceiling = numVectors * floatSlots;

            ReadOnlySpan<Vector<float>> leftVecArray = MemoryMarshal.Cast<float, Vector<float>>(leftMemory.Span);
            ReadOnlySpan<Vector<float>> rightVecArray = MemoryMarshal.Cast<float, Vector<float>>(rightMemory.Span);
            Span<Vector<float>> resultsVecArray = MemoryMarshal.Cast<float, Vector<float>>(resultsMemory.Span);

            for (int i = 0; i < numVectors; i++)
            {
                resultsVecArray[i] = leftVecArray[i] + rightVecArray[i];
            }
            // Finish operation with any numbers leftover
            for (int i = ceiling; i < left.Length; i++)
            {
                results[i] = left[i] + right[i];
            }
        }







//https://github.com/CBGonzalez/SIMDPerformance




 private static void TestSIMD()
		{

			int[] arr = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
			var vectorSize = Vector<int>.Count;
			var vec = new Vector<int>(arr, 3);  
		}



//https://dev.to/mstbardia/simd-a-parallel-processing-at-hardware-level-in-c-42p4
 [MemoryDiagnoser]
public class Counter
{
    private readonly int[] _left;
    private readonly int[] _right;

    public Counter()
    {
        _left = Faker.BuildArray(10000);
        _right = Faker.BuildArray(10000);
    }

    [Benchmark]
    public int[] VectorSum()
    {
        var vectorSize = Vector<int>.Count;
        var result = new Int32[_left.Length];
        for (int i = 0; i < _left.Length ; i += vectorSize)
        {
            var v1 = new Vector<int>(_left, i);
            var v2 = new Vector<int>(_right, i);
            (v1 + v2).CopyTo(result, i);
        }
        return result;
    }

    [Benchmark]
    public int[] LinQSum()
    {
        var result = _left.Zip(_right, (l, r) => l + r).ToArray();
        return result;
    }

    [Benchmark]
    public int[] ForSum()
    {
        var result = new Int32[_left.Length];  
        for (int i = 0; i <= _left.Length - 1; i++)
        {
            result[i] = _left[i] + _right[i];
        }
        return result;
    }
}

public static class Faker
{
    public static int[] BuildArray(int length)
    {
        var list = new List<int>();
        var rnd = new Random(DateTime.Now.Millisecond);     
        for (int i = 1; i <= length; i++)
        {
            list.Add(rnd.Next(1,99));
        }
        return list.ToArray();
    }
}



 */
#endregion

#region RegEx Samples
/*
  


[GeneratedRegex("(dog|cat|fish)")]
partial bool IsPetMatch(string input);




 * 
	private static readonly Regex SpacesPattern = new(@"\s");
	var v = SpacesPattern.Replace(value, match => string.Empty);

	private static readonly Regex ExponentPattern = new(@"[-+]?\d*\.?\d+[eE][-+]?\d+");
	if (ExponentPattern.IsMatch(v))    return new NumericParseResult(v.TryParseExponent());

	private static readonly Regex DecimalPattern = new(@"[\d\.\,\s]+");
	if (!DecimalPattern.IsMatch(value))    return null;//Doesn't look like a number at all.

	^\[(?<PropKey>.+)\]\:\s\[(?<PropValue>.+)(?:\r\n)*\]

	Private Shared ReadOnly _REO AsRegexOptions = C_REGEXP_FLAGS_IgnoreCase_IgnorePatternWhitespace_Singleline
	Private Shared ReadOnly _RegExp_SRPLogRow As string = My.Resources.SAFER_RegExp_SRPLogRow
	Private Shared ReadOnly _RegExp As NewRegex(_RegExp_SRPLogRow, _REO)

	Dim rMatch = _RegExp.Match(sLogFileRow)
	With rMatch
	If (rMatch.Success) Then
	Dim aGroups = _RegExp.GetGroupNames
	Dim rGroups = rMatch.Groups

	Me.sCallerProcess = rGroups!CallerProcess.Value
	Me.PID = rGroups!PID.Value
	Me.sIndentification = rGroups!Indentification.Value
	Me.EXEFile = rGroups!ExePath.Value
	Me.ActionText = rGroups!Action.Value
	Me.sRuleType = rGroups!RuleType.Value
	Dim sRuleGUID = rGroups!RuleGUID.Value
	Me.RuleGUID = New Guid(sRuleGUID)
	Else
	Throw New NotSupportedException("Не удалось разобрать строку журнала: " & sLogFileRow)
	End If
	End With
*/
#endregion
#region ASYNC_AWAIT SAMPLE
/*
	IAsyncResult rAsincRslt = psInstance.BeginInvoke();
	void waitAsyncFinished() { while (rAsincRslt.IsCompleted == false) Thread.Sleep(500); }
	await ((Action)waitAsyncFinished).eStartAndWaitAsync(true);
	PSDataCollection<PSObject> psResult = psInstance.EndInvoke(rAsincRslt);

	**** AWAIT with IAsyncResult 
	var result = await Task.Factory.FromAsync(psInstance.BeginInvoke(), psInstance.EndInvoke); 

 * 

When yo do not need any task to run, just return
Task.CompletedTask;
or
Task.FromResult<TResult>(TResult)




private static async Task<int> Sampe1()
{
	var results = await (new Func<int>(() =>
	{
		Thread.Sleep(1000);
		return 5;
	})).eRunAsync();
	return results;
}

 Public Sub Main()
 Dim results = Task.WhenAll(SlowCalculation, AnotherSlowCalculation).Result
 For Each result In results
 Console.WriteLine(result)
 Next
 End Sub


private static async Task<int> SlowCalculation()
{
	await Task.Delay(2000); return 40;
}
private static async Task<int> AnotherSlowCalculation()
{
	await Task.Delay(2000); return 20;
}


// TASK(OF T) EXAMPLE
// Async Function TaskOfT_MethodAsync() As Task(Of Integer)

// Task.FromResult Не запускает задачу, а лишь оборачивает резуоттат в обёртку Task(of XX)
// Dim today As string = Await Task.FromResult(Of string)(DateTime.Now.DayOfWeek.ToString())

// The method then can process the result in some way.
// Dim leisureHours As Integer
// If today.First() = "S" Then
// leisureHours = 16
// Else
// leisureHours = 5
// End If

// ' Because the return statement specifies an operand of type Integer, the
// ' method must have a return type of Task(Of Integer).
// Return leisureHours
// End Function



// Public Sub Main()
// Dim tasks = Enumerable.Range(0, 100).Select(AddressOf TurnSlowlyIntegerIntoString)

// Dim resultingStrings = Task.WhenAll(tasks).Result

// For Each value In resultingStrings
// Console.WriteLine(value)
// Next
// End Sub
// Async Function TurnSlowlyIntegerIntoString(input As Integer) As Task(Of string)
// Await Task.Delay(2000)

// Return input.ToString()
// End Function



#region FUNCTION SAMPLE

// Public Class Form1
// Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
// ' ExampleMethodAsync returns a Task.
// Await ExampleMethodAsync()
// TextBox1.Text = vbCrLf & "Control returned to button1_Click."
// End Sub

// Async Function ExampleMethodAsync() As Task
// ' The following line simulates a task-returning asynchronous process.
// Await Task.Delay(1000)
// End Function
// End Class


// Public Class Form1
// Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
// AddHandler Button1.Click,
// Async Sub(sender1, e1)
// ' ExampleMethodAsync returns a Task.
// Await ExampleMethodAsync()
// TextBox1.Text = vbCrLf & "Control returned to Button1_ Click."
// End Sub
// End Sub
// Async Function ExampleMethodAsync() As Task
// ' The following line simulates a task-returning asynchronous process.
// Await Task.Delay(1000)
// End Function
// End Class

// Private Async Sub _Click(sender As Object, e As EventArgs) Handles Button1.Click
// Полный вариант
// 'Dim tsk As New Task(Of Integer)(AddressOf func_for_task)
// 'Call tsk.Start()
// 'Dim val = Await tsk
// 'MsgBox(val)

// Сокращённый вариант
// Dim val = Await Task(Of Integer).Factory.StartNew(AddressOf func_for_task)
// MsgBox(val)
// End Sub

// Private Function func_for_task() As Integer
// Dim val As Integer
// For i = 1 To 999999999
// val += 1
// Next
// Return val
// End Function


// ----------------------------

// Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
// ' Call the method that runs asynchronously.
// Dim result As string = Await WaitAsynchronouslyAsync()

// ' Call the method that runs synchronously.
// 'Dim result As string = Await WaitSynchronously()

// ' Display the result.
// TextBox1.Text &= result
// End Sub

// ' The following method runs asynchronously. The UI thread is not
// ' blocked during the delay. You can move or resize the Form1 window 
// ' while Task.Delay is running.
// Public Async Function WaitAsynchronouslyAsync() As Task(Of string)
// Await Task.Delay(10000)
// Return "Finished"
// End Function

// ' The following method runs synchronously, despite the use of Async.
// ' You cannot move or resize the Form1 window while Thread.Sleep
// ' is running because the UI thread is blocked.
// Public Async Function WaitSynchronously() As Task(Of string)
// ' Import System.Threading for the Sleep method.
// Thread.Sleep(10000)
// Return "Finished"
// End Function



#endregion

#region SUB SAMPLE

// Public Async Function LoadProductsListFromWSUS(rWSUS As IUpdateServer, rWSUSSubscription As ISubscription) As Task
// 'Call DEBUG_SHOW_LINE("Start Loading Products...")

// *** ВАРИАНТ 1
// Using tskAsyncTask As New Task(Sub()
// _Products = WsusProductFamily.BuildProductsTree(rWSUS, rWSUSSubscription)
// 'Global.System.Threading.Thread.Sleep(10000)
// End Sub, TaskCreationOptions.LongRunning)
// Call tskAsyncTask.Start()
// Await tskAsyncTask
// End Using
// 'Call DEBUG_SHOW_LINE("LOADED!")

// *** ВАРИАНТ 2
// 'Call DEBUG_SHOW_LINE("Start Loading Products...")
// Await Task.Run(Sub()
// _Products = WsusProductFamily.BuildProductsTree(rWSUS, rWSUSSubscription)
// 'Global.System.Threading.Thread.Sleep(10000)
// End Sub)
// 'Call DEBUG_SHOW_LINE("LOADED!")
// End Function



// Private Async Sub StartAsyncTask()
// Полный вариант
// Dim tsk As New Task(AddressOf AsyncTaskCallBack)
// tsk.Start()
// MsgBox("Started")
// Await tsk
// MsgBox("Finished")

// Краткий вариант
// MsgBox("Started")
// 'Await Task.Factory.StartNew(AddressOf TTT)
// MsgBox("Finished")

// End Sub

// Private Sub AsyncTaskCallBack()
// Dim val As Integer
// For i = 1 To 999999999
// val += 1
// Next
// End Sub
#endregion


//spinner.IsBusy = true;
//try
//{
//  Task t1 = Task.Run(() => dataX = loadX());
//  Task t2 = Task.Run(() => dataY = loadY());
//  Task t3 = Task.Run(() => dataZ = loadZ());
//  await Task.WhenAll(t1, t2, t3);
//}
//finally
//{
//    spinner.IsBusy = false;
//}


//private SemaphoreSlim _mutex = new SemaphoreSlim(5);
//private HttpClient _client = new HttpClient();
//private async Task<string> DownloadStringAsyncddd(string url)
//{
//    await _mutex.TakeAsync();
//    try
//    {
//        return await _client.GetStringAsync(url);
//    }
//    finally
//    {
//        _mutex.Release();
//    }
//}

//IEnumerable<string> urls = ...;
//var data = await Task.WhenAll(urls.Select(url => DownloadStringAsync(url));





#endregion

#region Use of cancellationToken for async

// Public Async Task StartTimer(CancellationToken cancellationToken)
// {

// await Task.Run(async () =>
// {
// While (True)
// {
// DoSomething();
// await Task.Delay(10000, cancellationToken);
// If (cancellationToken.IsCancellationRequested)
// break;
// }
// });

// }
// When you want to stop the thread just abort the token
// cancellationToken.Cancel();
*/
#endregion
#region CancellationToken
/*
 		https://learn.microsoft.com/ru-ru/dotnet/api/system.threading.cancellationtoken.-ctor?view=net-7.0
			CancellationTokenSource source = new CancellationTokenSource();
			CancellationToken token = source.Token;
			source.Cancel();

 */
#endregion

/*
 
 class Program
{
    static async Task Main(string[] args)
    {
        // Add this to your C# console app's Main method to give yourself
        // a CancellationToken that is canceled when the user hits Ctrl+C.
        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (s, e) =>
        {
            Console.WriteLine("Canceling...");
            cts.Cancel();
            e.Cancel = true;
        };
        
        await MainAsync(args, cts.Token);
    }
  private static async Task MainAsync(string[] args, CancellationToken cancellationToken)
    {
        try
        {
            // code using the cancellation token
            Console.WriteLine("Waiting");
            await Task.Delay(10_000, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Operation canceled");
        }
    }
}
 
 */

#endregion


// <summary>Commnon Tools For Net Multiplatform (C) UOM 2000-2024 </summary>
namespace uom
{


	/// <summary>Constants</summary>
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
	internal static partial class constants
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
	{

		internal const char vbNullChar = '\0';
		internal const char vbCr = '\r';
		internal const char vbLf = '\n';
		internal const char vbTab = '\t';

		internal const string vbCrLf = "\r\n";
		internal const string vbCrLf2 = vbCrLf + vbCrLf;
		internal const string vbCrCrLf = "\r" + vbCrLf;

		internal const char CC_NULL = '\0';
		internal const char CC_EQUAL = '=';
		internal const char CC_SLASH = '\\';
		internal const char CC_SPACE = ' ';
		internal const char CC_QUOTE = '"';

		internal const string CS_SLASH_SLASH = "\\\\";
		internal const string CS_SEPARATOR_10 = "==========";
		internal const string CS_CONSOLE_SEPARATOR = CS_SEPARATOR_10 + CS_SEPARATOR_10 + CS_SEPARATOR_10 + CS_SEPARATOR_10 + CS_SEPARATOR_10 + CS_SEPARATOR_10 + CS_SEPARATOR_10;
		internal const string CS_ALPHABET_EN = "abcdefghijklmnopqrstuvwxyz";
		internal const string CS_ALPHABET_RU = "абвгдежзийклмнопрстуфхцчшщъыьэюя";
		internal const string CS_ALPHABET_DIGITS = "1234567890";
		internal const string CS_QUOTE = "\"";


		internal static readonly string CS_QUOTE2 = QUOTE_X (2);
		internal static readonly string CS_QUOTE4 = QUOTE_X (4);
		internal static string QUOTE_X ( int X ) => new (CC_QUOTE, X);


		internal const string C_FAILED_TO_RUN_RUS = "Не удалось выполнить!";
		internal const string C_FAILED_TO_RUN_ENG = "Failed to execute!";
		internal const string C_FAILED_TO_RUN = C_FAILED_TO_RUN_ENG;

		internal const string C_FMT_LONGNUMBER = "### ### ### ### ### ### ### ### ### ### ### ### ### ### ##0";
		/// <summary>Used in any formating functions for format decimal numbers</summary>
		internal const int C_DEFAULT_DECIMAL_DIGITS = 2;

		internal static readonly IntPtr HANDLE_INVALID = new (-1);



		internal const string CS_NONE_RU = "[нет]";
		internal const string CS_READY_RU = "Готово.";
		internal const string CS_YES_EN = "Yes";
		internal const string CS_YES_RU = "Да";
		internal const string CS_NO_EN = "No";
		internal const string CS_NO_RU = "Нет";

		public enum CHAR_MODE : int
		{
			Auto = 0,
			Uni,
			Ansi
		}


		internal const float C_MM_IN_INCH = 25.4f;
		internal const float C_CM_IN_INCH = C_MM_IN_INCH / 10f;


		#region Console Const


		#region ASCII Pseudorgaphics
		//https://www.asciitable.com/
		/// <summary>░</summary>
		internal const char CC_ASCII_176 = '░';
		/// <summary>▒</summary>
		internal const char CC_ASCII_177 = '▒';
		/// <summary>▓</summary>
		internal const char CC_ASCII_178 = '▓';
		/// <summary>█</summary>
		internal const char CC_ASCII_219 = '█';
		#endregion


		internal const char CC_PROGRESSBAR_EMPTY = '_';
		internal const char CC_PROGRESSBAR_FULL = '#';

		/// <summary>░</summary>
		internal const char CC_ASCII_PROGRESSBAR_EMPTY = CC_ASCII_176;
		/// <summary>▓</summary>
		internal const char CC_ASCII_PROGRESSBAR_FULL = CC_ASCII_178;

		internal const string CS_ENTER_PWD_EN = "Enter password: ";
		internal const int C_DEFAULT_CONSOLE_WIDTH = 80;
		internal const int C_DEFAULT_CONSOLE_WIDTH_1 = C_DEFAULT_CONSOLE_WIDTH - 1;

		#endregion


		/// <summary>Standart system byte seperator char</summary>
		internal static readonly Lazy<char> SystemDefaultHexByteSeparator = new (BitConverter.ToString ([ 0x1, 0x2 ])[ 2 ]);


		// Поиск в многострочном тексте
		internal const RegexOptions C_REGEXP_FLAGS_IgnoreCase_IgnorePatternWhitespace_Singleline = RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.CultureInvariant;

		// Поиск в многострочном тексте, в каждой строке по-отдельности
		internal const RegexOptions C_REGEXP_FLAGS_IgnoreCase_IgnorePatternWhitespace_Multiline = RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.CultureInvariant;

		// Поиск в многострочном тексте, и в каждой строке по-отдельности
		internal const RegexOptions C_REGEXP_FLAGS_IgnoreCase_IgnorePatternWhitespace_Multiline_Singleline = RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.CultureInvariant;


		//[MethodImpl(MethodImplOptions.AggressiveInlining)]
		//public static UOM.Globalization_.mGlobalization.CountryPhoneFormatAttribute ExtGlb_GetCountryPhoneFormat(this UOM.Globalization_.mGlobalization.ECounties CNT)
		//{
		//    return UOM.Globalization_.mGlobalization.CountryPhoneFormatAttribute.GetCountryPhoneFormat(CNT);
		//}

		///// <summary>Выделяет из строки номер телефона</summary>
		//[MethodImpl(MethodImplOptions.AggressiveInlining)]
		//public static string ExtGlb_PhoneParse(this string sPhone, UOM.Globalization_.mGlobalization.ECounties CNT = UOM.Globalization_.mGlobalization.ECounties._UNKNOWN)
		//{
		//    return UOM.Globalization_.mGlobalization.PhoneParse(sPhone, CNT);
		//}




	}


	#region Structures _Int16/32/64

	[StructLayout (LayoutKind.Explicit, Pack = 1)]
	internal partial struct _Int16
	{
		[FieldOffset (0)][MarshalAs (UnmanagedType.I4)] public short Word = 0;
		[FieldOffset (0)][MarshalAs (UnmanagedType.U4)] public ushort UWord = 0;
		[FieldOffset (0)][MarshalAs (UnmanagedType.U1)] public byte LoByte = 0;
		[FieldOffset (1)][MarshalAs (UnmanagedType.U1)] public byte HiByte = 0;
		[FieldOffset (0)][MarshalAs (UnmanagedType.U1)] public byte Byte_0 = 0;
		[FieldOffset (1)][MarshalAs (UnmanagedType.U1)] public byte Byte_1 = 0;

		#region CORE

		public _Int16 ( short V ) => Word = V;
		public _Int16 ( ushort V ) => UWord = V;

		public byte[] Bytes
		{
			get => new byte[] { Byte_0, Byte_1 };
			set
			{
				if (value.Length != 2)
				{
					throw new ArgumentException ($"{nameof (_Int16)} Constructor failed! byteArray lenght is wrong!");
				}

				(Byte_0, Byte_1) = (value[ 0 ], value[ 1 ]);
			}
		}

		#endregion

		public override string ToString () => $"{UWord} ({Bytes.eToStringHex (true)})";

		#region Conversions

		public static implicit operator ushort ( _Int16 L ) => L.UWord;
		public static implicit operator short ( _Int16 L ) => L.Word;
		public static implicit operator _Int16 ( ushort L ) => new (L);
		public static implicit operator _Int16 ( short L ) => new (L);
		public static implicit operator _Int16 ( byte I ) => new (0) { LoByte = I };
		public static implicit operator _Int16 ( byte[] ab ) => new () { Bytes = ab };

		#endregion

		#region Operators
		public static bool operator < ( _Int16 I1, _Int16 I2 ) => I1.UWord < I2.UWord;
		public static bool operator > ( _Int16 I1, _Int16 I2 ) => I1.UWord > I2.UWord;
		public static bool operator == ( _Int16 I1, _Int16 I2 ) => I1.UWord == I2.UWord;
		public static bool operator != ( _Int16 I1, _Int16 I2 ) => I1.UWord != I2.UWord;
		public static bool operator < ( _Int16 I1, ushort I2 ) => I1.UWord < I2;
		public static bool operator > ( _Int16 I1, ushort I2 ) => I1.UWord > I2;
		public static bool operator == ( _Int16 I1, ushort I2 ) => I1.UWord == I2;
		public static bool operator != ( _Int16 I1, ushort I2 ) => I1.UWord != I2;
		public static _Int16 operator - ( _Int16 I1, _Int16 I2 ) => new ((ushort) ( I1.UWord - I2.UWord ));
		public static _Int16 operator - ( _Int16 I1, ushort I2 ) => new ((ushort) ( I1.UWord - I2 ));
		public static _Int16 operator + ( _Int16 I1, _Int16 I2 ) => new ((ushort) ( I1.UWord + I2.UWord ));
		public static _Int16 operator + ( _Int16 I1, ushort I2 ) => new ((ushort) ( I1.UWord + I2 ));
		#endregion

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
		public override readonly bool Equals ( object obj ) => ( this == (_Int16) obj );
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

		public override int GetHashCode () => UWord.GetHashCode ();
	}

	[StructLayout (LayoutKind.Explicit, Pack = 1)]
	internal partial struct _Int32
	{
		[FieldOffset (0)][MarshalAs (UnmanagedType.I4)] public int DWord = 0;
		[FieldOffset (0)][MarshalAs (UnmanagedType.U4)] public uint UDWord = 0;
		[FieldOffset (0)][MarshalAs (UnmanagedType.I2)] public short LoWord = 0;
		[FieldOffset (0)][MarshalAs (UnmanagedType.U2)] public ushort ULoWord = 0;
		[FieldOffset (2)][MarshalAs (UnmanagedType.I2)] public short HiWord = 0;
		[FieldOffset (2)][MarshalAs (UnmanagedType.U2)] public ushort UHiWord = 0;
		[FieldOffset (0)][MarshalAs (UnmanagedType.LPStruct)] public _Int16 LoWord16 = new (0);
		[FieldOffset (2)][MarshalAs (UnmanagedType.LPStruct)] public _Int16 HiWord16 = new (0);
		[FieldOffset (0)][MarshalAs (UnmanagedType.U1)] public byte Byte_0 = 0;
		[FieldOffset (1)][MarshalAs (UnmanagedType.U1)] public byte Byte_1 = 0;
		[FieldOffset (2)][MarshalAs (UnmanagedType.U1)] public byte Byte_2 = 0;
		[FieldOffset (3)][MarshalAs (UnmanagedType.U1)] public byte Byte_3 = 0;

#if NET
		//[FieldOffset(0)][MarshalAs(UnmanagedType.U1)] public unsafe fixed byte BytesUnsafe[4];
#endif

		#region CORE

		public _Int32 ( int V ) { DWord = V; }
		public _Int32 ( uint V ) { UDWord = V; }


		public byte[] Bytes
		{
			get => new byte[] { Byte_0, Byte_1, Byte_2, Byte_3 };
			set
			{
				if (value.Length != 4)
				{
					throw new ArgumentException ($"{nameof (_Int32)} Constructor failed! byteArray lenght is wrong!");
				}

				(Byte_0, Byte_1, Byte_2, Byte_3) = (value[ 0 ], value[ 1 ], value[ 2 ], value[ 3 ]);
			}
		}

#if NET

		/*

		Memory<byte> ToMemory()
		{
			var mm = new Memory<byte>(BytesUnsafe);
			return mm;
		}
		 */
#endif

		#endregion
		public override string ToString () => ( UDWord.ToString () + " / " + Bytes.eToStringHex (true) );

		#region Conversions
		public static implicit operator uint ( _Int32 L ) => L.UDWord;
		public static implicit operator int ( _Int32 L ) => L.DWord;

		public static implicit operator _Int32 ( uint L ) => new (L);
		public static implicit operator _Int32 ( int L ) => new (L);
		public static implicit operator _Int32 ( short I ) => new (0) { LoWord = I };
		public static implicit operator _Int32 ( ushort I ) => new (0) { ULoWord = I };
		public static implicit operator _Int32 ( byte[] ab ) => new () { Bytes = ab };
		#endregion

		#region Operators
		public static bool operator < ( _Int32 I1, _Int32 I2 ) => I1.UDWord < I2.UDWord;
		public static bool operator > ( _Int32 I1, _Int32 I2 ) => I1.UDWord > I2.UDWord;
		public static bool operator == ( _Int32 I1, _Int32 I2 ) => I1.UDWord == I2.UDWord;
		public static bool operator != ( _Int32 I1, _Int32 I2 ) => I1.UDWord != I2.UDWord;
		public static bool operator < ( _Int32 I1, uint I2 ) => I1.UDWord < I2;
		public static bool operator > ( _Int32 I1, uint I2 ) => I1.UDWord > I2;
		public static bool operator == ( _Int32 I1, uint I2 ) => I1.UDWord == I2;
		public static bool operator != ( _Int32 I1, uint I2 ) => I1.UDWord != I2;
		public static _Int32 operator - ( _Int32 I1, _Int32 I2 ) => new (I1.UDWord - I2.UDWord);
		public static _Int32 operator - ( _Int32 I1, uint I2 ) => new (I1.UDWord - I2);
		public static _Int32 operator + ( _Int32 I1, _Int32 I2 ) => new (I1.UDWord + I2.UDWord);
		public static _Int32 operator + ( _Int32 I1, uint I2 ) => new (I1.UDWord + I2);
		#endregion

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
		public override bool Equals ( object obj ) => ( this == (_Int32) obj );
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

		public override int GetHashCode () => UDWord.GetHashCode ();
	}

	[StructLayout (LayoutKind.Explicit, Pack = 1)]
	internal partial struct _Int64
	{
		[FieldOffset (0)][MarshalAs (UnmanagedType.I8)] public long QWord = 0;
		[FieldOffset (0)][MarshalAs (UnmanagedType.U8)] public ulong UQWord = 0;
		[FieldOffset (0)][MarshalAs (UnmanagedType.I4)] public int LoDWord = 0;
		[FieldOffset (4)][MarshalAs (UnmanagedType.I4)] public int HiDWord = 0;
		[FieldOffset (0)][MarshalAs (UnmanagedType.U4)] public uint ULoDWord = 0;
		[FieldOffset (4)][MarshalAs (UnmanagedType.U4)] public uint UHiDWord = 0;
		[FieldOffset (0)][MarshalAs (UnmanagedType.LPStruct)] public _Int32 LoDWord32 = new (0);
		[FieldOffset (4)][MarshalAs (UnmanagedType.LPStruct)] public _Int32 HiDWord32 = new (0);
		[FieldOffset (0)][MarshalAs (UnmanagedType.U1)] public byte Byte_0 = 0;
		[FieldOffset (1)][MarshalAs (UnmanagedType.U1)] public byte Byte_1 = 0;
		[FieldOffset (2)][MarshalAs (UnmanagedType.U1)] public byte Byte_2 = 0;
		[FieldOffset (3)][MarshalAs (UnmanagedType.U1)] public byte Byte_3 = 0;
		[FieldOffset (4)][MarshalAs (UnmanagedType.U1)] public byte Byte_4 = 0;
		[FieldOffset (5)][MarshalAs (UnmanagedType.U1)] public byte Byte_5 = 0;
		[FieldOffset (6)][MarshalAs (UnmanagedType.U1)] public byte Byte_6 = 0;
		[FieldOffset (7)][MarshalAs (UnmanagedType.U1)] public byte Byte_7 = 0;

		#region CORE

		public _Int64 ( long V ) => QWord = V;
		public _Int64 ( ulong V ) => UQWord = V;



		public byte[] Bytes
		{
			get => new byte[] { Byte_0, Byte_1, Byte_2, Byte_3, Byte_4, Byte_5, Byte_6, Byte_7 };
			set
			{
				if (value.Length != 8)
				{
					throw new ArgumentException ($"{nameof (_Int64)} Constructor failed! byteArray lenght is wrong!");
				}

				(Byte_0, Byte_1, Byte_2, Byte_3, Byte_4, Byte_5, Byte_6, Byte_7) = (value[ 0 ], value[ 1 ], value[ 2 ], value[ 3 ], value[ 4 ], value[ 5 ], value[ 6 ], value[ 7 ]);
			}
		}
		#endregion

		public override string ToString () => UQWord.ToString () + " / " + Bytes.eToStringHex (true);

		#region Conversions
		public static implicit operator ulong ( _Int64 L ) => L.UQWord;
		public static implicit operator long ( _Int64 L ) => L.QWord;
		public static implicit operator _Int64 ( ulong L ) => new (L);
		public static implicit operator _Int64 ( long L ) => new (L);
		public static implicit operator _Int64 ( int I ) => new (0L) { LoDWord = I };
		public static implicit operator _Int64 ( uint I ) => new (0L) { ULoDWord = I };
		public static implicit operator _Int64 ( byte[] ab ) => new () { Bytes = ab };
		#endregion

		#region Operators

		public static bool operator < ( _Int64 I1, _Int64 I2 ) => I1.UQWord < I2.UQWord;
		public static bool operator > ( _Int64 I1, _Int64 I2 ) => I1.UQWord > I2.UQWord;
		public static bool operator == ( _Int64 I1, _Int64 I2 ) => I1.UQWord == I2.UQWord;
		public static bool operator != ( _Int64 I1, _Int64 I2 ) => I1.UQWord != I2.UQWord;
		public static bool operator < ( _Int64 I1, ulong I2 ) => I1.UQWord < I2;
		public static bool operator > ( _Int64 I1, ulong I2 ) => I1.UQWord > I2;
		public static bool operator == ( _Int64 I1, ulong I2 ) => I1.UQWord == I2;
		public static bool operator != ( _Int64 I1, ulong I2 ) => I1.UQWord != I2;
		public static _Int64 operator - ( _Int64 I1, _Int64 I2 ) => new (I1.UQWord - I2.UQWord);
		public static _Int64 operator - ( _Int64 I1, ulong I2 ) => new (I1.UQWord - I2);
		public static _Int64 operator + ( _Int64 I1, _Int64 I2 ) => new (I1.UQWord + I2.UQWord);
		public static _Int64 operator + ( _Int64 I1, ulong I2 ) => new (I1.UQWord + I2);
		#endregion

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
		public override bool Equals ( object obj ) => ( this == (_Int64) obj );
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

		public override int GetHashCode () => UQWord.GetHashCode ();

	}


	#endregion



	/// <summary>Application Tools</summary>
	internal static class OSInfo
	{
		public static bool IsOSPlatform_Windows => RuntimeInformation.IsOSPlatform (OSPlatform.Windows);
		public static bool IsOSPlatform_OSX => RuntimeInformation.IsOSPlatform (OSPlatform.OSX);
	}



	/// <summary>Info about application assembly</summary>
	internal static partial class AppInfo
	{

		public static Assembly Assembly => Assembly.GetExecutingAssembly ();



		public static string? Title
#if ( UWP )
			=> Assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;
#else
			=> ProductName;
#endif


		public static string? CompanyName
#if ( UWP )
			=> Assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;
#else
			=> Application.CompanyName;
#endif



		public static string StartupPath
#if ( UWP )
		=> new FileInfo(Assembly.Location).DirectoryName!;
#else
			=> System.Windows.Forms.Application.StartupPath;
#endif



#if ( !ANDROID )

		public static string? ProductName
#if ( UWP )
			=> Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyProductAttribute>()?.Product;
#else
			=> Application.ProductName;
#endif


		public static string? ProductVersion
#if ( UWP )
			=> AssemblyFileVersionInfo?.FileVersion
				?? AssemblyFileVersionInfo?.ProductVersion;
#else
			=> Application.ProductVersion;
#endif




		public static string? Description => Assembly.GetCustomAttribute<AssemblyDescriptionAttribute> ()?.Description;
		public static string? Copyright => Assembly.GetCustomAttribute<AssemblyCopyrightAttribute> ()?.Copyright;
		public static string? Trademark => Assembly.GetCustomAttribute<AssemblyTrademarkAttribute> ()?.Trademark;



		public static string? @AssemblyVersionAttribute => Assembly.GetCustomAttribute<AssemblyVersionAttribute> ()?.Version;
		public static AssemblyName @AssemblyName => Assembly.GetName ();
		public static Version? @AssemblyVersion => AssemblyName.Version;


		public static string? Comments => AssemblyFileVersionInfo?.Comments;


		/// <summary>ProductName + ProductVersion</summary>
		internal static string AppProductNameAndVersion
			=> ( ProductName + " " + ProductVersion );



		public static FileInfo File => new (Assembly.Location);
		public static FileVersionInfo AssemblyFileVersionInfo => FileVersionInfo.GetVersionInfo (Assembly.Location);


#endif


		public static string? @AssemblyFileVersionAttribute => Assembly.GetCustomAttribute<AssemblyFileVersionAttribute> ()?.Version;


		////public static string? FileVersion() => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
		//public static string? FileVersion()
		//{
		//    var FVI = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
		//    return FVI.FileVersion;
		//}
		public static string? Configuration => Assembly.GetCustomAttribute<AssemblyConfigurationAttribute> ()?.Configuration;
		public static uint? AlgorithmId => Assembly.GetCustomAttribute<AssemblyAlgorithmIdAttribute> ()?.AlgorithmId;
		public static string? Culture => Assembly.GetCustomAttribute<AssemblyCultureAttribute> ()?.Culture;
		public static AssemblyNameFlags? Flags => (AssemblyNameFlags?) ( Assembly.GetCustomAttribute<AssemblyFlagsAttribute> ()?.AssemblyFlags );



		public static T? GetAssemblyAttribute<T> ( Assembly asmbl ) where T : Attribute
		{
			var attributes = asmbl.GetCustomAttributes (typeof (T), true);
			return ( null == attributes || !attributes!.Any () ) ? null : (T) attributes.First ();
		}

		public static T? GetAssemblyAttribute<T> () where T : Attribute
			=> GetAssemblyAttribute<T> (Assembly);


		/// <summary>Gets dir like: 'C:\Users\user\AppData\Roaming_or_Local\company\product\AssemblyName' (no version number included)'</summary>
		public static DirectoryInfo UserAppDataPath ( bool roaming, bool createIfNotExist = true, string company = "", string product = "", bool useAssemblyName = true, string assemblymName = "" )
		{
			company = company.eCheckNullOrWhiteSpace (AppInfo.CompanyName!);
			if (company.IsNullOrWhiteSpace ()) throw new ArgumentNullException (nameof (company));

			product = product.eCheckNullOrWhiteSpace (AppInfo.ProductName!);
			if (product.IsNullOrWhiteSpace ()) throw new ArgumentNullException (nameof (product));

			string rootPath = Environment.GetFolderPath (roaming
				? Environment.SpecialFolder.ApplicationData
				: Environment.SpecialFolder.LocalApplicationData);

			if (useAssemblyName)
			{
#if NET
				if (assemblymName.IsNullOrWhiteSpace ()) assemblymName = Assembly.GetExecutingAssembly ().GetName ().Name!;
#endif
				if (assemblymName.IsNullOrWhiteSpace ()) throw new ArgumentNullException (nameof (assemblymName));
			}

			string fullPath = Path.Combine (rootPath, company, product);
			if (useAssemblyName && assemblymName.IsNotNullOrWhiteSpace ()) fullPath = Path.Combine (fullPath, assemblymName);

			DirectoryInfo di = new (fullPath);
			if (createIfNotExist) di.eCreateIfNotExist ();
			return di;
		}


		public static string? TitleHeader => Title + ( ( @AssemblyFileVersionAttribute == null )
			? ""
			: " v" + @AssemblyFileVersionAttribute );

		public static bool CurrentUICultureIsRuTree => CultureInfo.CurrentUICulture.eIsRussianTree ();




#if NET


#if !WINDOWS
		internal static bool IsEmulator => DeviceInfo.Current.DeviceType == DeviceType.Virtual;

		/// <summary> AppInfo.Product</summary>
		internal static string TitleShort => AppInfo.Product!;

		internal static Page? CurrentUIContext => Application.Current!.MainPage!;

#endif


#endif


	}





	internal static partial class AppTools
	{




#if NETCOREAPP3_1_OR_GREATER //Required In NetCORE

		/// <summary>
		/// In Net & Core we need to RegisterEncodingProvider to use encodigs like Win-1251 or other
		/// </summary>
		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		internal static void RegisterEncodings ()
			=> System.Text.Encoding.RegisterProvider (System.Text.CodePagesEncodingProvider.Instance);

#endif

		internal static readonly System.Lazy<Encoding> LEncoding_Windows1251 = new (() =>
		{
#if NETCOREAPP3_1_OR_GREATER //Required In NetCORE
			RegisterEncodings ();
#endif
			return Encoding.GetEncoding ("Windows-1251");
		});


		internal static readonly Lazy<Encoding> LEncoding_cp866 = new (() =>
		{
#if NETCOREAPP3_1_OR_GREATER //Required In NetCORE
			RegisterEncodings ();
#endif
			return Encoding.GetEncoding ("cp866");
		}
			);


		#region EmbeddedResource Tools


		internal static string AssemblyEntryPointNamespace = AppInfo.Assembly.EntryPoint?.DeclaringType?.Namespace!;

		internal static string? LocalizedStringsManager_Namespace = AssemblyEntryPointNamespace;
		internal static string LocalizedStringsManager_ResourcesPath = "Localization.LStrings";

		internal static Lazy<System.Resources.ResourceManager> LocalizedStringsManager => new (() => new System.Resources.ResourceManager (
				 $"{LocalizedStringsManager_Namespace}.{LocalizedStringsManager_ResourcesPath}",
				   AppInfo.Assembly));


		/*
		/// <param name="resourcePath"><c>"Localization.LStrings"</c> or something like</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static System.Resources.ResourceManager CreateResourceManagerFromAssemblyEntryPoint(string resourcePath)
		=> new($"{Assembly.EntryPoint!.DeclaringType!.Namespace}.{resourcePath}", Assembly);
		 */


		/// <param name="resourcePath"><c>"Localization.LStrings"</c> or something like</param>
		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		internal static System.Resources.ResourceManager CreateResourceManager ( Type resourceType )
			=> new (resourceType);      //=> new($"{resourceType.FullName}", Assembly);


		/// <param name="embeddedResourceFileSuffix">Format: <c>"{Namespace}.{Folder}.{filename}.{Extension}"</c></param>
		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		public static Stream GetEmbeddedResourceStream ( string embeddedResourceFileSuffix )
		{
			string[] resNames = AppInfo.Assembly.GetManifestResourceNames ();
			string? resourcePath = resNames.FirstOrDefault (str => str.EndsWith (embeddedResourceFileSuffix));
			return resourcePath == null
				? throw new ArgumentException (null, nameof (embeddedResourceFileSuffix))
				: AppInfo.Assembly.GetManifestResourceStream (resourcePath)!;
		}


		/// <param name="embeddedResourceFileSuffix">Format: <c>"{Namespace}.{Folder}.{filename}.{Extension}"</c></param>
		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		public static string GetEmbeddedResourceAsString ( string embeddedResourceFileSuffix, Encoding? e = null )
		{
			using Stream stream = GetEmbeddedResourceStream (embeddedResourceFileSuffix);

			using StreamReader reader = ( e == null )
				? new StreamReader (stream, true)
				: new StreamReader (stream, e!);

			return reader.ReadToEnd ();
		}

		/// <param name="embeddedResourceFileSuffix">Format: <c>"{Namespace}.{Folder}.{filename}.{Extension}"</c></param>
		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		public static void SaveEmbeddedResourceAsString ( string embeddedResourceFileSuffix, FileInfo writeTo, Encoding? e = null )
		{
			e ??= Encoding.Unicode;
			string body = GetEmbeddedResourceAsString (embeddedResourceFileSuffix, e);
			writeTo.eWriteAllText (body, e);
		}


		#endregion


		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		internal static FileInfo GetFileIn_AppStartupDir ( string FileName )
			=> new (Path.Combine (AppInfo.StartupPath, FileName));


		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		internal static FileInfo GetFileIn_AppData ( string filePath, bool roaming )
			=> new (Path.Combine (AppInfo.UserAppDataPath (roaming).FullName, filePath));


		internal static TypeInfo[] GetAllAssemblyClassesDerivedFrom ( Type T )
			=> Assembly
			.GetExecutingAssembly ()
			.DefinedTypes
			.Where (rT => ReferenceEquals (rT.BaseType, T))
			.ToArray ();





		public static Process[] GetAllInstancesOfMyself ()
		{
			using Process cp = Process.GetCurrentProcess ();
			return Process.GetProcessesByName (cp.ProcessName);
		}


		public static bool HasOtherInstancesOfMyself ()
		{
			using Process cp = Process.GetCurrentProcess ();
			return GetAllInstancesOfMyself ()
				.SingleOrDefault (p => ( p.Id != cp.Id )) != null;
		}

		#region StartProcess...

		public enum ConsoleAppErrorMode : int
		{
			ErrorMustThrowException,
			ErrorReturnsAsOutput
		}

		internal static string StartConsoleAppAndCaptureOutput (
			string sExe,
			string? sArguments = null,
			string? workingDirectory = null,
			int WAIT_TIMEOUT_SEC = 5,
			bool SUPRESS_DEBUG_INFO = false,
			Encoding? StandardOutputEncoding = null,
			ConsoleAppErrorMode cem = ConsoleAppErrorMode.ErrorMustThrowException )
		{
			Debug.WriteLine ("*** erun '" + sExe + "' with args: '" + sArguments + "'");
			FileInfo fiExe = new (sExe);
			var PSI = new ProcessStartInfo
			{
				FileName = sExe,
				WorkingDirectory = workingDirectory ?? fiExe.DirectoryName,
				Arguments = sArguments,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true
			};

			if (StandardOutputEncoding != null)
			{
				PSI.StandardOutputEncoding = StandardOutputEncoding;
			}

			int iWaitMilliseconds = WAIT_TIMEOUT_SEC * 1000;
			using var prcExe = Process.Start (PSI);
			_ = prcExe ?? throw new Exception ($"Process.Start({fiExe}) Failed!");

			string sError = prcExe.StandardError.ReadToEnd ();
			if (sError.IsNotNullOrWhiteSpace () && cem == ConsoleAppErrorMode.ErrorMustThrowException)
			{
				throw new Exception (sError);
			}

			string sOutput = prcExe.StandardOutput.ReadToEnd ();
			if (sOutput.IsNullOrWhiteSpace ())
			{
				sOutput = "";
			}
			else
			{
				sOutput = sOutput
					.Replace (constants.vbCrCrLf, constants.vbCrLf)
					.Trim ();
			}

			bool bWaitResult = prcExe.WaitForExit (iWaitMilliseconds);
			if (!bWaitResult || !prcExe.HasExited)
			{
				Debug.WriteLine ($"{fiExe} - Failed to finish for {WAIT_TIMEOUT_SEC} sec. Answer: '{sOutput}'");
				Debug.WriteLine ($"{fiExe} - Force Closing...");
				prcExe.Close ();
				Debug.WriteLine ($"{fiExe} - Force Closed!");
			}
			// Call PRC.WaitForExit()
			else if (!SUPRESS_DEBUG_INFO)
			{
				Debug.WriteLine ($"{fiExe} - finished ok. Output = '{sOutput}'");
			}

			return sOutput;
		}


		#endregion


	}







	internal class DateTimeInterval : Stopwatch
	{

		[Flags]
		private enum TimeParts : int
		{
			Start = 1,
			Stop = 2
		}
		public DateTime StartTime { get; set; } = DateTime.Now;
		public DateTime StopTime { get; set; } = DateTime.Now;

		private DateTimeInterval () : base () { UpdateFromTime (); }

		public override string ToString () => $"с {StartTime} по {StopTime}";

		private void UpdateFromTime ( TimeParts flg = TimeParts.Start | TimeParts.Stop )
		{
			var dtNow = DateTime.Now;
			if (flg.HasFlag (TimeParts.Start))
			{
				StartTime = dtNow;
			}

			if (flg.HasFlag (TimeParts.Stop))
			{
				StopTime = dtNow;
			}
		}

		/// <summary>Starts, or resumes, measuring elapsed time for an interval.</summary>
		public new void Start ()
		{
			base.Start ();
			UpdateFromTime (TimeParts.Start);
		}

		/// <summary>Initializes a new System.Diagnostics.Stopwatch instance, sets the elapsed time property to zero, and starts measuring elapsed time.</summary>
		public static new DateTimeInterval StartNew ()
		{
			var DTI = new DateTimeInterval ();
			DTI.Start ();
			return DTI;
		}

		/// <summary>Stops measuring elapsed time for an interval.</summary>
		public new void Stop ()
		{
			base.Stop ();
			UpdateFromTime (TimeParts.Stop);
		}


		/// <summary>Stops time interval measurement and resets the elapsed time to zero.</summary>
		public new void Reset ()
		{
			base.Reset ();
			UpdateFromTime ();
		}

		/// <summary>Stops time interval measurement, resets the elapsed time to zero, and starts measuring elapsed time.</summary>
		public new void Restart ()
		{
			Stop ();
			Start ();
		}
	}


	namespace Comparers
	{
		/*
	 internal class StringIEqualityComparer_OrdinalIgnoreCase : IEqualityComparer<string>
	 {
		 public bool Equals(string x, string y)
			 => x.Equals(y, StringComparison.OrdinalIgnoreCase);

		 public int GetHashCode(string obj) => obj.GetHashCode();
	 }

	 internal class StringIEqualityComparer_CurrentCultureIgnoreCase : IEqualityComparer<string>
	 {
		 public bool Equals(string x, string y)
			 => x.Equals(y, StringComparison.CurrentCultureIgnoreCase);

		 public int GetHashCode(string obj) => obj.GetHashCode();

	 }

	 internal static class Instances
	 {

		 internal static Lazy<StringIEqualityComparer_OrdinalIgnoreCase> @StringIEqualityComparer_OrdinalIgnoreCase = new(new StringIEqualityComparer_OrdinalIgnoreCase());

		 internal static Lazy<StringIEqualityComparer_CurrentCultureIgnoreCase> @StringIEqualityComparer_CurrentCultureIgnoreCase = new(new StringIEqualityComparer_CurrentCultureIgnoreCase());

	 }
		 */
	}



	namespace Interfaces
	{

		internal interface ICloneableT<T> : ICloneable
		{
			public T CloneT ();

#if NET
			object ICloneable.Clone () => CloneT ()!;
#endif
		}


		internal interface IFilterableOf<T>
		{

			public bool Validate ( T filter );

		}


		internal interface IFilterableOfString : IFilterableOf<string> { }


		/*
	internal interface INotifyPropertyChangedEx : INotifyPropertyChanged
	{

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
	var pea = new PropertyChangedEventArgs(propertyName ?? throw new ArgumentNullException(nameof(propertyName)));

	INotifyPropertyChanged npc = this;
	//this.PropertyChanged
	var dd = npc.PropertyChanged;
	dd?.Invoke(this, pea);
	}


	[MethodImpl(MethodImplOptions.NoInlining)]
	protected T NotifyPropertyChanged<T>(T newValue, [CallerMemberName] string? propertyName = null)
	{
	this.OnPropertyChanged(propertyName);
	return newValue;
	}
	}
		 */

	}


	namespace Paralel
	{

		/// <summary>
		/// Provides a task scheduler that ensures a maximum concurrency level while
		/// running on top of the ThreadPool.
		/// </summary>
		internal class _LimitedConcurrencyLevelTaskScheduler : TaskScheduler
		{
			/// <summary>Whether the current thread is processing work items.</summary>
			[ThreadStatic]
			private static bool s_currentThreadIsProcessingItems;
			/// <summary>The list of tasks to be executed.</summary>
			private readonly LinkedList<Task> _tasks = new LinkedList<Task> (); // protected by lock(_tasks)
			/// <summary>The maximum concurrency level allowed by this scheduler.</summary>
			private readonly int _maxDegreeOfParallelism;
			/// <summary>Whether the scheduler is currently processing work items.</summary>
			private int _delegatesQueuedOrRunning = 0; // protected by lock(_tasks)

			/// <summary>
			/// Initializes an instance of the LimitedConcurrencyLevelTaskScheduler class with the
			/// specified degree of parallelism.
			/// </summary>
			/// <param name="maxDegreeOfParallelism">The maximum degree of parallelism provided by this scheduler.</param>
			public _LimitedConcurrencyLevelTaskScheduler ( int maxDegreeOfParallelism )
			{
				if (maxDegreeOfParallelism < 1)
				{
					throw new ArgumentOutOfRangeException (nameof (maxDegreeOfParallelism));
				}

				_maxDegreeOfParallelism = maxDegreeOfParallelism;
			}

			/// <summary>Queues a task to the scheduler.</summary>
			/// <param name="task">The task to be queued.</param>
			protected sealed override void QueueTask ( Task task )
			{
				// Add the task to the list of tasks to be processed.  If there aren't enough
				// delegates currently queued or running to process tasks, schedule another.
				lock (_tasks)
				{
					_tasks.AddLast (task);
					if (_delegatesQueuedOrRunning < _maxDegreeOfParallelism)
					{
						++_delegatesQueuedOrRunning;
						NotifyThreadPoolOfPendingWork ();
					}
				}
			}

			/// <summary>
			/// Informs the ThreadPool that there's work to be executed for this scheduler.
			/// </summary>
			private void NotifyThreadPoolOfPendingWork () => ThreadPool.UnsafeQueueUserWorkItem (_ =>
			{
				// Note that the current thread is now processing work items.
				// This is necessary to enable inlining of tasks into this thread.
				s_currentThreadIsProcessingItems = true;
				try
				{
					// Process all available items in the queue.
					while (true)
					{
						Task item;
						lock (_tasks)
						{
							// When there are no more items to be processed,
							// note that we're done processing, and get out.
							if (_tasks.Count == 0)
							{
								--_delegatesQueuedOrRunning;
								break;
							}

							// Get the next item from the queue
							item = _tasks.First!.Value;
							_tasks.RemoveFirst ();
						}

						// Execute the task we pulled out of the queue
						TryExecuteTask (item);
					}
				}
				// We're done processing items on the current thread
				finally { s_currentThreadIsProcessingItems = false; }
			}, null);

			/// <summary>Attempts to execute the specified task on the current thread.</summary>
			/// <param name="task">The task to be executed.</param>
			/// <param name="taskWasPreviouslyQueued"></param>
			/// <returns>Whether the task could be executed on the current thread.</returns>
			protected sealed override bool TryExecuteTaskInline ( Task task, bool taskWasPreviouslyQueued )
			{
				// If this thread isn't already processing a task, we don't support inlining
				if (!s_currentThreadIsProcessingItems)
				{
					return false;
				}

				// If the task was previously queued, remove it from the queue
				if (taskWasPreviouslyQueued)
				{
					TryDequeue (task);
				}

				// Try to run the task.
				return TryExecuteTask (task);
			}

			/// <summary>Attempts to remove a previously scheduled task from the scheduler.</summary>
			/// <param name="task">The task to be removed.</param>
			/// <returns>Whether the task could be found and removed.</returns>
			protected sealed override bool TryDequeue ( Task task )
			{
				lock (_tasks)
				{
					return _tasks.Remove (task);
				}
			}

			/// <summary>Gets the maximum concurrency level supported by this scheduler.</summary>
			public sealed override int MaximumConcurrencyLevel => _maxDegreeOfParallelism;

			/// <summary>Gets an enumerable of the tasks currently scheduled on this scheduler.</summary>
			/// <returns>An enumerable of the tasks currently scheduled.</returns>
			protected sealed override IEnumerable<Task> GetScheduledTasks ()
			{
				bool lockTaken = false;
				try
				{
					Monitor.TryEnter (_tasks, ref lockTaken);
					if (lockTaken)
					{
						return _tasks.ToArray ();
					}
					else
					{
						throw new NotSupportedException ();
					}
				}
				finally
				{
					if (lockTaken)
					{
						Monitor.Exit (_tasks);
					}
				}
			}
		}


	}


	namespace AutoDisposable
	{


		/// <summary> Base Class than automaticaly disposes its any attached values </summary>
		internal abstract class AutoDisposableUniversal : IDisposable
		{

			#region IDisposable Support
			private bool disposedValue;
			protected virtual void Dispose ( bool disposing )
			{
				if (!disposedValue)
				{
					if (disposing)
					{
						try { FreeManagedObjects (); } catch { }// Ignore Any Errors
					}

					// TODO: free unmanaged resources (unmanaged objects) and override finalizer                
					try { FreeUnmanagedObjects (); } catch { }// Ignore Any Errors    

					disposedValue = true;
				}
			}

			//// TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
			~AutoDisposableUniversal ()
			{
				// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
				Dispose (disposing: false);
			}

			public void Dispose ()
			{
				// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
				Dispose (disposing: true);
				GC.SuppressFinalize (this);
			}
			#endregion


			protected Stack<IDisposable> ManagedObjectsToDispose { get; private set; } = new Stack<IDisposable> ();
			protected Stack<IDisposable> UnManagedObjectsToDispose { get; private set; } = new Stack<IDisposable> ();
			protected Stack<Action> ManagedDisposeCallBacks { get; private set; } = new Stack<Action> ();
			protected Stack<Action> UnManagedDisposeCallBacks { get; private set; } = new Stack<Action> ();

			public AutoDisposableUniversal ( Action? rManagedDisposeCallBack = null, Action? rUnManagedDisposeCallBack = null ) : base ()
			{
				if (null != rManagedDisposeCallBack)
				{
					RegisterDisposeCallback (rManagedDisposeCallBack);
				}

				if (null != rUnManagedDisposeCallBack)
				{
					RegisterDisposeCallback (rUnManagedDisposeCallBack);
				}
			}

			/// <summary>Регистрируем объекты, которые надо будет уничтожить, при уничтожении родительского объекта</summary>
			protected internal void RegisterDisposableObject ( IDisposable MDO, bool IsManaged = true )
			{
				_ = MDO ?? throw new ArgumentNullException (nameof (MDO));
				if (ManagedObjectsToDispose.Contains (MDO) || UnManagedObjectsToDispose.Contains (MDO))
				{
					throw new ArgumentException ("Object already added to dispose list!", nameof (MDO));
				}

				var rListToAdd = UnManagedObjectsToDispose;
				if (IsManaged)
				{
					rListToAdd = ManagedObjectsToDispose;
				}

				rListToAdd.Push (MDO);
			}


			/// <summary>Регистрируем действия, которые надо будет выполнить при уничтожении родительского объекта</summary>
			protected internal void RegisterDisposeCallback ( Action onDispose, bool IsManaged = true )
			{
				_ = onDispose ?? throw new ArgumentNullException (nameof (onDispose));
				if (ManagedDisposeCallBacks.Contains (onDispose) || UnManagedDisposeCallBacks.Contains (onDispose))
				{
					throw new ArgumentException ($"'{nameof (onDispose)}' Already regidtered!", nameof (onDispose));
				}

				if (IsManaged)
				{
					ManagedDisposeCallBacks.Push (onDispose);
				}
				else
				{
					UnManagedDisposeCallBacks.Push (onDispose);
				}
			}

			protected virtual void FreeManagedObjects ()
			{
				OnBeforeDispose (true);
				DisposeList (ManagedObjectsToDispose);
				RunDisposeActions (ManagedDisposeCallBacks);
			}

			protected virtual void FreeUnmanagedObjects ()
			{
				OnBeforeDispose (false);
				DisposeList (UnManagedObjectsToDispose);
				RunDisposeActions (UnManagedDisposeCallBacks);
			}

			/// <summary>Just template, override if need</summary>            
			protected virtual void OnBeforeDispose ( bool bManages ) { }

			private static void DisposeList ( Stack<IDisposable> rList )
			{
				while (rList.Any ())
				{
					IDisposable rObjectToKill = rList.Pop ();
					rObjectToKill.eDisposeAndSetNothing ();
				}
			}

			private static void RunDisposeActions ( Stack<Action> rList )
			{
				while (rList.Any ())
				{
					var A = rList.Pop ();
					A.Invoke ();
				}
			}


		}

		/// <summary> Class than automaticaly disposes 1 attached value </summary>
		internal abstract class AutoDisposable1 : AutoDisposableUniversal
		{
			public AutoDisposable1 () : base () { }

			protected IDisposable? _Value = null;
			public IDisposable? Value
			{
				get => _Value;
				set { _Value = value; RegisterDisposableObject (_Value!, true); }
			}
		}

		/// <summary> Class than automaticaly disposes 1 attached value </summary>
		internal class AutoDisposable1T<T> : AutoDisposableUniversal where T : IDisposable
		{
			public AutoDisposable1T () : base () { }

			protected T? _Value = default;
			public T? Value
			{
				get => _Value;
				set
				{
					_Value = value;
					RegisterDisposableObject (_Value!, true);
				}
			}
		}


		namespace SafeContainers
		{



			/// <summary>Container for multithread safe get / set property value
			/// Supports notification on property changes (doe not check real changing of value, just notify about SET calls)</summary>
			/// <remarks>Valid for simple types (int, string, boolean), class and struts will not be MT safe, bc their child props has no MTsafe protect</remarks>
			[DefaultProperty ("Value")]
			internal abstract partial class MTSafeContainerBase<T> : AutoDisposableUniversal
			{

				#region ValueChangedEventArgs
				public partial class ValueChangedEventArgs : EventArgs
				{
					public readonly T? OldValue = default;
					public readonly T? NewValue = default;
					public bool Cancel = false;

					public ValueChangedEventArgs ( T? rOldValue, T? rNewValue ) : base ()
					{
						OldValue = rOldValue;
						NewValue = rNewValue;
						Cancel = false;
					}
				}
				#endregion

				public event EventHandler<ValueChangedEventArgs>? BeforeValueChanged;
				public event EventHandler<ValueChangedEventArgs>? AfterValueChanged;
				public event EventHandler<ValueChangedEventArgs>? ValueChangeCanceled;

				/// <summary>MT-UNsafe</summary>
				private T? _UnsafeValue = default;

				/// <summary>MT-UNsafe</summary>
				protected T? UnsafeValue
				{
					get => _UnsafeValue;
					set
					{
						var EA = new ValueChangedEventArgs (_UnsafeValue, value);
						OnBeforeValueChanged (EA);
						if (EA.Cancel)
						{
							OnValueChangeCanceled (EA);
						}
						else
						{
							_UnsafeValue = value;
							OnAfterValueChanged (EA);
						}
					}
				}

				public abstract T? Value { get; set; }
				public void SetValue ( T NewValue ) => Value = NewValue;

				public override string ToString () => $"{typeof (T)} = {Value}";

				protected void OnBeforeValueChanged ( ValueChangedEventArgs EA ) => BeforeValueChanged?.Invoke (this, EA);
				protected void OnValueChangeCanceled ( ValueChangedEventArgs EA ) => ValueChangeCanceled?.Invoke (this, EA);
				protected void OnAfterValueChanged ( ValueChangedEventArgs EA ) => AfterValueChanged?.Invoke (this, EA);




			}

			internal partial class MTSafeContainerSyncLock<T> : MTSafeContainerBase<T>
			{
				protected EventArgs _MTSyncObject = new ();

				public MTSafeContainerSyncLock ( T InitialValue ) : base () => Value = InitialValue;

				public override T? Value
				{
					get
					{
						lock (_MTSyncObject)
						{
							return UnsafeValue;
						}
					}
					set
					{
						lock (_MTSyncObject)
						{
							UnsafeValue = value;
						}
					}
				}
			}

			/// <summary>'Threading.ReaderWriterLockSlim' Контейнер для многопоточно-безопасного получения и установки какого либо значения
			/// Позволяет множественные чтения, но эксклюзивную запись
			/// Поддерживает уведомления об изменении свойств (не проверяет реально ли значение изменилось, просто срабатывает после каждой установки значения)</summary>
			/// <typeparam name="T">Тип значения в контейнере</typeparam>
			/// <remarks>Подходит для простых типов (int, string, boolean), составные типы или классы не будут безопасными, т.к. их дочерние свойства и поля не защищаются данным механизмом</remarks>
			internal partial class MTSafeContainer<T> : MTSafeContainerBase<T>
			{
				public ReaderWriterLockSlim? MTSyncObject { get; private set; } = null;

				public MTSafeContainer ( T InitialValue, LockRecursionPolicy LRP = LockRecursionPolicy.NoRecursion ) : base ()
				{
					MTSyncObject = new ReaderWriterLockSlim (LRP);
					RegisterDisposeCallback (Destroy);
					Value = InitialValue;
				}

				protected void RunInSafeLock ( Action a, bool write )
				{
					if (write)
					{
						MTSyncObject?.EnterWriteLock ();
					}
					else
					{
						MTSyncObject?.EnterReadLock ();
					}

					try
					{
						a.Invoke ();
					}
					finally
					{
						if (write)
						{
							MTSyncObject?.ExitWriteLock ();
						}
						else
						{
							MTSyncObject?.ExitReadLock ();
						}
					}
				}

				/// <summary>Многопоточнобезопасное получение и установка значения</summary>
				public override T? Value
				{
					get
					{
						T? v = default (T?);
						RunInSafeLock (() => { v = UnsafeValue; }, false);
						return v;
					}
					set
					{
						RunInSafeLock (() => { UnsafeValue = value; }, true);
					}
				}

				/// <summary> IDisposable</summary>
				private void Destroy () => MTSyncObject?.eDisposeAndSetNothing ();
			}

			//[DefaultProperty("IsSet")]
			internal partial class MTSafeBooleanFlag : MTSafeContainer<bool>
			{
				public MTSafeBooleanFlag ( bool bDefaultValue = false ) : base (bDefaultValue) { }

				public void SetlFlag ( bool bFlagValue = true ) => Value = bFlagValue;
				public void ClearFlag () => Value = false;
				/// <summary>Инвертирует текущее состояние. Возвращает новое, установленное состояние</summary>
				public bool Invert () { bool bInverted = !Value; Value = bInverted; return bInverted; }
				public bool IsSet { get => Value; }

				public static bool operator true ( MTSafeBooleanFlag R ) => R.Value;
				public static bool operator false ( MTSafeBooleanFlag R ) => !R.Value;

				public static implicit operator bool ( MTSafeBooleanFlag d ) => d.IsSet;

				public static implicit operator MTSafeBooleanFlag ( bool bFlag ) => new (bFlag);
			}

			internal partial class MTSafeCounterInt32 : MTSafeContainer<int>
			{
				public MTSafeCounterInt32 ( int iDefaultValue = 0 ) : base (iDefaultValue) { }

				public void Increment ( int iStep = 1 )
					=> RunInSafeLock (() => { UnsafeValue += iStep; }, true);

				public void Decrement ( int iStep = 1 )
					=> RunInSafeLock (() => { UnsafeValue -= iStep; }, true);

				public void Reset () => Value = 0;

				public static implicit operator int ( MTSafeCounterInt32 I ) => I.Value;
				public static implicit operator MTSafeCounterInt32 ( int I ) => new (I);
				public static bool operator < ( MTSafeCounterInt32 I1, int I2 ) => ( I1 < I2 );
				public static bool operator <= ( MTSafeCounterInt32 I1, int I2 ) => ( I1 <= I2 );
				public static bool operator > ( MTSafeCounterInt32 I1, int I2 ) => ( I1 > I2 );
				public static bool operator >= ( MTSafeCounterInt32 I1, int I2 ) => ( I1 >= I2 );
				public static bool operator == ( MTSafeCounterInt32 I1, int I2 ) => ( I1 == I2 );
				public static bool operator != ( MTSafeCounterInt32 I1, int I2 ) => ( I1 != I2 );

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
				public override bool Equals ( object obj ) => ( this == (MTSafeCounterInt32) obj );
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

				public override int GetHashCode () => Value.GetHashCode ();
			}


			/// <summary>Очередь для уничтожаемых одбектов, которые надо не забыть уничтожить, но прямо сейчас их уничтожать нельзя.
			/// Например для текущего захваченного с камеры кадра, который в данный момент отображается на экране - его нельзя сейчас уничтожать,
			/// но при добовлении следующего кадра, этот старый уже можно уничтожать, и его надо где-то хранить - для этого эта помойка.</summary>
			internal class DisposableTrashBin : AutoDisposableUniversal
			{

				private readonly EventArgs _lock = new ();
				private readonly Queue<IDisposable> _Q = new ();
				private readonly int _iQueueLength = 10;

				public DisposableTrashBin ( int QueueLength ) : base ()
				{
					_iQueueLength = QueueLength;

					RegisterDisposeCallback (() => ClearAll ());
				}

				public void Put ( IDisposable ObjectToDispose )
				{
					if (ObjectToDispose == null)
					{
						return;
					}

					lock (_lock)
					{
						if (_Q.Contains (ObjectToDispose))
						{
							return;
						}

						_Q.Enqueue (ObjectToDispose);

						while (_Q.Count > _iQueueLength)
						{
							ObjectToDispose = _Q.Dequeue ();
							ObjectToDispose.Dispose ();

							//Debug.WriteLine($"Trash disposed old object, queue = {_Q.Count}");
						}
					}
				}

				public void ClearAll ()
				{
					lock (_lock)
					{
						while (_Q.Count > 0)
						{
							var ObjectToDispose = _Q.Dequeue ();
							try { ObjectToDispose.Dispose (); }
							catch { }//Ignore any errors
						}
					}
					//Debug.WriteLine($"Trash bin cleared.");
				}
			}



		}
	}


	namespace Mem
	{


		internal class SlidingWindow<T> ( ArraySegment<T> mem, int startPos = 0, int windowSize = 1 )
		{
			private ArraySegment<T> _mem = mem;
			private int _startPos = startPos;
			private int _windowSize = windowSize;


			public SlidingWindow ( T[] arr, int startPos = 0, int windowSize = 1 ) : this (new ArraySegment<T> (arr), startPos, windowSize) { }

#if NET
			public SlidingWindow ( Memory<T> mem, int startPos = 0, int windowSize = 1 ) : this (mem.ToArray (), startPos, windowSize) { }
#endif

			/// <summary>Get/set Sliding Window StartPos.
			/// if StartPos + WindoowSize > right bound, StartPos sets to MaxStartPos to fit WindowSize.</summary>
			public int StartPos
			{
				get => _startPos;
				set
				{
					int sp = value;
					if (sp < 0) throw new ArgumentOutOfRangeException (nameof (StartPos));
					if (sp > MaxStartPos) sp = MaxStartPos;
					_startPos = sp;
				}
			}

			/// <summary>Get/set sliding WindowSize.
			/// if StartPos + new Windoow Size > right bound, size aligns to the right bound (sets to MaxWindoowSize)</summary>
			public int WindowSize
			{
				get => _windowSize;
				set
				{
					int ws = value;
					if (ws < 1) throw new ArgumentOutOfRangeException (nameof (WindowSize));

					if (ws > MaxWindowSize) ws = MaxWindowSize;

					_windowSize = ws;
				}
			}

			/// <summary>Gets maximum StartPos for current WindowSize</summary>
			public int MaxStartPos => _mem.Count () - WindowSize;

			/// <summary>Gets maximum WindowSize for current StartPos</summary>
			public int MaxWindowSize => _mem.Count () - StartPos;

			/// <summary>Slides Windows to the right side, preserving WindowSize</summary>
			public void SlideToEnd () => StartPos = MaxStartPos;

			/// <summary>Slides Windows to the start</summary>
			public void SlideToStart () => StartPos = 0;

			public bool CanExpandRight => ( WindowSize < MaxWindowSize );

			public void ExpandToEnd () => WindowSize = MaxWindowSize;


			public IEnumerable<T> WindowData => _mem
				.Skip (_startPos)
				.Take (_windowSize);

			public bool SlideRight ( bool allowShrinkWindowSize = true )
			{
				if (!CanExpandRight)
				{
					return false; // Window size already include all elements to the end! no more space rigth to move
				}

				if (StartPos > MaxStartPos && !allowShrinkWindowSize)
				{
					return false;
				}

				_startPos += WindowSize;
				if (_startPos > MaxStartPos)
				{
					ExpandToEnd ();//Recalculate new Window Size
				}

				return true;
			}


		}


		/// <summary>
		/// https://afana.me/archive/2023/06/19/array-pool-and-memory-pool/
		/// Samle:
		/// <code>
		/// [Benchmark]
		/// public void ArrayPoolHelper()
		/// {
		///     using var buffer = ArrayPoolHelper.Rent<byte>(1_000);
		///     for(int i=0; i<1_000;i++) {
		///         buffer.Value[i]=(byte) (i % 256);
		///     }
		/// }
		/// </code>
		/// </summary>
		internal static class ArrayPoolHelper
		{

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static SharedObject<T> Rent<T> ( int minimumLength )
				=> new (minimumLength);



			/// <summary>
			/// var streamNameBuffer = pool.Rent ((int) ptr->dwStreamNameSize);
			/// </summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static SharedObject<T> Rent<T> ( uint minimumLength )
				=> Rent<T> ((int) minimumLength);


			public readonly struct SharedObject<T> ( int minimumLength ) : IDisposable
			{

				private readonly T[] _value = ArrayPool<T>.Shared.Rent (minimumLength);


				public T[] Value { get => _value; }

				public ref T this[ int index ] => ref Value[ index ];

				public ref T FirstElement => ref _value[ 0 ];

				public void Dispose ()
					=> ArrayPool<T>.Shared.Return (Value);

			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static SharedObject<byte> FromBase64String ( string value, out int bytesWritten )
			{
				using var buffer = Rent<byte> (Encoding.UTF8.GetMaxByteCount (value.Length));
				int bufferSize = Encoding.UTF8.GetBytes (value, buffer.Value);
				var decodedBuffer = Rent<byte> (Base64.GetMaxDecodedFromUtf8Length (value.Length));
				try
				{
					Base64.DecodeFromUtf8 (buffer.Value.AsSpan (0, bufferSize), decodedBuffer.Value, out int _, out bytesWritten);
					if (bytesWritten == 0)
						throw new InvalidOperationException ("Error writing to buffer.");
				}
				catch
				{
					decodedBuffer.Dispose ();
					throw;
				}
				return decodedBuffer;
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string ToBase64String ( ReadOnlySpan<byte> value )
			{
				using var encodedBuffer = Rent<byte> (Base64.GetMaxEncodedToUtf8Length (value.Length));
				Base64.EncodeToUtf8 (value, encodedBuffer.Value, out int _, out int bytesWritten);
				if (bytesWritten == 0)
					throw new InvalidOperationException ("Error writing to buffer.");

				// Convert bytes to string            
				return Encoding.UTF8.GetString (encodedBuffer.Value.AsSpan (0, bytesWritten));
			}


		}

	}


	namespace Network
	{


		internal static class Helpers
		{

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool IsInDomain ()
			{
				try
				{
					//_ = System.DirectoryServices.ActiveDirectory.Domain.GetComputerDomain();
					return System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties ().DomainName.IsNotNullOrWhiteSpace ();
				}
				catch { return false; }
			}


			/// <summary>Gets IP of installed network adapters</summary>
			/// <param name="os">Adapter operational status filter</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			private static IEnumerable<UnicastIPAddressInformation> GelLocalIP ( OperationalStatus os = OperationalStatus.Up )
			{

				var ual = System.Net.NetworkInformation.NetworkInterface
					.GetAllNetworkInterfaces ()
					.Where (nic => nic.OperationalStatus == os)
					.SelectMany (nic => nic.GetIPProperties ().UnicastAddresses)
					.Where (ip => ip.Address != null);

				foreach (var ip in ual)
				{
					yield return ip;
				}
			}

			/// <summary>Gets IP of installed network adapters</summary>
			/// <param name="os">Adapter operational status filter</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<UnicastIPAddressInformation> GelLocalIP4 ( OperationalStatus os = OperationalStatus.Up )
			{
				var a = GelLocalIP (os)
					.Where (ua => ua.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && ua.IPv4Mask != null);

				return a;
			}


			#region GetRemoteMAC

			/*

			/// <summary>Запрашивает в ARP таблице адрес удалённого хоста</summary>
			/// 		''' <param name="RemoteIP">IP удалённого хоста</param>
			/// 		''' <param name="TryPing">Предварительно пинговать хост (чтобы он появился в таблице ARP)</param>
			/// 		''' <param name="PingTimeOut">Тайм-аут пинга</param>
			public static MACAddress GetRemoteMAC(IPAddress RemoteIP, bool TryPing, int PingTimeOut = 3000)
			{


				if (RemoteIP.Equals(IPAddress.None) || RemoteIP.Equals(IPAddress.Broadcast))
				{
					throw new ArgumentNullException("RemoteIP");
				}

				else if (RemoteIP.Equals(IPAddress.Loopback))
				{
					// Попытка узнать МАК для адреса 127.0.0.1
					string sLocalHostDNS = Dns.GetHostName();
					IPAddress[] argResolvedIP = null;
					MACAddress[] aMACS = GetRemoteMAC(sLocalHostDNS, false, 3000, ResolvedIP: ref argResolvedIP, false);
					if (aMACS.Length > 0)
					{
						return aMACS[0];
					}
					else
					{
						string sERR = string.Format("MAC Resolve for {0} FAILED!", sLocalHostDNS);
						throw new Exception(sERR);
					}
				}


				int iPhyAddrLen = 6;
				var hMACBuffer = Marshal.AllocHGlobal(iPhyAddrLen);
				try
				{
					// retrieve the remote MAC address
			uint iIP = (uint)RemoteIP.Address;
			uint iIPAny = (uint)IPAddress.Any.Address;
			int iResult = SendARP(iIP, iIPAny, hMACBuffer, ref iPhyAddrLen);
					if (iResult != 0)
					{
						var WEX = new Win32Exception(iResult);
						throw WEX;
					}

		byte[] abMAC = new byte[] { 0, 0, 0, 0, 0, 0 };
		Marshal.Copy(hMACBuffer, abMAC, 0, 6);
					var MAC = new MACAddress(abMAC);
					return MAC;
				}
				finally
				{
		Marshal.FreeHGlobal(hMACBuffer);
		}
			}



			/// <summary>Запрашивает в ARP таблице MAC адрес удалённого хоста</summary>
			/// 		''' <param name="RemoteIP">IP удалённого хоста</param>
			public static PhysicalAddress GetRemoteMAC(IPAddress RemoteIP)
			{

				if (RemoteIP.Equals(IPAddress.None) || RemoteIP.Equals(IPAddress.Broadcast) || RemoteIP.Equals(IPAddress.Loopback))

				{

					throw new ArgumentNullException("RemoteIP");
				}

				int iPhyAddrLen = 6;
				var hMACBuffer = Marshal.AllocHGlobal(iPhyAddrLen);
				try
				{

			uint iIP = (uint)RemoteIP.Address;
			uint iIPAny = (uint)IPAddress.Any.Address;

			int iResult = SendARP(iIP, iIPAny, hMACBuffer, ref iPhyAddrLen);
					if (iResult != 0)
					{
						var WEX = new Win32Exception(iResult);
						throw WEX;
					}

		byte[] abMAC = new byte[] { 0, 0, 0, 0, 0, 0 };
		Marshal.Copy(hMACBuffer, abMAC, 0, 6);
					return new PhysicalAddress(abMAC);
		}
				finally
				{
		Marshal.FreeHGlobal(hMACBuffer);
		}
			}



		/// <summary>Возвращает несколько адресов, которые удалось выяснить</summary>
		/// 		''' <param name="RemoteIPOrDNSMane">IP или DNS имя удаленного хоста</param>
		/// 		''' <param name="TryPing">Предварительно пинговать хост ()</param>
		/// 		''' <param name="PingTimeOut">Тайм-аут пинга</param>
		/// 		''' <param name="CanThrowError">Генерировать исключение если, произошла ошибка с одним из адресов</param>
		public static MACAddress[] GetRemoteMAC(string RemoteIPOrDNSMane, bool TryPing, int PingTimeOut = 3000, [Optional, DefaultParameterValue(null)] ref IPAddress[] ResolvedIP, bool CanThrowError = false)
		{

		var aHE = Dns.GetHostEntry(RemoteIPOrDNSMane);
		ResolvedIP = aHE.AddressList;
		var aMACs = new List<MACAddress>();
		foreach (IPAddress IP in ResolvedIP)
		{
		try
		{
		   var MAC = GetRemoteMAC(IP, TryPing, PingTimeOut);
		   aMACs.Add(MAC);
		}
		catch
		{
		   if (CanThrowError)
			   throw;
		}
		}
		return aMACs.ToArray();
		}

			 */

			#endregion


			public static FileInfo? DownloadFile_WebRequest ( string url, string localFile, TimeSpan timeout )
			{
				try
				{
#pragma warning disable SYSLIB0014 // Type or member is obsolete
					HttpWebRequest request = (HttpWebRequest) WebRequest.Create (url);
#pragma warning restore SYSLIB0014 // Type or member is obsolete
					request.Timeout = 30000;
					request.AllowWriteStreamBuffering = false;
					using (var response = (HttpWebResponse) request.GetResponse ())
					using (var s = response.GetResponseStream ())
					using (var fs = new FileStream ("test.csv", FileMode.Create))
					{
						byte[] buffer = new byte[ 4096 ];
						int bytesRead;
						while (( bytesRead = s.Read (buffer, 0, buffer.Length) ) > 0)
						{
							fs.Write (buffer, 0, bytesRead);
							bytesRead = s.Read (buffer, 0, buffer.Length);
						}
					}

					return new (localFile);
				}
				catch { return null; }

			}

#if NET


			internal const string HTTP_HEADER_AGENT_DEFAULT = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";


			/*
	public static FileInfo? DownloadFile_HttpClient(string url, string localFile, TimeSpan timeout)
	{
		try
		{
			using HttpClient client = new() { Timeout = timeout };
			using var s = client.GetStreamAsync(remoteUrl);
			//s.Start();
			//s.Wait();
			Stream remoteStream = s.Result;
			using FileStream fs = new(localFile, FileMode.OpenOrCreate);
			remoteStream.CopyTo(fs);
			return new(localFile);
		}
		catch { return null; }
	}
			 */
			public static async Task<FileInfo> DownloadFile_HttpClient_Async ( string remoteUrl, string localFile, TimeSpan timeout, CancellationToken ct )
			{
				// Set custom User Agent

				using (HttpClient client = new () { Timeout = timeout })
				{
					client.DefaultRequestHeaders.Add ("User-Agent", HTTP_HEADER_AGENT_DEFAULT);

					/*
					var productValue = new ProductInfoHeaderValue("ScraperBot", "1.0");
					var commentValue = new ProductInfoHeaderValue("(+http://www.example.com/ScraperBot.html)");
					client.DefaultRequestHeaders.UserAgent.Add(productValue);
					client.DefaultRequestHeaders.UserAgent.Add(commentValue);
					client.Headers.UserAgent.Add(productValue);
					client.Headers.UserAgent.Add(commentValue);
					 */

					using (Stream webStream = await client.GetStreamAsync (remoteUrl, ct))
					{
						using (FileStream localFileStream = new (localFile, FileMode.OpenOrCreate))
						{
							await webStream.CopyToAsync (localFileStream, ct);
						}
					}
				}

				return new (localFile);
			}

#endif


		}



		/// <summary>Comparable PhysicalAddress</summary>
		internal class PhysicalAddressEx ( byte[] mac ) : PhysicalAddress (mac), IComparable<PhysicalAddressEx>, IComparable<PhysicalAddress>
		{
			/// <summary>
			/// <code>
			/// 001122334455
			/// 00-11-22-33-44-55
			/// 0011.2233.4455
			/// 00:11:22:33:44:55
			/// F0-E1-D2-C3-B4-A5
			/// f0-e1-d2-c3-b4-a5
			/// </code>
			/// </summary>
			public PhysicalAddressEx ( string mac ) : this (PhysicalAddress.Parse (mac).GetAddressBytes ()) { }

			public string GetHash () => this.ToString ();

			public int CompareTo ( PhysicalAddressEx? other )
				=> this.GetHash ().CompareTo (other?.GetHash () ?? string.Empty);


			public static bool operator == ( PhysicalAddressEx A, PhysicalAddressEx B ) => ( A.GetHash () ?? string.Empty ) == ( B.GetHash () ?? string.Empty );
			public static bool operator != ( PhysicalAddressEx A, PhysicalAddressEx B ) => ( A.GetHash () ?? string.Empty ) != ( B.GetHash () ?? string.Empty );

			public override bool Equals ( object? obj ) => base.Equals (obj);

			public override int GetHashCode () => GetHash ().GetHashCode ();

			public int CompareTo ( PhysicalAddress? other )
				=> this.GetHash ().CompareTo (other?.ToString () ?? string.Empty);

		}

		[Obsolete ("Not tested!", true)]
		internal class IPAddressEx : System.Net.IPAddress
		{
			public IPAddressEx ( byte[] address ) : base (address) { }

			public IPAddressEx ( Int64 address ) : base (address) { }

			public IPAddressEx ( byte[] address, Int64 scopeid ) : base (address, scopeid) { }


			/// <summary>Determines the relative value of adddress1 to address2.</summary>
			/// <returns>-1 indicates address1 is less than address2. 1 indicates address1 is greater than address2. 0 indicates both are equal. -2 indicates addresses are incompatible for comparison.</returns>
			public int CompareTo ( IPAddressEx value )
			{
				int returnVal = 0;
				if (this.AddressFamily == value.AddressFamily)
				{
					byte[] b1 = this.GetAddressBytes ();
					byte[] b2 = value.GetAddressBytes ();

					for (int i = 0 ; i < b1.Length ; i++)
					{
						if (b1[ i ] < b2[ i ])
						{
							returnVal = -1;
							break;
						}
						else if (b1[ i ] > b2[ i ])
						{
							returnVal = 1;
							break;
						}
					}
				}
				else
				{
					throw new ArgumentOutOfRangeException ("value", "Cannot compare two addresses no in the same Address Family.");
				}

				return returnVal;
			}


			/// <summary>Determines if the current IP Address is in the given range.</summary>
			/// <param name="rangeStartAddress">The beginning of the range.</param>
			/// <param name="rangeEndAddress">The end of the range.</param>
			/// <returns>True if the IP Address is within the passed range.</returns>
			public bool IsInRange ( IPAddressEx rangeStartAddress, IPAddressEx rangeEndAddress )
			{
				bool returnVal = false;
				// ensure that all addresses are of the same type otherwise reject //
				if (rangeStartAddress.AddressFamily != rangeEndAddress.AddressFamily)
				{
					throw new ArgumentOutOfRangeException (nameof (rangeStartAddress),
						  $"The Start Range type {rangeStartAddress.AddressFamily} and End Range type {rangeEndAddress.AddressFamily} are not compatible ip address families.");
				}

				if (rangeStartAddress.AddressFamily == this.AddressFamily)
				{
					returnVal = ( CompareTo (rangeStartAddress) >= 0 && CompareTo (rangeEndAddress) <= 0 );   // no need to check for -2 value as this check has already been undertaken to get into this block //
				}
				else
				{
					throw new ArgumentOutOfRangeException (nameof (rangeStartAddress),
						  $"The range type {rangeStartAddress.AddressFamily} and current value type {this.AddressFamily} are not compatible ip address families");
				}

				return returnVal;
			}

			public static IPAddressEx[] GetLocalAddresses ()
			{
				string hostName = System.Net.Dns.GetHostName ();
				System.Net.IPHostEntry entry = System.Net.Dns.GetHostEntry (hostName);
				List<IPAddressEx> list = [];
				foreach (System.Net.IPAddress address in entry.AddressList)
				{
					list.Add (new IPAddressEx (address.GetAddressBytes ()));
				}
				return list.ToArray ();
			}

			public static bool IsLocalAddressInRange ( IPAddressEx rangeStartAddress, IPAddressEx rangeEndAddress )
			{
				bool returnVal = false;
				foreach (IPAddressEx address in GetLocalAddresses ())
				{
					if (address.IsInRange (rangeStartAddress, rangeEndAddress))
					{
						returnVal = true;
						break;
					}
				}
				return returnVal;
			}
		}


		internal class IP4AddressWithMask ( IPAddress ip, uint prefixLength = IP4AddressWithMask.PREFIX_32 ) : System.Net.NetworkInformation.IPAddressInformation (), IComparable<IP4AddressWithMask>
		{
			internal static readonly IPAddress CloudFlareRecursiveDNSConverter = IPAddress.Parse ("1.1.1.1");

			internal static readonly IP4AddressWithMask CloudFlareDNSNetwork = new (CloudFlareRecursiveDNSConverter, 8);


			internal const uint PREFIX_0 = 0;
			internal const uint PREFIX_32 = 32;

			private static Lazy<Regex> _rxMaskedIP = new (() => new Regex (@"\b(?<IP>\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})(?<Mask>\/\d{1,5})?\b"));
			private readonly IPAddress _ipAddress = ip;
			public readonly uint PrefixLength = prefixLength.eCheckRange (0, PREFIX_32);

			public override IPAddress Address => _ipAddress;

			public IPAddress Mask => PrefixLength.eGetIP4SubnetMask ();

			public override bool IsDnsEligible => throw new NotImplementedException ();
			public override bool IsTransient => throw new NotImplementedException ();


			public IP4AddressWithMask ( IPAddress ip, IPAddress mask ) : this (ip, mask.eGetIP4SubnetPrefixSizeFromMask ()) { }

			public override string ToString () => $"{_ipAddress}/{PrefixLength}";


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static IEnumerable<IP4AddressWithMask> ParseIPs ( string multiIPString )
			{
				MatchCollection result = _rxMaskedIP.Value.Matches (multiIPString);
				if (result.Count < 1)
				{
					yield break;
				}

				foreach (Match m in result)
				{
					string ip = m.Groups[ "IP" ].Value;
					string prefixSizeString = m.Groups[ "Mask" ].Value;

					if (!IPAddress.TryParse (ip, out IPAddress? ipa))
					{
						continue;
					}

					uint prefixSize = PREFIX_32;
					if (prefixSizeString.IsNotNullOrWhiteSpace () && uint.TryParse (prefixSizeString.Substring (1), out var parsedPrefixSize))
					{
						prefixSize = parsedPrefixSize;
					}

					yield return new IP4AddressWithMask (ipa!, prefixSize);
				}
				yield break;
			}





			public int CompareTo ( IP4AddressWithMask? other )
				=> ( other == null )
				? 1
				: Address!.eCompareTo (other!.Address!);
		}


		internal class IP4AddressComparer : IComparer<IPAddress>
		{
			public static readonly Lazy<IP4AddressComparer> StaticInstance = new (() => new ());

			public int Compare ( IPAddress? x, IPAddress? y ) => CompareIPAddress (x, y);

			public static int CompareIPAddress ( IPAddress? x, IPAddress? y )
			{
				if (x == y && x == null)
				{
					return 0;
				}

				if (x == null)
				{
					return -1;
				}

				if (y == null)
				{
					return 1;
				}

				uint uX = x.eToUInt32CalculableOrder (), uY = y.eToUInt32CalculableOrder ();
				return uX.CompareTo (uY);
			}
		}

		internal class IP4AddressWithMaskComparer : IComparer<string>, IComparer<IP4AddressWithMask>
		{
			public static readonly Lazy<IP4AddressWithMaskComparer> StaticInstance = new (() => new ());

			public int Compare ( string? x, string? y )
			{
				try
				{
					IP4AddressWithMask?
						ipX = IP4AddressWithMask.ParseIPs (x ?? string.Empty).FirstOrDefault (),
						ipY = IP4AddressWithMask.ParseIPs (y ?? string.Empty).FirstOrDefault ();

					return Compare (ipX, ipY);
				}
				catch { }
				return string.Compare (x, y);
			}

			public int Compare ( IP4AddressWithMask? x, IP4AddressWithMask? y )
			{
				if (x == null && y == null)
				{
					return 0;
				}

				if (x == null)
				{
					return -1;
				}

				return x.CompareTo (y);
			}
		}


		internal class MACComparer : IComparer<PhysicalAddress>
		{
			public static readonly Lazy<MACComparer> StaticInstance = new (() => new ());


			public static int CompareMAC ( PhysicalAddress? x, PhysicalAddress? y )
				=> string.Compare (x?.ToString () ?? string.Empty, y?.ToString () ?? string.Empty, StringComparison.OrdinalIgnoreCase);

			public int Compare ( PhysicalAddress? x, PhysicalAddress? y ) => CompareMAC (x, y);
		}



		//https://newbedev.com/progress-bar-with-httpclient
		internal class HttpClientDownloadWithProgress ( string downloadUrl, string destinationFilePath ) : uom.AutoDisposable.AutoDisposable1T<HttpClient> ()
		{
			private readonly string _downloadUrl = downloadUrl;
			private readonly string _destinationFilePath = destinationFilePath;

			private HttpClient? _httpClient = null;

			public delegate void ProgressChangedHandler ( long? totalFileSize, long totalBytesDownloaded, double? progressPercentage );
			public event ProgressChangedHandler ProgressChanged = delegate { };
			//public event EventHandler<string> LineRead = delegate { };

			public async Task StartDownload ()
			{
				_httpClient = new HttpClient { Timeout = TimeSpan.FromDays (1) };
				this.Value = _httpClient;

				using var response = await _httpClient.GetAsync (_downloadUrl, HttpCompletionOption.ResponseHeadersRead);
				await DownloadFileFromHttpResponseMessage (response);
			}

			private async Task DownloadFileFromHttpResponseMessage ( HttpResponseMessage response )
			{
				response.EnsureSuccessStatusCode ();
				var totalBytes = response.Content.Headers.ContentLength;
				using var contentStream = await response.Content.ReadAsStreamAsync ();
				await ProcessContentStream (totalBytes, contentStream);
			}

			private async Task ProcessContentStream ( long? totalDownloadSize, Stream contentStream )
			{
				var totalBytesRead = 0L;
				var readCount = 0L;
				var buffer = new byte[ 8192 ];
				var isMoreToRead = true;

				using FileStream fileStream = new (_destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
				do
				{
					int bytesRead = await contentStream.ReadAsync (buffer, 0, buffer.Length);
					if (bytesRead == 0)
					{
						isMoreToRead = false;
						TriggerProgressChanged (totalDownloadSize, totalBytesRead);
						continue;
					}

					await fileStream.WriteAsync (buffer, 0, bytesRead);

					totalBytesRead += bytesRead;
					readCount += 1;

					if (readCount % 100 == 0)
					{
						TriggerProgressChanged (totalDownloadSize, totalBytesRead);
					}
				}
				while (isMoreToRead);
			}

			private void TriggerProgressChanged ( long? totalDownloadSize, long totalBytesRead )
			{
				if (ProgressChanged == null)
				{
					return;
				}

				double? progressPercentage = null;
				if (totalDownloadSize.HasValue)
				{
					progressPercentage = Math.Round ((double) totalBytesRead / totalDownloadSize.Value * 100, 2);
				}

				ProgressChanged (totalDownloadSize, totalBytesRead, progressPercentage);
			}


		}


	}


	internal static class Globalize
	{
		internal struct CURRENCY
		{
			private RegionInfo _ri;
			public CURRENCY ( RegionInfo ri ) { _ri = ri; }

			public string ISOCurrencySymbol => _ri.ISOCurrencySymbol;
			public string CurrencySymbol => _ri.CurrencySymbol;
			public string CurrencyEnglishName => _ri.CurrencyEnglishName;
		}

		/// <summary>
		/// AUD, $, Australian Dollar
		/// CAD, $, Canadian Dollar
		/// EUR, ?, Euro
		/// GBP, £, British Pound
		/// JPY, ¥, Japanese Yen
		/// USD, $, US Dollar
		/// </summary>
		internal static CURRENCY[] GetCurrencies ()
		{
			return CultureInfo.GetCultures (CultureTypes.SpecificCultures)
				.Select (ci => ci.LCID).Distinct ()
				.Select (id => new RegionInfo (id))
				.GroupBy (r => r.ISOCurrencySymbol)
				.Select (g => g.First ())
				.Select (r => new CURRENCY (r)
				{
				}).ToArray ();
		}

		/* Список валют и культур 1 уровня для этой валюты
		var culturesByCurrency = CultureInfo.GetCultures(CultureTypes.AllCultures)
			.Where(ci => ci.Parent == CultureInfo.InvariantCulture)
			.Select(ci => new { ci, ci.NumberFormat.CurrencySymbol })
			.Where(x => !string.IsNullOrWhiteSpace(x.CurrencySymbol))
			.GroupBy(x => x.CurrencySymbol)
			.Select(x => new { Symbol = x.First().CurrencySymbol, Cultures = x.Select(z => z.ci).ToArray() })
			.ToDictionary(x => x.Symbol, x => x.Cultures);
		*/




	}


	internal static class I_O
	{
		/// <summary>Для файлового ввода-вывода префикс «\\?\» указывает API-интерфейсам Windows отключить весь синтаксический анализ строки и отправить строку, 
		/// которая следует за ней, прямо в файловую систему.
		/// Это позволяет использовать . или .. в именах путей, а также ослабить ограничение в 
		/// 260 символов для имени пути, если базовая файловая система поддерживает длинные пути и имена файлов.
		/// 
		/// https://docs.microsoft.com/ru-ru/windows/win32/fileio/naming-a-file?redirectedfrom=MSDN
		/// 
		/// Так как он отключает автоматическое расширение строки пути, \\префикс "?\" также позволяет использовать ".." и "." в именах путей, 
		/// которые могут быть полезны при попытке выполнить операции с файлом, в противном случае зарезервированные описатели относительных путей в составе полного пути.
		/// 
		/// Многие, но не все API-интерфейсы файлового ввода/вывода поддерживают "\\?\"; для проверки каждого API следует обратиться к справочному разделу.
		/// </summary>
		internal const string CS_PATH_PREFIX_WIN_LONG_PATH = @"\\?\";

		/// <summary> при работе с функциями API Windows следует использовать \\.\ для доступа только к устройствам, а не файлам!.
		/// Большинство интерфейсов API не поддерживают "\\.\";
		/// только те, которые предназначены для работы с пространством имен устройства, распознают его.
		/// Всегда проверяйте справочный раздел для каждого API, чтобы быть уверенным в этом.
		/// </summary>
		internal const string CS_PATH_PREFIX_WIN_DEVICE = @"\\.\";//" \\.\COM56 etc



		/// <summary>Multithread File system scanner</summary>
		internal abstract class FileSystemScannerBase
		{
			public enum ERROR_SOURCES
			{
				Unknown,
				FAILED_GET_FOLDER_CONTENT,
				FAILED_READ_FILE
				//FAILED_GET_CHILD_DIRECTORIES,
			}
			public struct SCAN_ERROR
			{
				public readonly FileSystemInfo fsi;
				public readonly Exception ex;
				public readonly ERROR_SOURCES source;

				public SCAN_ERROR ( FileSystemInfo fso, Exception ex, ERROR_SOURCES src )
				{
					fsi = fso;
					this.ex = ex;
					source = src;
				}
				public override string ToString () => $"{fsi.eFullName_RemoveLongPathPrefix ()} {source} {ex.Message.Trim ()}";
			}

			public struct SCAN_RUNNING_INFO
			{
				public readonly int TasksToWait;
				public readonly int TasksFinished;

				public SCAN_RUNNING_INFO ( int iTasksToWait, int iTasksFinished )
				{
					TasksToWait = iTasksToWait;
					TasksFinished = iTasksFinished;
				}
			}


			private uom.Paralel._LimitedConcurrencyLevelTaskScheduler? _lcts = null;
			//private TaskFactory? _TF = null;

			protected List<Task> _lTasks = [];
			protected CancellationToken _cts = new ();

			public CancellationToken StopFlag { get => _cts; }

			protected FileSystemScannerBase () { }


			protected volatile UInt32 ThreadsCounter = 0;

			protected void Scan (
				DirectoryInfo[]? aRootDirsToScan = null,
				int? maxDegreeOfParallelism = null
				)
			{





				if (null == aRootDirsToScan || !aRootDirsToScan.Any ())
				{
					//Not specidef Root Dirs for scan
					var aDisks = Environment.GetLogicalDrives ();
					if (!aDisks.Any ())
					{
						throw new Exception ("Failed to get logical drives!");
					}

					aRootDirsToScan = ( from sDir in aDisks
										orderby sDir
										let fiDir = sDir.eToDirectoryInfo (true)
										select fiDir ).ToArray ();
				}



				maxDegreeOfParallelism ??= Environment.ProcessorCount;

				_cts = new CancellationToken ();
				_lcts = new ((int) maxDegreeOfParallelism);
				//_TF = _lcts.CreateTaskFactory();
				_lTasks = [];

				//Start Main Scan Core
				ScanDirs (aRootDirsToScan);

				//sleep some time for backgroubd jobs starts...
				Thread.Sleep (500);

				//Wait all tasks finished...
				var iTasksToWait = 1;
				while (iTasksToWait > 0)
				{
					var iTaskTotal = 0;
					var iTasksFinished = 0;
					lock (_lTasks)
					{
						iTaskTotal = _lTasks.Count;
						//Remove all finished tasks
						_lTasks.RemoveAll (( T ) => T.Status == TaskStatus.RanToCompletion);
						iTasksToWait = _lTasks.Count;
						iTasksFinished = ( iTaskTotal - iTasksToWait );
					}
					OnWaitTasks (new SCAN_RUNNING_INFO (iTasksToWait, iTasksFinished));
				}

			}

			private void ScanDirs ( DirectoryInfo[] aDirs )
			{
				if (_cts.IsCancellationRequested)
				{
					return;
				}

				aDirs.eForEach (( d ) =>
				{
					if (_cts.IsCancellationRequested)
					{
						return;
					}

					lock (_lTasks)
					{
						var tskNew = Task.Factory.StartNew (
							() => CheckDir (d),
							_cts,
							TaskCreationOptions.LongRunning, _lcts!);

						_lTasks.Add (tskNew);
					}
				});
			}

			private void CheckDir ( DirectoryInfo fiDir )
			{
				ThreadsCounter++;
				try
				{


					//lock (_ConsoleLock) Console.WriteLine($"Start scan dir '{fiDir.FullName}'");

					if (!OnEnterDir (fiDir))
					{
						return;
					}

					if (_cts.IsCancellationRequested)
					{
						return;
					}

					FileSystemInfo[] aChildrens = Array.Empty<FileSystemInfo> ();
					try { aChildrens = fiDir.GetFileSystemInfos (); }
					catch (Exception exGetFiles)
					{
						var ERR = new SCAN_ERROR (fiDir, exGetFiles, ERROR_SOURCES.FAILED_GET_FOLDER_CONTENT);
						if (!OnError (ERR))
						{
							return;
						}
					}


					try
					{
						var aFiles = aChildrens.Where (fsi => fsi.eIsFileInfo ()).Cast<FileInfo> ().ToArray ();
						foreach (var F in aFiles)
						{
							if (!OnFileFound (F))
							{
								break;
							}
						}
					}
					catch (Exception exGetFiles)
					{
						var ERR = new SCAN_ERROR (fiDir, exGetFiles, ERROR_SOURCES.Unknown);
						if (!OnError (ERR))
						{
							return;
						}
					}

					if (_cts.IsCancellationRequested)
					{
						return;
					}

					try
					{
						var aDirs = aChildrens
							.Where (fsi => fsi.eExistAndIsDirectory ())
							.Cast<DirectoryInfo> ()
							.ToArray ();

						aDirs = OnBeforeCheckSubDirs (aDirs);
						if (null == aDirs || !aDirs.Any ())
						{
							return;
						}

						ScanDirs (aDirs);
					}
					catch (Exception exGetDirs)
					{
						var ERR = new SCAN_ERROR (fiDir, exGetDirs, ERROR_SOURCES.Unknown);
						if (!OnError (ERR))
						{
							return;
						}
					}


				}
				finally
				{
					ThreadsCounter--;
				}
			}

			/// <summary>Occurs when SCANNER enters to found directory</summary>
			/// <returns>True to continue scan or false to stop scan</returns>
			//[MethodImpl(MethodImplOptions.Synchronized)]
			protected virtual bool OnEnterDir ( DirectoryInfo D ) => ( !_cts.IsCancellationRequested );

			/// <summary>Occurs before SCANNER begins scan subdirectories</summary>
			/// <returns>Tou can modify array of subDirs to scan, or return null to stop</returns>
			//[MethodImpl(MethodImplOptions.Synchronized)]
			protected virtual DirectoryInfo[] OnBeforeCheckSubDirs ( DirectoryInfo[] aDirs ) => aDirs;

			/// <summary>Occurs when SCANNER found any file</summary>
			/// <returns>True to continue scan or false to stop scan</returns>
			//[MethodImpl(MethodImplOptions.Synchronized)]
			protected virtual bool OnFileFound ( FileInfo F ) => ( !_cts.IsCancellationRequested );


			/// <summary>Periodicaly Occurs when some tasks started and finished
			/// Just used for write any messages showinf that app is not hang</summary>            
			protected virtual void OnWaitTasks ( SCAN_RUNNING_INFO e )
			{
				//Just waste some time to finish some tasks
				Thread.Sleep (500);
			}

			/// <summary>Occurs when SCANNER found any ERROR</summary>
			/// <returns>True to continue scan or false to stop scan</returns>
			[MethodImpl (MethodImplOptions.Synchronized)]
			protected virtual bool OnError ( SCAN_ERROR e ) => ( !_cts.IsCancellationRequested );
		}

		/// <summary>Multithread File Searcher</summary>
		internal class FileSearcher : uom.I_O.FileSystemScannerBase
		{
			public FileSearcher ( int iParralelTasks = 20 ) : base () { }

			private readonly List<FileInfo> _lResult = new (1000);
			private string[] _aFilesToFind = Array.Empty<string> ();


			private Func<FileInfo, bool>? _cbOnFileFound = null;
			private Func<DirectoryInfo, bool>? _cbOnOnEnterDir = null;
			private Func<DirectoryInfo[], DirectoryInfo[]>? _cbOnBeforeCheckSubDirs = null;
			private Func<SCAN_ERROR, bool>? _cbOnError = null;
			private Action<SCAN_RUNNING_INFO>? _cbOnWaitTasks = null;

			public FileInfo[] SearchFiles (
				string[] aFilesToFind,
				DirectoryInfo[]? aRootDirsToScan = null,
				Func<DirectoryInfo, bool>? fuOnEnterDir = null,
				Func<DirectoryInfo[], DirectoryInfo[]>? fuOnBeforeCheckSubDirs = null,
				Func<FileInfo, bool>? fuFileFound = null,
				Func<SCAN_ERROR, bool>? fuOnError = null,
				Action<SCAN_RUNNING_INFO>? OnWaitTasks = null,
				int? maxDegreeOfParallelism = null
				)
			{
				_ = aFilesToFind ?? throw new ArgumentNullException (nameof (aFilesToFind));
				_aFilesToFind = aFilesToFind.Where (( f ) => f.IsNotNullOrWhiteSpace ()).ToArray ();
				if (!_aFilesToFind.Any ())
				{
					throw new ArgumentNullException (nameof (aFilesToFind));
				}

				_cbOnOnEnterDir = fuOnEnterDir;
				_cbOnBeforeCheckSubDirs = fuOnBeforeCheckSubDirs;
				_cbOnFileFound = fuFileFound;
				_cbOnError = fuOnError;
				_cbOnWaitTasks = OnWaitTasks;

				_lResult.Clear ();
				base.Scan (aRootDirsToScan, maxDegreeOfParallelism);

				var aFiles = ( from L in _lResult orderby L.FullName select L ).ToArray ();
				return aFiles;
			}


			protected override bool OnEnterDir ( DirectoryInfo D )
			{
				if (!base.OnEnterDir (D))
				{
					return false;
				}

				var bContinueToSearch = _cbOnOnEnterDir?.Invoke (D);
				return bContinueToSearch.eToBool ();
			}

			protected override DirectoryInfo[] OnBeforeCheckSubDirs ( DirectoryInfo[] aDirs ) =>
				( _cbOnBeforeCheckSubDirs == null ) ? base.OnBeforeCheckSubDirs (aDirs) : _cbOnBeforeCheckSubDirs.Invoke (aDirs);

			protected override bool OnFileFound ( FileInfo F )
			{
				if (!base.OnFileFound (F))
				{
					return false;
				}

				var aFound = _aFilesToFind.Where (( s ) => s.ToLower ().Equals (F.Name.ToLower ()));
				if (!aFound.Any ())
				{
					return true;
				}

				_lResult.Add (F);

				var bContinueToSearch = _cbOnFileFound?.Invoke (F);
				return bContinueToSearch.eToBool ();
			}

			protected override bool OnError ( SCAN_ERROR e )
			{
				return ( _cbOnError == null ) ? base.OnError (e) : _cbOnError.Invoke (e);
			}

			protected override void OnWaitTasks ( SCAN_RUNNING_INFO e )
			{
				if (null == _cbOnWaitTasks) { base.OnWaitTasks (e); return; }
				_cbOnWaitTasks?.Invoke (e);
			}






			public static async Task<FileInfo[]> SearchFileAsync (
				string[] aFilesToFind,
				DirectoryInfo[]? aRootDirsToScan = null,
				Func<DirectoryInfo, bool>? fuOnEnterDir = null,
				Func<DirectoryInfo[], DirectoryInfo[]>? fuOnBeforeCheckSubDirs = null,
				Func<FileInfo, bool>? fuFileFound = null,
				Func<SCAN_ERROR, bool>? fuOnError = null,
				Action<SCAN_RUNNING_INFO>? OnWaitTasks = null,
				int? maxDegreeOfParallelism = null
				)

			{
				var FS = new FileSearcher ();
				using (var tskSearch = new Task<FileInfo[]> (
					() => FS.SearchFiles (
						aFilesToFind,
						aRootDirsToScan,
						fuOnEnterDir,
						fuOnBeforeCheckSubDirs,
						fuFileFound,
						fuOnError,
						OnWaitTasks,
						maxDegreeOfParallelism
					), TaskCreationOptions.LongRunning))
				{
					tskSearch.Start ();
					var aFound = await tskSearch;
					return aFound;
				};
			}

			public static async Task<FileInfo[]> SearchFileAsync (
			   string sFileToFind,
			   DirectoryInfo[]? aRootDirsToScan = null,
				Func<DirectoryInfo, bool>? fuOnEnterDir = null,
				Func<DirectoryInfo[], DirectoryInfo[]>? fuOnBeforeCheckSubDirs = null,
				Func<FileInfo, bool>? fuFileFound = null,
				Func<SCAN_ERROR, bool>? fuOnError = null,
				Action<SCAN_RUNNING_INFO>? OnWaitTasks = null,
				int? maxDegreeOfParallelism = null
				)
			{
				var aFiles = new string[] { sFileToFind };
				return await SearchFileAsync (
					aFiles,
					aRootDirsToScan,
					fuOnEnterDir,
					fuOnBeforeCheckSubDirs,
					fuFileFound,
					fuOnError,
					OnWaitTasks,
					maxDegreeOfParallelism);
			}
		}


		/// <summary>Reads file lines in background thread.
		/// Usable fo read any log files</summary>
		internal class BackgroundLogFileReader
		{
			public event EventHandler<string> LineRead = delegate { };
			public event EventHandler<Exception> IOError = delegate { };

			private FileInfo? _File = null;
			private StreamReader? _SR = null;

			public volatile bool StopFlag = false;

			private Thread? _thRead = null;
			private readonly ManualResetEvent _evtFinished = new (false);

			public bool DeleteFileOnFinish { get; private set; } = false;



			public BackgroundLogFileReader ( string sPath, bool bDeleteFileOnFinish = false ) : base ()
			{
				DeleteFileOnFinish = bDeleteFileOnFinish;
				_File = sPath.eToFileInfo ();
				_SR = _File!.eCreateReader ();
				StartCore (sPath);
			}
			public BackgroundLogFileReader ( Stream S ) : base ()
			{
				DeleteFileOnFinish = false;
				_SR = S.eCreateReader ();
				StartCore ("[Stream]");
			}

			private void StartCore ( string ThreadName )
			{
				_thRead = new Thread (MainReadThread);
				{
					_thRead.IsBackground = true;
					_thRead.Name = $"Reading '{ThreadName}' lines in background thread...";
					_thRead.Start ();
				}
			}

			private bool IsFile => ( _File != null );

			private void MainReadThread ()
			{


				try
				{
					ReadStream ();
				}
				catch (Exception ex)
				{
					IOError?.Invoke (this, ex);
				}
				finally
				{
					if (IsFile)
					{
						try { _SR!.Dispose (); } catch { }
						if (DeleteFileOnFinish)
						{
							try { _File!.Delete (); } catch { }
						}

						_SR = null;
						_File = null;
					}
					_evtFinished.Set ();
				}
			}


			private void ReadStream ()
			{
				while (!StopFlag)
				{
					try
					{
						string? sLine = _SR!.ReadLine ();
						if (null != sLine)
						{
							LineRead?.Invoke (this, sLine!);
						}
						else
						{
							Thread.Sleep (100);
						}
					}
					catch { }// Just Ignore error on read single line and try read next line
				}
			}

			public void Stop ( int iTimeout = System.Threading.Timeout.Infinite )
			{
				StopFlag = true;
				_evtFinished.WaitOne (iTimeout);
			}
		}
	}





#pragma warning disable IDE1006 // Naming Styles

	namespace Extensions
	{


		internal static partial class Extensions_NumericConversions
		{

			/// <summary>Checks if new value is not equal to old value and updates old value to new value.</summary>
			/// <returns>true if value was updated</returns>
			public static bool eUpdateIfNotEquals<T> ( this ref T oldValue, T newValue, Action? onUpdatedCallback ) where T : struct
			{
				if (oldValue.Equals (newValue))
				{
					return false;
				}

				oldValue = newValue;
				onUpdatedCallback?.Invoke ();
				return true;
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static T IIF<T> ( this bool Expression, T TrueResult, T FalseResult ) => Expression ? TrueResult : FalseResult;



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool eToBool ( this bool? expr )
				=> ( null != expr ) && expr.HasValue && expr.Value;


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool eToBool ( this bool expr ) => expr;


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool eToBool ( this int iValue ) => ( 0 != iValue );









			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static int eToInt32ABS ( this bool bValue ) => (Int32) ( bValue ? 1 : 0 );











			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static int[] eRangeTo ( this int iFrom, int iTo ) => Enumerable.Range (iFrom, iTo).ToArray ();








			/// <summary>Чётное</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eIsEven ( this int N ) => N % 2 == 0;


			/// <summary>Нечётное</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eIsOdd ( this int N ) => ( !N.eIsEven () );


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eIsКратно ( this int Value, int ЧемуКратно )
				=> ( Value % ЧемуКратно ) == 0;


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eIsКратно ( this long Value, long ЧемуКратно )
				=> ( Value % ЧемуКратно ) == 0;


			#region CheckRange

			/*             
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static int eCheckRange(this int value, int minValue = 0, int maxValue = 100)
			{
				if (value < minValue) value = minValue;
				else if (value > maxValue) value = maxValue;
				return value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Int64 eCheckRange(this Int64 value, Int64 minValue = 0, Int64 maxValue = 100)
			{
				if (value < minValue) value = minValue;
				else if (value > maxValue) value = maxValue;
				return value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static float eCheckRange(this float value, float minValue = 0, float maxValue = 100)
			{
				if (value < minValue) value = minValue;
				else if (value > maxValue) value = maxValue;
				return value;
			}
			*/

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T eCheckRange<T> ( this T Value, T? MinLimit = default, T? MaxLimit = default ) where T : struct, IComparable
			{
				try
				{
					if (null != MinLimit && MinLimit.HasValue && Value.CompareTo (MinLimit.Value) < 0)
					{
						Value = MinLimit.Value;
					}
					else if (null != MaxLimit && MaxLimit.HasValue && Value.CompareTo (MaxLimit.Value) > 0)
					{
						Value = MaxLimit.Value;
					}

					return Value;
				}
				catch
				{
					return MinLimit.eValueOrNull () ?? default;
				}
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T? eValueOrNull<T> ( this T? v ) where T : struct
				=> ( null != v && v.HasValue ) ? v.Value : null;

			#endregion


			private const MidpointRounding DEFAULT_ROUNDING_RULE = MidpointRounding.ToEven;

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static float eRound ( this float N, int Precision, MidpointRounding mmr = DEFAULT_ROUNDING_RULE ) => (float) Math.Round (N, Precision, mmr);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static double eRound ( this double N, int Precision, MidpointRounding mmr = DEFAULT_ROUNDING_RULE ) => Math.Round (N, Precision, mmr);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static decimal eRound ( this decimal N, int Precision, MidpointRounding mmr = DEFAULT_ROUNDING_RULE ) => Math.Round (N, Precision, mmr);










			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static uom._Int64 eToInt64 ( this long V ) => new (V);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static uom._Int64 eToInt64 ( this ulong V ) => new (V);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static uom._Int32 eToInt32 ( this int V ) => new (V);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static uom._Int32 eToInt32 ( this uint V ) => new (V);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static uom._Int16 eToInt16 ( this short V ) => new (V);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static uom._Int16 eToInt16 ( this ushort V ) => new (V);



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static T eParseAsNumeric<T> ( this string stringValue, T defaultValue, NumberStyles style = NumberStyles.None ) where T : struct
			{
				if (stringValue.IsNullOrWhiteSpace ())
				{
					return defaultValue;
				}

				object numericValue = Type.GetTypeCode (typeof (T)) switch
				{
					TypeCode.Int16 => Int16.Parse (stringValue, style),
					TypeCode.Int32 => Int32.Parse (stringValue, style),
					TypeCode.Int64 => Int64.Parse (stringValue, style),

					TypeCode.UInt16 => UInt16.Parse (stringValue, style),
					TypeCode.UInt32 => UInt32.Parse (stringValue, style),
					TypeCode.UInt64 => UInt64.Parse (stringValue, style),

					TypeCode.Decimal => Decimal.Parse (stringValue, style),
					TypeCode.Double => Double.Parse (stringValue, style),
					TypeCode.Single => Single.Parse (stringValue, style),

					TypeCode.Byte => Byte.Parse (stringValue, style),

					_ => defaultValue
				};

				return (T) numericValue;
			}




			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static Int32 eToInt32 ( this IEnumerable<byte> ab ) => BitConverter.ToInt32 (ab.ToArray (), 0);

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static UInt32 eToUInt32 ( this IEnumerable<byte> ab ) => BitConverter.ToUInt32 (ab.ToArray (), 0);


			/// <summary>Reverse bytes from AABBCCDD to DDCCBBAA</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static UInt32 eReverseBytes ( this UInt32 a ) => a.eGetBytes ().Reverse ().eToUInt32 ();

			/// <summary>Reverse bytes from AABBCCDD to DDCCBBAA</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static Int32 eReverseBytes ( this Int32 a ) => a.eGetBytes ().Reverse ().eToInt32 ();


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static UInt64 eBinaryPow ( this UInt64 baseNumber, UInt64 exponent )
			{
				UInt64 result = 1;
				while (exponent > 0)
				{
					if (( exponent & 1 ) == 1)
					{
						result *= baseNumber;
					}

					baseNumber *= baseNumber;
					exponent >>= 1;
				}
				return result;
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static UInt32 eBinaryPow ( this UInt32 baseNumber, UInt32 exponent )
			{
				UInt32 result = 1;
				while (exponent > 0)
				{
					if (( exponent & 1 ) == 1)
					{
						result *= baseNumber;
					}

					baseNumber *= baseNumber;
					exponent >>= 1;
				}
				return result;
			}



			internal const UInt64 C_BYTES_IN_KBYTE = 1024UL;
			internal const UInt64 C_BYTES_IN_MBYTE = 1048576UL;
			internal const UInt64 C_BYTES_IN_GBYTE = 1073741824UL;

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static UInt64 eKBToBytes ( this UInt32 mb ) => (UInt64) mb * C_BYTES_IN_KBYTE;


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static UInt64 eMBToBytes ( this UInt32 mb ) => (UInt64) mb * C_BYTES_IN_MBYTE;

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static UInt64 eMBToBytes ( this UInt64 mb ) => mb * C_BYTES_IN_MBYTE;




			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static float eInchesToMM ( this float Inches ) => Inches * constants.C_MM_IN_INCH;


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static float eMMToInches ( this float mm ) => mm / constants.C_MM_IN_INCH;


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static float eInchesToCM ( this float inches ) => inches * constants.C_CM_IN_INCH;


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static float eCMToInches ( this float cm ) => cm / constants.C_CM_IN_INCH;

		}


		internal static partial class Extensions_Debug_Dump
		{



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eToHexDump ( this byte[] data, ulong startAddress = ulong.MinValue, int elementsInLine = 16 )
			{
				if (data == null || data.Length < 1)
				{
					return "[NULL OR EMPTY]";
				}

				StringBuilder sbResult = new ();

				int fullLineLen = BitConverter.ToString (( new string ('-', elementsInLine) ).eGetBytes_ASCII ()).Length;
				ulong rowAddress = startAddress;
				int addressLen = Marshal.SizeOf (rowAddress) * 2;

				Mem.SlidingWindow<byte> sw = new (data) { WindowSize = elementsInLine };
				do
				{
					sbResult.Append (rowAddress.ToString ("X").PadLeft (addressLen, '0')); //Address
					sbResult.Append ('|');
					sbResult.Append (sw.WindowData.eToStringHex ().PadRight (fullLineLen, '-')); //Hex Data
					sbResult.Append ('|');
					sbResult.Append (new string (sw.WindowData.Select (b => b.eToChar ('.')).ToArray ())); // Data as readable text
					sbResult.AppendLine ();
					rowAddress += (ulong) elementsInLine;

				} while (sw.SlideRight (true));

				return sbResult.ToString ();
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eToHexDump ( this IntPtr lpBuffer, int nBytes )
			=> lpBuffer
					.ePtrToBytes (nBytes)
					.eToHexDump ((ulong) lpBuffer.ToInt64 ());

#if NET
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eToHexDump ( this Span<Byte> abData, ulong startAddress = ulong.MinValue, int elementsInLine = 16 )
				=> abData.ToArray ().eToHexDump (startAddress, elementsInLine);

#endif

			/// <summary>
			/// sbyte, byte, short, ushort, int, uint, long, ulong, nint, nuint, char, float, double, decimal, or bool
			/// Any enum type
			/// Any pointer type
			/// Any user-defined struct type that contains fields of unmanaged types only.
			/// </summary>
			private static string eDumpValueToString<T> ( this T? value,
				[System.Runtime.CompilerServices.CallerArgumentExpression ("value")] string? valueName = null ) where T : unmanaged
			{

				string val = "null";
				if (value != null)
				{
					val = $"[{typeof (T)}]";
					val += $": '{value}'";
				}
				return $"{valueName} = {val}";
			}

			private static string eDumpValueToString<T> ( this string? value,
				[System.Runtime.CompilerServices.CallerArgumentExpression ("value")] string? valueName = null )
			{
				string val = "null";
				if (value != null)
				{
					val += $"string[len={value.Length}]: '{value}'";
				}
				return $"{valueName} = {val}";
			}

			/*

	  private static string eDumpValue<T>(this T[]? value,
		  [System.Runtime.CompilerServices.CallerArgumentExpression("value")] string? valueName = null) where T : unmanaged
	  {
		  string val = "null";
		  if (value.HasValue)
		  {
			  Type vt = value.Value.GetType();
			  if (vt.IsArray)
			  {
				  try
				  {
					  Array? a = (value!.Value as Array);
					  if (a == null)
						  val = "NULL_Array";
					  else
						  val = a.eDumpArrayAsString2();
				  }
				  catch (Exception ex)
				  { val = ex.Message; }
			  }
			  else
			  {
				  val = $"[{value.GetType()}]";
				  if (value is string s)
				  {
					  val += $"[len={s.Length}]";
				  }

				  val += $": '{value}'";
			  }
		  }
		  return $"{valueName} = {val}";
	  }
		  */

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eDumpObjectToString<T> ( this T? value,
				[System.Runtime.CompilerServices.CallerArgumentExpression ("value")] string? valueName = null,
				params string[] excludeMembers ) where T : class
			{
				if (value == null)
				{
					return $"{valueName} = null";
				}

				StringBuilder sb = new (2048);

				System.Type t = value.GetType ();
				sb.AppendLine ($"{valueName} = {t}:");

				MemberInfo[] members = t.GetMembers ()
					.OrderBy (m => m.Name)
					.Where (m => !excludeMembers.eContainsOrdinalIgnoreCase (m.Name))
					.ToArray ();



				if (value is System.MarshalByRefObject)
				{
					//int gggg = 9;
					//System.MarshalByRefObject mro = value;

				}



				string RWAsString ( bool r, bool w )
				{
					string rr = r ? "R" : string.Empty;
					string ww = w ? "W" : string.Empty;
					string slash = ( r && w ) ? @"/" : string.Empty;
					return $"{rr}{slash}{ww}";
				}

				foreach (MemberInfo mi in members)
				{
					string memberDump = "";
					try
					{
						object? objMemberValue = null;

						switch (mi)
						{
							case PropertyInfo pi:
								objMemberValue = pi.GetValue (value);
								memberDump = $"Property '{mi.Name}'({RWAsString (pi.CanRead, pi.CanWrite)})[{pi.PropertyType}]";
								break;

							case FieldInfo fi:
								objMemberValue = fi.GetValue (value);
								memberDump = $"Field '{fi.Name}'[{fi.FieldType}]";

								break;

							default:
								continue;
						}

						/*
						memberDump = mi switch
						{
							PropertyInfo pi => (pi.GetValue(value) ?? "").ToString(),
							FieldInfo fi => (fi.GetValue(value) ?? "").ToString(),
							_ => null
						};
						if (memberDump == null) continue;
						 */
						memberDump += $" = '{objMemberValue ?? string.Empty}'";

						bool isOwn = ( mi.DeclaringType == t );
						if (!isOwn)
						{
							memberDump = $"->[inherit from: {mi.DeclaringType}]-> " + memberDump;
						}
					}
					catch (Exception ex)
					{
						memberDump = $"{mi.Name}: {ex.Message}";
					}

					memberDump = "\t" + memberDump;
					sb.AppendLine (memberDump);

				}
				return sb.ToString ().TrimEnd ();
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eDumpStaticMembers ( this Type t, params string[] excludeMembers )
			{
				StringBuilder sb = new (2048);

				sb.AppendLine ($"{t}:");

				MemberInfo[] members = t.GetMembers (BindingFlags.Public | BindingFlags.Static)
					.OrderBy (m => m.Name)
					.Where (m => !excludeMembers.eContainsOrdinalIgnoreCase (m.Name))
					.ToArray ();

				foreach (MemberInfo mi in members)
				{
					string memberDump = "";
					try
					{
						object? objMemberValue = null;

						switch (mi)
						{
							case PropertyInfo pi:
								objMemberValue = pi.GetValue (null);
								memberDump = $"Property '{mi.Name}'[{pi.PropertyType}]";
								break;

							case FieldInfo fi:
								objMemberValue = fi.GetValue (null);
								memberDump = $"Field '{fi.Name}'[{fi.FieldType}]";

								break;

							default:
								continue;
						}


						memberDump += $" = '{objMemberValue ?? string.Empty}'";

						bool isOwn = ( mi.DeclaringType == t );
						if (!isOwn)
						{
							memberDump = $"->[inherit from: {mi.DeclaringType}]-> " + memberDump;
						}
					}
					catch (Exception ex)
					{
						memberDump = $"{mi.Name}: {ex.Message}";
					}
					memberDump = "\t" + memberDump;
					sb.AppendLine (memberDump);
				}

				return sb.ToString ().TrimEnd ();
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eDumpObjectMembersTreeToString<T> ( this T? value,
				[System.Runtime.CompilerServices.CallerArgumentExpression ("value")] string? valueName = null ) where T : class
			{

				if (value == null)
				{
					return $"{valueName} = null";
				}

				StringBuilder sb = new ();

				//WARNING!!! NOT WORKING!!!
				//return sb.ToString();

				System.Type t = value.GetType ();
				sb.AppendLine ($"{valueName} = {t}:");

				MemberInfo[] members = t.GetMembers ()
					.OrderBy (m => m.Name)
					.ToArray ();

				foreach (MemberInfo mi in members)
				{
					string memberDump = "";
					try
					{

						memberDump = mi switch
						{
							PropertyInfo pi => pi.GetValue (value).eDumpObjectToString (pi.Name),
							FieldInfo fi => fi.GetValue (value).eDumpObjectToString (fi.Name),
							_ => ""
						};

						if (memberDump.IsNullOrWhiteSpace ())
						{
							continue;
						}

						bool isOwn = ( mi.DeclaringType == t );
						if (!isOwn)
						{
							memberDump = $"->[{mi.DeclaringType}]-> " + memberDump;
						}
					}
					catch (Exception ex)
					{
						memberDump = $"{mi.Name}: {ex.Message}";
					}

					memberDump = "\t" + memberDump;
					sb.AppendLine (memberDump);

				}
				return sb.ToString ();
			}



			private const int C_DEFAULT_ARRAY_DUMP_ITEMS_COUNT = 100;


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eDumpArrayToString<T> (
				this IEnumerable<T>? src,
				string itemSeparator = ",",
				int limitArrayItemsOutput = C_DEFAULT_ARRAY_DUMP_ITEMS_COUNT,
				[System.Runtime.CompilerServices.CallerArgumentExpression ("src")] string? arrayName = null
				)
			{
				if (src == null)
				{
					return $"{arrayName} = null";
				}

				if (!src.Any ())
				{
					return $"{arrayName} = {typeof (T)}[0]";
				}

				string result = $"{arrayName} = {typeof (T)}[{src.Count ()}]";

				if (limitArrayItemsOutput > 0 && src.Count () > limitArrayItemsOutput)
				{
					src = src.Take (limitArrayItemsOutput).ToArray ();
					result += $":FIRST:{limitArrayItemsOutput}";
				}

				result += " = ";


				if (src is byte[] data)
				{
					result += data.eToStringHex ();
				}
				else if (src is string[] strArr)
				{
					result += strArr.eJoin (itemSeparator, null, true)!;
				}
				else
				{
					string[] strArr2 = src
						.Select (o =>
						( ( o == null )
							? "null"
							: o!.ToString () )!)
						.ToArray ();

					result += strArr2.eJoin (itemSeparator)!;
				}
				return result;
			}


			/*

	   internal static string eDumpArrayToString2(
		   this System.Array? src,
		   string itemSeparator = ",",
		   int limitArrayItemsOutput = C_DEFAULT_ARRAY_DUMP_ITEMS_COUNT
		   )
	   {
		   if (src == null) return "null";


		   System.Array.


		   var e = src as IEnumerable;
		   return e.eDumpArrayToString();




		   System.Type t = src.GetType();
		   if (src.Length < 1) return $"{t}[0]";

		   if (t == typeof(byte[]))
		   {
			   byte[] a = (byte[])src;
			   return a.eDumpArrayToString(itemSeparator, limitArrayItemsOutput);
		   }

		   if (t == typeof(string[]))
		   {
			   string[] a = (string[])src;
			   return a.eDumpArrayToString(itemSeparator, limitArrayItemsOutput);
		   }

		   string result = $"{t}[{src.Length}]";

		   if (limitArrayItemsOutput > 0 && src.Length > limitArrayItemsOutput)
			   result += $":FIRST:{limitArrayItemsOutput}";

		   result += " = ";

		   IEnumerable ie = src;
		   List<string> l = new();
		   foreach (var o in ie)
		   {
	#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
			   string s = (o ?? "null").ToString();
	#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
	#pragma warning disable CS8604 // Possible null reference argument.
			   l.Add(s);
	#pragma warning restore CS8604 // Possible null reference argument.
			   if (l.Count >= limitArrayItemsOutput) break;
		   }
		   result += l.ToArray().eJoin(itemSeparator)!;
		   return result;
	   }
				  */





		}


		internal static partial class Extensions_Binary_Hex
		{


			#region From BitArray Source (https://github.com/dotnet/corefx/blob/76f566a281bbe979e80cbbb3a48ddf522cdcb4e1/src/System.Collections/src/System/Collections/BitArray.cs#L19)


			// XPerY=n means that n Xs can be stored in 1 Y.
			private const int BitsPerInt32 = 32;
			private const int BitsPerByte = 8;

			private const int BitShiftPerInt32 = 5;
			private const int BitShiftPerByte = 3;
			private const int BitShiftForBytesPerInt32 = 2;

			/// <summary>
			/// Used for conversion between different representations of bit array.
			/// Returns (n + (32 - 1)) / 32, rearranged to avoid arithmetic overflow.
			/// For example, in the bit to int case, the straightforward calc would
			/// be (n + 31) / 32, but that would cause overflow. So instead it's
			/// rearranged to ((n - 1) / 32) + 1.
			/// Due to sign extension, we don't need to special case for n == 0, if we use
			/// bitwise operations (since ((n - 1) >> 5) + 1 = 0).
			/// This doesn't hold true for ((n - 1) / 32) + 1, which equals 1.
			///
			/// Usage:
			/// GetArrayLength(77): returns how many ints must be
			/// allocated to store 77 bits.			
			/// </summary>
			/// <param name="bitLength"></param>
			/// <returns>how many ints are required to store n bytes</returns>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			private static int GetInt32ArrayLengthFromBitLength ( int bitLength )
			{
				Debug.Assert (bitLength >= 0);
				return (int) ( (uint) ( bitLength - 1 + ( 1 << BitShiftPerInt32 ) ) >> BitShiftPerInt32 );
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			private static int GetInt32ArrayLengthFromByteLength ( int byteLength )
			{
				Debug.Assert (byteLength >= 0);
				// Due to sign extension, we don't need to special case for n == 0, since ((n - 1) >> 2) + 1 = 0
				// This doesn't hold true for ((n - 1) / 4) + 1, which equals 1.
				return (int) ( (uint) ( byteLength - 1 + ( 1 << BitShiftForBytesPerInt32 ) ) >> BitShiftForBytesPerInt32 );
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			private static int GetByteArrayLengthFromBitLength ( int n )
			{
				Debug.Assert (n >= 0);
				// Due to sign extension, we don't need to special case for n == 0, since ((n - 1) >> 3) + 1 = 0
				// This doesn't hold true for ((n - 1) / 8) + 1, which equals 1.
				return (int) ( (uint) ( n - 1 + ( 1 << BitShiftPerByte ) ) >> BitShiftPerByte );
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			private static int Div32Rem ( int number, out int remainder )
			{
				uint quotient = (uint) number / 32;
				remainder = number & ( 32 - 1 );    // equivalent to number % 32, since 32 is a power of 2
				return (int) quotient;
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			private static int Div4Rem ( int number, out int remainder )
			{
				uint quotient = (uint) number / 4;
				remainder = number & ( 4 - 1 );   // equivalent to number % 4, since 4 is a power of 2
				return (int) quotient;
			}


			#endregion


			#region HIWORD/LOWORD/MAKELPARAM

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static uint eMAKELPARAM ( this ushort LoWord16, ushort HiWord16 ) => (uint) ( (uint) HiWord16 << 16 | LoWord16 & 0xFFFFL );


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static uint eMakeLong ( this ushort LoWord16, ushort HiWord16 ) => LoWord16.eMAKELPARAM (HiWord16);


			//e_HiWord
			//if ((UI32 & 0x80000000U) == 0x80000000U) return (ushort)(UI32 >> 16);
			//else return (ushort)(UI32 >> 16 & 0xFFFFu);
			//internal static ushort eLoWord(this uint UI32) => (ushort)(UI32 & 0xFFFFL);
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static ushort eLoWord ( this uint i ) => new _Int32 (i).ULoWord;

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static short eLoWord ( this int i ) => new _Int32 (i).LoWord;

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static ushort eHiWord ( this uint i ) => new _Int32 (i).UHiWord;

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static short eHiWord ( this int i ) => new _Int32 (i).HiWord;


			#endregion


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static long eMakeFourCC ( this byte ch0, byte ch1, byte ch2, byte ch3 )
			{
				throw new NotImplementedException ();
				// Dim lRes&, lVal&
				// lRes = ch0

				// lVal = ch1
				// lVal = lVal * (2 ^ 8) : lRes = lRes Or lVal

				// lVal = ch2
				// lVal = lVal * (2 ^ 16) : lRes = lRes Or lVal

				// lVal = ch3
				// Dim A&
				// A = 2 ^ 24
				// MsgBox A, , Hex(A)
				// lVal = lVal * (2 ^ 24)
				// lRes = lRes Or lVal

				// Return lRes
			}



			private const string C_BAD_BYTE_SEPARATOR_CHARS = @" :_|./*+,\~`'=";
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eNormalizeHexString ( this string HexString, string? BadSeparators = null, char? GoodHexSeparator = null )
			{
				if (HexString.IsNullOrWhiteSpace ())
				{
					return HexString;
				}

				BadSeparators ??= C_BAD_BYTE_SEPARATOR_CHARS;
				GoodHexSeparator ??= constants.SystemDefaultHexByteSeparator.Value;

				return HexString.Trim ().ToUpper ().eReplaceAll (BadSeparators.ToCharArray ().eToStrings (), GoodHexSeparator!.ToString ()!);
			}


			/// <summary>Получаем массив байт из строки</summary>
			/// <param name="HexString">Строка вида: 0D-0A-2B / 43:53:51:3A / 43.53.51.3A / </param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static byte[] eHexStringToBytes ( this string HexString, string ByteSeparatorChars = C_BAD_BYTE_SEPARATOR_CHARS )
			{
				if (HexString.IsNullOrWhiteSpace ())
				{
					return Array.Empty<byte> ();
				}

				string[] hexBytedStrings = HexString
					.eNormalizeHexString (ByteSeparatorChars, constants.SystemDefaultHexByteSeparator.Value)
					.Trim ()
					.Split (constants.SystemDefaultHexByteSeparator.Value); // Делим по разделителю на отдельные элементы

				if (!hexBytedStrings.Any ())
				{
					return Array.Empty<byte> ();
				}

				return hexBytedStrings
					.Select (sByte => byte.Parse (sByte, NumberStyles.HexNumber))
					.ToArray ();
			}



			#region BitArray / Bits


			#region Get / Set Bit




			/// <summary>Sets specifed bit by index</summary>
			/// <param name="bitIndex">Zero-baset bit index</param>			
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eSetBitRef ( this ref byte a, int bitIndex, bool bitValue = true )
			{
				if (bitValue)
				{
					a |= (byte) ( 0x1 << bitIndex );
				}
				else
				{
					a &= (byte) ~( 0x1 << bitIndex );
				}
			}

			/// <inheritdoc cref="eSetBitRef(ref byte, int, bool)" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eSetBitRef ( this ref UInt32 a, int bitIndex, bool bitValue = true )
			{
				if (bitValue)
				{
					a |= ( 1U << bitIndex );
				}
				else
				{
					a &= ~( 1U << bitIndex );
				}
			}

			/// <inheritdoc cref="eSetBitRef(ref byte, int, bool)" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eSetBitRef ( this ref Int32 a, int bitIndex, bool bitValue = true )
			{
				if (bitValue)
				{
					a |= ( 1 << bitIndex );
				}
				else
				{
					a &= ~( 1 << bitIndex );
				}
			}

			/// <inheritdoc cref="eSetBitRef(ref byte, int, bool)" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eSetBitRef ( this ref UInt64 a, int bitIndex, bool bitValue = true )
			{
				if (bitValue)
				{
					a |= ( 1UL << bitIndex );
				}
				else
				{
					a &= ~( 1UL << bitIndex );
				}
			}

			/// <inheritdoc cref="eSetBitRef(ref byte, int, bool)" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eSetBitRef ( this ref Int64 a, int bitIndex, bool bitValue = true )
			{
				if (bitValue)
				{
					a |= ( 1L << bitIndex );
				}
				else
				{
					a &= ~( 1L << bitIndex );
				}
			}


			/// <summary>Sets specifed bit by index</summary>
			/// <param name="bitIndex">Zero-baset bit index</param>			
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static UInt32 eSetBit ( this UInt32 a, int bitIndex ) => a | ( 1U << bitIndex );

			/// <inheritdoc cref="eSetBit(UInt32, int)" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static Int32 eSetBit ( this Int32 a, int bitIndex ) => a | ( 1 << bitIndex );

			/// <inheritdoc cref="eSetBit(UInt32, int)" />
			/// <param name="bitIndex">Zero-baset bit index</param>			
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static UInt64 eSetBit ( this UInt64 a, int bitIndex ) => a | ( 1UL << bitIndex );

			/// <inheritdoc cref="eSetBit(UInt32, int)" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static Int64 eSetBit ( this Int64 a, int bitIndex ) => a | ( 1L << bitIndex );


			/// <summary>Gets specifed bit by index</summary>
			/// <param name="bitIndex">Zero-baset bit index</param>			
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eGetBit ( this UInt32 a, int bitIndex ) => ( a & ( 1U << bitIndex ) ) != 0;

			/// <inheritdoc cref="eGetBit(UInt32, int)" />"
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eGetBit ( this Int32 a, int bitIndex ) => ( a & ( 1 << bitIndex ) ) != 0;

			/// <inheritdoc cref="eGetBit(UInt32, int)" />"
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eGetBit ( this UInt64 a, int bitIndex ) => ( a & ( 1UL << bitIndex ) ) != 0;

			/// <inheritdoc cref="eGetBit(UInt32, int)" />"
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eGetBit ( this Int64 a, int bitIndex ) => ( a & ( 1L << bitIndex ) ) != 0;


			#endregion


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static byte[] eGetBytes ( this long value ) => BitConverter.GetBytes (value);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static byte[] eGetBytes ( this ulong value ) => BitConverter.GetBytes (value);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static byte[] eGetBytes ( this int value ) => BitConverter.GetBytes (value);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static byte[] eGetBytes ( this uint value ) => BitConverter.GetBytes (value);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static byte[] eGetBytes ( this short value ) => BitConverter.GetBytes (value);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static byte[] eGetBytes ( this ushort value ) => BitConverter.GetBytes (value);



			/// <summary>Divides value to divideTo with rounding to the nearest max int</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static uint eDivideWithRoundToMax ( this uint value, uint divideTo )
			{
				uint floor = value % divideTo;
				uint newVal = value / divideTo;
				if (floor > 0)
				{
					newVal++;
				}

				return newVal;
			}

			/// <summary>Divides value to divideTo with rounding to the nearest max int</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static int eDivideWithRoundToMax ( this int value, int divideTo )
			{
				int floor = value % divideTo;
				int newVal = value / divideTo;
				if (floor > 0)
				{
					newVal++;
				}

				return newVal;
			}


			/// <summary>Переводит биты в числовое значение. Количество байт, рассчитывается как число_бит/8</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static byte[] eGetBytes ( this BitArray ba )
			{
				//var bytesCount = (int)Math.Round(Math.Ceiling(ba.Count / 8f));
				int bytesCount = ba.Count.eDivideWithRoundToMax (8);
				var abMaxIP = new byte[ bytesCount ];
				ba.CopyTo (abMaxIP, 0);
				return abMaxIP;
			}


			/// <summary>Переводит биты в числовое значение. Количество байт, рассчитывается как число_бит/8</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static byte[] eGetBytes ( this IEnumerable<bool> bits ) => ( new BitArray (bits.ToArray ()) ).eGetBytes ();


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static BitArray eToBitArray ( this byte value ) => new (new byte[] { value });

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static BitArray eToBitArray ( this Int64 value ) => new (value.eGetBytes ());

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static BitArray eToBitArray ( this UInt64 value ) => new (value.eGetBytes ());

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static BitArray eToBitArray ( this Int32 value ) => new (value.eGetBytes ());

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static BitArray eToBitArray ( this UInt32 value ) => new (value.eGetBytes ());


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static BitArray eToBitArray ( this UInt16 value ) => new (value.eGetBytes ());


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static BitArray eToBitArray ( this Int16 value ) => new (value.eGetBytes ());




			//internal static bool[] eGetBits(this BitArray ba) => [.. ba.Cast<bool>()];
			/// <summary>Returns bits array in double reversed mode (See Bitarray), lower bit starts (0x1 = '10000000')</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool[] eGetBits ( this BitArray ba )
			{
				bool[] bytes = new bool[ ba.Length ];
				ba.CopyTo (bytes, 0);
				return bytes;
			}

			/// <inheritdoc cref="eGetBits(BitArray)" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool[] eGetBits ( this Int32 value ) => value.eToBitArray ().eGetBits ();

			/// <inheritdoc cref="eGetBits(BitArray)" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool[] eGetBits ( this UInt32 value ) => value.eToBitArray ().eGetBits ();

			/// <inheritdoc cref="eGetBits(BitArray)" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool[] eGetBits ( this Int64 value ) => value.eToBitArray ().eGetBits ();

			/// <inheritdoc cref="eGetBits(BitArray)" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool[] eGetBits ( this UInt64 value ) => value.eToBitArray ().eGetBits ();

			/// <inheritdoc cref="eGetBits(BitArray)" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool[] eGetBits ( this Int16 value ) => value.eToBitArray ().eGetBits ();

			/// <inheritdoc cref="eGetBits(BitArray)" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool[] eGetBits ( this byte value ) => value.eToBitArray ().eGetBits ();

















			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eSetBitsFromStartToEnd ( this BitArray bits, int startIndex, int endIndex, bool value = true )
			{
				for (int i = startIndex ; i < endIndex ; i++)
				{
					bits.Set (i, value);
				}
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eSetBitsFromStart ( this BitArray bits, int startIndex, int count, bool value = true )
				=> bits.eSetBitsFromStartToEnd (startIndex, startIndex + count, value);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static Byte[] eSetBitsFromStartToEnd ( this Byte[] bytes, int startIndex, int endIndex, bool value = true )
			{
				BitArray bits = new (bytes);
				bits.eSetBitsFromStartToEnd (startIndex, endIndex, value);
				bits.CopyTo (bytes, 0);
				return bytes;
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static Byte[] eSetManyFromStart ( this Byte[] bytes, int startIndex, int count, bool value = true )
				=> bytes.eSetBitsFromStartToEnd (startIndex, startIndex + count, value);




			/// <summary>Returns string of bitss like 0000-0001</summary>
			/// <param name="groupSize">Octet size which will be saparated with groupsSeparator</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eToBitsString ( this BitArray ba, bool reorderToHumanReadableView = true, char char_0 = '0', char char_1 = '1', int groupSize = 8, string groupsSeparator = "-" )
			{
				bool[] aBoolValues = ba.eGetBits ();
				Span<bool> bitsSpan = new (aBoolValues);

				List<string> octetsList = [];
				int slicePos = 0;
				do
				{
					Span<bool> slice = bitsSpan.Slice (slicePos, groupSize);
					bool[] bits = [ .. slice ];
					char[] cc = [ .. bits.Select (b => b ? char_1 : char_0) ];
					string byteBitsString = new (cc);
					octetsList.Add (byteBitsString);
					slicePos += groupSize;
				}
				while (slicePos < bitsSpan.Length);
				string r = octetsList.eJoin (groupsSeparator)!.Trim ();
				if (reorderToHumanReadableView)
				{
					r = new string (r.Reverse ().ToArray ());
				}

				return r;
			}

#if NET


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static System.Numerics.BigInteger eToBigInteger ( this uint value ) => new (value);

#endif

			#endregion










			#region Srtings Byte Functioms


			/// <inheritdoc cref="System.Convert.ToBase64String(byte[])"/>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eToBase64String ( this byte[] data ) => Convert.ToBase64String (data);

			/// <inheritdoc cref="System.Convert.FromBase64String"/>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static byte[] eFromBase64String ( this string base64String ) => Convert.FromBase64String (base64String);



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static byte[] eGetBytes_Default ( this string sData ) => Encoding.Default.GetBytes (sData);

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static byte[] eGetBytes_ASCII ( this string sData ) => Encoding.ASCII.GetBytes (sData);

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static byte[] eGetBytes_Unicode ( this string sData ) => Encoding.Unicode.GetBytes (sData);

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eGetBytes_Unicode_ToBase64String ( this string sData ) => Convert.ToBase64String (sData.eGetBytes_Unicode ());


			#endregion


		}


		internal static partial class Extensions_RegEx
		{


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static T eParseRegexValueAsNumeric<T> ( this GroupCollection g, string groupName, T defaultValue, NumberStyles style = NumberStyles.None ) where T : struct
				=> ( g[ groupName ].Value ?? "" ).eParseAsNumeric (defaultValue, style);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static T eParseRegexValueAsNumeric<T> ( this Match mx, string groupName, T defaultValue, NumberStyles style = NumberStyles.None ) where T : struct
				=> mx.Groups.eParseRegexValueAsNumeric<T> (groupName, defaultValue, style);

		}


		internal static partial class Extensions_StringAndFormat
		{

			// (parenthesis)
			// [brackets]
			// {curly brackets}.

			public const string DEFAULT_REPLACE_WITH = "";
			public const StringComparison DEFAULT_STRING_COMPARSION = StringComparison.OrdinalIgnoreCase;
			private const byte FIRST_READABLE_CHAR = 32;



			/// <returns>string.Empty if sourceText is null</returns>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eToNonNull ( this string? sourceText ) => ( sourceText ?? string.Empty );

			/// <inheritdoc cref="string.IsNullOrEmpty"/>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool IsNullOrEmpty ( this string? sourceText ) => string.IsNullOrEmpty (sourceText);


			/// <inheritdoc cref="string.IsNullOrWhiteSpace"/>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool IsNullOrWhiteSpace ( this string? sourceText ) => string.IsNullOrWhiteSpace (sourceText!);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool IsNotNullOrWhiteSpace ( this string? sourceText ) => ( !sourceText.IsNullOrWhiteSpace () );


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool IsNotNullOrWhiteSpaceAndStartsWith ( this string? sourceText, string findWhat, StringComparison sc = StringComparison.InvariantCultureIgnoreCase )
				=> sourceText.IsNotNullOrWhiteSpace () && sourceText!.StartsWith (findWhat, sc);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool IsNotNullOrWhiteSpaceAndEndsWith ( this string? sourceText, string findWhat, StringComparison sc = StringComparison.InvariantCultureIgnoreCase )
				=> sourceText.IsNotNullOrWhiteSpace () && sourceText!.EndsWith (findWhat, sc);




			[MethodImpl (MethodImplOptions.NoOptimization | MethodImplOptions.AggressiveInlining)]
			public static bool eAssert_NullOrWhiteSpace (
				this string? source,
				[System.Runtime.CompilerServices.CallerArgumentExpression (nameof (source))] string? valueName = null )
				=> !string.IsNullOrWhiteSpace (source)
					? true
					: throw new ArgumentNullException (valueName);

			[MethodImpl (MethodImplOptions.NoOptimization | MethodImplOptions.AggressiveInlining)]
			public static bool eAssert_NullOrEmpty (
				this string? source,
				[System.Runtime.CompilerServices.CallerArgumentExpression (nameof (source))] string? valueName = null )
				=> !string.IsNullOrEmpty (source)
					? true
					: throw new ArgumentNullException (valueName);

			[MethodImpl (MethodImplOptions.NoOptimization | MethodImplOptions.AggressiveInlining)]
			public static bool eAssert_Null<T> (
				this T? source,
				[System.Runtime.CompilerServices.CallerArgumentExpression (nameof (source))] string? valueName = null ) where T : class
				=> ( source != null )
					? true
					: throw new ArgumentNullException (valueName);





			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eReverse ( this string source )
				=> new ([ .. source.Reverse () ]);


			/// <returns>'{source}'</returns>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eEncloseWithCurlyBrackets ( this string source )
				=> '{' + source.ToString () + '}';


			/// <returns>'{Guid}'</returns>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eEncloseWithCurlyBrackets ( this Guid source )
				=> source.ToString ().eEncloseWithCurlyBrackets ();


			/// <summary>true if 'A' and 'a'</summary>
			internal static bool eIsDifferOnlyByCase ( this string? s1, string? s2 )
				=> !s1.IsNotNullOrWhiteSpace () && !s2.IsNotNullOrWhiteSpace ()
				&& s1!.Equals (s2, StringComparison.OrdinalIgnoreCase)
				&& !s1!.Equals (s2, StringComparison.InvariantCulture);


			/// <inheritdoc cref="string.Join(string?, IEnumerable{string?})"/>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string? eJoin (
				this IEnumerable<string>? source,
				string separator = " ",
				string? emptyOrNullValue = null,
				bool enquoteAllStrings = false
				)
			{
				if (( null == source ) || !source!.Any ()) return emptyOrNullValue;

				if (enquoteAllStrings)
					source = [ .. source.Select (s => '"' + s + '"') ];

				return string.Join (separator, source);
			}

			/// <inheritdoc cref="string.Join(string?, IEnumerable{string?})"/>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eJoin (
				this System.Collections.Specialized.StringCollection? source,
				string separator = " ",
				string? emptyOrNullValue = null,
				bool enquoteAllStrings = false
				)
				=> ( null == source )
					? string.Empty
					: source!.Cast<string> ().eJoin (separator, emptyOrNullValue, enquoteAllStrings)!;


			/// <summary>Make string.Format(sFormatString, Args)</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eRepeat ( this char c, int Length = 70 )
				=> new (c, Length);



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<string> eIndent ( this string source, int level = 1, Char indentChar = '\t' )
			{
				foreach (var s in source.eReadLines ())
				{
					yield return ( new string (indentChar, level) + s );
				}
			}


			/// <returns>
			/// if (source = null) return nullValue;<br/>
			/// if (source = empty) return emptyValue;<br/>
			/// return source;
			/// </returns>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eFormatAllowNullOrEmpty (
				this string? source,
				string nullValue = "null",
				string emptyValue = "''" )
			{
				if (source == null) return nullValue;
				if (source.IsNullOrEmpty ()) return emptyValue;
				return source;
			}


			/// <inheritdoc cref="string.Format(string, object?[])" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eFormat ( this string formatString, params object[] args )
				=> string.Format (formatString, args);



#if NET


			private static readonly string[] TIMESPAN_PARTS_EN = [ "d", "h", "m", "s", "ms" ];
			private static readonly string[] TIMESPAN_PARTS_RU = [ "д", "ч", "м", "с", "мс" ];



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eFormatTimespan ( this TimeSpan source, bool useMilliseconds = false, bool skipZeroParts = false )
			{
				string[] parts = uom.AppInfo.CurrentUICultureIsRuTree
					? TIMESPAN_PARTS_RU
					: TIMESPAN_PARTS_EN;

				StringBuilder sb = new (200);
				if (source.Days > 0)
				{
					sb.Append ($"{source.Days}{parts[ 0 ]},");
				}

				if (source.Hours > 0)
				{
					sb.Append ($"{source.Hours}{parts[ 1 ]}:");
				}

				if (source.Minutes > 0)
				{
					sb.Append ($"{source.Minutes}{parts[ 2 ]}:");
				}

				if (source.Seconds > 0)
				{
					sb.Append ($"{source.Seconds}{parts[ 3 ]}");
				}

				if (useMilliseconds)
				{
					sb.Append ($":{source.Milliseconds}{parts[ 4 ]}");
				}

				return sb
					.ToString ()
					.eReplaceAll ("::", ":")
					.Trim ()
					.TrimEnd (':')
					.TrimEnd (',');
			}



#endif

			private static readonly string[] ByteSize_En = [ "B", "KB", "MB", "GB", "TB", "PB", "EB" ];
			private static readonly string[] ByteSize_Ru = [ "Б", "КБ", "МБ", "ГБ", "ТБ", "ПБ", "ЕБ" ];
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eFormatByteSize ( this UInt64 bytesLength, int decimalPlaces = constants.C_DEFAULT_DECIMAL_DIGITS )
			{
				string[] sizes = uom.AppInfo.CurrentUICultureIsRuTree
					? ByteSize_Ru
					: ByteSize_En;

				double dblLen = (double) bytesLength;
				int order = 0;
				while (dblLen >= 1024 && ( order < sizes.Length - 1 ))
				{
					order++;
					dblLen /= 1024;
				}
				// Adjust the format string to your preferences. For example "{0:0.#}{1}" would show a single decimal place, and no space.
				string format = ( decimalPlaces > 0 ) ? ( "{0:0." + new string ('#', decimalPlaces) + "} {1}" ) : "{0:0} {1}";
				var result = string.Format (format, dblLen, sizes[ order ]);
				return result;
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eFormatByteSize ( this Int64 bytesLength, int decimalPlaces = constants.C_DEFAULT_DECIMAL_DIGITS )
				=> ( (UInt64) bytesLength ).eFormatByteSize (decimalPlaces);

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eFormatByteSize ( this Int32 bytesLength, int decimalPlaces = constants.C_DEFAULT_DECIMAL_DIGITS )
				=> ( (UInt64) bytesLength ).eFormatByteSize (decimalPlaces);

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eFormatByteSize ( this UInt32 bytesLength, int decimalPlaces = constants.C_DEFAULT_DECIMAL_DIGITS )
				=> ( (UInt64) bytesLength ).eFormatByteSize (decimalPlaces);



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eFormatPercent ( this int value, bool alignRight = true )
				=> ( (float) ( (float) value.eCheckRange () / 100f ) ).eFormatPercent (0, alignRight);

			/// <summary>Возвращает строку вида '20,23 %'</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eFormatPercent ( this float value, int decimalPlaces = constants.C_DEFAULT_DECIMAL_DIGITS, bool alignRight = true )
			{
				if (float.IsNaN (value))
				{
					value = 0F;
				}

				var format = $"P{decimalPlaces}";
				var sValue = value.ToString (format);
				if (alignRight)
				{
					var sPercent100 = ( (float) 1 ).ToString (format);
					sValue = sValue.PadLeft (sPercent100.Length, ' ');
				}
				return sValue;
			}


			/// <summary>Возвращает строку вида '20,2%'</summary>
			/// <param name="value">Значение от 0,0 до 1,0 !!!НЕ от 0 до 100!!!</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eFormatPercent ( this double value, int decimalPlaces = constants.C_DEFAULT_DECIMAL_DIGITS )
			{
				if (double.IsNaN (value))
				{
					value = 0d;
				}

				return ( (float) value ).eFormatPercent (decimalPlaces);
			}






			/// <summary>Format number like '1 000 000'</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eFormatReadable ( this int value )
				=> value.ToString ("N0").Trim ();

			/// <inheritdoc cref="eFormatReadable(int)" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eFormatReadable ( this long value )
				=> value.ToString ("N0").Trim ();

			/// <inheritdoc cref="eFormatReadable(int)" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eFormatReadable ( this IntPtr value )
				=> value.ToInt64 ().eFormatReadable ();

			/// <summary>Format number like '1 000 000.00'</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eFormatReadable ( this float value, int decimalPlaces = constants.C_DEFAULT_DECIMAL_DIGITS )
				=> value.ToString ($"N{decimalPlaces}").Trim ();

			/// <inheritdoc cref="eFormatReadable(float,int)" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eFormatReadable ( this double value, int decimalPlaces = constants.C_DEFAULT_DECIMAL_DIGITS )
				=> value.ToString ($"N{decimalPlaces}").Trim ();

			/// <inheritdoc cref="eFormatReadable(float,int)" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eFormatReadable ( this uint value, int decimalPlaces = constants.C_DEFAULT_DECIMAL_DIGITS )
				=> value.ToString ($"N{decimalPlaces}").Trim ();




			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eFormat_PlusMinus ( this bool bValue ) => bValue
				? "+"
				: "-";



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eFormat_YesNoGlobal ( this bool value )
				=> ( uom.AppInfo.CurrentUICultureIsRuTree )
					? ( value ? constants.CS_YES_RU : constants.CS_NO_RU )
					: ( value ? constants.CS_YES_EN : constants.CS_NO_EN );


			/// <summary>iProgress из iMax (20.25%)</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eFormatProgress ( this int progressValue, int maxValue, int percentDigits = 2 )
			{
				float sngProgress = 0f;
				if (maxValue > 0)
				{
					sngProgress = progressValue / (float) maxValue;
				}

				return "{0} из {1} ({2})".eFormat (progressValue, maxValue, sngProgress.eFormatPercent (percentDigits));
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eFormatProgressConsole ( this int progressValue, int maxValue, string format = "G" )
			{
				string sBTotal = maxValue.ToString (format);
				string sACurrent = progressValue.ToString (format).PadLeft (sBTotal.Length, ' ');
				return "{0} из {1}".eFormat (sACurrent, sBTotal);
			}


			internal const char C_UNICODE_MODIFERS_StrikeThrough = '\u0336';


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eFormatUnicode_StrikeThrough ( this string src )
			{
				List<char> cc = new (src.Length * 2);
				foreach (var character in src)
				{
					cc.Add (character);
					cc.Add (C_UNICODE_MODIFERS_StrikeThrough);
				}
				return new string ([ .. cc ]);
			}





			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static char eToChar ( this byte b, char? notReadableCharReplacement = null )
				=> ( b >= FIRST_READABLE_CHAR )
				? Convert.ToChar (b)
				: notReadableCharReplacement.HasValue
					? notReadableCharReplacement.Value
					: Convert.ToChar (b);

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static IEnumerable<char> eToChars ( this IEnumerable<byte> data, char? notReadableCharReplacement = null )
				=> data.Select (B => B.eToChar (notReadableCharReplacement));

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static char[] eToCharArray ( this IEnumerable<byte> data, char? notReadableCharReplacement = null )
				=> data.eToChars (notReadableCharReplacement).ToArray ();



			//[MethodImpl(MethodImplOptions.AggressiveInlining)]
			//internal static long  eUnHex(this string HexVal) => (long)Math.Round(Conversion.Val("&H" + HexVal));

			/// <summary>Возвращает строку из байт, заменяя нечинаемые символы заменителями</summary>
			/// <param name="abData">Массив байт</param>
			/// <param name="notReadableCharReplacement">Заменитель нечитаемых символов</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eToStringAnsiEx ( this IEnumerable<byte>? abData, char? notReadableCharReplacement = '.' )
				=> ( abData == null || !abData.Any () )
					? string.Empty
					: new string (abData!.eToCharArray (notReadableCharReplacement));


			/// <inheritdoc cref="Encoding.ASCII.GetString" />			
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eToStringAnsi ( this IEnumerable<byte>? abData )
				=> ( abData == null || !abData.Any () )
					? string.Empty
					: Encoding.ASCII.GetString ((byte[]) ( abData! ));


			/// <inheritdoc cref="Encoding.Unicode.GetString" />			
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eToStringUni ( this IEnumerable<byte>? abData )
				=> ( abData == null || !abData.Any () )
					? string.Empty
					: Encoding.Unicode.GetString ((byte[]) ( abData! ));


			/// <inheritdoc cref="Encoding.Unicode.GetString" />			
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eToStringUni ( this Span<byte> bytes )
				=> ( bytes == Span<byte>.Empty )
					? string.Empty
					: Encoding.Unicode.GetString (bytes);



			/// <summary>Возвращает строку байт вида 00-00-01-01-05-AA</summary>
			/// <param name="reverseRTL">Развернуть в программисткий вид (младший байт будет справа)</param>
			/// <param name="bytesSeparator">Рзделитель байтов. Если не указан, используется системный (по-умолчанию) - обычно это минус</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eToStringHex ( this IEnumerable<byte>? data, bool reverseRTL = false, string? bytesSeparator = null )
			{
				if (null == data || data.Count () < 1)
				{
					return string.Empty;
				}

				if (reverseRTL)
				{
					data = data.Reverse ();
				}

				string sResult = BitConverter.ToString (data.ToArray ());

				if (null != bytesSeparator)
				{
					string defaultSeparator = constants.SystemDefaultHexByteSeparator.ToString ();
					if (bytesSeparator != defaultSeparator)
					{
						sResult = sResult.eReplaceAll (defaultSeparator, bytesSeparator);
					}
				}

				return sResult;
			}


#if NET





#endif


			#region IP Address

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eToStringHex ( this System.Net.NetworkInformation.PhysicalAddress MAC ) => MAC.GetAddressBytes ().eToStringHex (false);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eToStringHex ( this System.Net.IPAddress IP ) => IP.GetAddressBytes ().eToStringHex (false);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eToStringHex ( this System.Net.IPEndPoint IPEP ) => $"{IPEP.Address.eToStringHex ()}-{BitConverter.GetBytes ((ushort) IPEP.Port).eToStringHex ()}";

			#endregion

			/// <summary>Removes only Space char (0x32)</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eRemoveSpacesFast ( this string source )
				=> source.Replace (" ", string.Empty);

			/// <summary>Removes all Unicode character which is categorized as white space.</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eRemoveSpacesEx ( this string source )
				=> string.Concat (source.Where (c => !char.IsWhiteSpace (c)));

#if NET


			/// <summary>Заменяет все множественные пробелы на один пробел</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eRemoveDoubleSpaces ( this string source )
				=> source.eReplaceAll ("  ", " ");


#endif


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eReplaceCharsWithString ( this string source, char[] charsToReplace, string replaceWith, StringComparison comparison = DEFAULT_STRING_COMPARSION )
				=> string.Concat (source
					.Select (x => charsToReplace.Contains (x)
						? replaceWith
						: x.ToString ()
						)
					.ToArray ());


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eReplaceCharsWithString ( this string source, char[] charsToReplace, Func<char, string> replaceFunc )
				=> string.Concat (source
					.Select (x => charsToReplace.Contains (x)
						? replaceFunc.Invoke (x)
						: x.ToString ()
						)
					.ToArray ());


#if NET
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eReplaceAll ( this string source, string search, string replaceWith = DEFAULT_REPLACE_WITH, StringComparison comparison = DEFAULT_STRING_COMPARSION )
			{
				if (search.IsNullOrEmpty ())
				{
					throw new ArgumentNullException (nameof (search));
				}

				if (replaceWith.Contains (search, comparison))
				{
					throw new ArgumentException ($"{nameof (replaceWith)} = '{replaceWith}', and contains search = '{search}'");
				}

				while (source.Contains (search, comparison))
				{
					source = source.Replace (search, replaceWith, comparison);
				}
				return source;
			}


			/// <param name="replacePairs">(FindWhat As string, ReplaceWith As string)</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eReplacePairs ( this string source, IEnumerable<(string search, string replaceWith)> replacePairs, StringComparison comparison = DEFAULT_STRING_COMPARSION )
			{
				if (!replacePairs.Any ())
				{
					throw new ArgumentNullException (nameof (replacePairs));
				}

				foreach (var frp in replacePairs)
				{
					source = source.eReplaceAll (frp.search, frp.replaceWith, comparison);
				}

				return source;
			}


#else
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string eReplaceAll(this string source, string search, string replaceWith = DEFAULT_REPLACE_WITH)
			{
				if (search.IsNullOrEmpty()) throw new ArgumentNullException(nameof(search));
				if (replaceWith.Contains(search)) throw new ArgumentException($"{nameof(replaceWith)} = '{replaceWith}', and contains search = '{search}'");

				while (source.Contains(search))
				{
					source = source.Replace(search, replaceWith);
				}
				return source;
			}


			/// <param name="replacePairs">(FindWhat As string, ReplaceWith As string)</param>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string eReplacePairs(this string source, IEnumerable<(string search, string replaceWith)> replacePairs)
			{
				if (!replacePairs.Any()) throw new ArgumentNullException(nameof(replacePairs));

				foreach (var frp in replacePairs)
					source = source.eReplaceAll(frp.search, frp.replaceWith);

				return source;
			}

#endif






			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eЗаменитьЗапятыеНаТочки ( this string source )
				=> source.eReplaceAll (",", ".");






			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eReplaceAll ( this string source, IEnumerable<string> search, string replaceWith = DEFAULT_REPLACE_WITH, StringComparison comparison = DEFAULT_STRING_COMPARSION )
			{
				if (null == search || !search.Any ())
				{
					throw new ArgumentNullException (nameof (search));
				}

				foreach (var sFind in search)
				{
					source = source.eReplaceAll (search, replaceWith, comparison);
				}

				return source;
			}




			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static KeyValuePair<string, string> eToKeyValuePair ( this string source, string separator, StringComparison comparison = DEFAULT_STRING_COMPARSION, bool trim = true )
			{
				if (source.IsNullOrWhiteSpace ())
				{
					throw new ArgumentNullException (nameof (source));
				}

				int sepPos = source.IndexOf (separator, comparison);
				if (sepPos < 1)
				{
					throw new ArgumentException ("Separator pos < 1!");
				}

				string key = source.Substring (0, sepPos);
				string value = source.Substring (sepPos + separator.Length);

				if (trim)
				{
					key = key.Trim ();
					value = value.Trim ();
				}
				return new (key, value);
			}






			/// <summary>Создаёт строку нулевых символов заданной длинны</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eCreateNullCharSrting ( this int iNullCharCount ) => new (constants.CC_NULL, iNullCharCount);







#if NET6_0_OR_GREATER
			/// <summary>Take left part of the string</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string? eLeft ( this string? source, int charCount )
			{
				if (source == null)
				{
					return null;
				}

				if (charCount <= 0)
				{
					throw new ArgumentOutOfRangeException (nameof (charCount));
				}

				if (source.Length < charCount)
				{
					charCount = source.Length;
				}

				return source[ ..charCount ];
			}

#else
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string? eLeft(this string? source, int charCount)
			{
				if (source == null) return null;
				if (charCount <= 0) throw new ArgumentOutOfRangeException(nameof(charCount));
				if (source.Length < charCount) charCount = source.Length;

				return string.Concat(source.Take(charCount));
			}
#endif



			/// <summary>Split string to left and right parts</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static (string Left, string Right)? eSplitByIndex ( this string source, int leftPartLen )
			{
				if (source == null)
				{
					return null;
				}

				if (source.IsNullOrWhiteSpace () || leftPartLen <= 0)
				{
					return (string.Empty, string.Empty);
				}

				if (leftPartLen >= source.Length)
				{
					return (source, string.Empty);
				}

				string left = source.Substring (0, leftPartLen);
				string right = source.Substring (leftPartLen);
				return (left, right);
			}

			/// <summary>Split string to left and right parts</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static (string Left, string Right, int SplitCharIndex)? eSplitByChar ( this string source, char separatorChar, bool excludeSeparatorChar )
			{
				if (source == null)
				{
					return null;
				}

				if (source.IsNullOrWhiteSpace ())
				{
					return (string.Empty, string.Empty, -1);
				}

				int separatorCharIndex = source.IndexOf (separatorChar);
				if (separatorCharIndex < 0)
				{
					return (source, string.Empty, separatorCharIndex);
				}

				string left = source.Substring (0, separatorCharIndex);
				string right = source.Substring (separatorCharIndex);

				if (excludeSeparatorChar)
				{
					right = ( right.Length > 1 )
						? right.Substring (1)
						: string.Empty;
				}

				return (left, right, separatorCharIndex);
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eContains ( this string sourceText, string search, StringComparison comparison = DEFAULT_STRING_COMPARSION )
			{
				if (null == sourceText || null == search)
				{
					return false;
				}

				return sourceText.IndexOf (search, comparison) >= 0;
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eContains ( this string sourceText, IEnumerable<string> search, StringComparison comparison = DEFAULT_STRING_COMPARSION )
			{
				foreach (var S in search)
				{
					if (sourceText.eContains (S, comparison))
					{
						return true;
					}
				}
				return false;
			}


			#region Wrap
			//private const string CS_ESCAPE_LF = "\n";

			/// <summary>Replacing "\n" to Environment.NewLine</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			private static string eWrapCPP ( this string source )
				=> source.Replace (@"\n", Environment.NewLine);

			/// <summary>Replacing "|" to Environment.NewLine</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			private static string eWrapVB ( this string source )
				=> source.Replace ('|'.ToString (), Environment.NewLine);


			/// <summary>Replacing "\n" and "|" to Environment.NewLine</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eWrap ( this string source ) => source.eWrapVB ().eWrapCPP ();

			#endregion



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eAppend ( this string? source, string append, string separator = constants.vbCrLf )
			{
				source ??= string.Empty;
				if (source.Length > 0)
				{
					source += separator;
				}

				return ( source + append );
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static StringBuilder eToStringBuilder ( this string sourceText )
				=> new (sourceText);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eReplace ( this string source, int startIndex, int len, string replaceWith )
			{
				string before = source.Substring (0, startIndex);
				string after = source.Substring (startIndex + len);
				return before + replaceWith + after;
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eCut ( this string source, int cutStart, int len )
				=> source.eReplace (cutStart, len, "");


			/// <summary>Заменяет в тексте строки &lt;0x0A&gt; на соответствующий символ</summary>
			/// <param name="source">Текст например: aaa&lt;0x0A&gt;bbb&lt;0x0D&gt;</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eReplaceSpecialHexChars ( this string source )
			{
				const string CS_HEX_PREFIX = "<0x";
				Regex rx = new (CS_HEX_PREFIX + @"[0-9a-fA-F]{2}>");
				var mm = rx.Matches (source).Cast<Match> ().Reverse ();
				foreach (var m in mm)
				{
					string hex = m.Value;
					hex = hex.Substring (CS_HEX_PREFIX.Length, m.Length - CS_HEX_PREFIX.Length - 1);
					byte b = byte.Parse (hex, NumberStyles.HexNumber);
					source = source.eReplace (m.Index, m.Length, b.eToChar ().ToString ());
				}
				return source;
			}



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static char[] eToChars ( this IEnumerable<byte> abData )
				=> [..
					abData.Select(b => b.eToChar())
					];


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eToString ( this IEnumerable<char> chars )
				=> new ([ .. chars ]);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string[] eToStrings ( this IEnumerable<char> chars )
				=> [..
					chars.Select(c => c.ToString())
					];


			/// <summary>using StringReader.ReadLine()</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static IEnumerable<string> eSplitLines ( this string? source, bool skipEmptyLines = false, bool trimEachLine = false )
			{
				if (source.IsNullOrEmpty ())
				{
					yield break;
				}

				using (StringReader sr = new (source!))
				{
					string? line = sr.ReadLine ();
					while (null != line)
					{
						if (trimEachLine)
						{
							line = line.Trim ();
						}

						bool bAdd = !skipEmptyLines
	|| line.IsNotNullOrWhiteSpace ();

						if (bAdd)
						{
							yield return line;
						}

						line = sr.ReadLine ();
					}
				}
			}


			/// <summary>Создаёт объект System.Guid из строки</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static Guid eToGUID ( this string GUIDString ) => Guid.Parse (GUIDString);









			#region eTrimStart / eTrimEnd






			/// <inheritdoc cref="string.TrimStart(char)" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eTrimStart ( this string source, string trimPrefix, bool onlyOnce = false, StringComparison comparison = DEFAULT_STRING_COMPARSION )
			{
				if (trimPrefix.IsNullOrEmpty ())
				{
					return source;
				}

				while (source.StartsWith (trimPrefix, comparison) && source.Length >= trimPrefix.Length)
				{
					source = source.Substring (trimPrefix.Length);
					if (onlyOnce)
					{
						break;
					}
				}
				return source;
			}

			/// <inheritdoc cref="string.TrimStart(char)" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eTrimStart ( this string source, char trimPrefix, bool onlyOnce = false, StringComparison comparison = DEFAULT_STRING_COMPARSION )
				=> source.eTrimStart (trimPrefix.ToString (), onlyOnce, comparison);


			/// <inheritdoc cref="string.TrimEnd(char)" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eTrimEnd ( this string source, string trimSuffix, bool onlyOnce = false, StringComparison comparison = DEFAULT_STRING_COMPARSION )
			{
				if (trimSuffix.IsNullOrEmpty ())
				{
					return source;
				}

				while (source.EndsWith (trimSuffix, comparison) && source.Length >= trimSuffix.Length)
				{
					int charsToTake = source.Length - trimSuffix.Length;
					if (charsToTake == 0)
					{
						return string.Empty;
					}

					source = source.Substring (0, charsToTake);
					if (onlyOnce)
					{
						break;
					}
				}
				return source;
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eTrimEnd ( this string source, char trimSuffix, bool onlyOnce = false, StringComparison comparison = DEFAULT_STRING_COMPARSION )
				=> source.eTrimEnd (trimSuffix.ToString (), onlyOnce, comparison);


			#endregion





			/// <summary>Возвращает окончание строки после указанного фрагмента</summary>
			/// <param name="source">Исходная строка</param>
			/// <param name="startWithString">Вернётся отсток, после конца этой строки</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eSubstring ( this string source, string startWithString, StringComparison comparison = DEFAULT_STRING_COMPARSION )
			{
				if (startWithString.IsNullOrWhiteSpace ())
				{
					return source;
				}

				int pos = source.IndexOf (startWithString, comparison);
				if (pos < 0)
				{
					return source;
				}

				string S = source.Substring (pos + startWithString.Length);
				return S;
			}

			/// <summary>Возвращает кусок строки между Prefix и Suffix</summary>
			/// <param name="source">Исходная строка от которой выделяется остаток</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string? eSubstringBetween ( this string? source, string prefix, string suffix, StringComparison comparison = DEFAULT_STRING_COMPARSION )
			{
				if (source.IsNullOrWhiteSpace ())
				{
					return null;
				}

				var s = source!.eSubstring (prefix, comparison);
				if (s.IsNullOrWhiteSpace () || suffix.IsNullOrWhiteSpace ())
				{
					return null;
				}

				int pos = s.IndexOf (suffix, comparison);
				if (pos <= 0)
				{
					return null;
				}

				s = s.Substring (0, pos);
				return s;
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static StringCollection eToStringCollection ( this IEnumerable<string> source )
				=> [ .. source ];


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static List<string> eToList ( this StringCollection source )
				=> source.Cast<string> ().ToList ();


			#region MiltiStringZ

			/// <summary>Создаёт одну строку (REG_MULTI_SZ) из элементов массива (где каждая строка отделена [0] а в конце [00])</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eToAPIMultiStringZ ( this IEnumerable<string> sources )
				=> sources!.eJoin ("\0") + "\0\0";


			/// <summary>Возвращает строки из REG_MULTI_SZ (где каждая строка отделена [0] а в конце [00])</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			[Obsolete ("!!!  eReadMiltiStringZUni НАДО ПРОВЕРИТЬ РАБОТОСПОСОБНОСТЬ!!!")]
			internal static string[] eReadMiltiStringZUni ( this IntPtr Ptr )
			{
				const int UNICODE_CHAR_SIZE = 2;
				List<string> aData = new List<string> (100);
				do
				{
					// Если Первый байт нулевой, текста нету
					if (Marshal.ReadByte (Ptr) == 0)
					{
						break;
					}

					var sLine = Marshal.PtrToStringUni (Ptr);
					if (string.IsNullOrEmpty (sLine) || sLine.Length < 1)
					{
						// Вроде нет больше строк
						break;
					}
					else
					{
						aData.Add (sLine);
						int iOffset = ( sLine.Length + 1 ) * UNICODE_CHAR_SIZE;
						Ptr += iOffset;
					}
				}
				while (true);
				return [ .. aData ];
			}
			#endregion



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static IEnumerable<int> eAllIndexesOf ( this string? source, string search, StringComparison comparison = DEFAULT_STRING_COMPARSION )
			{
				if (null == source)
				{
					yield break;
				}

				int minIndex = source.IndexOf (search!, comparison);
				while (minIndex != -1)
				{
					yield return minIndex;
					minIndex = source.IndexOf (search, minIndex + search.Length, comparison);
				}
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static int[] eAllIndexesOfAsArray ( this string? source, string searchString, StringComparison comparison = DEFAULT_STRING_COMPARSION )
				=> [..
					source.eAllIndexesOf(searchString, comparison)
					];


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static IEnumerable<int> eAllIndexesOf ( this string? source, char c )
			{
				if (null == source)
				{
					yield break;
				}

				var result2 = source?
					.Select (( b, i ) => b.Equals (c) ? i : -1)?
					.Where (i => i != -1);

				int minIndex = source!.IndexOf (c);
				while (minIndex != -1)
				{
					yield return minIndex;
					minIndex = source.IndexOf (c, minIndex + 1);
				}
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static int[] eAllIndexesOfAsArray ( this string? source, char c )
				=> [..
					source.eAllIndexesOf(c)
					];





			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static int eCount ( this string source, char charToCount )
				=> source.Count (c => c.Equals (charToCount));


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eCountOfCharIsAtLeast ( this string source, char c, int countAtLeast )
			{
				char[] cc = source.ToCharArray ();

				int iCharCount = 0;
				int iCharOld = -1;

				while (iCharCount < countAtLeast)
				{
					int iCharNew = Array.IndexOf (cc, c, iCharOld + 1);
					if (iCharNew == -1)
					{
						return false;//Not found Next
					}

					iCharOld = iCharNew;
				}
				return true;//Found all
			}



			/// <summary>Добавляет перед и после строки указанный символ</summary>
			/// <param name="encloseString">Строка добавляемая перед и после строки</param>
			/// <param name="notEncloseIfExist">Если строка уже окружена, то не окружать повторно</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eEnclose (
				this string source,
				string encloseString = constants.CS_QUOTE,
				bool notEncloseIfExist = true,
				bool atStart = true,
				bool atEnd = true )
			{
				if (!notEncloseIfExist | !source.StartsWith (encloseString) && atStart)
				{
					source = encloseString + source;
				}

				if (!notEncloseIfExist | !source.EndsWith (encloseString) && atEnd)
				{
					source += encloseString;
				}

				return source;
			}

			/// <summary>Добавляет перед и после строки указанный символ</summary>
			/// <param name="encloseChar">Строка добавляемая перед и после строки</param>
			/// <param name="notEncloseIfExist">Если строка уже окружена, то не окружать повторно</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eEncloseC (
				this string source,
				char encloseChar = constants.CC_QUOTE,
				bool notEncloseIfExist = true,
				bool atStart = true,
				bool atEnd = true )
				=> source.eEnclose (encloseChar.ToString (), notEncloseIfExist, atStart, atEnd);


			/// <summary>Удаляет окружающие строку кавычки или иной окружающий текст</summary>
			/// <param name="sourceText"></param>
			/// <param name="encloseChar"></param>
			/// <param name="onlyOnePass">Только один проход</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eExclose (
				this string sourceText,
				char encloseChar = constants.CC_QUOTE,
				bool onlyOnePass = false )
			{
				while (sourceText.IsNotNullOrWhiteSpace () && sourceText.StartsWith (Convert.ToString (encloseChar)))
				{
					sourceText = sourceText.eTrimStart (encloseChar);
					if (onlyOnePass)
					{
						break;
					}
				}

				while (sourceText.IsNotNullOrWhiteSpace () && sourceText.EndsWith (Convert.ToString (encloseChar)))
				{
					sourceText = sourceText.eTrimEnd (encloseChar);
					if (onlyOnePass)
					{
						break;
					}
				}
				return sourceText;
			}

			/// <summary>Возрвщвем количество одинаковых символов с начла строки, пока они совпадают с заданными</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static int eTakeWhile_Count ( this string sourceText, char firstCharToSelect )
			{
				if (sourceText.IsNullOrWhiteSpace () || !sourceText.StartsWith (firstCharToSelect.ToString ()))
				{
					return 0;
				}

				int iCount = 0;
				foreach (char C in sourceText)
				{
					if (C != firstCharToSelect)
					{
						break;
					}

					iCount += 1;
				}

				return iCount;
			}



			/// <summary>Добавляет в конце строки указанный символ</summary>
			/// <param name="sourceText">Исходная строка</param>
			/// <param name="appendix">Строка добавляемая в конце исходной</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eAppend ( this string sourceText, string appendix = "" )
				=> sourceText.EndsWith (appendix)
				? sourceText
				: sourceText + appendix;


			// uom.Comparers.Instances.StringIEqualityComparer_OrdinalIgnoreCase.Value

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eContainsOrdinalIgnoreCase ( this IEnumerable<string> sources, string target )
				=> sources.Contains (target, StringComparer.OrdinalIgnoreCase);

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eContainsCurrentCultureIgnoreCase ( this IEnumerable<string> sources, string target )
				=> sources.Contains (target, StringComparer.CurrentCultureIgnoreCase);

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eContainsInvariantCultureIgnoreCase ( this IEnumerable<string> sources, string target )
				=> sources.Contains (target, StringComparer.InvariantCultureIgnoreCase);





			internal enum CHANGE_CASE_MODES : int
			{
				TO_LOWER,
				TO_UPPER,
				TO_TITLE
			}

			/// <summary> преобразует первый символ каждого слова в верхний регистр, а остальные символы — в нижний. 
			/// Однако слова, состоящие только из прописных букв, считаются сокращениями и не преобразуются.
			/// Метод TextInfo.ToTitleCase учитывает регистр, то есть он использует соглашения об использовании регистров, действующие для определенного языка и региональных параметров. 
			/// Чтобы вызвать этот метод, сначала нужно получить объект TextInfo, представляющий соглашения об использовании регистров, из свойства CultureInfo.TextInfo конкретного языка и региональных параметров.
			/// 
			/// В примере ниже каждая строка из массива передается в метод TextInfo.ToTitleCase. 
			/// Среди строк есть как строки заголовков, так и сокращения. Строки преобразуются в последовательности слов, начинающихся с заглавных букв, согласно соглашениям об использовании регистров для языка и региональных параметров Английский (США). </summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eToTitleCase ( this string source )
				=> CultureInfo.CurrentCulture.TextInfo.ToTitleCase (source);


			/// <summary>Добавляет пробелы перед заглавными буквами</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eInsertSpacesBeforeUpperCaseChars ( this string source )
			{
				if (source.IsNullOrWhiteSpace () || source.Length < 2)
				{
					return source;
				}

				return source.Select (c => char.IsUpper (c) ? ( " " + c.ToString () ) : c.ToString ()).ToArray ().eJoin ("")!.TrimStart ();

				/*
			 var sbResult = new StringBuilder(source.Length * 2);
			 //var aWordChars = source.ToCharArray();
			 bool bFirst = true;
			 foreach (char C in source)
			 {
				 if (!bFirst && char.IsUpper(C)) sbResult.Append(' ');
				 sbResult.Append(C);
				 bFirst = false;
			 }
			 return sbResult.ToString();
				 */
			}

			/// <summary>Проверяет строку на (Is Nothing) и (string.IsNullOrEmpty) и (SourceString.Length>0) и возвращает либо исходную строку, либо пустую</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eCheckNullChar ( this char SourceChar, string DefaultValue = "" )
				=> ( SourceChar == constants.vbNullChar ) ? DefaultValue : SourceChar.ToString ();


			/// <summary>If string is null or WhiteSpace - returns defaultValue, Else leave source string</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eCheckNullOrWhiteSpace ( this string? src, string defaultValue = "" )
				=> src.IsNullOrWhiteSpace ()
				? defaultValue
				: src!;





			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static byte[] eTakeReadableChars ( this IEnumerable<byte> abData, byte NotReadableMinValue = 31 )
				=> abData.Where (B => B > NotReadableMinValue).ToArray ();


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static char[] eGetDigitChars ( this IEnumerable<char> aPhoneChars )
				=> aPhoneChars.Where (C => char.IsDigit (C)).ToArray ();


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string? eGetDigitChars ( this string S )
			{
				if (S.IsNullOrWhiteSpace ())
				{
					return null;
				}

				var AB = S.ToArray ().eGetDigitChars ();
				return ( !AB.Any () )
					? null
					: new string (AB);
			}







			#region API Related / Win / DOS / NString

			/// <summary>Взвращает строку слева до символа с кодом 0 (ноль)</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eNString ( this string sourceText )
			{
				string sText = sourceText.eCheckNullOrWhiteSpace ();
				int iZeroPos = sText.IndexOf (constants.vbNullChar);
				switch (iZeroPos)
				{
					case < 0: break;                    // Нет нулевого символа в строке
					case 0: return "";                            // Нулевой символ это первый символ
					case > 0: sText = sText.Substring (0, iZeroPos); break;// Есть нулевой символ в строке
				}
				return sText;
			}

			// ''' <summary>Убирает все символы с кодом 0 справа до первого ненулевого символа</summary>
			// <System.Runtime.CompilerServices.Extension()> _
			// Friend Function TrimmNullCharsRight(ByVal sText As string) As string
			// If (sText Is Nothing) Then Return ""
			// Dim sNewText As string = sText
			// While sNewText.EndsWith(Chr(0))
			// sNewText = sNewText.Substring(0, sNewText.Length - 1)
			// End While
			// Return sNewText
			// End Function

			#region Encoding Win-Dos


			// <DllImport(  UOM.Win32.WINDLL_USER, _
			// SetLastError:=True, _
			// CharSet:=CharSet.Auto, _
			// ExactSpelling:=False, _
			// CallingConvention:=CallingConvention.Winapi)> _
			// Friend Overloads Shared Function CharToOemBuff( _
			// <MarshalAs(UnmanagedType.LPTStr)> ByVal lpszSrc As string, _
			// <MarshalAs(UnmanagedType.LPStr)> ByVal lpszDst As string, _
			// ByVal cchDstLength As Integer) As Integer
			// '
			// End Function

			// <DllImport(UOM.Win32.WINDLL_USER, SetLastError:=True, CharSet:=CharSet.Auto, ExactSpelling:=False, CallingConvention:=CallingConvention.Winapi)>
			// Private Function CharToOemBuff(<[In](), MarshalAs(UnmanagedType.LPTStr)> ByVal lpszSrc As string,
			// ByVal lpszDst() As Byte,
			// ByVal cchDstLength As Integer) As Integer
			// End Function

			// ''' <summary>Перевод строки в DOS кодировку</summary>
			// ''' <param name="Строка_в_WIN_кодировке"></param>
			// Friend Function  eCharToOem(ByVal Строка_в_WIN_кодировке As string) As Byte()
			// Dim abDOS(Строка_в_WIN_кодировке.Length - 1) As Byte
			// Call CharToOemBuff(Строка_в_WIN_кодировке, abDOS, abDOS.Length)
			// Dim WEX As New System.ComponentModel.Win32Exception
			// If (WEX.NativeErrorCode <> 0) Then Throw WEX
			// 'sText =  UOM.Convert.CharToOemBuff(sText)
			// 'Dim sDos As string = Строка_в_WIN_кодировке
			// 'Call CharToOemBuff(Строка_в_WIN_кодировке, sDos, Len(Строка_в_WIN_кодировке))
			// Return abDOS
			// End Function

			// 'Friend Overloads Shared Function  eCharToOemBuff(ByVal Строка_в_WIN_кодировке As string) As string
			// '    Dim sDos As string = Строка_в_WIN_кодировке
			// '    Call CharToOemBuff(Строка_в_WIN_кодировке, sDos, Len(Строка_в_WIN_кодировке))
			// '    Return sDos
			// 'End Function


			// <DllImport(UOM.Win32.WINDLL_USER, SetLastError:=True, CharSet:=CharSet.Auto, ExactSpelling:=False, CallingConvention:=CallingConvention.Winapi)>
			// Private Function OemToCharBuff(<MarshalAs(UnmanagedType.LPTStr)> ByVal lpszSrc As string,
			// <MarshalAs(UnmanagedType.LPTStr)> ByVal lpszDst As string,
			// ByVal cchDstLength As Integer) As Integer
			// End Function
			// Friend Function  eOemToChar(ByVal Строка_в_DOS_кодировке As string) As string
			// Dim sWin As string = Строка_в_DOS_кодировке
			// Call OemToCharBuff(Строка_в_DOS_кодировке, sWin, Строка_в_DOS_кодировке.Length)
			// Return sWin
			// End Function


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eConvertEncoding ( this string TextToConvert, string FromEncoding, string ToEncoding )
			{
				var encFrom = Encoding.GetEncoding (FromEncoding);
				var encTo = Encoding.GetEncoding (ToEncoding);
				return TextToConvert.eConvertEncoding (encFrom, encTo);
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eConvertEncoding ( this string TextToConvert, Encoding encFrom, Encoding encTo )
			{

				// #$OutputEncoding = [System.Text.Encoding]::GetEncoding("windows-1251")
				// #[Console]::OutputEncoding = [System.Text.Encoding]::GetEncoding("windows-1251")	
				// #chcp 1251 - установить кодовую страницу, соответствующую Windows-кодировке.		
				// #1251 – Windows (кириллица);
				// #866 – DOC-кодировка;
				// #65001 – UTF-8;

				var abData = encTo.GetBytes (TextToConvert);
				abData = Encoding.Convert (encFrom, encTo, abData);
				return encTo.GetString (abData);
			}

			/// <summary>Преобразование текста из cp866 в windows-1251</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eConvertEncoding_Dos_Windows_1251 ( this string TextToConvert )
				=> TextToConvert.eConvertEncoding (uom.AppTools.LEncoding_cp866.Value, uom.AppTools.LEncoding_Windows1251.Value);
			#endregion

			#endregion



			/// <summary>Create string like '\\x.x.x.x\' or \\server\</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eCreateWinSharePrefix ( this string host ) => $@"\\{host}\";


			/// <summary>RegEx Words Count In string </summary>
			private static readonly Lazy<Regex> _rexWordsInString = new Lazy<Regex> (() => new Regex (@"\w+"));

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static IEnumerable<Match> eGetWords ( this string s )
				=> _rexWordsInString.Value.Matches (s).Cast<Match> ();

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string[] eGetWordsStrings ( this string s )
				=> s.eGetWords ().Select (m => m.Value).ToArray ();


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static int eGetWordsCount ( this string s ) => _rexWordsInString.Value.Matches (s).Count;
			//public static int eGetWordsCount(this string str) => str.Split(new char[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static (
				int MinLen,
				string CommonPrefix,
				int CommonPrefixWordsCount,
				string[] TotalWordsInS1,
				string[] TotalWordsInS2,
				string[] UniqueWordsInBothStrings
				)
				eGetEqualityMetrics ( this string s1, string s2 )
			{
				int minLen = Math.Min (s1.Length, s2.Length);
				int equalChars = 0;
				while (( equalChars < minLen ) && ( s1[ equalChars ] == s2[ equalChars ] )) { equalChars++; }
				string equalPrefix = s1.Substring (0, equalChars);
				int wordsCountInPrefix = equalPrefix.eGetWordsCount ();
				string[] totalWordsInS1 = s1.eGetWordsStrings ();
				string[] totalWordsInS2 = s2.eGetWordsStrings ();
				string[] uniqueWordsInBothStrings = totalWordsInS1
					.Distinct ()
					.Where (w => s2.IndexOf (w) >= 0)
					.ToArray ();


				return (
					minLen,
					equalPrefix,
					wordsCountInPrefix,
					totalWordsInS1,
					totalWordsInS2,
					uniqueWordsInBothStrings
					);
			}



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static int eCompareTo ( this IPAddress x, IPAddress y )
			{

				var abX = x.GetAddressBytes ();
				var abY = y.GetAddressBytes ();
				if (abX.Length != 4)
				{
					throw new ArgumentOutOfRangeException ("IP4 only can be compared!");
				}

				if (abX.Length != abX.Length)
				{
					throw new ArgumentOutOfRangeException ("asfd");
				}

				for (int i = 0 ; i < 4 ; i++)
				{
					int res = abX[ i ].CompareTo (abY[ i ]);
					if (res != 0)
					{
						return res;
					}
				}
				return 0;
			}



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static (string Result, bool StringWasTrimmed) eTruncate ( this string src, int maxLen, bool addHorizontalEllipsisChar = false )
			{
				if (src.IsNotNullOrWhiteSpace () && ( src.Length > maxLen ))
				{
					string s = src.Substring (0, maxLen);
					if (addHorizontalEllipsisChar)
					{
						s = src.Substring (0, s.Length - 1);
						s += (char) 0x2026;
					}
					return (s, true);
				}
				return (src, false);
			}

		}


		internal static partial class Extensions_Dictionary
		{


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool eIsDictionaryEqualTo<TKey, TValue> ( this Dictionary<TKey, TValue> dic1, Dictionary<TKey, TValue> dic2 ) where TKey : notnull
				=> dic1
				.OrderBy (x => x.Key)
				.SequenceEqual (dic2.OrderBy (x => x.Key));



#if NET5_0_OR_GREATER


			/// <summary>Sample:
			/// <code>
			/// var val2 = dic.eGetOrAdd (1334,"234");
			/// </code>
			/// </summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static TValue? eGetOrAdd<TKey, TValue> ( this Dictionary<TKey, TValue> dic, TKey key, TValue? value )
				where TKey : notnull
			{
				ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault (dic, key, out var exist);
				if (exist) return val;

				val = value;
				return value;
			}


			/// <summary>Sample:
			/// <code>
			/// var val2 = dic.eGetOrAdd (1334,"234");
			/// </code>
			/// </summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool eTryUpdate<TKey, TValue> ( this Dictionary<TKey, TValue> dic, TKey key, TValue? value )
				where TKey : notnull
			{
				ref var val = ref CollectionsMarshal.GetValueRefOrNullRef (dic, key);
				if (Unsafe.IsNullRef (ref val)) return false;
				val = value;
				return true;
			}



#endif



			/*
			 public bool Compare1<TKey, TValue>(
		Dictionary<TKey, TValue> dic1, 
		Dictionary<TKey,TValue> dic2)
		{
		return dic1.OrderBy(x => x.Key).
		SequenceEqual(dic2.OrderBy(x => x.Key));
		}

		public bool Compare2<TKey, TValue>(
		Dictionary<TKey, TValue> dic1, 
		Dictionary<TKey, TValue> dic2)
		{
		return (dic1.Count == dic2.Count && 
		dic1.Intersect(dic2).Count().
		Equals(dic1.Count));
		}

		public bool Compare3<TKey, TValue>(
		Dictionary<TKey, TValue> dic1, 
		Dictionary<TKey, TValue> dic2)
		{
		return (dic1.Intersect(dic2).Count().
		Equals(dic1.Union(dic2).Count()));
		}

			 */

		}


		internal static partial class Extensions_Arrays
		{

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static int eIndexOfComparableElement<T> ( this IEnumerable<T>? objList, T itemToSearch ) where T : IComparable<T>
			{
				//if (objList == null || itemToSearch == null) return -1;
				int index = 0;
				foreach (T item in objList.eOrEmptyIfNull ())
				{
					if (item.CompareTo (itemToSearch!) == 0)
					{
						return index;
					}

					index++;
				}
				return -1;
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static int eIndexOfElement<T> ( this IEnumerable<T>? objList, T itemToSearch ) where T : class
			{
				//if (objList == null || itemToSearch == null) return -1;
				int index = 0;
				foreach (T item in objList.eOrEmptyIfNull ())
				{
					if (item.Equals (itemToSearch))
					{
						return index;
					}

					index++;
				}
				return -1;
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void eForEach<T> ( this IEnumerable<T>? objList, Action<T>? action )
			{
				foreach (T item in objList?.eOrEmptyIfNull ()!)
				{
					action?.Invoke (item);
				}
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<T> eSort<T> ( this IEnumerable<T> source ) where T : System.IComparable<T>
			{
				_ = source ?? throw new ArgumentNullException (nameof (source));
				return ( from N in source orderby N select N ).ToArray ();
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string[] eSortArrayOfStrings ( this string[] source )
			{
				Array.Sort (source, StringComparer.InvariantCultureIgnoreCase);
				return source;
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static T[] eSortAsArray<T> ( this IEnumerable<T> source ) where T : System.IComparable<T>
				=> source.eSort ().ToArray ();

			public static List<T> eSortAsList<T> ( this IEnumerable<T> source ) where T : System.IComparable<T>
				=> source.eSort ().ToList ();

			/// <summary>Return string that contains in source text</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<string> eContains ( this string? source, IEnumerable<string>? aWhatToFind )
				=> ( null == source || null == aWhatToFind ) ? Array.Empty<string> () : aWhatToFind!.Where (S => source!.Contains (S));

			/// <summary>Return true if any string contains in source text</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool eContainsAny ( this string? source, IEnumerable<string>? aWhatToFind ) => source.eContains (aWhatToFind).Any ();

			/// <summary>Creates 1 element array with specifed item</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static T[] eToArrayOf<T> ( this T? source ) => ( null == source ) ? System.Array.Empty<T> () : new T[] { source };



			/// <summary>Creates 1 element array with specifed item</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<T> eAsIEnumerableOf<T> ( this IList<T> source )
			{
				foreach (T item in source)
				{
					yield return item;
				}
			}

			/// <summary>Creates 1 element array with specifed item</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static IEnumerable eAsIEnumerable ( this IList source )
			{
				foreach (var item in source)
				{
					yield return item;
				}
			}



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T? eFirstIfContains<T> ( this IEnumerable<T> A, Func<T, bool> ContainCheck )
			{
				/*
				If (Not A.Any) Then Return Nothing
				For Each R In A
					If ContainCheck.Invoke(R) Then Return R
				Next
				Return Nothing
				 */
				if (!A.Any ())
				{
					return default;
				}

				foreach (var R in A)
				{
					if (ContainCheck.Invoke (R))
					{
						return R;
					}
				}
				return default;
			}
			/*
			 */

			/// <summary>Return source or Empty {source} if source is null</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static IEnumerable<T> eOrEmptyIfNull<T> ( this IEnumerable<T>? source ) => source ?? Enumerable.Empty<T> ();

			/// <summary>Return source or Empty {source} if source is null</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T[] eOrEmptyArrayIfNull<T> ( this IEnumerable<T>? source ) => source.eOrEmptyIfNull ().ToArray ();



			/// <summary>Checks source to null or not ANY()</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eIsEmptyOrNull<T> ( this IEnumerable<T>? items ) => ( ( null == items ) || !items.Any () );

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eIsNotEmptyOrNull<T> ( this IEnumerable<T>? items ) => !items.eIsEmptyOrNull ();



			/// <summary>Возвращает массив строк, где каждая строка = 'ключ = значение'</summary>
			/// <param name="dic"></param>
			/// <param name="skipNullValues"></param>
			/// <param name="nullValuesDisplayName"></param>
			/// <returns></returns>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string[] eToArrayOfString<T> (
				this Dictionary<string, T> dic,
				bool skipNullValues = true,
				string nameValueSeperator = " = ",
				string nullValuesDisplayName = "[NULL]" )
				=> dic
				.Select<KeyValuePair<string, T>, string?> (kvp =>
				{
					if (kvp.Value == null && skipNullValues)
					{
						return null;
					}

					object val = kvp.Value!;

					var vt = val.GetType ();
					if (vt.IsGenericType && vt.GetGenericTypeDefinition () == typeof (Nullable<>))
					{
						//TODO: Some code need
					}

					return $"{kvp.Key}{nameValueSeperator}{val.ToString () ?? nullValuesDisplayName}";

				})
				.Where (s => ( null != s ))
				.ToArray ()!;


			/// <summary>Make string.Format(sFormatString, Args)</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string[]? eToArrayOfString ( this System.Collections.Specialized.StringCollection? sc )
				=> sc?.Cast<string> ()?.ToArray ();


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T[] eCreateArray<T> ( this T fillWith, int iCount )
			{
				T[] arr = new T[ iCount ];

#if NET5_0_OR_GREATER
				System.Array.Fill (arr, fillWith);
#else
				for (int n = 1; n < iCount; n++) arr[n] = fillWith;
#endif
				return arr;
			}

			// ''' <summary>Очень медленно!</summary>
			// <DebuggerNonUserCode, DebuggerStepThrough>
			// <MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
			// Friend Function  eCreateArray2(Of T)(tFillWithItems As T, ItemsCount As Integer) As T()
			// Dim aResult = Enumerable.Range(1, ItemsCount).Select(Function(X) tFillWithItems).ToArray
			// Return aResult
			// End Function

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T[] eAddRange<T> ( this T[] arr, T[] arrAppend )
			{
				var lData = arr.ToList ();
				lData.AddRange (arrAppend);
				return lData.ToArray ();
			}

			/// <summary>Null Safe ANY implementation</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eAny<T> ( this IEnumerable<T>? a ) => null != a && a.Any ();


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eContainsAnyOf<T> ( this IEnumerable<T> Arr1, IEnumerable<T> Arr2 )
			{
				foreach (var Element2 in Arr2)
				{
					if (Arr1.Contains (Element2))
					{
						return true;
					}
				}

				return false;
			}


			/// <summary>Возвращает элементы массива начиная с заданного</summary>
			/// <param name="iStartTakeIndex">Zero-based start char index</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eTakeFrom ( this string S, int iStartTakeIndex ) => S.Substring (iStartTakeIndex);


			/// <summary>Возвращает элементы массива начиная с заданного</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T[] eTakeFrom<T> ( this T[] A, int iStartTakeIndex )
			{
				if (A.Length <= iStartTakeIndex)
				{
					return Array.Empty<T> ();
				}

				var L = A.Length - iStartTakeIndex;
				var aResult = new T[ L ];
				Array.Copy (A, iStartTakeIndex, aResult, 0, L);
				return aResult;
			}


			///// <summary>Возвращает первые элементы набора в виде массива</summary>
			//[MethodImpl(MethodImplOptions.AggressiveInlining)]
			//internal static T[] eTakeFirst<T>(this IEnumerable<T> A, int iCount)
			//{
			//    if (A.Count() <= iCount) return A.ToArray();
			//    var B = A.Take(iCount).ToArray();
			//    return B;
			//}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T[] eRemoveFirst<T> ( this IEnumerable<T> ie, int countOfRemodevFirstItems = 1 )
			{
				/*
				List<T> l = ie.ToList();
				if (countOfRemodevFirstItems > l.Count) countOfRemodevFirstItems = l.Count;
				for (int n = 1; n <= countOfRemodevFirstItems; n++) l.RemoveAt(0);
				return l.ToArray();
				 */
				T[] arrInput = ie.ToArray ();
				T[] cutinput = new T[ arrInput.Length - countOfRemodevFirstItems ];
				Array.Copy (arrInput, countOfRemodevFirstItems, cutinput, 0, cutinput.Length);
				return cutinput;
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T[] eRemoveLast<T> ( this IEnumerable<T> ie, int countOfRemodevFirstItems = 1 )
			{
				List<T> l = ie.ToList ();
				if (countOfRemodevFirstItems > l.Count)
				{
					countOfRemodevFirstItems = l.Count;
				}

				for (int n = 1 ; n <= countOfRemodevFirstItems ; n++)
				{
					l.RemoveAt (l.Count - 1);
				}

				return l.ToArray ();
			}

			//[MethodImpl(MethodImplOptions.AggressiveInlining)]
			//public static void eRemoveRange<T>(this List<T> L, IEnumerable<T> ItemsToRemove)				=> ItemsToRemove.eForEach(rItem => L.Remove(rItem));

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static T[] eRemoveRange<T> ( this IEnumerable<T> ie, int index, int count )
			{
				List<T> l = ie.ToList ();
				l.RemoveRange (index, count);
				return l.ToArray ();
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void eRemoveRange<T> ( this List<T> l, IEnumerable<T> whatToRemove )
			{
				foreach (T i in whatToRemove)
				{
					l.Remove (i);
				}
			}


			/// <summary>Объединяет двумерный массив в одномерный. !!!БЕЗ СОРТИРОВКИ!!!</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			[Obsolete ("Use array.SelectMany instead!", true)]
			public static T[] eMerge2Dto1D<T> ( this IEnumerable<IEnumerable<T>> array2D )
			{
				/* OLD NET
				var lResult = new List<T>();
				Array2D.eForEach(arrSecondLevel => lResult.AddRange(arrSecondLevel.ToArray()));
				return lResult.ToArray();
				 */
				return [ .. array2D.SelectMany (a => a) ];
			}


			/// <summary>Считываем первый элемент списка, и удаляем его из списка (как в стеке)</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T? ePeekFirstOrDefault<T> ( this List<T> L )
			{
				var firstItem = L.FirstOrDefault ();
				if (firstItem != null)
				{
					L.RemoveAt (0);
				}

				return firstItem;
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T eSelectSwitch<T> ( this int ReturnIndex, params T[] ReturnValues ) => ReturnValues[ ReturnIndex ];






			/*

			internal enum NEAREST_SEARCH_MODES : int
			{
				eSmallerOrEqual,
				eSmallerOnly,
				eLargerOrEqual,
				eLargerOnly
			}

			/// <summary>Находит индекс в массиве ближайшего меньшего или большего числа</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static int eGetNearestIndex(this int[] aSource, int NearValue, NEAREST_SEARCH_MODES eMode)
			{
				switch (eMode)
				{
					case NEAREST_SEARCH_MODES.eSmallerOrEqual:
					case NEAREST_SEARCH_MODES.eSmallerOnly:
						{
							var aSmaller = (from I in aSource
											where I <= NearValue
											orderby I
											select I).ToArray();
							if (eMode == NEAREST_SEARCH_MODES.eSmallerOnly)
								aSmaller = aSmaller.Except(new int[] { NearValue }).ToArray(); // исключаем само число

							if (!aSmaller.Any()) return -1; // Нет ни одного элемента меньше заданного

							int iLast = aSmaller.Last();
							int iPos = Array.IndexOf(aSource, iLast);
							return iPos;
						}

					default:
						{
							var aLarger = (from I in aSource
										   where I >= NearValue
										   select I).ToArray();
							if (eMode == NEAREST_SEARCH_MODES.eLargerOnly)
								aLarger = aLarger.Except(new int[] { NearValue }).ToArray(); // исключаем само число
							if (!aLarger.Any())
								return -1; // Нет ни одного элемента больше заданного
							int iFirst = aLarger.First();
							int iPos = Array.IndexOf(aSource, iFirst);
							return iPos;
						}
				}
			}
			*/


			///// <summary>Сдвиг на 1 элемент влево (удаление первого элемента)</summary>
			//[MethodImpl(MethodImplOptions.AggressiveInlining)]
			//internal static IEnumerable<T> eShift<T>(this IEnumerable<T> ArrayToShift)
			//{
			//    _ = ArrayToShift ?? throw new ArgumentNullException(nameof(ArrayToShift));
			//    if (!ArrayToShift.Any()) return Array.Empty<T>();

			//    var L = ArrayToShift.ToList();
			//    L.RemoveAt(0);
			//    return L;
			//}


			#region Compare Arrays

#if NET
			/// <summary>Compare Arrays using processor SIMD acceleration and direct memory pointers
			/// The best results is for arrays of Int16, Int32, Int64, float,double.
			/// <br/>
			/// <c>
			/// !!! For compare binary arrays - the fastest methos is to use eCompareArrays_MemCmp !!!"
			/// </c>
			/// </summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool eCompareArrays_SIMD<T> ( this Span<T> a, Span<T> b ) where T : unmanaged, IComparable<T>
			{
				if (a.Length != b.Length)
				{
					return false;
				}

				int vectorSize = Vector<T>.Count;
				int numVectors = a.Length / vectorSize;
				int ceiling = numVectors * vectorSize;


				ReadOnlySpan<Vector<T>> leftVecArray = MemoryMarshal.Cast<T, Vector<T>> (a);
				ReadOnlySpan<Vector<T>> rightVecArray = MemoryMarshal.Cast<T, Vector<T>> (b);

				for (int i = 0 ; i < numVectors ; i++)
				{
					if (!System.Numerics.Vector.EqualsAll (leftVecArray[ i ], rightVecArray[ i ]))
					{
						return false;
					}
				}

				// Finish operation with any numbers leftover
				for (int i = ceiling ; i < a.Length ; i++)
				{
					if (a[ i ].CompareTo (b[ i ]) != 0)
					{
						return false;
					}
				}
				return true;
			}

			/// <summary>Compare Arrays using processor SIMD acceleration and direct memory pointers
			/// The best results is for arrays of Int16, Int32, Int64, float,double.
			/// <br/>
			/// <c>
			/// !!! For compare binary arrays - the fastest methos is to use eCompareArrays_MemCmp !!!"
			/// </c>
			/// </summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool eCompareArrays_SIMD<T> ( this Memory<T> a, Memory<T> b ) where T : unmanaged, IComparable<T>
				=> a.Span.eCompareArrays_SIMD<T> (b.Span);
#endif





#if NET6_0_OR_GREATER

			//Use this only in net.Core's and also....
			//For compare byte arrays in Win32/64 use most fastest eCompareArrays_MemCmp instead !


			/// <summary>Compare Arrays using processor SIMD acceleration and direct memory pointers
			/// The best results is for arrays of Int16, Int32, Int64, float,double.
			/// <br/>
			/// <c>
			/// !!! For compare binary arrays - the fastest methos is to use eCompareArrays_MemCmp !!!"
			/// </c>
			/// </summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool eCompareArrays_SIMD_Byte ( this ReadOnlySpan<byte> a, ReadOnlySpan<byte> b )
			{
				if (a.Length != b.Length)
				{
					return false;
				}

				int vectorSize = Vector<byte>.Count;
				int numVectors = a.Length / vectorSize;
				int ceiling = numVectors * vectorSize;

				ReadOnlySpan<Vector<byte>> leftVecArray = MemoryMarshal.Cast<byte, Vector<byte>> (a);
				ReadOnlySpan<Vector<byte>> rightVecArray = MemoryMarshal.Cast<byte, Vector<byte>> (b);
				//Vector<byte> zeroVector = Vector<byte>.Zero;
				for (int i = 0 ; i < numVectors ; i++)
				{
					//Comparing two vectors by XOR them. This must be very fast. Result must be zero if vectors is equal
					//if ((leftVecArray[i] ^ rightVecArray[i]) != zeroVector) return false;
					if (leftVecArray[ i ] != rightVecArray[ i ])
					{
						return false;
					}
				}

				// Finish operation with any numbers leftover
				for (int i = ceiling ; i < a.Length ; i++)
				{
					if (a[ i ] != b[ i ])
					{
						return false;
					}
				}
				return true;
			}


			/// <summary>Compare Arrays using processor SIMD acceleration and direct memory pointers
			/// The best results is for arrays of Int16, Int32, Int64, float,double.
			/// <br/>
			/// <c>
			/// !!! For compare binary arrays - the fastest methos is to use eCompareArrays_MemCmp !!!"
			/// </c>
			/// </summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool eCompareArrays_SIMD_Byte ( this ReadOnlyMemory<byte> a, ReadOnlyMemory<byte> b )
				=> a.Span.eCompareArrays_SIMD_Byte (b.Span);
#endif

			/// <summary>Поэлементное сравнение массивов</summary>'
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eCompareArrays_ByElements<T> ( this T[] arrA, T[] arrB, int iCompareCount = 0 ) where T : IComparable<T>
			{
				if (arrA.Length != arrB.Length)
				{
					return false;
				}

				if (!arrA.Any ())
				{
					return true;
				}

				if (iCompareCount < 1)
				{
					iCompareCount = arrA.Length;
				}

				for (int n = 0 ; n < iCompareCount ; n++)
				{
					if (arrA[ n ].CompareTo (arrB[ n ]) != 0)
					{
						return false;
					}
				}

				return true;
			}

			/// <summary>uses Enumerable.SequenceEqual() 
			/// This is most long-running method. Use it only for Unmanaged</summary>'
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eCompareArrays_Linq<T> ( this T[] arrA, T[] arrB ) where T : class //, IComparable<T>
			{
				if (arrA.Length != arrB.Length)
				{
					return false;
				}

				if (!arrA.Any ())
				{
					return true;
				}

				return arrA.SequenceEqual (arrB);
			}



			/// <summary>Использование интерфейса <see cref="System.Collections.IStructuralEquatable"/> - Это новый способ, появился только в NET_4</summary>'
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eCompareArrays_StructuralEquatable<T> ( this T[] arrA, T[] arrB ) where T : IStructuralEquatable, IComparable<T>
			{
				if (arrA.Length != arrB.Length)
				{
					return false;
				}

				if (!arrA.Any ())
				{
					return true;
				}

				return ( arrA as IStructuralEquatable ).Equals (arrB as IStructuralEquatable, StructuralComparisons.StructuralEqualityComparer);
			}



			/// <summary>Использование интерфейса <see cref="System.Collections.IStructuralEquatable"/> - Это новый способ, появился только в NET_4</summary>'
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eAny<T> ( this T arr ) where T : IList, ICollection, IEnumerable
				=> ( arr != null ) && ( arr.Count > 0 );










			//[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET5_0_OR_GREATER
			/// <summary>
			/// Решение на основе векторов из System.Numerics
			/// Теоретически должно работать с аппаратным ускорением, но надо чтобы размер массива был не менее системного размера вектора)
			/// https://docs.microsoft.com/ru-ru/dotnet/api/system.numerics.vector-1?view=net-6.0
			/// </summary>'
			internal static bool eCompareArrays_Vector<T> ( this T[] arrA, T[] arrB ) where T : struct
			{
				if (arrA.Length != arrB.Length)
				{
					return false;
				}

				int platformVectorSize = System.Numerics.Vector<T>.Count;   //On x64 Windows platformVectorSize = 32 byte
																			//bool hwa = System.Numerics.Vector.IsHardwareAccelerated;

				System.Numerics.Vector<T> va, vb;
				int i = 0;

				// Compare main body by blocks,
				// with block size = hardware accelerated platformVectorSize
				int iMax = ( arrA.Length - platformVectorSize );
				for (; i <= iMax ; i += platformVectorSize)
				{
					va = new (arrA, i);
					vb = new (arrB, i);
					if (!System.Numerics.Vector.EqualsAll (va, vb))
					{
						return false;
					}
				}

				// Compare Tail
				if (arrA.Length < platformVectorSize)
				{
					Array.Resize (ref arrA, platformVectorSize);
					Array.Resize (ref arrB, platformVectorSize);
					i = 0;
				}
				else
				{
					i = arrA.Length - platformVectorSize;
				}
				va = new (arrA, i);
				vb = new (arrB, i);

				return System.Numerics.Vector.EqualsAll (va, vb);
			}
#endif

			/*
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eShiftL(this ref int[] arr, int steps = 1, int fillEmpty = default)
			{
				Array.Copy(arr, 0 + steps, arr, 0, arr.Length - steps);
				Span<int> sp = new(arr, arr.Length - steps, steps);
				sp.Fill(fillEmpty);


				//var sl = new Memory<T>();				
				//int bytes2copy = yourArray.length - 4;
				//Buffer.BlockCopy(yourArray, 4, yourArray, 0, bytes2copy);
				//yourArray[yourArray.length - 1] = null;

		}

		internal static void eSwapWith<T>(this ref T x, ref T y)
		{
			T tmp = x; x = y; y = tmp;
		}

			 */
			#endregion











			/*
			internal static (bool) eGroupBy_Bool<T>(this IEnumerable<T> items, Func<T, bool> groupingPredicate)
			{
				var grp = items
					.GroupBy(
					element => groupingPredicate.Invoke(element),
					element => element,
					(value, rows) => new { Value = value, Rows = rows });

				var enabledRows = grp
					.FirstOrDefault(g => g.Enabled == true)?
					.Rows?
					.ToArray()?
					.Length ?? 0;

				var disabledRows = grp
					.FirstOrDefault(g => g.Enabled == false)?
					.Rows?
					.ToArray()?
					.Length ?? 0;
			}
			 */


		}


		internal static partial class Extensions_ListsSync
		{

			#region SYNC Engine

			internal delegate bool IsEqualDelegate<TOld, TNew> ( TOld OldItem, TNew NewItem );

			internal delegate void UpdateOldItemDelegate<TOld, TNew> ( TOld OldItem, TNew NewItem );

			/// <summary>Синхронизирует 2 списка.
			/// Сравнивает "старый" список существующих элементов, с "новым" списком, 
			/// чтобы определить какие элементы из старого списка отсутствуют в новом, 
			/// и каких новых элементов нет в старом списке.</summary>
			/// <typeparam name="TCurrent">Тип сущестующего списка</typeparam>
			/// <typeparam name="TNew">Тип нового списка</typeparam>
			/// <param name="OldList">Текущий список элементов</param>
			/// <param name="NewList">Список новых элементов</param>
			/// <param name="CompareFunc">Сравниватель новых и старых элементов</param>
			/// <param name="UpdateOldItemCallback">Вызывается, когда в новом списке есть такой-же элемент и его данными можно обновить старый список</param>
			/// <param name="OnItemObsolete">Элемент старого списка отсутствует в новом. Его можно удалить или пометить, как старый</param>
			/// <param name="OnNewItemNeedToAdd">New item need to be added to old list</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eSync<TCurrent, TNew> (
				this IEnumerable<TCurrent> OldList,
				IEnumerable<TNew> NewList,
				IsEqualDelegate<TCurrent, TNew> CompareFunc,
				UpdateOldItemDelegate<TCurrent, TNew>? UpdateOldItemCallback = null,
				Action<TCurrent>? OnItemObsolete = null,
				Action<TNew>? OnNewItemNeedToAdd = null )
			{
				if (!OldList.Any () && !NewList.Any ())
				{
					return;
				}

				// В старом списке ищем записи, кторорых нет в новых данных, для пометки как устаревшие и возможного удаления
				foreach (var rOld in OldList)
				{
					// Для каждого существующего элемента проверяем есть ли он в новом списке
					var OldRecordsNeedToBeUpdated = NewList.Where (rNew => CompareFunc.Invoke (rOld, rNew));
					if (OldRecordsNeedToBeUpdated.Any ())
					{
						// New list contains any elements as in Old list
						if (UpdateOldItemCallback != null)
						{
							OldRecordsNeedToBeUpdated.eForEach (rNewItem => UpdateOldItemCallback?.Invoke (rOld, rNewItem));// Обновляем элемент старого списка новыми значениями
						}
					}
					else // В новом списке нет этого старого элемента
					{
						OnItemObsolete?.Invoke (rOld);// Сообщаем что элемент устарел (РЕШИТЬ ВОПРОС С ВОЗМОЖНОСТЬЮ УДАЛЕНИЯ!!!) Внутри вызова можно удалять элемент, т.к. щас смотрим array-копию старого списка и это безопасно
					}
				}

				// Ищем новые элементы, которых нет в старом списке
				foreach (var rNew in NewList)
				{
					// Для каждого элемента нового списка проверяем есть ли он в старом списке
					var aНовыйЭлеменНайденВСтаромСписке = OldList.Where (rOld => CompareFunc.Invoke (rOld, rNew));
					if (aНовыйЭлеменНайденВСтаромСписке.Any ())
					{
						//
					}
					// В старом списке есть такой же элемент (элементы уже обновлены в первом абзаце)
					else // В старом списке нет этого нового элемента
					{
						OnNewItemNeedToAdd?.Invoke (rNew);
					}
				}
			}
			#endregion

		}


		internal static partial class Extensions_Enum
		{


			//public static bool eHas<T>(this System.Enum type, T value)
			//{
			//    try { return (((int)(object)type & (int)(object)value!) == (int)(object)value); }
			//    catch { return false; }
			//}



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool eEqualsOneOf<TEnum> ( this TEnum value, params TEnum[] values ) where TEnum : Enum
				=> values.FirstOrDefault (e => e.Equals (value)) != null;


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static T eBuildFromFlags<T> ( this T initialValue, params (bool flagCondition, T flagToSet)[] flags ) where T : Enum
			{
				Int64 val = Convert.ToInt64 (initialValue);
				foreach (var item in flags)
				{
					if (item.flagCondition)
					{
						val |= Convert.ToInt64 (item.flagToSet);
					}
				}
				T TResult = (T) Enum.ToObject (typeof (T), val);
				return TResult;
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static T1 eBuildFromFlags<T1, T2> ( this T1 initialValue, params (T2 value, T2 flagToCheck, T1 flagToSet)[] flags ) where T1 : Enum where T2 : Enum
			{
				Int64 val = Convert.ToInt64 (initialValue);
				foreach (var item in flags)
				{
					if (item.value.HasFlag (item.flagToCheck))
					{
						val |= Convert.ToInt64 (item.flagToSet);
					}
				}
				T1 TResult = (T1) Enum.ToObject (typeof (T1), val);
				return TResult;
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool eIs<T> ( this System.Enum type, T value )
			{
				try { return (int) (object) type == (int) (object) value!; }
				catch { return false; }
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static T eAdd<T> ( this System.Enum type, T value )
			{
				try { return (T) (object) ( ( (int) (object) type | (int) (object) value! ) ); }
				catch (Exception ex) { throw new ArgumentException ($"Could not append value from enumerated type '{typeof (T).Name}'", ex); }
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static T eRemove<T> ( this System.Enum type, T value )
			{
				try { return (T) (object) ( ( (int) (object) type & ~(int) (object) value! ) ); }
				catch (Exception ex) { throw new ArgumentException ($"Could not remove value from enumerated type '{typeof (T).Name}'", ex); }
			}


#if !ANDROID

			public static Int32 eMixFlagsAsInt32<T> ( this T flag, params T[] flagsToExclude ) where T : Enum
			{
				if (flag == null)
				{
					throw new ArgumentNullException (nameof (flag));
				}

				var allValues = Enum
					.GetValues (typeof (T))
					.Cast<T> ()
					.Where (e => !flagsToExclude.Contains (e))
					.Select (e => (Int32) (object) e)
					.ToArray ();

				Int32 mixResult = 0;
				foreach (var f in allValues)
				{
					mixResult |= f;
				}

				return mixResult;
			}

#endif








			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static (bool IsDirectDefinedEnumValue, FieldInfo? DirectDefinedEnumFieldInfo, bool IsMaskedEnumValue, Enum[] MaskValues, FieldInfo[] MasksFieldInfos) eGetEnumValueInfo ( this Enum eValue )
			{
				Type T = eValue.GetType ();
				bool isDefinedEnumField = ( Enum.IsDefined (T, eValue) );
				if (isDefinedEnumField) //This value is direct defined in ENUM! not bit mask
				{
					string enumFieldName = eValue.ToString ();
					FieldInfo fi = T.GetField (enumFieldName)!;
					return (true, fi, false, Array.Empty<Enum> (), Array.Empty<FieldInfo> ());
				}

				//This is bitmasked enum, build result from flags
				var fields = Enum
					.GetValues (T)
					.Cast<Enum> ()
					.Where (enumFieldValue => eValue.HasFlag (enumFieldValue))
					.Select (enumFieldValue => (MaskValue: enumFieldValue, MaskFieldInfo: T.GetField (enumFieldValue.ToString ())!));

				if (!fields.Any ())
				{
					return (false, null, false, Array.Empty<Enum> (), Array.Empty<FieldInfo> ());
				}

				Enum[] maskValue = fields.Select (ff => ff.MaskValue).ToArray ();
				FieldInfo[] maskInfo = fields
					.Select (ff => ff.MaskFieldInfo)
					.ToArray ();
				return (false, null, true, maskValue, maskInfo);
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static T[] eGetEnumFieldCustomAttributes<T> ( this Enum eValue, bool inherit = true ) where T : System.Attribute
			{
				(bool isDirectDefinedEnumValue, FieldInfo? efi, _, _, _) = eValue.eGetEnumValueInfo ();
				if (!isDirectDefinedEnumValue || efi == null)
				{
					throw new ArgumentOutOfRangeException (nameof (eValue));
				}

				var attrs = efi!.GetCustomAttributes<T> (inherit);
				if (!attrs.Any ())
				{
					return Array.Empty<T> ();
				}

				return attrs.ToArray ();
			}



			///<summary>Return value of <see cref="System.ComponentModel.DescriptionAttribute"/></summary>    
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eGetDescriptionValue ( this Enum value, bool ignoreZerroFlagInMultiValue = true, bool ignoreNegativeFlagsInMultiValue = true, string multiFlagsSeparator = ", " )
			{
				static string cbGetEnumFiledDescr ( Type enumType, Enum enumValue )
				{
					string fieldName = enumValue.ToString ();
					return ( (DescriptionAttribute?) ( enumType!
						.GetField (fieldName)!
						.GetCustomAttributes (typeof (DescriptionAttribute), true) )
						.FirstOrDefault () )?.Description ?? fieldName;
				}

				var T = value.GetType ();
				if (Enum.IsDefined (T, value))
				{
					return cbGetEnumFiledDescr (T, value);//This value id direct defined in ENUM! not bit mask
				}

				/*
		 string sFieldName = rEnumValue.ToString();
		 return ((DescriptionAttribute?)T
			 .GetField(sFieldName)
			 .GetCustomAttributes(typeof(DescriptionAttribute), true)
			 .FirstOrDefault())?.Description ?? DefaultValue;
				 */

				//This is bitmasked enum, build result from flags
				return Enum
					.GetValues (T)
					.Cast<Enum> ()
					.Where (v => !ignoreNegativeFlagsInMultiValue || ( Convert.ToInt64 (v) >= 0L ))
					.Where (v => !ignoreZerroFlagInMultiValue || ( Convert.ToInt64 (v) != 0L ))
					.Where (v => value.HasFlag (v))
					.Select (v => cbGetEnumFiledDescr (T, v))
					.ToArray ()
					.eJoin (multiFlagsSeparator)!;

				/*
		  var aAllEnumValues = Enum.GetValues(T).Cast<Enum>().ToArray();
		  var lDescriptionsList = new List<string>(aAllEnumValues.Length);
		  foreach (var eFieldValue in aAllEnumValues)
		  {
			  //if (rEnumValue.HasFlag(eFieldValue))
			  {
				  string sFieldName = cbGetEnumFiledDescr(T, eFieldValue);
				  /*
				  string sFieldName = ((DescriptionAttribute?)T
					  .GetField(eFieldValue.ToString())
					  .GetCustomAttributes(typeof(DescriptionAttribute), true)
					  .FirstOrDefault())?.Description ?? eFieldValue.ToString();
				lDescriptionsList.Add(sFieldName);
			}
		}
		var S = lDescriptionsList.eJoin(", ");
				return S;
				   */

				//}
			}




			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static TEnum eToEnumValue<TEnum> ( this string valueName, TEnum defaultValue ) where TEnum : Enum
			{
				var et = typeof (TEnum);
				var values = Enum.GetValues (et).Cast<TEnum> ();

				var found = Enum
					.GetValues (et)
					.Cast<TEnum> ()
					.FirstOrDefault (v => v.ToString ()!.Equals (valueName, StringComparison.InvariantCultureIgnoreCase));

				if (found != null)
				{
					return found;
				}

				return defaultValue;
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static TEnum eToEnumValue<TEnum> ( this string valueName ) where TEnum : Enum
				=> valueName.eToEnumValue<TEnum> ((TEnum) (object) 0);


			/// <returns>
			/// Returns "{EnumTypeName}.{ValueName}"
			/// </returns>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eGetFullName ( this Enum e, bool fullName )
			{
				Type t = e.GetType ();
				string n = fullName
					? t.FullName!
					: t.Name;

				return $"{n}.{e}";
			}



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static TypeCode eGetEnumTypeCode<T> ( this T e ) where T : Enum => Type.GetTypeCode (typeof (T).GetEnumUnderlyingType ());


			/// <summary>Gets the enum default (zero) value.</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static Enum? eGetEnumDefaultValue ( this Type enumType )
				=> enumType == null
					? null
					: (Enum?) Activator.CreateInstance (enumType);


			/// <summary>Returns a value indicating whether a zero value is valid for the Enum type.</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eIsZeroValueDefined ( this Type enumType )
				=> Enum.IsDefined (enumType, enumType.eGetEnumDefaultValue ()!);





			#region UInt64 inplementation of net 9 IEnumerable.Index()

			/// <summary>Returns an enumerable that incorporates the element's index into a tuple.</summary>
			/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
			/// <param name="source">The source enumerable providing the elements.</param>
			/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<(ulong Index, TSource Item)> AsIndexedU64<TSource> ( this IEnumerable<TSource> source )
			{
				if (source is null) throw new ArgumentNullException (nameof (source));
				return source!.IndexIteratorU64 ();
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			private static IEnumerable<(ulong Index, TSource Item)> IndexIteratorU64<TSource> ( this IEnumerable<TSource> source )
			{
				var index = 0ul;
				foreach (var element in source)
				{
					yield return (index, element);

					checked
					{
						index++;
					}
				}
			}

#if NET9_0_OR_GREATER

			/// <summary>Returns an enumerable that incorporates the element's index into a tuple.</summary>
			/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
			/// <param name="source">The source enumerable providing the elements.</param>
			/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<(int Index, TSource Item)> AsIndexed<TSource> ( this IEnumerable<TSource> source )
				=> source.Index ();

#else

			/// <summary>Returns an enumerable that incorporates the element's index into a tuple.</summary>
			/// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
			/// <param name="source">The source enumerable providing the elements.</param>
			/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<(int Index, TSource Item)> AsIndexed<TSource> (this IEnumerable<TSource> source)
			{
				if (source is null) throw new ArgumentNullException(nameof(source));
				return source!.IndexIterator();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static IEnumerable<(int Index, TSource Item)> IndexIterator<TSource> (this IEnumerable<TSource> source)
			{
				int index = 0;
				foreach (var element in source)
				{
					yield return (index, element);

					checked
					{
						index++;
					}
				}
			}
#endif


			#endregion




			/*


		////// <summary>Возвращает значение атрибута <see cref="My.UOM.EnumTools.Description2Attribute"/> </summary>    
		<Obsolete>
		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>

		Friend Function ExtEnum_GetDescription2Value(ByVal E As[Enum]) As string
		return New My.UOM.EnumTools.EnumTypeConverter(E.typeof).ConvertToString(E)
		End Function

		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_GetDescription2Value(EnumValue As [Enum],
		 rObject As Object,
		 EnumValueFieldName As string,
		 Optional bTrowErrorIfCommentAttributeNotFound As bool  = true) As string

		var OT = rObject.typeof
		var S = ExtEnum_GetDescription2Value(EnumValue, OT, EnumValueFieldName, bTrowErrorIfCommentAttributeNotFound)
		return S
		End Function

		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_GetDescription2Value(EnumValue As [Enum],
		 rObjectType As System.Type,
		 EnumValueFieldName As string,
		 Optional bTrowErrorIfCommentAttributeNotFound As bool  = true) As string


		var sEnumValue = EnumValue.ToString

		var VT = EnumValue.typeof
		var bIsDefinedInEnum = [Enum].IsDefined(VT, EnumValue)
		if(Not bIsDefinedInEnum) {
		if(Not bTrowErrorIfCommentAttributeNotFound) { return sEnumValue
		var sErr = "Enum {0} не содержит элемента = {1}!". eFormat(VT.ToString, sEnumValue)
		return sEnumValue
		}

		var OT = rObjectType
		var aProperties = OT.GetMember(EnumValueFieldName).ToArray
		if(Not aProperties.Any) {
		//if (Not bTrowError) { return sEnumValue
		var sErr = "Класс //{0}// не содержит поля или свойства //{1}//!". eFormat(OT.ToString, EnumValueFieldName)
		Throw New ArgumentOutOfRangeException(EnumValueFieldName, sErr)
		ElseIf(aProperties.Count<> 1) {
		//if (Not bTrowError) { return sEnumValue
		var sErr = "Класс //{0}// содержит БОЛЕЕ ОДГОГО поля или свойства //{1}//!". eFormat(OT.ToString, EnumValueFieldName)
		Throw New ArgumentOutOfRangeException(EnumValueFieldName, sErr)
		}

		var rFirstProperty = aProperties.First
		var ECAT = typeof(My.UOM.EnumTools.Description2Attribute)
		var aAttrs = rFirstProperty.GetCustomAttributes(ECAT, false)
		var aAttrs2 = aAttrs.Cast(Of My.UOM.EnumTools.Description2Attribute).ToArray
		if Not aAttrs2.Any { //Не найден  ни один аттрибут
		if(Not bTrowErrorIfCommentAttributeNotFound) { return sEnumValue
		var sErr = "У свойства //{0}.{1}// не задан ни один аттрибут //{2}!". eFormat(OT.ToString, EnumValueFieldName, ECAT.ToString)
		Throw New ArgumentException(sErr)
		}

		For Each rAttr In aAttrs2
		var eAttr As System.Enum = CType(rAttr.Value, [Enum])
		if eAttr.CompareTo(EnumValue) = 0 {
		sEnumValue = rAttr.Description
		return sEnumValue
		}
		Next

		return sEnumValue
		End Function














		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_IsFlagSet(Of TEnum As Structure)(rEnumValue As TEnum,
				 EnumFlag As TEnum) As Boolean

		var iEnumValue = System.Convert.ToInt32(rEnumValue)
		var iEnumFlag = System.Convert.ToInt32(EnumFlag)
		return iEnumValue. eIsBitsSetByMask(iEnumFlag)
		End Function

		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_SetFlag(Of TEnum As Structure)(rEnumValue As TEnum,
			   EnumFlagToSet As TEnum,
			   Optional ByVal bSet As bool  = true) As TEnum

		var iEnumValue = System.Convert.ToInt32(rEnumValue)
		var iEnumFlag = System.Convert.ToInt32(EnumFlagToSet)

		iEnumValue = iEnumValue. eSetBitsByMask(iEnumFlag)
		var objValue As Object = iEnumValue
		return DirectCast(objValue, TEnum)
		End Function



	# Region "OLD"


		//    //////<summary>Возвращает значение атрибута<see cref="System.ComponentModel.DescriptionAttribute"/> </summary>    
		//    <System.Runtime.CompilerServices.Extension()>
		//    Friend Function ExtEnum_GetFieldDescription(E As [Enum],
		//                                             Optional AttributeNotFoundDefaultValue As string = null) As string

		//        return Enum_GetFieldDescription(E.typeof, E.ToString, AttributeNotFoundDefaultValue)
		//    End Function




		//    //////<summary>Возвращает значение атрибута<see cref="System.ComponentModel.DescriptionAttribute"/> </summary>    
		//    <System.Runtime.CompilerServices.Extension()>
		//    Friend Function ExtEnum_GetFieldDescription(EnumType As Type,
		//                                             EnumValueString As string,
		//                                             Optional AttributeNotFoundDefaultValue As string = null) As string


		//        var T = EnumType
		//        var aEnumTypeFields = T.GetFields().ToArray //Все значения этого ENUMA

		//        var aEnumFieldsInValue = (From FLD In aEnumTypeFields Where EnumValueString.Contains(FLD.Name)).ToArray
		//        if Not (aEnumFieldsInValue.Any) {
		//            //Ни одно из полей типа ENUM не входит в данную строку!
		//            return AttributeNotFoundDefaultValue
		//        }


		//        var F = aEnumFieldsInValue.First
		//        var aDA = F.GetCustomAttributes(typeof(DescriptionAttribute), true).ToArray
		//        if (Not aDA.Any) {

		//#if DEBUG {
		//            var MSG = string.Format("!!! Enum_GetFieldDescription, для поля: //{0}// (типа: //{1}//) нет поля //System.ComponentModel.DescriptionAttribute//!",
		//                                    EnumValueString,
		//                                    T.ToString)

		//            Call DEBUG_SHOW_LINE(MSG)
		//            //Throw New Exception(MSG)
		//#}
		//            return AttributeNotFoundDefaultValue
		//        }

		//        var rFirst = aDA.First
		//        var DA = DirectCast(rFirst, DescriptionAttribute)
		//        return DA.Description
		//        }
		//        return AttributeNotFoundDefaultValue
		//    End Function


	# End Region




		////// <summary>Разделяем составной тип ENUM (Собранный через OR) на флаги</summary>
		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_SplitToFlags(ByVal E As[Enum]) As[Enum] ()
		var aResult() As[Enum] = {}
		if(E IsNot Nothing) {
		var T = E.typeof
		var aTypeValues = [Enum].GetValues(T)

		//if CLng(E) = 0 {
		//Нулевое значение
		//}else{
		aResult = (From rTypeValue As[Enum] In aTypeValues.Cast(Of[Enum])()
		Where(E.HasFlag(rTypeValue))
		Select rTypeValue).ToArray
		}
		return aResult
		End Function

		////// <summary>Разделяем составной тип ENUM (Собранный через OR) на флаги</summary>
		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_SplitToFlagsAsStrings(ByVal E As[Enum]) As string
		var aResult = ExtEnum_SplitToFlags(E)
		var sbResult = ""
		For Each V In aResult
		sbResult &= V.ToString
		sbResult &= " "
		Next
		sbResult = sbResult.Trim
		return sbResult
		End Function


		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_GetAllValuesAsNumericFlags(ByVal EnumType As System.Type) As Long
		var Arr = EnumType.GetEnumValues // [Enum].GetValues(EnumType)
		var TotalValue As Long = 0
		For Each EnumItemValue As Long In Arr
		TotalValue = (TotalValue Or EnumItemValue)
		Next EnumItemValue
		return TotalValue
		End Function

		////// <summary>НЕ ИСПОЛЬЗОВАТЬ вот так: typeof(XXX).EnumGetAllValuesArray</summary>
		<DebuggerNonUserCode, DebuggerStepThrough>
		<MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
		Friend Function ExtEnum_GetAllValuesArray(Of T)(EnumItem As T) As T()

		//***ВОТ ТАК РАБОТАЕТ!
		//var RR As ACCEPT_STATE_CONTAINER.eAcceptStates = ACCEPT_STATE_CONTAINER.eAcceptStates.NotApprovedAndNotDeclined
		//var eAA1 = RR.EnumGetAllValuesArray

		//*** А ВОТ ТАК НЕ БУДЕТ РАБОТАТЬ!!! 
		//var eAA2 = typeof(XXX).EnumGetAllValuesArray

		var TT As System.Type = EnumItem.typeof
		var objArr = TT.GetEnumValues
		var tArr = objArr.Cast(Of T)()
		return tArr.ToArray
		End Function




		End Module




				Namespace My.UOM.EnumTools

				////// <summary>
				////// Используется для задания описания к полю ENUMa, непосредственно в коде (у свойства) 
				////// или если этот енум определён не нами и нельзя в каждому его полю добавить  <see cref="DescriptionAttribute"/> для ENUM
				////// Пример применения см. в самом начале класса...
				////// </summary>
				<AttributeUsage(AttributeTargets.Property Or AttributeTargets.Enum Or AttributeTargets.Field, AllowMultiple:=true)>
				Friend Class Description2Attribute
					Inherits System.Attribute

					Public ReadOnly Property Value As Object = Nothing
					Public ReadOnly Property Description As string = Nothing

					Sub New(ByVal EnumElement As Object, ByVal ElementDescription As string)
						MyBase.New()

						Me.Value = EnumElement
						Me.Description = ElementDescription
					End Sub
				End Class

	# Region "EnumTypeConverter"
				Friend Class EnumTypeConverter
					Inherits System.ComponentModel.EnumConverter
					Public Sub New(ByVal type As System.Type)
						MyBase.New(type)
					End Sub

					Public Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext,
														ByVal culture As System.Globalization.CultureInfo,
														ByVal value As Object, ByVal destinationType As System.Type) As Object


						if(value IsNot Nothing) AndAlso destinationType.Equals(typeof(System.string)) {
							//Требуется приеобразовать значение в строку

							//var sEnumValueForConvertToString As string = value.ToString

							////Проверяем что есть атрибут EnumCommentAttribute
							//if (context IsNot Nothing) _
							//    AndAlso (context.PropertyDescriptor IsNot Nothing) _
							//    AndAlso (context.Instance IsNot Nothing) {

							//    var sPropertyName = context.PropertyDescriptor.Name
							//    var rObjectRef = context.Instance

							//    var aMI() = rObjectRef.typeof.GetMember(sPropertyName)
							//    For Each rMemberInfo In aMI
							//        var aEnumCommentAttributes() = CType(System.Attribute.GetCustomAttributes(rMemberInfo, typeof(EnumCommentAttribute)), EnumCommentAttribute())
							//        if (aEnumCommentAttributes.Any) {
							//            For Each ATTR In aEnumCommentAttributes
							//                if ATTR.Value.Equals(value) {
							//                    //Нашли!
							//                    return ATTR.Comment
							//                }
							//            Next
							//        }
							//    Next rMemberInfo
							//}

							////У свойства объекта нет атрибута Проверяем что у самого элемента ENUMа есть атрибут System.ComponentModel.DescriptionAttribute
							//var FI As System.Reflection.FieldInfo = value.typeof().GetField(sEnumValueForConvertToString)
							//if (FI IsNot Nothing) {
							//    var aDescriptionAttributes() = CType(FI.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false), System.ComponentModel.DescriptionAttribute())
							//    if (aDescriptionAttributes IsNot Nothing) AndAlso (aDescriptionAttributes.Any) {
							//        return (aDescriptionAttributes.First.Description)
							//    }
							//}
							//var V = GetComment(value)
							//return V
							////У элемента нет атрибута EnumDisplayNameAttribute


							var E = CType(value, [Enum])

							//Сначала пытаемся прочитать System.ComponentModel.DescriptionAttribute
							var sDescription = E.ExtEnum_GetDescriptionValue(null)
							if (sDescription. IsNullOrWhiteSpace) { //DescriptionAttribute не найден!
								//Читаем EnumCommentAttribute
								sDescription = ExtEnum_GetDescription2Value(E)
							}
							return sDescription
						}
						return MyBase.ConvertTo(context, culture, value, destinationType)
					End Function


					Public Overrides Function ConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext,
														  ByVal culture As System.Globalization.CultureInfo,
														  ByVal value As Object) As Object
						Try
							if(Me.EnumType IsNot Nothing) _
							   AndAlso(value IsNot Nothing) _
							  AndAlso(value.typeof.Equals(typeof(System.string))) { //Требуется преобразовать значение из строки

								var sValue = CStr(value) //строка для которой надо найти значение ENUMа
								var ValueType = Me.EnumType //Получаем тип нашего ENUMа

								if(Not System.ComponentModel.TypeDescriptor.GetConverter(ValueType).IsValid(value)) {
									//в самом ENUMе нету элемента с таким именем! надо искать по строке-описанию!


									//Проверяем что у редакритуемого свойства есть атрибут EnumCommentAttribute
									if(context IsNot Nothing) _
									   AndAlso(context.PropertyDescriptor IsNot Nothing) _
									  AndAlso(context.Instance IsNot Nothing) {

									 var rObjRef = context.Instance
									 var sPropertyName As string = context.PropertyDescriptor.Name

									 var aMI() = rObjRef.typeof.GetMember(sPropertyName)
									 For Each MI In aMI
											var aEnumCommentAttributes() = CType(System.Attribute.GetCustomAttributes(MI, typeof(Description2Attribute)), Description2Attribute())
											if(aEnumCommentAttributes IsNot Nothing) AndAlso(aEnumCommentAttributes.Any) {
											  For Each ATTR As Description2Attribute In aEnumCommentAttributes
													if ATTR.Description.ToUpper.Equals(sValue.ToUpper) { //Нашли!
														//Получаем все имена элементов нашего ENUMа текстом
														For Each oEnumItem As Object In[Enum].GetValues(ValueType)
															//Throw New NotImplementedException
															if ATTR.Value.Equals(oEnumItem) {
																return oEnumItem
															}
														Next oEnumItem
													}
												Next
											}
										Next
									}

									//Получаем все имена элементов нашего ENUMа текстом
									var aEnumNames() = [Enum].GetNames(ValueType)
									//Для каждого из значений ENUMа смотрим есть ли у него аттрибут System.ComponentModel.DescriptionAttribute с переданной нам строкой
									For Each sEnumItem In aEnumNames
										var FI As System.Reflection.FieldInfo = ValueType.GetField(sEnumItem)
										var aDescriptionAttributes() = CType(FI.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false), System.ComponentModel.DescriptionAttribute())
										if(aDescriptionAttributes IsNot Nothing) AndAlso(aDescriptionAttributes.Any) {
										  var sEnumItemDisplayName As string = aDescriptionAttributes(0).Description
										  if sEnumItemDisplayName.Equals(sValue) {
												//Нашли элемент в ENUM с таким атрибутом
												return MyBase.ConvertFrom(context, culture, sEnumItem)
											}
										}
									Next sEnumItem
									//ни у одного элемента данного ENUMа нет атрибута EnumDisplayNameAttribute с таким значением
								}
							}

						Catch ex As Exception
	#if DEBUG {
							ex.eLogError(true)
	#}
						End Try
						return MyBase.ConvertFrom(context, culture, value)
					End Function



					////// <summary>Возвращает значение атрибута <see cref="Description2Attribute"/> для ENUM</summary>
					Private Shared Function ExtEnum_GetDescription2Value(ByVal V As[Enum],
																			  Optional ByVal MultiEnumSplitter As string = ", ") As string
						var ET = V.typeof
						if([Enum].IsDefined(ET, V)) {
							//Это одиночное значение ENUMа, определённое в нём, а не составленное из нескольких значений через OR

							var AAA = ET.GetCustomAttributes(typeof(Description2Attribute), false)
							var aComments = AAA.Cast(Of Description2Attribute)().ToArray
							if(aComments.Any) {
							   var rComment = (From EnumValue In aComments Where(EnumValue.Value.Equals(V))).ToArray
							  if(rComment.Any) {
								 var Atr = rComment.First
								 return Atr.Description
							 }
						 }

	#if DEBUG {
							var MSG = string.Format("!!! Enum_GetEnumCommentAttributeValue, для одиночного значения: //{0}// (тип: //{1}//) нет поля //EnumCommentAttribute//! - используем текстовое значение .ToString = //{0}//",
													V.ToString,
													VType.ToString)

							Call DEBUG_SHOW_LINE(MSG)
	#}

						}else{ //Это составной тип ENUM (Собранный через OR)
							var aEnumFlags = ExtEnum_SplitToFlags(V)
							if(aEnumFlags.Any) {
							   var A = (From E In aEnumFlags
										Let S = ExtEnum_GetDescription2Value(E)
										 Select S).ToArray

								var B = A. eJoin(MultiEnumSplitter)
								return B
							}


	#if DEBUG {
							var MSG = string.Format("!!! Enum_GetEnumCommentAttributeValue, для систавного значения: //{0}// (тип: //{1}//) нет полей //EnumCommentAttribute//! - используем текстовое значение .ToString = //{0}//",
													V.ToString,
													VType.ToString)

							Call DEBUG_SHOW_LINE(MSG)
	#}

						}


						return V.ToString
					End Function




				End Class
	# End Region



			End Namespace
			 */

		}


		internal static partial class Extensions_DateTime
		{
			internal const string CS_DATETIME_YEAR = "yyyy";
			internal const string CS_DATETIME_MONTH_NUM = "MM";
			internal const string CS_DATETIME_MONTH_NAME = "MMM";
			internal const string CS_DATETIME_DAY = "dd";
			internal const string CS_DATETIME_HOUR = "HH";
			internal const string CS_DATETIME_MINUTE = "mm";
			internal const string CS_DATETIME_SECONDS = "ss";
			internal const string CS_DATETIME_SECONDSFRACTION = "ffff";
			internal const string C_FMT_DATETIME_LONG = "dd MMM yyyy, HH:mm:ss";
			internal const string C_FMT_DATETIME_LONGTIMESTAMP = "HH:mm:ss.fff";
			internal const string C_FMT_DATETIME_LONGDATETIMESTAMP = "dd MMM yyyy, " + C_FMT_DATETIME_LONGTIMESTAMP;
			internal const string C_FMT_DATETIME_LONGFILEDATETIMESTAMP = "yyyy-MM-dd__HH-mm-ss-fff";

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eReplaceDateTimePattern ( this string SourceString, DateTime DateToInsert )
			{
				SourceString = SourceString.Replace (CS_DATETIME_YEAR, DateToInsert.Year.ToString ().PadLeft (CS_DATETIME_YEAR.Length, '0'));
				SourceString = SourceString.Replace (CS_DATETIME_MONTH_NUM, DateToInsert.Month.ToString ().PadLeft (CS_DATETIME_MONTH_NUM.Length, '0'));
				SourceString = SourceString.Replace (CS_DATETIME_DAY, DateToInsert.Day.ToString ().PadLeft (CS_DATETIME_DAY.Length, '0'));
				SourceString = SourceString.Replace (CS_DATETIME_HOUR, DateToInsert.Hour.ToString ().PadLeft (CS_DATETIME_HOUR.Length, '0'));
				SourceString = SourceString.Replace (CS_DATETIME_MINUTE, DateToInsert.Minute.ToString ().PadLeft (CS_DATETIME_MINUTE.Length, '0'));
				SourceString = SourceString.Replace (CS_DATETIME_SECONDS, DateToInsert.Second.ToString ().PadLeft (CS_DATETIME_SECONDS.Length, '0'));
				SourceString = SourceString.Replace (CS_DATETIME_SECONDSFRACTION, DateToInsert.Millisecond.ToString ().PadLeft (CS_DATETIME_SECONDSFRACTION.Length, '0'));
				return SourceString;
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static DateTime eRemoveSeconds ( this DateTime DT ) => new (DT.Year, DT.Month, DT.Day, DT.Hour, DT.Minute, 0);


			/// <summary>Следующий День</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static DateTime eNextDay ( this DateTime DT ) => DT.Date.AddDays (1d);


			/// <summary>"dd MMM yyyy, HH:mm:ss"</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eToLongDateTimeString ( this DateTime DT ) => DT.ToString (C_FMT_DATETIME_LONG);


			/// <summary>"dd MMM yyyy, HH:mm:ss.fff"</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eToLongDateTimeStamp ( this DateTime DT ) => DT.ToString (C_FMT_DATETIME_LONGDATETIMESTAMP);


			/// <summary>"HH:mm:ss.fff"</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eToLongTimeStamp ( this DateTime DT ) => DT.ToString (C_FMT_DATETIME_LONGTIMESTAMP);


			/// <summary>"yyyy-MM-dd__HH-mm-ss-fff"</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eToFileName ( this DateTime DT ) => DT.ToString (C_FMT_DATETIME_LONGFILEDATETIMESTAMP).Trim ();


			#region To MS Access Format

			internal enum AccessTimeModes
			{
				ВзятьИзДаты,
				НачалоПериода,
				КонецПериода
			}
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eToAccessFormat ( this DateTime SourceData, AccessTimeModes Mode = AccessTimeModes.ВзятьИзДаты )
			{
				SourceData = Mode switch
				{
					AccessTimeModes.НачалоПериода => SourceData.Date,
					AccessTimeModes.КонецПериода => new DateTime (SourceData.Year, SourceData.Month, SourceData.Day, 23, 59, 59),
					_ => throw new NotImplementedException (),
				};

				return string.Format ("#{1}/{0}/{2} {3}:{4}:{5}#",
					SourceData.Day.ToString ("00"),
					SourceData.Month.ToString ("00"),
					SourceData.Year.ToString ("0000"),
					SourceData.Hour.ToString ("00"),
					SourceData.Minute.ToString ("00"),
					SourceData.Second.ToString ("00"));
			}
			#endregion





		}


		internal static partial class Extensions_IO
		{

			/// <summary>Just test file exist to throw error if not.</summary>
			/// <exception cref="System.IO.FileNotFoundException"></exception>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void ethrowIfNotExist ( this string path )
			{
				var fsi = path.eToFileSystemInfo ();
				if (!fsi!.Exists)
				{
					throw new System.IO.FileNotFoundException (null, path);
				}
				//var atr = System.IO.File.GetAttributes(path);
				//return !(atr.HasFlag(FileAttributes.Directory));
			}

			/// <summary>"Add \\?\ to start of string</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string ePathAddLongPathPrefix ( this string sPath )
			{
				if (OSInfo.IsOSPlatform_Windows && sPath != null && !sPath.StartsWith (uom.I_O.CS_PATH_PREFIX_WIN_LONG_PATH))
				{
					sPath = uom.I_O.CS_PATH_PREFIX_WIN_LONG_PATH + sPath;
				}

				return sPath!;
			}


			/// <summary>"Remove \\?\ from start of string</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string ePathRemoveLongPathPrefix ( this string sPath )
			{
				if (OSInfo.IsOSPlatform_Windows && ( sPath != null ) && sPath!.StartsWith (uom.I_O.CS_PATH_PREFIX_WIN_LONG_PATH))
				{
					sPath = sPath.Substring (I_O.CS_PATH_PREFIX_WIN_LONG_PATH.Length);
				}

				return sPath!;
			}

			/// <summary>"Remove \\?\ from start of string</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eFullName_RemoveLongPathPrefix ( this FileSystemInfo fsiPath ) => fsiPath.FullName.ePathRemoveLongPathPrefix ();


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static DirectoryInfo eToDirectoryInfo ( this string sPath, bool AddLongPathSupportIfNeed = false )
			{
				if (AddLongPathSupportIfNeed)
				{
					sPath = sPath.ePathAddLongPathPrefix ();
				}

				return new DirectoryInfo (sPath);
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static FileInfo eToFileInfo ( this string sPath, bool AddLongPathSupportIfNeed = false )
			{
				if (AddLongPathSupportIfNeed)
				{
					sPath = sPath.ePathAddLongPathPrefix ();
				}

				return new FileInfo (sPath);
			}






			/// <summary>Multiplatform FileAttributes.ReparsePoint
			/// The file contains a reparse point, which is a block of user-defined data associated with a file or a directory. 
			/// ReparsePoint is supported on Windows, Linux, and macOS.
			/// </summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool eIsNTFS_SymLinkMP ( this FileSystemInfo fsi ) => fsi.Attributes.HasFlag (FileAttributes.ReparsePoint) && !fsi.Attributes.HasFlag (FileAttributes.SparseFile);

#if NET6_0

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string eSymLinkToString(this FileSystemInfo fsi)
			{
				if (!fsi.eIsNTFS_SymLinkMP()) return fsi.ToString();
				var sMsg = $"SymLink '{fsi.eFullName_RemoveLongPathPrefix()}'";
				try
				{
					if (fsi.LinkTarget != null) sMsg += $" => '{fsi.LinkTarget}'";
				}
				catch { }
				return sMsg;
			}
#endif



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static Uri eToURI ( this string sourceText ) => new (sourceText);


			/// <summary>Replaces sample: 'c:\windows\system23' -> 'c_windows_system23'</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eToFlatFileSystemString ( this string src, char cReplaceWith = '_', bool replaceDots = false )
			{
				var lInvalidChars = new List<char> ();
				lInvalidChars.AddRange (Path.GetInvalidPathChars ());
				lInvalidChars.AddRange (Path.GetInvalidFileNameChars ());
				lInvalidChars.AddRange ("%");
				if (replaceDots)
				{
					lInvalidChars.AddRange (".");
				}

				var aInvalidChars = lInvalidChars.Distinct ().ToArray ();

				foreach (var C in aInvalidChars)
				{
					while (src.Contains (Convert.ToString (C)))
					{
						src = src.Replace (C, cReplaceWith);
					}
				}

				// Заменяем двойные символы на одинарные
				string C_REPLACE_CHAR2 = new (cReplaceWith, 2);//.ToString() + cReplaceWith;
				while (src.Contains (C_REPLACE_CHAR2))
				{
					src = src.Replace (C_REPLACE_CHAR2, cReplaceWith.ToString ());
				}

				return src;
			}









			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static FileSystemInfo eToFileSystemInfo ( this string path )
			{
				if (File.Exists (path))
				{
					return new FileInfo (path);
				}

				if (Directory.Exists (path))
				{
					return new DirectoryInfo (path);
				}
				// invalid path or does not exist
				throw new System.IO.FileNotFoundException ($"'{path}' was not found!");
			}



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static FileInfo eToFileInfo ( this string sPath ) => new (sPath);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static DirectoryInfo eToDirectoryInfo ( this string sPath ) => new (sPath);



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static DirectoryInfo eParent ( this FileSystemInfo fsi )
				=> ( fsi is DirectoryInfo di )
				? di.Parent!
				: fsi.eToFileInfo ().Directory!;


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string? eGetFileSystemParent ( this string path )
			{
				//Find last dir separator from the end
				string pathRev = path.eReverse ();
				int sepIndex = pathRev.IndexOf (System.IO.Path.PathSeparator);
				if (sepIndex < 1 || sepIndex >= pathRev.Length)
				{
					return null;
				}

				pathRev = pathRev.Substring (sepIndex + 1);
				return pathRev.eReverse ();
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static FileSystemInfo? eFindFirstExistingFileSystemInfo ( this string path )
			{
				//First think this is file
				if (File.Exists (path))
				{
					return new FileInfo (path);
				}

				//Not exist or this is directory
				if (Directory.Exists (path))
				{
					return new DirectoryInfo (path);
				}

				//Not exist!

				string? parentDir = path.eGetFileSystemParent ();
				while (parentDir != null)
				{
					if (Directory.Exists (parentDir))
					{
						return new DirectoryInfo (path);
					}

					parentDir = parentDir!.eGetFileSystemParent ();
				}
				return null;
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string[] eGetFullNames ( this IEnumerable<FileSystemInfo> efsi ) => efsi.Select (fsi => fsi.FullName).ToArray ();



			/// <summary>Checks by Attributes.HasFlag(FileAttributes.Directory)</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eExistAndIsDirectory ( this FileSystemInfo fsi ) => fsi.Exists && fsi.Attributes.HasFlag (FileAttributes.Directory);

			/* 			 
			/// <summary>
			/// File.Exists() will return false if it's not a file even if the directory does exist, 
			/// so if it returns true, we know we got a file, 
			/// if it returns false, we either have a directory or an invalid path 
			/// so next we test if it's a valid directory with Directory.Exists() 
			/// if that returns true, we have a directory if not it's an invalid path.
			/// </summary>
			/// <param name="path"></param>
			/// <returns></returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool ePathIsDirectorySafe(this string path)
			{
				//https://stackoverflow.com/questions/1395205/better-way-to-check-if-a-path-is-a-file-or-a-directory

				if (File.Exists(path))
				{
					// is file
					return false;
				}
				else if (Directory.Exists(path))
				{
					// is Folder 
					return true;
				}
				else
				{
					// invalid path
					return false;
				}
			}
			 */


			/// <summary>Create Backup copy of file. (COPY or MOVE) Return BackUp File</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static FileInfo? eMakeBackUpIfExist ( this FileInfo fi, bool moveInsteadOfCopy = false )
			{
				if (!fi.Exists)
				{
					return null;
				}

				FileInfo fiBak = new ($"{fi.FullName}.{DateTime.Now.eToFileName ()}.bak.");
				fiBak.eDeleteIfExist ();

				if (moveInsteadOfCopy)
				{
					fi.MoveTo (fiBak.FullName);
				}
				else
				{
					fi.CopyTo (fiBak.FullName);
				}

				return fiBak;
			}


			public const string BAK_EXT = ".bak";


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			private static FileInfo eMoveToArhive_Core ( this FileInfo file, string arhiveFilesExt = BAK_EXT )
			{
				FileInfo fiBackup = new ($"{file.FullName}.{DateTime.Now.eToFileName ()}{arhiveFilesExt}");
				fiBackup.eDeleteIfExist ();

				FileInfo fileToMove = new (file.FullName);
				fileToMove.MoveTo (fiBackup.FullName);
				return fiBackup;
			}






			/// <returns>Arhived FileInfo if file backup success or null if source file not exist</returns>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static FileInfo? eMoveToArhive ( this FileInfo source, int maxArhiveFileCount = 10, string arhiveFilesExt = BAK_EXT )
			{
				if (!source.Exists)
				{
					return null;
				}

				FileInfo fiBackup = source.eMoveToArhive_Core (arhiveFilesExt);

				//Clearing Arhive dir
				string arhiveFilePrefix = $"{source.Name}.";
				FileInfo[] arhiveFiles = [..
					source.Directory!.GetFiles()
					.Where(fi => fi.Name.EndsWith(arhiveFilesExt, StringComparison.InvariantCultureIgnoreCase))
					.Where(fi => fi.Name.StartsWith(arhiveFilePrefix, StringComparison.InvariantCultureIgnoreCase))
					.OrderBy(fi => fi.Name, StringComparer.InvariantCultureIgnoreCase)
					];

				int filesToKillCount = arhiveFiles.Length - maxArhiveFileCount;
				if (filesToKillCount > 0)
				{
					FileInfo[] filesToKill = [ .. arhiveFiles.Take (filesToKillCount) ];
					try
					{
						filesToKill.AsParallel ().ForAll (fi => fi.eDeleteIfExistSafe ());
					}
					catch { }
				}

				return fiBackup;
			}


			/// <returns>Arhived FileInfo if file backup success or null if source file not exist</returns>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static FileInfo? eMoveToArhive ( this FileInfo file, DateTime killBefore, string arhiveFilesExt = BAK_EXT )
			{
				if (!file.Exists)
				{
					return null;
				}

				FileInfo fiBackup = file.eMoveToArhive_Core (arhiveFilesExt);

				//Clearing Arhive dir				
				FileInfo[] filesToKill = [..
					file.Directory!.GetFiles()
					.Where(fi => fi.Extension.Equals(arhiveFilesExt, StringComparison.InvariantCultureIgnoreCase))
					.Where(fi => fi.Name.StartsWith($"{file}.", StringComparison.InvariantCultureIgnoreCase))
					.Where(fi => fi.CreationTime <= killBefore)
					.OrderByDescending(fi => fi.Name, StringComparer.InvariantCultureIgnoreCase)
					];

				filesToKill.eForEach (fi => fi.eDeleteIfExistSafe ());

				return fiBackup;
			}

#if !UWP
#endif


			/*
			 * 
			 * 
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eClearArhiveOldFiles(this DirectoryInfo arhive, string arhiveFilesExt = BAK_EXT, int maxArhiveFileCount = 10)
			{
				//Get All arhive files in dir
				FileInfo[] filesInDir = [.. (from fi in arhive.GetFiles($"*.{arhiveFilesExt}")
											 orderby fi.Name ascending
											 select fi)];

				int iFilesToKill = filesInDir.Length - maxArhiveFileCount;
				if (iFilesToKill > 0)
				{
					var lFilesToKill = filesInDir.Take(iFilesToKill).ToList();
					lFilesToKill.ForEach(fi => fi.Delete());
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eClearArhiveOldFiles(this DirectoryInfo arhive, DateTime killBefore, string arhiveFilesExt = BAK_EXT)
			{
				//Get All arhive files in dir
				FileInfo[] filesToKill = [.. (from fi in arhive.GetFiles($"*.{arhiveFilesExt}")
											  where fi.CreationTime <= killBefore
											  select fi)];

				if (filesToKill.Any())
				{
					filesToKill.eForEach(fi => fi.Delete());
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void eClearArhiveOldFiles(this DirectoryInfo arhive, TimeSpan maxAge, string arhiveFilesExt = BAK_EXT)
				=> arhive.eClearArhiveOldFiles(DateTime.Now - maxAge, arhiveFilesExt);


			 */



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eCreateIfNotExist ( this DirectoryInfo di )
			{
				if (!di.Exists)
				{
					di.Create ();
				}
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eDeleteIfExist ( this FileSystemInfo fsi )
			{
				if (fsi.Exists)
				{
					fsi.Delete ();
				}
			}




#if !UWP
#endif
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eDeleteIfExistSafe ( this FileSystemInfo? fsi )
			{
				if (fsi != null)
					( (Action) fsi!.eDeleteIfExist ).eTryCatch ();
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static FileInfo eGetFileIn_SpecialFolder ( this string FileName, Environment.SpecialFolder SF )
				=> new (Path.Combine (Environment.GetFolderPath (SF), FileName));


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static FileInfo eGetFileIn_TempDir ( this string FileName )
				=> new (Path.Combine (Path.GetTempPath (), FileName));





			#region read / write


			#region Read

			#region Create Readers

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static FileStream eCreateStreamR ( this string file,
				FileMode fm = FileMode.Open, FileAccess fa = FileAccess.Read, FileShare fs = FileShare.ReadWrite )
					=> new (file, fm, fa, fs);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static FileStream eCreateStreamR ( this FileInfo fi, FileMode fm = FileMode.Open, FileAccess fa = FileAccess.Read, FileShare fs = FileShare.ReadWrite )
					=> new (fi.FullName, fm, fa, fs);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static StreamReader eCreateReader ( this Stream S, bool detectEncodingFromByteOrderMarks = true )
				=> new (S, detectEncodingFromByteOrderMarks);

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static StreamReader eCreateReader (
				this string file,
				FileMode fm = FileMode.Open,
				FileAccess fa = FileAccess.Read,
				FileShare fs = FileShare.ReadWrite,
				bool detectEncodingFromByteOrderMarks = true )
					=> file.eCreateStreamR (fm, fa, fs).eCreateReader (detectEncodingFromByteOrderMarks);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static StreamReader eCreateReader (
				this FileInfo fi,
				FileMode fm = FileMode.Open,
				FileAccess fa = FileAccess.Read,
				FileShare fs = FileShare.ReadWrite,
				bool detectEncodingFromByteOrderMarks = true )
					=> fi.FullName.eCreateReader (fm, fa, fs, detectEncodingFromByteOrderMarks);


			#endregion



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static (byte[] Buffer, int ReadBytesCount) eRead ( this Stream sm, int iCount, int Offset = 0 )
			{
				byte[] abBuffer = new byte[ iCount ];
				int iRead = sm.Read (abBuffer, Offset, iCount);
				return (abBuffer, iRead);
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static byte[] eReadAllBytes ( this FileInfo fi ) => File.ReadAllBytes (fi.FullName);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static byte[] eReadAllBytes ( this Stream s )
			{
				if (s.Length < 1L) return [];

				s.Seek (0L, SeekOrigin.Begin);
				using BinaryReader br = new (s);
				return br.ReadBytes ((int) s.Length);
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static MemoryStream eReadToMemory ( this FileInfo fi ) => new (fi.eReadAllBytes ());






			#region ReadLine

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static IEnumerable<string> eReadLines ( this TextReader src, bool skipEmptyLines = false )
			{
				string? sLine = src.ReadLine ();
				while (sLine != null)
				{
					if (!string.IsNullOrWhiteSpace (sLine) || !skipEmptyLines)
					{
						yield return sLine;
					}

					sLine = src.ReadLine ();
				}
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string[] eReadLines ( this string src, bool skipEmptyLines = false )
			{
				using var sr = new StringReader (src);
				return [ .. sr.eReadLines (skipEmptyLines) ];
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string[] eReadLines ( this FileInfo fi, Encoding? encoding = null, bool skipEmptyLines = false )
			{
				using FileStream fs = fi.eCreateStreamR ();
				using StreamReader sr = new (fs, encoding ?? Encoding.Unicode);
				return [ .. sr.eReadLines (skipEmptyLines) ];
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static async Task<string[]> eReadLinesAsync ( this FileInfo fi, Encoding? encoding = null, bool skipEmptyLines = false )
				=> await Task.Factory.StartNew (
					() => fi.eReadLines (encoding, skipEmptyLines).ToArray ()
					, TaskCreationOptions.LongRunning
					);

			#endregion




			/// <summary>If rhe file is not exist return null</summary>

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string? eReadAsText ( this FileInfo fi, System.Text.Encoding? @encoding = null, bool detectEncodingFromByteOrderMarks = false )
			{
				if (!fi.Exists)
				{
					return null;
				}

				if (!detectEncodingFromByteOrderMarks || @encoding != null)
				{
					return File.ReadAllText (fi.FullName, @encoding ?? Encoding.Unicode);
				}

				using FileStream fs = fi.eCreateStreamR (FileMode.Open, FileAccess.Read, FileShare.Read);
				using StreamReader sr = new (fs, true);
				return sr.ReadToEnd ();
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static async Task<string?> eReadAsTextAsync ( this FileInfo fi, System.Text.Encoding? @encoding = null, bool detectEncodingFromByteOrderMarks = false )
			{
				if (!fi.Exists)
				{
					return null;
				}

				if (!detectEncodingFromByteOrderMarks || @encoding != null)
				{
					return await Task.Factory.StartNew (() => File.ReadAllText (fi.FullName, @encoding ?? Encoding.Unicode));
				}

				using FileStream fs = fi.eCreateStreamR (FileMode.Open, FileAccess.Read, FileShare.Read);
				using StreamReader sr = new (fs, true);
				return await sr.ReadToEndAsync ();
			}

			/*
		   [MethodImpl(MethodImplOptions.AggressiveInlining)]
		   internal static async Task<string> eReadToEndAsync(this FileInfo f, System.Text.Encoding? enc = null, bool detectEncodingFromByteOrderMarks = false)
		   {
			   using (FileStream fs = f.eCreateStreamR(FileMode.Open, FileAccess.Read, FileShare.Read))
			   {
				   StreamReader sr;
				   if (detectEncodingFromByteOrderMarks)
					   sr = new StreamReader(fs, true);
				   else
				   {
					   enc ??= System.Text.Encoding.Unicode;
					   sr = new StreamReader(fs, enc);
				   }

				   using (StreamReader sr2 = sr)
				   {
					   return await sr2.ReadToEndAsync();
				   }
			   }
		   }
			 */






			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static char[] eReadAllChars ( this StreamReader sr )
			{
				List<char> lBuffer = [];
				char[] cBuffer = new char[ 1024 ];
				int iReadCount;
				do
				{
					iReadCount = sr.Read (cBuffer, 0, cBuffer.Length);
					if (iReadCount > 0)
					{
						ArraySegment<char> dataSlice = new (cBuffer, 0, iReadCount);
						lBuffer.AddRange (dataSlice);
					}
				}
				while (iReadCount > 0);
				var aChars = lBuffer.ToArray ();
				return aChars;
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eReadAllCharsAsString ( this StreamReader SR ) => new (SR.eReadAllChars ());




			#endregion

			#region Write


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void eTruncate ( this Stream fs, Int64 newLen = 0L )
			{
				fs.SetLength (newLen);
				fs.Flush ();
			}




			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eWriteAllBytes ( this FileInfo fi, byte[] data ) => File.WriteAllBytes (fi.FullName, data);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eWriteAllBytes ( this Stream s, byte[] data, bool truncateBeforeWrite = true )
			{
				if (truncateBeforeWrite)
				{
					s.eTruncate ();
				}

				if (data.Any ())
				{
					s.Seek (0L, SeekOrigin.Begin);
					using BinaryWriter bw = new (s, System.Text.Encoding.Default, true);
					bw.Write (data);
				}
			}





			/// <summary>
			/// If file does not exist - creates and writes it.
			/// If file already exist - opens it, truncates to zero and wrires it.
			/// </summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void eWriteAllText ( this FileInfo fi, string text, Encoding? @encoding = null )
			{
				if (!fi.Directory!.Exists)
				{
					fi.Directory.Create ();
				}

				using StreamWriter sw = fi.eCreateWriter (FileMode.OpenOrCreate, encoding: @encoding ?? Encoding.Unicode);
				sw.BaseStream.eTruncate ();
				if (!string.IsNullOrEmpty (text))
				{
					sw.Write (text);
				}

				sw.Flush ();
			}


			#region CreateWriter


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static FileStream eCreateStreamW ( this string file,
				FileMode fm = FileMode.CreateNew, FileAccess fa = FileAccess.Write, FileShare fs = FileShare.ReadWrite )
					=> new (file, fm, fa, fs);

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static FileStream eCreateStreamW ( this FileInfo fi,
				FileMode fm = FileMode.CreateNew, FileAccess fa = FileAccess.Write, FileShare fs = FileShare.ReadWrite )
					=> fi.FullName.eCreateStreamW (fm, fa, fs);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static StreamWriter eCreateWriter ( this FileStream fs,
				Encoding? @encoding = null,
				bool? autoFlush = true )
			{
				@encoding ??= Encoding.Unicode;
				var sw = new StreamWriter (fs, @encoding);
				if (null != autoFlush)
				{
					sw.AutoFlush = autoFlush.eToBool ();
				}

				return sw;
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static StreamWriter eCreateWriter ( this string file,
				FileMode fm = FileMode.CreateNew,
				FileAccess fa = FileAccess.Write,
				FileShare fs = FileShare.ReadWrite,
				Encoding? @encoding = null,
				bool? autoFlush = true )
				=> file.eCreateStreamW (fm, fa, fs)
				.eCreateWriter (@encoding, autoFlush);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static StreamWriter eCreateWriter ( this FileInfo fi,
				FileMode fm = FileMode.CreateNew,
				FileAccess fa = FileAccess.Write,
				FileShare fs = FileShare.ReadWrite,
				Encoding? @encoding = null,
				bool? autoFlush = true )
					=> fi.FullName.eCreateWriter (fm, fa, fs, @encoding, autoFlush);

			#endregion







			#endregion






			#endregion



			/// <summary>Check (TypeOf FSI Is DirectoryInfo)</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eIsDirectoryInfo ( this FileSystemInfo fsi ) => fsi is DirectoryInfo;


			/// <summary>Check (TypeOf FSI Is DirectoryInfo)</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eIsFileInfo ( this FileSystemInfo fsi ) => fsi is FileInfo;


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static FileInfo eToFileInfo ( this FileSystemInfo fsi ) => (FileInfo) fsi;


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static DirectoryInfo? eGetFirstChildDir ( this DirectoryInfo diParent, string childDirName )
				=> diParent.GetDirectories (childDirName).FirstOrDefault ();


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static FileInfo[] eGetFilesSorted ( this DirectoryInfo di, string? searchPattern = null, SearchOption searchOption = SearchOption.TopDirectoryOnly )
				=> di
					.GetFiles (searchPattern ??= "*.*", searchOption)
					.OrderBy (fi => fi.Name)
					.ThenBy (fi => fi.FullName)
					.ToArray ();




			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static async Task eCopyAsync ( this Stream source, Stream target )
			{
				using Task tskCopy = new (() => source.CopyTo (target), TaskCreationOptions.LongRunning);
				tskCopy.Start ();
				await tskCopy;
			}





		}


		/// <summary>Network Extensions</summary>
		internal static partial class Extensions_Network
		{

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eIzSero ( this PhysicalAddress mac )
				=> mac.Equals (new PhysicalAddress ([ 0, 0, 0, 0, 0, 0 ]));


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static Network.IP4AddressWithMask eToIP4AddressWithMask ( this UnicastIPAddressInformation uai )
				=> new (uai.Address, uai.IPv4Mask);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static BitArray eToBitArray ( this IPAddress ip )
			{
				byte[] ipBytes = [ .. ip.GetAddressBytes ().Reverse () ];
				BitArray baTest = new (ipBytes);
				return baTest;
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static IPAddress eToIPAddress ( this BitArray ba )
			{
				byte[] ipBytes = [ 0, 0, 0, 0 ];
				ba.CopyTo (ipBytes, 0);
				IPAddress ip = new (ipBytes.Reverse ().ToArray ());
				return ip;
			}


			//https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
			/// <summary>
			/// Return Human-Readable bytes order (Sample: 192.168.1.1 => [192,168,1,1])
			/// <example>
			/// <code>
			/// !!! Don't use for any arifmetic calculations! Wrong Byte Order! Use only for Saving/Restoring IP.
			/// </code>
			/// For Math calculations with IP use <see cref="eToUInt32CalculableOrder"/> instead!!!
			/// </example>
			/// </summary>
			/// <returns>for 192.168.1.1 => [192,168,1,1]</returns>
			[Obsolete ("!!! Don't use for any arifmetic calculations! Wrong Byte Order! Use only for Saving/Restoring IP")]
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static UInt32 eToUInt32 ( this IPAddress ip )
				=> (uom._Int32) ip.GetAddressBytes ();//return System.BitConverter.ToUInt32(ip.GetAddressBytes());

			////			/// <inheritdoc cref="eToUInt32CalculableOrder" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static UInt32 eToUInt32CalculableOrder ( this IPAddress ip )
				=> (uom._Int32) ip.GetAddressBytes ().Reverse ().ToArray ();

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static IPAddress eFromIPCalculableOrderToIP4Address ( this UInt32 ip )
				=> new (ip.eReverseBytes ());

			/// <summary>Returns string like '192.168.0.1/24'</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eFormatCIDR ( this IPAddress ip, uint prefixLen, string separator = "/" )
				=> $"{ip}{separator}{prefixLen}";


			/// <summary>Returns (0-32) part like 192.196.5.0/[24], calculated by specifed Mask like 255.255.255.0</summary>
			/// <param name="ipMask">Mask like '255.255.255.0'</param>			
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static uint eGetIP4SubnetPrefixSizeFromMask ( this IPAddress ipMask )
			{
#if NET7_0_OR_GREATER
				uint uMask = ipMask.eToUInt32CalculableOrder ();
				//var sss = uMask.eToBitArray().eToBitsString();
				uint tlz = (uint) BitOperations.TrailingZeroCount (uMask);
				return 32u - tlz;
#else
				BitArray bits = ipMask.eToBitArray();
				return (uint)(bits.Cast<bool>().Reverse().TakeWhile(b => b).Count());
#endif
			}


			/// <summary>Generates Subnet Mask like '255.255.255.0' from specifed maskPrefix (192.168.1.0/[24])</summary>
			/// <param name="maskPrefix">MaskPrefixSize (0-32) like '24' from 192.168.1.0/[24]</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static IPAddress eGetIP4SubnetMask ( this uint maskPrefix )
			{
				BitArray bits = new (32, true);
				if (maskPrefix < 32)
				{
					bits.eSetBitsFromStart (0, (int) ( 32u - maskPrefix ), false);
				}
				//var BitsString = bits.eToBitsString();
				IPAddress m = bits.eToIPAddress ();
				return m;
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static IEnumerable<IPAddress> eGetAllIP4List ( this IPAddress ipStart, IPAddress ipEnd )
			{
				uint @start = ipStart.eToUInt32CalculableOrder ();
				uint @end = ipEnd.eToUInt32CalculableOrder ();
				for (uint ip = @start ; ip <= @end ; ip++)
				{
					IPAddress ipa = new (ip.eReverseBytes ());
					yield return ipa;
				}
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static (
				IPAddress SubnetZeroIP,
				IPAddress FirstIP,
				IPAddress LastIP,
				IPAddress BroadcastIP,
				uint HostCount,
				IEnumerable<IPAddress>? SubnetHosts
				) eCalculateIP4Subnet ( this IPAddress ipa, IPAddress subnetMask )
			{
				IPAddress ipaFirst = ipa, ipaLast = ipa, ipaBroadcast = ipa;
				uint uSubnetIPCount = 1u;

				uint uMask = subnetMask.eToUInt32CalculableOrder ();
				//if (uMask == 0xFF_FF_FF_FF) return (ipaFirst, ipaLast, ipaBroadcast, uSubnetIPCount);
				uint uIP = ipa.eToUInt32CalculableOrder ();
				uint uSubnetZeroIP = uIP & uMask;
				IPAddress ipaSubnetZeroIP = uSubnetZeroIP.eFromIPCalculableOrderToIP4Address ();
				if (subnetMask.Equals (IPAddress.Broadcast))
				{
					return (ipaSubnetZeroIP, ipaFirst, ipaLast, ipaBroadcast, uSubnetIPCount, null);
				}

				{
					uint uMaskNot = ~uMask;
					uint uBroadcastIP = uIP | uMaskNot;
					ipaBroadcast = uBroadcastIP.eFromIPCalculableOrderToIP4Address ();
				}

				uint uFirstIP = uSubnetZeroIP | 0x00_00_00_01;
				ipaFirst = uFirstIP.eFromIPCalculableOrderToIP4Address ();
				ipaLast = ipaFirst;

				uint uPrefixLen = subnetMask.eGetIP4SubnetPrefixSizeFromMask ();
				if (uPrefixLen >= 31)
				{
					return (ipaSubnetZeroIP, ipaFirst, ipaLast, ipaBroadcast, uSubnetIPCount, ipaFirst.eToArrayOf ());
				}

				uint uChangeableBits = 32u - uPrefixLen;
				uSubnetIPCount = 0;
				for (uint i = 0 ; i < uChangeableBits ; i++)
				{
					uSubnetIPCount <<= 1;
					uSubnetIPCount++;
				}
				uSubnetIPCount--;//Now uSubnetIPCount includes BROADCAST_IP, therefore decreasing it to 1 = last valid IP

				uint uLastIP = uSubnetIPCount | uSubnetZeroIP;
				ipaLast = uLastIP.eFromIPCalculableOrderToIP4Address ();
				var ipList = ipaFirst.eGetAllIP4List (ipaLast);

				return (ipaSubnetZeroIP, ipaFirst, ipaLast, ipaBroadcast, uSubnetIPCount, ipList);
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static (
				IPAddress SubnetZeroIP,
				IPAddress FirstIP,
				IPAddress LastIP,
				IPAddress BroadcastIP,
				uint HostCount,
				IEnumerable<IPAddress>? SubnetHosts
				) eCalculateIP4Subnet ( this Network.IP4AddressWithMask IP4 )
				=> IP4.Address.eCalculateIP4Subnet (IP4.Mask);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static (
				IPAddress SubnetZeroIP,
				IPAddress FirstIP,
				IPAddress LastIP,
				IPAddress BroadcastIP,
				uint HostCount,
				IEnumerable<IPAddress>? SubnetHosts)
				eCalculateIP4Subnet ( this IPAddress ipa, uint subnetPrefix )
				=> ipa.eCalculateIP4Subnet (subnetPrefix.eGetIP4SubnetMask ());


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eIsInSubnet ( this uom.Network.IP4AddressWithMask host, uom.Network.IP4AddressWithMask targetSubnet )
			{
				var localSubnet = host.eCalculateIP4Subnet ();
				var remoteSubnet = targetSubnet.eCalculateIP4Subnet ();

				if (localSubnet.BroadcastIP.Equals (remoteSubnet.BroadcastIP))
				{
					return true;
				}

				return false;
			}


			/// <summary>Checks if host is in subnet of any local network adapter</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eIsInLocalSubnet ( this Network.IP4AddressWithMask ip )
			{
				foreach (UnicastIPAddressInformation uaLocal in uom.Network.Helpers.GelLocalIP4 ())
				{
					uom.Network.IP4AddressWithMask subnetLocal = uaLocal.eToIP4AddressWithMask ();
					if (ip.eIsInSubnet (subnetLocal))
					{
						return true;
					}
				}
				return false;
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eIsInLocalSubnet ( this IPAddress ip, uint ipPrefixLen )
				=> new uom.Network.IP4AddressWithMask (ip, ipPrefixLen).eIsInLocalSubnet ();


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eIsInSubnet ( this IPAddress ip, uom.Network.IP4AddressWithMask subnet )
			{
				var ipaZerro = subnet.eCalculateIP4Subnet ().SubnetZeroIP;

				uint uZerro = ipaZerro.eToUInt32 ();
				uint uIP = ip.eToUInt32 ();

				bool inSubNet = ( uZerro & uIP ) == uZerro;
				return inSubNet;
			}



			public static async void eDownloadFile ( this string FileUrl,
				Action<System.IO.Stream> cbNetworkStreamAction,
				int iTimeout = 30000 )
			{
#if NET40
        WebClient _client = new WebClient();
              HttpWebRequest? webRequest = WebRequest.Create(FileUrl) as HttpWebRequest;

 HttpWebRequest? webRequest = WebRequest.Create(FileUrl) as HttpWebRequest;
            webRequest!.AllowWriteStreamBuffering = true;
            webRequest.Timeout = iTimeout;
            webRequest.ServicePoint.ConnectionLeaseTimeout = 5000;
            webRequest.ServicePoint.MaxIdleTime = 5000;

            try
            {
                using (WebResponse webResponse = webRequest!.GetResponse())
                {
                    var tskGetStream = new Task<System.IO.Stream>(() => webResponse.GetResponseStream());

                    //    .GetAwaiter().GetResult();




                    using (System.IO.Stream stream = webResponse.GetResponseStream())
                    {
                        cbNetworkStreamAction.Invoke(stream);
                    }
                }
            }
            finally
            {
                webRequest.ServicePoint.CloseConnectionGroup(webRequest.ConnectionGroupName!);
                webRequest = null;
            }
#else
				using var _client = new HttpClient () { Timeout = TimeSpan.FromMilliseconds (iTimeout) };
				using var stream = await _client.GetStreamAsync (FileUrl);
				cbNetworkStreamAction.Invoke (stream);
#endif
			}

			/// <summary>Download File From Remote Using 'HttpWebRequest'</summary>
			/// <param name="Target">Local File Path To Save File</param>        
			public static System.IO.FileInfo? eDownloadFile ( this string FileUrl, System.IO.FileInfo? Target = null )
			{
				FileInfo? fiDownloaded = null;
				FileUrl.eDownloadFile (stDownload =>
				{
					FileInfo fiTMP = Target ?? new FileInfo (System.IO.Path.GetTempFileName ());
					try
					{
						using FileStream fsTMP = fiTMP.OpenWrite ();
						stDownload.CopyTo (fsTMP);
						fsTMP.Flush ();
						fiDownloaded = fiTMP;
					}
					finally
					{
						if (null == fiDownloaded)
						{
							fiTMP.Delete ();
						}
					}
				});
				return fiDownloaded;
			}


			/// <summary>Download File From Remote Using 'WebClient'</summary>
			/// <param name="Target">Local File Path To Save File</param>        
			public static (System.IO.FileInfo? DownloadedFile, AsyncCompletedEventArgs? AsyncDownloadResult)
				eDownloadFile (
				this string FileUrl,
				System.IO.FileInfo? Target = null,
				Action<DownloadProgressChangedEventArgs>? downloadProgress = null )
			{
				using ManualResetEvent evtFinished = new (false);

				//https://newbedev.com/progress-bar-with-httpclient
				//using (var client = new HttpClient())
				//{
				//    client.Timeout = TimeSpan.FromMinutes(5);

				//    // Create a file stream to store the downloaded data.
				//    // This really can be any type of writeable stream.
				//    using (var file = new FileStream("sdf", FileMode.Create, FileAccess.Write, FileShare.None))
				//    {

				//        // Use the custom extension method below to download the data.
				//        // The passed progress-instance will receive the download status updates.
				//        await client.DownloadAsync(FileUrl, file, progress, cancellationToken);
				//    }
				//}


#pragma warning disable SYSLIB0014 // Type or member is obsolete
				using WebClient web = new ();
#pragma warning restore SYSLIB0014 // Type or member is obsolete
				try
				{
					FileInfo fiBuffer = Target ?? new FileInfo (System.IO.Path.GetTempFileName ());
					web.DownloadProgressChanged += ( _, e ) => downloadProgress?.Invoke (e);

					AsyncCompletedEventArgs? acea = null;
					web.DownloadFileCompleted += ( _, e ) =>
					{
						acea = e;
						evtFinished.Set ();
					};

					web.DownloadFileAsync (new Uri (FileUrl), fiBuffer.FullName);
					evtFinished.WaitOne ();
					return (DownloadedFile: fiBuffer, AsyncDownloadResult: acea);
				}
				finally { evtFinished.Set (); }
			}



#if !ANDROID


			/// <summary>Download File From Remote Using 'WebClient' and displaying progress bar</summary>
			/// <param name="Target">Local File Path To Save File</param>        
			public static (System.IO.FileInfo? DownloadedFile, AsyncCompletedEventArgs? AsyncDownloadResult)
				eDownloadFileConsole ( this string FileUrl,
				System.IO.FileInfo? Target = null,
				int ProgressBarLenght = 30,
				char cProgressBarFillChar = '#',
				char cProgressBarEmptyChar = '-',
				string ProgressPrefixString = "Downloading:" )
			{

				try { Console.CursorVisible = false; } catch { }//Just Ignore Error bc not all platforms support Show/Hide cursor.
				try
				{
					( 0 ).eWriteConsoleProgress (ProgressBarLenght, cProgressBarFillChar, cProgressBarEmptyChar, ProgressPrefixString);//Display Zero Progress
					var dlResult = FileUrl.eDownloadFile (
						Target,
						e => e.ProgressPercentage.eWriteConsoleProgress (ProgressBarLenght, cProgressBarFillChar, cProgressBarEmptyChar, ProgressPrefixString));

					return (dlResult.DownloadedFile, dlResult.AsyncDownloadResult);
				}
				finally
				{
					Console.WriteLine ();
					try { Console.CursorVisible = true; } catch { }//Just Ignore Error bc not all platforms support Show/Hide cursor.
				}
			}

#endif




		}


		internal static partial class Extensions_Object
		{
			/// <summary>Compares Classes via ReferenceEquals, and unmanaged via Equals</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool eEqualsUniversal<T> ( this T? A, T? B )
			{
				if (A == null && B == null)
				{
					return true;
				}

				if (( A == null && B != null ) || ( B == null && A != null ))
				{
					return false;
				}

				if (A!.GetType ().IsClass)
				{
					return Object.ReferenceEquals (A, B);
				}

				if (A is IEquatable<T> ea)
				{
					return ea.Equals (B!);
				}

				if (A is IComparable<T> ca)
				{
					return ca.CompareTo (B!) == 0;
				}

				return A.Equals (B);
			}



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void ThrowIfNull<T> ( this T? obj, [CallerArgumentExpression ("obj")] string argName = "" )
			{
				_ = obj ?? throw new ArgumentNullException (argName);
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eDisposeAndSetNothing<T> ( this T ObjToDispose, bool ThrowExceptionOnError = false ) where T : IDisposable
			{
				try { ObjToDispose?.Dispose (); }
				catch
				{
					if (ThrowExceptionOnError)
					{
						throw;
					}
				}
			}




			/// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eCompareToNulls<T> ( this T? x, T? y, out int compareResult )
			{
				if (x is null && y is null)
				{
					compareResult = 0;
					return true;
				}
				else if (x is null)
				{
					compareResult = -1;
					return true;
				}
				else if (y is null)
				{
					compareResult = 1;
					return true;
				}
				compareResult = 0;
				return false;
			}




			//[MethodImpl(MethodImplOptions.AggressiveInlining)]
			//internal static void  eDisposeAll([In()] this IEnumerable<IDisposable> T, bool ThrowExceptionOnError = false)
			//{
			//    if (T !=null)
			//    {
			//        foreach (var ObjRef in T)
			//        {
			//            if (ObjRef !=null)
			//            {
			//                try
			//                {
			//                    ObjRef.Dispose();
			//                }
			//                catch
			//                {
			//                    if (ThrowExceptionOnError)
			//                        throw;
			//                }
			//            }
			//        }
			//    }

			//    T = null;
			//}

			///// <summary>Clean up the COM variables</summary>
			//[MethodImpl(MethodImplOptions.AggressiveInlining)]
			//internal static void  eDisposeAndSetNothingCOMObject([MarshalAs(UnmanagedType.IUnknown)][In] this out object rCOMObject, bool ThrowExceptionOnError = false)
			//{
			//    try
			//    {
			//        while (Marshal.ReleaseComObject(rCOMObject) != 0)
			//            Application.DoEvents(); // Wait
			//        rCOMObject = null;
			//    }
			//    catch
			//    {
			//        if (ThrowExceptionOnError)
			//            throw;
			//    }
			//}

		}


		internal static partial class Extensions_Async_MT
		{


			/// <summary>
			/// Awaiting specifed total 'timeToWait' with delays by 'stepInterval' and checking periodicaly cancelationFunc
			/// </summary>
			public static async Task eDelayWithCancelation ( this int timeToWait, int stepInterval, Func<bool> cancelationFunc )
			{
				if (stepInterval >= timeToWait)
				{
					throw new ArgumentOutOfRangeException (nameof (stepInterval), "stepInterval must be < than timeToWait");
				}

				int timeSpent = 0;
				while (true)
				{
					if (cancelationFunc.Invoke ())
					{
						return;
					}

					await Task.Delay (stepInterval);
					timeSpent += stepInterval;
					if (timeSpent >= timeToWait)
					{
						break;
					}
				}
			}





			/// <summary>Run Task synchronously, using SynchronizationContext helpers</summary>
			/// <param name="func">Task<T> method to run</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void eRunSync ( this Func<Task> func )
			{
				SynchronizationContext? oldContext = SynchronizationContext.Current;
				ExclusiveSynchronizationContext synCtx = new ();
				SynchronizationContext.SetSynchronizationContext (synCtx);
				synCtx.Post (async _ =>
				{
					try
					{
						await func ();
					}
					catch (Exception e)
					{
						synCtx.InnerException = e;
						throw;
					}
					finally
					{ synCtx.EndMessageLoop (); }
				}, null);
				synCtx.BeginMessageLoop ();
				SynchronizationContext.SetSynchronizationContext (oldContext);
			}


			/// <inheritdoc cref="eRunSync(Func[Task])"/>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static T? eRunSync<T> ( this Func<Task<T?>> func )
			{
				var oldContext = SynchronizationContext.Current;
				var synch = new ExclusiveSynchronizationContext ();
				SynchronizationContext.SetSynchronizationContext (synch);
				T? ret = default;
				synch.Post (async _ =>
				{
					try
					{
						ret = await func.Invoke ();
					}
					catch (Exception e)
					{
						synch.InnerException = e;
						throw;
					}
					finally
					{
						synch.EndMessageLoop ();
					}
				}, null);
				synch.BeginMessageLoop ();
				SynchronizationContext.SetSynchronizationContext (oldContext);
				return ret;
			}




			/// <inheritdoc cref="eRunSync"/>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void eRunSync ( this Task tsk )
				=> eRunSync (() => tsk);


			/// <inheritdoc cref="eRunSync"/>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static T eRunSync<T> ( this Task<T> task )
				=> eRunSync (() => task!)!;





			/// <summary>Run Task synchronously using Task.Run helpers</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void eRunSync2 ( this Task t1 )
			{
				using Task t2 = Task.Run (async () => await t1);
				t2.Wait ();
			}

			/// <inheritdoc cref="eRunSync2"/>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static T eRunSync2<T> ( this Task<T> t1 )
			{
				using Task t2 = Task.Run (async () => await t1);
				t2.Wait ();
				return t1.Result;
			}



			private class ExclusiveSynchronizationContext : SynchronizationContext
			{
				private bool done;
				public Exception? InnerException { get; set; }

				private readonly AutoResetEvent workItemsWaiting = new (false);
				private readonly Queue<Tuple<SendOrPostCallback, object?>> items = new ();

				public override void Send ( SendOrPostCallback d, object? state )
				{
					throw new NotSupportedException ("We cannot send to our same thread");
				}

				public override void Post ( SendOrPostCallback d, object? state )
				{
					lock (items)
					{
						items.Enqueue (Tuple.Create (d, state));
					}
					workItemsWaiting.Set ();
				}

				public void EndMessageLoop () => Post (_ => done = true, null);

				public void BeginMessageLoop ()
				{
					while (!done)
					{
						Tuple<SendOrPostCallback, object?>? task = null;
						lock (items)
						{
							if (items.Count > 0)
							{
								task = items.Dequeue ();
							}
						}
						if (task != null)
						{
							task.Item1 (task.Item2);
							if (InnerException != null) // the method threw an exeption
							{
								//throw new AggregateException("AsyncHelpers.Run method threw an exception.", InnerException);
								throw InnerException;
							}
						}
						else
						{
							workItemsWaiting.WaitOne ();
						}
					}
				}

				public override SynchronizationContext CreateCopy ()
				{
					return this;
				}
			}









			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static Task eAsTask ( this WaitHandle handle, int timeout = Timeout.Infinite )
			{
				TaskCompletionSource<object> tcs = new ();
				RegisteredWaitHandle registration = ThreadPool.RegisterWaitForSingleObject (handle,
					( state, timedOut ) =>
					{
						//if (state == null) throw new ArgumentNullException(nameof(state));
						var localTcs = (TaskCompletionSource<object>) state!;

						if (timedOut)
						{
							localTcs.TrySetCanceled ();
						}
						else
						{
#pragma warning disable CS8600, CS8625 // Possible null reference argument for parameter.
							localTcs.TrySetResult (null);
						}
#pragma warning restore CS8600, CS8625 // Possible null reference argument for parameter.

					},
				tcs, timeout, executeOnlyOnce: true);

				tcs.Task.ContinueWith (( _, state ) => ( (RegisteredWaitHandle) ( state! ) ).Unregister (null), registration, TaskScheduler.Default);
				return tcs.Task;
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static Task eWaitAsync ( this WaitHandle handle, int timeout = Timeout.Infinite ) => handle.eAsTask (timeout);












			public static void eInvoke ( this SynchronizationContext sc, Action a )
				=> sc.Send (_ => a.Invoke (), null);


			public static Task eWhenCanceled ( this CancellationToken cancellationToken )
			{
				var tcs = new TaskCompletionSource<bool> ();
				cancellationToken.Register (s => ( (TaskCompletionSource<bool>) s! ).SetResult (true), tcs);
				return tcs.Task;
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static async Task eWaitAsync ( this Task t, CancellationTokenSource cancel )
			{
#if NET
				await t.WaitAsync (cancel.Token);
#else
				//await Task.WhenAny(t, cancel.Token.WaitHandle.eAsTask());
				await Task.WhenAny(t, cancel.Token.eWhenCanceled());
#endif
			}




			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static async Task eStartAndWaitAsync ( this Action a )
				=> await Task.Run (a);



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static async Task eStartAndWaitAsync ( this Task t )
			{
				t.Start ();
				await t;
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static async Task<T> eStartAndWaitAsync<T> ( this Task<T> tsk )
			{
				tsk.Start ();
				return await tsk;
			}



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static async Task eStartAndWaitLongAsync ( this Action a )
			{
				using Task tsk = new (a.Invoke, TaskCreationOptions.LongRunning);
				await tsk.eStartAndWaitAsync ();
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static async Task<T> eStartAndWaitLongAsync<T> ( this Func<T> a )
			{
				using Task<T> tsk = new (a.Invoke, TaskCreationOptions.LongRunning);
				return await tsk.eStartAndWaitAsync ();
			}


			/// <summary>Exec FUNC. Return result</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T eRunSyncLock<T> ( this object rLockObject, Func<T> f )
			{
				lock (rLockObject)
				{
					return f.Invoke ();
				}
			}



			public static void eRunAssert_0 (
				this Func<uint> operation,
				string messageTemplate,
				params object[] messageArgs )
			{
				uint result = operation.Invoke ();
				if (result != 0)
				{
					string message = string.Format (messageTemplate, messageArgs);
					throw new Exception ($"{message}. Error code {result} (see WinError.h)");
				}
			}

			public static void eRunAssert_true (
				this Func<bool> operation,
				string messageTemplate,
				params object[] messageArgs )
			{
				bool result = operation.Invoke ();
				if (!result)
				{
					string message = string.Format (messageTemplate, messageArgs);
					throw new Exception ($"{message}. Error code {result} (see WinError.h)");
				}
			}








			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static async Task eWhenAll<T> ( this IEnumerable<Task<T?>> aTasks, bool startTasks, Action<T?> onEachTaskCompleted )
			{
				List<Task<T?>> lTasks = aTasks.ToList ();
				if (startTasks)
				{
					lTasks.ForEach (TSK => TSK.Start ()); // Start All Tasks
				}

				while (lTasks.Any ())
				{
					Task<T?> finishedTask = await Task.WhenAny (lTasks);
					lTasks.Remove (finishedTask); // Remove this task from list
					var rslt = finishedTask.Result;
					onEachTaskCompleted?.Invoke (rslt);
				}
			}


			/// <summary>Check event Flag</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eIsSet ( this EventWaitHandle Evt )
				=> ( null != Evt ) && Evt.WaitOne (0, false);


		}


		internal static partial class Extensions_DebugAndErrors
		{


			#region eTryCatch

			internal class ExceptionEventArgs ( Exception ex, string callerName, string callerFile, int callerLine ) : System.EventArgs ()
			{
				public readonly Exception Exception = ex;
				public readonly string CallerMemberName = callerName;
				public readonly string CallerFilePath = callerFile;
				public readonly int CallerLineNumber = callerLine;
			}


			/// <summary>Try execute action</summary>
			/// <param name="onError">error handler</param>
			/// <returns>Exception if error occurs</returns>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static Exception? eTryCatch (
				this Action a,
				Action<ExceptionEventArgs>? onError = null,
				[CallerMemberName] string callerName = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0 )
			{
				try
				{
					a.Invoke ();
					return null;
				}
				catch (Exception ex)
				{
					if (onError != null)
						onError?.Invoke (new ExceptionEventArgs (ex, callerName, callerFile, callerLine));
					else
						ex.Message.eDebugWriteLine ();

					return ex;
				}
			}


			/// <inheritdoc cref="eTryCatch" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static Exception? eTryCatch (
				this System.Windows.Forms.MethodInvoker mi,
				Action<ExceptionEventArgs>? onError = null,
				[CallerMemberName] string callerName = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0 )
			{
				try
				{
					mi.Invoke ();
					return null;
				}
				catch (Exception ex)
				{
					if (onError != null)
						onError?.Invoke (new ExceptionEventArgs (ex, callerName, callerFile, callerLine));
					else
						ex.Message.eDebugWriteLine ();

					return ex;
				}
			}


			/// <inheritdoc cref="eTryCatch" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static Exception? eTryCatch<T> (
				this Func<T?> func,
				out T? funcResult,
				Action<ExceptionEventArgs>? onError = null,
				[CallerMemberName] string callerName = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0 )
			{
				try
				{
					funcResult = func.Invoke ();
					return null;
				}
				catch (Exception ex)
				{
					funcResult = default;

					if (onError != null)
						onError?.Invoke (new ExceptionEventArgs (ex, callerName, callerFile, callerLine));
					else
						ex.Message.eDebugWriteLine ();

					return ex;
				}
			}


			#region eTryCatchUIAsync


			/// <inheritdoc cref="eTryCatch" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static async Task<(bool Result, Exception? Error)> eTryCatchAsync (
				this Task tsk,
				Action<ExceptionEventArgs>? onError = null,
				[CallerMemberName] string callerName = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0 )
			{
				try
				{
					await tsk;
					return (true, null);
				}
				catch (Exception ex)
				{
					switch (ex)
					{
						case OperationCanceledException: break;

						default:
							{
								if (onError != null)
									onError?.Invoke (new ExceptionEventArgs (ex, callerName, callerFile, callerLine));
								else
									ex.Message.eDebugWriteLine ();

								break;
							}
					}
					return (false, ex);
				}
			}


			/// <inheritdoc cref="eTryCatch" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static async Task<(T? Result, Exception? Error)> eTryCatchAsync<T> (
				this Task<T> tsk,
				T? defaultValue = default,
				Action<ExceptionEventArgs>? onError = null,
				[CallerMemberName] string callerName = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0 )
			{
				try { return (await tsk, null); }
				catch (Exception ex)
				{
					switch (ex)
					{
						case OperationCanceledException: break;

						default:
							{
								if (onError != null)
									onError?.Invoke (new ExceptionEventArgs (ex, callerName, callerFile, callerLine));
								else
									ex.Message.eDebugWriteLine ();

								break;
							}
					}
					return (defaultValue, ex);
				}
			}


			/// <inheritdoc cref="eTryCatch" />
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static async Task<(T? Result, Exception? Error)> eTryCatchAsync<T> (
				this Func<T> func,
				T? defaultValue = default,
				CancellationTokenSource? cancel = null,
				Action<ExceptionEventArgs>? onError = null,
				[CallerMemberName] string callerName = "", [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = 0 )
			{
				try
				{
					if (cancel != null)
					{
						return (await Task.Factory.StartNew (func, cancel.Token), null);
					}
					else
					{
						return (await Task.Factory.StartNew (func), null);
					}
				}
				catch (Exception ex)
				{
					switch (ex)
					{
						case OperationCanceledException: break;

						default:
							{
								//			ex.eLogError(onErrorShowUI, supressAnyModalPopEvenInDEBUG: true, callerName: callerName, callerFile: callerFile, callerLine: callerLine);
								if (cancel == null || !cancel.IsCancellationRequested)
								{
									if (onError != null)
										onError?.Invoke (new ExceptionEventArgs (ex, callerName, callerFile, callerLine));
									else
										ex.Message.eDebugWriteLine ();

								}
								break;
							}
					}

					return (defaultValue, ex);
				}
			}

			#endregion




			#endregion



			[Conditional ("DEBUG")]
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void eDebugWriteLine ( this string sMessage )
			{
#if DEBUG
				Debug.WriteLine (sMessage);
#endif
			}


			[Conditional ("DEBUG")]
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void eDebugWrite ( this string sMessage )
			{
#if DEBUG
				Debug.Write (sMessage);
#endif
			}






			#region eFullDump


			// ''<summary>Фиксация ошибки в журнале и в DEBUG MODE вывод сообщения</summary>
			//[MethodImpl(MethodImplOptions.AggressiveInlining)]			internal static string eDumpTree(this Exception ex, [CallerMemberName] string caller = "")				=> ex.eDumpExceptionTree(caller) + CS_CONSOLE_SEPARATOR;

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eFullDump ( this Exception? ex,
				[CallerMemberName] string callerName = "",
				[CallerFilePath] string callerFile = "",
				[CallerLineNumber] int callerLine = 0 )
			{
				if (ex == null)
				{
					return string.Empty;
				}

				StringBuilder sbExceptionTree = new ();
				using StringWriter sw = new (sbExceptionTree);

				sw.WriteLine ($"{ex.GetType ()}: '{ex.Message}'");
				sw.WriteLine ($"Caller: '{callerName}', File: '{callerFile}', Line: {callerLine}");
				sw.WriteLine ($"StackTrace:\n{ex.StackTrace}");

				if (ex.InnerException != null)
				{
					sw.WriteLine ($"Exception Stack Tree:");
					while (ex.InnerException != null)
					{
						ex = ex.InnerException;
						sw.WriteLine ($"{ex.GetType ()}\n{ex.Message}");
					}
				}
				return sbExceptionTree.ToString ();
			}


			#endregion







			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eLogError_CONSOLE ( this Exception ex,
				bool fullDump = false,
				[CallerMemberName] string callerName = "",
				[CallerFilePath] string callerFile = "",
				[CallerLineNumber] int callerLine = 0 )
			{
				const string C_CONSOLE_ERROR_HEADER = "*** ERROR:\n";
				string msg = C_CONSOLE_ERROR_HEADER + ex.Message;

				if (fullDump)
				{
					msg = C_CONSOLE_ERROR_HEADER + ex.eFullDump (callerName, callerFile, callerLine);
				}
				Console.WriteLine ($"{CS_CONSOLE_SEPARATOR}/n{msg}/n{CS_CONSOLE_SEPARATOR}");
			}




		}


		internal static partial class Extensions_Handle
		{



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eIsValid ( this IntPtr h ) => ( ( h != IntPtr.Zero ) && ( h != uom.constants.HANDLE_INVALID ) );


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eIsNotValid ( this IntPtr h ) => !h.eIsValid ();


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eIsValid ( this SafeHandle? sh ) => ( sh != null ) && ( !sh!.IsInvalid );


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static bool eIsValid ( this HandleRef HR ) => HR.Handle.eIsValid ();




			// <MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
			// Friend Function IsValid(ByVal Handle As Global.Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid) As Boolean
			// SafeHandleZeroOrMinusOneIsInvalid
			// Return Not Handle.IsInvalid
			// End Function

			// <MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
			// Friend Function IsValid(ByVal hFile As SafeFileHandle) As Boolean
			// Dim B = (Not hFile.IsInvalid) AndAlso (Not hFile.IsClosed)
			// Return B
			// End Function


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void RunWithGCPinned ( this object obj, Action<IntPtr> a, GCHandleType t = GCHandleType.Pinned )
			{
				var gch = GCHandle.Alloc (obj, t);
				try
				{
					IntPtr ptr = gch.AddrOfPinnedObject ();
					a.Invoke (ptr);
				}
				finally
				{
					gch.Free ();
				}
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T? RunWithGCPinned<T> ( this object obj, Func<IntPtr, T?> a, GCHandleType t = GCHandleType.Pinned )
			{
				var gch = GCHandle.Alloc (obj, t);
				try
				{
					IntPtr ptr = gch.AddrOfPinnedObject ();
					return a.Invoke (ptr);
				}
				finally
				{
					gch.Free ();
				}
			}



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void RunWithGCPinned ( this IEnumerable<object> objList, Action<IReadOnlyList<IntPtr>> a, GCHandleType t = GCHandleType.Pinned )
			{
				var gchList = objList
					.Select (obj => GCHandle.Alloc (obj, t))
					.ToList ();

				try
				{
					var ptrList = gchList.Select (gch => gch.AddrOfPinnedObject ()).ToList ();
					a.Invoke (ptrList);
				}
				finally
				{
					foreach (var gch in gchList)
						gch.Free ();

					gchList.Clear ();
				}
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T? RunWithGCPinned<T> ( this IEnumerable<object> objList, Func<IReadOnlyList<IntPtr>, T?> a, GCHandleType t = GCHandleType.Pinned )
			{
				var gchList = objList
					.Select (obj => GCHandle.Alloc (obj, t))
					.ToList ();

				try
				{
					var ptrList = gchList.Select (gch => gch.AddrOfPinnedObject ()).ToList ();
					return a.Invoke (ptrList);
				}
				finally
				{
					foreach (var gch in gchList)
						gch.Free ();

					gchList.Clear ();
				}

			}

		}


		internal static partial class Extensions_Globalization
		{


			/// <summary>Возвращает корневой элемент для дерева языков.
			/// для Ru-Ru будет RU,
			/// для EN-US будет EN</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static CultureInfo eGetTopParent ( this CultureInfo Cult )
			{
				while (Cult.Parent != null && Cult.Parent.Name.IsNotNullOrWhiteSpace ())
				{
					Cult = Cult.Parent;
				}

				return Cult;
			}

			/// <summary>Это российское дерево языков. 
			/// Любой язык, где корневой элемент = RU</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool eIsRussianTree ( this CultureInfo Cult ) => Cult.eGetTopParent ().LCID == 25;

		}


		internal static partial class Extensions_Resources
		{


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string[] eGetManifestResourceNames ( this Assembly asm, Func<string, bool> predicate )
			{
				var aaa = asm.GetManifestResourceNames ()
				.Where (resFileName => predicate.Invoke (resFileName))
				.ToArray ();

				return aaa;
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eGetManifestResourceName ( this Assembly asm, string nameSuffix )
				=> asm
					.eGetManifestResourceNames (str => str.EndsWith (nameSuffix))
					.Single () ?? throw new ArgumentOutOfRangeException (nameof (nameSuffix));


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static Stream eGetManifestResourceStream ( this Assembly asm, string nameSuffix )
				=> asm.GetManifestResourceStream (asm.eGetManifestResourceName (nameSuffix))!;



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static (Stream @Stream, StreamReader Reader) eGetManifestResourceStreamReader ( this Assembly asm, string nameSuffix, Encoding? e = null, bool detectEncodingFromByteOrderMarks = false )
			{
				e ??= Encoding.Unicode;

				Stream stream = asm.eGetManifestResourceStream (nameSuffix)!;
				StreamReader sr = detectEncodingFromByteOrderMarks
					? new (stream, true)
					: new (stream, e, false);

				return (stream, sr);
			}



			/// <summary>Searches specifed assembly for file and load it
			/// Sample:
			/// txtTextBox.Text = uom.AppInfo.Assembly.eReadResourceFileAsString("EULA.txt");
			/// </summary>
			/// <param name="asm">assembly that contains file resource</param>
			/// <param name="nameSuffix">Resource File Name</param>
			/// <param name="e">Resource File Encoding</param>
			/// <returns></returns>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eReadResourceFileAsString ( this Assembly asm, string nameSuffix, Encoding? e = null, bool detectEncodingFromByteOrderMarks = false )
			{
				var r = asm.eGetManifestResourceStreamReader (nameSuffix, e, detectEncodingFromByteOrderMarks);
				using Stream s = r.Stream;
				using StreamReader sr = r.Reader;
				return sr.ReadToEnd ();
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static async Task<string> eReadResourceFileAsStringAsync ( this Assembly asm, string nameSuffix, Encoding? e = null, bool detectEncodingFromByteOrderMarks = false )
			{
				var r = asm.eGetManifestResourceStreamReader (nameSuffix, e, detectEncodingFromByteOrderMarks);
				using Stream s = r.Stream;
				using StreamReader sr = r.Reader;
				return await sr.ReadToEndAsync ();
			}


		}

		internal static partial class Extensions_Process
		{

			internal enum CtrlEvents : uint
			{
				/// <summary>
				/// Generates a CTRL+C signal. This signal cannot be limited to a specific process group. 
				/// If dwProcessGroupId is nonzero, this function will succeed, but the CTRL+C signal will not be received by processes within the specified process group.
				/// </summary>
				CTRL_C_EVENT = 0,

				/// <summary>Generates a CTRL+BREAK signal.</summary>
				CTRL_BREAK_EVENT = 1,

				ctrl_close = 2,
				ctrl_logoff = 5,
				ctrl_shutdown = 6
			}
			#region API

			[DllImport ("kernel32.dll")]
			private static extern bool GenerateConsoleCtrlEvent ( [In] CtrlEvents dwctrlevent, [In] uint dwprocessgroupid );

			[DllImport ("kernel32.dll", SetLastError = true)]
			private static extern bool AttachConsole ( [In] uint dwprocessid );

			[DllImport ("kernel32.dll", SetLastError = true, ExactSpelling = true)]
			private static extern bool FreeConsole ();

			[DllImport ("kernel32.dll")]
			private static extern bool SetConsoleCtrlHandler ( [In] ConsoleCtrlHandlerDelegate? handlerroutine, [In] bool add );

			/// <summary>delegate type to be used as the handler routine for scch</summary>
			private delegate bool ConsoleCtrlHandlerDelegate ( uint ctrltype );
			#endregion


			/// <summary>Generates a CTRL+C signal</summary>
			internal static bool eConsole_SendCtrlEvent ( this Process p, CtrlEvents e, bool wait )
			{
				if (AttachConsole ((uint) p.Id))
				{
					SetConsoleCtrlHandler (null, true);
					try
					{
						if (!GenerateConsoleCtrlEvent (CtrlEvents.CTRL_C_EVENT, 0))
						{
							return false;
						}

						if (wait)
						{
							p.WaitForExit ();
						}
						else
						{
							Thread.Sleep (1000);
						}
					}
					finally
					{
						SetConsoleCtrlHandler (null, false);
						FreeConsole ();
					}
					return true;
				}
				return false;
			}

		}


		internal static partial class Extensions_Reflection
		{


			internal static string GetCallingMethodNamespacePart ( int Count )
			{
				StackTrace ST = new ();
				var lF = ST.GetFrames ().ToList ();
				lF.RemoveAt (0);
				var aF = lF.ToArray ();
				var CallingFrame = aF.First ();
				var M = CallingFrame.GetMethod ();
				var T = M!.DeclaringType!;
				string NS = T.Namespace!;
				var aNS = NS.Split ('.').Reverse ().Take (Count).Reverse ();
				return aNS.eJoin (".", NS)!;
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static TypeCode eGetTypeCode ( this Type t ) => Type.GetTypeCode (t);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static TypeCode eGetTypeCode<T> ( this T t ) => Type.GetTypeCode (typeof (T));


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T? TryCast<T> ( this object? value ) where T : class
			{
				try
				{
					return value as T;
				}
				catch (Exception ex)
				{
					// This was originally used to help me figure out why some types weren't casting in Convert.ChangeType.
					// It could be removed, but you never know, somebody might comment on a better way to do THAT to.
					var traceMessage = ex is InvalidCastException || ex is FormatException || ex is OverflowException
						? $"The given value {value} could not be cast as Type {typeof (T).FullName}."
						: ex.Message;
					Trace.WriteLine (traceMessage);
				}
				return default (T);
			}


			///// <summary>
			/////   Tries to cast <paramref name="value" /> to an instance of type <typeparamref name="T" /> .
			///// </summary>
			///// <typeparam name="T"> The type of the instance to return. </typeparam>
			///// <param name="value"> The value to cast. </param>
			///// <param name="result"> When this method returns true, contains <paramref name="value" /> cast as an instance of <typeparamref
			/////    name="T" /> . When the method returns false, contains default(T). </param>
			///// <returns> True if <paramref name="value" /> is an instance of type <typeparamref name="T" /> ; otherwise, false. </returns>
			//[MethodImpl(MethodImplOptions.AggressiveInlining)]
			//internal static bool TryCast<T>(this object value, out T? result)
			//{
			//	var destinationType = typeof(T);
			//	var inputIsNull = (value == null || value == DBNull.Value);

			//	/*
			//	 * If the given value is null, we'd normally set result to null and be done with it.
			//	 * HOWEVER, if T is not a nullable type, then we can't REALLY cast null to that type, so
			//	 * TryCast should return false.
			//	 */
			//	if (inputIsNull)
			//	{
			//		// If T is nullable, this will result in a null value in result.
			//		// Otherwise this will result in a default instance in result.
			//		result = default(T);

			//		// If the input is null and T is nullable, we report success.  Otherwise we report failure.
			//		return destinationType.IsNullable();
			//	}

			//	// Convert.ChangeType fails when the destination type is nullable.  If T is nullable we use the underlying type.
			//	var underlyingType = Nullable.GetUnderlyingType(destinationType) ?? destinationType;

			//	try
			//	{
			//		/*
			//		 * At the moment I cannot remember why I handled Guid as a separate case, but
			//		 * I must have been having problems with it at the time or I'd not have bothered.
			//		 */
			//		if (underlyingType == typeof(Guid))
			//		{
			//			if (value is string)
			//			{
			//				value = new Guid(value as string);
			//			}
			//			if (value is byte[])
			//			{
			//				value = new Guid(value as byte[]);
			//			}

			//			result = (T)Convert.ChangeType(value, underlyingType);
			//			return true;
			//		}

			//		result = (T)Convert.ChangeType(value, underlyingType);
			//		return true;
			//	}
			//	catch (Exception ex)
			//	{
			//		// This was originally used to help me figure out why some types weren't casting in Convert.ChangeType.
			//		// It could be removed, but you never know, somebody might comment on a better way to do THAT to.
			//		var traceMessage = ex is InvalidCastException || ex is FormatException || ex is OverflowException
			//								? string.Format("The given value {0} could not be cast as Type {1}.", value, underlyingType.FullName)
			//								: ex.Message;
			//		Trace.WriteLine(traceMessage);

			//		result = default(T);
			//		return false;
			//	}
			//}



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static object eCreateInstance ( this Type classType ) => Activator.CreateInstance (classType)!;


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T eCreateInstance<T> ( this Type classType ) => (T) classType.eCreateInstance ()!;


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static object eCreateInstanceParametrized ( this Type t, object[] ConstructorArgs )
			{
				// Dim rConstructor = rT.GetConstructor(Type.EmptyTypes)
				// Dim rConstructor = rT.GetConstructor(New Type() {BTSD.GetType})
				// If (rConstructor Is Nothing) Then
				// Dim sErr = "Запуск фонового задания '{0}' не удался, не найден конструктор с параметрами '{1}'!".Trim. eFormatWrap(rT.FullName,
				// BTSD.GetType.FullName)
				// Throw New System.Reflection.AmbiguousMatchException(sErr)
				// End If
				// Dim rInst = rConstructor.Invoke(New Object() {BTSD})


				if (ConstructorArgs.Length < 1)
				{
					throw new Exception ("use Typed eCreateInstance(Of XXX) instead!");
				}

				var aParamsTypes = ( from rArg in ConstructorArgs
									 let tArg = rArg.GetType ()
									 select tArg ).ToArray ();

				var rConstructor = t.GetConstructor (aParamsTypes);
				if (null == rConstructor)
				{
					throw new AmbiguousMatchException ($"Cant find class constructor for type '{t.FullName}' with specifed args count ('{ConstructorArgs.Length}') and arg types!");
				}

				return rConstructor.Invoke (ConstructorArgs);
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T eCreateInstanceParametrized<T> ( this Type t, object[] ConstructorArgs )
				=> (T) t.eCreateInstanceParametrized (ConstructorArgs);


			[Flags]
			public enum MemberTypes : int
			{
				Property = 1,
				Field = 2,
				Method = 4
			}


			//public delegate string ValueToString

			internal static Dictionary<string, string> eDumpMembersAsDictionary (
				this object obj,
				string nullValuePlaceholder = "null",
				bool skipNullValuedPropertiesAndFields = false,
				BindingFlags bindingAttr = BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.DeclaredOnly,
				MemberTypes memberType = MemberTypes.Property,
				Func<object?, MemberInfo, string?>? valueToStringConverter = null

				)
			{
				Dictionary<string, string> dicMembers = [];
				if (obj == null)
				{
					return dicMembers;
				}

				var t = obj.GetType ();
				var members = t.GetMembers (bindingAttr);

				foreach (var mi in members)
				{
					string valStr = mi.MemberType.ToString ();
					switch (mi)
					{
						case PropertyInfo pi:
							{
								if (!memberType.HasFlag (MemberTypes.Property))
								{
									continue;
								}

								if (pi.CanRead)
								{
									object? val = pi.GetValue (obj, []);
									if (val == null && skipNullValuedPropertiesAndFields)
									{
										continue;
									}

									/*
									if (val != null)
									{
										var vt = val.GetType();
										if (vt.IsGenericType && vt.GetGenericTypeDefinition() == typeof(Nullable<>))
										{
											int dddd = 5;
											//…
										}

									}
									 */

									valStr = valueToStringConverter?.Invoke (val, mi)
										?? ( val ?? nullValuePlaceholder ).ToString ()
										?? nullValuePlaceholder;
								}
								else
								{
									valStr = "[!CanRead!]";
								}
							}
							break;

						case FieldInfo fi:
							{
								if (!memberType.HasFlag (MemberTypes.Field))
								{
									continue;
								}

								object? val = fi.GetValue (obj);
								if (val == null && skipNullValuedPropertiesAndFields)
								{
									continue;
								}

								valStr = valueToStringConverter?.Invoke (val, mi)
										?? ( val ?? nullValuePlaceholder ).ToString ()
										?? nullValuePlaceholder;
							}
							break;

						case MethodInfo:
							{
								if (!memberType.HasFlag (MemberTypes.Method))
								{
									continue;
								}
							}
							break;

						default:
							break;
					}
					dicMembers.Add (mi.Name, valStr);
				}
				return dicMembers;
			}


			private const BindingFlags DEFAULT_BF = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy;

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static object? eGetPropertyValue ( this object obj, string propertyName, object? defaultValue = null, BindingFlags bf = DEFAULT_BF )
			{
				PropertyInfo? piID = obj.GetType ().GetProperty (propertyName, bf);
				if (null == piID)
				{
					return defaultValue; //throw new ArgumentOutOfRangeException($"Object '{obj.GetType()}' does not have property '{propertyName}' or wrong BindingFlags!");
				}

				return piID.GetValue (obj, null);
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static T? eGetPropertyValueAs<T> ( this object obj, string propertyName, T? defaultValue = default, BindingFlags bf = DEFAULT_BF )
				=> (T?) obj.eGetPropertyValue (propertyName, defaultValue, bf);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static Int32? eGetPropertyValue_Int32 ( this object obj, string propertyName, Int32? defaultValue = default, BindingFlags bf = DEFAULT_BF )
				=> obj.eGetPropertyValueAs<Int32?> (propertyName, defaultValue, bf);




			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void eSetPrivateFieldValue<T> (
				this T obj,
				string fieldNameStartWith,
				object newValue,
				BindingFlags bFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.FlattenHierarchy )
			{
				obj!
					.GetType ()
					.GetFields (bFlags)
					.FirstOrDefault (f => ( f.Name.StartsWith (fieldNameStartWith, StringComparison.OrdinalIgnoreCase) ))?
					.SetValue (obj, newValue);
			}



			#region Копирование свойств объектов

#if NET



			private const BindingFlags DEF_BINDING = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy;

			/// <summary>Копируем значения всех свойств</summary>
			/// <param name="target">У этого свойства заполняются данными</param>
			/// <param name="source">У этого объекта берутся значения свойств.</param>
			/// <param name="throwIfNotFound">Вызываеть ошибку если необходимое свойство не найдено в источнике данных</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eCopyPropertyValuesFrom (
				this object target,
				object source,
				IEnumerable<string> propertyNames,
				BindingFlags bf = DEF_BINDING,
				bool throwIfNotFound = true )
			{
				/*
				PropertyDescriptor[] srcProps = [.. TypeDescriptor.GetProperties(source).Cast<PropertyDescriptor>()];
				PropertyDescriptor[] trgProps = [.. TypeDescriptor.GetProperties(target).Cast<PropertyDescriptor>()];
				$"*** Copy Properties From '{source.GetType()}' to '{target.GetType()}'".eDebugWriteLine();
				 */

				PropertyInfo[] srcProps = [..
					source.GetType()
					.GetProperties(bf)
					.Where(p => p.CanRead)
					.IntersectBy(propertyNames, pd => pd.Name)];

				PropertyInfo[] trgProps = [..
					target.GetType()
					.GetProperties(bf)
					.Where(p => p.CanWrite)
					.IntersectBy(propertyNames, pd => pd.Name)];

				/*
				string[] srcPropNames = [.. srcProps.Select(p => p.Name)];
				string[] trgPropNames = [.. trgProps.Select(p => p.Name)];
				if (!Enumerable.SequenceEqual(srcPropNames, trgPropNames))
				{
					//throw new Exception("The source and target propertieslist is not Equal!");
				}
				 */

				var srcPropsDic = srcProps.ToDictionary (p => p.Name);
				foreach (var propTarget in trgProps)
				{
					if (!srcPropsDic.TryGetValue (propTarget.Name, out var propSource) || propSource == null)
					{
						if (throwIfNotFound)
						{
							throw new ArgumentOutOfRangeException (nameof (source), $"Property '{propTarget.Name}' was not found in '{source}' object!");
						}
					}

					object? val = propSource!.GetValue (source);
					propTarget.SetValue (target, val);

					/*
					var foundProp = trgProps
						.Where(p => p.Name.Equals(propName, StringComparison.InvariantCultureIgnoreCase))
						.FirstOrDefault();

					if (foundProp != null)
					{
						var tarpetProp = foundProp;
						if (!tarpetProp.IsReadOnly)
						{
							var aSoupceProperty = (from P in srcProps.Cast<PropertyDescriptor>()
												   where (P.Name.ToLower() ?? "") == (propName.ToLower() ?? "")
												   select P).ToArray();
							if (aSoupceProperty.Any())
							{
								var rFirstProp = aSoupceProperty.First();
								var objVal = rFirstProp.GetValue(source);
								tarpetProp.SetValue(target, objVal);

	#if DEBUG
								var sVal = "[Nothing]".ToUpper();
								var sType = sVal;
								if (null != objVal)
								{
									sType = objVal.GetType().ToString();
									sVal = objVal.ToString();
								}
								$"Copied '{propName}' = {sType}:('{sVal}')".eDebugWriteLine();
	#endif
							}
							else
							{
								// Свойство с таким именем не найдено в объекте-источнике
								string err = $"Свойство '{propName}' не найдено в объекте-источнике!";
	#if DEBUG
								err.eDebugWriteLine();
	#endif
								if (throwIfNotFound) throw new ArgumentOutOfRangeException(nameof(propertyNames), err);
							}
						}
					}
					 */
				}
			}


			internal enum PROPERTY_LIST_SOURCES : int
			{
				/// <summary>Take property list from Target object</summary>
				Target = 0,
				/// <summary>Take property list from Source object</summary>
				Source
			}
			/// <summary>Копируем значения всех свойств</summary>
			/// <param name="target">set property values to</param>
			/// <param name="source">get property values from</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eCopyPropertyValuesFrom ( this object target,
				object source,
				PROPERTY_LIST_SOURCES propertyListSource,
				BindingFlags bf = DEF_BINDING,
				bool throwIfNotFound = true )
			{
				/*
				var propList = propertyListSource switch
				{
					PROPERTIES_SOURCES.Source => TypeDescriptor.GetProperties(source).Cast<PropertyDescriptor>(),
					PROPERTIES_SOURCES.Target => TypeDescriptor.GetProperties(target).Cast<PropertyDescriptor>(),
					_ => throw new ArgumentOutOfRangeException(nameof(propertyListSource))
				};
				 */

				var propList = propertyListSource switch
				{
					PROPERTY_LIST_SOURCES.Source => source.GetType ().GetProperties (bf),
					PROPERTY_LIST_SOURCES.Target => target.GetType ().GetProperties (bf),
					_ => throw new ArgumentOutOfRangeException (nameof (propertyListSource))
				};

				var propNames = propList
					.Select (p => p.Name);

				target.eCopyPropertyValuesFrom (source, propNames.ToArray (), bf, throwIfNotFound);
			}

#endif

			#endregion




			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static IntPtr OffsetOfField ( this Type T, string FieldName )
				=> Marshal.OffsetOf (T, FieldName);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static int OffsetOfField32 ( this Type T, string FieldName )
				=> T.OffsetOfField (FieldName).ToInt32 ();


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static long OffsetOfField64 ( this Type T, string FieldName )
				=> T.OffsetOfField (FieldName).ToInt64 ();





			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static T? eGetAttributeOf<T> ( this PropertyDescriptor pd ) where T : System.Attribute
				=> (T?) pd.Attributes[ typeof (T) ];



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void eSetAttributeValueOf<TAttr> (
				this PropertyDescriptor pd,
				string attributeInternalFiledStartWith,
				object newValue,
				BindingFlags bFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.FlattenHierarchy
				) where TAttr : Attribute
					=> pd.eGetAttributeOf<TAttr> ()?.eSetPrivateFieldValue (attributeInternalFiledStartWith, newValue, bFlags);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void eSetAttribute_Browsable ( this PropertyDescriptor pd, bool browsable )
				=> pd.eSetAttributeValueOf<BrowsableAttribute> ("<Browsable", browsable);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void eSetAttribute_ReadOnly ( this PropertyDescriptor pd, bool readOnly )
				=> pd.eSetAttributeValueOf<ReadOnlyAttribute> ("<isReadOnly", readOnly);




			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void eSetPropertiyAttribute<TAttr> ( this object targetObject, object propertyValue, object newAttributeValue,
				 [CallerArgumentExpression (nameof (propertyValue))] string propName = ""
				) where TAttr : System.Attribute
			{

				if (!propName.Contains ("."))
				{
					throw new ArgumentOutOfRangeException (nameof (propertyValue));
				}

				propName = propName.Split ('.').Last ();

				PropertyDescriptor[] props = [ .. TypeDescriptor.GetProperties (targetObject.GetType ()).Cast<PropertyDescriptor> () ];
				props = [ .. props.Where (pd => pd.Name == propName) ];

				PropertyDescriptor? pd = props.FirstOrDefault ();
				if (pd == null)
				{
					throw new ArgumentOutOfRangeException (nameof (propertyValue));
				}

				string attrPrefix = @"<" + typeof (TAttr).Name;
				//pd.eSetAttributeValueOf<TAttribute>(attrPrefix, attributeValue);
				var a = pd.eGetAttributeOf<TAttr> ();

				BindingFlags bFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.FlattenHierarchy;
				a.eSetPrivateFieldValue (attrPrefix, newAttributeValue, bFlags);
			}





			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void PropertyGrid_SetClassPropertiesBrowsable<T> (
				this T o,
				bool browsable,
				Func<PropertyDescriptor, bool>? wherePredicate = null )
			{
				PropertyDescriptor[] aProps = [ .. TypeDescriptor.GetProperties (o!.GetType ()).Cast<PropertyDescriptor> () ];
				if (wherePredicate != null)
				{
					aProps = [ .. aProps.Where (pd => wherePredicate (pd)) ];
				}

				aProps.eForEach (pd => pd.eSetAttribute_Browsable (browsable));
			}




			/// <summary>
			/// Enables Readonly attribute as specifed class properties for edit in ProperGrig!
			/// WARNING! Readonly changes for not only THIS class instance, but for all classes with use this class defenition in app domain!
			/// bc. you must to reset this attribute value, imediately after property grid is closed, for avoid unpredictable effects...
			/// 
			/// readonly attribute has all fields in class, not only direct specifed in code!
			/// </summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void PropertyGrid_SetClassPropertiesReadOnly<T> (
				this T o,
				bool readOnly,
				Func<PropertyDescriptor, bool>? wherePredicate = null )
			{
				PropertyDescriptor[] aProps = TypeDescriptor.GetProperties (o!.GetType ()).Cast<PropertyDescriptor> ().ToArray ();
				if (wherePredicate != null)
				{
					aProps = aProps.Where (pd => wherePredicate (pd)).ToArray ();
				}

				aProps.eForEach (pd => pd.eSetAttribute_ReadOnly (readOnly));
			}







		}


		internal static partial class Extensions_Serialize_Clone
		{

			#region Serialize

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eWriteCSV<T> ( this TextWriter TW, string[] aColumnHeadersArray, IEnumerable<T> aRows, Func<T, string[]> cbGetRowValuesArray, string C_CSV_SEPARATOR = ";", bool MakeSafeChars = false )
			{

				//</summary> C_CSV_SEPARATOR = ";"
				int iColumnCount = aColumnHeadersArray.Count ();

				static string cbPrepareValue ( string sSourceValue )
				{
					sSourceValue = sSourceValue.eCheckNullOrWhiteSpace ();
					sSourceValue = sSourceValue.Replace ("\"", "\"\"").Replace (",", @"\,").Replace (";", @"\;").Replace (Environment.NewLine, @"\" + Environment.NewLine).Replace (@"\", @"\\");
					return sSourceValue;
				};

				if (MakeSafeChars)
				{
					aColumnHeadersArray = ( from sColumnValue in aColumnHeadersArray
											let S = cbPrepareValue (sColumnValue)
											select S ).ToArray ();
				}

				var sHeaderLine = aColumnHeadersArray.eJoin (C_CSV_SEPARATOR);
				TW.WriteLine (sHeaderLine);
				foreach (var CP in aRows)
				{
					var aValuesArray = cbGetRowValuesArray (CP);
					if (aValuesArray.Count () != iColumnCount)
					{
						throw new Exception ("Current Row aValuesArray.Count <> aColumnHeadersArray.Count!");
					}

					if (MakeSafeChars)
					{
						aValuesArray = aValuesArray
							.Select (columnValue => cbPrepareValue (columnValue))
							.ToArray ();
					}

					string sLine = aValuesArray.eJoin (C_CSV_SEPARATOR)!;
					TW.WriteLine (sLine);
				}
			}

			#region XML Serialization


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eSerializeXML ( this object SerializableObject, string FileName )
				=> SerializableObject.eSerializeXML (FileName, Encoding.Unicode);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eSerializeXML ( this object SerializableObject, string file, Encoding? enc = null )
				=> SerializableObject.eSerializeXML (file.eToFileInfo ()!, enc ??= Encoding.Unicode);


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eSerializeXML ( this object SerializableObject, FileInfo f, Encoding? enc = null )
			{
				using FileStream fs = f.Create ();
				SerializableObject.eSerializeXML (fs, enc ??= Encoding.Unicode);
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eSerializeXML ( this object SerializableObject, Stream s, Encoding? enc = null )
			{
				using StreamWriter sw = new (s, enc ??= Encoding.Unicode, 2048, true);
				System.Xml.Serialization.XmlSerializer XS = new (SerializableObject.GetType ());
				XS.Serialize (sw, SerializableObject);
				sw.Flush ();
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eSerializeAsXML ( this object SerializableObject )
			{
				StringBuilder sb = new ();
				using (StringWriter sw = new (sb))
				{
					System.Xml.Serialization.XmlSerializer xs = new (SerializableObject.GetType ());
					xs.Serialize (sw, SerializableObject);
				}
				return sb.ToString ();
			}

			/*
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static string eSerializeAsXML(this System.Collections.IList SerializableObject)
		{
		StringBuilder sb = new();
		using (StringWriter sw = new(sb))
		{
		System.Xml.Serialization.XmlSerializer xs = new(SerializableObject.GetType());
		xs.Serialize(sw, SerializableObject);
		}
		return sb.ToString();
		}
			 */


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static byte[] eSerializeAsXML ( this System.Data.DataSet dts )
			{
				using MemoryStream ms = new ();
				dts.WriteXml (ms, System.Data.XmlWriteMode.IgnoreSchema);
				return ms.eReadAllBytes ();
			}




			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static object? eDeSerializeXMLAsObject (
				this string xmlString,
				Type deserializeTo,
				Object? defaultValue = null,
				bool throwOnError = false )
			{
				try
				{
					if (string.IsNullOrWhiteSpace (xmlString))
					{
						return defaultValue;
					}

					using StringReader sr = new (xmlString);
					using System.Xml.XmlTextReader xtr = new (sr);
					System.Xml.Serialization.XmlSerializer xs = new (deserializeTo);
					var O = xs.Deserialize (xtr);
					return O;
				}
				catch
				{
					if (throwOnError)
					{
						throw;
					}

					return defaultValue;
				}
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static object? eDeSerializeXMLAsObject (
				this FileInfo xmlFile,
				Type deserializeTo,
				Object? defaultValue = null,
				bool throwOnError = false )
			{
				return xmlFile?
					.eReadAsText (System.Text.Encoding.Unicode)?
					.eDeSerializeXMLAsObject (deserializeTo, defaultValue, throwOnError);
			}






			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T? eDeSerializeXML<T> ( this string xmlString, T? defaultValue = default, bool throwOnError = false )
			{
				try
				{
					if (string.IsNullOrWhiteSpace (xmlString))
					{
						return defaultValue;
					}

					using StringReader sr = new (xmlString);
					using System.Xml.XmlTextReader xtr = new (sr);
					System.Xml.Serialization.XmlSerializer xs = new (typeof (T));
					var O = xs.Deserialize (xtr);
					return (T?) O;
				}
				catch
				{
					if (throwOnError)
					{
						throw;
					}

					return defaultValue;
				}
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T? eDeSerializeXML<T> (
				this Stream sm,
				T? defaultValue = default,
				Encoding? enc = null,
				bool throwOnError = false )
			{
				try
				{
					enc ??= Encoding.Unicode;
					StreamReader sr = new (sm, enc);
					System.Xml.Serialization.XmlSerializer xs = new (typeof (T));
					return (T?) xs.Deserialize (sr);
				}
				catch
				{
					if (throwOnError)
					{
						throw;
					}

					return defaultValue;
				}
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T? eDeSerializeXMLFile<T> (
				this string sFile,
				T? defaultValue = default,
				Encoding? enc = null,
				bool throwOnError = false )
			{
				enc ??= Encoding.Unicode;

				try
				{
					return sFile.eToFileInfo ()!.eDeSerializeXML<T> (defaultValue, enc, throwOnError);
				}
				catch
				{
					if (throwOnError)
					{
						throw;
					}

					return defaultValue;
				}
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T? eDeSerializeXML<T> (
				this FileInfo File,
				T? defaultValue = default,
				Encoding? enc = null,
				bool throwOnError = false )
			{
				enc ??= Encoding.Unicode;
				try
				{
					using FileStream fs = File.OpenRead ();
					var r = fs.eDeSerializeXML<T> (defaultValue, throwOnError: throwOnError);
					return r;
				}
				catch
				{
					if (throwOnError)
					{
						throw;
					}

					return defaultValue;
				}
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T[] eDeSerializeXMLArrays<T> ( this FileInfo[] fiFiles, bool ThrowExceptionOnError = false )
			{
				if (!fiFiles.Any ())
				{
					return Array.Empty<T> ();
				}

				var lTotalDeserializedObjects = new List<T> ();
				foreach (var fiFileToDeserialize in fiFiles)
				{
					var ArrayOfDeserializedObjects = fiFileToDeserialize.eDeSerializeXML<T[]> (throwOnError: ThrowExceptionOnError);
					if (ArrayOfDeserializedObjects != null && ArrayOfDeserializedObjects.Any ())
					{
						lTotalDeserializedObjects.AddRange (ArrayOfDeserializedObjects);
					}
				}

				var aTotalRulesToImport = lTotalDeserializedObjects.ToArray ();
				return aTotalRulesToImport;
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static List<T> eDeSerializeXMLLists<T> ( this FileInfo[] fiFiles, bool ThrowExceptionOnError = false )
			{
				var lTotalDeserializedObjects = new List<T> ();
				if (!fiFiles.Any ())
				{
					return lTotalDeserializedObjects;
				}

				foreach (var fiFileToDeserialize in fiFiles)
				{
					var ListOfDeserializedObjects = fiFileToDeserialize.eDeSerializeXML<List<T>> (throwOnError: ThrowExceptionOnError);
					if (ListOfDeserializedObjects != null && ListOfDeserializedObjects.Any ())
					{
						lTotalDeserializedObjects.AddRange (ListOfDeserializedObjects);
					}
				}
				return lTotalDeserializedObjects;
			}


			#endregion



			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eSerializeXMLSettings ( this object SerializableObject, string ParamName )
			{
				throw new NotImplementedException ();

				//string sXML = SerializableObject.eSerializeXML();
				//UOM.Settings.mAppSettings.SaveSetting(ParamName, sXML, ThrowExceptionIfError: true);
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T eDeSerializeXMLSettings<T> ( this string ParamName, T defaultValue )
			{
				throw new NotImplementedException ();

				//string sXML = UOM.Settings.mAppSettings.GetSetting_String(ParamName, null, ThrowExceptionIfError: false).Value;
				//if (sXML.IsNotNullOrWhiteSpace())
				//{
				//    var Obj = sXML.eDeSerializeXML(defaultValue);
				//    return Obj;
				//}
				//else
				//{
				//    return defaultValue;
				//}
			}



			#endregion

			/// <summary>Клонирует объект черех XML сериализацию в памяти</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static T? eCloneViaXMLSerialization<T> ( this T O )
			{
				_ = O ?? throw new ArgumentNullException (nameof (O));
				using MemoryStream ms = new ();
				O.eSerializeXML (ms);
				ms.Seek (0L, SeekOrigin.Begin);
				return ms.eDeSerializeXML<T> (throwOnError: true);
			}


			/// <summary>Клонирует объект системным методом CLONE, возвращая объект такого-же типа, что и исходный</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static T eCloneAsSomeType<T> ( this T rSourceObject ) where T : ICloneable
				=> (T) rSourceObject.Clone ();



		}


		internal static class Extensions_Security_Ecryption_AES
		{

			private const string PLATFORM_WINDOWS = "Windows";

			public enum AES_KEY_SIZES : int
			{
				KEY_128 = 128,
				KEY_256 = 256
			}

			/// <summary>the iteration count must be greater than zero. The minimum recommended number of iterations is 1000.</summary>
			public const int AES_DEFAULT_ITERATIONS = 1000;
			public const int AES_MIN_SALT_SIZE = 8;
			private const int AES_BLOCK_SIZE = 128;

			private static readonly byte[] DEFAULT_SALT_AES = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

			/// <summary>Encrypt</summary>
			/// <param name="saltBytes">The salt size must be 8 bytes or larger</param>
			/// <param name="createSaltFromPassword"></param>
			/// <param name="iterations">the iteration count must be greater than zero. The minimum recommended number of iterations is 1000</param>
			/// <exception cref="ArgumentOutOfRangeException"></exception>
			/// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.rfc2898derivebytes.-ctor?view=netframework-4.8#system-security-cryptography-rfc2898derivebytes-ctor(system-byte()-system-byte()-system-int32)"/>
			public static byte[] eEncrypt_AES (
				this byte[] bytesToBeEncrypted,
				byte[] passwordBytes,
				byte[]? saltBytes = null,
				bool createSaltFromPassword = true,
				AES_KEY_SIZES keySize = AES_KEY_SIZES.KEY_256,
				int iterations = AES_DEFAULT_ITERATIONS )
			{

				if (createSaltFromPassword)
				{
					var lSalt = passwordBytes.ToList ();
					while (lSalt.Count < AES_MIN_SALT_SIZE)
					{
						lSalt.AddRange (passwordBytes);
					}
					saltBytes = [ .. lSalt ];
				}
				else
				{
					if (saltBytes != null && saltBytes.Length < AES_MIN_SALT_SIZE)
					{
						throw new ArgumentOutOfRangeException (nameof (saltBytes), "The salt size must be 8 bytes or larger!");
					}

					saltBytes ??= DEFAULT_SALT_AES;
				}

				Rfc2898DeriveBytes key = new (passwordBytes, saltBytes, iterations);
				//using System.Security.Cryptography.AesCng aesChiper = new()
				using Aes aesChiper = Aes.Create ();
				{
					aesChiper.KeySize = (int) keySize;
					aesChiper.BlockSize = AES_BLOCK_SIZE;
					aesChiper.Key = key.GetBytes ((int) keySize / 8);
					aesChiper.IV = key.GetBytes (AES_BLOCK_SIZE / 8);
					aesChiper.Mode = CipherMode.CBC;
				}

				using MemoryStream ms = new ();
				using (CryptoStream cs = new (ms, aesChiper.CreateEncryptor (), CryptoStreamMode.Write))
				{
					cs.Write (bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
					cs.Close ();
				}
				return ms.ToArray ();
			}

			/// <inheritdoc cref="eEncrypt_AES(byte[], byte[], byte[], bool, AES_KEY_SIZES, int)" />
			public static byte[] eEncrypt_AES (
				this string text,
				string password,
				byte[]? saltBytes = null,
				bool createSaltFromPassword = true,
				AES_KEY_SIZES keySize = AES_KEY_SIZES.KEY_256,
				int iterations = AES_DEFAULT_ITERATIONS )
				=> text
					.eGetBytes_Unicode ()
					.eEncrypt_AES (password.eGetBytes_Unicode (), saltBytes, createSaltFromPassword, keySize, iterations);

			/// <inheritdoc cref="eEncrypt_AES(byte[], byte[], byte[], bool, AES_KEY_SIZES, int)" />
			public static string eEncrypt_AES_ToBase64String (
				this string text,
				string password,
				byte[]? saltBytes = null,
				bool createSaltFromPassword = true,
				AES_KEY_SIZES keySize = AES_KEY_SIZES.KEY_256,
				int iterations = AES_DEFAULT_ITERATIONS )
					=> text.eEncrypt_AES (password, saltBytes, createSaltFromPassword, keySize, iterations)
						.eToBase64String ();



			/// <summary>Decrypt</summary>
			/// <param name="saltBytes">The salt size must be 8 bytes or larger</param>
			/// <param name="createSaltFromPassword"></param>
			/// <param name="iterations">the iteration count must be greater than zero. The minimum recommended number of iterations is 1000</param>
			/// <exception cref="ArgumentOutOfRangeException"></exception>
			/// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.rfc2898derivebytes.-ctor"/>
			public static byte[] eDecrypt_AES (
				this byte[] bytesToBeDecrypted,
				byte[] passwordBytes,
				byte[]? saltBytes = null,
				bool createSaltFromPassword = true,
				AES_KEY_SIZES keySize = AES_KEY_SIZES.KEY_256,
				int iterations = AES_DEFAULT_ITERATIONS )
			{

				if (createSaltFromPassword)
				{
					var lSalt = passwordBytes.ToList ();
					while (lSalt.Count < AES_MIN_SALT_SIZE)
					{
						lSalt.AddRange (passwordBytes);
					}
					saltBytes = lSalt.ToArray ();
				}
				else
				{
					if (saltBytes != null && saltBytes.Length < AES_MIN_SALT_SIZE)
					{
						throw new ArgumentOutOfRangeException (nameof (saltBytes), "The salt size must be 8 bytes or larger!");
					}

					saltBytes ??= DEFAULT_SALT_AES;
				}

				Rfc2898DeriveBytes key = new (passwordBytes, saltBytes, iterations);
				//using System.Security.Cryptography.AesCng aesChiper = new()
				using Aes aesChiper = Aes.Create ();
				{
					aesChiper.KeySize = (int) keySize;
					aesChiper.BlockSize = AES_BLOCK_SIZE;
					aesChiper.Key = key.GetBytes ((int) keySize / 8);
					aesChiper.IV = key.GetBytes (AES_BLOCK_SIZE / 8);
					aesChiper.Mode = CipherMode.CBC;
				}

				using MemoryStream ms = new ();
				using (CryptoStream cs = new (ms, aesChiper.CreateDecryptor (), CryptoStreamMode.Write))
				{
					cs.Write (bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
					cs.Close ();
				}
				return ms.ToArray ();
			}

			public static byte[] eDecrypt_AES_FromBase64String (
				this string base64String,
				string password,
				byte[]? saltBytes = null,
				bool createSaltFromPassword = true,
				AES_KEY_SIZES keySize = AES_KEY_SIZES.KEY_256,
				int iterations = AES_DEFAULT_ITERATIONS )
				=> base64String
					.eFromBase64String ()
					.eDecrypt_AES (password.eGetBytes_Unicode (), saltBytes, createSaltFromPassword, keySize, iterations);

		}


		internal static partial class Extensions_Security_SecureString
		{


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static SecureString eToSecureString ( this string src, bool makeReadOnly = true )
			{
				SecureString sec = new ();
				src.ToCharArray ().ToList ().ForEach (sec.AppendChar);
				if (makeReadOnly)
				{
					sec.MakeReadOnly ();
				}

				return sec;
			}


			/// <summary>
			/// Creates a managed character array from the secure string using methods in System.Runetime.InteropServices
			/// copying data into a BSTR (unmanaged binary string) and then into a managed character array which is returned from this method.
			/// Data in the unmanaged memory temporarily used are freed up before the method returns.
			/// </summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static char[] eFromSecureStringToCharArray ( this SecureString secureString )
			{
				var ptr = IntPtr.Zero;
				try
				{
					// alloc unmanaged binary string  (BSTR) and copy contents of SecureString into this BSTR
					// The code should use SecureStringToBSTR because SecureString can contain \0 as non-terminating characters,
					// but SecureStringToGlobalAllocUnicode treat it as a null-terminated string.
					ptr = Marshal.SecureStringToBSTR (secureString);
					char[] bytes = new char[ secureString.Length ];

					//copy to managed memory char array from unmanaged memory 
					Marshal.Copy (ptr, bytes, 0, secureString.Length);
					return bytes;
				}
				finally
				{
					if (ptr != IntPtr.Zero)
					{
						Marshal.ZeroFreeBSTR (ptr);
					}
				}
			}

			/// <summary>Returns an unsafe string in managed memory from SecureString. </summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static string eFromSecureStringToUnsafeString ( this SecureString secureString )
				=> new (secureString!.eFromSecureStringToCharArray ());

			/* ALTERNATIVE WAY
		SecureString theSecureString = new NetworkCredential("", "myPass").SecurePassword;
		string theString = new NetworkCredential("", theSecureString).Password;
		*/




		}


		internal static partial class Extensions_Security_Hash
		{


			/// <summary>
			/// https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm.create
			/// </summary>
			public enum HashNames : int
			{
				SHA1,
				MD5,
				SHA256,
				SHA384,
				SHA512
			}

			/// <inheritdoc cref="HashAlgorithm.Create(string)"/>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			private static HashAlgorithm? CreateHashAlgorithm ( HashNames hn ) => HashAlgorithm.Create (hn.ToString ());

			/// <summary>
			/// https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.keyedhashalgorithm.create
			/// </summary>
			public enum KeyedHashNames : int
			{
				HMACSHA1,
				HMACMD5,
				HMACRIPEMD160,
				HMACSHA256,
				HMACSHA384,
				HMACSHA512,
				MACTripleDES
			}

			/// <inheritdoc cref="KeyedHashAlgorithm.Create(string)"/>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			private static KeyedHashAlgorithm? CreateKeyedHashAlgorithm ( KeyedHashNames kha ) => KeyedHashAlgorithm.Create (kha.ToString ());


			/// <inheritdoc cref="HashAlgorithm.ComputeHash(byte[])"/>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static byte[] eComputeHash ( this byte[] ab, HashNames ha )
			{
				using HashAlgorithm? H = CreateHashAlgorithm (ha);
				return H!.ComputeHash (ab);
			}

			/// <inheritdoc cref="eComputeHash"/>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static byte[] eComputeHashUni ( this string str, HashNames ha )
				=> str.eGetBytes_Unicode ().eComputeHash (ha);

			/// <inheritdoc cref="CreateKeyedHashAlgorithm"/>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static byte[] eComputeKeyedHash ( this byte[] ab, KeyedHashNames kha )
			{
				using KeyedHashAlgorithm? H = CreateKeyedHashAlgorithm (kha);
				return H!.ComputeHash (ab);
			}


			/// <inheritdoc cref="eComputeKeyedHash"/>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static byte[] eComputeKeyedHashUni ( this string str, KeyedHashNames kha )
				=> str.eGetBytes_Unicode ().eComputeKeyedHash (kha);
		}


		internal static partial class Extensions_Security_Random
		{

			/// <summary>Use a fast System.Random class, using a time-dependent default seed value.</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static byte[] eGetRandomBytesOld ( this int count )
			{
				var bytes = new byte[ count ];
				if (count > 0)
				{
					var R = new Random ();
					R.NextBytes (bytes);
				}
				return bytes;
			}
#if NET
			/// <summary>This is modern method in .Net Core
			/// Uses RandomNumberGenerator.GetBytes</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static byte[] eGetRandomBytes ( this int count ) => RandomNumberGenerator.GetBytes (count);
#else
			/// <summary>Uses RandomNumberGenerator.GetBytes</summary>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static byte[] eGetRandomBytes(this int count)
			{
				if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

				using RandomNumberGenerator rng = RandomNumberGenerator.Create();
				byte[] bytes = new byte[count];
				rng.GetBytes(bytes);
				return bytes;
			}
#endif

		}


		internal static partial class Extensions_Structures_Ptr
		{

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static byte[] ePtrToBytes ( this IntPtr lpBuffer, int nBytes )
			{
				var abData = new byte[ nBytes ];
				Marshal.Copy (lpBuffer, abData, 0, nBytes);
				return abData;
			}


			/// <inheritdoc cref="Marshal.PtrToStructure(IntPtr)"/>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static T eToStructure<T> ( this IntPtr Ptr ) where T : struct
				=> Marshal.PtrToStructure<T> (Ptr);


			/// <summary>Последовательно читаем с указателя в массив одинаковых структур</summary>
			/// <param name="structCount">Количество структур для чтения</param>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static IEnumerable<T> eToStructuresSequential<T> ( this IntPtr Ptr, int structCount, int initialOffset = 0 ) where T : struct
			{
				if (initialOffset != 0)
				{
					Ptr += initialOffset;
				}

				int structSize = Marshal.SizeOf (typeof (T));
				for (int structIndex = 1, loopTo = structCount ; structIndex <= loopTo ; structIndex++)
				{
					T structInstance = Ptr.eToStructure<T> ();
					yield return structInstance;
					Ptr += structSize;
				}
			}


			/// <inheritdoc cref="Marshal.StructureToPtr"/>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static void eStructureToPtr<T> ( this T structureRef, IntPtr memoryPtr, bool deleteOld = false ) where T : struct
				=> Marshal.StructureToPtr (structureRef, memoryPtr, deleteOld);


			/// <summary>
			/// Allocates memory for structure and invokes acton with it, finally free memory.
			/// </summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static TStruct AllocStruct<TStruct> ( Action<IntPtr, int> a ) where TStruct : struct
			{
				int len = Marshal.SizeOf (typeof (TStruct));
				IntPtr mem = Marshal.AllocHGlobal (len);
				try
				{
					a.Invoke (mem, len);
					return (TStruct) Marshal.PtrToStructure (mem, typeof (TStruct))!;
				}
				finally
				{
					// Free the unmanaged memory.
					Marshal.FreeHGlobal (mem);
				}
			}


			public static unsafe T UnsafeCast<T, R> ( this R input )
				where T : unmanaged
				where R : unmanaged
			{
				return *(T*) &input;
			}
		}


		internal static partial class Extensions_Lazy
		{


			[MethodImpl (MethodImplOptions.NoOptimization)]
			public static void ForceCreate<T> ( this Lazy<T> lz )
			{
				var a = lz.Value;
			}

		}


		internal static partial class Extensions_XML
		{

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static XmlNode[] eToArray ( this XmlNodeList xnl ) => xnl.Cast<XmlNode> ().ToArray ();

			public static string eAsString ( this XmlDocument xmlDoc )
			{
				using (StringWriter sw = new ())
				{
					using (XmlTextWriter tx = new (sw))
					{
						xmlDoc.WriteTo (tx);
					}
					return sw.ToString ();
				}
			}

			public static System.Xml.Linq.XDocument eToXDocument ( this XmlDocument xd )
			{
				System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Parse (xd.eAsString ());
				return doc;
			}


			private static IEnumerable<System.Xml.Linq.XElement> eGetElementsOrDescendants ( this XContainer node, SearchOption so = SearchOption.TopDirectoryOnly )
			{
				//https://stackoverflow.com/questions/8460464/finding-element-in-xdocument
				//Имейте в виду, что свойство Name возвращает объект, который имеет LocalName и Namespace. Вот почему вы должны использовать Name.LocalName, если хотите сравнить по имени.
				//Мой опыт работы с большими и сложными файлами XML заключается в том, что иногда ни элементы, ни потомки не работают при извлечении определенного элемента (и я до сих пор не знаю, почему).

				/*										 				 
				 Elements()будет проверять только прямые дочерние элементы, 
				которые в первом случае являются корневыми элементами, 
				а во втором — дочерними элементами корневого элемента, 
				поэтому во втором случае вы получите совпадение. 

				Если вы просто хотите, чтобы любой соответствующий потомок использовал Descendants()вместо этого:
				var query = from c in xmlFile.Descendants("Band") select c;
				 */

				return ( so == SearchOption.TopDirectoryOnly )
					? node.Elements ()
					: node.Descendants ();
			}

			public delegate bool XElementToNameCompareDelegate ( XElement node, string name );

			private static Lazy<XElementToNameCompareDelegate> defaultXElementToNameComparer = new (() => new XElementToNameCompareDelegate (( x, s ) => x.Name.LocalName == s));


			public static System.Xml.Linq.XElement[] eFindNodes (
				this XContainer node,
				string name,
				SearchOption so = SearchOption.TopDirectoryOnly,
				XElementToNameCompareDelegate? comparePredicate = null )
			{
				//https://stackoverflow.com/questions/8460464/finding-element-in-xdocument
				//Имейте в виду, что свойство Name возвращает объект, который имеет LocalName и Namespace. Вот почему вы должны использовать Name.LocalName, если хотите сравнить по имени.
				//Мой опыт работы с большими и сложными файлами XML заключается в том, что иногда ни элементы, ни потомки не работают при извлечении определенного элемента (и я до сих пор не знаю, почему).

				var elements = node.eGetElementsOrDescendants (so);
				return elements
					.Where (x => ( comparePredicate ?? defaultXElementToNameComparer.Value )!.Invoke (x, name))
					.ToArray ();
			}

			public static System.Xml.Linq.XElement? eFindSingleOrDefaultNode (
				this XContainer node,
				string name, SearchOption so = SearchOption.TopDirectoryOnly,
				XElementToNameCompareDelegate? comparePredicate = null )
				=> node
				.eGetElementsOrDescendants (so)
				.SingleOrDefault (x => ( comparePredicate ?? defaultXElementToNameComparer.Value )!.Invoke (x, name));


			/// <summary>
			/// Gets Last node in each tree path which corresponds to tree like 'nodeTreeNames1\nodeTreeNames2\nodeTreeNamesXXX'
			/// SAMPLE: var xProps = Manifest.eFindTree(null, "Package", "Properties").FirstOrDefault();
			/// </summary>
			/// <param name="comparePredicate">Custom node to name comparer, or NULL, to use default comparer</param>			
			/// <returns> Last node in each tree path which corresponds to tree path 'nodeTreeNames1\nodeTreeNames2\nodeTreeNamesXXX'</returns>
			public static System.Xml.Linq.XElement[] eFindTree (
				this XContainer node,
				SearchOption startPointSearchOptions,
				XElementToNameCompareDelegate? comparePredicate = null,
				params string[] nodeTreeNames )
			{
				if (nodeTreeNames.Length < 2)
				{
					throw new ArgumentOutOfRangeException (nameof (nodeTreeNames), $"{nameof (nodeTreeNames)} count must be > 1 !");
				}

				comparePredicate ??= defaultXElementToNameComparer.Value;

				var treesBranches = node.eFindNodes (nodeTreeNames[ 0 ], startPointSearchOptions);
				List<XElement> lFoundLastNodes = [];


				void FindNextChildForNode ( XContainer x, string[] childrensToFind )
				{
					string childToFind = childrensToFind[ 0 ];
					XElement[] foundChildrens = x
						.Elements ()
						.Where (p => comparePredicate!.Invoke (p, childToFind))
						.ToArray ();

					if (childrensToFind.Length == 1)
					{
						//We Found last needed Child in this branch path!
						foundChildrens.ToList ().ForEach (x => lFoundLastNodes.Add (x));
					}
					else
					{
						//We have to found next children
						childrensToFind = childrensToFind.eTakeFrom (1);
						foundChildrens
							.ToList ()
							.ForEach (x => FindNextChildForNode (x, childrensToFind));
					}
				}

				nodeTreeNames = nodeTreeNames.eTakeFrom (1);
				treesBranches
					.ToList ()
					.ForEach (x => FindNextChildForNode (x, nodeTreeNames));

				XElement[] foundLastNodes = lFoundLastNodes.ToArray ();

				return foundLastNodes;
				//return Array.Empty<System.Xml.Linq.XElement>();
			}


			public static System.Xml.Linq.XElement[] eFindTree ( this XContainer node, params string[] nodeTreeNames )
				=> node.eFindTree (SearchOption.AllDirectories, null, nodeTreeNames);



		}


#if !ANDROID


		internal static partial class Extensions_Console
		{

			internal static EventArgs _ConsoleLock = new ();


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void eWriteConsole (
				this string sText,
				ConsoleColor? clrFore = null,
				ConsoleColor? clrBack = null,
				bool WtiteLine = true,
				StreamWriter? swLog = null )
			{
				lock (_ConsoleLock)
				{
					var clrCurrrenFore = Console.ForegroundColor;
					var clrCurrrentBk = Console.ForegroundColor;
					try
					{
						if (clrFore.HasValue)
						{
							Console.ForegroundColor = clrFore.Value;
						}

						if (clrBack.HasValue)
						{
							Console.BackgroundColor = clrBack.Value;
						}

						if (WtiteLine)
						{
							Console.WriteLine (sText);
							swLog?.WriteLine (sText);
						}
						else
						{
							Console.Write (sText);
							swLog?.Write (sText);
						}
					}
					finally
					{
						//Restoring Old Colors
						//if (clrBack.HasValue) Console.BackgroundColor = clrCurrrentBk;
						//if (clrFore.HasValue) Console.ForegroundColor = clrCurrrenFore;
						Console.ResetColor ();
					}
				}
			}

			/// <summary>Run Action in Try/catch block And outputs Error to console if occurs</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void runTryCatchCon ( this Action A, string ActionTitle = "" )
			{
				if (!string.IsNullOrWhiteSpace (ActionTitle))
				{
					lock (_ConsoleLock)
					{
						Console.WriteLine ($"{ActionTitle}".Trim ());
					}
				}

				try { A.Invoke (); }
				catch (Exception ex)
				{
					$"ERROR: {ex.Message}".Trim ().eWriteConsole (ConsoleColor.Yellow, ConsoleColor.DarkRed, false); Console.WriteLine ();
				}
			}




			/// <summary>Display progress bar</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void eWriteConsoleProgress (
				this float fProgress,
				int iDecimalPlaces = constants.C_DEFAULT_DECIMAL_DIGITS,
				int ProgressBarLenght = 30,
				char cProgressBarFillChar = '#',
				char cProgressBarEmptyChar = '-',
				string ProgressPrefixString = "Downloading:" )
			{
				fProgress = fProgress.eCheckRange (0, 1);

				var iBarFill = (Int32) ( (float) ProgressBarLenght * fProgress );
				if (iBarFill > ProgressBarLenght)
				{
					iBarFill = ProgressBarLenght;
				}

				var sProgressBar = new string (cProgressBarFillChar, iBarFill);
				if (iBarFill < ProgressBarLenght)
				{
					sProgressBar = sProgressBar.PadRight (ProgressBarLenght, cProgressBarEmptyChar);
				}

				lock (_ConsoleLock)
				{
					Console.Write ($"{ProgressPrefixString} [{sProgressBar}] {fProgress.eFormatPercent (iDecimalPlaces)}\r");
				}
			}

			/// <summary>Display progress bar</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static void eWriteConsoleProgress (
				this int iProgress,
				int ProgressBarLenght = 30,
				char cProgressBarFillChar = '#',
				char cProgressBarEmptyChar = '-',
				string ProgressPrefixString = "Downloading:" )
				=> ( (float) ( (float) iProgress / (float) 100 ) ).eWriteConsoleProgress (0, ProgressBarLenght, cProgressBarFillChar, cProgressBarEmptyChar, ProgressPrefixString);


			/// <summary>Возвращает строку вида 'HEADER---------'</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string eConsole_CreateHeaderLine ( this string Text, int iWith = constants.C_DEFAULT_CONSOLE_WIDTH_1 )
				=> Text.PadRight (iWith, '-');

			/// <summary>Если строка превышает заданную длинну, то разбивает на строки этой длинны.</summary>
			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			internal static string[] eConsole_SplitStringToFixedWidth ( this string sText, int MaxRowWidth = constants.C_DEFAULT_CONSOLE_WIDTH_1 )
			{
				var aList = new List<string> ();
				while (sText.Length > MaxRowWidth)
				{
					string sLeftPart = sText.Substring (0, MaxRowWidth);
					aList.Add (sLeftPart);
					sText = sText.Substring (MaxRowWidth);
				}

				aList.Add (sText);
				return aList.ToArray ();
			}





			/// <summary>Read a password from the console into a SecureString</summary>
			/// <returns>Password stored in a secure string</returns>
			public static SecureString ReadPassword ( string Question = constants.CS_ENTER_PWD_EN, char PwdChar = '*' )
			{
				lock (_ConsoleLock)
				{
					var password = new SecureString ();
					Console.WriteLine (Question);

					// get the first character of the password
					var nextKey = Console.ReadKey (true);
					while (nextKey.Key != ConsoleKey.Enter)
					{
						if (nextKey.Key == ConsoleKey.Backspace)
						{
							if (password.Length > 0)
							{
								password.RemoveAt (password.Length - 1);

								// erase the last * as well
								Console.Write (nextKey.KeyChar);
								Console.Write (" ");
								Console.Write (nextKey.KeyChar);
							}
						}
						else
						{
							password.AppendChar (nextKey.KeyChar);
							Console.Write (PwdChar);
						}

						nextKey = Console.ReadKey (true);
					}

					Console.WriteLine ();

					// lock the password down
					password.MakeReadOnly ();
					return password;
				}
			}



			internal static string CreateHSplitter ( int iWidth = constants.C_DEFAULT_CONSOLE_WIDTH_1 ) => new ('=', iWidth);



			internal static string CreateArgsText ( IEnumerable<(string Key, string Value)> ArgsAndDescriptions, int MaxWidth = constants.C_DEFAULT_CONSOLE_WIDTH )
			{
				if (null == ArgsAndDescriptions || !ArgsAndDescriptions.Any ())
				{
					throw new ArgumentException (nameof (ArgsAndDescriptions));
				}

				var sb = new StringBuilder ();
				int iMaxKeyLenght = ( from T in ArgsAndDescriptions select T.Key.Length ).Max ();

				iMaxKeyLenght += 1;
				var aTabbed = ArgsAndDescriptions
					.Select (T => new { Key = ( T.Key.PadRight (iMaxKeyLenght) + "-" ), Value = ( T.Value ) });

				aTabbed.eForEach (T =>
				{
					sb.Append (T.Key);
					string sDescr = T.Value;
					int iMaxDescrLenght = MaxWidth - iMaxKeyLenght - 2;
					var aLines = sDescr.eConsole_SplitStringToFixedWidth (iMaxDescrLenght);
					if (aLines.Any ())
					{
						sb.AppendLine (aLines.First ());
						if (aLines.Length > 1)
						{
							var aRows = aLines.Except (aLines.Take (1));
							aRows.eForEach (S =>
							{
								string sRow = new string (' ', iMaxKeyLenght + 1) + S;
								sb.AppendLine (sRow);
							});
						}
					}
				});
				return sb.ToString ();
			}


			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool Console_AskInputIsYes ( this string sMessage, string Suffix = " (y/n)?: ", string YesAnswer = "y" )
			{
				lock (_ConsoleLock)
				{
					Console.Write (sMessage + Suffix);
					var sInput = Console.ReadLine ();
					return ( sInput.IsNotNullOrWhiteSpace () && ( sInput!.ToLower () == YesAnswer.ToLower () ) );
				}
			}

			[MethodImpl (MethodImplOptions.AggressiveInlining)]
			public static bool Console_AskKeyIsYes ( this string sMessage )
			{
				lock (_ConsoleLock)
				{
					sMessage += " (y/n)?: ";
					Console.Write (sMessage);
					var kKey = Console.ReadKey ();
					Console.WriteLine ("");
					if (kKey.Key == ConsoleKey.Y)
					{
						return true;
					}
				}

				return false;
			}

			public static bool Console_IsKeyPressed ( ConsoleKey KK = ConsoleKey.Y )
			{
				lock (_ConsoleLock)
				{
					if (Console.KeyAvailable)
					{
						if (Console.ReadKey ().Key == KK)
						{
							return true;
						}
					}
				}

				return false;
			}


		}

#endif

	}

}

#pragma warning restore IDE1006 // Naming Styles






#if !NET
namespace System.Runtime.CompilerServices
{
	/// <summary>
	/// Позволяет захватывать выражения, переданные методу.
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	internal sealed class CallerArgumentExpressionAttribute : Attribute
	{
		/// <summary>
		/// Инициализирует новый экземпляр <see cref="T:System.Runtime.CompilerServices.CallerArgumentExpressionAttribute" /> class.
		/// </summary>
		/// <param name="parameterName">Имя целевого параметра.</param>
		public CallerArgumentExpressionAttribute(string parameterName) => this.ParameterName = parameterName;

		/// <summary>Получает имя целевого параметра <c>CallerArgumentExpression</c>.</summary>
		/// <returns>Имя целевого параметра <c>CallerArgumentExpression</c>.</returns>
		public string ParameterName { get; }
	}
}
#endif
