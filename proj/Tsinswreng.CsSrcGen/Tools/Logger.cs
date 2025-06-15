#pragma warning disable RS1035
namespace Tsinswreng.CsSrcGen.Tools;
using System.IO;

public class Logger{
	public static string Path = "./Tsinswreng.CsSrcGen.log.txt";
	public static void Append(string s){
		File.AppendAllText(Path, s+"\n");
	}

	public static nil Write(string Path, string s){
		File.WriteAllText(Path, s);
		return NIL;
	}

	public static nil Debug(string Path, string s){
		return NIL;
		var Base = @"E:\_code\CsNgaq\Ngaq.Core\Tsinswreng.CsSrcGen.LogDir";
		Directory.CreateDirectory(Base);
		Path = $"{Base}/"+Path;
		File.WriteAllText(""+Path, s);
		return NIL;
	}
}
