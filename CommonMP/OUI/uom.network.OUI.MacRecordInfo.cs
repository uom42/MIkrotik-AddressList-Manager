using System.Net.NetworkInformation;

namespace uom.network.OUI;


internal class MacRecordInfo ( string company , string country2LetterUCode , string postAddress , string postAddressCity )
{

    public MacRecordsGroup? Parent;
    public readonly string Company = company;
    /// <summary>2-Letter Uppercase Country Code</summary>
    public readonly string Country2LetterUCode = country2LetterUCode;
    public readonly string PostAddress = postAddress;
    public readonly string PostAddressCity = postAddressCity;



    public static (string MFGCode, MacRecordInfo Info) Parse ( Match m )
    {
        ArgumentNullException.ThrowIfNull(m);

        var rGroups = m!.Groups;

        string mgfCode = rGroups[ "MAC_SPLITTED" ].Value;
        PhysicalAddress mac = PhysicalAddress.Parse(mgfCode)!;
        mgfCode = mac.FormatMfgOctets();

        string Company = rGroups[ "COMPANY" ].Value.Trim();
        string country2LetterCode = rGroups[ "ADDR_COUNTRY" ].Value.Trim().ToUpper();

        string addr = rGroups[ "ADDR_1" ].Value.Trim();
        string city = rGroups[ "ADDR_CITY" ].Value.Trim();

        return (mgfCode, new MacRecordInfo(Company , country2LetterCode , addr , city));
    }


    /// <summary> У первого байта реального MAC надо отрезать первые 2 бита - они служебные</summary>
    internal static byte[] NormalizeMACMfgCode ( byte[] mac )
    {
        mac = [ .. mac.Take(3) ]; // Берём первые 3 байта MAC-адреса

        if ( mac.Length != 6 ) throw new ArgumentOutOfRangeException(nameof(mac));
        mac[ 0 ].setBitRef(0 , false);
        mac[ 0 ].setBitRef(1 , false);
        return mac;
    }


    public override string ToString ()
    {
        string result = Company;
        if ( Country2LetterUCode.isNotNullOrWhiteSpace ) result = $"({Country2LetterUCode}) {result}";
        return result;
    }


}

