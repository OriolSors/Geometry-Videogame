using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionListDesignerScript : MonoBehaviour
{
    public RectTransform missionsScroll;
    public GameObject missionView;

    public Canvas userStatistics;
    public RectTransform userStatisticsScroll;
    public GameObject userView;

    private MissionListController missionListController;

    void Start()
    {
        userStatistics.enabled = false;
        missionListController = new MissionListController();
        FillMissionScroll();
    }

    private void FillMissionScroll()
    {
        List<string> missions = missionListController.GetAllMissionDesigner(AuthController.Instance.GetCurrentUser());
        foreach (string mission in missions)
        {
            GameObject go = Instantiate(missionView);
            go.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = mission;
            go.transform.SetParent(missionsScroll);

            go.GetComponentInChildren<Button>().onClick.AddListener(delegate { FillUserStatistics(mission); });
        }
    }

    private void FillUserStatistics(string mission)
    {
        userStatistics.enabled = true;

        Dictionary<string, string> players = missionListController.GetAllUserStatisticsInMission(AuthController.Instance.GetCurrentUser(), mission);

        foreach (Transform child in userStatisticsScroll.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (string user in players.Keys)
        {
            GameObject go = Instantiate(userView);
            go.transform.Find("Username Text").GetComponent<TextMeshProUGUI>().text = user;
            go.transform.Find("Status Text").GetComponent<TextMeshProUGUI>().text = players[user];
            go.transform.SetParent(userStatisticsScroll);

        }
    }

    public void ReturnMissionList()
    {
        userStatistics.enabled = false;
    }


    public void ToEditor3D()
    {
        SceneManager.LoadScene("3D Editor");
    }

    public void ToLogin()
    {
        AuthController.Instance.SignOut();
        SceneManager.LoadScene("Auth Screen");
    }

}
