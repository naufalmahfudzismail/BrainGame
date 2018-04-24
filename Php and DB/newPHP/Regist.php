<?php

$host = "localhost";
$dbName = "katadasar";
$user = "root";
$password = "";

  $conn = new mysqli($host, $user, $password, $dbName);

  if(!$conn)
  {
      die("Connection Failed. ".mysqli_connect_error());
  }


    $user = $_POST["username"];
    $user = mysqli_real_escape_string($conn, $user);
    $pass = $_POST["password"];
    $pass = mysqli_real_escape_string($conn, $pass);
    $nama = $_POST["nama"];
    $nama = mysqli_real_escape_string($conn, $nama);
    $kerja = $_POST["kerja"];
    $kerja = mysqli_real_escape_string($conn, $kerja);
    $umur = $_POST["umur"];
    $umur = mysqli_real_escape_string($conn, $umur);

    $sql_check = "SELECT * FROM data_user where (username = '$user')";
    $result = mysqli_query($conn, $sql_check);
    $rowcount = mysqli_num_rows($result);

      if ($rowcount > 0)
      {
        echo "Username sudah dipakai!";
        mysqli_free_result($result);
      }

      elseif (trim($user) == "" && trim($pass) == "") {
        echo "Username dan Password Kosong!";
        mysqli_free_result($result);
      }

      elseif (trim($user) == "") {
        echo "Username Kosong!";
        mysqli_free_result($result);
      }

      elseif (trim($pass) == "") {
        echo "Password Kosong!";
        mysqli_free_result($result);
      }

      else
      {
          $sql = "INSERT INTO data_user (username, password, Nama, Pekerjaan, Umur) values ('$user', '$pass', '$nama', '$kerja', '$umur')";
          $result = mysqli_query($conn, $sql);

          if(!$result)
          {
             echo "Terdapat Error pada server";
          }

          else echo "It's Okay";
      }

 ?>
