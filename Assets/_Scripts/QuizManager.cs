using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Link to Unity video player
using UnityEngine.Video;

// Link to the Unity UI engine
using UnityEngine.UI;

/// <summary>
/// Handles all of the Quiz controls and saving of the certificate when the user passes the quiz.
/// </summary>
public class QuizManager : MonoBehaviour
{
    // Section (1) *******************************************************************

    // **************************************
    // screen transforms
    // **************************************

    // link to the camera's cameraMove to move through the scene
    public cameraMove cameraMove;

    // study/video screen anchor transform to link to that screen
    public Transform studyScreen;

    // quiz screen anchor transform to link to that screen
    public Transform quizScreen;

    // fail screen anchor transform to link to that screen
    public Transform failScreen;

    // pass screen anchor transform to link to that screen
    public Transform passScreen;

    // confirm screen anchor transform to link to that screen
    public Transform confirmScreen;

    // thank you screen anchor transform to link to that screen
    public Transform thankYouScreen;

    // main menu screen anchor transform to link to that screen
    public Transform mainMenuScreen;

    // **************************************
    // Button Methods - Main Menu
    // **************************************

    /// <summary>
    /// On click, goes from the Main Menu Screen to the Study Screen.
    /// </summary>
    public void StudyScreen()
    {
        // pass the study screen anchor to the camera's move script
        cameraMove.setAnchor(studyScreen);
    }

    /// <summary>
    /// On click, goes from the Main Menu Screen to the Quiz Screen.
    /// </summary>
    public void TakeQuiz()
    {
        // pass the take quiz screen anchor to the camera's move script
        cameraMove.setAnchor(quizScreen);
    }

   /// <summary>
   /// Quits the application.
   /// </summary>
    public void QuitApp()
    {
        // "reload" main menu to play audio
        cameraMove.setAnchor(mainMenuScreen);

        // print out message to the console
        Debug.Log("Quit Application");

        // quit the application
        Application.Quit();
    }

    // Section (1) *******************************************************************


    // Section (2) *******************************************************************

    // ********** Study Screen Variables **********

    // link to the vieo player game object
    public VideoPlayer videoPlayer;

    // **************************************
    // Button Methods - Study Screen
    // **************************************

    /// <summary>
    /// Plays the video.
    /// </summary>
    public void PlayVideo()
    {
        // play the video
        videoPlayer.Play();
    }

    /// <summary>
    /// Pauses the video.
    /// </summary>
    public void PauseVideo()
    {
        // pause the video
        videoPlayer.Pause();
    }

    /// <summary>
    /// Rewinds the video.
    /// </summary>
    public void RewindVideo()
    {
        // pause the video
        videoPlayer.Pause();

        // Set the frame to zero (0) - rewind
        videoPlayer.frame = 0;
    }

    /// <summary>
    /// On click, returns to the Main Menu Screen from the Study Screen.
    /// </summary>
    public void ReturnFromVideo()
    {
        // pause the video
        videoPlayer.Pause();

        // Set the frame to zero (0) - rewind
        videoPlayer.frame = 0;

        // pass the main menu screen anchor to the camera's move script
        cameraMove.setAnchor(mainMenuScreen);
    }

    // Section (2) *********************************************************************


    // Section (3) *********************************************************************

    // ********** Quiz Variables **********

    // string holds all questions as the file
    // is read from disk
    string allQuestions;

    // list of list that will hold the questions seperated into indiviual lists
    // each list will hld an entire question - question, answer1, answer2, answer3, answer4, correct answer
    public List<List<string>> questionsList = new List<List<string>>();

    // int holding the current question we are looking at
    // this starts at zero as the allQuestions List is zero (0) based
    int currentQuestion = 0;

    // link to quiz Text fields in the application
    public Text questionTitleText, questionText, answer1Text, answer2Text, answer3Text, answer4Text;

    // string holding the correct answer
    public string correctAnswerText;

