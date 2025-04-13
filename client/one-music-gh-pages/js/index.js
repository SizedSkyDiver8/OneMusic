const swaggerAPI = `https://localhost:7052/api`;
let rupAPI;
let currApi = swaggerAPI;
const lastfmKEY = "711cd7242581234c484cb8a564931277"
const deezerSecret = "b5d9a7955fc9a8b367bebcd125339bb6"


//this function mostly deals with the state of the connected user (or if there is no connected user). when the DOM is loaded it checks if there is 
// a connected user (by checking the local storage), and present it to the navbar alongside to signout option. if the connected user is an admin, it
// also present a "Admin Panel" button.
$(document).ready(() => {

    if(localStorage.getItem("userObj")){
        let loggedInUser = JSON.parse(localStorage.getItem("userObj"))
        document.querySelector(".login-register-btn").innerHTML = `<div class="login-register-btn logged-user-div mr-50">
        <a href="user-page.html" id="loginBtn">Logged in as ${loggedInUser.username}</a><span class="line"></span><a onclick="signout()" id="loginBtn">&nbsp;&nbsp;Signout</a></div>`
        if(loggedInUser.email == "admin@gmail.com"){
          document.querySelector(".logged-user-div").innerHTML = `<a href="admin-page.html" id="">&nbsp;&nbsp;Admin Panel</a><span class="line"></span><a onclick="signout()" id="loginBtn">&nbsp;&nbsp;Signout</a></div>`
        }
      }


  // handler for register form. takes the parameters from the form and handles the submittion to registration.
  $("#register-form").submit(() => { // register form
    let user = {
      name: $("#registerName").val().toLowerCase(),
      email: $("#registerEmail").val().toLowerCase(),
      password: $("#registerPassword").val(),
    };
    if($("#registerName").val() == ""){
      const Toast = Swal.mixin({
        toast: true,
        position: 'top-start',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
          toast.addEventListener('mouseenter', Swal.stopTimer)
          toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
      })
      
      Toast.fire({
        icon: 'error',
        title: 'Please enter a username and try again.'
      })        
        return false;
    }
    let api = currApi + `/Users/Register`;
    ajaxCall("POST",api,JSON.stringify(user),registerSuccessCB,registerErrorCB);
    return false;
  });

  // check if the password is matching with the validation password, and show a message if not
  function checkPassword() {
    if (this.value != $("#registerPassword").val()) {
      this.validity.valid = false;
      this.setCustomValidity("passwords must be identical!");
    } else {
      this.validity.valid = true;
      this.setCustomValidity("");
    }
  }
  
  // handler for login form. takes the parameters from the form and handles the submittion to login using ajax call.
  $("#login-form").submit(() => { // Login form
    let user = {
      name: "",
      email: $("#loginEmail").val().toLowerCase(),
      password: $("#loginPassword").val(),
    };
    if($("#loginEmail").val() == ""){
        const Toast = Swal.mixin({
          toast: true,
          position: 'top-start',
          showConfirmButton: false,
          timer: 3000,
          timerProgressBar: true,
          didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
          }
        })
        
        Toast.fire({
          icon: 'error',
          title: 'Please enter a username and try again.'
        })
        return false;
    }
    let api = currApi + `/Users/Login`;
    ajaxCall("POST",api,JSON.stringify(user),loginSuccessCB,errorCB);
    return false;
  });

  $("#reEnterRegisterPassword").on("blur", checkPassword); // check for password validation
  
});

function registerErrorCB(err){
  if(err.responseText.split("$")[1] == "Username taken"){
    Swal.fire({
      icon: "error",
      title: "Oops...",
      text: "Username is taken, try differnet one.",
    });
  }
   if(err.responseText.split("$")[1] == "email taken"){
    Swal.fire({
      icon: "error",
      title: "Oops...",
      text: "Email is taken, try differnet one.",
    });
  }
}


// success callback to the regsiter form submission. if there were no errors, it returns with true or false (whice indicates if the user been able to register or not).
// if the user registerd successfully (true) - present a success message and change the page to the home page, else -present an alert indicating that the user already exist, else (false)
function registerSuccessCB(data) {
  if (!data) {
    Swal.fire({
      icon: "error",
      title: "Oops...",
      text: "User already exist, try different User or Email combination!",
    });
    $("#registerEmail").val("") 
  } else if (data == true) {
    Swal.fire({
      icon: "success",
      title: "Welcome!",
      text: "New Account Created!",
    });
    userObj = {
        username: $("#registerName").val().toLowerCase(),
        email: $("#registerEmail").val().toLowerCase() 
    }
    localStorage.setItem("userObj", JSON.stringify(userObj))
    setTimeout(() => {
      window.location.href = "index.html";
    }, 3000);
  }
}

