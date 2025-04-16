using Google.Protobuf;
using System.Collections.Generic;
using System;
using System.Net;
using UnityClient.Network;
using UnityClient.Packet;
using UnityEngine;

public class HelloWorld : MonoBehaviour
{
    ServerSession session = new();

    void Start()
    {
        IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 27015);

        ClientService service = new ClientService();
        service.Connect(endPoint, () => { return session; }, 1);

    }

    // Update is called once per frame
    void Update()
    {
        List<PacketMessage> list = PacketQueue.Instance.PopAll();
        foreach (PacketMessage packet in list)
        {
            Action<PacketSession, IMessage> handler = ClientPacketManager.Instance.GetPacketHandler(packet.Id);
            if (handler != null && packet.Message != null)
                handler.Invoke(session, packet.Message);
        }
    }
}
