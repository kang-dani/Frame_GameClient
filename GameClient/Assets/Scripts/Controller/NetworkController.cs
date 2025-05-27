using Google.Protobuf;
using System.Collections.Generic;
using System;
using System.Net;
using UnityEngine;
using ClientCSharp.Network;
using ClientCSharp.Packet;


public class NetworkController
{
    

    //사용하기 편하기 위해 한번 감쌈
    public void Send(IMessage packet)
    {
        session.Send(ClientPacketHandler.MakeSendBuffer(packet));
    }

    public void Init()
    {
        //string host = Dns.GetHostName();
        //IPHostEntry ipHost = Dns.GetHostEntry(host);
        //IPAddress ipAddr = ipHost.AddressList[0];

        if (initialized) return;
		initialized = true;

        //유니티 메인 스레드에서 호출 할수 있게 만듬
        ClientPacketHandler.Instance.UnityThreadHandler = (session, id, msg) => { PacketQueue.Instance.Push(id, msg); };

    }

    // Update is called once per frame
    public void Update()
    {
        List<PacketMessage> list = PacketQueue.Instance.PopAll();
        foreach (PacketMessage packet in list)
        {
            Action<PacketSession, IMessage> handler = ClientPacketHandler.Instance.GetPacketHandler(packet.Id);
            if (handler != null && packet.Message != null)
                handler.Invoke(session, packet.Message);
        }
    }

	public void ConnectToGameServer(string host, int port)
	{
		if (clientService == null)
			clientService = new ClientService();

		IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(host), port);
		clientService.Connect(endPoint, () => session);
	}

	private ServerSession session = new();
	private ClientService clientService;
	private bool initialized = false;

	public ServerSession Session => session;
}