// three options for returned data : "0" - email incorrect, "1" - email and password correct and match, "2" - password incorrect.
// the function handles the login form success and present a message according to the data that came from the server and SP.
// if its connected succefully - calls to ajaxCall that will save the user to the localstorage for further use, and change the page to the homepage. 
function loginSuccessCB(data){
    switch(data){
        case 0: // email incorrect
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Email or Username is incorrect, Please try again.",
          });
            break;
        case 1: //email & password correct
        Swal.fire({
            icon: "success",
            title: "Welcome!",
            text: "Connected Succefully!",
          });
        let email = $("#loginEmail").val()
        ajaxCall("GET",currApi + `/Users/GetUserByEmail/${email}`,"",emailSuccessCB,errorCB);
        setTimeout(() => {
          window.location.href = "index.html";
        }, 3000);

            break;
        case 2: // password incorrect
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "Password is incorrect, Please try again.",
          });
            break;
            default:
                Swal.fire({
                    icon: "error",
                    title: "Oops...",
                    text: "somthing went wrong, Please try again.",
                  });
            break;
    }
}
// user to save the registerd/login user to the localstorage for further use in the app.
function emailSuccessCB(data){
    userObj = {
        username: data.name,
        email: data.email        
    }
    localStorage.removeItem("email");
    localStorage.setItem("userObj", JSON.stringify(userObj));
  }

// this function returns 10 most liked artists
function TopTenArtists(){
  const qs = `/Artists/TopArtists`;
                const api=currAPI+qs;
                ajaxCall("GET",api,"",successCB,errorCB); 
}

function successCB(data) {
  for (let i = 0; i < data.length; i++) {
    let artistId = `artist${i + 1}`;
    document.getElementById(artistId).innerHTML = `${i + 1}. ${data[i]}`;
  }
  let check=document.getElementById("a6");
  let header=document.createElement("h");
  header.innertext=data[5];
  check.appendChild(header);
}
  function errorCB(err){
    console.log(err);
  }


//this function will be activaeted when entering lyrics page (on load). in gets the song object by the song name, and sets the current like status in th page.
//in the second ajaxCall use the userObj in local storage to get the user and check if he liked the song.
function renderSongPage(songName){
  let user = JSON.parse(localStorage.getItem("userObj"));
  ajaxCall("GET",currApi + `/Songs/GetSongBySongName/${songName}`,"",songSuccessCB,errorCB);
  if(user != null){
    ajaxCall("GET",currApi + `/Songs/GetIfUserLikedSong/${user.email}/${songName}`,"",ifUserLikedSongSuccessCB,errorCB);
    ajaxCall("GET",currApi + `/Comments/GetAllCommentsSongs/${songName}`,"",renderCommentsToSongPageSuccessCB,errorCB);
  }

}

//fill the song page with relevant data, into pre-built containers.
function songSuccessCB(data){

  document.querySelector("#artistName").innerHTML = data.artistName;
  document.querySelector("#songName").innerHTML = data.title;
  document.querySelector("#lyricsContainer").innerText = data.lyrics
  document.querySelector("#song-likes-count").innerHTML = data.favoriteCount
  

}

// sets the status of the like button (if the user likes the song - sets the color and background to liked)
function ifUserLikedSongSuccessCB(data){
  let likeHeart = document.querySelector("#heart-button")
  if(data){
  likeHeart.style.color = "#FB076D"
  likeHeart.style.backgroundColor = "#fb076d40"

  }
  else{
    likeHeart.style.color = "#000000"
    likeHeart.style.backgroundColor = "#DDDDDD"
  }
}
//renders the artist page by calling ajax calls the fill the artist page. uses the user object from the stored procedure to get if the connect
//user liked the artist or not. the function also uses LAST.FM api to rertive data about the artist.
function renderArtistPage(artistName){
  let user = JSON.parse(localStorage.getItem("userObj"))
  document.querySelector("#artist").innerHTML = artistName;
  ajaxCall("GET",`http://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist=${artistName}&api_key=${lastfmKEY}&format=json`,"",artistInfoSuccessCB,errorCB);
  ajaxCall("GET",currApi + `/Songs/GetSongsByArtist/${artistName}`,"",songByArtistSuccessCB,errorCB);
  ajaxCall("GET",currApi + `/Artists/ArtistsLikes/${artistName}`,"",getArtistLikesSuccessCB,errorCB);
  if(user != null){
    ajaxCall("GET",currApi + `/Artists/GetIfUserLikedArtist/${user.email}/${artistName}`,"",ifUserLikedSongSuccessCB,errorCB);
    ajaxCall("GET",currApi + `/Comments/GetAllCommentsArtists/${artistName}`,"",renderCommentsToArtistPageSuccessCB,errorCB);
  }
}

//success callback to render the number of likes the artist have in total.
function getArtistLikesSuccessCB(data){
document.querySelector("#artist-likes-count").innerHTML = data
}

//success callback to get all the song related to given artist. renders the result data as links that will redirect to the song page when clicked.
//used "replace" func to escape single quetes error while rendering dynamiclly.
function songByArtistSuccessCB(data){
  let artistName = document.querySelector("#artist").innerHTML
  getDeezerDetails(artistName)
  songsCont = document.querySelector("#songs-content");
  for(let song of data){
    songsCont.innerHTML += `<a class="visitPage admin-panel-song-links" href="#" onclick="songSelectedFromList('${song.title.replace(/'/g, "\\'")}')">${song.title}</a><br>`
  }
}

//last.fm api call success callback. fill the data that came for the artist from last.fm api to its place in the page.
function artistInfoSuccessCB(data) {
  document.querySelector("#artist-summary").innerHTML = data.artist.bio.summary;
  document.querySelector("#artist-content").innerText = data.artist.bio.content;
}

//function to ajaxCall the method to fill the artists-list page with all the artists data.
function renderAllArtistsList(){
  ajaxCall("GET",currApi + `/Artists/GetAllArtists`,"",artistSuccessCB,errorCB);

}

