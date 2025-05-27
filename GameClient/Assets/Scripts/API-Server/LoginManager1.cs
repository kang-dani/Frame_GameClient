using TMPro;
using UnityEngine;

public class LoginManager1 : MonoBehaviour
{
	[SerializeField] TMP_InputField id;
	[SerializeField] TMP_InputField password;

	public void OnLoginButtonClicked()
	{
		string userId = id.text;
		string userPassword = password.text;

		TryLR
	}
}
