
//rendeting quiz object
var quiz = {}
    quiz.hWrap = document.getElementById("quizWrap");
    
    quiz.hQn = document.createElement("div");
    quiz.hQn.id = "quizQn";
    quiz.hQn.innerHTML = "Press Start Or Select Game To Play";
    quiz.hWrap.appendChild(quiz.hQn);
    
    quiz.hAns = document.createElement("div");
    quiz.hAns.id = "quizAns";
    quiz.hWrap.appendChild(quiz.hAns);
    quiz.hAns.innerHTML = "";
    let radio = document.createElement("input");
    radio.type = "radio";
    radio.name = "quiz";
    radio.id = "quizo";
    quiz.hAns.appendChild(radio);
    //initalize the first quiz design
    for(let i = 1; i<5; i++){
        let label = document.createElement("label");
        label.id = `label${i}`
        quiz.hAns.appendChild(label);
        label.onclick = () => {
            selectedAnswer = label.innerHTML
            checkObj = {
                artist: document.querySelector("#quizQn").innerHTML,
                song: label.innerHTML
            }
            
            ajaxCall("POST",currApi + `/Questions/CheckAnswerSongForArtist/${checkObj.artist}/${checkObj.song}`, JSON.stringify(checkObj),checkAnswerSuccessCB,errorCB);
            sessionStorage.setItem("selectedAnswerLabelId",label.id);
        }
    }

    function errorCB(err){
        console.log(err)
    }

    //initiallize a game for guessing the artits, sets the button to start and the header to the game name. also set the start "onclick" to the relevant game.
    //uses an ajax call for a label "onclick" to check the answer and see if its correct.
    function songQuizInit(){
        matchSongsToArtistCounter = 0
        matchSongsToArtistCorrectAnswers = 0
        document.querySelector(".quiz-header").innerHTML = "Guess The Artist!"
        document.querySelector("#quizAns").style.display = "none";
        document.querySelector("#quizQn").innerHTML = "Press Start Or Select Game To Play"
        document.querySelector("#getArtistQ-play-again").style.display = "block"
        document.querySelector("#getArtistQ-play-again").innerHTML = "Start"
        document.querySelector("#getArtistQ-play-again").onclick = () => {getSongQuestion()}
        for(let i = 1; i< 5; i++){
            let label = document.querySelector(`#label${i}`)
            label.onclick = () => {
                selectedAnswer = label.innerHTML
                checkObj = {
                    artist: document.querySelector("#quizQn").innerHTML,
                    song: label.innerHTML
                }
                
                ajaxCall("POST",currApi + `/Questions/CheckAnswerSongForArtist/${checkObj.artist}/${checkObj.song}`, JSON.stringify(checkObj),checkQuestionAnswerSuccessCB,errorCB);
                sessionStorage.setItem("selectedAnswerLabelId",label.id);
            }
        }
    }
    //initiallize a game for guessing the song, sets the button to start and the header to the game name. also set the start "onclick" to the relevant game.
    //uses an ajax call for a label "onclick" to check the answer and see if its correct.
    function artistQuizInit(){
        matchSongsToArtistCounter = 0
        matchSongsToArtistCorrectAnswers = 0
        document.querySelector(".quiz-header").innerHTML = "Guess The Song!"
        document.querySelector("#quizAns").style.display = "none";
        document.querySelector("#quizQn").innerHTML = "Press Start Or Select Game To Play"
        document.querySelector("#getArtistQ-play-again").style.display = "block"
        document.querySelector("#getArtistQ-play-again").innerHTML = "Start"
        document.querySelector("#getArtistQ-play-again").onclick = () => {getArtistQuestion()}
        for(let i = 1; i< 5; i++){
            let label = document.querySelector(`#label${i}`)
            label.onclick = () => {
                selectedAnswer = label.innerHTML
                checkObj = {
                    artist: document.querySelector("#quizQn").innerHTML,
                    song: label.innerHTML
                }
                
                ajaxCall("POST",currApi + `/Questions/CheckAnswerSongForArtist/${checkObj.artist}/${checkObj.song}`, JSON.stringify(checkObj),checkAnswerSuccessCB,errorCB);
                sessionStorage.setItem("selectedAnswerLabelId",label.id);
            }
        }
    }
    //initiallize a game for guessing the lyrics, sets the button to start and the header to the game name. also set the start "onclick" to the relevant game.
    //uses an ajax call for a label "onclick" to check the answer and see if its correct.
    function lyricsQuizInit(){
        matchSongsToArtistCounter = 0
        matchSongsToArtistCorrectAnswers = 0
        document.querySelector(".quiz-header").innerHTML = "Guess The Lyrics!"
        document.querySelector("#quizAns").style.display = "none";
        document.querySelector("#quizQn").innerHTML = "Press Start Or Select Game To Play"
        document.querySelector("#getArtistQ-play-again").style.display = "block"
        document.querySelector("#getArtistQ-play-again").innerHTML = "Start"
        document.querySelector("#getArtistQ-play-again").onclick = () => {getSongLyrics()}
        for(let i = 1; i< 5; i++){
            let label = document.querySelector(`#label${i}`)
            label.onclick = () => {
                selectedAnswer = label.innerHTML
                checkObj = {
                    lyric: document.querySelector("#quizQn").innerHTML,
                    song: label.innerHTML
                }
                
                ajaxCall("POST",currApi + `/Questions/CheckAnswerLyricSong/${checkObj.lyric}/${checkObj.song}`, JSON.stringify(checkObj),checkLyricsSuccessCB,errorCB);
                sessionStorage.setItem("selectedAnswerLabelId",label.id);
            }
        }
    }

    //counters to check for number of correct answers out of 10/
    let matchSongsToArtistCounter = 0
    let matchSongsToArtistCorrectAnswers = 0

    //answer check success callback' sets the color of the label based of if it was correct or not, wait half seconf and go to next question while counting the score
    function checkAnswerSuccessCB(data){
        let labelID = sessionStorage.getItem("selectedAnswerLabelId")
        if(data){
            document.querySelector(`#${labelID}`).style.backgroundColor = "#34c724";
            matchSongsToArtistCorrectAnswers++;
            matchSongsToArtistCounter++;
            setTimeout(() => {
                getArtistQuestion();
            }, 500);
        }
        else{
            document.querySelector(`#${labelID}`).style.backgroundColor = "#f54a45";
            matchSongsToArtistCounter++;
            setTimeout(() => {
                getArtistQuestion();
            }, 500);

        }
    }

    //answer check success callback' sets the color of the label based of if it was correct or not, wait half seconf and go to next question while counting the score
    function checkQuestionAnswerSuccessCB(data){
        let labelID = sessionStorage.getItem("selectedAnswerLabelId")
        if(data){
            document.querySelector(`#${labelID}`).style.backgroundColor = "#34c724";
            matchSongsToArtistCorrectAnswers++;
            matchSongsToArtistCounter++;
            setTimeout(() => {
                getSongQuestion();
            }, 500);
        }
        else{
            document.querySelector(`#${labelID}`).style.backgroundColor = "#f54a45";
            matchSongsToArtistCounter++;
            setTimeout(() => {
                getSongQuestion();
            }, 500);

        }
    }

    //answer check success callback' sets the color of the label based of if it was correct or not, wait half seconf and go to next question while counting the score
    function checkLyricsSuccessCB(data){
        let labelID = sessionStorage.getItem("selectedAnswerLabelId")
        if(data){
            document.querySelector(`#${labelID}`).style.backgroundColor = "#34c724";
            matchSongsToArtistCorrectAnswers++;
            matchSongsToArtistCounter++;
            setTimeout(() => {
                getSongLyrics();
            }, 500);
        }
        else{
            document.querySelector(`#${labelID}`).style.backgroundColor = "#f54a45";
            matchSongsToArtistCounter++;
            setTimeout(() => {
                getSongLyrics();
            }, 500);

        }
    }

    //gets a new question for guess the song game
    function getArtistQuestion(){
        document.querySelector(".quiz-header").innerHTML = "Guess The Song!"
        document.querySelector("#getArtistQ-play-again").style.display = "none"
        document.querySelector("#quizAns").style.display = "grid";
        if(matchSongsToArtistCounter == 10){
            document.querySelector("#getArtistQ-play-again").innerHTML = "Play Again";
            document.querySelector("#getArtistQ-play-again").style.display = "block"
            document.querySelector("#quizAns").style.display = "none";
            document.querySelector("#quizQn").innerHTML = `you got ${matchSongsToArtistCorrectAnswers}/10 correct!`;
            matchSongsToArtistCorrectAnswers = 0;
            matchSongsToArtistCounter = 0;
            return;
        }

        ajaxCall("GET",currApi + `/Questions/GetQuestionArtist`,"",artistQuestionSuccessCB,errorCB);
        for(let i = 1; i<5;i++){
            document.querySelector(`#label${i}`).style.backgroundColor = "#fff"
        }

    }

    // sets the labels with the current questions and possible answers
    function artistQuestionSuccessCB(data){
            document.querySelector(`#quizQn`).innerHTML = data.contentQ;
            document.querySelector(`#label1`).innerHTML = data.answerA;
            document.querySelector(`#label2`).innerHTML = data.answerB;
            document.querySelector(`#label3`).innerHTML = data.answerC;
            document.querySelector(`#label4`).innerHTML = data.answerD;

    }

    //gets a new question for guess the song artist
    function getSongQuestion(){
        document.querySelector(".quiz-header").innerHTML = "Guess The Artist!"
        document.querySelector("#getArtistQ-play-again").style.display = "none"
        document.querySelector("#quizAns").style.display = "grid";
        if(matchSongsToArtistCounter == 10){
            document.querySelector("#getArtistQ-play-again").innerHTML = "Play Again";
            document.querySelector("#getArtistQ-play-again").style.display = "block"
            document.querySelector("#quizAns").style.display = "none";
            document.querySelector("#quizQn").innerHTML = `you got ${matchSongsToArtistCorrectAnswers}/10 correct!`;
            matchSongsToArtistCorrectAnswers = 0;
            matchSongsToArtistCounter = 0;
            return;
        }

        ajaxCall("GET",currApi + `/Questions/GetQuestionSong`,"",artistQuestionSuccessCB,errorCB);
        for(let i = 1; i<5;i++){
            document.querySelector(`#label${i}`).style.backgroundColor = "#fff"
        }

    }

    //gets a new question for guess the song artist
    function getSongLyrics(){
        document.querySelector(".quiz-header").innerHTML = "Guess The Lyrics!"
        document.querySelector("#getArtistQ-play-again").style.display = "none"
        document.querySelector("#quizAns").style.display = "grid";
        if(matchSongsToArtistCounter == 10){
            document.querySelector("#getArtistQ-play-again").innerHTML = "Play Again";
            document.querySelector("#getArtistQ-play-again").style.display = "block"
            document.querySelector("#quizAns").style.display = "none";
            document.querySelector("#quizQn").innerHTML = `you got ${matchSongsToArtistCorrectAnswers}/10 correct!`;
            matchSongsToArtistCorrectAnswers = 0;
            matchSongsToArtistCounter = 0;
            return;
        }

        ajaxCall("GET",currApi + `/Questions/GetQuestionLyrics`,"",artistQuestionSuccessCB,errorCB);
        for(let i = 1; i<5;i++){
            document.querySelector(`#label${i}`).style.backgroundColor = "#fff"
        }

    }

    