//success callback for get all artists. used to fill the list of artists in artists-list page.
//uses accordion for each artist so when it gets clicked, it will present a brief summary about the artist, and option to go to his artist page.
//uses the split and repleace functions to deal with dots and single qoutes in the artist names.
// after rendering the lists, for each artist a function named "fillArtistListInfo" to fill with last.fm api data.
function artistSuccessCB(data) {
  let container = document.querySelector(".list-accordion");
  for (let i = 0; i < data.length; i++) {
    let name = data[i];
    let accordionId = `collapse${i + 1}`; // Generate unique id for each accordion

    container.innerHTML += `
        <div class="panel single-accordion">
          <h6>
            <a role="button" aria-expanded="true" aria-controls="${accordionId}" class="collapsed" data-parent="#accordion" data-toggle="collapse" href="#${accordionId}">
              ${name}
              <span class="accor-open"><i class="fa fa-plus" aria-hidden="true"></i></span>
              <span class="accor-close"><i class="fa fa-minus" aria-hidden="true"></i></span>
            </a>
          </h6>
          <div id="${accordionId}" class="accordion-content collapse">
            <p id="${name.split(" ").join("-").toLowerCase().replace(/'/g, "\\'").split(".").join("").split("'").join("")}-list-item-summary"></p>
            <p class="clickHereForInfo"></p>
          </div>
        </div>`;
  }

  let clickHereForInfoElements = document.querySelectorAll(".clickHereForInfo");
for (let i = 0; i < clickHereForInfoElements.length; i++) {
  clickHereForInfoElements[i].innerHTML = `<a class="visitPage" href="#" onclick="artistSelectedFromList('${data[i].replace(/'/g, "\\'")}')">Visit ${data[i]} Page</a>`;
}

  for(let name of data){
    fillArtistListInfo(name)
  }
}

//uses last.fm api to fill data to the artists list
function fillArtistListInfo(artistName){
  ajaxCall("GET",`http://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist=${artistName.toLowerCase()}&api_key=${lastfmKEY}&format=json`,"",fillSuccessCB,errorCB);
}

//success callback for last.fm api to handle filling the inforamtion to artist elements in artists list page.
//split, replace and join used to escape dots, single qoutes and dashes in the names.
function fillSuccessCB(data){
  if("artist" in data){
      document.querySelector(`#${data.artist.name.split(" ").join("-").toLowerCase().replace(/'/g, "\\'").split(".").join("").split("'").join("")}-list-item-summary`).innerHTML =  data.artist.bio.summary
  }  
  document.querySelector(`#${data.artist.name.split(" ").join("-").toLowerCase().replace(/'/g, "\\'").split(".").join("").split("'").join("")}-list-item-summary`).innerHTML +=  `<a class="visitPage" onclick="artistSelectedFromList(${data.artist.name}) >Visit ${data.artist.name} Page</audio>`

}

//will be activated "onclick" when clicking an artist. sets the artist name temp to sessionStorage and redirect it 
//to the artist page, the artist page will get the name from session storage on load and present the artist full page.
function artistSelectedFromList(artistName){
  localStorage.setItem('selectedArtist', artistName);
  sessionStorage.setItem("tempArtist", artistName)
window.location.href = 'artist-page.html'
}

//same as artistSelectedFromList
function songSelectedFromList(songName){
  localStorage.setItem('selectedSong', songName);
  sessionStorage.setItem("tempSong",songName);
window.location.href = 'song-page.html'
}

//handles the signout of a user from the website by removing it from the localstorage.
function signout(){
  localStorage.removeItem("userObj");
  window.location.href = "login.html"
}

//a fucntion to handle the search in the home page.
//resets the containers to empty (to avoid them if there was a previous search)
//calls the methods with ajaxCall to get the relevant result from the server.
$(document).ready (() => {
  $("#search-form").submit(() => {
    let toSearch = $("#search").val();
    document.querySelector("#artist-title").innerHTML = ""
    document.querySelector("#artist-result").innerHTML = ""
    document.querySelector("#song-title").innerHTML = ""
    document.querySelector("#song-result").innerHTML = ""

    ajaxCall("GET",currApi + `/Artists/ArtistsByWord/${toSearch}`,"",searchArtistSuccessCB,errorCB);
    ajaxCall("GET",currApi + `/Songs/GetSongByWord/${toSearch}`,"",searchSongSuccessCB,errorCB);
    return false;
  })

})

//handles the data from the server that searched for artists by the search query given.
//renders it dynamiclly to a specified container for artists result.
function searchArtistSuccessCB(data){
  if(data.length > 0){
    document.querySelector("#artist-title").innerHTML = "Artists:"
    for(let artist of data){
      document.querySelector("#artist-result").innerHTML += `<a style="display:inline;" class="visitPage" href="#" onclick="artistSelectedFromList('${artist.replace(/'/g, "\\'")}')">${artist}</a><br>` 
    }
  }

}


//handles the data from the server that searched for songs by the search query given (songs or lyrics).
//renders it dynamiclly to a specified container for songs result
function searchSongSuccessCB(data){
  if(data.length > 0){
    document.querySelector("#song-title").innerHTML = "Songs:"
    for(let song of data){
      document.querySelector("#song-result").innerHTML += `${song.artistName} - <a style="display:inline;" class="visitPage" href="#" onclick="songSelectedFromList('${song.title.replace(/'/g, "\\'")}')">${song.title}</a><br>` 
    }
  }
}

