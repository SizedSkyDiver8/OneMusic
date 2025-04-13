$(document).ready(() => {
    getTopArtists();
    getFiveSongsByUser();
        function getTopArtists(){
            let userObjString=localStorage.getItem('userObj');
            let userObj = JSON.parse(userObjString);
            document.getElementById("userName").innerHTML=userObj.username
            let api = currApi + `/Artists/TopArtistsByUsername/${userObj.email}`;
            ajaxCall("GET",api,"",TopArtistsSuccessCB,errorCB);
        }

        function TopArtistsSuccessCB(data){
            let elements = document.querySelectorAll('#fav-artist');
            for (let i=0; i<5;i++){
                elements[i].innerHTML=data[i];
            }
        }

        function getFiveSongsByUser(){
            let userObjString=localStorage.getItem('userObj');
            let userObj = JSON.parse(userObjString);
            let api = currApi + `/Users/GetUserLikedSongs/${userObj.email}`;
            ajaxCall("GET",api,"",TopSongsSuccessCB,errorCB);
        }
        function TopSongsSuccessCB(data){
            let elements = document.querySelectorAll('#fav-song');
            let numbers=[];
            while(numbers.length<5){
                let number=Math.floor(Math.random()*data.length) + 1
                if (!numbers.includes(number)) {
                    numbers.add(number);
                }
            }
            for (let i=0; i<5;i++){
                elements[i].innerHTML= `<a class="visitPage admin-panel-song-links" href="#" onclick="songSelectedFromList('${data[randomNumbers[i]].title}')"></a>`
            }
        }
    });