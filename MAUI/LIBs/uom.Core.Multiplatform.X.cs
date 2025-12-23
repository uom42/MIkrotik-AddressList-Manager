global using uom.Extensions;

using System.Runtime.Serialization;
using System.Xml.Serialization;

#if UWP
using Windows.Data.Xml.Dom;

using Application = Microsoft.UI.Xaml.Application;
#endif

/*
global using static uom.constants;
global using static uom.Extensions.Extensions_DebugAndErrors;

using System.Collections.Concurrent;
using System.Runtime.Versioning;

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

using static Microsoft.Maui.ApplicationModel.Permissions;


 		   

public class BooleanToReverseConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
     => !(bool?) value ?? true;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
     => !(value as bool?);
}
Xaml

IsEnabled="{Binding IsSuccess Converter={StaticResource BooleanToReverseConverter}}"
App.Xaml I like to put all my converter statics in the app.xaml file so I don't have to redeclare them throughout the windows/pages/controls of the project.

<Application.Resources>
    <converters:BooleanToReverseConverter x:Key="BooleanToReverseConverter"/>
    <local:FauxVM x:Key="VM" />
</Application.Resources>

 
 */




namespace uom;


[Serializable]
public partial class InvertableBool :
#if UWP
	ObservableObject,
#endif
	IXmlSerializable
{

	public InvertableBool () : base () { }

	public InvertableBool ( bool value ) : this () { _value = value; }


	[XmlIgnore]
#if UWP
	[ObservableProperty]
	[property: XmlIgnore]
	[NotifyPropertyChangedFor (nameof (Invert))]
#endif
	private bool _value = false;
#if ( !UWP )
	[XmlIgnore]
	public bool Value { get => _value; }
#endif


	[XmlIgnore]
	public bool Invert { get => !Value; }


	public static implicit operator bool ( InvertableBool b ) => b.Value;
	public static implicit operator InvertableBool ( bool b ) => new (b);


	System.Xml.Schema.XmlSchema? IXmlSerializable.GetSchema () => throw new NotImplementedException ();
	void IXmlSerializable.WriteXml ( XmlWriter writer )
	{
		var cc = Value.ToString ().ToLower ().ToCharArray ();
		writer.WriteRaw (cc, 0, cc.Length);
	}
	void IXmlSerializable.ReadXml ( XmlReader reader )
	{
		var s = reader.ReadElementContentAsString ();
		var b = bool.Parse (s);
		_value = b;
	}

	public override string ToString () => Value.ToString ();

}



internal static class ResourceHelper
{

	/*
	public static object FindResource2(this VisualElement o, string key)
	{
		while (o != null)
		{
			if (o.Resources.TryGetValue(key, out var r1)) return r1;
			if (o is Page) break;
			if (o is IElement e) o = e.Parent as VisualElement;
		}
		if (Application.Current!.Resources.TryGetValue(key, out var r2)) return r2;
		return null;
	}
	 */

#if !WINDOWS

	[MethodImpl (MethodImplOptions.AggressiveInlining)]
	public static T FindAppResource<T> ( this string resourcekey, T defaultValue = default, Action<Exception>? onError = null ) where T : struct
	{
		try
		{
			var resFound = Application.Current!.Resources.TryGetValue (resourcekey, out var res);
			if (!resFound) throw new ArgumentOutOfRangeException (nameof (resourcekey), $"FindAppResource FAILED!");

			return (T) res;
		}
		catch (Exception ex)
		{
			onError?.Invoke (ex);
			//ex.eLogErrorNoUI(); 
		}
		return defaultValue;
	}


	[MethodImpl (MethodImplOptions.AggressiveInlining)]
	public static Int32 FindAppResource_Int32 ( this string key, Int32 def = 0 ) => FindAppResource (key, def);


	[MethodImpl (MethodImplOptions.AggressiveInlining)]
	public static UInt32 FindAppResource_UInt32 ( this string key, UInt32 def = 0u ) => FindAppResource (key, def);


#endif

}