//handles the event of pressed like in artist page. call an ajaxCall to add the like to the artist.
//uses the user object from local storage to indicate which user liked.
function likePressedArtist(){
  let artistName = document.querySelector("#artist").innerHTML
  let user = JSON.parse(localStorage.getItem("userObj"))
  if(user == null){
    const Toast = Swal.mixin({
      toast: true,
      position: 'top-start',
      showConfirmButton: false,
      timer: 3000,
      timerProgressBar: true,
      didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer)
        toast.addEventListener('mouseleave', Swal.resumeTimer)
      }
    })
    
    Toast.fire({
      icon: 'error',
      title: 'No user connected, could not proccess like.'
    })
    return
  }
  ajaxCall("POST",currApi + `/Artists/AddRemoveLike/${user.email}/${artistName}`,"",artistAddRemoveLikeSuccessCB,errorCB);
}

//handles the event of pressed like in song page. call an ajaxCall to add the like to the song.
//uses the user object from local storage to indicate which user liked.
function likePressedSong(){
  let songName = document.querySelector("#songName").innerHTML
  let user = JSON.parse(localStorage.getItem("userObj"))
  if(user == null){
    const Toast = Swal.mixin({
      toast: true,
      position: 'top-start',
      showConfirmButton: false,
      timer: 3000,
      timerProgressBar: true,
      didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer)
        toast.addEventListener('mouseleave', Swal.resumeTimer)
      }
    })
    
    Toast.fire({
      icon: 'error',
      title: 'No user connected, could not proccess like.'
    })
    return
  }
  ajaxCall("POST",currApi + `/Songs/UserLikesSong/${user.email}/${songName}`,"",songAddRemoveLikeSuccessCB,errorCB);
}

//sets the likes count on artist page
function artistLikeSuccessCB(data){
  document.querySelector("#artist-likes-count").innerHTML = data
  
}

//handles the event of like pressed by a user.sets the count to new counted liked and calls a function to 
//check wether it was like or unlike and set the icon to the proper color.
function songAddRemoveLikeSuccessCB(data){
  let user  = JSON.parse(localStorage.getItem("userObj"))
  document.querySelector("#song-likes-count").innerHTML = data
  let songName = document.querySelector("#songName").innerHTML
  ajaxCall("GET",currApi + `/Songs/GetIfUserLikedSong/${user.email}/${songName}`,"",ifUserLikedSongSuccessCB,errorCB);
}

//handles the event of like pressed by a user.sets the count to new counted liked and calls a function to 
//check wether it was like or unlike and set the icon to the proper color.
function artistAddRemoveLikeSuccessCB(){
  let user  = JSON.parse(localStorage.getItem("userObj"))
  let artistName = document.querySelector("#artist").innerHTML
  ajaxCall("GET",currApi + `/Artists/ArtistsLikes/${artistName}`,"",artistLikeSuccessCB,errorCB);
  ajaxCall("GET",currApi + `/Artists/GetIfUserLikedArtist/${user.email}/${artistName}`,"",ifUserLikedSongSuccessCB,errorCB);
}

//rendred the admin page by calling to get all the full lists of users,songs, and artists.
//for each user rendering its details, liked songs and liked artists. in addition also render some statistics to the page.
function renderAdminPage(){
  ajaxCall("GET",currApi + `/Users/GetAllUsers`,"",getAllUsersSuccessCB,errorCB);
  ajaxCall("GET",currApi + `/Users/GetStatisticsAdmin`,"",getStatisticSuccessCB,errorCB);
  ajaxCall("GET",currApi + `/Artists/GetAllArtistsWithLikes`,"",getAllArtistsSuccessCB,errorCB);
  ajaxCall("GET",currApi + `/Songs/GetAllSongs`,"",getAllSongsSuccessCB,errorCB);

}

//gets the statistics for the top counter in the admin page.
//sets the number of registerd users, the number of songs and number of artists in the database.
function getStatisticSuccessCB(data){
document.querySelectorAll(".counter")[0].innerHTML = data[0]
document.querySelectorAll(".counter")[1].innerHTML = data[1]
document.querySelectorAll(".counter")[2].innerHTML = data[2]
}

//function to handle success callback when calling to get all the songs. sets all the song as a list in a container
// with their artist name, the name of the songs and number of total likes.
function getAllSongsSuccessCB(data){
  console.log(data)
  let songsCont = document.querySelector("#songs-list-content");
  for (let song of data){
    songsCont.innerHTML += `${song.artistName} - <a style="display:inline;" class="visitPage" href="#" onclick="songSelectedFromList('${song.title.replace(/'/g, "\\'")}')">${song.title}</a> - ${song.favoriteCount} Likes<br>`
  }
}

//function to handle success callback when calling to get all the artists. sets all the artists as a list in a container
// with their artist name and number of total likes.
function getAllArtistsSuccessCB(data){
  let artistCont = document.querySelector("#artist-list-content") 
  for(let artist of data){
    artistCont.innerHTML += `<a class="visitPage admin-panel-song-links" href="#" onclick="artistSelectedFromList('${artist.name.replace(/'/g, "\\'")}.replace(/'/g, "\\'")')">${artist.name}</a> - ${artist.favoriteCount} Likes<br> `
  }
}

