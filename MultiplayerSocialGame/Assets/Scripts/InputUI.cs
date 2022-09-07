using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;

public class InputUI : MonoBehaviour
{
    [SerializeField] private NetworkManager manager = null;
    [SerializeField] private Button botao = null;
    [SerializeField] private InputField inputField = null;
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private InputField portField = null;

    private string ipInput = null;
    private ushort portInput;

    public void Join()
    {
        ipInput = inputField.text;

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipInput, (ushort)7777);

        canvas.enabled = false;

        manager.StartClient();
    }

    public void Host()
    {
        portInput = ushort.Parse(portField.text);

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("127.0.0.1", portInput);

        canvas.enabled = false;

        manager.StartHost();
    }

    public void Close()
    {
        canvas.enabled = false;
    }

}
