using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] private TextAsset[] imageQuizJsons, flagQuizJsons;
    [SerializeField] private List<Sprite> flagQuizSprites;
    [SerializeField] private List<Sprite> textQuizSprites;
    private Dictionary<string, Sprite> flagQuizSpriteDictionary = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> textQuizSpriteDictionary = new Dictionary<string, Sprite>();
    private QuizQuestion[] flagQuestions;
    private QuizQuestion[] textQuestions;

    private void Start()
    {
        FillDictionary();
        ParseFlagQuestions();
        ParseTextQuestions();
    }

    private void ParseTextQuestions()
    {
        textQuestions = new QuizQuestion[imageQuizJsons.Length];
        for (int i = 0; i < imageQuizJsons.Length; i++)
        {
            textQuestions[i] = JsonUtility.FromJson<QuizQuestion>(imageQuizJsons[i].text);
        }
    }

    private void ParseFlagQuestions()
    {
        flagQuestions = new QuizQuestion[flagQuizJsons.Length];
        for (int i = 0; i < flagQuizJsons.Length; i++)
        {
            flagQuestions[i] = JsonUtility.FromJson<QuizQuestion>(flagQuizJsons[i].text);
        }
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
        return flagQuestions[Random.Range(0, flagQuestions.Length)];
    }

    public QuizQuestion GetTextQuestion()
    {
        return textQuestions[Random.Range(0, textQuestions.Length)];
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