//function to handle success callback when calling to get all the users. sets all the artists as a list in a container
// for each artists renderd 3 tabs that will contain the user details, their liked songs, and their liked artists.
//each tab will cotnain different info.
//also calls to get the specific used liked artists and songs with ajaxCall.
function getAllUsersSuccessCB(data) {
  let accordionCont = document.querySelector(".users-accordion");
  let collapseCounter = 1;

  for (let user of data) {
    let registerDate = new Date(user.dateRegister)
    const formattedDate = registerDate.toLocaleDateString("en-GB", {
      day: "numeric",
      month: "numeric",
      year: "numeric",
    });
    accordionCont.innerHTML += `
      <div class="panel single-accordion">
        <h6>
          <a role="button" class="collapsed inner-accordion" aria-expanded="false" aria-controls="collapse${collapseCounter}" data-toggle="collapse" data-parent="#accordion" href="#collapse${collapseCounter}">
            ${user.name}
            <span class="accor-open"><i class="fa fa-plus" aria-hidden="true"></i></span>
            <span class="accor-close"><i class="fa fa-minus" aria-hidden="true"></i></span>
          </a>
        </h6>
        <div id="collapse${collapseCounter}" class="accordion-content collapse">
          <div class="col-12">
            <div class="oneMusic-tabs-content">
              <ul class="nav nav-tabs tabs-container" id="myTab${collapseCounter}" role="tablist">
                <li class="nav-item">
                  <a class="nav-link active" id="tab--1-${collapseCounter}" data-toggle="tab" href="#tab1-${collapseCounter}" role="tab" aria-controls="tab1-${collapseCounter}" aria-selected="true">User Details</a>
                </li>
                <li class="nav-item">
                  <a class="nav-link" id="tab--2-${collapseCounter}" data-toggle="tab" href="#tab2-${collapseCounter}" role="tab" aria-controls="tab2-${collapseCounter}" aria-selected="false">Liked Songs</a>
                </li>
                <li class="nav-item">
                  <a class="nav-link " id="tab--3-${collapseCounter}" data-toggle="tab" href="#tab3-${collapseCounter}" role="tab" aria-controls="tab3-${collapseCounter}" aria-selected="false">Liked Artists</a>
                </li>
              </ul>

              <div class="tab-content mb-100" id="myTabContent${collapseCounter}">
                <div class="tab-pane fade show active" id="tab1-${collapseCounter}" role="tabpanel" aria-labelledby="tab--1-${collapseCounter}">
                  <div class="oneMusic-tab-content">
                    <!-- Tab Text -->
                    <div class="oneMusic-tab-text">
                      <p class="user-details-tab"><span class="user-details-titles">Username:</span> ${user.name}</p>
                      <p class="user-details-tab"><span class="user-details-titles">Email:</span> ${user.email}</p>
                      <p class="user-details-tab"><span class="user-details-titles">Date Created:</span> ${(formattedDate)}</p>
                    </div>
                  </div>
                </div>
                <div class="tab-pane fade" id="tab2-${collapseCounter}" role="tabpanel" aria-labelledby="tab--2-${collapseCounter}">
                  <div class="oneMusic-tab-content">
                    <!-- Tab Text -->
                    <div class="oneMusic-tab-text" id="${user.email.split("@")[0]}-liked-songs">
                    </div>
                  </div>
                </div>
                <div class="tab-pane fade " id="tab3-${collapseCounter}" role="tabpanel" aria-labelledby="tab--3-${collapseCounter}">
                  <div class="oneMusic-tab-content">
                    <!-- Tab Text -->
                    <div class="oneMusic-tab-text liked-artists" id="${user.email.split("@")[0]}-liked-artists">
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>`;

    collapseCounter++;
    ajaxCall("GET",currApi + `/Users/GetUserLikedSongs/${user.email}`,"",getUserLikedSongSuccessCB,errorCB);
    ajaxCall("GET",currApi + `/Users/GetUserLikedArtists/${user.email}`,"",getUserLikedArtistsSuccessCB,errorCB);
  }
}

//sets the liked song by a specific user to the matching place in the admin page list of users (inside the liked songs tab)
function getUserLikedSongSuccessCB(data){
  let likedSongsCont = document.querySelector(`#${data[0].split("@")[0]}-liked-songs`)
  for(let i = 1; i<data.length; i++){
    likedSongsCont.innerHTML += `<a class="visitPage admin-panel-song-links" href="#" onclick="artistSelectedFromList('${data[i].artistName.replace(/'/g, "\\'")}')">${data[i].artistName}</a> - <a class="visitPage admin-panel-song-links" href="#" onclick="songSelectedFromList("${data[i].title.replace(/'/g, "\\'")}")">${data[i].title}</a><br> `
  }
}

//sets the liked artists by a specific user to the matching place in the admin page list of users (inside the liked artists tab)
function getUserLikedArtistsSuccessCB(data){
  let likedArtistsCont = document.querySelector(`#${data[0].split("@")[0]}-liked-artists`)
  for(let i = 1; i<data.length; i++){
    likedArtistsCont.innerHTML += `<a class="visitPage admin-panel-song-links" href="#" onclick="artistSelectedFromList('${data[i].name.replace(/'/g, "\\'")}')">${data[i].name}</a><br> `
  }
}

