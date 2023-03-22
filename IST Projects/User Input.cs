// A list of questions, and a dictionary of possible answers and responses
List<KeyValuePair<string, Dictionary<string, string>>> questions = new()
{
    new ("What is your name?", new ()
    {
        {"declan", "Hey Declan!"}, 
        {"mr. ng", "Hey sir! I feel like I should get full marks for this"}, 
        {"ria", "RIA DO YOUR WORK!"},
        {"", "Lovely to meet you, {0}"}
    }),
    new ("What is your favourite programming language?", new ()
    {
        {"c#", "Ah yes, the only correct answer"},
        {"python", "... I can't even with you."},
        {"javascript", "A web developer, eh? Very cool!"},
        {"_", "Oh that's fantastic! I love {0}"},
    }),
    new ("Given a programming problem, what is your preferred way of troubleshooting it?", new ()
    {
        {"ask declan", "Well I can't tell if you're Ria, Jackson, Oliver or any number of people in the class..."},
        {"rubber ducky", "A very nice technique! Unfortunately, never really worked for me..."},
        {"_", "Oh that's awesome! Any way is better than no way, eh?"}
    }),
    new ("If you had a million dollars, what would you do with it?", new ()
    {
        {"give it to declan", "Oh my! Thank you!"},
        {"give it to sir", "I'm not sure you can buy marks, but I'm sure he'll love it"},
        {"_", "Oh, that's a fantastic use of the money!"}
    })
    // Add more questions here
};

foreach (var question in questions)
{
    // Print question
    Console.WriteLine(question.Key);
    
    // Make sure we get an answer
    string? answer; 
    do
    {
        answer = Console.ReadLine();
    } while (answer == null);

    string lowerAnswer = answer.ToLower();

    if (question.Value.ContainsKey(lowerAnswer))
    {
        Console.WriteLine(question.Value[lowerAnswer]);
    }
    else
    {
        // Formats string automatically
        Console.WriteLine(question.Value[""], answer);
    }
}