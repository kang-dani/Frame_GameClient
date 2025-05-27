using ClientCSharp.Network;
using Google.Protobuf;
using Protocol;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace ClientCSharp.Packet
{
    public class ServerSession : PacketSession
    {
        public void Init(string userId, string token, string userNickname)
        {
            _userId = userId;
            _token = token;
			_userNickname = userNickname;
		}

        public override void OnConnected(EndPoint endPoint)
        {
			Debug.Log($"OnConnected session: {this.GetHashCode()}");
			Debug.Log($"_userId: {_userId}, _token: {_token}");

			if (string.IsNullOrEmpty(_userId) || string.IsNullOrEmpty(_token))
			{
				Debug.LogError("Init 안 된 세션에서 LoginRequest 전송 시도됨");
				return;
			}

			var loginPacket = new LoginRequest
			{
				UserId = _userId,
				Token = "debug-token-12345",
				UserNickname = _userNickname
			};

			var buffer = ClientPacketHandler.MakeSendBuffer(loginPacket);
			Debug.Log($"LoginRequest 전송 (크기: {buffer.Count} bytes)");

			Send(buffer);

		}

        public override void OnDisconnected(EndPoint endPoint)
        {
			Debug.Log("OnDisconnected from GameServer");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
			Debug.Log("Packet Received");
            ClientPacketHandler.Instance.HandlePacket(this, buffer);
        }

        public override void OnSend(int numOfBytes)
        {
            Debug.Log($"Pakcet Sent : {numOfBytes} bytes");
        }

		private string _userId;
		private string _token;
		private string _userNickname;

		public string UserId => _userId;
		public string Token => _token;
		public string UserNickname => _userNickname;

		public Action OnConnectedCallback;
	}
}
