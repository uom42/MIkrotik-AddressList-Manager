using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Android.Provider.Settings;

namespace uom.maui
{
	internal class GetDeviceInfo : IGetDeviceInfo
	{
		public string? GetDeviceID()
		{
			var context = Android.App.Application.Context;

			string? id = Android.Provider.Settings.Secure.GetString(context.ContentResolver, Secure.AndroidId);
			return id;
		}
	}


	public interface IGetDeviceInfo { string GetDeviceID(); }
}