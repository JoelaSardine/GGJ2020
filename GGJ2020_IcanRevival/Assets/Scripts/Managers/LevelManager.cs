using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [HideInInspector]
    public SicknessManager sicknessManager;
    
    public TextMeshProUGUI patientInfoField;
    public RectTransform winUIPanel;
    public RectTransform loseUIPanel;

    //public bool shuffleSpawns = false;
    public List<Transform> playersSpawns = new List<Transform>();

    //public bool shuffleSpawners = false;
    public List<PatientSpawner> patientSpawners = new List<PatientSpawner>();

    public int basePatientPerSpawner = 5;
    public float maxDeadPerPlayer = 2;

    [Header("Debug")]
    [SerializeField] private int patHealedCount = 0;
    [SerializeField] private int patSickCount = 0;
    [SerializeField] private int patDeadCount = 0;
    

    private void Awake()
    {
        sicknessManager = GetComponent<SicknessManager>();

        /*if (shuffleSpawns)
        {
            ShuffleList(playersSpawns);
        }
        if (shuffleSpawners)
        {
            ShuffleList(patientSpawners);
        }*/

        UpdateUI();
    }

    private void ShuffleList<T>(List<T> list)
    {
        List<T> newList = new List<T>(list.Count);
        while (list.Count > 0)
        {
            int r = Random.Range(0, list.Count);
            newList.Add(list[r]);
            list.RemoveAt(r);
        }
        list = newList;
    }

    public void Init()
    {
        for (int i = 0; i < patientSpawners.Count; i++)
        {
            if (i >= GameManager.Instance.currentPlayerCount)
            {
                patientSpawners[i].gameObject.SetActive(false);
            }
            else
            {
                patientSpawners[i].sicknessManager = sicknessManager;
            }
        }
    }

    public void AddSickPatient()
    {
        patSickCount++;
        UpdateUI();
    }

    public void KillPatient()
    {
        patSickCount--;
        patDeadCount++;
        UpdateUI();

        if (patDeadCount >= maxDeadPerPlayer * GameManager.Instance.currentPlayerCount)
        {
            loseUIPanel.gameObject.SetActive(true);
        }
    }

    public void HealPatient()
    {
        patSickCount--;
        patHealedCount++;
        UpdateUI();

        if (patSickCount == 0 && patHealedCount > 0 &&
            patDeadCount < maxDeadPerPlayer * GameManager.Instance.currentPlayerCount)
        {
            winUIPanel.gameObject.SetActive(true);
        }
    }

    public void UpdateUI()
    {
        string s = string.Format("<color=#60B717>{0} Healed</color>\n<color=#C8A800>{1} Sick</color>\n<color=#9C1F00>{2} Dead</color>",
            patHealedCount, patSickCount, patDeadCount);
        patientInfoField.text = s;
    }
}