//get artists that the user liked
function getTopArtists(){
  let userObjString=localStorage.getItem('userObj');
  let userObj = JSON.parse(userObjString);
  document.getElementById("userName").innerHTML=userObj.username
  let api = currApi + `/Users/GetUserLikedArtists/${userObj.email}`;
  ajaxCall("GET",api,"",TopArtistsSuccessCB,errorCB);
}

// returns the favorite arists of the user (max 5)
function TopArtistsSuccessCB(data){
  if(data.length>=2){
    document.getElementById("noArtistsForUser").remove();
    for (let i=0; i<data.length-1;i++){
      if(i==5){
        break;
      }
      let div=document.createElement("div");
      div.className="single-artist";
      div.innerHTML = `<img class="top-image" id="=${data[i+1].name.replace(/'/g, "\\'").split(" ").join("")}" src="img/bg-img/a4.jpg" alt=""><div class="album-info"><a class="ppp" href="#" onclick="artistSelectedFromList('${data[i+1].name.replace(/'/g, "\\'")}')"><h5 id="fav-artist">${data[i+1].name}</h5></a></div>`;
      document.getElementById("userContainerArtists").appendChild(div);
      ajaxCall("GET",currApi + `/Artists/GetDeezerInfo/${data[i+1].name}`,"",imagesTopSuccessCB,errorCB);
      }
  }
  
}

//get 5 songs that the user liked
function getFiveSongsByUser(){
  let userObjString=localStorage.getItem('userObj');
  let userObj = JSON.parse(userObjString);
  let api = currApi + `/Users/GetUserLikedSongs/${userObj.email}`;
  ajaxCall("GET",api,"",TopSongsSuccessCB,errorCB);
}

// sets 5 random songs that the user liked 
function TopSongsSuccessCB(data){
  if(data.length>=2){
    document.getElementById("noSongsForUser").remove();
    let numbers=[];
  while(numbers.length<5){
    if(numbers.length<data.length-1)
    break;
      let number=Math.floor(Math.random() * (data.length - 1 - 1)) + 1;
      if (!numbers.includes(number)) {
          numbers.push(number);
      }
  }
  for (let i=0; i<data.length-1;i++){
    let div=document.createElement("div");
    div.className="single-song";
    div.innerHTML = `<div class="album-info"><a class="ppp" href="#" onclick="songSelectedFromList('${data[i+1].title.replace(/'/g, "\\'")}')">${data[i+1].title}</a></div>`;
    document.getElementById("userContainerSongs").appendChild(div);  
  }
  }
  
}

// render the user profile with 5 liked artists and 5 liked songs
function renderUserProfile(){
  getTopArtists();
  getFiveSongsByUser();
}

//used to handle the event of refreshing a page. gets the temp item of the last artist visited from sessionStorage and renders it again to the page.
function renderArtistFromStorage(){
  if(sessionStorage.getItem("tempArtist")){
    let artist = sessionStorage.getItem("tempArtist");
    renderArtistPage(artist);
  }
}

//used to handle the event of refreshing a page. gets the temp item of the last song visited from sessionStorage and renders it again to the page.
function renderSongFromStorage(){
  if(sessionStorage.getItem("tempSong")){
    let song = sessionStorage.getItem("tempSong");
    renderSongPage(song);
  }
}

//calls a method in the server that calls to Deezer api, and return another info about the artists.
function getDeezerDetails(artistName){
  ajaxCall("GET",currApi + `/Artists/GetDeezerInfo/${artistName}`,"",deezerSuccessCB,errorCB);
}

//gets the Deezer api info came back from the server and sets the image in designated place in artist profile.
//iterates the results because the data returnd is 25 results related to the name given.
//while iterating we check for match with the artist name to be sure it is his image.
function deezerSuccessCB(data){
  for(let i = 0; i<data.data.length;i++){
    let artistName = document.querySelector("#artist").innerHTML
    let searchName = data.data[i].artist.name;
    if(searchName.includes(artistName)){
      document.querySelector("#artist-image").src =  data.data[i].artist.picture_medium
      break
    }
  }
}
// render the songs page with songs
function renderAllSongsList(){
  ajaxCall("GET",currApi + `/Songs/GetAllSongs`,"",allSongsSuccessCB,errorCB);
}

// sets the page with all songs from the database
function allSongsSuccessCB(data){
  let container = document.querySelector(".song-list-accordion");
  for (let i = 0; i < data.length; i++) {
    let songName = data[i].title;
    let artistName = data[i].artistName;
    let accordionId = `collapse${i + 1}`; // Generate unique id for each accordion

    container.innerHTML += `
        <div class="panel single-accordion">
          <h6>
            <a role="button" aria-expanded="true" aria-controls="${accordionId}" class="collapsed" data-parent="#accordion" data-toggle="collapse" href="#" onclick="songSelectedFromList('${data[i].title.replace(/'/g, "\\'")}')">
              ${artistName} - ${songName}
            </a>
          </h6>
          <div id="${accordionId}" class="accordion-content collapse">
          </div>
        </div>`;
  }
}

// render the home page with favorite artists
function renderHomepageTop(){
  ajaxCall("GET",currApi + `/Artists/TopArtists`,"",homepageTopSuccessCB,errorCB);

}

