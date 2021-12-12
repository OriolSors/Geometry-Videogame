using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class TatamiController
{
    private DatabaseReference reference;

    private List<Question> questions;

    public TatamiController()
    {
        reference = FirebaseDatabase.GetInstance("https://geometry-videog-default-rtdb.firebaseio.com/").RootReference;
    }

    public async Task LoadQuestions()
    {
        List<Question> questions = new List<Question>();
        await reference.Child("Questions").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.Result.Value == null)
            {
                Debug.Log("No questions");

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot characteristic in snapshot.Children)
                {
                    List<string> characteristics = MissionListController.Instance.GetCurrentMissionPlayer().GetCharacteristics();
                    if (characteristics.Contains(characteristic.Key.ToString()))
                    {
                        foreach (DataSnapshot question in characteristic.Children)
                        {
                            string q = question.Children.First().Key.ToString();
                            string correct;

                            string first = question.Children.First().Children.ElementAt(0).Key.ToString();
                            string second = question.Children.First().Children.ElementAt(1).Key.ToString();
                            string third = question.Children.First().Children.ElementAt(2).Key.ToString();
                            string fourth = question.Children.First().Children.ElementAt(3).Key.ToString();

                            if ((bool)question.Children.First().Children.ElementAt(0).Value) correct = first;
                            else if ((bool)question.Children.First().Children.ElementAt(1).Value) correct = second;
                            else if ((bool)question.Children.First().Children.ElementAt(2).Value) correct = third;
                            else correct = fourth;

                            questions.Add(new Question(q, first, second, third, fourth, correct));
                        }

                    }
                }

                this.questions = questions;
            }

        });
        return;
    }

    public List<Question> GetQuestions()
    {
        return questions;
    }

    public Dictionary<int,bool> GetWavesDict()
    {
        return MissionListController.Instance.GetCurrentMissionPlayer().GetTatami().GetFiguresInWaves();
    }

    public void IncreaseInventory()
    {
        MissionListController.Instance.GetCurrentMissionPlayer().IncreaseInventory();
    }

    public Question RequestQuestion()
    {
        var rand = new System.Random();
        int index = rand.Next(questions.Count);
        return questions[index];
    }

    public void UpdateMissionPlayer()
    {
        MissionListController.Instance.UpdateMissionPlayer();
    }
}

public class Question
{
    public string question, first, second, third, fourth, correct;

    public Question(string question, string first, string second, string third, string fourth, string correct)
    {
        this.question = question;
        this.first = first;
        this.second = second;
        this.third = third;
        this.fourth = fourth;
        this.correct = correct;
    }

    public bool CheckCorrectAnswer(string text)
    {
        return correct == text;
    }
}
