using UnityEngine;
using IngameDebugConsole;

public class BuildCommand : MonoBehaviour
{
    [ConsoleMethod( "send", "Sendf a command to all ESP" )]
	public static void SendCommandToAllESP( string data )
	{
		var list = GameObject.FindObjectsByType<SerialPortManager>(FindObjectsSortMode.None);
        foreach( SerialPortManager serialPort in list )
        {
            serialPort.SendMessage( data );
        }

	}
}
