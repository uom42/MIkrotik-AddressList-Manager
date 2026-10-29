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

global using uom.AutoDisposable;
global using uom.Extensions;

global using static uom.constants;
global using static uom.Extensions.Extensions_DebugAndErrors;

using System.Buffers;
using System.Buffers.Text;
using System.CodeDom;
using System.Collections.Concurrent;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO.Compression;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Transactions;
using System.Xml.Serialization;
//<PackageReference Include="Microsoft.Extensions.Logging" Version="10.0.3" />
//<PackageReference Include="Microsoft.Extensions.Logging.Console" Version="10.0.3" />
using Microsoft.Extensions.Logging;

using Aes = System.Security.Cryptography.Aes;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Task = System.Threading.Tasks.Task;



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
				if (ae.InnerExceptions  is not null )
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
	})).runAsync();
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



#region Unsafe
/*
public override unsafe byte[] WriteStructsToArray()
{
	var headerSize = MarshalEx.SizeOf<RawInputHeader> ();
	var mouseSize = MarshalEx.SizeOf<RawKeyboard> ();
	var bytes = new byte[ headerSize + mouseSize ];

	fixed (byte* bytesPtr = bytes)
	{
		*(RawInputHeader*) bytesPtr = Header;
		*(RawKeyboard*) ( bytesPtr + headerSize ) = Keyboard;
	}

	return bytes;
}
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


#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
#pragma warning disable IDE1006 // Naming Styles


// <summary>Commnon Tools For Net Multiplatform (C) UOM 2000-2026 </summary>
namespace uom
{


	/// <summary>Constants</summary>
	internal static partial class constants
	{

		internal const string
			C_PLATFORM_WINDOWS = "windows"
			;

		// XPerY=n means that n Xs can be stored in 1 Y.
		internal const int
			BITS_PER_INT32 = 32,
			BITS_PER_BYTE = 8,
			BIT_SHIFT_PER_INT32 = 5,
			BIT_SHIFT_PER_BYTE = 3,
			BIT_SHIFT_FOR_BYTES_PER_INT32 = 2;

		internal const byte FIRST_READABLE_CHAR = 32;


		internal const char
			CHAR_NULL = '\0',
			CHAR_CR = '\r',
			CHAR_LF = '\n',
			CHAR_TAB = '\t',

			vbNullChar = CHAR_NULL,
			vbCr = CHAR_CR,
			vbLf = CHAR_LF,
			vbTab = CHAR_TAB,

			CHAR_EQUAL = '=',
			CHAR_SLASH = '\\',
			CHAR_SPACE = ' ',
			CHAR_QUOTE = '"'
			;


		internal const string
			vbCrLf = "\r\n",
			vbCrLf2 = vbCrLf + vbCrLf,
			vbCrCrLf = "\r" + vbCrLf,
			SLASH_SLASH = @"\\",
			QUOTE = "\"",
			QUOTES_EMPTY = "''",

			NULL_TEXT = "NULL"
			;



		internal static readonly string
			CS_QUOTE2 = QUOTE_X(2),
			CS_QUOTE4 = QUOTE_X(4);

		internal static string QUOTE_X ( int x ) => new(CHAR_QUOTE , x);


		internal static readonly IntPtr HANDLE_INVALID = new(-1);


		internal const float C_MM_IN_INCH = 25.4f;
		internal const float C_CM_IN_INCH = C_MM_IN_INCH / 10f;


		internal static class GlobalizationConst
		{

			public const string
				OK = "OK",
				NONE_RU = "[нет]",
				READY_RU = "Готово.",

				YES_EN = "Yes",
				NO_EN = "No",

				YES_RU = "Да",
				NO_RU = "Нет";


			public const string
				ALPHABET_EN = @"abcdefghijklmnopqrstuvwxyz",
				ALPHABET_EN_QWERTY = @"qwertyuiopasdfghjklzxcvbnm",

				ALPHABET_RU = @"абвгдежзийклмнопрстуфхцчшщъыьэюя",
				ALPHABET_RU_QWERTY = @"ёйцукенгшщзхъфывапролджэячсмитьбю",

				ALPHABET_NUM = @"0123456789";


			public const string
			   ERR_FAILED_TO_RUN_RU = "Не удалось выполнить!",
			   ERR_FAILED_TO_RUN_EN = "Failed to execute!";


			public const string
			   RX_PATTERN_ALPHABET_CYRILLIC = @"\p{IsCyrillic}", // это блок кириллицы в Unicode. 
			   RX_PATTERN_ALPHABET_LAT = @"\p{IsBasicLatin}"; // Для английского (en-US) это будет \p{IsBasicLatin}

		}





		internal static partial class IOConst
		{



			/// <summary>Для файлового ввода-вывода префикс «\\?\» указывает API-интерфейсам Windows отключить весь синтаксический анализ строки и отправить строку, 
			/// которая следует за ней, прямо в файловую систему.<br/>
			/// Это позволяет использовать <c>.</c> или <c>..</c> в именах путей, а также ослабить ограничение в 
			/// 260 символов для имени пути, если базовая файловая система поддерживает длинные пути и имена файлов
			/// (<see cref="https://docs.microsoft.com/ru-ru/windows/win32/fileio/naming-a-file?redirectedfrom=MSDN"/>). <br/><br/>
			/// Так как он отключает автоматическое расширение строки пути, \\префикс "?\" также позволяет использовать ".." и "." в именах путей, <br/>
			/// которые могут быть полезны при попытке выполнить операции с файлом, в противном случае зарезервированные описатели относительных путей в составе полного пути.<br/>
			/// <br/>
			/// Многие, но не все API-интерфейсы файлового ввода/вывода поддерживают <c>\\?\</c>; для проверки каждого API следует обратиться к справочному разделу.<br/>
			/// </summary>
			internal const string CS_PATH_PREFIX_WIN_LONG_PATH = @"\\?\";

			/// <summary> при работе с функциями API Windows следует использовать \\.\ для доступа только к устройствам, а не файлам!.
			/// Большинство интерфейсов API не поддерживают "\\.\";
			/// только те, которые предназначены для работы с пространством имен устройства, распознают его.
			/// Всегда проверяйте справочный раздел для каждого API, чтобы быть уверенным в этом.
			/// </summary>
			internal const string CS_PATH_PREFIX_WIN_DEVICE = @"\\.\";//" \\.\COM56 etc



			internal const string
				FILE_EXTENSION_TMP = ".tmp",
				FILE_EXTENSION_ZIP = ".zip",
				FILE_EXTENSION_XLS = ".xls",
				FILE_EXTENSION_XLSX = ".xlsx",
				FILE_EXTENSION_CSV = ".csv",
				FILE_EXTENSION_BAK = ".bak",
				FILE_EXTENSION_TXT = ".txt";



			internal static readonly string[]
				BYTE_SIZE_EN = [ "B" , "KB" , "MB" , "GB" , "TB" , "PB" , "EB" ],
				BYTE_SIZE_RU = [ "Б" , "КБ" , "МБ" , "ГБ" , "ТБ" , "ПБ" , "ЕБ" ];

			internal const ulong
				BYTES_IN_KBYTE = 1024UL,
				BYTES_IN_MBYTE = 1048576UL,
				BYTES_IN_GBYTE = 1073741824UL;

		}


		internal static partial class FormatConst
		{


			internal const string BAD_BYTE_SEPARATOR_CHARS = @" :_|./*+,\~`'=";


			internal const string
				DATETIME_YEAR = "yyyy",
				DATETIME_MONTH_NUMBER = "MM",
				DATETIME_MONTH_NAME_SHORT = "MMM",
				DATETIME_MONTH_NAME_LONG = "MMMM",
				DATETIME_DAY = "dd",
				DATETIME_HOUR = "HH",
				DATETIME_MINUTE = "mm",
				DATETIME_SECONDS = "ss",
				DATETIME_SECONDS_FRACTION = "ffff",

				DATETIME_FORMAT_LONG_TIME = "HH:mm:ss",
				DATETIME_FORMAT_LONG_TIMESTAMP = $"{DATETIME_FORMAT_LONG_TIME}.fff",

				DATETIME_FORMAT_LONG = $"dd {DATETIME_MONTH_NAME_LONG} {DATETIME_YEAR}, HH:mm:ss",
				DATETIME_FORMAT_LONG_DATE_TIME = $"dd-{DATETIME_MONTH_NUMBER}-{DATETIME_YEAR}, " + DATETIME_FORMAT_LONG_TIME,
				DATETIME_FORMAT_LONG_DATE_TIMESTAMP = $"dd-{DATETIME_MONTH_NUMBER}-{DATETIME_YEAR}, " + DATETIME_FORMAT_LONG_TIMESTAMP,
				DATETIME_FORMAT_LONG_FILE_DATE_TIMESTAMP = $"{DATETIME_YEAR}-{DATETIME_MONTH_NUMBER}-dd__HH-mm-ss-fff";


			internal const string FORMAT_LONGNUMBER = "### ### ### ### ### ### ### ### ### ### ### ### ### ### ##0";


			/// <summary>Used in any formating functions for format decimal numbers</summary>
			internal const int DEFAULT_DECIMAL_DIGITS = 2;


			internal const NumberStyles DEFAULT_PARSE_NUMBER_STYLE = NumberStyles.Any;



		}


		internal static partial class ConsoleConst
		{



			internal const int
				CONSOLE_WIDTH_DEFAULT = 80,
				CONSOLE_WIDTH_DEFAULT_1 = CONSOLE_WIDTH_DEFAULT - 1;

			internal const string
				SEPARATOR_10 = "==========",
				SEPARATOR_70 = SEPARATOR_10 + SEPARATOR_10 + SEPARATOR_10 + SEPARATOR_10 + SEPARATOR_10 + SEPARATOR_10 + SEPARATOR_10;


			internal const int PROGRESS_BAR_SIZE_DEFAULT = 40;

			internal const char
				PROGRESSBAR_EMPTY = '_',
				PROGRESSBAR_FULL = '#';

			/// <summary>░</summary>
			internal const char PROGRESSBAR_EMPTY_ASCII = ASCII.ASCII_176;
			/// <summary>▓</summary>
			internal const char PROGRESSBAR_FULL_ASCII = ASCII.CC_ASCII_178;

			internal const string
				ENTER_PWD = "Enter password: ",
				DOWNLOADING = "Downloading:",
				FROM = "from";






			/// <summary>ASCII Pseudorgaphics https://www.asciitable.com</summary>
			internal static partial class ASCII
			{

				/// <summary>░</summary>
				internal const char ASCII_176 = '░';

				/// <summary>▒</summary>
				internal const char CC_ASCII_177 = '▒';

				/// <summary>▓</summary>
				internal const char CC_ASCII_178 = '▓';

				/// <summary>█</summary>
				internal const char CC_ASCII_219 = '█';

			}



			internal static partial class ANSIEscape
			{

				internal const char ESCAPE = '\e';

				/// <summary>
				/// Reset or normal	All attributes become turned off
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string DEFAULT = "\e[0m"; // "\u001b[0m"

				/// <summary>
				/// Bold or increased intensity	As with faint, the color change is a PC (SCO / CGA) invention
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string BOLD_START = "\e[1m";

				/// <summary>
				/// Faint, decreased intensity, or dim	May be implemented as a light font weight like bold.[27]
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string FAINT_START = "\e[2m";

				/// <summary>
				/// Italic	Not widely supported. Sometimes treated as inverse or blink.
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string
					ITALIC_START = "\e[3m",
					ITALIC_END = "\e[24m";

				/// <summary>
				/// Underline	Style extensions exist for Kitty, VTE, mintty, iTerm2 and Konsole.
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string
					UNDERLINE_START = "\e[4m",
					UNDERLINE_END = "\e[24m";

				/// <summary>
				/// Double-underline per ECMA-48,[16]: 8.3.117  but instead disables bold intensity on several terminals, including in the Linux kernel's console before version 4.17.[31]
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string
					UNDERLINE_DOUBLY_START = "\e[21m";

				/// <summary>
				/// Slow blink	Sets blinking to less than 150 times per minute
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string BLINK_SLOW_START = "\e[5m";

				/// <summary>
				/// Rapid blink	MS-DOS ANSI.SYS, 150+ per minute; not widely supported
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string BLINK_RAPID_START = "\e[6m";


				/// <summary>                
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				/// </summary>
				internal const string FONT_PRIMARY = "\e[10m";

				/// <summary>                
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				/// </summary>
				internal const string FONT_GOTHIC = "\e[20m";

				/// <summary>                
				/// Select alternative font n − 10
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string FONT_ALTERNATIVE_FORMAT = "\e[{0}m";


				/// <summary>
				/// Turn blinking off
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string BLINK_END = "\e[25m";


				/// <summary>
				/// Reverse video or invert	Swap foreground and background colors
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string
					INVERT_START = "\e[7m",
					INVERT_END = "\e[27m";


				/// <summary>
				/// Normal intensity	Neither bold nor faint; color changes where intensity is implemented as such.
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string NORMAL_INTENSITY = "\e[22m";



				internal enum ConsoleColorsANSI : uint
				{
					Black = 30,
					Red = 31,
					Green = 32,
					Yellow = 33,
					Blue = 34,
					Magenta = 35,
					Cyan = 36,
					White = 37,
					BrightBlack_Gray = 90,
					BrightRed = 91,
					BrightGreen = 92,
					BrightYellow = 93,
					BrightBlue = 94,
					BrightMagenta = 95,
					BrightCyan = 96,
					BrightWhite = 97,
				}


				internal const string COLOR_FOREGROUND_BLACK_START = "\e[30m";
				internal const string COLOR_FOREGROUND_RED_START = "\e[31m";
				internal const string COLOR_FOREGROUND_GREEN_START = "\e[32m";
				internal const string COLOR_FOREGROUND_YELLOW_START = "\e[33m";
				internal const string COLOR_FOREGROUND_BLUE_START = "\e[34m";
				internal const string COLOR_FOREGROUND_MAGENTA_START = "\e[35m";
				internal const string COLOR_FOREGROUND_CYAN_START = "\e[36m";
				internal const string COLOR_FOREGROUND_WHITE_START = "\e[37m";

				/// <summary>Gray</summary>
				internal const string COLOR_FOREGROUND_BRIGHT_BLACK_START = "\e[90m";
				internal const string COLOR_FOREGROUND_BRIGHT_RED_START = "\e[91m";
				internal const string COLOR_FOREGROUND_BRIGHT_GREEN_START = "\e[92m";
				internal const string COLOR_FOREGROUND_BRIGHT_YELLOW_START = "\e[93m";
				internal const string COLOR_FOREGROUND_BRIGHT_BLUE_START = "\e[94m";
				internal const string COLOR_FOREGROUND_BRIGHT_MAGENTA_START = "\e[95m";
				internal const string COLOR_FOREGROUND_BRIGHT_CYAN_START = "\e[96m";
				internal const string COLOR_FOREGROUND_BRIGHT_WHITE_START = "\e[97m";


				internal const string COLOR_BACKGROUND_BLACK_START = "\e[40m";
				internal const string COLOR_BACKGROUND_RED_START = "\e[41m";
				internal const string COLOR_BACKGROUND_GREEN_START = "\e[42m";
				internal const string COLOR_BACKGROUND_YELLOW_START = "\e[43m";
				internal const string COLOR_BACKGROUND_BLUE_START = "\e[44m";
				internal const string COLOR_BACKGROUND_MAGENTA_START = "\e[45m";
				internal const string COLOR_BACKGROUND_CYAN_START = "\e[46m";
				internal const string COLOR_BACKGROUND_WHITE_START = "\e[47m";

				/// <summary>Gray</summary>
				internal const string COLOR_BACKGROUND_BRIGHT_BLACK_START = "\e[100m";
				internal const string COLOR_BACKGROUND_BRIGHT_RED_START = "\e[101m";
				internal const string COLOR_BACKGROUND_BRIGHT_GREEN_START = "\e[102m";
				internal const string COLOR_BACKGROUND_BRIGHT_YELLOW_START = "\e[103m";
				internal const string COLOR_BACKGROUND_BRIGHT_BLUE_START = "\e[104m";
				internal const string COLOR_BACKGROUND_BRIGHT_MAGENTA_START = "\e[105m";
				internal const string COLOR_BACKGROUND_BRIGHT_CYAN_START = "\e[106m";
				internal const string COLOR_BACKGROUND_BRIGHT_WHITE_START = "\e[107m";


				/// <summary>                
				/// Set foreground color (30–37, 90-97)
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string COLOR_FOREGROUND_FORMAT_START = "\e[{0}m";

				/// <summary>                
				/// Set foreground color (40–77, 100-107)
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string COLOR_BACKGROUND_FORMAT_START = "\e[{0}m";


				/// <summary>                
				/// Set foreground color	Next arguments are 5;n or 2;r;g;b
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string COLOR_FOREGROUND_RGB_FORMAT_START = "\e[38;2;{0};{1};{2}m";

				/// <summary>                
				/// Set background color	Next arguments are 5;n or 2;r;g;b
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string COLOR_BACKGROUND_RGB_FORMAT_START = "\e[48;2;{0};{1};{2}m";

				/// <summary>                
				/// Default background color	Implementation defined (according to standard)
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string COLOR_BACKGROUND_DEFAULT = "\e[49m";


				/// <summary>                
				/// Set underline color	Not in standard; implemented in Kitty, VTE, mintty, and iTerm2.[28][29] Next arguments are 5;n or 2;r;g;b.
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string COLOR_UNDERLINE_RGB_FORMAT_START = "\e[58;2;{0};{1};{2}m";

				/// <summary>                
				/// 	Default underline color	Not in standard; implemented in Kitty, VTE, mintty, and iTerm2.[28][29]
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string COLOR_UNDERLINE_DEFAULT = "\e[59m";




				/// <summary>                
				/// 73	Superscript
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string SCRIPT_SUPER_START = "\e[73m";

				/// <summary>                
				/// 74	Subscript
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string SCRIPT_SUB_START = "\e[74m";


				/// <summary>                
				/// 75	Neither superscript nor subscript
				/// </summary>
				/// <see cref="https://en.wikipedia.org/wiki/ANSI_escape_code"/>
				internal const string SCRIPT_END = "\e[75m";



				public enum ConsoleBlinkModes : uint
				{
					None,
					Slow,
					Rapid
				}

				public enum ConsoleUnderlineModes : uint
				{
					None,
					Single,
					Double
				}




			}


		}


		internal static partial class ErrorsConst
		{


		}


		public enum CHAR_MODES : int
		{
			Auto = 0,
			Uni,
			Ansi
		}


		internal enum CHANGE_CASE_MODES : int
		{
			TO_LOWER,
			TO_UPPER,
			TO_TITLE
		}

		private static readonly Lazy<char> _systemDefaultHexByteSeparator = new(BitConverter.ToString([ 0x1 , 0x2 ])[ 2 ]);
		/// <summary>Standart system byte seperator char</summary>
		internal static char SystemHexByteSeparator => _systemDefaultHexByteSeparator.Value;


		// Поиск в многострочном тексте
		internal const RegexOptions
			C_REGEXP_FLAGS_IgnoreCase_IgnorePatternWhitespace_Singleline = RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.CultureInvariant,

			// Поиск в многострочном тексте, в каждой строке по-отдельности
			C_REGEXP_FLAGS_IgnoreCase_IgnorePatternWhitespace_Multiline = RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.CultureInvariant,

			// Поиск в многострочном тексте, и в каждой строке по-отдельности
			C_REGEXP_FLAGS_IgnoreCase_IgnorePatternWhitespace_Multiline_Singleline = RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.CultureInvariant;




		//public static UOM.Globalization_.mGlobalization.CountryPhoneFormatAttribute ExtGlb_GetCountryPhoneFormat(this UOM.Globalization_.mGlobalization.ECounties CNT)
		//{
		//    return UOM.Globalization_.mGlobalization.CountryPhoneFormatAttribute.GetCountryPhoneFormat(CNT);
		//}

		///// <summary>Выделяет из строки номер телефона</summary>
		//
		//public static string ExtGlb_PhoneParse(this string sPhone, UOM.Globalization_.mGlobalization.ECounties CNT = UOM.Globalization_.mGlobalization.ECounties._UNKNOWN)
		//{
		//    return UOM.Globalization_.mGlobalization.PhoneParse(sPhone, CNT);
		//}




		//BINDING_FLAGS

		internal const BindingFlags
			BINDING_FLAGS_PUBLIC = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy,
			BINDING_FLAGS_NONPUBLIC = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.FlattenHierarchy,
			BINDING_FLAGS_PUBLIC_NONPUBLIC = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.FlattenHierarchy,
			BINDING_FLAGS_PROPERTIES_PUBLIC = BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.DeclaredOnly;


		[Flags]
		public enum MemberTypes : int
		{
			Property = 1,
			Field = 2,
			Method = 4
		}

		internal enum PROPERTY_LIST_SOURCES : int
		{
			/// <summary>Take property list from Target object</summary>
			Target = 0,
			/// <summary>Take property list from Source object</summary>
			Source
		}



	}


	#region Structures _Int16/32/64

	[StructLayout(LayoutKind.Explicit , Pack = 1 , Size = 2)]
	internal partial struct Int16_
	{
		[FieldOffset(0)][MarshalAs(UnmanagedType.I4)] public short Word = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.U4)] public ushort UWord = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.U1)] public byte LoByte = 0;
		[FieldOffset(1)][MarshalAs(UnmanagedType.U1)] public byte HiByte = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.U1)] public byte Byte_0 = 0;
		[FieldOffset(1)][MarshalAs(UnmanagedType.U1)] public byte Byte_1 = 0;

		[FieldOffset(0)][MarshalAs(UnmanagedType.U1)] public unsafe fixed byte BytesUnsafe[ 2 ];

		#region CORE

		public Int16_ ( short V ) => Word = V;
		public Int16_ ( ushort V ) => UWord = V;

		public byte[] Bytes
		{
			readonly get => [ Byte_0 , Byte_1 ];
			set
			{
				if ( value.Length != 2 )
					throw new ArgumentException($"{nameof(Int16_)} Constructor failed! byteArray lenght is wrong!");

				(Byte_0 , Byte_1) = (value[ 0 ] , value[ 1 ]);
			}
		}

		#endregion

		public override string ToString () => $"{UWord} ({Bytes.toStringHex(true)})";

		#region Conversions

		public static implicit operator ushort ( Int16_ L ) => L.UWord;
		public static implicit operator short ( Int16_ L ) => L.Word;
		public static implicit operator Int16_ ( ushort L ) => new(L);
		public static implicit operator Int16_ ( short L ) => new(L);
		public static implicit operator Int16_ ( byte I ) => new(0) { LoByte = I };
		public static implicit operator Int16_ ( byte[] ab ) => new() { Bytes = ab };
		public static implicit operator Int16_ ( Span<byte> span ) => (Int16_)span[ 0..2 ].ToArray();


		#endregion

		#region Operators

		public static bool operator < ( Int16_ I1 , Int16_ I2 ) => I1.UWord < I2.UWord;
		public static bool operator > ( Int16_ I1 , Int16_ I2 ) => I1.UWord > I2.UWord;
		public static bool operator == ( Int16_ I1 , Int16_ I2 ) => I1.UWord == I2.UWord;
		public static bool operator != ( Int16_ I1 , Int16_ I2 ) => I1.UWord != I2.UWord;
		public static bool operator < ( Int16_ I1 , ushort I2 ) => I1.UWord < I2;
		public static bool operator > ( Int16_ I1 , ushort I2 ) => I1.UWord > I2;
		public static bool operator == ( Int16_ I1 , ushort I2 ) => I1.UWord == I2;
		public static bool operator != ( Int16_ I1 , ushort I2 ) => I1.UWord != I2;
		public static Int16_ operator - ( Int16_ I1 , Int16_ I2 ) => new((ushort)(I1.UWord - I2.UWord));
		public static Int16_ operator - ( Int16_ I1 , ushort I2 ) => new((ushort)(I1.UWord - I2));
		public static Int16_ operator + ( Int16_ I1 , Int16_ I2 ) => new((ushort)(I1.UWord + I2.UWord));
		public static Int16_ operator + ( Int16_ I1 , ushort I2 ) => new((ushort)(I1.UWord + I2));

		#endregion

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
		public override readonly bool Equals ( object obj ) => this == (Int16_)obj;
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

		public override int GetHashCode () => UWord.GetHashCode();

	}


	[StructLayout(LayoutKind.Explicit , Pack = 1 , Size = 4)]
	internal partial struct Int32_
	{
		[FieldOffset(0)][MarshalAs(UnmanagedType.I4)] public int DWord = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.U4)] public uint UDWord = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.I2)] public short LoWord = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.U2)] public ushort ULoWord = 0;
		[FieldOffset(2)][MarshalAs(UnmanagedType.I2)] public short HiWord = 0;
		[FieldOffset(2)][MarshalAs(UnmanagedType.U2)] public ushort UHiWord = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.LPStruct)] public Int16_ LoWord16 = new(0);
		[FieldOffset(2)][MarshalAs(UnmanagedType.LPStruct)] public Int16_ HiWord16 = new(0);
		[FieldOffset(0)][MarshalAs(UnmanagedType.U1)] public byte Byte_0 = 0;
		[FieldOffset(1)][MarshalAs(UnmanagedType.U1)] public byte Byte_1 = 0;
		[FieldOffset(2)][MarshalAs(UnmanagedType.U1)] public byte Byte_2 = 0;
		[FieldOffset(3)][MarshalAs(UnmanagedType.U1)] public byte Byte_3 = 0;

		[FieldOffset(0)][MarshalAs(UnmanagedType.U1)] public unsafe fixed byte BytesUnsafe[ 4 ];

		#region CORE

		public Int32_ ( int V ) { DWord = V; }
		public Int32_ ( uint V ) { UDWord = V; }


		public byte[] Bytes
		{
			readonly get => [ Byte_0 , Byte_1 , Byte_2 , Byte_3 ];
			set
			{
				if ( value.Length != 4 )
					throw new ArgumentException($"{nameof(Int32_)} Constructor failed! byteArray lenght is wrong!");


				(Byte_0 , Byte_1 , Byte_2 , Byte_3) = (value[ 0 ] , value[ 1 ] , value[ 2 ] , value[ 3 ]);
			}
		}


		#endregion
		public override string ToString () => $"{UDWord} / {Bytes.toStringHex(true)}";

		#region Conversions
		public static implicit operator uint ( Int32_ L ) => L.UDWord;
		public static implicit operator int ( Int32_ L ) => L.DWord;

		public static implicit operator Int32_ ( uint L ) => new(L);
		public static implicit operator Int32_ ( int L ) => new(L);
		public static implicit operator Int32_ ( short I ) => new(0) { LoWord = I };
		public static implicit operator Int32_ ( ushort I ) => new(0) { ULoWord = I };
		public static implicit operator Int32_ ( byte[] ab ) => new() { Bytes = ab };
		#endregion

		#region Operators
		public static bool operator < ( Int32_ I1 , Int32_ I2 ) => I1.UDWord < I2.UDWord;
		public static bool operator > ( Int32_ I1 , Int32_ I2 ) => I1.UDWord > I2.UDWord;
		public static bool operator == ( Int32_ I1 , Int32_ I2 ) => I1.UDWord == I2.UDWord;
		public static bool operator != ( Int32_ I1 , Int32_ I2 ) => I1.UDWord != I2.UDWord;
		public static bool operator < ( Int32_ I1 , uint I2 ) => I1.UDWord < I2;
		public static bool operator > ( Int32_ I1 , uint I2 ) => I1.UDWord > I2;
		public static bool operator == ( Int32_ I1 , uint I2 ) => I1.UDWord == I2;
		public static bool operator != ( Int32_ I1 , uint I2 ) => I1.UDWord != I2;
		public static Int32_ operator - ( Int32_ I1 , Int32_ I2 ) => new(I1.UDWord - I2.UDWord);
		public static Int32_ operator - ( Int32_ I1 , uint I2 ) => new(I1.UDWord - I2);
		public static Int32_ operator + ( Int32_ I1 , Int32_ I2 ) => new(I1.UDWord + I2.UDWord);
		public static Int32_ operator + ( Int32_ I1 , uint I2 ) => new(I1.UDWord + I2);
		#endregion

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
		public override readonly bool Equals ( object obj ) => this == (Int32_)obj;
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

		public override readonly int GetHashCode () => UDWord.GetHashCode();
	}

	[StructLayout(LayoutKind.Explicit , Pack = 1 , Size = 8)]
	internal partial struct Int64_
	{
		[FieldOffset(0)][MarshalAs(UnmanagedType.I8)] public long QWord = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.U8)] public ulong UQWord = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.I4)] public int LoDWord = 0;
		[FieldOffset(4)][MarshalAs(UnmanagedType.I4)] public int HiDWord = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.U4)] public uint ULoDWord = 0;
		[FieldOffset(4)][MarshalAs(UnmanagedType.U4)] public uint UHiDWord = 0;
		[FieldOffset(0)][MarshalAs(UnmanagedType.LPStruct)] public Int32_ LoDWord32 = new(0);
		[FieldOffset(4)][MarshalAs(UnmanagedType.LPStruct)] public Int32_ HiDWord32 = new(0);
		[FieldOffset(0)][MarshalAs(UnmanagedType.U1)] public byte Byte_0 = 0;
		[FieldOffset(1)][MarshalAs(UnmanagedType.U1)] public byte Byte_1 = 0;
		[FieldOffset(2)][MarshalAs(UnmanagedType.U1)] public byte Byte_2 = 0;
		[FieldOffset(3)][MarshalAs(UnmanagedType.U1)] public byte Byte_3 = 0;
		[FieldOffset(4)][MarshalAs(UnmanagedType.U1)] public byte Byte_4 = 0;
		[FieldOffset(5)][MarshalAs(UnmanagedType.U1)] public byte Byte_5 = 0;
		[FieldOffset(6)][MarshalAs(UnmanagedType.U1)] public byte Byte_6 = 0;
		[FieldOffset(7)][MarshalAs(UnmanagedType.U1)] public byte Byte_7 = 0;

		[FieldOffset(0)][MarshalAs(UnmanagedType.U1)] public unsafe fixed byte BytesUnsafe[ 8 ];

		#region CORE

		public Int64_ ( long V ) => QWord = V;
		public Int64_ ( ulong V ) => UQWord = V;



		public byte[] Bytes
		{
			readonly get => [ Byte_0 , Byte_1 , Byte_2 , Byte_3 , Byte_4 , Byte_5 , Byte_6 , Byte_7 ];
			set
			{
				if ( value.Length != 8 )
					throw new ArgumentException($"{nameof(Int64_)} Constructor failed! byteArray lenght is wrong!");

				(Byte_0 , Byte_1 , Byte_2 , Byte_3 , Byte_4 , Byte_5 , Byte_6 , Byte_7) = (value[ 0 ] , value[ 1 ] , value[ 2 ] , value[ 3 ] , value[ 4 ] , value[ 5 ] , value[ 6 ] , value[ 7 ]);
			}
		}
		#endregion

		public override readonly string ToString () => $"{UQWord} / {Bytes.toStringHex(true)}";

		#region Conversions
		public static implicit operator ulong ( Int64_ L ) => L.UQWord;
		public static implicit operator long ( Int64_ L ) => L.QWord;
		public static implicit operator Int64_ ( ulong L ) => new(L);
		public static implicit operator Int64_ ( long L ) => new(L);
		public static implicit operator Int64_ ( int I ) => new(0L) { LoDWord = I };
		public static implicit operator Int64_ ( uint I ) => new(0L) { ULoDWord = I };
		public static implicit operator Int64_ ( byte[] ab ) => new() { Bytes = ab };
		#endregion

		#region Operators

		public static bool operator < ( Int64_ I1 , Int64_ I2 ) => I1.UQWord < I2.UQWord;
		public static bool operator > ( Int64_ I1 , Int64_ I2 ) => I1.UQWord > I2.UQWord;
		public static bool operator == ( Int64_ I1 , Int64_ I2 ) => I1.UQWord == I2.UQWord;
		public static bool operator != ( Int64_ I1 , Int64_ I2 ) => I1.UQWord != I2.UQWord;
		public static bool operator < ( Int64_ I1 , ulong I2 ) => I1.UQWord < I2;
		public static bool operator > ( Int64_ I1 , ulong I2 ) => I1.UQWord > I2;
		public static bool operator == ( Int64_ I1 , ulong I2 ) => I1.UQWord == I2;
		public static bool operator != ( Int64_ I1 , ulong I2 ) => I1.UQWord != I2;
		public static Int64_ operator - ( Int64_ I1 , Int64_ I2 ) => new(I1.UQWord - I2.UQWord);
		public static Int64_ operator - ( Int64_ I1 , ulong I2 ) => new(I1.UQWord - I2);
		public static Int64_ operator + ( Int64_ I1 , Int64_ I2 ) => new(I1.UQWord + I2.UQWord);
		public static Int64_ operator + ( Int64_ I1 , ulong I2 ) => new(I1.UQWord + I2);
		#endregion

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
		public override readonly bool Equals ( object obj ) => this == (Int64_)obj;
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

		public override readonly int GetHashCode () => UQWord.GetHashCode();

	}


	#endregion







	#region Railway Oriented Result


	//https://www.youtube.com/watch?v=C1oGnDEnS14

	/*
	public class Result0
	{
	//https://www.youtube.com/shorts/txvO1m6F0-s

	public Result ( bool isSucces, Exception? error = null )
	{
	IsSucess = isSucces;
	Error = error;

	if (( isSucces && error  is not null  ) || ( !isSucces && error is null )) throw new Exception ("Invalid error!");
	}

	public bool IsSucess { get; }
	public Exception? Error { get; }

	public bool IsFailure => !IsSucess;

	public static Result Success () => new (true);
	}

	public class Result0<TValue> : Result0
	{

	}
	 */



	internal class ExceptionEventArgs ( Exception ex , string callerName , string callerFile , int callerLine ) : System.EventArgs()
	{
		public readonly Exception Exception = ex;
		public readonly string CallerMemberName = callerName;
		public readonly string CallerFilePath = callerFile;
		public readonly int CallerLineNumber = callerLine;
	}

	internal sealed class Result<T>
	{

		public T? Value { get; }
		public ExceptionEventArgs? ExceptionArgs { get; }

		public bool IsSuccess { get; private set; }


		private Result ( T? value )
		{
			Value = value;
			IsSuccess = true;
		}

		private Result ( ExceptionEventArgs error )
		{
			ExceptionArgs = error;
			IsSuccess = false;
		}


		public bool IsCanceled
			=> !IsSuccess && ExceptionArgs is { Exception: TaskCanceledException };// OperationCanceledException;


		public static Result<T> Success ( T? value ) => new(value);

		public static Result<T> Failure ( ExceptionEventArgs error ) => new(error);

		public static Result<T> Failure ( Exception ex ,
			string callerMemberName = "" ,
			string callerFilePath = "" ,
			int callerLineNumber = 0
			)
			=> Failure(new ExceptionEventArgs(ex , callerMemberName , callerFilePath , callerLineNumber));

		public static Result<T> Canceled ( OperationCanceledException? cex = null , string callerMemberName = "" , string callerFilePath = "" , int callerLineNumber = 0 )
			=> Failure(cex ?? new OperationCanceledException() , callerMemberName , callerFilePath , callerLineNumber);
	}



	/*
	public record Error ( ErrorType Type , string Description )
	{
		public static Error NoLineItems = new(ErrorType.Validation, "Line items are empty");
		public static Error NotEnoughStock = new(ErrorType.Validation, "Not enough stock for order");
		public static Error PaymentFailed = new(ErrorType.Failure, "Failed to process payment");
	}

	public enum ErrorType
	{
	OperationFailure = 0,
	ValidationFailure = 1
}


public record Error ( ErrorType type , string description )
{
	public static Error NoLineItems = new (ErrorType.ValidationFailure, "Line items are empty");
	public static Error NotEnoughStock = new (ErrorType.ValidationFailure, "Not enough stock for order");
	public static Error PaymentFailed = new (ErrorType.OperationFailure, "Failed to process payment");
}



	 */


	#endregion



	/// <summary>Application Tools</summary>
	internal static class OSInfo
	{




		internal static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

		internal static bool IsOSX => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

		internal static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

		internal static bool IsFreeBSD => RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD);

		internal static bool IsUNIX => IsLinux || IsFreeBSD;

		/*
		internal static bool IsWindows2 =>
#if UNIX
				false;
#else
			 true;
#endif
		 */
	}



	/// <summary>Info about application assembly</summary>
	internal static partial class AppInfo
	{


		public static Assembly AppAssembly
			=> Assembly.GetEntryAssembly()
			?? Assembly.GetExecutingAssembly()
			?? typeof(AppInfo).Assembly;

		public static string AppPath
			=> Environment.ProcessPath.ensureNotNullOrWhiteSpace(AppAssembly.Location);

		/*
				[UnconditionalSuppressMessage("SingleFile" , "IL3000:Avoid accessing Assembly file path when publishing as a single file" , Justification = "<Pending>")]
				[Obsolete("IL3000:Avoid accessing Assembly file path when publishing as a single file!" , false)]
				public static FileInfo? AssemblyLocation
					=> AppAssembly.Location.pathToFileInfo();

		 */


		public static string? Title
#if WINDOWS
			=> ProductName;
#else
			//#if UWP || ANDROID
			=> AppAssembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;
#endif


		public static string? CompanyName
#if WINDOWS && !MAUI
			=> Application.CompanyName;
#else
			=> AppAssembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;
#endif

		public static string StartupPath
#if WINDOWS && !MAUI
			=> System.Windows.Forms.Application.StartupPath;
#else
		=> new FileInfo(AppAssembly.Location).DirectoryName!;
#endif

		public static string? ProductName
#if WINDOWS && !MAUI
			=> Application.ProductName;
#else
					=> Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
#endif

		public static string? ProductVersion
#if WINDOWS && !MAUI
			=> Application.ProductVersion;
#else
			=> AppInfo.AssemblyFileVersionAttribute // . AppAssembly.f AssemblyFil VersionInfo?.FileVersion
				?? @AssemblyVersionAttributeValue;
		//@AssemblyVersionAttribute. . .pro FileVersionInfo.GetVersionInfo(AppInfo.AppAssembly). .VersionInfo?.FileVersion.ProductVersion;


#endif


		public static string? AssemblyTitle => AppAssembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;


		public static string? Description => AppAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
		public static string? Copyright => AppAssembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;
		public static string? Trademark => AppAssembly.GetCustomAttribute<AssemblyTrademarkAttribute>()?.Trademark;

		private static AssemblyVersionAttribute? @AssemblyVersionAttribute => AppAssembly.GetCustomAttribute<AssemblyVersionAttribute>();
		public static string? @AssemblyVersionAttributeValue => @AssemblyVersionAttribute?.Version;




		public static AssemblyName @AssemblyName => AppAssembly.GetName();
		public static Version? @AssemblyVersion => AssemblyName.Version;


		public static string? Comments => AssemblyFileVersionInfo?.Comments;


		/// <summary>ProductName + ProductVersion</summary>
		internal static string AppProductNameAndVersion
			=> ProductName + " " + ProductVersion;



		//public static FileInfo File => new(AppAssembly.Location);
		public static FileVersionInfo AssemblyFileVersionInfo => FileVersionInfo.GetVersionInfo(AppAssembly.Location);



		public static string? @AssemblyFileVersionAttribute => AppAssembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;


		////public static string? FileVersion() => FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).FileVersion;
		//public static string? FileVersion()
		//{
		//    var FVI = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
		//    return FVI.FileVersion;
		//}
		public static string? Configuration => AppAssembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration;
		//public static uint? AlgorithmId => AppAssembly.GetCustomAttribute<AssemblyAlgorithmIdAttribute>()?.AlgorithmId;
		//public static string? Culture => AppAssembly.GetCustomAttribute<AssemblyCultureAttribute>()?.Culture;
		//public static AssemblyNameFlags? Flags => (AssemblyNameFlags?)(AppAssembly.GetCustomAttribute<AssemblyFlagsAttribute>()?.AssemblyFlags);




		public static T? GetAssemblyAttribute<T> ( Assembly asmbl ) where T : Attribute
		{
			var attributes = asmbl.GetCustomAttributes(typeof(T) , true);
			return (null == attributes || attributes!.Length == 0) ? null : (T)attributes.First();
		}

		public static T? GetAssemblyAttribute<T> () where T : Attribute
			=> GetAssemblyAttribute<T>(AppAssembly);

#if WINDOWS

		/// <summary>Gets dir like: 'C:\Users\user\AppData\Roaming_or_Local\company\product\AssemblyName' (no version number included)'</summary>
		public static DirectoryInfo UserAppDataPath ( bool roaming , bool createIfNotExist = true , string company = "" , string product = "" , bool useAssemblyName = true , string assemblymName = "" )
		{
			company = company.ensureNotNullOrWhiteSpace(AppInfo.CompanyName!);
			ArgumentNullException.ThrowIfNullOrWhiteSpace(company);

			product = product.ensureNotNullOrWhiteSpace(AppInfo.ProductName!);
			ArgumentNullException.ThrowIfNullOrWhiteSpace(product);

			string rootPath = Environment.GetFolderPath(roaming
				? Environment.SpecialFolder.ApplicationData
				: Environment.SpecialFolder.LocalApplicationData);

			if ( useAssemblyName )
			{

				if ( assemblymName.isNullOrWhiteSpace ) assemblymName = Assembly.GetEntryAssembly()?.GetName()?.Name ?? string.Empty;
				ArgumentNullException.ThrowIfNullOrWhiteSpace(assemblymName);
			}

			string fullPath = Path.Combine(rootPath , company , product);
			if ( useAssemblyName && assemblymName.isNotNullOrWhiteSpace ) fullPath = Path.Combine(fullPath , assemblymName);

			DirectoryInfo di = new(fullPath);
			if ( createIfNotExist ) di.createIfNotExist();
			return di;
		}
#endif


		public static string? TitleHeader => Title + ((@AssemblyFileVersionAttribute is null)
			? ""
			: " v" + @AssemblyFileVersionAttribute);

		public static bool CurrentUICultureIsRuTree
		   => CultureInfo.CurrentUICulture.isRussianTree;




#if ANDROID
		internal static bool IsEmulator => DeviceInfo.Current.DeviceType == DeviceType.Virtual;

		/// <summary> AppInfo.Product</summary>
		//internal static string TitleShort => AppInfo.Title!;

		internal static Page? CurrentUIContext => Application.Current!.MainPage!;

#endif





	}




	internal static partial class AppTools
	{

		/// <summary>
		/// In Net & Core we need to RegisterEncodingProvider to use encodigs like Win-1251 or other
		/// </summary>
		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void RegisterEncodings ()
			=> uom.Globalize.encodings.RegisterEncodings();


		#region EmbeddedResource Tools

		//internal static partial class AppResources        {        }


		//internal static string AssemblyEntryPointNamespace = AppInfo.AppAssembly.EntryPoint?.DeclaringType?.Namespace!;


		private static string? _localizedStringsManager_Namespace = null;

		internal static string? LocalizedStringsManager_Namespace
		{
			get
			{
				return _localizedStringsManager_Namespace ??= AppInfo.AppAssembly.EntryPoint?.DeclaringType?.Namespace!;
				//=> AppInfo.AppAssembly.EntryPoint?.DeclaringType?.Namespace!;
			}
			set
			{
				_localizedStringsManager_Namespace = value;
			}
		}


		internal static string LocalizedStringsManager_ResourcesPath = "Localization.LStrings";

		internal static Lazy<System.Resources.ResourceManager> LocalizedStringsManager => new(
			new System.Resources.ResourceManager(
				$"{LocalizedStringsManager_Namespace}.{LocalizedStringsManager_ResourcesPath}" , AppInfo.AppAssembly));


		/// <param name="resourcePath"><c>"Localization.LStrings"</c> or something like</param>
		internal static System.Resources.ResourceManager CreateResourceManager ( Type resourceType )
			=> new(resourceType);      //=> new($"{resourceType.FullName}", Assembly);


		/// <param name="embeddedResourceFileSuffix">Format: <c>"{Namespace}.{Folder}.{filename}.{Extension}"</c></param>
		public static Stream GetEmbeddedResourceStream ( string embeddedResourceFileSuffix )
		{
			string[] resNames = AppInfo.AppAssembly.GetManifestResourceNames();
			string? resourcePath = resNames.FirstOrDefault(str => str.EndsWith(embeddedResourceFileSuffix));
			return resourcePath is null
				? throw new ArgumentException(null , nameof(embeddedResourceFileSuffix))
				: AppInfo.AppAssembly.GetManifestResourceStream(resourcePath)!;
		}


		/// <param name="embeddedResourceFileSuffix">Format: <c>"{Namespace}.{Folder}.{filename}.{Extension}"</c></param>
		public static string GetEmbeddedResourceAsString ( string embeddedResourceFileSuffix , Encoding? e = null )
		{
			using Stream stream = GetEmbeddedResourceStream(embeddedResourceFileSuffix);

			using StreamReader reader = (e is null)
				? new StreamReader(stream , true)
				: new StreamReader(stream , e!);

			return reader.ReadToEnd();
		}

		/// <param name="embeddedResourceFileSuffix">Format: <c>"{Namespace}.{Folder}.{filename}.{Extension}"</c></param>
		public static void SaveEmbeddedResourceAsString ( string embeddedResourceFileSuffix , FileInfo writeTo , Encoding? e = null )
		{
			e ??= Encoding.Unicode;
			string body = GetEmbeddedResourceAsString(embeddedResourceFileSuffix , e);
			writeTo.writeAllText(body , e);
		}


		#endregion



		internal static FileInfo GetFileIn_AppStartupDir ( string FileName )
			=> new(Path.Combine(AppInfo.StartupPath , FileName));


#if WINDOWS


		internal static FileInfo GetFileIn_AppData ( string filePath , bool roaming )
			=> new(Path.Combine(AppInfo.UserAppDataPath(roaming).FullName , filePath));
#endif


		internal static TypeInfo[] GetAllAssemblyClassesDerivedFrom ( Type T )
			=> [ ..
				Assembly
			.GetEntryAssembly()!
			.DefinedTypes
			.Where(rT => ReferenceEquals(rT.BaseType , T))
				];



		internal static string GetCallingMethodNamespacePart ( int count )
		{
			var callingFrame = new StackTrace().GetFrames()[ 1 ];
			var mdt = callingFrame.GetMethod()!.DeclaringType!;
			string ns = mdt.Namespace!;
			var nsParts = ns.Split('.');
			nsParts.Reverse();
			nsParts = [ .. nsParts.Take(count) ];
			return nsParts.join("." , listEmptyValue: ns)!;
		}


		public static Process[] GetAllInstancesOfMyself ()
		{
			using Process cp = Process.GetCurrentProcess();
			return Process.GetProcessesByName(cp.ProcessName);
		}


		public static bool HasOtherInstancesOfMyself ()
		{
			using Process cp = Process.GetCurrentProcess();
			return GetAllInstancesOfMyself()
				.SingleOrDefault(p => p.Id != cp.Id) is not null;
		}

		#region StartProcess...

		public enum ConsoleAppErrorMode : int
		{
			ErrorMustThrowException,
			ErrorReturnsAsOutput
		}

		internal static string StartConsoleAppAndCaptureOutput (
			string sExe ,
			string? sArguments = null ,
			string? workingDirectory = null ,
			int WAIT_TIMEOUT_SEC = 5 ,
			bool SUPRESS_DEBUG_INFO = false ,
			Encoding? StandardOutputEncoding = null ,
			ConsoleAppErrorMode cem = ConsoleAppErrorMode.ErrorMustThrowException )
		{
			Debug.WriteLine("*** erun '" + sExe + "' with args: '" + sArguments + "'");
			FileInfo fiExe = new(sExe);
			var PSI = new ProcessStartInfo
			{
				FileName = sExe ,
				WorkingDirectory = workingDirectory ?? fiExe.DirectoryName ,
				Arguments = sArguments ,
				UseShellExecute = false ,
				RedirectStandardOutput = true ,
				RedirectStandardError = true
			};

			if ( StandardOutputEncoding is not null )
			{
				PSI.StandardOutputEncoding = StandardOutputEncoding;
			}

			int iWaitMilliseconds = WAIT_TIMEOUT_SEC * 1000;
			using var prcExe = Process.Start(PSI);
			_ = prcExe ?? throw new Exception($"Process.Start({fiExe}) Failed!");

			string sError = prcExe.StandardError.ReadToEnd();
			if ( sError.isNotNullOrWhiteSpace && cem == ConsoleAppErrorMode.ErrorMustThrowException )
			{
				throw new Exception(sError);
			}

			string sOutput = prcExe.StandardOutput.ReadToEnd();
			sOutput = sOutput.isNullOrWhiteSpace
				? ""
				: sOutput
					.Replace(constants.vbCrCrLf , constants.vbCrLf)
					.Trim();

			bool bWaitResult = prcExe.WaitForExit(iWaitMilliseconds);
			if ( !bWaitResult || !prcExe.HasExited )
			{
				Debug.WriteLine($"{fiExe} - Failed to finish for {WAIT_TIMEOUT_SEC} sec. Answer: '{sOutput}'");
				Debug.WriteLine($"{fiExe} - Force Closing...");
				prcExe.Close();
				Debug.WriteLine($"{fiExe} - Force Closed!");
			}
			// Call PRC.WaitForExit()
			else if ( !SUPRESS_DEBUG_INFO )
			{
				Debug.WriteLine($"{fiExe} - finished ok. Output = '{sOutput}'");
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

		private DateTimeInterval () : base() { UpdateFromTime(); }

		public override string ToString () => $"с {StartTime} по {StopTime}";

		private void UpdateFromTime ( TimeParts flg = TimeParts.Start | TimeParts.Stop )
		{
			var dtNow = DateTime.Now;
			if ( flg.HasFlag(TimeParts.Start) )
			{
				StartTime = dtNow;
			}

			if ( flg.HasFlag(TimeParts.Stop) )
			{
				StopTime = dtNow;
			}
		}

		/// <summary>Starts, or resumes, measuring elapsed time for an interval.</summary>
		public new void Start ()
		{
			base.Start();
			UpdateFromTime(TimeParts.Start);
		}

		/// <summary>Initializes a new System.Diagnostics.Stopwatch instance, sets the elapsed time property to zero, and starts measuring elapsed time.</summary>
		public static new DateTimeInterval StartNew ()
		{
			var DTI = new DateTimeInterval();
			DTI.Start();
			return DTI;
		}

		/// <summary>Stops measuring elapsed time for an interval.</summary>
		public new void Stop ()
		{
			base.Stop();
			UpdateFromTime(TimeParts.Stop);
		}


		/// <summary>Stops time interval measurement and resets the elapsed time to zero.</summary>
		public new void Reset ()
		{
			base.Reset();
			UpdateFromTime();
		}

		/// <summary>Stops time interval measurement, resets the elapsed time to zero, and starts measuring elapsed time.</summary>
		public new void Restart ()
		{
			Stop();
			Start();
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
			T CloneT ();

#if NET
			object ICloneable.Clone () => CloneT()!;
#endif
		}


		internal interface IConvertibleTo<T>
		{

			T ConvertTo ();

		}



		internal interface IFilterableOf<T>
		{

			bool Validate ( T filter );

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


	internal partial class ValueBeforeChangeEventArgs<T> ( T? oldValue , T? newValue ) : EventArgs()
	{
		public readonly T? OldValue = oldValue;
		public readonly T? NewValue = newValue;
		public bool Cancel = false;
	}



	namespace parallel
	{





		/// <summary>Container for multithread safe get / set property value
		/// Supports notification on property changes (doe not check real changing of value, just notify about SET calls)</summary>
		/// <remarks>Valid for simple types (int, string, boolean), class and struts will not be MT safe, bc their child props has no MTsafe protect</remarks>
		internal partial class MTSafeContainerSyncLock<T> //: AutoDisposableUniversal<T>
		{
			protected EventArgs _MTSyncObject = new();

			public MTSafeContainerSyncLock ( T InitialValue ) : base() => Value = InitialValue;

			public virtual T? Value
			{
				get
				{
					lock ( _MTSyncObject )
					{
						return field;
					}
				}
				set
				{
					lock ( _MTSyncObject )
					{
						field = value;
					}
				}
			}
		}

		/// <summary>'Threading.ReaderWriterLockSlim' Контейнер для многопоточно-безопасного получения и установки какого либо значения
		/// Позволяет множественные чтения, но эксклюзивную запись
		/// Поддерживает уведомления об изменении свойств (не проверяет реально ли значение изменилось, просто срабатывает после каждой установки значения)</summary>
		/// <typeparam name="T">Тип значения в контейнере</typeparam>
		/// <remarks>Подходит для простых типов (int, string, boolean), составные типы или классы не будут безопасными, т.к. их дочерние свойства и поля не защищаются данным механизмом</remarks>
		internal partial class MTSafeContainerReaderWriterLockSlim<T> : MTSafeContainerSyncLock<T>
		{

			private readonly ReaderWriterLockSlim _sync;

			public MTSafeContainerReaderWriterLockSlim ( T InitialValue , LockRecursionPolicy LRP = LockRecursionPolicy.NoRecursion ) : base(InitialValue)
			{
				_sync = new ReaderWriterLockSlim(LRP);
				Value = InitialValue;
			}

			protected void RunInSafeLock ( Action a , bool write )
			{
				if ( write )
				{
					_sync.EnterWriteLock();
				}
				else
				{
					_sync.EnterReadLock();
				}

				try
				{
					a.Invoke();
				}
				finally
				{
					if ( write )
					{
						_sync.ExitWriteLock();
					}
					else
					{
						_sync.ExitReadLock();
					}
				}
			}

			/// <summary>Многопоточнобезопасное получение и установка значения</summary>
			public override T? Value
			{
				get
				{
					T? v = default;
					RunInSafeLock(() => { v = field; } , false);
					return v;
				}
				set
				{
					RunInSafeLock(() => { field = value; } , true);
				}
			}

			/// <summary> IDisposable</summary>
			private void Destroy () => _sync?.disposeSafe();
		}


		//[DefaultProperty("IsSet")]
		internal partial class MTSafeBooleanFlag : MTSafeContainerReaderWriterLockSlim<bool>
		{
			public MTSafeBooleanFlag ( bool bDefaultValue = false ) : base(bDefaultValue) { }

			public void SetlFlag ( bool bFlagValue = true ) => Value = bFlagValue;
			public void ClearFlag () => Value = false;
			/// <summary>Инвертирует текущее состояние. Возвращает новое, установленное состояние</summary>
			public bool Invert () { bool bInverted = !Value; Value = bInverted; return bInverted; }
			public bool IsSet { get => Value; }

			public static bool operator true ( MTSafeBooleanFlag R ) => R.Value;
			public static bool operator false ( MTSafeBooleanFlag R ) => !R.Value;

			public static implicit operator bool ( MTSafeBooleanFlag d ) => d.IsSet;

			public static implicit operator MTSafeBooleanFlag ( bool bFlag ) => new(bFlag);
		}

		internal partial class MTSafeCounterInt32 : MTSafeContainerReaderWriterLockSlim<int>
		{
			public MTSafeCounterInt32 ( int iDefaultValue = 0 ) : base(iDefaultValue) { }

			public void Increment ( int iStep = 1 )
				=> RunInSafeLock(() => { Value += iStep; } , true);

			public void Decrement ( int iStep = 1 )
				=> RunInSafeLock(() => { Value -= iStep; } , true);

			public void Reset () => Value = 0;

			public static implicit operator int ( MTSafeCounterInt32 I ) => I.Value;
			public static implicit operator MTSafeCounterInt32 ( int I ) => new(I);
			public static bool operator < ( MTSafeCounterInt32 I1 , int I2 ) => I1 < I2;
			public static bool operator <= ( MTSafeCounterInt32 I1 , int I2 ) => I1 <= I2;
			public static bool operator > ( MTSafeCounterInt32 I1 , int I2 ) => I1 > I2;
			public static bool operator >= ( MTSafeCounterInt32 I1 , int I2 ) => I1 >= I2;
			public static bool operator == ( MTSafeCounterInt32 I1 , int I2 ) => I1 == I2;
			public static bool operator != ( MTSafeCounterInt32 I1 , int I2 ) => I1 != I2;

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
			public override bool Equals ( object obj ) => this == (MTSafeCounterInt32)obj;
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

			public override int GetHashCode () => Value.GetHashCode();
		}



		/// <summary>Универсальная обертка на основе lock (Для любых типов данных)</summary>
		/// <remarks>Для работы со ссылочными типами (class) или сложными структурами (struct), где требуется гарантировать атомарность чтения и записи.</remarks>
		internal class AtomicLock<TValue> ( TValue val = default )
		{

			public event EventHandler<ValueBeforeChangeEventArgs<TValue>> OnBeforeValueChange = delegate { };
			public event EventHandler<TValue?> OnValueChanged = delegate { };

			private readonly object _lock = new();

			public TValue Value
			{
				get
				{
					lock ( _lock )
					{
						return val;
					}
				}
				set
				{
					lock ( _lock )
					{
						ValueBeforeChangeEventArgs<TValue> ea = new(val , value);
						OnBeforeValueChange?.Invoke(this , ea);
						if ( ea.Cancel ) return;

						val = value;

						OnValueChanged?.Invoke(this , value);
					}
				}
			}

			// Потокобезопасное обновление значения на основе текущего (избегает состояния гонки)
			public TValue Update ( Func<TValue , TValue> updateFunc )
			{
				lock ( _lock )
				{
					val = updateFunc(val);
					return val;
				}



			}
		}


		internal class AtomicRef<TValue> ( TValue? val = default ) where TValue : class
		{


			public event EventHandler<ValueBeforeChangeEventArgs<TValue>> OnBeforeValueChange = delegate { };
			public event EventHandler<TValue?> OnValueChanged = delegate { };

			public TValue? Value
			{
				get => Volatile.Read(ref val);
				set
				{
					ValueBeforeChangeEventArgs<TValue> ea = new(val , value);
					OnBeforeValueChange?.Invoke(this , ea);
					if ( ea.Cancel ) return;

					Interlocked.Exchange(ref val , value);

					OnValueChanged?.Invoke(this , value);
				}
			}


			// Атомарное обновление значения без блокировки потоков (Optimistic Concurrency)
			public TValue? Update ( Func<TValue? , TValue?> updateFunc )
			{
				TValue?
					spinWaitValue,
					newValue;
				do
				{
					spinWaitValue = val;
					newValue = updateFunc(spinWaitValue);
				}
				// Если за время расчета updateFunc значение в _value изменилось другим потоком, 
				// цикл повторится заново с актуальным значением.
				while ( Interlocked.CompareExchange(ref val , newValue , spinWaitValue) != spinWaitValue );

				return newValue;
			}

			public static implicit operator TValue? ( AtomicRef<TValue> I ) => I.Value;
			public static implicit operator AtomicRef<TValue> ( TValue? I ) => new(I);
		}


		/// <summary> Высокопроизводительная обертка на основе Interlocked (Для примитивов)</summary>
		/// <remarks>
		/// Если ваше свойство — это число (int, long, float), double, bool или ссылка на объект, вы можете использовать класс Interlocked. <br/>
		/// Он работает на уровне инструкций процессора (без блокировки потоков), что делает его значительно быстрее, чем lock.
		/// </remarks>
		internal class AtomicInt
		{
			private int _value;

			public AtomicInt ( int initialValue = 0 ) => _value = initialValue;

			public int Value
			{
				get => Volatile.Read(ref _value);
				set => Volatile.Write(ref _value , value);
			}

			// Безопасный инкремент (++i)
			public int Increment () => Interlocked.Increment(ref _value);

			// Безопасный декремент (--i)
			public int Decrement () => Interlocked.Decrement(ref _value);
		}



		/// <summary>Потокобезопасная обертка, оптимизированная под математические типы .NET 11+.</summary>
		internal class AtomicNumber<T> ( T initialValue = default ) where T : struct, INumber<T>
		{

			// Новый легковесный тип блокировки из .NET 9+
			private readonly Lock _lock = new();
			private T _value = initialValue;

			/// <summary>
			/// Потокобезопасное чтение и запись значения свойства.
			/// </summary>
			public T Value
			{
				get
				{
					lock ( _lock )
					{
						return _value;
					}
				}
				set
				{
					lock ( _lock )
					{
						_value = value;
					}
				}
			}

			/// <summary>
			/// Атомарный инкремент (аналог ++i). Возвращает новое значение.
			/// </summary>
			public T Increment ()
			{
				lock ( _lock )
				{
					_value += T.One;
					return _value;
				}
			}

			/// <summary>
			/// Атомарный декремент (аналог --i). Возвращает новое значение.
			/// </summary>
			public T Decrement ()
			{
				lock ( _lock )
				{
					_value -= T.One;
					return _value;
				}
			}

			/// <summary>
			/// Атомарное сложение с произвольным числом того же типа (аналог +=).
			/// </summary>
			public T Add ( T amount )
			{
				lock ( _lock )
				{
					_value += amount;
					return _value;
				}
			}

			/// <summary>
			/// Атомарное обновление значения с помощью кастомной лямбды.
			/// </summary>
			public T Update ( Func<T , T> updateFunc )
			{
				lock ( _lock )
				{
					_value = updateFunc(_value);
					return _value;
				}
			}
		}



		/// <summary>Обертка ReaderWriterLockSlim (Оптимизация для частого чтения) </summary>
		/// <remarks>
		/// Если ваше свойство считывается сотни раз в секунду из разных потоков, но изменяется относительно редко, блокировка lock может стать узким местом. <br/>
		/// Использование ReaderWriterLockSlim позволяет множеству потоков читать данные одновременно, но полностью изолирует поток при записи.
		/// </remarks>        
		internal class SharedResource<T> : IDisposable
		{
			private readonly ReaderWriterLockSlim _lock = new();
			private T _value;

			public SharedResource ( T initialValue = default )
			{
				_value = initialValue;
			}

			public T Value
			{
				get
				{
					_lock.EnterReadLock();
					try
					{
						return _value;
					}
					finally
					{
						_lock.ExitReadLock();
					}
				}
				set
				{
					_lock.EnterWriteLock();
					try
					{
						_value = value;
					}
					finally
					{
						_lock.ExitWriteLock();
					}
				}
			}

			public void Dispose () => _lock.Dispose();
		}


		namespace taskSchedulers
		{

			/// <summary>
			/// Provides a task scheduler that ensures a maximum concurrency level while
			/// running on top of the ThreadPool.
			/// </summary>
			internal class _LimitedConcurrencyLevelTaskScheduler : TaskScheduler
			{

				//Parallel
				/// <summary>Whether the current thread is processing work items.</summary>
				[ThreadStatic]
				private static bool s_currentThreadIsProcessingItems;
				/// <summary>The list of tasks to be executed.</summary>
				private readonly LinkedList<Task> _tasks = new LinkedList<Task>(); // protected by lock(_tasks)
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
					if ( maxDegreeOfParallelism < 1 )
						throw new ArgumentOutOfRangeException(nameof(maxDegreeOfParallelism));


					_maxDegreeOfParallelism = maxDegreeOfParallelism;
				}

				/// <summary>Queues a task to the scheduler.</summary>
				/// <param name="task">The task to be queued.</param>
				protected sealed override void QueueTask ( Task task )
				{
					// Add the task to the list of tasks to be processed.  If there aren't enough
					// delegates currently queued or running to process tasks, schedule another.
					lock ( _tasks )
					{
						_tasks.AddLast(task);
						if ( _delegatesQueuedOrRunning < _maxDegreeOfParallelism )
						{
							++_delegatesQueuedOrRunning;
							NotifyThreadPoolOfPendingWork();
						}
					}
				}

				/// <summary>
				/// Informs the ThreadPool that there's work to be executed for this scheduler.
				/// </summary>
				private void NotifyThreadPoolOfPendingWork () => ThreadPool.UnsafeQueueUserWorkItem(_ =>
				{
					// Note that the current thread is now processing work items.
					// This is necessary to enable inlining of tasks into this thread.
					s_currentThreadIsProcessingItems = true;
					try
					{
						// Process all available items in the queue.
						while ( true )
						{
							Task item;
							lock ( _tasks )
							{
								// When there are no more items to be processed,
								// note that we're done processing, and get out.
								if ( _tasks.Count == 0 )
								{
									--_delegatesQueuedOrRunning;
									break;
								}

								// Get the next item from the queue
								item = _tasks.First!.Value;
								_tasks.RemoveFirst();
							}

							// Execute the task we pulled out of the queue
							TryExecuteTask(item);
						}
					}
					// We're done processing items on the current thread
					finally { s_currentThreadIsProcessingItems = false; }
				} , null);

				/// <summary>Attempts to execute the specified task on the current thread.</summary>
				/// <param name="task">The task to be executed.</param>
				/// <param name="taskWasPreviouslyQueued"></param>
				/// <returns>Whether the task could be executed on the current thread.</returns>
				protected sealed override bool TryExecuteTaskInline ( Task task , bool taskWasPreviouslyQueued )
				{
					// If this thread isn't already processing a task, we don't support inlining
					if ( !s_currentThreadIsProcessingItems )
					{
						return false;
					}

					// If the task was previously queued, remove it from the queue
					if ( taskWasPreviouslyQueued )
					{
						TryDequeue(task);
					}

					// Try to run the task.
					return TryExecuteTask(task);
				}

				/// <summary>Attempts to remove a previously scheduled task from the scheduler.</summary>
				/// <param name="task">The task to be removed.</param>
				/// <returns>Whether the task could be found and removed.</returns>
				protected sealed override bool TryDequeue ( Task task )
				{
					lock ( _tasks )
					{
						return _tasks.Remove(task);
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
						Monitor.TryEnter(_tasks , ref lockTaken);
						return lockTaken ? (IEnumerable<Task>)_tasks.ToArray() : throw new NotSupportedException();
					}
					finally
					{
						if ( lockTaken )
						{
							Monitor.Exit(_tasks);
						}
					}
				}
			}


		}


	}


	namespace AutoDisposable
	{


		[DefaultProperty("Value")]
		internal abstract partial class DisposableContainerBase<T> : AutoDisposableUniversal where T : IDisposable
		{


			public event EventHandler<ValueBeforeChangeEventArgs<T>>?
				BeforeValueChanged,
				AfterValueChanged,
				ValueChangeCanceled;

			/// <summary>MT-UNsafe</summary>
			public virtual T? Value
			{
				get => field;
				set
				{
					var ea = new ValueBeforeChangeEventArgs<T>(field , value);
					OnBeforeValueChanged(ea);
					if ( ea.Cancel )
					{
						OnValueChangeCanceled(ea);
						return;
					}

					field = value;
					OnAfterValueChanged(ea);

				}
			}



			public override string ToString () => $"{typeof(T)} = {Value}";



			protected virtual void OnBeforeValueChanged ( ValueBeforeChangeEventArgs<T> EA ) => BeforeValueChanged?.Invoke(this , EA);
			protected virtual void OnValueChangeCanceled ( ValueBeforeChangeEventArgs<T> EA ) => ValueChangeCanceled?.Invoke(this , EA);
			protected virtual void OnAfterValueChanged ( ValueBeforeChangeEventArgs<T> EA ) => AfterValueChanged?.Invoke(this , EA);


			public override void Dispose ()
			{
				Value?.Dispose();
				base.Dispose();
			}

		}

		/// <summary>
		/// Used to cache any kind of IDisposable objects and dispose them when the cache is disposed
		/// </summary>
		internal class DisposableCache : IDisposable
		{

			private readonly Dictionary<string , IDisposable> _cache = [];


			private bool _disposed = false;


			public void Dispose ()
			{
				if ( _disposed ) return;
				_disposed = true;

				lock ( _cache )
				{
					//Disposing all cached objects in reverse order
					IDisposable[] items = [ .. _cache.Values.Reverse() ];
					foreach ( var item in items )
					{
						try
						{
							item.disposeSafe();
						}
						catch { }
					}

					_cache.Clear();

				}
			}


			public void Append ( IDisposable obj , string? key = null )
			{
				if ( _disposed ) return;
				if ( obj is null ) throw new ArgumentNullException(nameof(obj));

				key ??= System.Guid.NewGuid().ToString();
				lock ( _cache )
					_cache.Add(key , obj);
			}


			public IDisposable GetByID ( string id )
			{
				lock ( _cache )
					return _cache[ id ];
			}


			public T GetByID<T> ( string id ) where T : IDisposable
				=> (T)GetByID(id);


			public IDisposable[] GetByType ( Type T )
			{

				lock ( _cache )
				{
					IDisposable[] dd = [..
				_cache
				.Values
				.Where(d => d.GetType() == T)
					];

					return dd;
				}
			}

		}





		/// <summary> Base Class than automaticaly disposes its any attached values </summary>
		internal abstract class AutoDisposableUniversal : IDisposable
		{


			private bool _disposed = false;


			//// TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
			~AutoDisposableUniversal ()
				=> Dispose();

			public virtual void Dispose ()
			{

				if ( _disposed ) return;
				_disposed = true;

				try { FreeManagedObjects(); } catch { }// Ignore Any Errors
				try { FreeUnmanagedObjects(); } catch { }// Ignore Any Errors    
				GC.SuppressFinalize(this);
			}


			protected readonly List<IDisposable>
				_disposableObjectsStack_Managed = [],
				_disposableObjectsStack_UnManaged = [];

			protected readonly Stack<Action>
				_disposableCallBacksStack_Managed = [],
				_disposableCallBacksStack_UnManaged = [];

			public AutoDisposableUniversal ( Action? managedDisposeCallBack = null , Action? unManagedDisposeCallBack = null ) : base()
			{
				if ( null != managedDisposeCallBack ) RegisterDisposeCallback(managedDisposeCallBack);
				if ( null != unManagedDisposeCallBack ) RegisterDisposeCallback(unManagedDisposeCallBack);
			}


			protected internal T RegisterDisposableObject<T> ( T mdo , bool managed = true ) where T : IDisposable
			{
				ArgumentNullException.ThrowIfNull(mdo);

				var list = managed
					? _disposableObjectsStack_Managed
					: _disposableObjectsStack_UnManaged;

				if ( list.Contains(mdo) )
					throw new ArgumentException("Object already added to dispose list!" , nameof(mdo));

				list.Add(mdo);
				return mdo;
			}
			protected internal T UnRegisterDisposableObject<T> ( T mdo , bool managed = true ) where T : IDisposable
			{
				ArgumentNullException.ThrowIfNull(mdo);

				var list = managed
					? _disposableObjectsStack_Managed
					: _disposableObjectsStack_UnManaged;

				if ( list.Contains(mdo) )
					list.Remove(mdo);

				return mdo;
			}



			/// <summary>Регистрируем действия, которые надо будет выполнить при уничтожении родительского объекта</summary>
			protected internal void RegisterDisposeCallback ( Action onDispose , bool managed = true )
			{
				ArgumentNullException.ThrowIfNull(onDispose);

				var stack = managed
				  ? _disposableCallBacksStack_Managed
				  : _disposableCallBacksStack_UnManaged;

				if ( stack.Contains(onDispose) )
					throw new ArgumentException($"'{nameof(onDispose)}' Already regidtered!" , nameof(onDispose));

				stack.Push(onDispose);
			}

			protected virtual void FreeManagedObjects ()
			{
				OnBeforeDispose(true);
				dispose(_disposableObjectsStack_Managed);
				invokeDisposeActions(_disposableCallBacksStack_Managed);
			}

			protected virtual void FreeUnmanagedObjects ()
			{
				OnBeforeDispose(false);
				dispose(_disposableObjectsStack_UnManaged);
				invokeDisposeActions(_disposableCallBacksStack_UnManaged);
			}

			/// <summary>Just template, override if need</summary>            
			protected virtual void OnBeforeDispose ( bool bManages ) { }




			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static void dispose ( List<IDisposable> list )
			{
				IDisposable[] a = [ .. list.ToArray().Reverse() ];
				foreach ( var o in a )
				{
					o.disposeSafe();
				}
				list.Clear();
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static void dispose ( Stack<IDisposable> stack )
			{
				while ( stack.Any() )
				{
					stack.Pop().disposeSafe();
				}
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static void invokeDisposeActions ( Stack<Action> stack )
			{
				while ( stack.Any() )
				{
					stack.Pop().Invoke();
				}
			}


		}


		/*
		/// <summary> Class than automaticaly disposes 1 attached value </summary>
		internal abstract class AutoDisposable1 : AutoDisposableUniversal
		{
			public AutoDisposable1 () : base() { }

			protected IDisposable? _Value = null;

			public IDisposable? Value
			{
				get => _Value;
				set { _Value = value; RegisterDisposableObject(_Value! , true); }
			}
		}
		 */



		/// <summary> Class than automaticaly disposes 1 attached value </summary>
		internal class AutoDisposable1T<T> : AutoDisposableUniversal where T : IDisposable
		{
			public AutoDisposable1T () : base() { }

			//protected T? _Value = default;

			public T? Value
			{
				get => field;
				set
				{
					if ( field is not null ) UnRegisterDisposableObject(field! , true);
					if ( value is not null ) RegisterDisposableObject(value! , true);
					field = value;
				}
			}
		}


		/// <summary>Очередь для уничтожаемых одбектов, которые надо не забыть уничтожить, но прямо сейчас их уничтожать нельзя.
		/// Например для текущего захваченного с камеры кадра, который в данный момент отображается на экране - его нельзя сейчас уничтожать,
		/// но при добовлении следующего кадра, этот старый уже можно будет уничтожать, и его надо где-то хранить чтобы его не уничтожил GC - для этого эта помойка.</summary>
		internal class DisposableTrashBin : AutoDisposableUniversal
		{

			private readonly EventArgs _lock = new();
			private readonly Queue<IDisposable> _Q = new();
			private readonly int _iQueueLength = 10;

			public DisposableTrashBin ( int QueueLength ) : base()
			{
				_iQueueLength = QueueLength;

				RegisterDisposeCallback(() => ClearAll());
			}

			public void Put ( IDisposable ObjectToDispose )
			{
				if ( ObjectToDispose is null )
				{
					return;
				}

				lock ( _lock )
				{
					if ( _Q.Contains(ObjectToDispose) )
					{
						return;
					}

					_Q.Enqueue(ObjectToDispose);

					while ( _Q.Count > _iQueueLength )
					{
						ObjectToDispose = _Q.Dequeue();
						ObjectToDispose.Dispose();

						//Debug.WriteLine($"Trash disposed old object, queue = {_Q.Count}");
					}
				}
			}

			public void ClearAll ()
			{
				lock ( _lock )
				{
					while ( _Q.Count > 0 )
					{
						var ObjectToDispose = _Q.Dequeue();
						try { ObjectToDispose.Dispose(); }
						catch { }//Ignore any errors
					}
				}
				//Debug.WriteLine($"Trash bin cleared.");
			}
		}



	}



	internal static class mem
	{


		/// <summary>
		/// Allocates memory for structure and invokes acton with it, finally free memory.
		/// </summary>
		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static TStruct AllocStruct<TStruct> ( Action<IntPtr , int> memoryInitializer ) where TStruct : struct
		{


			int len = Marshal.SizeOf<TStruct>();
			IntPtr mem = Marshal.AllocHGlobal(len);
			try
			{
				memoryInitializer.Invoke(mem , len);
				return Marshal.PtrToStructure<TStruct>(mem)!;
			}
			finally
			{
				// Free the unmanaged memory.
				Marshal.FreeHGlobal(mem);
			}

		}


#pragma warning disable CS9198 // Reference kind modifier of parameter doesn't match the corresponding parameter in target.
		[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe T** AsPointer<T> ( ref T* value ) where T : unmanaged
		{
			delegate*< ref byte , void* > d = &Unsafe.AsPointer;
			return ((delegate*< ref T* , T** >)d)(ref value);
		}
#pragma warning restore CS9198 // Reference kind modifier of parameter doesn't match the corresponding parameter in target.






		internal class SlidingWindow<T> ( ArraySegment<T> mem , int startPos = 0 , int windowSize = 1 )
		{
			private readonly ArraySegment<T> _mem = mem;
			private int _startPos = startPos;
			private int _windowSize = windowSize;


			public SlidingWindow ( T[] arr , int startPos = 0 , int windowSize = 1 ) : this(new ArraySegment<T>(arr) , startPos , windowSize) { }

#if NET
			public SlidingWindow ( Memory<T> mem , int startPos = 0 , int windowSize = 1 ) : this(mem.ToArray() , startPos , windowSize) { }
#endif

			/// <summary>Get/set Sliding Window StartPos.
			/// if StartPos + WindoowSize > right bound, StartPos sets to MaxStartPos to fit WindowSize.</summary>
			public int StartPos
			{
				get => _startPos;
				set
				{
					int sp = value;
					if ( sp < 0 ) throw new ArgumentOutOfRangeException(nameof(StartPos));
					if ( sp > MaxStartPos ) sp = MaxStartPos;
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
					if ( ws < 1 ) throw new ArgumentOutOfRangeException(nameof(WindowSize));

					if ( ws > MaxWindowSize ) ws = MaxWindowSize;

					_windowSize = ws;
				}
			}

			/// <summary>Gets maximum StartPos for current WindowSize</summary>
			public int MaxStartPos => _mem.Count() - WindowSize;

			/// <summary>Gets maximum WindowSize for current StartPos</summary>
			public int MaxWindowSize => _mem.Count() - StartPos;

			/// <summary>Slides Windows to the right side, preserving WindowSize</summary>
			public void SlideToEnd () => StartPos = MaxStartPos;

			/// <summary>Slides Windows to the start</summary>
			public void SlideToStart () => StartPos = 0;

			public bool CanExpandRight => WindowSize < MaxWindowSize;

			public void ExpandToEnd () => WindowSize = MaxWindowSize;


			public IEnumerable<T> WindowData => _mem
				.Skip(_startPos)
				.Take(_windowSize);

			public bool SlideRight ( bool allowShrinkWindowSize = true )
			{
				if ( !CanExpandRight )
				{
					return false; // Window size already include all elements to the end! no more space rigth to move
				}

				if ( StartPos > MaxStartPos && !allowShrinkWindowSize )
				{
					return false;
				}

				_startPos += WindowSize;
				if ( _startPos > MaxStartPos )
				{
					ExpandToEnd();//Recalculate new Window Size
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
			internal const bool CLEAR_ON_DISPOSE_DEFAULT = false;

			/// <inheritdoc cref="DisposableSharedArrayContainer(uint, bool)"/>
			/// <param name="minimumLength">
			/// ! Rented array may be greater than asked minimumLength !<br/>
			/// Allways use array.AsSpanWithRequiredSize() to ensure array.Length corresponds minimumLength!
			/// </param>
			/// <param name="clearOnDispose">Clear array if it was strings or any class</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static DisposableSharedArrayContainer<T> Rent<T> ( uint minimumLength , bool zeroArray = false , bool clearOnDispose = CLEAR_ON_DISPOSE_DEFAULT )
			{
				var pool = new DisposableSharedArrayContainer<T>(minimumLength , clearOnDispose);
				if ( zeroArray ) pool.AsSpanWithRequiredSize().Clear();
				return pool;
			}

			/// <inheritdoc cref="Rent(uint, bool, bool)"/>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static DisposableSharedArrayContainer<T> Rent<T> ( int minimumLength , bool zeroArray = false , bool clearOnDispose = CLEAR_ON_DISPOSE_DEFAULT )
				=> Rent<T>((uint)minimumLength , zeroArray , clearOnDispose);



			/// <inheritdoc cref="Rent(uint, bool, bool)"/>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static DisposableSharedArrayContainer<byte> RentBytes ( uint minimumLength , bool zeroArray = false , bool clearOnDispose = CLEAR_ON_DISPOSE_DEFAULT )
				=> Rent<byte>(minimumLength , zeroArray , clearOnDispose);

			/// <inheritdoc cref="RentBytes(uint, bool, bool)"/>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static DisposableSharedArrayContainer<byte> RentBytes ( int minimumLength , bool zeroArray = false , bool clearOnDispose = CLEAR_ON_DISPOSE_DEFAULT )
				=> RentBytes((uint)minimumLength , zeroArray , clearOnDispose);




			/// <summary>
			/// ! Rented array may be greater than asked minimumLength !<br/>
			/// Allways use array.AsSpanWithRequiredSize() to ensure array.Length corresponds minimumLength!
			/// </summary>
			/// <param name="minimumLength">
			/// ! Rented array may be greater than asked minimumLength !<br/>
			/// Allways use array.AsSpanWithRequiredSize() to ensure array.Length corresponds minimumLength!
			/// </param>
			/// <param name="clearOnDispose">Clear array if it was strings or any class</param>
			public readonly struct DisposableSharedArrayContainer<T> ( uint minimumLength , bool clearOnDispose = CLEAR_ON_DISPOSE_DEFAULT ) : IDisposable
			{
				private readonly uint _minimumLength = minimumLength;
				private readonly bool _cearOnDispose = clearOnDispose;


				/// <summary>
				/// ! Rented array may be greater than asked minimumLength !<br/>
				/// Allways use array.AsSpanWithRequiredSize() to ensure array.Length corresponds minimumLength!
				/// </summary>
				public readonly T[] Value = ArrayPool<T>.Shared.Rent((int)minimumLength);


				public ref T this[ int index ] => ref Value[ index ];

				public ref T FirstElement => ref Value[ 0 ];

				public void Dispose ()
					=> ArrayPool<T>.Shared.Return(Value , _cearOnDispose);

				public Span<T> AsSpanWithRequiredSize ()
					=> Value.AsSpan().Slice(0 , (int)_minimumLength);


				//public static implicit operator byte ( Digit d ) => d.digit;
				//public static explicit operator Digit ( byte b ) => new Digit (b);

				public static implicit operator Span<T> ( DisposableSharedArrayContainer<T> src )
					=> src.AsSpanWithRequiredSize();

			}


#if NET



			public static DisposableSharedArrayContainer<byte> FromBase64String ( string value , out int bytesWritten )
			{
				using var bufferContainer = RentBytes(Encoding.UTF8.GetMaxByteCount(value.Length));
				var buffer = bufferContainer.AsSpanWithRequiredSize();
				int bufferSize = Encoding.UTF8.GetBytes(value , buffer);

				var decodedBufferContainer = RentBytes(Base64.GetMaxDecodedFromUtf8Length(value.Length));
				var decodedBuffer = decodedBufferContainer.AsSpanWithRequiredSize();
				try
				{
					Base64.DecodeFromUtf8(buffer[ ..bufferSize ] , decodedBuffer , out int _ , out bytesWritten);
					if ( bytesWritten == 0 )
						throw new InvalidOperationException("Error writing to buffer.");
				}
				catch
				{
					decodedBufferContainer.Dispose();
					throw;
				}
				return decodedBufferContainer;
			}



			public static string ToBase64String ( ReadOnlySpan<byte> value )
			{
				using var encodedBufferContainer = RentBytes(Base64.GetMaxEncodedToUtf8Length(value.Length));
				var encodedBuffer = encodedBufferContainer.AsSpanWithRequiredSize();
				Base64.EncodeToUtf8(value , encodedBuffer , out int _ , out int bytesWritten);
				if ( bytesWritten == 0 ) throw new InvalidOperationException("Error writing to buffer.");

				// Convert bytes to string            
				return Encoding.UTF8.GetString(encodedBuffer[ ..bytesWritten ]);
			}


#endif

		}


		/// <summary>Высокооптимизированный высокоскоростной поиск</summary>
		internal static class StringSearchCache
		{
			/*
			 Почему это эффективно в .NET 10?

	Экономия RAM: SearchValues для длинных списков строк могут занимать значительный объем памяти из-за внутренних таблиц переходов. 
			WeakReference позволяет GC удалять их, если приложение испытывает нехватку памяти.
	Thread-Safety: ConcurrentDictionary обеспечивает безопасный доступ из разных потоков без блокировок (lock).
	Авто-восстановление: Метод TryGetTarget гарантирует, что мы либо получим готовый объект, либо прозрачно создадим его заново, если он был удален.
	Сравнение кортежей: В .NET 10 кортеж (string, StringComparison) идеально подходит в качестве ключа, так как у него эффективно реализованы GetHashCode и Equals.
	Есть ли подводные камни?
	Если вы используете этот кэш постоянно (в каждом кадре или запросе), SearchValues никогда не будет собран GC, так как на него всегда будет "сильная" ссылка в стеке выполняемого метода. Кэш сработает на очистку только тогда, когда запросы на поиск конкретной строки прекратятся.
			 */


			// Кэшируем слабую ссылку на SearchValues. 
			// Если GC соберет SearchValues, мы пересоздадим его при следующем обращении.
			private static readonly Lazy<ConcurrentDictionary<(string Target , StringComparison Comparison) , WeakReference<SearchValues<string>>>> _cache
				= new();


			/// <summary>
			/// Высокооптимизированный высокоскоростной поиск<br/>
			/// Важный нюанс: Этот метод находит непересекающиеся вхождения. Если искать "aaa" в "aaaa", он найдет только один индекс (0). Если вам нужны пересекающиеся (0 и 1), нужно сдвигать currentPos на 1, а не на targetLen.<br/>
			/// ? Утечка памяти ? <br/>
			/// Если вы будете искать миллионы разных уникальных строк, ConcurrentDictionary _keys будет бесконечно расти, так как он держит сильные ссылки на объекты SearchKey. Need to use uom.mem.StringSearchCache.Clean periodically 
			/// </summary>
			internal static int[] GetIndexes ( ReadOnlySpan<char> source , string target , StringComparison comparison = StringComparison.Ordinal )
			{

				if ( source.isNullOrEmpty || target.isNullOrEmpty ) return [];

				// 1. Пытаемся достать живой объект из кэша
				var key = (target , comparison);
				if ( !_cache.Value.TryGetValue(key , out var weakRef) || !weakRef.TryGetTarget(out var searcher) )
				{
					// 2. Если объекта нет или он собран GC — создаем заново
					searcher = SearchValues.Create([ target ] , comparison);

					// Обновляем кэш новой слабой ссылкой
					_cache.Value[ key ] = new WeakReference<SearchValues<string>>(searcher);
				}

				// --- Логика поиска (высокопроизводительный двухпроходный алгоритм .NET 10) ---
				int targetLen = target.Length;
				int count = 0;
				int currentPos = 0;

				// Первый проход: считаем количество
				while ( currentPos <= source.Length - targetLen )
				{
					int offset = source[ currentPos.. ].IndexOfAny(searcher);
					if ( offset == -1 ) break;
					count++;
					currentPos += offset + targetLen;
				}

				if ( count == 0 ) return [];

				// Второй проход: заполняем массив
				int[] result = new int[ count ];
				int writeIdx = 0;
				currentPos = 0;
				while ( writeIdx < count )
				{
					int offset = source[ currentPos.. ].IndexOfAny(searcher);
					if ( offset == -1 ) break;

					int actualIndex = currentPos + offset;
					result[ writeIdx++ ] = actualIndex;
					currentPos = actualIndex + targetLen;
				}

				return result;
			}

			/// <inheritdoc cref="GetIndexes"/>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static int[] GetIndexes ( string source , string target , StringComparison comparison = StringComparison.Ordinal )
				=> GetIndexes(source.AsSpan() , target , comparison);








			/// <summary>
			/// Высокооптимизированный высокоскоростной поиск<br/>
			/// ? Утечка памяти ? <br/>
			/// Если вы будете искать миллионы разных уникальных строк, ConcurrentDictionary _keys будет бесконечно расти, так как он держит сильные ссылки на объекты SearchKey. Need to use uom.mem.StringSearchCache.Clean periodically 
			/// </summary>
			internal static int GetFirstIndex ( ReadOnlySpan<char> source , string target , StringComparison comparison = StringComparison.Ordinal )
			{
				// 1. Быстрые проверки на входе
				if ( source.isNullOrEmpty || target.isNullOrEmpty ) return -1;

				// 2. Получаем оптимизированный SearchValues из кэша (thread-safe + weak ref)
				var key = (target , comparison);
				if ( !_cache.Value.TryGetValue(key , out var weakRef) || !weakRef.TryGetTarget(out var searcher) )
				{
					searcher = SearchValues.Create([ target ] , comparison);
					_cache.Value[ key ] = new WeakReference<SearchValues<string>>(searcher);
				}

				// 3. Поиск первого вхождения через SIMD-ускоренный IndexOfAny
				// В .NET 10 это эффективнее, чем обычный IndexOf для сложных паттернов
				int index = source.IndexOfAny(searcher);
				return index;
			}


			/// <inheritdoc cref="GetFirstIndex"/>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static int GetFirstIndex ( string source , string target , StringComparison comparison = StringComparison.Ordinal )
				=> GetFirstIndex(source.AsSpan() , target , comparison);









			/// <summary>
			/// Метод для периодической очистки кэша от "мертвых" WeakReference
			/// </summary>
			public static void CleanUp ()
			{
				foreach ( var kvp in _cache.Value )
				{
					if ( !kvp.Value.TryGetTarget(out _) )
					{
						_cache.Value.TryRemove(kvp.Key , out _);
					}
				}
			}
		}



		/// <summary>
		/// Wrapper class to transfer in and out ref values to async functions
		/// </summary>
		internal class RefWrapper<T>
		{
			public T? Value { get; set; }
			public RefWrapper ( T? value ) => Value = value;
		}

	}


	namespace network
	{



		internal static class Helpers
		{


			public static bool IsInDomain ()
			{
				try
				{
					//_ = System.DirectoryServices.ActiveDirectory.Domain.GetComputerDomain();
					return System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName.isNotNullOrWhiteSpace;
				}
				catch { return false; }
			}


			/// <summary>Gets IP of installed network adapters</summary>
			/// <param name="os">Adapter operational status filter</param>

			private static IEnumerable<UnicastIPAddressInformation> GelLocalIP ( OperationalStatus os = OperationalStatus.Up )
			{

				var ual = System.Net.NetworkInformation.NetworkInterface
					.GetAllNetworkInterfaces()
					.Where(nic => nic.OperationalStatus == os)
					.SelectMany(nic => nic.GetIPProperties().UnicastAddresses)
					.Where(ip => ip.Address is not null);

				foreach ( var ip in ual )
				{
					yield return ip;
				}
			}

			/// <summary>Gets IP of installed network adapters</summary>
			/// <param name="os">Adapter operational status filter</param>

			public static IEnumerable<UnicastIPAddressInformation> GelLocalIP4 ( OperationalStatus os = OperationalStatus.Up )
			{
				var a = GelLocalIP(os)
					.Where(ua => ua.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && ua.IPv4Mask is not null);

				return a;
			}


			#region GetRemoteMAC

			/*

			/// <summary>Запрашивает в ARP таблице адрес удалённого хоста</summary>
			/// 		/// <param name="RemoteIP">IP удалённого хоста</param>
			/// 		/// <param name="TryPing">Предварительно пинговать хост (чтобы он появился в таблице ARP)</param>
			/// 		/// <param name="PingTimeOut">Тайм-аут пинга</param>
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
			/// 		/// <param name="RemoteIP">IP удалённого хоста</param>
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
		/// 		/// <param name="RemoteIPOrDNSMane">IP или DNS имя удаленного хоста</param>
		/// 		/// <param name="TryPing">Предварительно пинговать хост ()</param>
		/// 		/// <param name="PingTimeOut">Тайм-аут пинга</param>
		/// 		/// <param name="CanThrowError">Генерировать исключение если, произошла ошибка с одним из адресов</param>
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


			internal const int DOWNLOAD_DEFAULT_TIMEOUT_MINUTES = 10;
			internal const int DOWNLOAD_DEFAULT_BUFFER_SIZE = 81920;
			internal static readonly Lazy<HttpClient> _httpClient = new(new HttpClient() { Timeout = TimeSpan.FromMinutes(DOWNLOAD_DEFAULT_TIMEOUT_MINUTES) });
			internal const string HTTP_HEADER_AGENT_DEFAULT = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36";


			internal class DownloadProgressEventArgs ( ulong? totalBytes , ulong? completedBytes ) : System.EventArgs()
			{
				public readonly ulong? TotalBytes = totalBytes;
				public readonly ulong? CompletedBytes = completedBytes;

				public float? ProgressPercent => !TotalBytes.HasValue
					? null
					: (TotalBytes > 0)
						? (float)((CompletedBytes ?? 0ul) / (double)TotalBytes.Value)
						: 0f;

				public DownloadProgressEventArgs ( ulong totalBytes ) : this(totalBytes , totalBytes) { }


				//public override string ToString ()                    => base.ToString() ?? string.Empty;
			}

			internal delegate void DownloadProgressDelegate ( DownloadProgressEventArgs e );

		}




		internal class IP4AddressWithMask ( IPAddress ip , uint prefixLength = IP4AddressWithMask.PREFIX_32 ) : System.Net.NetworkInformation.IPAddressInformation(), IComparable<IP4AddressWithMask>
		{
			internal static readonly IPAddress CloudFlareRecursiveDNSConverter = IPAddress.Parse("1.1.1.1");

			internal static readonly IP4AddressWithMask CloudFlareDNSNetwork = new(CloudFlareRecursiveDNSConverter , 8);


			internal const uint PREFIX_0 = 0;
			internal const uint PREFIX_32 = 32;

			private static readonly Lazy<Regex> _rxMaskedIP = new(new Regex(@"\b(?<IP>\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})(?<Mask>\/\d{1,5})?\b"));
			private readonly IPAddress _ipAddress = ip;
			public readonly uint PrefixLength = prefixLength.clamp(0u , PREFIX_32);

			public override IPAddress Address => _ipAddress;

			public IPAddress Mask => PrefixLength.getIP4SubnetMaskFromMaskPrefix();

			public override bool IsDnsEligible => throw new NotImplementedException();
			public override bool IsTransient => throw new NotImplementedException();


			public IP4AddressWithMask ( IPAddress ip , IPAddress mask ) : this(ip , mask.getIP4SubnetPrefixSizeFromMask()) { }

			public override string ToString () => $"{_ipAddress}/{PrefixLength}";



			internal static IEnumerable<IP4AddressWithMask> ParseIPs ( string multiIPString )
			{
				MatchCollection result = _rxMaskedIP.Value.Matches(multiIPString);
				if ( result.Count < 1 )
				{
					yield break;
				}

				foreach ( Match m in result )
				{
					string ip = m.Groups[ "IP" ].Value;
					string prefixSizeString = m.Groups[ "Mask" ].Value;

					if ( !IPAddress.TryParse(ip , out IPAddress? ipa) )
					{
						continue;
					}

					uint prefixSize = PREFIX_32;
					if ( prefixSizeString.isNotNullOrWhiteSpace && uint.TryParse(prefixSizeString.Substring(1) , out var parsedPrefixSize) )
					{
						prefixSize = parsedPrefixSize;
					}

					yield return new IP4AddressWithMask(ipa! , prefixSize);
				}
				yield break;
			}





			public int CompareTo ( IP4AddressWithMask? other )
				=> (other is null)
				? 1
				: Address!.compareTo(other!.Address!);


			internal (IPAddress SubnetZeroIP , IPAddress FirstIP , IPAddress LastIP , IPAddress BroadcastIP , uint HostCount , IEnumerable<IPAddress>? SubnetHosts)
				CalculateIP4Subnet () => Address.calculateIP4Subnet(Mask);


			internal bool IsInSubnet ( uom.network.IP4AddressWithMask targetSubnet )
			{
				var localSubnet = CalculateIP4Subnet();
				var remoteSubnet = targetSubnet.CalculateIP4Subnet();
				return localSubnet.BroadcastIP.Equals(remoteSubnet.BroadcastIP);
			}


			/// <summary>Checks if host is in subnet of any local network adapter</summary>

			internal bool IsInLocalSubnet ()
			{
				foreach ( UnicastIPAddressInformation uaLocal in uom.network.Helpers.GelLocalIP4() )
				{
					uom.network.IP4AddressWithMask subnetLocal = uaLocal.toIP4AddressWithMask();
					if ( IsInSubnet(subnetLocal) ) return true;
				}
				return false;
			}











		}


		internal class IP4AddressComparer : IComparer<IPAddress>
		{

			private static readonly Lazy<IP4AddressComparer> _instance = new(new IP4AddressComparer());
			public static IP4AddressComparer Instance = _instance.Value;


			public int Compare ( IPAddress? x , IPAddress? y )
				=> CompareIPAddress(x , y);

			public static int CompareIPAddress ( IPAddress? x , IPAddress? y )
				=> x?.compareTo(y) ?? -1;

		}


		internal class IP4AddressWithMaskComparer : IComparer<string>, IComparer<IP4AddressWithMask>
		{
			private static readonly Lazy<IP4AddressWithMaskComparer> _instance = new(new IP4AddressWithMaskComparer());
			public static IP4AddressWithMaskComparer Instance = _instance.Value;

			public int Compare ( string? x , string? y )
			{
				try
				{
					IP4AddressWithMask?
						ipX = IP4AddressWithMask.ParseIPs(x ?? string.Empty).FirstOrDefault(),
						ipY = IP4AddressWithMask.ParseIPs(y ?? string.Empty).FirstOrDefault();

					return Compare(ipX , ipY);
				}
				catch { }
				return string.Compare(x , y);
			}

			public int Compare ( IP4AddressWithMask? x , IP4AddressWithMask? y )
			{
				return x is null && y is null ? 0 : x is null ? -1 : x.CompareTo(y);
			}
		}


		internal class MACComparer : IComparer<PhysicalAddress>
		{
			private static readonly Lazy<MACComparer> _instance = new(new MACComparer());
			public static readonly MACComparer Instance = _instance.Value;


			public static int CompareMAC ( PhysicalAddress? x , PhysicalAddress? y )
				=> string.Compare(x?.ToString() ?? string.Empty , y?.ToString() ?? string.Empty , StringComparison.OrdinalIgnoreCase);

			public int Compare ( PhysicalAddress? x , PhysicalAddress? y ) => CompareMAC(x , y);
		}



		//https://newbedev.com/progress-bar-with-httpclient
		internal class HttpClientDownloadWithProgress ( string downloadUrl , string destinationFilePath ) : uom.AutoDisposable.AutoDisposable1T<HttpClient>()
		{
			private readonly string _downloadUrl = downloadUrl;
			private readonly string _destinationFilePath = destinationFilePath;

			private HttpClient? _httpClient = null;

			public delegate void ProgressChangedHandler ( long? totalFileSize , long totalBytesDownloaded , double? progressPercentage );
			public event ProgressChangedHandler ProgressChanged = delegate { };
			//public event EventHandler<string> LineRead = delegate { };

			public async Task StartDownload ()
			{
				_httpClient = new HttpClient { Timeout = TimeSpan.FromDays(1) };
				Value = _httpClient;

				using var response = await _httpClient.GetAsync(_downloadUrl , HttpCompletionOption.ResponseHeadersRead);
				await DownloadFileFromHttpResponseMessage(response);
			}

			private async Task DownloadFileFromHttpResponseMessage ( HttpResponseMessage response )
			{
				response.EnsureSuccessStatusCode();
				var totalBytes = response.Content.Headers.ContentLength;
				using var contentStream = await response.Content.ReadAsStreamAsync();
				await ProcessContentStream(totalBytes , contentStream);
			}

			private async Task ProcessContentStream ( long? totalDownloadSize , Stream contentStream )
			{
				var totalBytesRead = 0L;
				var readCount = 0L;
				var buffer = new byte[ 8192 ];
				var isMoreToRead = true;

				using FileStream fileStream = new(_destinationFilePath , FileMode.Create , FileAccess.Write , FileShare.None , 8192 , true);
				do
				{
					int bytesRead = await contentStream.ReadAsync(buffer , 0 , buffer.Length);
					if ( bytesRead == 0 )
					{
						isMoreToRead = false;
						TriggerProgressChanged(totalDownloadSize , totalBytesRead);
						continue;
					}

					await fileStream.WriteAsync(buffer , 0 , bytesRead);

					totalBytesRead += bytesRead;
					readCount += 1;

					if ( readCount % 100 == 0 )
					{
						TriggerProgressChanged(totalDownloadSize , totalBytesRead);
					}
				}
				while ( isMoreToRead );
			}

			private void TriggerProgressChanged ( long? totalDownloadSize , long totalBytesRead )
			{
				if ( ProgressChanged is null )
				{
					return;
				}

				double? progressPercentage = null;
				if ( totalDownloadSize.HasValue )
				{
					progressPercentage = Math.Round((double)totalBytesRead / totalDownloadSize.Value * 100 , 2);
				}

				ProgressChanged(totalDownloadSize , totalBytesRead , progressPercentage);
			}


		}


	}


	internal static class Globalize
	{



		public const string
					PHONE_FORMAT_MOBILE1 = "###-###-###-##-##",
					PHONE_FORMAT_MOBILE2 = "###-###-###-####"
					;



		internal readonly struct CURRENCY_INFO
		{
			private readonly RegionInfo _ri;


			private CURRENCY_INFO ( RegionInfo ri ) { _ri = ri; }


			public string ISOCurrencySymbol => _ri.ISOCurrencySymbol;
			public string CurrencySymbol => _ri.CurrencySymbol;
			public string CurrencyEnglishName => _ri.CurrencyEnglishName;


			/// <summary>
			/// AUD, $, Australian Dollar <br/>
			/// CAD, $, Canadian Dollar  <br/>
			/// EUR, ?, Euro  <br/>
			/// GBP, £, British Pound  <br/>
			/// JPY, ¥, Japanese Yen  <br/>
			/// USD, $, US Dollar  <br/>
			/// </summary>
			internal static CURRENCY_INFO[] GetCurrencies ()
			{
				return [ ..
					CultureInfo
					.GetCultures(CultureTypes.SpecificCultures)
					.Select(ci => ci.LCID)
					.Distinct()
					.Select(id => new RegionInfo(id))
					.GroupBy(r => r.ISOCurrencySymbol)
					.Select(g => g.First())
					.Select(r => new CURRENCY_INFO(r)
					)
					];
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




		public static class encodings
		{

			private static bool _encodingAlreadyRegistered = false;

			/// <summary>
			/// In Net & Core we need to RegisterEncodingProvider to use encodigs like Win-1251 or other
			/// </summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void RegisterEncodings ()
			{
				if ( _encodingAlreadyRegistered ) return;
				System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
				_encodingAlreadyRegistered = true;
			}


			private static readonly System.Lazy<Encoding> _encoding_Windows1251 = new(() =>
			{
				RegisterEncodings();
				return Encoding.GetEncoding("Windows-1251");
			});
			internal static Encoding Encoding_Windows1251 => _encoding_Windows1251.Value;


			private static readonly System.Lazy<Encoding> _Encoding_cp866 = new(() =>
			{
				RegisterEncodings();
				return Encoding.GetEncoding("cp866");
			});
			internal static Encoding Encoding_cp866 => _Encoding_cp866.Value;


			private static readonly Lazy<Encoding> _encoding_UTF8WithoutBom = new(() =>
			{
				RegisterEncodings();
				return new UTF8Encoding(false);
			});
			public static Encoding Encoding_UTF8WithoutBom => _encoding_UTF8WithoutBom.Value;
		}


		public static class cultures
		{

			private static readonly Lazy<CultureInfo> _culture_ruRU = new(CultureInfo.GetCultureInfo("ru-RU"));
			private static readonly Lazy<CultureInfo> _culture_ru = new(CultureInfo.GetCultureInfo("ru"));

			public static CultureInfo culture_ru => _culture_ru.Value;
			public static CultureInfo culture_ruRU => _culture_ruRU.Value;


		}


	}


	internal static class I_O
	{

		/// <summary>
		/// Ensures file directory exist and file has correct extansion
		/// </summary>
		/// <param name="targetFile">If not specidef - will be generated via timesamp</param>
		/// <param name="targetDir">if not specidef - targetFile directory will be used</param>
		/// <param name="autogeneratedFileNameSuffix"></param>
		/// <param name="defaultFileExtension"></param>
		/// <param name="ensureFileExtensionIsCorrect">ensure targetFile extension is defaultFileExtension</param>
		/// <param name="logger"></param>
		/// <returns></returns>
		public static FileInfo PrepareForWrite (
			FileInfo? targetFile ,
			DirectoryInfo? targetDir = null ,
			Environment.SpecialFolder? defaultTargetDir = Environment.SpecialFolder.DesktopDirectory ,
			string autogeneratedFileNameSuffix = "" ,
			string defaultFileExtension = uom.constants.IOConst.FILE_EXTENSION_TMP ,
			bool ensureFileExtensionIsCorrect = false ,
			bool deleteIfExist = true ,
			ILogger? logger = null )
		{

			if ( targetFile is null && targetDir is null && defaultTargetDir is not null )
				targetDir = defaultTargetDir.Value.value;

			if ( targetFile is not null )
				targetDir = targetFile.Directory;

			ArgumentNullException.ThrowIfNull(targetDir);
			logger?.LogDebug($"Target dir '{targetDir!.FullName}', Exist: {targetDir.Exists}");
			if ( !targetDir!.Exists )
			{
				logger?.LogDebug($"Creating '{targetDir.FullName}'...");
				targetDir!.createIfNotExist();
			}

			if ( targetFile is null )
			{
				string outFileName = $"{DateTime.Now.toFileName()}_{autogeneratedFileNameSuffix}";
				logger?.LogDebug($"Target file name generated: '{outFileName}'");
				targetFile = new FileInfo(Path.Combine(targetDir.FullName , outFileName));
			}

			if ( targetFile.Extension.isNullOrWhiteSpace )
			{
				//Append .xlsx extension
				targetFile = new(targetFile.FullName + defaultFileExtension);
			}
			else if ( ensureFileExtensionIsCorrect && !targetFile.Extension.Equals(defaultFileExtension , StringComparison.OrdinalIgnoreCase) )
			{
				targetFile = new(Path.ChangeExtension(targetFile.FullName , defaultFileExtension));
			}


			logger?.LogDebug($"Target file: '{targetFile!.FullName}', Exist: {targetFile.Exists}");
			if ( deleteIfExist && targetFile!.Exists )
			{
				logger?.LogDebug($"Deleting '{targetFile.FullName}'...");
				targetFile.tryDeleteIfExist();
			}

			return targetFile;
		}





		internal enum LinkTypes : int
		{
			Unknown,
			HardLink,
			SymLink,
			Junction

		}



		/// <summary>Multithread File system scanner</summary>
		internal abstract class FileSystemScannerBase
		{


			public enum ErrorReasons : int
			{
				Unknown = 0,
				FAILED_GET_FOLDER_CONTENT,
				FAILED_READ_FILE
				//FAILED_GET_CHILD_DIRECTORIES,
			}


			#region structures


			public class ScanError ( FileSystemInfo fso , Exception ex , ErrorReasons er )
			{
				public readonly FileSystemInfo Target = fso;
				public readonly Exception Error = ex;
				public readonly ErrorReasons Reason = er;
				public object? SyncObject = null;

				public override string ToString ()
					=> $"{Target.removeLongPathPrefix()} {Reason} {Error.Message.Trim()}";
			}


			public class ScanStatus ( int waitingTasks , int finishedTasks )
			{
				public readonly int TasksToWait = waitingTasks;
				public readonly int TasksFinished = finishedTasks;
			}


			#endregion


			#region delegates


			/// <summary>Returns true to continue scan or false to break scannong</summary>
			public delegate bool EnterFolderDelegate ( DirectoryInfo dir );

			/// <summary>Returns true to continue scan or false to break scannong</summary>
			public delegate (bool ContinueScan , bool AddFoundFileToResult) FileFoundDelegate ( FileInfo file );

			#endregion


			private uom.parallel.taskSchedulers._LimitedConcurrencyLevelTaskScheduler? _lcts = null;

			protected List<Task> _tasks = [];

			private CancellationToken _cancelToken;

			protected FileSystemScannerBase () { }

			protected volatile uint ThreadsCounter = 0;


			protected void Scan (
				IEnumerable<DirectoryInfo>? folders = null ,
				int maxDegreeOfParallelism = 0 ,
				CancellationToken? ct = null
				)
			{

				if ( null == folders || !folders.Any() )
				{
					//Not specifed Dirs for scan. Using system OS drives
					var diskLetters = Environment.GetLogicalDrives();
					if ( diskLetters.Length == 0 ) throw new Exception("Failed to get OS disk drives!");

					folders = diskLetters
#if NET
						.Order()
#else
						.eSortAsArray()
#endif
						.Select(disk => disk.pathToDirectoryInfo(true))
						.ToArray();

				}

				if ( maxDegreeOfParallelism < 1 ) maxDegreeOfParallelism = Environment.ProcessorCount;

				_cancelToken = ct ?? new();
				_lcts = new(maxDegreeOfParallelism);
				_tasks = [];

				//Start Main Scan Core
				ScanDirs(folders);

				//sleep some time for background jobs starts...
				Thread.Sleep(500);

				//Wait all tasks finished...
				var tasksToWait = 1;
				while ( tasksToWait > 0 && !_cancelToken.IsCancellationRequested )
				{
					int tasksTotal = 0, tasksFinished = 0;
					lock ( _tasks )
					{
						tasksTotal = _tasks.Count;
						//Remove all finished tasks
						_tasks.RemoveAll(T => T.Status == TaskStatus.RanToCompletion);
						tasksToWait = _tasks.Count;
						tasksFinished = tasksTotal - tasksToWait;
					}
					OnWaitTasks(new ScanStatus(tasksToWait , tasksFinished));
				}

			}

			private void ScanDirs ( IEnumerable<DirectoryInfo> dirs )
			{
				//if ( _cancelToken.IsCancellationRequested ) return;
				foreach ( var dir in dirs )
				{
					if ( _cancelToken.IsCancellationRequested ) return;

					var tskNew = Task.Factory.StartNew(
							() => ProcessFolder(dir) ,
							_cancelToken ,
							TaskCreationOptions.LongRunning , _lcts!);

					lock ( _tasks )
					{
						_tasks.Add(tskNew);
					}
				}
			}


			private void ProcessFolder ( DirectoryInfo folder )
			{
				ThreadsCounter++;
				try
				{
					//lock (_ConsoleLock) Console.WriteLine($"Start scan dir '{fiDir.FullName}'");

					if ( _cancelToken.IsCancellationRequested || !OnEnterDir(folder) )
						return;

					FileSystemInfo[] allChildrens = [];
					try
					{
						allChildrens = folder.GetFileSystemInfos();
					}
					catch ( Exception exGetFiles )
					{
						if ( _cancelToken.IsCancellationRequested ) return;
						ScanError err = new(folder , exGetFiles , ErrorReasons.FAILED_GET_FOLDER_CONTENT);
						if ( !OnError(err) ) return;
					}


					try
					{
						foreach ( var fi in allChildrens.OfType<FileInfo>() )
						{
							if ( _cancelToken.IsCancellationRequested || !OnFileFound(fi) ) break;
						}
					}
					catch ( Exception exGetFiles )
					{
						if ( _cancelToken.IsCancellationRequested ) return;
						var ERR = new ScanError(folder , exGetFiles , ErrorReasons.Unknown);
						if ( !OnError(ERR) )
						{
							return;
						}
					}

					if ( _cancelToken.IsCancellationRequested ) return;

					try
					{
						DirectoryInfo[] childDirs = [ .. allChildrens.OfType<DirectoryInfo>() ];
						childDirs = OnBeforeCheckSubDirs(childDirs);
						if ( _cancelToken.IsCancellationRequested || null == childDirs || childDirs.Length == 0 ) return;
						ScanDirs(childDirs);
					}
					catch ( Exception exGetDirs )
					{
						if ( _cancelToken.IsCancellationRequested ) return;
						ScanError err = new(folder , exGetDirs , ErrorReasons.Unknown);
						if ( !OnError(err) ) return;
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
			protected virtual bool OnEnterDir ( DirectoryInfo dir )
				=> !_cancelToken.IsCancellationRequested;


			/// <summary>Occurs before SCANNER begins scan subdirectories</summary>
			/// <returns>Tou can modify array of subDirs to scan, or return null to stop</returns>
			//[MethodImpl(MethodImplOptions.Synchronized)]
			protected virtual DirectoryInfo[] OnBeforeCheckSubDirs ( DirectoryInfo[] dirs )
				=> dirs;


			/// <summary>Occurs when SCANNER found any file</summary>
			/// <returns>True to continue scan or false to stop scan</returns>
			//[MethodImpl(MethodImplOptions.Synchronized)]
			protected virtual bool OnFileFound ( FileInfo file )
				=> !_cancelToken.IsCancellationRequested;


			/// <summary>Periodicaly Occurs when some tasks started and finished
			/// Just used for write any messages showinf that app is not hang</summary>            
			protected virtual void OnWaitTasks ( ScanStatus e )
			{
				//Just waste some time to finish some tasks
				Thread.Sleep(500);
			}

			/// <summary>Occurs when SCANNER found any ERROR</summary>
			/// <returns>True to continue scan or false to stop scan</returns>
			[MethodImpl(MethodImplOptions.Synchronized)]
			protected virtual bool OnError ( ScanError err )
				=> !_cancelToken.IsCancellationRequested;

		}




		/// <summary>Multithread File Searcher</summary>
		internal class FileSearcher (
			int maxDegreeOfParallelism = 0 ,
			FileSystemScannerBase.EnterFolderDelegate? onEnterDir = null ,
			Func<DirectoryInfo[] , DirectoryInfo[]>? onBeforeCheckSubDirs = null ,
			FileSystemScannerBase.FileFoundDelegate? onFileFound = null ,
			Func<FileSystemScannerBase.ScanError , bool>? onError = null ,
			Action<FileSystemScannerBase.ScanStatus>? onStatus = null

			)
			: uom.I_O.FileSystemScannerBase()
		{


			[SuppressMessage("Style" , "IDE0028:Simplify collection initialization" , Justification = "<Pending>")]
			private readonly List<FileInfo> _result = new(1_000);
			private string[] _filesToFind = [];

			private readonly FileFoundDelegate? _cbOnFileFound = onFileFound;
			private readonly EnterFolderDelegate? _cbOnOnEnterDir = onEnterDir;
			private readonly Func<DirectoryInfo[] , DirectoryInfo[]>? _cbOnBeforeCheckSubDirs = onBeforeCheckSubDirs;
			private readonly Func<ScanError , bool>? _cbOnError = onError;
			private readonly Action<ScanStatus>? _cbOnStatus = onStatus;

			public readonly int MaxDegreeOfParallelism = maxDegreeOfParallelism;
			protected StringComparer FileNameComparer = StringComparer.OrdinalIgnoreCase;


			/// <summary>Search specifed files</summary>
			public FileInfo[] SearchFiles (
				string[] filesToFind ,
				DirectoryInfo[]? scanFolders = null ,
				StringComparer? fileNameComparer = null ,
				CancellationToken? ct = null
				)
			{
				_filesToFind = filesToFind;
				if ( _filesToFind.Any(f => f.isNullOrWhiteSpace) )
					throw new ArgumentNullException(nameof(filesToFind));

				FileNameComparer = fileNameComparer ?? StringComparer.OrdinalIgnoreCase;

				_result.Clear();
				ct ??= new();
				base.Scan(scanFolders , MaxDegreeOfParallelism , ct);

				if ( ct.Value.IsCancellationRequested ) return [];

				var files = _result
					.OrderBy(fi => fi.FullName)
					.ToArray();

				return files;
			}


			/// <summary>Search all files</summary>
			public FileInfo[] SearchAllFiles (
				DirectoryInfo[]? scanFolders = null ,
				CancellationToken? ct = null
				)
			{
				_filesToFind = [];
				_result.Clear();
				ct ??= new();
				base.Scan(scanFolders , MaxDegreeOfParallelism , ct);

				var files = _result
					.OrderBy(fi => fi.FullName)
					.ToArray();

				return files;
			}


			protected override bool OnEnterDir ( DirectoryInfo dir )
				=> base.OnEnterDir(dir) && (_cbOnOnEnterDir?.Invoke(dir) ?? true);


			protected override DirectoryInfo[] OnBeforeCheckSubDirs ( DirectoryInfo[] dirs )
				=>
				 _cbOnBeforeCheckSubDirs is null
				? base.OnBeforeCheckSubDirs(dirs)
				: _cbOnBeforeCheckSubDirs.Invoke(dirs);


			protected override bool OnFileFound ( FileInfo file )
			{
				if ( !base.OnFileFound(file) ) return false;

				if ( _filesToFind.Length != 0 && !_filesToFind.Contains(file.Name , FileNameComparer) ) return true;

				if ( _cbOnFileFound is null ) return true; // No custom found file processors attached

				(bool continueScan , bool addFoundFileToResult) = _cbOnFileFound!.Invoke(file);
				if ( addFoundFileToResult ) _result.Add(file);
				return continueScan;
			}


			protected override bool OnError ( ScanError e )
				=> _cbOnError is null
				? base.OnError(e)
				: _cbOnError.Invoke(e);


			protected override void OnWaitTasks ( ScanStatus e )
			{
				if ( null == _cbOnStatus ) { base.OnWaitTasks(e); return; }
				_cbOnStatus?.Invoke(e);
			}




			public static async Task<FileInfo[]> SearchFilesAsync (
				string[] filesToFind ,
				DirectoryInfo[]? scanFolders = null ,
				StringComparer? fileNameComparer = null ,
				FileSystemScannerBase.EnterFolderDelegate? onEnterDir = null ,
				Func<DirectoryInfo[] , DirectoryInfo[]>? onBeforeCheckSubDirs = null ,
				FileFoundDelegate? onFileFound = null ,
				Func<ScanError , bool>? onError = null ,
				Action<ScanStatus>? onStatus = null ,
				int maxDegreeOfParallelism = 0 ,
				CancellationToken? cts = null
				)

			{

				FileSearcher fs = new(
					maxDegreeOfParallelism ,
					onEnterDir ,
					onBeforeCheckSubDirs ,
					onFileFound ,
					onError ,
					onStatus);

				CancellationToken ct = cts ?? new CancellationToken();

				using var tskSearch = new Task<FileInfo[]>(() => fs.SearchFiles(filesToFind , scanFolders , fileNameComparer , ct) , ct , TaskCreationOptions.LongRunning);
				tskSearch.Start();
				var found = await tskSearch;
				return ct.IsCancellationRequested
					? []
					: found;
				;
			}

			public static async Task<FileInfo[]> SearchFileAsync (
				string fileToFind ,
				DirectoryInfo[]? scanFolders = null ,
				StringComparer? fileNameComparer = null ,
				FileSystemScannerBase.EnterFolderDelegate? onEnterDir = null ,
				Func<DirectoryInfo[] , DirectoryInfo[]>? onBeforeCheckSubDirs = null ,
				FileFoundDelegate? onFileFound = null ,
				Func<ScanError , bool>? onError = null ,
				Action<ScanStatus>? onStatus = null ,
				int maxDegreeOfParallelism = 0 ,
				CancellationToken? cts = null
				)
			{

				return await SearchFilesAsync(
					[ fileToFind ] ,
					scanFolders ,
					fileNameComparer ,
					onEnterDir ,
					onBeforeCheckSubDirs ,
					onFileFound ,
					onError ,
					onStatus ,
					maxDegreeOfParallelism ,
					cts);
			}

			public static async Task<FileInfo[]> SearchAllFilesAsync (
				DirectoryInfo[]? scanFolders = null ,
				FileSystemScannerBase.EnterFolderDelegate? onEnterDir = null ,
				Func<DirectoryInfo[] , DirectoryInfo[]>? onBeforeCheckSubDirs = null ,
				FileFoundDelegate? onFileFound = null ,
				Func<ScanError , bool>? onError = null ,
				Action<ScanStatus>? onStatus = null ,
				int maxDegreeOfParallelism = 0 ,
				CancellationToken? cts = null
			 )
			{

				return await SearchFilesAsync(
					[] ,
					scanFolders ,
					null ,
					onEnterDir ,
					onBeforeCheckSubDirs ,
					onFileFound ,
					onError ,
					onStatus ,
					maxDegreeOfParallelism ,
					cts);
			}
		}


		/// <summary>Reads lines of the file in background thread.
		/// Usable fo read any log files</summary>
		internal class BackgroundLogFileReader
		{
			public event EventHandler<string> LineRead = delegate { };
			public event EventHandler<Exception> IOError = delegate { };

			private FileInfo? _File = null;
			private StreamReader? _SR = null;

			public volatile bool StopFlag = false;

			private Thread? _thRead = null;
			private readonly ManualResetEvent _evtFinished = new(false);

			public bool DeleteFileOnFinish { get; private set; } = false;



			public BackgroundLogFileReader ( string sPath , bool bDeleteFileOnFinish = false ) : base()
			{
				DeleteFileOnFinish = bDeleteFileOnFinish;
				_File = sPath.pathToFileInfo();
				_SR = _File!.createStreamReader();
				StartCore(sPath);
			}
			public BackgroundLogFileReader ( Stream S ) : base()
			{
				DeleteFileOnFinish = false;
				_SR = S.createStreamReader();
				StartCore("[Stream]");
			}

			private void StartCore ( string ThreadName )
			{
				_thRead = new Thread(MainReadThread);
				{
					_thRead.IsBackground = true;
					_thRead.Name = $"Reading '{ThreadName}' lines in background thread...";
					_thRead.Start();
				}
			}

			private bool IsFile => _File is not null;

			private void MainReadThread ()
			{


				try
				{
					ReadStream();
				}
				catch ( Exception ex )
				{
					IOError?.Invoke(this , ex);
				}
				finally
				{
					if ( IsFile )
					{
						try { _SR!.Dispose(); } catch { }
						if ( DeleteFileOnFinish )
						{
							try { _File!.Delete(); } catch { }
						}

						_SR = null;
						_File = null;
					}
					_evtFinished.Set();
				}
			}


			private void ReadStream ()
			{
				while ( !StopFlag )
				{
					try
					{
						string? line = _SR!.ReadLine();
						if ( null != line )
						{
							LineRead?.Invoke(this , line!);
						}
						else
						{
							Thread.Sleep(100);
						}
					}
					catch { }// Just Ignore error on read single line and try read next line
				}
			}

			public void Stop ( int iTimeout = System.Threading.Timeout.Infinite )
			{
				StopFlag = true;
				_evtFinished.WaitOne(iTimeout);
			}
		}


	}









	namespace Extensions
	{




		internal static partial class Extensions_Numeric_String_Format
		{
			// (parenthesis)
			// [brackets]
			// {curly brackets}.


			//private const MidpointRounding DEFAULT_ROUNDING_RULE = MidpointRounding.ToEven;


			private const string DEFAULT_REPLACE_WITH = "";
			private const StringComparison DEFAULT_STRING_COMPARSION = StringComparison.OrdinalIgnoreCase;

			private const string DEFAULT_JOIN_SEPARATOR_STRING = " ";
			private const char DEFAULT_JOIN_SEPARATOR_CHAR = ' ';

			private static readonly string[] TIMESPAN_PARTS_EN = [ "d" , "h" , "m" , "s" , "ms" ];
			private static readonly string[] TIMESPAN_PARTS_RU = [ "д" , "ч" , "м" , "с" , "мс" ];


			internal enum AccessTimeModes
			{
				ВзятьИзДаты,
				НачалоПериода,
				КонецПериода
			}

			/// <summary>RegEx Words Count In string </summary>
			[GeneratedRegex(@"\w+")]
			private static partial Regex _rxWordsInStringF ();

			private static readonly Lazy<Regex> _rxWordsInString = new(_rxWordsInStringF());



			internal static string toAPIMiltiStringZ ( this IEnumerable<string> sources )
			{
				StringBuilder sb = new(sources.join(uom.constants.CHAR_NULL));
				sb.Append(uom.constants.CHAR_NULL);
				sb.Append(uom.constants.CHAR_NULL);
				return sb.ToString();
			}




			public class FormatCellEventArgs ( int rowIndex , int columnIndex , uint columnWidth , string cellText ) : EventArgs()
			{
				public readonly System.Drawing.Point Location = new(columnIndex , rowIndex);
				public readonly uint ColumnWidth = columnWidth;
				public readonly string CellText = cellText;


				public string AlignCenter ()
				{
					string trimmedCell = CellText.Trim();
					if ( (ColumnWidth - trimmedCell.Length) > 1 )
					{
						char[] newCellText = ' '.repeat((int)ColumnWidth).ToCharArray();
						var x = (int)((ColumnWidth - trimmedCell.Length) / 2);
						trimmedCell.ToCharArray().CopyTo(newCellText , x);
						string result = new string(newCellText)
							.PadRight((int)ColumnWidth);

						return result;
					}
					return CellText;
				}

			}
			public delegate string FormatCellDelegate ( FormatCellEventArgs ea );








			extension( ushort value )
			{
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public uom.Int16_ toInt16_ () => new(value);

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal uint MAKELPARAM ( ushort HiWord16 ) => (uint)(((uint)HiWord16 << 16) | (value & 0xFFFFL));

			}
			extension( short value )
			{
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public uom.Int16_ toInt16_ () => new(value);


			}
			extension( uint value )
			{
				#region HIWORD/LOWORD/MAKELPARAM
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal uom.Int32_ toInt32_ () => new(value);
				internal ushort loWord => new Int32_(value).ULoWord;
				internal ushort hiWord => new Int32_(value).UHiWord;
				#endregion

			}
			extension( int value )
			{
				#region HIWORD/LOWORD/MAKELPARAM
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal uom.Int32_ toInt32_ () => new(value);
				internal short loWord => new Int32_(value).LoWord;
				internal short hiWord => new Int32_(value).HiWord;
				#endregion


				/// <summary>
				/// Create Enumerable.Range(value , maximun) ]
				/// </summary>                
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public int[] createRangeTo ( int maximun ) => [ .. Enumerable.Range(value , maximun) ];


				/// <summary>Создаёт строку нулевых символов заданной длинны</summary>
				internal string toNullCharString () => new(constants.CHAR_NULL , value);



				#region From BitArray Source (https://github.com/dotnet/corefx/blob/76f566a281bbe979e80cbbb3a48ddf522cdcb4e1/src/System.Collections/src/System/Collections/BitArray.cs#L19)

				/// <summary>
				/// Used for conversion between different representations of bit array.<br/>
				/// Returns (n + (32 - 1)) / 32, rearranged to avoid arithmetic overflow.<br/>
				/// For example, in the bit to int case, the straightforward calc would be (n + 31) / 32, but that would cause overflow. So instead it's rearranged to ((n - 1) / 32) + 1.<br/>
				/// Due to sign extension, we don't need to special case for n == 0, if we use bitwise operations (since ((n - 1) >> 5) + 1 = 0). This doesn't hold true for ((n - 1) / 32) + 1, which equals 1.<br/>
				///
				/// Usage:<br/>
				/// GetArrayLength(77): returns how many ints must be allocated to store 77 bits.			
				/// </summary>
				/// <param name="bitLength"></param>
				/// <returns>how many ints are required to store n bytes</returns>
				private int getInt32ArrayLengthFromBitsCount ()
				{
					Debug.Assert(value >= 0);
					return (int)((uint)(value - 1 + (1 << BIT_SHIFT_PER_INT32)) >> BIT_SHIFT_PER_INT32);
				}


				private int getInt32ArrayLengthFromByteCount ()
				{
					Debug.Assert(value >= 0);
					// Due to sign extension, we don't need to special case for n == 0, since ((n - 1) >> 2) + 1 = 0
					// This doesn't hold true for ((n - 1) / 4) + 1, which equals 1.
					return (int)((uint)(value - 1 + (1 << BIT_SHIFT_FOR_BYTES_PER_INT32)) >> BIT_SHIFT_FOR_BYTES_PER_INT32);
				}


				private int getByteArrayLengthFromBitCount ()
				{
					Debug.Assert(value >= 0);
					// Due to sign extension, we don't need to special case for n == 0, since ((n - 1) >> 3) + 1 = 0
					// This doesn't hold true for ((n - 1) / 8) + 1, which equals 1.
					return (int)((uint)(value - 1 + (1 << BIT_SHIFT_PER_BYTE)) >> BIT_SHIFT_PER_BYTE);
				}


				private int div32Rem ( out int remainder )
				{
					uint quotient = (uint)value / 32;
					remainder = value & (32 - 1);    // equivalent to number % 32, since 32 is a power of 2
					return (int)quotient;
				}


				private int div4Rem ( out int remainder )
				{
					uint quotient = (uint)value / 4;
					remainder = value & (4 - 1);   // equivalent to number % 4, since 4 is a power of 2
					return (int)quotient;
				}


				#endregion






			}
			extension( ulong value )
			{
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public uom.Int64_ toInt64_ () => new(value);

			}
			extension( long value )
			{
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public uom.Int64_ toInt64_ () => new(value);
			}


			extension( bool? value )
			{

				[NotNull]
				public bool valueOrFalse => value.HasValue && value.Value;
				//public bool valueOrFalse => value ?? false;


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool toBool ( bool defaultValue = false ) => value ?? defaultValue;




				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string formatReadable_PlusMinus () => value.HasValue
					? value.Value
					? "+"
					: "-"
				   : "?";

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string formatReadable_1_0 () => (value ?? false) ? "1" : "0";


				//public static explicit operator bool ( bool? b ) => b.toBool();


				// Или свойство для проверки на true
				public bool isTrue => value == true;

				// Свойство для проверки на false (отличается от !IsTrue, так как учитывает null)
				public bool isFalse => value == false;

				// Проверка на наличие значения (аналог HasValue, но короче)
				public bool isSet => value.HasValue;

				// Инвертированное значение с приведением (null становится true)
				public bool not => !(value ?? false);
			}


			extension( bool b )
			{

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public int toInt32ABS () => b ? 1 : 0;

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string formatReadable_PlusMinus ()
					=> b ? "+" : "-";

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string formatReadable_1_0 ()
					=> b ? "1" : "0";



				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string formatReadable_YesNoGlobal ()
					=> uom.AppInfo.CurrentUICultureIsRuTree
						? (b ? uom.constants.GlobalizationConst.YES_RU : uom.constants.GlobalizationConst.NO_RU)
						: (b ? uom.constants.GlobalizationConst.YES_EN : uom.constants.GlobalizationConst.NO_EN);


			}




			extension<T> ( ref T currentValue ) where T : struct
			{


				/// <summary>Checks if new value is not equal to old value and updates old value to new value.</summary>
				/// <returns>true if value was updated</returns>
				//[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool updateIfNotEquals ( T newValue , Action? onUpdatedCallback = null )
				{
					if ( currentValue.Equals(newValue) ) return false;

					currentValue = newValue;
					onUpdatedCallback?.Invoke();
					return true;
				}

			}



			extension<TValue> ( TValue? value ) where TValue : INumber<TValue>
			{

				[Obsolete("!!! ensureInRange() is deprecated! use 'x.clamp()' instead !!!" , true)]
				internal TValue ensureInRange ( TValue minimum , TValue maximum )
					=> throw new NotImplementedException("ensureInRange() is deprecated! use 'x.clamp()' instead!");

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal TValue clamp ( TValue minimum , TValue maximum )
				{
					var v = value;
					try
					{
						if ( null != minimum && v!.CompareTo(minimum) < 0 )
							v = minimum;

						else if ( null != maximum && v!.CompareTo(maximum) > 0 )
							v = maximum;

						return v!;
					}
					catch
					{
						return (null != minimum)
							? minimum
							: default!;
					}
				}

				internal TValue clamp<TT> ( IEnumerable<TT> elements )
				{
					ulong
						minimum = 0,
						maximum = (ulong)elements.Count(),
						v = value?.toUInt64() ?? minimum;

					try
					{
						if ( v.CompareTo(minimum) < 0 )
							v = minimum;

						else if ( v.CompareTo(maximum) > 0 )
							v = maximum;

						//return (TValue)Convert.ChangeType(v , typeof(TValue));
					}
					catch
					{
						v = minimum;
					}
					var vv = (TValue)Convert.ChangeType(v , typeof(TValue));
					return vv;
				}
			}


			extension<TValue> ( TValue value ) where TValue : INumber<TValue>
			{


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal TValue throwIf128 ()
				{
					switch ( value )
					{

						case TValue v when v is Int128 or UInt128:
							throw new NotSupportedException("Int128 and UInt128 is not supported by this method.");
					}
					return value;
				}


				/// <inheritdoc cref="Convert.ToUInt64(object)" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal ulong toUInt64 () => Convert.ToUInt64(value.throwIf128());

				/// <inheritdoc cref="Convert.ToInt64(object)" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal long toInt64 () => Convert.ToInt64(value.throwIf128());



				/// <inheritdoc cref="Convert.ToInt64(object)" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal ulong toUInt32 () => Convert.ToUInt32(value.throwIf128());

				/// <inheritdoc cref="Convert.ToInt32(object)" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal int toInt32 () => Convert.ToInt32(value.throwIf128());


				/// <inheritdoc cref="Convert.ToInt16(object)" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal short toInt16 () => Convert.ToInt16(value.throwIf128());

				/// <inheritdoc cref="Convert.ToUInt16(object)" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal ushort toUInt16 () => Convert.ToUInt16(value.throwIf128());


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal bool toBool () => !value.toUInt64().Equals(0ul);


				internal TValue abs
				{
					get
					{

						switch ( value )
						{
							case TValue v when v is UInt128 or ulong or uint or ushort or byte:
								return value; // already positive

							case TValue v when v is long:
								return (TValue)Convert.ChangeType(Math.Abs(v.toInt64()) , typeof(TValue));

							case TValue v when v is int:
								return (TValue)Convert.ChangeType(Math.Abs(v.toInt32()) , typeof(TValue));

							case TValue v when v is short:
								return (TValue)Convert.ChangeType(Math.Abs(v.toInt32()) , typeof(TValue));

							case TValue v when v is sbyte:
								return (TValue)Convert.ChangeType(Math.Abs(Convert.ToSByte(v)) , typeof(TValue));




							case TValue v when v is decimal: //128 bit 16 bytes
								return (TValue)Convert.ChangeType(Math.Abs(Convert.ToDecimal(v)) , typeof(TValue));

							case TValue v when v is double: // 8 bytes
								return (TValue)Convert.ChangeType(Math.Abs(Convert.ToDouble(v)) , typeof(TValue));

							case TValue v when v is float: // 4 bytes
								return (TValue)Convert.ChangeType(Math.Abs(Convert.ToSByte(v)) , typeof(TValue));


							default:
								throw new NotSupportedException($"Math.Abs({typeof(TValue)}) is not supported!");
						}

						/*                 
				public static nint Abs ( nint value );                
				public static float Abs ( float value );              
				public static sbyte Abs ( sbyte value );             
				public static long Abs ( long value );               
				public static short Abs ( short value );              
				public static double Abs ( double value );            
				public static decimal Abs ( decimal value );               
				public static int Abs ( int value );               
						 */
					}

				}


			}


			extension<TValue> ( ref TValue value ) where TValue : struct, IBinaryInteger<TValue>
			{


				/// <summary>Sets specifed bit by index</summary>
				/// <param name="bitIndex">Zero-baset bit index</param>			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal void setBitRef ( int bitIndex , bool bitValue = true )
					=> value = value.setBit(bitIndex , bitValue);


			}

			//https://learn.microsoft.com/en-us/dotnet/standard/generics/math#numeric-interfaces
			//The binary integer types are Byte (byte), Int16 (short), Int32 (int), Int64 (long), Int128, IntPtr (nint), SByte (sbyte), UInt16 (ushort), UInt32 (uint), UInt64 (ulong), UInt128, and UIntPtr (nuint).            
			extension<TValue> ( TValue value ) where TValue : IBinaryInteger<TValue>
			{


				/// <summary>Чётное</summary>
				internal bool isEven => value.toUInt64() % 2 == 0;
				/// <summary>Нечётное</summary>
				internal bool isOdd => !value.isEven;




				internal bool isКратно ( TValue ЧемуКратно )
					=> (value.toUInt64() % ЧемуКратно.toUInt64()) == 0;



				#region Bits, Bytes


				/// <summary>Gets specifed bit by index</summary>
				/// <param name="bitIndex">Zero-baset bit index</param>			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal bool getBit ( int bitIndex ) => (value.toUInt64() & (1UL << bitIndex)) != 0;


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal byte[] getBytes ()
					=> BitConverter.GetBytes(value.toUInt64())[ 0..Marshal.SizeOf<TValue>() ];


				/// <summary>Reverse bytes from AABBCCDD to DDCCBBAA</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public TValue reverseBytesOrder ()
					=> value.getBytes().Reverse().ToArray().toNumber<TValue>();


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal BitArray toBitArray () => new(value.getBytes());

				/// <inheritdoc cref="getBits(BitArray)" />
				internal bool[] getBits () => value.toBitArray().getBits();


				/// <summary>Sets specifed bit by index</summary>
				/// <param name="bitIndex">Zero-baset bit index</param>			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal TValue setBit ( int bitIndex , bool bitValue = true )
				{
					ulong v64 = value.toUInt64();
					if ( bitValue )
						v64 |= 0x1UL << bitIndex;
					else
						v64 &= ~(0x1UL << bitIndex);

					//return v64.convertTo<TValue>();
					return (TValue)Convert.ChangeType(v64 , typeof(TValue));
				}


				#endregion

				#region Format...


				/// <summary>Format integer number like '1 000 000'</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string formatReadable () => $"{value:N0}".Trim();


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string formatAsStringOfBits ( bool reorderToHumanReadableView = true , char char_0 = '0' , char char_1 = '1' , int groupSize = 8 , string groupsSeparator = "-" )
					=> value.toBitArray().formatAsStringOfBits(reorderToHumanReadableView , char_0 , char_1 , groupSize , groupsSeparator);


				//[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string formatByteSizeMP ( int decimalPlaces = constants.FormatConst.DEFAULT_DECIMAL_DIGITS )
				{

					ulong valueLong = Convert.ToUInt64(value);
					double valueDouble = valueLong;

					string[] sizes = uom.AppInfo.CurrentUICultureIsRuTree
						? constants.IOConst.BYTE_SIZE_RU
						: constants.IOConst.BYTE_SIZE_EN;

					int level = 0;
					while ( valueDouble >= 1024 && (level < sizes.Length - 1) )
					{
						level++;
						valueDouble /= 1024;
					}
					// Adjust the format string to your preferences. For example "{0:0.#}{1}" would show a single decimal place, and no space.
					string format = (decimalPlaces > 0)
						? ("{0:0." + new string('0' , decimalPlaces) + "}{1}")
						: "{0:0}{1}";

					return string.Format(format , valueDouble , sizes[ level ]);
				}


				/// <summary>Возвращает строку вида '20,2%'.<br/> value must be at range 0-100</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string formatPercent ( bool alignRight = true )
				{
					var v = value.clamp(
						(TValue)Convert.ChangeType(0 , typeof(TValue)) ,
						(TValue)Convert.ChangeType(100 , typeof(TValue)));

					double doubleValue = Convert.ToDouble(v) / 100d;
					return doubleValue.formatPercent(0 , alignRight);
				}



				/// <summary>value из maxValue (20.25%)</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string formatPercentProgress ( TValue totalValue , int percentDigits = 2 , string format = "N" , bool padLeft = false , string fromString = constants.ConsoleConst.FROM )
				{
					var totalDouble = Convert.ToDouble(totalValue.throwIf128());
					string maxString = totalValue.toUInt64().ToString(format);
					string currentValueString = value.toUInt64().ToString(format).PadLeft(maxString.Length , ' ');

					float sngProgress = (totalDouble > 0)
						? (float)(value.toUInt64() / totalDouble)
						: 0f;

					return $"{currentValueString} {fromString} {maxString} ({sngProgress.formatPercent(percentDigits)})";

				}


				#endregion



				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public ulong fileSize_KBToBytes () => value.toUInt64() * constants.IOConst.BYTES_IN_KBYTE;

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public ulong fileSize_MBToBytes () => value.toUInt64() * constants.IOConst.BYTES_IN_MBYTE;



				#region Math


				/// <summary>Binary Exponentiation</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public TValue binaryPow<TExponent> ( TExponent exponent ) where TExponent : IBinaryInteger<TExponent>
				{
					var baseNumber64 = value.toUInt64();
					var exponent64 = exponent.toUInt64();

					ulong result = 1;
					while ( exponent64 > 0 )
					{
						if ( (exponent64 & 1) == 1 )
						{
							result *= baseNumber64;
						}

						baseNumber64 *= baseNumber64;
						exponent64 >>= 1;
					}

					return (TValue)Convert.ChangeType(result , typeof(TValue));
					//return result2.convertTo<TValue , TValue>();
				}




				internal float calculatePercentFrom ( TValue total )
					=> (float)(value.toUInt64() / (double)total.toUInt64());

				#endregion



			}

			extension( ulong value )
			{


				/// <summary>uses <see cref="uom.Globalize.PHONE_FORMAT_MOBILE1"/></summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string formatReadableAsPhone ()
					=> value.ToString(uom.Globalize.PHONE_FORMAT_MOBILE1).Trim();

			}



			/*
			 C# type/keyword	Approximate range	Precision	Size	.NET type
			float	±1.5 x 10−45 to ±3.4 x 1038	~6-9 digits	4 bytes	System.Single
			double	±5.0 × 10−324 to ±1.7 × 10308	~15-17 digits	8 bytes	System.Double
			decimal	±1.0 x 10-28 to ±7.9228 x 1028	28-29 digits	16 bytes	System.Decimal
			 */
			extension<TValue> ( TValue value ) where TValue : IFloatingPoint<TValue>
			{

				#region Math


				/// <inheritdoc cref="Convert.ToDouble(object)" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal double toDouble () => Convert.ToDouble(value);

				/// <inheritdoc cref="Convert.ToDecimal(object)" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal decimal toDecimal () => Convert.ToDecimal(value);

				/// <inheritdoc cref="Convert.ToSingle(object)" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal float toFloat () => Convert.ToSingle(value);

				/// <inheritdoc cref="IFloatingPoint{TValue}.Round(TValue)" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public TValue round ( int decimalPlaces )
					=> TValue.Round(value , decimalPlaces);

				/// <inheritdoc cref="IFloatingPoint{TValue}.Round(TValue)" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public TValue round ( int decimalPlaces , MidpointRounding midpointrounding )
					=> TValue.Round(value , decimalPlaces , midpointrounding);








				#endregion





				/// <summary>Format float number like '1 000 000.00'</summary>
				public string formatReadable ( int decimalPlaces = constants.FormatConst.DEFAULT_DECIMAL_DIGITS )
					=> Convert.ToDouble(value).ToString($"N{decimalPlaces}").Trim();


				/// <summary>Возвращает строку вида '20,2%'</summary>
				/// <param name="value">Значение от 0,0 до 1,0 !!!НЕ от 0 до 100!!!</param>
				public string formatPercent ( int decimalPlaces = constants.FormatConst.DEFAULT_DECIMAL_DIGITS , bool alignRight = true )
				{
					//if ( float.IsNaN(value) )                    {                        value = 0F;                    }

					var format = $"P{decimalPlaces}";
					var s = Convert.ToDouble(value).ToString(format);
					if ( alignRight )
					{
						var percent100String = ((float)1).ToString(format);
						s = s.PadLeft(percent100String.Length , ' ');
					}
					return s;
				}



				internal TValue distance_InchesToMM ()
					=> (TValue)Convert.ChangeType(value.toDouble() * constants.C_MM_IN_INCH , typeof(TValue));

				internal TValue distance_MMToInches ()
					=> (TValue)Convert.ChangeType(value.toDouble() / constants.C_MM_IN_INCH , typeof(TValue));

				internal TValue distance_InchesToCM ()
					=> (TValue)Convert.ChangeType(value.toDouble() * constants.C_CM_IN_INCH , typeof(TValue));

				internal TValue distance_CMToInches ()
					=> (TValue)Convert.ChangeType(value.toDouble() / constants.C_CM_IN_INCH , typeof(TValue));







			}

			extension( byte b )
			{

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal char toChar ( char? notReadableCharReplacement = null )
					=> (b >= FIRST_READABLE_CHAR)
					? Convert.ToChar(b)
					: notReadableCharReplacement ?? Convert.ToChar(b);



			}


			extension( IEnumerable<byte> data )
			{


				internal byte[] getOnlyReadableChars ( byte firstReadableChar = constants.FIRST_READABLE_CHAR )
					=> [ .. data.Where(B => B >= firstReadableChar) ];



				/// <inheritdoc cref="BitConverter.ToString(byte[])" />			
				/// <param name="reverseRTL">Развернуть в программисткий вид (младший байт будет справа)</param>
				/// <param name="separator">Рзделитель байтов. Если не указан, используется системный (по-умолчанию) - обычно это минус</param>
				internal string toStringHex ( bool reverseRTL = false , string? separator = null )
				{
					if ( null == data || !data.Any() ) return string.Empty;

					if ( reverseRTL ) data = (IList<byte>)data.Reverse();
					string r = BitConverter.ToString([ .. data ]);

					string defaultSeparator = constants.SystemHexByteSeparator.ToString();
					if ( null != separator && separator != defaultSeparator )
					{
						r = r.replaceEx(defaultSeparator , separator);
					}

					return r;
				}


				internal char[] toChars ( char? notReadableCharReplacement = null )
					=> [ .. data.Select(B => B.toChar(notReadableCharReplacement)) ];


				/// <summary>Возвращает строку из байт, заменяя нечинаемые символы заменителями</summary>
				/// <param name="abData">Массив байт</param>
				/// <param name="notReadableCharReplacement">Заменитель нечитаемых символов</param>
				internal string toStringAnsiEx ( char? notReadableCharReplacement = '.' )
					=> (data is null || !data.Any())
						? string.Empty
						: new string(data!.toChars(notReadableCharReplacement));

				/// <inheritdoc cref="Encoding.GetString(ReadOnlySpan{byte})" />			
				internal string toStringAnsi ()
					=> (data is null || !data.Any())
						? string.Empty
						: Encoding.ASCII.GetString([ .. data ]);

				/// <inheritdoc cref="Encoding.GetString(ReadOnlySpan{byte})" />			
				internal string toStringUni ()
					=> (data is null || !data.Any())
						? string.Empty
						: Encoding.Unicode.GetString([ .. data ]);


				/// <inheritdoc cref="BitConverter.ToInt16(ReadOnlySpan{byte})" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public short toInt16 () => BitConverter.ToInt16([ .. data ]);

				/// <inheritdoc cref="BitConverter.ToUInt16(ReadOnlySpan{byte})" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public ushort toUInt16 () => BitConverter.ToUInt16([ .. data ]);

				/// <inheritdoc cref="BitConverter.ToInt32(ReadOnlySpan{byte})" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public int toInt32 () => BitConverter.ToInt32([ .. data ]);

				/// <inheritdoc cref="BitConverter.ToInt64(ReadOnlySpan{byte})" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public uint toUInt32 () => BitConverter.ToUInt32([ .. data ]);

				/// <inheritdoc cref="BitConverter.ToUInt32(ReadOnlySpan{byte})" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public ulong toUInt64 () => System.BitConverter.ToUInt64([ .. data ]);

				/// <inheritdoc cref="BitConverter.ToUInt64(ReadOnlySpan{byte})" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public long toInt64 () => System.BitConverter.ToInt64([ .. data ]);

				/// <inheritdoc cref="BitConverter.ToUInt128(ReadOnlySpan{byte})" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public UInt128 toUInt128 () => System.BitConverter.ToUInt128([ .. data ]);

				/// <inheritdoc cref="BitConverter.ToInt128(ReadOnlySpan{byte})" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public Int128 toInt128 () => System.BitConverter.ToInt128([ .. data ]);


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public TValue toNumber<TValue> () where TValue : IBinaryInteger<TValue>
				{
					var data2 = data.ToArray()[ 0..Marshal.SizeOf<TValue>() ].ToArray();

					switch ( typeof(TValue) )
					{
						case Type t when t == typeof(short) || t == typeof(ushort):
							return (TValue)Convert.ChangeType(data2.toUInt16() , typeof(TValue));

						case Type t when t == typeof(int) || t == typeof(uint):
							return (TValue)Convert.ChangeType(data2.toUInt32() , typeof(TValue));

						case Type t when t == typeof(long) || t == typeof(ulong):
							return (TValue)Convert.ChangeType(data2.toUInt64() , typeof(TValue));

						case Type t when t == typeof(Int128) || t == typeof(UInt128):
							return (TValue)Convert.ChangeType(data2.toUInt128() , typeof(TValue));
					}

					throw new NotSupportedException($"bytes.toNumber<{typeof(TValue)}> is not supported!");
				}




			}

			extension( ReadOnlySpan<byte> data )
			{

				/// <inheritdoc cref="Encoding.GetString" />			
				internal string toStringAnsi () => Encoding.ASCII.GetString(data);

				/// <inheritdoc cref="Encoding.GetString" />			
				internal string toStringUni () => Encoding.Unicode.GetString(data);



				/// <inheritdoc cref="BitConverter.ToInt16(ReadOnlySpan{byte})" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public short toInt16 () => BitConverter.ToInt16(data);

				/// <inheritdoc cref="BitConverter.ToUInt16(ReadOnlySpan{byte})" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public ushort toUInt16 () => BitConverter.ToUInt16(data);

				/// <inheritdoc cref="BitConverter.ToInt32(ReadOnlySpan{byte})" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public int toInt32 () => BitConverter.ToInt32(data);

				/// <inheritdoc cref="BitConverter.ToUInt32(ReadOnlySpan{byte})" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public uint toUInt32 () => BitConverter.ToUInt32(data);

				/// <inheritdoc cref="BitConverter.ToUInt64(ReadOnlySpan{byte})" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public ulong toUInt64 () => System.BitConverter.ToUInt64(data);

				/// <inheritdoc cref="BitConverter.ToInt64(ReadOnlySpan{byte})" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public long toInt64 () => System.BitConverter.ToInt64(data);

				/// <inheritdoc cref="BitConverter.ToUInt128(ReadOnlySpan{byte})" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public UInt128 toUInt128 () => System.BitConverter.ToUInt128(data);

				/// <inheritdoc cref="BitConverter.ToInt128(ReadOnlySpan{byte})" />			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public Int128 toInt128 () => System.BitConverter.ToInt128(data);


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public TValue toNumber<TValue> () where TValue : IBinaryInteger<TValue>
				{
					var data2 = data.ToArray()[ 0..Marshal.SizeOf<TValue>() ].ToArray();

					switch ( typeof(TValue) )
					{
						case Type t when t == typeof(short) || t == typeof(ushort):
							return (TValue)Convert.ChangeType(data2.toUInt16() , typeof(TValue));

						case Type t when t == typeof(int) || t == typeof(uint):
							return (TValue)Convert.ChangeType(data2.toUInt32() , typeof(TValue));

						case Type t when t == typeof(long) || t == typeof(ulong):
							return (TValue)Convert.ChangeType(data2.toUInt64() , typeof(TValue));

						case Type t when t == typeof(Int128) || t == typeof(UInt128):
							return (TValue)Convert.ChangeType(data2.toUInt128() , typeof(TValue));
					}

					throw new NotSupportedException($"bytes.toNumber<{typeof(TValue)}> is not supported!");
				}





				/// <summary>
				/// From 8-bytes .NET (DateTime.ToBinary())
				/// </summary>
				public DateTime? readDate_Net ()
				{
					//var array = data.ToArray();
					if ( data is not { Length: 8 } ) return null;

					// Читаем 64-битное целое число (long) с учетом порядка байт (например, Little Endian)
					long binaryData = System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(data);

					// Восстанавливаем оригинальный DateTime
					DateTime date = DateTime.FromBinary(binaryData);
					return date;
				}

				/// <summary>
				/// From 8-bytes .NET (DateTime.Ticks)
				/// </summary>
				public DateTime? readDate_Ticks ()
				{
					//var array = data.ToArray();
					if ( data is not { Length: 8 } ) return null;

					long ticks = System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(data);
					// Создаем дату напрямую через конструктор
					return new(ticks);
				}

				/// <summary>From 8-bytes Unix Timestamp</summary>
				public DateTime? readDate_UnixTimestamp ()
				{
					//var array = data.ToArray();
					if ( data is not { Length: 8 } ) return null;

					// Пример для микросекунд (часто встречается при обмене данными с базами или другими языками)
					long unixMicroseconds = System.Buffers.Binary.BinaryPrimitives.ReadInt64BigEndian(data);

					// Прямое восстановление через встроенную эпоху в .NET
					return DateTime.UnixEpoch.AddMicroseconds(unixMicroseconds);
				}








			}


			extension( char c )
			{

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string repeat ( int Length = 70 ) => new(c , Length);

			}


			extension( IEnumerable<char> source )
			{



				internal string toString () => new([ .. source ]);

				internal string[] toStrings () => [ .. source.Select(c => c.ToString()) ];

			}


			extension( string source )
			{


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal StringBuilder toStringBuilder () => new(source);




				public string indentLines ( int indentLevel = 1 , char indentChar = '\t' )
					=> source
						.readLines()
						.Select(line => new string(indentChar , indentLevel) + line)
						.joinAsLines();


				/// <returns>{source}</returns>
				public string encloseWithCurlyBrackets ()
					=> $$"""{{{source}}}""";



				/// <summary>true if 'A' and 'a'</summary>
				internal bool isDifferOnlyByCase ( string? target )
					=> !source.isNotNullOrWhiteSpace && !target.isNotNullOrWhiteSpace
					&& source!.Equals(target , StringComparison.OrdinalIgnoreCase)
					&& !source!.Equals(target , StringComparison.Ordinal);


				#region Format





				/// <inheritdoc cref="string.Format(string, object?[])" />
				public string format ( params object[] args )
					=> string.Format(source.valueOrEmpty , args);



				/// <summary>Create string like '\\x.x.x.x\' or \\server\</summary>
				internal string formatAsWinSharePrefix () => $@"\\{source}\";



				/// <inheritdoc cref="formatAsTable" />
				public string formatLinesAsTable (
					char inColumnSeparator ,
					string outColumnSeparator = "|" ,
					char outCellPadChar = ' ' ,
					bool trimCells = true ,
					bool firstRowIsHeader = true ,
					FormatCellDelegate? cellFinalizer = null ,
					Func<string , string>? rowFinalizer = null
					)
					=> source
						.readLines()
						.formatAsTable(
						inColumnSeparator ,
						outColumnSeparator ,
						outCellPadChar ,
						trimCells ,
						firstRowIsHeader ,
						cellFinalizer ,
						rowFinalizer
						);


				#endregion






				/// <summary> преобразует первый символ каждого слова в верхний регистр, а остальные символы — в нижний. 
				/// Однако слова, состоящие только из прописных букв, считаются сокращениями и не преобразуются.
				/// Метод TextInfo.ToTitleCase учитывает регистр, то есть он использует соглашения об использовании регистров, действующие для определенного языка и региональных параметров. 
				/// Чтобы вызвать этот метод, сначала нужно получить объект TextInfo, представляющий соглашения об использовании регистров, из свойства CultureInfo.TextInfo конкретного языка и региональных параметров.
				/// 
				/// В примере ниже каждая строка из массива передается в метод TextInfo.ToTitleCase. 
				/// Среди строк есть как строки заголовков, так и сокращения. Строки преобразуются в последовательности слов, начинающихся с заглавных букв, согласно соглашениям об использовании регистров для языка и региональных параметров Английский (США). </summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string toTitleCase ()
					=> CultureInfo.CurrentCulture.TextInfo.ToTitleCase(source);


				[Obsolete("NOT OPTIMIZED!" , true)]
				internal bool isCountСontinuousAtLeast ( char c , int countAtLeast )
				{
					var cc = source.AsSpan();

					int iCharCount = 0;
					int iCharOld = -1;

					while ( iCharCount < countAtLeast )
					{
						int charPos = cc.IndexOf(c);
						//int charPos = Array.IndexOf(cc , c , iCharOld + 1);
						if ( charPos < 0 ) return false;//Not found Next
						iCharOld = charPos;
					}
					return true;//Found all
				}





				#region Wrap


				//private const string CS_ESCAPE_LF = "\n";

				/// <summary>Replacing "\n" to Environment.NewLine</summary>
				private string wrapCPP ()
					=> source.Replace(@"\n" , Environment.NewLine);

				/// <summary>Replacing "|" to Environment.NewLine</summary>
				private string wrapVB ()
					=> source.Replace('|'.ToString() , Environment.NewLine);

				/// <summary>Replacing "\n" and "|" to Environment.NewLine</summary>
				internal string wrap () => source.wrapVB().wrapCPP();


				#endregion


				#region eUnsafeEdit


				public unsafe void unsafeEdit ( int pos , char newValue )
				{
					fixed ( char* cc = source )
					{
						cc[ pos ] = newValue;
					}
				}

				public unsafe void unsafeEdit ( int charPos , Func<char , char> editor )
				{
					fixed ( char* cc = source )
					{
						cc[ charPos ] = editor.Invoke(cc[ charPos ]);
					}
				}

				public unsafe void unsafeEdit ( Func<(int charIndex , char c) , char> editor )
				{
					fixed ( char* cc = source )
					{
						for ( int charPos = 0 ; charPos < source.Length ; charPos++ )
						{
							char c = cc[ charPos ];
							cc[ charPos ] = editor.Invoke((charPos , c));
						}
					}
				}

				public unsafe void unsafeMakeFirstCharUpper ()
					=> source.unsafeEdit(0 , c => char.ToUpper(c));


				#endregion


				#region API Related / Win / DOS / NString


				/// <summary>Return all chars before null char</summary>
				internal string nString ()
				{
					string sText = source.ensureNotNullOrWhiteSpace();
					int iZeroPos = sText.IndexOf(constants.vbNullChar);
					switch ( iZeroPos )
					{
						case < 0: break; // not found
						case 0: return string.Empty; // zero at begin
						case > 0: sText = sText[ ..iZeroPos ]; break; // zero found inside string
					}
					return sText;
				}


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

				// /// <summary>Перевод строки в DOS кодировку</summary>
				// /// <param name="Строка_в_WIN_кодировке"></param>
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



				internal string convertEncoding ( string FromEncoding , string ToEncoding )
				{
					var encFrom = Encoding.GetEncoding(FromEncoding);
					var encTo = Encoding.GetEncoding(ToEncoding);
					return source.convertEncoding(encFrom , encTo);
				}

				internal string convertEncoding ( Encoding encFrom , Encoding encTo )
				{

					// #$OutputEncoding = [System.Text.Encoding]::GetEncoding("windows-1251")
					// #[Console]::OutputEncoding = [System.Text.Encoding]::GetEncoding("windows-1251")	
					// #chcp 1251 - установить кодовую страницу, соответствующую Windows-кодировке.		
					// #1251 – Windows (кириллица);
					// #866 – DOC-кодировка;
					// #65001 – UTF-8;

					var abData = encTo.GetBytes(source);
					abData = Encoding.Convert(encFrom , encTo , abData);
					return encTo.GetString(abData);
				}

				/// <summary>Преобразование текста из cp866 в windows-1251</summary>
				internal string convertEncoding_Dos_Windows_1251 ()
					=> source.convertEncoding(uom.Globalize.encodings.Encoding_cp866 , uom.Globalize.encodings.Encoding_Windows1251);



				#endregion


				#endregion


				#region indexesOf




				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal int[] indexesOf ( char target ) => [ ..
					source
					.Select(( c , i ) => c.Equals(target) ? i : -1)
					.Where(i => i != -1)
						];



				/// <inheritdoc cref="uom.mem.StringSearchCache.GetIndexes"/>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal int[] indexesOf ( string target , StringComparison comparison = StringComparison.Ordinal )
					=> uom.mem.StringSearchCache.GetIndexes(source , target , comparison);

				/// <inheritdoc cref="uom.mem.StringSearchCache.GetIndexes"/>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal int[] indexesOf_IgnoreCase ( string target )
					=> source.indexesOf(target , StringComparison.OrdinalIgnoreCase);


				/// <inheritdoc cref="uom.mem.StringSearchCache.GetFirstIndex"/>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal int indexOfFisrt ( string target , StringComparison comparison = StringComparison.Ordinal )
					=> uom.mem.StringSearchCache.GetFirstIndex(source , target , comparison);

				/// <inheritdoc cref="uom.mem.StringSearchCache.GetFirstIndex"/>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal int indexOfFisrt_IgnoreCase ( string target )
					=> uom.mem.StringSearchCache.GetFirstIndex(source , target , StringComparison.OrdinalIgnoreCase);

				/// <summary>Логика поиска с конца
				/// В .NET 10 LastIndexOf для Span<char> использует AVX-512 / Arm Neon "из коробки"
				/// Мы используем AsSpan(), чтобы избежать лишних аллокаций строк
				/// </summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal int indexOfLast ( string target , StringComparison comparison = StringComparison.Ordinal )
					=> source.isNullOrEmpty || target.isNullOrEmpty
					? -1
					: source.AsSpan().LastIndexOf(target.AsSpan() , comparison);

				#endregion

				/// <summary>
				/// Usage
				/// bool match1 = IsLike("document_v2.pdf" , "doc*.pdf"); // True
				/// bool match2 = IsLike("cat.png" , "c?t.png");
				/// </summary>
				/// <param name="template"></param>
				/// <returns></returns>
				internal bool isLike ( string template , RegexOptions options = RegexOptions.IgnoreCase )
				{
					// Escape any regex special characters in the template, 
					// then convert '*' to '.*' and '?' to '.'
					string pattern = "^" + Regex
						.Escape(template)
						.Replace(@"\*" , ".*")
						.Replace(@"\?" , ".")
						.Replace(@"\#" , @"\d")
						+ "$";

					return Regex.IsMatch(source , pattern , options);
				}


				internal bool isLikeSafe ( string template , RegexOptions options = RegexOptions.IgnoreCase , Func<Exception , bool>? onError = null )
				{
					try
					{
						return source.isLike(template , options);
					}
					catch ( Exception ex )
					{
						return onError?.Invoke(ex) ?? false;
					}
				}


			}



			/// <param name="source">Исходная строка</param>
			extension( [NotNullWhen(false)] string? source )
			{

				/// <returns>string.Empty if source is null</returns>
				[NotNull]
				public string valueOrEmpty => source ?? string.Empty;


				/// <inheritdoc cref="string.IsNullOrEmpty"/>
				[NotNull]
				public bool isNullOrEmpty => string.IsNullOrEmpty(source);


				/// <inheritdoc cref="string.IsNullOrWhiteSpace"/>
				[NotNull]//public static bool IsNullOrWhiteSpace ( [NotNullWhen(false)] String? value );
				public bool isNullOrWhiteSpace => string.IsNullOrWhiteSpace(source);


				[NotNull]
				public bool isNotNullOrWhiteSpace => !string.IsNullOrWhiteSpace(source);



				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool isNotNullOrWhiteSpaceAndStartsWith ( string findWhat , StringComparison sc = StringComparison.InvariantCultureIgnoreCase )
					=> source.isNotNullOrWhiteSpace && source!.StartsWith(findWhat , sc);

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool isNotNullOrWhiteSpaceAndEndsWith ( string findWhat , StringComparison sc = StringComparison.InvariantCultureIgnoreCase )
					=> source.isNotNullOrWhiteSpace && source!.EndsWith(findWhat , sc);


				/// <summary>If value is null - returns defaultValue, Else leave source</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string ensureNotNullOrEmpty ( string defaultValue = "" )
					=> source.isNullOrEmpty
					? defaultValue
					: source!;

				/// <summary>If value is null or WhiteSpace - returns defaultValue, Else leave source</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string ensureNotNullOrWhiteSpace ( string defaultValue = "" )
					=> source.isNullOrWhiteSpace
					? defaultValue
					: source!;


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string append ( string append , string separator = constants.vbCrLf )
				{
					string s = source.valueOrEmpty;
					if ( s.Length > 0 ) s += separator;
					return s + append;
				}


				/// <summary>Return true if any string contains in source text</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool containsAny ( IList<string> whatToFind , StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase )
				{
					if ( !source.isNullOrEmpty && whatToFind.Any() )
					{
						foreach ( var wtf in whatToFind )
						{
							if ( source!.Contains(wtf , comparisonType) ) return true;
						}
					}
					return false;
				}




				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool contains_OrdinalIgnoreCase ( string target )
					=> source?.Contains(target , StringComparison.OrdinalIgnoreCase) ?? false;

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool contains_InvariantCultureIgnoreCase ( string target )
					=> source?.Contains(target , StringComparison.InvariantCultureIgnoreCase) ?? false;

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool contains_CurrentCultureIgnoreCase ( string target )
					=> source?.Contains(target , StringComparison.CurrentCultureIgnoreCase) ?? false;



				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool equals_OrdinalIgnoreCase ( string target )
				 => source?.Equals(target , StringComparison.OrdinalIgnoreCase) ?? false;

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool equals_InvariantCultureIgnoreCase ( string target )
					=> source?.Equals(target , StringComparison.InvariantCultureIgnoreCase) ?? false;

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool equals_CurrentCultureIgnoreCase ( string target )
					=> source?.Equals(target , StringComparison.CurrentCultureIgnoreCase) ?? false;




				#region Arguments formatting



				/// <summary>Подставляет самого себя в форматированном виде, если не пустая строка</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string selfIfNotNullOrWhiteSpace ( string valueFormatString = "" , string nullValue = "" )
					=> source.isNullOrWhiteSpace
						? nullValue
						: valueFormatString.isNullOrWhiteSpace
							? source!
							: valueFormatString.format(source!);


				/// <summary>Подставляет самого себя в форматированном виде, если не пустая строка</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string appendListElementIfNotNullOrWhiteSpace ( string elementName )
					=> source.selfIfNotNullOrWhiteSpace($", {elementName}: '{{0}}'");



				#endregion



				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string reverse () => new([ .. source.valueOrEmpty.Reverse() ]);



				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal (string Result , bool StringWasTrimmed) truncateEx ( int maxLen , bool addHorizontalEllipsisChar = false )
				{
					if ( source.isNotNullOrWhiteSpace && (source!.Length > maxLen) )
					{
						string s = source[ ..maxLen ];

						if ( addHorizontalEllipsisChar )
						{
							s = source[ ..(s.Length - 1) ];
							s += (char)0x2026;
						}
						return (s , true);
					}
					return (source.valueOrEmpty , false);
				}

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string truncate ( int maxLen , bool addHorizontalEllipsisChar = false )
					=> source
					.truncateEx(maxLen , addHorizontalEllipsisChar)
					.Result
					?? string.Empty;



				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string cut ( int cutStart , int len )
					=> source.valueOrEmpty.replace(cutStart , len , string.Empty);


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string cut ( string whatToCut )
					=> source.valueOrEmpty.replaceEx(whatToCut , string.Empty);



				#region Format






				/// <returns>
				/// if (source = null) return nullValue;<br/>
				/// if (source = empty) return emptyValue;<br/>
				/// return source;
				/// </returns>
				public string formatReadable ( string nullValue = constants.NULL_TEXT , string emptyValue = "''" )
					=> (source is null)
						? nullValue
						: source.isNullOrEmpty
							? emptyValue
							: source;




				internal string formatUnicode_StrikeThrough ()
				{
					const char C_UNICODE_MODIFERS_StrikeThrough = '\u0336';

					string src = source.valueOrEmpty;


#pragma warning disable IDE0028 // Simplify collection initialization
					List<char> cc = new(src.Length * 2);
#pragma warning restore IDE0028 // Simplify collection initialization

					foreach ( var character in src )
					{
						cc.Add(character);
						cc.Add(C_UNICODE_MODIFERS_StrikeThrough);
					}
					return new string([ .. cc ]);
				}



				#endregion








				/// <summary>Добавляет пробелы перед заглавными буквами</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string insertSpacesBeforeUpperCaseChars ()
					=> source.isNullOrWhiteSpace || source!.Length < 2
						? source.valueOrEmpty
						: source.Select(c => char.IsUpper(c)
							? (" " + c.ToString())
							: c.ToString()).ToArray().join("")!.TrimStart();



				/// <summary>Возрвщвем количество одинаковых символов с начла строки, пока они совпадают с заданными</summary>
				[Obsolete("MUST BE RENAMED AND REWORKED!" , true)]
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal int takeWhile_Count ( char firstCharToSelect )
				{
					if ( source.isNullOrWhiteSpace || !source.StartsWith(firstCharToSelect.ToString()) ) return 0;

					int iCount = 0;
					foreach ( char C in source )
					{
						if ( C != firstCharToSelect )
						{
							break;
						}

						iCount += 1;
					}

					return iCount;
				}





				#region eTrimStart / eTrimEnd



				/// <inheritdoc cref="string.TrimStart(char)" />
				internal string trimStart ( string trimPrefix , bool onlyOnce = false , StringComparison comparison = DEFAULT_STRING_COMPARSION )
				{
					if ( source.isNullOrEmpty ) return string.Empty;
					if ( trimPrefix.isNullOrEmpty ) return source.valueOrEmpty;

					while ( source.StartsWith(trimPrefix , comparison) && source.Length >= trimPrefix.Length )
					{
						source = source.Substring(trimPrefix.Length);
						if ( onlyOnce ) break;
					}
					return source;
				}

				/// <inheritdoc cref="string.TrimEnd(char)" />
				internal string trimEnd ( string trimSuffix , bool onlyOnce = false , StringComparison comparison = DEFAULT_STRING_COMPARSION )
				{
					if ( source.isNullOrEmpty ) return string.Empty;
					if ( trimSuffix.isNullOrEmpty ) return source;

					while ( source.EndsWith(trimSuffix , comparison) && source.Length >= trimSuffix.Length )
					{
						int charsToTake = source.Length - trimSuffix.Length;
						if ( charsToTake == 0 ) return string.Empty;

						source = source.Substring(0 , charsToTake);
						if ( onlyOnce ) break;
					}
					return source;
				}

				internal string trimStart ( char trimPrefix , bool onlyOnce = false )
					=> source.trimStart(trimPrefix.ToString() , onlyOnce , StringComparison.OrdinalIgnoreCase);

				internal string trimEnd ( char trimSuffix , bool onlyOnce = false )
					=> source.trimEnd(trimSuffix.ToString() , onlyOnce , StringComparison.OrdinalIgnoreCase);


				#endregion





				#region Split


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal ReadOnlyMemory<char>[] splitAsMemory ( char splitChar , StringSplitOptions sso = StringSplitOptions.None )
				=>
				   source.isNullOrWhiteSpace
					? []
					: [ .. source.Split(splitChar , sso).Select(s => s.AsMemory()) ];



				/// <summary>Split string to left and right parts</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal (string LeftPart , string RightPart) splitByIndex ( int leftPartLen )
				{
					if ( source.isNullOrWhiteSpace || leftPartLen <= 0 )
						return (string.Empty , string.Empty);

					if ( leftPartLen >= source.Length )
						return (source , string.Empty);

					string left = source.Substring(0 , leftPartLen);
					string right = source.Substring(leftPartLen);
					return (left , right);
				}


				/// <summary>Split string to left and right parts</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal int splitByChar (
					char separator ,
					out ReadOnlySpan<char> LeftPart ,
					out ReadOnlySpan<char> RightPart ,
					bool skipSeparator = true
					)
				{
					var src = source.AsSpan();
					LeftPart = src;
					RightPart = [];

					if ( source.isNullOrWhiteSpace ) return -1;

					int separatorCharIndex = src.IndexOf(separator);
					if ( separatorCharIndex < 0 ) return -1;

					if ( separatorCharIndex == 0 ) return 0;

					LeftPart = src[ ..separatorCharIndex ];
					RightPart = src[ separatorCharIndex.. ];

					if ( skipSeparator )
					{
						RightPart = (RightPart.Length > 1)
							? RightPart[ 1.. ]
							: [];
					}

					return separatorCharIndex;
				}



				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal KeyValuePair<Range , Range> splitToKeyValuePair ( string separator , StringComparison comparison = DEFAULT_STRING_COMPARSION )
				{
					ArgumentNullException.ThrowIfNullOrWhiteSpace(source);

					var src = source.AsSpan();
					int separatorPos = source.indexOfFisrt(separator , comparison);
					if ( separatorPos < 1 ) throw new ArgumentException("Separator pos < 1!");

					Range key = Range.EndAt(separatorPos);
					Range value = Range.StartAt(separatorPos + separator.Length);
					return new(key , value);
				}


				/// <summary>Если строка превышает заданную длинну, то разбивает на строки этой длинны.</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string[] splitStringToFixedWidth ( int targetRowWidth = constants.ConsoleConst.CONSOLE_WIDTH_DEFAULT_1 )
				{
					ArgumentNullException.ThrowIfNullOrWhiteSpace(source);

					var aList = new List<string>();
					while ( source.Length > targetRowWidth )
					{
						string sLeftPart = source[ ..targetRowWidth ];
						aList.Add(sLeftPart);
						source = source.Substring(targetRowWidth);
					}

					aList.Add(source);
					return [ .. aList ];
				}





				#endregion




				#region Replace



				/*

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string replaceEx ( string findWhat , string replaceWith = DEFAULT_REPLACE_WITH , StringComparison comparison = DEFAULT_STRING_COMPARSION )
				{
					if ( source.isNullOrEmpty || findWhat.isNullOrEmpty ) return source ?? string.Empty;

					if ( replaceWith.Contains(findWhat , comparison) )
						throw new ArgumentException($"{nameof(replaceWith)} = '{replaceWith}', and contains search = '{findWhat}'");

					while ( source!.Contains(findWhat , comparison) )
					{
						source = source.Replace(findWhat , replaceWith , comparison);
					}
					return source;
				}
				 */


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string replaceEx ( string findWhat , string replaceWith )
				{
					if ( source.isNullOrEmpty ) return source ?? string.Empty;

					int count = source.AsSpan().Count(findWhat.AsSpan());
					if ( count == 0 ) return source ?? string.Empty;

					string valToInsert = replaceWith ?? string.Empty;
					int newLength = checked(source.Length + ((valToInsert.Length - findWhat.Length) * count));

					return newLength == 0
						? string.Empty
						: string.Create(newLength , (source , findWhat , valToInsert) , ( dest , state ) =>
						{
							ReadOnlySpan<char>
								src = state.source.AsSpan(),
								oldV = state.findWhat.AsSpan(),
								newV = state.valToInsert.AsSpan();

							Span<char> output = dest;

							while ( true )
							{
								int idx = src.IndexOf(oldV);
								if ( idx == -1 )
								{
									src.CopyTo(output);
									break;
								}

								// Копируем префикс
								src[ ..idx ].CopyTo(output);
								output = output[ idx.. ];

								// Вставляем новое (если newValue "", это просто ничего не скопирует и не сдвинет)
								if ( !newV.IsEmpty )
								{
									newV.CopyTo(output);
									output = output[ newV.Length.. ];
								}

								// Сдвигаем источник
								src = src[ (idx + oldV.Length).. ];
							}
						});
				}










				/*
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string replaceEx ( string[] findsWhat , string replaceWith = DEFAULT_REPLACE_WITH , StringComparison comparison = DEFAULT_STRING_COMPARSION )
				{
					if ( null == source || findsWhat.Length < 1 ) return source ?? string.Empty;

					var sv = SearchValues.Create(findsWhat , comparison);
					if ( !source.ContainsAny(sv) ) return source ?? string.Empty;

					var sourceSpan = source.ToCharArray().AsSpan();
					foreach ( var find in findsWhat )
					{
						//source = source.replaceEx(find , replaceWith , comparison);
						sourceSpan.Replace(find , replaceWith , comparison);
					}
					return source;
				}
				 */

				/// <summary>
				/// Почему это эффективно в .NET 10?
				/// SearchValues.Create: Рантайм анализирует массив строк. Если строк мало, он применит быстрый поиск первого символа + сравнение. Если много — построит сложный автомат (Aho-Corasick), работающий за .
				/// IndexOfAny(searcher): В .NET 10 этот метод максимально векторизован (SIMD). Он ищет сразу все возможные варианты из списка одновременно, а не по очереди.
				/// Минимум аллокаций: Мы выделяем память ровно один раз — под результирующую строку.
				/// string.Create: Позволяет нам безопасно работать с Span<char> внутри "иммутабельной" строки до того, как она будет отдана пользователю.
				/// Нюанс с перекрытиями
				/// Этот алгоритм заменяет вхождения слева направо. Если у вас есть строки "abc" и "ab", он найдет ту, которая встретится раньше. Если они начинаются в одной позиции, порядок зависит от того, как SearchValues приоритизирует совпадения (обычно по самому длинному или первому в массиве).
				/// </summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string replaceEx ( string[] oldValues , string newValue )
				{
					if ( source.isNullOrEmpty || oldValues is null || oldValues.Length < 1 ) return source ?? string.Empty;

					ReadOnlySpan<char> srcSpan = source.AsSpan();
					string valToInsert = newValue ?? string.Empty;

					// 1. Создаем оптимизированный поисковик (.NET 10 поддерживает SearchValues для строк)
					var searcher = SearchValues.Create(oldValues , StringComparison.Ordinal);

					// 2. Первый проход: считаем итоговую длину
					int count = 0;
					int totalOldLength = 0;
					int pos = 0;

					// Используем IndexOfAny для поиска любого вхождения из массива
					while ( true )
					{
						int offset = srcSpan[ pos.. ].IndexOfAny(searcher);
						if ( offset == -1 ) break;

						int matchIdx = pos + offset;

						// Нам нужно знать длину конкретной найденной строки для точного расчета
						// В .NET 10 IndexOfAny может возвращать информацию о найденном элементе
						// Для простоты найдем, какая именно строка совпала:
						foreach ( var oldVal in oldValues )
						{
							if ( srcSpan[ matchIdx.. ].StartsWith(oldVal) )
							{
								count++;
								totalOldLength += oldVal.Length;
								pos = matchIdx + oldVal.Length;
								break;
							}
						}
					}

					if ( count == 0 ) return source;

					int newLength = checked(source.Length - totalOldLength + (valToInsert.Length * count));

					// 3. Второй проход: формирование строки
					return string.Create(newLength , (source , searcher , oldValues , valToInsert) , ( dest , state ) =>
					{
						ReadOnlySpan<char> src = state.source.AsSpan();
						ReadOnlySpan<char> newV = state.valToInsert.AsSpan();
						Span<char> output = dest;

						int currentPos = 0;
						while ( true )
						{
							int offset = src[ currentPos.. ].IndexOfAny(state.searcher);
							if ( offset == -1 )
							{
								src[ currentPos.. ].CopyTo(output);
								break;
							}

							int matchIdx = currentPos + offset;

							// Копируем текст до совпадения
							int prefixLen = matchIdx - currentPos;
							src.Slice(currentPos , prefixLen).CopyTo(output);
							output = output[ prefixLen.. ];

							// Находим, что именно заменить и на сколько сдвинуть src
							int matchLength = 0;
							foreach ( var oldVal in state.oldValues )
							{
								if ( src[ matchIdx.. ].StartsWith(oldVal) )
								{
									matchLength = oldVal.Length;
									break;
								}
							}

							// Вставляем замену
							if ( !newV.IsEmpty )
							{
								newV.CopyTo(output);
								output = output[ newV.Length.. ];
							}

							currentPos = matchIdx + matchLength;
						}
					});
				}









				/*

								/// <param name="replacePairs">(FindWhat As string, ReplaceWith As string)</param>
								[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
								internal string replacePairs ( IEnumerable<(string search , string replaceWith)> replacePairs , StringComparison comparison = DEFAULT_STRING_COMPARSION )
								{
									if ( source.isNullOrEmpty ) return string.Empty;

									if ( !replacePairs.Any() ) throw new ArgumentNullException(nameof(replacePairs));

									foreach ( var frp in replacePairs )
									{
										source = source.replaceEx(frp.search , frp.replaceWith , comparison);
									}

									return source;
								}
				 */




				/// <summary>
				/// Почему это решение — "Level 10" (по версии .NET 10):
				/// Алгоритмическая сложность: Благодаря SearchValues, поиск выполняется не по очереди для каждого слова, а параллельно (на уровне логики автомата). Это критично, если в словаре сотни ключей.
				/// Zero-Heap Overhead: Весь расчет и копирование происходят в stackalloc (неявно через Span) или напрямую в память будущей строки. Мы не создаем промежуточных substring.
				/// Гибкость: Если значение в словаре — null или "", код просто удаляет найденный ключ из итоговой строки (аналогично предыдущему примеру).
				/// Безопасность: Использование string.Create гарантирует, что строка не будет изменена после создания, сохраняя принцип иммутабельности.
				/// Нюанс для профилирования:
				/// Если ваш словарь статичен (не меняется во время работы программы), вынесите SearchValues.Create в статическое поле. Это сэкономит время на построение поискового дерева при каждом вызове функции.
				/// </summary>
				/// <param name="replacements"></param>
				/// <returns></returns>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string replaceEx ( Dictionary<string , string> replacements )
				{
					if ( source.isNullOrEmpty || replacements is null || replacements.Count == 0 ) return source ?? string.Empty;

					// 1. Подготавливаем SearchValues для всех ключей словаря
					// .NET 10 оптимизирует это в высокопроизводительный поисковый автомат
					var keys = replacements.Keys.ToArray();
					var searcher = SearchValues.Create(keys , StringComparison.Ordinal);

					ReadOnlySpan<char> srcSpan = source.AsSpan();

					// 2. Первый проход: считаем итоговую длину новой строки
					int newLength = 0;
					int lastPos = 0;
					int currentPos = 0;

					while ( true )
					{
						int offset = srcSpan[ currentPos.. ].IndexOfAny(searcher);
						if ( offset == -1 ) break;

						int matchIdx = currentPos + offset;
						newLength += matchIdx - lastPos; // Длина текста до совпадения

						// Определяем, какой именно ключ сработал
						foreach ( var key in keys )
						{
							if ( srcSpan[ matchIdx.. ].StartsWith(key) )
							{
								newLength += replacements[ key ]?.Length ?? 0;
								lastPos = matchIdx + key.Length;
								currentPos = lastPos;
								break;
							}
						}
					}
					newLength += srcSpan.Length - lastPos; // Остаток строки

					if ( lastPos == 0 ) return source; // Совпадений не найдено

					// 3. Второй проход: Запись в целевой буфер
					return string.Create(newLength , (source , searcher , replacements , keys) , ( dest , state ) =>
					{
						ReadOnlySpan<char> src = state.source.AsSpan();
						Span<char> output = dest;
						int cursor = 0;

						while ( true )
						{
							int offset = src[ cursor.. ].IndexOfAny(state.searcher);
							if ( offset == -1 )
							{
								src[ cursor.. ].CopyTo(output);
								break;
							}

							int matchIdx = cursor + offset;

							// Копируем префикс
							int prefixLen = matchIdx - cursor;
							src.Slice(cursor , prefixLen).CopyTo(output);
							output = output[ prefixLen.. ];

							// Находим ключ и вставляем значение из словаря
							foreach ( var key in state.keys )
							{
								if ( src[ matchIdx.. ].StartsWith(key) )
								{
									string replacement = state.replacements[ key ];
									if ( !string.IsNullOrEmpty(replacement) )
									{
										replacement.AsSpan().CopyTo(output);
										output = output[ replacement.Length.. ];
									}
									cursor = matchIdx + key.Length;
									break;
								}
							}
						}
					});
				}


				public string replaceExIgnoreCase ( Dictionary<string , string> replacements )
				{
					if ( source.isNullOrEmpty || replacements is null || replacements.Count == 0 ) return source ?? string.Empty;

					// 1. Создаем поисковик с игнорированием регистра
					// В .NET 10 это создает максимально эффективный SIMD-автомат
					var keys = replacements.Keys.ToArray();
					var searcher = SearchValues.Create(keys , StringComparison.OrdinalIgnoreCase);

					ReadOnlySpan<char> srcSpan = source.AsSpan();

					// 2. Первый проход: Расчет длины
					int newLength = 0;
					int lastMatchEnd = 0;
					int currentSearchPos = 0;

					while ( currentSearchPos < srcSpan.Length )
					{
						int offset = srcSpan[ currentSearchPos.. ].IndexOfAny(searcher);
						if ( offset == -1 ) break;

						int matchIdx = currentSearchPos + offset;

						// Находим, какой именно ключ совпал (без учета регистра)
						foreach ( var key in keys )
						{
							if ( srcSpan[ matchIdx.. ].StartsWith(key , StringComparison.OrdinalIgnoreCase) )
							{
								newLength += matchIdx - lastMatchEnd; // Длина текста до совпадения
								newLength += replacements[ key ]?.Length ?? 0;

								lastMatchEnd = matchIdx + key.Length;
								currentSearchPos = lastMatchEnd;
								break;
							}
						}
					}
					newLength += srcSpan.Length - lastMatchEnd;

					if ( lastMatchEnd == 0 ) return source;

					// 3. Второй проход: Сборка строки
					return string.Create(newLength , (source , searcher , replacements , keys) , ( dest , state ) =>
					{
						ReadOnlySpan<char> src = state.source.AsSpan();
						Span<char> output = dest;
						int cursor = 0;

						while ( true )
						{
							int offset = src[ cursor.. ].IndexOfAny(state.searcher);
							if ( offset == -1 )
							{
								src[ cursor.. ].CopyTo(output);
								break;
							}

							int matchIdx = cursor + offset;

							// Копируем префикс (оригинальный регистр сохраняется)
							int prefixLen = matchIdx - cursor;
							src.Slice(cursor , prefixLen).CopyTo(output);
							output = output[ prefixLen.. ];

							foreach ( var key in state.keys )
							{
								if ( src[ matchIdx.. ].StartsWith(key , StringComparison.OrdinalIgnoreCase) )
								{
									string replacement = state.replacements[ key ];
									if ( !string.IsNullOrEmpty(replacement) )
									{
										replacement.AsSpan().CopyTo(output);
										output = output[ replacement.Length.. ];
									}
									cursor = matchIdx + key.Length;
									break;
								}
							}
						}
					});
				}












				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string replace ( int startIndex , int len , string replaceWith )
				{
					if ( source.isNullOrEmpty ) return string.Empty;

					string before = source[ ..startIndex ];
					string after = source[ (startIndex + len).. ];
					return before + replaceWith + after;
				}














				/*
			 // SearchValues поддерживает StringComparison при создании
		var searcher = SearchValues.Create([oldValue], StringComparison.OrdinalIgnoreCase);

		// Но для подсчета количества (Count) всё равно придется пройтись циклом через IndexOfAny
		int count = 0;
		int pos = 0;
		ReadOnlySpan<char> slice = sourceSpan;
		while ((pos = slice.IndexOfAny(searcher)) != -1)
		{
		count++;
		slice = slice[(pos + oldValue.Length)..];
		}


			 */



				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string replaceEx ( char[] findsWhat , char replaceWith )
				{
					if ( source.isNullOrEmpty || findsWhat.Length < 1 ) return source ?? string.Empty;

					var sv = SearchValues.Create(findsWhat);
					if ( !source.ContainsAny(sv) ) return source ?? string.Empty;

					var sourceSpan = source.ToCharArray().AsSpan();
					sourceSpan.ReplaceAny(sv , replaceWith);
					return sourceSpan.ToString();
				}







				/// <summary>Заменяет в тексте строки &lt;0x0A&gt; на соответствующий символ</summary>
				/// <param name="source">Текст например: aaa&lt;0x0A&gt;bbb&lt;0x0D&gt;</param>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string replaceSpecialHexChars ()
				{
					if ( source.isNullOrEmpty ) return string.Empty;

					const string CS_HEX_PREFIX = "<0x";
					Regex rx = new(CS_HEX_PREFIX + @"[0-9a-fA-F]{2}>");
					var mm = rx.Matches(source.valueOrEmpty).Cast<Match>().Reverse();
					foreach ( var m in mm )
					{
						string hex = m.Value;
						hex = hex.Substring(CS_HEX_PREFIX.Length , m.Length - CS_HEX_PREFIX.Length - 1);
						byte b = byte.Parse(hex , NumberStyles.HexNumber);
						source = source.replace(m.Index , m.Length , b.toChar().ToString());
					}
					return source;
				}




				/// <summary>Removes only Space char (0x32)</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string removeSpacesFast ()
					=> source.valueOrEmpty.Replace(" " , string.Empty);

				/// <summary>Removes all Unicode character which is categorized as white space.</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string removeSpacesEx ()
					=> string.Concat(source.valueOrEmpty.Where(c => !char.IsWhiteSpace(c)));

				/// <summary>Заменяет все множественные пробелы на один пробел</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string removeDoubleSpaces ()
					=> source.replaceEx("  " , " ");



				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string replaceCharsWithString ( char[] charsToReplace , Func<char , string> replaceFunc )
					=> string.Concat(source.valueOrEmpty
						.Select(x => charsToReplace.Contains(x)
							? replaceFunc.Invoke(x)
							: x.ToString()
							)
						.ToArray());

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string replaceCharsWithString ( char[] charsToReplace , string replaceWith , StringComparison comparison = DEFAULT_STRING_COMPARSION )
					=> source.replaceCharsWithString(charsToReplace , c => replaceWith);

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string replaceDateTimePattern ( DateTime dateToInsert )
					=> source.valueOrEmpty
					.Replace(constants.FormatConst.DATETIME_YEAR , dateToInsert.Year.ToString().PadLeft(constants.FormatConst.DATETIME_YEAR.Length , '0'))
					.Replace(constants.FormatConst.DATETIME_MONTH_NUMBER , dateToInsert.Month.ToString().PadLeft(constants.FormatConst.DATETIME_MONTH_NUMBER.Length , '0'))
					.Replace(constants.FormatConst.DATETIME_DAY , dateToInsert.Day.ToString().PadLeft(constants.FormatConst.DATETIME_DAY.Length , '0'))
					.Replace(constants.FormatConst.DATETIME_HOUR , dateToInsert.Hour.ToString().PadLeft(constants.FormatConst.DATETIME_HOUR.Length , '0'))
					.Replace(constants.FormatConst.DATETIME_MINUTE , dateToInsert.Minute.ToString().PadLeft(constants.FormatConst.DATETIME_MINUTE.Length , '0'))
					.Replace(constants.FormatConst.DATETIME_SECONDS , dateToInsert.Second.ToString().PadLeft(constants.FormatConst.DATETIME_SECONDS.Length , '0'))
					.Replace(constants.FormatConst.DATETIME_SECONDS_FRACTION , dateToInsert.Millisecond.ToString().PadLeft(constants.FormatConst.DATETIME_SECONDS_FRACTION.Length , '0'));


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string replaceЗапятыеНаТочки ()
					=> source.replaceEx("," , ".");



				#endregion






				/// <summary>Возвращает окончание строки после указанного фрагмента</summary>
				/// <param name="startWith">Вернётся отсток, после конца этой строки</param>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string substringAfter ( string startWith , StringComparison comparison = DEFAULT_STRING_COMPARSION )
				{
					if ( source.isNullOrWhiteSpace ) return string.Empty;

					if ( startWith.isNullOrWhiteSpace ) return source.valueOrEmpty;

					int pos = source.IndexOf(startWith , comparison);
					return pos < 0
						? source
						: source[ (pos + startWith.Length).. ];
				}

				/// <summary>Возвращает кусок строки между prefix и Suffix</summary>
				/// <param name="source">Исходная строка от которой выделяется остаток</param>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string substringBetween ( string prefix , string suffix , StringComparison comparison = DEFAULT_STRING_COMPARSION )
				{
					if ( source.isNullOrWhiteSpace ) return string.Empty;

					var s = source!.substringAfter(prefix , comparison);
					if ( s.isNullOrWhiteSpace || suffix.isNullOrWhiteSpace ) return string.Empty;

					int pos = s.IndexOf(suffix , comparison);
					if ( pos <= 0 ) return string.Empty;

					s = s.Substring(0 , pos);
					return s;
				}

				#region Enclose / Exclose



				/// <summary>Добавляет перед и после строки указанный символ</summary>
				/// <param name="encloseString">Строка добавляемая перед и после строки</param>
				/// <param name="notEncloseIfExist">Если строка уже окружена, то не окружать повторно</param>
				internal string enclose (
					string encloseString = constants.QUOTE ,
					bool notEncloseIfExist = true ,
					bool atStart = true ,
					bool atEnd = true )
				{
					var s = source.valueOrEmpty;

					if ( !notEncloseIfExist | !s.StartsWith(encloseString) && atStart )
						s = encloseString + s;

					if ( !notEncloseIfExist | !s.EndsWith(encloseString) && atEnd )
						s += encloseString;

					return s;
				}


				/// <summary>Добавляет перед и после строки указанный символ</summary>
				/// <param name="encloseChar">Строка добавляемая перед и после строки</param>
				/// <param name="notEncloseIfExist">Если строка уже окружена, то не окружать повторно</param>
				internal string encloseC (
					char encloseChar = constants.CHAR_QUOTE ,
					bool notEncloseIfExist = true ,
					bool atStart = true ,
					bool atEnd = true )
					=> source.enclose(encloseChar.ToString() , notEncloseIfExist , atStart , atEnd);


				/// <summary>Удаляет окружающие строку кавычки или иной окружающий текст</summary>
				/// <param name="sourceText"></param>
				/// <param name="encloseChar"></param>
				/// <param name="onlyOnePass">Только один проход</param>
				internal string exclose ( char encloseChar = constants.CHAR_QUOTE , bool onlyOnePass = false )
				{
					var s = source.valueOrEmpty;
					while ( s.isNotNullOrWhiteSpace && s.StartsWith(Convert.ToString(encloseChar)) )
					{
						s = s.TrimStart(encloseChar);
						if ( onlyOnePass ) break;
					}

					while ( s.isNotNullOrWhiteSpace && s.EndsWith(Convert.ToString(encloseChar)) )
					{
						s = s.TrimEnd(encloseChar);
						if ( onlyOnePass ) break;
					}
					return s;
				}



				#endregion






				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string normalizeHexString ( string? badSeparators = null , char? goodHexSeparator = null )
				{
					if ( source.isNullOrWhiteSpace ) return source.valueOrEmpty;

					badSeparators ??= constants.FormatConst.BAD_BYTE_SEPARATOR_CHARS;
					goodHexSeparator ??= constants.SystemHexByteSeparator;

					return source.Trim().ToUpper().replaceEx(badSeparators.ToCharArray().toStrings() , goodHexSeparator!.ToString()!);
				}


				/// <summary>Получаем массив байт из строки</summary>
				/// <param name="HexString">Строка вида: 0D-0A-2B / 43:53:51:3A / 43.53.51.3A / </param>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal byte[] hexStringToBytes ( string ByteSeparatorChars = constants.FormatConst.BAD_BYTE_SEPARATOR_CHARS )
				{
					if ( source.isNullOrWhiteSpace ) return [];

					string[] hexBytedStrings = source
						.normalizeHexString(ByteSeparatorChars , constants.SystemHexByteSeparator)
						.Trim()
						.Split(constants.SystemHexByteSeparator); // Делим по разделителю на отдельные элементы

					return hexBytedStrings.Length == 0
						? []
						: [ .. hexBytedStrings.Select(sByte => byte.Parse(sByte , NumberStyles.HexNumber)) ];
				}







			}


			extension( [NotNullWhen(false)] ReadOnlySpan<char> source )
			{


				/// <inheritdoc cref="string.IsNullOrEmpty"/>
				[NotNull]
				public bool isNullOrEmpty => source.IsEmpty;


				/// <inheritdoc cref="string.IsNullOrWhiteSpace"/>
				[NotNull]
				public bool isNullOrWhiteSpace => source.IsEmpty || source.IsWhiteSpace();


				[NotNull]
				public bool isNotNullOrWhiteSpace => !source.isNullOrWhiteSpace;




				/// <inheritdoc cref="uom.mem.StringSearchCache.GetIndexes"/>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal int[] indexesOf ( string target , StringComparison comparison = StringComparison.Ordinal )
					=> uom.mem.StringSearchCache.GetIndexes(source , target , comparison);

				/// <inheritdoc cref="uom.mem.StringSearchCache.GetIndexes"/>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal int[] indexesOf_IgnoreCase ( string target )
					=> source.indexesOf(target , StringComparison.OrdinalIgnoreCase);


				/// <inheritdoc cref="uom.mem.StringSearchCache.GetFirstIndex"/>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal int indexOfFisrt ( string target , StringComparison comparison = StringComparison.Ordinal )
					=> uom.mem.StringSearchCache.GetFirstIndex(source , target , comparison);

				/// <inheritdoc cref="uom.mem.StringSearchCache.GetFirstIndex"/>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal int indexOfFisrt_IgnoreCase ( string target )
					=> uom.mem.StringSearchCache.GetFirstIndex(source , target , StringComparison.OrdinalIgnoreCase);

				/// <summary>Логика поиска с конца
				/// В .NET 10 LastIndexOf для Span<char> использует AVX-512 / Arm Neon "из коробки"
				/// Мы используем AsSpan(), чтобы избежать лишних аллокаций строк
				/// </summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal int indexOfLast ( string target , StringComparison comparison = StringComparison.Ordinal )
					=> source.isNullOrEmpty || target.isNullOrEmpty
					? -1
					: source.LastIndexOf(target , comparison);



				/// <summary>Возвращает окончание строки после указанного фрагмента</summary>
				/// <param name="source">Исходная строка</param>
				/// <param name="startWith">Вернётся отсток, после конца этой строки</param>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal ReadOnlySpan<char> substringAfter ( string startWith , StringComparison comparison = DEFAULT_STRING_COMPARSION )
				{
					if ( source.isNullOrEmpty || startWith.isNullOrEmpty ) return source;
					int pos = source.IndexOf(startWith , comparison);
					return pos < 0
						? source
						: source[ (pos + startWith.Length).. ];
				}



				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public ReadOnlySpan<char> slice ( Range rr )
					=> source[ rr.Start.Value..rr.End.Value ];



				#region PARSE AS



				/// <summary>Создаёт объект System.Guid из строки</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal bool tryParseAsGUID ( out Guid parsedGuid )
					=> Guid.TryParse(source , out parsedGuid);

				/// <summary>If "1" return true. Othervwise returns false</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool parseAsBool_0or1 ( bool defaultValue = false )
				{
					var s = source.Trim();
					return s.Length < 1
						? defaultValue
						: s[ 0 ].Equals('1');
				}




				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public TValue parseAsNumeric<TValue> ( TValue defaultValue , NumberStyles style = uom.constants.FormatConst.DEFAULT_PARSE_NUMBER_STYLE , IFormatProvider? provider = null ) where TValue : INumber<TValue>
				  => source.isNullOrWhiteSpace
					  ? defaultValue
					  : TValue.Parse(source , style , provider);

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public TValue parseAsNumericInvariant<TValue> ( TValue defaultValue , NumberStyles style = uom.constants.FormatConst.DEFAULT_PARSE_NUMBER_STYLE ) where TValue : INumber<TValue>
					=> source.parseAsNumeric(defaultValue , style , CultureInfo.InvariantCulture.NumberFormat);

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public TValue parseAsNumericCurrent<TValue> ( TValue defaultValue , NumberStyles style = uom.constants.FormatConst.DEFAULT_PARSE_NUMBER_STYLE ) where TValue : INumber<TValue>
				  => source.parseAsNumeric(defaultValue , style , CultureInfo.CurrentCulture.NumberFormat);


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool tryParseAsNumeric<TValue> ( out TValue? parsedValue , NumberStyles style = uom.constants.FormatConst.DEFAULT_PARSE_NUMBER_STYLE , IFormatProvider? provider = null ) where TValue : INumber<TValue>
				{
					parsedValue = default;
					return !source.isNullOrWhiteSpace && TValue.TryParse(source , style , provider , out parsedValue);
				}

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool tryParseAsNumericInvariant<TValue> ( out TValue? parsedValue , NumberStyles style = uom.constants.FormatConst.DEFAULT_PARSE_NUMBER_STYLE ) where TValue : INumber<TValue>
					=> source.tryParseAsNumeric(out parsedValue , style , CultureInfo.InvariantCulture.NumberFormat);

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool tryParseAsNumericCurrent<TValue> ( out TValue? parsedValue , NumberStyles style = uom.constants.FormatConst.DEFAULT_PARSE_NUMBER_STYLE ) where TValue : INumber<TValue>
					=> source.tryParseAsNumeric(out parsedValue , style , CultureInfo.CurrentCulture.NumberFormat);




				#endregion





				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string getOnlyDigits ()
					=> new([ ..
						source
						.ToArray()
						.Where(c => char.IsDigit(c))
						]);






				#region statistics




				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal uint getWordsCount ()
				{
					uint wc = 0;
					_rxWordsInString.Value.enumerateMatches(source , _ => wc++);
					return wc;
				}

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal Range[] getWordsRanges ()
					=> _rxWordsInString.Value.matchRanges(source);


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
				internal string[] getWords ()
				{
					List<string> l = [];
					foreach ( var r in source.getWordsRanges() )
					{
						var w = source.slice(r);
						l.Add(w.ToString());
					}
					return [ .. l ];
				}


				internal (
					int MinLen ,
					int CommonPrefixLen ,
					uint CommonPrefixWordsCount ,
					string[] TotalWordsInS1 ,
					string[] TotalWordsInS2 ,
					string[] UniqueWordsInBothStrings
					)
					getEqualityMetricsTo ( string s2 )
				{

					StringComparison comparison = StringComparison.CurrentCultureIgnoreCase;

					int minLen = Math.Min(source.Length , s2.Length);

					int commonLength = source.CommonPrefixLength(s2);
					ReadOnlySpan<char> commonPrefix = source[ ..commonLength ];

					var commonPrefixWordsCount = commonPrefix.getWordsCount();
					var totalWordsInS1 = source.getWords();
					var totalWordsInS2 = s2.getWords();

					string[] uniqueWordsInBothStrings = [ ..
						totalWordsInS1
						.Distinct()
						.Where(wordFromFirstString => s2.Contains(wordFromFirstString , comparison))
						];


					return (
						minLen ,
						commonLength ,
						commonPrefixWordsCount ,
						totalWordsInS1 ,
						totalWordsInS2 ,
						uniqueWordsInBothStrings
						);
				}


				#endregion



			}


			extension( Regex rx )
			{


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void enumerateMatches ( ReadOnlySpan<char> source , Action<ValueMatch> action )
				{
					foreach ( var vm in rx.EnumerateMatches(source) )
					{
						action?.Invoke(vm);
					}
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public Range[] matchRanges ( ReadOnlySpan<char> source )
				{
					List<Range> l = [];
					rx.enumerateMatches(source , vm => l.Add(new Range(vm.Index , vm.Index + vm.Length)));
					return [ .. l ];
				}



			}

			extension( Span<char> source )
			{
				/*

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public Span<char> slice ( Range rr )                     => source[ rr.Start.Value..rr.End.Value ];
				 */

			}

			extension( ReadOnlyMemory<char> source )
			{

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public ReadOnlySpan<char> slice ( Range rr )
					=> source.Span[ rr.Start.Value..rr.End.Value ];

			}

			extension( Range rr )
			{

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public ReadOnlySpan<char> slice ( ReadOnlyMemory<char> source )
					=> source.Span[ rr.Start.Value..rr.End.Value ];


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public Span<char> slice ( Span<char> source )
					=> source[ rr.Start.Value..rr.End.Value ];


			}

			extension( MemoryExtensions.SpanSplitEnumerator<char> rr )
			{

				/// <summary>Split в .NET 10 через Span для скорости</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public Range[] toArray ()
				{
					List<Range> l = [];
					foreach ( var r in rr )
					{
						l.Add(r);
					}
					return [ .. l ];
				}


			}









			extension( IList<string> rows )
			{


				/// <inheritdoc cref="formatReadable(float,int)" />
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string formatAsTable (
					char inColumnSeparator ,
					string outColumnSeparator = "|" ,
					char outCellPadChar = ' ' ,
					bool trimCells = true ,
					bool firstRowIsHeader = true ,
					FormatCellDelegate? cellFinalizer = null ,
					Func<string , string>? rowFinalizer = null
					)

				{

					string[][] array2D = [ .. rows.Select(row => row.Split(inColumnSeparator)) ];

					var maxColumnCount = array2D
						.Select(cc => cc.Length)
						.Max();

					//Modyfing table
					array2D = array2D
						.Select(rowCells =>
						{
							rowCells = [ ..
							rowCells.Select(cellValue =>
								(trimCells && !cellValue.isNullOrEmpty)
								? cellValue.Trim()
								: cellValue
							) ];

							string[] fullRow = [];
							if ( rowCells.Length == maxColumnCount )
								fullRow = rowCells;
							else
							{
								// new string[ maxColumnCount ];
								fullRow = new string[ maxColumnCount ];
								rowCells.CopyTo(fullRow , 0);
							}
							return fullRow;
						}
						)
						.ToArray();

					//Counting Max cell len for each column
					var maxColumnLen = Enumerable
						.Range(0 , maxColumnCount)
						.Select(columnIndex => array2D
							.Select(rowCells => rowCells[ columnIndex ].Length)
							.Max())
						.ToArray();

					//Creating final table
					var outTableRows = array2D.Index()
						.Select(row =>
						{
							var rowCells = row.Item.Index().Select(cc =>
							{
								var columnIndex = cc.Index;
								var colWidth = maxColumnLen[ columnIndex ];
								string cell = cc.Item;
								if ( cell.Length < colWidth )
								{
									cell = cell.PadRight(colWidth , outCellPadChar);
								}


								if ( cellFinalizer is not null )
								{
									FormatCellEventArgs ea = new(row.Index , columnIndex , (uint)colWidth , cell);
									cell = cellFinalizer.Invoke(ea);
								}

								return cell;
							})
							.ToArray();

							var rowString = rowCells.join(outColumnSeparator.ToString())!;
							if ( rowFinalizer is not null )
							{
								rowString = rowFinalizer.Invoke(rowString);
							}
							return rowString;
						}
						)
						.ToList();



					if ( firstRowIsHeader )
					{
						//Table header
						string headerSeparator = '-'.repeat(outTableRows[ 0 ].Length);

						foreach ( var col in maxColumnLen.Take(maxColumnLen.Length - 1) )
						{
							headerSeparator.unsafeEdit(col , '+');
						}

						if ( outTableRows.Count > 1 )
							outTableRows.Insert(1 , headerSeparator);
						else
							outTableRows.Add(headerSeparator);
					}

					var result = outTableRows.joinAsLines();
					return result;
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal bool containsOrdinalIgnoreCase ( string whatToFind ) => rows.Contains(whatToFind , StringComparer.OrdinalIgnoreCase);

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal bool containsCurrentCultureIgnoreCase ( string whatToFind ) => rows.Contains(whatToFind , StringComparer.CurrentCultureIgnoreCase);

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal bool containsInvariantCultureIgnoreCase ( string whatToFind ) => rows.Contains(whatToFind , StringComparer.InvariantCultureIgnoreCase);


				/// <summary>Создаёт одну строку (REG_MULTI_SZ) из элементов массива (где каждая строка отделена [0] а в конце [00])</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string toAPIMultiStringZ ()
					=> rows.join("\0") + "\0\0";


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal StringCollection toStringCollection ()
					=> [ .. rows ];

			}


			extension( IEnumerable<string>? rows )
			{

				/// <inheritdoc cref="string.Join(string?, IEnumerable{string?})"/>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string join (
					string separator = DEFAULT_JOIN_SEPARATOR_STRING ,
					bool skipNullOrWhiteSpaces = true ,
					string listEmptyValue = "" ,
					bool enquote = false
					)
				{
					if ( !(rows?.any ?? false) ) return listEmptyValue;

					if ( skipNullOrWhiteSpaces ) rows = [ .. rows.Where(s => !s.isNullOrWhiteSpace) ];
					if ( enquote ) rows = [ .. rows.Select(s => '"' + s + '"') ];

					return string.Join(separator , rows);
				}

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string join (
				   char separator ,
				   bool skipNullOrWhiteSpaces = true ,
				   string listEmptyValue = "" ,
				   bool enquote = false
				   )
				{
					if ( !(rows?.any ?? false) ) return listEmptyValue;

					if ( skipNullOrWhiteSpaces ) rows = [ .. rows.Where(s => !s.isNullOrWhiteSpace) ];
					if ( enquote ) rows = [ .. rows.Select(s => '"' + s + '"') ];

					return string.Join(separator , rows);
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string joinAsLines () => rows.join(Environment.NewLine)!;






			}


			extension( StringCollection? source )
			{
				/// <inheritdoc cref="string.Join(string?, IEnumerable{string?})"/>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string join (
					string separator = DEFAULT_JOIN_SEPARATOR_STRING ,
					bool skipNullOrWhiteSpaces = true ,
					string listEmptyValue = "" ,
					bool enquote = false
					)
					=> (null == source)
						? string.Empty
						: source!.Cast<string>().join(separator , skipNullOrWhiteSpaces: skipNullOrWhiteSpaces , listEmptyValue: listEmptyValue , enquote: enquote)!;

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal List<string> toList ()
					=> [ .. source?.Cast<string>() ?? [] ];



			}












			extension( Regex rx )
			{


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool tryEnumerateMatchesFirstOrEmpty ( ReadOnlySpan<char> source , out ReadOnlySpan<char> value )
				{
					value = null;
					if ( source.isNullOrEmpty ) return false;
					var me = rx.EnumerateMatches(source);
					me.MoveNext();
					var vm = me.Current;
					if ( vm.Length < 1 )
					{
						value = string.Empty;
						return false;
					}
					value = source.Slice(vm.Index , vm.Length);
					return true;
				}



			}


			extension( Guid source )
			{
				/// <returns>{Guid}</returns>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string encloseWithCurlyBrackets ()
					=> source.ToString().encloseWithCurlyBrackets();


				public string upperCaseString => source.encloseWithCurlyBrackets().ToUpper();
				public string lowerCaseString => source.encloseWithCurlyBrackets().ToLower();

			}


			extension( TimeSpan source )
			{

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string formatReadable ( bool useMilliseconds = false )
				{
					string[] parts = uom.AppInfo.CurrentUICultureIsRuTree
						? TIMESPAN_PARTS_RU
						: TIMESPAN_PARTS_EN;

					StringBuilder sb = new(200);
					if ( source.Days > 0 )
						sb.Append($"{source.Days}{parts[ 0 ]},");

					if ( source.Hours > 0 )
						sb.Append($"{source.Hours}{parts[ 1 ]}:");

					if ( source.Minutes > 0 )
						sb.Append($"{source.Minutes}{parts[ 2 ]}:");

					if ( source.Seconds > 0 )
						sb.Append($"{source.Seconds}{parts[ 3 ]}");

					if ( useMilliseconds )
						sb.Append($":{source.Milliseconds}{parts[ 4 ]}");

					return sb
						.ToString()
						.replaceEx("::" , ":")
						.Trim()
						.TrimEnd(':')
						.TrimEnd(',');
				}


			}

			extension( DateTime src )
			{
				// Usage
				//var today = DateOnly.Today;
				//public static DateOnly eToday => DateOnly.FromDateTime( DateTime.Today );

				/// <summary>Следующий День</summary>
				public DateTime nextDay => src.Date.AddDays(1);

				public DateTime endOfTheDay => src.nextDay.Date.AddSeconds(-1);
				public DateTime startOfTheDay => src.Date;



				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public DateTime removeSeconds ()
					=> new(src.Year , src.Month , src.Day , src.Hour , src.Minute , 0);

				/// <summary>"dd MMM yyyy, HH:mm:ss"</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string toLongDateTimeString () => src.ToString(constants.FormatConst.DATETIME_FORMAT_LONG);

				/// <summary>"dd MMM yyyy, HH:mm:ss.fff"</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string toLongDateTimeStamp () => src.ToString(constants.FormatConst.DATETIME_FORMAT_LONG_DATE_TIMESTAMP);

				/// <summary>"HH:mm:ss.fff"</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string toLongTimeStamp () => src.ToString(constants.FormatConst.DATETIME_FORMAT_LONG_TIMESTAMP);

				/// <summary>"yyyy-MM-dd__HH-mm-ss-fff"</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string toFileName () => src
					.ToString(constants.FormatConst.DATETIME_FORMAT_LONG_FILE_DATE_TIMESTAMP)
					.pathToSafeFileNameString()
					.Trim();



				#region To MS Access Format


				public string toAccessFormat ( AccessTimeModes mode = AccessTimeModes.ВзятьИзДаты )
				{
					var dt = mode switch
					{
						AccessTimeModes.НачалоПериода => src.Date,
						AccessTimeModes.КонецПериода => src.nextDay.AddSeconds(-1),//  new DateTime(src.Year , src.Month , src.Day , 23 , 59 , 59),
						_ => throw new NotImplementedException(),
					};

					const string
						ACCESS_NUMBER_FORMAT_2 = "00",
						ACCESS_NUMBER_FORMAT_4 = "0000";

					return string.Format("#{1}/{0}/{2} {3}:{4}:{5}#" ,
						dt.Day.ToString(ACCESS_NUMBER_FORMAT_2) ,
						dt.Month.ToString(ACCESS_NUMBER_FORMAT_2) ,
						dt.Year.ToString(ACCESS_NUMBER_FORMAT_4) ,
						dt.Hour.ToString(ACCESS_NUMBER_FORMAT_2) ,
						dt.Minute.ToString(ACCESS_NUMBER_FORMAT_2) ,
						dt.Second.ToString(ACCESS_NUMBER_FORMAT_2));
				}


				#endregion



				public DateOnly dateOnly
					=> DateOnly.FromDateTime(src);


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public TimeSpan subtractAbs ( DateTime value )
					=> (src >= value ? src : value).Subtract(src < value ? src : value);


			}

			extension( DateOnly? dt )
			{


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public DateTime? toDateTime ()
					=> dt?.ToDateTime(TimeOnly.MinValue);


			}



			#region MAKEFOURCC


			/// <summary>
			/// https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/d3dukmdt/nf-d3dukmdt-makefourcc
			/// </summary>
			internal enum FourCC_D3DTextureCodes : byte
			{
				/// <summary>Alpha</summary>
				A,

				/// <summary>Red</summary>
				R,

				/// <summary>Green</summary>
				G,

				/// <summary>Blue</summary>
				B,

				/// <summary>Unused Bits</summary>
				X,

				/// <summary>Palette</summary>
				P,

				/// <summary>Luminance</summary>
				L,

				/// <summary>dU coordinate for BumpMap</summary>
				U,

				/// <summary>dV coordinate for BumpMap</summary>
				V,

				/// <summary>Stencil</summary>
				S,

				/// <summary>Depth(e.g.Z or W buffer)</summary>
				D,

				/// <summary>Computed from other channels (typically on certain read operations)</summary>
				C
			}


			internal static uint MAKEFOURCC ( this FourCC_D3DTextureCodes ch0 , FourCC_D3DTextureCodes ch1 , FourCC_D3DTextureCodes ch2 , FourCC_D3DTextureCodes ch3 )
			{
				FourCC_D3DTextureCodes[] cc = [ ch0 , ch1 , ch2 , ch3 ];
				var fccChars = cc.Select(ch => ch.ToString()[ 0 ]).ToArray();
				return MAKEFOURCC(fccChars[ 0 ] , fccChars[ 1 ] , fccChars[ 2 ] , fccChars[ 3 ]);
			}


			/// <summary>
			/// https://pypi.org/project/makefourcc/
			/// </summary>
			internal static uint MAKEFOURCC ( this char ch0 , char ch1 , char ch2 , char ch3 )
			{
				char[] cc = [ ch0 , ch1 , ch2 , ch3 ];
				byte[] fccBytes = [ ..
					cc.Select(ch => (byte)(int)ch)
					];
				return MAKEFOURCC(fccBytes[ 0 ] , fccBytes[ 1 ] , fccBytes[ 2 ] , fccBytes[ 3 ]);
			}

			internal static uint MAKEFOURCC ( this byte ch0 , byte ch1 , byte ch2 , byte ch3 )
				=> ch0 | (uint)(ch1 << 8) | (uint)(ch2 << 16) | (uint)(ch3 << 24);



			#endregion


			#region BitArray / Bits





			/// <summary>Divides value to divideTo with rounding to the nearest max int</summary>

			internal static uint eDivideWithRoundToMax ( this uint value , uint divideTo )
			{
				uint floor = value % divideTo;
				uint newVal = value / divideTo;
				if ( floor > 0 )
				{
					newVal++;
				}

				return newVal;
			}

			/// <summary>Divides value to divideTo with rounding to the nearest max int</summary>

			internal static int eDivideWithRoundToMax ( this int value , int divideTo )
			{
				int floor = value % divideTo;
				int newVal = value / divideTo;
				if ( floor > 0 )
				{
					newVal++;
				}

				return newVal;
			}


			/// <summary>Переводит биты в числовое значение. Количество байт, рассчитывается как число_бит/8</summary>

			internal static byte[] eGetBytes ( this BitArray ba )
			{
				//var bytesCount = (int)Math.Round(Math.Ceiling(ba.Count / 8f));
				int bytesCount = ba.Count.eDivideWithRoundToMax(8);
				var abMaxIP = new byte[ bytesCount ];
				ba.CopyTo(abMaxIP , 0);
				return abMaxIP;
			}


			/// <summary>Переводит биты в числовое значение. Количество байт, рассчитывается как число_бит/8</summary>

			internal static byte[] getBytes ( this IEnumerable<bool> bits ) => new BitArray(bits.ToArray()).eGetBytes();


			/// <summary>Returns bits array in double reversed mode (See Bitarray), lower bit starts (0x1 = '10000000')</summary>
			internal static bool[] getBits ( this BitArray ba )
			{
				return [ .. ba.Cast<bool>() ];
				/*
				bool[] bytes = new bool[ba.Length];
				ba.CopyTo( bytes , 0 );
				return bytes;
				 */
			}
			//internal static bool[] eGetBits(this BitArray ba) => [.. ba.Cast<bool>()];




			internal static void eSetBitsFromStartToEnd ( this BitArray bits , int startIndex , int endIndex , bool value = true )
			{
				for ( int i = startIndex ; i < endIndex ; i++ )
				{
					bits.Set(i , value);
				}
			}


			internal static void eSetBitsFromStart ( this BitArray bits , int startIndex , int count , bool value = true )
				=> bits.eSetBitsFromStartToEnd(startIndex , startIndex + count , value);



			internal static byte[] eSetBitsFromStartToEnd ( this byte[] bytes , int startIndex , int endIndex , bool value = true )
			{
				BitArray bits = new(bytes);
				bits.eSetBitsFromStartToEnd(startIndex , endIndex , value);
				bits.CopyTo(bytes , 0);
				return bytes;
			}


			internal static byte[] eSetManyFromStart ( this byte[] bytes , int startIndex , int count , bool value = true )
				=> bytes.eSetBitsFromStartToEnd(startIndex , startIndex + count , value);




			/// <summary>Returns string of bitss like 0000-0001</summary>
			/// <param name="groupSize">Octet size which will be saparated with groupsSeparator</param>

			internal static string formatAsStringOfBits ( this BitArray ba , bool reorderToHumanReadableView = true , char char_0 = '0' , char char_1 = '1' , int groupSize = 8 , string groupsSeparator = "-" )
			{
				bool[] aBoolValues = ba.getBits();
				Span<bool> bitsSpan = new(aBoolValues);

				List<string> octetsList = [];
				int slicePos = 0;
				do
				{
					Span<bool> slice = bitsSpan.Slice(slicePos , groupSize);
					bool[] bits = [ .. slice ];
					char[] cc = [ .. bits.Select(b => b ? char_1 : char_0) ];
					string byteBitsString = new(cc);
					octetsList.Add(byteBitsString);
					slicePos += groupSize;
				}
				while ( slicePos < bitsSpan.Length );

				string r = octetsList.join(groupsSeparator).Trim();
				if ( reorderToHumanReadableView )
					r = r.reverse();

				return r;
			}


			internal static System.Numerics.BigInteger eToBigInteger ( this uint value ) => new(value);

			#endregion










			#region Srtings Byte Functioms


			/// <inheritdoc cref="System.Convert.ToBase64String(byte[])"/>

			public static string eToBase64String ( this byte[] data ) => Convert.ToBase64String(data);

			/// <inheritdoc cref="System.Convert.FromBase64String"/>

			public static byte[] eFromBase64String ( this string base64String ) => Convert.FromBase64String(base64String);




			internal static byte[] eGetBytes_Default ( this string sData ) => Encoding.Default.GetBytes(sData);


			internal static byte[] eGetBytes_ASCII ( this string sData ) => Encoding.ASCII.GetBytes(sData);


			internal static byte[] eGetBytes_Unicode ( this string sData ) => Encoding.Unicode.GetBytes(sData);


			internal static string eGetBytes_Unicode_ToBase64String ( this string sData ) => Convert.ToBase64String(sData.eGetBytes_Unicode());


			#endregion


		}


		internal static partial class Extensions_Debug_Dump
		{




			internal static string dumpHex ( this byte[] data , ulong startAddress = ulong.MinValue , int elementsInLine = 16 )
			{
				if ( data is null || data.Length < 1 )
				{
					return "[NULL OR EMPTY]";
				}

				StringBuilder sbResult = new();

				int fullLineLen = BitConverter.ToString(new string('-' , elementsInLine).eGetBytes_ASCII()).Length;
				ulong rowAddress = startAddress;
				int addressLen = Marshal.SizeOf(rowAddress) * 2;

				mem.SlidingWindow<byte> sw = new(data) { WindowSize = elementsInLine };
				do
				{
					sbResult.Append(rowAddress.ToString("X").PadLeft(addressLen , '0')); //Address
					sbResult.Append('|');
					sbResult.Append(sw.WindowData.ToArray().toStringHex().PadRight(fullLineLen , '-')); //Hex Data
					sbResult.Append('|');
					sbResult.Append(new string(sw.WindowData.Select(b => b.toChar('.')).ToArray())); // Data as readable text
					sbResult.AppendLine();
					rowAddress += (ulong)elementsInLine;

				} while ( sw.SlideRight(true) );

				return sbResult.ToString();
			}


			internal static string dumpHex ( this IntPtr lpBuffer , int nBytes )
				=> lpBuffer
					.toBytes_Marshal(nBytes)
					.dumpHex((ulong)lpBuffer.ToInt64());


			internal static string dumpHex ( this Span<byte> abData , ulong startAddress = ulong.MinValue , int elementsInLine = 16 )
				=> abData
				.ToArray()
				.dumpHex(startAddress , elementsInLine);


			/// <summary>
			/// sbyte, byte, short, ushort, int, uint, long, ulong, nint, nuint, char, float, double, decimal, or bool
			/// Any enum type
			/// Any pointer type
			/// Any user-defined struct type that contains fields of unmanaged types only.
			/// </summary>
			private static string dumpValueToString<T> ( this T? value ,
				[System.Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? valueName = null ) where T : unmanaged
			{

				string val = "null";
				if ( value is not null )
				{
					val = $"[{typeof(T)}]";
					val += $": '{value}'";
				}
				return $"{valueName} = {val}";
			}

			private static string dumpValueToString<T> ( this string? value ,
				[System.Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? valueName = null )
			{
				string val = "null";
				if ( value is not null )
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
					  if (a is null )
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


			internal static string dumpObjectToString<T> ( this T? value , [CallerArgumentExpression(nameof(value))] string? valueName = null , params string[] excludeMembers ) where T : class
			{
				if ( value is null )
				{
					return $"{valueName} = null";
				}

				StringBuilder sb = new(2048);

				System.Type t = value.GetType();
				sb.AppendLine($"{valueName} = {t}:");

				MemberInfo[] members = t.GetMembers()
					.OrderBy(m => m.Name)
					.Where(m => !excludeMembers.containsOrdinalIgnoreCase(m.Name))
					.ToArray();



				if ( value is System.MarshalByRefObject )
				{
					//int gggg = 9;
					//System.MarshalByRefObject mro = value;

				}



				string RWAsString ( bool r , bool w )
				{
					string rr = r ? "R" : string.Empty;
					string ww = w ? "W" : string.Empty;
					string slash = (r && w) ? @"/" : string.Empty;
					return $"{rr}{slash}{ww}";
				}

				foreach ( MemberInfo mi in members )
				{
					string memberDump = "";
					try
					{
						object? objMemberValue = null;

						switch ( mi )
						{
							case PropertyInfo pi:
								objMemberValue = pi.GetValue(value);
								memberDump = $"Property '{mi.Name}'({RWAsString(pi.CanRead , pi.CanWrite)})[{pi.PropertyType}]";
								break;

							case FieldInfo fi:
								objMemberValue = fi.GetValue(value);
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
						if (memberDump is null ) continue;
						 */
						memberDump += $" = '{objMemberValue ?? string.Empty}'";

						bool isOwn = mi.DeclaringType == t;
						if ( !isOwn )
						{
							memberDump = $"->[inherit from: {mi.DeclaringType}]-> " + memberDump;
						}
					}
					catch ( Exception ex )
					{
						memberDump = $"{mi.Name}: {ex.Message}";
					}

					memberDump = "\t" + memberDump;
					sb.AppendLine(memberDump);

				}
				return sb.ToString().TrimEnd();
			}



			internal static string dumpStaticMembers ( this Type t , params string[] excludeMembers )
			{
				StringBuilder sb = new(2048);

				sb.AppendLine($"{t}:");

				MemberInfo[] members = t.GetMembers(BindingFlags.Public | BindingFlags.Static)
					.OrderBy(m => m.Name)
					.Where(m => !excludeMembers.containsOrdinalIgnoreCase(m.Name))
					.ToArray();

				foreach ( MemberInfo mi in members )
				{
					string memberDump = "";
					try
					{
						object? objMemberValue = null;

						switch ( mi )
						{
							case PropertyInfo pi:
								objMemberValue = pi.GetValue(null);
								memberDump = $"Property '{mi.Name}'[{pi.PropertyType}]";
								break;

							case FieldInfo fi:
								objMemberValue = fi.GetValue(null);
								memberDump = $"Field '{fi.Name}'[{fi.FieldType}]";

								break;

							default:
								continue;
						}


						memberDump += $" = '{objMemberValue ?? string.Empty}'";

						bool isOwn = mi.DeclaringType == t;
						if ( !isOwn )
						{
							memberDump = $"->[inherit from: {mi.DeclaringType}]-> " + memberDump;
						}
					}
					catch ( Exception ex )
					{
						memberDump = $"{mi.Name}: {ex.Message}";
					}
					memberDump = "\t" + memberDump;
					sb.AppendLine(memberDump);
				}

				return sb.ToString().TrimEnd();
			}



			internal static string dumpObjectMembersTreeToString<T> ( this T? value , [CallerArgumentExpression(nameof(value))] string? valueName = null ) where T : class
			{

				if ( value is null )
				{
					return $"{valueName} = null";
				}

				StringBuilder sb = new();

				//WARNING!!! NOT WORKING!!!
				//return sb.ToString();

				System.Type t = value.GetType();
				sb.AppendLine($"{valueName} = {t}:");

				MemberInfo[] members = t.GetMembers()
					.OrderBy(m => m.Name)
					.ToArray();

				foreach ( MemberInfo mi in members )
				{
					string memberDump = "";
					try
					{

						memberDump = mi switch
						{
							PropertyInfo pi => pi.GetValue(value).dumpObjectToString(pi.Name),
							FieldInfo fi => fi.GetValue(value).dumpObjectToString(fi.Name),
							_ => ""
						};

						if ( memberDump.isNullOrWhiteSpace )
						{
							continue;
						}

						bool isOwn = mi.DeclaringType == t;
						if ( !isOwn )
						{
							memberDump = $"->[{mi.DeclaringType}]-> " + memberDump;
						}
					}
					catch ( Exception ex )
					{
						memberDump = $"{mi.Name}: {ex.Message}";
					}

					memberDump = "\t" + memberDump;
					sb.AppendLine(memberDump);

				}
				return sb.ToString();
			}



			private const int C_DEFAULT_ARRAY_DUMP_ITEMS_COUNT = 100;



			internal static string dumpArrayToString<T> (
				this IEnumerable<T>? src ,
				string itemSeparator = "," ,
				string beforeValuesPrefix = "" ,
				int limitArrayItemsOutput = C_DEFAULT_ARRAY_DUMP_ITEMS_COUNT ,
				bool outVariableName = true ,
				bool outVariableType = true ,
				[System.Runtime.CompilerServices.CallerArgumentExpression(nameof(src))] string? arrayName = null
				)
			{
				if ( src is null ) return $"{arrayName} = null";
				//if ( !src.Any() ) return $"{arrayName} = {(outVariableType ? typeof(T).ToString() : string.Empty)}[0]: EMPTY";

				StringBuilder result = new();
				if ( outVariableName )
				{
					result.Append($"{arrayName}:");
				}

				if ( outVariableType )
				{
					result.Append($"{typeof(T)}:");
				}

				result.Append($"[{src.Count()}]");

				if ( !src.any )
				{
					result.Append("EMPTY");
					return result.ToString();
				}

				if ( limitArrayItemsOutput > 0 && src.Count() > limitArrayItemsOutput )
				{
					src = src.Take(limitArrayItemsOutput).ToArray();
					result.Append($":FIRST:{limitArrayItemsOutput}");
				}

				result.Append("=");
				result.Append(beforeValuesPrefix);

				if ( src is byte[] data )
				{
					result.Append(data.toStringHex());
				}
				else if ( src is string[] strArr )
				{
					result.Append(strArr.join(itemSeparator , enquote: true)!);
				}
				else
				{
					string[] arrayElementValues = [ .. src.Select(arrayElement => arrayElement?.ToString() ?? "NULL") ];
					result.Append(arrayElementValues.join(itemSeparator)!);
				}

				return result.ToString();
			}


			/*

		internal static string eDumpArrayToString2(
		   this System.Array? src,
		   string itemSeparator = ",",
		   int limitArrayItemsOutput = C_DEFAULT_ARRAY_DUMP_ITEMS_COUNT
		   )
		{
		   if (src is null ) return "null";


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
		   result += l.ToArray().join(itemSeparator)!;
		   return result;
		}
				  */





		}



		internal static partial class Extensions_RegEx
		{


			public static TValue parseValueAsNumeric<TValue> ( this GroupCollection g , string groupName , TValue defaultValue , NumberStyles style = NumberStyles.None ) where TValue : INumber<TValue>
				=> (g[ groupName ].Value ?? "").parseAsNumericInvariant(defaultValue , style);


			public static TValue parseValueAsNumeric<TValue> ( this Match mx , string groupName , TValue defaultValue , NumberStyles style = NumberStyles.None ) where TValue : INumber<TValue>
				=> mx.Groups.parseValueAsNumeric<TValue>(groupName , defaultValue , style);



			extension( string source )
			{

				public bool tryRegexMatch ( [StringSyntax("Regex")] string pattern , out Match? m )
				{
					m = Regex.Match(source , pattern);
					return m is { Success: true };
				}

			}


		}



		internal static partial class Extensions_Arrays_List_Enumerable_Dictionary
		{



			extension<TList> ( TList? source ) where TList : IList, ICollection, IEnumerable
			{

				/// <summary>Null-Safe Any</summary>'
				[NotNull]
				public bool any => (source is not null) && (source.Count > 0);

				/// <summary>Checks source to null or not ANY()</summary>
				[NotNull]
				internal bool isEmptyOrNull => (null == source) || !source.any;

				[NotNull]
				internal bool isNotEmptyOrNull => !source.isEmptyOrNull;


				internal IEnumerable<TItem> toIEnumerable<TItem> ()
					=> source?.Cast<TItem>() ?? [];

			}


			extension<TItem> ( IEnumerable<TItem>? source )
			{
				/// <summary>Null-Safe Any</summary>'
				[NotNull]
				public bool any => (source is not null) && source.Any();

				/// <summary>Checks source to null or not ANY()</summary>
				[NotNull]
				internal bool isEmptyOrNull => (null == source) || !source.any;

				[NotNull]
				internal bool isNotEmptyOrNull => !source.isEmptyOrNull;


				/// <summary>Return source or Empty {source} if source is null</summary>
				internal IEnumerable<TItem> orEmptyIfNull () => source?.AsEnumerable() ?? [];

				/// <summary>Return source or Empty {source} if source is null</summary>
				internal TItem[] orEmptyArrayIfNull () => [ .. source?.AsEnumerable() ?? [] ];


				internal bool containsAnyOf ( IEnumerable<TItem>? targets )
					=> source is not null
					&& (targets?.Any(target => source.Contains(target)) ?? false);


				public void forEach ( Action<TItem>? action )
				{
					if ( source is null || action is null ) return;
					foreach ( TItem item in source )
					{
						action!.Invoke(item);
					}
				}

				#region UInt64 inplementation of net 9 IEnumerable.Index()


				private IEnumerable<(ulong Index , TItem Item)> getIndexIteratorU64 ()
				{
					var index = 0ul;
					foreach ( var element in source?.orEmptyIfNull() ?? [] )
					{
						yield return (index , element);

						checked
						{
							index++;
						}
					}
				}


				/// <summary>Returns an enumerable that incorporates the element's index into a tuple.</summary>
				/// <typeparam name="Item">The type of the elements of <paramref name="source" />.</typeparam>				
				/// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>

				public IEnumerable<(ulong Index , TItem Item)> asIndexedU64 ()
					=> source is null
						? throw new ArgumentNullException(nameof(source))
						: source!.getIndexIteratorU64();


				#endregion

				/*
		internal TSource[] removeFirst ( int countOfRemodevFirstItems = 1 )
		{
			TSource[] arrInput = [ .. source ];
			TSource[] cutinput = new TSource[ arrInput.Length - countOfRemodevFirstItems ];
			Array.Copy(arrInput , countOfRemodevFirstItems , cutinput , 0 , cutinput.Length);
			return cutinput;
		}


		internal TSource[] removeLast ( int countOfRemodevFirstItems = 1 )
		{
			List<TSource> l = [ .. source ];
			if ( countOfRemodevFirstItems > l.Count ) countOfRemodevFirstItems = l.Count;

			for ( int n = 1 ; n <= countOfRemodevFirstItems ; n++ )
			{
				l.RemoveAt(l.Count - 1);
			}

			return [ .. l ];
		}
		 */


				public TItem[] removeRange ( int index , int count )
				{
					if ( source is null ) return [];

					List<TItem> l = [ .. source ];
					l.RemoveRange(index , count);
					return [ .. l ];
				}


			}

			extension<TItem> ( IEnumerable<TItem> source ) where TItem : IComparable<TItem>
			{


				public int indexOfComparable ( TItem itemToSearch )
				{
					int index = 0;
					foreach ( TItem item in source!.orEmptyIfNull() )
					{
						if ( item.CompareTo(itemToSearch!) == 0 ) return index;
						index++;
					}
					return -1;
				}


				public IEnumerable<TItem> sortComparable ()
					=> source is null
						? []
						: [ .. from N in source orderby N select N ];


				public TItem[] sortComparableArray ()
					=> [ .. source.sortComparable() ];

				public List<TItem> sortComparableList ()
					=> [ .. source.sortComparable() ];


			}

			extension<TSource> ( IEnumerable<TSource> source ) where TSource : class
			{

				public int indexOf ( TSource itemToSearch )
				{
					var found = source
						.orEmptyIfNull()
						.Index()
						.FirstOrDefault(element => element.Item.Equals(itemToSearch));

					return (found.Item is not null)
							? found.Index
							: -1;
				}

			}


			extension( IEnumerable<string> sourceStrings )
			{
				public string[] sortStrings ( StringComparer? comparer = null )
				{
					string[] srcArr = [ .. sourceStrings ];
					Array.Sort(srcArr , comparer ?? StringComparer.CurrentCultureIgnoreCase);
					return srcArr;
				}

			}




			/// <summary>Creates 1 element array with specifed item</summary>
			public static T[] toArrayFromSingleElement<T> ( this T? source )
				=> (null == source)
				? []
				: [ source ];


			extension<TKey, TValue> ( Dictionary<TKey , TValue> dic ) where TKey : notnull
			{


				public ConcurrentDictionary<TKey , TValue> toConcurrentDictionary ()
					=> new([ .. dic ]);


				public bool isDictionaryEqualTo ( Dictionary<TKey , TValue>? target )
					=> ReferenceEquals(dic , target)
					|| (target is not null &&
							dic
							.OrderBy(x => x.Key)
							.SequenceEqual(target.OrderBy(x => x.Key)));



				/// <summary>
				/// Returns true if value was added. Returns false if value was updated
				/// </summary>
				/// <returns>Returns true if value was added. Returns false if value was updated</returns>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool addOrUpdate ( TKey key , TValue value )
				{
					if ( !dic.TryAdd(key , value) )
					{
						dic[ key ] = value;
						return false;
					}
					return true;
				}

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal void addOrUpdateRange ( Dictionary<TKey , TValue> source )
				{
					foreach ( var kvp in source )
					{
						dic.addOrUpdate(kvp.Key , kvp.Value);
					}
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal void addOrUpdateRange ( IEnumerable<KeyValuePair<TKey , TValue>> source , bool clearBefore = false )
				{
					if ( clearBefore ) dic.Clear();
					foreach ( var kvp in source )
					{
						dic.addOrUpdate(kvp.Key , kvp.Value);
					}
				}


				/// <summary>Sample:
				/// <code>
				/// var val2 = dic.getOrAdd (1334,"234");
				/// </code>
				/// </summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public TValue? getOrAdd ( TKey key , TValue? value , out bool exist )
				{
					ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(dic , key , out exist);
					if ( exist ) return val;

					val = value;
					return value;
				}


				/// <summary>Sample:
				/// <code>
				/// tryUpdate  (1334,"234");
				/// </code>
				/// </summary>
				public bool tryUpdateUnsafe ( TKey key , TValue? value )
				{
					ref var val = ref CollectionsMarshal.GetValueRefOrNullRef(dic , key);
					if ( Unsafe.IsNullRef(ref val) ) return false;
					val = value;
					return true;
				}






				/// <summary>Возвращает массив строк, где каждая строка = 'ключ = значение'</summary>
				/// <param name="dic"></param>
				/// <param name="skipNullValues"></param>
				/// <param name="nullValuesDisplayName"></param>
				/// <returns></returns>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string[] toArrayOfKeyValuePairStrings (
					bool skipNullValues = true ,
					string nameValueSeperator = " = " ,
					string nullValuesDisplayName = "[NULL]" )
					=> dic
					.Select<KeyValuePair<TKey , TValue> , string?>(kvp =>
					{
						if ( kvp.Value is null && skipNullValues ) return null;

						object val = kvp.Value!;

						var vt = val.GetType();
						if ( vt.IsGenericType && vt.GetGenericTypeDefinition() == typeof(Nullable<>) )
						{
							//TODO: Some code need
						}

						return $"{kvp.Key}{nameValueSeperator}{val.ToString() ?? nullValuesDisplayName}";

					})
					.Where(s => null != s)
					.ToArray()!;




			}


			extension<TValue> ( Dictionary<string , TValue> dic )
			{

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static Dictionary<string , TValue> newWithOrdinalIgnoreCaseComparer ()
					=> [ with(StringComparer.OrdinalIgnoreCase) ];


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static Dictionary<string , TValue> newWithCurrentCultureIgnoreCaseComparer ()
					=> [ with(StringComparer.CurrentCultureIgnoreCase) ];


			}




			extension<TKey, TValue> ( System.Collections.Concurrent.ConcurrentDictionary<TKey , TValue> source ) where TKey : notnull
			{

				/// <inheritdoc cref="System.Collections.Concurrent.ConcurrentDictionary{TKey, TValue}.AddOrUpdate" />
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void addOrUpdate ( TKey key , TValue newValue )
					=> source.AddOrUpdate(key , newValue , ( _ , oldValue ) => newValue);

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool remove ( TKey key )
					=> source.Remove(key , out _);
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string[] toArray ( this StringCollection sc )
				=> [ .. sc ];




			// /// <summary>Очень медленно!</summary>
			// <DebuggerNonUserCode, DebuggerStepThrough>
			// <MethodImpl(MethodImplOptions.AggressiveInlining), System.Runtime.CompilerServices.Extension()>
			// Friend Function  eCreateArray2(Of T)(tfillWithItems As T, ItemsCount As Integer) As T()
			// Dim aResult = Enumerable.Range(1, ItemsCount).Select(Function(X) tfillWithItems).ToArray
			// Return aResult
			// End Function


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static T[] append<T> ( this T[] arr , T[] arrAppend )
			{
				List<T> l = [ .. arr ];
				l.AddRange(arrAppend);
				return [ .. l ];
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void removeRange<T> ( this List<T> l , IEnumerable<T> whatToRemove )
			{
				foreach ( T i in whatToRemove )
				{
					l.Remove(i);
				}
			}


			/// <summary>Считываем первый элемент списка, и удаляем его из списка (как в стеке)</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static T? peekFirstOrDefault<T> ( this List<T> L )
			{
				var firstItem = L.FirstOrDefault();
				if ( firstItem is not null )
				{
					L.RemoveAt(0);
				}

				return firstItem;
			}



			internal static T selectSwitch<T> ( this int ReturnIndex , params T[] ReturnValues )
				=> ReturnValues[ ReturnIndex ];






			/*

			internal enum NEAREST_SEARCH_MODES : int
			{
				eSmallerOrEqual,
				eSmallerOnly,
				eLargerOrEqual,
				eLargerOnly
			}

			/// <summary>Находит индекс в массиве ближайшего меньшего или большего числа</summary>

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
			//
			//internal static IEnumerable<T> eShift<T>(this IEnumerable<T> ArrayToShift)
			//{
			//    _ = ArrayToShift ?? throw new ArgumentNullException(nameof(ArrayToShift));
			//    if (!ArrayToShift.Any()) return Array.Empty<T>();

			//    var L = ArrayToShift.ToList();
			//    L.RemoveAt(0);
			//    return L;
			//}
			/*

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

			#region Compare Arrays

			/// <summary>Compare Arrays using processor SIMD acceleration and direct memory pointers
			/// The best results is for arrays of Int16, Int32, Int64, float,double.
			/// <br/>
			/// <c>
			/// !!! For compare binary arrays - the fastest methos is to use <see cref="compare_MemCmp_IsEqual"/> !!!"
			/// </c>
			/// </summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool compareIsEqual_SIMD<T> ( this Span<T> a , Span<T> b ) where T : unmanaged, IComparable<T>
			{
				if ( a.Length != b.Length )
				{
					return false;
				}

				int vectorSize = Vector<T>.Count;
				int numVectors = a.Length / vectorSize;
				int ceiling = numVectors * vectorSize;


				ReadOnlySpan<Vector<T>> leftVecArray = MemoryMarshal.Cast<T , Vector<T>>(a);
				ReadOnlySpan<Vector<T>> rightVecArray = MemoryMarshal.Cast<T , Vector<T>>(b);

				for ( int i = 0 ; i < numVectors ; i++ )
				{
					if ( !System.Numerics.Vector.EqualsAll(leftVecArray[ i ] , rightVecArray[ i ]) )
					{
						return false;
					}
				}

				// Finish operation with any numbers leftover
				for ( int i = ceiling ; i < a.Length ; i++ )
				{
					if ( a[ i ].CompareTo(b[ i ]) != 0 )
					{
						return false;
					}
				}
				return true;
			}

			/// <inheritdoc cref="compareIsEqual_SIMD" />
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool compareIsEqual_SIMD<T> ( this Memory<T> a , Memory<T> b ) where T : unmanaged, IComparable<T>
				=> a.Span.compareIsEqual_SIMD<T>(b.Span);



			/// <inheritdoc cref="compareIsEqual_SIMD" />
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool compareIsEqual_SIMD_Byte ( this ReadOnlySpan<byte> a , ReadOnlySpan<byte> b )
			{
				if ( a.Length != b.Length ) return false;

				int vectorSize = Vector<byte>.Count;
				int numVectors = a.Length / vectorSize;
				int ceiling = numVectors * vectorSize;

				ReadOnlySpan<Vector<byte>> leftVecArray = MemoryMarshal.Cast<byte , Vector<byte>>(a);
				ReadOnlySpan<Vector<byte>> rightVecArray = MemoryMarshal.Cast<byte , Vector<byte>>(b);
				var zeroVector = Vector<byte>.Zero;

				for ( int i = 0 ; i < numVectors ; i++ )
				{
					//if ( leftVecArray[ i ] != rightVecArray[ i ] ) return false;

					//Comparing two vectors by XOR them. This must be very fast. Result must be zero if vectors is equal                    
					if ( (leftVecArray[ i ] ^ rightVecArray[ i ]) != zeroVector ) return false;
				}

				if ( ceiling != a.Length )
				{
					// Finish operation with any numbers leftover
					return a[ ceiling.. ].SequenceEqual(b[ ceiling.. ]);
					/*
					for ( int i = ceiling ; i < a.Length ; i++ )
					{
						if ( a[ i ] != b[ i ] ) return false;
					}
					 */
				}
				return true;
			}


			/// <inheritdoc cref="compareIsEqual_SIMD_Byte" />
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool compareIsEqual_SIMD_Byte ( this ReadOnlyMemory<byte> a , ReadOnlyMemory<byte> b )
				=> a.Span.compareIsEqual_SIMD_Byte(b.Span);



			/// <summary>Использование интерфейса <see cref="System.Collections.IStructuralEquatable"/> - Это новый способ, появился только в NET_4</summary>'
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool compareIsEqual_StructuralEquatable<T> ( this T[] arrA , T[] arrB ) where T : IStructuralEquatable, IComparable<T>
				=> arrA.Length == arrB.Length
				&& (arrA.Length == 0 || (arrA as IStructuralEquatable).Equals(arrB , StructuralComparisons.StructuralEqualityComparer));


			/// <summary>
			/// Решение на основе векторов из System.Numerics
			/// Теоретически должно работать с аппаратным ускорением, но надо чтобы размер массива был не менее системного размера вектора)
			/// https://docs.microsoft.com/ru-ru/dotnet/api/system.numerics.vector-1?view=net-6.0
			/// </summary>'
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool compareIsEqual_Vector<T> ( this T[] arrA , T[] arrB ) where T : struct
			{
				if ( arrA.Length != arrB.Length )
				{
					return false;
				}

				int platformVectorSize = System.Numerics.Vector<T>.Count;   //On x64 Windows platformVectorSize = 32 byte
																			//bool hwa = System.Numerics.Vector.IsHardwareAccelerated;

				System.Numerics.Vector<T> va, vb;
				int i = 0;

				// Compare main body by blocks,
				// with block size = hardware accelerated platformVectorSize
				int iMax = arrA.Length - platformVectorSize;
				for ( ; i <= iMax ; i += platformVectorSize )
				{
					va = new(arrA , i);
					vb = new(arrB , i);
					if ( !System.Numerics.Vector.EqualsAll(va , vb) )
					{
						return false;
					}
				}

				// Compare Tail
				if ( arrA.Length < platformVectorSize )
				{
					Array.Resize(ref arrA , platformVectorSize);
					Array.Resize(ref arrB , platformVectorSize);
					i = 0;
				}
				else
				{
					i = arrA.Length - platformVectorSize;
				}
				va = new(arrA , i);
				vb = new(arrB , i);

				return System.Numerics.Vector.EqualsAll(va , vb);
			}


			#endregion





		}


		internal static partial class Extensions_ListsSync
		{

			#region SYNC Engine

			internal delegate bool IsEqualDelegate<TOld, TNew> ( TOld OldItem , TNew NewItem );

			internal delegate void UpdateOldItemDelegate<TOld, TNew> ( TOld OldItem , TNew NewItem );

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

			internal static void eSync<TCurrent, TNew> (
				this IEnumerable<TCurrent> OldList ,
				IEnumerable<TNew> NewList ,
				IsEqualDelegate<TCurrent , TNew> CompareFunc ,
				UpdateOldItemDelegate<TCurrent , TNew>? UpdateOldItemCallback = null ,
				Action<TCurrent>? OnItemObsolete = null ,
				Action<TNew>? OnNewItemNeedToAdd = null )
			{
				if ( !OldList.Any() && !NewList.Any() )
				{
					return;
				}

				// В старом списке ищем записи, кторорых нет в новых данных, для пометки как устаревшие и возможного удаления
				foreach ( var rOld in OldList )
				{
					// Для каждого существующего элемента проверяем есть ли он в новом списке
					var OldRecordsNeedToBeUpdated = NewList.Where(rNew => CompareFunc.Invoke(rOld , rNew));
					if ( OldRecordsNeedToBeUpdated.Any() )
					{
						// New list contains any elements as in Old list
						if ( UpdateOldItemCallback is not null )
						{
							OldRecordsNeedToBeUpdated.forEach(rNewItem => UpdateOldItemCallback?.Invoke(rOld , rNewItem));// Обновляем элемент старого списка новыми значениями
						}
					}
					else // В новом списке нет этого старого элемента
					{
						OnItemObsolete?.Invoke(rOld);// Сообщаем что элемент устарел (РЕШИТЬ ВОПРОС С ВОЗМОЖНОСТЬЮ УДАЛЕНИЯ!!!) Внутри вызова можно удалять элемент, т.к. щас смотрим array-копию старого списка и это безопасно
					}
				}

				// Ищем новые элементы, которых нет в старом списке
				foreach ( var rNew in NewList )
				{
					// Для каждого элемента нового списка проверяем есть ли он в старом списке
					var aНовыйЭлеменНайденВСтаромСписке = OldList.Where(rOld => CompareFunc.Invoke(rOld , rNew));
					if ( aНовыйЭлеменНайденВСтаромСписке.Any() )
					{
						//
					}
					// В старом списке есть такой же элемент (элементы уже обновлены в первом абзаце)
					else // В старом списке нет этого нового элемента
					{
						OnNewItemNeedToAdd?.Invoke(rNew);
					}
				}
			}
			#endregion

		}


		internal static partial class Extensions_Enum
		{


			/*
			public static unsafe TEnum toEnumFastUnsafe<TEnum> ( this Int64 value ) where TEnum : struct, Enum
				=> Unsafe.As<long , TEnum>(ref value);

			public static unsafe TEnum toEnumFastUnsafe<TEnum> ( this ulong value ) where TEnum : struct, Enum
				=> Unsafe.As<ulong , TEnum>(ref value);

			 */


			public static TEnum toEnumFastUnsafe<TValue, TEnum> ( this TValue value ) where TEnum : struct, Enum where TValue : INumber<TValue>//, allows ref struct
			{
				//int size = Marshal.SizeOf(Enum.GetUnderlyingType(typeof(TEnum)));

				var enumSize = Marshal.SizeOf(Enum.GetUnderlyingType(typeof(TEnum)));
				if ( enumSize != Marshal.SizeOf<TValue>() )
					throw new NotSupportedException($"The size of source value '{nameof(value)}'({Marshal.SizeOf<TValue>()} bytes) must be equal to the size of the target enum type '{typeof(TEnum)}'({enumSize} bytes)!");

				//throw new ArgumentOutOfRangeException(nameof(value) , $"sizeof(TEnum) != sizeof(TInt)!");
				//: Unsafe.As<TEnum , TEnum>(ref value);

				unsafe
				{
					return *(TEnum*)(void*)&value;
				}

				/*
				switch ( value )
				{
					case byte byt: return Unsafe.As<byte , TEnum>(ref byt);

					case Int16 i16: return Unsafe.As<Int16 , TEnum>(ref i16);
					case ushort ui16: return Unsafe.As<ushort , TEnum>(ref ui16);

					case Int32 i: return Unsafe.As<Int32 , TEnum>(ref i);
					case uint ui: return Unsafe.As<uint , TEnum>(ref ui);

					case Int64 i64: return Unsafe.As<Int64 , TEnum>(ref i64);
					case ulong ui64: return Unsafe.As<ulong , TEnum>(ref ui64);

					case Int128 i128: return Unsafe.As<Int128 , TEnum>(ref i128);
					case UInt128 ui6128: return Unsafe.As<UInt128 , TEnum>(ref ui6128);

					default: throw new NotImplementedException();
				}
				Int64 longValue = value.toInt64();
				return Unsafe.As<long , TEnum>(ref longValue);
				 */
			}


			/*
			public static unsafe TEnum toEnum<TEnum> ( this long value ) where TEnum : struct, Enum
			{
				switch ( sizeof(TEnum) )
				{
					case 1: byte b = (byte)value; return Unsafe.As<byte , TEnum>(ref b);
					case 2: short s = (short)value; return Unsafe.As<short , TEnum>(ref s);
					case 4: int i = (int)value; return Unsafe.As<int , TEnum>(ref i);
					case 8: return Unsafe.As<long , TEnum>(ref value);
					default: throw new NotSupportedException("Неподдерживаемый размер Enum.");
				}
			}
			 */

			extension<TEnum> ( TEnum flags ) where TEnum : struct, Enum
			{

				/*
				* IS EQUAL TO: Type.getTypeCode()
				* 
			   internal TypeCode getEnumTypeCode()                    => Type.GetTypeCode(typeof(TEnum).GetEnumUnderlyingType());
				*/


				public int int_32 => Convert.ToInt32(flags);
				public uint uint_32 => Convert.ToUInt32(flags);

				public long int_64 => Convert.ToInt64(flags);
				public ulong uint_64 => Convert.ToUInt64(flags);


				/// <summary>
				/// !!! Unsafe!!!  Reads numeric value from memory. <br/>
				/// Does not checks sizes of the old and new vaues
				/// </summary>
				public unsafe INumber<TNumber> toNumberUnsafe<TNumber> () where TNumber : INumber<TNumber>
					=> *(TNumber*)(void*)&flags;


				public unsafe INumber<TNumber> toNumber<TNumber> () where TNumber : INumber<TNumber>
				{
					var enumType = Enum.GetUnderlyingType(typeof(TEnum));
					var enumSize = Marshal.SizeOf(enumType);
					var intSize = Marshal.SizeOf<TNumber>();

					if ( enumSize == intSize )
					{
						return *(TNumber*)(void*)&flags;
					}

					if ( intSize < enumSize )
					{
						throw new NotSupportedException($"The size of the target type '{typeof(TNumber)}'({intSize} bytes) is smaller than the size of the source enum type '{typeof(TEnum)}'({enumType}={enumSize} bytes).");
					}

					void* pBuffer = stackalloc byte[ intSize ];
					*(TEnum*)pBuffer = flags;
					return *(TNumber*)pBuffer;
				}


				public bool equalsOneOf ( params TEnum[] values )
					=> values.Any(e => e.Equals(flags));



				public bool equalsAsInt32 ( TEnum target )
				{
					try
					{
						return flags.int_32 == target.int_32;
					}
					catch { }
					return false;
				}

				public bool equalsAsInt64 ( TEnum target )
				{
					try
					{
						return flags.int_64 == target.int_64;
					}
					catch { }
					return false;
				}

				public TEnum[] getAllValues ( params TEnum[] except )
					=> (except is null || !except.any)
					? Enum.GetValues<TEnum>()
					: [ .. Enum.GetValues<TEnum>().Where(e => !except.Contains(e)) ];


#if !ANDROID

				public int mixFlagsAsInt32 ( params TEnum[] flagsToExclude )
				{
					var allValues = Enum.GetValues<TEnum>()
						.Where(e => !flagsToExclude.Contains(e))
						.Select(e => e.int_32);

					int mixResult = 0;
					foreach ( var e in allValues )
					{
						mixResult |= e;
					}

					return mixResult;
				}

#endif


				public (bool IsDirectDefinedEnumValue , FieldInfo? DirectDefinedEnumFieldInfo , bool IsMaskedEnumValue , TEnum[] MaskValues , FieldInfo[] MasksFieldInfos) getValueInfo ()
				{
					bool isDefinedEnumField = Enum.IsDefined(flags);

					Type enumType = typeof(TEnum);
					if ( isDefinedEnumField ) //This value is direct defined in ENUM! not bit mask
					{
						string enumFieldName = flags.ToString();
						FieldInfo fi = enumType.GetField(enumFieldName)!;
						return (true , fi , false , [] , []);
					}

					//This is bitmasked enum, build result from flags
					var fields = Enum.GetValues<TEnum>()
						.Where(enumFieldValue => flags.HasFlag(enumFieldValue))
						.Select(enumFieldValue => (MaskValue: enumFieldValue , MaskFieldInfo: enumType.GetField(enumFieldValue.ToString())!));

					if ( !fields.Any() ) return (false , null , false , [] , []);

					TEnum[] maskValue = fields.Select(ff => ff.MaskValue).ToArray();
					FieldInfo[] maskInfo = [ .. fields.Select(ff => ff.MaskFieldInfo) ];
					return (false , null , true , maskValue , maskInfo);
				}



				public TAttr[] getEnumFieldCustomAttributes<TAttr> ( bool inherit = true ) where TAttr : System.Attribute
				{
					(bool isDirectDefinedEnumValue , FieldInfo? fi , _ , _ , _) = flags.getValueInfo();
					return !isDirectDefinedEnumValue || fi is null
						? throw new ArgumentOutOfRangeException(nameof(flags))
						: [ .. fi!.GetCustomAttributes<TAttr>(inherit) ];
				}


				///<summary>Return value of <see cref="System.ComponentModel.DescriptionAttribute"/></summary>    
#if WINDOWS
				[Obsolete("use Vanara.GetDescription() instead!!!" , false)]
#endif
				public string getDescriptionEx (
					bool ignoreZerroFlags = true ,
					bool ignoreNegativeFlagsInMultiValue = true ,
					string multiFlagsSeparator = ", " ,
					bool useFieldNameIfNotFound = true
					)
				{

					if ( Enum.IsDefined(flags) )
					{
						//var dic = flags.getEnumFieldCustomAttributes<DescriptionAttribute>();
						return flags.GetType().tryGetEnumFieldDescription(flags.ToString() , out var fieldDescriptionValue)
							? fieldDescriptionValue ?? flags.ToString()
							: flags.ToString();
					}

					//This is bitmasked enum, or value is not defined in this enum build result from flags
					string[] enumElements = [..
						Enum.GetValues<TEnum>()
						.Where(v => !ignoreNegativeFlagsInMultiValue || (v.int_64 >= 0))
						.Where(v => !ignoreZerroFlags || (v.int_64 != 0L))
						.Where(v => flags.HasFlag(v))
						.Select(v => v.getDescriptionEx(useFieldNameIfNotFound))
						];

					if ( enumElements is { Length: > 0 } )
					{
						var s = enumElements.join(multiFlagsSeparator)!;
						return s;
					}

					return flags.ToString();
				}



				internal Dictionary<TEnum , string> getDescriptionAsDicrionary ( bool useFieldNameIfNotFound = true )
				{
					var enumValues = Enum.GetValues<TEnum>();

					var dic = enumValues
						.Select(e =>
						{
#if WINDOWS && !MAUI
							string? description = e.GetDescription();
#else
							string? description = e.getDescriptionEx();
#endif
							if ( description.isNullOrWhiteSpace && useFieldNameIfNotFound ) description = e.ToString();
							return new { Key = e , Description = description! };
						}
						)
						.Where(kvp => kvp.Description.isNotNullOrWhiteSpace)
						.ToDictionary(kvp => kvp.Key , kvp => kvp.Description)!;

					return dic;
				}


				/// <returns>
				/// Returns "{EnumTypeName}.{ValueName}"
				/// </returns>
				internal string getFullName ( bool fullName )
				{
					Type enumType = typeof(TEnum);
					string n = fullName
					? enumType.FullName!
					: enumType.Name;

					return $"{n}.{flags}";
				}


				/*
				 //TODO: Need rework!!!
				public TEnum buildFromFlags ( params (bool flagCondition, TEnum flagToSet)[] flags2 )
				{
					long val = flags.int_64;
					foreach ( var item in flags2 )
					{
						if ( item.flagCondition )
						{
							val |= Convert.ToInt64(item.flagToSet);
						}
					}
					TEnum TResult = (TEnum)Enum.ToObject(typeof(TEnum) , val);
					return TResult;
				}


				public static T1 eBuildFromFlags<T1, T2> ( this T1 initialValue , params (T2 value, T2 flagToCheck, T1 flagToSet)[] flags ) where T1 : Enum where T2 : Enum
				{
					long val = Convert.ToInt64(initialValue);
					foreach ( var item in flags )
					{
						if ( item.value.HasFlag(item.flagToCheck) )
						{
							val |= Convert.ToInt64(item.flagToSet);
						}
					}
					T1 TResult = (T1)Enum.ToObject(typeof(T1) , val);
					return TResult;
				}


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



		End Module








			End Namespace
			 */


				public TEnum oR ( TEnum value )
				{
					try
					{
						var vv = value.int_64 | value.int_64;
						return Unsafe.As<long , TEnum>(ref vv);
					}
					catch ( Exception ex ) { throw new ArgumentException($"Could not append value from enumerated type '{typeof(TEnum).Name}'" , ex); }
				}


				public TEnum nOt ( TEnum value )
				{
					try
					{
						var vv = value.int_64 & ~value.int_64;
						return Unsafe.As<long , TEnum>(ref vv);
						//    return (T)(object)((int)(object)enumType & ~(int)(object)value!); 
					}
					catch ( Exception ex ) { throw new ArgumentException($"Could not remove value from enumerated type '{typeof(TEnum).Name}'" , ex); }

				}





				/*
			}


			extension( Enum enumValue )
			{

				internal string getDescriptionEx ( bool useFieldNameIfNotFound = true )
					=> enumValue
					.GetType()
					.getEnumFieldDescription(enumValue.ToString() , useFieldNameIfNotFound);
				 */


			}




			extension( System.Type enumType )
			{

				internal bool tryGetEnumFieldDescription ( string fieldName , out string description )
				{
					description = string.Empty;

					var attrDescr =
						enumType
						.GetField(fieldName , BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.GetField | BindingFlags.Static)?
						.GetCustomAttribute<DescriptionAttribute>();

					description = attrDescr?.Description ?? fieldName;
					return attrDescr is not null;
				}

				internal string getEnumFieldDescription ( string fieldName , bool useFieldNameIfNotFound = true )
					=> enumType.tryGetEnumFieldDescription(fieldName , out var descr)
						? descr.valueOrEmpty
						: useFieldNameIfNotFound
							? fieldName
							: string.Empty;


			}

			extension<TEnum> ( string stringValue ) where TEnum : struct, Enum
			{


				internal bool tryParseAsEnumExact ( out TEnum result , TEnum defaultValue = default )
				{
					result = defaultValue;

					if ( stringValue.isNullOrWhiteSpace ) return false;

					if ( Enum.TryParse<TEnum>(stringValue , true , out result) )
						return true;

					var itemsDescriptions = Enum
						.GetValues<TEnum>()
						.ToDictionary(e => e.getDescriptionEx<TEnum>() ?? e.ToString() , StringComparer.OrdinalIgnoreCase);

					return itemsDescriptions.TryGetValue(stringValue , out result);
				}


				internal TEnum parseAsEnumExact ( TEnum defaultValue = default )
					=> stringValue
					.tryParseAsEnumExact<TEnum>(out var e)
						? e
						: defaultValue;



				internal TEnum parseAsEnumFlags ( params string[] splitSeparators )
				{

					if ( stringValue.tryParseAsEnumExact<TEnum>(out var e) )
						return e;

					if ( splitSeparators.Length < 1 )
						splitSeparators = [ "," , ";" , " " ];

					// split by common separators and combine flags
					string[] parts = [..
						stringValue.Split(splitSeparators , StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
						.Select(p => p.ToLower ())
						.Distinct()
						];

					long result = 0;
					var enumType = typeof(TEnum);
					if ( parts.Length > 0 )
					{
						var itemsDescriptions = Enum
							.GetValues<TEnum>()
							.ToDictionary(e => e.getDescriptionEx<TEnum>() ?? e.ToString() , StringComparer.OrdinalIgnoreCase);

						foreach ( var enumValueStringElement in parts )
						{
							if ( enumValueStringElement.tryParseAsEnumExact<TEnum>(out e) )
							{
								long longEnum = Convert.ToInt64(e);
								if ( longEnum != 0L )
								{
									result |= longEnum;
								}
							}
						}
					}
					return (TEnum)Enum.ToObject(enumType , result);
				}




			}








		}



		internal static partial class Extensions_IO
		{




			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static FileInfo prepareForWrite (
				this FileInfo targetFile ,
				DirectoryInfo? targetDir = null ,
				Environment.SpecialFolder? defaultTargetDir = Environment.SpecialFolder.DesktopDirectory ,
				string autogeneratedFileNameSuffix = "" ,
				string defaultFileExtension = uom.constants.IOConst.FILE_EXTENSION_TMP ,
				bool ensureFileExtensionIsCorrect = false ,
				bool deleteIfExist = true ,
				ILogger? logger = null )
				=> uom.I_O.PrepareForWrite(
					targetFile: targetFile ,
					targetDir: targetDir ,
					defaultTargetDir: defaultTargetDir ,
					autogeneratedFileNameSuffix: autogeneratedFileNameSuffix ,
					defaultFileExtension: defaultFileExtension ,
					ensureFileExtensionIsCorrect: ensureFileExtensionIsCorrect ,
					deleteIfExist: deleteIfExist ,
					logger: logger);






			#region read / write



			extension( string? source )
			{


				/// <summary>using StringReader.ReadLine()</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string[] readLines ( bool skipEmptyLines = false , bool trimEachLine = false )
				{
					if ( source is null ) return [];
					using StringReader sr = new(source);
					return [ .. sr.enumerateLines(skipEmptyLines , trimEachLine) ];
				}



				/// <summary>Split в .NET 10 через Span для скорости</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public MemoryExtensions.SpanSplitEnumerator<char> enumLinesSpans ()
					=> source is null
						? new MemoryExtensions.SpanSplitEnumerator<char>()
						: source.AsMemory().Span.Split('\n');

			}



			extension( string path )
			{



				/// <summary>
				/// Validates that the current <c>path</c> (string) points to an existing file system entry (file or directory).
				/// If the path does not exist, a <see cref="System.IO.FileNotFoundException"/> is thrown with the path provided
				/// as the <c>fileName</c> argument of the exception.
				/// </summary>
				/// <remarks>
				/// This method uses the local helper <c>toFileSystemInfo()</c> to resolve the path into a <see cref="FileSystemInfo"/>.
				/// Note that <c>toFileSystemInfo()</c> itself may throw <see cref="System.IO.FileNotFoundException"/> for invalid paths;
				/// this method will also throw when the resolved entry does not exist.
				/// Use this to fail fast when an expected file or directory is missing.
				/// </remarks>
				/// <exception cref="System.IO.FileNotFoundException">
				/// Thrown when the path does not reference an existing file or directory.
				/// </exception>
				/// <example>
				/// string configPath = @"C:\app\config.json";
				/// // Throws if the file or directory does not exist
				/// configPath.pathThrowIfNotExist();
				/// </example>                
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void pathThrowIfNotExist ()
				{
					if ( !path!.pathToFileSystemInfo()!.Exists ) throw new System.IO.FileNotFoundException(null , path);
				}

				/// <summary>"Add \\?\ to start of path</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string pathAddLongPathPrefix ()
				{
					string p = path;
					if ( OSInfo.IsWindows && !p.valueOrEmpty.StartsWith(constants.IOConst.CS_PATH_PREFIX_WIN_LONG_PATH) )
					{
						p = constants.IOConst.CS_PATH_PREFIX_WIN_LONG_PATH + p;
					}
					return p!;
				}


				/// <summary>"Remove \\?\ from start of path</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string pathRemoveLongPathPrefix ()
				{
					string p = path;
					if ( OSInfo.IsWindows && (p is not null) && p!.StartsWith(constants.IOConst.CS_PATH_PREFIX_WIN_LONG_PATH) )
					{
						p = p[ constants.IOConst.CS_PATH_PREFIX_WIN_LONG_PATH.Length.. ];
					}
					return p!;
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal FileSystemInfo pathToFileSystemInfo ()
				{
					if ( File.Exists(path) ) return new FileInfo(path);
					if ( Directory.Exists(path) ) return new DirectoryInfo(path);

					// invalid path or does not exist
					throw new System.IO.FileNotFoundException($"'{path}' was not found!");
				}

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal FileInfo pathToFileInfo ( bool addLongPathPrefix = false )
					=> addLongPathPrefix
							? new(path.pathAddLongPathPrefix())
							: new(path);

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal bool pathTryToFileInfo ( out FileInfo? result , bool addLongPathPrefix = false , bool checkExist = false )
				{
					result = null;
					try
					{
						if ( !checkExist || (checkExist && File.Exists(path)) )
						{
							result = path.pathToFileInfo(addLongPathPrefix);
						}
					}
					catch { }
					return result is not null;

				}

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal bool pathTryGetFileVersionInfo ( out FileVersionInfo? result )
				{
					result = null;
					try
					{
						if ( File.Exists(path) ) result = FileVersionInfo.GetVersionInfo(path);
					}
					catch { }
					return result is not null;
				}

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal DirectoryInfo pathToDirectoryInfo ( bool addLongPathPrefix = false )
					=> addLongPathPrefix
							? new(path.pathAddLongPathPrefix())
							: new(path);


				/// <summary>
				/// Returns the parent path segment for the current file-system path string.
				/// </summary>
				/// <remarks>
				/// - The method finds the last occurrence of <see cref="Path.DirectorySeparatorChar"/> and returns
				///   the substring of the path up to (but excluding) that separator.
				/// - If the path has no suitable separator (for example a simple file name) the method returns <c>null</c>.
				/// - This method does not normalize the path, resolve relative segments (such as <c>..</c>), or consider
				///   <see cref="Path.AltDirectorySeparatorChar"/>. The caller should pre-normalize if needed.
				/// </remarks>
				/// <example>
				/// - <c>"C:\folder\file.txt"</c> => <c>"C:\folder"</c>
				/// - <c>"C:\"</c> => <c>"C:"</c> (the trailing separator is removed by the substring operation)
				/// - <c>"filename.ext"</c> => <c>null</c>
				/// </example>
				/// <returns>
				/// The parent path string (without the trailing directory separator), or <c>null</c> when no parent exists
				/// or the input is not suitable for extracting a parent path.
				/// </returns>                
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string? pathGetFileSystemParent ()
				{
					string p = path;

					int lastSeperatorPos = p.LastIndexOf(Path.DirectorySeparatorChar);
					if ( lastSeperatorPos < 1 || lastSeperatorPos >= p.Length ) return null;
					p = p[ ..lastSeperatorPos ];
					return p;
				}


				/// <summary>
				/// Search upward from the current path and return the first existing file-system entry.
				/// </summary>
				/// <remarks>
				/// Behavior:<br/>
				/// - If the exact <c>path</c> exists and is a file, a <see cref="FileInfo"/> for that path is returned.<br/>
				/// - If the exact <c>path</c> exists and is a directory, a <see cref="DirectoryInfo"/> for that path is returned.<br/>
				/// - Otherwise the method walks the directory tree upward (parent by parent) and returns a
				///   <see cref="DirectoryInfo"/> for the first existing parent directory found.<br/>
				/// - If no existing file or directory is found in the upward chain, <c>null</c> is returned.
				/// <br/>
				/// Note: the method returns a <see cref="FileSystemInfo"/> which must be cast to <see cref="FileInfo"/>
				/// or <see cref="DirectoryInfo"/> by the caller depending on the concrete type.
				/// </remarks>
				/// <returns>
				/// A <see cref="FileSystemInfo"/> instance for the first existing file or directory discovered,
				/// or <c>null</c> if none was found.
				/// </returns>
				/// <example>
				/// Usage (inside the same extension context):
				/// <code>
				/// var result = path.pathFindFirstExistingFileSystemInfoUpward();
				/// if (result is FileInfo fi)  Console.WriteLine($"Found file: {fi.FullName}");
				/// else if (result is DirectoryInfo di) Console.WriteLine($"Found directory: {di.FullName}");
				/// else Console.WriteLine("No existing file or directory found upward from path.");
				/// </code>
				/// </example>                
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal FileSystemInfo? pathFindFirstExistingFileSystemInfoUpward ()
				{
					//First think this is file
					if ( File.Exists(path) ) return new FileInfo(path);

					//Not exist or this is directory
					if ( Directory.Exists(path) ) return new DirectoryInfo(path);

					//Not exist!

					string? parentDir = path.pathGetFileSystemParent();
					while ( parentDir is not null )
					{
						if ( Directory.Exists(parentDir) ) return new DirectoryInfo(path);

						parentDir = parentDir!.pathGetFileSystemParent();
					}
					return null;
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal FileInfo pathGetFileInSpecialFolder ( Environment.SpecialFolder SF ) => new(Path.Combine(Environment.GetFolderPath(SF) , path));

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal FileInfo pathGetFileInTempDir () => new(Path.Combine(Path.GetTempPath() , path));


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal Uri pathToURI () => new(path);


				/// <summary>Replaces sample: 'c:\windows\system23' -> 'c_windows_system23'</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string pathToSafeFileNameString ( char replaceWith = '_' , bool replaceDots = false )
				{
					List<char> invalidCharsList = [
						.. Path.GetInvalidPathChars() ,
						.. Path.GetInvalidFileNameChars() ,
						'%'
						];

					if ( replaceDots ) invalidCharsList.AddRange('.');

					var invalidChars = invalidCharsList.Distinct().ToArray();

					string p = path;
					foreach ( var invalidChar in invalidChars )
					{
						while ( p.Contains(invalidChar) )
						{
							p = p.Replace(invalidChar , replaceWith);
						}
					}

					// Заменяем двойные символы на одинарные
					string C_REPLACE_CHAR2 = new(replaceWith , 2);
					while ( p.Contains(C_REPLACE_CHAR2) )
					{
						p = p.Replace(C_REPLACE_CHAR2 , replaceWith.ToString());
					}

					return p;
				}











				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public FileStream pathCreateFileStreamR ( FileMode fm = FileMode.Open , FileAccess fa = FileAccess.Read , FileShare fs = FileShare.ReadWrite )
						=> new(path , fm , fa , fs);

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal FileStream pathCreateFileStreamW (
					FileMode fm = FileMode.CreateNew , FileAccess fa = FileAccess.Write , FileShare fs = FileShare.ReadWrite )
						=> new(path , fm , fa , fs);

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal StreamWriter pathCreateStreamWriter (
					FileMode fm = FileMode.CreateNew ,
					FileAccess fa = FileAccess.Write ,
					FileShare fs = FileShare.ReadWrite ,
					Encoding? @encoding = null ,
					bool? autoFlush = true )
					=> path.pathCreateFileStreamW(fm , fa , fs).createStreamWriter(@encoding , autoFlush);


			}


			extension( Stream stream )
			{


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void truncate ( long newLen = 0L )
				{
					stream.SetLength(newLen);
					stream.Flush();
				}






				/// <summary>
				/// Reads up to <paramref name="readBytesCount"/> bytes from the current <paramref name="stream"/> stream
				/// into a buffer rented from <see cref="uom.mem.ArrayPoolHelper"/>.
				/// </summary>
				/// <param name="readBytesCount">
				/// The minimum buffer length to rent and the maximum number of bytes to attempt to read.
				/// Must be &gt;= 0.
				/// </param>
				/// <param name="readOffset">
				/// If greater than zero, the stream will be seeked to this offset using <paramref name="offsetMode"/>
				/// before reading. Default is zero (no seek).
				/// </param>
				/// <param name="offsetMode">The <see cref="SeekOrigin"/> to use when applying <paramref name="readOffset"/>.</param>
				/// <returns>
				/// A tuple containing:
				/// <list type="bullet">
				/// <item><c>Buffer</c> — a <see cref="uom.mem.ArrayPoolHelper.DisposableSharedArrayContainer{byte}"/> that wraps the rented array. The caller is responsible for disposing
				/// the returned <c>SharedObject</c> to return the array to the pool.</item>
				/// <item><c>ReadBytesCount</c> — the actual number of bytes read from the stream; this may be less than or equal to <paramref name="readBytesCount"/>.</item>
				/// </list>
				/// </returns>
				/// <remarks>
				/// - The rented buffer length may be larger than the reported <c>ReadBytesCount</c>. Only the first
				///   <c>ReadBytesCount</c> bytes contain valid data — callers should slice the span accordingly
				///   (for example: <c>result.Buffer.AsSpan().Slice(0, result.ReadBytesCount)</c>).
				/// - It is critical to dispose the returned <c>SharedObject&lt;byte&gt;</c> when finished to avoid leaking
				///   pooled arrays.
				/// - This documentation notes current ownership semantics. If the implementation is changed to dispose the
				///   rented buffer before returning, callers will receive an unusable/disposed buffer; verify implementation
				///   behavior if you rely on the returned buffer.
				/// </remarks>
				/// <example>
				/// using var r = sm.readEx(4096);
				/// int n = r.ReadBytesCount;
				/// var span = r.Buffer.AsSpan().Slice(0, n);
				/// // use span...
				/// // r.Dispose() returns the buffer to the pool
				/// </example>
				/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="readBytesCount"/> is negative.</exception>
				/// <exception cref="IOException">Propagates I/O errors from the underlying stream operations.</exception>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal (mem.ArrayPoolHelper.DisposableSharedArrayContainer<byte> Buffer , int ReadBytesCount) readEx ( int readBytesCount , int readOffset = 0 , SeekOrigin offsetMode = SeekOrigin.Begin )
				{

					using mem.ArrayPoolHelper.DisposableSharedArrayContainer<byte> readBufferPool = uom.mem.ArrayPoolHelper.Rent<byte>(readBytesCount , false);
					if ( readOffset > 0 ) stream.Seek(readOffset , offsetMode);
					int readBytes = stream.Read(readBufferPool);
					return (readBufferPool , readBytes);
				}


				/// <summary>
				/// Reads all bytes from the associated <c>Stream</c> into a buffer rented from <c>ArrayPoolHelper</c>
				/// and returns a <see cref="mem.ArrayPoolHelper.DisposableSharedArrayContainer{byte}"/> that wraps that rented buffer.
				/// </summary>
				/// <param name="seekStart">
				/// If <c>true</c> the underlying stream position will be seeked to the beginning (<c>SeekOrigin.Begin</c>)
				/// before reading. Default is <c>true</c>.
				/// </param>
				/// <returns>
				/// A <see cref="mem.ArrayPoolHelper.DisposableSharedArrayContainer{byte}"/> that contains the bytes read from the stream.
				/// The caller is responsible for disposing the returned <c>SharedObject&lt;byte&gt;</c> to return the array to the pool.
				/// The valid data length equals the stream's <c>Length</c>; callers should slice the returned buffer accordingly
				/// (for example, use <c>result.AsSpan().Slice(0, (int)sm.Length)</c>).
				/// </returns>
				/// <exception cref="Exception">
				/// Thrown when the stream's length is greater than <see cref="int.MaxValue"/> (cannot allocate that much memory),
				/// or when the number of bytes actually read does not match the stream's reported <c>Length</c>.
				/// Note: lower-level I/O exceptions from the underlying read may also propagate.
				/// </exception>
				/// <remarks>
				/// - This method reads the entire stream synchronously into a rented buffer. It is intended for streams
				///   whose full contents fit into memory (see the <c>int.MaxValue</c> check).
				/// - Because the method returns a pooled buffer, always call <c>Dispose()</c> on the returned
				///   <c>SharedObject&lt;byte&gt;</c> when finished to avoid leaking pooled arrays.
				/// </remarks>                
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal mem.ArrayPoolHelper.DisposableSharedArrayContainer<byte> readExAllBytes ( bool seekStart = true )
				{
					if ( stream.Length > int.MaxValue ) throw new Exception($"Stream size ({stream.Length.formatByteSizeMP()}) is too large to read all bytes into memory!");

					if ( seekStart ) stream.Seek(0L , SeekOrigin.Begin);
					var r = stream.readEx((int)stream.Length);

					return r.ReadBytesCount != stream.Length
						? throw new Exception($"Stream read all bytes error! Readed {r.ReadBytesCount} bytes from {stream.Length} bytes!")
						: r.Buffer;
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal byte[] readAllBytes ( bool seekStart = true )
				{
					if ( stream.Length > int.MaxValue ) throw new Exception($"Stream size ({stream.Length.formatByteSizeMP()}) is too large to read all bytes into memory!");

					// Ensure the stream is at the beginning for reading the full content
					if ( seekStart ) stream.Seek(0L , SeekOrigin.Begin);

					using MemoryStream ms = new();
					stream.CopyTo(ms);
					return ms.ToArray();
				}

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal async Task<byte[]> readAllBytesAsync ( bool seekStart = true )
				{
					if ( stream.Length > int.MaxValue ) throw new Exception($"Stream size ({stream.Length.formatByteSizeMP()}) is too large to read all bytes into memory!");

					// Ensure the stream is at the beginning for reading the full content
					if ( seekStart ) stream.Seek(0L , SeekOrigin.Begin);

					using MemoryStream ms = new();
					await stream.CopyToAsync(ms);
					return ms.ToArray();
				}

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal void writeAllBytes ( byte[] data , bool truncateBeforeWrite = true )
				{
					if ( truncateBeforeWrite ) stream.truncate();

					if ( data.Length != 0 )
					{
						stream.Seek(0L , SeekOrigin.Begin);
						using BinaryWriter bw = new(stream , System.Text.Encoding.Default , true);
						bw.Write(data);
					}
				}

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal void writeToFile ( string file )
				{
					using var fs = File.OpenWrite(file);
					stream.CopyTo(fs);
				}



				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal void writeToFile ( FileInfo file , bool newFile = true )
				{
					if ( newFile )
					{
						file.Refresh();
						file.tryDeleteIfExist();
					}
					stream.writeToFile(file.FullName);
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal async Task writeToFileAsync ( FileInfo file )
				{
					file.tryDeleteIfExist();
					using var fs = file.createFileStreamW();
					await stream.CopyToAsync(fs);
				}

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal async Task writeToFileAsync ( string file )
				{
					using var fs = File.OpenWrite(file);
					await stream.CopyToAsync(fs);
				}




				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public StreamReader createStreamReader ( bool detectEncodingFromByteOrderMarks = true )
					=> new(stream , detectEncodingFromByteOrderMarks);







			}


			extension( FileStream stream )
			{

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public StreamWriter createStreamWriter ( Encoding? @encoding = null , bool? autoFlush = true )
				{
					@encoding ??= Encoding.Unicode;
					StreamWriter sw = new(stream , @encoding);
					if ( autoFlush.HasValue ) sw.AutoFlush = autoFlush.Value;
					return sw;
				}



			}


			extension( StreamReader reader )
			{


				/*
				internal char[] eReadAllChars ()
				{


					List<char> lBuffer = sr.Read( (cBuffer , 0 , cBuffer.Length); [];


					char[] cBuffer = new char[ 1024 ];
					int iReadCount;
					do
					{
						iReadCount = sr.Read(cBuffer , 0 , cBuffer.Length);



						if ( iReadCount > 0 )
						{
							ArraySegment<char> dataSlice = new(cBuffer , 0 , iReadCount);
							lBuffer.AddRange(dataSlice);
						}
					}
					while ( iReadCount > 0 );
					var aChars = lBuffer.ToArray();
					return aChars;


				}


				internal string eReadAllCharsAsString ()
						=> new(sr.eReadAllChars());

				 */

			}


			extension( TextReader reader )
			{


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public IEnumerable<string> enumerateLines ( bool skipEmptyLines = false , bool trimEachLine = false )
				{
					string? line = reader.ReadLine();
					while ( line is not null )
					{
						if ( !line.isNullOrWhiteSpace || !skipEmptyLines )
						{
							yield return trimEachLine ? line.Trim() : line;
						}

						line = reader.ReadLine();
					}
				}

			}


			extension( FileInfo? file )
			{


				/// <summary>
				/// Reads the entire contents of the file into a buffer rented from <see cref="uom.mem.ArrayPoolHelper"/>
				/// and returns that buffer wrapped in a <see cref="uom.mem.ArrayPoolHelper.DisposableSharedArrayContainer{byte}"/>.
				/// </summary>
				/// <remarks>
				/// - The method verifies the file size fits into a 32-bit integer and will throw if the file is too large
				///   to be loaded into memory (<see cref="int.MaxValue"/>).</remarks>
				/// <remarks>
				/// - The returned <see cref="uom.mem.ArrayPoolHelper.DisposableSharedArrayContainer{byte}"/> wraps a pooled array. It is the caller's
				///   responsibility to dispose the returned SharedObject when finished so that the array is returned to the pool.
				///   The rented array length may be larger than the actual number of valid bytes; use the documented
				///   length/ReadBytesCount from the SharedObject consumer pattern when slicing the buffer.
				/// </remarks>
				/// <returns>
				/// A <see cref="uom.mem.ArrayPoolHelper.DisposableSharedArrayContainer{byte}"/> containing the file bytes.
				/// </returns>
				/// <exception cref="System.Exception">
				/// Thrown when the file length exceeds <see cref="int.MaxValue"/> (cannot be read into a single buffer).
				/// </exception>                
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal mem.ArrayPoolHelper.DisposableSharedArrayContainer<byte> readExAllBytes ()
				{
					ArgumentNullException.ThrowIfNull(file);
					if ( file.Length > int.MaxValue ) throw new Exception($"File '{file.FullName}' too large to read all bytes into memory!");
					using FileStream fs = new(file.FullName , FileMode.Open , FileAccess.Read);
					return fs.readExAllBytes();
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal MemoryStream readToMemoryStream ()
				{
					ArgumentNullException.ThrowIfNull(file);
					using var r = file.readExAllBytes();
					return new MemoryStream([ .. r.Value ]);
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string readAsText ( System.Text.Encoding? @encoding = null , bool detectEncodingFromByteOrderMarks = false )
				{
					ArgumentNullException.ThrowIfNull(file);
					if ( !detectEncodingFromByteOrderMarks || @encoding is not null )
					{
						return File.ReadAllText(file.FullName , @encoding ?? Encoding.Unicode);
					}

					using FileStream fs = file.createStreamR(FileMode.Open , FileAccess.Read , FileShare.Read);
					using StreamReader sr = new(fs , true);
					return sr.ReadToEnd();
				}

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal async Task<string> readAsTextAsync ( System.Text.Encoding? @encoding = null , bool detectEncodingFromByteOrderMarks = false )
					=> await Task.Factory.StartNew(()
						=> file.readAsText(@encoding , detectEncodingFromByteOrderMarks));


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string[] readLines ( Encoding? encoding = null , bool skipEmptyLines = false , bool trimEachLine = false )
				{
					ArgumentNullException.ThrowIfNull(file);
					using FileStream fs = file.createStreamR();
					using StreamReader sr = new(fs , encoding ?? Encoding.Unicode);
					return [ .. sr.enumerateLines(skipEmptyLines , trimEachLine) ];
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal async Task<string[]> readLinesAsync ( Encoding? encoding = null , bool skipEmptyLines = false , bool trimEachLine = false )
					=> await Task.Factory.StartNew(()
						=> file.readLines(encoding , skipEmptyLines , trimEachLine));





				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal void writeAllBytes ( byte[] data )
						=> File.WriteAllBytes(file!.FullName , data);


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal void writeAllText ( string text , Encoding? @encoding = null )
				{
					ArgumentNullException.ThrowIfNull(file);
					if ( !file.Directory!.Exists ) file.Directory.Create();

					using StreamWriter sw = file.createStreamWriter(FileMode.OpenOrCreate , encoding: @encoding ?? Encoding.Unicode);
					sw.BaseStream.truncate();
					if ( !text.isNullOrEmpty ) sw.Write(text);
					sw.Flush();
				}





				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal FileStream createStreamR ( FileMode fm = FileMode.Open , FileAccess fa = FileAccess.Read , FileShare fs = FileShare.ReadWrite )
						=> new(file!.FullName , fm , fa , fs);

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal StreamReader createStreamReader (
					FileMode fm = FileMode.Open ,
					FileAccess fa = FileAccess.Read ,
					FileShare fs = FileShare.ReadWrite ,
					bool detectEncodingFromByteOrderMarks = true )
						=> file!.createStreamR(fm , fa , fs).createStreamReader(detectEncodingFromByteOrderMarks);




				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal FileStream createFileStreamW (
					FileMode fm = FileMode.CreateNew ,
					FileAccess fa = FileAccess.Write ,
					FileShare fs = FileShare.ReadWrite )
						=> file!.FullName.pathCreateFileStreamW(fm , fa , fs);

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal StreamWriter createStreamWriter (
					FileMode fm = FileMode.CreateNew ,
					FileAccess fa = FileAccess.Write ,
					FileShare fs = FileShare.ReadWrite ,
					Encoding? @encoding = null ,
					bool? autoFlush = true )
						=> file!.FullName.pathCreateStreamWriter(fm , fa , fs , @encoding , autoFlush);



				#region Arhive & Backup




				/// <summary>Create Backup copy of file. (COPY or MOVE) Return BackUp File</summary>

				public FileInfo? makeBackUpIfExist ( bool moveInsteadOfCopy = false )
				{
					if ( !file!.Exists ) return null;

					FileInfo fiBak = new($"{file.FullName}.{DateTime.Now.toFileName()}.bak.");
					_ = fiBak.tryDeleteIfExist();

					if ( moveInsteadOfCopy )
						file.MoveTo(fiBak.FullName);
					else
						file.CopyTo(fiBak.FullName);

					return fiBak;
				}






				private FileInfo moveToArhive_Core ( string arhiveFilesExt = constants.IOConst.FILE_EXTENSION_BAK )
				{
					FileInfo fiBackup = new($"{file!.FullName}.{DateTime.Now.toFileName()}{arhiveFilesExt}");
					_ = fiBackup.tryDeleteIfExist();

					FileInfo fileToMove = new(file.FullName);
					fileToMove.MoveTo(fiBackup.FullName);
					return fiBackup;
				}



				/// <returns>Arhived FileInfo if file backup success or null if source file not exist</returns>
				public FileInfo? moveToArhive ( int maxArhiveFileCount = 10 , string arhiveFilesExt = constants.IOConst.FILE_EXTENSION_BAK )
				{
					if ( !file!.Exists ) return null;

					FileInfo fiBackup = file.moveToArhive_Core(arhiveFilesExt);

					//Clearing Arhive dir
					string arhiveFilePrefix = $"{file.Name}.";
					FileInfo[] arhiveFiles = [..
					file.Directory!.GetFiles()
					.Where(fi => fi.Name.EndsWith(arhiveFilesExt, StringComparison.InvariantCultureIgnoreCase))
					.Where(fi => fi.Name.StartsWith(arhiveFilePrefix, StringComparison.InvariantCultureIgnoreCase))
					.OrderBy(fi => fi.Name, StringComparer.InvariantCultureIgnoreCase)
						];

					int filesToKillCount = arhiveFiles.Length - maxArhiveFileCount;
					if ( filesToKillCount > 0 )
					{
						FileInfo[] filesToKill = [ .. arhiveFiles.Take(filesToKillCount) ];
						try
						{
							filesToKill.AsParallel().ForAll(fi => fi.tryDeleteIfExist());
						}
						catch { }
					}

					return fiBackup;
				}


				/// <returns>Arhived FileInfo if file backup success or null if source file not exist</returns>
				public FileInfo? moveToArhive ( DateTime killBefore , string arhiveFilesExt = constants.IOConst.FILE_EXTENSION_BAK )
				{
					if ( !file!.Exists ) return null;

					FileInfo fiBackup = file.moveToArhive_Core(arhiveFilesExt);

					//Clearing Arhive dir				
					FileInfo[] filesToKill = [..
					file.Directory!.GetFiles()
					.Where(fi => fi.Extension.Equals(arhiveFilesExt, StringComparison.InvariantCultureIgnoreCase))
					.Where(fi => fi.Name.StartsWith($"{file}.", StringComparison.InvariantCultureIgnoreCase))
					.Where(fi => fi.CreationTime <= killBefore)
					.OrderByDescending(fi => fi.Name, StringComparer.InvariantCultureIgnoreCase)
						];

					filesToKill.forEach(fi => fi.tryDeleteIfExist());

					return fiBackup;
				}



				/*
				 * 
				 * 

				public static void clearArhiveOldFiles(this DirectoryInfo arhive, string arhiveFilesExt = BAK_EXT, int maxArhiveFileCount = 10)
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


				public static void clearArhiveOldFiles(this DirectoryInfo arhive, DateTime killBefore, string arhiveFilesExt = BAK_EXT)
				{
					//Get All arhive files in dir
					FileInfo[] filesToKill = [.. (from fi in arhive.GetFiles($"*.{arhiveFilesExt}")
												  where fi.CreationTime <= killBefore
												  select fi)];

					if (filesToKill.Any())
					{
						filesToKill.forEach(fi => fi.Delete());
					}
				}


				public static void clearArhiveOldFiles(this DirectoryInfo arhive, TimeSpan maxAge, string arhiveFilesExt = BAK_EXT)
					=> arhive.clearArhiveOldFiles(DateTime.Now - maxAge, arhiveFilesExt);


				 */



				#endregion


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string formatByteSizeMP ( int decimalPlaces = constants.FormatConst.DEFAULT_DECIMAL_DIGITS )
					=> file?.Length.formatByteSizeMP(decimalPlaces) ?? string.Empty;







			}



			#endregion


			extension( Dictionary<string , MemoryStream> ramFiles )
			{


				/// <summary>
				/// Create ZIP arhive from specifed files with flat file structure - all files placed in root of zip
				/// </summary>
				internal void zipFlatTo ( string zipFile , System.IO.Compression.CompressionLevel compressionLevel = System.IO.Compression.CompressionLevel.SmallestSize )
				{
					// Используем ZipArchiveMode.Create для создания нового архива
					using var archive = ZipFile.Open(zipFile , ZipArchiveMode.Create);

					foreach ( var ramFile in ramFiles )
					{
						// Первый параметр — путь к файлу на диске
						// Второй параметр — имя файла внутри архива
						//archive.CreateEntryFromFile( fileToArhive.Value , fileToArhive.Key , compressionLevel: compressionLevel);

						var zipEntry = archive.CreateEntry(ramFile.Key);
						using ( var zipFileStream = zipEntry.Open() )
						{
							using var ms = ramFile.Value;
							ms.Seek(0 , SeekOrigin.Begin);
							ms.CopyTo(zipFileStream);
						}
					}
				}
			}

			extension( IEnumerable<FileInfo> files )
			{


				/// <summary>
				/// Create ZIP arhive from specifed files with flat file structure - all files placed in root of zip
				/// </summary>
				internal void zipFlatTo ( string zipFile , System.IO.Compression.CompressionLevel compressionLevel = System.IO.Compression.CompressionLevel.SmallestSize )
				{
					// Используем ZipArchiveMode.Create для создания нового архива
					using var archive = ZipFile.Open(zipFile , ZipArchiveMode.Create);

					foreach ( var fileToArhive in files )
					{
						// Первый параметр — путь к файлу на диске
						// Второй параметр — имя файла внутри архива
						archive.CreateEntryFromFile(fileToArhive.FullName , fileToArhive.Name , compressionLevel: compressionLevel);
					}
				}


			}



			extension( IList<FileSystemInfo> fsiList )
			{

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string[] getFullNames () => [ .. fsiList.Select(fsi => fsi.FullName) ];

			}

			extension( FileSystemInfo fsi )
			{

				internal bool isFile => fsi is FileInfo;
				internal bool isDirectory => fsi is DirectoryInfo;


				internal string nameWithoutExtension
					=> Path.GetFileNameWithoutExtension(fsi.Name);




				/// <summary>Checks by Attributes.HasFlag(FileAttributes.Directory)</summary>
				internal bool isDirectoryAndExist
					=> fsi!.Exists && fsi!.Attributes.HasFlag(FileAttributes.Directory);

				internal DirectoryInfo parent => (fsi is DirectoryInfo di)
					? di.Parent!
					: fsi.toFile().Directory!;

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal FileInfo toFile () => (FileInfo)fsi!;

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal bool tryDeleteIfExist ( bool refresh = true )
				{
					try
					{
						if ( fsi is not null )
						{
							if ( refresh ) fsi.Refresh();
							if ( fsi.Exists ) fsi.Delete();
						}
						return true;
					}
					catch { return false; }
				}


				/// <summary>"Remove \\?\ from start of string</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string removeLongPathPrefix () => fsi!.FullName.pathRemoveLongPathPrefix();


				/// <summary>Multiplatform FileAttributes.ReparsePoint<br/>
				/// The file contains a reparse point, which is a block of user-defined data associated with a file or a directory. <br/>
				/// ReparsePoint is supported on Windows, Linux, and macOS.
				/// </summary>
				internal bool isSymLinkMP
					=> fsi is not null
					&& fsi.Exists
					&& fsi.Attributes.HasFlag(FileAttributes.ReparsePoint)
					&& !fsi.Attributes.HasFlag(FileAttributes.SparseFile);



			}


			extension( DirectoryInfo folder )
			{


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal DirectoryInfo? getFirstChildDir ( string childDirName )
					=> folder.GetDirectories(childDirName).FirstOrDefault();


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal FileInfo[] getFilesSorted ( string? searchPattern = null , SearchOption searchOption = SearchOption.TopDirectoryOnly )
					=> folder
						.GetFiles(searchPattern ??= "*.*" , searchOption)
						.OrderBy(fi => fi.Name)
						.ThenBy(fi => fi.FullName)
						.ToArray();


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal void createIfNotExist ()
				{
					if ( !folder!.Exists ) folder!.Create();
				}



			}






		}


		/// <summary>Network Extensions</summary>
		internal static partial class Extensions_Network
		{


			private static readonly Lazy<System.Net.NetworkInformation.PhysicalAddress> _macZero = new(new PhysicalAddress([ 0 , 0 , 0 , 0 , 0 , 0 ]));




			extension( uint maskPrefix )
			{
				/// <summary>Generates Subnet Mask like '255.255.255.0' from specifed maskPrefix (192.168.1.0/[24])</summary>
				/// <param name="maskPrefix">MaskPrefixSize (0-32) like '24' from 192.168.1.0/[24]</param>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal IPAddress getIP4SubnetMaskFromMaskPrefix ()
				{
					BitArray bits = new(32 , true);
					if ( maskPrefix < 32 )
					{
						bits.eSetBitsFromStart(0 , (int)(32u - maskPrefix) , false);
					}
					//var BitsString = bits.eToBitsString();
					IPAddress m = bits.toIPAddress();
					return m;
				}

			}


			extension( uint sourceIP )
			{

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal IPAddress ip4CalculableOrderToIP4Address () => new(sourceIP.reverseBytesOrder());
			}


			extension( System.Net.IPAddress sourceIP )
			{


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string toStringHex ()
					=> sourceIP.GetAddressBytes().toStringHex(false);

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal BitArray toBitArray ()
				{

					byte[] ipBytes = [ .. sourceIP.GetAddressBytes().Reverse() ];
					return new(ipBytes);
				}


				/// <summary>Determines the relative value of adddress1 to address2.</summary>
				/// <returns>-1 indicates address1 is less than address2. 1 indicates address1 is greater than address2. 0 indicates both are equal. -2 indicates addresses are incompatible for comparison.</returns>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal int compareTo ( IPAddress? target )
				{
					if ( target is null ) return 1;

					if ( sourceIP.AddressFamily != target.AddressFamily )
						throw new ArgumentOutOfRangeException(nameof(target) , $"AddressFamily is not equal for '{sourceIP}' and '{target}'");


					byte[]
						sourceIPBytes = sourceIP.GetAddressBytes(),
						targetIPBytes = target.GetAddressBytes();

					if ( sourceIPBytes.Length != 4 )
						throw new ArgumentOutOfRangeException(nameof(sourceIP) , "IP4 only can be compared!");


					if ( sourceIPBytes.Length != sourceIPBytes.Length )
						throw new ArgumentOutOfRangeException(nameof(target) , "Target IP has different size!");

					/*
					uint
						uX = sourceIP.toUInt32(),// .toUInt32CalculableOrder(),
						uY = target.toUInt32();// .toUInt32CalculableOrder();
					 */

					uint
						uX = sourceIP.toUInt32CalculableOrder(),
						uY = target.toUInt32CalculableOrder();

					var cmpResult = uX.CompareTo(uY);

					return cmpResult;


					//: sourceIPBytes.SequenceCompareTo(targetIPBytes);
					/*

					for ( int byteIndex = 0 ; byteIndex < sourceIPBytes.Length ; byteIndex++ )
					{
						int res = sourceIPBytes[ byteIndex ].CompareTo(targetIPBytes[ byteIndex ]);
						if ( res != 0 ) return res; // bytes are not equal
					}
					return 0; // all bytes are equal
					 */

				}


				/// <summary>Determines if the current IP Address is in the given range.</summary>
				/// <param name="rangeStartAddress">The beginning of the range.</param>
				/// <param name="rangeEndAddress">The end of the range.</param>
				/// <returns>True if the IP Address is within the passed range.</returns>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool isInRange ( IPAddress rangeStartAddress , IPAddress rangeEndAddress )
				{
					bool returnVal = false;
					// ensure that all addresses are of the same type otherwise reject //
					if ( rangeStartAddress.AddressFamily != rangeEndAddress.AddressFamily )
					{
						throw new ArgumentOutOfRangeException(nameof(rangeStartAddress) ,
							  $"The Start Range type {rangeStartAddress.AddressFamily} and End Range type {rangeEndAddress.AddressFamily} are not compatible address families.");
					}

					if ( rangeStartAddress.AddressFamily == sourceIP.AddressFamily )
					{
						returnVal = sourceIP.compareTo(rangeStartAddress) >= 0 && sourceIP.compareTo(rangeEndAddress) <= 0;   // no need to check for -2 value as this check has already been undertaken to get into this block //
					}
					else
					{
						throw new ArgumentOutOfRangeException(nameof(rangeStartAddress) ,
							  $"The range type {rangeStartAddress.AddressFamily} and current value type {sourceIP.AddressFamily} are not compatible address families");
					}

					return returnVal;
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static IPAddress[] getLocalAddresses ()
				{
					string hostName = System.Net.Dns.GetHostName();
					System.Net.IPHostEntry entry = System.Net.Dns.GetHostEntry(hostName);

					var ipList = entry
						.AddressList
						.Select(ip => new IPAddress(ip.GetAddressBytes()))
						.ToArray();

					return ipList;
				}


				/*
				public static bool isLocalAddressInRange ( IPAddress rangeStartAddress , IPAddress rangeEndAddress )
				{
					foreach ( IPAddress address in getLocalAddresses() )
					{
						if ( address.isInRange(rangeStartAddress , rangeEndAddress) )
						{
							return true;
						}
					}
					return false;
				}

				 public IPAddressEx ( byte[] address ) : base(address) { }

			public IPAddressEx ( long address ) : base(address) { }

			public IPAddressEx ( byte[] address , long scopeid ) : base(address , scopeid) { }


				 */



				//https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
				/// <summary>
				/// Return Human-Readable bytes order (Sample: 192.168.1.1 => [192,168,1,1])
				/// <example>
				/// <code>
				/// !!! Don't use for any arifmetic calculations! Wrong Byte Order! Use only for Saving/Restoring IP.
				/// </code>
				/// For Math calculations with IP use <see cref="toUInt32CalculableOrder"/> instead!!!
				/// </example>
				/// </summary>
				/// <returns>for 192.168.1.1 => [192,168,1,1]</returns>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal uint toUInt32 ()
					=> (uom.Int32_)sourceIP.GetAddressBytes();//return System.BitConverter.ToUInt32(ip.GetAddressBytes());

				////			/// <inheritdoc cref="eToUInt32CalculableOrder" />
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal uint toUInt32CalculableOrder ()
					=> (uom.Int32_)sourceIP.GetAddressBytes().Reverse().ToArray();






				/// <summary>Returns string like '192.168.0.1/24'</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string formatCIDR ( uint prefixLen , string separator = "/" ) => $"{sourceIP}{separator}{prefixLen}";

				/// <summary>Returns (0-32) part like 192.196.5.0/[24], calculated by specifed Mask like 255.255.255.0</summary>
				/// <param name="ipMask">Mask like '255.255.255.0'</param>			
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal uint getIP4SubnetPrefixSizeFromMask ()
				{
					uint uMask = sourceIP.toUInt32CalculableOrder();
					uint tlz = (uint)BitOperations.TrailingZeroCount(uMask);
					return 32u - tlz;
				}

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal IEnumerable<IPAddress> getAllIP4List ( IPAddress ipEnd )
				{
					uint @start = sourceIP.toUInt32CalculableOrder();
					uint @end = ipEnd.toUInt32CalculableOrder();
					for ( uint ip = @start ; ip <= @end ; ip++ )
					{
						IPAddress ipa = new(ip.reverseBytesOrder());
						yield return ipa;
					}
				}


				internal (
					IPAddress SubnetZeroIP ,
					IPAddress FirstIP ,
					IPAddress LastIP ,
					IPAddress BroadcastIP ,
					uint HostCount ,
					IEnumerable<IPAddress>? SubnetHosts
					)
					calculateIP4Subnet ( IPAddress subnetMask )
				{

					IPAddress
						ipaFirst = sourceIP,
						ipaLast = sourceIP,
						ipaBroadcast = sourceIP;

					uint uSubnetIPCount = 1u;

					uint uMask = subnetMask.toUInt32CalculableOrder();
					//if (uMask == 0xFF_FF_FF_FF) return (ipaFirst, ipaLast, ipaBroadcast, uSubnetIPCount);
					uint uIP = sourceIP.toUInt32CalculableOrder();
					uint uSubnetZeroIP = uIP & uMask;
					IPAddress ipaSubnetZeroIP = uSubnetZeroIP.ip4CalculableOrderToIP4Address();
					if ( subnetMask.Equals(IPAddress.Broadcast) )
					{
						return (ipaSubnetZeroIP , ipaFirst , ipaLast , ipaBroadcast , uSubnetIPCount , null);
					}

					{
						uint uMaskNot = ~uMask;
						uint uBroadcastIP = uIP | uMaskNot;
						ipaBroadcast = uBroadcastIP.ip4CalculableOrderToIP4Address();
					}

					uint uFirstIP = uSubnetZeroIP | 0x00_00_00_01;
					ipaFirst = uFirstIP.ip4CalculableOrderToIP4Address();
					ipaLast = ipaFirst;

					uint uPrefixLen = subnetMask.getIP4SubnetPrefixSizeFromMask();
					if ( uPrefixLen >= 31 )
					{
						return (ipaSubnetZeroIP , ipaFirst , ipaLast , ipaBroadcast , uSubnetIPCount , ipaFirst.toArrayFromSingleElement());
					}

					uint uChangeableBits = 32u - uPrefixLen;
					uSubnetIPCount = 0;
					for ( uint i = 0 ; i < uChangeableBits ; i++ )
					{
						uSubnetIPCount <<= 1;
						uSubnetIPCount++;
					}
					uSubnetIPCount--;//Now uSubnetIPCount includes BROADCAST_IP, therefore decreasing it to 1 = last valid IP

					uint uLastIP = uSubnetIPCount | uSubnetZeroIP;
					ipaLast = uLastIP.ip4CalculableOrderToIP4Address();
					var ipList = ipaFirst.getAllIP4List(ipaLast);

					return (ipaSubnetZeroIP , ipaFirst , ipaLast , ipaBroadcast , uSubnetIPCount , ipList);
				}


				internal (
					IPAddress SubnetZeroIP ,
					IPAddress FirstIP ,
					IPAddress LastIP ,
					IPAddress BroadcastIP ,
					uint HostCount ,
					IEnumerable<IPAddress>? SubnetHosts)
					calculateIP4Subnet ( uint subnetPrefix ) => sourceIP.calculateIP4Subnet(subnetPrefix.getIP4SubnetMaskFromMaskPrefix());


				internal bool isInLocalSubnet ( uint ipPrefixLen )
					=> new uom.network.IP4AddressWithMask(sourceIP , ipPrefixLen).IsInLocalSubnet();


				internal bool isInSubnet ( uom.network.IP4AddressWithMask subnet )
				{
					var ipaZerro = subnet.CalculateIP4Subnet().SubnetZeroIP;

					uint uZerro = ipaZerro.toUInt32();
					uint uIP = sourceIP.toUInt32();

					bool inSubNet = (uZerro & uIP) == uZerro;
					return inSubNet;
				}






			}


			extension( System.Net.IPEndPoint ipep )
			{

				internal string toStringHex () => $"{ipep.Address.toStringHex()}-{((ushort)ipep.Port).getBytes().toStringHex()}";

			}


			extension( UnicastIPAddressInformation uai )
			{

				internal network.IP4AddressWithMask toIP4AddressWithMask () => new(uai.Address , uai.IPv4Mask);

			}




			private static string FormatOUIOctets ( this byte[] mac )
			{
				if ( mac.Length < 3 ) throw new ArgumentOutOfRangeException(nameof(mac));

				mac[ 0 ].setBitRef(0 , false);
				mac[ 0 ].setBitRef(1 , false);

				return $"{mac[ 0 ]:X2}-{mac[ 1 ]:X2}-{mac[ 2 ]:X2}";
				//byte[] macBytes = [.. mac.GetAddressBytes().Take(3)];
			}

			extension( System.Net.NetworkInformation.PhysicalAddress mac ) //:IComparable<PhysicalAddress>
			{

				public static System.Net.NetworkInformation.PhysicalAddress ZERO => _macZero.Value;

				internal bool izSero => mac.Equals(System.Net.NetworkInformation.PhysicalAddress.ZERO);


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string toStringHex () => mac.GetAddressBytes().toStringHex(false);


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

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal static PhysicalAddress FromString ( string macString )
					=> new(PhysicalAddress.Parse(macString).GetAddressBytes());


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string GetHash () => mac.ToString();

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal int GetHashCode () => mac.GetHash().GetHashCode();

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal int compareTo ( PhysicalAddress? other ) => mac.GetHash().CompareTo(other?.GetHash() ?? string.Empty);

				public static bool operator == ( PhysicalAddress A , PhysicalAddress B )
					=> (A.GetHash() ?? string.Empty) == (B.GetHash() ?? string.Empty);

				public static bool operator != ( PhysicalAddress A , PhysicalAddress B )
					=> (A.GetHash() ?? string.Empty) != (B.GetHash() ?? string.Empty);





				public string FormatOUIOctets ()
					=> mac.GetAddressBytes().FormatOUIOctets();

				public string FormatReadable ()
					=> mac.GetAddressBytes().toStringHex();





			}


			extension( BitArray ba )
			{

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal IPAddress toIPAddress ()
				{
					byte[] ipBytes = [ 0 , 0 , 0 , 0 ];
					ba.CopyTo(ipBytes , 0);
					IPAddress ip = new([ .. ipBytes.Reverse() ]);
					return ip;
				}

			}


			extension( Stream source )
			{

				internal async Task copyToAsync (
					Stream destination ,
					ulong totalSize ,
					int bufferSize = uom.network.Helpers.DOWNLOAD_DEFAULT_BUFFER_SIZE ,
					uom.network.Helpers.DownloadProgressDelegate? progress = null ,
				CancellationToken cancellationToken = default )
				{
					ArgumentNullException.ThrowIfNull(source);
					if ( !source.CanRead ) throw new ArgumentException("Has to be readable" , nameof(source));
					ArgumentNullException.ThrowIfNull(destination);
					if ( !destination.CanWrite ) throw new ArgumentException("Has to be writable" , nameof(destination));
					if ( bufferSize < 0 ) throw new ArgumentOutOfRangeException(nameof(bufferSize));


					//ulong len = totalSize;// (ulong)source.Length;
					using var buffer = uom.mem.ArrayPoolHelper.RentBytes(bufferSize , true);
					ulong totalBytesRead = 0;
					int bytesRead;

					progress?.Invoke(new(totalSize , totalBytesRead));

					while ( (bytesRead = await source.ReadAsync(buffer.Value , cancellationToken).ConfigureAwait(false)) != 0 )
					{
						await destination.WriteAsync(buffer.Value , 0 , bytesRead , cancellationToken).ConfigureAwait(false);
						totalBytesRead += (uint)bytesRead;
						progress?.Invoke(new(totalSize , totalBytesRead));
					}
				}
			}


			extension( HttpClient client )
			{

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal async Task downloadAsync (
					string requestUri ,
					Stream destination ,
					int bufferSize = uom.network.Helpers.DOWNLOAD_DEFAULT_BUFFER_SIZE ,
					uom.network.Helpers.DownloadProgressDelegate? progress = null ,
					CancellationToken cancellationToken = default )
				{
					// Get the http headers first to examine the content length
					using var response = await client.GetAsync(requestUri , HttpCompletionOption.ResponseHeadersRead , cancellationToken);

					if ( !response.IsSuccessStatusCode )
					{
						string err = $"({(int)response.StatusCode}) {response.ReasonPhrase}".Trim();
						throw new Exception(err);
					}


					var contentLength = response.Content.Headers.ContentLength;
					using var downloadStream = await response.Content.ReadAsStreamAsync(cancellationToken);

					// Ignore progress reporting when no progress reporter was passed or when the content length is unknown
					if ( !contentLength.HasValue )
					{
						await downloadStream.CopyToAsync(destination , cancellationToken);
						uom.network.Helpers.DownloadProgressEventArgs dp = new(null , null);
						progress?.Invoke(dp);
						return;
					}

					ArgumentNullException.ThrowIfNull(progress);
					// Convert absolute progress (bytes downloaded) into relative progress (0% - 100%)
					//var relativeProgress = new Progress<ulong>(totalBytes => progress.Report((float)totalBytes / contentLength.Value));
					// Use extension method to report progress while downloading
					await downloadStream.copyToAsync(destination , (ulong)contentLength.Value , bufferSize , progress , cancellationToken);
					progress?.Invoke(new((ulong)contentLength.Value));
					//progress?.Report(1);
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal async Task<string> getStringAsyncEncoded ( [StringSyntax(StringSyntaxAttribute.Uri)] string requestUri , Encoding enc )
				{
					byte[] data = await client.GetByteArrayAsync(requestUri);
					return enc.GetString(data);
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal async Task<string> getStringAsyncEncoded ( [StringSyntax(StringSyntaxAttribute.Uri)] string requestUri , string encodingName = "windows-1251" )
				{
					uom.AppTools.RegisterEncodings();
					var enc = Encoding.GetEncoding(encodingName);
					return await client.getStringAsyncEncoded(requestUri , enc);
				}

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal async Task<string> getStringAsyncEncoded ( Uri requestUri , Encoding enc )
				{
					byte[] data = await client.GetByteArrayAsync(requestUri);
					return enc.GetString(data);
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal async Task<string> getStringAsyncEncoded ( Uri requestUri , string encodingName = "windows-1251" )
				{
					uom.AppTools.RegisterEncodings();
					var enc = Encoding.GetEncoding(encodingName);
					return await client.getStringAsyncEncoded(requestUri , enc);
				}

			}


			extension( string fileUrl )
			{

				internal async Task urlHttpClient_GetStreamAsync (
					Action<System.IO.Stream> onStream ,
					int timeoutMinutes = uom.network.Helpers.DOWNLOAD_DEFAULT_TIMEOUT_MINUTES ,
					Action<HttpClient>? initHttpClient = null )
				{
					//using HttpClient client = new() { Timeout = TimeSpan.FromMinutes(timeoutMinutes) };
					initHttpClient?.Invoke(uom.network.Helpers._httpClient.Value);
					using var stream = await uom.network.Helpers._httpClient.Value.GetStreamAsync(fileUrl);
					onStream.Invoke(stream);

				}



				public async Task<FileInfo> urlHttpClient_DownloadFileAsync ( string localFile , TimeSpan timeout , CancellationToken ct )
				{
					// Set custom User Agent

					using ( HttpClient client = new() { Timeout = timeout } )
					{
						client.DefaultRequestHeaders.Add("User-Agent" , uom.network.Helpers.HTTP_HEADER_AGENT_DEFAULT);

						/*
						var productValue = new ProductInfoHeaderValue("ScraperBot", "1.0");
						var commentValue = new ProductInfoHeaderValue("(+http://www.example.com/ScraperBot.html)");
						client.DefaultRequestHeaders.UserAgent.Add(productValue);
						client.DefaultRequestHeaders.UserAgent.Add(commentValue);
						client.Headers.UserAgent.Add(productValue);
						client.Headers.UserAgent.Add(commentValue);
						 */

						using ( Stream webStream = await client.GetStreamAsync(fileUrl , ct) )
						{
							using ( FileStream localFileStream = new(localFile , FileMode.OpenOrCreate) )
							{
								await webStream.CopyToAsync(localFileStream , ct);
							}
						}
					}

					return new(localFile);
				}


				/// <summary>Download File From Remote Using 'HttpWebRequest'</summary>
				/// <param name="localTarget">Local File Path To Save File</param>        
				internal async Task<FileInfo?> urlHttpClient_DownloadFileAsync (
					System.IO.FileInfo? localTarget = null ,
					CancellationToken? cancellationToken = null ,
					int timeoutMinutes = uom.network.Helpers.DOWNLOAD_DEFAULT_TIMEOUT_MINUTES ,
					Action<HttpClient>? initHttpClient = null
					)
				{
					FileInfo? fiDownloaded = null;
					await fileUrl.urlHttpClient_GetStreamAsync(async downloadStream =>
					{
						FileInfo localCahe = localTarget ?? new FileInfo(System.IO.Path.GetTempFileName());
						using FileStream cacheStream = localCahe.OpenWrite();
						try
						{
							if ( cancellationToken is not null )
							{
								await downloadStream.CopyToAsync(cacheStream , 1 , cancellationToken.Value);
							}
							else
							{
								await downloadStream.CopyToAsync(cacheStream);
							}
							cacheStream.Flush();
							fiDownloaded = localCahe;
						}
						finally
						{
							localCahe.tryDeleteIfExist();
						}
					} , timeoutMinutes , initHttpClient);
					return fiDownloaded;
				}




				/// <summary>Download File From Remote Using 'HttpWebRequest'</summary>
				/// <param name="localTarget">Local File Path To Save File</param>        
				internal async Task<FileInfo?> urlHttpClient_DownloadFileAsync (
					System.IO.FileInfo? localTarget = null ,
					int bufferSize = uom.network.Helpers.DOWNLOAD_DEFAULT_BUFFER_SIZE ,
					uom.network.Helpers.DownloadProgressDelegate? progress = null ,
					CancellationToken cancellationToken = default ,
					Action<HttpClient>? initHttpClient = null
					)
				{
					// Seting up the http client used to download the data
					initHttpClient?.Invoke(uom.network.Helpers._httpClient.Value);

					// Create a file stream to store the downloaded data. This really can be any type of writeable stream.
					FileInfo localCahe = localTarget ?? new FileInfo(System.IO.Path.GetTempFileName());
					try
					{
						using ( FileStream cacheStream = localCahe.OpenWrite() )
						{
							// Use the custom extension method below to download the data. The passed progress-instance will receive the download status updates.
							await uom.network.Helpers._httpClient.Value.downloadAsync(fileUrl , cacheStream , bufferSize , progress , cancellationToken);
							cacheStream.Flush();
						}
						localCahe.Refresh();
						return localCahe;
					}
					catch
					{
						localCahe.tryDeleteIfExist();
						throw;
					}
					finally
					{
						// localCahe.tryDeleteIfExist();
					}



				}


				public async Task urlHttpClient_DownloadSmallFileAsync ( string destinationPath , Action<HttpClient>? initHttpClient = null )
				{
					initHttpClient?.Invoke(uom.network.Helpers._httpClient.Value);
					// Get the file content as a byte array
					byte[] fileBytes = await uom.network.Helpers._httpClient.Value.GetByteArrayAsync(fileUrl);

					// Write the byte array to the specified file path
					File.WriteAllBytes(destinationPath , fileBytes);
				}


				[Obsolete("!!! Is InTest MODE, NOT FINISHED!!!" , true)]
				public FileInfo? urlHttpWebRequest_DownloadFile ( string localFile , TimeSpan timeout )
				{
					try
					{
#pragma warning disable SYSLIB0014 // Type or member is obsolete
						HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fileUrl);
#pragma warning restore SYSLIB0014 // Type or member is obsolete
						request.Timeout = (int)timeout.TotalMilliseconds;
						request.AllowWriteStreamBuffering = false;
						using ( var response = (HttpWebResponse)request.GetResponse() )
						using ( var s = response.GetResponseStream() )
						using ( var fs = new FileStream("test.csv" , FileMode.Create) )
						{
							byte[] buffer = new byte[ 4096 ];
							int bytesRead;
							while ( (bytesRead = s.Read(buffer , 0 , buffer.Length)) > 0 )
							{
								fs.Write(buffer , 0 , bytesRead);
								bytesRead = s.Read(buffer , 0 , buffer.Length);
							}
						}

						return new(localFile);
					}
					catch { return null; }

				}








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



				/// <summary>Download File From Remote Using 'WebClient'</summary>
				/// <param name="localTarget">Local File Path To Save File</param>        
				[Obsolete("Use urlHttpClient_xxx instead!" , false)]
				internal (System.IO.FileInfo? DownloadedFile , AsyncCompletedEventArgs? AsyncDownloadResult)
					urlWebClient_DownloadFile ( System.IO.FileInfo? localTarget = null , Action<DownloadProgressChangedEventArgs>? onProgress = null )
				{
					using ManualResetEvent evtFinished = new(false);

#pragma warning disable SYSLIB0014 // Type or member is obsolete
					using WebClient web = new();
#pragma warning restore SYSLIB0014 // Type or member is obsolete
					try
					{
						FileInfo localCache = localTarget ?? new FileInfo(System.IO.Path.GetTempFileName());
						web.DownloadProgressChanged += ( _ , e ) => onProgress?.Invoke(e);

						AsyncCompletedEventArgs? acea = null;
						web.DownloadFileCompleted += ( _ , e ) =>
						{
							acea = e;
							evtFinished.Set();
						};

						web.DownloadFileAsync(new Uri(fileUrl) , localCache.FullName);
						evtFinished.WaitOne();
						return (DownloadedFile: localCache , AsyncDownloadResult: acea);
					}
					finally { evtFinished.Set(); }
				}





				/*

				public async Task<ActionResult> GetAttachment ( int FileID )
				{
					UriBuilder uriBuilder = new UriBuilder();
					uriBuilder.Scheme = "https";
					uriBuilder.Host = "api.example.com";

					var Path = "/files/download";
					uriBuilder.Path = Path;
					using ( HttpClient client = new HttpClient() )
					{
						client.BaseAddress = new Uri(uriBuilder.ToString());
						client.DefaultRequestHeaders.Accept.Clear();
						client.DefaultRequestHeaders.Add("authorization" , access_token); //if any
						client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
						HttpResponseMessage response = await client.GetAsync(uriBuilder.ToString());

						if ( response.IsSuccessStatusCode )
						{
							System.Net.Http.HttpContent content = response.Content;
							var contentStream = await content.ReadAsStreamAsync(); // get the actual content stream
							return File(contentStream , content_type , filename);
						}
						else
						{
							throw new FileNotFoundException();
						}
					}
				}

				 */




			}


			extension( string url )
			{



				/// <inheritdoc cref="System.Net.WebUtility.UrlEncode" />			
				/// <remarks>Hello World! -> %d0%9f%d1%80%d0%b8%d0%b2%d0%b5%d1%82+%d0%bc%d0%b8%d1%80!</remarks>
				internal string urlEncode () => System.Net.WebUtility.UrlEncode(url);

				/// <inheritdoc cref="Uri.EscapeDataString" />			
				/// <remarks>"Hello World!" -> Hello%20World%21</remarks>
				internal string urlEscapeDataString () => Uri.EscapeDataString(url);

			}

		}


		internal static partial class Extensions_Object
		{


			/// <summary>Compares Classes via ReferenceEquals, and unmanaged via Equals</summary>          
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool equalsUniversal<T> ( this T? A , T? B )
			{
				return (A is null && B is null) || ((A is not null || B is null) && (B is not null || A is null) && (A!.GetType().IsClass
					? Object.ReferenceEquals(A , B)
					: A is IEquatable<T> ea ? ea.Equals(B!) : A is IComparable<T> ca ? ca.CompareTo(B!) == 0 : A.Equals(B)));
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void throwIfNull<T> ( this T? argument , [CallerArgumentExpression(nameof(argument))] string? paramName = null )
				=> ArgumentNullException.ThrowIfNull(argument , paramName: paramName);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void disposeSafe<T> ( this T obj ) where T : IDisposable
			{
				try { obj?.Dispose(); }
				catch { }
			}




			/// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>

			internal static bool compareToNulls<T> ( this T? x , T? y , out int compareResult )
			{
				if ( x is null && y is null )
				{
					compareResult = 0;
					return true;
				}
				else if ( x is null )
				{
					compareResult = -1;
					return true;
				}
				else if ( y is null )
				{
					compareResult = 1;
					return true;
				}
				compareResult = 0;
				return false;
			}


		}


		internal static partial class Extensions_Async_MT
		{


			/// <summary>
			/// Awaiting specifed total 'timeToWait' with delays by 'stepInterval' and checking periodicaly cancelationFunc
			/// </summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static async Task delayWithCancelation ( this int timeToWait , int stepInterval , Func<bool> cancelationFunc )
			{
				if ( stepInterval >= timeToWait )
				{
					throw new ArgumentOutOfRangeException(nameof(stepInterval) , "stepInterval must be < than timeToWait");
				}

				int timeSpent = 0;
				while ( true )
				{
					if ( cancelationFunc.Invoke() )
					{
						return;
					}

					await Task.Delay(stepInterval);
					timeSpent += stepInterval;
					if ( timeSpent >= timeToWait )
					{
						break;
					}
				}
			}





			/// <summary>Run Task synchronously, using SynchronizationContext helpers</summary>
			/// <param name="func">Task<T> method to run</param>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void runSync ( this Func<Task> func )
			{
				SynchronizationContext? oldContext = SynchronizationContext.Current;
				ExclusiveSynchronizationContext synCtx = new();
				SynchronizationContext.SetSynchronizationContext(synCtx);
				synCtx.Post(async _ =>
				{
					try
					{
						await func();
					}
					catch ( Exception e )
					{
						synCtx.InnerException = e;
						throw;
					}
					finally
					{ synCtx.EndMessageLoop(); }
				} , null);
				synCtx.BeginMessageLoop();
				SynchronizationContext.SetSynchronizationContext(oldContext);
			}


			/// <inheritdoc cref="runSync(Func[Task])"/>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T? runSync<T> ( this Func<Task<T?>> func )
			{
				var oldContext = SynchronizationContext.Current;
				var synch = new ExclusiveSynchronizationContext();
				SynchronizationContext.SetSynchronizationContext(synch);
				T? ret = default;
				synch.Post(async _ =>
				{
					try
					{
						ret = await func.Invoke();
					}
					catch ( Exception e )
					{
						synch.InnerException = e;
						throw;
					}
					finally
					{
						synch.EndMessageLoop();
					}
				} , null);
				synch.BeginMessageLoop();
				SynchronizationContext.SetSynchronizationContext(oldContext);
				return ret;
			}




			/// <inheritdoc cref="runSync"/>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void runSync ( this Task tsk )
				=> runSync(() => tsk);


			/// <inheritdoc cref="runSync"/>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T runSync<T> ( this Task<T> task )
				=> runSync(() => task!)!;





			/// <summary>Run Task synchronously using Task.Run helpers</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void runSync2 ( this Task t1 )
			{
				using Task t2 = Task.Run(async () => await t1);
				t2.Wait();
			}

			/// <inheritdoc cref="runSync2"/>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static T runSync2<T> ( this Task<T> t1 )
			{
				using Task t2 = Task.Run(async () => await t1);
				t2.Wait();
				return t1.Result;
			}



			private class ExclusiveSynchronizationContext : SynchronizationContext
			{

				private bool done;
				public Exception? InnerException { get; set; }

				private readonly AutoResetEvent workItemsWaiting = new(false);
				private readonly Queue<Tuple<SendOrPostCallback , object?>> items = new();

				public override void Send ( SendOrPostCallback d , object? state )
				{
					throw new NotSupportedException("We cannot send to our same thread");
				}

				public override void Post ( SendOrPostCallback d , object? state )
				{
					lock ( items )
					{
						items.Enqueue(Tuple.Create(d , state));
					}
					workItemsWaiting.Set();
				}

				public void EndMessageLoop () => Post(_ => done = true , null);

				public void BeginMessageLoop ()
				{
					while ( !done )
					{
						Tuple<SendOrPostCallback , object?>? task = null;
						lock ( items )
						{
							if ( items.Count > 0 )
							{
								task = items.Dequeue();
							}
						}
						if ( task is not null )
						{
							task.Item1(task.Item2);
							if ( InnerException is not null ) // the method threw an exeption
							{
								//throw new AggregateException("AsyncHelpers.Run method threw an exception.", InnerException);
								throw InnerException;
							}
						}
						else
						{
							workItemsWaiting.WaitOne();
						}
					}
				}

				public override SynchronizationContext CreateCopy () => this;
			}









			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Task asTask ( this WaitHandle handle , int timeout = Timeout.Infinite )
			{
				TaskCompletionSource<object> tcs = new();
				RegisteredWaitHandle registration = ThreadPool.RegisterWaitForSingleObject(handle ,
					( state , timedOut ) =>
					{
						//if (state is null ) throw new ArgumentNullException(nameof(state));
						var localTcs = (TaskCompletionSource<object>)state!;

						if ( timedOut )
						{
							localTcs.TrySetCanceled();
						}
						else
						{
#pragma warning disable CS8600, CS8625 // Possible null reference argument for parameter.
							localTcs.TrySetResult(null);
						}
#pragma warning restore CS8600, CS8625 // Possible null reference argument for parameter.

					} ,
				tcs , timeout , executeOnlyOnce: true);

				tcs.Task.ContinueWith(( _ , state ) => ((RegisteredWaitHandle)state!).Unregister(null) , registration , TaskScheduler.Default);
				return tcs.Task;
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Task waitAsync ( this WaitHandle handle , int timeout = Timeout.Infinite )
				=> handle.asTask(timeout);


			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Task waitOneAsync ( this WaitHandle waitHandle )
			{
				if ( waitHandle == null )
					throw new ArgumentNullException(nameof(waitHandle));


				var tcs = new TaskCompletionSource<bool>();
				var rwh = ThreadPool.RegisterWaitForSingleObject(waitHandle ,
					delegate { tcs.TrySetResult(true); } , null , -1 , true);
				var t = tcs.Task;
				t.ContinueWith(( antecedent ) => rwh.Unregister(null));
				return t;
			}




			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void invoke ( this SynchronizationContext sc , Action a )
				=> sc.Send(_ => a.Invoke() , null);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static Task whenCanceled ( this CancellationToken cancellationToken )
			{
				var tcs = new TaskCompletionSource<bool>();
				cancellationToken.Register(s => ((TaskCompletionSource<bool>)s!).SetResult(true) , tcs);
				return tcs.Task;
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static async Task waitAsync ( this Task t , CancellationTokenSource cancel )
			{
#if NET
				await t.WaitAsync(cancel.Token);
#else
				//await Task.WhenAny(t, cancel.Token.WaitHandle.eAsTask());
				await Task.WhenAny(t, cancel.Token.eWhenCanceled());
#endif
			}



			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static async Task startAndWaitAsync ( this Action a )
				=> await Task.Run(a);

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static async Task startAndWaitAsync ( this Task t )
			{
				t.Start();
				await t;
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static async Task<T> startAndWaitAsync<T> ( this Task<T> tsk )
			{
				tsk.Start();
				return await tsk;
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static async Task startAndWaitLongAsync ( this Action a )
			{
				using Task tsk = new(a.Invoke , TaskCreationOptions.LongRunning);
				await tsk.startAndWaitAsync();
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static async Task<T> startAndWaitLongAsync<T> ( this Func<T> a )
			{
				using Task<T> tsk = new(a.Invoke , TaskCreationOptions.LongRunning);
				return await tsk.startAndWaitAsync();
			}


			/// <summary>Exec FUNC. Return result</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static T runSyncLock<T> ( this object rLockObject , Func<T> f )
			{
				lock ( rLockObject )
				{
					return f.Invoke();
				}
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void runAssert_0 (
				this Func<uint> operation ,
				string messageTemplate ,
				params object[] messageArgs )
			{
				uint result = operation.Invoke();
				if ( result != 0 )
				{
					string message = string.Format(messageTemplate , messageArgs);
					throw new Exception($"{message}. Error code {result} (see WinError.h)");
				}
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void runAssert_true (
				this Func<bool> operation ,
				string messageTemplate ,
				params object[] messageArgs )
			{
				bool result = operation.Invoke();
				if ( !result )
				{
					string message = string.Format(messageTemplate , messageArgs);
					throw new Exception($"{message}. Error code {result} (see WinError.h)");
				}
			}



			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static async Task whenAll<T> ( this IEnumerable<Task<T?>> aTasks , bool startTasks , Action<T?> onEachTaskCompleted )
			{
				List<Task<T?>> lTasks = aTasks.ToList();
				if ( startTasks )
				{
					lTasks.ForEach(TSK => TSK.Start()); // Start All Tasks
				}

				while ( lTasks.Count != 0 )
				{
					Task<T?> finishedTask = await Task.WhenAny(lTasks);
					lTasks.Remove(finishedTask); // Remove this task from list
					var rslt = finishedTask.Result;
					onEachTaskCompleted?.Invoke(rslt);
				}
			}


			/// <summary>Check event Flag</summary>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool isSet ( this EventWaitHandle Evt )
				=> Evt?.WaitOne(0 , false) ?? false;


			extension( System.Threading.Timer timer )
			{

				/// <summary>
				/// Полностью останавливает таймер (аналог Stop)
				/// </summary>
				public bool stop ()
				{
					ArgumentNullException.ThrowIfNull(timer);
					return timer.Change(Timeout.Infinite , Timeout.Infinite);   // Timeout.Infinite (-1) останавливает таймер полностью
				}

				/// <summary>
				/// Сбрасывает и перезапускает таймер с новыми или прежними параметрами (аналог Restart)
				/// </summary>
				public bool restart ( TimeSpan period , bool runNow = false )
				{
					ArgumentNullException.ThrowIfNull(timer);

					var due = runNow
						? TimeSpan.Zero
						: period;

					return timer.Change(due , period);
				}
			}



		}


		internal static partial class Extensions_DebugAndErrors
		{


			#region tryCatch



			/// <summary>Try execute action</summary>
			/// <param name="onError">error handler</param>
			/// <returns>Exception if error occurs</returns>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static bool tryCatch (
				this Action a ,
				Action<ExceptionEventArgs>? onError = null ,
				[CallerMemberName] string callerName = "" , [CallerFilePath] string callerFile = "" , [CallerLineNumber] int callerLine = 0 )
			{
				try
				{
					a.Invoke();
					return true;
				}
				catch ( Exception ex )
				{
					if ( onError is not null )
						onError?.Invoke(new ExceptionEventArgs(ex , callerName , callerFile , callerLine));

					else
						ex.Message.debugWriteLine();

					return false;
				}
			}



			/// <inheritdoc cref="tryCatch{T}" />
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Result<T> tryCatch<T> (
				this Func<T?> func ,
				//out T? funcResult ,
				//Action<ExceptionEventArgs>? onError = null ,
				[CallerMemberName] string callerName = "" , [CallerFilePath] string callerFile = "" , [CallerLineNumber] int callerLine = 0 )
			{
				try
				{
					var funcResult = func.Invoke();
					return Result<T>.Success(funcResult);
					//return true;
				}
				catch ( Exception ex )
				{
					//var eea=new ExceptionEventArgs( ex , callerName , callerFile , callerLine );
					//funcResult = default;
					//if ( onError  is not null  )
					//  onError?.Invoke( new ExceptionEventArgs( ex , callerName , callerFile , callerLine ) );
					//else
					ex.Message.debugWriteLine();

					//return false;
					return Result<T>.Failure(ex , callerName , callerFile , callerLine);
				}
			}


			#region tryCatchAsync


			/// <inheritdoc cref="tryCatch{T}" />            
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static async Task<bool> tryCatchAsync (
				this Task tsk ,
				Action<ExceptionEventArgs>? onError = null ,
				[CallerMemberName] string callerName = "" , [CallerFilePath] string callerFile = "" , [CallerLineNumber] int callerLine = 0 )
			{
				try
				{
					await tsk
						.ContinueWith(( t ) =>
						{
							if ( t.IsFaulted ) throw t.Exception;
						} , TaskScheduler.FromCurrentSynchronizationContext());

					return true;
				}
				catch ( Exception ex )
				{
					switch ( ex )
					{
						case OperationCanceledException: break;

						default:
							{
								if ( onError is not null )
									onError?.Invoke(new ExceptionEventArgs(ex , callerName , callerFile , callerLine));
								else
									ex.Message.debugWriteLine();

								break;
							}
					}
					return false;
				}
			}



			/// <inheritdoc cref="tryCatch{T}" />
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static async Task<Result<T>> tryCatchAsync<T> (
				this Task<T> tsk ,
				T? defaultValue = default ,
				//Action<ExceptionEventArgs>? onError = null ,
				[CallerMemberName] string callerName = "" , [CallerFilePath] string callerFile = "" , [CallerLineNumber] int callerLine = 0 )
			{

				try
				{
					var rslt = await tsk
						.ContinueWith(( t ) =>
						{
							return t.IsFaulted ? throw t.Exception : t.Result;
						} , TaskScheduler.FromCurrentSynchronizationContext());

					return Result<T>.Success(rslt);
				}
				catch ( Exception ex )
				{
					switch ( ex )
					{
						case OperationCanceledException cex:
							return Result<T>.Failure(cex , callerName , callerFile , callerLine);

						//return Result<T>.Canceled( cex );
						//break;

						default:
							{
								//if ( onError  is not null  )
								//  onError?.Invoke( new ExceptionEventArgs( ex , callerName , callerFile , callerLine ) );
								//else
								ex.Message.debugWriteLine();

								//return Result<T>.Failure( ex );
								return Result<T>.Failure(ex , callerName , callerFile , callerLine);
								//break;
							}
					}
					//                    return Result<T>.Failure( defaultValue );
				}
			}


			/// <inheritdoc cref="tryCatch{T}" />
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static async Task<Result<T>> tryCatchAsync<T> (
				this Func<T> func ,
				CancellationToken? cancel = null ,
				//Action<ExceptionEventArgs>? onError = null ,
				[CallerMemberName] string callerName = "" , [CallerFilePath] string callerFile = "" , [CallerLineNumber] int callerLine = 0 )
			{
				try
				{
					Task<T> tsk = (cancel is null)
						? Task.Factory.StartNew(func)
						: Task.Factory.StartNew(func , cancel.Value!);

					var rslt = await tsk
						.ContinueWith(( t ) =>
						{
							return t.IsFaulted
							? throw t.Exception
							: t.Result;
						} , TaskScheduler.FromCurrentSynchronizationContext());

					return Result<T>.Success(rslt);

				}
				catch ( Exception ex )
				{
					switch ( ex )
					{
						case OperationCanceledException cex:
							return Result<T>.Failure(cex , callerName , callerFile , callerLine);

						default:
							{
								ex.Message.debugWriteLine();
								return Result<T>.Failure(ex , callerName , callerFile , callerLine);
								/*
								//			ex.eLogError(onErrorShowUI, supressAnyModalPopEvenInDEBUG: true, callerName: callerName, callerFile: callerFile, callerLine: callerLine);
								if ( cancel is null || !cancel.Value.IsCancellationRequested )
								{
									if ( onError  is not null  )
										onError?.Invoke( new ExceptionEventArgs( ex , callerName , callerFile , callerLine ) );
									else

								}
								break;
								 */
							}
					}

					//return (defaultValue, ex);
				}
			}

			#endregion


			#endregion



			[Conditional("DEBUG")]
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void debugWriteLine ( this string sMessage )
			{
#if DEBUG
				Debug.WriteLine(sMessage);
#endif
			}


			[Conditional("DEBUG")]
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void debugWrite ( this string sMessage )
			{
#if DEBUG
				Debug.Write(sMessage);
#endif
			}






			#region eFullDump


			// ''<summary>Фиксация ошибки в журнале и в DEBUG MODE вывод сообщения</summary>
			//			internal static string eDumpTree(this Exception ex, [CallerMemberName] string caller = "")				=> ex.eDumpExceptionTree(caller) + CS_CONSOLE_SEPARATOR;

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string fullDump ( this Exception? ex ,
				[CallerMemberName] string callerName = "" ,
				[CallerFilePath] string callerFile = "" ,
				[CallerLineNumber] int callerLine = 0 )
			{
				if ( ex is null )
				{
					return string.Empty;
				}

				StringBuilder sbExceptionTree = new();
				using StringWriter sw = new(sbExceptionTree);

				sw.WriteLine($"{ex.GetType()}: '{ex.Message}'");
				sw.WriteLine($"Caller: '{callerName}', File: '{callerFile}', Line: {callerLine}");
				sw.WriteLine($"StackTrace:\n{ex.StackTrace}");

				if ( ex.InnerException is not null )
				{
					sw.WriteLine($"Exception Stack Tree:");
					while ( ex.InnerException is not null )
					{
						ex = ex.InnerException;
						sw.WriteLine($"{ex.GetType()}\n{ex.Message}");
					}
				}
				return sbExceptionTree.ToString();
			}


			#endregion




#if !ANDROID


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void logError_CONSOLE ( this Exception ex ,
				ConsoleColor? clrFore = ConsoleColor.Red ,
				ConsoleColor? clrBack = null ,
				bool fullDump = false ,
				[CallerMemberName] string callerName = "" ,
				[CallerFilePath] string callerFile = "" ,
				[CallerLineNumber] int callerLine = 0 )
			{
				const string C_CONSOLE_ERROR_HEADER = "*** ERROR:\n";
				string msg = C_CONSOLE_ERROR_HEADER + ex.Message;

				if ( fullDump )
				{
					msg = C_CONSOLE_ERROR_HEADER + ex.fullDump(callerName , callerFile , callerLine);
				}
				Console.writeLine($"{constants.ConsoleConst.SEPARATOR_70}\n{msg}\n{constants.ConsoleConst.SEPARATOR_70}" , clrFore , clrBack);
			}

#endif



		}


		internal static partial class Extensions_DebugAndErrors_Result
		{
			/*

			public static Result<TOut> tryCatch<TIn, TOut> ( this Result<TIn> result , Func<TIn , TOut> func , Exception error )
			{
				try
				{
					return result.IsSuccess ?
						Result<TOut>.Success( func( result.Value ) ) :
						Result<TOut>.Failure( result.Error );
				}
				catch
				{
					return Result<TOut>.Failure( error );
				}
			}


			public static Result<TIn> tap<TIn> ( this Result<TIn> result , Action<TIn> action )
			{
				if ( result.IsSuccess )
				{
					action( result.Value );
				}

				return result;
			}


			public static TOut match<TIn, TOut> (
				this Result<TIn> result ,
				Func<TIn , TOut> onSuccess ,
				Func<Exception , TOut> onFailure )
			{
				return result.IsSuccess ?
					onSuccess( result.Value ) :
					onFailure( result.Error );
			}
			 */


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Result<TOut> bind<TIn, TOut> ( this Result<TIn> result , Func<TIn , Result<TOut>> bind )
			{
				return result.IsSuccess
					? bind(result.Value!)
					: Result<TOut>.Failure(result.ExceptionArgs!);
			}

		}


		internal static partial class Extensions_Memory
		{


#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static unsafe T* pointerTo<T> ( this ReadOnlySpan<T> from ) where T : struct
				=> *(T**)&from;

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static unsafe T* pointerTo<T> ( this Span<T> from ) where T : struct
				=> *(T**)&from;
#pragma warning restore CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type




			/// <inheritdoc cref="Marshal.StructureToPtr"/>
			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void structureToPtr<T> ( this T structureRef , IntPtr ptrTarget , bool deleteOld = false ) where T : struct
				=> Marshal.StructureToPtr(structureRef , ptrTarget , deleteOld);



			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static unsafe TTarget castUnsafe<TSource, TTarget> ( this TSource input )
				where TSource : unmanaged
				where TTarget : unmanaged
			{
				return *(TTarget*)&input;
			}





			extension( IntPtr ptrSource )
			{

				public bool isValid => (ptrSource != IntPtr.Zero) && (ptrSource != uom.constants.HANDLE_INVALID);
				public bool isNotValid => !ptrSource.isValid;


				/// <summary>Возвращает строки из REG_MULTI_SZ (где каждая строка отделена [0] а в конце [00])</summary>
				[Obsolete("!!!  eReadMiltiStringZUni MUST BE REWRITED!!!!" , true)]
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal string[] readMiltiStringZUni ()
				{
					const int UNICODE_CHAR_SIZE = 2;

#pragma warning disable IDE0028 // Simplify collection initialization
					List<string> rows = new(100);
#pragma warning restore IDE0028 // Simplify collection initialization

					var ptr = ptrSource;
					do
					{
						if ( Marshal.ReadByte(ptr) == 0 ) break; // we have not any text if first byte is zero

						var sLine = Marshal.PtrToStringUni(ptr);
						if ( sLine.isNullOrEmpty || sLine!.Length < 1 )
							break; // no more rows
						else
						{
							rows.Add(sLine);
							int iOffset = (sLine.Length + 1) * UNICODE_CHAR_SIZE;
							ptr += iOffset;
						}
					}
					while ( true );
					return [ .. rows ];
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal byte[] toBytes_Marshal ( int nBytes )
				{
					var data = new byte[ nBytes ];
					Marshal.Copy(ptrSource , data , 0 , nBytes);
					return data;
				}

			}

			extension<TStruct> ( IntPtr ptrSource ) where TStruct : struct
			{
				/// <inheritdoc cref="Marshal.PtrToStructure(IntPtr)"/>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal TStruct toStructure_Marshal () => Marshal.PtrToStructure<TStruct>(ptrSource);


				/// <summary>Последовательно читаем с указателя в массив одинаковых структур</summary>
				/// <param name="structCount">Количество структур для чтения</param>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal IEnumerable<TStruct> readStructuresSequential_Marshal ( int structCount , int readOffset = 0 )
				{
					var ptr = ptrSource;
					if ( readOffset != 0 ) ptr += readOffset;

					int structSize = Marshal.SizeOf<TStruct>();
					for ( int structIndex = 1 ; structIndex <= structCount ; structIndex++ )
					{
						TStruct structInstance = ptr.toStructure_Marshal<TStruct>();
						yield return structInstance;
						ptr += structSize;
					}
				}
			}


			extension( SafeHandle source )
			{

				public bool isValid => (source is not null) && (!source.IsInvalid);
				public bool isNotValid => !source.isValid;

			}

			extension( HandleRef source )
			{

				public bool isValid => source.Handle.isValid;
				public bool isNotValid => !source.isValid;

			}

			extension( object obj )
			{

				#region RunWithGCPinned

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal void runWithGCPinned ( Action<IntPtr> a , GCHandleType handleType = GCHandleType.Pinned )
				{
					var gch = GCHandle.Alloc(obj , handleType);
					try
					{
						IntPtr ptr = gch.AddrOfPinnedObject();
						a.Invoke(ptr);
					}
					finally
					{
						gch.Free();
					}
				}

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal T? runWithGCPinned<T> ( Func<IntPtr , T?> a , GCHandleType handleType = GCHandleType.Pinned )
				{
					var gch = GCHandle.Alloc(obj , handleType);
					try
					{
						IntPtr ptr = gch.AddrOfPinnedObject();
						return a.Invoke(ptr);
					}
					finally
					{
						gch.Free();
					}
				}

				#endregion

			}

			extension( IEnumerable<object> objList )
			{

				#region RunWithGCPinned

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal void runWithGCPinned ( Action<IReadOnlyList<IntPtr>> a , GCHandleType handleType = GCHandleType.Pinned )
				{
					var gchList = objList
						.Select(obj => GCHandle.Alloc(obj , handleType))
						.ToList();

					try
					{
						var ptrList = gchList.Select(gch => gch.AddrOfPinnedObject()).ToList();
						a.Invoke(ptrList);
					}
					finally
					{
						foreach ( var gch in gchList )
							gch.Free();

						gchList.Clear();
					}
				}

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal T? runWithGCPinned<T> ( Func<IReadOnlyList<IntPtr> , T?> a , GCHandleType handleType = GCHandleType.Pinned )
				{
					var gchList = objList
						.Select(obj => GCHandle.Alloc(obj , handleType))
						.ToList();

					try
					{
						var ptrList = gchList.Select(gch => gch.AddrOfPinnedObject()).ToList();
						return a.Invoke(ptrList);
					}
					finally
					{
						foreach ( var gch in gchList )
							gch.Free();

						gchList.Clear();
					}

				}

				#endregion

			}


			extension<T> ( ArrayPool<T> )
			{

				/// <inheritdoc cref="uom.mem.ArrayPoolHelper.Rent(uint,bool,bool)"/>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static uom.mem.ArrayPoolHelper.DisposableSharedArrayContainer<T> rent ( uint minimumLength , bool zeroArray = false , bool clearOnDispose = uom.mem.ArrayPoolHelper.CLEAR_ON_DISPOSE_DEFAULT )
					=> uom.mem.ArrayPoolHelper.Rent<T>(minimumLength , zeroArray , clearOnDispose);

				/// <inheritdoc cref="uom.mem.ArrayPoolHelper.Rent(int,bool,bool)"/>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static uom.mem.ArrayPoolHelper.DisposableSharedArrayContainer<T> rent ( int minimumLength , bool zeroArray = false , bool clearOnDispose = uom.mem.ArrayPoolHelper.CLEAR_ON_DISPOSE_DEFAULT )
					=> uom.mem.ArrayPoolHelper.Rent<T>(minimumLength , zeroArray , clearOnDispose);


				/// <inheritdoc cref="uom.mem.ArrayPoolHelper.RentBytes(uint,bool,bool)"/>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static uom.mem.ArrayPoolHelper.DisposableSharedArrayContainer<byte> rentBytes ( uint minimumLength , bool zeroArray = false , bool clearOnDispose = uom.mem.ArrayPoolHelper.CLEAR_ON_DISPOSE_DEFAULT )
					=> uom.mem.ArrayPoolHelper.RentBytes(minimumLength , zeroArray , clearOnDispose);

				/// <inheritdoc cref="uom.mem.ArrayPoolHelper.RentBytes(int,bool,bool)"/>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static uom.mem.ArrayPoolHelper.DisposableSharedArrayContainer<byte> rentBytes ( int minimumLength , bool zeroArray = false , bool clearOnDispose = uom.mem.ArrayPoolHelper.CLEAR_ON_DISPOSE_DEFAULT )
					=> uom.mem.ArrayPoolHelper.RentBytes(minimumLength , zeroArray , clearOnDispose);




			}


			extension( MemoryExtensions.SpanSplitEnumerator<char> token )
			{

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public ReadOnlySpan<char> itemAtIndex ( int index )
				{
					int curreent = 0;

					// Перебираем элементы внутри обычного цикла, превращая их в строки
					while ( token.MoveNext() )
					{
						if ( curreent == index )
						{
							var range = token.Current;
							ReadOnlySpan<char> text = token.Source[ range.Start..range.End ];
							return text;
						}

						curreent++;
					}
					throw new ArgumentOutOfRangeException(nameof(index));
				}


				//[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public ReadOnlySpan<char> this[ int index ]
					=> token.itemAtIndex(index);


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string[] toArray ()
				{
					List<string> result = [];

					while ( token.MoveNext() )
					{
						var range = token.Current;
						ReadOnlySpan<char> text = token.Source[ range.Start..range.End ];
						result.Add(text.ToString());
					}

					return [ .. result ];
				}
			}



		}

		internal static partial class Extensions_Globalization
		{

			extension( CultureInfo culture )
			{

				/// <summary>Возвращает корневой элемент для дерева языков.
				/// для Ru-Ru будет RU,
				/// для EN-US будет EN</summary>

				internal CultureInfo getTopParent ()
				{
					while ( culture.Parent is { Name: var name } && name.isNotNullOrWhiteSpace )
					{
						culture = culture.Parent;
					}

					return culture;
				}

				/// <summary>Это российское дерево языков. 
				/// Любой язык, где корневой элемент = RU</summary>
				public bool isRussianTree => culture.getTopParent().LCID == 25;




			}

			extension( string source )
			{


				/// <summary>Находит все кириллические символы в строке</summary>
				public Match[] rxMatches_Cyr ()
					=> Regex.Matches(source , uom.constants.GlobalizationConst.RX_PATTERN_ALPHABET_CYRILLIC).ToArray();

				/// <summary>Находит все латинские символы в строке</summary>
				public Match[] rxMatches_Lat ()
					=> Regex.Matches(source , uom.constants.GlobalizationConst.RX_PATTERN_ALPHABET_LAT).ToArray();


			}
		}


		internal static partial class Extensions_Process
		{

			extension( Environment.SpecialFolder sf )
			{

				public DirectoryInfo value => new(Environment.GetFolderPath(sf));


			}
		}


		internal static partial class Extensions_Reflection_Assembly_Resources
		{



			/*

			//[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static string selfNameIf<T> ( this T source , Func<T , bool> predicate , [CallerArgumentExpression(nameof(source))] string argExpression = "" )
				=> source.selfIf(predicate , v => argExpression);

				/// <summary>Подставляет самого себя в форматированном виде, если не пустая строка</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string selfIf ( Func<T , bool> predicate , Func<T , string> outFormatter , string nullValue = "" )
					=> predicate.Invoke(source)
						? outFormatter.Invoke(source)
						: nullValue;
				 */
			extension( bool source )
			{


				/// <summary>Подставляет самого себя в форматированном виде, если не пустая строка</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string formatReadable_IfTrue ( string trueValue , string falseValue = "" )
					=> source ? trueValue : falseValue;

				/*
				/// <summary>Подставляет самого себя в форматированном виде, если не пустая строка</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string formatReadable ( Func<bool , string> outFormatter , string falseValue = "" )
					=> source.formatReadable_IfTrue(outFormatter.Invoke(source) , falseValue);
				 */


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string formatReadable_AsNameOfSelf ( string falseValue = "" , [CallerArgumentExpression(nameof(source))] string argExpression = "" )
					=> source.formatReadable_IfTrue(argExpression , falseValue);

			}

			extension( bool? source )
			{


				/// <summary>Подставляет самого себя в форматированном виде, если не пустая строка</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string formatReadable_IfTrue ( string trueValue , string falseValue = "" )
					=> (source.HasValue && source.Value).formatReadable_IfTrue(trueValue , falseValue);


			}

			extension<T> ( T source )
			{

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string formatReadable_IfNotNull ( string notNullValue , string nullValue = "" )
					=> (source is not null).formatReadable_IfTrue(notNullValue , nullValue);

			}

			extension( string? source )
			{

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string formatReadable_IfNotNullOrWhitespace ( string notNullOrWhitespaceValue , string nullValue = "" )
					=> source.isNotNullOrWhiteSpace.formatReadable_IfTrue(notNullOrWhitespaceValue , nullValue);

			}

			extension<T> ( IEnumerable<T> source )
			{

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string formatReadable_IfAny ( string notEmptyValue , string nullValue = "" )
					=> (source is { any: true })
						.formatReadable_IfTrue(notEmptyValue , nullValue);

			}




			extension( Type t )
			{

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal TypeCode getTypeCode () => Type.GetTypeCode(t);


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal object? createInstance ( params object[] constructorArgs )
				{
					if ( constructorArgs.Length < 1 )
						return Activator.CreateInstance(t);


					Type[] argTypeList = [ .. constructorArgs.Select(arg => arg.GetType()) ];
					var constructorWithThisArgsType = t.GetConstructor(argTypeList);

					return null == constructorWithThisArgsType
						? throw new AmbiguousMatchException($"Cant find class constructor for type '{t.FullName}' with specifed args count ('{constructorArgs.Length}') and arg types!")
						: constructorWithThisArgsType.Invoke(constructorArgs);
				}

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal T? createInstanceOf<T> ( params object[] constructorArgs ) => (T?)t.createInstance(constructorArgs);


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal IntPtr fieldOffset_ptr ( string fieldName ) => Marshal.OffsetOf(t , fieldName);

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public int fieldOffset_32 ( string fieldName ) => t.fieldOffset_ptr(fieldName).ToInt32();

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public long fieldOffset_64 ( string fieldName ) => t.fieldOffset_ptr(fieldName).ToInt64();


			}


			extension( object? value )
			{

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal bool tryCast<T> ( out T? newValue ) where T : class
				{
					newValue = default;
					if ( value is null ) return false;

					try
					{
						newValue = value as T;
						return newValue is not null;
					}
					catch ( Exception ex )
					{
						// This was originally used to help me figure out why some types weren't casting in Convert.ChangeType.
						// It could be removed, but you never know, somebody might comment on a better way to do THAT to.
						var traceMessage = ex is InvalidCastException || ex is FormatException || ex is OverflowException
							? $"The given value {value} could not be cast to '{typeof(T).FullName}'!"
							: ex.Message;

						Trace.WriteLine(traceMessage);
					}
					return false;
				}



				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal Dictionary<string , string> dumpMembersAsDictionary
					(
					string nullValuePlaceholder = "null" ,
					bool skipNullValuedPropertiesAndFields = false ,
					BindingFlags bindingAttr = BINDING_FLAGS_PROPERTIES_PUBLIC ,
					constants.MemberTypes memberType = constants.MemberTypes.Property ,
					Func<object? , MemberInfo , string?>? valueToStringConverter = null
					)
				{
					if ( value is null ) return [];

					Dictionary<string , string> dicMembers = [];
					var t = value.GetType();
					var members = t.GetMembers(bindingAttr);

					foreach ( var mi in members )
					{
						string valStr = mi.MemberType.ToString();
						switch ( mi )
						{
							case PropertyInfo pi:
								{
									if ( !memberType.HasFlag(constants.MemberTypes.Property) )
									{
										continue;
									}

									if ( pi.CanRead )
									{
										object? val = pi.GetValue(value , []);
										if ( val is null && skipNullValuedPropertiesAndFields )
										{
											continue;
										}

										/*
										if (val  is not null )
										{
											var vt = val.GetType();
											if (vt.IsGenericType && vt.GetGenericTypeDefinition() == typeof(Nullable<>))
											{
												int dddd = 5;
												//…
											}

										}
										 */

										valStr = valueToStringConverter?.Invoke(val , mi)
											?? (val ?? nullValuePlaceholder).ToString()
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
									if ( !memberType.HasFlag(constants.MemberTypes.Field) )
									{
										continue;
									}

									object? val = fi.GetValue(value);
									if ( val is null && skipNullValuedPropertiesAndFields )
									{
										continue;
									}

									valStr = valueToStringConverter?.Invoke(val , mi)
											?? (val ?? nullValuePlaceholder).ToString()
											?? nullValuePlaceholder;
								}
								break;

							case MethodInfo:
								{
									if ( !memberType.HasFlag(constants.MemberTypes.Method) )
									{
										continue;
									}
								}
								break;

							default:
								break;
						}
						dicMembers.Add(mi.Name , valStr);
					}
					return dicMembers;
				}



				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public object? getPropertyValue ( string propertyName , object? defaultValue = null , BindingFlags binding = BINDING_FLAGS_PUBLIC_NONPUBLIC )
				{
					PropertyInfo? piID = value?.GetType()?.GetProperty(propertyName , binding);
					return null == piID
						? defaultValue
						: piID.GetValue(value , null);
				}

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public T? getPropertyValue<T> ( string propertyName , T? defaultValue = default , BindingFlags binding = BINDING_FLAGS_PUBLIC_NONPUBLIC )
					=> (T?)value.getPropertyValue(propertyName , (object?)defaultValue , binding);

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public int? getPropertyValue_Int32 ( string propertyName , int? defaultValue = default , BindingFlags binding = BINDING_FLAGS_PUBLIC_NONPUBLIC )
					=> value.getPropertyValue<int?>(propertyName , defaultValue: defaultValue , binding: binding);

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void setPrivateField ( string fieldNameStartWith , object newValue , BindingFlags binding = BINDING_FLAGS_NONPUBLIC )
				{
					value?
						.GetType()
						.GetFields(binding)
						.FirstOrDefault(f => f.Name.StartsWith(fieldNameStartWith , StringComparison.OrdinalIgnoreCase))?
						.SetValue(value , newValue);
				}









			}


			extension<T> ( T value ) where T : class
			{

				#region copyPropertyValues


				/// <summary>Копируем значения всех свойств</summary>
				/// <param name="target">У этого свойства заполняются данными</param>
				/// <param name="source">У этого объекта берутся значения свойств.</param>
				/// <param name="throwIfNotFound">Вызываеть ошибку если необходимое свойство не найдено в источнике данных</param>
				internal void copyPropertyValuesFrom<TSource> (
					TSource source ,
					IEnumerable<string> propertyNames ,
					BindingFlags bf = BINDING_FLAGS_PUBLIC ,
					bool throwIfNotFound = true )
				{

					ArgumentNullException.ThrowIfNull(value , nameof(value));
					ArgumentNullException.ThrowIfNull(source , nameof(source));

					var srcProps =
						source
						.GetType()
						.GetProperties(bf)
						.Where(p => p.CanRead)
						.IntersectBy(propertyNames , pd => pd.Name)
						.ToDictionary(p => p.Name);


					T target = value;
					PropertyInfo[] trgProps = [..
						target
						.GetType()
						.GetProperties(bf)
						.Where(p => p.CanWrite)
						.IntersectBy(propertyNames, pd => pd.Name)
					];


#if DEBUG
					StringBuilder sbCopiedProperties = new();
					sbCopiedProperties.AppendLine($"copyPropertyValuesFrom({source.GetType()}->{target.GetType()}):");
#endif
					foreach ( var propTarget in trgProps )
					{
						if ( !srcProps.TryGetValue(propTarget.Name , out var propSource) && throwIfNotFound )
							throw new ArgumentOutOfRangeException(nameof(source) , $"Property '{propTarget.Name}' was not found in '{source}' object!");

						object? val = propSource!.GetValue(source);
						propTarget.SetValue(target , val);
#if DEBUG
						sbCopiedProperties.AppendLine($"\t{propTarget.Name}', New value: '{val?.ToString() ?? "NULL"}'");
#endif
					}
#if DEBUG
					//sbCopiedProperties.AppendLine();
					Debug.WriteLine(sbCopiedProperties.ToString());
#endif

				}


				/// <summary>Копируем значения всех свойств</summary>
				/// <param name="source">get property values from</param>
				internal void copyPropertyValuesFrom<TSource> (
					TSource source ,
					PROPERTY_LIST_SOURCES propertyListSource ,
					BindingFlags bf = BINDING_FLAGS_PUBLIC ,
					Func<PropertyInfo , bool>? canCopyProperty = null ,
					bool throwIfNotFound = true )
				{

					T target = value;
					var propList = propertyListSource switch
					{
						PROPERTY_LIST_SOURCES.Source => source!.GetType().GetProperties(bf),
						PROPERTY_LIST_SOURCES.Target => target!.GetType().GetProperties(bf),
						_ => throw new ArgumentOutOfRangeException(nameof(propertyListSource))
					};

					string[] propNames = [..
						propList                        .Where(pi => canCopyProperty?.Invoke(pi) ?? true)
						.Select(pi => pi.Name)
						];

					target.copyPropertyValuesFrom(source , [ .. propNames ] , bf , throwIfNotFound);
				}


				#endregion



				public void setPropertiyAttribute<TAttr> (
					object propertyValue ,
					object newAttributeValue ,
					[CallerArgumentExpression(nameof(propertyValue))] string propName = ""
					) where TAttr : System.Attribute
				{

					if ( !propName.Contains('.') ) throw new ArgumentOutOfRangeException(nameof(propertyValue));

					object targetObject = value ?? throw new ArgumentNullException(nameof(value) , "Target object is null!");

					propName = propName.Split('.').Last();

					PropertyDescriptor[] props = [ .. TypeDescriptor.GetProperties(targetObject.GetType()).Cast<PropertyDescriptor>() ];
					props = [ .. props.Where(pd => pd.Name == propName) ];

					PropertyDescriptor? pd = props.FirstOrDefault();
					if ( pd is null )
					{
						throw new ArgumentOutOfRangeException(nameof(propertyValue));
					}

					string attrPrefix = @"<" + typeof(TAttr).Name;
					//pd.eSetAttributeValueOf<TAttribute>(attrPrefix, attributeValue);
					var a = pd.getAttribute<TAttr>();

					BindingFlags bFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.FlattenHierarchy;
					a.setPrivateField(attrPrefix , newAttributeValue , bFlags);
				}


				/// <summary>
				/// Enables Readonly attribute as specifed class properties for edit in ProperGrig!
				/// WARNING! Readonly changes for not only THIS class instance, but for all classes with use this class defenition in app domain!
				/// bc. you must to reset this attribute value, imediately after property grid is closed, for avoid unpredictable effects...
				/// 
				/// readonly attribute has all fields in class, not only direct specifed in code!
				/// </summary>
				[Obsolete("OBSOLETE! this changes class property visibility across all app instance, but not only for property grid!. Use Dynamic browsable attribute instead!" , true)]
				public void setPropertiesAttributeForPropertyGrid_Browsable ( bool browsable , Func<PropertyDescriptor , bool>? wherePredicate = null )
				{
					PropertyDescriptor[] pdList = [ .. TypeDescriptor.GetProperties(value!.GetType()).Cast<PropertyDescriptor>() ];
					if ( wherePredicate is not null )
						pdList = [ ..
							pdList
							.Where(pd => wherePredicate(pd))
							];

					pdList.forEach(pd => pd.setAttribute_Browsable(browsable));
				}


				/// <summary>
				/// Enables Readonly attribute as specifed class properties for edit in ProperGrig!
				/// WARNING! Readonly changes for not only THIS class instance, but for all classes with use this class defenition in app domain!
				/// bc. you must to reset this attribute value, imediately after property grid is closed, for avoid unpredictable effects...
				/// 
				/// readonly attribute has all fields in class, not only direct specifed in code!
				/// </summary>
				[Obsolete("OBSOLETE! this changes class property writeable across all app instance, but not only for property grid!. Use Dynamic readonly attribute instead!" , true)]
				public void setPropertiesAttributeForPropertyGrid_ReadOnly ( bool readOnly , Func<PropertyDescriptor , bool>? wherePredicate = null )
				{
					PropertyDescriptor[] pdList = TypeDescriptor.GetProperties(value!.GetType()).Cast<PropertyDescriptor>().ToArray();
					if ( wherePredicate is not null )
						pdList = [ ..
							pdList.Where(pd => wherePredicate(pd))
							];

					pdList.forEach(pd => pd.setAttribute_ReadOnly(readOnly));
				}






			}


			extension( PropertyDescriptor pd )
			{

				public T? getAttribute<T> () where T : System.Attribute => (T?)pd.Attributes[ typeof(T) ];


				public void setAttributeValue<TAttr> (
					string attributeInternalFiledStartWith ,
					object newValue ,
					BindingFlags bFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.FlattenHierarchy
					) where TAttr : Attribute
						=> pd.getAttribute<TAttr>()?.setPrivateField(attributeInternalFiledStartWith , newValue , bFlags);

				public void setAttribute_Browsable ( bool browsable )
					=> pd.setAttributeValue<BrowsableAttribute>("<Browsable" , browsable);


				public void setAttribute_ReadOnly ( bool readOnly )
					=> pd.setAttributeValue<ReadOnlyAttribute>("<isReadOnly" , readOnly);


			}







			extension( Assembly asm )
			{


				public string[] getManifestResourceNames ( Func<string , bool> resFileNamePredicate )
					=> [ ..
						asm
						.GetManifestResourceNames()
						.Where(resFileName => resFileNamePredicate.Invoke(resFileName))
						];


				public string getManifestResourceName ( string nameSuffix )
					=> asm
						.getManifestResourceNames(str => str.EndsWith(nameSuffix))
						.Single() ?? throw new ArgumentOutOfRangeException(nameof(nameSuffix));


				public Stream getManifestResourceStream ( string nameSuffix )
					=> asm.GetManifestResourceStream(asm.getManifestResourceName(nameSuffix))!;


				public (Stream @Stream , StreamReader Reader) getManifestResourceStreamReader ( string nameSuffix , Encoding? e = null , bool detectEncodingFromByteOrderMarks = false )
				{
					e ??= Encoding.Unicode;

					Stream stream = asm.getManifestResourceStream(nameSuffix)!;
					StreamReader sr = detectEncodingFromByteOrderMarks
						? new(stream , true)
						: new(stream , e , false);

					return (stream , sr);
				}



				/// <summary>Searches specifed assembly for file and load it
				/// Sample:
				/// txtTextBox.Text = uom.AppInfo.Assembly.eReadResourceFileAsString("EULA.txt");
				/// </summary>
				/// <param name="asm">assembly that contains file resource</param>
				/// <param name="nameSuffix">Resource File Name</param>
				/// <param name="e">Resource File Encoding</param>
				public string readResourceFileAsString ( string nameSuffix , Encoding? e = null , bool detectEncodingFromByteOrderMarks = false )
				{
					var r = asm.getManifestResourceStreamReader(nameSuffix , e , detectEncodingFromByteOrderMarks);
					using Stream s = r.Stream;
					using StreamReader sr = r.Reader;
					return sr.ReadToEnd();
				}



				public async Task<string> readResourceFileAsStringAsync ( string nameSuffix , Encoding? e = null , bool detectEncodingFromByteOrderMarks = false )
				{
					var r = asm.getManifestResourceStreamReader(nameSuffix , e , detectEncodingFromByteOrderMarks);
					using Stream s = r.Stream;
					using StreamReader sr = r.Reader;
					return await sr.ReadToEndAsync();
				}

			}


		}

		internal static partial class Extensions_Serialize_Clone
		{


			#region Serialize


			internal static void eWriteCSV<T> ( this TextWriter TW , string[] aColumnHeadersArray , IEnumerable<T> aRows , Func<T , string[]> cbGetRowValuesArray , string C_CSV_SEPARATOR = ";" , bool MakeSafeChars = false )
			{

				//</summary> C_CSV_SEPARATOR = ";"
				int iColumnCount = aColumnHeadersArray.Count();

				static string cbPrepareValue ( string sSourceValue )
				{
					sSourceValue = sSourceValue.ensureNotNullOrWhiteSpace();
					sSourceValue = sSourceValue.Replace("\"" , "\"\"").Replace("," , @"\,").Replace(";" , @"\;").Replace(Environment.NewLine , @"\" + Environment.NewLine).Replace(@"\" , @"\\");
					return sSourceValue;
				}
				;

				if ( MakeSafeChars )
				{
					aColumnHeadersArray = (from sColumnValue in aColumnHeadersArray
										   let S = cbPrepareValue(sColumnValue)
										   select S).ToArray();
				}

				var sHeaderLine = aColumnHeadersArray.join(C_CSV_SEPARATOR);
				TW.WriteLine(sHeaderLine);
				foreach ( var CP in aRows )
				{
					var aValuesArray = cbGetRowValuesArray(CP);
					if ( aValuesArray.Count() != iColumnCount )
					{
						throw new Exception("Current Row aValuesArray.Count <> aColumnHeadersArray.Count!");
					}

					if ( MakeSafeChars )
					{
						aValuesArray = aValuesArray
							.Select(columnValue => cbPrepareValue(columnValue))
							.ToArray();
					}

					string sLine = aValuesArray.join(C_CSV_SEPARATOR)!;
					TW.WriteLine(sLine);
				}
			}






			#region XML Serialization


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eSerializeXML ( this object SerializableObject , string FileName )
				=> SerializableObject.eSerializeXML(FileName , Encoding.Unicode);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eSerializeXML ( this object SerializableObject , string file , Encoding? enc = null )
				=> SerializableObject.eSerializeXML(file.pathToFileInfo()! , enc ??= Encoding.Unicode);


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eSerializeXML ( this object SerializableObject , FileInfo f , Encoding? enc = null )
			{
				using FileStream fs = f.Create();
				SerializableObject.eSerializeXML(fs , enc ??= Encoding.Unicode);
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void eSerializeXML ( this object SerializableObject , Stream s , Encoding? enc = null )
			{
				using StreamWriter sw = new(s , enc ??= Encoding.Unicode , 2048 , true);
				System.Xml.Serialization.XmlSerializer XS = new(SerializableObject.GetType());
				XS.Serialize(sw , SerializableObject);
				sw.Flush();
			}


			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string serializeAsXML<TObj> ( this TObj SerializableObject ) //where TObj : ISerializable
			{
				StringBuilder sb = new();
				using ( StringWriter sw = new(sb) )
				{
					System.Xml.Serialization.XmlSerializer xs = new(typeof(TObj));
					xs.Serialize(sw , SerializableObject);
				}
				return sb.ToString();
			}

			//using System.Text.Json;

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static string serializeAsJson<TObj> ( this TObj SerializableObject , JsonSerializerOptions? opt = null ) where TObj : class
			{
				/*
				StringBuilder sb = new();
				using ( StringWriter sw = new(sb) )
				{
					System.Xml.Serialization.XmlSerializer xs = new(typeof(TObj));
					xs.Serialize(sw , SerializableObject);
				}
				return sb.ToString();
				 */
				// Будет работать со словарями без каких-либо ошибок
				return JsonSerializer.Serialize(SerializableObject , opt ?? new JsonSerializerOptions { WriteIndented = true });
			}

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static TObj? deserializeFromJsonString<TObj> ( this string jsonString ) where TObj : class
			{
				if ( jsonString.isNullOrWhiteSpace ) return null;//				default;
																 // Восстанавливает любой объект, включая ваши словари Dictionary<string, bool>
				return JsonSerializer.Deserialize<TObj>(jsonString);
			}

			/*
			 */

			[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static TObj? deserializeStructFromJsonString<TObj> ( this string jsonString ) where TObj : struct
			{
				if ( jsonString.isNullOrWhiteSpace ) return default;
				// Восстанавливает любой объект, включая ваши словари Dictionary<string, bool>
				return JsonSerializer.Deserialize<TObj>(jsonString);
			}



			/*

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



			internal static byte[] eSerializeAsXML ( this System.Data.DataSet dts )
			{
				using MemoryStream ms = new();
				dts.WriteXml(ms , System.Data.XmlWriteMode.IgnoreSchema);
				return ms.readExAllBytes().Value;
			}





			internal static T? deSerializeXML<T> ( this string? xml , T? defaultValue = default , bool throwOnError = false ) where T : class
			{
				try
				{
					if ( xml.isNullOrWhiteSpace ) return defaultValue;

					using StringReader sr = new(xml);
					using System.Xml.XmlTextReader xtr = new(sr);
					System.Xml.Serialization.XmlSerializer xs = new(typeof(T));
					var O = xs.Deserialize(xtr);
					return (T?)O ?? defaultValue;
				}
				catch
				{
					if ( throwOnError ) throw;
					return defaultValue;
				}
			}

			internal static T? deSerializeXML<T> ( this FileInfo? xml , T? defaultValue = default , Encoding? encoding = null , bool throwOnError = false ) where T : class
				=> xml?
				.readAsText(encoding ?? Encoding.Unicode)?
				.deSerializeXML<T>(defaultValue , throwOnError) ?? defaultValue;

			internal static T? deSerializeXML<T> ( this Stream xml , T? defaultValue = default , Encoding? encoding = null , bool throwOnError = false ) where T : class
			{
				try
				{
					StreamReader sr = new(xml , encoding ?? Encoding.Unicode);
					var xs = new XmlSerializer(typeof(T));
					return (T?)xs.Deserialize(sr) ?? defaultValue;
				}
				catch
				{
					if ( throwOnError ) throw;
					return defaultValue;
				}
			}

			internal static T? pathDeserializeXML<T> ( this string path , T? defaultValue = default , Encoding? encoding = null , bool throwOnError = false ) where T : class
			{
				try
				{
					return path.isNullOrWhiteSpace
						? defaultValue
						: path
							.pathToFileInfo()
							.deSerializeXML<T>(defaultValue , encoding , throwOnError);
				}
				catch
				{
					if ( throwOnError ) throw;
					return defaultValue;
				}
			}




			internal static T deSerializeXMLStruct<T> ( this string? xml , T defaultValue = default , bool throwOnError = false ) where T : struct
			{
				try
				{
					if ( xml.isNullOrWhiteSpace ) return defaultValue;

					using StringReader sr = new(xml);
					using System.Xml.XmlTextReader xtr = new(sr);
					System.Xml.Serialization.XmlSerializer xs = new(typeof(T));
					var O = xs.Deserialize(xtr);
					return (T?)O ?? defaultValue;
				}
				catch
				{
					if ( throwOnError ) throw;
					return defaultValue;
				}
			}

			internal static T deSerializeXMLStruct<T> ( this FileInfo? xml , T defaultValue = default , Encoding? encoding = null , bool throwOnError = false ) where T : struct
				=> xml?
				.readAsText(encoding ?? Encoding.Unicode)?
				.deSerializeXMLStruct<T>(defaultValue , throwOnError) ?? defaultValue;

			internal static T deSerializeXMLStruct<T> ( this Stream xml , T defaultValue = default , Encoding? encoding = null , bool throwOnError = false ) where T : struct
			{
				try
				{
					StreamReader sr = new(xml , encoding ?? Encoding.Unicode);
					var xs = new XmlSerializer(typeof(T));
					return (T?)xs.Deserialize(sr) ?? defaultValue;
				}
				catch
				{
					if ( throwOnError ) throw;
					return defaultValue;
				}
			}

			internal static T deSerializeXMLStruct<T> ( this string path , T defaultValue = default , Encoding? encoding = null , bool throwOnError = false ) where T : struct
			{
				try
				{
					return path.isNullOrWhiteSpace
						? defaultValue
						: path
							.pathToFileInfo()
							.deSerializeXMLStruct<T>(defaultValue , encoding , throwOnError);
				}
				catch
				{
					if ( throwOnError ) throw;
					return defaultValue;
				}
			}






















			/*
			internal static T[] deSerializeXMLArrays<T> ( this FileInfo[] fiFiles , bool throwOnError = false )
			{
				if ( fiFiles.Length == 0 )
				{
					return Array.Empty<T>();
				}

				var lTotalDeserializedObjects = new List<T>();
				foreach ( var fiFileToDeserialize in fiFiles )
				{
					var ArrayOfDeserializedObjects = fiFileToDeserialize.deSerializeXML<T[]>(throwOnError: throwOnError);
					if ( ArrayOfDeserializedObjects is { Length: > 0 } )
					{
						lTotalDeserializedObjects.AddRange(ArrayOfDeserializedObjects);
					}
				}

				var aTotalRulesToImport = lTotalDeserializedObjects.ToArray();
				return aTotalRulesToImport;
			}



			internal static List<T> eDeSerializeXMLLists<T> ( this FileInfo[] fiFiles , bool ThrowExceptionOnError = false )
			{
				var lTotalDeserializedObjects = new List<T>();
				if ( fiFiles.Length == 0 )
				{
					return lTotalDeserializedObjects;
				}

				foreach ( var fiFileToDeserialize in fiFiles )
				{
					var ListOfDeserializedObjects = fiFileToDeserialize.deSerializeXML<List<T>>(throwOnError: ThrowExceptionOnError);
					if ( ListOfDeserializedObjects is { Count: > 0 } )
					{
						lTotalDeserializedObjects.AddRange(ListOfDeserializedObjects);
					}
				}
				return lTotalDeserializedObjects;
			}

			 */

			#endregion


			/*

			internal static void eSerializeXMLSettings ( this object SerializableObject , string ParamName )
			{
				throw new NotImplementedException();

				//string sXML = SerializableObject.eSerializeXML();
				//UOM.Settings.mAppSettings.SaveSetting(ParamName, sXML, ThrowExceptionIfError: true);
			}



			internal static T eDeSerializeXMLSettings<T> ( this string ParamName , T defaultValue )
			{
				throw new NotImplementedException();

				//string sXML = UOM.Settings.mAppSettings.GetSetting_String(ParamName, null, ThrowExceptionIfError: false).Value;
				//if (sXML.isNotNullOrWhiteSpace )
				//{
				//    var Obj = sXML.eDeSerializeXML(defaultValue);
				//    return Obj;
				//}
				//else
				//{
				//    return defaultValue;
				//}
			}


			 */

			#endregion


			/// <inheritdoc cref="ICloneable.Clone" />

			public static T clone<T> ( this T src ) where T : ICloneable
				=> (T)src.Clone();


			/// <summary>Клонирует объект черех XML сериализацию в памяти</summary>

			public static T? cloneXML<T> ( this T src ) where T : class
			{
				using MemoryStream ms = new();
				src!.eSerializeXML(ms);
				ms.Seek(0L , SeekOrigin.Begin);
				return ms.deSerializeXML<T>(throwOnError: true);
			}


			#region Binary Serialization disbaled due to unsafe
			/*



			internal static void eSerializeBinary ( this object SerializableObject , string FileName )
			{
				using FileStream SM = FileName.toFileInfo()!.OpenWrite();
				SerializableObject.eSerializeBinary(SM);
				SM.Flush();
			}

			internal static void eSerializeBinary ( this object SerializableObject , Stream SM )
			{
				byte[] abData = SerializableObject.eSerializeBinary();
				SM.Write(abData , 0 , abData.Length);
			}

			internal static byte[] eSerializeBinary ( this object SerializableObject )
			{
				using MemoryStream ms = new();
				new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(ms , SerializableObject);
		#pragma warning disable SYSLIB0011 // Type or member is obsolete
		#pragma warning restore SYSLIB0011 // Type or member is obsolete
				return ms.eReadAllBytes();
			}


			internal static T? eDeSerializeBinary<T> ( this Stream SM , T? defaultValue = default , bool ThrowExceptionOnError = false )
			{
				try
				{
		#pragma warning disable SYSLIB0011 // Type or member is obsolete
					return (T)new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Deserialize(SM);
		#pragma warning restore SYSLIB0011 // Type or member is obsolete					
				}
				catch
				{
					if ( ThrowExceptionOnError )
					{
						throw;
					}

					return defaultValue;
				}
			}



			internal static T? eDeSerializeBinary<T> ( this byte[] SerializedData , T? defaultValue = default , bool ThrowExceptionOnError = false )
			{
				using MemoryStream ms = new(SerializedData);
				return ms.eDeSerializeBinary(defaultValue , ThrowExceptionOnError);
			}



			internal static T? eDeSerializeBinary<T> ( this FileInfo File , T? defaultValue = default , bool ThrowExceptionOnError = false )
			{
				using FileStream fs = File.OpenRead();
				return fs.eDeSerializeBinary(defaultValue , ThrowExceptionOnError);
			}
			 */

			#endregion


		}



		internal static class Extensions_Security_Ecryption_AES
		{

			//private const string PLATFORM_WINDOWS = "Windows";

			public enum AES_KEY_SIZES : int
			{
				KEY_128 = 128,
				KEY_256 = 256
			}

			/// <summary>the iteration count must be greater than zero. The minimum recommended number of iterations is 1000.</summary>
			public const int AES_DEFAULT_ITERATIONS = 1000;
			public const int AES_MIN_SALT_SIZE = 8;
			private const int AES_BLOCK_SIZE = 128;

			private static readonly byte[] DEFAULT_SALT_AES = new byte[] { 1 , 2 , 3 , 4 , 5 , 6 , 7 , 8 };

			/// <summary>Encrypt</summary>
			/// <param name="saltBytes">The salt size must be 8 bytes or larger</param>
			/// <param name="createSaltFromPassword"></param>
			/// <param name="iterations">the iteration count must be greater than zero. The minimum recommended number of iterations is 1000</param>
			/// <exception cref="ArgumentOutOfRangeException"></exception>
			/// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.rfc2898derivebytes.-ctor?view=netframework-4.8#system-security-cryptography-rfc2898derivebytes-ctor(system-byte()-system-byte()-system-int32)"/>
			public static byte[] eEncrypt_AES (
				this byte[] bytesToBeEncrypted ,
				byte[] passwordBytes ,
				byte[]? saltBytes = null ,
				bool createSaltFromPassword = true ,
				AES_KEY_SIZES keySize = AES_KEY_SIZES.KEY_256 ,
				int iterations = AES_DEFAULT_ITERATIONS )
			{

				if ( createSaltFromPassword )
				{
					var lSalt = passwordBytes.ToList();
					while ( lSalt.Count < AES_MIN_SALT_SIZE )
					{
						lSalt.AddRange(passwordBytes);
					}
					saltBytes = [ .. lSalt ];
				}
				else
				{
					if ( saltBytes is { Length: < AES_MIN_SALT_SIZE } ) throw new ArgumentOutOfRangeException(nameof(saltBytes) , "The salt size must be 8 bytes or larger!");

					saltBytes ??= DEFAULT_SALT_AES;
				}

				Rfc2898DeriveBytes key = new(passwordBytes , saltBytes , iterations);
				//Rfc2898DeriveBytes key = Rfc2898DeriveBytes.Pbkdf2(  passwordBytes, saltBytes, iterations ,HashAlgorithmName.SHA3_512 ,5);
				//using System.Security.Cryptography.AesCng aesChiper = new()
				using Aes aesChiper = Aes.Create();
				{
					aesChiper.KeySize = (int)keySize;
					aesChiper.BlockSize = AES_BLOCK_SIZE;
					aesChiper.Key = key.GetBytes((int)keySize / 8);
					aesChiper.IV = key.GetBytes(AES_BLOCK_SIZE / 8);
					aesChiper.Mode = CipherMode.CBC;
				}

				using MemoryStream ms = new();
				using ( CryptoStream cs = new(ms , aesChiper.CreateEncryptor() , CryptoStreamMode.Write) )
				{
					cs.Write(bytesToBeEncrypted , 0 , bytesToBeEncrypted.Length);
					cs.Close();
				}
				return ms.ToArray();
			}

			/// <inheritdoc cref="eEncrypt_AES(byte[], byte[], byte[], bool, AES_KEY_SIZES, int)" />
			public static byte[] eEncrypt_AES (
				this string text ,
				string password ,
				byte[]? saltBytes = null ,
				bool createSaltFromPassword = true ,
				AES_KEY_SIZES keySize = AES_KEY_SIZES.KEY_256 ,
				int iterations = AES_DEFAULT_ITERATIONS )
				=> text
					.eGetBytes_Unicode()
					.eEncrypt_AES(password.eGetBytes_Unicode() , saltBytes , createSaltFromPassword , keySize , iterations);

			/// <inheritdoc cref="eEncrypt_AES(byte[], byte[], byte[], bool, AES_KEY_SIZES, int)" />
			public static string eEncrypt_AES_ToBase64String (
				this string text ,
				string password ,
				byte[]? saltBytes = null ,
				bool createSaltFromPassword = true ,
				AES_KEY_SIZES keySize = AES_KEY_SIZES.KEY_256 ,
				int iterations = AES_DEFAULT_ITERATIONS )
					=> text.eEncrypt_AES(password , saltBytes , createSaltFromPassword , keySize , iterations)
						.eToBase64String();



			/// <summary>Decrypt</summary>
			/// <param name="saltBytes">The salt size must be 8 bytes or larger</param>
			/// <param name="createSaltFromPassword"></param>
			/// <param name="iterations">the iteration count must be greater than zero. The minimum recommended number of iterations is 1000</param>
			/// <exception cref="ArgumentOutOfRangeException"></exception>
			/// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.rfc2898derivebytes.-ctor"/>
			public static byte[] eDecrypt_AES (
				this byte[] bytesToBeDecrypted ,
				byte[] passwordBytes ,
				byte[]? saltBytes = null ,
				bool createSaltFromPassword = true ,
				AES_KEY_SIZES keySize = AES_KEY_SIZES.KEY_256 ,
				int iterations = AES_DEFAULT_ITERATIONS )
			{

				if ( createSaltFromPassword )
				{
					var lSalt = passwordBytes.ToList();
					while ( lSalt.Count < AES_MIN_SALT_SIZE )
					{
						lSalt.AddRange(passwordBytes);
					}
					saltBytes = [ .. lSalt ];
				}
				else
				{
					if ( saltBytes is { Length: < AES_MIN_SALT_SIZE } )
						throw new ArgumentOutOfRangeException(nameof(saltBytes) , "The salt size must be 8 bytes or larger!");

					saltBytes ??= DEFAULT_SALT_AES;
				}

				Rfc2898DeriveBytes key = new(passwordBytes , saltBytes , iterations);
				//using System.Security.Cryptography.AesCng aesChiper = new()
				using Aes aesChiper = Aes.Create();
				{
					aesChiper.KeySize = (int)keySize;
					aesChiper.BlockSize = AES_BLOCK_SIZE;
					aesChiper.Key = key.GetBytes((int)keySize / 8);
					aesChiper.IV = key.GetBytes(AES_BLOCK_SIZE / 8);
					aesChiper.Mode = CipherMode.CBC;
				}

				using MemoryStream ms = new();
				using ( CryptoStream cs = new(ms , aesChiper.CreateDecryptor() , CryptoStreamMode.Write) )
				{
					cs.Write(bytesToBeDecrypted , 0 , bytesToBeDecrypted.Length);
					cs.Close();
				}
				return ms.ToArray();
			}

			public static byte[] eDecrypt_AES_FromBase64String (
				this string base64String ,
				string password ,
				byte[]? saltBytes = null ,
				bool createSaltFromPassword = true ,
				AES_KEY_SIZES keySize = AES_KEY_SIZES.KEY_256 ,
				int iterations = AES_DEFAULT_ITERATIONS )
				=> base64String
					.eFromBase64String()
					.eDecrypt_AES(password.eGetBytes_Unicode() , saltBytes , createSaltFromPassword , keySize , iterations);

		}


		internal static partial class Extensions_Security_SecureString
		{



			public static SecureString eToSecureString ( this string src , bool makeReadOnly = true )
			{
				SecureString sec = new();
				src.ToCharArray().ToList().ForEach(sec.AppendChar);
				if ( makeReadOnly )
				{
					sec.MakeReadOnly();
				}

				return sec;
			}


			/// <summary>
			/// Creates a managed character array from the secure string using methods in System.Runetime.InteropServices
			/// copying data into a BSTR (unmanaged binary string) and then into a managed character array which is returned from this method.
			/// Data in the unmanaged memory temporarily used are freed up before the method returns.
			/// </summary>

			public static char[] eFromSecureStringToCharArray ( this SecureString secureString )
			{
				var ptr = IntPtr.Zero;
				try
				{
					// alloc unmanaged binary string  (BSTR) and copy contents of SecureString into this BSTR
					// The code should use SecureStringToBSTR because SecureString can contain \0 as non-terminating characters,
					// but SecureStringToGlobalAllocUnicode treat it as a null-terminated string.
					ptr = Marshal.SecureStringToBSTR(secureString);
					char[] bytes = new char[ secureString.Length ];

					//copy to managed memory char array from unmanaged memory 
					Marshal.Copy(ptr , bytes , 0 , secureString.Length);
					return bytes;
				}
				finally
				{
					if ( ptr != IntPtr.Zero )
					{
						Marshal.ZeroFreeBSTR(ptr);
					}
				}
			}

			/// <summary>Returns an unsafe string in managed memory from SecureString. </summary>

			public static string eFromSecureStringToUnsafeString ( this SecureString secureString )
				=> new(secureString!.eFromSecureStringToCharArray());

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
				SHA512,
			}



			/// <inheritdoc cref="HashAlgorithm.Create(string)"/>

			private static HashAlgorithm? CreateHashAlgorithm ( HashNames hashName )
			{
#if NET
				return hashName switch
				{
					HashNames.SHA1 => System.Security.Cryptography.SHA1.Create(),
					HashNames.SHA256 => System.Security.Cryptography.SHA256.Create(),
					HashNames.SHA384 => System.Security.Cryptography.SHA384.Create(),
					HashNames.SHA512 => System.Security.Cryptography.SHA512.Create(),
					HashNames.MD5 => System.Security.Cryptography.MD5.Create(),
					_ => throw new ArgumentOutOfRangeException(nameof(hashName))
				};
#else
				//Only for Net.Framework
				return HashAlgorithm.Create(hashName.ToString());
#endif
			}



#if NET


			public static int GetHashSize ( this HashNames hashName )
				=> hashName switch
				{
					HashNames.SHA1 => 20,//The SHA-1 algorithm always produces a 160-bit hash, or 20 bytes.
					HashNames.SHA256 => 32,//The SHA-256 algorithm always produces a 256-bit hash, or 32 bytes.
					HashNames.SHA384 => 48,//The SHA-3-384 algorithm always produces a 384-bit hash, or 48 bytes.
					HashNames.SHA512 => 64,//The SHA-512 algorithm always produces a 512-bit hash, or 64 bytes.
					HashNames.MD5 => 16, //The MD5 algorithm always produces a 128-bit hash, or 16 bytes.
					_ => throw new ArgumentOutOfRangeException(nameof(hashName))
				};



			/*
		/// <inheritdoc cref="HashAlgorithm.ComputeHash(byte[])"/>

		public static ArrayPoolBuffer eComputeHashFast ( this ReadOnlySpan<byte> src, HashNames hashName )
		{
		//int hashSize = hashName.GetHashSize ();
		var buf = Mem.ArrayPoolHelper.Rent<byte> (hashName.GetHashSize ());
		_ = hashName switch
		{
		HashNames.SHA1 => System.Security.Cryptography.SHA1.HashData (src, buf),
		HashNames.SHA256 => System.Security.Cryptography.SHA256.HashData (src, buf.Buffer),
		HashNames.SHA384 => System.Security.Cryptography.SHA384.HashData (src, buf.Buffer),
		HashNames.SHA512 => System.Security.Cryptography.SHA512.HashData (src, buf.Buffer),
		HashNames.MD5 => System.Security.Cryptography.MD5.HashData (src, buf.Buffer),
		_ => throw new ArgumentOutOfRangeException (nameof (hashName))
		};

		return buf;
		}

		/// <inheritdoc cref="HashAlgorithm.ComputeHash(byte[])"/>

		public static ArrayPoolBuffer eComputeHashFast ( this Span<byte> src, HashNames hashName )
		=> ( (ReadOnlySpan<byte>) src ).eComputeHashFast (hashName);
			 */

#endif



			/// <inheritdoc cref="HashAlgorithm.ComputeHash(byte[])"/>

			public static byte[] eComputeHash ( this byte[] srcData , HashNames ha )
			{
				using HashAlgorithm? H = CreateHashAlgorithm(ha);
				return H!.ComputeHash(srcData);
			}


			/// <inheritdoc cref="eComputeHash"/>

			public static byte[] eComputeHashUni ( this string srcData , HashNames ha )
				=> srcData.eGetBytes_Unicode().eComputeHash(ha);



#if NET   //NET.Core           

			/// <summary>Computes 16 bytes hash and storess it into 16 bytes GUID</summary>
			public static unsafe Guid eComputeHashMD5 ( this ReadOnlySpan<byte> source )
			{
				Guid fileHash = new();
				byte* hashBufferPtr = (byte*)&fileHash;
				{
					var hashBuffer = new Span<byte>(hashBufferPtr , 16);
					int bytesWritten = System.Security.Cryptography.MD5.HashData(source , hashBuffer);//16 bytes hash
					if ( bytesWritten != 16 ) throw new Exception($"ComputeHashMD5 Failed!");
				}
				return fileHash;
			}


#else
			//Only for Net.Framework

			/// <summary>https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.keyedhashalgorithm.create</summary>
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
			
			private static KeyedHashAlgorithm? CreateKeyedHashAlgorithm (KeyedHashNames kha)
				=> KeyedHashAlgorithm.Create(kha.ToString());



			/// <inheritdoc cref="CreateKeyedHashAlgorithm"/>
			
			public static byte[] eComputeKeyedHash (this byte[] srcData, KeyedHashNames kha)
			{
				using KeyedHashAlgorithm? H = CreateKeyedHashAlgorithm(kha);
				return H!.ComputeHash(srcData);
			}


			/// <inheritdoc cref="eComputeKeyedHash"/>
			
			public static byte[] eComputeKeyedHashUni (this string srcData, KeyedHashNames kha)
				=> srcData.eGetBytes_Unicode().eComputeKeyedHash(kha);



#endif





		}


		internal static partial class Extensions_Security_Random
		{

			/// <summary>Use a fast System.Random class, using a time-dependent default seed value.</summary>

			public static byte[] eGetRandomBytesOld ( this int count )
			{
				var bytes = new byte[ count ];
				if ( count > 0 )
				{
					var R = new Random();
					R.NextBytes(bytes);
				}
				return bytes;
			}
#if NET
			/// <summary>This is modern method in .Net Core
			/// Uses RandomNumberGenerator.GetBytes</summary>

			public static byte[] eGetRandomBytes ( this int count ) => RandomNumberGenerator.GetBytes(count);
#else
			/// <summary>Uses RandomNumberGenerator.GetBytes</summary>
			
			public static byte[] eGetRandomBytes (this int count)
			{
				if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

				using RandomNumberGenerator rng = RandomNumberGenerator.Create();
				byte[] bytes = new byte[ count ];
				rng.GetBytes(bytes);
				return bytes;
			}
#endif

		}




		// Helper to perform atomic reads/writes for nullable value-types (e.g. bool?).
		// Volatile.Read<T> requires a reference type T, so we reinterpret the nullable<T>
		// storage as an object reference and use Volatile on that reference.
		// This avoids CS0452 (T must be a reference type) when you need atomic access
		// semantics for nullable value types.
		internal static class Extensions_VolatileEx
		{

			extension( System.Threading.Volatile )
			{


				public static T? ReadNullable<T> ( ref T? location ) where T : struct
				{
					// Reinterpret the ref T? as ref object? and perform Volatile.Read on the reference.
					// The value is boxed when stored in object; if null, result is null.
					object? boxed = System.Threading.Volatile.Read(ref System.Runtime.CompilerServices.Unsafe.As<T? , object?>(ref location));
					return boxed is null ? null : (T)boxed;
				}

				public static void WriteNullable<T> ( ref T? location , T? value ) where T : struct
				{
					// Write the boxed value (or null) atomically.
					object? boxed = value.HasValue ? value.Value : null;
					System.Threading.Volatile.Write(ref System.Runtime.CompilerServices.Unsafe.As<T? , object?>(ref location) , boxed);
				}
			}
		}

		internal static partial class Extensions_Lazy
		{


			[MethodImpl(MethodImplOptions.NoOptimization)]
			public static T ForceCreate<T> ( this Lazy<T> lz )
				=> lz.Value;


			extension( LazyInitializer )
			{


				public static bool EnsureInitializedBool ( ref bool? target , Func<bool> valueFactory )
				{
					var current = Volatile.ReadNullable(ref target);

					if ( current.HasValue ) return current.Value;

					bool value = valueFactory();

					Interlocked.CompareExchange(ref target , value , null);

					return target.Value;
				}
			}

		}


		internal static partial class Extensions_XML
		{


			public static XmlNode[] toArray ( this XmlNodeList xnl ) => [ .. xnl.Cast<XmlNode>() ];

			public static string eAsString ( this XmlDocument xmlDoc )
			{
				using ( StringWriter sw = new() )
				{
					using ( XmlTextWriter tx = new(sw) )
					{
						xmlDoc.WriteTo(tx);
					}
					return sw.ToString();
				}
			}

			public static System.Xml.Linq.XDocument eToXDocument ( this XmlDocument xd )
			{
				System.Xml.Linq.XDocument doc = System.Xml.Linq.XDocument.Parse(xd.eAsString());
				return doc;
			}


			private static IEnumerable<System.Xml.Linq.XElement> eGetElementsOrDescendants ( this XContainer node , SearchOption so = SearchOption.TopDirectoryOnly )
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

				return (so == SearchOption.TopDirectoryOnly)
					? node.Elements()
					: node.Descendants();
			}

			public delegate bool XElementToNameCompareDelegate ( XElement node , string name );

			private static readonly Lazy<XElementToNameCompareDelegate> defaultXElementToNameComparer = new(() => new XElementToNameCompareDelegate(( x , s ) => x.Name.LocalName == s));


			public static System.Xml.Linq.XElement[] eFindNodes (
				this XContainer node ,
				string name ,
				SearchOption so = SearchOption.TopDirectoryOnly ,
				XElementToNameCompareDelegate? comparePredicate = null )
			{
				//https://stackoverflow.com/questions/8460464/finding-element-in-xdocument
				//Имейте в виду, что свойство Name возвращает объект, который имеет LocalName и Namespace. Вот почему вы должны использовать Name.LocalName, если хотите сравнить по имени.
				//Мой опыт работы с большими и сложными файлами XML заключается в том, что иногда ни элементы, ни потомки не работают при извлечении определенного элемента (и я до сих пор не знаю, почему).

				var elements = node.eGetElementsOrDescendants(so);
				return elements
					.Where(x => (comparePredicate ?? defaultXElementToNameComparer.Value)!.Invoke(x , name))
					.ToArray();
			}

			public static System.Xml.Linq.XElement? eFindSingleOrDefaultNode (
				this XContainer node ,
				string name , SearchOption so = SearchOption.TopDirectoryOnly ,
				XElementToNameCompareDelegate? comparePredicate = null )
				=> node
				.eGetElementsOrDescendants(so)
				.SingleOrDefault(x => (comparePredicate ?? defaultXElementToNameComparer.Value)!.Invoke(x , name));


			/// <summary>
			/// Gets Last node in each tree path which corresponds to tree like 'nodeTreeNames1\nodeTreeNames2\nodeTreeNamesXXX'
			/// SAMPLE: var xProps = Manifest.eFindTree(null, "Package", "Properties").FirstOrDefault();
			/// </summary>
			/// <param name="comparePredicate">Custom node to name comparer, or NULL, to use default comparer</param>			
			/// <returns> Last node in each tree path which corresponds to tree path 'nodeTreeNames1\nodeTreeNames2\nodeTreeNamesXXX'</returns>
			public static System.Xml.Linq.XElement[] eFindTree (
				this XContainer node ,
				SearchOption startPointSearchOptions ,
				XElementToNameCompareDelegate? comparePredicate = null ,
				params string[] nodeTreeNames )
			{
				if ( nodeTreeNames.Length < 2 )
				{
					throw new ArgumentOutOfRangeException(nameof(nodeTreeNames) , $"{nameof(nodeTreeNames)} count must be > 1 !");
				}

				comparePredicate ??= defaultXElementToNameComparer.Value;

				var treesBranches = node.eFindNodes(nodeTreeNames[ 0 ] , startPointSearchOptions);
				List<XElement> lFoundLastNodes = [];


				void FindNextChildForNode ( XContainer x , string[] childrensToFind )
				{
					string childToFind = childrensToFind[ 0 ];
					XElement[] foundChildrens = x
						.Elements()
						.Where(p => comparePredicate!.Invoke(p , childToFind))
						.ToArray();

					if ( childrensToFind.Length == 1 )
					{
						//We Found last needed Child in this branch path!
						foundChildrens.ToList().ForEach(x => lFoundLastNodes.Add(x));
					}
					else
					{
						//We have to found next children
						childrensToFind = childrensToFind[ 1.. ];
						foundChildrens
							.ToList()
							.ForEach(x => FindNextChildForNode(x , childrensToFind));
					}
				}

				nodeTreeNames = nodeTreeNames[ 1.. ];
				treesBranches
					.ToList()
					.ForEach(x => FindNextChildForNode(x , nodeTreeNames));

				XElement[] foundLastNodes = lFoundLastNodes.ToArray();

				return foundLastNodes;
				//return Array.Empty<System.Xml.Linq.XElement>();
			}


			public static System.Xml.Linq.XElement[] eFindTree ( this XContainer node , params string[] nodeTreeNames )
				=> node.eFindTree(SearchOption.AllDirectories , null , nodeTreeNames);


		}

		internal static partial class Extensions_System_Data
		{



			public static string[] cellsToStrings ( this System.Data.DataRow dr )
			{
				string[] cells = [ .. dr.ItemArray!.Select(d => d!.ToString()!) ];
				return cells;
			}



		}

#if !ANDROID





		internal static partial class Extensions_Console
		{

			internal static EventArgs _consoleLock = new();


			/*
			internal static string CreateArgsText ( IEnumerable<(string Key, string Value)> ArgsAndDescriptions , int MaxWidth = constants.C_DEFAULT_CONSOLE_WIDTH )
			{
				if ( null == ArgsAndDescriptions || !ArgsAndDescriptions.Any() )
				{
					throw new ArgumentException(nameof(ArgsAndDescriptions));
				}

				var sb = new StringBuilder();
				int iMaxKeyLenght = (from T in ArgsAndDescriptions select T.Key.Length).Max();

				iMaxKeyLenght += 1;
				var aTabbed = ArgsAndDescriptions
					.Select(T => new { Key = T.Key.PadRight(iMaxKeyLenght) + "-" , Value = T.Value });

				aTabbed.forEach(T =>
				{
					sb.Append(T.Key);
					string sDescr = T.Value;
					int iMaxDescrLenght = MaxWidth - iMaxKeyLenght - 2;
					var aLines = sDescr.splitStringToFixedWidth(iMaxDescrLenght);
					if ( aLines.Length != 0 )
					{
						sb.AppendLine(aLines.First());
						if ( aLines.Length > 1 )
						{
							var aRows = aLines.Except(aLines.Take(1));
							aRows.forEach(S =>
							{
								string sRow = new string(' ' , iMaxKeyLenght + 1) + S;
								sb.AppendLine(sRow);
							});
						}
					}
				});
				return sb.ToString();
			}

			 */




			extension( System.Console )
			{


				/// <summary>Возвращает строку вида 'HEADER---------'</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal static string createHeaderLine ( string text , int with = constants.ConsoleConst.CONSOLE_WIDTH_DEFAULT_1 )
					=> text.PadRight(with , '-');


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal static string createHSplitter ( char splitterChar = '=' , int width = constants.ConsoleConst.CONSOLE_WIDTH_DEFAULT_1 )
					=> new(splitterChar , width);

				/// <summary>Возвращает строку вида 'HEADER---------'</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				internal static string createHeader ( string text , int with = constants.ConsoleConst.CONSOLE_WIDTH_DEFAULT_1 )
					=> createHSplitter(width: with) + $"\n{text}\n" + createHSplitter(width: with);

				//public static string ConsoleSeparator                    => '-'.repeat();


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void writeLine (
					string? text = null ,
					ConsoleColor? clrFore = null ,
					ConsoleColor? clrBack = null ,
					bool newLine = true ,
					StreamWriter? logWriter = null )
				{
					if ( text is null )
					{
						Console.WriteLine();
						return;
					}

					Action<string> consoleWriteFunc = newLine
					 ? Console.WriteLine
					 : Console.Write;

					Action<string>? logWriteFunc = (logWriter is null)
						? null
						: newLine
							? logWriter!.WriteLine
							: logWriter!.Write;

					lock ( _consoleLock )
					{
						var clrCurrrenFore = Console.ForegroundColor;
						var clrCurrrentBk = Console.BackgroundColor;
						try
						{
							if ( clrFore.HasValue ) Console.ForegroundColor = clrFore.Value;
							if ( clrBack.HasValue ) Console.BackgroundColor = clrBack.Value;

							consoleWriteFunc.Invoke(text);
							logWriteFunc?.Invoke(text);
						}
						finally
						{
							//Restoring Old Colors
							if ( clrBack.HasValue ) Console.BackgroundColor = clrCurrrentBk;
							if ( clrFore.HasValue ) Console.ForegroundColor = clrCurrrenFore;
							//Console.ResetColor();
						}
					}

				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void writeLineEx (
				  string? text ,
				  System.Drawing.Color? foreColor = null ,
				  System.Drawing.Color? backColor = null ,

				  bool bold = false ,
				  bool italic = false ,

				  constants.ConsoleConst.ANSIEscape.ConsoleUnderlineModes underline = constants.ConsoleConst.ANSIEscape.ConsoleUnderlineModes.None ,
				  System.Drawing.Color? underlineColor = null ,

				  bool invert = false ,

				  constants.ConsoleConst.ANSIEscape.ConsoleBlinkModes blink = constants.ConsoleConst.ANSIEscape.ConsoleBlinkModes.None ,

				  int alternativeFontIndex = -1 ,

				  bool resetToDefaultAtEnd = true ,
				  StreamWriter? logWriter = null ,
				  bool newLine = true )
				{
					if ( text is null ) return;

					var outString = (text ?? string.Empty).consoleColorize(
						foreColor: foreColor ,
						backColor: backColor ,
						bold: bold ,
						italic: italic ,
						underline: underline ,
						underlineColor: underlineColor ,
						invert: invert ,
						blink: blink ,
						alternativeFontIndex: alternativeFontIndex ,
						resetToDefaultAtEnd: resetToDefaultAtEnd
						);

					Action<string> consoleWriteFunc = newLine
						   ? Console.WriteLine
						   : Console.Write;

					Action<string>? logWriteFunc = (logWriter is null)
							? null
							: newLine
								? logWriter!.WriteLine
								: logWriter!.Write;

					lock ( _consoleLock )
					{
						consoleWriteFunc.Invoke(outString);
						logWriteFunc?.Invoke(outString);
					}

				}



				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void writeEx (
					string? text ,
					System.Drawing.Color? foreColor = null ,
					System.Drawing.Color? backColor = null ,
					bool bold = false ,
					bool italic = false ,
					constants.ConsoleConst.ANSIEscape.ConsoleUnderlineModes underline = constants.ConsoleConst.ANSIEscape.ConsoleUnderlineModes.None ,
					System.Drawing.Color? underlineColor = null ,
					bool invert = false ,
					constants.ConsoleConst.ANSIEscape.ConsoleBlinkModes blink = constants.ConsoleConst.ANSIEscape.ConsoleBlinkModes.None ,
					bool blinkSlow = false ,
					bool blinkRapid = false ,
					int alternativeFontIndex = -1 ,
					bool resetToDefaultAtEnd = true ,
					StreamWriter? logWriter = null
					)
					=> writeLineEx(
						text: text ,
						foreColor: foreColor ,
						backColor: backColor ,
						newLine: false ,
						bold: bold ,
						italic: italic ,
						underline: underline ,
						underlineColor: underlineColor ,
						invert: invert ,
						blink: blink ,
						alternativeFontIndex: alternativeFontIndex ,
						resetToDefaultAtEnd: resetToDefaultAtEnd ,
						logWriter: logWriter);





				/// <summary>Display progress bar</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void writeProgress (
					float progress ,
					string prefixText = constants.ConsoleConst.DOWNLOADING ,
					int barSize = constants.ConsoleConst.PROGRESS_BAR_SIZE_DEFAULT ,
					int decimalPlaces = constants.FormatConst.DEFAULT_DECIMAL_DIGITS ,
					char barFillChar = constants.ConsoleConst.PROGRESSBAR_FULL ,
					char barEmptyChar = constants.ConsoleConst.PROGRESSBAR_EMPTY
					)
				{
					progress = progress.clamp(0 , 1);

					var barFill = ((int)(barSize * progress)).clamp(0 , barSize);
					var barString = new string(barFillChar , barFill);
					if ( barFill < barSize ) barString = barString.PadRight(barSize , barEmptyChar);

					lock ( _consoleLock )
					{
						string s = $"{prefixText} [{barString}] {progress.formatPercent(decimalPlaces)}\r";
						Console.writeLine(s , newLine: false);
					}
				}


				/// <summary>Display progress bar</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void writeProgress (
					int progress ,
					string prefixText = constants.ConsoleConst.DOWNLOADING ,
					int barSize = constants.ConsoleConst.PROGRESS_BAR_SIZE_DEFAULT ,
					char barFillChar = constants.ConsoleConst.PROGRESSBAR_FULL ,
					char barEmptyChar = constants.ConsoleConst.PROGRESSBAR_EMPTY
					)
					=> writeProgress(
						(float)(progress / 100f) ,
						prefixText ,
						barSize ,
						0 ,
						barFillChar ,
						barEmptyChar
						);


				/// <summary>Display progress bar</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void writeProgress (
					uom.network.Helpers.DownloadProgressEventArgs progress ,
					string prefixText = constants.ConsoleConst.DOWNLOADING ,
					int barSize = constants.ConsoleConst.PROGRESS_BAR_SIZE_DEFAULT ,
					int decimalPlaces = constants.FormatConst.DEFAULT_DECIMAL_DIGITS ,
					char barFillChar = constants.ConsoleConst.PROGRESSBAR_FULL ,
					char barEmptyChar = constants.ConsoleConst.PROGRESSBAR_EMPTY
					)
				{

					if ( !progress.ProgressPercent.HasValue )
					{
						writeProgress(0.0f , prefixText , barSize , decimalPlaces , barFillChar , barEmptyChar);
						return;
					}

					var progressFloat = progress.ProgressPercent.Value.clamp(0 , 1);

					var barFill = (barSize * progressFloat).clamp(0 , barSize);
					var barString = new string(barFillChar , (int)barFill);
					if ( barFill < barSize ) barString = barString.PadRight(barSize , barEmptyChar);

					lock ( _consoleLock )
					{
						string percentString = $"{progress.CompletedBytes.Value.formatByteSizeMP()}/{progress.TotalBytes.Value.formatByteSizeMP()} ({progressFloat.formatPercent(decimalPlaces).Trim()})   ";
						string s = $"{prefixText} [{barString}] {percentString}\r";
						Console.writeLine(s , newLine: false);
					}

				}



				/// <summary>Read a password from the console into a SecureString</summary>
				/// <returns>Password stored in a secure string</returns>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static SecureString readPassword ( string question = constants.ConsoleConst.ENTER_PWD , char PwdChar = '*' )
				{
					lock ( _consoleLock )
					{
						SecureString password = new();
						Console.WriteLine(question);

						// get the first character of the password
						var nextKey = Console.ReadKey(true);
						while ( nextKey.Key != ConsoleKey.Enter )
						{
							if ( nextKey.Key == ConsoleKey.Backspace )
							{
								if ( password.Length > 0 )
								{
									password.RemoveAt(password.Length - 1);

									// erase the last * as well
									Console.Write(nextKey.KeyChar);
									Console.Write(" ");
									Console.Write(nextKey.KeyChar);
								}
							}
							else
							{
								password.AppendChar(nextKey.KeyChar);
								Console.Write(PwdChar);
							}

							nextKey = Console.ReadKey(true);
						}

						Console.WriteLine();

						// lock the password down
						password.MakeReadOnly();
						return password;
					}
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static bool askInputIsYes ( string sMessage , string Suffix = " (y/n)?: " , string YesAnswer = "y" )
				{
					lock ( _consoleLock )
					{
						Console.Write(sMessage + Suffix);
						var sInput = Console.ReadLine();
						return sInput.isNotNullOrWhiteSpace && (sInput!.ToLower() == YesAnswer.ToLower());
					}
				}

				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static bool askKeyIsYes ( string sMessage )
				{
					lock ( _consoleLock )
					{
						sMessage += " (y/n)?: ";
						Console.Write(sMessage);
						var kKey = Console.ReadKey();
						Console.WriteLine("");
						if ( kKey.Key == ConsoleKey.Y )
						{
							return true;
						}
					}

					return false;
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static bool isKeyPressed ( ConsoleKey KK = ConsoleKey.Y )
				{
					lock ( _consoleLock )
					{
						if ( Console.KeyAvailable )
						{
							if ( Console.ReadKey
								().Key == KK )
							{
								return true;
							}
						}
					}

					return false;
				}


				/// <summary>Download File From Remote Using 'WebClient' and displaying progress bar</summary>
				/// <param name="Target">Local File Path To Save File</param>        
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static async Task<System.IO.FileInfo?> downloadFile (
					string remoteFileUrl ,
					System.IO.FileInfo? Target = null ,
					int bufferSize = uom.network.Helpers.DOWNLOAD_DEFAULT_BUFFER_SIZE ,
					string prefixText = constants.ConsoleConst.DOWNLOADING ,
					int barSize = constants.ConsoleConst.PROGRESS_BAR_SIZE_DEFAULT ,
					char barFillChar = constants.ConsoleConst.PROGRESSBAR_FULL ,
					char barEmptyChar = constants.ConsoleConst.PROGRESSBAR_EMPTY
					)
				{

					try
					{
						Console.CursorVisible = false;
					}
					catch { }//Just Ignore Error bc not all platforms support Show/Hide cursor.
					try
					{

						void onProgress ( uom.network.Helpers.DownloadProgressEventArgs pp )
						{
							Console.writeProgress(
									pp ,
									prefixText ,
									barSize ,
									barFillChar: barFillChar ,
									barEmptyChar: barEmptyChar
									);
						}

						var result = await remoteFileUrl.urlHttpClient_DownloadFileAsync(Target , bufferSize , onProgress);

						return result;
					}
					finally
					{
						Console.WriteLine();
						try
						{
							Console.CursorVisible = true;
						}
						catch { }//Just Ignore Error bc not all platforms support Show/Hide cursor.
					}
				}


				/// <summary>Run Action in Try/catch block And outputs Error to console if occurs</summary>
				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static bool tryCatch ( Action a , string header = "" )
				{
					if ( !header.isNullOrWhiteSpace ) Console.writeLine(header);

					try
					{
						a.Invoke();
						return true;
					}
					catch ( Exception ex )
					{
						Console.writeLine($"ERROR: {ex.Message}".Trim() , ConsoleColor.Yellow , ConsoleColor.DarkRed , false);
						Console.WriteLine();
						return false;
					}
				}

			}


			extension( string ANSIEscapedFormatString )
			{


				internal string consoleFormatANSIEscaped ( System.Drawing.Color color )
					=> ANSIEscapedFormatString.format(color.R , color.G , color.B);


				internal string consoleFormatANSIEscaped ( int value )
					=> ANSIEscapedFormatString.format(value);


			}


			extension( string text )
			{

				public string consoleColorize ( string subStringToColorize ,
					uom.constants.ConsoleConst.ANSIEscape.ConsoleColorsANSI? foreColor = null ,
					uom.constants.ConsoleConst.ANSIEscape.ConsoleColorsANSI? backColor = null ,
					bool bold = false ,
					bool italic = false ,
					constants.ConsoleConst.ANSIEscape.ConsoleUnderlineModes underline = constants.ConsoleConst.ANSIEscape.ConsoleUnderlineModes.None ,
					System.Drawing.Color? underlineColor = null ,
					bool invert = false ,
					constants.ConsoleConst.ANSIEscape.ConsoleBlinkModes blink = constants.ConsoleConst.ANSIEscape.ConsoleBlinkModes.None ,
					int alternativeFontIndex = -1 ,
					StringComparison comparison = StringComparison.Ordinal
					)
				{

					if ( subStringToColorize.isNullOrEmpty ) return text;


					var source = text.AsSpan();
					var startIndex = source.indexOfFisrt(subStringToColorize , comparison);
					if ( startIndex < 0 ) return text;



					var leftPart = source[ ..startIndex ];
					var rightPart = source[ (startIndex + subStringToColorize.Length).. ];


					if ( leftPart.LastIndexOf(uom.constants.ConsoleConst.ANSIEscape.ESCAPE) >= 0 )
					{
						// Left part already contains ssss


					}


					var midString = subStringToColorize.consoleColorize(
					  foreColor: foreColor ,
					  backColor: backColor ,
					  bold: bold ,
					  italic: italic ,
					  underline: underline ,
					  underlineColor: underlineColor ,
					  invert: invert ,
					  blink: blink ,
					  alternativeFontIndex: alternativeFontIndex ,
					  resetToDefaultAtEnd: true);

					var outString = $"{leftPart}{midString}{rightPart}";
					return outString;
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string consoleColorize (
					uom.constants.ConsoleConst.ANSIEscape.ConsoleColorsANSI? foreColor = null ,
					uom.constants.ConsoleConst.ANSIEscape.ConsoleColorsANSI? backColor = null ,

				 bool bold = false ,
				 bool italic = false ,

				 constants.ConsoleConst.ANSIEscape.ConsoleUnderlineModes underline = constants.ConsoleConst.ANSIEscape.ConsoleUnderlineModes.None ,
				 System.Drawing.Color? underlineColor = null ,

				 bool invert = false ,

				 constants.ConsoleConst.ANSIEscape.ConsoleBlinkModes blink = constants.ConsoleConst.ANSIEscape.ConsoleBlinkModes.None ,

				 int alternativeFontIndex = -1 ,

				 bool resetToDefaultAtEnd = true )
				{

					StringBuilder sb = new(text.Length * 2);

					if ( bold ) sb.Append(constants.ConsoleConst.ANSIEscape.BOLD_START);
					if ( italic ) sb.Append(constants.ConsoleConst.ANSIEscape.ITALIC_START);


					switch ( underline )
					{
						case constants.ConsoleConst.ANSIEscape.ConsoleUnderlineModes.Single:
							sb.Append(constants.ConsoleConst.ANSIEscape.UNDERLINE_START); break;

						case constants.ConsoleConst.ANSIEscape.ConsoleUnderlineModes.Double:
							sb.Append(constants.ConsoleConst.ANSIEscape.UNDERLINE_DOUBLY_START); break;

					}

					switch ( blink )
					{
						case constants.ConsoleConst.ANSIEscape.ConsoleBlinkModes.Slow:
							sb.Append(constants.ConsoleConst.ANSIEscape.BLINK_SLOW_START); break;

						case constants.ConsoleConst.ANSIEscape.ConsoleBlinkModes.Rapid:
							sb.Append(constants.ConsoleConst.ANSIEscape.BLINK_RAPID_START); break;
					}

					if ( alternativeFontIndex > 0 )
					{
						alternativeFontIndex = alternativeFontIndex.clamp(1 , 9);
						string altFont = constants.ConsoleConst.ANSIEscape.FONT_ALTERNATIVE_FORMAT.format(alternativeFontIndex + 10);
						sb.Append(altFont);
					}

					if ( foreColor.HasValue )
						sb.Append(constants.ConsoleConst.ANSIEscape.COLOR_FOREGROUND_FORMAT_START.consoleFormatANSIEscaped((int)foreColor.Value));

					if ( backColor.HasValue )
						sb.Append(constants.ConsoleConst.ANSIEscape.COLOR_BACKGROUND_FORMAT_START.consoleFormatANSIEscaped(10 + ((int)backColor.Value)));

					if ( underlineColor.HasValue )
						sb.Append(constants.ConsoleConst.ANSIEscape.COLOR_UNDERLINE_RGB_FORMAT_START.consoleFormatANSIEscaped(underlineColor.Value));

					if ( invert ) sb.Append(constants.ConsoleConst.ANSIEscape.INVERT_START);

					sb.Append(text);
					if ( resetToDefaultAtEnd ) sb.Append(constants.ConsoleConst.ANSIEscape.DEFAULT);

					var outString = sb.ToString();

					return outString;
				}


				[DebuggerNonUserCode, DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
				public string consoleColorize (
					System.Drawing.Color? foreColor = null ,
					System.Drawing.Color? backColor = null ,

					bool bold = false ,
					bool italic = false ,

			   constants.ConsoleConst.ANSIEscape.ConsoleUnderlineModes underline = constants.ConsoleConst.ANSIEscape.ConsoleUnderlineModes.None ,
			   System.Drawing.Color? underlineColor = null ,

			   bool invert = false ,

			   constants.ConsoleConst.ANSIEscape.ConsoleBlinkModes blink = constants.ConsoleConst.ANSIEscape.ConsoleBlinkModes.None ,

			   int alternativeFontIndex = -1 ,

			   bool resetToDefaultAtEnd = true )
				{

					StringBuilder sb = new(text.Length * 2);

					if ( bold ) sb.Append(constants.ConsoleConst.ANSIEscape.BOLD_START);
					if ( italic ) sb.Append(constants.ConsoleConst.ANSIEscape.ITALIC_START);


					switch ( underline )
					{
						case constants.ConsoleConst.ANSIEscape.ConsoleUnderlineModes.Single:
							sb.Append(constants.ConsoleConst.ANSIEscape.UNDERLINE_START); break;

						case constants.ConsoleConst.ANSIEscape.ConsoleUnderlineModes.Double:
							sb.Append(constants.ConsoleConst.ANSIEscape.UNDERLINE_DOUBLY_START); break;

					}

					switch ( blink )
					{
						case constants.ConsoleConst.ANSIEscape.ConsoleBlinkModes.Slow:
							sb.Append(constants.ConsoleConst.ANSIEscape.BLINK_SLOW_START); break;

						case constants.ConsoleConst.ANSIEscape.ConsoleBlinkModes.Rapid:
							sb.Append(constants.ConsoleConst.ANSIEscape.BLINK_RAPID_START); break;
					}

					if ( alternativeFontIndex > 0 )
					{
						alternativeFontIndex = alternativeFontIndex.clamp(1 , 9);
						string altFont = constants.ConsoleConst.ANSIEscape.FONT_ALTERNATIVE_FORMAT.format(alternativeFontIndex + 10);
						sb.Append(altFont);
					}

					if ( foreColor.HasValue )
						sb.Append(constants.ConsoleConst.ANSIEscape.COLOR_FOREGROUND_RGB_FORMAT_START.consoleFormatANSIEscaped(foreColor.Value));

					if ( backColor.HasValue )
						sb.Append(constants.ConsoleConst.ANSIEscape.COLOR_BACKGROUND_RGB_FORMAT_START.consoleFormatANSIEscaped(backColor.Value));

					if ( underlineColor.HasValue )
						sb.Append(constants.ConsoleConst.ANSIEscape.COLOR_UNDERLINE_RGB_FORMAT_START.consoleFormatANSIEscaped(underlineColor.Value));

					if ( invert ) sb.Append(constants.ConsoleConst.ANSIEscape.INVERT_START);

					sb.Append(text);
					if ( resetToDefaultAtEnd ) sb.Append(constants.ConsoleConst.ANSIEscape.DEFAULT);
					var outString = sb.ToString();

					return outString;
				}



			}


		}






#endif

	}
}







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
		public CallerArgumentExpressionAttribute (string parameterName) => this.ParameterName = parameterName;

		/// <summary>Получает имя целевого параметра <c>CallerArgumentExpression</c>.</summary>
		/// <returns>Имя целевого параметра <c>CallerArgumentExpression</c>.</returns>
		public string ParameterName { get; }
	}
}
#endif




#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
