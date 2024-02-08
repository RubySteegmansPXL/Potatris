using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardLoadingWarning : MonoBehaviour
{

    public GameObject warningObject;

    void Start()
    {
        StartCoroutine(ShowWarning());
    }

    IEnumerator ShowWarning()
    {
        yield return new WaitForSeconds(5f);
        warningObject.SetActive(true);
    }

}
