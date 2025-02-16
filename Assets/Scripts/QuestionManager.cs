using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] private TextAsset imageQuizQuestion, flagQuizJson;
    [SerializeField] private List<Sprite> flagQuizSprites;
    [SerializeField] private List<Sprite> textQuizSprites;
    private Dictionary<string, Sprite> flagQuizSpriteDictionary = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> textQuizSpriteDictionary = new Dictionary<string, Sprite>();
    private QuizQuestion flagQuestion;

    private void Start()
    {
        FillDictionary();
        ParseFlagQuestions();
    }

    private void ParseTextQuestions()
    {
    }

    private void ParseFlagQuestions()
    {
        flagQuestion = JsonUtility.FromJson<QuizQuestion>(flagQuizJson.text);
    }

    private void FillDictionary()
    {
        foreach(var element in flagQuizSprites)
        {
            flagQuizSpriteDictionary.Add(element.name, element);
        }
    }

    public Sprite GetFlag(string id)
    {
        return flagQuizSpriteDictionary[id];
    }

    public QuizQuestion GetFlagQuestion()
    {
        return flagQuestion;
    }
}

[System.Serializable]
public class Answer
{
    public string ImageID;
    public string Text;
}

[System.Serializable]
public class QuizQuestion
{
    public string ID;
    public int QuestionType;
    public string Question;
    public string CustomImageID;
    public Answer[] Answers;
    public int CorrectAnswerIndex;
}