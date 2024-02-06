using Jint;
using System.IO;
using Jint.Native;

public class JavaScriptExecuter
{
    private Engine _engine;

    public void Init()
    {
        _engine = new Engine();
    }

    public void GetHashKey(string a, string b, string c, int r, out string output)
    {
        Engine engine = new Engine()
            .Execute(File.ReadAllText("JS files/aes.js"));

        JsValue tempResult = engine.Invoke("GetHashKey", a, b, c, r);
        string result = tempResult.AsString();

        output = result;
    }
}
