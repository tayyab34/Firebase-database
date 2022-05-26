using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Database;

public class DatabaseManager : MonoBehaviour
{
    private string UserID;
    public delegate void onCallback(string callback);
    public event onCallback callback;
    private DatabaseReference dbreference;
    public TMP_InputField Name;
    public TMP_InputField Gold;
    public TMP_Text nametext;
    public TMP_Text goldtext;

    // Start is called before the first frame update
    void Start()
    {
        UserID = SystemInfo.deviceUniqueIdentifier;
       dbreference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void CreateUser()
    {
        User newUser = new User(Name.text, int.Parse(Gold.text));
        string JSon = JsonUtility.ToJson(newUser);
        dbreference.Child("users").Child(UserID).SetRawJsonValueAsync(JSon);
    }

    public IEnumerator GetName(onCallback callback)
    {
        var userNameData=dbreference.Child("users").Child(UserID).Child("name").GetValueAsync();
        yield return new WaitUntil(predicate:()=> userNameData.IsCompleted);
        if (userNameData != null)
        {
            DataSnapshot snapshot = userNameData.Result;
            callback.Invoke(snapshot.Value.ToString());
        }
    }
    public IEnumerator GetGold(onCallback callback)
    {
        var userGoldData = dbreference.Child("users").Child(UserID).Child("gold").GetValueAsync();
        yield return new WaitUntil(predicate: () => userGoldData.IsCompleted);
        if (userGoldData != null)
        {
            DataSnapshot snapshot = userGoldData.Result;
            callback.Invoke(snapshot.Value.ToString());
        }
    }

    public void GetUserInfo()
    {
        StartCoroutine(GetName((string name) => {
            nametext.text = name;

        }));
        StartCoroutine(GetGold((string gold) => {
            goldtext.text = gold;

        }));
    }

    public void UpdateName()
    {
        dbreference.Child("users").Child("name").SetValueAsync(Name.text);
    }
    public void UpdateGold()
    {
        dbreference.Child("users").Child("name").SetValueAsync(Gold.text);
    }
}
