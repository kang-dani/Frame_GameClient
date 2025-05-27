using UnityEngine;

[System.Serializable]
public class LoginResponseData
{
	public string userId;
	public string password;
	public string userNickname;
	public string token;
	public bool success;
	public string error;
}