// sets the page with favorite artists and call the Deezer API for images of the artists 
function homepageTopSuccessCB(data){
  let i = 0;
  for(let artist of data){
    if(i >5 || artist.favoriteCount == 0){
      return;    
    }
    i++
    document.querySelector(".top-cont").innerHTML+=
     `
     <div class="col-2">
     <div class="single-album">
     <a  onclick="artistSelectedFromList('${artist.name.replace(/'/g, "\\'")}')">
     <img class="top-image" id="${artist.name.replace(/'/g, "\\'").split(" ").join("")}" src="img/bg-img/a9.jpg" alt="">
    <div class="album-info">
            <h5 id="artist${i}" class="homepage-tops">${artist.name}<br><span class="top-likes-count"> ${artist.favoriteCount} Likes </span></h5>
        </a>
        <p >#${i}</p>
    </div>
</div>
</div>
`
ajaxCall("GET",currApi + `/Artists/GetDeezerInfo/${artist.name}`,"",imagesTopSuccessCB,errorCB);
  }
}

//gets the images for the artists and sets them in place
function imagesTopSuccessCB(data){
  let topImgElements = document.querySelectorAll(".top-image")
  for(let elem of topImgElements){
    for(let i = 0; i<data.data.length;i++){
      let dataArtistName = data.data[i].artist.name.replace(/'/g, "\\'").split(" ").join("")
      if(elem.id.includes(dataArtistName)){
        elem.src = data.data[i].artist.picture_medium
        break 
      }
    }
  }
}

// sets the comments that were writen on an artist in his page
function renderCommentsToArtistPageSuccessCB(data){
   let userObj = JSON.parse(localStorage.getItem("userObj"))
  let commentsCont = document.querySelector(".artist-comment-list");
  commentsCont.innerHTML = "";
  document.querySelector(".label-info").innerHTML = data.length
  if(data.length == 0){
    commentsCont.innerHTML = "No Comments Yet"
  }
  else{
    for(let comment of data){
      commentsCont.innerHTML += `  <li class="list-group-item artist-comment-list">
                            <div class="row">
  <div class="col-xs-10 col-md-11">
      <div>
          <div class="mic-info">
              By: <a class="comment-username">${comment.username}</a> on ${new Date(comment.date).toLocaleString()}
          </div>
      </div>
      <div id="comment-${comment.id}" class="comment-text">
          ${comment.content}
      </div>
      <div class="action">
      ${userObj.email == comment.email ? `<button id="edit-btn" onclick="editCommentArtist(${comment.id})" type="button" class="btn btn-primary btn-xs" title="Edit"><span class="glyphicon glyphicon-pencil"></span></button>` : ""}
          ${userObj.email == comment.email ? `<button onclick="deleteCommentArtist(${comment.id})" type="button" class="btn btn-danger btn-xs" title="Delete"><span class="glyphicon glyphicon-trash"></span></button>`: ""}
      
      </div>
  </div>  
   </div>
   </li>`
    }
  }
}

// sets the comments that were writen on a song in its page
function renderCommentsToSongPageSuccessCB(data){
   let userObj = JSON.parse(localStorage.getItem("userObj"))
  let commentsCont = document.querySelector(".song-comment-list");
  commentsCont.innerHTML = "";
  document.querySelector(".label-info").innerHTML = data.length
  if(data.length == 0){
    commentsCont.innerHTML = "No Comments Yet"
  }
  else{
    for(let comment of data){
      commentsCont.innerHTML += `  <li class="list-group-item artist-comment-list">
                            <div class="row">
  <div class="col-xs-10 col-md-11">
      <div>
          <div class="mic-info">
              By: <a class="comment-username">${comment.username}</a> on ${new Date(comment.date).toLocaleString()}
          </div>
      </div>
      <div id="comment-${comment.id}" class="comment-text">
          ${comment.content}
      </div>
      <div class="action">
      ${userObj.email == comment.email ? `<button id="edit-btn" onclick="editCommentSong(${comment.id})" type="button" class="btn btn-primary btn-xs" title="Edit"><span class="glyphicon glyphicon-pencil"></span></button>` : ""}
          ${userObj.email == comment.email ? `<button onclick="deleteCommentSong(${comment.id})" type="button" class="btn btn-danger btn-xs" title="Delete"><span class="glyphicon glyphicon-trash"></span></button>`: ""}
      
      </div>
  </div>  
   </div>
   </li>`
    }
  }
}

//using document ready for the form submitting, first we check if there is a user connected, if not we return a message.
//else we create a new comment object with the text from the comment element in the page and send it to the server FromBody.
$(document).ready(() => {
  $("#comment-artist-form").submit(() => {
    let user = JSON.parse(localStorage.getItem("userObj"))
    if(user == null){
      const Toast = Swal.mixin({
        toast: true,
        position: 'top-start',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
          toast.addEventListener('mouseenter', Swal.stopTimer)
          toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
      })
      
      Toast.fire({
        icon: 'error',
        title: 'No user connected, could not comment.'
      })
      return false;
    }
    let commentContent = document.querySelector(".form-control").value
    newComment = {
      username: user.username,
      email: user.email,
      content: commentContent,
      whom: document.querySelector("#artist").innerHTML
    }
    ajaxCall("POST",currApi + `/Comments/CommentToArtist`,JSON.stringify(newComment),newCommentArtistSuccessCB,emptyCommenterrorCB);
    return false;
  })
  //same as comment on artist
  $("#comment-song-form").submit(() => {
    let user = JSON.parse(localStorage.getItem("userObj"))
    if(user == null){
      const Toast = Swal.mixin({
        toast: true,
        position: 'top-start',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
          toast.addEventListener('mouseenter', Swal.stopTimer)
          toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
      })
      
      Toast.fire({
        icon: 'error',
        title: 'No user connected, could not comment.'
      })
      return false;
    }
    let commentContent = document.querySelector(".form-control").value
    newComment = {
      username: user.username,
      email: user.email,
      content: commentContent,
      whom: document.querySelector("#songName").innerHTML
    }
    ajaxCall("POST",currApi + `/Comments/CommentToSong`,JSON.stringify(newComment),newCommentSongSuccessCB,emptyCommenterrorCB);
    return false;
  })
})

// error callback to handle a case where a user tried to submit conmment with no text at all. shows a message indicating he should try again.
function emptyCommenterrorCB(err){
  let status = err.responseText.split("$")[1];
  if(status == "Comment is empty"){
    const Toast = Swal.mixin({
      toast: true,
      position: 'bottom-start',
      showConfirmButton: false,
      timer: 3000,
      timerProgressBar: true,
      didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer)
        toast.addEventListener('mouseleave', Swal.resumeTimer)
      }
    })
    
    Toast.fire({
      icon: 'error',
      title: 'Your comment is empty, please fill it and try again.'
    })
  }
}

