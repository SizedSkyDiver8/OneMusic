using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Lyrics_Final_Proj.Models;
using System.Globalization;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{

    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    //---------------------------------------------------------------------------------
    // Create the SqlCommand using a stored procedure
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommandWithStoredProcedure(String spName, SqlConnection con, Dictionary<string, object> paramDic)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        if (paramDic != null)
            foreach (KeyValuePair<string, object> param in paramDic)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);

            }


        return cmd;
    }


    /////////////////////////////////////////////////////////////////
    ///////////////////////// SONG /////////////////////////////////
    ////////////////////////////////////////////////////////////////


    //--------------------------------------------------------------------------------------------------
    // This method Reads all Songs
    //--------------------------------------------------------------------------------------------------
    public List<Song> ReadAllSongs()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("Final_GetAllSongs", con, null);             // create the command


        List<Song> songsList = new List<Song>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Song s = new Song();
                s.ArtistName = dataReader["artistName"].ToString();
                s.Title = dataReader["song"].ToString();
                s.Link = dataReader["link"].ToString();
                s.Lyrics = dataReader["text"].ToString();
                s.FavoriteCount = Convert.ToInt32(dataReader["favourite"]);








                songsList.Add(s);
            }
            return songsList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // This method returns a list of songs for given artist
    //--------------------------------------------------------------------------------------------------
    public List<Song> GetSongsByArtist(string artistName)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@artistName", artistName);

        cmd = CreateCommandWithStoredProcedure("Final_GetSongsByArtist", con, paramDic);

        List<Song> songsList = new List<Song>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Song s = new Song();
                s.Title = dataReader["song"].ToString();
                s.Link = dataReader["link"].ToString();
                s.Lyrics = dataReader["text"].ToString();
                songsList.Add(s);
            }
            return songsList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns a list of songs for given Username
    //--------------------------------------------------------------------------------------------------
    public List<Song> GetSongsLikedByUsername(string artistName)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@artistName", artistName);

        cmd = CreateCommandWithStoredProcedure("Final_GetUsersList", con, paramDic);

        List<Song> songsList = new List<Song>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Song s = new Song();
                s.Title = dataReader["song"].ToString();
                s.Link = dataReader["link"].ToString();
                s.Lyrics = dataReader["text"].ToString();
                songsList.Add(s);
            }
            return songsList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }



    //--------------------------------------------------------------------------------------------------
    // This method returns a list of songs for given Song Name
    //--------------------------------------------------------------------------------------------------
    public Song GetSongsBySongName(string songName)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@songName", songName);

        cmd = CreateCommandWithStoredProcedure("Final_GetSongBySongname", con, paramDic);

        List<Song> songsList = new List<Song>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            Song s = new Song();
            while (dataReader.Read())
            {
                s.ArtistName = dataReader["artistName"].ToString();
                s.Title = dataReader["song"].ToString();
                s.Link = dataReader["link"].ToString();
                s.Lyrics = dataReader["text"].ToString();
                s.FavoriteCount = Convert.ToInt32(dataReader["favourite"]);
            }
            return s;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns a list of songs that include word/sentences in the name or lyric 
    //--------------------------------------------------------------------------------------------------
    public List<Song> GetSongsByWord(string word)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@word", word);

        cmd = CreateCommandWithStoredProcedure("Final_Get_Song_By_Word", con, paramDic);             // create the command

        List<Song> artistList = new List<Song>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Song song = new Song();
                song.ArtistName = dataReader["artistName"].ToString();
                song.Lyrics = dataReader["text"].ToString();
                song.Title = dataReader["song"].ToString();
                song.Link = dataReader["link"].ToString();
                song.FavoriteCount = Convert.ToInt32(dataReader["favourite"]);

                artistList.Add(song);
            }
            return artistList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }


    //--------------------------------------------------------------------------------------------------
    // This method checks if user liked song
    //--------------------------------------------------------------------------------------------------
    public bool GetIfUserLikedSong(string email, string songName)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@email", email);
        paramDic.Add("@songName", songName);




        cmd = CreateCommandWithStoredProcedure("Final_IsUserLikeSongExist", con, paramDic);             // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            int numOfResults = 0;
            while (dataReader.Read())
            {
                numOfResults++;
            }
            return numOfResults > 0;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }


    //--------------------------------------------------------------------------------------------------
    // This method returns a list of liked songs of a user
    //--------------------------------------------------------------------------------------------------
    public List<Object> GetUserLikedSongs(string email)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@email", email);

        cmd = CreateCommandWithStoredProcedure("Final_GetUserLikedSongs", con, paramDic);

        List<Object> songsList = new List<Object>();
        songsList.Add(email);
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Song s = new Song();
                s.ArtistName = dataReader["artistName"].ToString();
                s.Title = dataReader["song"].ToString();
                s.Link = dataReader["link"].ToString();
                s.Lyrics = dataReader["text"].ToString();
                songsList.Add(s);
            }
            return songsList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns a list of liked artists of a user
    //--------------------------------------------------------------------------------------------------
    public List<Object> GetUserLikedArtist(string email)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@email", email);

        cmd = CreateCommandWithStoredProcedure("Final_GetUserLikedArtists", con, paramDic);

        List<Object> songsList = new List<Object>();
        songsList.Add(email);
        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Artist a = new Artist();
                a.Name = dataReader["artistName"].ToString();
                songsList.Add(a);
            }
            return songsList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }


    /////////////////////////////////////////////////////////////////
    ///////////////////////// USER /////////////////////////////////
    ////////////////////////////////////////////////////////////////

    //--------------------------------------------------------------------------------------------------
    // This method checks if user exists 
    //--------------------------------------------------------------------------------------------------
    public int LoginUser(User user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@userName", user.Email);
        paramDic.Add("@userPassword", user.Password);




        cmd = CreateCommandWithStoredProcedure("[dbo].[Final_CheckUser]", con, paramDic);             // create the command

        try
        {
            //int numEffected = cmd.ExecuteNonQuery(); // execute the command
            int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id/
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method adds to user a favorite song 
    //--------------------------------------------------------------------------------------------------
    public int UserSong(string email, string songName)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@userEmail", email);
        paramDic.Add("@songName", songName);




        cmd = CreateCommandWithStoredProcedure("Final_UserLikesSong", con, paramDic);             // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            int numOfLikes = 0;
            while (dataReader.Read())
            {
                numOfLikes++;

            }
            return numOfLikes;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }


    //--------------------------------------------------------------------------------------------------
    // This method checks if user is already exist by email 
    //--------------------------------------------------------------------------------------------------
    public int CheckUserExistEmail(string email)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@email", email);




        cmd = CreateCommandWithStoredProcedure("Final_IsUserExist", con, paramDic);             // create the command

        try
        {
            //int numEffected = cmd.ExecuteNonQuery(); // execute the command
            int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id/
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method checks if user is already exist by username 
    //--------------------------------------------------------------------------------------------------
    public int CheckUserExistName(string name)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@name", name);




        cmd = CreateCommandWithStoredProcedure("Final_IsUserExistName", con, paramDic);             // create the command

        try
        {
            //int numEffected = cmd.ExecuteNonQuery(); // execute the command
            int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id/
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method Inserts a user to the Users table 
    //--------------------------------------------------------------------------------------------------
    public int InsertUser(User user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@name", user.Name);
        paramDic.Add("@email", user.Email);
        paramDic.Add("@password", user.Password);
        cmd = CreateCommandWithStoredProcedure("Final_UserRegister", con, paramDic);             // create the command

        try
        {
            // int numEffected = cmd.ExecuteNonQuery(); // execute the command
            int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    //--------------------------------------------------------------------------------------------------
    // This method returns all the users registered
    //--------------------------------------------------------------------------------------------------
    public List<User> GetAllUsers()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("Final_GetUsersList ", con, null);             // create the command


        List<User> usersList = new List<User>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dataReader.Read())
            {
                User u = new User();
                u.Name = dataReader["userName"].ToString();
                u.Email = dataReader["email"].ToString();
                u.DateRegister = Convert.ToDateTime(dataReader["created_at"]);
                usersList.Add(u);
            }
            return usersList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    //--------------------------------------------------------------------------------------------------
    // This method Read user by email
    //--------------------------------------------------------------------------------------------------
    public User ReadUserByEmail(string email)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@email", email);
        cmd = CreateCommandWithStoredProcedure("Final_GetUserByEmail", con, paramDic);             // create the command



        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            User u = new User();
            while (dataReader.Read())
            {
                u.Name = dataReader["userName"].ToString();
                u.Email = dataReader["email"].ToString();
                u.Password = dataReader["password"].ToString();
                u.DateRegister = Convert.ToDateTime(dataReader["created_at"]);
            }
            return u;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }


    //--------------------------------------------------------------------------------------------------
    // This method gets the data statistic for the admin
    //--------------------------------------------------------------------------------------------------
    public int[] GetStatisticsAdmin()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


       cmd = CreateCommandWithStoredProcedure("Final_GetAdminPanelCounts", con, null);             // create the command



        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            int[] statList = new int[3];
            int stat;
            while (dataReader.Read())
            {
                statList[0] = Convert.ToInt32(dataReader["SongsCount"]);
                statList[1] = Convert.ToInt32(dataReader["ArtistsCount"]);
                statList[2] = Convert.ToInt32(dataReader["UsersCount"]);

            }
            return statList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // This method deletes user 
    //--------------------------------------------------------------------------------------------------

    public bool DeleteUserByEmail(string email)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@email", email);

        cmd = CreateCommandWithStoredProcedure("Final_Delete_User", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            //int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id/
            if(numEffected > 0)
            {
                return true;
            }
            else { 
                return false;
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }


    /////////////////////////////////////////////////////////////////
    ///////////////////////// ARTIST /////////////////////////////////
    ////////////////////////////////////////////////////////////////

    //--------------------------------------------------------------------------------------------------
    // Adds or removes like from artists and adds/removes from UserAtrist table
    //--------------------------------------------------------------------------------------------------
    public int AddRemoveLike(string mail, string name)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@EmailUser", mail);
        paramDic.Add("@artistName", name);
        //paramDic.Add("@functionType", num);




        cmd = CreateCommandWithStoredProcedure("Final_Like_Artist", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            //int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id/
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns a list of artists
    //--------------------------------------------------------------------------------------------------
    public List<string> ReturnAllArtists()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("Final_Get_Artists ", con, null);             // create the command


        List<string> artistList = new List<string>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                string name = dataReader["artistName"].ToString();
                artistList.Add(name);
            }
            return artistList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // This method returns a 5 artists with most likes
    //--------------------------------------------------------------------------------------------------
    public List<Artist> ReturnTopArtists()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("Final_Top_Artists ", con, null);             // create the command


        List<Artist> artistList = new List<Artist>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dataReader.Read())
            {
            Artist a = new Artist();
                a.Name = dataReader["artistName"].ToString();
                a.FavoriteCount = Convert.ToInt32(dataReader["likes"]);
                artistList.Add(a);
            }
            return artistList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // This method returns a list of artists that include word in their name 
    //--------------------------------------------------------------------------------------------------
    public List<string> SearchArtistsByWord(string word)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@word", word);

        cmd = CreateCommandWithStoredProcedure("Final_Get_Artists_By_Word", con, paramDic);             // create the command

        List<string> artistList = new List<string>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                string name = dataReader["artistName"].ToString();
                artistList.Add(name);
            }
            return artistList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // This method receives a artist name and returns this amount of likes 
    //--------------------------------------------------------------------------------------------------
    public int GetArtistLikes(string name)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@artistName", name);


        cmd = CreateCommandWithStoredProcedure("Final_Get_Artist_Likes ", con, paramDic);             // create the command

        try
        {
            //int numEffected = cmd.ExecuteNonQuery(); // execute the command
            int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id/
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns 5 artists that the user liked 
    //--------------------------------------------------------------------------------------------------
    public List<string> GetTopArtistsByUser(string username)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@username", username);

        cmd = CreateCommandWithStoredProcedure("Final_Get_Artists_By_Username", con, paramDic);             // create the command

        List<string> artistList = new List<string>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                string name = dataReader["artistName"].ToString();
                artistList.Add(name);
            }
            return artistList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }



    //--------------------------------------------------------------------------------------------------
    // this method returns all the artists with their likes count
    //--------------------------------------------------------------------------------------------------
    public List<Artist> GetAllArtistsWithLikes()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("Final_GetAllArtists ", con, null);             // create the command


        List<Artist> artistList = new List<Artist>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Artist a = new Artist();
                a.Name = dataReader["artistName"].ToString();
                a.FavoriteCount = Convert.ToInt32(dataReader["likes"].ToString());
                artistList.Add(a);
            }
            return artistList;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // This method checks if user liked artist
    //--------------------------------------------------------------------------------------------------
    public bool GetIfUserLikedArtist(string email, string artistName)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@email", email);
        paramDic.Add("@artistName", artistName);




        cmd = CreateCommandWithStoredProcedure("Final_isUserLikeArtistExist", con, paramDic);             // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            int numOfResults = 0;
            while (dataReader.Read())
            {
                numOfResults++;
            }
            return numOfResults > 0;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    /////////////////////////////////////////////////////////////////
    ///////////////////////// Comment //////////////////////////////
    ////////////////////////////////////////////////////////////////

    //--------------------------------------------------------------------------------------------------
    // This method add comment to artist 
    //--------------------------------------------------------------------------------------------------

    public int AddCommentArtist(Comment comment)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@email", comment.Email);
        paramDic.Add("@comment", comment.Content);
        paramDic.Add("@artist", comment.Whom);


        cmd = CreateCommandWithStoredProcedure("Final_Add_Comment_Artist", con, paramDic);             // create the command

        try
        {
            //int numEffected = cmd.ExecuteNonQuery(); // execute the command
            int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id/
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method add comment to song 
    //--------------------------------------------------------------------------------------------------

    public int AddCommentSong(Comment comment)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@email", comment.Email);
        paramDic.Add("@comment", comment.Content);
        paramDic.Add("@nameSong", comment.Whom);


        cmd = CreateCommandWithStoredProcedure("Final_Add_Comment_Song", con, paramDic);             // create the command

        try
        {
            //int numEffected = cmd.ExecuteNonQuery(); // execute the command
            int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id/
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method deletes comment to artist 
    //--------------------------------------------------------------------------------------------------

    public int DeleteCommentArtist(int id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@id", id);

        cmd = CreateCommandWithStoredProcedure("Final_Delete_Comment_Artist", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            //int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id/
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method deletes comment to song 
    //--------------------------------------------------------------------------------------------------

    public int DeleteCommentSong(int id)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@id", id);

        cmd = CreateCommandWithStoredProcedure("Final_Delete_Comment_Song", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            //int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id/
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method returns a list of comments to an artist  
    //--------------------------------------------------------------------------------------------------
    public List<Comment> ReturnCommentsToArtist(string artistName)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@artistName", artistName);

        cmd = CreateCommandWithStoredProcedure("Final_ReturnCommentsToArtist", con, paramDic);             // create the command

        List<Comment> commentsToArtist = new List<Comment>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Comment comment = new Comment();
                comment.Id = Convert.ToInt32(dataReader["id"]);
                comment.Username = dataReader["username"].ToString();
                comment.Email= dataReader["email"].ToString();
                comment.Content = dataReader["comment"].ToString();
                comment.Whom= dataReader["artistName"].ToString();
                comment.Date = Convert.ToDateTime(dataReader["created_at"]);

                commentsToArtist.Add(comment);
            }
            return commentsToArtist;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    //--------------------------------------------------------------------------------------------------
    // This method returns a list of comments to a song
    //--------------------------------------------------------------------------------------------------
    public List<Comment> ReturnCommentsToSong(string song)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@songName", song);

        cmd = CreateCommandWithStoredProcedure("Final_ReturnCommentsToSong", con, paramDic);             // create the command

        List<Comment> commentsToSong = new List<Comment>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                Comment comment = new Comment();
                comment.Id = Convert.ToInt32(dataReader["id"]);
                comment.Username = dataReader["username"].ToString();
                comment.Email = dataReader["email"].ToString();
                comment.Content = dataReader["comment"].ToString();
                comment.Whom = dataReader["songName"].ToString();
                comment.Date = Convert.ToDateTime(dataReader["created_at"]);
                commentsToSong.Add(comment);
            }
            return commentsToSong;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    
    //--------------------------------------------------------------------------------------------------
    // This method changes comment for artist
    //--------------------------------------------------------------------------------------------------

    public bool ChangeCommentArtist(int id,string comment)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@id", id);
        paramDic.Add("@comment", comment);

        cmd = CreateCommandWithStoredProcedure("Final_ChangeCommentContentArtist", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            //int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id/
            if(numEffected > 0)
            {
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    //--------------------------------------------------------------------------------------------------
    // This method changes comment for song
    //--------------------------------------------------------------------------------------------------

    public bool ChangeCommentSong(int id, string comment)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@id", id);
        paramDic.Add("@comment", comment);

        cmd = CreateCommandWithStoredProcedure("Final_ChangeCommentContentSong", con, paramDic);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            //int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id/
            if (numEffected > 0)
            {
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    /////////////////////////////////////////////////////////////////
    ///////////////////////// Question //////////////////////////////
    ////////////////////////////////////////////////////////////////

    //--------------------------------------------------------------------------------------------------
    // This method generates artist name for question and right answer as song 
    //--------------------------------------------------------------------------------------------------

    public List<string> QandA()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("Final_GenerateQuestionCorrect", con, null);             // create the command


        List<string> questionANDanswer = new List<string>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                string Q = dataReader["artistName"].ToString();
                string A = dataReader["song"].ToString();
                questionANDanswer.Add(Q);
                questionANDanswer.Add(A);
            };
                return questionANDanswer;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    //--------------------------------------------------------------------------------------------------
    // This method generates 3 incorrect answers as songs for question 
    //--------------------------------------------------------------------------------------------------

    public List<string> ThreeIncorrectAnswersSongs(string artistName)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@artistName", artistName);

        cmd = CreateCommandWithStoredProcedure("Final_GenerateQuestionWrong", con, paramDic);             // create the command


        List<string> answers = new List<string>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                string answer = dataReader["song"].ToString();
                answers.Add(answer);
            };
            return answers;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // This method generates 3 incorrect answers (artist) for question
    //--------------------------------------------------------------------------------------------------


    public List<string> ThreeIncorrectAnswersArtist(string artistName, string songName)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@artistName", artistName);
        paramDic.Add("@songName", songName);

        cmd = CreateCommandWithStoredProcedure("Final_GenerateQuestionWrongSong", con, paramDic);             // create the command


        List<string> answers = new List<string>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                string answer = dataReader["artistName"].ToString();
                answers.Add(answer);
            };
            return answers;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
    //--------------------------------------------------------------------------------------------------
    // This method checks if the lyrics belong to the chosen song 
    //--------------------------------------------------------------------------------------------------
    public bool CheckAnswerLyric(string lyric, string song)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@lyric", lyric);
        paramDic.Add("@songName", song);

        cmd = CreateCommandWithStoredProcedure("Final_CheckQuestionLyric", con, paramDic);             // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            int rowCount = 0;
            while (dataReader.Read())
            {
                rowCount++;
            }
            return rowCount > 0;

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method checks if the artist sings the chosen song 
    //--------------------------------------------------------------------------------------------------
    public bool CheckAnswerArtist(string artist, string song)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@artistName", artist);
        paramDic.Add("@songName", song);

        cmd = CreateCommandWithStoredProcedure("Final_CheckSongRightToArtist", con, paramDic);             // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            int rowCount = 0;
            while (dataReader.Read())
            {
                rowCount++;
            }
            return rowCount > 0;

        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a question (list) where in the [0] is the question lyrics from song and in [1] the right song as answer  
    //--------------------------------------------------------------------------------------------------
    public List<string> QandALyric()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = CreateCommandWithStoredProcedure("Final_QuestionPartitionLyric", con, null);             // create the command


        List<string> questionANDanswer = new List<string>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                string A = dataReader["Lyrics"].ToString();
                string Q = dataReader["song"].ToString();
                questionANDanswer.Add(A);
                questionANDanswer.Add(Q);             
            };
            return questionANDanswer;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // This method adds 3 answers (songs) to the lyric question 
    //--------------------------------------------------------------------------------------------------
    public List<string> ThreeAnswersLyric(string songName)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        Dictionary<string, object> paramDic = new Dictionary<string, object>();
        paramDic.Add("@songName", songName);

        cmd = CreateCommandWithStoredProcedure("Final_ThreeSongsForQ", con, paramDic);             // create the command


        List<string> answers = new List<string>();

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dataReader.Read())
            {
                string answer = dataReader["song"].ToString();
                answers.Add(answer);
            };
            return answers;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }
}





