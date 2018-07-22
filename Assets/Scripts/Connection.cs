using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour
{
    public string UrlInsertFlexile = "http://segarbugarindo.com/braingame_insert_flexible.php";
    public string UrlInsertAnagram = "http://segarbugarindo.com/braingame_insert_anagram.php";
    public string UrlInsertPattern = "http://segarbugarindo.com/braingame_insert_pattern.php";
    public string UrlLogin = "http://segarbugarindo.com/braingame_login.php";
    public string UrlRegist = "http://segarbugarindo.com/braingame_regist.php";


    public void InsertAnagram(string username, string level, string huruf, int skor, string kalimat, string katadasar)
    {
        WWWForm form = new WWWForm();
        form.AddField("user", username);
        form.AddField("huruf", huruf);
        form.AddField("level", level);
        form.AddField("skor", skor);
        form.AddField("kalimat", kalimat);
        form.AddField("katadasar", katadasar);
        WWW www = new WWW(UrlInsertAnagram, form);
    }

    public void InsertFlexible(string username, string tipe, string kategori, string soal, string level, string huruf, int skor, string kalimat, string katadasar)
    {
        WWWForm form = new WWWForm();
        form.AddField("user", username);
        form.AddField("huruf", huruf);
        form.AddField("level", level);
        form.AddField("skor", skor);
        form.AddField("kalimat", kalimat);
        form.AddField("katadasar", katadasar);
        form.AddField("tipe", tipe);
        form.AddField("kategori", kategori);
        form.AddField("soal", soal);
        WWW www = new WWW(UrlInsertFlexile, form);
    }

     public void InsertPattern(string username, string level, int jawaban, int skor, string soal)
    {
        WWWForm form = new WWWForm();
        form.AddField("user", username);
        form.AddField("jawaban", jawaban);
        form.AddField("level", level);
        form.AddField("skor", skor);
        form.AddField("soal", soal);
        WWW www = new WWW(UrlInsertPattern, form);
    }

}
