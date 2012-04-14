using System;
using System.Windows;
using System.Windows.Resources;
using System.IO;

namespace Microsoft.Xna.Framework.Content
{
	public static class ContentHelpers
	{
		public static string GetAssetUri(string assetName, string rootDirectory, string extension)
		{
			string path = Path.Combine(rootDirectory, assetName + extension);
			path = path.Replace("\\", "/");
			return "/" + path;
		}

		public static Stream GetAssetStream(string assetName, string rootDirectory, string extension)
		{
			string path = Path.Combine(rootDirectory, assetName + extension);
			path = path.Replace("\\", "/");

			if(path.StartsWith("./")) // Remove leading dot directory
				path = path.Substring(2);

			// Application.GetResourceStream looks in the XAP
			// (and assemblies in the XAP with a /{shortAssemblyName};component/ URI)

			// This differs from the behaviour of Source URIs as described here:
			// http://nerddawg.blogspot.com/2008/03/silverlight-2-demystifying-uri.html
			// normally a leading slash is required (and will fall back to looking beside the XAP)

			// By using GetResourceStream, content is forced to be loaded into memory
			// immediately and synchronously.

			StreamResourceInfo info = Application.GetResourceStream(new Uri(path, UriKind.Relative));
			return info != null ? info.Stream : null;
		}

		public static Stream GetAssetStream(string assetName, string rootDirectory, string[] extensions)
		{
			foreach(string extension in extensions)
			{
				Stream stream = GetAssetStream(assetName, rootDirectory, extension);
				if(stream != null)
					return stream;
			}

			throw new ContentLoadException("Could not find a valid file for asset \"" + assetName + "\"");
		}
	}
}
