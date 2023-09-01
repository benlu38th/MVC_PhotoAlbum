using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MVC_PhotoAlbum.Function
{
    public class General
    {
		/// <summary>
		/// 判斷檔名尾綴是否是git或png或jpg或bmp
		/// </summary>
		/// <param name="filename">檔名</param>
		/// <returns></returns>
		public static bool IsPicture(string filename)
		{
			string extensionName = filename.Substring(filename.LastIndexOf(".") + 1);
			bool check = Regex.IsMatch(extensionName, @"^(git|png|jpg|bmp)$");
			return check;
		}

		/// <summary>
		/// 參數是圖片回傳圖片，不是回傳null
		/// </summary>
		/// <param name="photofile">HttpPostedFileBase檔案</param>
		/// <returns></returns>
		public static Image IsImage(HttpPostedFileBase photofile)
		{
			try
			{
				Image img = Image.FromStream(photofile.InputStream);
				return img;
			}
			catch
			{
				return null;
			}
		}
	}
}