    // int holding the total number of questions
    public int totalNumberOfQuestions;

    // create a new random object using systen.random
    public System.Random rnd = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        // load the questions from the Resources folder
        TextAsset readInQuestionsFile = Resources.Load("questions") as TextAsset;

        // place the questions in a string variable
        allQuestions = readInQuestionsFile.text;

        // split the text into individual question strings (all data together) using the delimiter ";"
        // each string within the Array will look like this example
        // "How many hours of sleep per night are recommended for children, 5-6 hours, 8-10 hours, 10-12 hours, The hour's don't matter, 8-10 hours";
        string[] questions = allQuestions.Split(';');

        // set the total question count
        totalNumberOfQuestions = questions.Length;

        // cycle through each string line
        foreach (string question in questions)
        {
            // split the question strings into individual question components using the delimiter ","
            // each string will look like this example
            // "How many hours of sleep per night are recommended for children", "5-6 hours", "8-10 hours", "10-12 hours", "The hour's don't matter", "8-10 hours"
            string[] questionParts = question.Split(',');

            // create a temp string list to hold the individual question parts for an entire question
            List<string> tempList = new List<string>();

            // add question
            tempList.Add(questionParts[0].Replace("\n", ""));

            // add answer1
            tempList.Add(questionParts[1].Replace("\n", ""));

            // add answer2
            tempList.Add(questionParts[2].Replace("\n", ""));

            // add answer3
            tempList.Add(questionParts[3].Replace("\n", ""));

            // add answer4
            tempList.Add(questionParts[4].Replace("\n", ""));

            // add correct answer 
            tempList.Add(questionParts[5].Replace("\n", ""));

            // add the question broken into individual parts to the questions List
            questionsList.Add(tempList);
        }

        // shuffle the questions in the list
        questionsList = ShuffleQuestions(questionsList);

        // load in the first question ****************************

        // show the user what question they are on out of the total questions
        questionTitleText.text = $"Question {currentQuestion + 1} of {totalNumberOfQuestions}";

        // show the question 
        questionText.text = $"{questionsList[currentQuestion][0]}?";

        // create a temp list to hold all possible question answers
        List<string> tempAnswersList = new List<string>();

        // get the possible answers for this question 
        // there are four (4) for each question
        tempAnswersList.Add(questionsList[currentQuestion][1]);
        tempAnswersList.Add(questionsList[currentQuestion][2]);
        tempAnswersList.Add(questionsList[currentQuestion][3]);
        tempAnswersList.Add(questionsList[currentQuestion][4]);

        // shuffle the possible answers - randomize them
        tempAnswersList = ShuffleAnswers(tempAnswersList);

        // display the text in answer1
        answer1Text.text = tempAnswersList[0];

        // display the text in answer2
        answer2Text.text = tempAnswersList[1];

        // display the text in answer3
        answer3Text.text = tempAnswersList[2];

        // display the text in answer4
        answer4Text.text = tempAnswersList[3];

