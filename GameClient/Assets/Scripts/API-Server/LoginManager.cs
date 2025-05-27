using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using ClientCSharp.Packet;
using TMPro;
using ClientCSharp.Network;
using System.Net;
using Protocol;

namespace LoginManager
{
	public class LoginManager : MonoBehaviour
	{
		[SerializeField] private TMP_InputField idInputField;
		[SerializeField] private TMP_InputField passwordInputField;

		public void OnLoginButtonClicked()
		{
			string userId = idInputField.text.Trim();
			string userPassword = passwordInputField.text.Trim();
			Debug.Log($"로그인 시도: {userId}, {userPassword}");
			TryLogin(userId, userPassword);
		}
		public void TryLogin(string userId, string password)
		{
			StartCoroutine(LoginRequest(userId, password));
		}

		private IEnumerator LoginRequest(string _userId, string _password)
		{
			LoginResponseData loginData = new LoginResponseData
			{
				userId = _userId,
				password = _password,
			};

			string json = JsonUtility.ToJson(loginData);
			var uwr = new UnityWebRequest(loginUrl, "POST");
			byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
			uwr.uploadHandler = new UploadHandlerRaw(body);
			uwr.downloadHandler = new DownloadHandlerBuffer();
			uwr.SetRequestHeader("Content-Type", "application/json");
			Debug.Log($"로그인 요청: {json}");

			yield return uwr.SendWebRequest();

			if (uwr.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError($"로그인 요청 실패: {uwr.error}");
			}
			else
			{
				var responseText = uwr.downloadHandler.text;
				Debug.Log($"로그인 응답: {responseText}");

				var loginRes = JsonUtility.FromJson<LoginResponseData>(responseText);

				if (loginRes.success)
				{
					Debug.Log("Node.js 인증 성공 -> 게임 서버 연결 시작");

					GameController.Network.Init();

					var session = GameController.Network.Session;
					session.Init(loginRes.userId, "debug-token-12345");
					Debug.Log($"LoginManager에서 Init된 session: {session.GetHashCode()}");

					GameController.Network.ConnectToGameServer(tcpHost, tcpPort);
				}
				else
				{
					Debug.LogWarning($"로그인 실패: {loginRes.error}");
				}
			}
		}

		[SerializeField] private string loginUrl = "http://localhost:3000/api/login";
		[SerializeField] private string tcpHost = "127.0.0.1";
		[SerializeField] private int tcpPort = 27015;
		
	}
}
