using System.IO;
using System.Text;

namespace Tooling.Utility
{
	public class FileHelper
	{
		public static bool TryGetEncoding(string filename, out Encoding encoding)
		{
			var bom = new byte[4];
			using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				file.Read(bom, 0, 4);
			}

			if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76)
			{
				encoding = Encoding.UTF7;
				return true;
			}

			if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
			{
				encoding = Encoding.UTF8;
				return true;
			}

			if (bom[0] == 0xff && bom[1] == 0xfe)
			{
				encoding = Encoding.Unicode;
				return true;
			}

			if (bom[0] == 0xfe && bom[1] == 0xff)
			{
				encoding = Encoding.BigEndianUnicode;
				return true;
			}

			if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff)
			{
				encoding = Encoding.UTF32;
				return true;
			}

			encoding = Encoding.Default;
			return false;
		}
	}
}