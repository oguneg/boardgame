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
    private QuizQuestion textQuestion;

    private void Start()
    {
        FillDictionary();
        ParseFlagQuestions();
        ParseTextQuestions();
    }

    private void ParseTextQuestions()
    {
        textQuestion = JsonUtility.FromJson<QuizQuestion>(imageQuizQuestion.text);
    }

    private void ParseFlagQuestions()
    {
        flagQuestion = JsonUtility.FromJson<QuizQuestion>(flagQuizJson.text);
    }

    private void FillDictionary()
    {
        foreach (var element in flagQuizSprites)
        {
            flagQuizSpriteDictionary.Add(element.name, element);
        }
        foreach (var element in textQuizSprites)
        {
            textQuizSpriteDictionary.Add(element.name, element);
        }
    }

    public Sprite GetFlag(string id)
    {
        return flagQuizSpriteDictionary[id];
    }

    public Sprite GetQuestionImage(string id)
    {
        return textQuizSpriteDictionary[id];
    }

    public QuizQuestion GetFlagQuestion()
    {
        return flagQuestion;
    }

    public QuizQuestion GetTextQuestion()
    {
        return textQuestion;
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