        // store this question's correct answer
        correctAnswerText = questionsList[currentQuestion][5];
    }

    /// <summary>
    /// Shuffles the questions, in a list of string lists and returns the new shuffled list.
    /// </summary>
    /// <param name="list">The list of questions to shuffle.</param>
    /// <returns>The new shuffled list of questions.</returns>
    /// <remarks>Method to shuffle questions (these are individual string lists), that resided in a list of lists and returns the new, shuffled list.
    /// Accepts the lists of lists as a parameter, shuffles it returns it</remarks>
    public List<List<string>> ShuffleQuestions(List<List<string>> list)
    {
        int n = list.Count;

        while (n > 1)
        {
            n--;

            int k = rnd.Next(n + 1);

            List<string> value = list[k];

            list[k] = list[n];

            list[n] = value;
        }

        return list;
    }

    /// <summary>
    /// Shuffles the answer choices to a question, in a string list and returns the new shuffled list.
    /// </summary>
    /// <param name="list">The list of answer choices to a question.</param>
    /// <returns>The new shuffled list of answer choice to a question.</returns>
    public List<string> ShuffleAnswers(List<string> list)
    {
        int n = list.Count;

        while (n > 1)
        {
            n--;

            int k = rnd.Next(n + 1);
            string value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }

    // Section (3) *********************************************************************


    // Section (4) *********************************************************************

    // int holding the total number of questions the user has got right
    public int totalAnswersRight = 0;

    // link to check answer button
    public GameObject checkAnswerButton;

    // student answer filled in when a toggle box is checked
    public string studentAnswer;

    // link to the Correct or Incorrect GameObject Panels
    public GameObject correctAnswerPanel, incorrectAnswerPanel;

    // link to the audio for the correct and incorrect answers
    public AudioSource appAudio;
    public AudioClip incorrectAnswerClip;
    public AudioClip correctAnswerClip;
    // **************************************
    // Button Methods - Quiz
    // **************************************

    /// <summary>
    /// Called everytime a toggle has input - clicked.
    /// </summary>
    /// <remarks>When a toggle is checked, the check answer button is set to active, otherwise the check answer button is set to false.</remarks>
    public void ToggleChecked()
    {
        // number of toggles that are checked
        // set to zero(0) everytime
        int togglesChecked = 0;

        // grab all objects tagged "Toggle" (should be 4) and place in an array
        GameObject[] toggles = GameObject.FindGameObjectsWithTag("Toggle");

        // walk through this array of 4 toggle objects
        foreach (GameObject toggle in toggles)
        {
            // if this toggle is checked
            if (toggle.GetComponent<Toggle>().isOn)
            {
                // add one (1) to the counter
                togglesChecked++;

                // since a toggle is checked, show the check answer button
                checkAnswerButton.SetActive(true);

                // get the text from the checked toggle and place in var studentAnswer
                studentAnswer = toggle.transform.Find("Label").GetComponent<Text>().text;
            }
        }

        // check if no toggles are checked
        if (togglesChecked == 0)
        {
            // no answer given yet, turn check answer button off (hidden)
            checkAnswerButton.SetActive(false);

            // empty student answer var
            studentAnswer = "";
        }
    }

    /// <summary>
    /// Called from the Check Answer button, compares the user's answer to the correct answer.
    /// </summary>
    public void CheckAnswer()
    {
        checkAnswerButton.SetActive(false);

        if (string.Equals(studentAnswer, correctAnswerText))
        {
            // add one to the total number of correct answers
            totalAnswersRight++;

            // show the correct answer feedback panel
            correctAnswerPanel.SetActive(true);

            // Play audio correct answer
            appAudio.PlayOneShot(correctAnswerClip);
        }
        else
        {
            // show the incorrect answer feedback panel
            incorrectAnswerPanel.SetActive(true);

            // Play audio correct answer
            appAudio.PlayOneShot(incorrectAnswerClip);
        }
    }

    /// <summary>
    /// Closes the correct panel or the incorrect panel based on which panel is active.
    /// </summary>
    public void CloseAnswerPanel()
    {
        GameObject[] toggles = GameObject.FindGameObjectsWithTag("Toggle");

        // uncheck all toggles
        foreach (GameObject toggle in toggles)
        {
            toggle.GetComponent<Toggle>().isOn = false;
        }

        // close incorrect answer panel
        incorrectAnswerPanel.SetActive(false);

        // close correct answer panel
        correctAnswerPanel.SetActive(false);

        // load the next question
        NextQuestion();
    }


    // Section (4) *********************************************************************


    // Section (5) *********************************************************************

    // grade for student passing
    public int passingGrade;

    // link to the fail screen text to give
    // give the student the final grade
    public Text failText, failTextDropShadow;

    // link to the pass screen text to give
    // give the student the final grade
    public Text passText, passTextDropShadow;

    /// <summary>
    /// Loads the next question to ask the user.
    /// </summary>
    public void NextQuestion()
    {
        // increase current question variable
        currentQuestion++;

        // check to see if we are at the last question
        if (currentQuestion > (questionsList.Count - 1))
        {
            // calculate the student's grade
            float studentScore = ((float)totalAnswersRight / (float)questionsList.Count) * 100;

            // go to the end quiz screen
            if (studentScore >= passingGrade)
            {
                // write the user's score to the passing grade text
                // on the pass screen
                passText.text = $"{totalAnswersRight} out of {totalNumberOfQuestions} questions is a grade of {studentScore}.\n{passingGrade} was the minimum passing grade.";

                // write the user's score to the passing grade text
                // on the pass screen
                passTextDropShadow.text = $"{totalAnswersRight} out of {totalNumberOfQuestions} questions is a grade of {studentScore}.\n{passingGrade} was the minimum passing grade.";

                // go the pass screen
                cameraMove.setAnchor(passScreen);

                // don't continue with this script
                return;
            }
            else
            {
                // write the user's score to the failing grade text 
                // on the fail screen
                failText.text = $"{totalAnswersRight} out of {totalNumberOfQuestions} questions is a grade of {studentScore}.\n{passingGrade} is the minimum passing grade.";

                // write the user's score to the failing grade text 
                // on the fail screen
                failTextDropShadow.text = $"{totalAnswersRight} out of {totalNumberOfQuestions} questions is a grade of {studentScore}.\n{passingGrade} is the minimum passing grade.";
                // go the fail screen
                cameraMove.setAnchor(failScreen);

                // don't continue with this script
                return;
            }
        }

        // load in the next question
        // show the user what question they are on out of the total questions
        questionTitleText.text = $"Question {currentQuestion + 1} of {totalNumberOfQuestions}";

        // show the question 
        questionText.text = $"{questionsList[currentQuestion][0]}?";

        // create a temp list to hold all possible question answers
        List<string> tempAnswersList = new List<string>();

        // get all the possible answers for this question 
        // we know there are four (4) for each question
        tempAnswersList.Add(questionsList[currentQuestion][1]);
        tempAnswersList.Add(questionsList[currentQuestion][2]);
        tempAnswersList.Add(questionsList[currentQuestion][3]);
        tempAnswersList.Add(questionsList[currentQuestion][4]);

        // shuffle the possible answers - randomize them
        tempAnswersList = ShuffleAnswers(tempAnswersList);

        // display the text in answer1
        answer1Text.text = tempAnswersList[0];

        // display the text in answer2
        answer2Text.text = tempAnswersList[1];

        // display the text in answer3
        answer3Text.text = tempAnswersList[2];

        // display the text in answer4
        answer4Text.text = tempAnswersList[3];

        // store this question's correct answer
        correctAnswerText = questionsList[currentQuestion][5];
    }


    // Section (5) *********************************************************************


    // Section (6) *********************************************************************

    // student/user signature on pass screen (Input)
    public InputField userSignature;

    // student/user signature on confirm screen (Text)
    public Text certificateSignature;

    // text field for date on confirm screen
    public Text certificateDate;

    // link to the certificate
    public RectTransform certificate;

    // link to the Enter Signature Panel
    public GameObject enterSignaturePanel;

    /// <summary>
    /// Sets the user's signature and sets the certificate completion date. 
    /// </summary>
    /// <remarks>When the submit button is clicked, if the user has entered a signature, the camera is moved from the Quiz Pass screen to the Confirm Signature screen. 
    /// If the user has not entered a signature, the user is alerted to enter a signature.
    /// </remarks>
    public void EnterSignature()
    {
        // Set enter signature panel to active, if the user has not entered a signature
        if (string.IsNullOrWhiteSpace(userSignature.text))
            enterSignaturePanel.SetActive(true);
        else
        {
            // get user's signature from the signature input field
            // and place "on" certificate
            certificateSignature.text = userSignature.text.Trim();

            // set the date to the current date and place "on" certificate
            certificateDate.text = System.DateTime.Now.ToShortDateString();

            // move to the confirm signature screen
            cameraMove.setAnchor(confirmScreen);
        }
    }

    /// <summary>
    /// Closes the enter signature panel when the users clicks return.
    /// </summary>
    public void CloseEnterSignaturePanel()
    {
        // close the enter signature panel
        enterSignaturePanel.SetActive(false);
    }


    // Section (6) *********************************************************************


    // Section (7) *********************************************************************

    /// <summary>
    /// Goes from the Confirm Signature screen back to the Quiz Pass screen to redo the signature.
    /// </summary>
    public void RedoSignature()
    {
        // empty signature text
        userSignature.text = "";

        // empty date text
        certificateDate.text = "";

        // move to the pass screen
        cameraMove.setAnchor(passScreen);
    }

    /// <summary>
    /// Calls method to capture the screenshot of the certifcate.
    /// </summary>
    /// <remarks>The certifcate is saved to the user's local device and the application then goes to the Thank You screen.</remarks>
    public void ConfirmSignature()
    {
        // call method to take the screen capture
        StartCoroutine(TakeScreenShotAndSave());
    }

    private IEnumerator TakeScreenShotAndSave()
    {
        // wait until the frame processes
        yield return new WaitForEndOfFrame();

        // set width and height for screen capture for 1920 x 1080 screen
        float widthFactor = Screen.width / 1920f;
        float heightFactor = Screen.height / 1080f;
        int width = Mathf.FloorToInt(certificate.rect.width * widthFactor);
        int height = Mathf.FloorToInt(certificate.rect.height * heightFactor);

        // get the X and Y of the certificate
        Vector2 certPos = certificate.anchoredPosition;

        // create new texture
        var ss = new Texture2D(width, height, TextureFormat.RGB24, false);

        // read pixels into the new texture from screen
        ss.ReadPixels(new Rect(certPos.x, certPos.y, width, height), 0, 0);

        // set string for file name
        string fileName = $"{userSignature.text.Replace(" ", "")}_Certificate.png";

        // apply pixels to texture
        ss.Apply();

#if UNITY_EDITOR

        // if in editor, write data to the application's folder
        System.IO.File.WriteAllBytes(fileName, ss.EncodeToPNG());

        Debug.Log("Unity Editor");

#endif

#if UNITY_ANDROID

        // Save to screenshot to Gallery/Photos
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(ss, "GalleryTest", fileName, (success, path) => Debug.Log("Media save result: " + success + " " + path));

        Debug.Log("Permission result: " + permission);

#endif

        // To avoid memory leaks
        Destroy(ss);

        // go to the thank you screen
        cameraMove.setAnchor(thankYouScreen);
    }


    // Section (7) *********************************************************************


    // Section (8) *********************************************************************

    /// <summary>
    /// The button click to return to the main menu.
    /// </summary>
    public void MainMenu()
    {
        // call restart to reset variables
        Restart();

        // go to the main menu
        cameraMove.setAnchor(mainMenuScreen);
    }

    /// <summary>
    /// Resets the quiz variables and calls the Start method to reload the quiz.
    /// </summary>
    public void Restart()
    {
        // empty questions text
        allQuestions = "";

        // reset user signature inputfield
        userSignature.text = "";

        // reset certifcate signature text
        certificateSignature.text = "";

        // empty questionsList
        questionsList = new List<List<string>>();

        // set the current question to zero (0)
        currentQuestion = 0;

        // set total answers right to zero (0)
        totalAnswersRight = 0;

        // call Start to reload questions, randomize, and prepare quiz
        Start();
    }


    // Section (8) *********************************************************************


    // Update is called once per frame
    void Update()
    {

    }
}
