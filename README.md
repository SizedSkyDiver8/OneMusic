# OneMusic - Song Lyrics Website
TRY IT NOW: https://proj.ruppin.ac.il/cgroup10/test2/tar5/index.html
## Overview

OneMusic is a web application designed to provide users with an immersive experience exploring song lyrics, artists, and music trivia. The project is built on real data obtained through web scraping from various song websites.Showing 500 songs from 100 different singers/bands, allowing for a diverse and extensive collection.

## Features

### User Interface

1. **Landing Page:**
   - Presents initial information, showcasing either a list of the 5 most liked songs.
   
2. **Song Search:**
   - Allows users to search for songs based on parameters such as song name, singer, or lyrics.
   
3. **Presentation of Songs:**
   - Displays songs according to singers, facilitating easy navigation.
   
4. **List of Artists:**
   - Provides a comprehensive list of artists, allowing users to explore their favorite musicians.
   
5. **Artist Information:**
   - Presents detailed information about artists, including data pulled from the last.fm API or other sources.
   
6. **Favorites Management:**
   - Enables users to add/remove songs to/from their favorites, enhancing user customization.
   
7. **Quiz:**
   - Automatically generates quiz questions related to songs and artists, offering an interactive challenge.
   
8. **User Management:**
   - Implements registration and login process with username, email, and password.

### System Administrator Interface

1. **User-Level Information:**
   - Displays information about users, including their registered details and favorite songs.
   
2. **Song-Level Information:**
   - Provides insights into song data, including how many times songs appear in users' favorites.
   
3. **Singer-Level Information:**
   - Presents data on singers, showing how many appear in users' favorites.

### Graphical Interface

- Ensures a uniform, clear, and aesthetically pleasing interface adaptable to mobile devices.

## Pages

### Homepage

- Features a list of the top 5 artists based on user preferences.

### Artists List Page

- Allows users to choose an artist from the list, providing a summary and a link to the full artist page.

### Songs List Page

- Enables users to choose a song from the list, displaying a summary and providing a link to the full song page with lyrics.

### Song Lyrics Page

- Allows users to read the lyrics of a song.
- Enables users to like the song if registered.
- Provides the option to comment on the song.

### Artist Page

- Presents a brief or detailed summary of the artist.
- Displays the songs the artist has on the site.
- Allows users to like the artist if registered.
- Provides the option to comment on the artist.

### Quizzes

- Three interactive quizzes based on matching song names to artist names, matching artist names to song names, and matching artists based on given song lyrics.

### Admin Panel

- Displays dynamic statistics on registered users, stored songs, and stored artists.
- Lists all users with their information, liked songs, and liked artists.
- Provides a full list of stored songs and artists.

## Technology Stack

- **Database:** SQL with stored procedures.
- **Server Code:** C# ASP.NET.
- **Client Code:** Vanilla JS and jQuery.
- **API's:** Last.fm and Deezer.
