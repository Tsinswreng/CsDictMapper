#pragma warning disable RS1035
namespace Tsinswreng.CsSrcGen.Tools;
using System.IO;

public class Logger{
	public static string Path = "./Tsinswreng.CsSrcGen.log.txt";
	public static void Append(string s){
		File.AppendAllText(Path, s+"\n");
	}

	public static void Write(string Path, string s){
		File.WriteAllText("./Tsinswreng.CsSrcGen/"+Path, s);
	}
}
