#pragma warning disable RS1035
namespace Tsinswreng.CsDictMapper.SrcGen;
using System.IO;

internal class Logger{
	public static string Path = "./Tsinswreng.CsDictMapper.log.txt";
	public static void Append(string s){
		File.AppendAllText(Path, s+"\n");
	}

	public static nil Write(string Path, string s){
		File.WriteAllText(Path, s);
		return NIL;
	}

	public static nil Debug(string Path, string s){
		return NIL;
		// var Base = @"E:\_code\CsNgaq\Ngaq.Core\Tsinswreng.CsDictMapper.LogDir";
		// Directory.CreateDirectory(Base);
		// Path = $"{Base}/"+Path;
		// File.WriteAllText(""+Path, s);
		// return NIL;
	}
}
