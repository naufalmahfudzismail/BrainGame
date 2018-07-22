<?php

  $host = "localhost";
  $dbName = "u983984019_katad";
  $user = "u983984019_game";
  $password = "123456";

  $conn = new mysqli($host, $user, $password, $dbName);


  $user = $_POST["user"];
  $user = mysqli_real_escape_string($conn, $user);
  $level = $_POST["level"];
  $level = mysqli_real_escape_string($conn, $level);
  $skor = $_POST["skor"];
  $skor = mysqli_real_escape_string($conn, $skor);

  if(!$conn)
  {
      die("Connection Failed. ".mysqli_connect_error());
  }

  $sql = "INSERT INTO tbl_hidden2 (username, level, score, tanggal) 
  values ('$user', '$level', '$skor', now())";
  
  if(trim($user) == "" || trim($skor) == "")
  {
      echo "Semua data harus ada !";
  }
  else
  {
      $result  = mysqli_query($conn, $sql);
          
          if(!$result)
          {
             echo "Terdapat Error pada server";
          }
        
           else echo "Success!";
    
  }



?>