//when adding a new comment, we come here to empty the text box and render all the comments again
function newCommentArtistSuccessCB(data){
  document.querySelector(".form-control").value = ""
  let artistName = document.querySelector("#artist").innerHTML
  ajaxCall("GET",currApi + `/Comments/GetAllCommentsArtists/${artistName}`,"",renderCommentsToArtistPageSuccessCB,errorCB);
}

//when adding a new comment, we come here to empty the text box and render all the comments again
function newCommentSongSuccessCB(data){
  document.querySelector(".form-control").value = ""
  let songName = document.querySelector("#songName").innerHTML
  ajaxCall("GET",currApi + `/Comments/GetAllCommentsSongs/${songName}`,"",renderCommentsToSongPageSuccessCB,errorCB);
}

//this function let the user edit his comment. it renders the button to a confirm button and sets the onclick function to submitting an update to the server using "PUT"
function editCommentArtist(id){
  let currComment = document.querySelector(`#comment-${id}`)
  currCommentContent = currComment.innerText;
  let editBtn = document.querySelector("#edit-btn")
  editBtn.style.backgroundColor = "green"
  editBtn.innerHTML = "✅"
  editBtn.onclick = () => {
  
      let content = document.querySelector("#editedTextArea").value
    ajaxCall("PUT",currApi + `/Comments/ChangeCommentToArtist/${id}`,JSON.stringify(content),newCommentArtistSuccessCB,emptyCommenterrorCB);
  }
  currComment.innerHTML = `<textarea id="editedTextArea" class="form-control" rows="3">${currCommentContent}</textarea>`

}
//this function let the user edit his comment. it renders the button to a confirm button and sets the onclick function to submitting an update to the server using "PUT"
function editCommentSong(id){
  let currComment = document.querySelector(`#comment-${id}`)
  currCommentContent = currComment.innerText;
  let editBtn = document.querySelector("#edit-btn")
  editBtn.style.backgroundColor = "green"
  editBtn.innerHTML = "✅"
  editBtn.onclick = () => {
  
      let content = document.querySelector("#editedTextArea").value
    ajaxCall("PUT",currApi + `/Comments/ChangeCommentToSong/${id}`,JSON.stringify(content),newCommentSongSuccessCB,emptyCommenterrorCB);
  }
  currComment.innerHTML = `<textarea id="editedTextArea" class="form-control" rows="3">${currCommentContent}</textarea>`

}
//deletes the comment on a song by given id
function deleteCommentSong(id){
  ajaxCall("DELETE",currApi + `/Comments/DeleteCommentSongByID/${id}`,"",deleteCommentSongSuccess,errorCB);
}

//deletes the comment on a artist by given id
function deleteCommentArtist(id){
  ajaxCall("DELETE",currApi + `/Comments/DeleteCommentArtistByID/${id}`,"",deleteCommentArtistSuccess,errorCB);

}

//render all the comments for artist again after deleting
function deleteCommentArtistSuccess(data){
  let artistName = document.querySelector("#artist")
  ajaxCall("GET",currApi + `/Comments/GetAllCommentsArtists/${artistName}`,"",newCommentArtistSuccessCB,errorCB);
}

//render all the comments for song again after deleting
function deleteCommentSongSuccess(data){
  let songName = document.querySelector("#songName")
  ajaxCall("GET",currApi + `/Comments/GetAllCommentsArtists/${songName}`,"",newCommentSongSuccessCB,errorCB);
}

//deletes a user from the database and all of related likes and comments
function deleteUser(){
  let user = JSON.parse(localStorage.getItem("userObj"))
  console.log(user.email)
  ajaxCall("DELETE",currApi + `/Users/Delete/${user.email}`,"",deleteUserSuccessCB,errorCB);
  
}
// if delete suecceded - signing out the user.
function deleteUserSuccessCB(data){
  signout();

}